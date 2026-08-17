using System;

namespace Epic.OnlineServices.P2P
{
	public sealed class P2PInterface : Handle
	{
		public Result ReceivePacket(ref ReceivePacketOptions options, ref ProductUserId outPeerId, ref SocketId outSocketId, out byte outChannel, ArraySegment<byte> outData, out uint outBytesWritten)
		{
			bool wasCacheValid = outSocketId.PrepareForUpdate();
			IntPtr value = Helper.AddPinnedBuffer(outSocketId.m_AllBytes);
			IntPtr value2 = Helper.AddPinnedBuffer(outData);
			ReceivePacketOptionsInternal options2 = new ReceivePacketOptionsInternal(ref options);
			try
			{
				IntPtr outPeerId2 = IntPtr.Zero;
				outChannel = 0;
				outBytesWritten = 0u;
				Result result = Bindings.EOS_P2P_ReceivePacket(base.InnerHandle, ref options2, out outPeerId2, value, out outChannel, value2, out outBytesWritten);
				if (outPeerId == null)
				{
					Helper.Get(outPeerId2, out outPeerId);
				}
				else if (outPeerId.InnerHandle != outPeerId2)
				{
					outPeerId.InnerHandle = outPeerId2;
				}
				outSocketId.CheckIfChanged(wasCacheValid);
				return result;
			}
			finally
			{
				Helper.Dispose(ref value);
				Helper.Dispose(ref value2);
				options2.Dispose();
			}
		}

		public Result AcceptConnection(ref AcceptConnectionOptions options)
		{
			AcceptConnectionOptionsInternal options2 = default(AcceptConnectionOptionsInternal);
			options2.Set(ref options);
			Result result = Bindings.EOS_P2P_AcceptConnection(base.InnerHandle, ref options2);
			Helper.Dispose(ref options2);
			return result;
		}

		public ulong AddNotifyPeerConnectionClosed(ref AddNotifyPeerConnectionClosedOptions options, object clientData, OnRemoteConnectionClosedCallback connectionClosedHandler)
		{
			if (connectionClosedHandler == null)
			{
				throw new ArgumentNullException("connectionClosedHandler");
			}
			AddNotifyPeerConnectionClosedOptionsInternal options2 = default(AddNotifyPeerConnectionClosedOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, connectionClosedHandler);
			ulong num = Bindings.EOS_P2P_AddNotifyPeerConnectionClosed(base.InnerHandle, ref options2, clientDataPointer, OnRemoteConnectionClosedCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
			Helper.AssignNotificationIdToCallback(clientDataPointer, num);
			return num;
		}

		public ulong AddNotifyPeerConnectionRequest(ref AddNotifyPeerConnectionRequestOptions options, object clientData, OnIncomingConnectionRequestCallback connectionRequestHandler)
		{
			if (connectionRequestHandler == null)
			{
				throw new ArgumentNullException("connectionRequestHandler");
			}
			AddNotifyPeerConnectionRequestOptionsInternal options2 = default(AddNotifyPeerConnectionRequestOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, connectionRequestHandler);
			ulong num = Bindings.EOS_P2P_AddNotifyPeerConnectionRequest(base.InnerHandle, ref options2, clientDataPointer, OnIncomingConnectionRequestCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
			Helper.AssignNotificationIdToCallback(clientDataPointer, num);
			return num;
		}

		public Result CloseConnection(ref CloseConnectionOptions options)
		{
			CloseConnectionOptionsInternal options2 = default(CloseConnectionOptionsInternal);
			options2.Set(ref options);
			Result result = Bindings.EOS_P2P_CloseConnection(base.InnerHandle, ref options2);
			Helper.Dispose(ref options2);
			return result;
		}

		public Result CloseConnections(ref CloseConnectionsOptions options)
		{
			CloseConnectionsOptionsInternal options2 = default(CloseConnectionsOptionsInternal);
			options2.Set(ref options);
			Result result = Bindings.EOS_P2P_CloseConnections(base.InnerHandle, ref options2);
			Helper.Dispose(ref options2);
			return result;
		}

		public void RemoveNotifyPeerConnectionClosed(ulong notificationId)
		{
			Bindings.EOS_P2P_RemoveNotifyPeerConnectionClosed(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		public void RemoveNotifyPeerConnectionRequest(ulong notificationId)
		{
			Bindings.EOS_P2P_RemoveNotifyPeerConnectionRequest(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		public Result SendPacket(ref SendPacketOptions options)
		{
			SendPacketOptionsInternal options2 = default(SendPacketOptionsInternal);
			options2.Set(ref options);
			Result result = Bindings.EOS_P2P_SendPacket(base.InnerHandle, ref options2);
			Helper.Dispose(ref options2);
			return result;
		}

		public Result SetRelayControl(ref SetRelayControlOptions options)
		{
			SetRelayControlOptionsInternal options2 = default(SetRelayControlOptionsInternal);
			options2.Set(ref options);
			Result result = Bindings.EOS_P2P_SetRelayControl(base.InnerHandle, ref options2);
			Helper.Dispose(ref options2);
			return result;
		}
	}
}
