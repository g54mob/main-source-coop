using Features.StatsUsageModule.Scripts.StatsData;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;

namespace Features.StoreModule.Scripts
{
	public class StoreStatsPresenter : PresenterBehaviour<StoreStatsViewBase>
	{
		private readonly SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private IStat _stat;

		private int _playerId = -1;

		public StoreStatsPresenter(SpawnedEntityStatsModel spawnedEntityStatsModel)
		{
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			if (_playerId >= 0)
			{
				_spawnedEntityStatsModel.OnPlayerStatRegistered -= TrackPlayerStat;
				_spawnedEntityStatsModel.OnPlayerStatRegistered += TrackPlayerStat;
				TrackPlayerStat(_playerId);
			}
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= TrackPlayerStat;
			if (_stat != null)
			{
				_stat.OnMaxValueChanged -= UpdateStatMaxValue;
				_stat.OnFullValueChanged -= UpdateStatCurrentValue;
			}
		}

		public void SetPlayerId(int playerId)
		{
			_playerId = playerId;
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= TrackPlayerStat;
			_spawnedEntityStatsModel.OnPlayerStatRegistered += TrackPlayerStat;
			TrackPlayerStat(playerId);
		}

		private void TrackPlayerStat(int playerId)
		{
			if (_playerId == playerId && _spawnedEntityStatsModel.PlayerStats.ContainsKey(playerId))
			{
				if (_stat != null)
				{
					_stat.OnMaxValueChanged -= UpdateStatMaxValue;
					_stat.OnFullValueChanged -= UpdateStatCurrentValue;
				}
				_stat = _spawnedEntityStatsModel.PlayerStats[playerId].GetStat(base.View.TrackedStatType);
				UpdateStatCurrentValue(_stat.FullValue);
				UpdateStatMaxValue(_stat.MaxValue);
				_stat.OnMaxValueChanged += UpdateStatMaxValue;
				_stat.OnFullValueChanged += UpdateStatCurrentValue;
			}
		}

		private void UpdateStatCurrentValue(float value)
		{
			base.View.SetCurrentValue(value);
		}

		private void UpdateStatMaxValue(float value)
		{
			base.View.SetMaxValue(value);
		}
	}
}
