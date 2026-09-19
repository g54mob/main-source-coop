using System.Collections.Generic;
using Features.AIModule.Scripts;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.SharkEnemy.Systems
{
	public class SharkUpdateAreaPositionSystem : MonoBehaviour
	{
		[SerializeField]
		private SharkEnemy _sharkEnemy;

		[SerializeField]
		private NavMeshAgent _navMeshAgent;

		[SerializeField]
		private float _safeAreaRange = 60f;

		[SerializeField]
		private int _attempts = 15;

		private EnemyTargetPositionsModel _targetPositionsModel;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private INavigationService _navigationService;

		[Inject]
		private void InjectDependencies(SpawnedPlayersModel spawnedPlayersModel, INavigationService navigationService, EnemyTargetPositionsModel targetPositionsModel)
		{
			_spawnedPlayersModel = spawnedPlayersModel;
			_navigationService = navigationService;
			_targetPositionsModel = targetPositionsModel;
		}

		private void Awake()
		{
			if (_sharkEnemy != null)
			{
				_sharkEnemy.OnChangeAreaTriggered += ProcessChangeArea;
			}
		}

		private void OnDestroy()
		{
			if (_sharkEnemy != null)
			{
				_sharkEnemy.OnChangeAreaTriggered -= ProcessChangeArea;
			}
		}

		private void ProcessChangeArea(Vector3 position)
		{
			ChangeAreaByCenter(position);
		}

		private void ChangeAreaByCenter(Vector3 centerPosition)
		{
			List<Vector3> list = new List<Vector3>(_spawnedPlayersModel.Players.Count);
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayersModel.Players)
			{
				if (_navigationService.TryGetPlayerTrackingPosition(player.Key, out var position))
				{
					list.Add(position);
				}
				else
				{
					list.Add(player.Value.NetworkObject.transform.position);
				}
			}
			Vector3 bestPosition2;
			if (_targetPositionsModel.AreaPosition.TryGetValue(EnemyType.Shark, out var value))
			{
				List<Vector3> list2 = new List<Vector3>(value.Positions.Values);
				list2.AddRange(list);
				if (_navigationService.TryGetRandomSafeNavmeshPosition(centerPosition, _safeAreaRange, _attempts, list2, _navMeshAgent.agentTypeID, -1, out var bestPosition, 5f, 3f, 3f, 5f))
				{
					_sharkEnemy.SetAreaPosition(bestPosition);
				}
			}
			else if (_navigationService.TryGetRandomSafeNavmeshPosition(centerPosition, _safeAreaRange, _attempts, list, _navMeshAgent.agentTypeID, -1, out bestPosition2, 5f, 3f, 3f, 5f))
			{
				_sharkEnemy.SetAreaPosition(bestPosition2);
			}
		}
	}
}
