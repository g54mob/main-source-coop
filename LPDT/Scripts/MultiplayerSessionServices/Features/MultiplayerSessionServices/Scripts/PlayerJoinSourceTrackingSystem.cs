using System;
using Fusion;
using NetworkServices.NetworkEvents;
using Zenject;

namespace Features.MultiplayerSessionServices.Scripts
{
	public class PlayerJoinSourceTrackingSystem : IInitializable, IDisposable, IPlayerJoinSourcesLobbyLifecycle
	{
		private readonly PlayerJoinSourcesModel _playerJoinSourcesModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly NetworkRunnerEventBus _eventBus;

		public PlayerJoinSourceTrackingSystem(PlayerJoinSourcesModel playerJoinSourcesModel, MultiplayerModel multiplayerModel, NetworkRunnerEventBus eventBus)
		{
			_playerJoinSourcesModel = playerJoinSourcesModel;
			_multiplayerModel = multiplayerModel;
			_eventBus = eventBus;
		}

		public void Initialize()
		{
			_playerJoinSourcesModel.AttachmentChanged += OnAttachmentChanged;
			_eventBus.Subscribe<OnPlayerLeftEvent>(OnPlayerLeft);
			if (_playerJoinSourcesModel.IsAttached)
			{
				ReportLocalJoinSource();
			}
		}

		public void Dispose()
		{
			_playerJoinSourcesModel.AttachmentChanged -= OnAttachmentChanged;
			_eventBus.Unsubscribe<OnPlayerLeftEvent>(OnPlayerLeft);
		}

		public void OnLobbyEntered()
		{
			_playerJoinSourcesModel.ClearForLobbyEnter();
			if (_playerJoinSourcesModel.IsAttached)
			{
				ReportLocalJoinSource();
			}
		}

		private void OnAttachmentChanged(bool isAttached)
		{
			if (isAttached)
			{
				ReportLocalJoinSource();
			}
		}

		private void ReportLocalJoinSource()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (!(networkRunner == null) && networkRunner.IsRunning)
			{
				_playerJoinSourcesModel.Report(networkRunner.LocalPlayer.PlayerId, _multiplayerModel.LocalJoinSource);
			}
		}

		private void OnPlayerLeft(OnPlayerLeftEvent playerLeftEvent)
		{
			_playerJoinSourcesModel.Remove(playerLeftEvent.Player.PlayerId);
		}
	}
}
