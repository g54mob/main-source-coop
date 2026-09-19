using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Fusion;

namespace Features.AIModuleStateMachine.Scripts.Core.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class EmptySystem : MonoSystem
	{
		private bool _enabled;

		public override bool IsEnabled => _enabled;

		public override void Enable()
		{
			_enabled = true;
		}

		public override void Disable()
		{
			_enabled = false;
			Clear();
		}

		private void Update()
		{
			if (base.Initialized)
			{
				_ = _enabled;
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
