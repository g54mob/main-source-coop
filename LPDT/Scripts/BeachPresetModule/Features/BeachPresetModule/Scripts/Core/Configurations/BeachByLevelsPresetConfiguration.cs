using Features.LevelModule.Scripts;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.BeachPresetModule.Scripts.Core.Configurations
{
	[CreateAssetMenu(fileName = "BeachByLevelsPresetConfiguration_Default", menuName = "Configurations/Beach Presets/BeachByLevelsPresetConfiguration")]
	public class BeachByLevelsPresetConfiguration : ScriptableObject
	{
		public SerializableDictionary<LevelType, BeachPreset> PresetsByLevel = new SerializableDictionary<LevelType, BeachPreset>();
	}
}
