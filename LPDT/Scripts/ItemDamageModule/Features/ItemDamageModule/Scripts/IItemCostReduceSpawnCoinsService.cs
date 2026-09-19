using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Features.ItemDamageModule.Scripts
{
	public interface IItemCostReduceSpawnCoinsService
	{
		UniTaskVoid SpawnCoins(Vector3 position, int coinCount);
	}
}
