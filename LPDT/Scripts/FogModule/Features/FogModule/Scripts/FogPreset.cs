using Features.BeachPresetModule.Scripts.Core;
using UnityEngine;

namespace Features.FogModule.Scripts
{
	[CreateAssetMenu(fileName = "FogPreset_Default", menuName = "Configurations/FogModule/FogPreset")]
	public class FogPreset : ScriptableObject
	{
		[field: SerializeField]
		public OptionalValue<Color> FogColor { get; private set; }

		[field: SerializeField]
		public OptionalValue<float> StartDistance { get; private set; }

		[field: SerializeField]
		public OptionalValue<float> EndDistance { get; private set; }
	}
}
