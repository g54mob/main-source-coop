using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Enviro;
using EvilCore.Extensions;
using EvilCore.Networking;
using Mirror;
using NomadDrive.Features.WorldGeneration;
using NomadDrive.Features.WorldGeneration.Utilities;
using NomadDrive.Managers.GameTime;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.GameTime
{
	public class DynamicWeatherDirector : NetworkBehaviour
	{
		private const byte NoWeather = 255;

		private const float WeightEpsilon = 0.0001f;

		[Tooltip("Weighted weather pool plus the dynamic-cycling parameters.")]
		[SerializeField]
		private SeededWeatherConfig config;

		[Inject]
		private ITimeManager _timeManager;

		[SyncVar(hook = "OnActiveWeatherChanged")]
		private byte _activeWeatherIndex = 255;

		private EnviroManager _enviroManager;

		private int _lastAppliedIndex = -1;

		private bool _firstApplyDone;

		private bool _transitionSpeedApplied;

		private bool _serverInitialized;

		private bool _sawWorldReady;

		private int _segmentIndex;

		private int _minutesUntilChange;

		private bool _minuteSubscribed;

		private bool _warnedNoValidCandidate;

		private bool _warnedHourlyAllZero;

		public Action<byte, byte> _Mirror_SyncVarHookDelegate__activeWeatherIndex;

		public byte Network_activeWeatherIndex
		{
			get
			{
				return _activeWeatherIndex;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _activeWeatherIndex, 1uL, _Mirror_SyncVarHookDelegate__activeWeatherIndex);
			}
		}

		private void Awake()
		{
			base.gameObject.InjectGameObject();
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			TrySubscribeMinute();
		}

		public override void OnStopServer()
		{
			base.OnStopServer();
			UnsubscribeMinute();
		}

		private void OnDestroy()
		{
			UnsubscribeMinute();
		}

		private void Update()
		{
			if (TryResolveEnviro())
			{
				ApplyTransitionSpeedOnce();
				if (base.isServer)
				{
					TrySubscribeMinute();
					ServerInitializeWeather();
				}
				ReconcileWeather();
			}
		}

		private void OnActiveWeatherChanged(byte oldValue, byte newValue)
		{
			ReconcileWeather();
		}

		private bool TryResolveEnviro()
		{
			if (config == null)
			{
				return false;
			}
			if (_enviroManager == null)
			{
				_enviroManager = EnviroManager.instance;
				if (_enviroManager == null)
				{
					return false;
				}
			}
			if (_enviroManager.Weather != null && _enviroManager.Weather.Settings != null && _enviroManager.Weather.Settings.weatherTypes != null)
			{
				return _enviroManager.Weather.Settings.weatherTypes.Count > 0;
			}
			return false;
		}

		private void ApplyTransitionSpeedOnce()
		{
			if (!_transitionSpeedApplied)
			{
				_transitionSpeedApplied = true;
				if (!(config.weatherTransitionSpeed <= 0f))
				{
					float weatherTransitionSpeed = config.weatherTransitionSpeed;
					EnviroWeather settings = _enviroManager.Weather.Settings;
					settings.cloudsTransitionSpeed = weatherTransitionSpeed;
					settings.fogTransitionSpeed = weatherTransitionSpeed;
					settings.lightingTransitionSpeed = weatherTransitionSpeed;
					settings.skyTransitionSpeed = weatherTransitionSpeed;
					settings.effectsTransitionSpeed = weatherTransitionSpeed;
					settings.auroraTransitionSpeed = weatherTransitionSpeed;
					settings.environmentTransitionSpeed = weatherTransitionSpeed;
					settings.audioTransitionSpeed = weatherTransitionSpeed;
				}
			}
		}

		private void ServerInitializeWeather()
		{
			if (_serverInitialized)
			{
				return;
			}
			WorldGenerator instance = NetworkSingleton<WorldGenerator>.Instance;
			if (instance == null || !instance.IsWorldFullyReady || instance.SeedManager == null)
			{
				return;
			}
			if (!_sawWorldReady)
			{
				_sawWorldReady = true;
				return;
			}
			_enviroManager.Weather.globalAutoWeatherChange = false;
			AdvanceSegment(instance, initial: true);
			if (_activeWeatherIndex != 255)
			{
				_serverInitialized = true;
			}
		}

		private void HandleMinutePassed()
		{
			if (!base.isServer || !_serverInitialized || config == null || !config.enableDynamicCycling)
			{
				return;
			}
			if (_minutesUntilChange > 0)
			{
				_minutesUntilChange--;
			}
			if (_minutesUntilChange <= 0)
			{
				WorldGenerator instance = NetworkSingleton<WorldGenerator>.Instance;
				if (!(instance == null) && instance.SeedManager != null)
				{
					AdvanceSegment(instance, initial: false);
				}
			}
		}

		private void AdvanceSegment(WorldGenerator worldGenerator, bool initial)
		{
			float hour = _enviroManager.Time.hours;
			int excludeIndex = (initial ? (-1) : _activeWeatherIndex);
			System.Random random = worldGenerator.SeedManager.CreateRandom($"{config.seedIdentifier}_seg{_segmentIndex}");
			int num = PickWeatherIndex(random, hour, excludeIndex);
			if (num >= 0)
			{
				Network_activeWeatherIndex = (byte)num;
				int num2 = Mathf.Max(1, config.minSegmentMinutes);
				int num3 = Mathf.Max(num2, config.maxSegmentMinutes);
				_minutesUntilChange = random.Next(num2, num3 + 1);
				_segmentIndex++;
			}
		}

		private int PickWeatherIndex(System.Random rng, float hour, int excludeIndex)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < config.candidates.Count; i++)
			{
				SeededWeatherConfig.WeatherChance weatherChance = config.candidates[i];
				if (weatherChance != null && !(weatherChance.weight <= 0f) && !string.IsNullOrEmpty(weatherChance.weatherTypeName) && WeatherExistsInEnviro(weatherChance.weatherTypeName))
				{
					list.Add(i);
				}
			}
			if (list.Count == 0)
			{
				if (!_warnedNoValidCandidate)
				{
					_warnedNoValidCandidate = true;
				}
				return -1;
			}
			if (excludeIndex >= 0 && config.avoidImmediateRepeat && list.Count > 1)
			{
				list.Remove(excludeIndex);
			}
			int[] array = list.ToArray();
			float num = 0f;
			int[] array2 = array;
			foreach (int index in array2)
			{
				num += EffectiveWeight(index, hour);
			}
			bool useHourly = num > 0.0001f;
			if (!useHourly && !_warnedHourlyAllZero)
			{
				_warnedHourlyAllZero = true;
			}
			return WeightedRandom.Select(array, (int index2) => (!useHourly) ? config.candidates[index2].weight : EffectiveWeight(index2, hour), rng);
		}

		private float EffectiveWeight(int index, float hour)
		{
			SeededWeatherConfig.WeatherChance weatherChance = config.candidates[index];
			float num = weatherChance.weight;
			if (weatherChance.hourlyWeight != null)
			{
				num *= Mathf.Max(0f, weatherChance.hourlyWeight.Evaluate(hour));
			}
			return num;
		}

		private bool WeatherExistsInEnviro(string weatherName)
		{
			return _enviroManager.Weather.Settings.weatherTypes.Exists((EnviroWeatherType w) => w != null && w.name == weatherName);
		}

		private void ReconcileWeather()
		{
			if (TryResolveEnviro() && _activeWeatherIndex != 255 && _activeWeatherIndex != _lastAppliedIndex)
			{
				ApplyWeatherLocally(_activeWeatherIndex, !_firstApplyDone);
				_lastAppliedIndex = _activeWeatherIndex;
				_firstApplyDone = true;
			}
		}

		private void ApplyWeatherLocally(byte index, bool instant)
		{
			if (config == null || index >= config.candidates.Count)
			{
				return;
			}
			string weatherTypeName = config.candidates[index].weatherTypeName;
			if (!string.IsNullOrEmpty(weatherTypeName) && WeatherExistsInEnviro(weatherTypeName))
			{
				_enviroManager.Weather.globalAutoWeatherChange = false;
				if (instant)
				{
					_enviroManager.Weather.ChangeWeatherInstant(weatherTypeName);
				}
				else
				{
					_enviroManager.Weather.ChangeWeather(weatherTypeName);
				}
			}
		}

		private void TrySubscribeMinute()
		{
			if (!_minuteSubscribed && _timeManager != null)
			{
				_timeManager.OnMinutePassed.AddListener(HandleMinutePassed);
				_minuteSubscribed = true;
			}
		}

		private void UnsubscribeMinute()
		{
			if (_minuteSubscribed && _timeManager != null)
			{
				_timeManager.OnMinutePassed.RemoveListener(HandleMinutePassed);
				_minuteSubscribed = false;
			}
		}

		public void DebugForceAdvanceSegment()
		{
			if (base.isServer && _serverInitialized)
			{
				WorldGenerator instance = NetworkSingleton<WorldGenerator>.Instance;
				if (!(instance == null) && instance.SeedManager != null)
				{
					AdvanceSegment(instance, initial: false);
				}
			}
		}

		public string DebugDescribeState()
		{
			string arg = ((_activeWeatherIndex != 255 && config != null && _activeWeatherIndex < config.candidates.Count) ? config.candidates[_activeWeatherIndex].weatherTypeName : "<none>");
			return $"segment {_segmentIndex} | active '{arg}' (idx {_activeWeatherIndex}) | " + $"{_minutesUntilChange} in-game min left | dynamic={config != null && config.enableDynamicCycling}";
		}

		public DynamicWeatherDirector()
		{
			_Mirror_SyncVarHookDelegate__activeWeatherIndex = OnActiveWeatherChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				NetworkWriterExtensions.WriteByte(writer, _activeWeatherIndex);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _activeWeatherIndex);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _activeWeatherIndex, _Mirror_SyncVarHookDelegate__activeWeatherIndex, NetworkReaderExtensions.ReadByte(reader));
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _activeWeatherIndex, _Mirror_SyncVarHookDelegate__activeWeatherIndex, NetworkReaderExtensions.ReadByte(reader));
			}
		}
	}
}
