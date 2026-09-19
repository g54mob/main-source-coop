using System;
using System.Collections.Generic;
using Features.LevelModule.Scripts;
using Features.StatsUsageModule.Scripts;
using Zenject;

namespace Features.RumModule.Scripts
{
	public class RumEffectsCleanupSystem : IInitializable, IDisposable
	{
		private readonly BeforeLevelChangeNetworkEvent _beforeLevelChangeNetworkEvent;

		private readonly RumStatsRewardModel _rumStatsRewardModel;

		private readonly IPlayerStatsUpgradeService _playerStatsUpgradeService;

		public RumEffectsCleanupSystem(RumStatsRewardModel rumStatsRewardModel, BeforeLevelChangeNetworkEvent beforeLevelChangeNetworkEvent, IPlayerStatsUpgradeService playerStatsUpgradeService)
		{
			_beforeLevelChangeNetworkEvent = beforeLevelChangeNetworkEvent;
			_rumStatsRewardModel = rumStatsRewardModel;
			_playerStatsUpgradeService = playerStatsUpgradeService;
		}

		public void Initialize()
		{
			_beforeLevelChangeNetworkEvent.OnNetworkEventSend += CleanUp;
		}

		public void Dispose()
		{
			_beforeLevelChangeNetworkEvent.OnNetworkEventSend -= CleanUp;
		}

		private void CleanUp(BeforeLevelChangeNetworkEvent _)
		{
			IReadOnlyList<TemporalRumData> activeTemporalRumsData = _rumStatsRewardModel.ActiveTemporalRumsData;
			for (int num = activeTemporalRumsData.Count - 1; num >= 0; num--)
			{
				TemporalRumData temporalRumData = activeTemporalRumsData[num];
				if (temporalRumData.RumData.PerLevel)
				{
					foreach (ModifierStatsData activatedModifier in temporalRumData.ActivatedModifiers)
					{
						_playerStatsUpgradeService.DiscardModifierStatsSynchronized(activatedModifier);
					}
					_rumStatsRewardModel.RemoveTemporalRumsData(temporalRumData);
				}
			}
			IReadOnlyList<RumData> activeRumsData = _rumStatsRewardModel.ActiveRumsData;
			for (int num2 = activeRumsData.Count - 1; num2 >= 0; num2--)
			{
				RumData rumData = activeRumsData[num2];
				if (rumData.PerLevel)
				{
					foreach (ModifierStatsData rumModifier in rumData.RumModifiers)
					{
						_playerStatsUpgradeService.DiscardModifierStatsSynchronized(rumModifier);
					}
					_rumStatsRewardModel.RemoveRumsData(rumData);
				}
			}
		}
	}
}
