using System;
using System.Collections.Generic;

namespace Fusion.Sockets
{
	public interface INetPeerGroupCallbacks
	{
		List<IntPtr> DeliverList { get; }

		ReadBuffer ReadBuffer { get; }

		unsafe void OnConnected(NetConnection* connection);

		unsafe void OnDisconnected(NetConnection* connection, NetDisconnectReason reason);

		unsafe void OnUnreliableData(NetConnection* connection, NetBitBuffer* buffer);

		unsafe void OnUnconnectedData(NetBitBuffer* buffer);

		unsafe bool OnNotifyData(NetConnection* connection, ReadBuffer buffer);

		unsafe void OnNotifyDataLost(NetConnection* connection, in NetNotifyDataInfo info);

		unsafe void OnNotifyDataDelivered(NetConnection* connection, in NetNotifyDataInfo info);

		unsafe void OnReliableData(NetConnection* connection, ReadOnlySpan<byte> data);

		unsafe void OnReliableDataProgress(NetConnection* connection, ReadOnlySpan<byte> data, int fragmentNum, int fragmentCount);

		OnConnectionRequestReply OnConnectionRequest(NetAddress remoteAddress, byte[] token, byte[] uniqueId);

		void OnConnectionFailed(NetAddress address, NetConnectFailedReason reason);

		unsafe void OnConnectionAttempt(NetConnection* connection, int attempts, int totalConnectAttempts);

		void OnRejoinRequest(string sessionId);
	}
}
