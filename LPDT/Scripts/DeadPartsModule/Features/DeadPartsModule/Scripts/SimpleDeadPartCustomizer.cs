using Fusion;
using UnityEngine;

namespace Features.DeadPartsModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class SimpleDeadPartCustomizer : BaseDeadPartCustomizer
	{
		[SerializeField]
		private PlayerDeadPart _playerDeadPart;

		public override void Spawned()
		{
			base.Spawned();
			_playerDeadPart.OnCustomizationDataChanged += base.CustomizeDeadPart;
			CustomizeDeadPart(_playerDeadPart.CustomizationData);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_playerDeadPart.OnCustomizationDataChanged -= base.CustomizeDeadPart;
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
