using Features.LevelModule.Scripts;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.StoreModule.Scripts
{
	[CreateAssetMenu(fileName = "StoreLevelConfiguration_Default", menuName = "Configurations/StoreModule/StoreLevelConfiguration")]
	public class StoreLevelConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<LevelType, StoreByLevelData> StoreByLevelData { get; private set; }

		[field: SerializeField]
		public StoreByLevelData DefaultStoreByLevelData { get; private set; }
	}
}
