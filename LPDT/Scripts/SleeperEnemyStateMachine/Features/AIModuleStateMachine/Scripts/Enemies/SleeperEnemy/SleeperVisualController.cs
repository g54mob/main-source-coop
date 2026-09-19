using Features.AnimationModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class SleeperVisualController : NetworkBehaviour
	{
		private const string VELOCITY_PARAMETER = "Velocity";

		private const string IS_SLEEPING_PARAMETER = "IsSleeping";

		private const string IS_MOVING_PARAMETER = "IsMoving";

		private const string IS_AGGRESSIVE_PARAMETER = "IsAggressive";

		private const string IS_ATTACKING_PARAMETER = "IsAttacking";

		private const string MOVING_IMMEDIATELY_PARAMETER = "MovingImmediately";

		[SerializeField]
		private NetworkedAnimationControllerBase _animatorController;

		private SleeperEnemyContext _context;

		[Inject]
		public void InjectDependencies(SleeperEnemyContext context)
		{
			_context = context;
		}

		public override void Render()
		{
			if (!(_animatorController == null))
			{
				SleeperVisualState visualState = _context.VisualState;
				float statValue = _context.GetStatValue(EntityStatType.SprintSpeed);
				float value = ((statValue > 0f) ? Mathf.Clamp01(_context.SmoothedVelocity / statValue) : 0f);
				bool value2 = visualState == SleeperVisualState.MovingImmediately || visualState == SleeperVisualState.DamageAggro;
				_animatorController.SetFloat("Velocity", value);
				_animatorController.SetBool("MovingImmediately", value2);
				_animatorController.SetBool("IsSleeping", visualState == SleeperVisualState.Sleep);
				_animatorController.SetBool("IsAggressive", visualState == SleeperVisualState.Aggressive || visualState == SleeperVisualState.WakeUp);
				_animatorController.SetBool("IsAttacking", visualState == SleeperVisualState.Attack);
				_animatorController.SetBool("IsMoving", visualState == SleeperVisualState.Chase || visualState == SleeperVisualState.GoHome || visualState == SleeperVisualState.Investigate || visualState == SleeperVisualState.DamageAggro || visualState == SleeperVisualState.Fear || visualState == SleeperVisualState.MovingImmediately);
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
