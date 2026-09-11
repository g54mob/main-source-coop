using ExitGames.Client.Photon;
using Photon.Pun;
using Steamworks;
using UnityEngine;
using Zorro.Core;
using Zorro.PhotonUtility;

public class InRoomState : ConnectionState
{
	public override void Enter()
	{
		base.Enter();
		if (Singleton<CustomCommandListener<CustomCommandType>>.Instance != null)
		{
			Singleton<CustomCommandListener<CustomCommandType>>.ClearInstance();
		}
		CommandListener commandListener = CustomCommands<CustomCommandType>.SpawnCommandListener<CommandListener>();
		commandListener.RegisterPackage(new StartRecordingCommandPackage());
		commandListener.RegisterPackage(new StopRecordingCommandPackage());
		commandListener.RegisterPackage(new SendVideoChunkPackage());
		commandListener.RegisterPackage(new AddClipToShareQueuePackage());
		commandListener.RegisterPackage(new ActivateNextSharingJobPackage());
		commandListener.RegisterPackage(new SendClipCompletedPackage());
		commandListener.RegisterPackage(new ItemInstancePackage());
		commandListener.RegisterPackage(new ReRequestClipPackage());
		commandListener.RegisterPackage(new KickPlayerNotificationPackage());
		RetrievableSingleton<RecordingsHandler>.Instance.ClearRecordings();
		PhotonNetwork.Instantiate("PlayerData", Vector3.zero, Quaternion.identity, 0);
		Hashtable hashtable = new Hashtable();
		hashtable.Add("SteamID", SteamUser.GetSteamID().m_SteamID.ToString());
		if (!PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable))
		{
			Debug.Log("Failed To Set SteamID In Custom Properties");
		}
		RetrievableSingleton<PersistentObjectsHolder>.Instance.ClearPersistentObjects();
		RichPresenceHandler.SetGroupSize(PhotonNetwork.PlayerList.Length, 4);
		RichPresenceHandler.SetGroup(PhotonNetwork.CurrentRoom.Name);
		SteamLobbyHandler steamLobbyHandler = MainMenuHandler.SteamLobbyHandler;
		if (steamLobbyHandler != null)
		{
			if (steamLobbyHandler.IsPlayingWithRandoms())
			{
				RichPresenceHandler.SetPresenceState(RichPresenceState.Status_PlayingWithRandoms);
			}
			else
			{
				RichPresenceHandler.SetPresenceState(RichPresenceState.Status_PlayingWithFriends);
			}
		}
	}
}
