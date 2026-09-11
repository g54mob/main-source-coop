using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[Serializable]
	[VolumeComponentMenu("SC Post Effects/Stylized/Edge Detection")]
	[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
	public sealed class EdgeDetection : VolumeComponent, IPostProcessComponent
	{
		public enum EdgeDetectMode
		{
			DepthNormals = 0,
			CrossDepthNormals = 1,
			SobelDepth = 2,
			LuminanceBased = 3
		}

		[Serializable]
		public sealed class EdgeDetectionMode : VolumeParameter<EdgeDetectMode>
		{
		}

		[Range(0f, 1f)]
		[Tooltip("Shows only the effect, to allow for finetuning")]
		public BoolParameter debug = new BoolParameter(value: false);

		[Space]
		[Tooltip("Choose one of the different edge solvers")]
		public EdgeDetectionMode mode = new EdgeDetectionMode
		{
			value = EdgeDetectMode.DepthNormals
		};

		public BoolParameter invertFadeDistance = new BoolParameter(value: false);

		[Tooltip("Fades out the effect between the cameras near and far clipping plane")]
		public BoolParameter distanceFade = new BoolParameter(value: false);

		public FloatParameter startFadeDistance = new FloatParameter(0f);

		public FloatParameter endFadeDistance = new FloatParameter(1000f);

		[Header("Sensitivity")]
		[InspectorName("Depth")]
		[Range(0f, 1f)]
		[Tooltip("Sets how much difference in depth between pixels contribute to drawing an edge")]
		public ClampedFloatParameter sensitivityDepth = new ClampedFloatParameter(0f, 0f, 1f);

		[InspectorName("Normals")]
		[Range(0f, 1f)]
		[Tooltip("Sets how much difference in normals between pixels contribute to drawing an edge")]
		public ClampedFloatParameter sensitivityNormals = new ClampedFloatParameter(1f, 0f, 1f);

		[Range(0.01f, 1f)]
		[InspectorName("Luminance Threshold")]
		[Tooltip("Luminance threshold, pixels above this threshold will contribute to the effect")]
		public ClampedFloatParameter lumThreshold = new ClampedFloatParameter(0.01f, 0.01f, 1f);

		[Header("Edges")]
		[InspectorName("Color")]
		[Tooltip("")]
		public ColorParameter edgeColor = new ColorParameter(Color.black);

		[Range(1f, 50f)]
		[Tooltip("Edge Exponent")]
		public ClampedFloatParameter edgeExp = new ClampedFloatParameter(1f, 1f, 50f);

		[InspectorName("Size")]
		[Range(1f, 4f)]
		[Tooltip("Edge Distance")]
		public ClampedIntParameter edgeSize = new ClampedIntParameter(1, 1, 4);

		[InspectorName("Opacity")]
		[Tooltip("Opacity")]
		public ClampedFloatParameter edgeOpacity = new ClampedFloatParameter(0f, 0f, 1f);

		[InspectorName("Thin")]
		[Tooltip("Limit the effect to inward edges only")]
		public BoolParameter sobelThin = new BoolParameter(value: false);

		[SerializeField]
		public Shader DepthNormalsShader;

		[SerializeField]
		public Shader shader;

		public bool IsActive()
		{
			if (active)
			{
				return edgeOpacity.value > 0f;
			}
			return false;
		}

		public bool IsTileCompatible()
		{
			return false;
		}

		private void Reset()
		{
			SerializeShader();
		}

		private bool SerializeShader()
		{
			bool result = !shader || !DepthNormalsShader;
			shader = Shader.Find("Hidden/SC Post Effects/Edge Detection");
			DepthNormalsShader = Shader.Find("Hidden/SC Post Effects/DepthNormals");
			return result;
		}
	}
}
