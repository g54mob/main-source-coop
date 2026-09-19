using Global.SerializableDictionary;
using UnityEngine;

namespace Features.LevelObjectSpawnModule.Scripts
{
	[CreateAssetMenu(fileName = "LevelObjectConfiguration_Default", menuName = "Level/LevelObjectConfiguration")]
	public class LevelObjectConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<int, LevelData> LevelObjects { get; private set; }
	}
}
