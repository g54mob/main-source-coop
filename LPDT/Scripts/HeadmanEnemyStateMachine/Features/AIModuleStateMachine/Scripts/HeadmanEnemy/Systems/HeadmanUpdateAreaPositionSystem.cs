using System.Collections.Generic;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Data;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Systems
{
	public class HeadmanUpdateAreaPositionSystem : MonoBehaviour
	{
		[SerializeField]
		private HeadmanEnemy _headManEnemyBehaviour;

		[SerializeField]
		private HeadManTargetsModel _headManTargetsModel;

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
			if (_headManTargetsModel != null)
			{
				_headManTargetsModel.OnTargetRemoved += ProcessHeadManLostTargetModel;
			}
			if (_headManEnemyBehaviour != null)
			{
				_headManEnemyBehaviour.OnChangeAreaTriggered += ProcessChangeArea;
			}
		}

		private void OnDestroy()
		{
			if (_headManTargetsModel != null)
			{
				_headManTargetsModel.OnTargetRemoved -= ProcessHeadManLostTargetModel;
			}
			if (_headManEnemyBehaviour != null)
			{
				_headManEnemyBehaviour.OnChangeAreaTriggered -= ProcessChangeArea;
			}
		}

		private void ProcessHeadManLostTargetModel(PlayerRef lostPlayer)
		{
			if (_headManTargetsModel.HeadManTargets.Count <= 0)
			{
				PlayerDataHolder value;
				if (_navigationService.TryGetPlayerTrackingPosition(lostPlayer, out var position))
				{
					ChangeAreaByCenter(position);
				}
				else if (_spawnedPlayersModel.Players.TryGetValue(lostPlayer, out value))
				{
					ChangeAreaByCenter(value.NetworkObject.transform.position);
				}
			}
		}

		private void ProcessChangeArea(Vector3 position)
		{
			ChangeAreaByCenter(position);
		}

		private void ChangeAreaByCenter(Vector3 centerPosition)
		{
			if (_headManTargetsModel.HeadManTargets.Count > 0)
			{
				return;
			}
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
			if (_targetPositionsModel.AreaPosition.TryGetValue(EnemyType.HeadMan, out var value))
			{
				List<Vector3> list2 = new List<Vector3>(value.Positions.Values);
				list2.AddRange(list);
				if (_navigationService.TryGetRandomSafeNavmeshPosition(centerPosition, _safeAreaRange, _attempts, list2, _navMeshAgent.agentTypeID, -1, out var bestPosition, 5f, 3f, 3f, 5f))
				{
					_headManEnemyBehaviour.SetAreaPosition(bestPosition);
				}
			}
			else if (_navigationService.TryGetRandomSafeNavmeshPosition(centerPosition, _safeAreaRange, _attempts, list, _navMeshAgent.agentTypeID, -1, out bestPosition2, 5f, 3f, 3f, 5f))
			{
				_headManEnemyBehaviour.SetAreaPosition(bestPosition2);
			}
		}
	}
}
