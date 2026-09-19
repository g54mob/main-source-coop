using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings;
using Features.NavigationModule.Scripts;
using UnityEngine;
using UnityEngine.AI;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Sensors
{
	public class CoinRobSwarmSpawnPositionService
	{
		private const int UnitSpawnPathAttempts = 12;

		private readonly CoinRobSwarmEnemyContext _context;

		private readonly CoinRobSwarmEnemySettings _swarmSettings;

		private readonly INavigationService _navigationService;

		public CoinRobSwarmSpawnPositionService(CoinRobSwarmEnemyContext context, CoinRobSwarmEnemySettings swarmSettings, INavigationService navigationService)
		{
			_context = context;
			_swarmSettings = swarmSettings;
			_navigationService = navigationService;
		}

		public bool TryGetReachableUnitSpawnPosition(out Vector3 spawnPosition)
		{
			spawnPosition = default(Vector3);
			if (_context.SwarmKing == null || !_context.SwarmKing.TryGetNavMeshQueryFilter(out var queryFilter))
			{
				return false;
			}
			Vector3 position = _context.SwarmKing.transform.position;
			NavMeshPath navMeshPath = new NavMeshPath();
			for (int i = 0; i < 12; i++)
			{
				if (TryGetRandomUnitSpawnPosition(out var spawnPosition2) && NavMesh.CalculatePath(spawnPosition2, position, queryFilter, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
				{
					spawnPosition = spawnPosition2;
					return true;
				}
			}
			return false;
		}

		private bool TryGetRandomUnitSpawnPosition(out Vector3 spawnPosition)
		{
			return _navigationService.TryGetRandomNavmeshPosition(_context.SwarmCenter, _swarmSettings.SwarmRadius, out spawnPosition);
		}
	}
}
