using System;
using System.Collections;
using System.Runtime.InteropServices;
using Enviro;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Managers.GameTime
{
	public class BasicTimeManager : NetworkBehaviour, ITimeManager
	{
		private EnviroManager _enviroManager;

		private Coroutine _forwardTimeCoroutine;

		private int _lastFiredMinute = -1;

		[SerializeField]
		private float rainingWetnessThreshold = 0.05f;

		[Header("Weather Classification")]
		[SerializeField]
		[Range(0f, 1f)]
		private float snowThreshold = 0.05f;

		[SerializeField]
		[Range(0f, 1f)]
		private float stormThreshold = 0.6f;

		[SerializeField]
		[Range(0f, 1f)]
		private float rainThreshold = 0.15f;

		[SerializeField]
		[Range(0f, 1f)]
		private float cloudyThreshold = 0.04f;

		[SyncVar]
		private float _syncedRainIntensity;

		private const float RainIntensitySyncEpsilon = 0.001f;

		[SyncVar(hook = "OnSyncedWeatherChanged")]
		private byte _syncedWeather;

		[SyncVar(hook = "OnSyncedHourChanged")]
		private byte _syncedHour;

		[SyncVar(hook = "OnSyncedMinuteChanged")]
		private byte _syncedMinute;

		[SyncVar]
		private sbyte _syncedTemperatureCelsius;

		private Action _pendingCompleteAction;

		public Action<byte, byte> _Mirror_SyncVarHookDelegate__syncedWeather;

		public Action<byte, byte> _Mirror_SyncVarHookDelegate__syncedHour;

		public Action<byte, byte> _Mirror_SyncVarHookDelegate__syncedMinute;

		public UnityEvent OnMinutePassed { get; } = new UnityEvent();

		public string CurrentTimeString { get; set; } = "00:00";

		public float Temperature
		{
			get
			{
				return _syncedTemperatureCelsius;
			}
			set
			{
			}
		}

		public int CurrentHour => _syncedHour;

		public float RainIntensity => Mathf.Clamp01(_syncedRainIntensity);

		public bool IsRaining => RainIntensity > rainingWetnessThreshold;

		public WeatherType CurrentWeather => (WeatherType)_syncedWeather;

		public float Network_syncedRainIntensity
		{
			get
			{
				return _syncedRainIntensity;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _syncedRainIntensity, 1uL, null);
			}
		}

		public byte Network_syncedWeather
		{
			get
			{
				return _syncedWeather;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _syncedWeather, 2uL, _Mirror_SyncVarHookDelegate__syncedWeather);
			}
		}

		public byte Network_syncedHour
		{
			get
			{
				return _syncedHour;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _syncedHour, 4uL, _Mirror_SyncVarHookDelegate__syncedHour);
			}
		}

		public byte Network_syncedMinute
		{
			get
			{
				return _syncedMinute;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _syncedMinute, 8uL, _Mirror_SyncVarHookDelegate__syncedMinute);
			}
		}

		public sbyte Network_syncedTemperatureCelsius
		{
			get
			{
				return _syncedTemperatureCelsius;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _syncedTemperatureCelsius, 16uL, null);
			}
		}

		public event Action<WeatherType> OnWeatherChanged;

		public override void OnStartClient()
		{
			_enviroManager = EnviroManager.instance;
			_ = _enviroManager == null;
			CurrentTimeString = FormatTime(_syncedHour, _syncedMinute);
			_lastFiredMinute = _syncedMinute;
		}

		private void Update()
		{
			if (base.isServer && _enviroManager != null && _enviroManager.Environment != null)
			{
				float num = Mathf.Clamp01(_enviroManager.Environment.Settings.wetness);
				if (Mathf.Abs(num - _syncedRainIntensity) > 0.001f)
				{
					Network_syncedRainIntensity = num;
				}
				float snow = Mathf.Clamp01(_enviroManager.Environment.Settings.snow);
				byte b = (byte)ClassifyWeather(num, snow);
				if (b != _syncedWeather)
				{
					Network_syncedWeather = b;
				}
				byte b2 = (byte)_enviroManager.Time.hours;
				byte b3 = (byte)_enviroManager.Time.minutes;
				sbyte b4 = (sbyte)Mathf.Clamp(_enviroManager.Environment.Settings.temperature, -128f, 127f);
				if (b2 != _syncedHour)
				{
					Network_syncedHour = b2;
				}
				if (b4 != _syncedTemperatureCelsius)
				{
					Network_syncedTemperatureCelsius = b4;
				}
				if (b3 != _syncedMinute)
				{
					Network_syncedMinute = b3;
				}
			}
			PollMinuteEdge();
		}

		private void OnSyncedHourChanged(byte oldValue, byte newValue)
		{
			CurrentTimeString = FormatTime(newValue, _syncedMinute);
		}

		private void OnSyncedWeatherChanged(byte oldValue, byte newValue)
		{
			this.OnWeatherChanged?.Invoke((WeatherType)newValue);
		}

		private WeatherType ClassifyWeather(float wetness, float snow)
		{
			if (snow >= snowThreshold)
			{
				return WeatherType.Snowy;
			}
			if (wetness >= stormThreshold)
			{
				return WeatherType.Storm;
			}
			if (wetness >= rainThreshold)
			{
				return WeatherType.Rainy;
			}
			if (wetness >= cloudyThreshold)
			{
				return WeatherType.Cloudy;
			}
			return WeatherType.Clear;
		}

		private void OnSyncedMinuteChanged(byte oldValue, byte newValue)
		{
			CurrentTimeString = FormatTime(_syncedHour, newValue);
		}

		private void PollMinuteEdge()
		{
			if (_lastFiredMinute < 0)
			{
				_lastFiredMinute = _syncedMinute;
			}
			else if (_syncedMinute != _lastFiredMinute)
			{
				_lastFiredMinute = _syncedMinute;
				OnMinutePassed.Invoke();
			}
		}

		private static string FormatTime(byte hour, byte minute)
		{
			return $"{hour:D2}:{minute:D2}";
		}

		public void ForwardTimeSmoothly(DayTimePart dayTimePart, Action completeAction = null)
		{
			ForwardTimeSmoothly(dayTimePart, 1f, completeAction);
		}

		public void ForwardTimeSmoothly(DayTimePart dayTimePart, float speedMultiplier, Action completeAction = null)
		{
			_pendingCompleteAction = completeAction;
			if (base.isServer)
			{
				StartForwardTimeServer(dayTimePart, speedMultiplier, null);
			}
			else
			{
				CmdForwardTimeSmoothly(dayTimePart, speedMultiplier);
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdForwardTimeSmoothly(DayTimePart dayTimePart, float speedMultiplier, NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NomadDrive_002EManagers_002EGameTime_002EDayTimePart(writer, dayTimePart);
			writer.WriteFloat(speedMultiplier);
			SendCommandInternal("System.Void NomadDrive.Managers.GameTime.BasicTimeManager::CmdForwardTimeSmoothly(NomadDrive.Managers.GameTime.DayTimePart,System.Single,Mirror.NetworkConnectionToClient)", 43385254, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		private void StartForwardTimeServer(DayTimePart dayTimePart, float speedMultiplier, NetworkConnectionToClient sender)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Managers.GameTime.BasicTimeManager::StartForwardTimeServer(NomadDrive.Managers.GameTime.DayTimePart,System.Single,Mirror.NetworkConnectionToClient)' called when server was not active");
				return;
			}
			if (_forwardTimeCoroutine != null)
			{
				if (sender != null)
				{
					TargetForwardTimeComplete(sender);
				}
				else
				{
					InvokePendingCompleteAction();
				}
				return;
			}
			float speedMultiplier2 = Mathf.Max(0.01f, speedMultiplier);
			_forwardTimeCoroutine = StartCoroutine(ForwardTimeCoroutine(dayTimePart, speedMultiplier2, delegate
			{
				if (sender != null)
				{
					TargetForwardTimeComplete(sender);
				}
				else
				{
					InvokePendingCompleteAction();
				}
			}));
		}

		[TargetRpc]
		private void TargetForwardTimeComplete(NetworkConnectionToClient target)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendTargetRPCInternal(target, "System.Void NomadDrive.Managers.GameTime.BasicTimeManager::TargetForwardTimeComplete(Mirror.NetworkConnectionToClient)", 1139205858, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		private void InvokePendingCompleteAction()
		{
			Action pendingCompleteAction = _pendingCompleteAction;
			_pendingCompleteAction = null;
			pendingCompleteAction?.Invoke();
		}

		private IEnumerator ForwardTimeCoroutine(DayTimePart dayTimePart, float speedMultiplier, Action completeAction = null)
		{
			_enviroManager.Time.Settings.simulate = false;
			DateTime currentTime = _enviroManager.Time.Settings.date;
			DateTime targetTime = currentTime.AddHours(dayTimePart.Hours).AddMinutes(dayTimePart.Minutes).AddSeconds(dayTimePart.Seconds);
			float internalTimeOverflow = 0f;
			while (currentTime < targetTime)
			{
				float num = (EnviroManager.instance.isNight ? _enviroManager.Time.Settings.nightLengthModifier : _enviroManager.Time.Settings.dayLengthModifier);
				float num2 = 0.4f / (_enviroManager.Time.Settings.cycleLengthInMinutes * num);
				num2 *= 3600f * Time.deltaTime * 60f * speedMultiplier;
				internalTimeOverflow = ((!(num2 < 1f)) ? num2 : (internalTimeOverflow + num2));
				_enviroManager.Time.seconds += (int)internalTimeOverflow;
				currentTime = _enviroManager.Time.Settings.date;
				if (internalTimeOverflow >= 1f)
				{
					internalTimeOverflow = 0f;
				}
				yield return null;
			}
			completeAction?.Invoke();
			_enviroManager.Time.Settings.simulate = true;
			StopCoroutine(_forwardTimeCoroutine);
			_forwardTimeCoroutine = null;
		}

		public BasicTimeManager()
		{
			_Mirror_SyncVarHookDelegate__syncedWeather = OnSyncedWeatherChanged;
			_Mirror_SyncVarHookDelegate__syncedHour = OnSyncedHourChanged;
			_Mirror_SyncVarHookDelegate__syncedMinute = OnSyncedMinuteChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdForwardTimeSmoothly__DayTimePart__Single__NetworkConnectionToClient(DayTimePart dayTimePart, float speedMultiplier, NetworkConnectionToClient sender)
		{
			StartForwardTimeServer(dayTimePart, speedMultiplier, sender);
		}

		protected static void InvokeUserCode_CmdForwardTimeSmoothly__DayTimePart__Single__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdForwardTimeSmoothly called on client.");
			}
			else
			{
				((BasicTimeManager)obj).UserCode_CmdForwardTimeSmoothly__DayTimePart__Single__NetworkConnectionToClient(GeneratedNetworkCode._Read_NomadDrive_002EManagers_002EGameTime_002EDayTimePart(reader), reader.ReadFloat(), senderConnection);
			}
		}

		protected void UserCode_TargetForwardTimeComplete__NetworkConnectionToClient(NetworkConnectionToClient target)
		{
			InvokePendingCompleteAction();
		}

		protected static void InvokeUserCode_TargetForwardTimeComplete__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC TargetForwardTimeComplete called on server.");
			}
			else
			{
				((BasicTimeManager)obj).UserCode_TargetForwardTimeComplete__NetworkConnectionToClient(null);
			}
		}

		static BasicTimeManager()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(BasicTimeManager), "System.Void NomadDrive.Managers.GameTime.BasicTimeManager::CmdForwardTimeSmoothly(NomadDrive.Managers.GameTime.DayTimePart,System.Single,Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdForwardTimeSmoothly__DayTimePart__Single__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterRpc(typeof(BasicTimeManager), "System.Void NomadDrive.Managers.GameTime.BasicTimeManager::TargetForwardTimeComplete(Mirror.NetworkConnectionToClient)", InvokeUserCode_TargetForwardTimeComplete__NetworkConnectionToClient);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteFloat(_syncedRainIntensity);
				NetworkWriterExtensions.WriteByte(writer, _syncedWeather);
				NetworkWriterExtensions.WriteByte(writer, _syncedHour);
				NetworkWriterExtensions.WriteByte(writer, _syncedMinute);
				writer.WriteSByte(_syncedTemperatureCelsius);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteFloat(_syncedRainIntensity);
			}
			if ((syncVarDirtyBits & 2L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _syncedWeather);
			}
			if ((syncVarDirtyBits & 4L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _syncedHour);
			}
			if ((syncVarDirtyBits & 8L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _syncedMinute);
			}
			if ((syncVarDirtyBits & 0x10L) != 0L)
			{
				writer.WriteSByte(_syncedTemperatureCelsius);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _syncedRainIntensity, null, reader.ReadFloat());
				GeneratedSyncVarDeserialize(ref _syncedWeather, _Mirror_SyncVarHookDelegate__syncedWeather, NetworkReaderExtensions.ReadByte(reader));
				GeneratedSyncVarDeserialize(ref _syncedHour, _Mirror_SyncVarHookDelegate__syncedHour, NetworkReaderExtensions.ReadByte(reader));
				GeneratedSyncVarDeserialize(ref _syncedMinute, _Mirror_SyncVarHookDelegate__syncedMinute, NetworkReaderExtensions.ReadByte(reader));
				GeneratedSyncVarDeserialize(ref _syncedTemperatureCelsius, null, reader.ReadSByte());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _syncedRainIntensity, null, reader.ReadFloat());
			}
			if ((num & 2L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _syncedWeather, _Mirror_SyncVarHookDelegate__syncedWeather, NetworkReaderExtensions.ReadByte(reader));
			}
			if ((num & 4L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _syncedHour, _Mirror_SyncVarHookDelegate__syncedHour, NetworkReaderExtensions.ReadByte(reader));
			}
			if ((num & 8L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _syncedMinute, _Mirror_SyncVarHookDelegate__syncedMinute, NetworkReaderExtensions.ReadByte(reader));
			}
			if ((num & 0x10L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _syncedTemperatureCelsius, null, reader.ReadSByte());
			}
		}
	}
}
