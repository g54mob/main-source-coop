using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.AIModule.Scripts.Services;
using Features.ExtendedLogger.Scripts;
using Features.GameModeModule.Scripts;
using Features.GamePhasesModule.Scripts.Data;
using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using GameplayEvents;
using UnityEngine;
using Zenject;

namespace Features.AIModule.Scripts
{
	public class EnemySpawnSystem : IInitializable, IDisposable, ISharedModeMasterMigrationHandler, IEnemySpawnStateReset
	{
		private const int RespawnUnarmed = -1;

		private readonly IEnemySpawnService _enemySpawnService;

		private readonly EnemySpawnPointsModel _enemySpawnPointsModel;

		private readonly IPlayerPositionsProvider _playerPositionsProvider;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly GameModeModel _gameModeModel;

		private readonly IEnemySpawnConfigurationProvider _enemySpawnConfigurationProvider;

		private readonly GamePhasesModel _gamePhasesModel;

		private readonly LevelModel _levelModel;

		private readonly EnemiesPoolModel _enemiesPoolModel;

		private readonly EnemySpawnConfigurationModel _enemySpawnConfigurationModel;

		private readonly IOverlayService _overlayService;

		private readonly GameplayEventBus _gameplayEventBus;

		private readonly IEnemySpawnGate _enemySpawnGate;

		private readonly EnemySpawnStatesModel _enemySpawnStatesModel;

		private CancellationTokenSource _cts;

		private bool _seededPlanBeforeFear;

		private readonly Dictionary<EnemyType, HashSet<IEnemyBehaviour>> _spawnedEnemies = new Dictionary<EnemyType, HashSet<IEnemyBehaviour>>();

		private readonly Dictionary<EnemyType, int> _pendingSpawns = new Dictionary<EnemyType, int>();

		public int SharedModeMasterMigrationHandleOrder => 0;

		[Inject]
		public EnemySpawnSystem(IEnemySpawnService enemySpawnService, EnemySpawnPointsModel enemySpawnPointsModel, IPlayerPositionsProvider playerPositionsProvider, MultiplayerModel multiplayerModel, GameModeModel gameModeModel, IEnemySpawnConfigurationProvider enemySpawnConfigurationProvider, GamePhasesModel gamePhasesModel, LevelModel levelModel, EnemiesPoolModel enemiesPoolModel, EnemySpawnConfigurationModel enemySpawnConfigurationModel, IOverlayService overlayService, GameplayEventBus gameplayEventBus, IEnemySpawnGate enemySpawnGate, EnemySpawnStatesModel enemySpawnStatesModel)
		{
			_enemySpawnConfigurationProvider = enemySpawnConfigurationProvider;
			_multiplayerModel = multiplayerModel;
			_enemySpawnService = enemySpawnService;
			_enemySpawnPointsModel = enemySpawnPointsModel;
			_playerPositionsProvider = playerPositionsProvider;
			_gameModeModel = gameModeModel;
			_gamePhasesModel = gamePhasesModel;
			_levelModel = levelModel;
			_enemiesPoolModel = enemiesPoolModel;
			_enemySpawnConfigurationModel = enemySpawnConfigurationModel;
			_overlayService = overlayService;
			_gameplayEventBus = gameplayEventBus;
			_enemySpawnGate = enemySpawnGate;
			_enemySpawnStatesModel = enemySpawnStatesModel;
		}

		public void Initialize()
		{
			if (_gameModeModel.CurrentGameMode != GameModeType.DisabledEnemies)
			{
				_gamePhasesModel.BeforeGamePhaseLoopActivated += CleanupEnemiesPool;
				_gamePhasesModel.BeforeFearAllEnemies += PrepareSpawning;
				_gamePhasesModel.OnGamePhaseActivated += StartupSpawning;
			}
		}

		public void Dispose()
		{
			_gamePhasesModel.BeforeGamePhaseLoopActivated -= CleanupEnemiesPool;
			_gamePhasesModel.BeforeFearAllEnemies -= PrepareSpawning;
			_gamePhasesModel.OnGamePhaseActivated -= StartupSpawning;
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				CleanupSpawnState();
			}
		}

		public void OnSharedModeMasterMigrationCompleted(NetworkRunner runner)
		{
			if (!(runner == null) && runner.IsRunning)
			{
				if (!runner.IsSharedModeMasterClient)
				{
					DisposeSpawnProcess();
				}
				else if (_gameModeModel.CurrentGameMode != GameModeType.DisabledEnemies && _enemySpawnConfigurationModel.CurrentSpawnConfiguration != EnemySpawnConfigurationType.No_Enemies && _cts == null)
				{
					_cts = new CancellationTokenSource();
					StartSpawnLoopAsync(_levelModel.CurrentLevel, _cts.Token, reseedForPhase: false).Forget();
				}
			}
		}

		public void ResetForLevelExit()
		{
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				CleanupSpawnState();
			}
		}

		private void PrepareSpawning()
		{
			if (!(_enemySpawnConfigurationProvider.GetEnemySpawnConfiguration() == null) && _enemySpawnConfigurationModel.CurrentSpawnConfiguration != EnemySpawnConfigurationType.No_Enemies && _multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				DisposeSpawnProcess();
				_seededPlanBeforeFear = false;
				if (_enemySpawnStatesModel.IsAttached)
				{
					SeedSpawnPlan(_levelModel.CurrentLevel);
					_seededPlanBeforeFear = true;
				}
			}
		}

		private void StartupSpawning()
		{
			if (!(_enemySpawnConfigurationProvider.GetEnemySpawnConfiguration() == null) && _enemySpawnConfigurationModel.CurrentSpawnConfiguration != EnemySpawnConfigurationType.No_Enemies && _multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				DisposeSpawnProcess();
				_cts = new CancellationTokenSource();
				bool reseedForPhase = !_seededPlanBeforeFear;
				_seededPlanBeforeFear = false;
				StartSpawnLoopAsync(_levelModel.CurrentLevel, _cts.Token, reseedForPhase).Forget();
			}
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

		private void CleanupSpawnState()
		{
			DisposeSpawnProcess();
			ClearSpawnedEnemies();
			_pendingSpawns.Clear();
			_enemiesPoolModel.EnemiesSpawnChances.Clear();
			_enemiesPoolModel.ChosenEnemyTypes.Clear();
			_seededPlanBeforeFear = false;
		}

		private async UniTaskVoid StartSpawnLoopAsync(LevelType levelType, CancellationToken token, bool reseedForPhase)
		{
			_ = 3;
			try
			{
				await UniTask.WaitUntil(() => _enemySpawnStatesModel.IsAttached, PlayerLoopTiming.Update, token);
				await UniTask.WaitUntil(() => _enemySpawnStatesModel.IsAuthority, PlayerLoopTiming.Update, token);
				if (reseedForPhase)
				{
					SeedSpawnPlan(levelType);
				}
				else
				{
					await UniTask.WaitUntil(() => _enemySpawnStatesModel.States.Count > 0, PlayerLoopTiming.Update, token);
					RebuildTrackingFromLiveWorld();
					ReseedRespawnScheduleToNow();
				}
				double lastSimTime = _multiplayerModel.NetworkRunner.SimulationTime;
				while (!token.IsCancellationRequested)
				{
					await UniTask.Delay(TimeSpan.FromSeconds(1.0), ignoreTimeScale: false, PlayerLoopTiming.Update, token);
					if (!_enemySpawnGate.IsSpawningAllowed)
					{
						lastSimTime = _multiplayerModel.NetworkRunner.SimulationTime;
						continue;
					}
					if (!_enemySpawnStatesModel.IsAttached)
					{
						lastSimTime = _multiplayerModel.NetworkRunner.SimulationTime;
						continue;
					}
					if (!_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
					{
						lastSimTime = _multiplayerModel.NetworkRunner.SimulationTime;
						continue;
					}
					int num = (int)((double)_multiplayerModel.NetworkRunner.SimulationTime - lastSimTime);
					if (num >= 1)
					{
						lastSimTime += (double)num;
						int num2 = _enemySpawnStatesModel.SessionTimePassed.Value + num;
						_enemySpawnStatesModel.SessionTimePassed.Value = num2;
						TickSpawns(levelType, num2);
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
				Debug.LogError($"EnemySpawnSystem loop error: {arg}");
			}
		}

		private void TickSpawns(LevelType levelType, int sessionTime)
		{
			List<EnemyType> list = new List<EnemyType>(_enemySpawnStatesModel.States.Items.Keys);
			List<EnemyType> list2 = new List<EnemyType>();
			foreach (EnemyType item in list)
			{
				if (!_enemySpawnStatesModel.States.TryGetValue(item, out var value))
				{
					continue;
				}
				ExactEnemySpawnConfiguration exactEnemySpawnConfiguration = ResolveConfig(levelType, item);
				if (exactEnemySpawnConfiguration == null)
				{
					continue;
				}
				if (AliveCount(item) + PendingCount(item) >= value.MaxSpawnedCount)
				{
					if (value.NextSpawnSessionTime != -1)
					{
						value.NextSpawnSessionTime = -1;
						_enemySpawnStatesModel.States.Set(item, value);
					}
				}
				else if (value.NextSpawnSessionTime == -1)
				{
					value.NextSpawnSessionTime = sessionTime + NextInterval(value, exactEnemySpawnConfiguration);
					_enemySpawnStatesModel.States.Set(item, value);
				}
				else if (sessionTime >= value.NextSpawnSessionTime)
				{
					list2.Add(item);
				}
			}
			list2.Sort(CompareDueSpawnTypes);
			foreach (EnemyType item2 in list2)
			{
				if (TryChooseSpawnPoint(item2, out var chosenPoint) && _enemySpawnStatesModel.States.TryGetValue(item2, out var value2))
				{
					ExactEnemySpawnConfiguration exactEnemySpawnConfiguration2 = ResolveConfig(levelType, item2);
					if (exactEnemySpawnConfiguration2 != null)
					{
						IncrementPending(item2);
						SpawnAndLog(item2, levelType, chosenPoint).Forget();
						value2.TotalSpawnedCount++;
						bool flag = AliveCount(item2) + PendingCount(item2) >= value2.MaxSpawnedCount;
						value2.NextSpawnSessionTime = (flag ? (-1) : (sessionTime + NextInterval(value2, exactEnemySpawnConfiguration2)));
						_enemySpawnStatesModel.States.Set(item2, value2);
						break;
					}
				}
			}
		}

		private int CompareDueSpawnTypes(EnemyType left, EnemyType right)
		{
			_enemySpawnStatesModel.States.TryGetValue(left, out var value);
			_enemySpawnStatesModel.States.TryGetValue(right, out var value2);
			int num = value.TotalSpawnedCount.CompareTo(value2.TotalSpawnedCount);
			if (num != 0)
			{
				return num;
			}
			int num2 = value.NextSpawnSessionTime.CompareTo(value2.NextSpawnSessionTime);
			if (num2 != 0)
			{
				return num2;
			}
			return left.CompareTo(right);
		}

		private static int NextInterval(EnemySpawnStateData state, ExactEnemySpawnConfiguration config)
		{
			if (state.TotalSpawnedCount >= state.MaxSpawnedCount)
			{
				return Math.Max(1, config.RespawnInterval);
			}
			return Math.Max(1, config.SpawnInterval);
		}

		private bool TryChooseSpawnPoint(EnemyType enemyType, out EnemySpawnPointData chosenPoint)
		{
			chosenPoint = null;
			List<EnemySpawnPointData> availableSpawnPoints = GetAvailableSpawnPoints(enemyType, isOneTimeSpawnPoint: true);
			List<EnemySpawnPointData> availableSpawnPoints2 = GetAvailableSpawnPoints(enemyType, isOneTimeSpawnPoint: false);
			if (availableSpawnPoints2.Count == 0 && availableSpawnPoints.Count == 0)
			{
				return false;
			}
			bool flag = _enemySpawnConfigurationProvider.GetEnemySpawnConfiguration().CloseSpawnEnemyTypes.Contains(enemyType);
			if (availableSpawnPoints.Count != 0)
			{
				chosenPoint = (flag ? ChooseClosestFromPlayers(availableSpawnPoints) : ChooseFurthestFromPlayers(availableSpawnPoints));
				_enemySpawnPointsModel.UnRegisterSpawnPoint(enemyType, chosenPoint);
				return true;
			}
			chosenPoint = (flag ? ChooseClosestFromPlayers(availableSpawnPoints2) : ChooseFurthestFromPlayers(availableSpawnPoints2));
			return true;
		}

		private async UniTaskVoid SpawnAndLog(EnemyType enemyType, LevelType levelType, EnemySpawnPointData point)
		{
			try
			{
				IEnemyBehaviour enemyBehaviour = await _enemySpawnService.SpawnEnemyAtSpawnPosition(enemyType, levelType, point);
				if (enemyBehaviour != null)
				{
					RegisterSpawnedEnemy(enemyType, enemyBehaviour);
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
				Debug.LogError($"EnemySpawnSystem failed to spawn {enemyType} on {levelType}: {arg}");
			}
			finally
			{
				DecrementPending(enemyType);
			}
		}

		private void RegisterSpawnedEnemy(EnemyType enemyType, IEnemyBehaviour spawnedEnemy)
		{
			if (spawnedEnemy != null)
			{
				if (!_spawnedEnemies.TryGetValue(enemyType, out var value))
				{
					value = new HashSet<IEnemyBehaviour>();
					_spawnedEnemies[enemyType] = value;
				}
				if (value.Add(spawnedEnemy))
				{
					spawnedEnemy.OnDeath += OnEnemyDeath;
				}
			}
		}

		private void RebuildTrackingFromLiveWorld()
		{
			ClearSpawnedEnemies();
			NetworkBehaviour[] array = UnityEngine.Object.FindObjectsByType<NetworkBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] is IEnemyBehaviour enemyBehaviour && !(enemyBehaviour.NetworkObject == null) && enemyBehaviour.NetworkObject.IsValid && _enemySpawnStatesModel.States.ContainsKey(enemyBehaviour.EnemyType))
				{
					RegisterSpawnedEnemy(enemyBehaviour.EnemyType, enemyBehaviour);
				}
			}
		}

		private void ReseedRespawnScheduleToNow()
		{
			int value = _enemySpawnStatesModel.SessionTimePassed.Value;
			foreach (EnemyType item in new List<EnemyType>(_enemySpawnStatesModel.States.Items.Keys))
			{
				if (_enemySpawnStatesModel.States.TryGetValue(item, out var value2) && value2.NextSpawnSessionTime != -1 && value2.NextSpawnSessionTime > value)
				{
					value2.NextSpawnSessionTime = value;
					_enemySpawnStatesModel.States.Set(item, value2);
				}
			}
		}

		private void ClearSpawnedEnemies()
		{
			foreach (KeyValuePair<EnemyType, HashSet<IEnemyBehaviour>> spawnedEnemy in _spawnedEnemies)
			{
				foreach (IEnemyBehaviour item in spawnedEnemy.Value)
				{
					if (item != null)
					{
						item.OnDeath -= OnEnemyDeath;
					}
				}
			}
			_spawnedEnemies.Clear();
		}

		private int AliveCount(EnemyType enemyType)
		{
			if (!_spawnedEnemies.TryGetValue(enemyType, out var value))
			{
				return 0;
			}
			value.RemoveWhere((IEnemyBehaviour enemy) => enemy == null || enemy.NetworkObject == null || !enemy.NetworkObject.IsValid);
			return value.Count;
		}

		private int PendingCount(EnemyType enemyType)
		{
			if (!_pendingSpawns.TryGetValue(enemyType, out var value))
			{
				return 0;
			}
			return value;
		}

		private void IncrementPending(EnemyType enemyType)
		{
			_pendingSpawns[enemyType] = PendingCount(enemyType) + 1;
		}

		private void DecrementPending(EnemyType enemyType)
		{
			int num = PendingCount(enemyType) - 1;
			if (num <= 0)
			{
				_pendingSpawns.Remove(enemyType);
			}
			else
			{
				_pendingSpawns[enemyType] = num;
			}
		}

		private ExactEnemySpawnConfiguration ResolveConfig(LevelType levelType, EnemyType enemyType)
		{
			EnemiesExactSpawnData exactConfigs = GetExactConfigs(levelType);
			for (int i = 0; i < exactConfigs.ExactSpawnConfigurations.Count; i++)
			{
				if (exactConfigs.ExactSpawnConfigurations[i].EnemyType == enemyType)
				{
					return exactConfigs.ExactSpawnConfigurations[i];
				}
			}
			return null;
		}

		private EnemiesExactSpawnData GetExactConfigs(LevelType levelType)
		{
			if (!_enemySpawnConfigurationProvider.GetEnemySpawnConfiguration().EnemiesSpawnConfigurationsByGamePhase.TryGetValue(levelType, out var value))
			{
				return _enemySpawnConfigurationProvider.GetEnemySpawnConfiguration().DefaultEnemiesSpawnConfigurations;
			}
			return value;
		}

		private void SeedSpawnPlan(LevelType levelType)
		{
			EnemiesExactSpawnData exactConfigs = GetExactConfigs(levelType);
			Dictionary<EnemyType, EnemySpawnStateData> dictionary = new Dictionary<EnemyType, EnemySpawnStateData>();
			foreach (KeyValuePair<EnemyType, EnemySpawnStateData> item in _enemySpawnStatesModel.States.Items)
			{
				dictionary[item.Key] = item.Value;
			}
			if (_enemiesPoolModel.EnemiesSpawnChances.Count == 0)
			{
				foreach (ExactEnemySpawnConfiguration exactSpawnConfiguration in exactConfigs.ExactSpawnConfigurations)
				{
					if (_enemiesPoolModel.EnemiesSpawnChances.Any((KeyValuePair<ExactEnemySpawnConfiguration, float> s) => s.Key.EnemyType == exactSpawnConfiguration.EnemyType))
					{
						Debug.LogError($"Some configuration of one enemy type {exactSpawnConfiguration.EnemyType} for level {levelType}");
					}
					else
					{
						_enemiesPoolModel.EnemiesSpawnChances.TryAdd(exactSpawnConfiguration, exactSpawnConfiguration.DefaultChance);
					}
				}
			}
			Dictionary<EnemyType, ExactEnemySpawnConfiguration> dictionary2 = new Dictionary<EnemyType, ExactEnemySpawnConfiguration>();
			Dictionary<EnemyType, int> dictionary3 = new Dictionary<EnemyType, int>();
			float currentSize = 0f;
			_enemiesPoolModel.ChosenEnemyTypes.Clear();
			ExactEnemySpawnConfiguration exactEnemySpawnConfiguration;
			for (; currentSize < exactConfigs.MaxSize; currentSize += exactEnemySpawnConfiguration.Size)
			{
				Dictionary<ExactEnemySpawnConfiguration, float> dictionary4 = _enemiesPoolModel.EnemiesSpawnChances.Where((KeyValuePair<ExactEnemySpawnConfiguration, float> e) => e.Key.Size <= exactConfigs.MaxSize - currentSize).ToDictionary((KeyValuePair<ExactEnemySpawnConfiguration, float> i) => i.Key, (KeyValuePair<ExactEnemySpawnConfiguration, float> i) => i.Value);
				if (dictionary4.Count == 0)
				{
					break;
				}
				float maxInclusive = dictionary4.Sum((KeyValuePair<ExactEnemySpawnConfiguration, float> e) => e.Value);
				float num = UnityEngine.Random.Range(0f, maxInclusive);
				exactEnemySpawnConfiguration = null;
				float num2 = 0f;
				foreach (KeyValuePair<ExactEnemySpawnConfiguration, float> item2 in dictionary4)
				{
					num2 += item2.Value;
					if (num <= num2)
					{
						exactEnemySpawnConfiguration = item2.Key;
						break;
					}
				}
				if (exactEnemySpawnConfiguration == null)
				{
					exactEnemySpawnConfiguration = dictionary4.Last().Key;
				}
				if (_enemiesPoolModel.EnemiesSpawnChances[exactEnemySpawnConfiguration] > exactEnemySpawnConfiguration.DefaultChance + (float)_gamePhasesModel.CurrentPhaseCount * exactEnemySpawnConfiguration.ChanceMultiplyByPhaseCount)
				{
					_enemiesPoolModel.EnemiesSpawnChances[exactEnemySpawnConfiguration] = exactEnemySpawnConfiguration.DefaultChance + (float)_gamePhasesModel.CurrentPhaseCount * exactEnemySpawnConfiguration.ChanceMultiplyByPhaseCount;
				}
				_enemiesPoolModel.EnemiesSpawnChances[exactEnemySpawnConfiguration] /= exactConfigs.ChanceDivideIfSpawned + exactConfigs.ChanceDivideIfSpawnedIncreasedByPhaseCount * (float)_gamePhasesModel.CurrentPhaseCount;
				_enemiesPoolModel.ChosenEnemyTypes.Add(exactEnemySpawnConfiguration.EnemyType);
				dictionary2[exactEnemySpawnConfiguration.EnemyType] = exactEnemySpawnConfiguration;
				dictionary3[exactEnemySpawnConfiguration.EnemyType] = ((!dictionary3.TryGetValue(exactEnemySpawnConfiguration.EnemyType, out var value)) ? 1 : (value + 1));
			}
			Dictionary<EnemyType, EnemySpawnStateData> dictionary5 = new Dictionary<EnemyType, EnemySpawnStateData>();
			foreach (KeyValuePair<EnemyType, int> item3 in dictionary3)
			{
				int nextSpawnSessionTime = dictionary2[item3.Key].FirstSpawnTime;
				int totalSpawnedCount = 0;
				if (dictionary.TryGetValue(item3.Key, out var value2))
				{
					nextSpawnSessionTime = ((value2.NextSpawnSessionTime == -1) ? dictionary2[item3.Key].FirstSpawnTime : value2.NextSpawnSessionTime);
					totalSpawnedCount = value2.TotalSpawnedCount;
				}
				dictionary5[item3.Key] = new EnemySpawnStateData
				{
					NextSpawnSessionTime = nextSpawnSessionTime,
					TotalSpawnedCount = totalSpawnedCount,
					MaxSpawnedCount = item3.Value
				};
			}
			_enemySpawnStatesModel.SessionTimePassed.Value = 0;
			_enemySpawnStatesModel.States.ReplaceAll(dictionary5);
			LogSpawnPlan(exactConfigs);
		}

		private void LogSpawnPlan(EnemiesExactSpawnData exactConfigs)
		{
			foreach (ExactEnemySpawnConfiguration item in _enemiesPoolModel.EnemiesSpawnChances.Keys.ToList())
			{
				if (_enemiesPoolModel.ChosenEnemyTypes.Contains(item.EnemyType))
				{
					_enemiesPoolModel.EnemiesSpawnChances[item] = item.DefaultChance + (float)_gamePhasesModel.CurrentPhaseCount * item.ChanceMultiplyByPhaseCount;
				}
				else
				{
					_enemiesPoolModel.EnemiesSpawnChances[item] *= exactConfigs.ChanceMultiplyIfNotSpawned + exactConfigs.ChanceMultiplyIfNotSpawnedIncreasedByPhaseCount * (float)_gamePhasesModel.CurrentPhaseCount;
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Spawned enemies: ");
			foreach (EnemyType chosenEnemyType in _enemiesPoolModel.ChosenEnemyTypes)
			{
				stringBuilder.Append(chosenEnemyType.ToString() + ", ");
			}
			stringBuilder.AppendLine(string.Empty);
			stringBuilder.AppendLine(string.Format("{0,-20} {1,10} {2,20}", "Enemy", "Chance", "Bar"));
			stringBuilder.AppendLine(new string('-', 52));
			float num = _enemiesPoolModel.EnemiesSpawnChances.Values.Max();
			float num2 = _enemiesPoolModel.EnemiesSpawnChances.Sum((KeyValuePair<ExactEnemySpawnConfiguration, float> e) => e.Value);
			foreach (KeyValuePair<ExactEnemySpawnConfiguration, float> enemiesSpawnChance in _enemiesPoolModel.EnemiesSpawnChances)
			{
				int num3 = Mathf.RoundToInt(enemiesSpawnChance.Value / num * 20f);
				string text = new string('█', num3) + new string('░', 20 - num3);
				stringBuilder.AppendLine($"{enemiesSpawnChance.Key.EnemyType}: {enemiesSpawnChance.Value}({enemiesSpawnChance.Value / num2 * 100f:F1}%) {text}");
			}
			ExtendedDebug.LogFiltered(DebugFilterType.EnemiesSpawn, stringBuilder);
			_overlayService.SwitchOverlay(DebugFilterType.EnemiesSpawn, stringBuilder);
		}

		private List<EnemySpawnPointData> GetAvailableSpawnPoints(EnemyType enemyType, bool isOneTimeSpawnPoint)
		{
			if (!_enemySpawnPointsModel.SpawnPointsPool.TryGetValue(enemyType, out var value))
			{
				return new List<EnemySpawnPointData>();
			}
			List<EnemySpawnPointData> list = new List<EnemySpawnPointData>(value.Count);
			for (int i = 0; i < value.Count; i++)
			{
				EnemySpawnPointData enemySpawnPointData = value[i];
				if (enemySpawnPointData.IsOneTimeSpawnPoint == isOneTimeSpawnPoint)
				{
					list.Add(enemySpawnPointData);
				}
			}
			List<Vector3> list2 = _playerPositionsProvider?.GetPlayerPositions();
			if (list2 == null || list2.Count == 0)
			{
				return list;
			}
			int num = _enemySpawnConfigurationProvider.GetEnemySpawnConfiguration()?.RadiusFromPlayerToNotSpawn ?? 0;
			float num2 = num * num;
			List<EnemySpawnPointData> list3 = new List<EnemySpawnPointData>(list.Count);
			foreach (EnemySpawnPointData item in list)
			{
				bool flag = false;
				foreach (Vector3 item2 in list2)
				{
					if ((item.Position - item2).sqrMagnitude <= num2)
					{
						flag = true;
						break;
					}
				}
				if (!flag && !item.Occupancy.IsOccupied)
				{
					list3.Add(item);
				}
			}
			return list3;
		}

		private EnemySpawnPointData ChooseFurthestFromPlayers(List<EnemySpawnPointData> candidates)
		{
			List<Vector3> list = _playerPositionsProvider?.GetPlayerPositions();
			if (list == null || list.Count == 0)
			{
				return candidates[0];
			}
			EnemySpawnPointData enemySpawnPointData = null;
			float num = float.MinValue;
			foreach (EnemySpawnPointData candidate in candidates)
			{
				float num2 = float.MaxValue;
				foreach (Vector3 item in list)
				{
					float sqrMagnitude = (candidate.Position - item).sqrMagnitude;
					if (sqrMagnitude < num2)
					{
						num2 = sqrMagnitude;
					}
				}
				if (num2 > num)
				{
					num = num2;
					enemySpawnPointData = candidate;
				}
			}
			return enemySpawnPointData ?? candidates[0];
		}

		private EnemySpawnPointData ChooseClosestFromPlayers(List<EnemySpawnPointData> candidates)
		{
			List<Vector3> list = _playerPositionsProvider?.GetPlayerPositions();
			if (list == null || list.Count == 0)
			{
				return candidates[0];
			}
			EnemySpawnPointData enemySpawnPointData = null;
			float num = float.MaxValue;
			foreach (EnemySpawnPointData candidate in candidates)
			{
				float num2 = float.MaxValue;
				foreach (Vector3 item in list)
				{
					float sqrMagnitude = (candidate.Position - item).sqrMagnitude;
					if (sqrMagnitude < num2)
					{
						num2 = sqrMagnitude;
					}
				}
				if (num2 < num)
				{
					num = num2;
					enemySpawnPointData = candidate;
				}
			}
			return enemySpawnPointData ?? candidates[0];
		}

		private void OnEnemyDeath(IEnemyBehaviour deadEnemy)
		{
			if (deadEnemy == null)
			{
				return;
			}
			deadEnemy.OnDeath -= OnEnemyDeath;
			foreach (KeyValuePair<EnemyType, HashSet<IEnemyBehaviour>> spawnedEnemy in _spawnedEnemies)
			{
				if (spawnedEnemy.Value.Remove(deadEnemy))
				{
					string networkObjectId = ((deadEnemy.NetworkObject != null && deadEnemy.NetworkObject.IsValid) ? deadEnemy.NetworkObject.Id.ToString() : string.Empty);
					_gameplayEventBus.Publish(new OnEnemyDespawnedGameplayEvent(spawnedEnemy.Key.ToString(), networkObjectId));
					break;
				}
			}
		}

		private void CleanupEnemiesPool()
		{
			_enemiesPoolModel.CleanupEnemiesPool();
		}
	}
}
