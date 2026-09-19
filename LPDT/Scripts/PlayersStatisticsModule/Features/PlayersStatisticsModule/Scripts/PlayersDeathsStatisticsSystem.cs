using System;
using Features.PlayerStatesModule.Scripts;
using Zenject;

namespace Features.PlayersStatisticsModule.Scripts
{
	public class PlayersDeathsStatisticsSystem : IInitializable, IDisposable
	{
		private readonly LevelPlayersGameStatisticsModel _levelPlayersGameStatisticsModel;

		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		public PlayersDeathsStatisticsSystem(LevelPlayersGameStatisticsModel levelPlayersGameStatisticsModel, PlayersStatesSynchronizer playersStatesSynchronizer)
		{
			_levelPlayersGameStatisticsModel = levelPlayersGameStatisticsModel;
			_playersStatesSynchronizer = playersStatesSynchronizer;
		}

		public void Initialize()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged += ProcessPlayerStateChanged;
		}

		public void Dispose()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= ProcessPlayerStateChanged;
		}

		private void ProcessPlayerStateChanged(PlayerStateData playerStateData)
		{
			if (playerStateData.PlayerState == PlayerState.Dead)
			{
				_levelPlayersGameStatisticsModel.AddDeath(playerStateData.PlayerId);
			}
		}
	}
}
