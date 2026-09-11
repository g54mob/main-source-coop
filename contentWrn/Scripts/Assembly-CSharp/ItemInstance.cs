using System;
using DefaultNamespace;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using Zorro.Core;
using Zorro.Core.Serizalization;
using pworld.Scripts.Extensions;

public class ItemInstance : MonoBehaviour
{
	public Item item;

	internal Rigidbody rig;

	public Bodypart itemBody;

	public UnityEvent<Player> onItemEquipped;

	private ItemDataSyncer m_syncer;

	public Optionable<Guid> m_guid;

	public UnityEvent<Player> onUnequip;

	private bool isHeld;

	private bool isHeldByMe;

	public ItemInstanceData instanceData;

	private bool wasHeldOnstart;

	private float m_fixedPositionTime;

	private float m_fixedRotationTime;

	public void InitItem(Item setItem, ItemInstanceData data, PhotonView playerView, PhotonView pickupView)
	{
		item = setItem;
		instanceData = data;
		m_fixedPositionTime = Time.fixedDeltaTime * 15f;
		m_fixedRotationTime = Time.fixedDeltaTime * 8f;
		rig = base.gameObject.AddComponent<Rigidbody>();
		rig.mass = item.mass;
		rig.useGravity = false;
		rig.interpolation = RigidbodyInterpolation.Interpolate;
		rig.collisionDetectionMode = CollisionDetectionMode.Continuous;
		ItemInstanceBehaviour[] components = GetComponents<ItemInstanceBehaviour>();
		if (playerView != null && playerView.IsMine)
		{
			m_syncer = data.CreateDataSyncer(roomOwned: false);
		}
		else if (playerView == null && PhotonNetwork.IsMasterClient)
		{
			m_syncer = data.CreateDataSyncer(roomOwned: true);
		}
		m_guid = Optionable<Guid>.Some(data.m_guid);
		ItemInstanceHandler.RegisterItemInstance(data.m_guid, this);
		isHeld = pickupView == null;
		isHeldByMe = playerView != null && playerView.IsMine;
		wasHeldOnstart = isHeld;
		instanceData.timeSinceGrounded = data.timeSinceGrounded;
		instanceData.timeInHand = data.timeInHand;
		ItemInstanceBehaviour[] array = components;
		foreach (ItemInstanceBehaviour itemInstanceBehaviour in array)
		{
			itemInstanceBehaviour.isHeld = isHeld;
			itemInstanceBehaviour.itemInstance = this;
			itemInstanceBehaviour.isHeldByMe = isHeldByMe;
			itemInstanceBehaviour.isSimulatedByMe = (playerView == null && pickupView != null && pickupView.IsMine) || itemInstanceBehaviour.isHeldByMe;
			itemInstanceBehaviour.ConfigItem(data, playerView);
		}
	}

	public void RegisterRPC(ItemRPC rpc, Action<BinaryDeserializer> action)
	{
		if (m_guid.IsNone)
		{
			Debug.LogError("Item instance has no guid");
		}
		else
		{
			ItemInstanceHandler.RegisterRPC(m_guid.Value, rpc, action);
		}
	}

	private void OnDestroy()
	{
		if (m_guid.IsSome)
		{
			ItemInstanceHandler.UnregisterItemInstance(m_guid.Value);
		}
		if (m_syncer != null && m_syncer.isMine && !PhotonNetwork.loadingLevelAndPausedNetwork)
		{
			PhotonNetwork.Destroy(m_syncer.gameObject);
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if (!(GetComponentInParent<Player>() != null) && other.relativeVelocity.magnitude > 0.1f)
		{
			other.collider.GetComponentInParent<IThrowTarget>()?.HitByThrowable(this);
		}
	}

	private void OnCollisionStay(Collision other)
	{
		if (other.gameObject.IsInLayer(HelperFunctions.terrainPropMask))
		{
			instanceData.timeSinceGrounded = 0f;
		}
	}

	public void SetItemBody(Bodypart part)
	{
		itemBody = part;
	}

	public void Update()
	{
		instanceData.timeInHand = ((!isHeld) ? 0f : (instanceData.timeInHand + Time.deltaTime));
		if (!isHeld && !rig.IsSleeping())
		{
			instanceData.timeSinceGrounded += Time.deltaTime;
		}
	}

	internal void ConfigItemPosition(Player player)
	{
		Transform transform = player.refs.ragdoll.GetBodypart(BodypartType.Head).animationTarget.transform.Find("CameraPoint").transform;
		Transform transform2 = itemBody.animationTarget.transform;
		Vector3 direction = item.holdPos;
		Vector3 vector = item.holdRotation;
		if (player.input.aimIsPressed || player.data.forceAimPressed)
		{
			if (item.useAlternativeHoldingPos)
			{
				direction = item.alternativeHoldPos;
			}
			if (item.useAlternativeHoldingRot)
			{
				vector = item.alternativeHoldRot;
			}
		}
		Quaternion b = Quaternion.LookRotation(player.data.lookDirection, player.GetUpDirection());
		b *= Quaternion.Euler(Vector3.up * vector.y);
		b *= Quaternion.Euler(Vector3.right * vector.x);
		b *= Quaternion.Euler(Vector3.forward * vector.z);
		transform2.SetPositionAndRotation(Vector3.Lerp(transform2.position, transform.position + player.LookDirection(direction), m_fixedPositionTime), Quaternion.Lerp(transform2.rotation, b, m_fixedRotationTime));
	}

	public void CallRPC(ItemRPC rpc, BinarySerializer binarySerializer)
	{
		if (m_guid.IsNone)
		{
			Debug.LogError("Item instance has no guid");
		}
		else
		{
			ItemInstanceHandler.CallRPC(m_guid.Value, rpc, binarySerializer);
		}
	}
}
