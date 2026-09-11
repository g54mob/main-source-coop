using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Photon.Pun;
using Portningsbolaget;
using Steamworks;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Zorro.Core;
using Zorro.Core.Serizalization;

public class SteamLobbyHandler
{
	public enum LobbyChatMsgType : byte
	{
		Invalid = 0,
		RequestRoomName = 1,
		RoomNameReceive = 2
	}

	private const string PHOTON_ROOM_KEY = "PhotonRoom";

	private const string PHOTON_REGION_KEY = "PhotonRegion";

	private const string PRIVATE_MATCH_KEY = "PrivateMatch";

	private const string GAME_VERSION_KEY = "ContentWarningVersion";

	private const string MAIN_LANGUAGE_KEY = "MainLanguage";

	private const string PLUGINS_KEY = "Plugins";

	private Action<ulong, bool> OnHostedAction;

	private Action<string, string, bool> OnJoinedAction;

	private CallResult<LobbyCreated_t> m_OnLobbyCreatedCallBack;

	private CallResult<LobbyEnter_t> m_OnLobbyEnterCallBack;

	private Callback<LobbyChatUpdate_t> m_OnLobbyChatUpdateCallBack;

	private Callback<LobbyDataUpdate_t> m_OnLobbyUpdateCallBack;

	private Callback<GameLobbyJoinRequested_t> m_OnInviteCallResult;

	private Callback<SteamNetworkingMessagesSessionRequest_t> m_OnMessageSessionRequest;

	private Callback<SteamNetworkingMessagesSessionFailed_t> m_OnMessageSessionFailed;

	private Callback<LobbyChatMsg_t> m_OnLobbyChatMessage;

	public bool UseSteamNetwork;

	private int m_MaxPlayers;

	private CSteamID m_CurrentLobby;

	private List<SteamNetworkingIdentity> m_Members;

	private bool m_Joined;

	private Thread m_ReceiveThread;

	private bool m_isPrivateMatch;

	private bool m_isHostingPrivate;

	private bool m_WaitingForRoomName;

	private Guid m_lobbyHandlerID;

	private CSteamID m_JoiningLobby = CSteamID.Nil;

	private IntPtr[] receiveBuffers = new IntPtr[16];

	public bool IsMasterClient => SteamMatchmaking.GetLobbyOwner(m_CurrentLobby) == SteamUser.GetSteamID();

	public CSteamID LobbyOwner => SteamMatchmaking.GetLobbyOwner(m_CurrentLobby);

	public IEnumerable<SteamNetworkingIdentity> Members => m_Members;

	public SteamLobbyHandler(int maxPlayers, bool useSteamNetwork)
	{
		m_lobbyHandlerID = Guid.NewGuid();
		if (!SteamManager.Initialized)
		{
			Debug.LogError("SteamManager Not Initialized, abort...");
			return;
		}
		UseSteamNetwork = useSteamNetwork;
		m_Members = new List<SteamNetworkingIdentity>();
		m_Joined = false;
		m_CurrentLobby = CSteamID.Nil;
		m_MaxPlayers = maxPlayers;
		Debug.Log($"SteamLobbyHandler Started With Max Players: {m_MaxPlayers}");
		InitCallbacks();
	}

	public void ClearAndReset(Action<string, string, bool> onJoinedRoomAction)
	{
		LeaveLobby();
		OnJoinedAction = onJoinedRoomAction;
		m_Members.Clear();
		m_Joined = false;
		m_CurrentLobby = CSteamID.Nil;
		m_isPrivateMatch = false;
		m_isHostingPrivate = false;
		m_WaitingForRoomName = false;
	}

	private void InitThread()
	{
		if (!UseSteamNetwork)
		{
			Debug.Log("Not Using Steamnetwork, returning Thread Init Early");
		}
		else if (m_ReceiveThread != null)
		{
			Debug.LogError("Trying to init receive thread more than once!");
		}
	}

	private void InitCallbacks()
	{
		m_OnLobbyCreatedCallBack = CallResult<LobbyCreated_t>.Create(OnLobbyCreatedCallback);
		m_OnLobbyEnterCallBack = CallResult<LobbyEnter_t>.Create(OnLobbyEnterCallback);
		m_OnInviteCallResult = Callback<GameLobbyJoinRequested_t>.Create(OnIniviteCallback);
		m_OnLobbyChatUpdateCallBack = Callback<LobbyChatUpdate_t>.Create(OnChatUpdateCallback);
		m_OnLobbyUpdateCallBack = Callback<LobbyDataUpdate_t>.Create(OnLobbyDataUpdate);
		m_OnLobbyChatMessage = Callback<LobbyChatMsg_t>.Create(OnLobbyChatMessage);
		if (UseSteamNetwork)
		{
			m_OnMessageSessionRequest = Callback<SteamNetworkingMessagesSessionRequest_t>.Create(OnNetworkingSessionRequest);
			m_OnMessageSessionFailed = Callback<SteamNetworkingMessagesSessionFailed_t>.Create(OnNetworkingSessionRequestFailed);
		}
		else
		{
			Debug.Log("Not Using SteamNetwork, skip OnRequest Callback");
		}
	}

	public void HostMatch(Action<ulong, bool> _action, bool privateMatch)
	{
		m_isHostingPrivate = privateMatch;
		m_Joined = false;
		OnHostedAction = _action;
		SteamAPICall_t hAPICall = SteamMatchmaking.CreateLobby(privateMatch ? ELobbyType.k_ELobbyTypeFriendsOnly : ELobbyType.k_ELobbyTypePrivate, m_MaxPlayers);
		m_OnLobbyCreatedCallBack.Set(hAPICall);
	}

	private void OnLobbyChatMessage(LobbyChatMsg_t param)
	{
		if (param.m_ulSteamIDLobby != m_CurrentLobby.m_SteamID)
		{
			Debug.LogError("Receiving ChatMsg from another lobby?!");
			return;
		}
		if (param.m_ulSteamIDUser == SteamUser.GetSteamID().m_SteamID)
		{
			Debug.Log("Skipping Msg From Local User!");
			return;
		}
		Debug.Log("LobbyChatMessage Received from: " + param.m_ulSteamIDUser);
		byte[] array = new byte[1024];
		CSteamID pSteamIDUser;
		EChatEntryType peChatEntryType;
		int lobbyChatEntry = SteamMatchmaking.GetLobbyChatEntry(m_CurrentLobby, (int)param.m_iChatID, out pSteamIDUser, array, array.Length, out peChatEntryType);
		if (lobbyChatEntry <= 0)
		{
			Debug.LogError("Failed To Read Steam Lobby Chat Message, Bytes Written Into Buffer Is Zero?");
			return;
		}
		_ = new byte[lobbyChatEntry - 1];
		BinaryDeserializer binaryDeserializer = new BinaryDeserializer(array.ToNativeArray(Allocator.Temp));
		LobbyChatMsgType msgType = (LobbyChatMsgType)binaryDeserializer.ReadByte();
		if (peChatEntryType != EChatEntryType.k_EChatEntryTypeChatMsg)
		{
			Debug.LogError("Unexpected Chatmsg: " + peChatEntryType);
			binaryDeserializer.Dispose();
		}
		else
		{
			HandleReceivedLobbyMessage(msgType, binaryDeserializer, param.m_ulSteamIDUser);
			binaryDeserializer.Dispose();
		}
	}

	private void HandleReceivedLobbyMessage(LobbyChatMsgType msgType, BinaryDeserializer deserializer, ulong sender)
	{
		switch (msgType)
		{
		case LobbyChatMsgType.RequestRoomName:
			if (!m_Joined)
			{
				SendLobbyChatMessage(LobbyChatMsgType.RoomNameReceive);
			}
			break;
		case LobbyChatMsgType.RoomNameReceive:
		{
			if (!m_Joined)
			{
				Debug.LogError("Received Room Name As Master, Should Not Happen!");
				break;
			}
			if (sender != LobbyOwner.m_SteamID)
			{
				Debug.LogError("Received Roomname From NOT Master client, should not happen, fishy?");
				break;
			}
			if (!m_WaitingForRoomName)
			{
				Debug.Log("Not Waiting For RoomName, Skipping...");
				break;
			}
			m_WaitingForRoomName = false;
			string text = deserializer.ReadString(Encoding.Unicode);
			Debug.Log("Got RoomName: " + text);
			JoinPhotonRoom(text);
			break;
		}
		default:
			Debug.LogError("Invalid Steam Lobby Message Received: " + msgType);
			break;
		}
	}

	private void OnLobbyDataUpdate(LobbyDataUpdate_t param)
	{
		if (param.m_bSuccess == 1 && param.m_ulSteamIDLobby == m_JoiningLobby.m_SteamID)
		{
			TryToJoinLobby(m_JoiningLobby);
		}
	}

	private void OnChatUpdateCallback(LobbyChatUpdate_t data)
	{
		Debug.Log("Steam Lobby Chat Update: " + data.m_ulSteamIDLobby);
		if (m_CurrentLobby.m_SteamID == data.m_ulSteamIDLobby)
		{
			EChatMemberStateChange rgfChatMemberStateChange = (EChatMemberStateChange)data.m_rgfChatMemberStateChange;
			Debug.Log("Steam Lobby Chat Update For This Lobby: " + data.m_ulSteamIDLobby + " State: " + rgfChatMemberStateChange);
			CSteamID cSteamID = new CSteamID(data.m_ulSteamIDUserChanged);
			switch (rgfChatMemberStateChange)
			{
			case EChatMemberStateChange.k_EChatMemberStateChangeEntered:
				AddSteamClient(cSteamID);
				break;
			case EChatMemberStateChange.k_EChatMemberStateChangeLeft:
			case EChatMemberStateChange.k_EChatMemberStateChangeDisconnected:
				RemoveSteamClient(cSteamID);
				break;
			}
			GetSteamClientsInLobby();
		}
	}

	private void OnIniviteCallback(GameLobbyJoinRequested_t param)
	{
		CSteamID steamIDFriend = param.m_steamIDFriend;
		Debug.Log("Invite from: " + steamIDFriend.ToString());
		TryToJoinLobby(param.m_steamIDLobby);
	}

	private void TryToJoinLobby(CSteamID lobbyID)
	{
		if (!SteamMatchmaking.RequestLobbyData(lobbyID))
		{
			return;
		}
		string lobbyData = SteamMatchmaking.GetLobbyData(lobbyID, "ContentWarningVersion");
		if (string.IsNullOrEmpty(lobbyData))
		{
			m_JoiningLobby = lobbyID;
			return;
		}
		m_JoiningLobby = CSteamID.Nil;
		if (lobbyData == PlatformsConfig.GetMatchmakingVersion())
		{
			JoinLobby(lobbyID);
			return;
		}
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.JoinError);
		string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.JoinErrorMismatch);
		string localizedString3 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Ok);
		Modal.Show(localizedString, localizedString2, new ModalOption[1]
		{
			new ModalOption(localizedString3)
		});
	}

	public void JoinLobby(CSteamID lobbyID)
	{
		CSteamID currentLobby;
		if (m_CurrentLobby != CSteamID.Nil)
		{
			currentLobby = m_CurrentLobby;
			Debug.Log("Leaving current lobby: " + currentLobby.ToString());
			SteamMatchmaking.LeaveLobby(m_CurrentLobby);
		}
		m_Joined = true;
		SteamAPICall_t hAPICall = SteamMatchmaking.JoinLobby(lobbyID);
		m_OnLobbyEnterCallBack.Set(hAPICall);
		currentLobby = lobbyID;
		Debug.Log("Joining lobby: " + currentLobby.ToString());
	}

	public void LeaveLobby()
	{
		if (m_ReceiveThread != null)
		{
			m_ReceiveThread.Abort();
		}
		m_ReceiveThread = null;
		if (m_CurrentLobby != CSteamID.Nil)
		{
			CSteamID currentLobby = m_CurrentLobby;
			Debug.Log("Leaving current lobby: " + currentLobby.ToString());
			SteamMatchmaking.LeaveLobby(m_CurrentLobby);
		}
		else
		{
			Debug.LogError("Leave SteamLobby Called but Cant Leave Since No Active Lobby!");
		}
		CloseConnections();
		m_Members.Clear();
	}

	private void CloseConnections()
	{
		foreach (SteamNetworkingIdentity member in m_Members)
		{
			SteamNetworkingIdentity identityRemote = member;
			if (SteamNetworkingMessages.CloseSessionWithUser(ref identityRemote))
			{
				Debug.Log("Closing Session With User: " + identityRemote.GetSteamID().m_SteamID);
			}
			else
			{
				Debug.Log("Error When Closing Session With User: " + identityRemote.GetSteamID().m_SteamID);
			}
		}
	}

	private void OnLobbyCreatedCallback(LobbyCreated_t param, bool bioFailure)
	{
		Debug.Log($"Steam Lobby Created Callback {param.m_eResult} BioFail {bioFailure}");
		if (param.m_eResult != EResult.k_EResultOK)
		{
			OnHostedAction?.Invoke(0uL, m_isHostingPrivate);
			return;
		}
		InitThread();
		m_CurrentLobby = new CSteamID(param.m_ulSteamIDLobby);
		Debug.Log("Making lobby not joinable to wait for photon");
		HideLobby();
		bool flag = SteamMatchmaking.SetLobbyData(m_CurrentLobby, "ContentWarningVersion", PlatformsConfig.GetMatchmakingVersion());
		flag = SteamMatchmaking.SetLobbyData(m_CurrentLobby, "MainLanguage", GameHandler.Instance.SettingsHandler.GetSetting<PreferredLanguageMatchmakingSetting>().Value.ToString());
		Debug.Log("Set Lobby Language: " + GameHandler.Instance.SettingsHandler.GetSetting<PreferredLanguageMatchmakingSetting>().Value);
		if (!m_isHostingPrivate)
		{
			flag = flag && SteamMatchmaking.SetLobbyData(m_CurrentLobby, "Plugins", GameHandler.GetPluginHash());
		}
		OnHostedAction?.Invoke(m_CurrentLobby.m_SteamID, m_isHostingPrivate);
	}

	private void OnLobbyEnterCallback(LobbyEnter_t param, bool bioFailure)
	{
		Debug.Log($"Steam Lobby Enter: {param.m_EChatRoomEnterResponse} BioFailure {bioFailure} says steam lobby handler: {m_lobbyHandlerID}");
		if (param.m_EChatRoomEnterResponse == 1)
		{
			Debug.Log("Successfully Joined Server: " + param.m_ulSteamIDLobby);
			m_CurrentLobby = new CSteamID(param.m_ulSteamIDLobby);
			if (UseSteamNetwork)
			{
				Debug.Log("Using SteamNetwork");
				InitThread();
				GetSteamClientsInLobby();
				InitHandshakeWithEveryone();
			}
			if (m_Joined)
			{
				RequestPhotonRoomName();
			}
		}
		else
		{
			string eChatRoomEnterResponse = GetEChatRoomEnterResponse(param.m_EChatRoomEnterResponse);
			if (param.m_EChatRoomEnterResponse != 4)
			{
				Modal.ShowError(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Error_Join), eChatRoomEnterResponse);
				LeaveLobby();
				RetrievableSingleton<ConnectionStateHandler>.Instance.Disconnect();
			}
			else
			{
				Debug.Log("Trying again!");
				m_Joined = false;
				LeaveLobby();
				Singleton<MatchmakingHandler>.Instance.StartMatchmaking();
			}
		}
	}

	private void RequestPhotonRoomName()
	{
		m_WaitingForRoomName = true;
		Debug.Log("Requesting Photon Room Name");
		SendLobbyChatMessage(LobbyChatMsgType.RequestRoomName);
	}

	private void JoinPhotonRoom(string roomName)
	{
		string lobbyData = SteamMatchmaking.GetLobbyData(m_CurrentLobby, "PhotonRegion");
		if (!bool.TryParse(SteamMatchmaking.GetLobbyData(m_CurrentLobby, "PrivateMatch"), out var result))
		{
			result = true;
			Debug.LogError("Failed to parse private match, defaulting to true");
		}
		m_isPrivateMatch = result;
		bool inRoom = PhotonNetwork.InRoom;
		Debug.Log("Joined Room as Non master, photon key is: " + roomName + " Region: " + lobbyData);
		OnJoinedAction?.Invoke(lobbyData, roomName, inRoom);
	}

	private string GetEChatRoomEnterResponse(uint code)
	{
		return code switch
		{
			1u => "Success", 
			2u => "Lobby doesn't exist (probably closed).", 
			3u => "General Denied - You don't have the permissions needed to join the lobby", 
			4u => "Lobby room has reached its maximum size", 
			5u => "Unexpected Error", 
			6u => "You are banned from this lobby and may not join.", 
			7u => "Joining this lobby is not allowed because you are a limited user (no value on account)", 
			8u => "Attempt to join a clan lobby when the clan is locked or disabled.", 
			9u => "Attempt to join a lobby when the user has a community lock on their account.", 
			10u => "Join failed - a user that is in the lobby has blocked you from joining.", 
			11u => "Join failed - you have blocked a user that is already in the lobby.", 
			15u => "Join failed - too many join attempts in a very short period of time.", 
			_ => "Unknown Error", 
		};
	}

	private void InitHandshakeWithEveryone()
	{
		for (int i = 0; i < m_Members.Count; i++)
		{
			SteamNetworkingIdentity identity = m_Members[i];
			Debug.Log("Init Handshake: " + identity.GetSteamID().ToString());
			using NativeArray<byte> buffer = new NativeArray<byte>(new byte[10], Allocator.Temp);
			SendPackage(buffer, ref identity);
		}
	}

	private void SendLobbyChatMessage(LobbyChatMsgType msgType)
	{
		BinarySerializer binarySerializer = new BinarySerializer(256, Allocator.Temp);
		binarySerializer.WriteByte((byte)msgType);
		switch (msgType)
		{
		case LobbyChatMsgType.RequestRoomName:
			if (!m_Joined)
			{
				Debug.LogError("Steam Lobby Master Should Not Request Lobby Name!");
				return;
			}
			break;
		case LobbyChatMsgType.RoomNameReceive:
			if (m_Joined)
			{
				Debug.LogError("Steam Lobby Client Should Not Send Lobby Name!");
				return;
			}
			binarySerializer.WriteString(PhotonNetwork.CurrentRoom.Name, Encoding.Unicode);
			break;
		default:
			Debug.LogError("No Handle for lobbyMsg: " + msgType);
			return;
		}
		byte[] array = binarySerializer.buffer.ToByteArray();
		binarySerializer.Dispose();
		if (!SteamMatchmaking.SendLobbyChatMsg(m_CurrentLobby, array, array.Length))
		{
			Debug.LogError("Failed To Send Steam Lobby Chat Message: " + msgType);
		}
	}

	public void SendPackageToAll(NativeArray<byte> buffer)
	{
		foreach (SteamNetworkingIdentity member in m_Members)
		{
			SteamNetworkingIdentity identity = member;
			SendPackage(buffer, ref identity);
		}
	}

	public unsafe void SendPackage(NativeArray<byte> buffer, ref SteamNetworkingIdentity identity)
	{
		IntPtr pubData = (IntPtr)buffer.GetUnsafeReadOnlyPtr();
		EResult eResult = SteamNetworkingMessages.SendMessageToUser(ref identity, pubData, (uint)buffer.Length, 8, 0);
		Debug.Log($"Sent Message To: {identity.GetSteamID().m_SteamID} Size: " + buffer.Length + " Result: " + eResult);
		if (eResult != EResult.k_EResultOK)
		{
			Debug.LogError("Failed To Send Steam Socket Package: " + eResult);
		}
	}

	public void ReceiveMessage()
	{
		int num = SteamNetworkingMessages.ReceiveMessagesOnChannel(0, receiveBuffers, receiveBuffers.Length);
		for (int i = 0; i < num; i++)
		{
			try
			{
				SteamNetworkingMessage_t steamNetworkingMessage_t = Marshal.PtrToStructure<SteamNetworkingMessage_t>(receiveBuffers[i]);
				byte[] array = new byte[steamNetworkingMessage_t.m_cbSize];
				Marshal.Copy(steamNetworkingMessage_t.m_pData, array, 0, array.Length);
				Debug.Log("Received: " + array.Length + " bytes from: " + steamNetworkingMessage_t.m_identityPeer.GetSteamID().m_SteamID);
				OnMessageReceived(array);
			}
			finally
			{
				Marshal.DestroyStructure<SteamNetworkingMessage_t>(receiveBuffers[i]);
			}
		}
	}

	private void OnMessageReceived(byte[] msg)
	{
		if (msg.Length == 10)
		{
			Debug.Log("Skipping Hello Package");
			return;
		}
		NativeArray<byte> dst = new NativeArray<byte>(msg.Length, Allocator.Temp);
		ByteArrayConvertion.MoveFromByteArray(ref msg, ref dst);
		BinaryDeserializer binaryDeserializer = new BinaryDeserializer(dst);
		SendVideoChunkPackage sendVideoChunkPackage = new SendVideoChunkPackage();
		sendVideoChunkPackage.DeserializeData(binaryDeserializer);
		binaryDeserializer.Dispose();
		RetrievableSingleton<PhotonSendVideoHandler>.Instance.RecieveClipChunk(sendVideoChunkPackage);
	}

	private void OnNetworkingSessionRequest(SteamNetworkingMessagesSessionRequest_t param)
	{
		if (!m_Members.Contains(param.m_identityRemote) || !PhotonNetwork.InRoom)
		{
			Debug.Log("Denying Session Request Since not in room or Requestee not in match");
			return;
		}
		Debug.Log("Got Session Request From: " + param.m_identityRemote.GetSteamID().ToString());
		Debug.Log("Accepted connection " + SteamNetworkingMessages.AcceptSessionWithUser(ref param.m_identityRemote) + " : " + param.m_identityRemote.GetSteamID().ToString());
	}

	private void OnNetworkingSessionRequestFailed(SteamNetworkingMessagesSessionFailed_t param)
	{
		Debug.Log("Session Request Fail: " + param.m_info.m_eState.ToString() + " Identity: " + param.m_info.m_identityRemote.m_eType);
	}

	private void AddSteamClient(CSteamID cSteamID)
	{
		if (cSteamID == SteamUser.GetSteamID())
		{
			Debug.Log("Not Adding Yourself");
			return;
		}
		SteamNetworkingIdentity item = default(SteamNetworkingIdentity);
		item.m_eType = ESteamNetworkingIdentityType.k_ESteamNetworkingIdentityType_SteamID;
		item.SetSteamID(cSteamID);
		if (!m_Members.Contains(item))
		{
			Debug.Log("Adding steam user: " + item.GetSteamID().m_SteamID);
			m_Members.Add(item);
		}
		else
		{
			CSteamID cSteamID2 = cSteamID;
			Debug.LogError("Steam user: " + cSteamID2.ToString() + " Already Added");
		}
	}

	private void RemoveSteamClient(CSteamID cSteamID)
	{
		SteamNetworkingIdentity item = default(SteamNetworkingIdentity);
		item.m_eType = ESteamNetworkingIdentityType.k_ESteamNetworkingIdentityType_SteamID;
		item.SetSteamID(cSteamID);
		if (m_Members.Contains(item))
		{
			Debug.Log("Removing steam user: " + item.GetSteamID().m_SteamID);
			m_Members.Remove(item);
		}
		else
		{
			Debug.LogError("Steam user: " + item.GetSteamID().m_SteamID + " Already Removed");
		}
	}

	private void GetSteamClientsInLobby()
	{
		int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(m_CurrentLobby);
		m_Members = new List<SteamNetworkingIdentity>();
		Debug.Log("Checking Number Of Steam Clients: " + numLobbyMembers);
		for (int i = 0; i < numLobbyMembers; i++)
		{
			CSteamID lobbyMemberByIndex = SteamMatchmaking.GetLobbyMemberByIndex(m_CurrentLobby, i);
			AddSteamClient(lobbyMemberByIndex);
			if (lobbyMemberByIndex != SteamUser.GetSteamID())
			{
				SteamFriends.SetPlayedWith(lobbyMemberByIndex);
			}
		}
	}

	public void SetLobbyName(string roomName)
	{
		if (IsMasterClient)
		{
			SteamMatchmaking.SetLobbyData(m_CurrentLobby, "PhotonRoom", roomName);
			Debug.Log("Setting SteamLobby Name Data: " + roomName);
		}
		else
		{
			Debug.LogError("Non Host is trying to add Lobby Data");
		}
	}

	public void SetLobbyRegion()
	{
		if (IsMasterClient)
		{
			string cloudRegion = PhotonNetwork.CloudRegion;
			SteamMatchmaking.SetLobbyData(m_CurrentLobby, "PhotonRegion", cloudRegion);
			Debug.Log("Setting SteamLobby Region Data: " + cloudRegion);
		}
		else
		{
			Debug.LogError("Non Host is trying to add Lobby Data");
		}
	}

	public void InviteScreen()
	{
		Debug.Log("Opening Steam Invite Screen!");
		SteamFriends.ActivateGameOverlayInviteDialog(m_CurrentLobby);
	}

	public void HideLobby()
	{
		if (IsMasterClient)
		{
			SteamMatchmaking.SetLobbyJoinable(m_CurrentLobby, bLobbyJoinable: false);
		}
	}

	public void OpenLobby()
	{
		if (IsMasterClient)
		{
			if (IsPlayingWithRandoms())
			{
				SteamMatchmaking.SetLobbyType(m_CurrentLobby, ELobbyType.k_ELobbyTypePublic);
			}
			SteamMatchmaking.SetLobbyJoinable(m_CurrentLobby, bLobbyJoinable: true);
		}
	}

	public bool IsPlayingWithRandoms()
	{
		if (IsMasterClient)
		{
			return !m_isHostingPrivate;
		}
		return !m_isPrivateMatch;
	}

	public void SetHostLobbyType()
	{
		if (IsMasterClient)
		{
			SteamMatchmaking.SetLobbyData(m_CurrentLobby, "PrivateMatch", m_isHostingPrivate.ToString());
		}
		else
		{
			Debug.LogError("Non Host is trying to add Lobby Data");
		}
	}

	public void SetLobbyLanguage(LanguageMatchmakingSetting.MatchmakingLanguage value)
	{
		if (IsMasterClient)
		{
			bool flag = SteamMatchmaking.SetLobbyData(m_CurrentLobby, "MainLanguage", value.ToString());
			Debug.Log("Set Lobby Language: " + value.ToString() + $", success: {flag}");
		}
		else
		{
			Debug.LogError("Non Host is trying to add Lobby Data");
		}
	}
}
