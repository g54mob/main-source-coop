using System;
using NetworkServices.NetworkEvents;
using Zenject;

namespace Features.MultiplayerSessionServices.Scripts
{
	public class MatchmakingAutoCloseSystem : IInitializable, IDisposable
	{
		private readonly MultiplayerModel _multiplayerModel;

		private readonly IMultiplayerService _multiplayerService;

		private readonly NetworkRunnerEventBus _eventBus;

		public MatchmakingAutoCloseSystem(MultiplayerModel multiplayerModel, IMultiplayerService multiplayerService, NetworkRunnerEventBus eventBus)
		{
			_multiplayerModel = multiplayerModel;
			_multiplayerService = multiplayerService;
			_eventBus = eventBus;
		}

		public void Initialize()
		{
			_eventBus.Subscribe<OnPlayerJoinedEvent>(OnPlayerJoined);
			_eventBus.Subscribe<OnPlayerLeftEvent>(OnPlayerLeft);
			_multiplayerService.RefreshSessionVisibility(_multiplayerModel.NetworkRunner);
		}

		public void Dispose()
		{
			_eventBus.Unsubscribe<OnPlayerJoinedEvent>(OnPlayerJoined);
			_eventBus.Unsubscribe<OnPlayerLeftEvent>(OnPlayerLeft);
		}

		private void OnPlayerJoined(OnPlayerJoinedEvent _)
		{
			_multiplayerService.RefreshSessionVisibility(_multiplayerModel.NetworkRunner);
		}

		private void OnPlayerLeft(OnPlayerLeftEvent _)
		{
			_multiplayerService.RefreshSessionVisibility(_multiplayerModel.NetworkRunner);
		}
	}
}
