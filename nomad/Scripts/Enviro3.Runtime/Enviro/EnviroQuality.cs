using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroQuality : ScriptableObject
	{
		public bool showEditor;

		public bool showSky;

		public bool showVolumeClouds;

		public bool showFog;

		public bool showFlatClouds;

		public bool showEffects;

		public bool showAurora;

		public EnviroSkyQualitySettings skyOverride;

		public EnviroVolumetricCloudsQualitySettings volumetricCloudsOverride;

		public EnviroFogQualitySettings fogOverride;

		public EnviroFlatCloudsQualitySettings flatCloudsOverride;

		public EnviroAuroraQualitySettings auroraOverride;

		public EnviroEffectsQualitySettings effectsOverride;
	}
}
