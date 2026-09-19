using Fusion;

namespace Features.Movement.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class ColorChangerByPlayerRef : StaticColorChangerByPlayerRef
	{
		public override void Spawned()
		{
			base.Spawned();
			PlayerCustomizationModel.OnSlotsChanged += base.AdjustColorByCustomization;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			PlayerCustomizationModel.OnSlotsChanged -= base.AdjustColorByCustomization;
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
