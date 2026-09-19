using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Fusion;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Systems.Navigation
{
	[NetworkBehaviourWeaved(0)]
	public class TargetPositionCompletedResetSystem : MonoSystem
	{
		private IMovementContext _movementContext;

		private bool _enabled;

		public override bool IsEnabled => _enabled;

		[Inject]
		private void InjectDependencies(IMovementContext movementContext)
		{
			_movementContext = movementContext;
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
			if (base.Initialized && _enabled && _movementContext.TargetPositionCompleted)
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
