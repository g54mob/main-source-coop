using System;
using NetworkServices.NetworkEvents;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using Zenject;

namespace Features.MultiplayerSessionServices.Scripts.NetworkMasterClientTracking
{
	public class NetworkMasterClientTrackerSpawnSystem : IInitializable, IDisposable
	{
		private readonly INetworkMasterClientTrackerFactory _networkMasterClientTrackerFactory;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly NetworkRunnerEventBus _eventBus;

		public NetworkMasterClientTrackerSpawnSystem(INetworkMasterClientTrackerFactory networkMasterClientTrackerFactory, MultiplayerModel multiplayerModel, NetworkRunnerEventBus eventBus)
		{
			_networkMasterClientTrackerFactory = networkMasterClientTrackerFactory;
			_multiplayerModel = multiplayerModel;
			_eventBus = eventBus;
		}

		public void Initialize()
		{
			_eventBus.Subscribe<OnSuccessfullyStartGameEvent>(SpawnMasterClientTracker);
			_eventBus.Subscribe<OnShutdownEvent>(DespawnMasterClientTracker);
		}

		public void Dispose()
		{
			_eventBus.Unsubscribe<OnSuccessfullyStartGameEvent>(SpawnMasterClientTracker);
			_eventBus.Unsubscribe<OnShutdownEvent>(DespawnMasterClientTracker);
		}

		private void SpawnMasterClientTracker(OnSuccessfullyStartGameEvent successfullyStartGameEvent)
		{
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient && _multiplayerModel.NetworkMasterClientTracker == null)
			{
				_networkMasterClientTrackerFactory.CreateNetworkMasterClientTracker();
			}
		}

		private void DespawnMasterClientTracker(OnShutdownEvent shutdownEvent)
		{
			if (!(_multiplayerModel.NetworkMasterClientTracker == null))
			{
				_multiplayerModel.NetworkMasterClientTracker.Object.DespawnHierarchy();
				UnityEngine.Object.Destroy(_multiplayerModel.NetworkMasterClientTracker.gameObject);
			}
		}
	}
}
