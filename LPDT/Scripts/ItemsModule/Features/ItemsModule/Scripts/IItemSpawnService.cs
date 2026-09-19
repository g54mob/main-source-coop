using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

namespace Features.ItemsModule.Scripts
{
	public interface IItemSpawnService
	{
		UniTask<NetworkBehaviour> SpawnItem(NetworkBehaviour prefab, Vector3 position, Quaternion rotation, ItemData itemData = null, bool addRandomForce = false, float randomForce = 0f);

		UniTask SpawnItemsMultiple(NetworkBehaviour prefab, Vector3 centerPosition, int count, float positionRandomRadius, bool useSpread, ItemData itemData = null, bool addRandomForce = false, float randomForce = 0f);

		UniTask SpawnItemsMultiple(Dictionary<NetworkBehaviour, MultipleSpawnData> prefabs, Vector3 centerPosition, float positionRandomRadius, bool useSpread, bool addRandomForce = false, float randomForce = 0f);
	}
}
