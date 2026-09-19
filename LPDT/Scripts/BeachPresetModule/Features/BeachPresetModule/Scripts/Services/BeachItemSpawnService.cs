using Cysharp.Threading.Tasks;
using Features.BeachPresetModule.Scripts.Core.Interfaces;
using Features.ItemsModule.Scripts;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;

namespace Features.BeachPresetModule.Scripts.Services
{
	public class BeachItemSpawnService : IBeachItemSpawnService
	{
		private readonly IItemSpawnService _itemSpawnService;

		public BeachItemSpawnService(IItemSpawnService itemSpawnService)
		{
			_itemSpawnService = itemSpawnService;
		}

		public UniTask<NetworkBehaviour> SpawnItem(NetworkBehaviour prefab, Vector3 position, Quaternion rotation, ItemData itemData, bool addRandomForce, float randomForce)
		{
			return _itemSpawnService.SpawnItem(prefab, position, rotation, itemData, addRandomForce, randomForce);
		}

		public void Despawn(NetworkBehaviour spawned)
		{
			if (!(spawned == null) && !(spawned.Object == null) && spawned.Object.IsValid && !(spawned.Runner == null))
			{
				spawned.Object.DespawnHierarchy();
			}
		}
	}
}
