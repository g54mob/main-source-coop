using Fusion;
using UnityEngine;

namespace Features.DeadPartsModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class DetachedDeadPartCracksRenderer : DeadPartCracksRenderer
	{
		[SerializeField]
		private PlayerDeadPart _playerDeadPart;

		public override void Spawned()
		{
			base.Spawned();
			_playerDeadPart.OnUsageCountChanged += base.UpdateCracksBasedOnUsage;
			base.DeadPartType = _playerDeadPart.DeadPartType;
			UpdateCracksBasedOnUsage(_playerDeadPart.UsageCount);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_playerDeadPart.OnUsageCountChanged -= base.UpdateCracksBasedOnUsage;
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
