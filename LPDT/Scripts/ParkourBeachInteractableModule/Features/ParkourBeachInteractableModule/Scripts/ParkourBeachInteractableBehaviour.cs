using Fusion;

namespace Features.ParkourBeachInteractableModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class ParkourBeachInteractableBehaviour : NetworkBehaviour
	{
		public override void Spawned()
		{
			base.Spawned();
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
