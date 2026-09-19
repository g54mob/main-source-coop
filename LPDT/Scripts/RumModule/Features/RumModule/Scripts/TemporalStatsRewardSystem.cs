using System;
using System.Collections.Generic;
using Features.GameUpdaterModule;
using Features.StatsUsageModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.RumModule.Scripts
{
	public class TemporalStatsRewardSystem : IInitializable, IDisposable
	{
		private readonly IGameUpdater _gameUpdater;

		private readonly RumStatsRewardModel _rumStatsRewardModel;

		private readonly IPlayerStatsUpgradeService _playerStatsUpgradeService;

		public TemporalStatsRewardSystem(IGameUpdater gameUpdater, RumStatsRewardModel rumStatsRewardModel, IPlayerStatsUpgradeService playerStatsUpgradeService)
		{
			_gameUpdater = gameUpdater;
			_rumStatsRewardModel = rumStatsRewardModel;
			_playerStatsUpgradeService = playerStatsUpgradeService;
		}

		public void Initialize()
		{
			_gameUpdater.OnUpdate += UpdateDuration;
		}

		public void Dispose()
		{
			_gameUpdater.OnUpdate -= UpdateDuration;
		}

		private void UpdateDuration()
		{
			IReadOnlyList<TemporalRumData> activeTemporalRumsData = _rumStatsRewardModel.ActiveTemporalRumsData;
			for (int num = activeTemporalRumsData.Count - 1; num >= 0; num--)
			{
				TemporalRumData temporalRumData = activeTemporalRumsData[num];
				temporalRumData.Duration += Time.deltaTime;
				if (!(temporalRumData.Duration < temporalRumData.RumData.Duration))
				{
					foreach (ModifierStatsData activatedModifier in temporalRumData.ActivatedModifiers)
					{
						_playerStatsUpgradeService.DiscardModifierStatsSynchronized(activatedModifier);
					}
					_rumStatsRewardModel.RemoveTemporalRumsData(temporalRumData);
				}
			}
		}
	}
}
