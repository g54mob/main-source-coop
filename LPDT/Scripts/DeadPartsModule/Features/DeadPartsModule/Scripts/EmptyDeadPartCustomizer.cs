using Fusion;
using UnityEngine;

namespace Features.DeadPartsModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class EmptyDeadPartCustomizer : BaseDeadPartCustomizer
	{
		[SerializeField]
		private DeadPartCustomizationData _customizationData;

		public override void Spawned()
		{
			base.Spawned();
			CustomizeDeadPart(_customizationData);
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
