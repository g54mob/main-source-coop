using System;
using Photon.Pun;
using UnityEngine;

public class ItemDataSyncer : MonoBehaviour
{
	private PhotonView m_photonView;

	private ItemInstanceData m_instanceData;

	private ItemInstance m_itemInstance;

	private Guid m_guid;

	public bool isMine => m_photonView.IsMine;

	private void Awake()
	{
		m_photonView = GetComponent<PhotonView>();
	}

	[PunRPC]
	public void RPC_SetInstanceData(byte[] id)
	{
		VerboseDebug.Log("Setting instance data...");
		m_guid = new Guid(id);
		if (ItemInstanceDataHandler.TryGetInstanceData(m_guid, out var o))
		{
			m_instanceData = o;
			VerboseDebug.Log($"Set Instance Data: {m_guid}");
		}
	}

	[PunRPC]
	public void RPC_Update(byte[] buffer)
	{
		VerboseDebug.Log($"Updating instance data, size: {buffer}");
		if (m_instanceData == null)
		{
			if (!ItemInstanceDataHandler.TryGetInstanceData(m_guid, out var o))
			{
				return;
			}
			m_instanceData = o;
			Debug.Log($"Set Instance Data: {m_guid}");
		}
		m_instanceData.Deserialize(buffer);
	}

	private void Update()
	{
		if (m_instanceData != null)
		{
			if (m_photonView.IsMine && m_instanceData.IsDirty())
			{
				m_instanceData.ClearDirty();
				VerboseDebug.Log($"Syncing item data for item: {m_instanceData.m_guid}");
				m_photonView.RPC("RPC_Update", RpcTarget.Others, m_instanceData.Serialize());
			}
			if (m_instanceData.IsForceDirty())
			{
				m_instanceData.ClearForceDirty();
				VerboseDebug.Log($"force Syncing item data for item: {m_instanceData.m_guid}");
				m_photonView.RPC("RPC_Update", RpcTarget.Others, m_instanceData.Serialize());
			}
		}
	}
}
