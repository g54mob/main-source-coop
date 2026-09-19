using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.GameModeModule.Scripts;
using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	public class EnemyPrefabWarmup : IEnemyPrefabWarmup
	{
		private readonly EnemyResourceWarmupConfiguration _warmupConfiguration;

		private readonly IEnemySpawnConfigurationProvider _enemySpawnConfigurationProvider;

		private readonly EnemySpawnConfigurationModel _enemySpawnConfigurationModel;

		private readonly GameModeModel _gameModeModel;

		private readonly LevelModel _levelModel;

		private readonly HashSet<int> _warmedAssetInstanceIds = new HashSet<int>();

		public EnemyPrefabWarmup(EnemyResourceWarmupConfiguration warmupConfiguration, IEnemySpawnConfigurationProvider enemySpawnConfigurationProvider, EnemySpawnConfigurationModel enemySpawnConfigurationModel, GameModeModel gameModeModel, LevelModel levelModel)
		{
			_warmupConfiguration = warmupConfiguration;
			_enemySpawnConfigurationProvider = enemySpawnConfigurationProvider;
			_enemySpawnConfigurationModel = enemySpawnConfigurationModel;
			_gameModeModel = gameModeModel;
			_levelModel = levelModel;
		}

		public async UniTask WarmupForCurrentLevelAsync()
		{
			if (_gameModeModel.CurrentGameMode == GameModeType.DisabledEnemies || _enemySpawnConfigurationModel.CurrentSpawnConfiguration == EnemySpawnConfigurationType.No_Enemies || _warmupConfiguration == null)
			{
				return;
			}
			EnemySpawnConfiguration enemySpawnConfiguration = _enemySpawnConfigurationProvider.GetEnemySpawnConfiguration();
			if (!(enemySpawnConfiguration == null))
			{
				List<GameObject> stubs = CollectStubsForCurrentLevel(enemySpawnConfiguration, _levelModel.CurrentLevel);
				for (int i = 0; i < stubs.Count; i++)
				{
					WarmupStub(stubs[i]);
					await UniTask.Yield(PlayerLoopTiming.Update);
				}
			}
		}

		private List<GameObject> CollectStubsForCurrentLevel(EnemySpawnConfiguration spawnConfiguration, LevelType levelType)
		{
			List<GameObject> list = new List<GameObject>();
			HashSet<int> seenThisPass = new HashSet<int>();
			foreach (EnemyType item in CollectEnemyTypes(spawnConfiguration, levelType))
			{
				if (_warmupConfiguration.TryGetWarmupPrefab(item, out var warmupPrefab))
				{
					TryAddStub(list, seenThisPass, warmupPrefab);
				}
			}
			IReadOnlyList<GameObject> extraWarmupPrefabs = _warmupConfiguration.ExtraWarmupPrefabs;
			for (int i = 0; i < extraWarmupPrefabs.Count; i++)
			{
				TryAddStub(list, seenThisPass, extraWarmupPrefabs[i]);
			}
			return list;
		}

		private static HashSet<EnemyType> CollectEnemyTypes(EnemySpawnConfiguration spawnConfiguration, LevelType levelType)
		{
			HashSet<EnemyType> hashSet = new HashSet<EnemyType>();
			EnemiesExactSpawnData value;
			EnemiesExactSpawnData enemiesExactSpawnData = ((spawnConfiguration.EnemiesSpawnConfigurationsByGamePhase != null && spawnConfiguration.EnemiesSpawnConfigurationsByGamePhase.TryGetValue(levelType, out value)) ? value : spawnConfiguration.DefaultEnemiesSpawnConfigurations);
			if (enemiesExactSpawnData?.ExactSpawnConfigurations != null)
			{
				for (int i = 0; i < enemiesExactSpawnData.ExactSpawnConfigurations.Count; i++)
				{
					hashSet.Add(enemiesExactSpawnData.ExactSpawnConfigurations[i].EnemyType);
				}
			}
			if (spawnConfiguration.SideBossesSpawnConfigurationsByLevel != null && spawnConfiguration.SideBossesSpawnConfigurationsByLevel.TryGetValue(levelType, out var value2) && value2.BossSpawnConfigurations != null)
			{
				for (int j = 0; j < value2.BossSpawnConfigurations.Count; j++)
				{
					hashSet.Add(value2.BossSpawnConfigurations[j].EnemyType);
				}
			}
			return hashSet;
		}

		private void TryAddStub(List<GameObject> stubs, HashSet<int> seenThisPass, GameObject stub)
		{
			if (!(stub == null))
			{
				int instanceID = stub.GetInstanceID();
				if (!_warmedAssetInstanceIds.Contains(instanceID) && seenThisPass.Add(instanceID))
				{
					stubs.Add(stub);
				}
			}
		}

		private void WarmupStub(GameObject stub)
		{
			GameObject gameObject = new GameObject("EnemyResourceWarmupHolder");
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			gameObject.SetActive(value: false);
			gameObject.transform.position = new Vector3(0f, -10000f, 0f);
			try
			{
				UnityEngine.Object.Instantiate(stub, gameObject.transform, worldPositionStays: false);
				gameObject.SetActive(value: true);
				_warmedAssetInstanceIds.Add(stub.GetInstanceID());
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[EnemyPrefabWarmup] Failed to activate stub '" + stub.name + "': " + ex.Message);
			}
			finally
			{
				UnityEngine.Object.DestroyImmediate(gameObject);
			}
		}
	}
}
