using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Fusion;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Systems.Navigation
{
	[NetworkBehaviourWeaved(0)]
	public class FindTargetPlayerPositionResetSystem : MonoSystem
	{
		private IMovementContext _movementContext;

		private IDetectionContext _detectionContext;

		private bool _isEnabled;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(IMovementContext movementContext, IDetectionContext detectionContext)
		{
			_movementContext = movementContext;
			_detectionContext = detectionContext;
		}

		public override void Enable()
		{
			_isEnabled = true;
			_detectionContext.OnPriorityPlayerChanged += ProcessPriorityPlayerChanged;
		}

		public override void Disable()
		{
			_isEnabled = false;
			_detectionContext.OnPriorityPlayerChanged -= ProcessPriorityPlayerChanged;
			Clear();
		}

		public override void Clear()
		{
		}

		private void ProcessPriorityPlayerChanged()
		{
			if (base.Initialized && _isEnabled)
			{
				_movementContext.NeedToFindTargetPosition = true;
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
