using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RadiantGI.Universal
{
	[ExecuteInEditMode]
	[VolumeComponentMenu("Kronnect/Radiant Global Illumination")]
	public class RadiantGlobalIllumination : VolumeComponent, IPostProcessComponent
	{
		public enum DebugView
		{
			None = 0,
			Albedo = 1,
			Normals = 2,
			Specular = 3,
			Depth = 4,
			Raycast = 20,
			DownscaledHalf = 30,
			DownscaledQuarter = 40,
			ReflectiveShadowMap = 41,
			UpscaleToHalf = 50,
			TemporalAccumulationBuffer = 60,
			FinalGI = 70
		}

		[Serializable]
		public sealed class CompareFunctionParameter : VolumeParameter<CompareFunction>
		{
		}

		[Serializable]
		public sealed class DebugViewParameter : VolumeParameter<DebugView>
		{
		}

		[Tooltip("Intensity of the indirect lighting.")]
		public FloatParameter indirectIntensity = new FloatParameter(0f);

		[Tooltip("Distance attenuation applied to indirect lighting. Reduces indirect intensity by square of distance.")]
		public ClampedFloatParameter indirectDistanceAttenuation = new ClampedFloatParameter(0f, 0f, 1f);

		[Tooltip("Maximum brightness of indirect source.")]
		public FloatParameter indirectMaxSourceBrightness = new FloatParameter(8f);

		[Tooltip("Determines how much influence has the surface normal map when receiving indirect lighting.")]
		public ClampedFloatParameter normalMapInfluence = new ClampedFloatParameter(1f, 0f, 1f);

		[Tooltip("Add one ray bounce.")]
		public BoolParameter rayBounce = new BoolParameter(value: false);

		[Tooltip("Only in forward rendering mode: uses pixel luma to enhance results by adding variety to the effect based on the perceptual brigthness. Set this value to 0 to disable this feature.")]
		public FloatParameter lumaInfluence = new FloatParameter(0f);

		[Tooltip("Intensity of the near field obscurance effect. Darkens surfaces occluded by other nearby surfaces.")]
		public FloatParameter nearFieldObscurance = new FloatParameter(0f);

		[Tooltip("Spread or radius of the near field obscurance effect")]
		public ClampedFloatParameter nearFieldObscuranceSpread = new ClampedFloatParameter(0.2f, 0.01f, 1f);

		[Tooltip("Maximum distance of Near Field Obscurance effect")]
		public FloatParameter nearFieldObscuranceMaxCameraDistance = new FloatParameter(125f);

		[Tooltip("Distance threshold of the occluder")]
		public ClampedFloatParameter nearFieldObscuranceOccluderDistance = new ClampedFloatParameter(0.825f, 0f, 1f);

		[Tooltip("Tint color of Near Field Obscurance effect")]
		[ColorUsage(false)]
		public ColorParameter nearFieldObscuranceTintColor = new ColorParameter(Color.black);

		[Tooltip("Enable user-defined light emitters in the scene.")]
		public BoolParameter virtualEmitters = new BoolParameter(value: false);

		[Tooltip("Number of rays per pixel")]
		public ClampedIntParameter rayCount = new ClampedIntParameter(1, 1, 4);

		[Tooltip("Max ray length. Increasing this value may also require increasing the 'Max Samples' value to avoid losing quality.")]
		public FloatParameter rayMaxLength = new FloatParameter(8f);

		[Tooltip("Max samples taken during raymarch.")]
		public IntParameter rayMaxSamples = new IntParameter(32);

		[Tooltip("Jitter adds a random offset to the ray direction to reduce banding. Useful when using low sample count.")]
		public FloatParameter rayJitter = new FloatParameter(0f);

		[Tooltip("The assumed thickness for any geometry. Used to determine if ray crosses a surface.")]
		public FloatParameter thickness = new FloatParameter(1f);

		[Tooltip("Improves raymarch accuracy by using binary search.")]
		public BoolParameter rayBinarySearch = new BoolParameter(value: true);

		[Tooltip("In case a ray miss a target, reuse rays from previous frames.")]
		public BoolParameter fallbackReuseRays = new BoolParameter(value: false);

		[Tooltip("If a ray misses a target, reuse result from history buffer. This value is the intensity of the previous color in case the ray misses the target.")]
		public ClampedFloatParameter rayReuse = new ClampedFloatParameter(0f, 0f, 1f);

		[Tooltip("In case a ray miss a target, use nearby probes if they're available.")]
		public BoolParameter fallbackReflectionProbes = new BoolParameter(value: false);

		[Tooltip("Custom global probe intensity multiplier. Note that each probe has also an intensity property.")]
		public FloatParameter probesIntensity = new FloatParameter(1f);

		[Tooltip("In case a ray miss a target, use reflective shadow map data from the main directional light. You need to add the ReflectiveShadowMap script to the directional light to use this feature.")]
		public BoolParameter fallbackReflectiveShadowMap = new BoolParameter(value: false);

		public ClampedFloatParameter reflectiveShadowMapIntensity = new ClampedFloatParameter(0.8f, 0f, 1f);

		[Tooltip("Reduces resolution of all GI stages improving performance")]
		public ClampedFloatParameter downsampling = new ClampedFloatParameter(1f, 1f, 4f);

		[Tooltip("Raytracing accuracy. Reducing this value will shrink the depth buffer used during raytracing, improving performance in exchange of accuracy.")]
		public ClampedIntParameter raytracerAccuracy = new ClampedIntParameter(8, 1, 8);

		[Tooltip("Extra blur passes")]
		public ClampedIntParameter smoothing = new ClampedIntParameter(3, 0, 4);

		[Tooltip("Uses motion vectors to blend into a history buffer to reduce flickering. Only applies in play mode.")]
		public BoolParameter temporalReprojection = new BoolParameter(value: true);

		[Tooltip("Reaction speed to screen changes. Higher values reduces ghosting but also the smoothing.")]
		public FloatParameter temporalResponseSpeed = new FloatParameter(12f);

		[Tooltip("Reaction speed to camera position change. Higher values reduces ghosting when camera moves.")]
		public FloatParameter temporalCameraTranslationResponse = new FloatParameter(100f);

		[Tooltip("Difference in depth with current frame to discard history buffer when reusing rays.")]
		public FloatParameter temporalDepthRejection = new FloatParameter(1f);

		[Tooltip("Allowed difference in color between history and current GI buffers.")]
		public ClampedFloatParameter temporalChromaThreshold = new ClampedFloatParameter(0.2f, 0f, 1f);

		[Tooltip("Renders the effect also in edit mode (when not in play-mode).")]
		public BoolParameter showInEditMode = new BoolParameter(value: true);

		[Tooltip("Renders the effect also in Scene View.")]
		public BoolParameter showInSceneView = new BoolParameter(value: true);

		[Tooltip("Computes GI emitted by objects with a minimum luminosity.")]
		public FloatParameter brightnessThreshold = new FloatParameter(0f);

		[Tooltip("Maximum GI brightness.")]
		public FloatParameter brightnessMax = new FloatParameter(8f);

		[Tooltip("Amount of GI which adds to specular surfaces. Reduce this value to avoid overexposition of shiny materials.")]
		public ClampedFloatParameter specularContribution = new ClampedFloatParameter(0.75f, 0f, 1f);

		[Tooltip("Attenuates GI brightness from nearby surfaces.")]
		public FloatParameter nearCameraAttenuation = new FloatParameter(0f);

		[Tooltip("Adjusted color saturation for the computed GI.")]
		public ClampedFloatParameter saturation = new ClampedFloatParameter(1f, 0f, 2f);

		[Tooltip("Applies GI only inside the post processing volume (use only if the volume is local)")]
		public BoolParameter limitToVolumeBounds = new BoolParameter(value: false);

		[Tooltip("Enables stencil check during GI composition. This option let you exclude GI over certain objects that also use stencil buffer.")]
		public BoolParameter stencilCheck = new BoolParameter(value: false);

		public IntParameter stencilValue = new IntParameter(1);

		public CompareFunctionParameter stencilCompareFunction = new CompareFunctionParameter
		{
			value = CompareFunction.NotEqual
		};

		[Tooltip("Integration with URP native screen space ambient occlusion (also with HBAO in Lit AO mode). Amount of ambient occlusion that influences indirect lighting created by Radiant.")]
		public ClampedFloatParameter aoInfluence = new ClampedFloatParameter(0f, 0f, 1f);

		public DebugViewParameter debugView = new DebugViewParameter
		{
			value = DebugView.None
		};

		[Tooltip("Depth values multiplier for the depth debug view")]
		public FloatParameter debugDepthMultiplier = new FloatParameter(10f);

		public BoolParameter compareMode = new BoolParameter(value: false);

		public BoolParameter compareSameSide = new BoolParameter(value: false);

		public ClampedFloatParameter comparePanning = new ClampedFloatParameter(0.25f, 0f, 0.5f);

		public ClampedFloatParameter compareLineAngle = new ClampedFloatParameter(1.4f, -MathF.PI, MathF.PI);

		public ClampedFloatParameter compareLineWidth = new ClampedFloatParameter(0.002f, 0.0001f, 0.05f);

		public bool IsActive()
		{
			if (!(indirectIntensity.value > 0f))
			{
				return compareMode.value;
			}
			return true;
		}

		public bool IsTileCompatible()
		{
			return true;
		}

		private void OnValidate()
		{
			indirectIntensity.value = Mathf.Max(0f, indirectIntensity.value);
			indirectMaxSourceBrightness.value = Mathf.Max(0f, indirectMaxSourceBrightness.value);
			temporalResponseSpeed.value = Mathf.Max(0f, temporalResponseSpeed.value);
			temporalDepthRejection.value = Mathf.Max(0f, temporalDepthRejection.value);
			rayMaxLength.value = Mathf.Max(0.1f, rayMaxLength.value);
			rayMaxSamples.value = Mathf.Max(2, rayMaxSamples.value);
			rayJitter.value = Mathf.Max(0f, rayJitter.value);
			lumaInfluence.value = Mathf.Max(0f, lumaInfluence.value);
			thickness.value = Mathf.Max(0.1f, thickness.value);
			brightnessThreshold.value = Mathf.Max(0f, brightnessThreshold.value);
			brightnessMax.value = Mathf.Max(0f, brightnessMax.value);
			nearCameraAttenuation.value = Mathf.Max(0f, nearCameraAttenuation.value);
			nearFieldObscurance.value = Mathf.Max(0f, nearFieldObscurance.value);
			nearFieldObscuranceMaxCameraDistance.value = Mathf.Max(0f, nearFieldObscuranceMaxCameraDistance.value);
			debugDepthMultiplier.value = Mathf.Max(0f, debugDepthMultiplier.value);
		}

		private void Reset()
		{
			RadiantRenderFeature.needRTRefresh = true;
		}
	}
}
