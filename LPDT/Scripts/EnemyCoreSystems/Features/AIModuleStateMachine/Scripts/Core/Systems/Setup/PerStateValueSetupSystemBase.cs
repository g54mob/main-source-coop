using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Fusion;

namespace Features.AIModuleStateMachine.Scripts.Core.Systems.Setup
{
	[NetworkBehaviourWeaved(0)]
	public abstract class PerStateValueSetupSystemBase<TStateId> : MonoSystem
	{
		private ICurrentStateProvider<TStateId> _stateProvider;

		private bool _enabled;

		public override bool IsEnabled => _enabled;

		protected abstract float DefaultValue { get; }

		protected void SetStateProvider(ICurrentStateProvider<TStateId> stateProvider)
		{
			_stateProvider = stateProvider;
		}

		public override void Enable()
		{
			_enabled = true;
			Apply(TryGetValue(_stateProvider.CurrentStateId, out var value) ? value : DefaultValue);
		}

		public override void Disable()
		{
			_enabled = false;
			Clear();
		}

		public override void Clear()
		{
		}

		protected abstract bool TryGetValue(TStateId stateId, out float value);

		protected abstract void Apply(float value);

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
