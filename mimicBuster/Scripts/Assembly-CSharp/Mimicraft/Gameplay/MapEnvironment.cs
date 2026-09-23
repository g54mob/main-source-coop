using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Mimicraft.Gameplay
{
	[Serializable]
	public class MapEnvironment
	{
		private struct SceneDefaults
		{
			public Material Skybox;

			public AmbientMode AmbientMode;

			public Color SkyColor;

			public Color EquatorColor;

			public Color GroundColor;

			public float AmbientIntensity;

			public bool Fog;

			public Color FogColor;

			public FogMode FogMode;

			public float FogDensity;

			public float FogStart;

			public float FogEnd;
		}

		[Tooltip("Skybox material for this map. Leave empty to keep whatever the Game scene uses.")]
		[SerializeField]
		private Material skybox;

		[Header("Ambient")]
		[Tooltip("Take over the scene's ambient lighting. Off leaves it untouched.")]
		[SerializeField]
		private bool overrideAmbient;

		[SerializeField]
		private AmbientMode ambientMode;

		[Tooltip("Used as the flat colour in Flat mode, and as the SKY colour in Trilight mode.")]
		[SerializeField]
		private Color ambientSkyColor = new Color(0.21f, 0.23f, 0.26f, 1f);

		[SerializeField]
		private Color ambientEquatorColor = new Color(0.11f, 0.12f, 0.13f, 1f);

		[SerializeField]
		private Color ambientGroundColor = new Color(0.05f, 0.05f, 0.05f, 1f);

		[SerializeField]
		[Min(0f)]
		private float ambientIntensity = 1f;

		[Header("Fog")]
		[Tooltip("Take over the scene's fog. Off leaves it untouched.")]
		[SerializeField]
		private bool overrideFog;

		[SerializeField]
		private bool fogEnabled = true;

		[SerializeField]
		private Color fogColor = new Color(0.5f, 0.5f, 0.5f, 1f);

		[SerializeField]
		private FogMode fogMode = FogMode.ExponentialSquared;

		[SerializeField]
		[Min(0f)]
		private float fogDensity = 0.01f;

		[SerializeField]
		private float fogStartDistance;

		[SerializeField]
		private float fogEndDistance = 300f;

		private static SceneDefaults defaults;

		private static bool defaultsCaptured;

		public void Apply()
		{
			CaptureDefaultsOnce();
			RestoreDefaults();
			if (skybox != null)
			{
				RenderSettings.skybox = skybox;
			}
			if (overrideAmbient)
			{
				RenderSettings.ambientMode = ambientMode;
				RenderSettings.ambientSkyColor = ambientSkyColor;
				RenderSettings.ambientEquatorColor = ambientEquatorColor;
				RenderSettings.ambientGroundColor = ambientGroundColor;
				RenderSettings.ambientLight = ambientSkyColor;
				RenderSettings.ambientIntensity = ambientIntensity;
			}
			if (overrideFog)
			{
				RenderSettings.fog = fogEnabled;
				RenderSettings.fogColor = fogColor;
				RenderSettings.fogMode = fogMode;
				RenderSettings.fogDensity = fogDensity;
				RenderSettings.fogStartDistance = fogStartDistance;
				RenderSettings.fogEndDistance = fogEndDistance;
			}
			DynamicGI.UpdateEnvironment();
		}

		private static void CaptureDefaultsOnce()
		{
			if (!defaultsCaptured)
			{
				defaultsCaptured = true;
				defaults = new SceneDefaults
				{
					Skybox = RenderSettings.skybox,
					AmbientMode = RenderSettings.ambientMode,
					SkyColor = RenderSettings.ambientSkyColor,
					EquatorColor = RenderSettings.ambientEquatorColor,
					GroundColor = RenderSettings.ambientGroundColor,
					AmbientIntensity = RenderSettings.ambientIntensity,
					Fog = RenderSettings.fog,
					FogColor = RenderSettings.fogColor,
					FogMode = RenderSettings.fogMode,
					FogDensity = RenderSettings.fogDensity,
					FogStart = RenderSettings.fogStartDistance,
					FogEnd = RenderSettings.fogEndDistance
				};
			}
		}

		private static void RestoreDefaults()
		{
			RenderSettings.skybox = defaults.Skybox;
			RenderSettings.ambientMode = defaults.AmbientMode;
			RenderSettings.ambientSkyColor = defaults.SkyColor;
			RenderSettings.ambientLight = defaults.SkyColor;
			RenderSettings.ambientEquatorColor = defaults.EquatorColor;
			RenderSettings.ambientGroundColor = defaults.GroundColor;
			RenderSettings.ambientIntensity = defaults.AmbientIntensity;
			RenderSettings.fog = defaults.Fog;
			RenderSettings.fogColor = defaults.FogColor;
			RenderSettings.fogMode = defaults.FogMode;
			RenderSettings.fogDensity = defaults.FogDensity;
			RenderSettings.fogStartDistance = defaults.FogStart;
			RenderSettings.fogEndDistance = defaults.FogEnd;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetOnPlay()
		{
			defaultsCaptured = false;
		}
	}
}
