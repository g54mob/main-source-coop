using Features.MultiplayerSessionServices.Scripts;
using NetworkServices.NetworkEvents;

namespace Features.SessionManagementModule.Models
{
	public sealed class SessionGameplayActivation : ISessionGameplayActivation
	{
		private readonly ILevelSelectionSource _levelSelectionSource;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly NetworkRunnerEventBus _networkRunnerEventBus;

		private bool _hasEnteredInSessionPhase;

		public SessionGameplayActivation(ILevelSelectionSource levelSelectionSource, MultiplayerModel multiplayerModel, NetworkRunnerEventBus networkRunnerEventBus)
		{
			_levelSelectionSource = levelSelectionSource;
			_multiplayerModel = multiplayerModel;
			_networkRunnerEventBus = networkRunnerEventBus;
		}

		public void NotifySessionStarted()
		{
			if (!_hasEnteredInSessionPhase)
			{
				_hasEnteredInSessionPhase = true;
				_networkRunnerEventBus.Publish(new OnStartGamePhaseEvent(_levelSelectionSource.SelectedLevelName, _multiplayerModel.NetworkRunner.SessionInfo.PlayerCount));
			}
		}
	}
}
