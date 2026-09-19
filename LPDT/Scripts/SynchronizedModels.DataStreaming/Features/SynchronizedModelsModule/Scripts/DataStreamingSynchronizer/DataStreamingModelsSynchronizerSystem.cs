using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fusion;
using NetworkServices.NetworkEvents;
using Zenject;

namespace Features.SynchronizedModelsModule.Scripts.DataStreamingSynchronizer
{
	public class DataStreamingModelsSynchronizerSystem : IInitializable, IDisposable
	{
		private readonly NetworkRunnerEventBus _eventBus;

		private readonly List<IDataStreamSynchronizable> _dataStreamSynchronizables;

		private readonly List<ICustomDataStreamSynchronizable> _customSynchronizedModels;

		public DataStreamingModelsSynchronizerSystem(NetworkRunnerEventBus eventBus, List<IDataStreamSynchronizable> dataStreamSynchronizables, List<ICustomDataStreamSynchronizable> customSynchronizedModels)
		{
			_eventBus = eventBus;
			_dataStreamSynchronizables = dataStreamSynchronizables;
			_customSynchronizedModels = customSynchronizedModels;
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
				SynchronizePlayersDelayed(playerJoinedEvent).Forget();
			}
		}

		private async UniTaskVoid SynchronizePlayersDelayed(OnPlayerJoinedEvent playerJoinedEvent)
		{
			NetworkRunner runner = playerJoinedEvent.Runner;
			for (int i = 0; i < 3; i++)
			{
				await UniTask.Yield(PlayerLoopTiming.Update);
				if (runner == null || !runner.IsRunning || !runner.IsSharedModeMasterClient)
				{
					return;
				}
			}
			SynchronizePlayers();
		}

		private void SynchronizePlayers()
		{
			foreach (IDataStreamSynchronizable dataStreamSynchronizable in _dataStreamSynchronizables)
			{
				if (dataStreamSynchronizable.IsNeedToSynchronizeOnSpawn)
				{
					dataStreamSynchronizable.Synchronize(isWithTrigggerEvent: true);
				}
			}
		}

		private void OnDisconnectedFromServer(OnShutdownEvent onShutdownEvent)
		{
			foreach (IDataStreamSynchronizable dataStreamSynchronizable in _dataStreamSynchronizables)
			{
				dataStreamSynchronizable.OnDisconnect();
			}
			foreach (ICustomDataStreamSynchronizable customSynchronizedModel in _customSynchronizedModels)
			{
				customSynchronizedModel.OnDisconnect();
			}
		}
	}
}
