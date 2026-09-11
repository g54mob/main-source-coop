using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GlobalPlayerData : MonoBehaviourPunCallbacks
{
	private PhotonView m_photonView;

	public PlayerInventory inventory;

	public float localVoiceVolume = 0.5f;

	public bool isMuted = true;

	public bool isBlocked;

	public bool canCommunicateWith;

	public bool Initialized;

	private static Dictionary<Photon.Realtime.Player, GlobalPlayerData> m_instances;

	private void Awake()
	{
		m_photonView = GetComponent<PhotonView>();
		inventory = GetComponent<PlayerInventory>();
		if (m_instances == null)
		{
			m_instances = new Dictionary<Photon.Realtime.Player, GlobalPlayerData>();
		}
		m_instances.Add(m_photonView.Owner, this);
	}

	private void OnDestroy()
	{
		m_instances.Remove(m_photonView.Owner);
	}

	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public static bool TryGetPlayerData(Photon.Realtime.Player player, out GlobalPlayerData globalPlayerData)
	{
		if (m_instances == null)
		{
			globalPlayerData = null;
			return false;
		}
		return m_instances.TryGetValue(player, out globalPlayerData);
	}

	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	{
		base.OnPlayerEnteredRoom(newPlayer);
		inventory.SyncInventoryToOthers();
		if (PhotonNetwork.IsMasterClient)
		{
			PickupHandler.SendPosToEveryone();
		}
	}

	public static void Cleanup()
	{
		m_instances = null;
	}
}
