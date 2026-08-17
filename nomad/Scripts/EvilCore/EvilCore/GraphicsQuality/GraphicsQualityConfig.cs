using System;
using UnityEngine;

namespace EvilCore.GraphicsQuality
{
	[CreateAssetMenu(fileName = "GraphicsQualityConfig", menuName = "EvilCore/Graphics Quality Config")]
	public class GraphicsQualityConfig : ScriptableObject
	{
		[Serializable]
		public class LevelSettings
		{
			[Header("Textures")]
			[Range(0f, 3f)]
			public int textureMipmapLimit;

			public AnisotropicFiltering anisotropicFiltering = AnisotropicFiltering.Enable;

			[Header("LOD")]
			[Range(0.3f, 2f)]
			public float lodBias = 1f;

			[Header("Shadows")]
			[Range(20f, 250f)]
			public float shadowDistance = 150f;

			[Header("SSAO")]
			public bool ssaoEnabled = true;

			[Tooltip("ScalableSetting quality tier: 0=Low, 1=Medium, 2=High, 3=Custom")]
			[Range(0f, 3f)]
			public int ssaoQuality = 1;

			[Header("SSGI")]
			public bool ssgiEnabled = true;

			[Range(1f, 64f)]
			public int ssgiRaySteps = 32;

			[Header("SSR")]
			public bool ssrEnabled = true;

			[Header("Bloom")]
			public bool bloomEnabled = true;

			[Tooltip("ScalableSetting quality tier: 0=Low, 1=Medium, 2=High, 3=Custom")]
			[Range(0f, 3f)]
			public int bloomQuality = 1;

			public bool bloomHighQualityFiltering = true;

			[Header("Volumetric Fog")]
			public bool volumetricFogEnabled = true;

			[Range(0.05f, 1f)]
			public float volumetricFogBudget = 0.5f;

			[Header("Contact Shadows")]
			public bool contactShadowsEnabled = true;

			[Header("Texture Streaming")]
			public bool textureStreamingEnabled = true;

			[Tooltip("Streamed-texture VRAM budget in MB. Keep low (≈1024) for 4GB GPUs.")]
			[Range(256f, 4096f)]
			public int textureStreamingBudgetMB = 1024;

			[Header("Custom Passes")]
			public bool customPassesEnabled = true;
		}

		[SerializeField]
		private LevelSettings low = new LevelSettings
		{
			textureMipmapLimit = 2,
			anisotropicFiltering = AnisotropicFiltering.Disable,
			lodBias = 0.5f,
			shadowDistance = 50f,
			ssaoEnabled = true,
			ssaoQuality = 0,
			ssgiEnabled = false,
			ssgiRaySteps = 16,
			ssrEnabled = false,
			bloomEnabled = false,
			bloomQuality = 0,
			bloomHighQualityFiltering = false,
			volumetricFogEnabled = false,
			volumetricFogBudget = 0.1f,
			contactShadowsEnabled = false,
			customPassesEnabled = false
		};

		[SerializeField]
		private LevelSettings medium = new LevelSettings
		{
			textureMipmapLimit = 1,
			anisotropicFiltering = AnisotropicFiltering.Enable,
			lodBias = 1f,
			shadowDistance = 100f,
			ssaoEnabled = true,
			ssaoQuality = 1,
			ssgiEnabled = false,
			ssgiRaySteps = 24,
			ssrEnabled = true,
			bloomEnabled = true,
			bloomQuality = 1,
			bloomHighQualityFiltering = true,
			volumetricFogEnabled = true,
			volumetricFogBudget = 0.33f,
			contactShadowsEnabled = true,
			customPassesEnabled = true
		};

		[SerializeField]
		private LevelSettings high = new LevelSettings
		{
			textureMipmapLimit = 0,
			anisotropicFiltering = AnisotropicFiltering.ForceEnable,
			lodBias = 1.5f,
			shadowDistance = 150f,
			ssaoEnabled = true,
			ssaoQuality = 2,
			ssgiEnabled = true,
			ssgiRaySteps = 32,
			ssrEnabled = true,
			bloomEnabled = true,
			bloomQuality = 2,
			bloomHighQualityFiltering = true,
			volumetricFogEnabled = true,
			volumetricFogBudget = 0.5f,
			contactShadowsEnabled = true,
			customPassesEnabled = true
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
