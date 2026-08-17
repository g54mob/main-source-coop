using System;
using EvilCore.GraphicsQuality;
using UnityEngine;

namespace NomadDrive.Features.OutdoorLighting
{
	[CreateAssetMenu(fileName = "OutdoorLightingLodConfig", menuName = "NomadDrive/Lighting/Outdoor Lighting LOD Config")]
	public class OutdoorLightingLodConfig : ScriptableObject
	{
		[Serializable]
		public class LevelSettings
		{
			[Tooltip("Within this distance: real Light + (if within the fog budget) LocalVolumetricFog.")]
			public float nearDistance = 45f;

			[Tooltip("Within this distance (beyond near): real Light, no volumetric/fog.")]
			public float midDistance = 110f;

			[Tooltip("Within this distance (beyond mid): emissive bulb only. Beyond it: fully off.")]
			public float emissiveDistance = 250f;

			[Tooltip("Margin (metres) added around each band boundary to stop flicker when standing near a threshold.")]
			public float hysteresis = 6f;

			[Tooltip("Max number of nearest lights allowed to carry volumetric fog at once (HDRP caps on-screen LocalVolumetricFog at 64). 0 = never.")]
			public int maxVolumetricLights = 6;

			[Tooltip("Broken lights only run their per-frame flicker within this distance.")]
			public float flickerDistance = 40f;

			[Tooltip("Seconds between full re-evaluations of the whole light registry.")]
			public float evaluateInterval = 0.33f;
		}

		[SerializeField]
		private LevelSettings low = new LevelSettings
		{
			nearDistance = 28f,
			midDistance = 60f,
			emissiveDistance = 140f,
			hysteresis = 5f,
			maxVolumetricLights = 0,
			flickerDistance = 24f,
			evaluateInterval = 0.5f
		};

		[SerializeField]
		private LevelSettings medium = new LevelSettings
		{
			nearDistance = 40f,
			midDistance = 95f,
			emissiveDistance = 200f,
			hysteresis = 6f,
			maxVolumetricLights = 4,
			flickerDistance = 34f,
			evaluateInterval = 0.4f
		};

		[SerializeField]
		private LevelSettings high = new LevelSettings
		{
			nearDistance = 50f,
			midDistance = 120f,
			emissiveDistance = 280f,
			hysteresis = 6f,
			maxVolumetricLights = 8,
			flickerDistance = 44f,
			evaluateInterval = 0.33f
		};

		public LevelSettings GetSettings(GraphicsQualityLevel level)
		{
			return level switch
			{
				GraphicsQualityLevel.Low => low, 
				GraphicsQualityLevel.Medium => medium, 
				GraphicsQualityLevel.High => high, 
				_ => high, 
			};
		}
	}
}
