using System;
using System.Collections.Generic;
using System.Globalization;
using Features.DebugModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using GameplayEvents;
using NetworkServices.NetworkEvents;
using PlayerCustomization.Data;
using RSG.Muffin.SteamSubmodule.SteamModule.Scripts.API;
using Steamworks;
using UnityEngine;
using Zenject;

namespace Features.NetworkTelemetry.Scripts
{
	public class TelemetryEventBusObserver : IInitializable, IDisposable
	{
		private readonly TelemetryService _telemetryService;

		private readonly NetworkRunnerEventBus _eventBus;

		private readonly GameplayEventBus _gameplayEventBus;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly PlayerProfileModel _playerProfileModel;

		private readonly SteamModel _steamModel;

		private readonly DiContainer _diContainer;

		public TelemetryEventBusObserver(TelemetryService telemetryService, NetworkRunnerEventBus eventBus, GameplayEventBus gameplayEventBus, MultiplayerModel multiplayerModel, PlayerProfileModel playerProfileModel, SteamModel steamModel, DiContainer diContainer)
		{
			_telemetryService = telemetryService;
			_eventBus = eventBus;
			_gameplayEventBus = gameplayEventBus;
			_multiplayerModel = multiplayerModel;
			_playerProfileModel = playerProfileModel;
			_steamModel = steamModel;
			_diContainer = diContainer;
		}

		public void Initialize()
		{
			_eventBus.Subscribe<OnSuccessfullyStartGameEvent>(OnSuccessfullyStartGame);
			_eventBus.Subscribe<OnStartGamePhaseEvent>(OnStartGamePhase);
			_eventBus.Subscribe<OnPlayerJoinedEvent>(OnPlayerJoined);
			_eventBus.Subscribe<OnPlayerLeftEvent>(OnPlayerLeft);
			_eventBus.Subscribe<OnShutdownEvent>(OnShutdown);
			_eventBus.Subscribe<OnDisconnectedFromServerEvent>(OnDisconnectedFromServer);
			_eventBus.Subscribe<OnConnectFailedEvent>(OnConnectFailed);
			_eventBus.Subscribe<OnFailedStartGameEvent>(OnFailedStartGame);
			_gameplayEventBus.Subscribe<OnItemGrabbedStateChangeEvent>(OnItemInteraction);
			_gameplayEventBus.Subscribe<OnGameplaySceneLoadedEvent>(OnGameplaySceneLoaded);
			_gameplayEventBus.Subscribe<OnSessionQuotaChangedEvent>(OnSessionQuotaChanged);
			_gameplayEventBus.Subscribe<OnPlayerDiedGameplayEvent>(OnPlayerDied);
			_gameplayEventBus.Subscribe<OnPlayerResurrectedGameplayEvent>(OnPlayerResurrected);
			_gameplayEventBus.Subscribe<OnPlayerTemporaryRagdollStartedGameplayEvent>(OnTemporaryRagdollStarted);
			_gameplayEventBus.Subscribe<OnPlayerTemporaryRagdollRecoveredGameplayEvent>(OnTemporaryRagdollRecovered);
			_gameplayEventBus.Subscribe<OnPlayerTemporaryRagdollStuckGameplayEvent>(OnTemporaryRagdollStuck);
			_gameplayEventBus.Subscribe<OnEnemyDespawnedGameplayEvent>(OnEnemyDespawned);
			_gameplayEventBus.Subscribe<OnPlayerGrabDiagnosticGameplayEvent>(OnPlayerGrabDiagnostic);
			_gameplayEventBus.Subscribe<OnLineArmGrabNullResolveGameplayEvent>(OnLineArmGrabNullResolve);
		}

		public void Dispose()
		{
			_eventBus.Unsubscribe<OnSuccessfullyStartGameEvent>(OnSuccessfullyStartGame);
			_eventBus.Unsubscribe<OnStartGamePhaseEvent>(OnStartGamePhase);
			_eventBus.Unsubscribe<OnPlayerJoinedEvent>(OnPlayerJoined);
			_eventBus.Unsubscribe<OnPlayerLeftEvent>(OnPlayerLeft);
			_eventBus.Unsubscribe<OnShutdownEvent>(OnShutdown);
			_eventBus.Unsubscribe<OnDisconnectedFromServerEvent>(OnDisconnectedFromServer);
			_eventBus.Unsubscribe<OnConnectFailedEvent>(OnConnectFailed);
			_eventBus.Unsubscribe<OnFailedStartGameEvent>(OnFailedStartGame);
			_gameplayEventBus.Unsubscribe<OnItemGrabbedStateChangeEvent>(OnItemInteraction);
			_gameplayEventBus.Unsubscribe<OnGameplaySceneLoadedEvent>(OnGameplaySceneLoaded);
			_gameplayEventBus.Unsubscribe<OnSessionQuotaChangedEvent>(OnSessionQuotaChanged);
			_gameplayEventBus.Unsubscribe<OnPlayerDiedGameplayEvent>(OnPlayerDied);
			_gameplayEventBus.Unsubscribe<OnPlayerResurrectedGameplayEvent>(OnPlayerResurrected);
			_gameplayEventBus.Unsubscribe<OnPlayerTemporaryRagdollStartedGameplayEvent>(OnTemporaryRagdollStarted);
			_gameplayEventBus.Unsubscribe<OnPlayerTemporaryRagdollRecoveredGameplayEvent>(OnTemporaryRagdollRecovered);
			_gameplayEventBus.Unsubscribe<OnPlayerTemporaryRagdollStuckGameplayEvent>(OnTemporaryRagdollStuck);
			_gameplayEventBus.Unsubscribe<OnEnemyDespawnedGameplayEvent>(OnEnemyDespawned);
			_gameplayEventBus.Unsubscribe<OnPlayerGrabDiagnosticGameplayEvent>(OnPlayerGrabDiagnostic);
			_gameplayEventBus.Unsubscribe<OnLineArmGrabNullResolveGameplayEvent>(OnLineArmGrabNullResolve);
		}

		private void OnSuccessfullyStartGame(OnSuccessfullyStartGameEvent networkEvent)
		{
			_telemetryService.EnterLobbyPhase(networkEvent.SessionName ?? string.Empty);
			if (networkEvent.SessionName != null)
			{
				Dictionary<string, string> extras = new Dictionary<string, string>
				{
					["started_as_host"] = networkEvent.StartedAsHost.ToString(),
					["session_name"] = networkEvent.SessionName,
					["session_region"] = networkEvent.SessionRegion,
					["player_handle"] = _playerProfileModel.PlayerName,
					["app_version"] = Application.version,
					["git_commit_short"] = GitRevision.CommitHashShort,
					["debug"] = Debug.isDebugBuild.ToString()
				};
				_telemetryService.RecordEvent("session_created", extras);
			}
			EnsureNetworkObjectBandwidthCollectorAttached(_multiplayerModel.NetworkRunner);
		}

		private void OnStartGamePhase(OnStartGamePhaseEvent networkEvent)
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			string sessionId = ResolveSessionId(networkRunner);
			string sessionPlayerId = ResolveLocalPlayerId(networkRunner);
			_telemetryService.EnterInSessionPhase(sessionId, sessionPlayerId);
			Dictionary<string, string> dictionary = new Dictionary<string, string>
			{
				["level"] = networkEvent.LevelName ?? string.Empty,
				["player_count"] = networkEvent.PlayerCount.ToString(),
				["player_handle"] = _playerProfileModel.PlayerName,
				["app_version"] = Application.version,
				["git_commit_short"] = GitRevision.CommitHashShort
			};
			AddSteamIdentity(dictionary);
			if (networkRunner != null && networkRunner.IsRunning)
			{
				try
				{
					dictionary["Static_Runner_TickRate"] = networkRunner.TickRate.ToString();
					dictionary["Static_Runner_IsClient"] = networkRunner.IsClient.ToString();
					dictionary["Static_Runner_IsServer"] = networkRunner.IsServer.ToString();
					dictionary["Static_Runner_IsSharedModeMasterClient"] = networkRunner.IsSharedModeMasterClient.ToString();
					dictionary["Static_Runner_GameMode"] = networkRunner.GameMode.ToString();
				}
				catch (Exception)
				{
				}
				try
				{
					SessionInfo sessionInfo = networkRunner.SessionInfo;
					if (sessionInfo.IsValid)
					{
						dictionary["Static_Session_MaxPlayers"] = sessionInfo.MaxPlayers.ToString();
						dictionary["Static_Session_Region"] = sessionInfo.Region ?? string.Empty;
					}
				}
				catch (Exception)
				{
				}
			}
			_telemetryService.RecordEvent("session_started", dictionary);
			EnsureNetworkObjectBandwidthCollectorAttached(networkRunner);
		}

		private void OnPlayerJoined(OnPlayerJoinedEvent e)
		{
			NetworkRunner runner = e.Runner;
			PlayerRef player = e.Player;
			bool flag = runner.LocalPlayer == player;
			_telemetryService.RecordEvent("player_joined", new Dictionary<string, string>
			{
				["subject_player"] = player.PlayerId.ToString(),
				["is_local"] = flag.ToString()
			});
		}

		private void OnPlayerLeft(OnPlayerLeftEvent e)
		{
			NetworkRunner runner = e.Runner;
			PlayerRef player = e.Player;
			bool flag = runner.LocalPlayer == player;
			_telemetryService.RecordEvent("player_left", new Dictionary<string, string>
			{
				["subject_player"] = player.PlayerId.ToString(),
				["is_local"] = flag.ToString()
			});
		}

		private void OnShutdown(OnShutdownEvent e)
		{
			NetworkRunner runner = e.Runner;
			Dictionary<string, string> extras = new Dictionary<string, string> { ["shutdown_reason"] = e.Reason.ToString() };
			if (runner != null && runner.IsRunning && runner.IsSharedModeMasterClient)
			{
				_telemetryService.RecordEvent("session_shutdown_host", extras);
			}
			else
			{
				_telemetryService.RecordEvent("session_shutdown_client", extras);
			}
			RemoveNetworkObjectBandwidthCollector(runner);
			_telemetryService.EnterInactivePhase();
		}

		private void OnDisconnectedFromServer(OnDisconnectedFromServerEvent e)
		{
			_telemetryService.RecordEvent("disconnected_from_server", new Dictionary<string, string> { ["reason"] = e.Reason.ToString() });
			RemoveNetworkObjectBandwidthCollector(_multiplayerModel.NetworkRunner);
			_telemetryService.EnterPreSessionPhase();
		}

		private void OnConnectFailed(OnConnectFailedEvent e)
		{
			_telemetryService.RecordEvent("connect_failed", new Dictionary<string, string>
			{
				["reason"] = e.Reason.ToString(),
				["remote_address"] = e.RemoteAddress.ToString()
			});
			_telemetryService.EnterPreSessionPhase();
		}

		private void OnFailedStartGame(OnFailedStartGameEvent e)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>
			{
				["shutdown_reason"] = e.ShutdownReason.ToString(),
				["error_message"] = e.ErrorMessage ?? string.Empty
			};
			if (!string.IsNullOrEmpty(e.StackTrace))
			{
				dictionary["stack_trace"] = TruncateForTelemetry(e.StackTrace, 8000);
			}
			_telemetryService.RecordEvent("start_game_failed", dictionary);
			_telemetryService.EnterPreSessionPhase();
		}

		private void OnItemInteraction(OnItemGrabbedStateChangeEvent stateChangeEvent)
		{
			if (stateChangeEvent.IsLocal)
			{
				_telemetryService.RecordItemInteractionEvent(stateChangeEvent.ItemType);
			}
		}

		private void OnGameplaySceneLoaded(OnGameplaySceneLoadedEvent e)
		{
			_telemetryService.RecordEvent("gameplay_scene_loaded", new Dictionary<string, string>
			{
				["loaded_scene"] = e.LoadedScenePathOrName ?? string.Empty,
				["active_scene"] = e.ActiveScenePathOrName ?? string.Empty
			});
		}

		private void OnSessionQuotaChanged(OnSessionQuotaChangedEvent e)
		{
			_telemetryService.RecordEvent("session_gold_target_changed", new Dictionary<string, string>
			{
				["current_value"] = e.CurrentValue.ToString(CultureInfo.InvariantCulture),
				["target_value"] = e.TargetValue.ToString(CultureInfo.InvariantCulture)
			});
		}

		private void OnPlayerDied(OnPlayerDiedGameplayEvent e)
		{
			if (e.IsLocal)
			{
				_telemetryService.RecordEvent("player_died", new Dictionary<string, string> { ["player_id"] = e.PlayerId.ToString() });
			}
		}

		private void OnPlayerResurrected(OnPlayerResurrectedGameplayEvent e)
		{
			if (e.IsLocal)
			{
				_telemetryService.RecordEvent("player_resurrected", new Dictionary<string, string> { ["player_id"] = e.PlayerId.ToString() });
			}
		}

		private void OnTemporaryRagdollStarted(OnPlayerTemporaryRagdollStartedGameplayEvent e)
		{
			if (e.IsLocal)
			{
				_telemetryService.RecordEvent("temporary_ragdoll_started", new Dictionary<string, string>
				{
					["player_id"] = e.PlayerId.ToString(),
					["reasons"] = e.Reasons ?? string.Empty
				});
			}
		}

		private void OnTemporaryRagdollRecovered(OnPlayerTemporaryRagdollRecoveredGameplayEvent e)
		{
			if (e.IsLocal)
			{
				_telemetryService.RecordEvent("temporary_ragdoll_recovered", new Dictionary<string, string>
				{
					["player_id"] = e.PlayerId.ToString(),
					["duration_seconds"] = e.DurationSeconds.ToString("0.###", CultureInfo.InvariantCulture)
				});
			}
		}

		private void OnTemporaryRagdollStuck(OnPlayerTemporaryRagdollStuckGameplayEvent e)
		{
			if (e.IsLocal)
			{
				_telemetryService.RecordEvent("temporary_ragdoll_stuck", new Dictionary<string, string>
				{
					["player_id"] = e.PlayerId.ToString(),
					["reasons"] = e.Reasons ?? string.Empty,
					["duration_seconds"] = e.DurationSeconds.ToString("0.###", CultureInfo.InvariantCulture)
				});
			}
		}

		private void OnEnemyDespawned(OnEnemyDespawnedGameplayEvent gameplayEvent)
		{
			_telemetryService.RecordEvent("enemy_despawned", new Dictionary<string, string>
			{
				["enemy_kind"] = gameplayEvent.EnemyKind ?? string.Empty,
				["network_object_id"] = gameplayEvent.NetworkObjectId ?? string.Empty
			});
		}

		private void OnPlayerGrabDiagnostic(OnPlayerGrabDiagnosticGameplayEvent gameplayEvent)
		{
			_telemetryService.RecordEvent("player_grab_diagnostic", gameplayEvent.Extras);
		}

		private void OnLineArmGrabNullResolve(OnLineArmGrabNullResolveGameplayEvent e)
		{
			_telemetryService.RecordEvent("linearm_grab_null_resolve", new Dictionary<string, string>
			{
				["grabber_player_id"] = e.GrabberPlayerId.ToString(),
				["observer_player_id"] = e.ObserverPlayerId.ToString(),
				["grabbed_object_raw_id"] = e.GrabbedObjectRawId.ToString()
			});
		}

		private void AddSteamIdentity(Dictionary<string, string> extras)
		{
			if (!_steamModel.IsSteamInitialized)
			{
				return;
			}
			try
			{
				extras["steam_id"] = SteamUser.GetSteamID().m_SteamID.ToString(CultureInfo.InvariantCulture);
				extras["steam_nickname"] = SteamFriends.GetPersonaName() ?? string.Empty;
			}
			catch (Exception)
			{
			}
		}

		private static string TruncateForTelemetry(string value, int maxLength)
		{
			if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
			{
				return value;
			}
			return value.Substring(0, maxLength);
		}

		private static string ResolveSessionId(NetworkRunner runner)
		{
			if (runner == null || !runner.IsRunning)
			{
				return string.Empty;
			}
			try
			{
				SessionInfo sessionInfo = runner.SessionInfo;
				if (sessionInfo.IsValid && !string.IsNullOrEmpty(sessionInfo.Name))
				{
					return sessionInfo.Name;
				}
			}
			catch (Exception)
			{
			}
			return string.Empty;
		}

		private static string ResolveLocalPlayerId(NetworkRunner runner)
		{
			if (runner == null || !runner.IsRunning)
			{
				return string.Empty;
			}
			try
			{
				return runner.LocalPlayer.PlayerId.ToString();
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}

		private void EnsureNetworkObjectBandwidthCollectorAttached(NetworkRunner runner)
		{
			if (_telemetryService.Configuration.CollectNetworkObjectBandwidth && !(runner == null) && runner.IsRunning)
			{
				TelemetryNetworkObjectBandwidthCollector telemetryNetworkObjectBandwidthCollector = runner.GetComponent<TelemetryNetworkObjectBandwidthCollector>();
				if (telemetryNetworkObjectBandwidthCollector == null)
				{
					telemetryNetworkObjectBandwidthCollector = _diContainer.InstantiateComponent<TelemetryNetworkObjectBandwidthCollector>(runner.gameObject);
				}
				telemetryNetworkObjectBandwidthCollector.RegisterWithRunner(runner);
			}
		}

		private void RemoveNetworkObjectBandwidthCollector(NetworkRunner runner)
		{
			if (!(runner == null))
			{
				TelemetryNetworkObjectBandwidthCollector component = runner.GetComponent<TelemetryNetworkObjectBandwidthCollector>();
				if (!(component == null))
				{
					component.UnregisterFromRunner(runner);
					UnityEngine.Object.Destroy(component);
				}
			}
		}
	}
}
