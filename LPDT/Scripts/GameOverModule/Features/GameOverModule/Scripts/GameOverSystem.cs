using System;
using Features.ViewSystemModule.Scripts.Windows;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using Zenject;

namespace Features.GameOverModule.Scripts
{
	public class GameOverSystem : IInitializable, IDisposable
	{
		private readonly GameOverNetworkEvent _gameOverNetworkEvent;

		private readonly GameOverWindow _gameOverWindow;

		private readonly QuotaCompletedWindow _quotaCompletedWindow;

		private readonly DeathWindow _deathWindow;

		public GameOverSystem(GameOverNetworkEvent gameOverNetworkEvent, GameOverWindow gameOverWindow, DeathWindow deathWindow, QuotaCompletedWindow quotaCompletedWindow)
		{
			_gameOverNetworkEvent = gameOverNetworkEvent;
			_gameOverWindow = gameOverWindow;
			_deathWindow = deathWindow;
			_quotaCompletedWindow = quotaCompletedWindow;
		}

		public void Initialize()
		{
			_gameOverNetworkEvent.OnNetworkEventSend += GameOver;
		}

		public void Dispose()
		{
			_gameOverNetworkEvent.OnNetworkEventSend -= GameOver;
		}

		private void GameOver(GameOverNetworkEvent gameOverNetworkEvent)
		{
			if (gameOverNetworkEvent.GameOverReason == GameOverReason.AllPlayersKilled)
			{
				if (_deathWindow.WindowStatus == WindowStatus.Showed)
				{
					_deathWindow.Close();
				}
				if (_gameOverWindow.WindowStatus != WindowStatus.Showed)
				{
					_gameOverWindow.Open();
				}
			}
			else if (gameOverNetworkEvent.GameOverReason == GameOverReason.QuotaCompleted)
			{
				if (_deathWindow.WindowStatus == WindowStatus.Showed)
				{
					_deathWindow.Close();
				}
				if (_quotaCompletedWindow.WindowStatus != WindowStatus.Showed)
				{
					_quotaCompletedWindow.Open();
				}
			}
		}
	}
}
