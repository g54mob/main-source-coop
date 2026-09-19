using System.Threading.Tasks;
using Fusion;
using UnityEngine;

namespace Features.StoreModule.Scripts
{
	public interface ICardItemSpawner
	{
		Task<NetworkObject> Spawn(StoreCardData storeCardData, Vector3 position, Quaternion rotation, Color color, Transform parent = null);
	}
}
