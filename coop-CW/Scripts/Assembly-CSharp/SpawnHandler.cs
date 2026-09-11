using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SpawnHandler : MonoBehaviour
{
	private bool m_Spawned;

	[SerializeField]
	private Transform[] m_HouseSpawns;

	[SerializeField]
	private Transform[] m_DiveBellSpawns;

	[SerializeField]
	private Transform[] m_HospitalSpawns;

	private int m_LocalSpawnIndex;

	public static SpawnHandler Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
		if (PhotonNetwork.InRoom)
		{
			FindLocalSpawnIndex();
		}
	}

	private void FindLocalSpawnIndex()
	{
		Photon.Realtime.Player[] playerList = PhotonNetwork.PlayerList;
		for (int i = 0; i < playerList.Length; i++)
		{
			if (object.Equals(playerList[i], PhotonNetwork.LocalPlayer))
			{
				m_LocalSpawnIndex = i;
				break;
			}
		}
	}

	private void Start()
	{
		if (!PhotonNetwork.InRoom)
		{
			return;
		}
		if (!PhotonGameLobbyHandler.IsSurface)
		{
			SpawnLocalPlayer(Spawns.DiveBell);
			return;
		}
		FindHospitalSpawns();
		if (Player.justDied)
		{
			SpawnLocalPlayer(Spawns.Hospital);
		}
	}

	private void FindHospitalSpawns()
	{
		Transform transform = Hospital.instance.transform;
		int childCount = transform.childCount;
		m_HospitalSpawns = new Transform[childCount];
		for (int i = 0; i < m_HospitalSpawns.Length; i++)
		{
			m_HospitalSpawns[i] = transform.GetChild(i);
		}
	}

	public void SpawnLocalPlayer(Spawns state)
	{
		if (m_Spawned)
		{
			Debug.Log("Already spawned player!");
			return;
		}
		FindLocalSpawnIndex();
		Debug.Log("Spawning local player: " + state);
		Transform spawnPoint = GetSpawnPoint(state);
		m_Spawned = true;
		PhotonNetwork.Instantiate("Player", spawnPoint.transform.position, spawnPoint.transform.rotation, 0);
	}

	private Transform GetSpawnPoint(Spawns state)
	{
		switch (state)
		{
		case Spawns.DiveBell:
			if (PhotonGameLobbyHandler.IsSurface)
			{
				return m_DiveBellSpawns[m_LocalSpawnIndex];
			}
			return DiveBellParent.instance.GetSpawn().transform;
		case Spawns.House:
			return m_HouseSpawns[m_LocalSpawnIndex];
		case Spawns.Hospital:
			return m_HospitalSpawns[m_LocalSpawnIndex];
		case Spawns.CustomSpawn:
			return UnityEngine.Object.FindObjectOfType<SpawnPoint>().transform;
		default:
			throw new Exception("Cant spawn on this site: " + state);
		}
	}
}
