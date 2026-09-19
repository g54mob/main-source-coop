using Features.BeachPresetModule.Scripts.Core;
using UnityEngine;

namespace Features.WeatherModule.Scripts
{
	[CreateAssetMenu(fileName = "WeatherPreset_Default", menuName = "Configurations/Weather/WeatherPreset")]
	public class WeatherPreset : BeachPreset
	{
		[ColorUsage(false, true)]
		[SerializeField]
		public Color DebugLocationAmbientColor;

		[SerializeField]
		public Color DebugLocationFogColor;

		[ColorUsage(false, true)]
		[SerializeField]
		public Color DebugBeachAmbientColor;

		[SerializeField]
		public Color DebugBeachFogColor;

		[ColorUsage(false, true)]
		[SerializeField]
		public Color DebugAdaptiveAmbientColor;

		[SerializeField]
		public Color DebugAdaptiveFogColor;

		[SerializeField]
		public Color DebugVolumetricLocationFogColor;

		[SerializeField]
		public Color DebugVolumetricBeachFogColor;
	}
}
