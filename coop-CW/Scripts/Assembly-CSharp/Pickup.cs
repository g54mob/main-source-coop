using System;
using System.Collections;
using EPOOutline;
using Photon.Pun;
using UnityEngine;

public class Pickup : Interactable
{
	public PhotonView m_photonView;

	private Rigidbody m_rigidbody;

	private ItemInstanceData instanceData;

	private byte m_itemID;

	private Vector3 lastPos;

	public ItemInstance itemInstance;

	public Rigidbody Rigidbody => m_rigidbody;

	protected override void Awake()
	{
		base.Awake();
		m_photonView = GetComponent<PhotonView>();
	}

	private void Start()
	{
		lastPos = base.transform.position;
	}

	public override void Interact(Player player)
	{
		if (player.refs.view.IsMine)
		{
			m_rigidbody.gameObject.SetActive(value: false);
			m_photonView.RPC("RPC_RequestPickup", RpcTarget.MasterClient, player.refs.view.ViewID);
		}
	}

	public override void OnStartHover(Player player)
	{
		base.OnStartHover(player);
	}

	public override void OnEndHover(Player player)
	{
		base.OnEndHover(player);
	}

	public void ConfigurePickup(byte itemID, ItemInstanceData data)
	{
		byte[] array = Array.Empty<byte>();
		if (data != null)
		{
			array = data.Serialize();
		}
		m_photonView.RPC("RPC_ConfigurePickup", RpcTarget.AllBuffered, itemID, array);
	}

	public void ConfigurePickup(byte itemID, ItemInstanceData data, Vector3 vel, Vector3 angVel)
	{
		byte[] array = Array.Empty<byte>();
		if (data != null)
		{
			array = data.Serialize();
		}
		m_photonView.RPC("RPC_ConfigurePickup", RpcTarget.AllBuffered, itemID, array);
		m_photonView.RPC("RPC_SetVelocity", RpcTarget.All, vel, angVel);
	}

	[PunRPC]
	public void RPC_ConfigurePickup(byte itemID, byte[] data)
	{
		m_itemID = itemID;
		Collider[] colliders;
		if (ItemDatabase.TryGetItemFromID(itemID, out var item))
		{
			string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.PickUp);
			if (item.displayName == "")
			{
				hoverText = localizedString + " " + item.GetLocalizedDisplayName();
			}
			else
			{
				hoverText = localizedString + " " + item.GetLocalizedDisplayName();
			}
			instanceData = new ItemInstanceData(Guid.Empty);
			instanceData.Deserialize(data);
			ItemInstanceDataHandler.AddInstanceData(instanceData);
			GameObject gameObject = UnityEngine.Object.Instantiate(item.itemObject, base.transform);
			itemInstance = gameObject.GetComponent<ItemInstance>();
			itemInstance.InitItem(item, instanceData, null, m_photonView);
			m_rigidbody = gameObject.GetComponent<Rigidbody>();
			m_rigidbody.useGravity = true;
			m_rigidbody.isKinematic = true;
			if (m_rigidbody.linearDamping < 0.1f)
			{
				m_rigidbody.linearDamping = 0.1f;
			}
			if (m_rigidbody.angularDamping < 0.1f)
			{
				m_rigidbody.angularDamping = 0.1f;
			}
			gameObject.transform.position = base.transform.position;
			gameObject.transform.rotation = base.transform.rotation;
			m_rigidbody.position = base.transform.position;
			m_rigidbody.rotation = base.transform.rotation;
			if (PlayerHandler.instance.TryGetPlayerFromOwnerID(m_photonView.OwnerActorNr, out var o))
			{
				ConfigurableJoint configurableJoint = gameObject.AddComponent<ConfigurableJoint>();
				configurableJoint.enableCollision = false;
				configurableJoint.connectedBody = o.refs.ragdoll.GetBodypart(BodypartType.Hand_R).rig;
				StartCoroutine(RemoveJoint(configurableJoint));
			}
			m_outline.AddAllChildRenderersToRenderingList(RenderersAddingMode.All, includeInactive: false);
			m_outline.enabled = false;
			colliders = GetComponentsInChildren<Collider>();
			StartCoroutine(EnableColliders());
			m_rigidbody.isKinematic = false;
			GameAPI.instance.objectSpawnedAction?.Invoke(base.gameObject);
		}
		else
		{
			Debug.LogError("Failed to get item from ID");
		}
		IEnumerator EnableColliders()
		{
			yield return null;
			yield return null;
			int layer = HelperFunctions.GetLayer(HelperFunctions.LayerType.Interactable);
			Collider[] array = colliders;
			foreach (Collider obj in array)
			{
				obj.enabled = true;
				obj.gameObject.layer = layer;
			}
		}
		static IEnumerator RemoveJoint(ConfigurableJoint configurableJoint2)
		{
			yield return new WaitForSeconds(0.5f);
			if (configurableJoint2 != null)
			{
				UnityEngine.Object.Destroy(configurableJoint2);
			}
		}
	}

	[PunRPC]
	public void RPC_SetVelocity(Vector3 vel, Vector3 angVel)
	{
		m_rigidbody.angularVelocity = angVel;
		m_rigidbody.linearVelocity = vel;
	}

	[PunRPC]
	public void RPC_Remove()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			if (ItemDatabase.TryGetItemFromID(m_itemID, out var item) && item.itemType == Item.ItemType.Camera && PhotonGameLobbyHandler.CurrentObjective is PickupTheCameraObjective)
			{
				PhotonGameLobbyHandler.Instance.SetCurrentObjective(new EnterDiveBellObjective());
			}
			PhotonNetwork.Destroy(base.gameObject);
		}
		else
		{
			Debug.LogError("Only master client can remove pickups");
		}
	}

	[PunRPC]
	public void RPC_RequestPickup(int photonView)
	{
		bool flag = false;
		Player component = PhotonNetwork.GetPhotonView(photonView).GetComponent<Player>();
		if (ItemDatabase.TryGetItemFromID(m_itemID, out var item) && component.TryGetInventory(out var o))
		{
			ItemInstanceData data = instanceData.Copy();
			if (o.TryAddItem(new ItemDescriptor(item, data), out var slot))
			{
				component.refs.view.RPC("RPC_SelectSlot", component.refs.view.Owner, slot.SlotID);
				m_photonView.RPC("RPC_Remove", RpcTarget.MasterClient);
				flag = true;
			}
		}
		if (!flag)
		{
			m_photonView.RPC("RPC_FailedToPickup", component.refs.view.Owner);
		}
	}

	private void OnEnable()
	{
		PickupHandler.RegisterPickup(this);
	}

	private void OnDisable()
	{
		PickupHandler.UnregisterPickup(this);
		ItemInstanceDataHandler.RemoveInstanceData(instanceData.m_guid);
	}

	public void ForceSync()
	{
		m_photonView.RPC("RPC_ForceSync", RpcTarget.Others, m_rigidbody.transform.position, m_rigidbody.transform.rotation);
	}

	[PunRPC]
	public void RPC_ForceSync(Vector3 pos, Quaternion rot)
	{
		GetComponent<PhysicsObjectSyncer>().SetInitialPos(pos, rot);
	}

	[PunRPC]
	public void RPC_FailedToPickup()
	{
		m_rigidbody.gameObject.SetActive(value: true);
	}
}
