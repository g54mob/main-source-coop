using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.AnimationModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class MimicEnemyAnimationSystem : MonoSystem
	{
		[SerializeField]
		private NetworkedAnimationControllerBase _animatorController;

		private MimicEnemyContext _context;

		private bool _enabled;

		public override bool IsEnabled => _enabled;

		[Inject]
		private void InjectDependencies(MimicEnemyContext context)
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
