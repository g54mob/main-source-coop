using System;

namespace UnityEngine.Rendering.HighDefinition
{
	[VolumeComponentMenu("Sky/Enviro 3 Skybox")]
	[SkyUniqueID(990)]
	public class EnviroHDRPSky : SkySettings
	{
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override int GetHashCode(Camera camera)
		{
			return GetHashCode();
		}

		public override Type GetSkyRendererType()
		{
			return typeof(EnviroHDRPSkyRenderer);
		}
	}
}
