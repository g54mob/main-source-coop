using Fusion.Sockets.V2;

namespace Fusion.Sockets
{
	public struct NetConnection
	{
		internal struct StateConnectingData
		{
			public int Attempts;

			public double AttemptTimeout;
		}

		internal struct StateShutdownData
		{
			public double Timeout;

			public int Unmapped;
		}

		internal struct StateDisconnectedData
		{
			public NetDisconnectReason Reason;

			public int CallbackInvoked;

			public int SentDisconnectCommand;
		}

		internal const byte UNIQUE_ID_SIZE = 8;

		internal ulong MapHash;

		internal unsafe NetConnection* MapNext;

		internal NetConnectionMap.EntryState MapState;

		internal NetConnectionId LocalId;

		internal NetConnectionId RemoteId;

		internal NetAddress Address;

		internal NetConnectionStatus Status;

		internal double Rtt;

		internal double SendTime;

		internal double RecvTime;

		internal StateConnectingData StateConnecting;

		internal StateDisconnectedData StateDisconnected;

		internal StateShutdownData StateShutdown;

		internal unsafe byte* ConnectionToken;

		internal int ConnectionTokenLength;

		internal unsafe byte* DisconnectToken;

		internal int DisconnectTokenLength;

		internal long UniqueIdHash;

		internal unsafe byte* UniqueId;

		internal uint Counter;

		internal unsafe Connection* V2;

		public readonly bool Active => MapState == NetConnectionMap.EntryState.Used;

		public readonly double RoundTripTime => Rtt;

		public readonly NetAddress RemoteAddress => Address;

		public readonly NetConnectionStatus ConnectionStatus => Status;

		public readonly NetConnectionId LocalConnectionId => LocalId;

		public readonly NetConnectionId RemoteConnectionId => RemoteId;

		internal unsafe static void Initialize(NetConnection* c, short group, short index, NetConfig* config)
		{
			c->LocalId.Group = group;
			c->LocalId.GroupIndex = index;
			Reset(c);
		}

		internal unsafe static void SetRtt(NetConnection* c, double rtt = 0.0)
		{
			c->Rtt = rtt;
		}

		internal unsafe static void Reset(NetConnection* c)
		{
			Connection.Free(ref c->V2);
			c->V2 = Connection.Alloc();
			c->LocalId.Generation++;
			if (c->LocalId.Generation == 0)
			{
				c->LocalId.Generation++;
			}
			FusionUnsafe.Free(ref c->ConnectionToken);
			c->ConnectionTokenLength = 0;
			FusionUnsafe.Free(ref c->DisconnectToken);
			c->DisconnectTokenLength = 0;
			FusionUnsafe.Free(ref c->UniqueId);
			c->UniqueIdHash = 0L;
			c->Address = default(NetAddress);
			c->Status = (NetConnectionStatus)0;
			c->StateShutdown = default(StateShutdownData);
			c->StateConnecting = default(StateConnectingData);
			c->StateDisconnected = default(StateDisconnectedData);
			c->RemoteId = default(NetConnectionId);
			c->MapState = NetConnectionMap.EntryState.None;
			c->UniqueId = null;
			c->UniqueIdHash = 0L;
			c->SendTime = 0.0;
			c->RecvTime = 0.0;
			c->Rtt = 0.0;
		}

		public override readonly string ToString()
		{
			return string.Format("[{0}: {1}={2}, {3}={4}]", "NetConnection", "RemoteAddress", RemoteAddress, "UniqueId", UniqueIdHash);
		}
	}
}
