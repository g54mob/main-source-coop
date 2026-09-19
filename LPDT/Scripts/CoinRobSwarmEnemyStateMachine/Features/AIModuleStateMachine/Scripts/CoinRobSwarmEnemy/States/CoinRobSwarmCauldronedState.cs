using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Units;
using Features.AIModuleStateMachine.Scripts.Data;
using Features.DamageableTrackModule.Scripts;
using Fusion;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.States
{
	public class CoinRobSwarmCauldronedState : StateBase<CoinRobSwarmStateId>
	{
		private readonly CoinRobSwarmEnemy _enemy;

		private readonly CoinRobSwarmEnemyContext _context;

		private readonly CoinRobSwarmFormationService _formation;

		private readonly CoinRobChaseSettings _chaseSettings;

		private readonly CoinRobCombatSettings _combatSettings;

		private readonly CoinRobCauldronSettings _cauldronSettings;

		private readonly EnemyHeadwearModel _enemyHeadwearModel;

		private float _dropTimer;

		private bool _isCauldronWorn;

		private CoinRobBehaviour _damageTrackedKing;

		public CoinRobSwarmCauldronedState(CoinRobSwarmEnemy enemy, CoinRobSwarmEnemyContext context, CoinRobSwarmFormationService formation, CoinRobChaseSettings chaseSettings, CoinRobCombatSettings combatSettings, CoinRobCauldronSettings cauldronSettings, EnemyHeadwearModel enemyHeadwearModel)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_formation = formation;
			_chaseSettings = chaseSettings;
			_combatSettings = combatSettings;
			_cauldronSettings = cauldronSettings;
			_enemyHeadwearModel = enemyHeadwearModel;
		}

		public override void OnEnter()
		{
			_context.DisableReactiveAggro();
			_context.ClearPendingReactivePlayerAttack();
			_context.ChasePlayer = PlayerRef.None;
			_context.ActiveStealTasks.Clear();
			_enemy.CancelCoinChase();
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				swarmUnit.ReleaseAttachedItem();
			}
			if (_context.SwarmKing != null)
			{
				_context.SwarmKing.ReleaseAttachedItem();
			}
			_context.RunAwayStartPosition = _context.SwarmCenter;
			_context.RunAwayPosition = _formation.ResolveRunAwayDestination(_context.RunAwayStartPosition);
			_formation.SetRunAwayDestination(_context.RunAwayPosition);
			ResetDropTimer();
			_isCauldronWorn = IsCauldronWorn();
			TrackKingDamage();
		}

		public override void OnLogic()
		{
			if (_context.SwarmKing == null || _context.SwarmKing.Object == null)
			{
				return;
			}
			_context.ClearPendingReactivePlayerAttack();
			uint raw = _context.SwarmKing.Object.Id.Raw;
			if (_enemyHeadwearModel.IsWearing(raw))
			{
				if (!_isCauldronWorn)
				{
					ResetDropTimer();
				}
				_dropTimer -= _enemy.GetTickDelta();
				if (_dropTimer <= 0f)
				{
					DropCauldron(raw);
				}
			}
			else
			{
				_enemyHeadwearModel.TryConsumeRemover(raw, out var _);
			}
			_isCauldronWorn = _enemyHeadwearModel.IsWearing(raw);
			if (HasReachedRunAwayDeposit())
			{
				_formation.SetSwarmIdleLocomotion();
				DropCauldron(raw);
				_enemy.DespawnSwarm();
			}
		}

		public override void OnExit()
		{
			UntrackKingDamage();
			if (_context.SwarmKing != null && _context.SwarmKing.Object != null)
			{
				DropCauldron(_context.SwarmKing.Object.Id.Raw);
			}
		}

		private void TrackKingDamage()
		{
			UntrackKingDamage();
			if (!(_context.SwarmKing == null))
			{
				_damageTrackedKing = _context.SwarmKing;
				_damageTrackedKing.OnMemberDamaged += OnKingDamaged;
			}
		}

		private void UntrackKingDamage()
		{
			if (!(_damageTrackedKing == null))
			{
				_damageTrackedKing.OnMemberDamaged -= OnKingDamaged;
				_damageTrackedKing = null;
			}
		}

		private void OnKingDamaged(DamageData damageData)
		{
			if (!(_context.SwarmKing == null) && !(_context.SwarmKing.Object == null))
			{
				uint raw = _context.SwarmKing.Object.Id.Raw;
				DropCauldron(raw);
				_isCauldronWorn = false;
				_enemyHeadwearModel.TryConsumeRemover(raw, out var _);
			}
		}

		private void ResetDropTimer()
		{
			_dropTimer = Random.Range(_cauldronSettings.MinDuration, _cauldronSettings.MaxDuration);
		}

		private bool IsCauldronWorn()
		{
			if (_context.SwarmKing != null && _context.SwarmKing.Object != null)
			{
				return _enemyHeadwearModel.IsWearing(_context.SwarmKing.Object.Id.Raw);
			}
			return false;
		}

		private void DropCauldron(uint kingObjectId)
		{
			if (_enemyHeadwearModel.IsWearing(kingObjectId))
			{
				_enemyHeadwearModel.TryGet(kingObjectId, out var headwear);
				_enemyHeadwearModel.Set(kingObjectId, isWearing: false);
				if (headwear != null)
				{
					Vector3 vector = ((_context.SwarmKing != null) ? _context.SwarmKing.transform.forward : _enemy.transform.forward);
					Vector3 impulse = Vector3.up * _cauldronSettings.UpImpulse + vector * _cauldronSettings.ForwardImpulse;
					headwear.DropFromEnemy(impulse);
				}
			}
		}

		private bool HasReachedRunAwayDeposit()
		{
			if (!HasRunAwayStartedFarEnoughFromDeposit())
			{
				return false;
			}
			if (_context.SwarmKing != null && HasMemberFinishedPathToDeposit(_context.SwarmKing))
			{
				return true;
			}
			if (Vector3.Distance(_context.SwarmCenter, _context.RunAwayPosition) <= _chaseSettings.TargetReachError)
			{
				return true;
			}
			return HaveAllGroundUnitsFinishedRunAwayPath();
		}

		private bool HasRunAwayStartedFarEnoughFromDeposit()
		{
			float num = _chaseSettings.TargetReachError + (_context.SwarmKing?.StoppingDistance ?? 0f);
			return Vector3.Distance(_context.RunAwayStartPosition, _context.RunAwayPosition) > num;
		}

		private bool HaveAllGroundUnitsFinishedRunAwayPath()
		{
			bool result = false;
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				if (!IsUnitStrandedAboveDeposit(swarmUnit))
				{
					result = true;
					if (!HasMemberFinishedPathToDeposit(swarmUnit))
					{
						return false;
					}
				}
			}
			return result;
		}

		private bool IsUnitStrandedAboveDeposit(CoinRobBehaviour member)
		{
			return member.transform.position.y - _context.RunAwayPosition.y >= _combatSettings.KingElevationSeparation;
		}

		private bool HasMemberFinishedPathToDeposit(CoinRobBehaviour member)
		{
			float num = Vector3.Distance(member.transform.position, _context.RunAwayPosition);
			float num2 = _chaseSettings.TargetReachError + member.StoppingDistance;
			if (Vector3.Distance(member.transform.position, _context.RunAwayStartPosition) < num2)
			{
				return false;
			}
			if (num <= num2)
			{
				return true;
			}
			if (member.PathPending)
			{
				return false;
			}
			if (!member.HasPath)
			{
				return false;
			}
			if (member.RemainingDistance <= num2)
			{
				return num <= num2;
			}
			return false;
		}
	}
}
