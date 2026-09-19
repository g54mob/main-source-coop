using Features.ItemsModule.Scripts;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.ItemDamageModule.Scripts
{
	[CreateAssetMenu(menuName = "ItemCollision/ItemCostReduceConfig", fileName = "ItemCostReduceConfig_Default", order = 0)]
	public class ItemCostReduceConfig : ScriptableObject
	{
		[Header("Coin Spawn Settings")]
		[SerializeField]
		private float _coinSpawnRandomRadius = 0.3f;

		[field: Header("Prefab")]
		[field: SerializeField]
		public SerializableDictionary<int, MonoItem> CoinPrefabs { get; private set; }

		public float CoinSpawnRandomRadius => _coinSpawnRandomRadius;
	}
}
