using Fusion;

namespace Features.LevelModule.Scripts.LevelTransition
{
	[NetworkBehaviourWeaved(0)]
	public class LevelTransitTrigger : NetworkBehaviour
	{
		public PlayerRef OriginalPlayerRef { get; private set; }

		public override void Spawned()
		{
			base.Spawned();
			OriginalPlayerRef = base.Object.StateAuthority;
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
