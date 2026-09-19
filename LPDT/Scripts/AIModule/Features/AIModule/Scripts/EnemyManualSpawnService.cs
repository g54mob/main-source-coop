using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Features.AIModule.Scripts.Services;
using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using UnityEngine.AI;

namespace Features.AIModule.Scripts
{
	public class EnemyManualSpawnService : IEnemyManualSpawnService
	{
		private const float NearPlayerSpawnMinRadius = 0.8f;

		private const float NearPlayerSpawnMaxRadius = 1.2f;

		private const float NearPlayerSpawnSampleRadius = 1.5f;

		private const int NearPlayerSpawnAttempts = 24;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly LevelModel _levelModel;

		private readonly IEnemySpawnConfigurationProvider _enemySpawnConfigurationProvider;

		private readonly EnemySpawnPointsModel _enemySpawnPointsModel;

		private readonly IPlayerPositionsProvider _playerPositionsProvider;

		public EnemyManualSpawnService(MultiplayerModel multiplayerModel, LevelModel levelModel, IEnemySpawnConfigurationProvider enemySpawnConfigurationProvider, EnemySpawnPointsModel enemySpawnPointsModel, IPlayerPositionsProvider playerPositionsProvider)
		{
			_multiplayerModel = multiplayerModel;
			_levelModel = levelModel;
			_enemySpawnConfigurationProvider = enemySpawnConfigurationProvider;
			_enemySpawnPointsModel = enemySpawnPointsModel;
			_playerPositionsProvider = playerPositionsProvider;
		}

		public async UniTask<IEnemyBehaviour> SpawnEnemy(EnemyType enemyType)
		{
			if (!TryPrepareSpawn(enemyType, out var runner, out var enemyPrefab))
			{
				return null;
			}
			if (!TryChooseSpawnPoint(enemyType, out var spawnPoint))
			{
				Debug.LogWarning($"Enemy manual spawn cannot find an available spawn point for {enemyType}.");
				return null;
			}
			IEnemyBehaviour enemyBehaviour = await SpawnEnemyAtPosition(runner, enemyType, enemyPrefab, spawnPoint.Position, spawnPoint.Rotation, spawnPoint.AreaPosition);
			EnemySpawnPointOccupancyUtility.TryOccupy(spawnPoint, enemyBehaviour);
			return enemyBehaviour;
		}

		public async UniTask<IEnemyBehaviour> SpawnEnemyNearPlayer(EnemyType enemyType)
		{
			if (!TryPrepareSpawn(enemyType, out var runner, out var enemyPrefab))
			{
				return null;
			}
			if (!TryChooseNearPlayerSpawnPose(out var spawnPosition, out var spawnRotation))
			{
				Debug.LogWarning($"Enemy manual spawn cannot find a near-player NavMesh point for {enemyType}.");
				return null;
			}
			return await SpawnEnemyAtPosition(runner, enemyType, enemyPrefab, spawnPosition, spawnRotation, spawnPosition);
		}

		private bool TryPrepareSpawn(EnemyType enemyType, out NetworkRunner runner, out NetworkPrefabRef enemyPrefab)
		{
			runner = null;
			enemyPrefab = default(NetworkPrefabRef);
			if (enemyType == EnemyType.None)
			{
				Debug.LogWarning("Enemy manual spawn requires a concrete enemy type.");
				return false;
			}
			runner = _multiplayerModel.NetworkRunner;
			if (runner == null || !runner.IsRunning)
			{
				Debug.LogWarning("Enemy manual spawn requires an active network session.");
				return false;
			}
			if (!runner.IsSharedModeMasterClient)
			{
				Debug.LogWarning("Enemy manual spawn is host-only.");
				return false;
			}
			LevelType currentLevel = _levelModel.CurrentLevel;
			if (currentLevel == LevelType.None)
			{
				Debug.LogWarning("Enemy manual spawn requires a loaded level.");
				return false;
			}
			EnemySpawnConfiguration enemySpawnConfiguration = _enemySpawnConfigurationProvider.GetEnemySpawnConfiguration();
			if (enemySpawnConfiguration == null)
			{
				Debug.LogWarning("Enemy manual spawn cannot find an enemy spawn configuration.");
				return false;
			}
			if (!TryResolveEnemyPrefab(enemySpawnConfiguration, currentLevel, enemyType, out enemyPrefab))
			{
				Debug.LogWarning($"Enemy manual spawn cannot resolve prefab for {enemyType} on {currentLevel}.");
				return false;
			}
			return true;
		}

		private async UniTask<IEnemyBehaviour> SpawnEnemyAtPosition(NetworkRunner runner, EnemyType enemyType, NetworkPrefabRef enemyPrefab, Vector3 spawnPosition, Quaternion spawnRotation, Vector3 areaPosition)
		{
			NetworkObject networkObject = await runner.SpawnAsync(enemyPrefab, spawnPosition, spawnRotation);
			if (networkObject == null)
			{
				return null;
			}
			if (!networkObject.TryGetComponent<IEnemyBehaviour>(out var component))
			{
				if (networkObject.IsValid)
				{
					networkObject.DespawnHierarchy();
				}
				Debug.LogWarning(string.Format("Enemy manual spawn prefab for {0} does not contain {1}.", enemyType, "IEnemyBehaviour"));
				return null;
			}
			component.SetAreaPosition(areaPosition);
			return component;
		}

		private bool TryResolveEnemyPrefab(EnemySpawnConfiguration configuration, LevelType levelType, EnemyType enemyType, out NetworkPrefabRef enemyPrefab)
		{
			if (TryResolveSideBossPrefab(configuration, levelType, enemyType, out enemyPrefab))
			{
				return true;
			}
			if (TryResolveNormalEnemyPrefab(configuration, levelType, enemyType, out enemyPrefab))
			{
				return true;
			}
			enemyPrefab = default(NetworkPrefabRef);
			return false;
		}

		private bool TryResolveNormalEnemyPrefab(EnemySpawnConfiguration configuration, LevelType levelType, EnemyType enemyType, out NetworkPrefabRef enemyPrefab)
		{
			if (configuration.EnemiesSpawnConfigurationsByGamePhase != null && configuration.EnemiesSpawnConfigurationsByGamePhase.TryGetValue(levelType, out var value) && TryFindNormalEnemyPrefab(value, enemyType, out enemyPrefab))
			{
				return true;
			}
			if (TryFindNormalEnemyPrefab(configuration.DefaultEnemiesSpawnConfigurations, enemyType, out enemyPrefab))
			{
				return true;
			}
			if (configuration.EnemiesSpawnConfigurationsByGamePhase != null)
			{
				LevelType[] array = Enum.GetValues(typeof(LevelType)).Cast<LevelType>().ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != levelType && configuration.EnemiesSpawnConfigurationsByGamePhase.TryGetValue(array[i], out var value2) && TryFindNormalEnemyPrefab(value2, enemyType, out enemyPrefab))
					{
						return true;
					}
				}
			}
			enemyPrefab = default(NetworkPrefabRef);
			return false;
		}

		private bool TryFindNormalEnemyPrefab(EnemiesExactSpawnData spawnData, EnemyType enemyType, out NetworkPrefabRef enemyPrefab)
		{
			ExactEnemySpawnConfiguration exactEnemySpawnConfiguration = spawnData?.ExactSpawnConfigurations?.FirstOrDefault((ExactEnemySpawnConfiguration configuration) => configuration.EnemyType == enemyType);
			if (exactEnemySpawnConfiguration == null)
			{
				enemyPrefab = default(NetworkPrefabRef);
				return false;
			}
			enemyPrefab = exactEnemySpawnConfiguration.EnemyPrefab;
			return true;
		}

		private bool TryResolveSideBossPrefab(EnemySpawnConfiguration configuration, LevelType levelType, EnemyType enemyType, out NetworkPrefabRef enemyPrefab)
		{
			if (configuration.SideBossesSpawnConfigurationsByLevel != null && configuration.SideBossesSpawnConfigurationsByLevel.TryGetValue(levelType, out var value) && TryFindSideBossPrefab(value, enemyType, out enemyPrefab))
			{
				return true;
			}
			if (configuration.SideBossesSpawnConfigurationsByLevel != null)
			{
				LevelType[] array = Enum.GetValues(typeof(LevelType)).Cast<LevelType>().ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != levelType && configuration.SideBossesSpawnConfigurationsByLevel.TryGetValue(array[i], out var value2) && TryFindSideBossPrefab(value2, enemyType, out enemyPrefab))
					{
						return true;
					}
				}
			}
			enemyPrefab = default(NetworkPrefabRef);
			return false;
		}

		private bool TryFindSideBossPrefab(SideBossSpawnData spawnData, EnemyType enemyType, out NetworkPrefabRef enemyPrefab)
		{
			SideBossSpawnConfiguration sideBossSpawnConfiguration = spawnData?.BossSpawnConfigurations?.FirstOrDefault((SideBossSpawnConfiguration configuration) => configuration.EnemyType == enemyType);
			if (sideBossSpawnConfiguration == null)
			{
				enemyPrefab = default(NetworkPrefabRef);
				return false;
			}
			enemyPrefab = sideBossSpawnConfiguration.BossPrefab;
			return true;
		}

		private bool TryChooseSpawnPoint(EnemyType enemyType, out EnemySpawnPointData spawnPoint)
		{
			if (!_enemySpawnPointsModel.SpawnPointsPool.TryGetValue(enemyType, out var value))
			{
				spawnPoint = null;
				return false;
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
			if (list.Count == 0)
			{
				spawnPoint = null;
				return false;
			}
			spawnPoint = list[UnityEngine.Random.Range(0, list.Count)];
			return true;
		}

		private bool TryChooseNearPlayerSpawnPose(out Vector3 spawnPosition, out Quaternion spawnRotation)
		{
			spawnPosition = Vector3.zero;
			spawnRotation = Quaternion.identity;
			List<Vector3> list = _playerPositionsProvider?.GetPlayerPositions();
			if (list == null || list.Count == 0)
			{
				return false;
			}
			Vector3 vector = list[UnityEngine.Random.Range(0, list.Count)];
			for (int i = 0; i < 24; i++)
			{
				Vector2 vector2 = UnityEngine.Random.insideUnitCircle.normalized;
				if (vector2.sqrMagnitude <= 0.001f)
				{
					vector2 = Vector2.right;
				}
				float num = UnityEngine.Random.Range(0.8f, 1.2f);
				if (NavMesh.SamplePosition(vector + new Vector3(vector2.x, 0f, vector2.y) * num, out var hit, 1.5f, -1))
				{
					spawnPosition = hit.position;
					Vector3 vector3 = vector - spawnPosition;
					vector3.y = 0f;
					spawnRotation = ((vector3.sqrMagnitude > 0.001f) ? Quaternion.LookRotation(vector3.normalized) : Quaternion.identity);
					return true;
				}
			}
			if (!NavMesh.SamplePosition(vector, out var hit2, 1.5f, -1))
			{
				return false;
			}
			spawnPosition = hit2.position;
			spawnRotation = Quaternion.identity;
			return true;
		}
	}
}
