using Fusion;
using UnityEngine;

namespace Features.LevelLightModule.Scripts
{
	[CreateAssetMenu(fileName = "FlashlightConfiguration_Default", menuName = "Configurations/LevelLightModule/FlashlightConfiguration")]
	public class FlashlightConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<FlashlightType, NetworkPrefabRef> FlashlightPrefabs { get; private set; } = new SerializableDictionary<FlashlightType, NetworkPrefabRef>();
	}
}
