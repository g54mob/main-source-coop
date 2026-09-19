using System;
using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using Features.SettingsMenuModule.Scripts.Data;
using Fusion;
using Fusion.Photon.Realtime;
using Global.StateMachinesModule.Scripts;
using NetworkServices.NetworkEvents;
using RSG.Muffin.SteamSubmodule.SteamModule.Scripts.API;
using Steamworks;
using UnityEngine;
using Zenject;

namespace Features.SteamInviteModule.Scripts
{
	public class SteamLobbyJoinSystem : IInitializable, IDisposable
	{
		private readonly SteamModel _steamModel;

		private readonly SteamLobbyModel _steamLobbyModel;

		private readonly IStartSessionService _startSessionService;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly ISteamInviteService _steamInviteService;

		private readonly RegionsPingModel _regionsPingModel;

		private readonly GameFlowStateMachine _gameFlowStateMachine;

		private readonly ISessionJoinGuardService _sessionJoinGuardService;

		private readonly NetworkRunnerEventBus _networkRunnerEventBus;

		private Callback<GameLobbyJoinRequested_t> _joinRequest;

		private Callback<LobbyEnter_t> _lobbyEntered;

		public SteamLobbyJoinSystem(SteamModel steamModel, SteamLobbyModel steamLobbyModel, IStartSessionService startSessionService, MultiplayerModel multiplayerModel, ISteamInviteService steamInviteService, RegionsPingModel regionsPingModel, GameFlowStateMachine gameFlowStateMachine, ISessionJoinGuardService sessionJoinGuardService, NetworkRunnerEventBus networkRunnerEventBus)
		{
			_steamModel = steamModel;
			_steamLobbyModel = steamLobbyModel;
			_startSessionService = startSessionService;
			_multiplayerModel = multiplayerModel;
			_steamInviteService = steamInviteService;
			_regionsPingModel = regionsPingModel;
			_gameFlowStateMachine = gameFlowStateMachine;
			_sessionJoinGuardService = sessionJoinGuardService;
			_networkRunnerEventBus = networkRunnerEventBus;
		}

		public void Initialize()
		{
			if (_steamModel.IsSteamInitialized)
			{
				RegisterSteamCallbacks();
			}
			else
			{
				_steamModel.OnInitialized += RegisterSteamCallbacks;
			}
		}

		public void Dispose()
		{
			_steamModel.OnInitialized -= RegisterSteamCallbacks;
			_joinRequest?.Dispose();
			_lobbyEntered?.Dispose();
		}

		private void RegisterSteamCallbacks()
		{
			_steamModel.OnInitialized -= RegisterSteamCallbacks;
			_joinRequest?.Dispose();
			_lobbyEntered?.Dispose();
			_joinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
			_lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
			if (!SteamLobbyJoinSessionState.HasConsumedLaunchLobbyJoin)
			{
				_steamInviteService.CheckLaunchCommandLineAndJoinLobby();
			}
		}

		private void OnJoinRequest(GameLobbyJoinRequested_t callback)
		{
			if (_gameFlowStateMachine.CurrentState == GameFlowState.MenuGameState)
			{
				_steamInviteService.JoinLobby(callback.m_steamIDLobby);
			}
		}

		private void OnLobbyEntered(LobbyEnter_t callback)
		{
			JoinFusionSessionFromSteamLobbyAsync(callback).Forget();
		}

		private async UniTaskVoid JoinFusionSessionFromSteamLobbyAsync(LobbyEnter_t callback)
		{
			if (SteamLobbyJoinSessionState.SuppressAutoFusionJoinFromSteamLobby)
			{
				return;
			}
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if ((object)networkRunner != null && networkRunner.IsRunning)
			{
				return;
			}
			CSteamID cSteamID = new CSteamID(callback.m_ulSteamIDLobby);
			try
			{
				string sessionCode = SteamMatchmaking.GetLobbyData(cSteamID, "SessionCode");
				if (string.IsNullOrEmpty(sessionCode))
				{
					return;
				}
				string lobbyData = SteamMatchmaking.GetLobbyData(cSteamID, "SessionRegion");
				if (!string.IsNullOrEmpty(lobbyData))
				{
					_regionsPingModel.CurrentRegion = lobbyData;
					PhotonAppSettings.Global.AppSettings.FixedRegion = lobbyData;
					_steamLobbyModel.SetCurrentLobby(cSteamID);
					if (await _sessionJoinGuardService.IsJoinBlockedAsync(sessionCode, SessionInProgressJoinPolicy.SettleThenBlock))
					{
						_steamInviteService.LeaveCurrentLobby();
						_networkRunnerEventBus.Publish(new OnSessionInProgressBlockedEvent());
						return;
					}
					await _startSessionService.StartJoinRoomTask(new SessionCreateData
					{
						RoomCode = sessionCode,
						JoinSource = JoinSource.SteamInvite
					});
					_steamInviteService.RefreshConnectRichPresence();
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}
	}
}
