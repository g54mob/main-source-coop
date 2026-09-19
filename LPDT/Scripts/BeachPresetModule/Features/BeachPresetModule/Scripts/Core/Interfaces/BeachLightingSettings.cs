using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Features.BeachPresetModule.Scripts.Core.Interfaces
{
	public class BeachLightingSettings
	{
		public bool ApplySkybox { get; set; }

		public Material Skybox { get; set; }

		public bool ApplyAmbient { get; set; }

		public AmbientMode AmbientMode { get; set; }

		public Color AmbientColor { get; set; }

		public Color AmbientSkyColor { get; set; }

		public Color AmbientEquatorColor { get; set; }

		public Color AmbientGroundColor { get; set; }

		public float AmbientIntensity { get; set; }

		public float ReflectionIntensity { get; set; }

		public int ReflectionBounces { get; set; }

		public bool ApplyDirectionalLight { get; set; }

		public BeachDirectionalLightSettings DirectionalLightSettings { get; set; }

		public IReadOnlyList<BeachLightGroupOverride> LightGroupOverrides { get; set; } = Array.Empty<BeachLightGroupOverride>();

		public IReadOnlyList<BeachIndexedLightOverride> IndexedLightOverrides { get; set; } = Array.Empty<BeachIndexedLightOverride>();
	}
}
