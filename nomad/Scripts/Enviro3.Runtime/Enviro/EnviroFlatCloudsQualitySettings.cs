using System;

namespace Enviro
{
	[Serializable]
	public class EnviroFlatCloudsQualitySettings
	{
		public bool cirrusClouds = true;

		public bool flatClouds = true;

		public int flatCloudsShadowSteps = 8;
	}
}
