using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Systems.Timing
{
	[NetworkBehaviourWeaved(0)]
	public class AttackCooldownSystem : MonoSystem
	{
		private IAttackTimingContext _context;

		private bool _isEnabled;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(IAttackTimingContext context)
		{
			_context = context;
		}

		public override void Enable()
		{
			_isEnabled = true;
		}

		public override void Disable()
		{
			_isEnabled = false;
			Clear();
		}

		private void Update()
		{
			if (base.Initialized && _isEnabled)
			{
				_context.AttackCooldown -= Time.deltaTime;
				_context.IsAttackOnCooldown = !(_context.AttackCooldown <= 0f);
			}
		}

		public override void Clear()
		{
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
