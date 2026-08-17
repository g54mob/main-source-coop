using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.Auth;
using Epic.OnlineServices.Connect;
using Epic.OnlineServices.Lobby;
using Epic.OnlineServices.Logging;
using Epic.OnlineServices.Metrics;
using Epic.OnlineServices.P2P;
using Epic.OnlineServices.Platform;
using Epic.OnlineServices.Presence;
using Epic.OnlineServices.RTC;
using Epic.OnlineServices.RTCAudio;
using Epic.OnlineServices.UI;

namespace Epic.OnlineServices
{
	public static class Bindings
	{
		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern ulong EOS_Auth_AddNotifyLoginStatusChanged(IntPtr handle, ref Epic.OnlineServices.Auth.AddNotifyLoginStatusChangedOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Auth.OnLoginStatusChangedCallbackInternal notification);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_Auth_CopyIdToken(IntPtr handle, ref CopyIdTokenOptionsInternal options, out IntPtr outIdToken);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_Auth_CopyUserAuthToken(IntPtr handle, ref CopyUserAuthTokenOptionsInternal options, IntPtr localUserId, out IntPtr outUserAuthToken);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Auth_DeletePersistentAuth(IntPtr handle, ref DeletePersistentAuthOptionsInternal options, IntPtr clientData, OnDeletePersistentAuthCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Auth_IdToken_Release(IntPtr idToken);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Auth_LinkAccount(IntPtr handle, ref Epic.OnlineServices.Auth.LinkAccountOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Auth.OnLinkAccountCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Auth_Login(IntPtr handle, ref Epic.OnlineServices.Auth.LoginOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Auth.OnLoginCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Auth_Logout(IntPtr handle, ref LogoutOptionsInternal options, IntPtr clientData, OnLogoutCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Auth_RemoveNotifyLoginStatusChanged(IntPtr handle, ulong inId);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Auth_Token_Release(IntPtr authToken);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern ulong EOS_Connect_AddNotifyAuthExpiration(IntPtr handle, ref AddNotifyAuthExpirationOptionsInternal options, IntPtr clientData, OnAuthExpirationCallbackInternal notification);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern ulong EOS_Connect_AddNotifyLoginStatusChanged(IntPtr handle, ref Epic.OnlineServices.Connect.AddNotifyLoginStatusChangedOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Connect.OnLoginStatusChangedCallbackInternal notification);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Connect_CreateDeviceId(IntPtr handle, ref CreateDeviceIdOptionsInternal options, IntPtr clientData, OnCreateDeviceIdCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Connect_CreateUser(IntPtr handle, ref CreateUserOptionsInternal options, IntPtr clientData, OnCreateUserCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Connect_LinkAccount(IntPtr handle, ref Epic.OnlineServices.Connect.LinkAccountOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Connect.OnLinkAccountCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Connect_Login(IntPtr handle, ref Epic.OnlineServices.Connect.LoginOptionsInternal options, IntPtr clientData, Epic.OnlineServices.Connect.OnLoginCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Connect_RemoveNotifyAuthExpiration(IntPtr handle, ulong inId);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Connect_RemoveNotifyLoginStatusChanged(IntPtr handle, ulong inId);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Connect_TransferDeviceIdAccount(IntPtr handle, ref TransferDeviceIdAccountOptionsInternal options, IntPtr clientData, OnTransferDeviceIdAccountCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_ContinuanceToken_ToString(IntPtr continuanceToken, IntPtr outBuffer, ref int inOutBufferLength);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int EOS_EResult_IsOperationComplete(Result result);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_EpicAccountId_ToString(IntPtr accountId, IntPtr outBuffer, ref int inOutBufferLength);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_Initialize(ref InitializeOptionsInternal options);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_LobbyDetails_CopyAttributeByKey(IntPtr handle, ref LobbyDetailsCopyAttributeByKeyOptionsInternal options, out IntPtr outAttribute);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_LobbyDetails_CopyInfo(IntPtr handle, ref LobbyDetailsCopyInfoOptionsInternal options, out IntPtr outLobbyDetailsInfo);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_LobbyDetails_CopyMemberAttributeByKey(IntPtr handle, ref LobbyDetailsCopyMemberAttributeByKeyOptionsInternal options, out IntPtr outAttribute);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr EOS_LobbyDetails_GetMemberByIndex(IntPtr handle, ref LobbyDetailsGetMemberByIndexOptionsInternal options);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern uint EOS_LobbyDetails_GetMemberCount(IntPtr handle, ref LobbyDetailsGetMemberCountOptionsInternal options);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_LobbyDetails_Info_Release(IntPtr lobbyDetailsInfo);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_LobbyDetails_Release(IntPtr lobbyHandle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_LobbyModification_AddAttribute(IntPtr handle, ref LobbyModificationAddAttributeOptionsInternal options);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_LobbyModification_AddMemberAttribute(IntPtr handle, ref LobbyModificationAddMemberAttributeOptionsInternal options);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_LobbyModification_Release(IntPtr lobbyModificationHandle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_LobbySearch_CopySearchResultByIndex(IntPtr handle, ref LobbySearchCopySearchResultByIndexOptionsInternal options, out IntPtr outLobbyDetailsHandle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_LobbySearch_Find(IntPtr handle, ref LobbySearchFindOptionsInternal options, IntPtr clientData, LobbySearchOnFindCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern uint EOS_LobbySearch_GetSearchResultCount(IntPtr handle, ref LobbySearchGetSearchResultCountOptionsInternal options);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_LobbySearch_Release(IntPtr lobbySearchHandle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_LobbySearch_SetParameter(IntPtr handle, ref LobbySearchSetParameterOptionsInternal options);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern ulong EOS_Lobby_AddNotifyJoinLobbyAccepted(IntPtr handle, ref AddNotifyJoinLobbyAcceptedOptionsInternal options, IntPtr clientData, OnJoinLobbyAcceptedCallbackInternal notificationFn);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern ulong EOS_Lobby_AddNotifyLobbyInviteAccepted(IntPtr handle, ref AddNotifyLobbyInviteAcceptedOptionsInternal options, IntPtr clientData, OnLobbyInviteAcceptedCallbackInternal notificationFn);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern ulong EOS_Lobby_AddNotifyLobbyInviteReceived(IntPtr handle, ref AddNotifyLobbyInviteReceivedOptionsInternal options, IntPtr clientData, OnLobbyInviteReceivedCallbackInternal notificationFn);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern ulong EOS_Lobby_AddNotifyLobbyMemberStatusReceived(IntPtr handle, ref AddNotifyLobbyMemberStatusReceivedOptionsInternal options, IntPtr clientData, OnLobbyMemberStatusReceivedCallbackInternal notificationFn);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern ulong EOS_Lobby_AddNotifyLobbyUpdateReceived(IntPtr handle, ref AddNotifyLobbyUpdateReceivedOptionsInternal options, IntPtr clientData, OnLobbyUpdateReceivedCallbackInternal notificationFn);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern ulong EOS_Lobby_AddNotifyRTCRoomConnectionChanged(IntPtr handle, ref AddNotifyRTCRoomConnectionChangedOptionsInternal options, IntPtr clientData, OnRTCRoomConnectionChangedCallbackInternal notificationFn);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Lobby_Attribute_Release(IntPtr lobbyAttribute);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_Lobby_CopyLobbyDetailsHandle(IntPtr handle, ref CopyLobbyDetailsHandleOptionsInternal options, out IntPtr outLobbyDetailsHandle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Lobby_CreateLobby(IntPtr handle, ref CreateLobbyOptionsInternal options, IntPtr clientData, OnCreateLobbyCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_Lobby_CreateLobbySearch(IntPtr handle, ref CreateLobbySearchOptionsInternal options, out IntPtr outLobbySearchHandle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Lobby_DestroyLobby(IntPtr handle, ref DestroyLobbyOptionsInternal options, IntPtr clientData, OnDestroyLobbyCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_Lobby_GetRTCRoomName(IntPtr handle, ref GetRTCRoomNameOptionsInternal options, IntPtr outBuffer, ref uint inOutBufferLength);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_Lobby_IsRTCRoomConnected(IntPtr handle, ref IsRTCRoomConnectedOptionsInternal options, out int outIsConnected);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Lobby_JoinLobbyById(IntPtr handle, ref JoinLobbyByIdOptionsInternal options, IntPtr clientData, OnJoinLobbyByIdCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Lobby_KickMember(IntPtr handle, ref KickMemberOptionsInternal options, IntPtr clientData, OnKickMemberCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Lobby_LeaveLobby(IntPtr handle, ref LeaveLobbyOptionsInternal options, IntPtr clientData, OnLeaveLobbyCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Lobby_RemoveNotifyJoinLobbyAccepted(IntPtr handle, ulong inId);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Lobby_RemoveNotifyLobbyInviteAccepted(IntPtr handle, ulong inId);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Lobby_RemoveNotifyLobbyInviteReceived(IntPtr handle, ulong inId);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Lobby_RemoveNotifyLobbyMemberStatusReceived(IntPtr handle, ulong inId);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Lobby_RemoveNotifyLobbyUpdateReceived(IntPtr handle, ulong inId);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Lobby_RemoveNotifyRTCRoomConnectionChanged(IntPtr handle, ulong inId);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Lobby_UpdateLobby(IntPtr handle, ref UpdateLobbyOptionsInternal options, IntPtr clientData, OnUpdateLobbyCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_Lobby_UpdateLobbyModification(IntPtr handle, ref UpdateLobbyModificationOptionsInternal options, out IntPtr outLobbyModificationHandle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_Logging_SetCallback(LogMessageFuncInternal callback);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_Logging_SetLogLevel(LogCategory logCategory, LogLevel logLevel);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_Metrics_BeginPlayerSession(IntPtr handle, ref BeginPlayerSessionOptionsInternal options);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_Metrics_EndPlayerSession(IntPtr handle, ref EndPlayerSessionOptionsInternal options);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_P2P_AcceptConnection(IntPtr handle, ref AcceptConnectionOptionsInternal options);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern ulong EOS_P2P_AddNotifyPeerConnectionClosed(IntPtr handle, ref AddNotifyPeerConnectionClosedOptionsInternal options, IntPtr clientData, OnRemoteConnectionClosedCallbackInternal connectionClosedHandler);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern ulong EOS_P2P_AddNotifyPeerConnectionRequest(IntPtr handle, ref AddNotifyPeerConnectionRequestOptionsInternal options, IntPtr clientData, OnIncomingConnectionRequestCallbackInternal connectionRequestHandler);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_P2P_CloseConnection(IntPtr handle, ref CloseConnectionOptionsInternal options);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_P2P_CloseConnections(IntPtr handle, ref CloseConnectionsOptionsInternal options);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_P2P_ReceivePacket(IntPtr handle, ref ReceivePacketOptionsInternal options, out IntPtr outPeerId, IntPtr outSocketId, out byte outChannel, IntPtr outData, out uint outBytesWritten);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_P2P_RemoveNotifyPeerConnectionClosed(IntPtr handle, ulong notificationId);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_P2P_RemoveNotifyPeerConnectionRequest(IntPtr handle, ulong notificationId);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_P2P_SendPacket(IntPtr handle, ref SendPacketOptionsInternal options);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_P2P_SetRelayControl(IntPtr handle, ref SetRelayControlOptionsInternal options);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr EOS_Platform_GetAchievementsInterface(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern ApplicationStatus EOS_Platform_GetApplicationStatus(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr EOS_Platform_GetAuthInterface(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr EOS_Platform_GetConnectInterface(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr EOS_Platform_GetEcomInterface(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr EOS_Platform_GetFriendsInterface(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr EOS_Platform_GetLeaderboardsInterface(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr EOS_Platform_GetLobbyInterface(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr EOS_Platform_GetMetricsInterface(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr EOS_Platform_GetModsInterface(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr EOS_Platform_GetP2PInterface(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr EOS_Platform_GetPlayerDataStorageInterface(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr EOS_Platform_GetPresenceInterface(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr EOS_Platform_GetRTCInterface(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr EOS_Platform_GetSessionsInterface(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr EOS_Platform_GetStatsInterface(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr EOS_Platform_GetTitleStorageInterface(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr EOS_Platform_GetUIInterface(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr EOS_Platform_GetUserInfoInterface(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Platform_Release(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_Platform_SetApplicationStatus(IntPtr handle, ApplicationStatus newStatus);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Platform_Tick(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_PlayerDataStorageFileTransferRequest_CancelRequest(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_PlayerDataStorageFileTransferRequest_Release(IntPtr playerDataStorageFileTransferHandle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_PresenceModification_SetRawRichText(IntPtr handle, ref PresenceModificationSetRawRichTextOptionsInternal options);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_PresenceModification_SetStatus(IntPtr handle, ref PresenceModificationSetStatusOptionsInternal options);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_Presence_CreatePresenceModification(IntPtr handle, ref CreatePresenceModificationOptionsInternal options, out IntPtr outPresenceModificationHandle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_Presence_SetPresence(IntPtr handle, ref SetPresenceOptionsInternal options, IntPtr clientData, SetPresenceCompleteCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr EOS_ProductUserId_FromString(IntPtr productUserIdString);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_ProductUserId_ToString(IntPtr accountId, IntPtr outBuffer, ref int inOutBufferLength);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern ulong EOS_RTCAudio_AddNotifyAudioDevicesChanged(IntPtr handle, ref AddNotifyAudioDevicesChangedOptionsInternal options, IntPtr clientData, OnAudioDevicesChangedCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern ulong EOS_RTCAudio_AddNotifyParticipantUpdated(IntPtr handle, ref AddNotifyParticipantUpdatedOptionsInternal options, IntPtr clientData, OnParticipantUpdatedCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_RTCAudio_CopyInputDeviceInformationByIndex(IntPtr handle, ref CopyInputDeviceInformationByIndexOptionsInternal options, out IntPtr outInputDeviceInformation);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_RTCAudio_CopyOutputDeviceInformationByIndex(IntPtr handle, ref CopyOutputDeviceInformationByIndexOptionsInternal options, out IntPtr outOutputDeviceInformation);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern uint EOS_RTCAudio_GetInputDevicesCount(IntPtr handle, ref GetInputDevicesCountOptionsInternal options);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern uint EOS_RTCAudio_GetOutputDevicesCount(IntPtr handle, ref GetOutputDevicesCountOptionsInternal options);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_RTCAudio_InputDeviceInformation_Release(IntPtr deviceInformation);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_RTCAudio_OutputDeviceInformation_Release(IntPtr deviceInformation);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_RTCAudio_QueryInputDevicesInformation(IntPtr handle, ref QueryInputDevicesInformationOptionsInternal options, IntPtr clientData, OnQueryInputDevicesInformationCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_RTCAudio_QueryOutputDevicesInformation(IntPtr handle, ref QueryOutputDevicesInformationOptionsInternal options, IntPtr clientData, OnQueryOutputDevicesInformationCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_RTCAudio_RemoveNotifyAudioDevicesChanged(IntPtr handle, ulong notificationId);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_RTCAudio_RemoveNotifyParticipantUpdated(IntPtr handle, ulong notificationId);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_RTCAudio_SetInputDeviceSettings(IntPtr handle, ref SetInputDeviceSettingsOptionsInternal options, IntPtr clientData, OnSetInputDeviceSettingsCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_RTCAudio_SetOutputDeviceSettings(IntPtr handle, ref SetOutputDeviceSettingsOptionsInternal options, IntPtr clientData, OnSetOutputDeviceSettingsCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_RTCAudio_UpdateReceiving(IntPtr handle, ref UpdateReceivingOptionsInternal options, IntPtr clientData, OnUpdateReceivingCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_RTCAudio_UpdateSending(IntPtr handle, ref UpdateSendingOptionsInternal options, IntPtr clientData, OnUpdateSendingCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern ulong EOS_RTC_AddNotifyParticipantStatusChanged(IntPtr handle, ref AddNotifyParticipantStatusChangedOptionsInternal options, IntPtr clientData, OnParticipantStatusChangedCallbackInternal completionDelegate);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr EOS_RTC_GetAudioInterface(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_RTC_RemoveNotifyParticipantStatusChanged(IntPtr handle, ulong notificationId);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_Shutdown();

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_TitleStorageFileTransferRequest_CancelRequest(IntPtr handle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void EOS_TitleStorageFileTransferRequest_Release(IntPtr titleStorageFileTransferHandle);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern ulong EOS_UI_AddNotifyDisplaySettingsUpdated(IntPtr handle, ref AddNotifyDisplaySettingsUpdatedOptionsInternal options, IntPtr clientData, OnDisplaySettingsUpdatedCallbackInternal notificationFn);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_UI_SetDisplayPreference(IntPtr handle, ref SetDisplayPreferenceOptionsInternal options);

		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl)]
		internal static extern Result EOS_UI_SetToggleFriendsButton(IntPtr handle, ref SetToggleFriendsButtonOptionsInternal options);
	}
}
