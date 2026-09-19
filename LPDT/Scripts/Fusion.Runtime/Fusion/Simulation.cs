#define FUSION_UNITY
#define TRACE
#define DEBUG
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Fusion.Sockets;
using Fusion.Sockets.V2;
using Fusion.Statistics;
using JetBrains.Annotations;
using UnityEngine;

namespace Fusion
{
	public abstract class Simulation : ILogSource, INetPeerGroupCallbacks
	{
		private class AreaOfInterestCell
		{
			public int Key;

			public NetworkObjectMeta.AreaOfInterestList Objects;

			public BitSet512 Connections;

			public bool Empty => Objects.IsEmpty && Connections.Empty();

			public void PoolInit(Simulation simulation, int cellKey)
			{
				Assert.Check(Objects.IsEmpty, "Objects.IsEmpty");
				Assert.Check(Connections.Empty(), "Connections.Empty()");
				Key = cellKey;
			}

			public void PoolReset(Simulation simulation)
			{
				Key = 0;
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		[Obsolete("Use NetworkRunner.AreaOfInterest instead")]
		public struct AreaOfInterest
		{
			[Obsolete("Use NetworkRunner.SetAreaOfInterestCellSize or NetworkRunner.AreaOfInterest.GetCellSize instead.")]
			public static int CELL_SIZE => GetFirstRunningRunner()?._simulation.AreaOfInterestInstance.GetCellSize() ?? 32;

			[Obsolete("Use NetworkRunner.AreaOfInterest.GetGridSize instead.")]
			public static (int x, int y, int z) GetGridSize()
			{
				return GetFirstRunningRunner()?._simulation.AreaOfInterestInstance.GetGridSize() ?? (32768, 32768, 32768);
			}

			[Obsolete("Use NetworkRunner.AreaOfInterest.GetCellSize instead.")]
			public static int GetCellSize()
			{
				return GetFirstRunningRunner()?._simulation.AreaOfInterestInstance.GetCellSize() ?? 32;
			}

			[Obsolete("Use NetworkRunner.AreaOfInterest.SphereToCells instead.")]
			public static void SphereToCells(Vector3 position, float radius, HashSet<int> cells)
			{
				GetFirstRunningRunner()?._simulation.AreaOfInterestInstance.SphereToCells(position, radius, cells);
			}

			[Obsolete("Use NetworkRunner.AreaOfInterest.ToCellCoords instead.")]
			public static (int x, int y, int z) ToCellCoords(Vector3 position)
			{
				return GetFirstRunningRunner()?._simulation.AreaOfInterestInstance.ToCellCoords(position) ?? default((int, int, int));
			}

			[Obsolete("Use NetworkRunner.AreaOfInterest.ToCellCoords instead.")]
			public static (int x, int y, int z) ToCellCoords(int index)
			{
				return GetFirstRunningRunner()?._simulation.AreaOfInterestInstance.ToCellCoords(index) ?? default((int, int, int));
			}

			[Obsolete("Use NetworkRunner.AreaOfInterest.ToCellCenter instead.")]
			public static Vector3 ToCellCenter(int index)
			{
				return GetFirstRunningRunner()?._simulation.AreaOfInterestInstance.ToCellCenter(index) ?? default(Vector3);
			}

			[Obsolete("Use NetworkRunner.AreaOfInterest.ToCell instead.")]
			public static int ToCell(Vector3 position)
			{
				return GetFirstRunningRunner()?._simulation.AreaOfInterestInstance.ToCell(position) ?? 0;
			}

			[Obsolete("Use NetworkRunner.AreaOfInterest.ToCell instead.")]
			public static int ToCell(int x, int y, int z)
			{
				return GetFirstRunningRunner()?._simulation.AreaOfInterestInstance.ToCell(x, y, z) ?? 0;
			}

			[Obsolete("Use NetworkRunner.AreaOfInterest.ClampCellCoords instead.")]
			public static (int x, int y, int z) ClampCellCoords(int x, int y, int z)
			{
				return GetFirstRunningRunner()?._simulation.AreaOfInterestInstance.ClampCellCoords(x, y, z) ?? default((int, int, int));
			}

			private static NetworkRunner GetFirstRunningRunner()
			{
				return NetworkRunner.Instances.FirstOrDefault((NetworkRunner runner) => runner.IsRunning);
			}
		}

		public readonly struct AreaOfInterestState
		{
			private const int MAX_SHARED_RADIUS_PER_CELL_SIZE = 8;

			internal const int CELL_SIZE_DEFAULT = 32;

			internal const int GRID_CELL_COUNT_DEFAULT = 1024;

			internal const int MIN_CELL_SIZE = 4;

			internal const int MAX_CELL_SIZE_SHARED = 128;

			private readonly int _xSize;

			private readonly int _ySize;

			private readonly int _zSize;

			private readonly int _cellSize;

			public int MaxSharedRadius => 8 * _cellSize;

			public AreaOfInterestState(InterestManagementConfig config)
			{
				_cellSize = config.AreaOfInterestCellSize;
				_xSize = config.AreaOfInterestCellGridSize.x;
				_ySize = config.AreaOfInterestCellGridSize.y;
				_zSize = config.AreaOfInterestCellGridSize.z;
			}

			public (int x, int y, int z) GetGridSize()
			{
				return (x: _xSize, y: _ySize, z: _zSize);
			}

			public int GetCellSize()
			{
				return _cellSize;
			}

			public void SphereToCells(Vector3 position, float radius, HashSet<int> cells)
			{
				(int, int, int) tuple = ToCellCoords(position - new Vector3(radius, radius, radius));
				(int, int, int) tuple2 = ToCellCoords(position + new Vector3(radius, radius, radius));
				var (i, _, _) = tuple;
				for (; i <= tuple2.Item1; i++)
				{
					for (int j = tuple.Item2; j <= tuple2.Item2; j++)
					{
						for (int k = tuple.Item3; k <= tuple2.Item3; k++)
						{
							cells.Add(ToCell(i, j, k));
						}
					}
				}
			}

			public (int x, int y, int z) ToCellCoords(Vector3 position)
			{
				int num = (int)(position.x / (float)_cellSize);
				int num2 = (int)(position.y / (float)_cellSize);
				int num3 = (int)(position.z / (float)_cellSize);
				if (position.x < 0f)
				{
					num--;
				}
				if (position.y < 0f)
				{
					num2--;
				}
				if (position.z < 0f)
				{
					num3--;
				}
				return ClampCellCoords(num + _xSize / 2, num2 + _ySize / 2, num3 + _zSize / 2);
			}

			public (int x, int y, int z) ToCellCoords(int index)
			{
				index--;
				int num = index / (_xSize * _ySize);
				index -= num * _xSize * _ySize;
				int item = index / _xSize;
				int item2 = index % _xSize;
				return (x: item2, y: item, z: num);
			}

			public Vector3 ToCellCenter(int index)
			{
				if (index == -1)
				{
					return default(Vector3);
				}
				var (num, num2, num3) = ToCellCoords(index);
				return new Vector3((num - _xSize / 2) * _cellSize + _cellSize / 2, (num2 - _ySize / 2) * _cellSize + _cellSize / 2, (num3 - _zSize / 2) * _cellSize + _cellSize / 2);
			}

			public int ToCell(Vector3 position)
			{
				var (x, y, z) = ToCellCoords(position);
				return ToCell(x, y, z);
			}

			public int ToCell(int x, int y, int z)
			{
				(x, y, z) = ClampCellCoords(x, y, z);
				return z * _xSize * _ySize + y * _xSize + x + 1;
			}

			public (int x, int y, int z) ClampCellCoords(int x, int y, int z)
			{
				return (x: Clamp(x, _xSize), y: Clamp(y, _ySize), z: Clamp(z, _zSize));
				static int Clamp(int v, int max)
				{
					return (v >= 0) ? ((v >= max) ? (max - 1) : v) : 0;
				}
			}
		}

		internal class Client : Simulation
		{
			private unsafe NetConnection* _server;

			private bool _stateReceived;

			private Timeline _history;

			private InterpolationParams _globalInterpolationParams;

			private SimulationInput.Buffer _inputBuffer;

			private SimulationInput[] _inputArray;

			private bool? _previousWasMC;

			public Tick PreviousServerTick;

			private readonly Stopwatch _timeSinceLastSimulation = new Stopwatch();

			private const int SendPacketsDespiteNoSimulationThresholdMilliseconds = 1000;

			internal unsafe NetConnection* ServerConnection => _server;

			public override Tick LatestServerTick => _history.IsEmpty ? default(Tick) : _history.Points.Back().Tick;

			public unsafe bool IsConnectedToServer => _server != null;

			public unsafe NetAddress ServerAddress => IsConnectedToServer ? _server->RemoteAddress : default(NetAddress);

			public unsafe double RttToServer => (_server == null) ? 0.0 : _server->RoundTripTime;

			public override PlayerRef LocalPlayer => _callbacks.LocalPlayerRef;

			public override IEnumerable<PlayerRef> ActivePlayers
			{
				get
				{
					foreach (PlayerRef player in _players)
					{
						yield return player;
					}
				}
			}

			private static string NullableToString<T>(T? value) where T : struct
			{
				if (value.HasValue)
				{
					return value.Value.ToString();
				}
				return "null";
			}

			internal Client(SimulationArgs args)
				: base(args)
			{
				ClientTimeProvider clientTimeProvider = new ClientTimeProvider();
				clientTimeProvider.OnReset(Clock.Local, ResetClientSimulationState);
				_time = clientTimeProvider;
				TickRate.Resolved resolved = Fusion.TickRate.Resolve(_projectConfig.Simulation.TickRateSelection);
				_history = new Timeline(resolved.ServerSend);
				_inputBuffer = new SimulationInput.Buffer(resolved.Client);
				_inputArray = new SimulationInput[resolved.Client];
			}

			public unsafe void Connect(NetAddress address, byte[] token = null, byte[] uniqueId = null)
			{
				NetPeerGroup.Connect(_netPeerGroup, address, token, uniqueId);
			}

			public unsafe void Connect(string ip, ushort port, byte[] token = null, byte[] uniqueId = null)
			{
				NetPeerGroup.Connect(_netPeerGroup, ip, port, token, uniqueId);
			}

			protected unsafe override void NetworkConnected(NetConnection* connection)
			{
				_server = connection;
			}

			protected unsafe override void NetworkDisconnected(NetConnection* connection, NetDisconnectReason reason)
			{
				try
				{
					Assert.Check(_server == connection, "_server == connection");
					_server = null;
					_callbacks.OnDisconnectedFromServer(reason);
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(this, error);
				}
			}

			internal unsafe override void OnNetworkShutdown()
			{
				if (_server != null)
				{
					NetPeerGroup.Disconnect(_netPeerGroup, _server, null);
					NetworkSend();
				}
				_server = null;
			}

			internal unsafe void ResetRttToServer(double rtt = 0.0)
			{
				NetConnection.SetRtt(_server, rtt);
			}

			internal override double GetPlayerRtt(PlayerRef player)
			{
				if (player == LocalPlayer || player == PlayerRef.None)
				{
					return RttToServer;
				}
				return 0.0;
			}

			internal unsafe override PlayerRef Connection2Player(NetConnection* c)
			{
				return LocalPlayer;
			}

			internal unsafe override int Player2Connection(PlayerRef player)
			{
				return _server->LocalId.GroupIndex;
			}

			internal override void RecvPacket(ref RecvContext rc)
			{
				Tick tick = rc.Tick;
				TimeFeedback feedback = default(TimeFeedback);
				feedback.Read(rc.Read);
				_stateReplicator.RecvPacket(ref rc);
				_stateReceived = true;
				if (base.HasRuntimeConfig)
				{
					_history.AddPoint(new TimelinePoint(tick, tick, base.TickDeltaDouble), base.TickDeltaDouble, allowInactiveHandling: false);
					UpdateObjectTimelines();
					_time.OnFeedbackReceived(feedback);
				}
			}

			protected override void NetworkReceiveDone()
			{
				if (!(LatestServerTick != PreviousServerTick))
				{
					return;
				}
				if (_time.IsRunning())
				{
					double num = (double)((int)LatestServerTick - (int)PreviousServerTick) * base.TickDeltaDouble;
					if (num <= 1.0)
					{
						InternalLogStreams.LogTraceSnapshots?.Log(this, $"received snapshot {LatestServerTick}");
						_time.OnSnapshotReceived(RttToServer, LatestServerTick);
					}
					else
					{
						InternalLogStreams.LogTraceSnapshots?.Log(this, $"connection was lost for a long time, received snapshot {LatestServerTick}");
						ResetRttToServer(Maths.Clamp(RttToServer - num, 0.15, 0.25));
						_time.Reset(RttToServer, LatestServerTick);
					}
				}
				else
				{
					InternalLogStreams.LogTraceSnapshots?.Log(this, $"received first snapshot {LatestServerTick}");
					Assert.Check(base.HasRuntimeConfig, "HasRuntimeConfig");
					_time.Configure(base.RuntimeConfig);
					_time.Configure(_projectConfig.TimeSync);
					_time.SetPlayerIndex(LocalPlayer.AsIndex);
					if (base.ProjectConfig.ClientsRecordFrameAndPacketTimingTraces)
					{
						_time.StartTrace();
					}
					_time.Reset(RttToServer, LatestServerTick);
				}
				PreviousServerTick = LatestServerTick;
			}

			internal override void WritePackets(ref SendContext sc)
			{
				switch (base.Topology)
				{
				case Topologies.ClientServer:
					WriteInput(ref sc);
					break;
				case Topologies.Shared:
					_stateReplicator.SendPacket(ref sc);
					break;
				}
			}

			private unsafe void WriteInput(ref SendContext sc)
			{
				int length = sc.Write.Length;
				int num = Maths.Clamp(Mathf.CeilToInt((float)(_server->Rtt / base.TickDeltaDouble)), 3, 6);
				if (base.Config.InputTransferMode == SimulationConfig.InputTransferModes.LatestState)
				{
					num = 1;
				}
				_inputRoot.Clear(_config.InputTotalWordCount);
				(SimulationInput[], int) sortedInputs = GetSortedInputs();
				SimulationInput[] item = sortedInputs.Item1;
				int item2 = sortedInputs.Item2;
				int i = Math.Max(0, item2 - num);
				int num2 = i;
				HostProfiler.Counters.InputQueue?.Add(item2 - num2);
				sc.Write.Byte((byte)(item2 - num2));
				if (base.Config.InputTransferMode == SimulationConfig.InputTransferModes.RedundancyUncompressed)
				{
					Assert.Check(_config.InputTotalWordCount >= 1 && _config.InputTotalWordCount <= 255, "_config.InputTotalWordCount >= 1 && _config.InputTotalWordCount <= 255");
					sc.Write.Short((short)_config.InputTotalWordCount);
					for (; i < item2; i++)
					{
						sc.Write.Span(new Span<byte>(item[i]._ptr, _config.InputTotalWordCount * 4));
					}
				}
				else
				{
					for (; i < item2; i++)
					{
						if (i == num2)
						{
							item[i].Serialize(_inputRoot, _config, sc.Write, null);
						}
						else
						{
							item[i].Serialize(item[i - 1], _config, sc.Write, null);
						}
					}
				}
				int num3 = sc.Write.Length - length;
				_fusionStatsManager.AddStat(FusionStatType.InputOutBandwidth, num3);
				HostProfiler.Counters.InputSize?.Add(num3);
			}

			private (SimulationInput[], int) GetSortedInputs()
			{
				int item = _inputBuffer.CopySortedTo(_inputArray);
				return (_inputArray, item);
			}

			internal override SimulationInput PollInput(Tick tick, PlayerRef player)
			{
				if (!base.IsPlayer || player != LocalPlayer)
				{
					return null;
				}
				SimulationInput obj = AcquireSimulationInput(_config);
				obj.Player = player;
				obj.Header.Tick = tick;
				Tick interpFromPrev = _interpFromPrev;
				Tick interpToPrev = _interpToPrev;
				float remoteAlphaPrev = _remoteAlphaPrev;
				Instant instant = _time.Now();
				double num = Math.Max(instant.Input - instant.Local, 0.0);
				if (num > 0.0)
				{
					double num2 = (double)Maths.Lerp((int)interpFromPrev, (int)interpToPrev, remoteAlphaPrev) * base.TickDeltaDouble;
					double num3 = num2 + num;
					double num4 = num3 / base.TickDeltaDouble;
					Assert.Check(base.HasRuntimeConfig, "HasRuntimeConfig");
					int num5 = base.RuntimeConfig.TickRate.Client / base.RuntimeConfig.TickRate.ServerSend;
					double num6 = (double)(int)interpFromPrev + Math.Floor((num4 - (double)(int)interpFromPrev) / (double)num5) * (double)num5;
					double num7 = (double)(int)interpFromPrev + Math.Ceiling((num4 - (double)(int)interpFromPrev) / (double)num5) * (double)num5;
					if (num7 == num6)
					{
						num7 += (double)num5;
					}
					double num8 = Maths.Clamp01((num4 - num6) / (num7 - num6));
					obj.Header.InterpFrom = (int)num6;
					obj.Header.InterpTo = (int)num7;
					obj.Header.InterpAlpha = (float)num8;
					obj.Delayed = true;
				}
				else
				{
					obj.Header.InterpFrom = interpFromPrev;
					obj.Header.InterpTo = interpToPrev;
					obj.Header.InterpAlpha = remoteAlphaPrev;
					obj.Delayed = false;
				}
				_callbacks.InvokeOnInput(obj);
				if (_inputBuffer.Add(obj))
				{
					return obj;
				}
				Release(ref obj);
				return null;
			}

			internal override SimulationInput GetBufferedInput(Tick tick, PlayerRef player)
			{
				if (!base.IsPlayer || player != LocalPlayer)
				{
					return null;
				}
				if (_inputBuffer.TryGet(tick, out var input))
				{
					if (input.Delayed)
					{
						int num = input.Header.InterpFrom;
						int num2 = input.Header.InterpTo;
						float interpAlpha = input.Header.InterpAlpha;
						double time = Maths.Lerp((double)num, (double)num2, (double)interpAlpha) * base.TickDeltaDouble;
						InterpolationParams interpolationParams = _history.GetInterpolationParams(time);
						input.Header.InterpFrom = interpolationParams.From;
						input.Header.InterpTo = interpolationParams.To;
						input.Header.InterpAlpha = interpolationParams.Alpha;
						input.Delayed = false;
					}
					return input;
				}
				return null;
			}

			protected override void BeforeFirstTick()
			{
				_callbacks.OnClientStart();
			}

			protected override void BeforeUpdate()
			{
				if (TryGetStruct(NetworkId.RuntimeConfig, out var meta))
				{
					bool flag = meta.GetStructData<SimulationRuntimeConfig>().MasterClient == LocalPlayer;
					if (base.Topology == Topologies.Shared && _previousWasMC.HasValue && _previousWasMC.Value != flag)
					{
						UpdateSimulationStateForMasterClientObjects(flag);
					}
					_previousWasMC = flag;
				}
			}

			protected override int BeforeSimulation()
			{
				int num = 0;
				using (HostProfiler.Markers.ClientBeforeSimulation())
				{
					if (_stateReceived)
					{
						_stateReceived = false;
						if (IsConnectedToServer)
						{
							HostProfiler.Counters.RoundTripTime?.Set((float)RttToServer);
							_fusionStatsManager.SetStat(FusionStatType.RoundTripTime, (float)RttToServer);
						}
						(SimulationInput[], int) sortedInputs = GetSortedInputs();
						SimulationInput[] item = sortedInputs.Item1;
						int item2 = sortedInputs.Item2;
						for (int i = 0; i < item2; i++)
						{
							if (item[i].Header.Tick <= LatestServerTick && _inputBuffer.Remove(item[i].Header.Tick, out var removed))
							{
								Release(ref removed);
							}
						}
						if (base.Topology == Topologies.ClientServer)
						{
							num = Math.Max(0, (int)base.Tick - (int)LatestServerTick);
							_tick = LatestServerTick;
							try
							{
								ResetPredictedObjectsToLatestServerState();
								if (num > 0)
								{
									RunClientSideResimulationLoop(num);
								}
							}
							catch (Exception error)
							{
								InternalLogStreams.LogException?.Log(this, error);
							}
						}
					}
				}
				return num;
			}

			internal void ResetClientSimulationState()
			{
				InternalLogStreams.LogTraceSnapshots?.Log(this, $"(re)setting client simulation state to {LatestServerTick}");
				_tick = LatestServerTick;
				_inputTick = LatestServerTick;
				_sendTick = LatestServerTick;
				if (base.Topology == Topologies.ClientServer)
				{
					ResetPredictedObjectsToLatestServerState();
				}
				_inputBuffer.Clear();
				_inputCollection.Clear();
				_stateReceived = false;
			}

			internal void ResetPredictedObjectsToLatestServerState()
			{
				_callbacks.OnBeforeClientSidePredictionReset();
				int num = 0;
				foreach (NetworkObjectMeta value in _metaLookup.Values)
				{
					if (value.HasSnapshots)
					{
						value.SnapshotLatest.CopyTo(value);
						value.SnapshotLatest.CopyTo(value.Previous);
						num++;
					}
				}
				_callbacks.OnAfterClientSidePredictionReset();
			}

			private void RunClientSideResimulationLoop(int ticks)
			{
				using (HostProfiler.Markers.RunClientSideResimulationLoop())
				{
					Assert.Check(base.Tick == LatestServerTick, "Tick == LatestServerTick");
					InvokeOnBeforeAllTicks(resimulation: true, ticks);
					for (int i = 0; i < ticks; i++)
					{
						StepSimulation(SimulationStages.Resimulate, i == ticks - 1, i == 0, freeInput: false);
					}
					Assert.Check(base.Tick == (int)LatestServerTick + ticks, "Tick == (LatestServerTick + ticks)");
					InvokeOnAfterAllTicks(resimulation: true, ticks);
				}
			}

			protected override void AfterSimulation()
			{
				_timeSinceLastSimulation.Restart();
			}

			protected override void NoSimulation()
			{
				if (!_isWaitingForShutdown && base.HasRuntimeConfig && _timeSinceLastSimulation.ElapsedMilliseconds > 1000)
				{
					InternalLogStreams.LogTraceSendRecv?.Log(this, $"No simulation for over {1000}, sending enqueued packets regardless");
					_timeSinceLastSimulation.Restart();
					SendPackets();
				}
			}

			protected override void UpdateInterpolationParams()
			{
				using (HostProfiler.Markers.UpdateInterpolationParams())
				{
					if (base.HasRuntimeConfig && !_history.IsEmpty)
					{
						_globalInterpolationParams = _history.GetInterpolationParams(Runner.RemoteRenderTime);
						_interpFromPrev = _interpFrom;
						_interpToPrev = _interpTo;
						_remoteAlphaPrev = _remoteAlpha;
						_interpFrom = _globalInterpolationParams.From;
						_interpTo = _globalInterpolationParams.To;
						_remoteAlpha = _globalInterpolationParams.Alpha;
					}
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal void UpdateObjectInterpolationParams(NetworkObjectMeta obj)
			{
				using (HostProfiler.Markers.UpdateObjectInterpolationParams())
				{
					if (obj.HasNetworkedState && !(Math.Abs(obj.LastGlobalRemoteTime - _globalInterpolationParams.Time) < 1.401298464324817E-45))
					{
						obj.LocalInterpolationParams = obj.Timeline.GetInterpolationParams(Runner.LocalRenderTime);
						obj.RemoteInterpolationParams = obj.Timeline.GetInterpolationParams(Runner.RemoteRenderTime);
						obj.LastGlobalRemoteTime = _globalInterpolationParams.Time;
					}
				}
			}

			private void UpdateObjectTimelines()
			{
				using (HostProfiler.Markers.UpdateObjectTimelines())
				{
					if (!base.IsClient || !base.HasRuntimeConfig)
					{
						return;
					}
					double tickDeltaDouble = base.TickDeltaDouble;
					foreach (var (_, networkObjectMeta2) in _metaLookup)
					{
						if (networkObjectMeta2.HasNetworkedState)
						{
							networkObjectMeta2.AddLatestSnapshotToTimelineFast(tickDeltaDouble);
						}
					}
				}
			}
		}

		internal enum ObjectChangeType
		{
			Created = 0,
			Updated = 1,
			Destroyed = 2
		}

		internal struct PlayerRefMapping
		{
			public int ActorId;

			public PlayerRef PlayerRef;
		}

		[StructLayout(LayoutKind.Explicit)]
		internal struct PlayerSimulationData
		{
			[FieldOffset(0)]
			public PlayerRef Player;

			[FieldOffset(4)]
			public NetworkId Object;

			[FieldOffset(8)]
			public int Actor;
		}

		internal class ExceptionLimits
		{
			public readonly LogStream.LogTracker ReadObjectUpdatesPerPlayer = LogStream.LogTracker.Create(2uL, repeat: false);

			public readonly LogStream.LogTracker ReadObjectDestroysOfInternalObjects = LogStream.LogTracker.Create(2uL, repeat: false);

			public readonly LogStream.LogTracker AllocateObjects = LogStream.LogTracker.Create(10uL, repeat: true);

			public readonly LogStream.LogTracker ReadMessagesFromBuffer = LogStream.LogTracker.Create(10uL, repeat: true);

			public readonly LogStream.LogTracker PluginVersionWrite = LogStream.LogTracker.Create(10uL, repeat: false);

			public readonly LogStream.LogTracker PluginAuthorityData = LogStream.LogTracker.Create(2uL, repeat: false);

			public void ResetTrackers(int dynamicUniqueId)
			{
				ReadObjectUpdatesPerPlayer.ResetTracker(dynamicUniqueId);
				ReadObjectDestroysOfInternalObjects.ResetTracker(dynamicUniqueId);
				AllocateObjects.ResetTracker(dynamicUniqueId);
				ReadMessagesFromBuffer.ResetTracker(dynamicUniqueId);
				PluginVersionWrite.ResetTracker(dynamicUniqueId);
				PluginAuthorityData.ResetTracker(dynamicUniqueId);
			}

			public void ResetAllTrackers()
			{
				ReadObjectUpdatesPerPlayer.ResetAllTrackers();
				ReadObjectDestroysOfInternalObjects.ResetAllTrackers();
				AllocateObjects.ResetAllTrackers();
				ReadMessagesFromBuffer.ResetAllTrackers();
				PluginVersionWrite.ResetAllTrackers();
				PluginAuthorityData.ResetAllTrackers();
			}
		}

		internal class History
		{
			private SimulationHistoryEntry.List _entryList;

			public SimulationHistoryEntry Latest => (_entryList.Head == null) ? null : _entryList.Prev(_entryList.Head);

			public SimulationHistoryEntry Oldest => _entryList.Head;

			public History(int capacity)
			{
				_entryList = new SimulationHistoryEntry.List();
				for (int i = 0; i < capacity; i++)
				{
					_entryList.AddLast(new SimulationHistoryEntry());
				}
			}

			public SimulationHistoryEntry Add(Tick tick, double time)
			{
				SimulationHistoryEntry simulationHistoryEntry = _entryList.RemoveFirst();
				simulationHistoryEntry.Tick = tick;
				simulationHistoryEntry.Time = time;
				_entryList.AddLast(simulationHistoryEntry);
				return simulationHistoryEntry;
			}
		}

		internal interface ICallbacks
		{
			bool IsSharedModeMasterClient { get; }

			bool CanReceivePlayerJoinLeaveCallbacks { get; }

			PlayerRef LocalPlayerRef { get; }

			void OnTick();

			void OnServerStart();

			void OnClientStart();

			void OnAfterClientSidePredictionReset();

			void OnBeforeClientSidePredictionReset();

			void OnAfterTick();

			void OnBeforeTick();

			void OnAfterAllTicks(bool resimulation, int tickCount);

			void OnBeforeAllTicks(bool resimulation, int tickCount);

			void OnAfterSimulation();

			void OnBeforeSimulation(int forwardTickCount);

			void OnBeforeCopyPreviousState();

			void OnConnectedToServer();

			void OnDisconnectedFromServer(NetDisconnectReason reason);

			OnConnectionRequestReply OnConnectionRequest(NetAddress remoteAddress, byte[] token);

			void OnConnectionFailed(NetAddress remoteAddress, NetConnectFailedReason reason);

			void OnRejoinRequest(string sessionId);

			void OnReliableData(PlayerRef player, ReliableKey key, bool local, ReadOnlySpan<byte> dataArray);

			void OnReliableDataProgress(PlayerRef player, ReliableKey key, float progress);

			void PlayerJoined(PlayerRef player);

			void PlayerLeft(PlayerRef player);

			void OnInternalConnectionAttempt(int attempt, int totalConnectionAttempts, out bool shouldChange, out NetAddress newAddress);

			void ObjectStateAuthorityChanged(NetworkId id, bool gained);

			void ObjectInputAuthorityChanged(NetworkId id, bool gained);

			void ObjectIsSimulatedChanged(NetworkId id, bool simulated);

			void ObjectEnterAOI(PlayerRef player, NetworkId id);

			void ObjectExitAOI(PlayerRef player, NetworkId id);

			RemoteObjectCreateResult OnRemoteCreate(PlayerRef player, [In][RequiresLocation] ref NetworkObjectHeader header);

			bool OnRemoteDestroy(PlayerRef player, NetworkId id);

			void UpdateRemotePrefabs();

			void OnObjectChanged(in NetworkObjectChange change);

			void OnObjectWrite(in NetworkObjectChange change);

			bool OnStateAuthorityRequested(PlayerRef player, NetworkObjectMeta meta, bool acquire);

			void OnInput(SimulationInput input);

			void OnInputMissing(SimulationInput input);
		}

		private struct SimulationMessageResult : IDisposable
		{
			public PooledList<SimulationConnection> TargetConnections;

			private readonly SimulationMessageResultCode _resultCode;

			private readonly Simulation _simulation;

			public bool IsRetry
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return _resultCode == SimulationMessageResultCode.Retry;
				}
			}

			public bool IsForward
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return _resultCode == SimulationMessageResultCode.Forward;
				}
			}

			public SimulationMessageResult(SimulationMessageResultCode result, Simulation simulation, PooledList<SimulationConnection> targetConnections)
			{
				_resultCode = result;
				_simulation = simulation;
				TargetConnections = targetConnections;
			}

			public void Dispose()
			{
				_simulation?.Release(ref TargetConnections);
			}

			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("SimulationMessageResult");
				stringBuilder.Append(" { ");
				if (PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				builder.Append("TargetConnections = ");
				builder.Append(TargetConnections.ToString());
				builder.Append(", IsRetry = ");
				builder.Append(IsRetry.ToString());
				builder.Append(", IsForward = ");
				builder.Append(IsForward.ToString());
				return true;
			}

			[CompilerGenerated]
			public static bool operator !=(SimulationMessageResult left, SimulationMessageResult right)
			{
				return !(left == right);
			}

			[CompilerGenerated]
			public static bool operator ==(SimulationMessageResult left, SimulationMessageResult right)
			{
				return left.Equals(right);
			}

			[CompilerGenerated]
			public override readonly int GetHashCode()
			{
				return (EqualityComparer<PooledList<SimulationConnection>>.Default.GetHashCode(TargetConnections) * -1521134295 + EqualityComparer<SimulationMessageResultCode>.Default.GetHashCode(_resultCode)) * -1521134295 + EqualityComparer<Simulation>.Default.GetHashCode(_simulation);
			}

			[CompilerGenerated]
			public override readonly bool Equals(object obj)
			{
				return obj is SimulationMessageResult && Equals((SimulationMessageResult)obj);
			}

			[CompilerGenerated]
			public readonly bool Equals(SimulationMessageResult other)
			{
				return EqualityComparer<PooledList<SimulationConnection>>.Default.Equals(TargetConnections, other.TargetConnections) && EqualityComparer<SimulationMessageResultCode>.Default.Equals(_resultCode, other._resultCode) && EqualityComparer<Simulation>.Default.Equals(_simulation, other._simulation);
			}
		}

		private enum SimulationMessageResultCode
		{
			Handled = 0,
			Forward = 1,
			Retry = 2,
			InvalidTargetPlayer = 3,
			InvalidTargetObject = 4,
			InsufficientSourceAuthority = 5,
			InsufficientTargetAuthority = 6,
			InstanceDoesNotExist = 7,
			InstanceTypeInvalid = 8,
			LocalInvokeThrewException = 9,
			TargetObjectNotInPlayerInterest = 10,
			InvalidTargetBehaviour = 11,
			InvalidRpcKey = 12
		}

		internal struct TimeFeedback
		{
			public const double SUSPEND_THRESHOLD = 1.0;

			private const int ACCURACY = 256;

			public float OffsetAvg;

			public float OffsetDev;

			public float RecvDeltaAvg;

			public float RecvDeltaDev;

			public TimeFeedback(SimulationConnection sc)
			{
				OffsetAvg = (float)sc._clientOffset.Smoothed(0.5);
				OffsetDev = (float)sc._clientOffset.MedianAbsDev;
				RecvDeltaAvg = (float)sc._packetRecvDelta.Smoothed(0.5);
				RecvDeltaDev = (float)sc._packetRecvDelta.MedianAbsDev;
			}

			public TimeFeedback(double offsetAvg, double offsetDev, double recvDeltaAvg, double recvDeltaDev)
			{
				OffsetAvg = (float)offsetAvg;
				OffsetDev = (float)offsetDev;
				RecvDeltaAvg = (float)recvDeltaAvg;
				RecvDeltaDev = (float)recvDeltaDev;
			}

			public void Read(ReadBuffer read)
			{
				OffsetAvg = FloatUtils.Decompress(read.IntVar(), 256f);
				OffsetDev = FloatUtils.Decompress(read.IntVar(), 256f);
				RecvDeltaAvg = FloatUtils.Decompress(read.IntVar(), 256f);
				RecvDeltaDev = FloatUtils.Decompress(read.IntVar(), 256f);
			}

			public void Write(WriteBuffer write)
			{
				write.IntVar(FloatUtils.Compress(OffsetAvg, 256));
				write.IntVar(FloatUtils.Compress(OffsetDev, 256));
				write.IntVar(FloatUtils.Compress(RecvDeltaAvg, 256));
				write.IntVar(FloatUtils.Compress(RecvDeltaDev, 256));
			}
		}

		internal ref struct RecvContext
		{
			public Simulation Simulation;

			public ReadBuffer Read;

			public SimulationConnection Connection;

			public PlayerRef Player;

			public Tick Tick;

			public int MessageBytes;

			public RecvContext(Simulation simulation)
			{
				Read = null;
				Connection = null;
				Player = default(PlayerRef);
				Tick = default(Tick);
				MessageBytes = 0;
				Simulation = simulation;
			}

			public void Init(SimulationConnection connection, ReadBuffer read)
			{
				Read = read;
				Connection = connection;
				Player = Simulation.Connection2Player(connection);
				Tick.Raw = read.Int();
				MessageBytes = read.Int();
			}

			public void Done()
			{
				Connection = null;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public readonly bool IsValidSenderForStateAuthority(PlayerRef stateAuthority)
			{
				Assert.Check(Simulation.IsClient, "Simulation.IsClient");
				return Simulation.IsClient;
			}
		}

		internal ref struct SendContext : IDisposable
		{
			public readonly Simulation Simulation;

			public readonly WriteBuffer Write;

			private SimulationPacketEnvelope _envelope;

			public Tick Tick;

			public int MessageBytes;

			public PlayerRef Player;

			public SimulationConnection Connection;

			public SendContext(Simulation simulation, WriteBuffer write)
			{
				_envelope = null;
				Tick = default(Tick);
				MessageBytes = 0;
				Player = default(PlayerRef);
				Connection = null;
				Simulation = simulation;
				Write = write;
			}

			public void Dispose()
			{
				if (Simulation.ReleaseReference(_envelope))
				{
					InternalLogStreams.LogTraceSendRecv?.Error("Envelope actually released in the context's dispose");
				}
				_envelope = null;
			}

			public void Init(SimulationConnection connection, Tick tick)
			{
				Assert.Always(Write.Length == 0, "Write.Length == 0");
				Tick = tick;
				Connection = connection;
				Player = Simulation.Connection2Player(connection);
				_envelope = Simulation.AcquireSimulationPacketEnvelope(tick);
				Assert.Check(_envelope.RefCount == 1, "_envelope.RefCount == 1");
				Write.Int(Tick.Raw);
				Write.Int(0);
			}

			public unsafe void Send(bool reliable)
			{
				Assert.Check(Connection != null, "Connection != null");
				Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(Write.GetSpan(4, 4)), MessageBytes);
				Simulation.NetworkSendBuffer(Connection.Connection, Write, _envelope, reliable);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void TrackMessage(SimulationMessagePacketData entry)
			{
				_envelope.Messages.Add(entry);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void TrackObject(NetworkId id, Tick tickSent, NetworkObjectPacketFlags flags)
			{
				_envelope.Objects.Add(new NetworkObjectPacketData
				{
					Flags = flags,
					Id = id,
					ResetTick = tickSent
				});
			}
		}

		internal class Server : Simulation
		{
			private SimulationInput.Buffer _inputBuffer;

			private SimulationInput _inputReadTarget;

			private unsafe NetBitBuffer* _hostMigrationWriteBuffer;

			private const int HostMigrationBufferSize = 65536;

			private const int HostMigrationMaxTransferBufferSize = 32768;

			public override Tick LatestServerTick => _tick;

			public override PlayerRef LocalPlayer => base.IsPlayer ? _callbacks.LocalPlayerRef : PlayerRef.None;

			private Dictionary<NetworkId, NetworkObjectHeaderSnapshot> NetworkObjectMap { get; } = new Dictionary<NetworkId, NetworkObjectHeaderSnapshot>();

			internal unsafe override double GetPlayerRtt(PlayerRef player)
			{
				if (LocalPlayer == player)
				{
					return 0.0;
				}
				if (_playersConnections.TryGetValue(player, out var value))
				{
					return value.Connection->RoundTripTime;
				}
				return 0.0;
			}

			internal Server(SimulationArgs args)
				: base(args)
			{
				_time = new ServerTimeProvider();
				_time.Configure(CreateRuntimeConfiguration());
				if (base.IsPlayer)
				{
					_time.Configure(_projectConfig.TimeSync);
				}
				if (args.ResumeState != null && args.ResumeTick != 0 && args.ResumeNetworkId.IsValid)
				{
					InternalLogStreams.LogTraceHostMigration?.Log($"Received Remote state: Tick={args.ResumeTick}, NetworkId={args.ResumeNetworkId}");
					_time.Reset(0.0, args.ResumeTick);
					_isResume = true;
					_inputTick = (_tick = args.ResumeTick);
					_idCounter = Math.Max(1023u, args.ResumeNetworkId.Raw);
					ReadHostMigrationData(args.ResumeState);
				}
				if (base.IsPlayer)
				{
					_inputBuffer = new SimulationInput.Buffer(Fusion.TickRate.Resolve(_projectConfig.Simulation.TickRateSelection).Client);
				}
			}

			internal unsafe void Disconnect(PlayerRef player, byte[] token)
			{
				if (PlayerValid(player) && _playersConnections.TryGetValue(player, out var value))
				{
					NetPeerGroup.DisconnectInternal(_netPeerGroup, value.Connection, NetDisconnectReason.Requested, token);
				}
			}

			internal unsafe void Disconnect(NetAddress address)
			{
				foreach (SimulationConnection value in _connections.Values)
				{
					if (value.Connection->Address.Equals(address))
					{
						NetPeerGroup.DisconnectInternal(_netPeerGroup, value.Connection, NetDisconnectReason.Requested);
						break;
					}
				}
			}

			internal unsafe void RequestRejoin(PlayerRef player, string sessionID)
			{
				if (PlayerValid(player) && _playersConnections.TryGetValue(player, out var value))
				{
					NetPeerGroup.RequestRejoin(_netPeerGroup, value.Connection, sessionID);
				}
			}

			protected override void AfterSimulation()
			{
				if ((base.Config.ReplicationFeatures & NetworkProjectConfig.ReplicationFeatures.InterestManagement) != NetworkProjectConfig.ReplicationFeatures.InterestManagement)
				{
					return;
				}
				foreach (SimulationConnection value in _connections.Values)
				{
					AOI_UpdateAreaOfInterest(value);
				}
			}

			protected override int BeforeSimulation()
			{
				if (base.IsPlayer && !PlayerValid(LocalPlayer))
				{
					PlayerAdd(LocalPlayer, null);
				}
				return 0;
			}

			protected override void UpdateInterpolationParams()
			{
				_interpFromPrev = _interpFrom;
				_interpToPrev = _interpTo;
				_remoteAlphaPrev = _remoteAlpha;
				_interpFrom = _tick.Next(base.TickStride);
				_interpTo = _tick.Next(base.TickStride);
				_remoteAlpha = 0f;
			}

			protected unsafe override void NetworkDisconnected(NetConnection* connection, NetDisconnectReason reason)
			{
			}

			internal override void RecvPacket(ref RecvContext rc)
			{
				switch (base.Topology)
				{
				case Topologies.ClientServer:
					ReadInput(ref rc);
					break;
				case Topologies.Shared:
					ReadStateTick(ref rc);
					_stateReplicator.RecvPacket(ref rc);
					break;
				}
			}

			protected unsafe override void NetworkConnected(NetConnection* connection)
			{
				if (_globalInterestObjects == null)
				{
					return;
				}
				if (!TryGetSimulationConnectionByIndex(connection->LocalConnectionId.GroupIndex, out var result))
				{
					Assert.AlwaysFail($"Failed to get SimulationConnection for {connection->LocalConnectionId}");
				}
				foreach (NetworkId globalInterestObject in _globalInterestObjects)
				{
					result.GetObjectData(globalInterestObject, create: true, allowFail: true);
				}
			}

			internal override void WritePackets(ref SendContext sc)
			{
				if (_time.IsRunning())
				{
					double num = (double)((int)base.Tick - (int)sc.Connection._latestTickAcknowledged) * base.TickDeltaDouble;
					if (num > 1.0)
					{
						sc.Connection.ResetTimeFeedback();
					}
				}
				new TimeFeedback(sc.Connection).Write(sc.Write);
				_stateReplicator.SendPacket(ref sc);
			}

			private SimulationRuntimeConfig CreateRuntimeConfiguration()
			{
				SimulationRuntimeConfig result = new SimulationRuntimeConfig
				{
					ServerMode = _mode,
					PlayerMaxCount = _config.PlayerCount,
					TickRate = Fusion.TickRate.Resolve(_config.TickRateSelection),
					Topology = _config.Topology
				};
				if (base.IsServer)
				{
					result.HostPlayer = LocalPlayer;
				}
				if (_mode == SimulationModes.Host)
				{
					result.TickRate.Server = result.TickRate.Client;
				}
				return result;
			}

			internal void SpawnRuntimeConfiguration()
			{
				AllocateStruct<SimulationRuntimeConfig>(NetworkId.RuntimeConfig) = CreateRuntimeConfiguration();
			}

			protected override void BeforeUpdate()
			{
				if (!HasObject(NetworkId.RuntimeConfig))
				{
					SpawnRuntimeConfiguration();
				}
			}

			protected override void BeforeFirstTick()
			{
				if (base.Mode == SimulationModes.Host)
				{
					Assert.Check(LocalPlayer.IsRealPlayer, "LocalPlayer.IsRealPlayer");
					TryGetPlayerSimulationData(LocalPlayer, create: true, out var _);
				}
				_callbacks.OnServerStart();
			}

			internal override SimulationInput PollInput(Tick tick, PlayerRef player)
			{
				if (!base.IsPlayer || player != LocalPlayer)
				{
					return null;
				}
				SimulationInput obj = AcquireSimulationInput(_config);
				obj.Player = player;
				obj.Header.Tick = tick;
				int num = Math.Max((int)base.TickPrevious - base.TickStride, 0);
				Tick tickPrevious = base.TickPrevious;
				float localAlphaPrev = _localAlphaPrev;
				Instant instant = _time.Now();
				double num2 = Math.Max(instant.Input - instant.Local, 0.0);
				if (num2 > 0.0)
				{
					double num3 = (double)Maths.Lerp(num, (int)tickPrevious, localAlphaPrev) * base.TickDeltaDouble;
					double num4 = num3 + num2;
					double num5 = num4 / base.TickDeltaDouble;
					Assert.Check(base.HasRuntimeConfig, "HasRuntimeConfig");
					int tickStride = base.TickStride;
					double num6 = (double)num + Math.Floor((num5 - (double)num) / (double)tickStride) * (double)tickStride;
					double num7 = (double)num + Math.Ceiling((num5 - (double)num) / (double)tickStride) * (double)tickStride;
					if (num7 == num6)
					{
						num7 += (double)tickStride;
					}
					double num8 = Maths.Clamp01((num5 - num6) / (num7 - num6));
					obj.Header.InterpFrom = (int)num6;
					obj.Header.InterpTo = (int)num7;
					obj.Header.InterpAlpha = (float)num8;
					obj.Delayed = true;
				}
				else
				{
					obj.Header.InterpFrom = num;
					obj.Header.InterpTo = tickPrevious;
					obj.Header.InterpAlpha = localAlphaPrev;
					obj.Delayed = false;
				}
				_callbacks.InvokeOnInput(obj);
				if (_inputBuffer.Add(obj))
				{
					return obj;
				}
				Release(ref obj);
				return null;
			}

			internal override SimulationInput GetBufferedInput(Tick tick, PlayerRef player)
			{
				if (_config.Topology == Topologies.Shared)
				{
					return null;
				}
				if (!PlayerValid(player))
				{
					return null;
				}
				SimulationConnection result;
				if (base.IsPlayer && player == LocalPlayer)
				{
					if (_inputBuffer.Remove(tick, out var removed))
					{
						if (removed.Delayed)
						{
							removed.Delayed = false;
						}
						return removed;
					}
				}
				else if (TryGetSimulationConnectionForPlayer(player, out result))
				{
					if (result._inputs.Remove(tick, out var removed2, out var timeInserted))
					{
						if (timeInserted.HasValue)
						{
							result.InputReceiveDelta(tick, timeInserted.Value, _updateTime);
						}
					}
					else
					{
						removed2 = AcquireSimulationInput(_config);
						removed2.Player = player;
						removed2.Header.Tick = tick;
						SimulationInputHeader lastUsedInputHeader = result._inputs.GetLastUsedInputHeader();
						int num = Math.Max((int)base.Tick - (int)lastUsedInputHeader.Tick, 0);
						removed2.Header.InterpFrom = (int)lastUsedInputHeader.InterpFrom + num;
						removed2.Header.InterpTo = (int)lastUsedInputHeader.InterpTo + num;
						removed2.Header.InterpAlpha = lastUsedInputHeader.InterpAlpha;
						try
						{
							_callbacks.InvokeOnInputMissing(removed2);
						}
						catch (Exception error)
						{
							InternalLogStreams.LogException?.Log(this, error);
						}
					}
					return removed2;
				}
				return null;
			}

			private unsafe void ReadInput(ref RecvContext rc)
			{
				int offset = rc.Read.Offset;
				SimulationConnection connection = rc.Connection;
				int num = rc.Read.Byte();
				if (num > 0)
				{
					if (base.Config.InputTransferMode == SimulationConfig.InputTransferModes.RedundancyUncompressed)
					{
						short num2 = rc.Read.Short();
						for (int i = 0; i < num; i++)
						{
							SimulationInputHeader simulationInputHeader = rc.Read.Peek<SimulationInputHeader>();
							if (simulationInputHeader.Tick > base.Tick)
							{
								if (_inputReadTarget == null)
								{
									_inputReadTarget = AcquireSimulationInput(_config);
								}
								_inputReadTarget.Player = rc.Player;
								rc.Read.Span(new Span<byte>(_inputReadTarget._ptr, num2 * 4));
								if (!connection._inputs.IsFull && connection._inputs.Add(_inputReadTarget, _updateTime))
								{
									_inputReadTarget = null;
								}
							}
							else
							{
								rc.Read.Skip(num2 * 4);
								if (_tickUpdateTimes.TryGetValue(simulationInputHeader.Tick, out var value))
								{
									connection.InputReceiveDelta(simulationInputHeader.Tick, _updateTime, value);
								}
								else
								{
									connection.InputReceiveDelta(simulationInputHeader.Tick, _updateTime, (double)(int)simulationInputHeader.Tick * base.TickDeltaDouble);
								}
							}
						}
					}
					else
					{
						SimulationInput inputRoot = _inputRoot;
						inputRoot.Clear(_config.InputTotalWordCount);
						for (int j = 0; j < num; j++)
						{
							SimulationInput obj = AcquireSimulationInput(_config);
							obj.Player = rc.Player;
							obj.Serialize(inputRoot, _config, null, rc.Read);
							inputRoot.CopyFrom(obj, _config.InputTotalWordCount);
							if (obj.Header.Tick > base.Tick)
							{
								if (connection._inputs.IsFull)
								{
									Release(ref obj);
								}
								else if (!connection._inputs.Add(obj, _updateTime))
								{
									Release(ref obj);
								}
							}
							else
							{
								if (_tickUpdateTimes.TryGetValue(obj.Header.Tick, out var value2))
								{
									connection.InputReceiveDelta(obj.Header.Tick, _updateTime, value2);
								}
								else
								{
									connection.InputReceiveDelta(obj.Header.Tick, _updateTime, (double)(int)obj.Header.Tick * base.TickDeltaDouble);
								}
								Release(ref obj);
							}
						}
					}
				}
				int num3 = rc.Read.Offset - offset;
				HostProfiler.Counters.InputSizeIn?.Add(num3);
				_fusionStatsManager.AddStat(FusionStatType.InputInBandwidth, num3);
			}

			private void ReadStateTick(ref RecvContext rc)
			{
				SimulationConnection connection = rc.Connection;
				Tick tick = rc.Tick;
				if (_tickUpdateTimes.TryGetValue(tick, out var value))
				{
					connection.InputReceiveDelta(tick, _updateTime, value);
				}
				else
				{
					connection.InputReceiveDelta(tick, _updateTime, (double)(int)tick * base.TickDeltaDouble);
				}
			}

			internal (Dictionary<NetworkId, NetworkObjectHeaderPtr>, Dictionary<NetworkId, List<NetworkId>>) GetResumeObjectHeader()
			{
				Dictionary<NetworkId, NetworkObjectHeaderPtr> dictionary = new Dictionary<NetworkId, NetworkObjectHeaderPtr>();
				Dictionary<NetworkId, List<NetworkId>> dictionary2 = new Dictionary<NetworkId, List<NetworkId>>();
				foreach (KeyValuePair<NetworkId, NetworkObjectHeaderSnapshot> item in NetworkObjectMap)
				{
					NetworkObjectHeader header = item.Value.Header;
					if (!header.Id.IsValid || header.Id.Raw <= 1023)
					{
						continue;
					}
					dictionary.Add(header.Id, item.Value.HeaderPtr);
					if (header.NestingRoot.IsValid)
					{
						if (!dictionary2.TryGetValue(header.NestingRoot, out var value))
						{
							dictionary2.Add(header.NestingRoot, value = new List<NetworkId>());
						}
						value.Add(header.Id);
					}
				}
				return (dictionary, dictionary2);
			}

			internal unsafe void DisposeHostMigration()
			{
				foreach (NetworkObjectHeaderSnapshot value in NetworkObjectMap.Values)
				{
					value?.FreeRaw(_allocator);
				}
				NetworkObjectMap.Clear();
				if (_hostMigrationWriteBuffer != null)
				{
					NetBitBuffer.ReleaseRef(ref _hostMigrationWriteBuffer);
				}
			}

			internal void ClearHostMigrationData()
			{
				NetworkId id = _metaMigration.Head.Id;
				bool flag = true;
				while (_metaMigration.Count > 0 && (flag || _metaMigration.Head.Id != id))
				{
					flag = false;
					NetworkObjectMeta networkObjectMeta = _metaMigration.RemoveFirst();
					networkObjectMeta.ResetMigrationSnapshot();
					_metaMigration.AddLast(networkObjectMeta);
				}
			}

			internal unsafe int WriteHostMigrationData(ref byte[] target, int targetBytes)
			{
				Assert.Check(target.Length <= 32768, "target.Length <= HostMigrationMaxTransferBufferSize");
				Assert.Check(targetBytes <= 32768, "targetBytes <= HostMigrationMaxTransferBufferSize");
				if (_metaMigration.Count == 0 && _metaMigrationRemoved.Count == 0)
				{
					InternalLogStreams.LogTraceHostMigration?.Log("No migration data to write");
					return 0;
				}
				int num = 0;
				if (_hostMigrationWriteBuffer == null)
				{
					_hostMigrationWriteBuffer = NetBitBuffer.Allocate(0, 65536);
				}
				Assert.Always(_hostMigrationWriteBuffer != null, "Failed to write host migration data. Invalid Write Buffer");
				_hostMigrationWriteBuffer->Clear();
				_hostMigrationWriteBuffer->OffsetBits = 0;
				NetworkId id = _metaMigration.Head.Id;
				bool flag = true;
				while (_metaMigration.Count > 0 && (flag || _metaMigration.Head.Id != id) && _hostMigrationWriteBuffer->OffsetBits < targetBytes * 8)
				{
					flag = false;
					int offsetBits = _hostMigrationWriteBuffer->OffsetBits;
					NetworkObjectMeta networkObjectMeta = _metaMigration.RemoveFirst();
					NetworkObjectHeaderSnapshotRef migration = networkObjectMeta.Migration;
					NetworkId.Write(_hostMigrationWriteBuffer, networkObjectMeta.Id);
					_hostMigrationWriteBuffer->WriteInt32(networkObjectMeta.WordCount);
					Span<int> raw = networkObjectMeta.Raw;
					Span<int> raw2 = migration.Raw;
					raw2[0] = raw[0];
					int num2 = 1;
					for (int i = num2; i < networkObjectMeta.WordCount; i++)
					{
						if (raw[i] != raw2[i])
						{
							_hostMigrationWriteBuffer->WriteInt32VarLength(i - num2);
							_hostMigrationWriteBuffer->WriteInt32VarLength(raw[i]);
							raw2[i] = raw[i];
							num2 = i;
						}
					}
					InternalLogStreams.LogTraceHostMigration?.Log($"{networkObjectMeta.Id}/{networkObjectMeta.Migration.Header.Id}: " + $"WordCount={networkObjectMeta.WordCount}/{networkObjectMeta.Migration.Header.WordCount}, " + $"Type={networkObjectMeta.Type}/{networkObjectMeta.Migration.Header.Type}, " + $"CRC={migration.SnapshotCRC}");
					if (num2 == 1)
					{
						_hostMigrationWriteBuffer->OffsetBits = offsetBits;
					}
					else
					{
						_hostMigrationWriteBuffer->WriteInt32VarLength(int.MaxValue);
						InternalLogStreams.LogTraceHostMigration?.Log($"{networkObjectMeta.Id} has changed: End mark at {_hostMigrationWriteBuffer->OffsetBits}");
						num++;
					}
					_metaMigration.AddLast(networkObjectMeta);
				}
				while (_metaMigrationRemoved.Count > 0 && _hostMigrationWriteBuffer->OffsetBits < targetBytes * 8)
				{
					NetworkId networkId = _metaMigrationRemoved.Dequeue();
					NetworkId.Write(_hostMigrationWriteBuffer, networkId);
					_hostMigrationWriteBuffer->WriteInt32(0);
					InternalLogStreams.LogTraceHostMigration?.Log($"{networkId} removed: End mark at {_hostMigrationWriteBuffer->OffsetBits}");
					num++;
				}
				if (num == 0)
				{
					InternalLogStreams.LogTraceHostMigration?.Log($"WriteHostMigrationData: Total {_metaMigration.Count} on storage, no migration data written");
					return 0;
				}
				_hostMigrationWriteBuffer->PadToByteBoundary();
				if (target.Length < _hostMigrationWriteBuffer->OffsetBytes)
				{
					target = new byte[_hostMigrationWriteBuffer->OffsetBytes * 2];
				}
				fixed (byte* destination = target)
				{
					FusionUnsafe.Copy(destination, _hostMigrationWriteBuffer->Data, _hostMigrationWriteBuffer->OffsetBytes);
				}
				InternalLogStreams.LogTraceHostMigration?.Log($"WriteHostMigrationData: Total {_metaMigration.Count} on storage, Changed {num}");
				return _hostMigrationWriteBuffer->OffsetBytes;
			}

			private void ReadHostMigrationData(byte[] data)
			{
				foreach (NetworkId key in NetworkObjectMap.Keys)
				{
					NetworkObjectHeaderSnapshot networkObjectHeaderSnapshot = NetworkObjectMap[key];
					NetworkObjectMap.Remove(key);
					networkObjectHeaderSnapshot.FreeRaw(_allocator);
				}
				NetworkObjectMap.Clear();
				ProcessHostMigrationData(data, NetworkObjectMap, _allocator);
			}

			internal static bool ProcessHostMigrationData(byte[] data, Dictionary<NetworkId, NetworkObjectHeaderSnapshot> networkObjectMap, Allocator allocator = null)
			{
				if (data == null || data.Length == 0)
				{
					return true;
				}
				InternalLogStreams.LogTraceHostMigration?.Log($"ProcessHostMigrationData: {data.Length}");
				NetBitBufferManaged buffer = new NetBitBufferManaged(data);
				int num = 0;
				while (true)
				{
					NetworkId? networkId = NetworkId.TryRead(ref buffer);
					if (!networkId.HasValue)
					{
						break;
					}
					NetworkId value = networkId.Value;
					if (!buffer.CanRead(32))
					{
						InternalLogStreams.LogError?.Log($"HostMigration: {value} missing WordCount");
						return false;
					}
					int num2 = buffer.ReadInt32();
					if (num2 < -32768 || num2 > 32767)
					{
						InternalLogStreams.LogError?.Log($"HostMigration: {value} wordCount {num2}, outside of permitted range of short");
						return false;
					}
					if (num2 == 0)
					{
						bool flag;
						if (flag = networkObjectMap.Remove(value, out var value2))
						{
							if (value2 == null)
							{
								InternalLogStreams.LogError?.Log($"HostMigration: {value}: Found null snapshot on remove?!");
							}
							else
							{
								value2.FreeRaw(allocator);
							}
						}
						InternalLogStreams.LogTraceHostMigration?.Log($"{value}: Removed ({flag}) at {buffer.OffsetBits}");
						num++;
						continue;
					}
					if (num2 < 0)
					{
						InternalLogStreams.LogError?.Log($"HostMigration: {value} wordCount {num2} should be 0 or greater");
						return false;
					}
					InternalLogStreams.LogTraceHostMigration?.Log($"{value}: WordCount {num2}");
					try
					{
						if (!networkObjectMap.TryGetValue(value, out var value3))
						{
							InternalLogStreams.LogTraceHostMigration?.Log($"{value}: not found, creating snapshot");
							value3 = new NetworkObjectHeaderSnapshot();
							value3.AllocRaw(allocator, num2);
							short wordCount = checked((short)num2);
							value3.Header = new NetworkObjectHeader(value, wordCount, 0, default(NetworkObjectTypeId));
							networkObjectMap[value] = value3;
						}
						Span<int> raw = value3.Raw;
						int num3 = 1;
						while (buffer.CanReadUInt32VarLength())
						{
							int num4 = buffer.ReadInt32VarLength();
							if (num4 == int.MaxValue)
							{
								InternalLogStreams.LogTraceHostMigration?.Log($"{value}: End Mark at {buffer.OffsetBits}");
								num++;
								break;
							}
							num3 += num4;
							if (num3 < 0 || num3 >= value3.Header.WordCount)
							{
								InternalLogStreams.LogError?.Log($"HostMigration: {value} WordOffset {num4} exceeds WordCount {value3.Header.WordCount} for {value3.Header.Id}");
								return false;
							}
							if (!buffer.CanReadUInt32VarLength())
							{
								InternalLogStreams.LogError?.Log($"HostMigration: {value}: Not enough data to read data");
								return false;
							}
							int num5 = buffer.ReadInt32VarLength();
							if (num3 < raw.Length)
							{
								raw[num3] = num5;
							}
						}
						InternalLogStreams.LogTraceHostMigration?.Log($"{value3.Header.Id}: " + $"WordCount={value3.Header.WordCount}, " + $"Type={value3.Header.Type}, " + $"CRC={value3.BuildCRC()}");
					}
					catch (Exception error)
					{
						InternalLogStreams.LogError?.Log($"HostMigration: {value}: Failed to read data", error);
						return false;
					}
				}
				InternalLogStreams.LogTraceHostMigration?.Log($"Total {networkObjectMap.Count} on storage, {num} changed. [{buffer.OffsetBits}/{buffer.LengthBits}]");
				return true;
			}
		}

		internal class StateReplicator
		{
			private enum ReadObjectUpdateWordsResult
			{
				Success = 0,
				NoMoreData = 1,
				Error = 2
			}

			internal const int DATA_BLOCK_SIZE = 6;

			internal const int OFFSET_BLOCK_SIZE = 4;

			protected const int HEADER_BLOCK_SIZE = 8;

			protected const int GLOBAL_BLOCK_SIZE = 8;

			private readonly Simulation _simulation;

			private readonly List<NetworkObjectMeta.AreaOfInterestList> _aoiQuery;

			private readonly bool _notUsingAreaOfInterest;

			private readonly SimulationConfig.DataConsistency _dataConsistency;

			private WordList _words;

			private const int POSITION_ACCURACY = 1024;

			private ExceptionLimits Trackers
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return _simulation?.ExceptionLimitTrackers;
				}
			}

			public StateReplicator(Simulation simulation)
			{
				_simulation = simulation;
				_dataConsistency = _simulation._config.ObjectDataConsistency;
				_aoiQuery = new List<NetworkObjectMeta.AreaOfInterestList>();
				_notUsingAreaOfInterest = (_simulation.Config.ReplicationFeatures & NetworkProjectConfig.ReplicationFeatures.InterestManagement) == 0;
			}

			public void SendPacket(ref SendContext sc)
			{
				if (_simulation.Topology == Topologies.Shared || _simulation.Mode != SimulationModes.Client)
				{
					int offset = sc.Write.Offset;
					WriteObjectDestroys(ref sc);
					WriteStructs(ref sc);
					if (_simulation.Config.AreaOfInterestEnabled && _simulation is Server)
					{
						WriteUsingAreaOfInterest(ref sc);
					}
					else
					{
						WriteUsingAllObjects(ref sc);
					}
					HostProfiler.Counters.ObjectsBytesOut?.Add(sc.Write.Offset - offset);
				}
			}

			public void RecvPacket(ref RecvContext rc)
			{
				if (_simulation.Topology != Topologies.Shared && _simulation.Mode != SimulationModes.Client)
				{
					return;
				}
				int offset = rc.Read.Offset;
				ReadObjectDestroys(ref rc);
				int num = 0;
				try
				{
					ReadObjectUpdateWordsResult readObjectUpdateWordsResult;
					NetworkId id;
					uint pluginVersion;
					while ((readObjectUpdateWordsResult = ReadObjectUpdate_GetWords(ref rc, out id, out pluginVersion)) == ReadObjectUpdateWordsResult.Success)
					{
						num++;
						if (!_simulation.TryGetMeta(id, out var meta))
						{
							meta = ReadObjectUpdate_TryCreateNew(ref rc, id);
							if (meta == null)
							{
								InternalLogStreams.LogTraceSendRecv?.Warn(_simulation, $"Received object {id} with {_words.AsSpan.Length}, but failed to create an instance. Ignoring");
							}
							else
							{
								ReadObjectUpdate_ApplyData(ref rc, meta, created: true, pluginVersion);
							}
						}
						else
						{
							ReadObjectUpdate_ApplyData(ref rc, meta, created: false, pluginVersion);
						}
					}
					if (readObjectUpdateWordsResult == ReadObjectUpdateWordsResult.NoMoreData && _simulation.Topology == Topologies.Shared && _simulation._mode == SimulationModes.Client && !rc.Read.IsEOF)
					{
						uint pluginVersion2;
						while ((readObjectUpdateWordsResult = ReadObjectUpdate_GetWords(ref rc, out id, out pluginVersion2)) == ReadObjectUpdateWordsResult.Success)
						{
							num++;
							if (!_simulation.TryGetMeta(id, out var meta2))
							{
								InternalLogStreams.LogTraceSendRecv?.Warn(_simulation, $"Received direct data for object {id}, but the object does not exist. Not going to create.");
							}
							else
							{
								ReadObjectUpdate_ApplyStateAuthDataOverride(ref rc, meta2);
							}
						}
					}
					if (readObjectUpdateWordsResult == ReadObjectUpdateWordsResult.Error)
					{
						InternalLogStreams.LogError?.Log(_simulation, $"Malformed data for {id}");
					}
				}
				catch (Exception)
				{
					throw;
				}
				HostProfiler.Counters.ObjectsIn?.Add(num);
				HostProfiler.Counters.ObjectsBytesIn?.Add(rc.Read.Offset - offset);
				_simulation._fusionStatsManager.AddStat(FusionStatType.WordsReadCount, FusionUnsafe.GetWordCount(rc.Read.Offset - offset, 4));
				_simulation._fusionStatsManager.AddStat(FusionStatType.InObjectUpdates, num);
			}

			private void ReadObjectUpdate_ApplyData(ref RecvContext rc, NetworkObjectMeta meta, bool created, uint pluginVersion)
			{
				Assert.Check(_simulation.IsClient, "_simulation.IsClient");
				if (!_simulation.IsClient)
				{
					return;
				}
				NetworkObjectHeaderSnapshotRef snapshot = meta.NextSnapshot(GetSnapshotTick(ref rc, meta));
				Span<int> raw = snapshot.Raw;
				bool flag = false;
				ChangeTick changeTick = ChangeTick.FromRecvTick(_simulation.Tick);
				Span<(int, int)> asSpan = _words.AsSpan;
				for (int i = 0; i < asSpan.Length; i++)
				{
					var (num, num2) = asSpan[i];
					if (raw[num] != num2)
					{
						flag = true;
						raw[num] = num2;
						if (changeTick > meta.Changes[num])
						{
							meta.Changes[num] = changeTick;
						}
						if (num == 10)
						{
							InternalLogStreams.LogTracePluginVersion?.Log(LogPrefix(meta) + $"received PV: {num2} (main apply) tick {changeTick}");
						}
					}
				}
				if (!flag && !created)
				{
					return;
				}
				if (_simulation.Topology != Topologies.Shared)
				{
					Span<int> behaviourChangedTickArray = meta.GetBehaviourChangedTickArray(snapshot);
					for (int j = 0; j < snapshot.Header.BehaviourCount; j++)
					{
						behaviourChangedTickArray[j] = snapshot.Tick;
					}
				}
				_simulation._fusionStatsManager.AddObjectStat(meta.Id, FusionObjectStatType.InPackets, 1f);
				_simulation._fusionStatsManager.AddObjectStat(meta.Id, FusionObjectStatType.InBandwidth, _words.AsSpan.Length * 4);
				bool? flag2 = null;
				bool? flag3 = null;
				bool flag4 = _simulation.IsStateAuthority(snapshot.Header.StateAuthority, _simulation.LocalPlayer);
				bool flag5 = _simulation.IsStateAuthority(meta.StateAuthority, _simulation.LocalPlayer);
				bool flag6 = _simulation.IsInputAuthority(snapshot.Header.InputAuthority, _simulation.LocalPlayer);
				bool flag7 = _simulation.IsInputAuthority(meta.InputAuthority, _simulation.LocalPlayer);
				if (_simulation.Topology == Topologies.Shared)
				{
					if (!_simulation.TryGetSimulationConnectionForPlayer(_simulation.LocalPlayer, out var result))
					{
						Assert.AlwaysFail("Failed to get the simcon for the local player");
					}
					NetworkObjectConnectionData objectData = result.GetObjectData(meta.Id, create: true);
					bool flag8 = meta.StateAuthority != snapshot.Header.StateAuthority;
					if (flag4)
					{
						snapshot.CopyTo(meta);
						snapshot.CopyTo(meta.Previous);
						snapshot.CopyTo(meta.Shadow);
						result.SetActive(objectData, meta);
						if (!meta.IsStruct && (!flag5 || created))
						{
							flag3 = true;
							_simulation._callbacks.ObjectIsSimulatedChanged(meta.Id, simulated: true);
						}
					}
					else if (!flag4 && flag5)
					{
						meta.Timeline.Clear();
						result.SetIdle(objectData);
						if (!meta.IsStruct)
						{
							snapshot.CopyTo(meta);
							snapshot.CopyTo(meta.Previous);
							flag3 = false;
							_simulation._callbacks.ObjectIsSimulatedChanged(meta.Id, simulated: false);
						}
					}
					else if (flag8)
					{
						flag3 = false;
					}
				}
				else if (snapshot.Header.InputAuthority == _simulation.LocalPlayer && (meta.InputAuthority != _simulation.LocalPlayer || created))
				{
					flag2 = true;
					_simulation._callbacks.ObjectIsSimulatedChanged(meta.Id, simulated: true);
				}
				else if (snapshot.Header.InputAuthority != _simulation.LocalPlayer && meta.InputAuthority == _simulation.LocalPlayer)
				{
					flag2 = false;
					_simulation._callbacks.ObjectIsSimulatedChanged(meta.Id, simulated: false);
				}
				if (created)
				{
					snapshot.CopyTo(meta);
					snapshot.CopyTo(meta.Previous);
					meta.InitialTick = meta.GetMaxBehaviourChangedTick(snapshot);
					if (_simulation.Topology == Topologies.Shared && _simulation.IsStateAuthority(snapshot.Header.StateAuthority, _simulation.LocalPlayer))
					{
						snapshot.CopyTo(meta.Shadow);
					}
				}
				else if (snapshot.Header.Type.IsStruct || !_simulation.IsSimulated(meta))
				{
					snapshot.CopyTo(meta);
				}
				if (created && meta.Type == NetworkObjectTypeId.PlayerData)
				{
					ref PlayerSimulationData structData = ref meta.GetStructData<PlayerSimulationData>();
					Assert.Check(structData.Player.IsRealPlayer, structData.Player);
					_simulation._invokeJoinedLeaveQueue.Enqueue((structData.Player, true));
					if (!_simulation.IsServer)
					{
						_simulation._players.Add(structData.Player);
					}
				}
				_simulation._callbacks.OnObjectChanged(new NetworkObjectChange(meta.Id, (!created) ? NetworkObjectChangeType.Update : NetworkObjectChangeType.Create, rc.Player, _words.AsSpan));
				if ((bool)meta.Instance)
				{
					bool flag9 = meta.PlayerFlags.HasAny(NetworkObjectHeaderPlayerDataFlags.AllInterestFlags);
					bool flag10 = snapshot.Header.PlayerFlags.HasAny(NetworkObjectHeaderPlayerDataFlags.AllInterestFlags);
					bool flag11 = flag9 != flag10;
					bool flag12 = flag5 != flag4 || flag2.HasValue;
					if (flag11 || flag12)
					{
						bool flag13 = (!flag5 && flag4) || (flag2 ?? false);
						bool flag14 = (flag5 && !flag4) || ((!flag2) ?? false);
						bool flag15 = !flag9 && flag10;
						bool flag16 = flag9 && !flag10;
						if (!(flag13 && flag9) && (!flag15 || !(flag5 || flag7)) && !(flag14 && flag10) && (!flag16 || !(flag4 || flag6)))
						{
							if (flag13 || flag15)
							{
								InternalLogStreams.LogTraceAreaOfInterest?.Log(_simulation, $"{meta.Id} entering for {_simulation.LocalPlayer} (replication)");
								if (_simulation.IsClient && _simulation.HasRuntimeConfig)
								{
									meta.Timeline.Clear();
								}
								((ICallbacks)_simulation.Runner).ObjectEnterAOI(_simulation.LocalPlayer, meta.Id);
							}
							else
							{
								InternalLogStreams.LogTraceAreaOfInterest?.Log(_simulation, $"{meta.Id} exiting for {_simulation.LocalPlayer} (replication)");
								((ICallbacks)_simulation.Runner).ObjectExitAOI(_simulation.LocalPlayer, meta.Id);
							}
						}
					}
				}
				meta.PlayerFlags = meta.SnapshotLatest.Header.PlayerFlags;
				if (flag3.HasValue)
				{
					_simulation._callbacks.ObjectStateAuthorityChanged(meta.Id, flag3.Value);
				}
				if (flag2.HasValue)
				{
					_simulation._callbacks.ObjectInputAuthorityChanged(meta.Id, flag2.Value);
				}
			}

			private void ReadObjectUpdate_ApplyStateAuthDataOverride(ref RecvContext rc, NetworkObjectMeta meta)
			{
				Assert.Check(meta.Raw.Length == meta.Shadow.Raw.Length, "meta.Raw.Length == meta.Shadow.Raw.Length");
				Assert.Check(meta.Raw.Length == meta.Changes.Length, "meta.Raw.Length == meta.Changes.Length");
				Tick snapshotTick = GetSnapshotTick(ref rc, meta);
				NetworkObjectHeaderSnapshotRef networkObjectHeaderSnapshotRef;
				NetworkObjectHeaderSnapshotRef networkObjectHeaderSnapshotRef2;
				if (meta.HasSnapshots)
				{
					networkObjectHeaderSnapshotRef = meta.SnapshotLatest;
					if (networkObjectHeaderSnapshotRef.Tick == snapshotTick)
					{
						networkObjectHeaderSnapshotRef2 = meta.SnapshotLatest;
						goto IL_00c0;
					}
				}
				networkObjectHeaderSnapshotRef2 = meta.NextSnapshot(snapshotTick, copyInitialState: false);
				networkObjectHeaderSnapshotRef2.CopyFrom(meta, meta.TypeDescriptor?.PluginAuthorityOffsets);
				goto IL_00c0;
				IL_00c0:
				Span<int> raw = meta.Raw;
				networkObjectHeaderSnapshotRef = meta.Shadow;
				Span<int> raw2 = networkObjectHeaderSnapshotRef.Raw;
				Span<int> raw3 = networkObjectHeaderSnapshotRef2.Raw;
				Span<ChangeTick> changes = meta.Changes;
				bool flag = false;
				ChangeTick changeTick = ChangeTick.FromRecvTick(_simulation.Tick);
				Span<(int, int)> asSpan = _words.AsSpan;
				for (int i = 0; i < asSpan.Length; i++)
				{
					var (num, num2) = asSpan[i];
					if (num >= 0 && num < raw.Length && raw[num] != num2)
					{
						if (num == 10)
						{
							InternalLogStreams.LogTracePluginVersion?.Log(LogPrefix(meta) + $"received PV: {num2} (SA override) tick {changeTick}");
						}
						InternalLogStreams.LogTracePluginVersion?.Log(LogPrefix(meta) + $"received : targetWords[{num}]={raw[num]} -> {num2} (SA override) tick {changeTick}");
						flag = true;
						raw[num] = num2;
						raw2[num] = num2;
						raw3[num] = num2;
						if (changeTick > changes[num])
						{
							changes[num] = changeTick;
						}
					}
				}
				if (!flag)
				{
					return;
				}
				OffsetSet offsetSet = meta.TypeDescriptor?.PluginAuthorityOffsets;
				if (offsetSet != null)
				{
					ReadOnlySpan<int> values = offsetSet.Values;
					for (int j = 0; j < values.Length; j++)
					{
						int index = values[j];
						raw2[index] = (raw[index] = raw3[index]);
					}
				}
				_simulation._callbacks.OnObjectChanged(new NetworkObjectChange(meta.Id, NetworkObjectChangeType.Update, rc.Player, _words.AsSpan));
			}

			private Tick GetSnapshotTick(ref RecvContext rc, NetworkObjectMeta meta)
			{
				Tick result = rc.Tick;
				if (_simulation.Topology == Topologies.Shared)
				{
					int num = 0;
					Span<(int, int)> asSpan = _words.AsSpan;
					for (int i = 0; i < asSpan.Length; i++)
					{
						var (num2, val) = asSpan[i];
						if (num2 >= meta.WordCount - meta.BehaviourCount)
						{
							num = Math.Max(num, val);
						}
					}
					if (num > 0)
					{
						result = num;
					}
				}
				return result;
			}

			private unsafe NetworkObjectMeta ReadObjectUpdate_TryCreateNew(ref RecvContext rc, NetworkId id)
			{
				Span<(int, int)> asSpan = _words.AsSpan;
				NetworkObjectHeader header = default(NetworkObjectHeader);
				Span<int> span = new Span<int>(&header, 20);
				for (int i = 0; i < asSpan.Length; i++)
				{
					(int, int) tuple = asSpan[i];
					if (tuple.Item1 >= 20)
					{
						break;
					}
					span[tuple.Item1] = tuple.Item2;
				}
				header.Id = id;
				if (_simulation.Topology == Topologies.Shared && rc.Connection.ObjectData_IsDestroyUnconfirmedOrPending(header.Id) == true)
				{
					if (_simulation.Runner.HasCustomPlugin && header.WordCount > 0)
					{
						InternalLogStreams.LogTraceSendRecv?.Warn(_simulation, $"Reverting a destroy of {header.Id}; basing this on a received word count");
						return CreateFromIncomingHeader(ref rc, in header);
					}
					if (header.StateAuthority == rc.Player)
					{
						InternalLogStreams.LogTraceSendRecv?.Warn(_simulation, $"Ignoring incoming data for {header.Id}, destroy is pending (player: {rc.Player})");
						InternalLogStreams.LogTraceObject?.Log(_simulation, $"Ignoring incoming data for {header.Id}, destroy is pending (player: {rc.Player})");
						return null;
					}
				}
				return CreateFromIncomingHeader(ref rc, in header);
			}

			private ReadObjectUpdateWordsResult ReadObjectUpdate_GetWords(ref RecvContext rc, out NetworkId id, out uint pluginVersion)
			{
				id = new NetworkId
				{
					Raw = rc.Read.UIntVar()
				};
				if (!id.IsValid)
				{
					pluginVersion = uint.MaxValue;
					return ReadObjectUpdateWordsResult.NoMoreData;
				}
				pluginVersion = uint.MaxValue;
				_words.Clear();
				int num = 0;
				int num2;
				while (true)
				{
					num2 = (int)rc.Read.UIntVar();
					if (num >= num + num2)
					{
						return (num2 != 0) ? ReadObjectUpdateWordsResult.Error : ReadObjectUpdateWordsResult.Success;
					}
					num += num2;
					if (num == 22)
					{
						_words.Add(num, FloatUtils.DecompressStoreAsInt(rc.Read.IntVar()));
						_words.Add(num + 1, FloatUtils.DecompressStoreAsInt(rc.Read.IntVar()));
						_words.Add(num + 2, FloatUtils.DecompressStoreAsInt(rc.Read.IntVar()));
					}
					else if (num == 25)
					{
						Quaternion quaternion = Maths.QuaternionDecompress(rc.Read.UInt());
						_words.Add(num, Maths.FloatAsInt(quaternion.x));
						_words.Add(num + 1, Maths.FloatAsInt(quaternion.y));
						_words.Add(num + 2, Maths.FloatAsInt(quaternion.z));
						_words.Add(num + 3, Maths.FloatAsInt(quaternion.w));
					}
					else
					{
						if (num >= 34)
						{
							break;
						}
						_words.Add(num, rc.Read.IntVar());
					}
				}
				do
				{
					_words.Add(num, rc.Read.IntVar());
					num2 = (int)rc.Read.UIntVar();
				}
				while (num < (num += num2));
				return (num2 != 0) ? ReadObjectUpdateWordsResult.Error : ReadObjectUpdateWordsResult.Success;
			}

			private NetworkObjectMeta CreateFromIncomingHeader(ref RecvContext rc, in NetworkObjectHeader header)
			{
				if (header.WordCount == 0)
				{
					InternalLogStreams.LogTraceSendRecv?.Error(_simulation, $"Received partial header: {header} (player: {rc.Player})");
					return null;
				}
				if (!rc.IsValidSenderForStateAuthority(header.StateAuthority))
				{
					InternalLogStreams.LogWarn?.Log(_simulation, $"Received header with invalid state authority: {header.StateAuthority} (player: {rc.Player})");
					rc.Connection.ObjectData_Destroyed(header.Id, force: true);
					return null;
				}
				RemoteObjectCreateResult remoteObjectCreateResult = _simulation._callbacks.OnRemoteCreate(rc.Player, ref header);
				Assert.Always(remoteObjectCreateResult == RemoteObjectCreateResult.Allowed, "createResult == RemoteObjectCreateResult.Allowed");
				NetworkObjectMeta networkObjectMeta;
				try
				{
					networkObjectMeta = _simulation.AllocateObject(in header);
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Limit(Trackers?.AllocateObjects, rc.Player.PlayerId, out var logCount)?.Log(_simulation, string.Format("{0} caused {1} exceptions [{2}/{3}].", rc.Player, "AllocateObject", logCount, Trackers?.AllocateObjects), error);
					return null;
				}
				networkObjectMeta.LocalFlags = NetworkObjectMetaFlags.None;
				if (_simulation.Topology == Topologies.Shared)
				{
					rc.Connection.ObjectData_CreateConfirmed(networkObjectMeta, force: true);
				}
				return networkObjectMeta;
			}

			public void OnObjectSpawnedLocal(NetworkId id)
			{
				if (_simulation.IsClient)
				{
					Assert.Check(_simulation.Topology == Topologies.Shared, "_simulation.Topology == Topologies.Shared");
					if (_simulation.TryGetSimulationConnectionForPlayer(_simulation.LocalPlayer, out var result))
					{
						result.GetObjectData(id, create: true);
					}
				}
			}

			private void WriteObjectDestroys(ref SendContext sc)
			{
				int num = 0;
				NetworkId id;
				while (sc.Connection.DestroyedNextId(out id))
				{
					sc.TrackObject(id, default(Tick), NetworkObjectPacketFlags.Destroy);
					sc.Write.UIntVar(id.Raw);
					_simulation._callbacks.OnObjectWrite(new NetworkObjectChange(id, NetworkObjectChangeType.Destroy, sc.Player, default(ReadOnlySpan<(int, int)>)));
					num++;
				}
				HostProfiler.Counters.DestroysOut?.Add(num);
				sc.Write.UIntVar(0u);
			}

			private void ReadObjectDestroys(ref RecvContext rc)
			{
				int num = 0;
				uint raw;
				while ((raw = rc.Read.UIntVar()) != 0)
				{
					NetworkId id = new NetworkId(raw);
					_simulation._callbacks.OnObjectChanged(new NetworkObjectChange(id, NetworkObjectChangeType.Destroy, rc.Player, default(ReadOnlySpan<(int, int)>)));
					if (!_simulation._callbacks.OnRemoteDestroy(rc.Player, id))
					{
						_simulation.DestroyStateObject(id);
						num++;
					}
				}
				HostProfiler.Counters.DestroysIn?.Add(num);
			}

			private void WriteUsingAreaOfInterest(ref SendContext sc)
			{
				using (HostProfiler.Markers.WriteUsingAreaOfInterest())
				{
					bool isServer = _simulation.IsServer;
					Topologies topology = _simulation.Topology;
					PlayerRef localPlayer = _simulation.LocalPlayer;
					HashSet<NetworkObjectConnectionData>.Enumerator active = sc.Connection.InterestedObjectList.Active;
					while (active.MoveNext())
					{
						NetworkObjectConnectionData current = active.Current;
						Assert.Check(current, "data");
						if (_simulation.TryGetMeta(current.Id, out var meta) && !meta.Flags.Has(NetworkObjectHeaderFlags.Struct) && HasWriteAuthorityFast(meta, topology, localPlayer, isServer))
						{
							ScanObjectForChanges(meta);
							if (CheckSendPriority(ref sc, meta, current) && WriteObject(ref sc, meta, current))
							{
								NotifyObjectWritten(ref sc, meta);
							}
						}
					}
					sc.Write.UIntVar(0u);
				}
			}

			private void WriteUsingAllObjects(ref SendContext sc)
			{
				using (HostProfiler.Markers.WriteUsingAllObjects())
				{
					bool isServer = _simulation.IsServer;
					Topologies topology = _simulation.Topology;
					PlayerRef localPlayer = _simulation.LocalPlayer;
					foreach (var (networkId2, networkObjectMeta2) in _simulation._metaLookup)
					{
						Assert.Check(networkId2 == networkObjectMeta2.Id, networkId2, networkObjectMeta2.Id);
						if (!networkObjectMeta2.Flags.Has(NetworkObjectHeaderFlags.Struct) && HasWriteAuthorityFast(networkObjectMeta2, topology, localPlayer, isServer))
						{
							ScanObjectForChanges(networkObjectMeta2);
							NetworkObjectConnectionData objectData = sc.Connection.GetObjectData(networkObjectMeta2.Id, create: true);
							if (CheckSendPriority(ref sc, networkObjectMeta2, objectData) && WriteObject(ref sc, networkObjectMeta2, objectData))
							{
								NotifyObjectWritten(ref sc, networkObjectMeta2);
							}
						}
					}
					sc.Write.UIntVar(0u);
				}
			}

			internal bool HasObjectInterest(SimulationConnection sc, NetworkObjectMeta meta)
			{
				if (_notUsingAreaOfInterest)
				{
					return true;
				}
				if (_simulation.IsStateAuthority(meta, sc.Player))
				{
					return true;
				}
				if (!sc.AreaOfInterestHasBeenUpdated)
				{
					return true;
				}
				if (sc.TryGetObjectData(meta.Id, out var data) && data.PlayerFlags.HasAny(NetworkObjectHeaderPlayerDataFlags.AllInterestFlags))
				{
					return sc.InterestedObjectList.IsActive(data);
				}
				return false;
			}

			internal void UpdateChangedStructSet()
			{
				foreach (NetworkId @struct in _simulation._structs)
				{
					if (_simulation.TryGetMeta(@struct, out var meta))
					{
						ScanObjectForChanges(meta);
					}
				}
			}

			private void WriteStructs(ref SendContext sc)
			{
				SimulationConnection connection = sc.Connection;
				foreach (NetworkId @struct in _simulation._structs)
				{
					if (_simulation.TryGetMeta(@struct, out var meta) && HasWriteAuthority(meta))
					{
						Assert.Check(meta.Flags.Has(NetworkObjectHeaderFlags.Struct), "meta.Flags.Has(NetworkObjectHeaderFlags.Struct)");
						NetworkObjectConnectionData objectData = sc.Connection.GetObjectData(meta.Id, create: true);
						if (WriteObject(ref sc, meta, objectData))
						{
							NotifyObjectWritten(ref sc, meta);
						}
					}
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private bool HasWriteAuthority(NetworkObjectMeta meta)
			{
				if (_simulation.IsServer)
				{
					return true;
				}
				return _simulation.IsLocalSimulationStateAuthority(ref meta.Header);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private bool HasWriteAuthorityFast(NetworkObjectMeta meta, Topologies topology, PlayerRef localPlayer, bool isServer)
			{
				if (isServer)
				{
					return true;
				}
				return _simulation.IsLocalSimulationStateAuthorityFast(ref meta.Header, topology, localPlayer, isServer);
			}

			private bool ScanObjectForChanges(NetworkObjectMeta meta)
			{
				if (meta.ScannedTick >= _simulation.Tick)
				{
					return false;
				}
				meta.ScannedTick = _simulation.Tick;
				ChangeTick changeTick = ChangeTick.FromSimulationTick(_simulation.Tick);
				Span<ChangeTick> changes = meta.Changes;
				Span<int> raw = meta.Shadow.Raw;
				Span<int> raw2 = meta.Raw;
				int i = 0;
				uint num = 0u;
				short num2 = meta.WordCount;
				if (_simulation.Topology == Topologies.Shared)
				{
					if (meta.Flags.HasNot(NetworkObjectHeaderFlags.Struct) && BehaviourUtils.IsAlive(meta.Instance))
					{
						NetworkBehaviour[] networkedBehaviours = meta.Instance.NetworkedBehaviours;
						if (networkedBehaviours != null && networkedBehaviours.Length != 0)
						{
							NetworkBehaviour[] networkedBehaviours2 = meta.Instance.NetworkedBehaviours;
							Span<int> behaviourChangedTickArray = meta.BehaviourChangedTickArray;
							Assert.Check(networkedBehaviours2.Length == meta.BehaviourCount, "behaviours.Length == meta.BehaviourCount");
							for (; i < 20; i++)
							{
								if (raw[i] != raw2[i])
								{
									raw[i] = raw2[i];
									changes[i] = changeTick;
									num++;
								}
							}
							OffsetSet offsetSet = meta.TypeDescriptor?.PluginAuthorityOffsets;
							if (offsetSet != null)
							{
								int[] asArray = offsetSet.AsArray;
								foreach (int index in asArray)
								{
									raw[index] = raw2[index];
								}
							}
							if (meta.HasNetworkedState)
							{
								for (int k = 0; k < networkedBehaviours2.Length; k++)
								{
									i = networkedBehaviours2[k].WordOffset;
									int num3 = networkedBehaviours2[k].WordOffset + networkedBehaviours2[k].WordCount;
									Assert.Check(num3 <= num2, "max <= wc");
									for (; i < num3; i++)
									{
										if (raw[i] != raw2[i])
										{
											raw[i] = raw2[i];
											changes[i] = changeTick;
											num++;
											behaviourChangedTickArray[k] = _simulation.Tick;
										}
									}
								}
							}
							else
							{
								i = num2;
							}
						}
					}
				}
				else if (!meta.HasNetworkedState)
				{
					num2 = 20;
				}
				for (; i < num2; i++)
				{
					if (raw[i] != raw2[i])
					{
						raw[i] = raw2[i];
						changes[i] = changeTick;
						num++;
					}
				}
				if (num != 0 && meta.ChangedTick != changeTick)
				{
					meta.SetChangedTick(changeTick, "ScanObjectForChanges");
				}
				if (num == 0)
				{
					return false;
				}
				return true;
			}

			public unsafe void OnPacketLost(NetConnection* c, Tick tick, ReadOnlySpan<NetworkObjectPacketData> objects)
			{
				if (!_simulation.TryGetSimulationConnection(c, out var result))
				{
					return;
				}
				ReadOnlySpan<NetworkObjectPacketData> readOnlySpan = objects;
				for (int i = 0; i < readOnlySpan.Length; i++)
				{
					NetworkObjectPacketData networkObjectPacketData = readOnlySpan[i];
					if (networkObjectPacketData.Flags.Has(NetworkObjectPacketFlags.Destroy))
					{
						result.ObjectData_Destroyed(networkObjectPacketData.Id);
						continue;
					}
					NetworkObjectConnectionData objectData = result.GetObjectData(networkObjectPacketData.Id, create: false);
					if (objectData != null)
					{
						if (objectData.TickSent > networkObjectPacketData.ResetTick)
						{
							objectData.SetTickSent(result, networkObjectPacketData.ResetTick, null, "OnPacketLost");
						}
						if (objectData.TickAcknowledged > networkObjectPacketData.ResetTick)
						{
							objectData.SetTickAck(result, networkObjectPacketData.ResetTick, "OnPacketLost");
						}
					}
				}
			}

			public unsafe void OnPacketDelivered(NetConnection* c, Tick tick, ReadOnlySpan<NetworkObjectPacketData> objects)
			{
				if (!_simulation.TryGetSimulationConnection(c, out var result))
				{
					return;
				}
				if (tick > 0)
				{
					Assert.Check(tick >= result._latestTickAcknowledged, "tick >= sc._latestTickAcknowledged");
					result._latestTickAcknowledged = tick;
				}
				ReadOnlySpan<NetworkObjectPacketData> readOnlySpan = objects;
				for (int i = 0; i < readOnlySpan.Length; i++)
				{
					NetworkObjectPacketData networkObjectPacketData = readOnlySpan[i];
					NetworkObjectConnectionData objectData = result.GetObjectData(networkObjectPacketData.Id, create: false);
					if (networkObjectPacketData.Flags.Has(NetworkObjectPacketFlags.Destroy))
					{
						NetworkObjectConnectionDataStatus? networkObjectConnectionDataStatus = objectData?.Status;
						if (networkObjectConnectionDataStatus == NetworkObjectConnectionDataStatus.DestroyPending)
						{
							result.ObjectData_Remove(result, networkObjectPacketData.Id);
						}
						else
						{
							InternalLogStreams.LogTraceSendRecv?.Warn(_simulation, $"Did not remove obj data: {networkObjectPacketData.Id}, because the status was: {networkObjectConnectionDataStatus}");
						}
					}
					else
					{
						if (objectData == null)
						{
							continue;
						}
						objectData.SetTickAck(result, Math.Max(objectData.TickAcknowledged, tick), "OnPacketDelivered");
						if (objectData.Status <= NetworkObjectConnectionDataStatus.CreatedConfirmed)
						{
							objectData.SetStatus(result, NetworkObjectConnectionDataStatus.CreatedConfirmed, "OnPacketDelivered");
							if (!objectData.PlayerFlags.HasAny(NetworkObjectHeaderPlayerDataFlags.AllInterestFlags) && networkObjectPacketData.ResetTick >= objectData.TickMin && _simulation.TryGetMeta(networkObjectPacketData.Id, out var _))
							{
								result.SetIdle(objectData);
							}
						}
					}
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private ChangeTick GetWriteBaseTick(NetworkObjectConnectionData data)
			{
				return ChangeTick.FromSimulationTick((_dataConsistency == SimulationConfig.DataConsistency.Eventual) ? data.TickSent : data.TickAcknowledged);
			}

			private bool CheckSendPriority(ref SendContext sc, NetworkObjectMeta meta, NetworkObjectConnectionData data)
			{
				int priorityLevel = data.PriorityLevel;
				if (priorityLevel <= 0 || sc.Connection.SendCounter % priorityLevel != 0)
				{
					return false;
				}
				return true;
			}

			private bool WriteObject(ref SendContext sc, NetworkObjectMeta meta, NetworkObjectConnectionData data)
			{
				ChangeTick writeBaseTick = GetWriteBaseTick(data);
				ChangeTick changeTick = ChangeTick.Max(meta.ChangedTick, data.PlayerFlagsChangeTick);
				if ((changeTick == default(ChangeTick)) ? data.Flags.Has(NetworkObjectConnectionDataFlags.HasBeenScannedForChangesAtLeastOnce) : (changeTick <= writeBaseTick))
				{
					return false;
				}
				bool flag = WriteObject_WriteData(ref sc, meta, data, writeBaseTick);
				Tick tickSent = data.TickSent;
				data.SetTickSent(sc.Connection, _simulation.Tick, null, "WriteObject");
				data.Flags |= NetworkObjectConnectionDataFlags.HasBeenScannedForChangesAtLeastOnce;
				if (!flag)
				{
					return false;
				}
				sc.TrackObject(meta.Id, tickSent, (NetworkObjectPacketFlags)0);
				if (_simulation._config.Topology == Topologies.Shared && BehaviourUtils.IsAlive(meta.Instance) && meta.Instance.HasStateAuthority)
				{
					meta.NextSnapshot(_simulation.Tick, copyInitialState: false).CopyFrom(meta, meta.TypeDescriptor?.PluginAuthorityOffsets);
					meta.AddLatestSnapshotToTimeline();
				}
				return true;
			}

			private bool WriteObject_WriteData(ref SendContext sc, NetworkObjectMeta meta, [JetBrains.Annotations.NotNull] NetworkObjectConnectionData data, ChangeTick baseTick)
			{
				bool flag = _simulation is Server;
				bool flag2 = flag;
				_words.Clear();
				WriteBuffer.ResetPoint resetPoint = sc.Write.GetResetPoint();
				sc.Write.UIntVar(meta.Id.Raw);
				if (_simulation.Topology == Topologies.Shared)
				{
					sc.Write.UIntVar(meta.Header.PluginVersion);
				}
				Span<int> raw = meta.Raw;
				Span<ChangeTick> changes = meta.Changes;
				short num = meta.WordCount;
				int i = 1;
				int p = 0;
				if (flag2)
				{
					meta.Header.PlayerFlags = data.PlayerFlags;
					changes[9] = data.PlayerFlagsChangeTick;
				}
				bool flag3 = false;
				if (!meta.HasNetworkedState)
				{
					num = 20;
				}
				else if (flag && data.Filter != ulong.MaxValue && (bool)meta.Instance && meta.Instance.NetworkedBehaviours != null)
				{
					for (; i < 20; i++)
					{
						if (changes[i] > baseTick)
						{
							WriteWord(sc.Write, raw, ref i, ref p);
						}
					}
					ulong filter = data.Filter;
					NetworkBehaviour[] networkedBehaviours = meta.Instance.NetworkedBehaviours;
					for (int j = 0; j < networkedBehaviours.Length; j++)
					{
						int num2 = networkedBehaviours[j].WordOffset + networkedBehaviours[j].WordCount;
						if ((filter & (ulong)(1L << networkedBehaviours[j].ObjectIndex)) == 0)
						{
							i = num2;
							continue;
						}
						for (i = networkedBehaviours[j].WordOffset; i < num2; i++)
						{
							if (changes[i] > baseTick)
							{
								WriteWord(sc.Write, raw, ref i, ref p);
							}
						}
					}
					Assert.Check(p <= num, p, num);
				}
				for (; i < num; i++)
				{
					if (changes[i] > baseTick)
					{
						WriteWord(sc.Write, raw, ref i, ref p);
					}
				}
				if (flag2)
				{
					meta.Header.PlayerFlags = (NetworkObjectHeaderPlayerDataFlags)0;
					changes[9] = default(ChangeTick);
				}
				InternalLogStreams.LogTracePluginVersion?.If(changes[10] > baseTick)?.Log($"NOT sending PV back to plugin. changes: {changes[10]} > base: {baseTick}. changed: {meta.ChangedTick}, player: {data.PlayerFlagsChangeTick}");
				Assert.Check(p <= num, p, num);
				if (p == 0)
				{
					resetPoint.Use();
					return false;
				}
				sc.Write.UIntVar(0u);
				return true;
			}

			private void NotifyObjectWritten(ref SendContext sc, NetworkObjectMeta meta)
			{
				_simulation._fusionStatsManager.AddStat(FusionStatType.OutObjectUpdates, 1f);
				_simulation._fusionStatsManager.AddObjectStat(meta.Id, FusionObjectStatType.OutPackets, 1f);
				_simulation._fusionStatsManager.AddObjectStat(meta.Id, FusionObjectStatType.OutBandwidth, _words.AsSpan.Length * 4);
				_simulation._fusionStatsManager.AddStat(FusionStatType.WordsWrittenCount, _words.Count);
				HostProfiler.Counters.ObjectsOut?.Add(1);
				_simulation._callbacks.OnObjectWrite(new NetworkObjectChange(meta.Id, NetworkObjectChangeType.Update, sc.Player, _words.AsSpan));
			}

			private void WriteWord(WriteBuffer writer, Span<int> ptr, ref int w, ref int p)
			{
				Assert.Check(w > p, "w > p");
				switch (w)
				{
				case 24:
					w -= 2;
					goto case 22;
				case 23:
					w--;
					goto case 22;
				case 22:
				{
					int num = w;
					Vector3 vector = ptr.Slice(num, ptr.Length - num).Read<Vector3>();
					writer.UIntVar((uint)(w - p));
					writer.IntVar(FloatUtils.Compress(vector.x));
					writer.IntVar(FloatUtils.Compress(vector.y));
					writer.IntVar(FloatUtils.Compress(vector.z));
					p = w;
					_words.Add(w, ptr[w]);
					_words.Add(w + 1, ptr[w + 1]);
					_words.Add(w + 2, ptr[w + 2]);
					w += 2;
					break;
				}
				case 28:
					w -= 3;
					goto case 25;
				case 27:
					w -= 2;
					goto case 25;
				case 26:
					w--;
					goto case 25;
				case 25:
				{
					writer.UIntVar((uint)(w - p));
					int num = w;
					writer.UInt(Maths.QuaternionCompress(ptr.Slice(num, ptr.Length - num).Read<Quaternion>()));
					p = w;
					_words.Add(w, ptr[w]);
					_words.Add(w + 1, ptr[w + 1]);
					_words.Add(w + 2, ptr[w + 2]);
					_words.Add(w + 3, ptr[w + 3]);
					w += 3;
					break;
				}
				case 10:
					break;
				default:
					writer.UIntVar((uint)(w - p));
					writer.IntVar(ptr[w]);
					_words.Add(w, ptr[w]);
					p = w;
					break;
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string LogPrefix(Simulation simulation, NetworkId id)
			{
				return $"Sim:{simulation.GetHashCode()}: {simulation.Tick}: {id} ";
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private string LogPrefix(NetworkObjectMeta meta)
			{
				Simulation simulation = _simulation;
				NetworkId? networkId = meta?.Id;
				NetworkId networkId2 = default(NetworkId);
				return LogPrefix(simulation, networkId ?? networkId2);
			}
		}

		private Dictionary<int, AreaOfInterestCell> _aoiCells = new Dictionary<int, AreaOfInterestCell>();

		private Dictionary<int, HashSet<int>> _aoiConnections = new Dictionary<int, HashSet<int>>();

		internal AreaOfInterestState AreaOfInterestInstance;

		private ulong _interpolateSequence;

		private bool _isShutdown;

		private bool _isWaitingForShutdown;

		internal NetworkRunner Runner;

		private ICallbacks _callbacks;

		private Tick _tick;

		private Tick _inputTick;

		private SimulationModes _mode;

		private SimulationStages _stage;

		private SimulationConfig _config;

		private NetworkProjectConfig _projectConfig;

		internal ITimeProvider _time;

		private Tick _interpTo;

		private Tick _interpFrom;

		private float _remoteAlpha;

		private float _localAlpha;

		private Tick _interpToPrev;

		private Tick _interpFromPrev;

		private float _remoteAlphaPrev;

		private float _localAlphaPrev;

		private SimulationInput _inputRoot;

		private SimulationInputCollection _inputCollection;

		private StateReplicator _stateReplicator;

		internal Dictionary<int, SimulationConnection> _connections = new Dictionary<int, SimulationConnection>();

		internal Dictionary<PlayerRef, SimulationConnection> _playersConnections = new Dictionary<PlayerRef, SimulationConnection>(PlayerRef.Comparer);

		private HashSet<PlayerRef> _players;

		private double _updateTime;

		private bool _isResume;

		private Tick _sendTick;

		private bool _isLastTick;

		private bool _isFirstTick;

		private bool _isResimulation;

		private bool _isInTick;

		private bool _isInitialLocalTick;

		private const int SendReliableFragmentThreshold = 8;

		private int _continuousFragmentLoss = 0;

		private bool? _isPaused;

		internal FusionStatisticsManager _fusionStatsManager;

		private Dictionary<Tick, double> _tickUpdateTimes;

		private Queue<(PlayerRef, bool)> _invokeJoinedLeaveQueue = new Queue<(PlayerRef, bool)>();

		private HashSet<NetworkId> _globalInterestObjects;

		private Dictionary<ulong, PlayerRefMapping> _uniqueIdPlayerRefMapping = new Dictionary<ulong, PlayerRefMapping>();

		private WriteBuffer _writer = new WriteBuffer();

		internal readonly ExceptionLimits ExceptionLimitTrackers = new ExceptionLimits();

		private readonly Dictionary<nuint, SimulationPacketEnvelope> _simulationPacketDictionary = new Dictionary<UIntPtr, SimulationPacketEnvelope>();

		private uint _nextSimulationPacketId;

		internal INetSocket _netSocket;

		internal unsafe NetPeer* _netPeer;

		private unsafe NetPeerGroup* _netPeerGroup;

		private System.Random _netPeerRng;

		private List<IntPtr> _deliverListCache;

		private ReadBuffer _readBuffer = new ReadBuffer();

		private uint _idCounter = 1023u;

		private Dictionary<NetworkId, NetworkObjectMeta> _metaLookup;

		private Dictionary<PlayerRef, NetworkId> _playerDataLookup;

		private Dictionary<PlayerRef, NetworkId> _playerLeftTempObjectCache;

		private HashSet<NetworkId> _structs;

		private int _structsVersion = 1;

		private Allocator _allocator;

		private Allocator _allocatorObjects;

		private readonly ObjectPool<SimulationMessage> _poolSimulationMessage = new ObjectPool<SimulationMessage>();

		private readonly ObjectPool<SimulationPacketEnvelope> _poolSimulationPacketEnvelope = new ObjectPool<SimulationPacketEnvelope>();

		private readonly ObjectPool<NetworkObjectMeta> _poolNetworkObjectMeta = new ObjectPool<NetworkObjectMeta>();

		private readonly ObjectPool<NetworkObjectHeaderSnapshot> _poolNetworkObjectHeaderSnapshot = new ObjectPool<NetworkObjectHeaderSnapshot>();

		private readonly ObjectPool<AreaOfInterestCell> _poolAreaOfInterestCell = new ObjectPool<AreaOfInterestCell>();

		private readonly ObjectPool<SimulationInput> _poolSimulationInput = new ObjectPool<SimulationInput>();

		private readonly ArrayPool<byte> _poolByte = ArrayPool<byte>.Create();

		private int _poolByteCount;

		private readonly ArrayPool<NetworkObjectPacketData> _poolNetworkObjectPacketData = ArrayPool<NetworkObjectPacketData>.Create();

		private int _poolNetworkObjectPacketDataCount;

		private readonly ArrayPool<SimulationMessagePacketData> _poolSimulationMessagePacketData = ArrayPool<SimulationMessagePacketData>.Create();

		private int _poolSimulationMessagePacketDataCount;

		private readonly ArrayPool<SimulationConnection> _poolSimulationConnection = ArrayPool<SimulationConnection>.Create();

		private int _poolSimulationConnectionCount;

		private int _allocatedObjectDataBytes;

		private int _allocatedObjectChangesBytes;

		private int _allocatedObjectSnapshotBytes;

		private int _allocatedInputDataBytes;

		private Queue<SimulationMessagePacketData> _pendingMessages = new Queue<SimulationMessagePacketData>(16);

		private byte[] _largeRpcWriteBuffer = null;

		private readonly Dictionary<NetworkObjectTypeId, NetworkObjectMeta> _metaSceneLookup = new Dictionary<NetworkObjectTypeId, NetworkObjectMeta>(NetworkObjectTypeId.Comparer);

		private NetworkObjectMeta.MigrationList _metaMigration;

		private readonly Queue<NetworkId> _metaMigrationRemoved = new Queue<NetworkId>();

		public abstract Tick LatestServerTick { get; }

		public double LatestServerTime => (double)(int)LatestServerTick * TickDeltaDouble;

		internal SimulationRuntimeConfig RuntimeConfig => RuntimeConfigRef;

		internal ref SimulationRuntimeConfig RuntimeConfigRef
		{
			get
			{
				if (TryGetStruct(NetworkId.RuntimeConfig, out var meta))
				{
					return ref meta.GetStructData<SimulationRuntimeConfig>();
				}
				throw new InvalidOperationException("RuntimeConfig can only be read after the first state update form the server has arrived, add a guard check with Simulation.HasObject(NetworkId.RuntimeConfig)");
			}
		}

		internal ulong InterpolateSequence => _interpolateSequence;

		internal bool HasRuntimeConfig
		{
			get
			{
				NetworkObjectMeta meta;
				return TryGetStruct(NetworkId.RuntimeConfig, out meta);
			}
		}

		public int TickStride => IsServer ? RuntimeConfig.TickRate.ServerTickStride : RuntimeConfig.TickRate.ClientTickStride;

		public int TickRate => RuntimeConfig.TickRate.Client;

		public double TickDeltaDouble => RuntimeConfig.TickRate.ClientTickDelta;

		public float TickDeltaFloat => (float)TickDeltaDouble;

		public int SendRate => IsServer ? RuntimeConfig.TickRate.ServerSend : RuntimeConfig.TickRate.ClientSend;

		public double SendDelta => IsServer ? RuntimeConfig.TickRate.ServerSendDelta : RuntimeConfig.TickRate.ClientSendDelta;

		public float DeltaTime => IsServer ? ((float)RuntimeConfig.TickRate.ServerTickDelta) : ((float)RuntimeConfig.TickRate.ClientTickDelta);

		public bool IsShutdown => _isShutdown;

		public float LocalAlpha => _localAlpha;

		public bool IsResimulation => _isResimulation;

		public bool IsLastTick => _isLastTick;

		public bool IsFirstTick => _isFirstTick;

		public bool IsForward => !_isResimulation;

		public bool IsLocalPlayerFirstExecution => _stage == SimulationStages.Forward;

		public Tick InputTick
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _inputTick;
			}
		}

		public Tick Tick => _tick;

		public Tick TickPrevious => Math.Max(0, (int)_tick - TickStride);

		public double Time => (double)(int)_tick * TickDeltaDouble;

		public int InputCount => _inputCollection.Count;

		public Topologies Topology => _config.Topology;

		public SimulationModes Mode => _mode;

		public SimulationStages Stage => _stage;

		public SimulationConfig Config => _config;

		public NetworkProjectConfig ProjectConfig => _projectConfig;

		public float RemoteAlpha => _remoteAlpha;

		public Tick RemoteTickPrevious => _interpFrom;

		public Tick RemoteTick => _interpTo;

		public bool IsClient
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this is Client;
			}
		}

		public bool IsServer
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this is Server;
			}
		}

		public bool IsPlayer => _mode == SimulationModes.Client || _mode == SimulationModes.Host;

		public bool IsSinglePlayer => _mode == SimulationModes.Host && _config.PlayerCount == 1;

		public bool IsMasterClient => _callbacks.IsSharedModeMasterClient;

		public virtual IEnumerable<PlayerRef> ActivePlayers
		{
			get
			{
				if (IsPlayer)
				{
					yield return LocalPlayer;
				}
				if (!IsServer)
				{
					yield break;
				}
				foreach (SimulationConnection value in _connections.Values)
				{
					yield return Connection2Player(value);
				}
			}
		}

		public bool IsRunning => !_isShutdown;

		internal StateReplicator Replicator => _stateReplicator;

		internal ICallbacks Callbacks => _callbacks;

		internal bool IsResume => _isResume;

		internal bool IsInTick => _isInTick;

		internal bool IsPaused => _isPaused.HasValue && _isPaused.Value;

		internal bool IsWaitingForTheInitialTick => _isInitialLocalTick;

		internal IEnumerable<SimulationConnection> Connections
		{
			get
			{
				if (!IsServer)
				{
					yield break;
				}
				foreach (SimulationConnection value in _connections.Values)
				{
					yield return value;
				}
			}
		}

		public unsafe NetAddress LocalAddress => _netPeer->Address;

		public unsafe NetConfig* NetConfigPointer => NetPeer.GetConfigPointer(_netPeer);

		public abstract PlayerRef LocalPlayer { get; }

		internal unsafe int ReliableDataSendRate
		{
			get
			{
				return (_netPeerGroup != null && _netPeerGroup->ReliableSendInterval != 0.0) ? ((int)(1.0 / _netPeerGroup->ReliableSendInterval)) : 0;
			}
			set
			{
				if (_netPeerGroup != null)
				{
					int sendRate = SendRate;
					if (value < 1)
					{
						InternalLogStreams.LogDebug?.Warn(this, $"Reliable Data Send Rate of {value}hz is too low, setting to {1}hz");
						value = 1;
					}
					if (value > sendRate)
					{
						InternalLogStreams.LogDebug?.Warn(this, $"Reliable Data Send Rate of {value}hz is too high, setting to {sendRate}hz");
						value = sendRate;
					}
					_netPeerGroup->ReliableSendInterval = 1f / (float)value;
					InternalLogStreams.LogDebug?.Log(this, $"Reliable Data Send Rate set to {value}hz");
				}
			}
		}

		List<IntPtr> INetPeerGroupCallbacks.DeliverList => _deliverListCache ?? (_deliverListCache = new List<IntPtr>());

		ReadBuffer INetPeerGroupCallbacks.ReadBuffer => _readBuffer;

		internal uint IdCounter => _idCounter;

		public int ObjectCount => _metaLookup.Count;

		public Dictionary<NetworkId, NetworkObjectMeta> Objects => _metaLookup;

		private bool IsForwardingUnregisteredRpcsEnabled => false;

		internal int MetaMigrationCount => _metaMigration.Count;

		public void GetAreaOfInterestGizmoData(List<(Vector3 center, Vector3 size, int playerCount, int objectCount)> result)
		{
			result.Clear();
			if (Runner.IsClient && Runner.Topology == Topologies.Shared && TryGetSimulationConnectionForPlayer(LocalPlayer, out var result2))
			{
				foreach (int areaOfInterestCell in result2.AreaOfInterestCells)
				{
					Vector3 item = AreaOfInterestInstance.ToCellCenter(areaOfInterestCell);
					Vector3 item2 = Vector3.one * AreaOfInterestInstance.GetCellSize();
					result.Add((item, item2, 0, 0));
				}
				return;
			}
			foreach (KeyValuePair<int, AreaOfInterestCell> aoiCell in _aoiCells)
			{
				Vector3 item3 = AreaOfInterestInstance.ToCellCenter(aoiCell.Key);
				Vector3 item4 = Vector3.one * AreaOfInterestInstance.GetCellSize();
				result.Add((item3, item4, aoiCell.Value.Connections.GetSetCount(), aoiCell.Value.Objects.Count));
			}
		}

		public List<NetworkId> GetObjectsInAreaOfInterestForPlayer(PlayerRef player)
		{
			List<NetworkId> list = new List<NetworkId>();
			if (!Runner.IsServer || _config.ReplicationFeatures != NetworkProjectConfig.ReplicationFeatures.InterestManagement)
			{
				return list;
			}
			if (!player.IsRealPlayer || player.IsNone)
			{
				InternalLogStreams.LogDebug?.Error($"Player {player} is not valid.");
				return list;
			}
			if (!TryGetSimulationConnectionForPlayer(player, out var result))
			{
				return list;
			}
			foreach (int areaOfInterestCell2 in result.AreaOfInterestCells)
			{
				AreaOfInterestCell areaOfInterestCell = AOI_GetCell(areaOfInterestCell2, create: false);
				if (areaOfInterestCell != null)
				{
					for (NetworkObjectMeta networkObjectMeta = areaOfInterestCell.Objects.Head; networkObjectMeta != null; networkObjectMeta = areaOfInterestCell.Objects.Next(networkObjectMeta))
					{
						list.Add(networkObjectMeta.Id);
					}
				}
			}
			return list;
		}

		public void GetObjectsAndPlayersInAreaOfInterestCell(int cellKey, List<PlayerRef> players, List<NetworkId> objects)
		{
			players.Clear();
			objects.Clear();
			AreaOfInterestCell areaOfInterestCell = AOI_GetCell(cellKey, create: false);
			if (areaOfInterestCell == null)
			{
				return;
			}
			for (NetworkObjectMeta networkObjectMeta = areaOfInterestCell.Objects.Head; networkObjectMeta != null; networkObjectMeta = areaOfInterestCell.Objects.Next(networkObjectMeta))
			{
				objects.Add(networkObjectMeta.Id);
			}
			if (areaOfInterestCell.Connections.Empty())
			{
				return;
			}
			BitSet512.Iterator iterator = areaOfInterestCell.Connections.GetIterator();
			int index;
			while (iterator.Next(out index))
			{
				if (TryGetSimulationConnectionByIndex(index, out var result))
				{
					players.Add(result.Player);
				}
			}
		}

		private AreaOfInterestCell AOI_GetCell(int cellKey, bool create)
		{
			Assert.Check(cellKey > 0, "cellKey > 0");
			if (!_aoiCells.TryGetValue(cellKey, out var value))
			{
				if (!create)
				{
					return null;
				}
				value = AcquireAreaOfInterestCell(cellKey);
				_aoiCells.Add(cellKey, value);
			}
			Assert.Check(value.Key == cellKey, value.Key, cellKey);
			return value;
		}

		private void AOI_ReleaseCell(AreaOfInterestCell cell)
		{
			if (cell.Empty)
			{
				_aoiCells.Remove(cell.Key);
				Release(ref cell);
			}
		}

		internal void AOI_RemoveConnection(SimulationConnection sc)
		{
			int connectionIndex = sc.ConnectionIndex;
			if (!_aoiConnections.TryGetValue(connectionIndex, out var value))
			{
				return;
			}
			foreach (int item in value)
			{
				AreaOfInterestCell areaOfInterestCell = AOI_GetCell(item, create: false);
				Assert.Check(areaOfInterestCell != null, "cell != null");
				areaOfInterestCell.Connections.Clear(connectionIndex);
				if (areaOfInterestCell.Empty)
				{
					AOI_ReleaseCell(areaOfInterestCell);
				}
			}
			value.Clear();
		}

		internal void AOI_UpdateAreaOfInterest(SimulationConnection sc)
		{
			if (sc?.AreaOfInterestCells == null || (!sc.AreaOfInterestHasBeenUpdated && sc.AreaOfInterestCells.Count == 0))
			{
				return;
			}
			int connectionIndex = sc.ConnectionIndex;
			if (!_aoiConnections.TryGetValue(connectionIndex, out var value))
			{
				_aoiConnections.Add(connectionIndex, value = new HashSet<int>());
			}
			sc.AreaOfInterestHasBeenUpdated = true;
			HashSet<int> areaOfInterestCells = sc.AreaOfInterestCells;
			bool flag = false;
			foreach (int item in value)
			{
				if (areaOfInterestCells.Contains(item))
				{
					continue;
				}
				flag = true;
				AreaOfInterestCell areaOfInterestCell = AOI_GetCell(item, create: false);
				if (areaOfInterestCell != null)
				{
					Assert.Check(areaOfInterestCell.Connections.IsSet(connectionIndex), "cell.Connections.IsSet(ci)");
					areaOfInterestCell.Connections.Clear(connectionIndex);
					NetworkObjectMeta networkObjectMeta = areaOfInterestCell.Objects.Head;
					while (networkObjectMeta != null)
					{
						NetworkObjectMeta networkObjectMeta2 = networkObjectMeta;
						networkObjectMeta = areaOfInterestCell.Objects.Next(networkObjectMeta);
						ExitAreaOfInterest(sc, networkObjectMeta2.Id);
					}
					if (areaOfInterestCell.Empty)
					{
						AOI_ReleaseCell(areaOfInterestCell);
					}
				}
			}
			if (!flag && value.Count == areaOfInterestCells.Count)
			{
				return;
			}
			foreach (int item2 in areaOfInterestCells)
			{
				if (!value.Contains(item2))
				{
					AreaOfInterestCell areaOfInterestCell2 = AOI_GetCell(item2, create: true);
					Assert.Check(!areaOfInterestCell2.Connections.IsSet(connectionIndex), "cell.Connections.IsSet(ci) == false");
					areaOfInterestCell2.Connections.Set(connectionIndex);
					NetworkObjectMeta networkObjectMeta3 = areaOfInterestCell2.Objects.Head;
					while (networkObjectMeta3 != null)
					{
						NetworkObjectMeta networkObjectMeta4 = networkObjectMeta3;
						networkObjectMeta3 = areaOfInterestCell2.Objects.Next(networkObjectMeta3);
						EnterAreaOfInterest(sc, networkObjectMeta4.Id);
					}
				}
			}
			value.Clear();
			value.UnionWith(areaOfInterestCells);
		}

		internal bool AOI_Query(SimulationConnection sc, List<NetworkObjectMeta.AreaOfInterestList> result, bool clearResult)
		{
			if (clearResult)
			{
				result.Clear();
			}
			int count = result.Count;
			if (_aoiConnections.TryGetValue(sc.ConnectionIndex, out var value))
			{
				foreach (int item in value)
				{
					AreaOfInterestCell areaOfInterestCell = AOI_GetCell(item, create: false);
					if (!areaOfInterestCell.Objects.IsEmpty)
					{
						result.Add(areaOfInterestCell.Objects);
					}
				}
			}
			return result.Count > count;
		}

		internal void AOI_RemoveFromAreaOfInterest(NetworkObjectMeta meta, bool invokeExit = false)
		{
			Assert.Check(meta.Id.IsValid, "meta.Id.IsValid");
			if (meta.AreaOfInterestCell == 0)
			{
				return;
			}
			AreaOfInterestCell areaOfInterestCell = AOI_GetCell(meta.AreaOfInterestCell, create: false);
			areaOfInterestCell.Objects.Remove(meta);
			if (invokeExit)
			{
				NetworkId id = meta.Id;
				BitSet512.Iterator iterator = areaOfInterestCell.Connections.GetIterator();
				int index;
				while (iterator.Next(out index))
				{
					ExitAreaOfInterest(index, id);
				}
			}
		}

		internal void AOI_UpdateAreaOfInterest(NetworkObjectMeta meta, int newCellKey)
		{
			if (meta.AreaOfInterestCell == newCellKey)
			{
				return;
			}
			NetworkId id = meta.Id;
			AreaOfInterestCell areaOfInterestCell;
			if (meta.AreaOfInterestCell > 0)
			{
				areaOfInterestCell = AOI_GetCell(meta.AreaOfInterestCell, create: false);
				areaOfInterestCell.Objects.Remove(meta);
				if (areaOfInterestCell.Empty)
				{
					AOI_ReleaseCell(areaOfInterestCell);
					areaOfInterestCell = null;
				}
			}
			else
			{
				areaOfInterestCell = null;
			}
			AreaOfInterestCell areaOfInterestCell2 = AOI_GetCell(newCellKey, create: true);
			meta.AreaOfInterestCell = newCellKey;
			areaOfInterestCell2.Objects.AddLast(meta);
			if (areaOfInterestCell != null)
			{
				if (!areaOfInterestCell.Connections.Equals(areaOfInterestCell2.Connections))
				{
					BitSet512.Iterator iterator = areaOfInterestCell.Connections.GetIterator();
					iterator._set.AndNot(areaOfInterestCell2.Connections);
					int index;
					while (iterator.Next(out index))
					{
						ExitAreaOfInterest(index, id);
					}
					BitSet512.Iterator iterator2 = areaOfInterestCell2.Connections.GetIterator();
					iterator2._set.AndNot(areaOfInterestCell.Connections);
					int index2;
					while (iterator2.Next(out index2))
					{
						EnterAreaOfInterest(index2, id);
					}
				}
			}
			else
			{
				BitSet512.Iterator iterator3 = areaOfInterestCell2.Connections.GetIterator();
				int index3;
				while (iterator3.Next(out index3))
				{
					EnterAreaOfInterest(index3, id);
				}
			}
		}

		internal void EnterAreaOfInterest(int connection, NetworkId id)
		{
			if (!TryGetSimulationConnectionByIndex(connection, out var result))
			{
				InternalLogStreams.LogError?.Log($"Failed to get connection {connection}");
			}
			else
			{
				EnterAreaOfInterest(result, id);
			}
		}

		internal void EnterAreaOfInterest(SimulationConnection connection, NetworkId id)
		{
			Assert.Always(TryGetMeta(id, out var meta), "Object not found");
			Assert.Always(meta.AreaOfInterestCell > 0, "AOI Cell not correct");
			NetworkObjectConnectionData objectData = connection.GetObjectData(id, create: true);
			if (objectData.PlayerFlags.HasNone(NetworkObjectHeaderPlayerDataFlags.AllInterestFlags))
			{
				InternalLogStreams.LogTraceAreaOfInterest?.Log(this, $"{id} entering for {connection.Player} (state auth decision)");
				_callbacks.ObjectEnterAOI(connection, id);
			}
			Assert.Check(objectData.PlayerFlags.HasNot(NetworkObjectHeaderPlayerDataFlags.InAreaOfInterest), "Already has interest in");
			connection.SetActive(objectData, meta);
			objectData.SetPlayerFlag(NetworkObjectHeaderPlayerDataFlags.InAreaOfInterest, this);
		}

		internal void ExitAreaOfInterest(int connection, NetworkId id)
		{
			if (!TryGetSimulationConnectionByIndex(connection, out var result))
			{
				InternalLogStreams.LogError?.Log($"Failed to get connection {connection}");
			}
			else
			{
				ExitAreaOfInterest(result, id);
			}
		}

		internal void ExitAreaOfInterest(SimulationConnection connection, NetworkId id)
		{
			NetworkObjectConnectionData objectData = connection.GetObjectData(id, create: false);
			if (objectData != null)
			{
				if (objectData.PlayerFlags.Has(NetworkObjectHeaderPlayerDataFlags.InAreaOfInterest, NetworkObjectHeaderPlayerDataFlags.AllInterestFlags))
				{
					InternalLogStreams.LogTraceAreaOfInterest?.Log(this, $"{id} exiting for {connection.Player} (state auth decision)");
					_callbacks.ObjectExitAOI(connection, id);
				}
				if (TryGetMeta(id, out var meta) && meta.AreaOfInterestCell > 0)
				{
					connection.SetActive(objectData, meta);
				}
				if (objectData.PlayerFlags.Has(NetworkObjectHeaderPlayerDataFlags.InAreaOfInterest))
				{
					objectData.ClearPlayerFlag(NetworkObjectHeaderPlayerDataFlags.InAreaOfInterest, this);
				}
			}
		}

		internal void InterpolateSequenceIncrement()
		{
			_interpolateSequence++;
		}

		internal abstract double GetPlayerRtt(PlayerRef player);

		internal abstract void RecvPacket(ref RecvContext rc);

		internal abstract void WritePackets(ref SendContext sc);

		internal abstract SimulationInput GetBufferedInput(Tick tick, PlayerRef player);

		internal abstract SimulationInput PollInput(Tick tick, PlayerRef player);

		internal unsafe Simulation(SimulationArgs args)
		{
			Assert.Check(sizeof(NetworkObjectHeader) == 80, "NetworkObjectHeader size != WORDS * REPLICATE_WORD_SIZE");
			_fusionStatsManager = new FusionStatisticsManager(this);
			_mode = args.Mode;
			_config = args.Config.Simulation;
			_projectConfig = args.Config;
			_allocator = Allocator.Create(_projectConfig.Heap.ToAllocatorConfig());
			_allocatorObjects = Allocator.Create(_projectConfig.Heap.ToAllocatorConfig());
			_callbacks = args.Callbacks;
			_isShutdown = false;
			_isWaitingForShutdown = false;
			_isInitialLocalTick = true;
			_inputRoot = AcquireSimulationInput(_config);
			_inputCollection = new SimulationInputCollection(_config.PlayerCount);
			_config.InterestManagementConfig.Validate();
			AreaOfInterestInstance = new AreaOfInterestState(_config.InterestManagementConfig);
			_players = new HashSet<PlayerRef>(PlayerRef.Comparer);
			_metaLookup = new Dictionary<NetworkId, NetworkObjectMeta>(NetworkId.Comparer);
			_playerDataLookup = new Dictionary<PlayerRef, NetworkId>();
			_playerLeftTempObjectCache = new Dictionary<PlayerRef, NetworkId>();
			_structs = new HashSet<NetworkId>(NetworkId.Comparer);
			NetworkInit(args.Socket, args.Address);
			_stateReplicator = new StateReplicator(this);
			if ((Config.ReplicationFeatures & NetworkProjectConfig.ReplicationFeatures.InterestManagement) == NetworkProjectConfig.ReplicationFeatures.InterestManagement)
			{
				_globalInterestObjects = new HashSet<NetworkId>();
			}
			_tickUpdateTimes = new Dictionary<Tick, double>();
			CreateInternalStateObjects();
		}

		internal unsafe PlayerRef Connection2Player(SimulationConnection c)
		{
			Assert.Check(c, "c");
			PlayerRef player = _connections[c.Connection->LocalId.GroupIndex].Player;
			Assert.Check(player == c.Player, "result == c.Player");
			Assert.Check(_connections[c.Connection->LocalId.GroupIndex] == c, "_connections[c.Connection->LocalId.GroupIndex] == c");
			return player;
		}

		internal unsafe virtual PlayerRef Connection2Player(NetConnection* c)
		{
			Assert.Check(c, "c");
			return _connections[c->LocalId.GroupIndex].Player;
		}

		internal virtual int Player2Connection(PlayerRef player)
		{
			if (_playersConnections.TryGetValue(player, out var value))
			{
				return value.ConnectionIndex;
			}
			return -1;
		}

		internal void RegisterUniqueIdPlayerMapping(int actorid, byte[] id, PlayerRef playerRef)
		{
			Assert.Check<byte>(id, "id");
			Assert.Check(id.Length == 8, "id.Length == sizeof(ulong)");
			_uniqueIdPlayerRefMapping[BitConverter.ToUInt64(id, 0)] = new PlayerRefMapping
			{
				ActorId = actorid,
				PlayerRef = playerRef
			};
			InternalLogStreams.LogInfo?.Log(this, $"RegisterUniqueIdPlayerMapping actorid:{actorid} id:{BitConverter.ToUInt64(id, 0)}, player:{playerRef}");
		}

		internal PlayerRefMapping? GetPlayerRefMapping(byte[] id)
		{
			Assert.Check<byte>(id, "id");
			Assert.Check(id.Length == 8, "id.Length == sizeof(ulong)");
			if (_uniqueIdPlayerRefMapping.TryGetValue(BitConverter.ToUInt64(id, 0), out var value))
			{
				return value;
			}
			InternalLogStreams.LogWarn?.Log($"no player mapping for {BitConverter.ToUInt64(id, 0)} exists");
			return null;
		}

		internal unsafe PlayerRefMapping? GetPlayerRefMapping(byte* id)
		{
			Assert.Check(id, "id");
			byte[] array = new byte[8];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = id[i];
			}
			return GetPlayerRefMapping(array);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CalculateUpdateTime()
		{
			if (!IsClient)
			{
				double updateTime = _updateTime;
				_updateTime = _time.Now().Local;
				Assert.Check(_updateTime > updateTime, "Current Update Time must be bigger than previous Update Time {0} {1}", _updateTime, updateTime);
			}
		}

		private void StepSimulation(SimulationStages stage, bool lastTick, bool firstTick, bool freeInput)
		{
			using (HostProfiler.Markers.StepSimulation())
			{
				try
				{
					bool isResimulation = stage == SimulationStages.Resimulate;
					_isLastTick = lastTick;
					_isFirstTick = firstTick;
					_isResimulation = isResimulation;
					if (IsLastTick && !IsResimulation)
					{
						Assert.Check(!IsResimulation, "IsResimulation should be false");
						_callbacks.OnBeforeCopyPreviousState();
						foreach (NetworkObjectMeta value in _metaLookup.Values)
						{
							if (value.IsObject)
							{
								value.Previous.CopyFrom(value);
							}
						}
					}
					_tick = _tick.Next(TickStride);
					if (IsServer)
					{
						_tickUpdateTimes.Remove((int)_tick - TickRate);
						_tickUpdateTimes.Add(_tick, _updateTime);
					}
					InvokeTick(stage, freeInput);
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(this, error);
				}
				finally
				{
					_isLastTick = false;
					_isFirstTick = false;
					_isResimulation = false;
				}
			}
		}

		protected virtual void AfterUpdate()
		{
		}

		protected unsafe virtual void NetworkConnected(NetConnection* connection)
		{
		}

		protected unsafe virtual void NetworkDisconnected(NetConnection* connection, NetDisconnectReason reason)
		{
		}

		protected virtual void NetworkReceiveDone()
		{
		}

		protected virtual void NoSimulation()
		{
		}

		protected virtual void UpdateInterpolationParams()
		{
		}

		protected virtual int BeforeSimulation()
		{
			return 0;
		}

		protected virtual void BeforeFirstTick()
		{
		}

		internal void SinglePlayerSetPaused(bool paused)
		{
			if (IsSinglePlayer)
			{
				_isPaused = paused;
			}
		}

		internal unsafe bool SetPriorityOfObjectForLocalPlayerInSharedMode(NetworkId id, int priority)
		{
			if (Topology != Topologies.Shared)
			{
				return false;
			}
			Assert.Check(sizeof(SimulationMessageInternal_SharedModeSetPriority) == 8, "SharedModeSetPriority unexpected size");
			SimulationMessageInternal_SharedModeSetPriority buffer = default(SimulationMessageInternal_SharedModeSetPriority);
			buffer.Priority = priority;
			buffer.Object = id;
			SendInternalSimulationMessage(SimulationMessageInternalTypes.SharedModeSetPriority, buffer);
			return true;
		}

		internal bool RequestStateAuthority(NetworkId id, bool wants)
		{
			if (Topology != Topologies.Shared || !IsClient)
			{
				return false;
			}
			if (!TryGetMeta(id, out var meta))
			{
				if (!Config.AreaOfInterestEnabled)
				{
					return false;
				}
				return RequestStateAuthorityByMessage(id, wants);
			}
			bool flag = IsStateAuthority(meta.Header.StateAuthority, LocalPlayer);
			if (wants)
			{
				if (flag)
				{
					return false;
				}
				if (meta.Flags.Has(NetworkObjectHeaderFlags.AllowStateAuthorityOverride))
				{
					return RequestStateAuthorityByMessage(id, wants);
				}
				if (meta.StateAuthority == default(PlayerRef))
				{
					return RequestStateAuthorityByMessage(id, wants);
				}
				return false;
			}
			if (!flag)
			{
				return false;
			}
			if (meta.Flags.Has(NetworkObjectHeaderFlags.AllowStateAuthorityOverride))
			{
				return false;
			}
			return RequestStateAuthorityByMessage(id, wants);
		}

		internal unsafe bool RequestStateAuthorityByMessage(NetworkId id, bool wants)
		{
			Assert.Check(Topology == Topologies.Shared, "Topology == Topologies.Shared");
			Assert.Check(IsClient, "IsClient");
			Assert.Check(sizeof(SimulationMessageInternal_SharedModeRequestStateAuthority) == 8, "SharedModeRequestStateAuthority unexpected size");
			SimulationMessageInternal_SharedModeRequestStateAuthority buffer = default(SimulationMessageInternal_SharedModeRequestStateAuthority);
			buffer.Acquire = (wants ? 1 : 0);
			buffer.Object = id;
			SendInternalSimulationMessage(SimulationMessageInternalTypes.SharedModeRequestStateAuthority, buffer);
			return true;
		}

		internal unsafe void SendTestMessage(int item, NetworkId id, int value)
		{
			Assert.Check(Topology == Topologies.Shared, "Topology == Topologies.Shared");
			Assert.Check(IsClient, "IsClient");
			Assert.Check(sizeof(SimulationMessageInternal_TestMessage) == 12, "sizeof(SimulationMessageInternal_TestMessage) == SimulationMessageInternal_TestMessage.SIZE");
			SimulationMessageInternal_TestMessage buffer = default(SimulationMessageInternal_TestMessage);
			buffer.Item = item;
			buffer.Id = id;
			buffer.Value = value;
			SendInternalSimulationMessage(SimulationMessageInternalTypes.TestMessage, buffer);
		}

		internal unsafe void SetPlayerAlwaysInterested(PlayerRef player, NetworkId id, bool alwaysInterested)
		{
			if ((Config.ReplicationFeatures & NetworkProjectConfig.ReplicationFeatures.InterestManagement) != NetworkProjectConfig.ReplicationFeatures.InterestManagement)
			{
				return;
			}
			NetworkObjectMeta meta;
			SimulationConnection result;
			if (Topology == Topologies.Shared)
			{
				Assert.Check(sizeof(SimulationMessageInternal_SharedModeSetAlwaysInterested) == 12, "SharedModeSetAlwaysInterested unexpected size");
				SimulationMessageInternal_SharedModeSetAlwaysInterested buffer = default(SimulationMessageInternal_SharedModeSetAlwaysInterested);
				buffer.Interested = (alwaysInterested ? 1 : 0);
				buffer.Object = id;
				buffer.Player = player.AsIndex;
				SendInternalSimulationMessage(SimulationMessageInternalTypes.SharedModeSetAlwaysInterested, buffer);
			}
			else if (!(LocalPlayer == player) && PlayerValid(player) && TryGetMeta(id, out meta) && TryGetSimulationConnectionForPlayer(player, out result))
			{
				if (alwaysInterested)
				{
					result.AddAlwaysInterested(meta);
				}
				else
				{
					result.RemoveAlwaysInterested(meta);
				}
			}
		}

		internal void ShutdownNativeSocket()
		{
			if (!_isShutdown)
			{
				NetworkShutdown();
			}
		}

		internal void Dispose()
		{
			if (!_isShutdown)
			{
				_isShutdown = true;
				Release(ref _inputRoot);
				Allocator.Dispose(_allocator);
				_allocator = null;
				Allocator.Dispose(_allocatorObjects);
				_allocatorObjects = null;
				HostMigrationDispose();
			}
		}

		internal void DestroyStateObject(NetworkId id)
		{
			InternalLogStreams.LogTraceObject?.Log(this, string.Format("{0}({1})", "DestroyStateObject", id));
			if (!id.IsValid)
			{
				return;
			}
			if (!IsServer)
			{
				if (TryGetSimulationConnectionByIndex(0, out var result))
				{
					result.ObjectData_Destroyed(id);
				}
			}
			else
			{
				foreach (SimulationConnection value2 in _connections.Values)
				{
					value2.ObjectData_Destroyed(id);
				}
			}
			if (!_metaLookup.Remove(id, out var value))
			{
				return;
			}
			if (id.IsReserved)
			{
				InternalLogStreams.LogError?.Log($"Trying do free an internal object that never should be freed: {id}");
				return;
			}
			if (IsServer || Config.Topology == Topologies.Shared)
			{
				foreach (SimulationConnection value3 in _connections.Values)
				{
					if (value3.TryGetObjectData(id, out var data))
					{
						value3.InterestedObjectList.Remove(data);
					}
				}
			}
			if (value.Flags.Has(NetworkObjectHeaderFlags.AreaOfInterest))
			{
				AOI_RemoveFromAreaOfInterest(value, IsServer && Config.AreaOfInterestEnabled);
			}
			else if (value.Flags.Has(NetworkObjectHeaderFlags.GlobalObjectInterest))
			{
				RemoveFromGlobalObjectInterest(value.Id);
			}
			HostMigrationAfterFreeObject(value);
			if (value.Flags.Has(NetworkObjectHeaderFlags.Struct))
			{
				_structs.Remove(id);
				_structsVersion++;
				if (value.Type == NetworkObjectTypeId.PlayerData)
				{
					ref PlayerSimulationData structData = ref value.GetStructData<PlayerSimulationData>();
					PlayerRef player = structData.Player;
					if (player.IsRealPlayer)
					{
						if (structData.Object != default(NetworkId))
						{
							_playerLeftTempObjectCache.Add(player, structData.Object);
						}
						_invokeJoinedLeaveQueue.Enqueue((player, false));
						if (!IsServer)
						{
							_players.Remove(player);
							_playerDataLookup.Remove(player);
						}
					}
				}
			}
			Release(ref value);
		}

		internal bool PlayerValid(PlayerRef player)
		{
			return _players.Contains(player);
		}

		internal void PlayerAdd(PlayerRef player, SimulationConnection connection)
		{
			InternalLogStreams.LogInfo?.Log($"adding player {player}");
			Assert.Always(_players.Add(player), "player can't exist");
			if (connection != null)
			{
				connection.Player = player;
				_playersConnections.Add(player, connection);
			}
			else
			{
				Assert.Always(IsServer && Mode == SimulationModes.Host && LocalPlayer == player, "if no connection is given the playerref passed has to be the local player on host");
			}
		}

		internal void PlayerRemove(PlayerRef player)
		{
			Assert.Always(_players.Remove(player), "player must exist");
			if (!IsServer || Mode != SimulationModes.Host || !(LocalPlayer == player))
			{
				Assert.Always(_playersConnections.Remove(player), "player connection must exist");
			}
		}

		internal bool IsHostPlayer(PlayerRef player)
		{
			NetworkObjectMeta meta;
			return TryGetStruct(NetworkId.RuntimeConfig, out meta) && meta.GetStructData<SimulationRuntimeConfig>().HostPlayer == player;
		}

		public bool TryGetHostPlayer(out PlayerRef player)
		{
			if (TryGetStruct(NetworkId.RuntimeConfig, out var meta))
			{
				player = meta.GetStructData<SimulationRuntimeConfig>().HostPlayer;
				return true;
			}
			player = default(PlayerRef);
			return false;
		}

		public PlayerRef GetRealPlayer(PlayerRef playerRef)
		{
			if (playerRef.IsRealPlayer)
			{
				return playerRef;
			}
			PlayerRef player;
			if (playerRef.IsNone)
			{
				return TryGetHostPlayer(out player) ? player : PlayerRef.None;
			}
			NetworkObjectMeta meta;
			if (playerRef.IsInternalMasterClientIdentifier)
			{
				return TryGetStruct(NetworkId.RuntimeConfig, out meta) ? meta.GetStructData<SimulationRuntimeConfig>().MasterClient : PlayerRef.Invalid;
			}
			return PlayerRef.Invalid;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool? IsInterestedIn(NetworkObjectMeta meta, PlayerRef player)
		{
			if (!Config.AreaOfInterestEnabled)
			{
				return true;
			}
			if (IsClient)
			{
				if (IsLocalSimulationStateAuthority(ref meta.Header))
				{
					return true;
				}
				if (LocalPlayer != player)
				{
					if (IsHostPlayer(player))
					{
						return true;
					}
					return null;
				}
				return meta.PlayerFlags.HasAny(NetworkObjectHeaderPlayerDataFlags.AllInterestFlags);
			}
			if (!PlayerValid(player))
			{
				return null;
			}
			if (IsHostPlayer(player))
			{
				return true;
			}
			if (TryGetSimulationConnectionForPlayer(player, out var result) && result.TryGetObjectData(meta.Id, out var data))
			{
				return data.PlayerFlags.HasAny(NetworkObjectHeaderPlayerDataFlags.AllInterestFlags);
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsInputAuthority(PlayerRef inputAuthority, PlayerRef playerRef)
		{
			if (inputAuthority.IsNone)
			{
				return false;
			}
			if (inputAuthority == playerRef)
			{
				return true;
			}
			PlayerRef player;
			if (playerRef.IsNone)
			{
				return TryGetHostPlayer(out player) && player == inputAuthority;
			}
			NetworkObjectMeta meta;
			if (inputAuthority.IsInternalMasterClientIdentifier)
			{
				return TryGetStruct(NetworkId.RuntimeConfig, out meta) && meta.GetStructData<SimulationRuntimeConfig>().MasterClient == playerRef;
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsInputAuthority([System.Diagnostics.CodeAnalysis.NotNull] NetworkObjectMeta meta, PlayerRef playerRef)
		{
			return IsInputAuthority(meta.InputAuthority, playerRef);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsStateAuthority(PlayerRef stateAuthority, PlayerRef playerRef)
		{
			if (stateAuthority == playerRef)
			{
				return true;
			}
			PlayerRef player;
			if (stateAuthority.IsNone)
			{
				return TryGetHostPlayer(out player) && player == playerRef;
			}
			NetworkObjectMeta meta;
			if (stateAuthority.IsInternalMasterClientIdentifier)
			{
				return TryGetStruct(NetworkId.RuntimeConfig, out meta) && meta.GetStructData<SimulationRuntimeConfig>().MasterClient == playerRef;
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal bool IsStateAuthority([System.Diagnostics.CodeAnalysis.NotNull] NetworkObjectMeta meta, PlayerRef playerRef)
		{
			return IsStateAuthority(meta.StateAuthority, playerRef);
		}

		internal bool IsLocalPlayer(PlayerRef player)
		{
			PlayerRef localPlayer = LocalPlayer;
			if (player == LocalPlayer)
			{
				return true;
			}
			PlayerRef player2;
			if (localPlayer.IsNone)
			{
				return TryGetHostPlayer(out player2) && player2 == player;
			}
			NetworkObjectMeta meta;
			if (localPlayer.IsInternalMasterClientIdentifier)
			{
				return TryGetStruct(NetworkId.RuntimeConfig, out meta) && meta.GetStructData<SimulationRuntimeConfig>().MasterClient == player;
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal bool IsLocalSimulationInputAuthority([In][RequiresLocation] ref NetworkObjectHeader obj)
		{
			return LocalPlayer.IsRealPlayer && obj.InputAuthority == LocalPlayer;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal bool IsLocalSimulationStateAuthority([In][RequiresLocation] ref NetworkObjectHeader obj)
		{
			return (_config.Topology == Topologies.ClientServer) ? IsServer : IsStateAuthority(obj.StateAuthority, LocalPlayer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal bool IsLocalSimulationStateAuthorityFast([In][RequiresLocation] ref NetworkObjectHeader obj, Topologies topology, PlayerRef player, bool isServer)
		{
			return (topology == Topologies.ClientServer) ? isServer : IsStateAuthority(obj.StateAuthority, player);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public PlayerRef GetStateAuthority(PlayerRef objectStateAuthority)
		{
			if (objectStateAuthority == PlayerRef.None)
			{
				return PlayerRef.None;
			}
			if (Topology == Topologies.Shared)
			{
				NetworkObjectMeta meta;
				if (objectStateAuthority.IsInternalMasterClientIdentifier)
				{
					return TryGetStruct(NetworkId.RuntimeConfig, out meta) ? meta.GetStructData<SimulationRuntimeConfig>().MasterClient : default(PlayerRef);
				}
			}
			else if (Topology == Topologies.ClientServer)
			{
				return default(PlayerRef);
			}
			return objectStateAuthority;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void SetStateAuthority(NetworkObjectMeta meta, PlayerRef newStateAuthority)
		{
			InternalLogStreams.LogTraceSendRecv?.Log(this, $"[SetStateAuthority] {meta.Id} from {meta.StateAuthority} -> {newStateAuthority} on tick: {_tick}");
			meta.StateAuthority = newStateAuthority;
		}

		internal unsafe byte[] GetPlayerConnectionToken(PlayerRef player)
		{
			if (PlayerValid(player))
			{
				if (!TryGetSimulationConnectionForPlayer(player, out var result))
				{
					return null;
				}
				if (result.Connection->ConnectionToken != null && result.Connection->ConnectionTokenLength > 0)
				{
					byte[] array = new byte[result.Connection->ConnectionTokenLength];
					fixed (byte* destination = array)
					{
						FusionUnsafe.Copy(destination, result.Connection->ConnectionToken, result.Connection->ConnectionTokenLength);
					}
					return array;
				}
			}
			return null;
		}

		internal unsafe NetAddress GetPlayerAddress(PlayerRef player)
		{
			if (PlayerValid(player) && TryGetSimulationConnectionForPlayer(player, out var result))
			{
				return result.Connection->Address;
			}
			return default(NetAddress);
		}

		internal unsafe long GetPlayerUniqueId(PlayerRef player)
		{
			if (PlayerValid(player) && TryGetSimulationConnectionForPlayer(player, out var result))
			{
				return result.Connection->UniqueIdHash;
			}
			return 0L;
		}

		public SimulationInput GetInputForPlayer(PlayerRef player)
		{
			return _inputCollection.GetByPlayer(player);
		}

		private void DeletePlayerSimulationDataOnDisconnect(PlayerRef player)
		{
			if (!IsClient && TryGetPlayerSimulationData(player, create: false, out var _) && _playerDataLookup.TryGetValue(player, out var value))
			{
				_playerDataLookup.Remove(player);
				DestroyStateObject(value);
			}
		}

		private unsafe bool TryGetPlayerSimulationData(PlayerRef player, bool create, out NetworkObjectMeta resultMeta)
		{
			if (player.IsInternalMasterClientIdentifier && TryGetStruct(NetworkId.RuntimeConfig, out var meta))
			{
				player = meta.GetStructData<SimulationRuntimeConfig>().MasterClient;
			}
			if (player.IsNone)
			{
				resultMeta = null;
				return false;
			}
			Assert.Always(player.IsRealPlayer, "invalid player {0}", player);
			if (_playerDataLookup.TryGetValue(player, out var value) && TryGetStruct(value, out resultMeta))
			{
				return true;
			}
			foreach (NetworkId @struct in _structs)
			{
				if (!TryGetStruct(@struct, out resultMeta) || !(resultMeta.Type == NetworkObjectTypeId.PlayerData) || resultMeta.GetStructData<PlayerSimulationData>().Player != player)
				{
					continue;
				}
				_playerDataLookup.Add(player, @struct);
				return true;
			}
			if (create && IsServer)
			{
				NetworkId nextId = GetNextId();
				int words = FusionUnsafe.MakeAligned(sizeof(PlayerSimulationData), 4) / 4;
				resultMeta = AllocateStruct(nextId, words, NetworkObjectTypeId.PlayerData);
				resultMeta.GetStructData<PlayerSimulationData>().Player = player;
				_invokeJoinedLeaveQueue.Enqueue((player, true));
				return true;
			}
			resultMeta = null;
			return false;
		}

		private void InvokePlayerJoinedLeft()
		{
			if (!_callbacks.CanReceivePlayerJoinLeaveCallbacks)
			{
				return;
			}
			while (_invokeJoinedLeaveQueue.Count > 0)
			{
				try
				{
					(PlayerRef, bool) tuple = _invokeJoinedLeaveQueue.Dequeue();
					var (playerRef, _) = tuple;
					if (tuple.Item2)
					{
						InternalLogStreams.LogDebug?.Log(this, $"Player Joined: {playerRef}");
						_callbacks.PlayerJoined(playerRef);
					}
					else
					{
						InternalLogStreams.LogDebug?.Log(this, $"Player Left: {playerRef}");
						ExceptionLimitTrackers.ResetTrackers(playerRef.PlayerId);
						_callbacks.PlayerLeft(playerRef);
					}
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(error);
				}
			}
		}

		internal int? GetPlayerActorId(PlayerRef player)
		{
			if (TryGetPlayerSimulationData(player, create: true, out var resultMeta))
			{
				return resultMeta.GetStructData<PlayerSimulationData>().Actor;
			}
			return null;
		}

		internal NetworkId GetPlayerObjectId(PlayerRef player)
		{
			NetworkId value;
			if (!TryGetPlayerSimulationData(player, create: false, out var resultMeta))
			{
				return _playerLeftTempObjectCache.TryGetValue(player, out value) ? value : default(NetworkId);
			}
			return resultMeta.GetStructData<PlayerSimulationData>().Object;
		}

		internal void SetPlayerObjectId(PlayerRef player, NetworkId id)
		{
			NetworkObjectMeta resultMeta3;
			if (Topology == Topologies.ClientServer)
			{
				if (!IsClient && PlayerValid(player) && TryGetPlayerSimulationData(player, create: true, out var resultMeta))
				{
					resultMeta.GetStructData<PlayerSimulationData>().Object = id;
				}
			}
			else if (IsClient)
			{
				if (TryGetMeta(id, out var meta) && IsStateAuthority(meta.StateAuthority, LocalPlayer) && player == LocalPlayer)
				{
					SimulationMessageInternal_SetPlayerObject buffer = default(SimulationMessageInternal_SetPlayerObject);
					buffer.Object = id;
					SendInternalSimulationMessage(SimulationMessageInternalTypes.SetPlayerObject, buffer);
					if (TryGetPlayerSimulationData(player, create: true, out var resultMeta2))
					{
						resultMeta2.GetStructData<PlayerSimulationData>().Object = id;
					}
				}
			}
			else if (PlayerValid(player) && TryGetPlayerSimulationData(player, create: true, out resultMeta3))
			{
				resultMeta3.GetStructData<PlayerSimulationData>().Object = id;
			}
		}

		public unsafe bool HasAnyActiveConnections()
		{
			NetConnectionMap.Iterator iterator = NetPeerGroup.ConnectionIterator(_netPeerGroup);
			while (iterator.Next())
			{
				if (iterator.Current->ConnectionStatus != NetConnectionStatus.Connected)
				{
					continue;
				}
				return true;
			}
			return false;
		}

		private void InvokeOnBeforeSimulation(int forwardTickCount)
		{
			using (HostProfiler.Markers.InvokeOnBeforeSimulation())
			{
				try
				{
					_callbacks.OnBeforeSimulation(forwardTickCount);
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(this, error);
				}
			}
		}

		private void InvokeOnAfterSimulation()
		{
			using (HostProfiler.Markers.InvokeOnAfterSimulation())
			{
				try
				{
					_callbacks.OnAfterSimulation();
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(this, error);
				}
			}
		}

		private void InvokeOnBeforeAllTicks(bool resimulation, int ticks)
		{
			using (HostProfiler.Markers.InvokeOnBeforeAllTicks())
			{
				try
				{
					_isResimulation = resimulation;
					_callbacks.OnBeforeAllTicks(resimulation, ticks);
					_isResimulation = false;
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(this, error);
				}
			}
		}

		private void InvokeOnAfterAllTicks(bool resimulation, int ticks)
		{
			using (HostProfiler.Markers.InvokeOnAfterAllTicks())
			{
				try
				{
					_isResimulation = resimulation;
					_callbacks.OnAfterAllTicks(resimulation, ticks);
					_isResimulation = false;
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(this, error);
				}
			}
		}

		protected virtual void BeforeUpdate()
		{
		}

		protected virtual void AfterSimulation()
		{
		}

		private void UpdateSimulationStateForMasterClientObjects(bool isMasterClient)
		{
			SimulationConnection simulationConnectionForLocalPlayer = GetSimulationConnectionForLocalPlayer();
			HashSet<NetworkId> hashSet = new HashSet<NetworkId>();
			foreach (var (networkId2, networkObjectMeta2) in _metaLookup)
			{
				try
				{
					if (networkId2.IsReserved || ((bool)networkObjectMeta2.Instance && networkObjectMeta2.Instance.Flags.HasNot(NetworkObjectFlags.MasterClientObject)))
					{
						continue;
					}
					NetworkObjectConnectionData objectData = simulationConnectionForLocalPlayer.GetObjectData(networkId2, create: true);
					if (!networkObjectMeta2.SnapshotLatest.Valid)
					{
						InternalLogStreams.LogError?.Log($"SnapshotLatest invalid when copying over master client state for object with NetworkId: {networkObjectMeta2.Id}");
						continue;
					}
					if (networkObjectMeta2.Shadow.Valid)
					{
						networkObjectMeta2.SnapshotLatest.CopyTo(networkObjectMeta2.Shadow);
					}
					else
					{
						InternalLogStreams.LogError?.Log($"Shadow invalid when copying over master client state for object with NetworkId: {networkObjectMeta2.Id}");
					}
					if (networkObjectMeta2.Previous.Valid)
					{
						networkObjectMeta2.SnapshotLatest.CopyTo(networkObjectMeta2.Previous);
					}
					else
					{
						InternalLogStreams.LogError?.Log($"Previous invalid when copying over master client state for object with NetworkId: {networkObjectMeta2.Id}");
					}
					networkObjectMeta2.SnapshotLatest.CopyTo(networkObjectMeta2);
					simulationConnectionForLocalPlayer.SetActive(objectData, networkObjectMeta2);
					hashSet.Add(networkId2);
				}
				catch (Exception error)
				{
					InternalLogStreams.LogError?.Log("Exception when copying over master client state");
					InternalLogStreams.LogException?.Log(error);
				}
			}
			foreach (NetworkId item in hashSet)
			{
				_callbacks.ObjectIsSimulatedChanged(item, isMasterClient);
				_callbacks.ObjectStateAuthorityChanged(item, isMasterClient);
				if (TryGetMeta(item, out var meta) && (bool)meta.Instance && !meta.PlayerFlags.HasAny(NetworkObjectHeaderPlayerDataFlags.AllInterestFlags))
				{
					if (isMasterClient)
					{
						InternalLogStreams.LogTraceAreaOfInterest?.Log(this, $"{item} entering for {LocalPlayer} (Master Client Object)");
						_callbacks.ObjectEnterAOI(LocalPlayer, item);
					}
					else
					{
						InternalLogStreams.LogTraceAreaOfInterest?.Log(this, $"{item} exiting for {LocalPlayer} (Master Client Object)");
						_callbacks.ObjectExitAOI(LocalPlayer, item);
					}
				}
			}
		}

		private int CalculateInputTicks()
		{
			if (HasRuntimeConfig && _time.IsRunning())
			{
				int val = (int)(_time.Now().Input * (double)TickRate);
				val = Math.Min(val, (int)LatestServerTick + TickRate);
				int val2 = (val - (int)_inputTick) / TickStride;
				return Math.Min(Math.Max(val2, 0), TickRate);
			}
			return 0;
		}

		private int CalculateForwardTicks()
		{
			if (HasRuntimeConfig && _time.IsRunning())
			{
				int val = (int)(_time.Now().Local * (double)TickRate);
				val = Math.Min(val, (int)LatestServerTick + TickRate);
				int val2 = (val - (int)_tick) / TickStride;
				return Math.Min(Math.Max(val2, 0), TickRate);
			}
			return 0;
		}

		private bool UpdateSendTick()
		{
			if (HasRuntimeConfig && _time.IsRunning())
			{
				Tick sendTick = ((!IsClient || Topology != Topologies.ClientServer) ? _tick : _inputTick);
				int num = sendTick.Raw - _sendTick.Raw;
				if (num < TickRate / SendRate)
				{
					return false;
				}
				_sendTick = sendTick;
				return true;
			}
			return false;
		}

		public int Update(double dt)
		{
			if (_isShutdown || dt == 0.0)
			{
				return 0;
			}
			int num = 0;
			using (HostProfiler.Markers.SimulationUpdate())
			{
				BeforeUpdate();
				NetworkRecv();
				if (HasRuntimeConfig && _time.IsRunning())
				{
					_time.Update(dt);
					if (!Runner.OnGameStartedInvoked)
					{
						Runner.OnRuntimeConfigReady();
					}
					if (IsServer)
					{
						CalculateUpdateTime();
					}
				}
				if (!_isWaitingForShutdown)
				{
					num = CalculateForwardTicks();
					if (num > 0)
					{
						InvokeOnBeforeSimulation(num);
						int num2;
						using (HostProfiler.Markers.BeforeSimulation())
						{
							num2 = BeforeSimulation();
						}
						_fusionStatsManager.AddStat(FusionStatType.Resimulations, num2);
					}
					UpdateInterpolationParams();
					int num3 = CalculateInputTicks();
					if (num3 > 0)
					{
						for (int i = 0; i < num3; i++)
						{
							_inputTick = _inputTick.Next(TickStride);
							foreach (PlayerRef player in _players)
							{
								PollInput(_inputTick, player);
							}
						}
					}
					if (num > 0)
					{
						_fusionStatsManager.AddStat(FusionStatType.ForwardTicks, num);
						try
						{
							InvokeOnBeforeAllTicks(resimulation: false, num);
							for (int j = 0; j < num; j++)
							{
								StepSimulation(SimulationStages.Forward, j == num - 1, j == 0, IsServer);
							}
							InvokeOnAfterAllTicks(resimulation: false, num);
						}
						catch (Exception error)
						{
							InternalLogStreams.LogException?.Log(this, error);
						}
						InvokeOnAfterSimulation();
						using (HostProfiler.Markers.AfterSimulation())
						{
							AfterSimulation();
						}
					}
					if (UpdateSendTick())
					{
						try
						{
							PreparePackets();
							SendPackets();
						}
						catch (Exception error2)
						{
							InternalLogStreams.LogException?.Log(this, error2);
						}
					}
				}
				if (num <= 0)
				{
					NoSimulation();
				}
				NetworkSend();
				Assert.Check(_stage == (SimulationStages)0, "Invalid Simulation.Stage {0}", _stage);
			}
			AfterUpdate();
			if (HasRuntimeConfig && _time.IsRunning() && IsPlayer)
			{
				_localAlphaPrev = _localAlpha;
				_localAlpha = (float)Maths.Clamp01((_time.Now().Local - (double)(int)_tick * TickDeltaDouble) * (double)TickRate);
				if (!IsClient)
				{
					_remoteAlpha = _localAlpha;
				}
				if (IsClient)
				{
					_time.Log(_fusionStatsManager);
				}
			}
			return num;
		}

		private void UpdateAreaOfInterest()
		{
			if (!IsServer || (Config.ReplicationFeatures & NetworkProjectConfig.ReplicationFeatures.InterestManagement) != NetworkProjectConfig.ReplicationFeatures.InterestManagement)
			{
				return;
			}
			using (HostProfiler.Markers.UpdateAreaOfInterest())
			{
				foreach (NetworkObjectMeta value in _metaLookup.Values)
				{
					if (value.Flags.HasNot(NetworkObjectHeaderFlags.AreaOfInterest))
					{
						continue;
					}
					Assert.Check(value.HasMainTRSP, value.Instance.gameObject.name);
					if (value.HasMainTRSP)
					{
						Vector3? vector = ResolveCellPosition(value);
						if (vector.HasValue)
						{
							AOI_UpdateAreaOfInterest(value, AreaOfInterestInstance.ToCell(vector.Value));
						}
						else
						{
							InternalLogStreams.LogDebug?.Error($"could not resolve aoi position for {value.Id} (type:{value.Type})");
						}
					}
				}
			}
			Vector3? ResolveCellPosition(NetworkObjectMeta m)
			{
				ref NetworkTRSPData mainTRSPData = ref m.MainTRSPData;
				while ((bool)mainTRSPData.AreaOfInterestOverride)
				{
					if (!TryGetMeta(mainTRSPData.AreaOfInterestOverride, out var meta) || !meta.HasMainTRSP)
					{
						return null;
					}
					mainTRSPData = ref meta.MainTRSPData;
				}
				while ((bool)mainTRSPData.Parent.Object)
				{
					if (!TryGetMeta(mainTRSPData.Parent.Object, out var meta2) || !meta2.HasMainTRSP)
					{
						return null;
					}
					mainTRSPData = ref meta2.MainTRSPData;
				}
				return mainTRSPData.Position;
			}
		}

		internal unsafe double GetMasterClientConnectionIdleTime()
		{
			if (TryGetStruct(NetworkId.RuntimeConfig, out var meta) && TryGetSimulationConnectionForPlayer(meta.GetStructData<SimulationRuntimeConfig>().MasterClient, out var result))
			{
				return NetPeerGroup.GetConnectionIdleTime(_netPeerGroup, result.Connection);
			}
			return 0.0;
		}

		private unsafe void PreparePackets()
		{
			UpdateAreaOfInterest();
			using (HostProfiler.Markers.PreparePackets())
			{
				_stateReplicator.UpdateChangedStructSet();
				NetConnectionMap.Iterator iterator = NetPeerGroup.ConnectionIterator(_netPeerGroup);
				while (iterator.Next())
				{
					NetConnection* current = iterator.Current;
					if (!TryGetSimulationConnection(current, out var result, logError: false))
					{
						continue;
					}
					double connectionIdleTime = NetPeerGroup.GetConnectionIdleTime(_netPeerGroup, current);
					if (connectionIdleTime >= 1.0 && _netPeerGroup->Time - result.LastSend < 0.5)
					{
						continue;
					}
					Assert.Check(current->V2 != null, "connection->V2 != null");
					if (current->V2 != null && !CanQueueSimulationPacket(result.Player, current->V2))
					{
						continue;
					}
					SendContext context = new SendContext(this, _writer);
					try
					{
						context.Init(result, _sendTick);
						using (context.Write)
						{
							WriteMessages(ref context);
							WritePackets(ref context);
							_fusionStatsManager.AddStat(FusionStatType.OutBandwidth, context.Write.Length);
							bool reliable = false;
							if (_continuousFragmentLoss >= 8)
							{
								InternalLogStreams.LogTraceSendRecv?.Warn("Continuous fragment loss exceeded threshold, sending reliable");
								reliable = true;
							}
							context.Send(reliable);
							result.SendCounter++;
							result.LastSend = _netPeerGroup->Time;
						}
					}
					finally
					{
						context.Dispose();
					}
				}
			}
		}

		private unsafe void SendPackets()
		{
			using (HostProfiler.Markers.SendPackets())
			{
				NetConnectionMap.Iterator iterator = NetPeerGroup.ConnectionIterator(_netPeerGroup);
				while (iterator.Next())
				{
					NetConnection* current = iterator.Current;
					if (TryGetSimulationConnection(current, out var _))
					{
						int num = Connection.Send(_netSocket, current->V2);
						_fusionStatsManager.AddStat(FusionStatType.OutPackets, num);
					}
				}
			}
		}

		private unsafe static bool CanQueueSimulationPacket(PlayerRef player, [System.Diagnostics.CodeAnalysis.NotNull] Connection* connection)
		{
			if (!Connection.CanQueue(connection, &connection->GameReliable))
			{
				InternalLogStreams.LogTraceSendRecv?.Log($"Not sending to {player}: GameReliable can't queue");
				return false;
			}
			for (Packet* ptr = connection->SendWindow.Head; ptr != null; ptr = connection->SendWindow.Next(ptr))
			{
				if (ptr->Header->Channel == connection->GameReliable.Id)
				{
					InternalLogStreams.LogTraceSendRecv?.Log($"Not sending to {player}: GameReliable packets in the send window ({*ptr->Header})");
					return false;
				}
			}
			if (!Connection.CanQueue(connection, &connection->Game))
			{
				InternalLogStreams.LogTraceSendRecv?.Log($"Not sending to {player}: Game can't queue");
				return false;
			}
			return true;
		}

		private void InvokeTick(SimulationStages stage, bool releaseAllInputs)
		{
			try
			{
				_stage = stage;
				InterpolateSequenceIncrement();
				Assert.Check(_inputCollection.Count == 0, "_inputCollection.Count == 0");
				foreach (PlayerRef player in _players)
				{
					SimulationInput bufferedInput = GetBufferedInput(_tick, player);
					if (bufferedInput != null)
					{
						_inputCollection.AddInput(bufferedInput);
					}
				}
				if (IsClient && IsFirstTick)
				{
					SimulationStages stage2 = _stage;
					try
					{
						_stage = SimulationStages.Forward;
						using (HostProfiler.Markers.UpdateRemotePrefabs())
						{
							_callbacks.UpdateRemotePrefabs();
						}
					}
					finally
					{
						_stage = stage2;
					}
				}
				if (_isInitialLocalTick)
				{
					_isInitialLocalTick = false;
					BeforeFirstTick();
				}
				using (HostProfiler.Markers.SimulationBeforeTick())
				{
					_callbacks.OnBeforeTick();
				}
				if (_stage == SimulationStages.Forward)
				{
					DeliverMessages(_tick);
				}
				try
				{
					_isInTick = true;
					InvokePlayerJoinedLeft();
					_playerLeftTempObjectCache.Clear();
					_callbacks.OnTick();
				}
				catch (Exception error)
				{
					InternalLogStreams.LogError?.Log(this, "OnTick Threw Exception");
					InternalLogStreams.LogException?.Log(this, error);
				}
				finally
				{
					_isInTick = false;
				}
				using (HostProfiler.Markers.SimulationAfterTick())
				{
					_callbacks.OnAfterTick();
				}
			}
			catch (Exception error2)
			{
				InternalLogStreams.LogException?.Log(this, error2);
			}
			finally
			{
				_stage = (SimulationStages)0;
				try
				{
					if (releaseAllInputs)
					{
						for (int i = 0; i < _inputCollection.Count; i++)
						{
							SimulationInput obj = _inputCollection.GetByIndex(i);
							Release(ref obj);
						}
					}
				}
				finally
				{
					_inputCollection.Clear();
				}
			}
		}

		private void OnMessageInternal(PlayerRef sourcePlayer, SimulationMessageInternalTypes internalType, Span<byte> payload)
		{
			InternalLogStreams.LogTraceNetwork?.Log($"OnMessageInternal({internalType})");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ContractAnnotation("=> true, result: notnull; => false, result: null")]
		internal bool TryGetSimulationConnectionByIndex(int index, [MaybeNullWhen(false)][NotNullWhen(true)] out SimulationConnection result)
		{
			return _connections.TryGetValue(index, out result);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ContractAnnotation("=> true, result: notnull; => false, result: null")]
		internal bool TryGetSimulationConnectionForPlayer(PlayerRef player, [MaybeNullWhen(false)][NotNullWhen(true)] out SimulationConnection result)
		{
			return _playersConnections.TryGetValue(player, out result);
		}

		[return: System.Diagnostics.CodeAnalysis.NotNull]
		internal SimulationConnection GetSimulationConnectionForLocalPlayer()
		{
			if (!TryGetSimulationConnectionForPlayer(LocalPlayer, out var result))
			{
				Assert.AlwaysFail($"Unable to get SimConn for local player {LocalPlayer}");
			}
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal bool TryGetConnectionIndexForPlayer(PlayerRef player, out int connectionIndex)
		{
			if (_playersConnections.TryGetValue(player, out var value))
			{
				connectionIndex = value.ConnectionIndex;
				return true;
			}
			connectionIndex = 0;
			return false;
		}

		[ContractAnnotation("=> true, result: notnull; => false, result: null")]
		private unsafe bool TryGetSimulationConnection(NetConnection* c, out SimulationConnection result, bool logError = true)
		{
			SimulationConnection simulationConnection = _connections[c->LocalConnectionId.GroupIndex];
			if (simulationConnection.ConnectionId == c->LocalConnectionId)
			{
				if (simulationConnection.Connection != c && logError)
				{
					InternalLogStreams.LogError?.Log($"SimulationConnection.Connection != NetConnection for {c->LocalConnectionId}");
				}
				result = simulationConnection;
				return true;
			}
			if (logError)
			{
				InternalLogStreams.LogError?.Log($"Failed getting SimulationConnection for {c->LocalConnectionId}");
			}
			result = null;
			return false;
		}

		internal void AddToGlobalObjectInterest(NetworkObjectMeta meta)
		{
			if (_globalInterestObjects == null)
			{
				return;
			}
			Assert.Check((Config.ReplicationFeatures & NetworkProjectConfig.ReplicationFeatures.InterestManagement) == NetworkProjectConfig.ReplicationFeatures.InterestManagement, "(Config.ReplicationFeatures & NetworkProjectConfig.ReplicationFeatures.InterestManagement) == NetworkProjectConfig.ReplicationFeatures.InterestManagement");
			_globalInterestObjects.Add(meta.Id);
			foreach (SimulationConnection value in _connections.Values)
			{
				value.GetObjectData(meta.Id, create: true).SetPlayerFlag(NetworkObjectHeaderPlayerDataFlags.ForceInterest, this);
			}
		}

		internal void RemoveFromGlobalObjectInterest(NetworkId id)
		{
			if (_globalInterestObjects == null)
			{
				return;
			}
			Assert.Check((Config.ReplicationFeatures & NetworkProjectConfig.ReplicationFeatures.InterestManagement) == NetworkProjectConfig.ReplicationFeatures.InterestManagement, "(Config.ReplicationFeatures & NetworkProjectConfig.ReplicationFeatures.InterestManagement) == NetworkProjectConfig.ReplicationFeatures.InterestManagement");
			if (!_globalInterestObjects.Remove(id))
			{
				return;
			}
			foreach (SimulationConnection value in _connections.Values)
			{
				value.GetObjectData(id, create: false)?.ClearPlayerFlag(NetworkObjectHeaderPlayerDataFlags.ForceInterest, this);
			}
		}

		internal unsafe void SendReliableData(int connection, SimulationMessageHeader header, ReadOnlySpan<byte> data, bool progress)
		{
			if (TryGetSimulationConnectionByIndex(connection, out var result))
			{
				ReadOnlySpan<byte> data2 = MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref header, 1));
				NetPeerGroup.SendReliable(_netPeerGroup, result.Connection, data2, data, progress);
			}
		}

		internal void SendReliableData(int connection, PlayerRef target, ReliableKey key, ReadOnlySpan<byte> data, bool progress = true)
		{
			if (TryGetSimulationConnectionByIndex(connection, out var result))
			{
				SimulationMessageHeader header = default(SimulationMessageHeader);
				header.Flags = SimulationMessageHeaderFlags.ReliableData | SimulationMessageHeaderFlags.HasTargetPlayer;
				header.SourcePlayer = result.Player;
				header.TargetPlayer = target;
				header.PayloadNumBytes = data.Length;
				header.ReliableKey = key;
				Unsafe.SkipInit<NetworkBehaviourId>(out header.TargetObject);
				Unsafe.SkipInit<uint>(out header.MessageType);
				SendReliableData(connection, header, data, progress);
			}
		}

		internal void NotifyWaitingForShutdown()
		{
			_isWaitingForShutdown = true;
		}

		internal void CreateInternalStateObjects()
		{
			PlayerRef stateAuthority = ((Topology == Topologies.Shared) ? PlayerRef.InternalMasterClientIdentifier : PlayerRef.None);
			NetworkObjectMeta networkObjectMeta = AllocateStruct(NetworkId.SceneInfo, 13);
			networkObjectMeta.StateAuthority = stateAuthority;
			networkObjectMeta.Shadow.CopyFrom(networkObjectMeta);
			NetworkObjectMeta networkObjectMeta2 = AllocateStruct(NetworkId.PhysicsInfo, 10);
			networkObjectMeta2.StateAuthority = stateAuthority;
			networkObjectMeta2.Shadow.CopyFrom(networkObjectMeta2);
		}

		UnityEngine.Object ILogSource.GetUnityObject()
		{
			return Runner;
		}

		[Conditional("DEBUG")]
		internal void DumpObject(NetworkId id, StringBuilder sb)
		{
			NetworkObjectMeta meta;
			if (id.IsReserved)
			{
				if (!TryGetStruct(id, out meta))
				{
					return;
				}
			}
			else if (!TryGetMeta(id, out meta))
			{
				return;
			}
			DumpObject(meta, sb);
		}

		[Conditional("DEBUG")]
		internal void DumpObject(NetworkObjectMeta meta, StringBuilder sb)
		{
			if (meta == null)
			{
				sb.AppendLine("null");
				return;
			}
			sb.AppendLine(meta.Header.ToString());
			sb.Append(BinUtils.WordsToHex(meta.Data));
		}

		internal string DumpObject(NetworkId id)
		{
			StringBuilder stringBuilder = new StringBuilder();
			DumpObject(id, stringBuilder);
			return stringBuilder.ToString();
		}

		internal string DumpObject(NetworkObjectMeta meta)
		{
			StringBuilder stringBuilder = new StringBuilder();
			DumpObject(meta, stringBuilder);
			return stringBuilder.ToString();
		}

		private unsafe void NetworkInit(INetSocket socket, NetAddress address)
		{
			NetConfig config = _projectConfig.Network.ToNetConfig(address);
			config.PacketSize = 8192;
			config.ConnectionGroups = 1;
			if (IsSinglePlayer)
			{
				config.MaxConnections = 0;
			}
			else if (IsClient)
			{
				config.MaxConnections = 1;
			}
			else
			{
				Assert.Check(IsServer, "IsServer");
				if (IsPlayer)
				{
					config.MaxConnections = _config.PlayerCount - 1;
				}
				else
				{
					config.MaxConnections = _config.PlayerCount;
				}
			}
			_netSocket = socket;
			_netPeer = NetPeer.Initialize(config, _netSocket);
			_netPeerGroup = NetPeer.GetGroup(_netPeer, 0);
			_netPeerRng = new System.Random(Environment.TickCount);
		}

		private unsafe void NetworkSend()
		{
			if (_netPeer == null)
			{
				return;
			}
			using (HostProfiler.Markers.NetworkSend())
			{
				NetPeer.Send(_netPeer, _netSocket);
			}
		}

		private unsafe void NetworkRecv()
		{
			if (_netPeer == null)
			{
				return;
			}
			using (HostProfiler.Markers.NetworkRecv())
			{
				if (_netPeerRng == null)
				{
					_netPeerRng = new System.Random(Environment.TickCount);
				}
				NetPeer.Recv(_netPeer, _netSocket, _netPeerRng);
				int num = NetPeerGroup.Update(_netPeerGroup, this);
				_fusionStatsManager.AddStat(FusionStatType.InPackets, num);
				NetworkReceiveDone();
			}
		}

		private unsafe void NetworkShutdown()
		{
			OnNetworkShutdown();
			foreach (SimulationConnection value in _connections.Values)
			{
				value.Free(this);
			}
			NetPeer.Destroy(_netPeer, _netSocket, this);
			_netPeer = null;
			_netPeerGroup = null;
			_netSocket = null;
		}

		internal virtual void OnNetworkShutdown()
		{
		}

		[ContractAnnotation("=> false, buffer:null; => true, buffer:notnull")]
		private unsafe bool NetworkGetBuffer(NetConnection* connection, out NetBitBuffer* buffer)
		{
			if (_netPeer == null)
			{
				buffer = null;
				return false;
			}
			return NetPeerGroup.GetNotifyDataBuffer(_netPeerGroup, connection, out buffer);
		}

		private unsafe bool NetworkSendBuffer(NetConnection* connection, WriteBuffer writer, SimulationPacketEnvelope envelope, bool reliable)
		{
			if (_netPeer == null)
			{
				return false;
			}
			nuint num = ++_nextSimulationPacketId;
			_simulationPacketDictionary.Add(num, envelope);
			using (PacketQueue packetQueue = Connection.Queue(connection->V2, reliable ? (&connection->V2->GameReliable) : (&connection->V2->Game), num, progress: false))
			{
				packetQueue.Add(new ReadOnlySpan<byte>(writer.Buffer, 0, writer.Length));
			}
			AddReference(envelope);
			return true;
		}

		internal unsafe bool NetworkSendPing(NetAddress address, void* data, int length)
		{
			if (_netPeer == null)
			{
				return false;
			}
			return NetPeerGroup.SendUnconnectedData(_netPeerGroup, address, data, length);
		}

		unsafe void INetPeerGroupCallbacks.OnConnectionAttempt(NetConnection* connection, int attempt, int totalConnectionAttempts)
		{
			Assert.Check(IsClient, "IsClient");
			_callbacks.OnInternalConnectionAttempt(attempt, totalConnectionAttempts, out var shouldChange, out var newAddress);
			if (shouldChange)
			{
				NetPeerGroup.ChangeConnectionAddressDuringConnecting(_netPeerGroup, connection, newAddress);
			}
		}

		void INetPeerGroupCallbacks.OnRejoinRequest(string sessionId)
		{
			Assert.Check(IsClient, "IsClient");
			_callbacks.OnRejoinRequest(sessionId);
		}

		unsafe void INetPeerGroupCallbacks.OnUnconnectedData(NetBitBuffer* buffer)
		{
		}

		unsafe void INetPeerGroupCallbacks.OnConnected(NetConnection* connection)
		{
			InternalLogStreams.LogTraceNetwork?.Log(this, $"OnConnected {connection->LocalConnectionId.GroupIndex} / {connection->Address}");
			SimulationConnection simulationConnection = new SimulationConnection(this);
			_connections.Add(connection->LocalConnectionId.GroupIndex, simulationConnection);
			simulationConnection.Connection = connection;
			simulationConnection.ConnectionId = connection->LocalConnectionId;
			if (IsServer)
			{
				PlayerRefMapping? playerRefMapping = GetPlayerRefMapping(connection->UniqueId);
				if (!playerRefMapping.HasValue)
				{
					throw new Exception("No PlayerRef mapping was found");
				}
				TryGetPlayerSimulationData(playerRefMapping.Value.PlayerRef, create: true, out var _);
				PlayerAdd(playerRefMapping.Value.PlayerRef, simulationConnection);
			}
			else
			{
				PlayerAdd(_callbacks.LocalPlayerRef, simulationConnection);
			}
			if (TryGetStruct(NetworkId.SceneInfo, out var meta))
			{
				simulationConnection.ObjectData_CreateConfirmed(meta, force: true);
			}
			if (TryGetStruct(NetworkId.PhysicsInfo, out meta))
			{
				simulationConnection.ObjectData_CreateConfirmed(meta, force: true);
			}
			NetworkConnected(connection);
			try
			{
				if (IsClient)
				{
					_callbacks.OnConnectedToServer();
				}
			}
			catch (Exception error)
			{
				InternalLogStreams.LogException?.Log(this, error);
			}
		}

		unsafe void INetPeerGroupCallbacks.OnDisconnected(NetConnection* connection, NetDisconnectReason reason)
		{
			InternalLogStreams.LogTraceNetwork?.Log(this, $"Disconnected: Address={connection->Address}, Reason={reason}");
			if (!TryGetSimulationConnection(connection, out var result, logError: false))
			{
				InternalLogStreams.LogWarn?.Log($"got disconnect for {connection->Address} without a simulation connection, reason: {reason}");
			}
			PlayerRef playerRef = Connection2Player(connection);
			NetworkDisconnected(connection, reason);
			if (IsServer)
			{
				if (result != null)
				{
					AOI_RemoveConnection(result);
				}
				PlayerRefMapping? playerRefMapping = GetPlayerRefMapping(connection->UniqueId);
				if (!playerRefMapping.HasValue)
				{
					throw new Exception();
				}
				DeletePlayerSimulationDataOnDisconnect(playerRefMapping.Value.PlayerRef);
			}
			PlayerRemove(playerRef);
			_playersConnections.Remove(playerRef);
			_connections.Remove(connection->LocalConnectionId.GroupIndex);
			result?.Free(this);
		}

		unsafe void INetPeerGroupCallbacks.OnReliableData(NetConnection* connection, ReadOnlySpan<byte> data)
		{
			SimulationMessageHeader header = Unsafe.ReadUnaligned<SimulationMessageHeader>(ref MemoryMarshal.GetReference(data));
			ReadOnlySpan<byte> readOnlySpan = data.Slice(32, data.Length - 32);
			if (IsServer)
			{
				if (header.TargetPlayer != PlayerRef.None && header.TargetPlayer != LocalPlayer)
				{
					if (ProjectConfig.Network.ReliableDataTransferModes.HasNot(NetworkConfiguration.ReliableDataTransfers.ClientToClientWithServerProxy))
					{
						NetPeerGroup.Disconnect(_netPeerGroup, connection, null);
						InternalLogStreams.LogDebug?.Error(this, "Disconnecting client for sending server-proxied reliable data when not allowed");
						return;
					}
				}
				else if (ProjectConfig.Network.ReliableDataTransferModes.HasNot(NetworkConfiguration.ReliableDataTransfers.ClientToServer))
				{
					NetPeerGroup.Disconnect(_netPeerGroup, connection, null);
					InternalLogStreams.LogDebug?.Error(this, "Disconnecting client for sending reliable data when not allowed");
					return;
				}
			}
			if (header.Flags.Has(SimulationMessageHeaderFlags.Rpc))
			{
				SimulationMessageResult simulationMessageResult = HandleMessage(in header, 0, readOnlySpan);
				try
				{
					InternalLogStreams.LogTraceSimulationMessage?.Log($"Data RPC Handle Message {header}: {simulationMessageResult}");
					if (simulationMessageResult.IsRetry)
					{
						InternalLogStreams.LogError?.Log("Not implemented yet");
					}
					else
					{
						if (!simulationMessageResult.IsForward)
						{
							return;
						}
						PooledList<SimulationConnection> targetConnections = simulationMessageResult.TargetConnections;
						Span<SimulationConnection> span = targetConnections.AsSpan();
						for (int i = 0; i < span.Length; i++)
						{
							SimulationConnection simulationConnection = span[i];
							if (!(simulationConnection.Player == header.SourcePlayer) || !header.Flags.HasNot(SimulationMessageHeaderFlags.AllowRoundtrip))
							{
								NetPeerGroup.SendReliable(_netPeerGroup, simulationConnection.Connection, data);
							}
						}
					}
					return;
				}
				finally
				{
					((IDisposable)simulationMessageResult/*cast due to .constrained prefix*/).Dispose();
				}
			}
			if (IsServer && header.TargetPlayer != PlayerRef.None && header.TargetPlayer != LocalPlayer)
			{
				if (_playersConnections.TryGetValue(header.TargetPlayer, out var value))
				{
					NetPeerGroup.SendReliable(_netPeerGroup, value.Connection, data);
				}
				else
				{
					InternalLogStreams.LogDebug?.Error(this, $"Target client connection ({header.TargetPlayer}) not found to send reliable data");
				}
			}
			else
			{
				_callbacks.OnReliableData(Connection2Player(connection), header.ReliableKey, local: false, readOnlySpan);
			}
		}

		unsafe void INetPeerGroupCallbacks.OnReliableDataProgress(NetConnection* connection, ReadOnlySpan<byte> data, int fragmentNum, int fragmentCount)
		{
		}

		OnConnectionRequestReply INetPeerGroupCallbacks.OnConnectionRequest(NetAddress remoteAddress, byte[] token, byte[] uniqueid)
		{
			ulong key = BitConverter.ToUInt64(uniqueid, 0);
			if (_uniqueIdPlayerRefMapping.TryGetValue(key, out var _))
			{
				return _callbacks.OnConnectionRequest(remoteAddress, token);
			}
			return OnConnectionRequestReply.Waiting;
		}

		void INetPeerGroupCallbacks.OnConnectionFailed(NetAddress address, NetConnectFailedReason reason)
		{
			try
			{
				_callbacks.OnConnectionFailed(address, reason);
			}
			catch (Exception error)
			{
				InternalLogStreams.LogException?.Log(this, error);
			}
		}

		unsafe void INetPeerGroupCallbacks.OnUnreliableData(NetConnection* connection, NetBitBuffer* buffer)
		{
			Assert.AlwaysFail("Not implemented");
		}

		unsafe bool INetPeerGroupCallbacks.OnNotifyData(NetConnection* c, ReadBuffer read)
		{
			if (!TryGetSimulationConnection(c, out var result))
			{
				return false;
			}
			RecvContext rc = new RecvContext(this);
			rc.Init(result, read);
			_fusionStatsManager.AddStat(FusionStatType.InBandwidth, read.Length);
			rc.Connection.PacketReceiveDelta();
			try
			{
				if (RecvMessages(ref rc))
				{
					RecvPacket(ref rc);
				}
			}
			catch (Exception error)
			{
				InternalLogStreams.LogException?.Limit(ExceptionLimitTrackers?.ReadMessagesFromBuffer, rc.Player.PlayerId, out var logCount)?.Log(string.Format("{0} caused {1}/{2} exceptions [{3}/{4}].", rc.Player.PlayerId, "RecvMessages", "RecvPacket", logCount, ExceptionLimitTrackers?.ReadMessagesFromBuffer), error);
			}
			if (rc.Read.Offset != rc.Read.Length)
			{
				InternalLogStreams.LogDebug?.Warn($"Not all data have been read for {rc.Player}: {rc.Read.Offset} < {rc.Read.Length}");
			}
			return true;
		}

		unsafe void INetPeerGroupCallbacks.OnNotifyDataLost(NetConnection* connection, in NetNotifyDataInfo info)
		{
			if (!_simulationPacketDictionary.Remove(info.UserData, out var value))
			{
				InternalLogStreams.LogError?.Log($"Failed to convert {info.UserData} to packet envelope");
				return;
			}
			try
			{
				if (connection->ConnectionStatus != NetConnectionStatus.Connected)
				{
					return;
				}
				if (value.Messages.IsValid || value.Objects.IsValid)
				{
					_continuousFragmentLoss += info.FragmentCount;
					InternalLogStreams.LogTraceSendRecv?.Warn(this, $"Continuous fragment loss: {_continuousFragmentLoss}");
				}
				if (value.Messages.IsValid)
				{
					if (!TryGetSimulationConnection(connection, out var result, logError: false))
					{
						InternalLogStreams.LogError?.Log($"Tried to requeue messages, but there's no SimulationConnection for {connection->LocalConnectionId}");
					}
					else if (value.Messages.Count > 0)
					{
						InternalLogStreams.LogTraceSimulationMessage?.Warn(this, $"Lost {value.Messages.Count} messages, requeuing");
						Span<SimulationMessagePacketData> span = value.Messages.AsSpan();
						for (int i = 0; i < span.Length; i++)
						{
							SimulationMessagePacketData simulationMessagePacketData = span[i];
							SimulationMessage message = simulationMessagePacketData.Message;
							Assert.Check(message != null && message.RefCount > 0, "data.Message?.RefCount > 0");
							result.RequeueMessage(simulationMessagePacketData.Message, simulationMessagePacketData.Tick, simulationMessagePacketData.Sequence);
						}
					}
				}
				_stateReplicator.OnPacketLost(connection, value.Tick, value.Objects);
			}
			finally
			{
				bool condition = ReleaseReference(value);
				Assert.Always(condition, "released");
			}
		}

		unsafe void INetPeerGroupCallbacks.OnNotifyDataDelivered(NetConnection* connection, in NetNotifyDataInfo info)
		{
			if (!_simulationPacketDictionary.Remove(info.UserData, out var value))
			{
				InternalLogStreams.LogError?.Log($"Failed to convert {info.UserData} to packet envelope");
				return;
			}
			if (_continuousFragmentLoss > 0 && (value.Messages.IsValid || value.Objects.IsValid))
			{
				InternalLogStreams.LogTraceSendRecv?.Log($"Continuous loss reset ({_continuousFragmentLoss})");
				_continuousFragmentLoss = 0;
			}
			try
			{
				if (value.Messages.IsValid && value.Messages.Count > 0)
				{
					TryGetSimulationConnection(connection, out var result, logError: false);
					InternalLogStreams.LogTraceSimulationMessage?.Log(this, string.Format("Delivered {0} to {1}: {2}", value.Messages.Count, (result != null) ? ((object)result.Player) : "unknown", string.Join(", ", from x in value.Messages.AsEnumerable()
						select $"(seq{x.Sequence})")));
				}
				_stateReplicator.OnPacketDelivered(connection, value.Tick, value.Objects.AsSpan());
			}
			finally
			{
				bool condition = ReleaseReference(value);
				Assert.Always(condition, "released");
			}
		}

		internal void GetGeneralAllocatorMemorySnapshot(ref FusionAllocatorMemorySnapshot snapshot)
		{
			_allocator.GetMemorySnapshot(ref snapshot);
		}

		internal void GetObjectAllocatorMemorySnapshot(ref FusionAllocatorMemorySnapshot snapshot)
		{
			_allocatorObjects.GetMemorySnapshot(ref snapshot);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal bool IsSimulated(NetworkObjectMeta meta)
		{
			return meta != null && (bool)meta.Instance && meta.Instance.IsInSimulation;
		}

		internal bool HasObject(NetworkId id)
		{
			if (_metaLookup.TryGetValue(id, out var value))
			{
				Assert.Check(id == value.Id, "id == meta.Id");
				return true;
			}
			return false;
		}

		internal void LogAllObjectIds()
		{
			InternalLogStreams.LogWarn?.Log(string.Join(", ", _metaLookup.Select((KeyValuePair<NetworkId, NetworkObjectMeta> x) => $"{x.Key} == {x.Value.Id}")));
		}

		[ContractAnnotation("=> true, meta: notnull; => false, meta: null")]
		internal bool TryGetMeta(NetworkId id, out NetworkObjectMeta meta)
		{
			meta = null;
			if (_metaLookup.TryGetValue(id, out meta))
			{
				Assert.Check(id == meta.Id, "id == meta.Id");
				return true;
			}
			return false;
		}

		internal unsafe NetworkId GetNextId()
		{
			NetworkId result = default(NetworkId);
			result.Raw = ++_idCounter;
			if (IsClient)
			{
				Assert.Always(Topology == Topologies.Shared, "Topology == Topologies.Shared");
				Assert.Always(LocalPlayer.IsRealPlayer, "LocalPlayer.IsRealPlayer");
				result.Raw &= 524287u;
				result.Raw |= (((Client)this).ServerConnection->Counter << 19) & 0xFFF80000u;
			}
			return result;
		}

		internal NetworkObjectHeaderSnapshotRef GetLatestSnapshot(NetworkId id)
		{
			if (TryGetMeta(id, out var meta) && meta.HasSnapshots)
			{
				return meta.SnapshotLatest;
			}
			return default(NetworkObjectHeaderSnapshotRef);
		}

		internal bool TryGetStruct(NetworkId id, out NetworkObjectMeta meta)
		{
			if (_metaLookup.TryGetValue(id, out meta))
			{
				Assert.Check(id == meta.Id, "id == meta.Id");
				Assert.Check(meta.Flags.Has(NetworkObjectHeaderFlags.Struct), "meta.Flags.Has(NetworkObjectHeaderFlags.Struct)");
				return true;
			}
			return false;
		}

		internal bool TryGetInstance(NetworkId id, out NetworkObject instance)
		{
			Assert.Check(!id.IsReserved, "id.IsReserved == false");
			if (_metaLookup.TryGetValue(id, out var value) && BehaviourUtils.IsAlive(value.Instance))
			{
				Assert.Check(id == value.Id, "id == meta.Id");
				instance = value.Instance;
				return true;
			}
			instance = null;
			return false;
		}

		internal unsafe ref T AllocateStruct<T>(NetworkId id, int extraWords = 0, NetworkObjectTypeId? objectTypeId = null) where T : unmanaged
		{
			NetworkObjectMeta networkObjectMeta = AllocateStruct(id, FusionUnsafe.MakeAligned(sizeof(T), 4) / 4 + extraWords, objectTypeId);
			return ref networkObjectMeta.GetStructData<T>();
		}

		internal NetworkObjectMeta AllocateStruct(NetworkId id, int words, NetworkObjectTypeId? objectTypeId = null)
		{
			Assert.Check(id.IsValid, "id.IsValid");
			InternalLogStreams.LogDebug?.Log(this, $"allocating struct {id} (words: {words})");
			int wordCount = 34 + words;
			return AllocateObject(id, wordCount, objectTypeId.GetValueOrDefault(), 0, default(NetworkId), default(NetworkObjectNestingKey), NetworkObjectHeaderFlags.Struct);
		}

		internal NetworkObjectMeta AllocateObject(NetworkId id, int wordCount, NetworkObjectTypeId type = default(NetworkObjectTypeId), int behaviourCount = 0, NetworkId nestingRoot = default(NetworkId), NetworkObjectNestingKey nestingKey = default(NetworkObjectNestingKey), NetworkObjectHeaderFlags flags = (NetworkObjectHeaderFlags)0)
		{
			return AllocateObject(new NetworkObjectHeader(id, (short)wordCount, (short)behaviourCount, type, nestingRoot, nestingKey, flags));
		}

		internal NetworkObjectMeta AllocateObject(in NetworkObjectHeader header)
		{
			Assert.Check(header.Id.IsValid, header.Id);
			int blockByteSize = _projectConfig.Heap.ToAllocatorConfig().BlockByteSize;
			Assert.Always(header.WordCount >= 20 && header.WordCount * 4 <= blockByteSize, "{0} >= NetworkObjectHeader.WORDS && {1} <= {2}", header.WordCount, header.WordCount * 4, blockByteSize);
			Assert.Always(!HasObject(header.Id), "id already exists: {0}", header.Id);
			NetworkObjectMeta networkObjectMeta = AcquireNetworkObjectMeta(in header);
			_metaLookup.Add(networkObjectMeta.Id, networkObjectMeta);
			HostMigrationAfterAllocateObject(networkObjectMeta);
			if (header.Flags.Has(NetworkObjectHeaderFlags.Struct))
			{
				_structs.Add(header.Id);
				_structsVersion++;
			}
			return networkObjectMeta;
		}

		internal int GetPlayerAuthorityMask(in NetworkObjectHeader header, PlayerRef player)
		{
			return AuthorityMasks.Create(IsStateAuthority(header.StateAuthority, player), IsInputAuthority(header.InputAuthority, player));
		}

		internal int GetLocalAuthorityMask([In][RequiresLocation] ref NetworkObjectHeader obj)
		{
			return AuthorityMasks.Create(IsLocalSimulationStateAuthority(ref obj), IsLocalSimulationInputAuthority(ref obj));
		}

		internal SimulationMessage AcquireSimulationMessage(in SimulationMessageHeader header)
		{
			SimulationMessage simulationMessage = _poolSimulationMessage.Get();
			Assert.Always(simulationMessage.RefCount == 0, "result.RefCount == 0");
			simulationMessage.RefCount = 1;
			simulationMessage.PoolInit(this, in header);
			InternalLogStreams.LogTracePools?.Log(this, $"reference acquired from the pool, 0x{simulationMessage.GetHashCode():X8}");
			return simulationMessage;
		}

		internal void AddReference(SimulationMessage obj)
		{
			Assert.Always(obj.RefCount > 0, "obj.RefCount > 0");
			obj.RefCount++;
			InternalLogStreams.LogTracePools?.Log(this, $"reference added ({obj.RefCount}, 0x{obj.GetHashCode():X8})");
		}

		internal bool ReleaseReference(SimulationMessage obj)
		{
			if (obj == null)
			{
				return false;
			}
			Assert.Always(obj.RefCount > 0, "Already released: 0x{0:X8}", obj.GetHashCode());
			if (--obj.RefCount > 0)
			{
				InternalLogStreams.LogTracePools?.Log(this, $"reference released ({obj.RefCount}, 0x{obj.GetHashCode():X8})");
				return false;
			}
			InternalLogStreams.LogTracePools?.Log(this, $"reference released ({obj.RefCount}, 0x{obj.GetHashCode():X8}), returning to the pool");
			obj.PoolReset(this);
			_poolSimulationMessage.Return(obj);
			return true;
		}

		internal SimulationPacketEnvelope AcquireSimulationPacketEnvelope(Tick tick)
		{
			SimulationPacketEnvelope simulationPacketEnvelope = _poolSimulationPacketEnvelope.Get();
			Assert.Always(simulationPacketEnvelope.RefCount == 0, "result.RefCount == 0");
			simulationPacketEnvelope.RefCount = 1;
			simulationPacketEnvelope.PoolInit(this, tick);
			InternalLogStreams.LogTracePools?.Log(this, $"reference acquired from the pool, 0x{simulationPacketEnvelope.GetHashCode():X8}");
			return simulationPacketEnvelope;
		}

		internal void AddReference(SimulationPacketEnvelope obj)
		{
			Assert.Always(obj.RefCount > 0, "obj.RefCount > 0");
			obj.RefCount++;
			InternalLogStreams.LogTracePools?.Log(this, $"reference added ({obj.RefCount}, 0x{obj.GetHashCode():X8})");
		}

		internal bool ReleaseReference(SimulationPacketEnvelope obj)
		{
			if (obj == null)
			{
				return false;
			}
			Assert.Always(obj.RefCount > 0, "Already released: 0x{0:X8}", obj.GetHashCode());
			if (--obj.RefCount > 0)
			{
				InternalLogStreams.LogTracePools?.Log(this, $"reference released ({obj.RefCount}, 0x{obj.GetHashCode():X8})");
				return false;
			}
			InternalLogStreams.LogTracePools?.Log(this, $"reference released ({obj.RefCount}, 0x{obj.GetHashCode():X8}), returning to the pool");
			obj.PoolReset(this);
			_poolSimulationPacketEnvelope.Return(obj);
			return true;
		}

		private NetworkObjectMeta AcquireNetworkObjectMeta(in NetworkObjectHeader header)
		{
			NetworkObjectMeta networkObjectMeta = _poolNetworkObjectMeta.Get();
			networkObjectMeta.PoolInit(this, in header);
			InternalLogStreams.LogTracePools?.Log(this, $"acquired from the pool, 0x{networkObjectMeta.GetHashCode():X8}");
			return networkObjectMeta;
		}

		private void Release(ref NetworkObjectMeta obj)
		{
			if (obj != null)
			{
				InternalLogStreams.LogTracePools?.Log(this, $"released (0x{obj.GetHashCode():X8}), returning to the pool");
				obj.PoolReset(this);
				_poolNetworkObjectMeta.Return(obj);
				obj = null;
			}
		}

		internal NetworkObjectHeaderSnapshot AcquireNetworkObjectHeaderSnapshot(int wordCount)
		{
			NetworkObjectHeaderSnapshot networkObjectHeaderSnapshot = _poolNetworkObjectHeaderSnapshot.Get();
			networkObjectHeaderSnapshot.PoolInit(this, wordCount);
			InternalLogStreams.LogTracePools?.Log(this, $"acquired from the pool, 0x{networkObjectHeaderSnapshot.GetHashCode():X8}");
			return networkObjectHeaderSnapshot;
		}

		internal void Release(ref NetworkObjectHeaderSnapshot obj)
		{
			if (obj != null)
			{
				InternalLogStreams.LogTracePools?.Log(this, $"released (0x{obj.GetHashCode():X8}), returning to the pool");
				obj.PoolReset(this);
				_poolNetworkObjectHeaderSnapshot.Return(obj);
				obj = null;
			}
		}

		private AreaOfInterestCell AcquireAreaOfInterestCell(int cellKey)
		{
			AreaOfInterestCell areaOfInterestCell = _poolAreaOfInterestCell.Get();
			areaOfInterestCell.PoolInit(this, cellKey);
			InternalLogStreams.LogTracePools?.Log(this, $"acquired from the pool, 0x{areaOfInterestCell.GetHashCode():X8}");
			return areaOfInterestCell;
		}

		private void Release(ref AreaOfInterestCell obj)
		{
			if (obj != null)
			{
				InternalLogStreams.LogTracePools?.Log(this, $"released (0x{obj.GetHashCode():X8}), returning to the pool");
				obj.PoolReset(this);
				_poolAreaOfInterestCell.Return(obj);
				obj = null;
			}
		}

		private SimulationInput AcquireSimulationInput(SimulationConfig config)
		{
			SimulationInput simulationInput = _poolSimulationInput.Get();
			simulationInput.PoolInit(this, config);
			InternalLogStreams.LogTracePools?.Log(this, $"acquired from the pool, 0x{simulationInput.GetHashCode():X8}");
			return simulationInput;
		}

		private void Release(ref SimulationInput obj)
		{
			if (obj != null)
			{
				InternalLogStreams.LogTracePools?.Log(this, $"released (0x{obj.GetHashCode():X8}), returning to the pool");
				obj.PoolReset(this);
				_poolSimulationInput.Return(obj);
				obj = null;
			}
		}

		internal PooledList<byte> AcquireByteList(int capacity)
		{
			_poolByteCount++;
			return new PooledList<byte>(_poolByte, capacity);
		}

		internal void Release(ref PooledList<byte> list)
		{
			if (list.IsValid)
			{
				_poolByteCount--;
				Assert.Check(_poolByteCount >= 0, "_poolByteCount >= 0");
				list.Dispose();
				list = default(PooledList<byte>);
			}
		}

		internal PooledList<NetworkObjectPacketData> AcquireNetworkObjectPacketDataList(int capacity)
		{
			_poolNetworkObjectPacketDataCount++;
			return new PooledList<NetworkObjectPacketData>(_poolNetworkObjectPacketData, capacity);
		}

		internal void Release(ref PooledList<NetworkObjectPacketData> list)
		{
			if (list.IsValid)
			{
				_poolNetworkObjectPacketDataCount--;
				Assert.Check(_poolNetworkObjectPacketDataCount >= 0, "_poolNetworkObjectPacketDataCount >= 0");
				list.Dispose();
				list = default(PooledList<NetworkObjectPacketData>);
			}
		}

		internal PooledList<SimulationMessagePacketData> AcquireSimulationMessagePacketDataList(int capacity)
		{
			_poolSimulationMessagePacketDataCount++;
			return new PooledList<SimulationMessagePacketData>(_poolSimulationMessagePacketData, capacity);
		}

		internal void Release(ref PooledList<SimulationMessagePacketData> list)
		{
			if (list.IsValid)
			{
				_poolSimulationMessagePacketDataCount--;
				Assert.Check(_poolSimulationMessagePacketDataCount >= 0, "_poolSimulationMessagePacketDataCount >= 0");
				list.Dispose();
				list = default(PooledList<SimulationMessagePacketData>);
			}
		}

		internal PooledList<SimulationConnection> AcquireSimulationConnectionList(int capacity)
		{
			_poolSimulationConnectionCount++;
			return new PooledList<SimulationConnection>(_poolSimulationConnection, capacity);
		}

		internal void Release(ref PooledList<SimulationConnection> list)
		{
			if (list.IsValid)
			{
				_poolSimulationConnectionCount--;
				Assert.Check(_poolSimulationConnectionCount >= 0, "_poolSimulationConnectionCount >= 0");
				list.Dispose();
				list = default(PooledList<SimulationConnection>);
			}
		}

		internal unsafe int* NativeAllocObjectData(int count)
		{
			_allocatedObjectDataBytes += count * 4;
			return Allocator.AllocAndClearArray<int>(_allocatorObjects, count);
		}

		internal unsafe void NativeFreeObjectData(ref int* ptr, int count)
		{
			if (Allocator.Free(_allocatorObjects, ref ptr))
			{
				_allocatedObjectDataBytes = Math.Max(0, _allocatedObjectDataBytes - count * 4);
			}
		}

		internal unsafe ChangeTick* NativeAllocObjectChanges(int count)
		{
			_allocatedObjectChangesBytes += count * sizeof(ChangeTick);
			return Allocator.AllocAndClearArray<ChangeTick>(_allocatorObjects, count);
		}

		internal unsafe void NativeFreeObjectChanges(ref ChangeTick* ptr, int count)
		{
			if (Allocator.Free(_allocatorObjects, ref ptr))
			{
				_allocatedObjectChangesBytes = Math.Max(0, _allocatedObjectChangesBytes - count * sizeof(ChangeTick));
			}
		}

		internal unsafe int* NativeAllocObjectSnapshot(int count)
		{
			_allocatedObjectSnapshotBytes += count * 4;
			return Allocator.AllocAndClearArray<int>(_allocatorObjects, count);
		}

		internal unsafe void NativeFreeObjectSnapshot(ref int* ptr, int count)
		{
			if (Allocator.Free(_allocatorObjects, ref ptr))
			{
				_allocatedObjectSnapshotBytes = Math.Max(0, _allocatedObjectSnapshotBytes - count * 4);
			}
		}

		internal unsafe int* NativeAllocInputData(int count)
		{
			_allocatedInputDataBytes += count * 4;
			return Allocator.AllocAndClearArray<int>(_allocator, count);
		}

		internal unsafe void NativeFreeInputData(ref int* ptr, int count)
		{
			if (Allocator.Free(_allocator, ref ptr))
			{
				_allocatedInputDataBytes = Math.Max(0, _allocatedInputDataBytes - count * 4);
			}
		}

		internal void UpdatePoolPerformanceCounters()
		{
			HostProfiler.Counters.SimulationMessageActive?.Add(_poolSimulationMessage.CountActive);
			HostProfiler.Counters.SimulationPacketEnvelopeActive?.Add(_poolSimulationPacketEnvelope.CountActive);
			HostProfiler.Counters.ByteLists?.Add(_poolByteCount);
			HostProfiler.Counters.NetworkObjectPacketDataLists?.Add(_poolNetworkObjectPacketDataCount);
			HostProfiler.Counters.SimulationMessagePacketDataLists?.Add(_poolSimulationMessagePacketDataCount);
			HostProfiler.Counters.SimulationConnectionLists?.Add(_poolSimulationConnectionCount);
			HostProfiler.Counters.NetworkObjectMetaActive?.Add(_poolNetworkObjectMeta.CountActive);
			HostProfiler.Counters.NetworkObjectHeaderSnapshotActive?.Add(_poolNetworkObjectHeaderSnapshot.CountActive);
			HostProfiler.Counters.AreaOfInterestCellActive?.Add(_poolAreaOfInterestCell.CountActive);
			HostProfiler.Counters.SimulationInputActive?.Add(_poolSimulationInput.CountActive);
			HostProfiler.Counters.ObjectDataBytes?.Add(_allocatedObjectDataBytes);
			HostProfiler.Counters.ObjectChangesBytes?.Add(_allocatedObjectChangesBytes);
			HostProfiler.Counters.ObjectSnapshotBytes?.Add(_allocatedObjectSnapshotBytes);
			HostProfiler.Counters.InputDataBytes?.Add(_allocatedInputDataBytes);
		}

		internal unsafe void SendInternalSimulationMessage<T>(SimulationMessageInternalTypes type, T buffer, PlayerRef? target = null) where T : unmanaged
		{
			Assert.Check(type > (SimulationMessageInternalTypes)0, "type > 0");
			SimulationConnection result;
			if (IsClient)
			{
				if (!TryGetSimulationConnectionByIndex(0, out result))
				{
					return;
				}
			}
			else
			{
				if (!target.HasValue)
				{
					Assert.Fail("Needs target");
				}
				if (!TryGetSimulationConnectionForPlayer(target.Value, out result))
				{
					return;
				}
			}
			SimulationMessageHeader header = default(SimulationMessageHeader);
			header.Flags = SimulationMessageHeaderFlags.Internal | SimulationMessageHeaderFlags.Reliable;
			header.MessageType = (uint)type;
			header.SourcePlayer = LocalPlayer;
			header.TargetPlayer = target ?? PlayerRef.None;
			header.PayloadNumBytes = sizeof(T);
			header.TargetObject = default(NetworkBehaviourId);
			Unsafe.SkipInit<ReliableKey>(out header.ReliableKey);
			SimulationMessage msg = AcquireSimulationMessage(in header);
			Assert.Check(msg.RefCount == 1, "msg.RefCount == 1");
			try
			{
				Unsafe.WriteUnaligned(ref msg.Payload[0], buffer);
				result.EnqueueMessage(in msg);
			}
			finally
			{
				ReleaseReference(msg);
			}
		}

		private bool RecvMessages(ref RecvContext rc)
		{
			Assert.Check(_pendingMessages.Count == 0, "_pendingMessages.Count == 0");
			try
			{
				int num = 0;
				while (rc.Read.Peek<SimulationMessageHeaderFlags>() != SimulationMessageHeaderFlags.None)
				{
					int offset = rc.Read.Offset;
					if (!SimulationMessageHeader.TryRead(rc.Read, out var header))
					{
						InternalLogStreams.LogTraceSimulationMessage?.Warn(this, $"Failed to read message {num}");
						return false;
					}
					HostProfiler.Counters.RpcIn?.Add(1);
					num++;
					int num2 = 0;
					if (header.Flags.Has(SimulationMessageHeaderFlags.TickAligned))
					{
						num2 = rc.Read.IntVar();
						Assert.Check(num2 > 0, "tick > 0");
					}
					ulong num3 = 0uL;
					if (header.Flags.Has(SimulationMessageHeaderFlags.Reliable))
					{
						num3 = rc.Read.ULongVar();
						Assert.Check(num3 != 0, "sequence > 0");
					}
					if (num3 != 0L && num3 <= rc.Connection.MessagesInSequenceV2)
					{
						InternalLogStreams.LogTraceSimulationMessage?.Log(this, $"Dropping (fast, min number: {rc.Connection.MessagesInSequenceV2}) {header}");
						rc.Read.Skip(header.PayloadNumBytes);
						continue;
					}
					SimulationMessage simulationMessage = AcquireSimulationMessage(in header);
					_pendingMessages.Enqueue(new SimulationMessagePacketData(simulationMessage, num2, num3));
					rc.Read.Span(simulationMessage.Payload);
					HostProfiler.Counters.RpcBytesIn?.Add(rc.Read.Offset - offset);
				}
				SimulationMessagePacketData result;
				while (_pendingMessages.TryDequeue(out result))
				{
					if (!rc.Connection.MessagesInV2.TryInsert(result.Sequence, (result.Message, result.Tick)))
					{
						InternalLogStreams.LogTraceSimulationMessage?.Log(this, $"Dropping (slow, already a message with this number) {LogUtils.GetDump(result.Message)} (seq:{result.Sequence})");
						ReleaseReference(result.Message);
					}
					else
					{
						InternalLogStreams.LogTraceSimulationMessage?.Log(this, $"Enqueued {LogUtils.GetDump(result.Message)} (seq:{result.Sequence})");
					}
				}
				rc.Read.Byte();
				return true;
			}
			finally
			{
				DrainPendingMessages();
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void DrainPendingMessages()
		{
			SimulationMessagePacketData result;
			while (_pendingMessages.TryDequeue(out result))
			{
				ReleaseReference(result.Message);
			}
		}

		private int WriteMessages(ref SendContext context)
		{
			ref Queue<SimulationMessagePacketData> messagesOutV = ref context.Connection.MessagesOutV2;
			int num = 0;
			int offset = context.Write.Offset;
			if (messagesOutV.Count > 0)
			{
				InternalLogStreams.LogTraceSimulationMessage?.Log(this, $"Going to write {messagesOutV.Count} messages for {context.Player}");
			}
			try
			{
				SimulationMessagePacketData result;
				while (messagesOutV.TryPeek(out result))
				{
					num++;
					SimulationMessage message = result.Message;
					InternalLogStreams.LogTraceSimulationMessage?.Log(this, $"Writing for {context.Player}: {LogUtils.GetDump(result.Message)} (seq:{result.Sequence})");
					SimulationMessageHeader.Write(context.Write, in message.Header);
					if (message.Header.Flags.Has(SimulationMessageHeaderFlags.TickAligned))
					{
						Assert.Check(result.Tick > 0, "entry.Tick > 0");
						context.Write.IntVar(result.Tick);
					}
					if (message.Header.Flags.Has(SimulationMessageHeaderFlags.Reliable))
					{
						Assert.Check(result.Sequence != 0, "entry.Sequence > 0");
						context.Write.ULongVar(result.Sequence);
					}
					context.Write.Span(message.Payload);
					if (message.Header.Flags.Has(SimulationMessageHeaderFlags.Reliable))
					{
						context.TrackMessage(result);
					}
					else
					{
						ReleaseReference(message);
					}
					HostProfiler.Counters.RpcBytesOut?.Add(context.Write.Offset - offset);
					messagesOutV.Dequeue();
				}
			}
			catch
			{
				if (messagesOutV.Count > 0)
				{
					InternalLogStreams.LogTraceSimulationMessage?.Warn(this, $"Going to drop {messagesOutV.Count} messages for {context.Player} due to an exception");
				}
				throw;
			}
			finally
			{
				SimulationMessagePacketData result2;
				while (messagesOutV.TryDequeue(out result2))
				{
					ReleaseReference(result2.Message);
				}
			}
			HostProfiler.Counters.RpcBytesOut?.Add(context.Write.Offset - offset);
			HostProfiler.Counters.RpcOut?.Add(num);
			context.Write.IntVar(0);
			return num;
		}

		private unsafe void DeliverMessages(int tick)
		{
			using (HostProfiler.Markers.DeliverMessages())
			{
				NetConnectionMap.Iterator iterator = NetPeerGroup.ConnectionIterator(_netPeerGroup);
				while (iterator.Next())
				{
					if (iterator.Current->ConnectionStatus != NetConnectionStatus.Connected || !TryGetSimulationConnection(iterator.Current, out var result))
					{
						continue;
					}
					int i;
					for (i = 0; i < result.MessagesInV2.Count; i++)
					{
						(ulong sequence, (SimulationMessage, int) value) tuple = result.MessagesInV2[i];
						(SimulationMessage, int) item = tuple.value;
						var (num, _) = tuple;
						var (msg, num2) = item;
						if (num2 > tick)
						{
							InternalLogStreams.LogTraceSimulationMessage?.Log(this, $"Not handling RPC: tick {num2} > {tick} (player: {LocalPlayer}) {LogUtils.GetDump(msg)}");
							break;
						}
						if (num != 0L && num != result.MessagesInSequenceV2 + 1)
						{
							InternalLogStreams.LogTraceSimulationMessage?.Log(this, $"Not handling RPC: (seq:{num}) != {result.MessagesInSequenceV2 + 1} (player: {LocalPlayer}) {LogUtils.GetDump(msg)}");
							break;
						}
						if (msg.Header.Flags.Has(SimulationMessageHeaderFlags.Internal))
						{
							OnMessageInternal(msg.Header.SourcePlayer, (SimulationMessageInternalTypes)msg.Header.MessageType, msg.Payload);
						}
						else
						{
							SimulationMessageResult simulationMessageResult = HandleMessage(in msg.Header, num2, msg.Payload);
							try
							{
								InternalLogStreams.LogTraceSimulationMessage?.Log(this, $"RPC Handle Message {msg.Header} (seq:{num}): {simulationMessageResult}");
								if (simulationMessageResult.IsRetry)
								{
									if (num == 0)
									{
										InternalLogStreams.LogTraceSimulationMessage?.Log(this, $"Unreliable will be retried (player: {LocalPlayer}) {LogUtils.GetDump(msg)}");
										continue;
									}
									InternalLogStreams.LogTraceSimulationMessage?.Log(this, $"Reliable RPC {num} will be retried (player: {LocalPlayer}) {LogUtils.GetDump(msg)}");
									break;
								}
								if (simulationMessageResult.IsForward)
								{
									int num3 = 0;
									bool flag = msg.Header.Flags.Has(SimulationMessageHeaderFlags.AllowRoundtrip);
									PooledList<SimulationConnection> targetConnections = simulationMessageResult.TargetConnections;
									Span<SimulationConnection> span = targetConnections.AsSpan();
									for (int j = 0; j < span.Length; j++)
									{
										SimulationConnection simulationConnection = span[j];
										if (flag || !(simulationConnection.Player == msg.Header.SourcePlayer))
										{
											simulationConnection.EnqueueMessage(in msg);
											num3++;
										}
									}
									InternalLogStreams.LogTraceSimulationMessage?.Info(this, $"Forwarded to {num3} clients {LogUtils.GetDump(msg)}");
								}
							}
							finally
							{
								((IDisposable)simulationMessageResult/*cast due to .constrained prefix*/).Dispose();
							}
						}
						if (num != 0)
						{
							Assert.Check(result.MessagesInSequenceV2 + 1 == num, "connection.MessagesInSequenceV2 + 1 == sequence");
							result.MessagesInSequenceV2 = num;
						}
						ReleaseReference(msg);
						result.MessagesInV2.MarkForCleanup(i);
					}
					result.MessagesInV2.CleanUp(0, i);
				}
			}
		}

		private unsafe SimulationMessageResult HandleMessage(in SimulationMessageHeader header, int msgTick, ReadOnlySpan<byte> payload)
		{
			bool isValid = header.TargetPlayer.IsValid;
			bool flag = true;
			bool flag2 = false;
			if (isValid)
			{
				PlayerRef realPlayer = GetRealPlayer(header.TargetPlayer);
				if (realPlayer == LocalPlayer)
				{
					flag2 = true;
				}
				else
				{
					flag = false;
					if (!IsServer)
					{
						InternalLogStreams.LogError?.Log(this, $"Peers should only receive messages targeted at them, but received. {header}");
						return Error(SimulationMessageResultCode.InvalidTargetPlayer);
					}
				}
			}
			NetworkObjectMeta meta = null;
			if (header.TargetObject.IsValid)
			{
				if (!TryGetMeta(header.TargetObject.Object, out meta))
				{
					InternalLogStreams.LogDebug?.Warn(this, $"Simulation message target object not found. {header}");
					return Error(SimulationMessageResultCode.InvalidTargetObject);
				}
				if (header.TargetObject.Behaviour < 0 || header.TargetObject.Behaviour >= meta.BehaviourCount)
				{
					InternalLogStreams.LogDebug?.Warn(this, $"Simulation message target behaviour not found. {header}");
					return Error(SimulationMessageResultCode.InvalidTargetBehaviour);
				}
			}
			int num;
			if (!NetworkTypesMeta.Instance.TryGetRpcMeta(header.MessageType, out var meta2))
			{
				if (!IsForwardingUnregisteredRpcsEnabled)
				{
					InternalLogStreams.LogDebug?.Error(this, $"Rpc {header.MessageType} not recognized. {header}");
					return Error(SimulationMessageResultCode.InvalidRpcKey);
				}
				if (IsServer && header.SourcePlayer == default(PlayerRef))
				{
					return Error(SimulationMessageResultCode.InsufficientSourceAuthority);
				}
				num = 7;
			}
			else
			{
				if (meta != null && (!(header.SourcePlayer == default(PlayerRef)) || !IsClient))
				{
					int num2 = AuthorityMasks.Create(IsStateAuthority(meta, header.SourcePlayer), IsInputAuthority(meta, header.SourcePlayer));
					if (((uint)meta2.Sources & (uint)num2) == 0)
					{
						InternalLogStreams.LogDebug?.Error(this, $"{header.SourcePlayer} sent rpc {header.MessageType} to {header.TargetObject} but is not allowed. {header}");
						return Error(SimulationMessageResultCode.InsufficientSourceAuthority);
					}
				}
				num = (int)meta2.Targets;
				if (flag && meta != null)
				{
					int num3 = AuthorityMasks.Create(IsLocalSimulationStateAuthority(ref meta.Header), IsLocalSimulationInputAuthority(ref meta.Header));
					if (((uint)meta2.Targets & (uint)num3) != 0)
					{
						num = (int)((uint)meta2.Targets & (uint)(~num3)) | (int)(meta2.Targets & RpcTargets.Proxies);
					}
					else
					{
						if (isValid)
						{
							InternalLogStreams.LogDebug?.Error(this, $"Not invoked locally because masks don't match: {meta2.Targets} vs {num3} {header}");
							return Error(SimulationMessageResultCode.InsufficientTargetAuthority);
						}
						flag = false;
					}
				}
				if (flag)
				{
					NetworkBehaviour networkBehaviour = null;
					if (meta != null)
					{
						if (BehaviourUtils.IsNotAlive(meta.Instance))
						{
							if (meta.LocalFlags.HasNot(NetworkObjectMetaFlags.InstanceWillNotBeCreated))
							{
								return Retry();
							}
							return Error(SimulationMessageResultCode.InstanceDoesNotExist);
						}
						Assert.Check(meta.BehaviourCount == meta.Instance.NetworkedBehaviours?.Length, "obj.BehaviourCount == obj.Instance.NetworkedBehaviours?.Length");
						networkBehaviour = meta.Instance.NetworkedBehaviours[header.TargetObject.Behaviour];
						if (BehaviourUtils.IsNotAlive(networkBehaviour))
						{
							InternalLogStreams.LogWarn?.Log(this, $"Behaviour {header.TargetObject.Behaviour} is not alive {header}");
							return Error(SimulationMessageResultCode.InstanceDoesNotExist);
						}
						if (!meta2.IsInstanceOfDeclaringType(networkBehaviour))
						{
							InternalLogStreams.LogWarn?.Log(this, $"Target behaviour {header.TargetObject.Behaviour} is not of the correct type ({meta2.DeclaringType}) {header}");
							return Error(SimulationMessageResultCode.InstanceTypeInvalid);
						}
					}
					try
					{
						bool flag3 = true;
						bool* validationResult = null;
						RpcInvokeContext context = new RpcInvokeContext(Runner, networkBehaviour, msgTick, header.SourcePlayer, header.TargetPlayer, payload, validationResult, networkBehaviour?.GetType());
						meta2.Invoke?.Invoke(in context);
						if (!flag3)
						{
							InternalLogStreams.LogTraceSimulationMessage?.Log(this, $"Handled locally {header}");
							return Handled();
						}
					}
					catch (Exception error)
					{
						InternalLogStreams.LogException?.Log(this, error);
						return Error(SimulationMessageResultCode.LocalInvokeThrewException);
					}
				}
			}
			if (IsClient)
			{
				return Handled();
			}
			if (flag2 || num == 0)
			{
				return Handled();
			}
			if (isValid)
			{
				PlayerRef realPlayer2 = GetRealPlayer(header.TargetPlayer);
				if (!TryGetSimulationConnectionForPlayer(realPlayer2, out var result))
				{
					return Error(SimulationMessageResultCode.InvalidTargetPlayer);
				}
				if (meta != null && !HasObjectInterest(result, meta))
				{
					InternalLogStreams.LogTraceSimulationMessage?.Warn(this, $"Can't forward to target {header.TargetPlayer}: not in the AOI {header}");
					return Error(SimulationMessageResultCode.TargetObjectNotInPlayerInterest);
				}
				if (meta != null && !HasObjectAuthority(result, meta, num))
				{
					InternalLogStreams.LogTraceSimulationMessage?.Warn(this, $"Can't forward to target {header.TargetPlayer}: insufficient state authority {header}");
					return Error(SimulationMessageResultCode.InsufficientTargetAuthority);
				}
				return ForwardToTarget(result);
			}
			return Forward(meta, num);
			SimulationMessageResult Error(SimulationMessageResultCode value)
			{
				return new SimulationMessageResult(value, this, default(PooledList<SimulationConnection>));
			}
			SimulationMessageResult Forward(NetworkObjectMeta obj, int forwardAuthorityMask)
			{
				PooledList<SimulationConnection> list = AcquireSimulationConnectionList(1);
				GetSimulationConnections(ref list, obj, forwardAuthorityMask);
				return new SimulationMessageResult(SimulationMessageResultCode.Forward, this, list);
			}
			SimulationMessageResult ForwardToTarget(SimulationConnection connection)
			{
				PooledList<SimulationConnection> targetConnections = AcquireSimulationConnectionList(1);
				targetConnections.Add(connection);
				return new SimulationMessageResult(SimulationMessageResultCode.Forward, this, targetConnections);
			}
			static SimulationMessageResult Handled()
			{
				return default(SimulationMessageResult);
			}
			SimulationMessageResult Retry()
			{
				return new SimulationMessageResult(SimulationMessageResultCode.Retry, this, default(PooledList<SimulationConnection>));
			}
		}

		internal bool HasObjectAuthority(SimulationConnection connection, NetworkObjectMeta obj, int authorityMask)
		{
			Assert.Check(obj, "obj");
			if (authorityMask == 7)
			{
				return true;
			}
			int playerAuthorityMask = GetPlayerAuthorityMask(in obj.Header, connection.Player);
			if ((playerAuthorityMask & authorityMask) == 0)
			{
				return false;
			}
			return true;
		}

		internal bool HasObjectInterest(SimulationConnection connection, NetworkObjectMeta obj)
		{
			Assert.Check(obj, "obj");
			Assert.Check(connection, "connection");
			if (connection.ObjectData_IsCreateUnconfirmed(obj.Id) == true)
			{
				return true;
			}
			if (IsServer)
			{
				return Replicator.HasObjectInterest(connection, obj);
			}
			return true;
		}

		internal int GetSimulationConnections(ref PooledList<SimulationConnection> list, NetworkObjectMeta targetObject = null, int authorityMask = 0)
		{
			if (authorityMask == 0)
			{
				return 0;
			}
			if (IsClient)
			{
				if (!TryGetSimulationConnectionByIndex(0, out var result))
				{
					return 0;
				}
				list.Add(result);
				return 1;
			}
			int num = 0;
			SimulationConnection simulationConnection = null;
			SimulationConnection simulationConnection2 = null;
			if (targetObject != null && authorityMask != 7)
			{
				if ((authorityMask & 1) != 0 && targetObject.StateAuthority != default(PlayerRef) && TryGetSimulationConnectionForPlayer(GetRealPlayer(targetObject.StateAuthority), out var result2))
				{
					simulationConnection = result2;
					if (HasObjectInterest(result2, targetObject))
					{
						list.Add(result2);
						num++;
					}
				}
				if ((authorityMask & 2) != 0 && targetObject.InputAuthority != default(PlayerRef) && TryGetSimulationConnectionForPlayer(GetRealPlayer(targetObject.InputAuthority), out var result3) && result3 != simulationConnection)
				{
					simulationConnection2 = result3;
					if (HasObjectInterest(result3, targetObject))
					{
						list.Add(result3);
						num++;
					}
				}
				if ((authorityMask & 4) == 0)
				{
					return num;
				}
			}
			foreach (var (_, simulationConnection4) in _connections)
			{
				if (targetObject == null || (simulationConnection4 != simulationConnection && simulationConnection4 != simulationConnection2 && HasObjectInterest(simulationConnection4, targetObject)))
				{
					list.Add(simulationConnection4);
					num++;
				}
			}
			return num;
		}

		internal Span<byte> GetLargeRpcWriteBuffer(int payloadNumBytes, bool clear)
		{
			if (payloadNumBytes < 0)
			{
				throw new ArgumentOutOfRangeException("payloadNumBytes");
			}
			if (_largeRpcWriteBuffer == null || _largeRpcWriteBuffer.Length < payloadNumBytes)
			{
				_largeRpcWriteBuffer = new byte[Maths.NextPowerOfTwo((uint)payloadNumBytes)];
			}
			Span<byte> span = _largeRpcWriteBuffer.AsSpan();
			if (clear)
			{
				FusionUnsafe.Clear(span);
			}
			return span;
		}

		private void HostMigrationAfterFreeObject(NetworkObjectMeta meta)
		{
			if (meta.Type.IsSceneObject)
			{
				_metaSceneLookup.Remove(meta.Type);
			}
			if (Mode == SimulationModes.Host)
			{
				_metaMigration.Remove(meta);
				_metaMigrationRemoved.Enqueue(meta.Id);
			}
		}

		private void HostMigrationAfterAllocateObject(NetworkObjectMeta meta)
		{
			if (meta.Type.IsSceneObject)
			{
				_metaSceneLookup[meta.Type] = meta;
			}
			if (Mode == SimulationModes.Host)
			{
				_metaMigration.AddLast(meta);
			}
		}

		private void HostMigrationDispose()
		{
			if (this is Server server)
			{
				server.DisposeHostMigration();
			}
		}

		internal bool TryGetSceneInstance(NetworkObjectTypeId sceneObjectTypeId, out NetworkObject instance)
		{
			Assert.Check(sceneObjectTypeId.IsSceneObject, "sceneObjectTypeId.IsSceneObject");
			if (_metaSceneLookup.TryGetValue(sceneObjectTypeId, out var value) && BehaviourUtils.IsAlive(value.Instance))
			{
				instance = value.Instance;
				return true;
			}
			instance = null;
			return false;
		}
	}
}
