using Cysharp.Threading.Tasks;
using Features.ItemsModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.BeachPresetModule.Scripts.Core.Interfaces
{
	public interface IBeachItemSpawnService
	{
		UniTask<NetworkBehaviour> SpawnItem(NetworkBehaviour prefab, Vector3 position, Quaternion rotation, ItemData itemData, bool addRandomForce, float randomForce);

		void Despawn(NetworkBehaviour spawned);
	}
}
