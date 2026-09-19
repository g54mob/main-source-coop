using System.Linq;
using Features.MultiplayerSessionServices.Scripts;
using Features.RumModule.Scripts;
using Features.StatsUsageModule.Scripts;
using Features.StatsUsageModule.Scripts.StatsData;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;

namespace Features.HUDModule.Scripts
{
	public class PlayerStatPresenter : PresenterBehaviour<PlayerStatViewBase>
	{
		private readonly SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly StatsViewModel _statsViewModel;

		private readonly RumStatsRewardModel _rumStatsRewardModel;

		private IStat _stat;

		public PlayerStatPresenter(SpawnedEntityStatsModel spawnedEntityStatsModel, MultiplayerModel multiplayerModel, StatsViewModel statsViewModel, RumStatsRewardModel rumStatsRewardModel)
		{
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_multiplayerModel = multiplayerModel;
			_statsViewModel = statsViewModel;
			_rumStatsRewardModel = rumStatsRewardModel;
		}

		protected override void OnViewEnabled()
		{
			_statsViewModel.OnStatsVisibilityChanged += ChangeVisibility;
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= TrackPlayerStat;
			_spawnedEntityStatsModel.OnPlayerStatRegistered += TrackPlayerStat;
			if (_spawnedEntityStatsModel.PlayerStats.ContainsKey(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				TrackPlayerStat(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			}
			_rumStatsRewardModel.OnRumDataAdded += TryTriggerAnimation;
			_rumStatsRewardModel.OnTemporalRumDataAdded += TryTriggerAnimation;
		}

		protected override void OnViewDisabled()
		{
			_statsViewModel.OnStatsVisibilityChanged -= ChangeVisibility;
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= TrackPlayerStat;
			if (_stat != null)
			{
				_stat.OnFullValueChanged -= UpdateStatCurrentValue;
				_stat.OnFullValueChanged -= UpdateStatMaxValue;
			}
			_rumStatsRewardModel.OnRumDataAdded -= TryTriggerAnimation;
			_rumStatsRewardModel.OnTemporalRumDataAdded -= TryTriggerAnimation;
		}

		private void TrackPlayerStat(int playerId)
		{
			if (_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId == playerId)
			{
				if (_stat != null)
				{
					_stat.OnFullValueChanged -= UpdateStatCurrentValue;
					_stat.OnFullValueChanged -= UpdateStatMaxValue;
				}
				_stat = _spawnedEntityStatsModel.PlayerStats[playerId].GetStat(base.View.TrackedStatType);
				_stat.OnFullValueChanged += UpdateStatCurrentValue;
				_stat.OnFullValueChanged += UpdateStatMaxValue;
				UpdateStatCurrentValue(_stat.FullValue);
				UpdateStatMaxValue(_stat.MaxValue);
			}
		}

		private void UpdateStatCurrentValue(float value)
		{
			base.View.SetCurrentValue(_stat.FullValue);
		}

		private void UpdateStatMaxValue(float value)
		{
			base.View.SetMaxValue(_stat.MaxValue);
		}

		private void ChangeVisibility(bool isVisible)
		{
			base.View.SetVisible(isVisible);
		}

		private void TryTriggerAnimation(RumData rumData)
		{
			if (rumData.RumModifiers.Any((ModifierStatsData m) => m.EntityStatType == base.View.TrackedStatType))
			{
				TriggerAnimation();
			}
		}

		private void TryTriggerAnimation(TemporalRumData rumData)
		{
			if (rumData.ActivatedModifiers.Any((ModifierStatsData m) => m.EntityStatType == base.View.TrackedStatType))
			{
				TriggerAnimation();
			}
		}

		private void TriggerAnimation()
		{
			base.View.TriggerIncreaseAnimation();
		}
	}
}
