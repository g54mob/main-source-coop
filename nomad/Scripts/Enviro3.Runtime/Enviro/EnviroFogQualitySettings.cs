using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroFogQualitySettings
	{
		public bool fog = true;

		public EnviroFogSettings.FogQualityMode fogQualityMode;

		public bool volumetrics = true;

		public bool unityFog;

		public EnviroFogSettings.Quality quality;

		[Range(16f, 96f)]
		public int steps = 32;
	}
}
