using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Units;
using Features.ItemsModule.Scripts;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.States
{
	public class CoinRobSwarmRunAwayState : StateBase<CoinRobSwarmStateId>
	{
		private readonly CoinRobSwarmEnemy _enemy;

		private readonly CoinRobSwarmEnemyContext _context;

		private readonly CoinRobSwarmFormationService _formation;

		private readonly CoinRobChaseSettings _chaseSettings;

		private readonly CoinRobCombatSettings _combatSettings;

		private float _carrierRepathTimer;

		public CoinRobSwarmRunAwayState(CoinRobSwarmEnemy enemy, CoinRobSwarmEnemyContext context, CoinRobSwarmFormationService formation, CoinRobChaseSettings chaseSettings, CoinRobCombatSettings combatSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_formation = formation;
			_chaseSettings = chaseSettings;
			_combatSettings = combatSettings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(CoinRobSwarmVisualState.RunAway);
			_context.EnableReactiveAggro();
			_carrierRepathTimer = 0f;
			if (_context.RunAwayPosition.sqrMagnitude > 0.0001f)
			{
				_formation.SetRunAwayDestination(_context.RunAwayPosition);
			}
		}

		public override void OnExit()
		{
			_context.DisableReactiveAggro();
		}

		public override void OnLogic()
		{
			if (!_enemy.TryResolveReactivePlayerAttackRequest())
			{
				bool num = HasReachedRunAwayDeposit();
				_formation.RefreshEmptyUnitsAroundKing();
				EnsureCarriersHaveRunAwayPath(_enemy.GetTickDelta());
				if (num)
				{
					CompleteRunAwayDeposit();
				}
			}
		}

		private void CompleteRunAwayDeposit()
		{
			ReleaseCarriedItems();
			_formation.SetSwarmIdleLocomotion();
			_enemy.Trigger(CoinRobSwarmEvent.OnRunAwayComplete);
		}

		private void ReleaseCarriedItems()
		{
			Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole.RatsHole hole;
			bool flag = _formation.TryGetRunAwayDepositHole(out hole);
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				if (!(swarmUnit == null) && swarmUnit.HasAttachedItem)
				{
					IItem attachedItem = swarmUnit.AttachedItem;
					swarmUnit.ReleaseAttachedItem();
					if (attachedItem != null && !attachedItem.IsDespawned && flag)
					{
						hole.TryAbsorbItem(attachedItem);
					}
				}
			}
		}

		private void EnsureCarriersHaveRunAwayPath(float deltaTime)
		{
			_carrierRepathTimer += deltaTime;
			if (_carrierRepathTimer < 0.5f)
			{
				return;
			}
			bool flag = false;
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				if (!(swarmUnit == null) && swarmUnit.HasAttachedItem && !HasCarrierReachedDeposit(swarmUnit) && !swarmUnit.PathPending && (!swarmUnit.HasPath || !(swarmUnit.Velocity.sqrMagnitude > 0.0001f)))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				_carrierRepathTimer = 0f;
				_formation.RepathCarriersToRunAwayDestination(_context.RunAwayPosition);
			}
		}

		private bool HasReachedRunAwayDeposit()
		{
			if (HasAnyCarrier())
			{
				return HaveAllCarriersReachedDeposit();
			}
			float num = _chaseSettings.RunAwayDepositArrivalDistance + _chaseSettings.KingRunAwaySpawnOrbitRadius;
			if (Vector3.Distance(_context.SwarmCenter, _context.RunAwayPosition) <= num)
			{
				return true;
			}
			if (_context.SwarmKing != null && IsMemberSettledNear(_context.SwarmKing, _context.RunAwayPosition, num))
			{
				return true;
			}
			if (!HasRunAwayStartedFarEnoughFromDeposit())
			{
				return false;
			}
			if (_context.SwarmKing != null && HasMemberFinishedPathToDeposit(_context.SwarmKing))
			{
				return true;
			}
			return HaveAllGroundUnitsFinishedRunAwayPath();
		}

		private bool HasAnyCarrier()
		{
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				if (swarmUnit != null && swarmUnit.HasAttachedItem)
				{
					return true;
				}
			}
			return false;
		}

		private bool HaveAllCarriersReachedDeposit()
		{
			bool result = false;
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				if (!(swarmUnit == null) && swarmUnit.HasAttachedItem)
				{
					result = true;
					if (!HasCarrierReachedDeposit(swarmUnit))
					{
						return false;
					}
				}
			}
			return result;
		}

		private bool HasCarrierReachedDeposit(CoinRobBehaviour swarmUnit)
		{
			if (Vector3.Distance(swarmUnit.transform.position, _context.RunAwayPosition) <= _chaseSettings.RunAwayDepositArrivalDistance)
			{
				return true;
			}
			return HasCarrierArrivedAtPathDestination(swarmUnit);
		}

		private bool HasCarrierArrivedAtPathDestination(CoinRobBehaviour swarmUnit)
		{
			if (swarmUnit.PathPending)
			{
				return false;
			}
			float num = _chaseSettings.TargetReachError + swarmUnit.StoppingDistance;
			if (swarmUnit.HasPath && swarmUnit.RemainingDistance <= num)
			{
				return true;
			}
			if (swarmUnit.HasPath)
			{
				return false;
			}
			if (swarmUnit.Velocity.sqrMagnitude > 0.0001f)
			{
				return false;
			}
			float num2 = _chaseSettings.RunAwayDepositArrivalDistance + _chaseSettings.KingRunAwaySpawnOrbitRadius;
			return Vector3.Distance(swarmUnit.transform.position, _context.RunAwayPosition) <= num2;
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

		private bool IsMemberSettledNear(CoinRobBehaviour member, Vector3 target, float distance)
		{
			if (member == null)
			{
				return false;
			}
			if (Vector3.Distance(member.transform.position, target) <= distance)
			{
				return true;
			}
			if (member.PathPending)
			{
				return false;
			}
			float num = _chaseSettings.TargetReachError + member.StoppingDistance;
			if (member.HasPath && member.RemainingDistance <= num)
			{
				return member.Velocity.sqrMagnitude <= 0.01f;
			}
			return false;
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
