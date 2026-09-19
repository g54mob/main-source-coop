using System;
using System.Collections.Generic;
using Fusion;
using NetworkServices.NetworkEvents;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using Zenject;

namespace Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer
{
	public class JsonModelsSynchronizerSpawnSystem : IInitializable, IDisposable
	{
		private readonly NetworkRunnerEventBus _eventBus;

		private readonly ModelsSynchronizerConfiguration _modelsSynchronizerConfiguration;

		private readonly Dictionary<PlayerRef, NetworkObject> _spawnedObjects = new Dictionary<PlayerRef, NetworkObject>();

		public JsonModelsSynchronizerSpawnSystem(NetworkRunnerEventBus eventBus, ModelsSynchronizerConfiguration modelsSynchronizerConfiguration)
		{
			_eventBus = eventBus;
			_modelsSynchronizerConfiguration = modelsSynchronizerConfiguration;
		}

		public void Initialize()
		{
			_eventBus.Subscribe<OnPlayerJoinedEvent>(OnPlayerJoined);
			_eventBus.Subscribe<OnPlayerLeftEvent>(OnPlayerLeft);
			_eventBus.Subscribe<OnShutdownEvent>(OnShutdown);
		}

		public void Dispose()
		{
			_eventBus.Unsubscribe<OnPlayerJoinedEvent>(OnPlayerJoined);
			_eventBus.Unsubscribe<OnPlayerLeftEvent>(OnPlayerLeft);
			_eventBus.Unsubscribe<OnShutdownEvent>(OnShutdown);
		}

		private void OnPlayerJoined(OnPlayerJoinedEvent playerJoinedEvent)
		{
			if (playerJoinedEvent.Player == playerJoinedEvent.Runner.LocalPlayer)
			{
				NetworkObject networkObject = playerJoinedEvent.Runner.Spawn(_modelsSynchronizerConfiguration.JsonModelsSynchronizer, Vector3.zero, Quaternion.identity, playerJoinedEvent.Player);
				UnityEngine.Object.DontDestroyOnLoad(networkObject);
				_spawnedObjects.Add(playerJoinedEvent.Player, networkObject);
			}
		}

		private void OnPlayerLeft(OnPlayerLeftEvent playerLeftEvent)
		{
			if (playerLeftEvent.Player == playerLeftEvent.Runner.LocalPlayer)
			{
				NetworkObject root = _spawnedObjects[playerLeftEvent.Player];
				_spawnedObjects.Remove(playerLeftEvent.Player);
				root.DespawnHierarchy();
			}
		}

		private void OnShutdown(OnShutdownEvent onShutdownEvent)
		{
			foreach (NetworkObject value in _spawnedObjects.Values)
			{
				value.DespawnHierarchy();
				UnityEngine.Object.Destroy(value.gameObject);
			}
			_spawnedObjects.Clear();
		}
	}
}
