using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Features.LevelLightModule.Scripts.Rendering
{
	[Serializable]
	[VolumeComponentMenu("Custom/Custom Fog")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public class CustomFogVolume : VolumeComponent, IPostProcessComponent
	{
		[Tooltip("Enable or disable the custom fog.")]
		public BoolParameter isEnabled = new BoolParameter(value: false);

		[Tooltip("The color of the fog.")]
		public ColorParameter fogColor = new ColorParameter(Color.gray);

		[Tooltip("Distance from the camera where the fog begins.")]
		public FloatParameter fogStart = new FloatParameter(10f);

		[Tooltip("Distance from the camera where the fog reaches maximum density.")]
		public FloatParameter fogEnd = new FloatParameter(50f);

		[Tooltip("Fog push distance if it encounters push plane.")]
		public FloatParameter fogPushEnd = new FloatParameter(50f);

		public bool IsActive()
		{
			if (isEnabled.value)
			{
				return fogStart.value < fogEnd.value;
			}
			return false;
		}

		public bool IsTileCompatible()
		{
			return false;
		}
	}
}
