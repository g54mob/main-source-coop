using System;
using Features.LevelModule.Scripts.RoomVariations;
using NetworkServices.NetworkEvents;
using Zenject;

namespace Features.LevelModule.Scripts
{
	public class SpawnedRoomsShutdownResetSystem : IInitializable, IDisposable
	{
		private readonly NetworkRunnerEventBus _networkRunnerEventBus;

		private readonly SpawnedRoomsModel _spawnedRoomsModel;

		public SpawnedRoomsShutdownResetSystem(NetworkRunnerEventBus networkRunnerEventBus, SpawnedRoomsModel spawnedRoomsModel)
		{
			_networkRunnerEventBus = networkRunnerEventBus;
			_spawnedRoomsModel = spawnedRoomsModel;
		}

		public void Initialize()
		{
			_networkRunnerEventBus.Subscribe<OnShutdownEvent>(OnShutdown);
		}

		public void Dispose()
		{
			_networkRunnerEventBus.Unsubscribe<OnShutdownEvent>(OnShutdown);
		}

		private void OnShutdown(OnShutdownEvent _)
		{
			_spawnedRoomsModel.Reset();
		}
	}
}
