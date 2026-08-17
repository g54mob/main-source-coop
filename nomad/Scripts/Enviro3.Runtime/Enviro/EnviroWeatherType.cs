using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroWeatherType : ScriptableObject
	{
		public bool showEditor;

		public bool showEffectControls;

		public bool showCloudControls;

		public bool showFlatCloudControls;

		public bool showFogControls;

		public bool showSkyControls;

		public bool showLightingControls;

		public bool showAuroraControls;

		public bool showEnvironmentControls;

		public bool showAudioControls;

		public bool showAmbientAudioControls;

		public bool showWeatherAudioControls;

		public bool showLightningControls;

		public EnviroWeatherTypeCloudsOverride cloudsOverride;

		public EnviroWeatherTypeFlatCloudsOverride flatCloudsOverride;

		public EnviroWeatherTypeLightingOverride lightingOverride;

		public EnviroWeatherTypeSkyOverride skyOverride;

		public EnviroWeatherTypeFogOverride fogOverride;

		public EnviroWeatherTypeAuroraOverride auroraOverride;

		public EnviroWeatherTypeEffectsOverride effectsOverride;

		public EnviroWeatherTypeAudioOverride audioOverride;

		public EnviroWeatherTypeLightningOverride lightningOverride;

		public EnviroWeatherTypeEnvironmentOverride environmentOverride;
	}
}
