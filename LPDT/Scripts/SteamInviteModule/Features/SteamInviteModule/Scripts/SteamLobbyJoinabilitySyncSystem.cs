using System;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using Global.StateMachinesModule.Scripts;
using Zenject;

namespace Features.SteamInviteModule.Scripts
{
	public class SteamLobbyJoinabilitySyncSystem : IInitializable, IDisposable
	{
		private readonly GameFlowStateMachine _gameFlowStateMachine;

		private readonly ISteamInviteService _steamInviteService;

		private readonly MultiplayerModel _multiplayerModel;

		public SteamLobbyJoinabilitySyncSystem(GameFlowStateMachine gameFlowStateMachine, ISteamInviteService steamInviteService, MultiplayerModel multiplayerModel)
		{
			_gameFlowStateMachine = gameFlowStateMachine;
			_steamInviteService = steamInviteService;
			_multiplayerModel = multiplayerModel;
		}

		public void Initialize()
		{
			_gameFlowStateMachine.OnStateEnter += OnGameFlowStateEntered;
			_multiplayerModel.OnSessionOpenedChanged += OnSessionOpenedChanged;
		}

		public void Dispose()
		{
			_gameFlowStateMachine.OnStateEnter -= OnGameFlowStateEntered;
			_multiplayerModel.OnSessionOpenedChanged -= OnSessionOpenedChanged;
		}

		private void OnGameFlowStateEntered(GameFlowState state)
		{
			switch (state)
			{
			case GameFlowState.LobbyGameState:
				OpenCurrentLobby();
				break;
			case GameFlowState.SessionGameState:
				_steamInviteService.CloseCurrentLobby();
				break;
			case GameFlowState.MenuGameState:
				_steamInviteService.LeaveCurrentLobby();
				break;
			}
		}

		private void OnSessionOpenedChanged(bool isOpened)
		{
			if (isOpened)
			{
				OpenCurrentLobby();
			}
			else
			{
				_steamInviteService.CloseCurrentLobby();
			}
		}

		private void OpenCurrentLobby()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (!(networkRunner == null) && networkRunner.IsRunning)
			{
				_steamInviteService.WriteLobbyData(networkRunner.SessionInfo.Name, networkRunner.SessionInfo.Region);
				_steamInviteService.OpenCurrentLobby();
			}
		}
	}
}
