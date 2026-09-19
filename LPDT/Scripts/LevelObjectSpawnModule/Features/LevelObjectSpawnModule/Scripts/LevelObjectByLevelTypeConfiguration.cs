using Features.LevelModule.Scripts;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.LevelObjectSpawnModule.Scripts
{
	[CreateAssetMenu(fileName = "LevelObjectByLevelTypeConfiguration_Default", menuName = "Level/LevelObjectByLevelTypeConfiguration")]
	public class LevelObjectByLevelTypeConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<LevelType, int> LevelTypeMapper { get; private set; }
	}
}
