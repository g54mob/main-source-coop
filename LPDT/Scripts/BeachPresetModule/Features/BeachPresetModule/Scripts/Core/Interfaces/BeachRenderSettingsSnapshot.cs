using UnityEngine;
using UnityEngine.Rendering;

namespace Features.BeachPresetModule.Scripts.Core.Interfaces
{
	internal struct BeachRenderSettingsSnapshot
	{
		public AmbientMode AmbientMode;

		public Color AmbientLight;

		public Color AmbientSkyColor;

		public Color AmbientEquatorColor;

		public Color AmbientGroundColor;

		public float AmbientIntensity;

		public Material Skybox;

		public int ReflectionBounces;

		public float ReflectionIntensity;
	}
}
