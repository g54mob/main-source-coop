using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class RatsHoleEnemyVisualController : NetworkBehaviour
	{
		[SerializeField]
		private RatsHoleEnemyAnimatorPresenter _animatorPresenter;

		private RatsHoleEnemyContext _context;

		private RatsHoleEnemyVisualState _lastState;

		private int _lastAttackTriggerCount;

		[Inject]
		public void InjectDependencies(RatsHoleEnemyContext context)
		{
			_context = context;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (_animatorPresenter == null)
			{
				_animatorPresenter = GetComponentInChildren<RatsHoleEnemyAnimatorPresenter>(includeInactive: true);
			}
			_lastAttackTriggerCount = _context.AttackTriggerCount;
			_lastState = _context.VisualState;
		}

		public override void Render()
		{
			if (_context.AttackTriggerCount != _lastAttackTriggerCount)
			{
				_animatorPresenter?.TriggerAttack();
				_lastAttackTriggerCount = _context.AttackTriggerCount;
			}
			_animatorPresenter?.SyncLocomotionFromVelocity(_context);
			RatsHoleEnemyVisualState visualState = _context.VisualState;
			if (visualState != _lastState)
			{
				_lastState = visualState;
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
