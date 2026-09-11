using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using Portningsbolaget.Platforms;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zorro.Core;

public class PhotonGameLobbyHandler : MonoBehaviourPunCallbacks
{
	private SteamLobbyHandler m_SteamLobby;

	public static PhotonGameLobbyHandler Instance;

	private bool m_GoingToSurface;

	public static bool IsSurface { get; private set; }

	public static Objective CurrentObjective { get; private set; }

	private void Awake()
	{
		Instance = this;
		m_SteamLobby = MainMenuHandler.SteamLobbyHandler;
		IsSurface = SceneManager.GetActiveScene().name == "SurfaceScene";
		if (!IsSurface)
		{
			SurfaceNetworkHandler.ReturnFromLostWorld();
			if (PhotonNetwork.IsMasterClient)
			{
				SetCurrentObjective(new FilmSomethingScaryObjective());
				RetrievableSingleton<PersistentObjectsHolder>.Instance.SpawnPersistentObjects();
			}
		}
		else if (PhotonNetwork.IsMasterClient)
		{
			RetrievableSingleton<PersistentObjectsHolder>.Instance.SpawnPersistentSurfaceObjects();
		}
	}

	private void Start()
	{
		PhotonNetwork.IsMessageQueueRunning = true;
		PhotonNetwork.AutomaticallySyncScene = true;
		NetworkVoiceHandler.InitNetworkVoice();
		MonoFunctions.instance.DelayCall(PickupHandler.SendPosToEveryone, 0.05f);
		Debug.Log("GameLobby Started, Surface: " + IsSurface);
		RetrievableResourceSingleton<TransitionHandler>.Instance.FadeOut(delegate
		{
			DiveBellTransitionSound.Remove();
		}, 1f);
	}

	public void SetCurrentObjective(Objective objective)
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			Debug.LogError("Only MasterClient can set the current objective!");
			return;
		}
		CurrentObjective = objective;
		base.photonView.RPC("RPC_SetCurrentObjective", RpcTarget.OthersBuffered, ObjectiveDatabase.GetIndex(objective));
		VerboseDebug.Log("Set Current Objective: " + objective.GetType().Name);
	}

	[PunRPC]
	public void RPC_SetCurrentObjective(byte index)
	{
		CurrentObjective = ObjectiveDatabase.GetObjectiveInstance(index);
		VerboseDebug.Log("Set Current Objective: " + CurrentObjective.GetType().Name);
	}

	private void Update()
	{
		if (m_SteamLobby != null && PhotonNetwork.CurrentRoom != null)
		{
			m_SteamLobby.ReceiveMessage();
			_ = PhotonNetwork.CurrentRoom.IsOpen;
		}
	}

	public void ReturnToSurface(ICollection<Player> playersInside = null)
	{
		if (!PhotonNetwork.IsMasterClient || m_GoingToSurface)
		{
			return;
		}
		SurfaceNetworkHandler.SetPlayersAliveFromUnderworld(playersInside);
		m_GoingToSurface = true;
		List<int> list = new List<int>();
		if (playersInside != null)
		{
			foreach (Player item in playersInside)
			{
				list.Add(item.refs.view.OwnerActorNr);
			}
		}
		base.photonView.RPC("RPCA_CheckIfOutsideOfDivebellPreSurface", RpcTarget.All, list.ToArray());
		TransitionGameFeel();
		base.photonView.RPC("RPC_StartTransition", RpcTarget.Others);
		RetrievableResourceSingleton<TransitionHandler>.Instance.TransitionToBlack(5f, delegate
		{
			if (PhotonNetwork.InRoom)
			{
				VerboseDebug.Log("Returning To Surface!");
				RetrievableSingleton<PersistentObjectsHolder>.Instance.FindPersistantObjects();
				PhotonNetwork.LoadLevel("SurfaceScene");
			}
		}, 3f);
	}

	private void TransitionGameFeel()
	{
		DivingBell divingBell = UnityEngine.Object.FindObjectOfType<DivingBell>();
		if (divingBell != null)
		{
			divingBell.sfx.PlayStartTransition();
		}
		GamefeelHandler.instance.perlin.AddShake(2f, 100f, 10f);
		divingBell.LockDoors();
		new GameObject("DiveBellTransitionSound").AddComponent<DiveBellTransitionSound>().Init(divingBell.sfx);
	}

	[PunRPC]
	public void RPC_StartTransition()
	{
		TransitionGameFeel();
		RetrievableResourceSingleton<TransitionHandler>.Instance.TransitionToBlack(0f, delegate
		{
			Debug.Log("Returning To Surface!");
		}, 3f);
	}

	[PunRPC]
	public void RPCA_CheckIfOutsideOfDivebellPreSurface(int[] playersAlive)
	{
		int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
		if (!playersAlive.Contains(actorNumber))
		{
			VerboseDebug.Log("I am Outside of divebell!");
			if (!Player.justDied)
			{
				Player.localPlayer.RPCA_PlayerDie();
				PlatformManager.UnlockAchievement(Achievements.ACH_LEFT_BEHIND);
			}
		}
		CheckForIllegalItems();
	}

	public void CheckForIllegalItems()
	{
		if (Player.localPlayer.TryGetInventory(out var o) && o.TryRemoveItemFromSlot(3, out var item))
		{
			Debug.Log("Removed ilegal item: " + item.item.name);
		}
	}

	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	{
		base.OnPlayerEnteredRoom(newPlayer);
		Debug.Log(newPlayer.NickName + " Entered The Room!");
		if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length > 1)
		{
			SetCurrentObjective(new LeaveHouseObjective());
		}
		RichPresenceHandler.SetGroupSize(PhotonNetwork.PlayerList.Length, 4);
	}

	public override void OnPlayerLeftRoom(Photon.Realtime.Player player)
	{
		base.OnPlayerLeftRoom(player);
		Debug.Log(player.NickName + " Left The Room! Is host? " + player.IsMasterClient);
		if (player.IsMasterClient)
		{
			if (Application.isEditor)
			{
				Debug.Log("Skipping Return to Mainmenu since editor");
			}
			string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Error_HostLeft);
			string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Error_Disconnected);
			Modal.ShowError(localizedString, localizedString2);
			RetrievableSingleton<ConnectionStateHandler>.Instance.Disconnect();
		}
		else
		{
			if (GlobalPlayerData.TryGetPlayerData(player, out var globalPlayerData))
			{
				if (PlayerHandler.instance.TryGetPlayerFromOwnerID(player.ActorNumber, out var o))
				{
					Debug.Log($"Player {player.NickName} left the room! Dropping items {o.data.dead}");
					if (globalPlayerData != null)
					{
						foreach (ItemDescriptor item in globalPlayerData.inventory.GetItems())
						{
							ItemInstanceData itemInstanceData = new ItemInstanceData(Guid.NewGuid());
							itemInstanceData.Deserialize(item.data.Serialize());
							itemInstanceData.m_guid = Guid.NewGuid();
							PickupHandler.CreatePickup(item.item.id, itemInstanceData, o.Center(), Quaternion.identity);
						}
					}
				}
				else
				{
					Debug.LogError("Player left the room but no player controller found!");
				}
			}
			else
			{
				Debug.LogError("Player left the room but no player data found!");
			}
			if (PhotonNetwork.IsMasterClient)
			{
				CheckForAllDead();
			}
		}
		RichPresenceHandler.SetGroupSize(PhotonNetwork.PlayerList.Length, 4);
	}

	public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
	{
		if (Application.isEditor)
		{
			Debug.Log("Skipping Return to Mainmenu since editor");
		}
		Debug.Log("MasterClient Left The Surface!");
		RetrievableSingleton<ConnectionStateHandler>.Instance.Disconnect();
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Error_HostLeft);
		string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Error_Disconnected);
		Modal.ShowError(localizedString, localizedString2);
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		base.OnDisconnected(cause);
		Debug.Log($"Disconnected! Returning to MainMenu {cause}");
		if (m_SteamLobby != null)
		{
			m_SteamLobby.LeaveLobby();
		}
		ConnectionState connectionState = RetrievableSingleton<ConnectionStateHandler>.Instance.CheckState();
		if (!(connectionState is JoiningSpecificRegionState) && !(connectionState is JoiningSpecificRoomState))
		{
			SceneManager.LoadScene("NewMainMenuOptimized");
		}
	}

	public void CheckForAllDead()
	{
		VerboseDebug.Log("Check for everyone dead!");
		Player[] array = (from pl in UnityEngine.Object.FindObjectsOfType<Player>()
			where !pl.ai
			select pl).ToArray();
		Player[] array2 = array;
		for (int num = 0; num < array2.Length; num++)
		{
			if (!array2[num].data.dead)
			{
				return;
			}
		}
		Debug.Log("Everyone dead!");
		if (array.Length <= 1)
		{
			_ = Application.isEditor;
		}
		StartCoroutine(WaitThen(5f, delegate
		{
			ReturnToSurface();
		}));
		if (Time.timeSinceLevelLoad < 30f && PhotonNetwork.InRoom)
		{
			PlatformManager.UnlockAchievement(Achievements.ACH_PWNED);
		}
	}

	private IEnumerator WaitThen(float t, Action a)
	{
		yield return new WaitForSecondsRealtime(t);
		a?.Invoke();
	}

	public void DiveToOldworld()
	{
		CheckForIllegalItems();
	}
}
