using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.LevelModule.Scripts;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	public class EnemySpawnService : IEnemySpawnService
	{
		private readonly IEnemyFactory _enemyFactory;

		private readonly EnemySpawnPointsModel _enemySpawnPointsModel;

		public EnemySpawnService(IEnemyFactory enemyFactory, EnemySpawnPointsModel enemySpawnPointsModel)
		{
			_enemyFactory = enemyFactory;
			_enemySpawnPointsModel = enemySpawnPointsModel;
		}

		public async UniTask<IEnemyBehaviour> SpawnEnemy(EnemyType enemyType, LevelType levelType)
		{
			EnemySpawnPointData targetSpawnPoint = PickRandomEnemySpawnPointData(enemyType);
			IEnemyBehaviour enemyBehaviour = await _enemyFactory.SpawnEnemy(enemyType, levelType, targetSpawnPoint.Position, targetSpawnPoint.AreaPosition, targetSpawnPoint.Rotation);
			EnemySpawnPointOccupancyUtility.TryOccupy(targetSpawnPoint, enemyBehaviour);
			return enemyBehaviour;
		}

		public async UniTask<IEnemyBehaviour> SpawnEnemy(EnemyType enemyType)
		{
			EnemySpawnPointData targetSpawnPoint = PickRandomEnemySpawnPointData(enemyType);
			IEnemyBehaviour enemyBehaviour = await _enemyFactory.SpawnEnemy(enemyType, targetSpawnPoint.Position, targetSpawnPoint.AreaPosition, targetSpawnPoint.Rotation);
			EnemySpawnPointOccupancyUtility.TryOccupy(targetSpawnPoint, enemyBehaviour);
			return enemyBehaviour;
		}

		private EnemySpawnPointData PickRandomEnemySpawnPointData(EnemyType enemyType)
		{
			if (!_enemySpawnPointsModel.SpawnPointsPool.TryGetValue(enemyType, out var value) || value.Count == 0)
			{
				return null;
			}
			List<EnemySpawnPointData> list = new List<EnemySpawnPointData>();
			for (int i = 0; i < value.Count; i++)
			{
				EnemySpawnPointData enemySpawnPointData = value[i];
				if (!enemySpawnPointData.IsOneTimeSpawnPoint && !enemySpawnPointData.Occupancy.IsOccupied)
				{
					list.Add(enemySpawnPointData);
				}
			}
			if (list.Count != 0)
			{
				return list[Random.Range(0, list.Count)];
			}
			return null;
		}

		public async UniTask<IEnemyBehaviour> SpawnEnemyAtSpawnPosition(EnemyType enemyType, LevelType levelType, EnemySpawnPointData spawnPosition)
		{
			IEnemyBehaviour enemyBehaviour = await _enemyFactory.SpawnEnemy(enemyType, levelType, spawnPosition.Position, spawnPosition.AreaPosition, spawnPosition.Rotation);
			EnemySpawnPointOccupancyUtility.TryOccupy(spawnPosition, enemyBehaviour);
			return enemyBehaviour;
		}

		public bool TryGetEnemyRandomSpawnPosition(EnemyType enemyType, out Vector3 spawnPosition)
		{
			spawnPosition = Vector3.zero;
			if (!_enemySpawnPointsModel.SpawnPointsPool.TryGetValue(enemyType, out var value) || value.Count == 0)
			{
				return false;
			}
			List<EnemySpawnPointData> list = new List<EnemySpawnPointData>(value.Count);
			for (int i = 0; i < value.Count; i++)
			{
				EnemySpawnPointData enemySpawnPointData = value[i];
				if (!enemySpawnPointData.IsOneTimeSpawnPoint)
				{
					list.Add(enemySpawnPointData);
				}
			}
			if (list.Count == 0)
			{
				return false;
			}
			spawnPosition = list[Random.Range(0, list.Count)].Position;
			return true;
		}
	}
}
