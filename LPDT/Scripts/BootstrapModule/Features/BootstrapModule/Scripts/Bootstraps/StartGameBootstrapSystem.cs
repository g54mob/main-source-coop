using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using Global.StateMachinesModule.Scripts;
using NetworkServices.NetworkEvents;
using RSG.SharedData.DataHolder;
using UnityEngine.SceneManagement;
using Zenject;

namespace Features.BootstrapModule.Scripts.Bootstraps
{
	public class StartGameBootstrapSystem : IInitializable, IDisposable
	{
		private const int MaxRoutingWaitFrames = 300;

		private static readonly HashSet<string> HubSceneNames = new HashSet<string> { "LobbyScene", "MenuScene", "GlobalScene", "BootstrapScene" };

		private readonly NetworkRunnerEventBus _eventBus;

		private readonly GameFlowStateMachine _gameFlowStateMachine;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly IMultiplayerService _multiplayerService;

		public StartGameBootstrapSystem(NetworkRunnerEventBus eventBus, GameFlowStateMachine gameFlowStateMachine, MultiplayerModel multiplayerModel, IMultiplayerService multiplayerService)
		{
			_eventBus = eventBus;
			_gameFlowStateMachine = gameFlowStateMachine;
			_multiplayerModel = multiplayerModel;
			_multiplayerService = multiplayerService;
		}

		public void Initialize()
		{
			_eventBus.Subscribe<OnSuccessfullyStartGameEvent>(OnStartGameHandler);
		}

		public void Dispose()
		{
			_eventBus.Unsubscribe<OnSuccessfullyStartGameEvent>(OnStartGameHandler);
		}

		private async void OnStartGameHandler(OnSuccessfullyStartGameEvent eventData)
		{
			GameFlowState state = await ResolveTargetStateAsync(_multiplayerModel.NetworkRunner);
			_gameFlowStateMachine.EnterState(state);
		}

		private async UniTask<GameFlowState> ResolveTargetStateAsync(NetworkRunner runner)
		{
			bool consultRecoveryPhase = _multiplayerModel.IsReconnecting;
			if (consultRecoveryPhase)
			{
				for (int i = 0; i < 300; i++)
				{
					if (IsSessionInProgress(runner, consultRecoveryPhase))
					{
						return PrepareSessionReconnectTarget();
					}
					await UniTask.Yield();
				}
			}
			if (IsSessionInProgress(runner, consultRecoveryPhase))
			{
				return PrepareSessionReconnectTarget();
			}
			return PrepareFreshSessionTarget();
		}

		private bool IsSessionInProgress(NetworkRunner runner, bool consultRecoveryPhase)
		{
			if (TryGetIsSessionStarted(runner))
			{
				return true;
			}
			if (IsCoreOrGameplaySceneLoaded(out var loadedSceneName))
			{
				return true;
			}
			if (HasActiveGameplayNetworkObjects(runner, out loadedSceneName))
			{
				return true;
			}
			if (!consultRecoveryPhase)
			{
				return false;
			}
			return PlayerSessionPrefs.GetRecoveryPhase() == SessionRecoveryPhase.InGame;
		}

		private static GameFlowState PrepareSessionReconnectTarget()
		{
			PlayerSessionPrefs.SetRecoveryPhase(SessionRecoveryPhase.InGame);
			return GameFlowState.SessionGameState;
		}

		private GameFlowState PrepareFreshSessionTarget()
		{
			_multiplayerModel.IsReconnecting = false;
			PlayerSessionPrefs.SetRecoveryPhase(SessionRecoveryPhase.InGame);
			return GameFlowState.SessionGameState;
		}

		private bool TryGetIsSessionStarted(NetworkRunner runner)
		{
			if (runner?.SessionInfo.Properties == null)
			{
				return false;
			}
			try
			{
				return _multiplayerService.GetSessionProperty<bool>(runner, SessionPropertyType.IsSessionStarted);
			}
			catch (Exception)
			{
				return false;
			}
		}

		private static bool IsCoreOrGameplaySceneLoaded(out string loadedSceneName)
		{
			loadedSceneName = null;
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene sceneAt = SceneManager.GetSceneAt(i);
				if (sceneAt.isLoaded)
				{
					string name = sceneAt.name;
					if (name == "SessionCoreScene")
					{
						loadedSceneName = name;
						return true;
					}
					if (!HubSceneNames.Contains(name) && Address.Scenes.AllAddressablesInGroup.Contains(name))
					{
						loadedSceneName = name;
						return true;
					}
				}
			}
			return false;
		}

		private static bool HasActiveGameplayNetworkObjects(NetworkRunner runner, out string summary)
		{
			summary = string.Empty;
			if (runner == null || !runner.IsRunning)
			{
				return false;
			}
			int num = 0;
			int num2 = 0;
			string text = null;
			foreach (NetworkObject allNetworkObject in runner.GetAllNetworkObjects())
			{
				if (allNetworkObject == null || !allNetworkObject.IsValid)
				{
					continue;
				}
				if (allNetworkObject.GetComponent<PlayerInitializer>() != null)
				{
					num++;
					continue;
				}
				string name = allNetworkObject.gameObject.scene.name;
				if (name == "SessionCoreScene" || IsGameplayLevelSceneName(name))
				{
					num2++;
					if (text == null)
					{
						text = name;
					}
				}
			}
			if (num > 0)
			{
				summary = $"playerAvatars={num}";
				return true;
			}
			if (num2 > 0)
			{
				summary = $"gameplaySceneObjects={num2}, scene='{text}'";
				return true;
			}
			return false;
		}

		private static bool IsGameplayLevelSceneName(string sceneName)
		{
			if (HubSceneNames.Contains(sceneName))
			{
				return false;
			}
			return Address.Scenes.AllAddressablesInGroup.Contains(sceneName);
		}
	}
}
