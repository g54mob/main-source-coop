using System;
using Mirror.RemoteCalls;
using UnityEngine;

namespace Mirror
{
	public abstract class NetworkBehaviourHybrid : NetworkBehaviour
	{
		[Tooltip("Occasionally send a full reliable state to delta compress against. This only applies to Components with SyncMethod=Unreliable.")]
		public int baselineRate = 1;

		protected double lastBaselineTime;

		protected double lastDeltaTime;

		private byte lastSerializedBaselineTick;

		private byte lastDeserializedBaselineTick;

		[Tooltip("Enable to send all unreliable messages twice. Only useful for extremely fast-paced games since it doubles bandwidth costs.")]
		public bool unreliableRedundancy;

		[Tooltip("When sending a reliable baseline, should we also send an unreliable delta or rely on the reliable baseline to arrive in a similar time?")]
		public bool baselineIsDelta = true;

		private bool changedSinceBaseline;

		[Header("Debug")]
		public bool debugLog;

		protected bool IsClientWithAuthority
		{
			get
			{
				if (base.isClient)
				{
					return base.authority;
				}
				return false;
			}
		}

		public float baselineInterval
		{
			get
			{
				if (baselineRate >= int.MaxValue)
				{
					return 0f;
				}
				return 1f / (float)baselineRate;
			}
		}

		public virtual void ResetState()
		{
			lastSerializedBaselineTick = 0;
			lastDeserializedBaselineTick = 0;
			changedSinceBaseline = false;
		}

		protected abstract void OnSerializeBaseline(NetworkWriter writer);

		protected abstract void OnDeserializeBaseline(NetworkReader reader, byte baselineTick);

		protected abstract void OnSerializeDelta(NetworkWriter writer);

		protected abstract void OnDeserializeDelta(NetworkReader reader, byte baselineTick);

		protected abstract void StoreState();

		protected abstract bool StateChanged();

		protected virtual void OnDrop(byte lastBaselineTick, byte baselineTick, NetworkReader reader)
		{
		}

		[ClientRpc(channel = 0)]
		private void RpcServerToClientBaseline(ArraySegment<byte> data)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteArraySegmentAndSize(data);
			SendRPCInternal("System.Void Mirror.NetworkBehaviourHybrid::RpcServerToClientBaseline(System.ArraySegment`1<System.Byte>)", 1668420003, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc(channel = 1)]
		private void RpcServerToClientDelta(ArraySegment<byte> data)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteArraySegmentAndSize(data);
			SendRPCInternal("System.Void Mirror.NetworkBehaviourHybrid::RpcServerToClientDelta(System.ArraySegment`1<System.Byte>)", -1576532378, writer, 1, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[Command(channel = 0)]
		private void CmdClientToServerBaseline(ArraySegment<byte> data)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteArraySegmentAndSize(data);
			SendCommandInternal("System.Void Mirror.NetworkBehaviourHybrid::CmdClientToServerBaseline(System.ArraySegment`1<System.Byte>)", 1687650464, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[Command(channel = 1)]
		private void CmdClientToServerDelta(ArraySegment<byte> data)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteArraySegmentAndSize(data);
			SendCommandInternal("System.Void Mirror.NetworkBehaviourHybrid::CmdClientToServerDelta(System.ArraySegment`1<System.Byte>)", -2130260641, writer, 1);
			NetworkWriterPool.Return(writer);
		}

		protected virtual void UpdateServerBaseline(double localTime)
		{
			if (!(localTime < lastBaselineTime + (double)baselineInterval) && changedSinceBaseline)
			{
				byte value = (byte)Time.frameCount;
				using (NetworkWriterPooled networkWriterPooled = NetworkWriterPool.Get())
				{
					networkWriterPooled.WriteByte(value);
					OnSerializeBaseline(networkWriterPooled);
					RpcServerToClientBaseline(networkWriterPooled);
				}
				lastSerializedBaselineTick = value;
				lastBaselineTime = NetworkTime.localTime;
				if (baselineIsDelta)
				{
					lastDeltaTime = localTime;
				}
				StoreState();
				changedSinceBaseline = false;
				if (debugLog)
				{
					Debug.Log($"[{base.name}] Server: sent baseline #{lastSerializedBaselineTick} to: {base.connectionToClient} at time: {localTime}");
				}
			}
		}

		protected virtual void UpdateServerDelta(double localTime)
		{
			if (localTime < lastDeltaTime + (double)syncInterval)
			{
				return;
			}
			if (StateChanged())
			{
				changedSinceBaseline = true;
			}
			if (!changedSinceBaseline)
			{
				return;
			}
			using (NetworkWriterPooled networkWriterPooled = NetworkWriterPool.Get())
			{
				networkWriterPooled.WriteByte(lastSerializedBaselineTick);
				OnSerializeDelta(networkWriterPooled);
				RpcServerToClientDelta(networkWriterPooled);
				if (unreliableRedundancy)
				{
					RpcServerToClientDelta(networkWriterPooled);
				}
			}
			lastDeltaTime = localTime;
			if (debugLog)
			{
				Debug.Log($"[{base.name}] Server: sent delta for #{lastSerializedBaselineTick} to: {base.connectionToClient} at time: {localTime}");
			}
		}

		protected virtual void UpdateServerSync()
		{
			double localTime = NetworkTime.localTime;
			UpdateServerBaseline(localTime);
			UpdateServerDelta(localTime);
		}

		protected virtual void UpdateClientBaseline(double localTime)
		{
			if (!(localTime < lastBaselineTime + (double)baselineInterval) && changedSinceBaseline)
			{
				byte value = (byte)Time.frameCount;
				using (NetworkWriterPooled networkWriterPooled = NetworkWriterPool.Get())
				{
					networkWriterPooled.WriteByte(value);
					OnSerializeBaseline(networkWriterPooled);
					CmdClientToServerBaseline(networkWriterPooled);
				}
				lastSerializedBaselineTick = value;
				lastBaselineTime = NetworkTime.localTime;
				if (baselineIsDelta)
				{
					lastDeltaTime = localTime;
				}
				StoreState();
				changedSinceBaseline = false;
				if (debugLog)
				{
					Debug.Log($"[{base.name}] Client: sent baseline #{lastSerializedBaselineTick} at time: {localTime}");
				}
			}
		}

		protected virtual void UpdateClientDelta(double localTime)
		{
			if (localTime < lastDeltaTime + (double)syncInterval)
			{
				return;
			}
			if (StateChanged())
			{
				changedSinceBaseline = true;
			}
			if (!changedSinceBaseline)
			{
				return;
			}
			using (NetworkWriterPooled networkWriterPooled = NetworkWriterPool.Get())
			{
				networkWriterPooled.WriteByte(lastSerializedBaselineTick);
				OnSerializeDelta(networkWriterPooled);
				CmdClientToServerDelta(networkWriterPooled);
				if (unreliableRedundancy)
				{
					CmdClientToServerDelta(networkWriterPooled);
				}
			}
			lastDeltaTime = localTime;
			if (debugLog)
			{
				Debug.Log($"[{base.name}] Client: sent delta for #{lastSerializedBaselineTick} at time: {localTime}");
			}
		}

		protected virtual void UpdateClientSync()
		{
			if (IsClientWithAuthority && NetworkClient.ready)
			{
				double localTime = NetworkTime.localTime;
				UpdateClientBaseline(localTime);
				UpdateClientDelta(localTime);
			}
		}

		protected virtual void Update()
		{
			if (base.isServer)
			{
				UpdateServerSync();
			}
			else if (base.isClient)
			{
				UpdateClientSync();
			}
		}

		public override void OnSerialize(NetworkWriter writer, bool initialState)
		{
			if (initialState)
			{
				byte value = (byte)Time.frameCount;
				writer.WriteByte(value);
				lastSerializedBaselineTick = (byte)Time.frameCount;
				lastBaselineTime = NetworkTime.localTime;
				StoreState();
			}
		}

		public override void OnDeserialize(NetworkReader reader, bool initialState)
		{
			if (initialState)
			{
				lastDeserializedBaselineTick = reader.ReadByte();
			}
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_RpcServerToClientBaseline__ArraySegment_00601(ArraySegment<byte> data)
		{
			if (IsClientWithAuthority || base.isServer)
			{
				return;
			}
			using NetworkReaderPooled networkReaderPooled = NetworkReaderPool.Get(data);
			lastDeserializedBaselineTick = networkReaderPooled.ReadByte();
			OnDeserializeBaseline(networkReaderPooled, lastDeserializedBaselineTick);
		}

		protected static void InvokeUserCode_RpcServerToClientBaseline__ArraySegment_00601(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcServerToClientBaseline called on server.");
			}
			else
			{
				((NetworkBehaviourHybrid)obj).UserCode_RpcServerToClientBaseline__ArraySegment_00601(reader.ReadArraySegmentAndSize());
			}
		}

		protected void UserCode_RpcServerToClientDelta__ArraySegment_00601(ArraySegment<byte> data)
		{
			if (IsClientWithAuthority || base.isServer)
			{
				return;
			}
			using NetworkReaderPooled networkReaderPooled = NetworkReaderPool.Get(data);
			byte b = networkReaderPooled.ReadByte();
			if (b != lastDeserializedBaselineTick)
			{
				OnDrop(lastDeserializedBaselineTick, b, networkReaderPooled);
				if (debugLog)
				{
					Debug.Log($"[{base.name}] Client: received delta for wrong baseline #{b}. Last was {lastDeserializedBaselineTick}. Ignoring.");
				}
			}
			else
			{
				OnDeserializeDelta(networkReaderPooled, b);
			}
		}

		protected static void InvokeUserCode_RpcServerToClientDelta__ArraySegment_00601(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcServerToClientDelta called on server.");
			}
			else
			{
				((NetworkBehaviourHybrid)obj).UserCode_RpcServerToClientDelta__ArraySegment_00601(reader.ReadArraySegmentAndSize());
			}
		}

		protected void UserCode_CmdClientToServerBaseline__ArraySegment_00601(ArraySegment<byte> data)
		{
			using NetworkReaderPooled networkReaderPooled = NetworkReaderPool.Get(data);
			lastDeserializedBaselineTick = networkReaderPooled.ReadByte();
			OnDeserializeBaseline(networkReaderPooled, lastDeserializedBaselineTick);
		}

		protected static void InvokeUserCode_CmdClientToServerBaseline__ArraySegment_00601(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdClientToServerBaseline called on client.");
			}
			else
			{
				((NetworkBehaviourHybrid)obj).UserCode_CmdClientToServerBaseline__ArraySegment_00601(reader.ReadArraySegmentAndSize());
			}
		}

		protected void UserCode_CmdClientToServerDelta__ArraySegment_00601(ArraySegment<byte> data)
		{
			using NetworkReaderPooled networkReaderPooled = NetworkReaderPool.Get(data);
			byte b = networkReaderPooled.ReadByte();
			if (b != lastDeserializedBaselineTick)
			{
				OnDrop(lastDeserializedBaselineTick, b, networkReaderPooled);
				if (debugLog)
				{
					Debug.Log($"[{base.name}] Server: received delta for wrong baseline #{b} from: {base.connectionToClient}. Last was {lastDeserializedBaselineTick}. Ignoring.");
				}
			}
			else
			{
				OnDeserializeDelta(networkReaderPooled, b);
			}
		}

		protected static void InvokeUserCode_CmdClientToServerDelta__ArraySegment_00601(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdClientToServerDelta called on client.");
			}
			else
			{
				((NetworkBehaviourHybrid)obj).UserCode_CmdClientToServerDelta__ArraySegment_00601(reader.ReadArraySegmentAndSize());
			}
		}

		static NetworkBehaviourHybrid()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkBehaviourHybrid), "System.Void Mirror.NetworkBehaviourHybrid::CmdClientToServerBaseline(System.ArraySegment`1<System.Byte>)", InvokeUserCode_CmdClientToServerBaseline__ArraySegment_00601, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkBehaviourHybrid), "System.Void Mirror.NetworkBehaviourHybrid::CmdClientToServerDelta(System.ArraySegment`1<System.Byte>)", InvokeUserCode_CmdClientToServerDelta__ArraySegment_00601, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkBehaviourHybrid), "System.Void Mirror.NetworkBehaviourHybrid::RpcServerToClientBaseline(System.ArraySegment`1<System.Byte>)", InvokeUserCode_RpcServerToClientBaseline__ArraySegment_00601);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkBehaviourHybrid), "System.Void Mirror.NetworkBehaviourHybrid::RpcServerToClientDelta(System.ArraySegment`1<System.Byte>)", InvokeUserCode_RpcServerToClientDelta__ArraySegment_00601);
		}
	}
}
