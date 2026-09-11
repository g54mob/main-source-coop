using CurvedUI;
using Photon.Pun;
using UnityEngine;
using Zorro.Core.Serizalization;

public class ItemGrabberArm : ItemInstanceBehaviour
{
	private class ClawRotator
	{
		public Transform claw;

		private float currentZ;

		public ItemGrabberArm itemGrabberArm;

		public float targetAngle;

		public void Update()
		{
			currentZ = Mathf.Lerp(currentZ, targetAngle, Time.deltaTime * itemGrabberArm.clawSpeed);
			claw.localRotation = Quaternion.Euler(-90f, 0f, currentZ);
		}
	}

	public Transform rightClaw;

	public Transform leftClaw;

	public Transform grabPoint;

	public float clawSpeed = 10f;

	public float closedAngle = 25f;

	public Vector2 breakForceStartEnd = new Vector2(1000f, 100f);

	public float breakForceTransitionTime;

	private ConfigurableJoint joint;

	private float jointLifeTime;

	private ClawRotator leftClawRotator;

	private ClawRotator rightClawRotator;

	private OnOffEntry onOffEntry;

	private Player playerHoldingItem;

	private bool wasOn;

	private Rigidbody attachedTo;

	private Bodypart ItemBodyPart => playerHoldingItem.refs.ragdoll.GetBodypart(BodypartType.Item);

	private void Awake()
	{
		leftClawRotator = new ClawRotator
		{
			claw = leftClaw,
			itemGrabberArm = this
		};
		rightClawRotator = new ClawRotator
		{
			claw = rightClaw,
			itemGrabberArm = this
		};
	}

	private void Update()
	{
		leftClawRotator.Update();
		rightClawRotator.Update();
		if (isHeldByMe)
		{
			if (Player.localPlayer.input.clickIsPressed && !Player.localPlayer.HasLockedInput())
			{
				if (!onOffEntry.on)
				{
					Debug.Log("set true");
					onOffEntry.on = true;
					onOffEntry.SetDirty();
					Collider[] array = Physics.OverlapSphere(grabPoint.position, 0.1f);
					Debug.Log("OverlapSphere");
					Collider[] array2 = array;
					foreach (Collider collider in array2)
					{
						Debug.Log(collider.transform.root.name);
						if (!(collider.transform.root == base.transform.root) && (TryGrabPlayer(collider) || TryGrabItem(collider)))
						{
							break;
						}
					}
				}
			}
			else if (onOffEntry.on)
			{
				Debug.Log("set false");
				onOffEntry.on = false;
				onOffEntry.SetDirty();
			}
		}
		if (!isHeld && onOffEntry.on)
		{
			onOffEntry.on = false;
			onOffEntry.SetDirty();
		}
		if (onOffEntry.on)
		{
			leftClawRotator.targetAngle = closedAngle;
			rightClawRotator.targetAngle = 0f - closedAngle;
			if (isHeldByMe && attachedTo != null && joint == null)
			{
				Debug.Log("Joint Broke");
				Object.DestroyImmediate(joint);
				attachedTo = null;
				itemInstance.CallRPC(ItemRPC.RPC2, new BinarySerializer());
			}
			if (!wasOn)
			{
				wasOn = true;
			}
		}
		else
		{
			leftClawRotator.targetAngle = 0f;
			rightClawRotator.targetAngle = 0f;
			if (joint != null)
			{
				if (!isHeld && PhotonNetwork.IsMasterClient)
				{
					Debug.Log("Calling RPC !isHeld && PhotonNetwork.IsMasterClient");
					Object.DestroyImmediate(joint);
					attachedTo = null;
					itemInstance.CallRPC(ItemRPC.RPC2, new BinarySerializer());
				}
				else if (isHeldByMe)
				{
					Debug.Log("Calling RPC isHeldByMe");
					Object.DestroyImmediate(joint);
					attachedTo = null;
					itemInstance.CallRPC(ItemRPC.RPC2, new BinarySerializer());
				}
			}
			if (wasOn)
			{
				wasOn = false;
			}
		}
		if (isHeldByMe && (bool)joint)
		{
			jointLifeTime += Time.deltaTime;
			joint.breakForce = Mathf.Lerp(breakForceStartEnd.x, breakForceStartEnd.y, (jointLifeTime / breakForceTransitionTime).Clamp(0f, 1f));
		}
	}

	private bool TryGrabItem(Collider hit)
	{
		Debug.Log("TryGrabItem");
		if (!hit.transform.root.GetComponent<Pickup>())
		{
			return false;
		}
		Debug.Log("TryGrabItem true");
		BinarySerializer binarySerializer = new BinarySerializer();
		BinarySerializer binarySerializer2 = new BinarySerializer();
		binarySerializer.WriteInt(hit.transform.root.GetComponent<PhotonView>().ViewID);
		binarySerializer2.WriteInt(hit.transform.root.GetComponent<PhotonView>().ViewID);
		Rigidbody componentInChildren = hit.transform.root.GetComponentInChildren<Rigidbody>();
		binarySerializer.WriteFloat3(ItemBodyPart.transform.InverseTransformPoint(componentInChildren.transform.position));
		binarySerializer2.WriteFloat3(ItemBodyPart.transform.InverseTransformPoint(componentInChildren.transform.position));
		binarySerializer2.WriteBool(value: true);
		binarySerializer.WriteBool(value: false);
		BinaryDeserializer binaryDeserializer = new BinaryDeserializer(binarySerializer2);
		RPCA_AttachToItem(binaryDeserializer);
		itemInstance.CallRPC(ItemRPC.RPC1, binarySerializer);
		binaryDeserializer.Dispose();
		return true;
	}

	public void RPCA_AttachToItem(BinaryDeserializer deserializer)
	{
		int viewID = deserializer.ReadInt();
		Vector3 position = deserializer.ReadFloat3();
		Rigidbody componentInChildren = PhotonView.Find(viewID).GetComponentInChildren<Rigidbody>();
		bool flag = deserializer.ReadBool();
		if (!isHeldByMe || flag)
		{
			if ((bool)joint)
			{
				Debug.LogError("already has a joint!");
				Object.DestroyImmediate(joint);
			}
			Bodypart itemBodyPart = ItemBodyPart;
			itemBodyPart.rig.transform.InverseTransformPoint(grabPoint.position);
			componentInChildren.transform.position = itemBodyPart.rig.transform.TransformPoint(position);
			joint = HelperFunctions.AttachPositionJoint(itemBodyPart.rig, componentInChildren, useCustomConnection: true, grabPoint.position);
			jointLifeTime = 0f;
			joint.angularXMotion = ConfigurableJointMotion.Locked;
			joint.angularYMotion = ConfigurableJointMotion.Locked;
			joint.angularZMotion = ConfigurableJointMotion.Locked;
			attachedTo = componentInChildren;
		}
	}

	private bool TryGrabPlayer(Collider hit)
	{
		Bodypart componentInParent = hit.GetComponentInParent<Bodypart>();
		Debug.Log($"TryGrabPlayer {componentInParent}");
		if (!componentInParent)
		{
			return false;
		}
		Player component = componentInParent.transform.root.GetComponent<Player>();
		Debug.Log(component);
		BinarySerializer binarySerializer = new BinarySerializer();
		binarySerializer.WriteInt(component.refs.view.ViewID);
		binarySerializer.WriteInt((int)componentInParent.bodypartType);
		Debug.Log("Calling RPC");
		itemInstance.CallRPC(ItemRPC.RPC0, binarySerializer);
		return true;
	}

	public void RPCA_AttachToPlayer(BinaryDeserializer deserializer)
	{
		Debug.Log("Calling RPC");
		if (isHeld)
		{
			int viewID = deserializer.ReadInt();
			BodypartType bodypartType = (BodypartType)deserializer.ReadInt();
			Player player = PlayerHandler.instance.TryGetPlayerFromViewID(viewID);
			if (!player)
			{
				Debug.LogError("cant find player");
			}
			AttachJoint(player.refs.ragdoll.GetBodypart(bodypartType).rig);
		}
	}

	private void AttachJoint(Rigidbody otherRig, bool lockRotation = false)
	{
		if ((bool)joint)
		{
			Debug.LogError("already has a joint!");
			Object.DestroyImmediate(joint);
		}
		Bodypart bodypart = playerHoldingItem.refs.ragdoll.GetBodypart(BodypartType.Item);
		bodypart.rig.transform.InverseTransformPoint(grabPoint.position);
		joint = HelperFunctions.AttachPositionJoint(bodypart.rig, otherRig, useCustomConnection: true, grabPoint.position);
		jointLifeTime = 0f;
		if (lockRotation)
		{
			joint.angularXMotion = ConfigurableJointMotion.Locked;
			joint.angularYMotion = ConfigurableJointMotion.Locked;
			joint.angularZMotion = ConfigurableJointMotion.Locked;
		}
		attachedTo = otherRig;
	}

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		if (data.TryGetEntry<OnOffEntry>(out onOffEntry))
		{
			Debug.Log($"OnOff entry found, state: {onOffEntry.on}");
		}
		else
		{
			onOffEntry = new OnOffEntry
			{
				on = false
			};
			data.AddDataEntry(onOffEntry);
			Debug.Log("OnOff entry not found, adding new entry with false.");
		}
		playerHoldingItem = base.transform.root.GetComponent<Player>();
		wasOn = onOffEntry.on;
		itemInstance.RegisterRPC(ItemRPC.RPC0, RPCA_AttachToPlayer);
		itemInstance.RegisterRPC(ItemRPC.RPC1, RPCA_AttachToItem);
		itemInstance.RegisterRPC(ItemRPC.RPC2, RPCA_DestroyJoint);
	}

	public void RPCA_DestroyJoint(BinaryDeserializer binaryDeserializer)
	{
		Debug.Log("RPCA_DestroyJoint");
		if ((bool)joint)
		{
			Debug.Log("Destroying joint");
			Object.DestroyImmediate(joint);
		}
		attachedTo = null;
	}
}
