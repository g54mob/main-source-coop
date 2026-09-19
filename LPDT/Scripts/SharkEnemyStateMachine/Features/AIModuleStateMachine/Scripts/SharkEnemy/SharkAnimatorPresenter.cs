using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.SharkEnemy
{
	public class SharkAnimatorPresenter : MonoBehaviour
	{
		[SerializeField]
		private SharkEnemy _enemy;

		[SerializeField]
		private SharkEnemyContext _context;

		private SharkVisualState? _lastVisualState;

		private bool _isIdleLoopPlaying;

		private void Update()
		{
			if (_enemy.Object.IsValid)
			{
				_context.UpdateFmod3DAttributes();
				SharkVisualState visualState = _enemy.VisualState;
				if (!_lastVisualState.HasValue || visualState != _lastVisualState.Value)
				{
					_lastVisualState = visualState;
					ApplyVisualRoots(visualState);
				}
			}
		}

		private bool IsIdleLoopVisualState(SharkVisualState state)
		{
			if (state != SharkVisualState.Idle && state != SharkVisualState.Backoff)
			{
				return state == SharkVisualState.Fear;
			}
			return true;
		}

		private bool IsSharkAttackVisualPhase(SharkVisualState state)
		{
			if (state != SharkVisualState.BiteSimple)
			{
				return state == SharkVisualState.BiteLethal;
			}
			return true;
		}

		private void ApplyVisualRoots(SharkVisualState state)
		{
			bool flag = IsSharkAttackVisualPhase(state);
			bool active = !flag;
			_context.FinRoot.SetActive(active);
			if (flag && _context.SharkRoot.activeSelf)
			{
				_context.SharkRoot.SetActive(value: false);
			}
			_context.SharkRoot.SetActive(flag);
		}
	}
}
