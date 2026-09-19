using System;
using System.Collections.Generic;
using NetworkServices.NetworkEvents;
using Zenject;

namespace Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer
{
	public class JsonModelsSynchronizerSystem : IInitializable, IDisposable
	{
		private readonly NetworkRunnerEventBus _eventBus;

		private readonly List<IJsonSynchronizable> _synchronizableList;

		private readonly List<ICustomJsonSynchronizable> _customSynchronizableList;

		public JsonModelsSynchronizerSystem(NetworkRunnerEventBus eventBus, List<IJsonSynchronizable> synchronizableList, List<ICustomJsonSynchronizable> customSynchronizableList)
		{
			_eventBus = eventBus;
			_synchronizableList = synchronizableList;
			_customSynchronizableList = customSynchronizableList;
		}

		public void Initialize()
		{
			_eventBus.Subscribe<OnPlayerJoinedEvent>(OnPlayerJoined);
			_eventBus.Subscribe<OnShutdownEvent>(OnDisconnectedFromServer);
		}

		public void Dispose()
		{
			_eventBus.Unsubscribe<OnPlayerJoinedEvent>(OnPlayerJoined);
			_eventBus.Unsubscribe<OnShutdownEvent>(OnDisconnectedFromServer);
		}

		private void OnPlayerJoined(OnPlayerJoinedEvent playerJoinedEvent)
		{
			if (playerJoinedEvent.Runner.IsSharedModeMasterClient)
			{
				SynchronizePlayers();
			}
		}

		private void SynchronizePlayers()
		{
			foreach (IJsonSynchronizable synchronizable in _synchronizableList)
			{
				if (synchronizable.IsNeedToSynchronizeOnSpawn)
				{
					synchronizable.SynchronizeOnStart();
				}
			}
		}

		private void OnDisconnectedFromServer(OnShutdownEvent onShutdownEvent)
		{
			foreach (IJsonSynchronizable synchronizable in _synchronizableList)
			{
				synchronizable.OnDisconnect();
			}
			foreach (ICustomJsonSynchronizable customSynchronizable in _customSynchronizableList)
			{
				customSynchronizable.OnDisconnect();
			}
		}
	}
}
