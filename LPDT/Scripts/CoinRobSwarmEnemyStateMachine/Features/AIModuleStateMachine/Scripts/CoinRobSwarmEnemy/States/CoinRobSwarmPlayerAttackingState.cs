using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Units;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.States
{
	public class CoinRobSwarmPlayerAttackingState : StateBase<CoinRobSwarmStateId>
	{
		private const float KingDestinationRefreshSqr = 0.25f;

		private readonly CoinRobSwarmEnemy _enemy;

		private readonly CoinRobSwarmEnemyContext _context;

		private readonly CoinRobSwarmFormationService _formation;

		private readonly CoinRobCombatSettings _combatSettings;

		private readonly CoinRobChaseSettings _chaseSettings;

		private readonly CoinRobSwarmEnemySettings _swarmSettings;

		private readonly INavigationService _navigationService;

		private readonly SpawnedPlayersModel _spawnedPlayersModel;

		private readonly IPlayerStateService _playerStateService;

		private readonly CoinRobSwarmAnimatorPresenter _presenter;

		private readonly CoinRobPlayerAttackValidator _attackValidator;

		private float _attackPerformingTimer;

		private Vector3 _lastKingChaseDestination;

		private bool _hasKingChaseDestination;

		private bool _unitsHeldForKingElevation;

		public CoinRobSwarmPlayerAttackingState(CoinRobSwarmEnemy enemy, CoinRobSwarmEnemyContext context, CoinRobSwarmFormationService formation, CoinRobCombatSettings combatSettings, CoinRobChaseSettings chaseSettings, CoinRobSwarmEnemySettings swarmSettings, INavigationService navigationService, SpawnedPlayersModel spawnedPlayersModel, IPlayerStateService playerStateService, CoinRobSwarmAnimatorPresenter presenter, CoinRobPlayerAttackValidator attackValidator)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_formation = formation;
			_combatSettings = combatSettings;
			_chaseSettings = chaseSettings;
			_swarmSettings = swarmSettings;
			_navigationService = navigationService;
			_spawnedPlayersModel = spawnedPlayersModel;
			_playerStateService = playerStateService;
			_presenter = presenter;
			_attackValidator = attackValidator;
		}

		public override void OnEnter()
		{
			_context.AttackTimer = 0f;
			_attackPerformingTimer = 0f;
			_context.IsAttackPerforming = false;
			_hasKingChaseDestination = false;
			_unitsHeldForKingElevation = false;
			_context.EnableReactiveAggro();
			_enemy.SetVisualState(CoinRobSwarmVisualState.PlayerAttacking);
			if (_spawnedPlayersModel.Players.TryGetValue(_context.ChasePlayer, out var value) && value.NetworkObject != null)
			{
				_formation.BeginPlayerAttackFormation(value.NetworkObject.transform.position);
			}
		}

		public override void OnLogic()
		{
			float tickDelta = _enemy.GetTickDelta();
			if (!TryResolveChasePlayer(out var playerDataHolder))
			{
				_enemy.BeginRunAway();
				_enemy.Trigger(CoinRobSwarmEvent.OnRunAway);
				return;
			}
			Vector3 position = playerDataHolder.NetworkObject.transform.position;
			bool isProvokedByMemberDamage = _context.IsProvokedByMemberDamage;
			if (_context.IsProvokedByMemberDamage)
			{
				_context.IsProvokedByMemberDamage = false;
			}
			float num = Vector3.Distance(position, _context.SwarmCenter);
			if (!isProvokedByMemberDamage && (!_navigationService.IsPointOnNavMeshProjected(position, out var _) || num >= _combatSettings.DiffuseRadius))
			{
				_enemy.BeginRunAway();
				_enemy.Trigger(CoinRobSwarmEvent.OnRunAway);
				return;
			}
			CoinRobBehaviour swarmKing = _context.SwarmKing;
			bool flag = _attackValidator.IsKingAbovePlayer(swarmKing, position);
			if (_attackValidator.IsPlayerProtectedBySafeZone(_context.ChasePlayer))
			{
				CancelPlayerAttackForSafeZone();
				return;
			}
			if (!_attackValidator.CanKingAttackPlayer(_context.ChasePlayer, swarmKing))
			{
				AbortPlayerAttack();
				return;
			}
			bool flag2 = _context.IsKingElevatedAboveSwarm(_combatSettings.KingElevationSeparation);
			bool flag3 = IsKingInPlayerAttackRange(position, swarmKing);
			bool flag4 = flag3 || _context.IsAttackPerforming;
			if (swarmKing != null && !flag4 && !flag && !flag2)
			{
				UpdateKingDestination(position, swarmKing);
			}
			else if (swarmKing != null && (flag4 || flag || flag2))
			{
				swarmKing.StopMovement();
			}
			if (flag2)
			{
				if (!_unitsHeldForKingElevation)
				{
					HoldUnitsIdleUntilKingDescends();
					_unitsHeldForKingElevation = true;
				}
			}
			else if (_unitsHeldForKingElevation)
			{
				ReleaseUnitsAfterElevationHold(position);
				_unitsHeldForKingElevation = false;
			}
			else
			{
				UpdateUnitsAroundPlayer(position);
			}
			if (_context.IsAttackPerforming)
			{
				if (flag || flag2)
				{
					CompleteAttackPerforming();
					AbortPlayerAttack();
					return;
				}
				if (swarmKing != null)
				{
					swarmKing.StopMovement();
				}
				_attackPerformingTimer += tickDelta;
				if (_attackPerformingTimer >= _combatSettings.KingAttackAnimationDuration)
				{
					CompleteAttackPerforming();
				}
			}
			else if (flag3)
			{
				_context.AttackTimer += tickDelta;
				if (_context.AttackTimer >= _combatSettings.AttackFrequency)
				{
					_presenter.SetKingAttack();
					_context.IsAttackPerforming = true;
					_attackPerformingTimer = 0f;
					_hasKingChaseDestination = false;
					if (swarmKing != null)
					{
						swarmKing.StopMovement();
						swarmKing.OnAttackPreformed += _enemy.DealDamageToChasePlayer;
					}
					_context.AttackTimer = 0f;
				}
			}
			else
			{
				_context.AttackTimer = 0f;
				_hasKingChaseDestination = false;
			}
		}

		public override void OnExit()
		{
			CompleteAttackPerforming();
			_hasKingChaseDestination = false;
			_unitsHeldForKingElevation = false;
			_context.IsProvokedByMemberDamage = false;
			_context.DisableReactiveAggro();
		}

		private void UpdateKingDestination(Vector3 chasePlayerPosition, CoinRobBehaviour king)
		{
			if (!_hasKingChaseDestination || (chasePlayerPosition - _lastKingChaseDestination).sqrMagnitude > 0.25f || !king.HasPath || king.PathStatus != NavMeshPathStatus.PathComplete)
			{
				Vector3 destination = _navigationService.ValidatePointOnNavmesh(chasePlayerPosition, _swarmSettings.SwarmRadius);
				king.SetDestination(destination);
				_lastKingChaseDestination = chasePlayerPosition;
				_hasKingChaseDestination = true;
			}
		}

		private void HoldUnitsIdleUntilKingDescends()
		{
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				swarmUnit.StopMovement();
				_presenter.SetMemberLocomotion(swarmUnit, isRunning: false);
			}
		}

		private void ReleaseUnitsAfterElevationHold(Vector3 chasePlayerPosition)
		{
			if (_context.SwarmKing != null)
			{
				_formation.RefreshUnitsFormationAroundKing();
			}
			else
			{
				_formation.SetUnitsFormationAroundPlayer(chasePlayerPosition);
			}
		}

		private void UpdateUnitsAroundPlayer(Vector3 chasePlayerPosition)
		{
			CoinRobBehaviour swarmKing = _context.SwarmKing;
			if (swarmKing == null)
			{
				_formation.SetUnitsFormationAroundPlayer(chasePlayerPosition);
				return;
			}
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				if (swarmUnit.RemainingDistance <= _chaseSettings.TargetReachError)
				{
					swarmUnit.SetDestination(_formation.GetRandomNavmeshPosition(swarmKing.transform.position, _swarmSettings.SwarmRadius));
				}
				_presenter.SetMemberLocomotion(swarmUnit, _formation.ShouldMemberRun(swarmUnit));
			}
		}

		private bool IsKingInPlayerAttackRange(Vector3 playerPosition, CoinRobBehaviour king)
		{
			if (king == null)
			{
				return false;
			}
			if (_attackValidator.IsKingAbovePlayer(king, playerPosition))
			{
				return false;
			}
			Vector3 vector = playerPosition - king.transform.position;
			if (new Vector2(vector.x, vector.z).magnitude > _combatSettings.PlayerAttackRange)
			{
				return false;
			}
			if (vector.y < 0f)
			{
				return Mathf.Abs(vector.y) <= _combatSettings.MaxAttackVerticalDelta;
			}
			return vector.y <= _combatSettings.MaxAttackVerticalDelta;
		}

		private bool TryResolveChasePlayer(out PlayerDataHolder playerDataHolder)
		{
			playerDataHolder = null;
			PlayerRef chasePlayer = _context.ChasePlayer;
			if (chasePlayer == PlayerRef.None)
			{
				return false;
			}
			if (!_playerStateService.IsPlayerAlive(chasePlayer.PlayerId))
			{
				return false;
			}
			if (!_spawnedPlayersModel.Players.TryGetValue(chasePlayer, out playerDataHolder))
			{
				return false;
			}
			if (playerDataHolder.NetworkObject != null)
			{
				return playerDataHolder.NetworkObject.IsValid;
			}
			return false;
		}

		private void AbortPlayerAttack()
		{
			CompleteAttackPerforming();
			_enemy.BeginRunAway();
			_enemy.Trigger(CoinRobSwarmEvent.OnRunAway);
		}

		private void CancelPlayerAttackForSafeZone()
		{
			CompleteAttackPerforming();
			if (_context.HasMembersCarryingLoot())
			{
				_enemy.BeginRunAway();
				_enemy.Trigger(CoinRobSwarmEvent.OnRunAway);
			}
			else
			{
				_enemy.BeginWandering();
				_enemy.Trigger(CoinRobSwarmEvent.OnPlayerAttackCancelled);
			}
		}

		private void CompleteAttackPerforming()
		{
			if (_context.SwarmKing != null)
			{
				_context.SwarmKing.OnAttackPreformed -= _enemy.DealDamageToChasePlayer;
			}
			_context.IsAttackPerforming = false;
			_attackPerformingTimer = 0f;
		}
	}
}
