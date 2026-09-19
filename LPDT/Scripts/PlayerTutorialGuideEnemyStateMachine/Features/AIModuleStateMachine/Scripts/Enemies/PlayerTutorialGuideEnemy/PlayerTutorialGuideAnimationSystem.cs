using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.AnimationModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.PlayerTutorialGuideEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerTutorialGuideAnimationSystem : MonoSystem
	{
		[SerializeField]
		private NetworkedAnimationControllerBase _animatorController;

		[SerializeField]
		private NetworkedAnimationControllerBase _handAnimatorController;

		private PlayerTutorialGuideEnemyContext _context;

		private bool _enabled;

		public override bool IsEnabled => _enabled;

		[Inject]
		private void InjectDependencies(PlayerTutorialGuideEnemyContext context)
		{
			_context = context;
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
		}

		private void Update()
		{
			if (base.Initialized)
			{
				_animatorController.SetFloat("Velosity", _context.SmoothedVelocity);
				_animatorController.SetBool("IsCrouch", _context.IsCrouching);
				_animatorController.SetBool("IsAggressive", _context.IsAggressive);
				_animatorController.SetBool("IsDead", _context.IsDead);
				_animatorController.SetBool("IsDancing", _context.IsDancing);
				_handAnimatorController.SetBool("IsGrabbed", _context.HasCarryItem);
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
