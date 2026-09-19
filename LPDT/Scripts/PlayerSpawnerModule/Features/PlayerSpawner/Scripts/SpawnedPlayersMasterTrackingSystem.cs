using System;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using NetworkServices.NetworkEvents;
using Zenject;

namespace Features.PlayerSpawner.Scripts
{
	public class SpawnedPlayersMasterTrackingSystem : IInitializable, IDisposable, IMasterPlayerAvatarRegistrar
	{
		private readonly MultiplayerModel _multiplayerModel;

		private readonly SpawnedPlayersModel _spawnedPlayersModel;

		private readonly NetworkRunnerEventBus _eventBus;

		public SpawnedPlayersMasterTrackingSystem(MultiplayerModel multiplayerModel, SpawnedPlayersModel spawnedPlayersModel, NetworkRunnerEventBus eventBus)
		{
			_multiplayerModel = multiplayerModel;
			_spawnedPlayersModel = spawnedPlayersModel;
			_eventBus = eventBus;
		}

		public void Initialize()
		{
			_eventBus.Subscribe<OnPlayerJoinedEvent>(OnPlayerJoined);
			RegisterAllKnownAvatars();
		}

		public void Dispose()
		{
			_eventBus.Unsubscribe<OnPlayerJoinedEvent>(OnPlayerJoined);
		}

		private void OnPlayerJoined(OnPlayerJoinedEvent _)
		{
			RegisterAllKnownAvatars();
		}

		public void RegisterAllKnownAvatars()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning || !networkRunner.IsSharedModeMasterClient)
			{
				return;
			}
			foreach (NetworkObject allNetworkObject in networkRunner.GetAllNetworkObjects())
			{
				if (!(allNetworkObject == null) && allNetworkObject.IsValid && !(allNetworkObject.GetComponent<PlayerInitializer>() == null))
				{
					PlayerRef inputAuthority = allNetworkObject.InputAuthority;
					if (!(inputAuthority == PlayerRef.None))
					{
						_spawnedPlayersModel.RegisterPlayer(inputAuthority, allNetworkObject);
					}
				}
			}
		}
	}
}
