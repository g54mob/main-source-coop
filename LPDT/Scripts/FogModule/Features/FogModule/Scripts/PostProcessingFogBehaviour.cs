using System;
using UnityEngine;

namespace Features.FogModule.Scripts
{
	[Serializable]
	public class PostProcessingFogBehaviour : FogBehaviour
	{
		protected override Color GetFogColor()
		{
			return RenderSettings.fogColor;
		}

		protected override float GetFogStartDistance()
		{
			return RenderSettings.fogStartDistance;
		}

		protected override float GetFogEndDistance()
		{
			return RenderSettings.fogEndDistance;
		}

		protected override FogMode GetFogMode()
		{
			return RenderSettings.fogMode;
		}

		protected override bool IsFogEnabled()
		{
			return RenderSettings.fog;
		}

		protected override void SetFogColor(Color fogColor)
		{
			RenderSettings.fogColor = fogColor;
		}

		protected override void SetFogStartDistance(float startDistance)
		{
			RenderSettings.fogStartDistance = startDistance;
		}

		protected override void SetFogEndDistance(float endDistance)
		{
			RenderSettings.fogEndDistance = endDistance;
		}

		protected override void SetFogMode(FogMode mode)
		{
			RenderSettings.fogMode = mode;
		}

		protected override void SetFogEnabled(bool enabled)
		{
			RenderSettings.fog = enabled;
		}
	}
}
