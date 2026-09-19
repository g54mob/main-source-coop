using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy
{
	public class ParrotMobAnimatorPresenter : MonoBehaviour
	{
		private const string ALERT_TRIGGER = "Alert";

		private const string SCREAM_STATE = "Scream";

		private const string IDLE_TRIGGER = "Idle";

		[SerializeField]
		private ParrotMobEnemy _enemy;

		[SerializeField]
		private Animator _animator;

		private readonly int _alertTriggerHash = Animator.StringToHash("Alert");

		private readonly int _screamStateHash = Animator.StringToHash("Scream");

		private readonly int _idleTriggerHash = Animator.StringToHash("Idle");

		private ParrotMobVisualState _lastAppliedVisualState = (ParrotMobVisualState)(-1);

		private void Update()
		{
			if (!(_enemy.Object == null) && _enemy.Object.IsValid)
			{
				ApplyVisualState(_enemy.VisualState);
			}
		}

		private void ApplyVisualState(ParrotMobVisualState visualState)
		{
			if (visualState == ParrotMobVisualState.Scream)
			{
				if (_lastAppliedVisualState != ParrotMobVisualState.Scream)
				{
					_lastAppliedVisualState = ParrotMobVisualState.Scream;
					_animator.Play(_screamStateHash, 0, 0f);
				}
			}
			else if (_lastAppliedVisualState != visualState)
			{
				_lastAppliedVisualState = visualState;
				if (visualState == ParrotMobVisualState.Alert)
				{
					_animator.SetTrigger(_alertTriggerHash);
				}
				else
				{
					_animator.SetTrigger(_idleTriggerHash);
				}
			}
		}
	}
}
