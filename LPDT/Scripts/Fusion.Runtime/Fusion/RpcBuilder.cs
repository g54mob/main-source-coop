#define DEBUG
using System;
using System.Runtime.CompilerServices;
using Fusion.Sockets;

namespace Fusion
{
	internal ref struct RpcBuilder : IDisposable
	{
		public readonly Simulation Simulation;

		internal readonly RpcMeta RpcMeta;

		public readonly int PayloadNumBytes;

		private NetworkBehaviour _targetBehaviour;

		private PlayerRef _targetPlayer;

		private int _localAuthorityMask;

		private PooledList<SimulationConnection> _connections;

		private SimulationMessage _msg;

		internal RpcBuilder(Simulation simulation, RpcMeta rpcMeta, int payloadBytes, NetworkBehaviour targetBehaviour, PlayerRef targetPlayer)
		{
			_targetBehaviour = null;
			_msg = null;
			_targetPlayer = PlayerRef.Invalid;
			_localAuthorityMask = 7;
			Simulation = simulation;
			RpcMeta = rpcMeta;
			PayloadNumBytes = payloadBytes;
			if (BehaviourUtils.IsNotNull(targetBehaviour))
			{
				_targetBehaviour = targetBehaviour;
				_localAuthorityMask = Simulation.GetLocalAuthorityMask(ref _targetBehaviour.Object.Header);
			}
			_targetPlayer = targetPlayer;
			_connections = simulation.AcquireSimulationConnectionList(0);
		}

		public void Dispose()
		{
			using (HostProfiler.Markers.RpcBuilderDispose())
			{
				Simulation.Release(ref _connections);
				Simulation.ReleaseReference(_msg);
			}
		}

		public RpcInvokeInfo Prepare(out RpcDataWriter writer)
		{
			using (HostProfiler.Markers.RpcBuilderPrepare())
			{
				RpcLocalInvokeResult local = CheckLocalInvoke();
				RpcSendMessageResult remote = CheckSend(out writer);
				return RpcInvokeInfo.Create(local, remote, PayloadNumBytes);
			}
		}

		private RpcLocalInvokeResult CheckLocalInvoke()
		{
			if (RpcMeta.LocalInvoke == RpcInvokeLocalMode.NotInvocable)
			{
				return RpcLocalInvokeResult.NotInvokableLocally;
			}
			if (Simulation.Stage == SimulationStages.Resimulate)
			{
				return RpcLocalInvokeResult.NotInvokableDuringResim;
			}
			if (RpcMeta.Channel != RpcChannel.ReliableLargeData && PayloadNumBytes > 512)
			{
				return RpcLocalInvokeResult.PayloadSizeExceeded;
			}
			if (((uint)_localAuthorityMask & (uint)RpcMeta.Sources) == 0)
			{
				return RpcLocalInvokeResult.InsufficientSourceAuthority;
			}
			if (_targetPlayer.IsValid && !Simulation.IsLocalPlayer(_targetPlayer))
			{
				return RpcLocalInvokeResult.TargetPlayerIsNotLocal;
			}
			if (((uint)_localAuthorityMask & (uint)RpcMeta.Targets) == 0)
			{
				return RpcLocalInvokeResult.InsufficientTargetAuthority;
			}
			if (RpcMeta.LocalInvoke == RpcInvokeLocalMode.ForwardToPlugin && Simulation.IsClient)
			{
				return RpcLocalInvokeResult.Forwarded;
			}
			return RpcLocalInvokeResult.Invoked;
		}

		private RpcSendMessageResult CheckSend(out RpcDataWriter writer)
		{
			writer = default(RpcDataWriter);
			if (Simulation.Stage == SimulationStages.Resimulate)
			{
				return RpcSendMessageResult.NotInvokableDuringResim;
			}
			if (RpcMeta.Channel != RpcChannel.ReliableLargeData && PayloadNumBytes > 512)
			{
				return RpcSendMessageResult.PayloadSizeExceeded;
			}
			if (((uint)_localAuthorityMask & (uint)RpcMeta.Sources) == 0)
			{
				return RpcSendMessageResult.InsufficientSourceAuthority;
			}
			bool flag = _targetPlayer.IsValid && Simulation.IsLocalPlayer(_targetPlayer);
			if (Simulation._connections.Count == 0)
			{
				return RpcSendMessageResult.NoActiveConnections;
			}
			if (Simulation.IsClient)
			{
				if (RpcMeta.LocalInvoke != RpcInvokeLocalMode.ForwardToPlugin)
				{
					if (flag)
					{
						return RpcSendMessageResult.TargetPlayerIsLocalPlayer;
					}
					if (RpcMeta.Targets == RpcTargets.StateAuthority && _localAuthorityMask == 1)
					{
						return RpcSendMessageResult.NoConnectionsWithAuthorityOrInterest;
					}
				}
				if (!Simulation.TryGetSimulationConnectionByIndex(0, out var result))
				{
					return RpcSendMessageResult.NoActiveConnections;
				}
				_connections.Add(result);
				writer = CreateReaderWriter();
				return RpcSendMessageResult.Sent;
			}
			if (flag)
			{
				return RpcSendMessageResult.TargetPlayerIsLocalPlayer;
			}
			NetworkObjectMeta networkObjectMeta = null;
			if (BehaviourUtils.IsNotNull(_targetBehaviour) && BehaviourUtils.IsNotNull(_targetBehaviour.Object))
			{
				networkObjectMeta = _targetBehaviour.Object.Meta;
			}
			if (_targetPlayer.IsValid)
			{
				PlayerRef realPlayer = Simulation.GetRealPlayer(_targetPlayer);
				if (!Simulation.TryGetSimulationConnectionForPlayer(realPlayer, out var result2))
				{
					return RpcSendMessageResult.TargetPlayerNotAvailable;
				}
				if (networkObjectMeta != null)
				{
					if (!Simulation.HasObjectInterest(result2, networkObjectMeta))
					{
						return RpcSendMessageResult.TargetObjectNotInPlayerInterest;
					}
					if (!Simulation.HasObjectAuthority(result2, networkObjectMeta, (int)RpcMeta.Targets))
					{
						return RpcSendMessageResult.InsufficientTargetAuthority;
					}
				}
				_connections.Add(result2);
				writer = CreateReaderWriter();
				return RpcSendMessageResult.Sent;
			}
			if (Simulation.GetSimulationConnections(ref _connections, networkObjectMeta, (int)RpcMeta.Targets) == 0)
			{
				return RpcSendMessageResult.NoConnectionsWithAuthorityOrInterest;
			}
			writer = CreateReaderWriter();
			return RpcSendMessageResult.Sent;
		}

		public int Send()
		{
			using (HostProfiler.Markers.RpcBuilderSend())
			{
				if (RpcMeta.Channel == RpcChannel.ReliableLargeData)
				{
					SimulationMessageHeader header = MakeHeader();
					Span<byte> largeRpcWriteBuffer = Simulation.GetLargeRpcWriteBuffer(PayloadNumBytes, clear: false);
					Span<SimulationConnection> span = _connections.AsSpan();
					for (int i = 0; i < span.Length; i++)
					{
						SimulationConnection simulationConnection = span[i];
						Assert.Always(simulationConnection != null, "connection != null");
						Simulation.SendReliableData(simulationConnection.ConnectionIndex, header, largeRpcWriteBuffer, progress: false);
					}
				}
				else
				{
					Span<SimulationConnection> span2 = _connections.AsSpan();
					for (int j = 0; j < span2.Length; j++)
					{
						SimulationConnection simulationConnection2 = span2[j];
						Assert.Always(simulationConnection2 != null, "connection != null");
						simulationConnection2.EnqueueMessage(in _msg);
					}
				}
				return _connections.Count;
			}
		}

		private SimulationMessageHeader MakeHeader()
		{
			SimulationMessageHeader result = default(SimulationMessageHeader);
			Unsafe.SkipInit<ReliableKey>(out result.ReliableKey);
			result.Flags = SimulationMessageHeaderFlags.Rpc;
			result.SourcePlayer = (Simulation.IsServer ? default(PlayerRef) : Simulation.LocalPlayer);
			result.TargetPlayer = _targetPlayer;
			result.PayloadNumBytes = PayloadNumBytes;
			result.MessageType = RpcMeta.Key;
			result.TargetObject = (BehaviourUtils.IsNull(_targetBehaviour) ? default(NetworkBehaviourId) : _targetBehaviour.Id);
			if (RpcMeta.LocalInvoke == RpcInvokeLocalMode.ForwardToPlugin)
			{
				result.Flags |= SimulationMessageHeaderFlags.AllowRoundtrip;
			}
			if (!BehaviourUtils.IsNull(_targetBehaviour))
			{
				result.Flags |= SimulationMessageHeaderFlags.HasTargetObject;
			}
			if (_targetPlayer.IsValid)
			{
				result.Flags |= SimulationMessageHeaderFlags.HasTargetPlayer;
			}
			if (RpcMeta.Channel == RpcChannel.Reliable)
			{
				result.Flags |= SimulationMessageHeaderFlags.Reliable;
			}
			if (RpcMeta.IsTickAligned)
			{
				result.Flags |= SimulationMessageHeaderFlags.TickAligned;
			}
			return result;
		}

		private RpcDataWriter CreateReaderWriter()
		{
			if (RpcMeta.Channel == RpcChannel.ReliableLargeData)
			{
				return new RpcDataWriter(Simulation.GetLargeRpcWriteBuffer(PayloadNumBytes, clear: true));
			}
			_msg = Simulation.AcquireSimulationMessage(MakeHeader());
			Assert.Check(_msg.RefCount == 1, "_msg.RefCount == 1");
			return new RpcDataWriter(_msg.Payload);
		}
	}
}
