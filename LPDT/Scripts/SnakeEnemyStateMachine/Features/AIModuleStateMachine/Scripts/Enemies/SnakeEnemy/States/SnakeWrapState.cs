using Features.PlayerSpawner.Scripts;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.States
{
	public class SnakeWrapState : StateBase<SnakeStateId>
	{
		private readonly SnakeEnemy _enemy;

		private readonly SnakeEnemyContext _context;

		private float _wrapElapsed;

		private bool _agentWasEnabled;

		public SnakeWrapState(SnakeEnemy enemy, SnakeEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(SnakeStateId.Wrap);
			_context.SetVisualState(SnakeVisualState.Wrap);
			_wrapElapsed = 0f;
			PlayerDataHolder priorityPlayer = _context.PriorityPlayer;
			if (priorityPlayer == null || priorityPlayer.NetworkObject == null || !_context.CanTargetPlayerForWrap(priorityPlayer.NetworkObject.InputAuthority.PlayerId))
			{
				FinishWrap();
				return;
			}
			if (Mathf.Abs(_enemy.transform.position.y - priorityPlayer.NetworkObject.transform.position.y) > _context.WrapMaxVerticalReach)
			{
				FinishWrap();
				return;
			}
			DisableNavMeshAgentForOrbit();
			TryBeginOrbitOnPriorityPlayer();
			if (_context.StepAggroWrapCoilDuration <= 0f)
			{
				_context.SetWrapStrangling(isStrangling: true);
			}
			_context.AreaTypeTrackSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
			_context.SnakeDamageReactionSystem.Enable();
		}

		public override void OnExit()
		{
			_context.AreaTypeTrackSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
			_context.SnakeDamageReactionSystem.Disable();
			_context.WrapOrbitSystem.End();
			RestoreNavMeshAgentAfterOrbit();
			_context.ApplyBaseMoveSpeed();
		}

		public override void OnLogic()
		{
			PlayerDataHolder priorityPlayer = _context.PriorityPlayer;
			if (priorityPlayer == null || priorityPlayer.NetworkObject == null)
			{
				FinishWrap();
				return;
			}
			if (!_context.CanTargetPlayerForWrap(priorityPlayer.NetworkObject.InputAuthority.PlayerId))
			{
				FinishWrap();
				return;
			}
			float num = ((_enemy.Runner != null) ? _enemy.Runner.DeltaTime : Time.deltaTime);
			_wrapElapsed += num;
			if (!_context.IsWrapStrangling && _context.StepAggroWrapCoilDuration > 0f && _wrapElapsed >= _context.StepAggroWrapCoilDuration)
			{
				_context.SetWrapStrangling(isStrangling: true);
			}
		}

		private void TryBeginOrbitOnPriorityPlayer()
		{
			PlayerDataHolder priorityPlayer = _context.PriorityPlayer;
			int targetPlayerId = 0;
			float orbitAngleRadians = 0f;
			if (priorityPlayer != null && priorityPlayer.NetworkObject != null)
			{
				targetPlayerId = priorityPlayer.NetworkObject.InputAuthority.PlayerId;
				orbitAngleRadians = ResolveInitialOrbitAngle(priorityPlayer);
			}
			float y = _enemy.transform.position.y;
			_context.WrapOrbitSystem.Begin(orbitAngleRadians, y, targetPlayerId);
		}

		private float ResolveInitialOrbitAngle(PlayerDataHolder priorityPlayer)
		{
			Vector3 vector = _enemy.transform.position - priorityPlayer.NetworkObject.transform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.0001f)
			{
				return 0f;
			}
			return Mathf.Atan2(vector.z, vector.x);
		}

		private void DisableNavMeshAgentForOrbit()
		{
			NavMeshAgent navMeshAgent = _context.NavMeshAgent;
			if (navMeshAgent == null)
			{
				_agentWasEnabled = false;
				return;
			}
			_agentWasEnabled = navMeshAgent.enabled;
			if (navMeshAgent.enabled)
			{
				navMeshAgent.ResetPath();
				navMeshAgent.velocity = Vector3.zero;
				navMeshAgent.isStopped = true;
				navMeshAgent.updatePosition = false;
				navMeshAgent.updateRotation = false;
				navMeshAgent.enabled = false;
			}
		}

		private void RestoreNavMeshAgentAfterOrbit()
		{
			NavMeshAgent navMeshAgent = _context.NavMeshAgent;
			if (!(navMeshAgent == null) && _agentWasEnabled)
			{
				navMeshAgent.enabled = true;
				navMeshAgent.updatePosition = true;
				navMeshAgent.updateRotation = true;
				navMeshAgent.isStopped = false;
				navMeshAgent.stoppingDistance = 0f;
				if (navMeshAgent.isOnNavMesh)
				{
					navMeshAgent.Warp(_enemy.transform.position);
				}
			}
		}

		private void FinishWrap()
		{
			_context.SetPriorityPlayer(null);
			_enemy.TriggerEvent(SnakeEvent.OnWrapFinished);
		}
	}
}
