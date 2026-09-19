using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Obi
{
	[Serializable]
	public struct RenderBatchParams
	{
		[HideInInspector]
		public int layer;

		public LightProbeUsage lightProbeUsage;

		public ReflectionProbeUsage reflectionProbeUsage;

		public ShadowCastingMode shadowCastingMode;

		public bool receiveShadows;

		public MotionVectorGenerationMode motionVectors;

		public uint renderingLayerMask;

		public RenderBatchParams(bool receiveShadow)
		{
			layer = 0;
			lightProbeUsage = LightProbeUsage.BlendProbes;
			reflectionProbeUsage = ReflectionProbeUsage.BlendProbes;
			shadowCastingMode = ShadowCastingMode.On;
			receiveShadows = receiveShadow;
			motionVectors = MotionVectorGenerationMode.Camera;
			renderingLayerMask = uint.MaxValue;
		}

		public RenderBatchParams(Renderer renderer)
		{
			layer = renderer.gameObject.layer;
			lightProbeUsage = renderer.lightProbeUsage;
			reflectionProbeUsage = renderer.reflectionProbeUsage;
			shadowCastingMode = renderer.shadowCastingMode;
			receiveShadows = renderer.receiveShadows;
			motionVectors = renderer.motionVectorGenerationMode;
			renderingLayerMask = renderer.renderingLayerMask;
		}

		public int CompareTo(RenderBatchParams param)
		{
			int num = layer.CompareTo(param.layer);
			if (num == 0)
			{
				num = renderingLayerMask.CompareTo(param.renderingLayerMask);
			}
			if (num == 0)
			{
				num = lightProbeUsage.CompareTo(param.lightProbeUsage);
			}
			if (num == 0)
			{
				num = reflectionProbeUsage.CompareTo(param.reflectionProbeUsage);
			}
			if (num == 0)
			{
				num = shadowCastingMode.CompareTo(param.shadowCastingMode);
			}
			if (num == 0)
			{
				num = receiveShadows.CompareTo(param.receiveShadows);
			}
			if (num == 0)
			{
				num = motionVectors.CompareTo(param.motionVectors);
			}
			return num;
		}

		public RenderParams ToRenderParams()
		{
			RenderParams result = default(RenderParams);
			result.renderingLayerMask = GraphicsSettings.defaultRenderingLayerMask;
			result.lightProbeUsage = lightProbeUsage;
			result.reflectionProbeUsage = reflectionProbeUsage;
			result.shadowCastingMode = shadowCastingMode;
			result.receiveShadows = receiveShadows;
			result.motionVectorMode = motionVectors;
			result.renderingLayerMask = renderingLayerMask;
			result.layer = layer;
			return result;
		}
	}
}
