using Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.Settings;
using Features.AnimationModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class EarVisualController : NetworkBehaviour
	{
		private static readonly int VelocityParameter = Animator.StringToHash("Velocity");

		private static readonly int IsMovingParameter = Animator.StringToHash("IsMoving");

		private static readonly int IsAttackingParameter = Animator.StringToHash("IsAttacking");

		private static readonly int IsStunnedParameter = Animator.StringToHash("IsStunned");

		private static readonly int IsFearParameter = Animator.StringToHash("IsFear");

		[SerializeField]
		private NetworkedAnimationControllerBase _animatorController;

		private EarEnemyContext _context;

		private EarEnemySettings _settings;

		[Inject]
		public void InjectDependencies(EarEnemyContext context, EarEnemySettings settings)
		{
			_context = context;
			_settings = settings;
		}

		public override void Render()
		{
			if (!(_animatorController == null))
			{
				EarVisualState visualState = _context.VisualState;
				float aggroSpeed = _settings.AggroSpeed;
				float value = ((aggroSpeed > 0f) ? Mathf.Clamp01(_context.SmoothedVelocity / aggroSpeed) : 0f);
				_animatorController.SetFloat(VelocityParameter, value);
				_animatorController.SetBool(IsMovingParameter, visualState == EarVisualState.Wandering || visualState == EarVisualState.Aggro || visualState == EarVisualState.Fear || visualState == EarVisualState.MoveToNewArea);
				_animatorController.SetBool(IsFearParameter, visualState == EarVisualState.Fear);
				_animatorController.SetBool(IsAttackingParameter, visualState == EarVisualState.Attack);
				_animatorController.SetBool(IsStunnedParameter, visualState == EarVisualState.Stun);
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
