using System.Collections.Generic;
using Features.AIModule.Scripts;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Pirato.Systems
{
	public class PirateUpdateAreaPositionSystem : MonoBehaviour
	{
		private const float MinDistanceFromCenter = 5f;

		private const float CenterDistanceWeight = 3f;

		private const float AverageAvoidDistanceWeight = 3f;

		private const float TooClosePenaltyRadius = 5f;

		private const float TooClosePenaltyWeight = 5f;

		[SerializeField]
		private PirateEnemy _pirateEnemy;

		[SerializeField]
		private NavMeshAgent _navMeshAgent;

		[SerializeField]
		private PirateConfiguration _configuration;

		private readonly List<Vector3> _avoidPositions = new List<Vector3>();

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
			if (_pirateEnemy != null)
			{
				_pirateEnemy.OnChangeAreaTriggered += ProcessChangeArea;
			}
		}

		private void OnDestroy()
		{
			if (_pirateEnemy != null)
			{
				_pirateEnemy.OnChangeAreaTriggered -= ProcessChangeArea;
			}
		}

		private void ProcessChangeArea(Vector3 position)
		{
			ChangeAreaByCenter(position);
		}

		private void ChangeAreaByCenter(Vector3 centerPosition)
		{
			if (!(_configuration == null) && !(_navMeshAgent == null) && _navigationService != null)
			{
				CollectAvoidPositions();
				if (_navigationService.TryGetRandomSafeNavmeshPosition(centerPosition, _configuration.IdleSafeAreaRange, _configuration.IdleAreaSearchAttempts, _avoidPositions, _navMeshAgent.agentTypeID, -1, out var bestPosition, 5f, 3f, 3f, 5f, 5f))
				{
					_pirateEnemy.SetAreaPosition(bestPosition);
				}
			}
		}

		private void CollectAvoidPositions()
		{
			_avoidPositions.Clear();
			AddPlayerPositions();
			AddPirateAreaPositions(EnemyType.PiratePistol);
			AddPirateAreaPositions(EnemyType.PirateBomb);
		}

		private void AddPlayerPositions()
		{
			if (_spawnedPlayersModel == null)
			{
				return;
			}
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayersModel.Players)
			{
				if (_navigationService.TryGetPlayerTrackingPosition(player.Key, out var position))
				{
					_avoidPositions.Add(position);
				}
				else if (player.Value != null && player.Value.NetworkObject != null)
				{
					_avoidPositions.Add(player.Value.NetworkObject.transform.position);
				}
			}
		}

		private void AddPirateAreaPositions(EnemyType enemyType)
		{
			if (_targetPositionsModel != null && _targetPositionsModel.AreaPosition.TryGetValue(enemyType, out var value))
			{
				_avoidPositions.AddRange(value.Positions.Values);
			}
		}
	}
}
