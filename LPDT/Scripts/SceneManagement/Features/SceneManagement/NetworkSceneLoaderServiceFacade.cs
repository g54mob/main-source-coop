using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Features.GameCycle.Scripts.SessionCleanup;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using GameplayEvents;
using NetworkServices.ObjectsProvider;
using RSG.Muffin.SceneLoaderSubmodule.SceneLoaderModule.Scripts;
using UnityEngine.SceneManagement;
using Zenject;

namespace Features.SceneManagement
{
	public class NetworkSceneLoaderServiceFacade : INetworkSceneLoaderServiceFacade, IInitializable, IDisposable
	{
		private const float CLEAN_UP_TIMEOUT = 15f;

		private readonly ISceneLoaderService _sceneLoaderService;

		private readonly INetworkSceneLoader _networkSceneLoader;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly DespawnNetworkObjectsNetworkEvent _despawnNetworkObjectsNetworkEvent;

		private readonly PeerDespawnedObjectsNetworkEvent _peerDespawnedObjectsNetworkEvent;

		private readonly GameplayEventBus _gameplayEventBus;

		private int _targetCleanedPeersCount;

		private int _cleanedPeersCount;

		public NetworkSceneLoaderServiceFacade(ISceneLoaderService sceneLoaderService, INetworkSceneLoader networkSceneLoader, MultiplayerModel multiplayerModel, DespawnNetworkObjectsNetworkEvent despawnNetworkObjectsNetworkEvent, PeerDespawnedObjectsNetworkEvent peerDespawnedObjectsNetworkEvent, GameplayEventBus gameplayEventBus)
		{
			_sceneLoaderService = sceneLoaderService;
			_networkSceneLoader = networkSceneLoader;
			_multiplayerModel = multiplayerModel;
			_despawnNetworkObjectsNetworkEvent = despawnNetworkObjectsNetworkEvent;
			_peerDespawnedObjectsNetworkEvent = peerDespawnedObjectsNetworkEvent;
			_gameplayEventBus = gameplayEventBus;
		}

		public void Initialize()
		{
			_despawnNetworkObjectsNetworkEvent.OnNetworkEventSend += DespawnNetworkObjects;
			_peerDespawnedObjectsNetworkEvent.OnNetworkEventSend += IncreaseCleanedPeersCount;
		}

		public void Dispose()
		{
			_despawnNetworkObjectsNetworkEvent.OnNetworkEventSend -= DespawnNetworkObjects;
			_peerDespawnedObjectsNetworkEvent.OnNetworkEventSend -= IncreaseCleanedPeersCount;
		}

		public async UniTask LoadSceneAsync(string sceneToLoad, NetworkSceneLoadingFlags networkSceneLoadingFlags)
		{
			ParsedSceneLoadingFlags sceneLoadingFlags = ParseSceneLoadingFlags(networkSceneLoadingFlags);
			await TryCleanUpNetworkObjects(sceneLoadingFlags.UnloadRedundant ? GetScenesToUnload() : null, sceneLoadingFlags.SyncLoading, sceneLoadingFlags.CleanUpObjectPool);
			if (!sceneLoadingFlags.SyncLoading)
			{
				await _sceneLoaderService.LoadSceneAsync(sceneToLoad, sceneLoadingFlags.UnloadRedundant);
			}
			else
			{
				await _networkSceneLoader.LoadScene(sceneToLoad, sceneLoadingFlags.UnloadRedundant);
			}
			PublishGameplaySceneLoaded(sceneToLoad, sceneToLoad);
		}

		public async UniTask LoadScenesAsync(List<string> scenesToLoad, string activeScene, NetworkSceneLoadingFlags networkSceneLoadingFlags)
		{
			ParsedSceneLoadingFlags sceneLoadingFlags = ParseSceneLoadingFlags(networkSceneLoadingFlags);
			await TryCleanUpNetworkObjects(sceneLoadingFlags.UnloadRedundant ? GetScenesToUnload() : null, sceneLoadingFlags.SyncLoading, sceneLoadingFlags.CleanUpObjectPool);
			if (!sceneLoadingFlags.SyncLoading)
			{
				await _sceneLoaderService.LoadScenesAsync(scenesToLoad, activeScene, sceneLoadingFlags.UnloadRedundant);
			}
			else
			{
				await _networkSceneLoader.LoadScenes(scenesToLoad, activeScene, sceneLoadingFlags.UnloadRedundant);
			}
			string loadedScenePathOrName = ((string.IsNullOrEmpty(activeScene) && scenesToLoad.Count > 0) ? scenesToLoad[0] : activeScene);
			PublishGameplaySceneLoaded(loadedScenePathOrName, activeScene);
		}

		public async UniTask UnloadSceneAsync(string sceneToUnload, NetworkSceneLoadingFlags networkSceneLoadingFlags)
		{
			ParsedSceneLoadingFlags sceneLoadingFlags = ParseSceneLoadingFlags(networkSceneLoadingFlags);
			await TryCleanUpNetworkObjects(new List<string> { sceneToUnload }, sceneLoadingFlags.SyncLoading, sceneLoadingFlags.CleanUpObjectPool);
			if (sceneLoadingFlags.SyncLoading)
			{
				await _networkSceneLoader.UnloadScene(sceneToUnload);
			}
			else
			{
				await _sceneLoaderService.UnloadSceneAsync(sceneToUnload);
			}
		}

		public async UniTask UnloadScenesAsync(List<string> scenesToUnload, NetworkSceneLoadingFlags networkSceneLoadingFlags)
		{
			ParsedSceneLoadingFlags sceneLoadingFlags = ParseSceneLoadingFlags(networkSceneLoadingFlags);
			await TryCleanUpNetworkObjects(scenesToUnload, sceneLoadingFlags.SyncLoading, sceneLoadingFlags.CleanUpObjectPool);
			if (sceneLoadingFlags.SyncLoading)
			{
				await _networkSceneLoader.UnloadScenes(scenesToUnload);
			}
			else
			{
				await _sceneLoaderService.UnloadScenesAsync(scenesToUnload);
			}
		}

		private async UniTask TryCleanUpNetworkObjects(List<string> objectsToDespawnScenes, bool syncLoad, bool cleanUpObjectPool)
		{
			if (objectsToDespawnScenes != null)
			{
				_cleanedPeersCount = 0;
				if (syncLoad)
				{
					_targetCleanedPeersCount = _multiplayerModel.NetworkRunner.ActivePlayers.Count();
					_despawnNetworkObjectsNetworkEvent.SendEvent(objectsToDespawnScenes, cleanUpObjectPool);
				}
				else
				{
					_targetCleanedPeersCount = 1;
					DespawnNetworkObjectsOnUnloadScenes(objectsToDespawnScenes, cleanUpObjectPool).Forget();
				}
				await UniTask.WaitUntil(() => _cleanedPeersCount >= _targetCleanedPeersCount, PlayerLoopTiming.Update, _multiplayerModel.NetworkRunner.destroyCancellationToken).TimeoutWithoutException(TimeSpan.FromSeconds(15.0));
			}
		}

		private async UniTask DespawnNetworkObjectsOnUnloadScenes(List<string> scenesToUnload, bool cleanUpObjectPool)
		{
			IForbiddableObjectProvider forbiddableObjectProvider = _multiplayerModel.NetworkRunner.ObjectProvider as IForbiddableObjectProvider;
			forbiddableObjectProvider?.ForbidAcquireInstance();
			foreach (NetworkObject allNetworkObject in _multiplayerModel.NetworkRunner.GetAllNetworkObjects())
			{
				if (allNetworkObject.IsValid && allNetworkObject.HasStateAuthority && scenesToUnload.Contains(allNetworkObject.gameObject.scene.name) && !allNetworkObject.HasParentNetworkObject())
				{
					allNetworkObject.DespawnHierarchy();
				}
			}
			await UniTask.WaitUntil(() => GetNetworkObjectsLeftToCleanup(scenesToUnload) == 0, PlayerLoopTiming.Update, _multiplayerModel.NetworkRunner.destroyCancellationToken).TimeoutWithoutException(TimeSpan.FromSeconds(15.0));
			if (cleanUpObjectPool)
			{
				_multiplayerModel.NetworkRunner.GetComponentInChildren<PoolObjectProvider>()?.CleanPoolsForScenes(scenesToUnload);
			}
			forbiddableObjectProvider?.AllowAcquireInstance();
			_peerDespawnedObjectsNetworkEvent.SendEvent();
		}

		private int GetNetworkObjectsLeftToCleanup(List<string> scenes)
		{
			return _multiplayerModel.NetworkRunner.GetAllNetworkObjects().Count((NetworkObject x) => scenes.Contains(x.gameObject.scene.name) && _multiplayerModel.NetworkRunner.ActivePlayers.Contains(x.StateAuthority) && !x.TryGetComponent<ISceneNotCleanableObject>(out var _));
		}

		private List<string> GetScenesToUnload()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				list.Add(SceneManager.GetSceneAt(i).name);
			}
			return list;
		}

		private void DespawnNetworkObjects(DespawnNetworkObjectsNetworkEvent despawnNetworkObjectsNetworkEvent)
		{
			DespawnNetworkObjectsOnUnloadScenes(despawnNetworkObjectsNetworkEvent.ObjectsToDespawnScenes, despawnNetworkObjectsNetworkEvent.CleanUpObjectPool).Forget();
		}

		private void IncreaseCleanedPeersCount(PeerDespawnedObjectsNetworkEvent peerDespawnedObjectsNetworkEvent)
		{
			_cleanedPeersCount++;
		}

		private void PublishGameplaySceneLoaded(string loadedScenePathOrName, string activeScenePathOrName)
		{
			if (!string.IsNullOrEmpty(loadedScenePathOrName))
			{
				string activeScenePathOrName2 = (string.IsNullOrEmpty(activeScenePathOrName) ? null : activeScenePathOrName);
				_gameplayEventBus.Publish(new OnGameplaySceneLoadedEvent(loadedScenePathOrName, activeScenePathOrName2));
			}
		}

		private bool CheckSceneLoadingFlag(NetworkSceneLoadingFlags networkSceneLoadingFlags, NetworkSceneLoadingFlags targetFlag)
		{
			return (networkSceneLoadingFlags & targetFlag) != 0;
		}

		private ParsedSceneLoadingFlags ParseSceneLoadingFlags(NetworkSceneLoadingFlags networkSceneLoadingFlags)
		{
			return new ParsedSceneLoadingFlags
			{
				SyncLoading = CheckSceneLoadingFlag(networkSceneLoadingFlags, NetworkSceneLoadingFlags.SyncLoading),
				UnloadRedundant = CheckSceneLoadingFlag(networkSceneLoadingFlags, NetworkSceneLoadingFlags.UnloadRedundant),
				CleanUpObjectPool = CheckSceneLoadingFlag(networkSceneLoadingFlags, NetworkSceneLoadingFlags.CleanUpObjectPool)
			};
		}
	}
}
