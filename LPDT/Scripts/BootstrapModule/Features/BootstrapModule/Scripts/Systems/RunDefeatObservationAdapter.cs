using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.SessionManagementModule.Models;
using Fusion;

namespace Features.BootstrapModule.Scripts.Systems
{
	public sealed class RunDefeatObservationAdapter : IRunDefeatObservation
	{
		private readonly MultiplayerModel _multiplayerModel;

		private readonly IPlayerStateService _playerStateService;

		public RunDefeatObservationAdapter(MultiplayerModel multiplayerModel, IPlayerStateService playerStateService)
		{
			_multiplayerModel = multiplayerModel;
			_playerStateService = playerStateService;
		}

		public bool IsRunLost()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (!networkRunner.IsRunning)
			{
				return false;
			}
			bool result = false;
			foreach (PlayerRef activePlayer in networkRunner.ActivePlayers)
			{
				result = true;
				if (!_playerStateService.IsPlayerDead(activePlayer.PlayerId))
				{
					return false;
				}
			}
			return result;
		}
	}
}
