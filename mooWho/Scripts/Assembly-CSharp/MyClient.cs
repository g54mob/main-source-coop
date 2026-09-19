using System;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using Steamworks;
using UnityEngine;

public class MyClient : NetworkBehaviour
{
	[SyncVar(hook = "PlayerInfoUpdate")]
	public PlayerInfoData playerInfo;

	[SyncVar(hook = "IsReadyUpdate")]
	public bool IsReady;

	[SyncVar]
	public bool InReadyZone;

	[SyncVar(hook = "OnWantsHunterChanged")]
	public bool WantsHunter;

	protected Callback<AvatarImageLoaded_t> avatarImageLoaded;

	private bool _inReadyTrigger;

	private bool _inHunterTrigger;

	public Action<PlayerInfoData, PlayerInfoData> _Mirror_SyncVarHookDelegate_playerInfo;

	public Action<bool, bool> _Mirror_SyncVarHookDelegate_IsReady;

	public Action<bool, bool> _Mirror_SyncVarHookDelegate_WantsHunter;

	public Sprite icon { get; private set; }

	public CharacterSkinElement characterInstance { get; set; }

	public bool IsHostPlayer => base.connectionToClient == NetworkServer.localConnection;

	public PlayerInfoData NetworkplayerInfo
	{
		get
		{
			return playerInfo;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref playerInfo, 1uL, _Mirror_SyncVarHookDelegate_playerInfo);
		}
	}

	public bool NetworkIsReady
	{
		get
		{
			return IsReady;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref IsReady, 2uL, _Mirror_SyncVarHookDelegate_IsReady);
		}
	}

	public bool NetworkInReadyZone
	{
		get
		{
			return InReadyZone;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref InReadyZone, 4uL, null);
		}
	}

	public bool NetworkWantsHunter
	{
		get
		{
			return WantsHunter;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref WantsHunter, 8uL, _Mirror_SyncVarHookDelegate_WantsHunter);
		}
	}

	private void OnAvatarImageLoaded(AvatarImageLoaded_t callback)
	{
		CSteamID steamID = callback.m_steamID;
		Debug.Log("Avatar loaded " + steamID.ToString());
		if (callback.m_steamID.m_SteamID == playerInfo.steamId)
		{
			SetIcon(callback.m_steamID);
		}
	}

	private void SetIcon(CSteamID steamId)
	{
		Texture2D avatar = SteamHelper.GetAvatar(steamId);
		if ((bool)avatar)
		{
			icon = SteamHelper.ConvertTextureToSprite(avatar);
		}
	}

	private void Start()
	{
		((MyNetworkManager)NetworkManager.singleton).allClients.Add(this);
		if ((bool)CharacterSkinHandler.instance)
		{
			CharacterSkinHandler.instance.SpawnCharacterMesh(this);
		}
		if (SteamManager.Initialized)
		{
			avatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);
		}
	}

	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();
		CmdEnterLobby();
		CSteamID cSteamID = (SteamManager.Initialized ? SteamUser.GetSteamID() : CSteamID.Nil);
		string username = (SteamManager.Initialized ? SteamFriends.GetPersonaName() : "Player");
		CmdSetPlayerInfo(username, cSteamID.m_SteamID);
	}

	[Command]
	private void CmdEnterLobby()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendCommandInternal("System.Void MyClient::CmdEnterLobby()", -891777530, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[Command]
	private void CmdMoveToLobbySpawn()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendCommandInternal("System.Void MyClient::CmdMoveToLobbySpawn()", -1844867025, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[TargetRpc]
	private void TargetMoveToLobbySpawn(NetworkConnection target, Vector3 pos, Quaternion rot)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteVector3(pos);
		writer.WriteQuaternion(rot);
		SendTargetRPCInternal(target, "System.Void MyClient::TargetMoveToLobbySpawn(Mirror.NetworkConnection,UnityEngine.Vector3,UnityEngine.Quaternion)", 1976512277, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[Command]
	private void CmdSetPlayerInfo(string username, ulong steamId)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(username);
		writer.WriteVarULong(steamId);
		SendCommandInternal("System.Void MyClient::CmdSetPlayerInfo(System.String,System.UInt64)", -756408138, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	public void ToggleReady()
	{
		Cmd_ToggleReady();
	}

	[Command]
	private void Cmd_ToggleReady()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendCommandInternal("System.Void MyClient::Cmd_ToggleReady()", 1550966656, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	public void OnEnterZone(LobbyZoneType zone)
	{
		switch (zone)
		{
		case LobbyZoneType.HunterVolunteer:
			_inHunterTrigger = true;
			CmdSetHunterVolunteer(wants: true);
			LobbyManager.Instance?.SetHunterVolunteerNotificationVisible(visible: true);
			break;
		case LobbyZoneType.Ready:
			_inReadyTrigger = true;
			LobbyManager.Instance?.SetReadyNotificationVisible(visible: true);
			break;
		}
		RecomputeReadyZone();
	}

	public void OnExitZone(LobbyZoneType zone)
	{
		switch (zone)
		{
		case LobbyZoneType.HunterVolunteer:
			_inHunterTrigger = false;
			CmdSetHunterVolunteer(wants: false);
			LobbyManager.Instance?.SetHunterVolunteerNotificationVisible(visible: false);
			break;
		case LobbyZoneType.Ready:
			_inReadyTrigger = false;
			LobbyManager.Instance?.SetReadyNotificationVisible(visible: false);
			break;
		}
		RecomputeReadyZone();
	}

	private void RecomputeReadyZone()
	{
		bool flag = _inReadyTrigger || _inHunterTrigger;
		CmdSetReadyZone(flag);
		LobbyManager.Instance?.RefreshWaitingText(flag);
	}

	[Command]
	public void CmdSetHunterVolunteer(bool wants)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteBool(wants);
		SendCommandInternal("System.Void MyClient::CmdSetHunterVolunteer(System.Boolean)", 1013091969, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[Command]
	public void CmdSetReadyZone(bool ready)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteBool(ready);
		SendCommandInternal("System.Void MyClient::CmdSetReadyZone(System.Boolean)", 1805865128, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[Server]
	public void ServerKick()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void MyClient::ServerKick()' called when server was not active");
			return;
		}
		PlayerRoleData component = GetComponent<PlayerRoleData>();
		if (component != null && component.Role == PlayerRole.Animal)
		{
			GameManager.Instance?.StopSoundsForAnimalType(component.AssignedAnimal);
			VoiceNetwork.Instance?.ServerClearRecord(component.AssignedAnimal);
		}
		base.connectionToClient?.Disconnect();
	}

	private void PlayerInfoUpdate(PlayerInfoData _, PlayerInfoData data)
	{
		if ((bool)characterInstance)
		{
			characterInstance.Initialize(this, IsReady);
		}
		SetIcon(new CSteamID(data.steamId));
	}

	public void IsReadyUpdate(bool _, bool value)
	{
		if ((bool)characterInstance)
		{
			characterInstance.Initialize(this, value);
		}
		if (base.isLocalPlayer)
		{
			MainMenu.instance.UpdateReadyButton(value);
		}
	}

	private void OnWantsHunterChanged(bool _, bool value)
	{
		PlayerRoleData component = GetComponent<PlayerRoleData>();
		if (component != null)
		{
			component.ApplyVisuals();
		}
	}

	private void OnDestroy()
	{
		if ((bool)this && (bool)(MyNetworkManager)NetworkManager.singleton)
		{
			((MyNetworkManager)NetworkManager.singleton).allClients.Remove(this);
		}
		if ((bool)characterInstance && !base.isLocalPlayer)
		{
			CharacterSkinHandler.instance.DestroyCharacterMesh(this);
		}
	}

	public MyClient()
	{
		_Mirror_SyncVarHookDelegate_playerInfo = PlayerInfoUpdate;
		_Mirror_SyncVarHookDelegate_IsReady = IsReadyUpdate;
		_Mirror_SyncVarHookDelegate_WantsHunter = OnWantsHunterChanged;
	}

	public override bool Weaved()
	{
		return true;
	}

	protected void UserCode_CmdEnterLobby()
	{
		if (BarnIntroController.Instance != null)
		{
			BarnIntroController.Instance.RequestIntro(base.connectionToClient);
		}
		else
		{
			CmdMoveToLobbySpawn();
		}
	}

	protected static void InvokeUserCode_CmdEnterLobby(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdEnterLobby called on client.");
		}
		else
		{
			((MyClient)obj).UserCode_CmdEnterLobby();
		}
	}

	protected void UserCode_CmdMoveToLobbySpawn()
	{
		TargetMoveToLobbySpawn(base.connectionToClient, BarnIntroController.BarnOutsidePosition, BarnIntroController.BarnOutsideRotation);
	}

	protected static void InvokeUserCode_CmdMoveToLobbySpawn(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdMoveToLobbySpawn called on client.");
		}
		else
		{
			((MyClient)obj).UserCode_CmdMoveToLobbySpawn();
		}
	}

	protected void UserCode_TargetMoveToLobbySpawn__NetworkConnection__Vector3__Quaternion(NetworkConnection target, Vector3 pos, Quaternion rot)
	{
		CharacterController component = GetComponent<CharacterController>();
		if (component != null)
		{
			component.enabled = false;
		}
		base.transform.SetPositionAndRotation(pos, rot);
		if (component != null)
		{
			component.enabled = true;
		}
	}

	protected static void InvokeUserCode_TargetMoveToLobbySpawn__NetworkConnection__Vector3__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetMoveToLobbySpawn called on server.");
		}
		else
		{
			((MyClient)obj).UserCode_TargetMoveToLobbySpawn__NetworkConnection__Vector3__Quaternion(null, reader.ReadVector3(), reader.ReadQuaternion());
		}
	}

	protected void UserCode_CmdSetPlayerInfo__String__UInt64(string username, ulong steamId)
	{
		NetworkplayerInfo = new PlayerInfoData(username, steamId);
		MyNetworkManager singleton = MyNetworkManager.Singleton;
		if (singleton != null)
		{
			singleton.CachePlayerInfo(base.connectionToClient.connectionId, playerInfo);
		}
	}

	protected static void InvokeUserCode_CmdSetPlayerInfo__String__UInt64(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetPlayerInfo called on client.");
		}
		else
		{
			((MyClient)obj).UserCode_CmdSetPlayerInfo__String__UInt64(reader.ReadString(), reader.ReadVarULong());
		}
	}

	protected void UserCode_Cmd_ToggleReady()
	{
		NetworkIsReady = !IsReady;
	}

	protected static void InvokeUserCode_Cmd_ToggleReady(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command Cmd_ToggleReady called on client.");
		}
		else
		{
			((MyClient)obj).UserCode_Cmd_ToggleReady();
		}
	}

	protected void UserCode_CmdSetHunterVolunteer__Boolean(bool wants)
	{
		MyNetworkManager singleton = MyNetworkManager.Singleton;
		if (!(singleton == null))
		{
			NetworkWantsHunter = wants;
			singleton.SetHunterVolunteer(base.connectionToClient.connectionId, wants);
		}
	}

	protected static void InvokeUserCode_CmdSetHunterVolunteer__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetHunterVolunteer called on client.");
		}
		else
		{
			((MyClient)obj).UserCode_CmdSetHunterVolunteer__Boolean(reader.ReadBool());
		}
	}

	protected void UserCode_CmdSetReadyZone__Boolean(bool ready)
	{
		NetworkInReadyZone = ready;
		LobbyManager.Instance?.OnPlayerReadyChanged();
	}

	protected static void InvokeUserCode_CmdSetReadyZone__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetReadyZone called on client.");
		}
		else
		{
			((MyClient)obj).UserCode_CmdSetReadyZone__Boolean(reader.ReadBool());
		}
	}

	static MyClient()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(MyClient), "System.Void MyClient::CmdEnterLobby()", InvokeUserCode_CmdEnterLobby, requiresAuthority: true);
		RemoteProcedureCalls.RegisterCommand(typeof(MyClient), "System.Void MyClient::CmdMoveToLobbySpawn()", InvokeUserCode_CmdMoveToLobbySpawn, requiresAuthority: true);
		RemoteProcedureCalls.RegisterCommand(typeof(MyClient), "System.Void MyClient::CmdSetPlayerInfo(System.String,System.UInt64)", InvokeUserCode_CmdSetPlayerInfo__String__UInt64, requiresAuthority: true);
		RemoteProcedureCalls.RegisterCommand(typeof(MyClient), "System.Void MyClient::Cmd_ToggleReady()", InvokeUserCode_Cmd_ToggleReady, requiresAuthority: true);
		RemoteProcedureCalls.RegisterCommand(typeof(MyClient), "System.Void MyClient::CmdSetHunterVolunteer(System.Boolean)", InvokeUserCode_CmdSetHunterVolunteer__Boolean, requiresAuthority: true);
		RemoteProcedureCalls.RegisterCommand(typeof(MyClient), "System.Void MyClient::CmdSetReadyZone(System.Boolean)", InvokeUserCode_CmdSetReadyZone__Boolean, requiresAuthority: true);
		RemoteProcedureCalls.RegisterRpc(typeof(MyClient), "System.Void MyClient::TargetMoveToLobbySpawn(Mirror.NetworkConnection,UnityEngine.Vector3,UnityEngine.Quaternion)", InvokeUserCode_TargetMoveToLobbySpawn__NetworkConnection__Vector3__Quaternion);
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			GeneratedNetworkCode._Write_PlayerInfoData(writer, playerInfo);
			writer.WriteBool(IsReady);
			writer.WriteBool(InReadyZone);
			writer.WriteBool(WantsHunter);
			return;
		}
		writer.WriteVarULong(syncVarDirtyBits);
		if ((syncVarDirtyBits & 1L) != 0L)
		{
			GeneratedNetworkCode._Write_PlayerInfoData(writer, playerInfo);
		}
		if ((syncVarDirtyBits & 2L) != 0L)
		{
			writer.WriteBool(IsReady);
		}
		if ((syncVarDirtyBits & 4L) != 0L)
		{
			writer.WriteBool(InReadyZone);
		}
		if ((syncVarDirtyBits & 8L) != 0L)
		{
			writer.WriteBool(WantsHunter);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref playerInfo, _Mirror_SyncVarHookDelegate_playerInfo, GeneratedNetworkCode._Read_PlayerInfoData(reader));
			GeneratedSyncVarDeserialize(ref IsReady, _Mirror_SyncVarHookDelegate_IsReady, reader.ReadBool());
			GeneratedSyncVarDeserialize(ref InReadyZone, null, reader.ReadBool());
			GeneratedSyncVarDeserialize(ref WantsHunter, _Mirror_SyncVarHookDelegate_WantsHunter, reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadVarULong();
		if ((num & 1L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref playerInfo, _Mirror_SyncVarHookDelegate_playerInfo, GeneratedNetworkCode._Read_PlayerInfoData(reader));
		}
		if ((num & 2L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref IsReady, _Mirror_SyncVarHookDelegate_IsReady, reader.ReadBool());
		}
		if ((num & 4L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref InReadyZone, null, reader.ReadBool());
		}
		if ((num & 8L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref WantsHunter, _Mirror_SyncVarHookDelegate_WantsHunter, reader.ReadBool());
		}
	}
}
