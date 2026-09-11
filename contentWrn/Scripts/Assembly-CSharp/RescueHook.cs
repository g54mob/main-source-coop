using Photon.Pun;
using UnityEngine;
using Zorro.Core.Serizalization;

public class RescueHook : ItemInstanceBehaviour
{
	public GameObject lightObject;

	public GameObject darkObject;

	public BatteryDisplay m_batteryDisplay;

	public float maxCharge = 150f;

	public int maxCharges = 3;

	private BatteryEntry m_batteryEntry;

	private LineRenderer line;

	public int range = 30;

	public float dragForce = 100f;

	public Transform firePoint;

	public Transform dragPoint;

	private Player playerHoldingItem;

	private Rigidbody targetRig;

	private Player targetPlayer;

	private Vector3 targetPos;

	public bool fly;

	public bool isPulling;

	public float currentDistance;

	public bool hitNothing;

	private float sinceFire;

	public float launchForce;

	public RopeRender ropeRender;

	private float massFactor = 1f;

	private void Start()
	{
		line = GetComponentInChildren<LineRenderer>();
		line.positionCount = 40;
	}

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		playerHoldingItem = base.transform.root.GetComponent<Player>();
		if (!data.TryGetEntry<BatteryEntry>(out m_batteryEntry))
		{
			m_batteryEntry = new BatteryEntry
			{
				m_charge = maxCharge,
				m_maxCharge = maxCharge
			};
			data.AddDataEntry(m_batteryEntry);
		}
		itemInstance.RegisterRPC(ItemRPC.RPC0, RPCA_RescueRagdollCharacter);
		itemInstance.RegisterRPC(ItemRPC.RPC1, RPCA_RescueWall);
		itemInstance.RegisterRPC(ItemRPC.RPC2, RPCA_RescueNothing);
		itemInstance.RegisterRPC(ItemRPC.RPC3, RPCA_LetGo);
	}

	private void LateUpdate()
	{
		float num = 1f / (float)m_batteryDisplay.m_renderer.Length * (float)maxCharges;
		m_batteryDisplay.SetCharge(m_batteryEntry.GetPercentage() * num - 0.01f);
	}

	private void FixedUpdate()
	{
		sinceFire += Time.fixedDeltaTime;
		if (isPulling)
		{
			if ((bool)targetPlayer)
			{
				targetPos = targetRig.position;
			}
			if (sinceFire > 0.25f)
			{
				Rigidbody rig = playerHoldingItem.refs.ragdoll.GetBodypart(BodypartType.Item).rig;
				if ((bool)targetPlayer)
				{
					Vector3 normalized = (base.transform.position - targetPlayer.Center()).normalized;
					targetPlayer.refs.ragdoll.AddForce(normalized * (dragForce / massFactor), ForceMode.Acceleration);
					targetPlayer.data.sinceRescueDragged = 0f;
					targetPlayer.data.sinceGrounded = Mathf.Clamp(targetPlayer.data.sinceGrounded, 0f, 1f);
					playerHoldingItem.refs.ragdoll.AddForce(-normalized * (dragForce * Mathf.Clamp(massFactor, 0f, 10f) * 0.1f), ForceMode.Acceleration);
					rig.AddForce((targetRig.position - rig.position).normalized * (dragForce * 0.3f), ForceMode.Acceleration);
					targetRig.AddForce((base.transform.position - targetRig.position).normalized * dragForce, ForceMode.Acceleration);
					if (Vector3.Distance(dragPoint.position, targetRig.position) < 2f || sinceFire > 2f)
					{
						LetGo();
					}
					else if (isHeldByMe && Player.localPlayer.input.clickWasPressed)
					{
						CallLetGo();
					}
				}
				else
				{
					if (playerHoldingItem.data.fallTime <= 0f && !hitNothing)
					{
						Vector3 normalized2 = (targetPos - playerHoldingItem.Center()).normalized;
						fly = true;
						playerHoldingItem.refs.ragdoll.Fall(2f);
						playerHoldingItem.refs.ragdoll.AddForce(normalized2 * launchForce, ForceMode.VelocityChange);
						playerHoldingItem.refs.ragdoll.GetBodypart(BodypartType.Item).rig.AddForce(normalized2 * launchForce, ForceMode.VelocityChange);
						playerHoldingItem.refs.ragdoll.GetBodypart(BodypartType.Torso).rig.AddForce(normalized2 * launchForce, ForceMode.VelocityChange);
					}
					if (sinceFire > 0.5f)
					{
						LetGo();
					}
				}
			}
			ropeRender.DisplayRope(dragPoint.position, targetPos, sinceFire, line);
			currentDistance = Vector3.Distance(dragPoint.position, targetPos);
		}
		else
		{
			fly = false;
			StopDisplayRope();
		}
	}

	private void CallLetGo()
	{
		BinarySerializer binarySerializer = new BinarySerializer();
		itemInstance.CallRPC(ItemRPC.RPC3, binarySerializer);
	}

	private void RPCA_LetGo(BinaryDeserializer deserializer)
	{
		LetGo();
	}

	private void LetGo()
	{
		targetRig = null;
		targetPlayer = null;
		isPulling = false;
	}

	private void StopDisplayRope()
	{
		line.enabled = false;
	}

	private void Update()
	{
		if ((bool)playerHoldingItem)
		{
			if (playerHoldingItem.input.aimIsPressed)
			{
				if (!lightObject.activeSelf)
				{
					lightObject.SetActive(value: true);
					darkObject.SetActive(value: false);
				}
			}
			else if (lightObject.activeSelf)
			{
				lightObject.SetActive(value: false);
				darkObject.SetActive(value: true);
			}
		}
		if (!isPulling && !(m_batteryEntry.m_charge <= 0f) && isHeldByMe && Player.localPlayer.input.clickWasPressed && !Player.localPlayer.HasLockedInput() && sinceFire > 0.25f)
		{
			Fire();
		}
	}

	private void Fire()
	{
		m_batteryEntry.m_charge -= m_batteryEntry.m_maxCharge / (float)maxCharges;
		m_batteryEntry.SetDirty();
		sinceFire = 0f;
		RaycastHit hit = HelperFunctions.LineCheck(firePoint.position, firePoint.position + firePoint.forward * range, HelperFunctions.LayerType.Tangible);
		if ((bool)hit.transform)
		{
			Player componentInParent = hit.transform.GetComponentInParent<Player>();
			if ((bool)componentInParent)
			{
				RescueRagdollCharacter(componentInParent, hit);
			}
			else if (!hit.rigidbody)
			{
				RescueWall(hit);
			}
			else
			{
				RescueNothing(hit.point);
			}
		}
		else
		{
			RescueNothing(firePoint.position + firePoint.forward * range);
		}
	}

	private void RescueNothing(Vector3 endPoint)
	{
		BinarySerializer binarySerializer = new BinarySerializer();
		binarySerializer.WriteFloat3(endPoint);
		binarySerializer.WriteBool(value: true);
		itemInstance.CallRPC(ItemRPC.RPC1, binarySerializer);
	}

	private void RescueWall(RaycastHit hit)
	{
		BinarySerializer binarySerializer = new BinarySerializer();
		binarySerializer.WriteFloat3(hit.point);
		binarySerializer.WriteBool(value: false);
		itemInstance.CallRPC(ItemRPC.RPC1, binarySerializer);
	}

	public void RPCA_RescueWall(BinaryDeserializer deserializer)
	{
		if (isHeld)
		{
			targetPos = deserializer.ReadFloat3();
			hitNothing = deserializer.ReadBool();
			sinceFire = 0f;
			GamefeelHandler.instance.perlin.AddShake(base.transform.position, 5f, 0.2f, 15f, 40f);
			isPulling = true;
		}
	}

	public void RPCA_RescueNothing(BinaryDeserializer deserializer)
	{
		if (isHeld)
		{
			sinceFire = 0f;
			GamefeelHandler.instance.perlin.AddShake(base.transform.position, 5f, 0.2f, 15f, 40f);
			isPulling = true;
		}
	}

	private void RescueRagdollCharacter(Player target, RaycastHit hit)
	{
		BinarySerializer binarySerializer = new BinarySerializer();
		binarySerializer.WriteInt(target.refs.view.ViewID);
		binarySerializer.WriteInt(target.refs.ragdoll.GetBodypartIDFromCollider(hit.collider));
		itemInstance.CallRPC(ItemRPC.RPC0, binarySerializer);
	}

	public void RPCA_RescueRagdollCharacter(BinaryDeserializer deserializer)
	{
		if (isHeld)
		{
			int viewID = deserializer.ReadInt();
			int bodyPartID = deserializer.ReadInt();
			targetPlayer = PhotonNetwork.GetPhotonView(viewID).transform.root.GetComponent<Player>();
			targetRig = targetPlayer.refs.ragdoll.GetBodypartFromID(bodyPartID).rig;
			sinceFire = 0f;
			GamefeelHandler.instance.perlin.AddShake(base.transform.position, 5f, 0.2f, 15f, 40f);
			float mass = playerHoldingItem.refs.ragdoll.GetBodypart(BodypartType.Hip).rig.mass;
			float mass2 = targetPlayer.refs.ragdoll.GetBodypart(BodypartType.Hip).rig.mass;
			massFactor = mass2 / mass;
			isPulling = true;
		}
	}
}
