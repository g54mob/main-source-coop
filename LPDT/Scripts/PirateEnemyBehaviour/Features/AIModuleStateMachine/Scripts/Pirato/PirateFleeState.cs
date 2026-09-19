using System.Collections.Generic;
using Features.Movement.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Pirato
{
	public class PirateFleeState : StateBase<PirateStateId>
	{
		private readonly PirateEnemy _pirateEnemy;

		private readonly PirateEnemyContext _pirateEnemyContext;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly List<Vector3> _alivePlayerPositions = new List<Vector3>();

		private readonly NavMeshPath _navMeshPath = new NavMeshPath();

		private bool _hasDestination;

		public PirateFleeState(PirateEnemy pirateEnemy, PirateEnemyContext pirateEnemyContext, PlayerMovableModel playerMovableModel)
			: base(false, false)
		{
			_pirateEnemy = pirateEnemy;
			_pirateEnemyContext = pirateEnemyContext;
			_playerMovableModel = playerMovableModel;
		}

		public override void OnEnter()
		{
			_pirateEnemyContext.PirateAudioController.PlayGoAwaySound();
			_hasDestination = false;
			_alivePlayerPositions.Clear();
			foreach (KeyValuePair<PlayerRef, PlayerCharacterMovableBase> allCharacterMovable in _playerMovableModel.AllCharacterMovables)
			{
				if (_pirateEnemyContext.IsPlayerAlive(allCharacterMovable.Key.PlayerId))
				{
					_alivePlayerPositions.Add(allCharacterMovable.Value.transform.position);
				}
			}
			if (_alivePlayerPositions.Count == 0 || !TryFindFleePosition(out var fleePosition))
			{
				_pirateEnemy.TriggerEvent(PirateEvent.OnIdle);
				return;
			}
			_hasDestination = true;
			_pirateEnemyContext.EnemyMovableBase.MoveToPoint(fleePosition);
		}

		public override void OnLogic()
		{
			if (_hasDestination && _pirateEnemyContext.EnemyMovableBase.HasReachedEnd())
			{
				_pirateEnemyContext.EnemyMovableBase.ResetPath();
				_pirateEnemy.TriggerEvent(PirateEvent.OnIdle);
			}
		}

		private bool TryFindFleePosition(out Vector3 fleePosition)
		{
			PirateConfiguration pirateConfiguration = _pirateEnemyContext.PirateConfiguration;
			Vector3 position = _pirateEnemy.transform.position;
			float num = pirateConfiguration.FleeMinDistanceFromCurrentPosition * pirateConfiguration.FleeMinDistanceFromCurrentPosition;
			float num2 = float.NegativeInfinity;
			Vector3 vector = Vector3.zero;
			bool flag = false;
			for (int i = 0; i < pirateConfiguration.FleeSearchAttempts; i++)
			{
				Vector2 vector2 = Random.insideUnitCircle * pirateConfiguration.FleeSearchRadius;
				if (!NavMesh.SamplePosition(position + new Vector3(vector2.x, 0f, vector2.y), out var hit, pirateConfiguration.FleeSearchRadius, -1))
				{
					continue;
				}
				Vector3 vector3 = hit.position - position;
				vector3.y = 0f;
				if (!(vector3.sqrMagnitude < num) && NavMesh.CalculatePath(position, hit.position, -1, _navMeshPath) && _navMeshPath.status == NavMeshPathStatus.PathComplete)
				{
					float distanceToClosestAlivePlayerSqr = GetDistanceToClosestAlivePlayerSqr(hit.position);
					if (!flag || !(distanceToClosestAlivePlayerSqr <= num2))
					{
						num2 = distanceToClosestAlivePlayerSqr;
						vector = hit.position;
						flag = true;
					}
				}
			}
			fleePosition = vector;
			return flag;
		}

		private float GetDistanceToClosestAlivePlayerSqr(Vector3 position)
		{
			float num = float.PositiveInfinity;
			Vector3 vector = position;
			vector.y = 0f;
			for (int i = 0; i < _alivePlayerPositions.Count; i++)
			{
				Vector3 vector2 = _alivePlayerPositions[i];
				vector2.y = 0f;
				float sqrMagnitude = (vector - vector2).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
				}
			}
			return num;
		}
	}
}
