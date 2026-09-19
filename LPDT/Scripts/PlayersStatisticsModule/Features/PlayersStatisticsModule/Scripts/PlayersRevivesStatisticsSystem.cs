using System;
using Features.DeadPartsModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Zenject;

namespace Features.PlayersStatisticsModule.Scripts
{
	public class PlayersRevivesStatisticsSystem : IInitializable, IDisposable
	{
		private readonly LevelPlayersGameStatisticsModel _levelPlayersGameStatisticsModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly PlayerResurrectionByPlayerEventClass _playerResurrectionByPlayerEventClass;

		public PlayersRevivesStatisticsSystem(LevelPlayersGameStatisticsModel levelPlayersGameStatisticsModel, MultiplayerModel multiplayerModel, PlayerResurrectionByPlayerEventClass playerResurrectionByPlayerEventClass)
		{
			_levelPlayersGameStatisticsModel = levelPlayersGameStatisticsModel;
			_multiplayerModel = multiplayerModel;
			_playerResurrectionByPlayerEventClass = playerResurrectionByPlayerEventClass;
		}

		public void Initialize()
		{
			_playerResurrectionByPlayerEventClass.OnPlayerResurrectedByPlayer += ProcessPlayersRevives;
		}

		public void Dispose()
		{
			_playerResurrectionByPlayerEventClass.OnPlayerResurrectedByPlayer -= ProcessPlayersRevives;
		}

		private void ProcessPlayersRevives(int resurrectedPlayerId, int resurrectedByPlayerId)
		{
			if (resurrectedByPlayerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				_levelPlayersGameStatisticsModel.RaiseRevive(resurrectedByPlayerId);
			}
		}
	}
}
