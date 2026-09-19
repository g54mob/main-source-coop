using Features.GameUpdaterModule;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.PlayersStatisticsModule.Scripts.Views.Statistics
{
	public class StatisticsPlayerPresenter : PresenterBehaviour<StatisticsPlayerViewBase>
	{
		private int _playerId = -1;

		private readonly LevelPlayersGameStatisticsModel _levelPlayersGameStatisticsModel;

		private readonly IGameUpdater _gameUpdater;

		public StatisticsPlayerPresenter(LevelPlayersGameStatisticsModel levelPlayersGameStatisticsModel, IGameUpdater gameUpdater)
		{
			_levelPlayersGameStatisticsModel = levelPlayersGameStatisticsModel;
			_gameUpdater = gameUpdater;
		}

		protected override void OnViewSet()
		{
			_gameUpdater.OnUpdate += UpdateView;
		}

		protected override void OnDisposed()
		{
			_gameUpdater.OnUpdate -= UpdateView;
		}

		private void UpdateView()
		{
			if (_playerId != -1)
			{
				base.View.DeathsText.text = _levelPlayersGameStatisticsModel.GetDeaths(_playerId).ToString();
				base.View.RevivesText.text = _levelPlayersGameStatisticsModel.GetRevives(_playerId).ToString();
				base.View.KillsText.text = _levelPlayersGameStatisticsModel.GetKills(_playerId).ToString();
				base.View.CentsText.text = _levelPlayersGameStatisticsModel.GetCents(_playerId).ToString();
			}
		}

		public void SetPlayerId(int playerId)
		{
			_playerId = playerId;
		}
	}
}
