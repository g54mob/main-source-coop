using System;
using System.Net;

namespace kcp2k
{
	public class KcpServerConnection : KcpPeer
	{
		public readonly EndPoint remoteEndPoint;

		protected readonly Action<KcpServerConnection> OnConnectedCallback;

		protected readonly Action<ArraySegment<byte>, KcpChannel> OnDataCallback;

		protected readonly Action OnDisconnectedCallback;

		protected readonly Action<ErrorCode, string> OnErrorCallback;

		protected readonly Action<ArraySegment<byte>> RawSendCallback;

		public KcpServerConnection(Action<KcpServerConnection> OnConnected, Action<ArraySegment<byte>, KcpChannel> OnData, Action OnDisconnected, Action<ErrorCode, string> OnError, Action<ArraySegment<byte>> OnRawSend, KcpConfig config, uint cookie, EndPoint remoteEndPoint)
			: base(config, cookie)
		{
			OnConnectedCallback = OnConnected;
			OnDataCallback = OnData;
			OnDisconnectedCallback = OnDisconnected;
			OnErrorCallback = OnError;
			RawSendCallback = OnRawSend;
			this.remoteEndPoint = remoteEndPoint;
		}

		protected override void OnAuthenticated()
		{
			SendHello();
			OnConnectedCallback(this);
		}

		protected override void OnData(ArraySegment<byte> message, KcpChannel channel)
		{
			OnDataCallback(message, channel);
		}

		protected override void OnDisconnected()
		{
			OnDisconnectedCallback();
		}

		protected override void OnError(ErrorCode error, string message)
		{
			OnErrorCallback(error, message);
		}

		protected override void RawSend(ArraySegment<byte> data)
		{
			RawSendCallback(data);
		}

		public void RawInput(ArraySegment<byte> segment)
		{
			if (segment.Count <= 5)
			{
				return;
			}
			byte b = segment.Array[segment.Offset];
			Utils.Decode32U(segment.Array, segment.Offset + 1, out var value);
			if (state == KcpState.Authenticated && value != cookie)
			{
				Log.Info($"[KCP] ServerConnection: dropped message with invalid cookie: {value} from {remoteEndPoint} expected: {cookie} state: {state}. This can happen if the client's Hello message was transmitted multiple times, or if an attacker attempted UDP spoofing.");
				return;
			}
			ArraySegment<byte> message = new ArraySegment<byte>(segment.Array, segment.Offset + 1 + 4, segment.Count - 1 - 4);
			switch (b)
			{
			case 1:
				OnRawInputReliable(message);
				break;
			case 2:
				OnRawInputUnreliable(message);
				break;
			default:
				Log.Warning($"[KCP] ServerConnection: invalid channel header: {b} from {remoteEndPoint}, likely internet noise");
				break;
			}
		}
	}
}
