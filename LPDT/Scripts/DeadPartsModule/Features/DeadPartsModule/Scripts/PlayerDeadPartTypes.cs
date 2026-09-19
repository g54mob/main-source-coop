using Features.PlayerPresenceModule.Networked;

namespace Features.DeadPartsModule.Scripts
{
	public sealed class PlayerDeadPartTypes
	{
		private readonly SessionPlayerDeadPartStore _sessionPlayerDeadPartStore;

		private readonly PlayerDeadPartsConfiguration _playerDeadPartsConfiguration;

		public PlayerDeadPartTypes(SessionPlayerDeadPartStore sessionPlayerDeadPartStore, PlayerDeadPartsConfiguration playerDeadPartsConfiguration)
		{
			_sessionPlayerDeadPartStore = sessionPlayerDeadPartStore;
			_playerDeadPartsConfiguration = playerDeadPartsConfiguration;
		}

		public DeadPartType GetDeadPartType(int playerId)
		{
			if (_sessionPlayerDeadPartStore.TryGetBottomPart(playerId, out var snapshot) && snapshot.HasValue)
			{
				return (DeadPartType)snapshot.TypeId;
			}
			return _playerDeadPartsConfiguration.InitialDeadPartType;
		}

		public bool HasConnectedPart(int playerId)
		{
			if (_sessionPlayerDeadPartStore.TryGetBottomPart(playerId, out var snapshot))
			{
				return snapshot.HasValue;
			}
			return false;
		}

		public int GetUsageCount(int playerId)
		{
			if (!_sessionPlayerDeadPartStore.TryGetBottomPart(playerId, out var snapshot))
			{
				return 0;
			}
			return snapshot.UsageCount;
		}

		public void SetLocalBottomPart(DeadPartType deadPartType, DeadPartCustomizationData customization, int usageCount)
		{
			_sessionPlayerDeadPartStore.TryWriteLocalBottomPart((int)deadPartType, customization.ButtColor, (int)customization.ButtSkinId, (int)customization.ButtTexturePreset, (int)customization.ButtMeshPreset, usageCount);
		}
	}
}
