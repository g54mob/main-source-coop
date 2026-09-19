using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Systems.Timing
{
	[NetworkBehaviourWeaved(0)]
	public class StateDurationTimeSystem : MonoSystem
	{
		private IStateTimingContext _context;

		private bool _isEnabled;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(IStateTimingContext context)
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
				_context.CurrentStateTime += Time.deltaTime;
			}
		}

		public override void Clear()
		{
			_context.CurrentStateTime = 0f;
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
