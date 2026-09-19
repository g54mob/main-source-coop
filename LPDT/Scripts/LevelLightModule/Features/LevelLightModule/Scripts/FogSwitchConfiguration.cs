using UnityEngine;

namespace Features.LevelLightModule.Scripts
{
	[CreateAssetMenu(fileName = "FogSwitchConfiguration_Default", menuName = "Configurations/LevelLightModule/FogSwitchConfiguration")]
	public class FogSwitchConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public float FogSwitchTime { get; private set; }

		[field: SerializeField]
		public float BeachFogEndDistance { get; private set; }

		[field: SerializeField]
		public float LocationFogEndDistance { get; private set; }
	}
}
