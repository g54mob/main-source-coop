using System;
using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Zenject;

namespace Features.LevelObjectSpawnModule.Scripts
{
	public class LevelObjectsSpawnSystem : IInitializable, IDisposable
	{
		private readonly ILevelObjectsSpawnService _levelObjectsSpawnService;

		private readonly LevelModel _levelModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly BeforeLevelChangeNetworkEvent _beforeLevelChangeNetworkEvent;

		public LevelObjectsSpawnSystem(MultiplayerModel multiplayerModel, ILevelObjectsSpawnService levelObjectsSpawnService, LevelModel levelModel, BeforeLevelChangeNetworkEvent beforeLevelChangeNetworkEvent)
		{
			_multiplayerModel = multiplayerModel;
			_levelObjectsSpawnService = levelObjectsSpawnService;
			_levelModel = levelModel;
			_beforeLevelChangeNetworkEvent = beforeLevelChangeNetworkEvent;
		}

		public void Initialize()
		{
			_beforeLevelChangeNetworkEvent.OnNetworkEventSend += InvalidatePreparedSpawns;
			_levelModel.OnBeforeLevelLoaded += PrepareSpawnItems;
			_levelModel.OnLevelLoaded += SpawnLevelObjects;
		}

		public void Dispose()
		{
			_beforeLevelChangeNetworkEvent.OnNetworkEventSend -= InvalidatePreparedSpawns;
			_levelModel.OnBeforeLevelLoaded -= PrepareSpawnItems;
			_levelModel.OnLevelLoaded -= SpawnLevelObjects;
		}

		private void InvalidatePreparedSpawns(BeforeLevelChangeNetworkEvent evt)
		{
			_levelObjectsSpawnService.InvalidatePreparedSpawns();
		}

		private void SpawnLevelObjects(LevelType levelType)
		{
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				_levelObjectsSpawnService.SpawnPreparedItems();
			}
		}

		private void PrepareSpawnItems(LevelType levelType)
		{
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				_levelObjectsSpawnService.PrepareSpawnItems(levelType);
			}
		}
	}
}
