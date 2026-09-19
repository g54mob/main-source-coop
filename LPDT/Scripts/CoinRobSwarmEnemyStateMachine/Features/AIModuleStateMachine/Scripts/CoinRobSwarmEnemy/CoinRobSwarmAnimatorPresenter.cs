using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Units;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy
{
	public class CoinRobSwarmAnimatorPresenter : MonoBehaviour
	{
		private enum LocomotionMode
		{
			Idle = 0,
			Run = 1,
			RunWithCoin = 2,
			Stealing = 3
		}

		[Header("King animator mapping (BigRatController)")]
		[SerializeField]
		private string _kingAttackTriggerName = "Attacking";

		[Header("Unit animator mapping (MiniRatController)")]
		[SerializeField]
		private string _unitRunStateName = "Rat_Run";

		[SerializeField]
		private string _unitRunWithCoinStateName = "Run_WithCoin";

		[SerializeField]
		private string _unitIdleStateName = "Idle";

		[SerializeField]
		private string _unitStealStateName = "Rat_Steal coin";

		[SerializeField]
		private CoinRobSwarmEnemyContext _context;

		private readonly Dictionary<CoinRobBehaviour, LocomotionMode> _memberLocomotion = new Dictionary<CoinRobBehaviour, LocomotionMode>();

		public void ClearAllLocomotionCache()
		{
			_memberLocomotion.Clear();
		}

		public void ClearUnitLocomotionCache()
		{
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				_memberLocomotion.Remove(swarmUnit);
			}
		}

		public void ApplyFsmVisualState(CoinRobSwarmVisualState visualState)
		{
			switch (visualState)
			{
			case CoinRobSwarmVisualState.Chasing:
			case CoinRobSwarmVisualState.RunAway:
			case CoinRobSwarmVisualState.Fear:
			case CoinRobSwarmVisualState.Cauldroned:
				ClearUnitLocomotionCache();
				SetUnitsRunning();
				break;
			case CoinRobSwarmVisualState.Attacking:
			case CoinRobSwarmVisualState.Stealing:
				_context.SwarmKing?.StopMovement();
				ClearUnitLocomotionCache();
				SetUnitsRunning();
				break;
			case CoinRobSwarmVisualState.PlayerAttacking:
				ClearUnitLocomotionCache();
				break;
			case CoinRobSwarmVisualState.Wandering:
				ClearUnitLocomotionCache();
				break;
			}
		}

		public void SetUnitsRunning()
		{
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				SetMemberLocomotion(swarmUnit, isRunning: true);
			}
		}

		public void SyncUnitsCarryLocomotion()
		{
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				if (!IsKing(swarmUnit) && !_context.ActiveStealTasks.ContainsKey(swarmUnit) && _memberLocomotion.TryGetValue(swarmUnit, out var value))
				{
					switch (value)
					{
					case LocomotionMode.Run:
					case LocomotionMode.RunWithCoin:
						ApplyMemberLocomotion(swarmUnit, isRunning: true);
						break;
					}
				}
			}
		}

		public void SetUnitsIdle()
		{
			foreach (CoinRobBehaviour swarmUnit in _context.SwarmUnits)
			{
				SetMemberLocomotion(swarmUnit, isRunning: false);
			}
		}

		public void SetMemberLocomotion(CoinRobBehaviour member, bool isRunning)
		{
			if (!(member == null) && !IsKing(member) && (!_memberLocomotion.TryGetValue(member, out var value) || value != LocomotionMode.Stealing))
			{
				ApplyMemberLocomotion(member, isRunning);
			}
		}

		public void SetKingAttack()
		{
			if (!(_context.SwarmKing == null))
			{
				_context.SwarmKing.NetworkedAnimator.SetTrigger(Animator.StringToHash(_kingAttackTriggerName));
			}
		}

		public void SetUnitSteal(CoinRobBehaviour unit)
		{
			if (!(unit == null))
			{
				_memberLocomotion[unit] = LocomotionMode.Stealing;
				unit.NetworkedAnimator.Play(Animator.StringToHash(_unitStealStateName), unit.AnimationsLayer, 0f);
			}
		}

		public void EndUnitSteal(CoinRobBehaviour unit)
		{
			if (!(unit == null) && _memberLocomotion.TryGetValue(unit, out var value) && value == LocomotionMode.Stealing)
			{
				_memberLocomotion.Remove(unit);
			}
		}

		public void ClearMemberLocomotionState(CoinRobBehaviour member)
		{
			if (member != null)
			{
				_memberLocomotion.Remove(member);
			}
		}

		private void ApplyMemberLocomotion(CoinRobBehaviour member, bool isRunning)
		{
			LocomotionMode locomotionMode = ResolveDesiredLocomotion(member, isRunning);
			if (!_memberLocomotion.TryGetValue(member, out var value) || value != locomotionMode)
			{
				_memberLocomotion[member] = locomotionMode;
				switch (locomotionMode)
				{
				case LocomotionMode.RunWithCoin:
					PlayLocomotionState(member, Animator.StringToHash(_unitRunWithCoinStateName));
					break;
				case LocomotionMode.Run:
					PlayLocomotionState(member, Animator.StringToHash(_unitRunStateName));
					break;
				default:
					PlayLocomotionState(member, Animator.StringToHash(_unitIdleStateName));
					break;
				}
			}
		}

		private void PlayLocomotionState(CoinRobBehaviour member, int stateHash)
		{
			member.NetworkedAnimator.Play(stateHash, member.AnimationsLayer, 0f);
		}

		private LocomotionMode ResolveDesiredLocomotion(CoinRobBehaviour member, bool isRunning)
		{
			if (!isRunning)
			{
				return LocomotionMode.Idle;
			}
			if (member.IsCarryingLoot)
			{
				return LocomotionMode.RunWithCoin;
			}
			return LocomotionMode.Run;
		}

		private bool IsKing(CoinRobBehaviour member)
		{
			if (member != null && _context != null)
			{
				return (object)member == _context.SwarmKing;
			}
			return false;
		}
	}
}
