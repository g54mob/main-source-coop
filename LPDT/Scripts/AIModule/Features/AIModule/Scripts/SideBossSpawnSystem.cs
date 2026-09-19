using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.AnalyticsExtensions;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.GameModeModule.Scripts;
using Features.GamePhasesModule.Scripts.Data;
using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using Zenject;

namespace Features.AIModule.Scripts
{
	public class SideBossSpawnSystem : IInitializable, IDisposable, ISharedModeMasterMigrationHandler
	{
		private class SideBossConfigRuntime
		{
			public SideBossSpawnConfiguration Config;

			public int NextRespawnSessionTime = -1;
		}

		private class SideBossTypeRuntime
		{
			public bool IsSpawning;

			public IEnemyBehaviour EnemyBehaviour;

			public SideBossSpawnConfiguration ActiveConfig;
		}

		private const int CheckIntervalSeconds = 1;

		private const int RespawnTimerNotActive = -1;

		private readonly IEnemySpawnConfigurationProvider _enemySpawnConfigurationProvider;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly GameModeModel _gameModeModel;

		private readonly LevelModel _levelModel;

		private readonly GamePhasesModel _gamePhasesModel;

		private readonly EnemySpawnPointsModel _enemySpawnPointsModel;

		private readonly SideBossSpawnPoolModel _sideBossSpawnPoolModel;

		private readonly CustomPlayerEventsSynchronizedModel _customPlayerEventsSynchronizedModel;

		private readonly IEnemySpawnGate _enemySpawnGate;

		private CancellationTokenSource _cts;

		private int _sessionTimePassed;

		private int _timeToSpawn = 30;

		private int _respawnBossTimer = 120;

		private int _nextPoolSpawnSessionTime;

		private int _lastBroadcastAnalyticsLevelNumber;

		private SideBossSpawnConfiguration _selectedBossConfig;

		private readonly List<SideBossConfigRuntime> _configRuntimes = new List<SideBossConfigRuntime>();

		private readonly Dictionary<SideBossSpawnConfiguration, SideBossConfigRuntime> _configRuntimeByConfig = new Dictionary<SideBossSpawnConfiguration, SideBossConfigRuntime>();

		private readonly Dictionary<EnemyType, SideBossTypeRuntime> _typeRuntimes = new Dictionary<EnemyType, SideBossTypeRuntime>();

		public int SharedModeMasterMigrationHandleOrder => 0;

		[Inject]
		public SideBossSpawnSystem(IEnemySpawnConfigurationProvider enemySpawnConfigurationProvider, MultiplayerModel multiplayerModel, GameModeModel gameModeModel, LevelModel levelModel, GamePhasesModel gamePhasesModel, EnemySpawnPointsModel enemySpawnPointsModel, SideBossSpawnPoolModel sideBossSpawnPoolModel, CustomPlayerEventsSynchronizedModel customPlayerEventsSynchronizedModel, IEnemySpawnGate enemySpawnGate)
		{
			_enemySpawnConfigurationProvider = enemySpawnConfigurationProvider;
			_multiplayerModel = multiplayerModel;
			_gameModeModel = gameModeModel;
			_levelModel = levelModel;
			_gamePhasesModel = gamePhasesModel;
			_enemySpawnPointsModel = enemySpawnPointsModel;
			_sideBossSpawnPoolModel = sideBossSpawnPoolModel;
			_customPlayerEventsSynchronizedModel = customPlayerEventsSynchronizedModel;
			_enemySpawnGate = enemySpawnGate;
		}

		public void Initialize()
		{
			if (_gameModeModel.CurrentGameMode == GameModeType.DisabledEnemies)
			{
				_levelModel.OnLevelLoaded += OnLevelLoadedForDisabledEnemiesAnalytics;
				return;
			}
			_gamePhasesModel.BeforeGamePhaseLoopActivated += CleanupSpawnState;
			_gamePhasesModel.OnSomePlayerMovedFromSpawn += OnSomePlayerMovedFromSpawn;
		}

		public void OnSharedModeMasterMigrationCompleted(NetworkRunner runner)
		{
			if (!(runner == null) && runner.IsRunning && runner.IsSharedModeMasterClient && _gameModeModel.CurrentGameMode != GameModeType.DisabledEnemies && _cts == null && _gamePhasesModel.IsSomePlayerMovedFromSpawn)
			{
				if (_configRuntimes.Count == 0)
				{
					InitializeSpawnStates(_levelModel.CurrentLevel);
				}
				if (_configRuntimes.Count != 0)
				{
					AdoptAliveBosses();
					_sessionTimePassed = 0;
					_cts = new CancellationTokenSource();
					StartSpawnLoopAsync(_cts.Token).Forget();
				}
			}
		}

		private void AdoptAliveBosses()
		{
			NetworkBehaviour[] array = UnityEngine.Object.FindObjectsByType<NetworkBehaviour>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			foreach (NetworkBehaviour networkBehaviour in array)
			{
				IEnemyBehaviour enemyBehaviour = networkBehaviour as IEnemyBehaviour;
				if (enemyBehaviour == null)
				{
					continue;
				}
				NetworkObject networkObject = enemyBehaviour.NetworkObject;
				if (!(networkObject == null) && networkObject.IsValid && _typeRuntimes.TryGetValue(enemyBehaviour.EnemyType, out var value) && value.EnemyBehaviour == null)
				{
					SideBossConfigRuntime sideBossConfigRuntime = _configRuntimes.Find((SideBossConfigRuntime runtime) => runtime.Config.EnemyType == enemyBehaviour.EnemyType);
					if (sideBossConfigRuntime != null)
					{
						value.EnemyBehaviour = enemyBehaviour;
						value.ActiveConfig = sideBossConfigRuntime.Config;
						enemyBehaviour.OnDeath += OnBossDeath;
						_sideBossSpawnPoolModel.RemainingPool.Remove(sideBossConfigRuntime.Config);
						_selectedBossConfig = sideBossConfigRuntime.Config;
					}
				}
			}
		}

		public void Dispose()
		{
			_levelModel.OnLevelLoaded -= OnLevelLoadedForDisabledEnemiesAnalytics;
			_gamePhasesModel.BeforeGamePhaseLoopActivated -= CleanupSpawnState;
			_gamePhasesModel.OnSomePlayerMovedFromSpawn -= OnSomePlayerMovedFromSpawn;
			DisposeSpawnProcess();
			UnsubscribeAllBossDeaths();
			_sessionTimePassed = 0;
			_lastBroadcastAnalyticsLevelNumber = 0;
			_configRuntimes.Clear();
			_configRuntimeByConfig.Clear();
			_typeRuntimes.Clear();
			_selectedBossConfig = null;
			_sideBossSpawnPoolModel.Clear();
		}

		private void OnLevelLoadedForDisabledEnemiesAnalytics(LevelType levelType)
		{
			if (!(_multiplayerModel.NetworkRunner == null) && _multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				TryBroadcastLevelStartedBoss(EnemyType.None);
			}
		}

		private void OnSomePlayerMovedFromSpawn()
		{
			if (!(_multiplayerModel.NetworkRunner == null) && _multiplayerModel.NetworkRunner.IsSharedModeMasterClient && _cts == null)
			{
				if (_configRuntimes.Count == 0)
				{
					InitializeSpawnStates(_levelModel.CurrentLevel);
				}
				SelectSideBossForLevel();
				if (_configRuntimes.Count != 0)
				{
					_sessionTimePassed = 0;
					_cts = new CancellationTokenSource();
					StartSpawnLoopAsync(_cts.Token).Forget();
				}
			}
		}

		private void SelectSideBossForLevel()
		{
			if (_multiplayerModel.NetworkRunner == null || !_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				return;
			}
			_selectedBossConfig = null;
			EnemyType bossType = EnemyType.None;
			if (_configRuntimes.Count > 0)
			{
				if (_sideBossSpawnPoolModel.RemainingPool.Count == 0)
				{
					RefillPool();
				}
				if (_sideBossSpawnPoolModel.RemainingPool.Count > 0)
				{
					_selectedBossConfig = PickWeighted(_sideBossSpawnPoolModel.RemainingPool);
				}
				if (_selectedBossConfig != null)
				{
					bossType = _selectedBossConfig.EnemyType;
				}
			}
			TryBroadcastLevelStartedBoss(bossType);
		}

		private void TryBroadcastLevelStartedBoss(EnemyType bossType)
		{
			if (_levelModel.CurrentSequenceLevelNumber >= 1 && _levelModel.CurrentSequenceLevelNumber != _lastBroadcastAnalyticsLevelNumber)
			{
				_lastBroadcastAnalyticsLevelNumber = _levelModel.CurrentSequenceLevelNumber;
				string evenName = SessionAnalyticsEventNameExtensions.BuildLevelStartedBossEventName($"L{_levelModel.CurrentSequenceLevelNumber:D2}", bossType.ToString());
				_customPlayerEventsSynchronizedModel.SendPlayerEvent(-1, evenName);
			}
		}

		private void InitializeSpawnStates(LevelType levelType)
		{
			_configRuntimes.Clear();
			_configRuntimeByConfig.Clear();
			_typeRuntimes.Clear();
			_selectedBossConfig = null;
			_sideBossSpawnPoolModel.Clear();
			SideBossSpawnData sideBossesConfig = GetSideBossesConfig(levelType);
			if (sideBossesConfig?.BossSpawnConfigurations == null)
			{
				return;
			}
			_timeToSpawn = Math.Max(1, sideBossesConfig.TimeToSpawn);
			_respawnBossTimer = Math.Max(1, sideBossesConfig.RespawnBossTimer);
			_nextPoolSpawnSessionTime = _timeToSpawn;
			foreach (SideBossSpawnConfiguration bossSpawnConfiguration in sideBossesConfig.BossSpawnConfigurations)
			{
				if (bossSpawnConfiguration.EnemyType != EnemyType.None)
				{
					SideBossConfigRuntime sideBossConfigRuntime = new SideBossConfigRuntime
					{
						Config = bossSpawnConfiguration,
						NextRespawnSessionTime = -1
					};
					_configRuntimes.Add(sideBossConfigRuntime);
					_configRuntimeByConfig[bossSpawnConfiguration] = sideBossConfigRuntime;
					_sideBossSpawnPoolModel.SpawnWeights[bossSpawnConfiguration] = Mathf.Max(0f, bossSpawnConfiguration.SpawnChance);
					if (!_typeRuntimes.ContainsKey(bossSpawnConfiguration.EnemyType))
					{
						_typeRuntimes[bossSpawnConfiguration.EnemyType] = new SideBossTypeRuntime();
					}
				}
			}
			RefillPool();
		}

		private SideBossSpawnData GetSideBossesConfig(LevelType levelType)
		{
			EnemySpawnConfiguration enemySpawnConfiguration = _enemySpawnConfigurationProvider.GetEnemySpawnConfiguration();
			if (enemySpawnConfiguration == null)
			{
				return null;
			}
			if (!enemySpawnConfiguration.SideBossesSpawnConfigurationsByLevel.TryGetValue(levelType, out var value))
			{
				return null;
			}
			return value;
		}

		private async UniTaskVoid StartSpawnLoopAsync(CancellationToken token)
		{
			try
			{
				while (!token.IsCancellationRequested)
				{
					await UniTask.Delay(TimeSpan.FromSeconds(1.0), ignoreTimeScale: false, PlayerLoopTiming.Update, token);
					if (!_enemySpawnGate.IsSpawningAllowed)
					{
						continue;
					}
					_sessionTimePassed++;
					foreach (SideBossTypeRuntime value in _typeRuntimes.Values)
					{
						ClearStaleBossReference(value);
					}
					TrySpawnReadyRespawns();
					if (_sessionTimePassed >= _nextPoolSpawnSessionTime && TrySpawnFromGlobalPool())
					{
						_nextPoolSpawnSessionTime = _sessionTimePassed + _timeToSpawn;
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (ObjectDisposedException)
			{
			}
			catch (Exception arg)
			{
				Debug.LogError($"[SideBossSpawn] Loop error: {arg}");
			}
		}

		private void TrySpawnReadyRespawns()
		{
			for (int i = 0; i < _configRuntimes.Count; i++)
			{
				SideBossConfigRuntime sideBossConfigRuntime = _configRuntimes[i];
				if (sideBossConfigRuntime.NextRespawnSessionTime != -1 && _sessionTimePassed >= sideBossConfigRuntime.NextRespawnSessionTime)
				{
					SideBossSpawnConfiguration config = sideBossConfigRuntime.Config;
					if (_typeRuntimes.TryGetValue(config.EnemyType, out var value) && !value.IsSpawning && !IsBossAlive(value) && TryGetSpawnPoint(config.EnemyType, out var spawnPoint))
					{
						SpawnBossAsync(value, config, spawnPoint, removeFromPool: false, clearRespawnTimerOnSuccess: true, rememberAsSelectedBoss: false).Forget();
					}
				}
			}
		}

		private bool TrySpawnFromGlobalPool()
		{
			SideBossSpawnConfiguration sideBossSpawnConfiguration = null;
			if (_selectedBossConfig != null)
			{
				if (!_sideBossSpawnPoolModel.RemainingPool.Contains(_selectedBossConfig))
				{
					return false;
				}
				if (!IsEligibleForSpawn(_selectedBossConfig))
				{
					return false;
				}
				sideBossSpawnConfiguration = _selectedBossConfig;
			}
			if (sideBossSpawnConfiguration == null)
			{
				if (_sideBossSpawnPoolModel.RemainingPool.Count == 0)
				{
					RefillPool();
				}
				List<SideBossSpawnConfiguration> list = CollectReadyPoolCandidates();
				if (list.Count == 0)
				{
					return false;
				}
				sideBossSpawnConfiguration = PickWeighted(list);
				if (sideBossSpawnConfiguration == null)
				{
					return false;
				}
			}
			if (!_typeRuntimes.TryGetValue(sideBossSpawnConfiguration.EnemyType, out var value))
			{
				return false;
			}
			if (value.IsSpawning || IsBossAlive(value))
			{
				return false;
			}
			if (!TryGetSpawnPoint(sideBossSpawnConfiguration.EnemyType, out var spawnPoint))
			{
				return false;
			}
			bool flag = _selectedBossConfig != null;
			SpawnBossAsync(value, sideBossSpawnConfiguration, spawnPoint, removeFromPool: true, clearRespawnTimerOnSuccess: false, !flag).Forget();
			return true;
		}

		private List<SideBossSpawnConfiguration> CollectReadyPoolCandidates()
		{
			List<SideBossSpawnConfiguration> list = new List<SideBossSpawnConfiguration>();
			for (int i = 0; i < _sideBossSpawnPoolModel.RemainingPool.Count; i++)
			{
				SideBossSpawnConfiguration sideBossSpawnConfiguration = _sideBossSpawnPoolModel.RemainingPool[i];
				if (IsEligibleForSpawn(sideBossSpawnConfiguration))
				{
					list.Add(sideBossSpawnConfiguration);
				}
			}
			return list;
		}

		private bool IsEligibleForSpawn(SideBossSpawnConfiguration config)
		{
			if (!_configRuntimeByConfig.TryGetValue(config, out var value))
			{
				return false;
			}
			if (value.NextRespawnSessionTime != -1)
			{
				return false;
			}
			if (!_typeRuntimes.TryGetValue(config.EnemyType, out var value2))
			{
				return false;
			}
			if (value2.IsSpawning || IsBossAlive(value2))
			{
				return false;
			}
			return HasAvailableSpawnPoints(config.EnemyType);
		}

		private void RefillPool()
		{
			_sideBossSpawnPoolModel.RemainingPool.Clear();
			for (int i = 0; i < _configRuntimes.Count; i++)
			{
				SideBossSpawnConfiguration config = _configRuntimes[i].Config;
				if (!_sideBossSpawnPoolModel.RemainingPool.Contains(config))
				{
					_sideBossSpawnPoolModel.RemainingPool.Add(config);
				}
			}
		}

		private SideBossSpawnConfiguration PickWeighted(List<SideBossSpawnConfiguration> candidates)
		{
			float num = 0f;
			for (int i = 0; i < candidates.Count; i++)
			{
				SideBossSpawnConfiguration key = candidates[i];
				num = ((!_sideBossSpawnPoolModel.SpawnWeights.TryGetValue(key, out var value)) ? (num + 1f) : (num + Mathf.Max(0f, value)));
			}
			if (num <= 0f)
			{
				return candidates[0];
			}
			float num2 = UnityEngine.Random.Range(0f, num);
			float num3 = 0f;
			SideBossSpawnConfiguration result = candidates[candidates.Count - 1];
			for (int j = 0; j < candidates.Count; j++)
			{
				SideBossSpawnConfiguration sideBossSpawnConfiguration = candidates[j];
				float value2;
				float num4 = (_sideBossSpawnPoolModel.SpawnWeights.TryGetValue(sideBossSpawnConfiguration, out value2) ? Mathf.Max(0f, value2) : 1f);
				num3 += num4;
				if (num2 <= num3)
				{
					result = sideBossSpawnConfiguration;
					break;
				}
			}
			return result;
		}

		private async UniTaskVoid SpawnBossAsync(SideBossTypeRuntime typeRuntime, SideBossSpawnConfiguration config, EnemySpawnPointData spawnPoint, bool removeFromPool, bool clearRespawnTimerOnSuccess, bool rememberAsSelectedBoss)
		{
			if (typeRuntime.IsSpawning || IsBossAlive(typeRuntime))
			{
				return;
			}
			typeRuntime.IsSpawning = true;
			bool selectedForInitialSpawn = rememberAsSelectedBoss && _selectedBossConfig == null;
			bool spawnSucceeded = false;
			if (selectedForInitialSpawn)
			{
				_selectedBossConfig = config;
			}
			try
			{
				if (_multiplayerModel.NetworkRunner == null || !_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
				{
					return;
				}
				NetworkObject networkObject = await _multiplayerModel.NetworkRunner.SpawnAsync(config.BossPrefab, spawnPoint.Position, spawnPoint.Rotation);
				if (!networkObject.TryGetComponent<IEnemyBehaviour>(out var component))
				{
					if (networkObject != null && networkObject.IsValid)
					{
						networkObject.DespawnHierarchy();
					}
					return;
				}
				component.SetAreaPosition(spawnPoint.AreaPosition);
				EnemySpawnPointOccupancyUtility.TryOccupy(spawnPoint, component);
				typeRuntime.ActiveConfig = config;
				typeRuntime.EnemyBehaviour = component;
				component.OnDeath += OnBossDeath;
				spawnSucceeded = true;
				if (removeFromPool)
				{
					_sideBossSpawnPoolModel.RemainingPool.Remove(config);
				}
				if (clearRespawnTimerOnSuccess && _configRuntimeByConfig.TryGetValue(config, out var value))
				{
					value.NextRespawnSessionTime = -1;
				}
			}
			catch (Exception arg)
			{
				Debug.LogError($"[SideBossSpawn] Failed to spawn boss {config.EnemyType}. SessionTime={_sessionTimePassed}. Error: {arg}");
			}
			finally
			{
				if (selectedForInitialSpawn && !spawnSucceeded && _selectedBossConfig == config)
				{
					_selectedBossConfig = null;
				}
				typeRuntime.IsSpawning = false;
			}
		}

		private void OnBossDeath(IEnemyBehaviour deadEnemy)
		{
			if (deadEnemy == null)
			{
				return;
			}
			deadEnemy.OnDeath -= OnBossDeath;
			foreach (KeyValuePair<EnemyType, SideBossTypeRuntime> typeRuntime in _typeRuntimes)
			{
				SideBossTypeRuntime value = typeRuntime.Value;
				if (value.EnemyBehaviour == deadEnemy)
				{
					value.EnemyBehaviour = null;
					SideBossSpawnConfiguration activeConfig = value.ActiveConfig;
					value.ActiveConfig = null;
					ArmRespawnTimer(activeConfig);
					break;
				}
			}
		}

		private void ArmRespawnTimer(SideBossSpawnConfiguration config)
		{
			if (config != null && config.IsRespawnable && _configRuntimeByConfig.TryGetValue(config, out var value) && value.NextRespawnSessionTime == -1)
			{
				value.NextRespawnSessionTime = _sessionTimePassed + _respawnBossTimer;
			}
		}

		private static bool IsBossAlive(SideBossTypeRuntime typeRuntime)
		{
			IEnemyBehaviour enemyBehaviour = typeRuntime.EnemyBehaviour;
			if (enemyBehaviour == null)
			{
				return false;
			}
			NetworkObject networkObject = enemyBehaviour.NetworkObject;
			if (networkObject != null)
			{
				return networkObject.IsValid;
			}
			return false;
		}

		private void ClearStaleBossReference(SideBossTypeRuntime typeRuntime)
		{
			if (typeRuntime.EnemyBehaviour != null && !IsBossAlive(typeRuntime))
			{
				typeRuntime.EnemyBehaviour.OnDeath -= OnBossDeath;
				typeRuntime.EnemyBehaviour = null;
				ArmRespawnTimer(typeRuntime.ActiveConfig);
				typeRuntime.ActiveConfig = null;
			}
		}

		private void UnsubscribeAllBossDeaths()
		{
			foreach (SideBossTypeRuntime value in _typeRuntimes.Values)
			{
				if (value.EnemyBehaviour != null)
				{
					value.EnemyBehaviour.OnDeath -= OnBossDeath;
				}
			}
		}

		private bool TryGetSpawnPoint(EnemyType enemyType, out EnemySpawnPointData spawnPoint)
		{
			List<EnemySpawnPointData> availableSpawnPoints = GetAvailableSpawnPoints(enemyType);
			if (availableSpawnPoints.Count == 0)
			{
				spawnPoint = null;
				return false;
			}
			spawnPoint = availableSpawnPoints[UnityEngine.Random.Range(0, availableSpawnPoints.Count)];
			return true;
		}

		private bool HasAvailableSpawnPoints(EnemyType enemyType)
		{
			return GetAvailableSpawnPoints(enemyType).Count > 0;
		}

		private List<EnemySpawnPointData> GetAvailableSpawnPoints(EnemyType enemyType)
		{
			if (!_enemySpawnPointsModel.SpawnPointsPool.TryGetValue(enemyType, out var value))
			{
				return new List<EnemySpawnPointData>();
			}
			List<EnemySpawnPointData> list = new List<EnemySpawnPointData>(value.Count);
			for (int i = 0; i < value.Count; i++)
			{
				EnemySpawnPointData enemySpawnPointData = value[i];
				if (!enemySpawnPointData.IsOneTimeSpawnPoint && !enemySpawnPointData.Occupancy.IsOccupied)
				{
					list.Add(enemySpawnPointData);
				}
			}
			return list;
		}

		private void CleanupSpawnState()
		{
			DisposeSpawnProcess();
			UnsubscribeAllBossDeaths();
			_sessionTimePassed = 0;
			_lastBroadcastAnalyticsLevelNumber = 0;
			_configRuntimes.Clear();
			_configRuntimeByConfig.Clear();
			_typeRuntimes.Clear();
			_selectedBossConfig = null;
			_sideBossSpawnPoolModel.Clear();
		}

		private void DisposeSpawnProcess()
		{
			if (_cts != null)
			{
				_cts.Cancel();
				_cts.Dispose();
				_cts = null;
			}
		}
	}
}
