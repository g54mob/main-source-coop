using Features.ItemSpawnerModule;
using Features.MultiplayerSessionServices.Scripts;
using UnityEngine;
using Zenject;

namespace Features.PlayersStatisticsModule.Scripts
{
	public class StatisticsSpawnedItemRegistrar : MonoBehaviour
	{
		[SerializeField]
		private ItemSpawnerBase _spawner;

		[SerializeField]
		private StatisticsSpawnedItemType _itemType;

		private LevelPlayersGameStatisticsModel _levelPlayersGameStatisticsModel;

		private MultiplayerModel _multiplayerModel;

		[Inject]
		private void InjectDependencies(LevelPlayersGameStatisticsModel levelPlayersGameStatisticsModel, MultiplayerModel multiplayerModel)
		{
			_levelPlayersGameStatisticsModel = levelPlayersGameStatisticsModel;
			_multiplayerModel = multiplayerModel;
		}

		private void OnEnable()
		{
			if (_spawner != null)
			{
				_spawner.OnItemSpawned += OnItemSpawned;
			}
		}

		private void OnDisable()
		{
			if (_spawner != null)
			{
				_spawner.OnItemSpawned -= OnItemSpawned;
			}
		}

		private void OnItemSpawned(ItemSpawnerBase itemSpawnerBase)
		{
			if (_itemType != StatisticsSpawnedItemType.None && !(_multiplayerModel.NetworkRunner == null) && _multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				_levelPlayersGameStatisticsModel.RegisterSpawnedItem(_itemType);
			}
		}
	}
}
