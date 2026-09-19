using Features.SnakeModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class SnakeVisualController : NetworkBehaviour
	{
		[SerializeField]
		private SnakeVisualSlither _slither;

		private SnakeEnemyContext _context;

		private MaterialPropertyBlock _propertyBlock;

		private SnakeVisualState _lastState;

		private SnakeVisualState _lastAttackVisualState;

		[Inject]
		public void InjectDependencies(SnakeEnemyContext context)
		{
			_context = context;
		}

		public override void Spawned()
		{
			ApplySlither(_context.VisualState);
		}

		private void Update()
		{
			if (!(_context == null))
			{
				SnakeVisualState visualState = _context.VisualState;
				ApplySlither(visualState);
				TryPlayWrapAttack(visualState);
			}
		}

		public override void Render()
		{
			SnakeVisualState visualState = _context.VisualState;
			if (visualState != _lastState)
			{
				ApplySlither(visualState);
				_lastState = visualState;
			}
		}

		private void TryPlayWrapAttack(SnakeVisualState state)
		{
			if (state != _lastAttackVisualState)
			{
				_lastAttackVisualState = state;
				if (state == SnakeVisualState.Wrap)
				{
					_context.TriggerAttackAnimationLocal();
				}
			}
		}

		private void ApplySlither(SnakeVisualState state)
		{
			if (!(_slither == null))
			{
				bool slitherActive = (!(_context.WrapOrbitSystem != null) || !_context.WrapOrbitSystem.IsVisualBusy) && (state == SnakeVisualState.Idle || state == SnakeVisualState.StepAggro || state == SnakeVisualState.SafeZoneApproach);
				_slither.SetSlitherActive(slitherActive);
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
