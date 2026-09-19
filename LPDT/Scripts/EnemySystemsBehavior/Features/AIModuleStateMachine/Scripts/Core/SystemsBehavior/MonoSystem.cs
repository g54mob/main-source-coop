using Fusion;

namespace Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior
{
	[NetworkBehaviourWeaved(0)]
	public abstract class MonoSystem : NetworkBehaviour
	{
		public bool Initialized { get; private set; }

		public abstract bool IsEnabled { get; }

		public abstract void Disable();

		public abstract void Enable();

		public abstract void Clear();

		public override void Spawned()
		{
			base.Spawned();
			Initialized = true;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			Initialized = false;
			base.Despawned(runner, hasState);
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
