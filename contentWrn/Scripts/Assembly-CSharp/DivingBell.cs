using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zorro.Core;

public class DivingBell : MonoBehaviourPunCallbacks
{
	public DiveBellSFX sfx;

	public DivingBellStateMachine StateMachine;

	public DiveBellPlayerDetector playerDetector;

	public DiveBellPickupDetector pickupDetector;

	public bool opened;

	public bool onSurface;

	private bool m_isMovingDoor;

	private PhotonView m_photonView;

	public float spawnDifficulty;

	public Transform itemSpawns;

	public DivingBellDoor door;

	public bool locked;

	private List<string> sceneNames = new List<string> { "FactoryScene", "MinesScene", "HarbourScene" };

	private void Awake()
	{
		StateMachine = new DivingBellStateMachine();
		StateMachine.RegisterState(new DivingBellReadyState(onSurface));
		StateMachine.RegisterState(new DivingBellNotReadyDoorOpenState());
		StateMachine.RegisterState(new DivingBellNotReadyMissingPlayersState());
		StateMachine.RegisterState(new DivingBellRechargingState());
		StateMachine.SwitchState<DivingBellNotReadyDoorOpenState>();
	}

	private void Start()
	{
		m_photonView = GetComponent<PhotonView>();
		opened = onSurface && TimeOfDayHandler.TimeOfDay == TimeOfDay.Morning;
		if (PhotonNetwork.IsMasterClient && onSurface && !opened && SurfaceNetworkHandler.NumberOfPlayersAliveFromUnderWorld == 0)
		{
			m_photonView.RPC("SetDoorStateInstant", RpcTarget.All, true);
		}
		door.Init(opened);
	}

	private void Update()
	{
		if (Time.frameCount % 3 != 0)
		{
			return;
		}
		List<Player> players = PlayerHandler.instance.players;
		ICollection<Player> collection = playerDetector.CheckForPlayers();
		bool flag = collection.Count == players.Count;
		foreach (Player item in players)
		{
			item.data.isInDiveBell = collection.Contains(item);
		}
		bool flag2 = door.IsFullyClosed();
		bool flag3 = onSurface && TimeOfDayHandler.TimeOfDay == TimeOfDay.Evening;
		bool flag4 = flag && flag2 && !flag3;
		if (!onSurface)
		{
			if (!flag2)
			{
				StateMachine.SwitchState<DivingBellNotReadyDoorOpenState>();
			}
			else
			{
				StateMachine.SwitchState<DivingBellReadyState>();
			}
		}
		else if (flag4 && !(StateMachine.CurrentState is DivingBellReadyState))
		{
			StateMachine.SwitchState<DivingBellReadyState>();
		}
		else if (onSurface && TimeOfDayHandler.TimeOfDay == TimeOfDay.Evening)
		{
			StateMachine.SwitchState<DivingBellRechargingState>();
		}
		else if (!flag)
		{
			StateMachine.SwitchState<DivingBellNotReadyMissingPlayersState>();
		}
		else if (!flag2)
		{
			StateMachine.SwitchState<DivingBellNotReadyDoorOpenState>();
		}
	}

	public void AttemptSetOpen(bool open)
	{
		if (open)
		{
			m_photonView.RPC("RPC_Open", RpcTarget.All);
		}
		else
		{
			m_photonView.RPC("RPC_Close", RpcTarget.All);
		}
	}

	[PunRPC]
	public void RPC_Open()
	{
		if (!opened && !m_isMovingDoor)
		{
			opened = true;
			StartCoroutine(MoveDoor(door.Open()));
		}
	}

	[PunRPC]
	public void RPC_Close()
	{
		if (opened && !m_isMovingDoor)
		{
			opened = false;
			StartCoroutine(MoveDoor(door.Close()));
		}
	}

	[PunRPC]
	public void SetDoorStateInstant(bool state)
	{
		opened = state;
		door.Init(opened);
	}

	[PunRPC]
	public void RPC_GoToUnderground()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			Debug.LogError("Only the master client can call this method");
			return;
		}
		SurfaceNetworkHandler instance = SurfaceNetworkHandler.Instance;
		if (instance == null)
		{
			Debug.LogError("SurfaceNetworkHandler is null");
		}
		else if (!(StateMachine.CurrentState is DivingBellReadyState))
		{
			Debug.Log("Diving Bell dive button pressed, but bell is not ready to go underground");
		}
		else if (instance.PreCheckHeadToUnderWorld())
		{
			base.photonView.RPC("RPC_StartTransition", RpcTarget.Others);
			PhotonGameLobbyHandler.Instance.CheckForIllegalItems();
			TransitionGameFeel();
			RetrievableResourceSingleton<TransitionHandler>.Instance.TransitionToBlack(0.15f, delegate
			{
				string undergroundSceneName = GetUndergroundSceneName();
				Debug.Log($"Day {GameAPI.CurrentDay} Going To " + undergroundSceneName);
				RetrievableSingleton<PersistentObjectsHolder>.Instance.FindPersistantSurfaceObjects();
				PhotonNetwork.LoadLevel(undergroundSceneName);
			}, 3f);
		}
	}

	private string GetUndergroundSceneName()
	{
		return sceneNames[SurfaceNetworkHandler.RoomStats.LevelToPlay];
	}

	private void TransitionGameFeel()
	{
		GamefeelHandler.instance.perlin.AddShake(2f, 100f, 10f);
		sfx.PlayStartTransition();
		new GameObject("DiveBellTransitionSound").AddComponent<DiveBellTransitionSound>().Init(sfx);
		LockDoors();
	}

	[PunRPC]
	public void RPC_StartTransition()
	{
		PhotonGameLobbyHandler.Instance.CheckForIllegalItems();
		TransitionGameFeel();
		RetrievableResourceSingleton<TransitionHandler>.Instance.TransitionToBlack(0f, delegate
		{
			Debug.Log("Going To Underworld!");
		}, 3f);
	}

	[PunRPC]
	public void RPC_GoToSurface()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			Debug.LogError("Only the master client can call this method");
			return;
		}
		if (!(StateMachine.CurrentState is DivingBellReadyState))
		{
			Debug.LogError("Diving Bell is not ready to return to the surface");
			return;
		}
		ICollection<Player> playersInside = playerDetector.CheckForPlayers();
		PhotonGameLobbyHandler.Instance.ReturnToSurface(playersInside);
	}

	private IEnumerator MoveDoor(IEnumerator door)
	{
		m_isMovingDoor = true;
		yield return door;
		m_isMovingDoor = false;
	}

	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	{
		base.OnPlayerEnteredRoom(newPlayer);
		if (PhotonNetwork.IsMasterClient)
		{
			m_photonView.RPC("SetDoorStateInstant", newPlayer, opened);
		}
	}

	public void GoUnderground()
	{
		m_photonView.RPC("RPC_GoToUnderground", RpcTarget.MasterClient);
	}

	public void GoToSurface()
	{
		m_photonView.RPC("RPC_GoToSurface", RpcTarget.MasterClient);
	}

	public void LockDoors()
	{
		locked = true;
	}
}
