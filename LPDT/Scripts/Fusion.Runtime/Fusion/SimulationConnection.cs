#define TRACE
#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Fusion.Sockets;

namespace Fusion
{
	internal class SimulationConnection
	{
		public const int INTEGRATOR_HISTORY_MULT = 10;

		public readonly Simulation Simulation;

		private Dictionary<NetworkId, NetworkObjectConnectionData> _objects;

		private Queue<NetworkId> _objectsDestroyed;

		public PlayerRef Player;

		public bool AreaOfInterestHasBeenUpdated = false;

		public HashSet<int> AreaOfInterestCells = new HashSet<int>();

		internal double LastSend;

		internal int SendCounter;

		internal TimeSeries _packetRecvDelta;

		internal Timer _packetRecvDeltaTimer;

		internal SimulationInput.Buffer _inputs;

		internal TimeSeries _clientOffset;

		internal Tick _latestTickReceived;

		internal Tick _latestTickAcknowledged;

		internal NetworkInterestedObjectList InterestedObjectList;

		internal unsafe NetConnection* Connection;

		internal NetConnectionId ConnectionId;

		public Queue<SimulationMessagePacketData> MessagesOutV2 = new Queue<SimulationMessagePacketData>(64);

		public ulong MessagesOutSequenceV2 = 0uL;

		public ulong MessagesInSequenceV2 = 0uL;

		public DualChannelPriorityQueue<(SimulationMessage, int)> MessagesInV2 = new DualChannelPriorityQueue<(SimulationMessage, int)>();

		public unsafe int ConnectionIndex => Connection->LocalId.GroupIndex;

		public int DestroysPending => _objectsDestroyed.Count;

		public static implicit operator PlayerRef(SimulationConnection c)
		{
			Assert.Check(c.Player.IsRealPlayer, "c.Player.IsRealPlayer");
			return c.Player;
		}

		internal SimulationConnection(Simulation simulation)
		{
			Simulation = simulation;
			SendCounter = 1;
			TickRate.Resolved resolved = TickRate.Resolve(simulation.Config.TickRateSelection);
			int clientSend = resolved.ClientSend;
			clientSend = Math.Max(clientSend, 5);
			_inputs = new SimulationInput.Buffer(resolved.Client);
			_clientOffset = new TimeSeries(clientSend);
			_latestTickReceived = default(Tick);
			_packetRecvDelta = new TimeSeries(clientSend);
			_packetRecvDeltaTimer = default(Timer);
			_objects = new Dictionary<NetworkId, NetworkObjectConnectionData>(NetworkId.Comparer);
			_objectsDestroyed = new Queue<NetworkId>();
			InterestedObjectList = new NetworkInterestedObjectList();
		}

		public bool TryGetObjectData(NetworkId id, out NetworkObjectConnectionData data)
		{
			return (data = GetObjectData(id, create: false)) != null;
		}

		[return: MaybeNull]
		public NetworkObjectConnectionData GetObjectData(NetworkId id, bool create, bool allowFail = false)
		{
			if (!_objects.TryGetValue(id, out var value) && create)
			{
				if (!Simulation.TryGetMeta(id, out var meta))
				{
					if (allowFail)
					{
						return null;
					}
					throw new InvalidOperationException($"tried to get connection object data for {id} but it does not exist");
				}
				value = new NetworkObjectConnectionData();
				value.Id = id;
				value.Filter = ulong.MaxValue;
				if ((bool)meta.Instance && meta.Instance.NetworkedBehaviours != null)
				{
					NetworkBehaviour[] networkedBehaviours = meta.Instance.NetworkedBehaviours;
					for (int i = 0; i < networkedBehaviours.Length; i++)
					{
						if (!networkedBehaviours[i].DefaultReplicated)
						{
							value.Filter &= (ulong)(~(1L << networkedBehaviours[i].ObjectIndex));
						}
					}
				}
				_objects.Add(id, value);
				if (!meta.IsStruct && Simulation.Config.AreaOfInterestEnabled && meta.Flags.Has(NetworkObjectHeaderFlags.GlobalObjectInterest))
				{
					value.SetPlayerFlag(NetworkObjectHeaderPlayerDataFlags.ForceInterest, Simulation);
					InterestedObjectList.SetActive(value, meta);
					try
					{
						InternalLogStreams.LogTraceAreaOfInterest?.Log(Simulation, $"{meta.Id} entering for {Player} (interested at the moment of connection data creation)");
						Simulation.Callbacks.ObjectEnterAOI(Player, id);
					}
					catch (Exception error)
					{
						InternalLogStreams.LogException?.Log(error);
					}
				}
			}
			return value;
		}

		public bool DestroyedNextId(out NetworkId id)
		{
			while (_objectsDestroyed.Count > 0)
			{
				id = _objectsDestroyed.Dequeue();
				if (_objects.TryGetValue(id, out var value))
				{
					if (value.Status != NetworkObjectConnectionDataStatus.DestroyUnconfirmed)
					{
						Assert.Check(value.Status == NetworkObjectConnectionDataStatus.CreatedConfirmed && Simulation.Topology == Topologies.Shared, "Expected destroy unconfirmed for {0}, got: {1} ({2})", id, value.Status, Simulation.Runner);
						InternalLogStreams.LogTraceSendRecv?.Log(Simulation, $"Object {id} was due to be written as a destroy, but something has changed its status to {value.Status}; ignoring");
						continue;
					}
					value.SetStatus(this, NetworkObjectConnectionDataStatus.DestroyPending, "DestroyedNextId");
				}
				return true;
			}
			id = default(NetworkId);
			return false;
		}

		public void ObjectData_Remove(SimulationConnection sc, NetworkId id)
		{
			if (_objects.Remove(id, out var value))
			{
				InternalLogStreams.LogTraceSendRecv?.Log(sc.Simulation, string.Format("[ConnectionStatus] {0}{1}: {2} -> <destroyed>", id, sc.Simulation.IsClient ? "" : ((object)sc.Player), value.Status));
			}
		}

		public void ObjectData_Destroyed(NetworkId id, bool force = false)
		{
			if (_objects.TryGetValue(id, out var value))
			{
				if (value.Status != NetworkObjectConnectionDataStatus.DestroyUnconfirmed)
				{
					value.SetStatus(this, NetworkObjectConnectionDataStatus.DestroyUnconfirmed, "ObjectData_Destroyed");
					_objectsDestroyed.Enqueue(id);
				}
			}
			else if (force)
			{
				_objectsDestroyed.Enqueue(id);
			}
		}

		public bool ObjectData_CreateConfirmed(NetworkObjectMeta meta, bool force = false)
		{
			NetworkObjectConnectionData data;
			if (force)
			{
				data = GetObjectData(meta.Id, create: true);
			}
			else if (!TryGetObjectData(meta.Id, out data))
			{
				return false;
			}
			data.SetStatus(this, NetworkObjectConnectionDataStatus.CreatedConfirmed, "ObjectData_CreateConfirmed");
			return true;
		}

		public bool? ObjectData_IsCreateUnconfirmed(NetworkId id)
		{
			NetworkObjectConnectionData objectData = GetObjectData(id, create: false);
			if (objectData == null)
			{
				return null;
			}
			return objectData.Status == NetworkObjectConnectionDataStatus.CreatedUnconfirmed;
		}

		public bool? ObjectData_IsDestroyUnconfirmedOrPending(NetworkId id)
		{
			NetworkObjectConnectionData objectData = GetObjectData(id, create: false);
			if (objectData == null)
			{
				return null;
			}
			return objectData.Status == NetworkObjectConnectionDataStatus.DestroyUnconfirmed || objectData.Status == NetworkObjectConnectionDataStatus.DestroyPending;
		}

		public void Free(Simulation simulation)
		{
			for (int i = 0; i < MessagesInV2.Count; i++)
			{
				var (obj, num) = MessagesInV2[i].value;
				simulation.ReleaseReference(obj);
			}
			MessagesInV2.Clear();
			if (MessagesOutV2.Count > 0)
			{
				InternalLogStreams.LogTraceSimulationMessage?.Log(simulation, $"Going to drop {MessagesOutV2.Count} messages due to shutdown");
			}
			SimulationMessagePacketData result;
			while (MessagesOutV2.TryDequeue(out result))
			{
				simulation.ReleaseReference(result.Message);
			}
			MessagesOutV2.Clear();
			LastSend = 0.0;
		}

		public void PacketReceiveDelta()
		{
			if (!_packetRecvDeltaTimer.IsRunning)
			{
				_packetRecvDeltaTimer = Timer.StartNew();
				Assert.Check(_packetRecvDelta.IsEmpty, "_packetRecvDelta.IsEmpty");
				if (Simulation.HasRuntimeConfig)
				{
					double value = ((!Simulation.IsClient) ? Simulation.RuntimeConfig.TickRate.ClientSendDelta : Simulation.RuntimeConfig.TickRate.ServerSendDelta);
					_packetRecvDelta.Add(value);
				}
			}
			else
			{
				double value = _packetRecvDeltaTimer.ElapsedInSeconds;
				if (!(value < 0.001))
				{
					_packetRecvDelta.Add(value);
					_packetRecvDeltaTimer.Restart();
				}
			}
		}

		public void ResetTimeFeedback()
		{
			_clientOffset.Clear();
			_packetRecvDelta.Clear();
			_packetRecvDeltaTimer.Reset();
		}

		public void InputReceiveDelta(Tick tick, double receive, double expected)
		{
			if (!(tick <= _latestTickReceived))
			{
				_latestTickReceived = tick;
				_clientOffset.Add(expected - receive);
			}
		}

		public void SetActive(NetworkObjectConnectionData data, NetworkObjectMeta meta)
		{
			InterestedObjectList.SetActive(data, meta);
		}

		public void SetIdle(NetworkObjectConnectionData data)
		{
			InterestedObjectList.SetIdle(data);
		}

		public bool AddAlwaysInterested(NetworkObjectMeta meta)
		{
			if (meta == null || meta.Flags.Has(NetworkObjectHeaderFlags.GlobalObjectInterest))
			{
				return false;
			}
			Assert.Check((Simulation.Config.ReplicationFeatures & NetworkProjectConfig.ReplicationFeatures.InterestManagement) == NetworkProjectConfig.ReplicationFeatures.InterestManagement, "(Simulation.Config.ReplicationFeatures & NetworkProjectConfig.ReplicationFeatures.InterestManagement) == NetworkProjectConfig.ReplicationFeatures.InterestManagement");
			NetworkObjectConnectionData objectData = GetObjectData(meta.Id, create: true);
			if (objectData.PlayerFlags.Has(NetworkObjectHeaderPlayerDataFlags.ForceInterest))
			{
				return false;
			}
			bool flag = Simulation.IsStateAuthority(meta.StateAuthority, Player) || Simulation.IsInputAuthority(meta.InputAuthority, Player);
			if (objectData.PlayerFlags.HasNone(NetworkObjectHeaderPlayerDataFlags.AllInterestFlags) && !flag)
			{
				InternalLogStreams.LogTraceAreaOfInterest?.Log(Simulation, $"{meta.Id} entering for {Player} (always interested)");
				Simulation.Callbacks.ObjectEnterAOI(Player, meta.Id);
			}
			SetActive(objectData, meta);
			objectData.SetPlayerFlag(NetworkObjectHeaderPlayerDataFlags.ForceInterest, Simulation);
			return true;
		}

		public bool RemoveAlwaysInterested(NetworkObjectMeta meta)
		{
			if (meta == null || meta.Flags.Has(NetworkObjectHeaderFlags.GlobalObjectInterest))
			{
				return false;
			}
			Assert.Check((Simulation.Config.ReplicationFeatures & NetworkProjectConfig.ReplicationFeatures.InterestManagement) == NetworkProjectConfig.ReplicationFeatures.InterestManagement, "(Simulation.Config.ReplicationFeatures & NetworkProjectConfig.ReplicationFeatures.InterestManagement) == NetworkProjectConfig.ReplicationFeatures.InterestManagement");
			NetworkObjectConnectionData objectData = GetObjectData(meta.Id, create: false);
			if (objectData == null)
			{
				return false;
			}
			if (objectData.PlayerFlags.HasNot(NetworkObjectHeaderPlayerDataFlags.ForceInterest))
			{
				return false;
			}
			bool flag = Simulation.IsStateAuthority(meta.StateAuthority, Player) || Simulation.IsInputAuthority(meta.InputAuthority, Player);
			if (objectData.PlayerFlags.Has(NetworkObjectHeaderPlayerDataFlags.ForceInterest, NetworkObjectHeaderPlayerDataFlags.AllInterestFlags) && !flag)
			{
				InternalLogStreams.LogTraceAreaOfInterest?.Log(Simulation, $"{meta.Id} exiting for {Player} (no longer always interested)");
				Simulation.Callbacks.ObjectExitAOI(Player, meta.Id);
			}
			objectData.ClearPlayerFlag(NetworkObjectHeaderPlayerDataFlags.ForceInterest, Simulation);
			return true;
		}

		public void EnqueueMessage(in SimulationMessage msg)
		{
			EnqueueMessage(msg, Simulation.Tick, msg.Header.Flags.Has(SimulationMessageHeaderFlags.Reliable) ? (++MessagesOutSequenceV2) : 0);
		}

		public void EnqueueMessage(SimulationMessage msg, int tick, ulong sequence)
		{
			Assert.Check(msg.Header.Flags.Has(SimulationMessageHeaderFlags.Reliable) == (sequence != 0), "msg.Header.Flags.Has(SimulationMessageHeaderFlags.Reliable) == (sequence > 0)");
			Simulation.AddReference(msg);
			MessagesOutV2.Enqueue(new SimulationMessagePacketData(msg, tick, sequence));
			InternalLogStreams.LogTraceSimulationMessage?.Log(Simulation, $"Appended Out Queue of {Player} (seq:{sequence}) (outSeq: {MessagesOutSequenceV2})");
		}

		public void RequeueMessage(SimulationMessage msg, int tick, ulong sequence)
		{
			Assert.Check(msg.Header.Flags.Has(SimulationMessageHeaderFlags.Reliable) == (sequence != 0), "msg.Header.Flags.Has(SimulationMessageHeaderFlags.Reliable) == (sequence > 0)");
			Simulation.AddReference(msg);
			MessagesOutV2.Enqueue(new SimulationMessagePacketData(msg, tick, sequence));
			InternalLogStreams.LogTraceSimulationMessage?.Log(Simulation, $"Appended Out Queue of {Player} (seq:{sequence}) (outSeq: {MessagesOutSequenceV2}) (Requeued)");
		}
	}
}
