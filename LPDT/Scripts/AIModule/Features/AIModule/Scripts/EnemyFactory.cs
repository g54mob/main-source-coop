using System.Linq;
using Cysharp.Threading.Tasks;
using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	public class EnemyFactory : IEnemyFactory
	{
		private readonly MultiplayerModel _multiplayerModel;

		private IEnemySpawnConfigurationProvider _enemySpawnConfigurationProvider;

		public EnemyFactory(MultiplayerModel multiplayerModel, IEnemySpawnConfigurationProvider enemySpawnConfigurationProvider)
		{
			_enemySpawnConfigurationProvider = enemySpawnConfigurationProvider;
			_multiplayerModel = multiplayerModel;
		}

		public async UniTask<IEnemyBehaviour> SpawnEnemy(EnemyType enemyType, LevelType levelType, Vector3 spawnPosition, Vector3 areaPosition, Quaternion spawnRotation)
		{
			EnemiesExactSpawnData value;
			ExactEnemySpawnConfiguration exactEnemySpawnConfiguration = (_enemySpawnConfigurationProvider.GetEnemySpawnConfiguration().EnemiesSpawnConfigurationsByGamePhase.TryGetValue(levelType, out value) ? value : _enemySpawnConfigurationProvider.GetEnemySpawnConfiguration().DefaultEnemiesSpawnConfigurations).ExactSpawnConfigurations.FirstOrDefault((ExactEnemySpawnConfiguration e) => e.EnemyType == enemyType);
			if (exactEnemySpawnConfiguration == null)
			{
				Debug.LogError("No exact configuration found for level type: " + levelType.ToString() + ", enemy type: " + enemyType);
				return null;
			}
			NetworkObject networkObject = await _multiplayerModel.NetworkRunner.SpawnAsync(exactEnemySpawnConfiguration.EnemyPrefab, spawnPosition, spawnRotation);
			if (networkObject == null)
			{
				return null;
			}
			if (networkObject.TryGetComponent<IEnemyBehaviour>(out var component))
			{
				component.SetAreaPosition(areaPosition);
			}
			return component;
		}

		public async UniTask<IEnemyBehaviour> SpawnEnemy(EnemyType enemyType, Vector3 spawnPosition, Vector3 areaPosition, Quaternion spawnRotation)
		{
			ExactEnemySpawnConfiguration exactEnemySpawnConfiguration = _enemySpawnConfigurationProvider.GetEnemySpawnConfiguration().DefaultEnemiesSpawnConfigurations.ExactSpawnConfigurations.FirstOrDefault((ExactEnemySpawnConfiguration e) => e.EnemyType == enemyType);
			if (exactEnemySpawnConfiguration == null)
			{
				Debug.LogError("No exact configuration found for enemy: " + enemyType);
				return null;
			}
			if ((await _multiplayerModel.NetworkRunner.SpawnAsync(exactEnemySpawnConfiguration.EnemyPrefab, spawnPosition, spawnRotation)).TryGetComponent<IEnemyBehaviour>(out var component))
			{
				component.SetAreaPosition(areaPosition);
			}
			return component;
		}
	}
}
