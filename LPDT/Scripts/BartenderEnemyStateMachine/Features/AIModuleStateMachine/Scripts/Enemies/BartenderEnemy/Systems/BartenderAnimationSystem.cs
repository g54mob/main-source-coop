using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.AIModuleStateMachine.Scripts.Enemies.BartenderEnemy.Settings;
using Features.AnimationModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.BartenderEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class BartenderAnimationSystem : MonoSystem
	{
		[SerializeField]
		private NetworkedAnimationControllerBase _animatorController;

		private BartenderEnemyContext _context;

		private BartenderAppearanceSettings _appearanceSettings;

		private NetworkedAnimationController _networkedAnimator;

		private bool _enabled;

		private int _appliedDanceIndex = -2;

		private bool _appliedIsDancing;

		public override bool IsEnabled => _enabled;

		[Inject]
		private void InjectDependencies(BartenderEnemyContext context, BartenderAppearanceSettings appearanceSettings)
		{
			_context = context;
			_appearanceSettings = appearanceSettings;
			_networkedAnimator = _animatorController as NetworkedAnimationController;
		}

		public override void Enable()
		{
			_enabled = true;
		}

		public override void Disable()
		{
			_enabled = false;
			Clear();
		}

		public override void Clear()
		{
			_appliedDanceIndex = -2;
			_appliedIsDancing = false;
		}

		private void Update()
		{
			if (base.Initialized && !(_animatorController == null) && !(_context == null))
			{
				_animatorController.SetFloat("Velosity", _context.SmoothedVelocity);
				_animatorController.SetBool("IsCrouch", _context.IsCrouching);
				_animatorController.SetBool("IsAggressive", _context.IsAggressive);
				_animatorController.SetBool("IsDancing", _context.IsDancing);
				_animatorController.SetBool("IsReacting", _context.IsReacting);
				ApplyDanceTriggerIfNeeded();
			}
		}

		private void ApplyDanceTriggerIfNeeded()
		{
			bool isDancing = _context.IsDancing;
			int activeDanceIndex = _context.ActiveDanceIndex;
			if (!isDancing)
			{
				_appliedIsDancing = false;
				_appliedDanceIndex = -1;
			}
			else
			{
				if (_appliedIsDancing && _appliedDanceIndex == activeDanceIndex)
				{
					return;
				}
				_appliedIsDancing = true;
				_appliedDanceIndex = activeDanceIndex;
				if (!(_networkedAnimator == null) && !(_appearanceSettings == null) && _appearanceSettings.DanceAnimatorTriggers != null && activeDanceIndex >= 0 && activeDanceIndex < _appearanceSettings.DanceAnimatorTriggers.Count)
				{
					string text = _appearanceSettings.DanceAnimatorTriggers[activeDanceIndex];
					if (!string.IsNullOrWhiteSpace(text))
					{
						_networkedAnimator.SetTriggerLocal(text);
					}
				}
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
