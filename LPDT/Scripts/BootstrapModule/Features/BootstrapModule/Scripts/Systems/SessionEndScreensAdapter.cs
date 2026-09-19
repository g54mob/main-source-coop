using Features.GameOverModule.Scripts;
using Features.InputModule.Scripts.Generated;
using Features.PlayersStatisticsModule.Scripts.Views.Statistics;
using Features.SessionManagementModule.Models;
using Features.ViewSystemModule.Scripts.Windows;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.BootstrapModule.Scripts.Systems
{
	public sealed class SessionEndScreensAdapter : ISessionEndScreens
	{
		private readonly GameOverNetworkEvent _gameOverNetworkEvent;

		private readonly GameOverWindow _gameOverWindow;

		private readonly QuotaCompletedWindow _quotaCompletedWindow;

		private readonly DeathWindow _deathWindow;

		private readonly StatisticWindow _statisticWindow;

		private readonly IInputService _inputService;

		public SessionEndScreensAdapter(GameOverNetworkEvent gameOverNetworkEvent, GameOverWindow gameOverWindow, QuotaCompletedWindow quotaCompletedWindow, DeathWindow deathWindow, StatisticWindow statisticWindow, IInputService inputService)
		{
			_gameOverNetworkEvent = gameOverNetworkEvent;
			_gameOverWindow = gameOverWindow;
			_quotaCompletedWindow = quotaCompletedWindow;
			_deathWindow = deathWindow;
			_statisticWindow = statisticWindow;
			_inputService = inputService;
		}

		public void ShowDefeat()
		{
			_gameOverNetworkEvent.SendEvent(GameOverReason.AllPlayersKilled);
		}

		public void ShowRunComplete()
		{
			_gameOverNetworkEvent.SendEvent(GameOverReason.QuotaCompleted);
		}

		public void Dismiss()
		{
			if (_gameOverWindow.WindowStatus == WindowStatus.Showed)
			{
				_gameOverWindow.Close();
			}
			if (_quotaCompletedWindow.WindowStatus == WindowStatus.Showed)
			{
				_quotaCompletedWindow.Close();
			}
			if (_deathWindow.WindowStatus == WindowStatus.Showed)
			{
				_deathWindow.Close();
			}
			if (_statisticWindow.WindowStatus == WindowStatus.Showed)
			{
				_statisticWindow.Close();
			}
			_inputService.EnableArmMap();
		}
	}
}
