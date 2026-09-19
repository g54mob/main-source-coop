using Fusion;
using Zenject;

namespace Features.DeadPartsModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerDeadPartCracksRenderer : DeadPartCracksRenderer
	{
		private PlayerDeadPartTypes _playerDeadPartTypes;

		private bool _hasRendered;

		private DeadPartType _lastType;

		private int _lastUsageCount;

		[Inject]
		public void InjectDependencies(PlayerDeadPartTypes playerDeadPartTypes)
		{
			_playerDeadPartTypes = playerDeadPartTypes;
		}

		public override void Render()
		{
			int playerId = base.Object.StateAuthority.PlayerId;
			DeadPartType deadPartType = _playerDeadPartTypes.GetDeadPartType(playerId);
			int usageCount = _playerDeadPartTypes.GetUsageCount(playerId);
			if (!_hasRendered || deadPartType != _lastType || usageCount != _lastUsageCount)
			{
				_hasRendered = true;
				_lastType = deadPartType;
				_lastUsageCount = usageCount;
				base.DeadPartType = deadPartType;
				UpdateCracksBasedOnUsage(usageCount);
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
