using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using FishNet.Connection;
using FishNet.Serializing;
using FishNet.Transporting;
using GameKit.Utilities;
using UnityEngine;

namespace FishNet.Managing.Timing
{
	[DisallowMultipleComponent]
	[AddComponentMenu("FishNet/Manager/TimeManager")]
	public sealed class TimeManager : MonoBehaviour
	{
		private enum TimingType
		{
			Tick = 0,
			Variable = 1
		}

		private enum UpdateOrder : byte
		{
			BeforeTick = 0,
			AfterTick = 1
		}

		private enum TimingUpdateChange
		{
			JustRight = 0,
			TooFast = 1,
			TooSlow = -1
		}

		internal uint LastOrderedPacketTick;

		[Tooltip("When to invoke OnUpdate and other Unity callbacks relayed by the TimeManager.")]
		[SerializeField]
		private UpdateOrder _updateOrder;

		[Tooltip("Timing for sending and receiving data.")]
		[SerializeField]
		private TimingType _timingType;

		[Tooltip("While true clients may drop local ticks if their devices are unable to maintain the tick rate. This could result in a temporary desynchronization but will prevent the client falling further behind on ticks by repeatedly running the logic cycle multiple times per frame.")]
		[SerializeField]
		private bool _allowTickDropping;

		[Tooltip("Maximum number of ticks which may occur in a single frame before remainder are dropped for the frame.")]
		[Range(1f, 25f)]
		[SerializeField]
		private byte _maximumFrameTicks = 2;

		[Tooltip("How many times per second the server will simulate. This does not limit server frame rate.")]
		[Range(1f, 240f)]
		[SerializeField]
		private ushort _tickRate = 30;

		[Tooltip("How often in seconds to a connections ping. This is also responsible for approximating server tick. This value does not affect prediction.")]
		[Range(1f, 15f)]
		[SerializeField]
		private byte _pingInterval = 1;

		[Tooltip("How to perform physics.")]
		[SerializeField]
		private PhysicsMode _physicsMode;

		private uint _clientTicks;

		private uint _lastUpdateTicks;

		private uint _localTick;

		private Stopwatch _pingStopwatch = new Stopwatch();

		private uint _pingTicks;

		private MovingAverage _pingAverage = new MovingAverage(5);

		private double _elapsedTickTime;

		private NetworkManager _networkManager;

		private double _adjustedTickDelta;

		private double[] _clientTimingRange;

		private int _lastIncomingIterationFrame = -1;

		private bool _receivedPong = true;

		private float _lastMultipleTicksTime;

		private static uint _manualPhysics;

		private float _timingTooFastCount;

		private bool _fixedUpdateTimeStep;

		internal const float TIMING_INTERVAL = 1f;

		public const uint UNSET_TICK = 0u;

		private const float CLIENT_TIMING_PERCENT_RANGE = 0.5f;

		private const double CLIENT_SPEEDUP_VALUE = 0.035;

		private const double CLIENT_SLOWDOWN_VALUE = 0.02;

		private const string SAVED_FIXED_TIME_TEXT = "SavedFixedTimeFN";

		private TimingUpdateChange _timingUpdateChange;

		private float _updateChangeMultiplier = 1f;

		public long RoundTripTime { get; private set; }

		internal bool LowFrameRate => Time.unscaledTime - _lastMultipleTicksTime < 1f;

		public uint LastPacketTick { get; private set; }

		public uint Tick { get; internal set; }

		[HideInInspector]
		public double TickDelta { get; private set; }

		public bool FrameTicked { get; private set; }

		public float ServerUptime { get; private set; }

		public float ClientUptime { get; private set; }

		public ushort TickRate
		{
			get
			{
				return _tickRate;
			}
			private set
			{
				_tickRate = value;
			}
		}

		public byte PingInterval => _pingInterval;

		public PhysicsMode PhysicsMode => _physicsMode;

		public uint LocalTick
		{
			get
			{
				if (!_networkManager.IsServer)
				{
					return _localTick;
				}
				return Tick;
			}
			private set
			{
				_localTick = value;
			}
		}

		internal byte RESET_ADJUSTMENT_THRESHOLD => (byte)Mathf.Max(3, TickRate / 3);

		public event Action<long> OnRoundTripTimeUpdated;

		public event Action OnPreTick;

		public event Action OnTick;

		public event Action<float> OnPrePhysicsSimulation;

		public event Action<float> OnPostPhysicsSimulation;

		public event Action OnPostTick;

		public event Action OnUpdate;

		public event Action OnLateUpdate;

		public event Action OnFixedUpdate;

		internal void SetLastPacketTick(uint tick)
		{
			if (tick > LastPacketTick)
			{
				LastOrderedPacketTick = tick;
			}
			LastPacketTick = tick;
		}

		internal void TickFixedUpdate()
		{
			this.OnFixedUpdate?.Invoke();
			if (PhysicsMode == PhysicsMode.Unity)
			{
				if (_fixedUpdateTimeStep)
				{
					this.OnPostPhysicsSimulation?.Invoke(Time.fixedDeltaTime);
				}
				_fixedUpdateTimeStep = true;
				this.OnPrePhysicsSimulation?.Invoke(Time.fixedDeltaTime);
			}
		}

		internal void TickUpdate()
		{
			if (_networkManager.IsServer)
			{
				ServerUptime += Time.deltaTime;
			}
			if (_networkManager.IsClient)
			{
				ClientUptime += Time.deltaTime;
			}
			if (_updateOrder == UpdateOrder.BeforeTick)
			{
				this.OnUpdate?.Invoke();
				MethodLogic();
			}
			else
			{
				MethodLogic();
				this.OnUpdate?.Invoke();
			}
			void MethodLogic()
			{
				IncreaseTick();
				if (PhysicsMode == PhysicsMode.Unity && _fixedUpdateTimeStep)
				{
					_fixedUpdateTimeStep = false;
					this.OnPostPhysicsSimulation?.Invoke(Time.fixedDeltaTime);
				}
			}
		}

		internal void TickLateUpdate()
		{
			this.OnLateUpdate?.Invoke();
		}

		internal void InitializeOnce_Internal(NetworkManager networkManager)
		{
			_networkManager = networkManager;
			SetInitialValues();
			_networkManager.ServerManager.OnServerConnectionState += ServerManager_OnServerConnectionState;
			_networkManager.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
			AddNetworkLoops();
		}

		private void AddNetworkLoops()
		{
			if (!base.gameObject.TryGetComponent<NetworkWriterLoop>(out var _))
			{
				base.gameObject.AddComponent<NetworkWriterLoop>();
			}
			if (!base.gameObject.TryGetComponent<NetworkReaderLoop>(out var _))
			{
				base.gameObject.AddComponent<NetworkReaderLoop>();
			}
		}

		private void ClientManager_OnClientConnectionState(ClientConnectionStateArgs obj)
		{
			if (obj.ConnectionState != LocalConnectionState.Started)
			{
				_pingStopwatch.Stop();
				ClientUptime = 0f;
				if (!_networkManager.IsServer)
				{
					LocalTick = 0u;
					Tick = 0u;
					SetTickRate(TickRate);
					_timingTooFastCount = 0f;
				}
			}
			else
			{
				_pingStopwatch.Restart();
			}
		}

		private void ServerManager_OnServerConnectionState(ServerConnectionStateArgs obj)
		{
			if (!_networkManager.ServerManager.AnyServerStarted())
			{
				ServerUptime = 0f;
				Tick = 0u;
			}
		}

		private void SetInitialValues()
		{
			SetTickRate(TickRate);
			InitializePhysicsMode(PhysicsMode);
		}

		private void UnsetSimulationSettings()
		{
			SetAutomaticPhysicsSimulation(automatic: true);
			float num = PlayerPrefs.GetFloat("SavedFixedTimeFN", float.MinValue);
			if (num != float.MinValue)
			{
				Time.fixedDeltaTime = num;
			}
		}

		private void SetAutomaticPhysicsSimulation(bool automatic)
		{
			if (automatic)
			{
				Physics.simulationMode = SimulationMode.FixedUpdate;
				Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
			}
			else
			{
				Physics.simulationMode = SimulationMode.Script;
				Physics2D.simulationMode = SimulationMode2D.Script;
			}
		}

		private void InitializePhysicsMode(PhysicsMode mode)
		{
			switch (mode)
			{
			case PhysicsMode.Disabled:
				SetPhysicsMode(mode);
				break;
			case PhysicsMode.TimeManager:
				Time.fixedDeltaTime = (float)TickDelta;
				if (_networkManager != null)
				{
					if (_manualPhysics != 0)
					{
						_networkManager.LogError("There are multiple TimeManagers instantiated which are using manual physics. Manual physics with multiple TimeManagers is not supported.");
					}
					_manualPhysics++;
				}
				SetPhysicsMode(mode);
				break;
			default:
				SetPhysicsMode(mode);
				break;
			}
		}

		public void SetPhysicsMode(PhysicsMode mode)
		{
			_physicsMode = mode;
			if (mode == PhysicsMode.Disabled || mode == PhysicsMode.TimeManager)
			{
				SetAutomaticPhysicsSimulation(automatic: false);
			}
			else
			{
				SetAutomaticPhysicsSimulation(automatic: true);
			}
		}

		internal void ModifyPing(uint clientTick)
		{
			uint num = LocalTick - clientTick;
			_pingAverage.ComputeAverage(num);
			double a = (double)_pingAverage.Average * TickDelta * 1000.0;
			RoundTripTime = (long)Math.Round(a);
			_receivedPong = true;
			this.OnRoundTripTimeUpdated?.Invoke(RoundTripTime);
		}

		private void TrySendPing(uint? tickOverride = null)
		{
			byte pingInterval = PingInterval;
			long num = pingInterval * 1000;
			float num2 = (_receivedPong ? 1f : 1.5f);
			num = (long)((float)num * num2);
			uint num3 = TimeToTicks((float)(int)pingInterval * num2);
			_pingTicks++;
			if (_pingTicks >= num3 && _pingStopwatch.ElapsedMilliseconds >= num)
			{
				_pingTicks = 0u;
				_pingStopwatch.Restart();
				_receivedPong = false;
				uint value = ((!tickOverride.HasValue) ? LocalTick : tickOverride.Value);
				PooledWriter pooledWriter = WriterPool.Retrieve();
				pooledWriter.WritePacketId(PacketId.PingPong);
				pooledWriter.WriteTickUnpacked(value);
				_networkManager.TransportManager.SendToServer(1, pooledWriter.GetArraySegment());
				pooledWriter.Store();
			}
		}

		internal void SendPong(NetworkConnection conn, uint clientTick)
		{
			if (conn.IsActive && conn.Authenticated)
			{
				PooledWriter pooledWriter = WriterPool.Retrieve();
				pooledWriter.WritePacketId(PacketId.PingPong);
				pooledWriter.WriteTickUnpacked(clientTick);
				conn.SendToClient(1, pooledWriter.GetArraySegment());
				pooledWriter.Store();
			}
		}

		private void IncreaseTick()
		{
			bool isClient = _networkManager.IsClient;
			bool isServer = _networkManager.IsServer;
			double tickDelta = TickDelta;
			double num = (isServer ? tickDelta : _adjustedTickDelta);
			if (num == 0.0)
			{
				UnityEngine.Debug.LogWarning("Simulation delta cannot be 0. Network timing will not continue.");
				return;
			}
			double num2 = Time.unscaledDeltaTime;
			_elapsedTickTime += num2;
			FrameTicked = _elapsedTickTime >= num;
			int num3 = Mathf.FloorToInt((float)(_elapsedTickTime / num));
			if (num3 > 1)
			{
				_lastMultipleTicksTime = Time.unscaledDeltaTime;
			}
			if (_allowTickDropping && !_networkManager.IsServer && num3 > _maximumFrameTicks)
			{
				_elapsedTickTime = num * (double)(int)_maximumFrameTicks;
			}
			bool flag = _timingType == TimingType.Variable;
			bool frameTicked = FrameTicked;
			do
			{
				if (frameTicked)
				{
					_elapsedTickTime -= num;
					this.OnPreTick?.Invoke();
				}
				if (frameTicked || flag)
				{
					TryIterateData(incoming: true);
				}
				if (frameTicked)
				{
					this.OnTick?.Invoke();
					if (PhysicsMode == PhysicsMode.TimeManager)
					{
						float num4 = (float)TickDelta;
						this.OnPrePhysicsSimulation?.Invoke(num4);
						Physics.Simulate(num4);
						Physics2D.Simulate(num4);
						this.OnPostPhysicsSimulation?.Invoke(num4);
					}
					this.OnPostTick?.Invoke();
					if (isClient && _elapsedTickTime < num)
					{
						_networkManager.ClientManager.TrySendLodUpdate(LocalTick, forceFullUpdate: false);
						TrySendPing(LocalTick + 1);
					}
					if (_networkManager.IsServer)
					{
						SendTimingAdjustment();
					}
				}
				if (frameTicked || flag)
				{
					TryIterateData(incoming: false);
				}
				if (frameTicked)
				{
					if (_networkManager.IsClient)
					{
						_clientTicks++;
					}
					Tick++;
					LocalTick++;
					_networkManager.ObserverManager.CalculateLevelOfDetail(LocalTick);
				}
			}
			while (_elapsedTickTime >= num);
		}

		public double GetTickPercent()
		{
			return _elapsedTickTime / TickDelta * 100.0;
		}

		public PreciseTick GetPreciseTick(uint tick)
		{
			double percent = _elapsedTickTime / TickDelta * 100.0;
			return new PreciseTick(tick, percent);
		}

		public PreciseTick GetPreciseTick(TickType tickType)
		{
			if (_networkManager == null)
			{
				return default(PreciseTick);
			}
			switch (tickType)
			{
			case TickType.Tick:
				return GetPreciseTick(Tick);
			case TickType.LocalTick:
				return GetPreciseTick(LocalTick);
			case TickType.LastPacketTick:
				return GetPreciseTick(LastPacketTick);
			default:
				_networkManager.LogError("TickType " + tickType.ToString() + " is unhandled.");
				return default(PreciseTick);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double TicksToTime(TickType tickType = TickType.LocalTick)
		{
			switch (tickType)
			{
			case TickType.LocalTick:
				return TicksToTime(LocalTick);
			case TickType.Tick:
				return TicksToTime(Tick);
			case TickType.LastPacketTick:
				return TicksToTime(LastPacketTick);
			default:
				_networkManager.LogError($"TickType {tickType} is unhandled.");
				return 0.0;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double TicksToTime(PreciseTick pt)
		{
			double num = TicksToTime(pt.Tick);
			double num2 = pt.Percent / 100.0 * TickDelta;
			return num + num2;
		}

		public double TicksToTime(uint ticks)
		{
			return TickDelta * (double)ticks;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double TimePassed(uint currentTick, uint previousTick)
		{
			double num;
			double num2;
			if (currentTick >= previousTick)
			{
				num = 1.0;
				num2 = TicksToTime(currentTick - previousTick);
			}
			else
			{
				num = -1.0;
				num2 = TicksToTime(previousTick - currentTick);
			}
			return num2 * num;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double TimePassed(PreciseTick preciseTick, bool allowNegative = false)
		{
			PreciseTick preciseTick2 = GetPreciseTick(TickType.Tick);
			long num = preciseTick2.Tick - preciseTick.Tick;
			double num2 = preciseTick2.Percent - preciseTick.Percent;
			bool flag = num < 0 || (num <= 0 && num2 <= 0.0);
			if (!allowNegative && flag)
			{
				return 0.0;
			}
			double num3 = TimePassed(preciseTick.Tick, allowNegative: true);
			double num4 = num2 / 100.0 * TickDelta;
			return num3 + num4;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double TimePassed(uint previousTick, bool allowNegative = false)
		{
			uint tick = Tick;
			if (tick >= previousTick)
			{
				return TicksToTime(tick - previousTick);
			}
			if (!allowNegative)
			{
				return 0.0;
			}
			return TicksToTime(previousTick - tick) * -1.0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint TimeToTicks(double time, TickRounding rounding = TickRounding.RoundNearest)
		{
			double num = time / TickDelta;
			return rounding switch
			{
				TickRounding.RoundNearest => (uint)Math.Round(num), 
				TickRounding.RoundDown => (uint)Math.Floor(num), 
				_ => (uint)Math.Ceiling(num), 
			};
		}

		public uint TickToLocalTick(uint tick)
		{
			if (_networkManager.IsServer)
			{
				return tick;
			}
			long num = Tick - tick;
			if (num <= 0)
			{
				return LocalTick;
			}
			long num2 = LocalTick - num;
			if (num2 <= 0)
			{
				num2 = 0L;
			}
			return (uint)num2;
		}

		public uint LocalTickToTick(uint localTick)
		{
			if (_networkManager.IsServer)
			{
				return localTick;
			}
			long num = LocalTick - localTick;
			if (num <= 0)
			{
				return Tick;
			}
			long num2 = Tick - num;
			if (num2 <= 0)
			{
				num2 = 0L;
			}
			return (uint)num2;
		}

		private void TryIterateData(bool incoming)
		{
			if (incoming)
			{
				int frameCount = Time.frameCount;
				if (frameCount != _lastIncomingIterationFrame)
				{
					_lastIncomingIterationFrame = frameCount;
					_networkManager.TransportManager.IterateIncoming(server: true);
					_networkManager.TransportManager.IterateIncoming(server: false);
				}
			}
			else
			{
				_networkManager.TransportManager.IterateOutgoing(toServer: true);
				_networkManager.TransportManager.IterateOutgoing(toServer: false);
			}
		}

		private void SendTimingAdjustment()
		{
			uint num = TimeToTicks(1.0);
			uint tick = Tick;
			if (tick - _lastUpdateTicks < num)
			{
				return;
			}
			PooledWriter pooledWriter = WriterPool.Retrieve();
			foreach (NetworkConnection value in _networkManager.ServerManager.Clients.Values)
			{
				if (value.Authenticated)
				{
					pooledWriter.Reset();
					pooledWriter.WritePacketId(PacketId.TimingUpdate);
					ushort andResetAverageQueueCount = value.GetAndResetAverageQueueCount();
					pooledWriter.WriteUInt16(andResetAverageQueueCount);
					value.SendToClient(1, pooledWriter.GetArraySegment());
				}
			}
			pooledWriter.Store();
			_lastUpdateTicks = tick;
		}

		internal void ParseTimingUpdate(PooledReader reader)
		{
			ushort queuedInputs = _networkManager.PredictionManager.QueuedInputs;
			ushort num = reader.ReadUInt16();
			if (_networkManager.IsServer)
			{
				return;
			}
			UpdateTick();
			ushort num2 = (ushort)((num > queuedInputs) ? ((ushort)(num - queuedInputs)) : 0);
			uint num3 = (uint)((float)(int)TickRate * 1f);
			uint clientTicks = _clientTicks;
			_clientTicks = 0u;
			long num4 = ((num2 != 0) ? num2 : ((num != 0) ? (-(queuedInputs - num)) : ((long)clientTicks - (long)num3)));
			TimingUpdateChange timingUpdateChange = ((num4 != 0L) ? ((num4 > 0) ? TimingUpdateChange.TooFast : TimingUpdateChange.TooSlow) : TimingUpdateChange.JustRight);
			if (timingUpdateChange != _timingUpdateChange)
			{
				if (_updateChangeMultiplier > 0.1f)
				{
					_updateChangeMultiplier -= 0.1f;
				}
			}
			else if (_updateChangeMultiplier < 1f)
			{
				_updateChangeMultiplier += 0.025f;
			}
			_timingUpdateChange = timingUpdateChange;
			num4 = (int)((float)num4 * _updateChangeMultiplier);
			if (Mathf.Abs(num4) >= (float)(int)RESET_ADJUSTMENT_THRESHOLD)
			{
				num4 = 0L;
			}
			double num5 = ((num4 > 0) ? 0.02 : 0.035);
			double num6 = TickDelta * ((double)num4 * num5);
			_adjustedTickDelta = TickDelta + num6;
			_adjustedTickDelta += TickDelta * (0.02 * (double)_timingTooFastCount);
			_adjustedTickDelta = Maths.ClampDouble(_adjustedTickDelta, _clientTimingRange[0], _clientTimingRange[1]);
			if (num4 > 0)
			{
				_timingTooFastCount += 0.5f;
			}
			else if (_timingTooFastCount >= 0.5f)
			{
				_timingTooFastCount -= 0.5f;
			}
			else
			{
				_timingTooFastCount = 0f;
			}
			void UpdateTick()
			{
				uint num7 = TimeToTicks((float)(RoundTripTime / 2) / 1000f);
				Tick = LastPacketTick + num7;
			}
		}

		public void SetTickRate(ushort value)
		{
			TickRate = value;
			TickDelta = 1.0 / (double)(int)TickRate;
			_adjustedTickDelta = TickDelta;
			_clientTimingRange = new double[2]
			{
				TickDelta * 0.5,
				TickDelta * 1.5
			};
		}

		private void OnValidate()
		{
			SetInitialValues();
		}
	}
}
