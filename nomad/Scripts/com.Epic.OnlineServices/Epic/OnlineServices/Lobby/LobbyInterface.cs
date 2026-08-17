using System;

namespace Epic.OnlineServices.Lobby
{
	public sealed class LobbyInterface : Handle
	{
		public static readonly Utf8String SEARCH_BUCKET_ID = "bucket";

		public static readonly Utf8String SEARCH_MINCURRENTMEMBERS = "mincurrentmembers";

		public static readonly Utf8String SEARCH_MINSLOTSAVAILABLE = "minslotsavailable";

		public ulong AddNotifyJoinLobbyAccepted(ref AddNotifyJoinLobbyAcceptedOptions options, object clientData, OnJoinLobbyAcceptedCallback notificationFn)
		{
			if (notificationFn == null)
			{
				throw new ArgumentNullException("notificationFn");
			}
			AddNotifyJoinLobbyAcceptedOptionsInternal options2 = default(AddNotifyJoinLobbyAcceptedOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, notificationFn);
			ulong num = Bindings.EOS_Lobby_AddNotifyJoinLobbyAccepted(base.InnerHandle, ref options2, clientDataPointer, OnJoinLobbyAcceptedCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
			Helper.AssignNotificationIdToCallback(clientDataPointer, num);
			return num;
		}

		public ulong AddNotifyLobbyInviteAccepted(ref AddNotifyLobbyInviteAcceptedOptions options, object clientData, OnLobbyInviteAcceptedCallback notificationFn)
		{
			if (notificationFn == null)
			{
				throw new ArgumentNullException("notificationFn");
			}
			AddNotifyLobbyInviteAcceptedOptionsInternal options2 = default(AddNotifyLobbyInviteAcceptedOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, notificationFn);
			ulong num = Bindings.EOS_Lobby_AddNotifyLobbyInviteAccepted(base.InnerHandle, ref options2, clientDataPointer, OnLobbyInviteAcceptedCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
			Helper.AssignNotificationIdToCallback(clientDataPointer, num);
			return num;
		}

		public ulong AddNotifyLobbyInviteReceived(ref AddNotifyLobbyInviteReceivedOptions options, object clientData, OnLobbyInviteReceivedCallback notificationFn)
		{
			if (notificationFn == null)
			{
				throw new ArgumentNullException("notificationFn");
			}
			AddNotifyLobbyInviteReceivedOptionsInternal options2 = default(AddNotifyLobbyInviteReceivedOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, notificationFn);
			ulong num = Bindings.EOS_Lobby_AddNotifyLobbyInviteReceived(base.InnerHandle, ref options2, clientDataPointer, OnLobbyInviteReceivedCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
			Helper.AssignNotificationIdToCallback(clientDataPointer, num);
			return num;
		}

		public ulong AddNotifyLobbyMemberStatusReceived(ref AddNotifyLobbyMemberStatusReceivedOptions options, object clientData, OnLobbyMemberStatusReceivedCallback notificationFn)
		{
			if (notificationFn == null)
			{
				throw new ArgumentNullException("notificationFn");
			}
			AddNotifyLobbyMemberStatusReceivedOptionsInternal options2 = default(AddNotifyLobbyMemberStatusReceivedOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, notificationFn);
			ulong num = Bindings.EOS_Lobby_AddNotifyLobbyMemberStatusReceived(base.InnerHandle, ref options2, clientDataPointer, OnLobbyMemberStatusReceivedCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
			Helper.AssignNotificationIdToCallback(clientDataPointer, num);
			return num;
		}

		public ulong AddNotifyLobbyUpdateReceived(ref AddNotifyLobbyUpdateReceivedOptions options, object clientData, OnLobbyUpdateReceivedCallback notificationFn)
		{
			if (notificationFn == null)
			{
				throw new ArgumentNullException("notificationFn");
			}
			AddNotifyLobbyUpdateReceivedOptionsInternal options2 = default(AddNotifyLobbyUpdateReceivedOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, notificationFn);
			ulong num = Bindings.EOS_Lobby_AddNotifyLobbyUpdateReceived(base.InnerHandle, ref options2, clientDataPointer, OnLobbyUpdateReceivedCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
			Helper.AssignNotificationIdToCallback(clientDataPointer, num);
			return num;
		}

		public ulong AddNotifyRTCRoomConnectionChanged(ref AddNotifyRTCRoomConnectionChangedOptions options, object clientData, OnRTCRoomConnectionChangedCallback notificationFn)
		{
			if (notificationFn == null)
			{
				throw new ArgumentNullException("notificationFn");
			}
			AddNotifyRTCRoomConnectionChangedOptionsInternal options2 = default(AddNotifyRTCRoomConnectionChangedOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, notificationFn);
			ulong num = Bindings.EOS_Lobby_AddNotifyRTCRoomConnectionChanged(base.InnerHandle, ref options2, clientDataPointer, OnRTCRoomConnectionChangedCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
			Helper.AssignNotificationIdToCallback(clientDataPointer, num);
			return num;
		}

		public Result CopyLobbyDetailsHandle(ref CopyLobbyDetailsHandleOptions options, out LobbyDetails outLobbyDetailsHandle)
		{
			CopyLobbyDetailsHandleOptionsInternal options2 = default(CopyLobbyDetailsHandleOptionsInternal);
			options2.Set(ref options);
			IntPtr outLobbyDetailsHandle2 = IntPtr.Zero;
			Result result = Bindings.EOS_Lobby_CopyLobbyDetailsHandle(base.InnerHandle, ref options2, out outLobbyDetailsHandle2);
			Helper.Dispose(ref options2);
			Helper.Get(outLobbyDetailsHandle2, out outLobbyDetailsHandle);
			return result;
		}

		public void CreateLobby(ref CreateLobbyOptions options, object clientData, OnCreateLobbyCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			CreateLobbyOptionsInternal options2 = default(CreateLobbyOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			Bindings.EOS_Lobby_CreateLobby(base.InnerHandle, ref options2, clientDataPointer, OnCreateLobbyCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
		}

		public Result CreateLobbySearch(ref CreateLobbySearchOptions options, out LobbySearch outLobbySearchHandle)
		{
			CreateLobbySearchOptionsInternal options2 = default(CreateLobbySearchOptionsInternal);
			options2.Set(ref options);
			IntPtr outLobbySearchHandle2 = IntPtr.Zero;
			Result result = Bindings.EOS_Lobby_CreateLobbySearch(base.InnerHandle, ref options2, out outLobbySearchHandle2);
			Helper.Dispose(ref options2);
			Helper.Get(outLobbySearchHandle2, out outLobbySearchHandle);
			return result;
		}

		public void DestroyLobby(ref DestroyLobbyOptions options, object clientData, OnDestroyLobbyCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			DestroyLobbyOptionsInternal options2 = default(DestroyLobbyOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			Bindings.EOS_Lobby_DestroyLobby(base.InnerHandle, ref options2, clientDataPointer, OnDestroyLobbyCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
		}

		public Result GetRTCRoomName(ref GetRTCRoomNameOptions options, out Utf8String outBuffer)
		{
			GetRTCRoomNameOptionsInternal options2 = default(GetRTCRoomNameOptionsInternal);
			options2.Set(ref options);
			uint inOutBufferLength = 256u;
			IntPtr value = Helper.AddAllocation(inOutBufferLength);
			Result result = Bindings.EOS_Lobby_GetRTCRoomName(base.InnerHandle, ref options2, value, ref inOutBufferLength);
			Helper.Dispose(ref options2);
			Helper.Get(value, out outBuffer);
			Helper.Dispose(ref value);
			return result;
		}

		public Result IsRTCRoomConnected(ref IsRTCRoomConnectedOptions options, out bool outIsConnected)
		{
			IsRTCRoomConnectedOptionsInternal options2 = default(IsRTCRoomConnectedOptionsInternal);
			options2.Set(ref options);
			int outIsConnected2 = 0;
			Result result = Bindings.EOS_Lobby_IsRTCRoomConnected(base.InnerHandle, ref options2, out outIsConnected2);
			Helper.Dispose(ref options2);
			Helper.Get(outIsConnected2, out outIsConnected);
			return result;
		}

		public void JoinLobbyById(ref JoinLobbyByIdOptions options, object clientData, OnJoinLobbyByIdCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			JoinLobbyByIdOptionsInternal options2 = default(JoinLobbyByIdOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			Bindings.EOS_Lobby_JoinLobbyById(base.InnerHandle, ref options2, clientDataPointer, OnJoinLobbyByIdCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
		}

		public void KickMember(ref KickMemberOptions options, object clientData, OnKickMemberCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			KickMemberOptionsInternal options2 = default(KickMemberOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			Bindings.EOS_Lobby_KickMember(base.InnerHandle, ref options2, clientDataPointer, OnKickMemberCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
		}

		public void LeaveLobby(ref LeaveLobbyOptions options, object clientData, OnLeaveLobbyCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			LeaveLobbyOptionsInternal options2 = default(LeaveLobbyOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			Bindings.EOS_Lobby_LeaveLobby(base.InnerHandle, ref options2, clientDataPointer, OnLeaveLobbyCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
		}

		public void RemoveNotifyJoinLobbyAccepted(ulong inId)
		{
			Bindings.EOS_Lobby_RemoveNotifyJoinLobbyAccepted(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		public void RemoveNotifyLobbyInviteAccepted(ulong inId)
		{
			Bindings.EOS_Lobby_RemoveNotifyLobbyInviteAccepted(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		public void RemoveNotifyLobbyInviteReceived(ulong inId)
		{
			Bindings.EOS_Lobby_RemoveNotifyLobbyInviteReceived(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		public void RemoveNotifyLobbyMemberStatusReceived(ulong inId)
		{
			Bindings.EOS_Lobby_RemoveNotifyLobbyMemberStatusReceived(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		public void RemoveNotifyLobbyUpdateReceived(ulong inId)
		{
			Bindings.EOS_Lobby_RemoveNotifyLobbyUpdateReceived(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		public void RemoveNotifyRTCRoomConnectionChanged(ulong inId)
		{
			Bindings.EOS_Lobby_RemoveNotifyRTCRoomConnectionChanged(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		public void UpdateLobby(ref UpdateLobbyOptions options, object clientData, OnUpdateLobbyCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			UpdateLobbyOptionsInternal options2 = default(UpdateLobbyOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			Bindings.EOS_Lobby_UpdateLobby(base.InnerHandle, ref options2, clientDataPointer, OnUpdateLobbyCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
		}

		public Result UpdateLobbyModification(ref UpdateLobbyModificationOptions options, out LobbyModification outLobbyModificationHandle)
		{
			UpdateLobbyModificationOptionsInternal options2 = default(UpdateLobbyModificationOptionsInternal);
			options2.Set(ref options);
			IntPtr outLobbyModificationHandle2 = IntPtr.Zero;
			Result result = Bindings.EOS_Lobby_UpdateLobbyModification(base.InnerHandle, ref options2, out outLobbyModificationHandle2);
			Helper.Dispose(ref options2);
			Helper.Get(outLobbyModificationHandle2, out outLobbyModificationHandle);
			return result;
		}
	}
}
