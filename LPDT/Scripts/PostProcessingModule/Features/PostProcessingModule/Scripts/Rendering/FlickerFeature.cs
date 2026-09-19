using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Features.PostProcessingModule.Scripts.Rendering
{
	public class FlickerFeature : ScriptableRendererFeature
	{
		private class FlickerPass : ScriptableRenderPass
		{
			private class PassData
			{
				public Material material;

				public TextureHandle source;

				public TextureHandle destination;

				public float frequency;

				public float effectWeight;

				public float randomization;

				public float minPostExposure;

				public float maxPostExposure;

				public float minSaturation;

				public float maxSaturation;
			}

			private static readonly int FrequencyId = Shader.PropertyToID("_FlickerFrequency");

			private static readonly int EffectWeightId = Shader.PropertyToID("_FlickerEffectWeight");

			private static readonly int RandomizationId = Shader.PropertyToID("_FlickerRandomization");

			private static readonly int MinPostExposureId = Shader.PropertyToID("_FlickerMinPostExposure");

			private static readonly int MaxPostExposureId = Shader.PropertyToID("_FlickerMaxPostExposure");

			private static readonly int MinSaturationId = Shader.PropertyToID("_FlickerMinSaturation");

			private static readonly int MaxSaturationId = Shader.PropertyToID("_FlickerMaxSaturation");

			private static readonly Vector4 BlitScaleBias = new Vector4(1f, 1f, 0f, 0f);

			private readonly Material _material;

			public FlickerPass(Material material, RenderPassEvent renderPassEvent)
			{
				_material = material;
				base.renderPassEvent = renderPassEvent;
			}

			public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
			{
				FlickerVolume component = VolumeManager.instance.stack.GetComponent<FlickerVolume>();
				if (component == null || !component.IsActive())
				{
					return;
				}
				UniversalResourceData universalResourceData = frameData.Get<UniversalResourceData>();
				RenderTextureDescriptor cameraTargetDescriptor = frameData.Get<UniversalCameraData>().cameraTargetDescriptor;
				cameraTargetDescriptor.depthBufferBits = 0;
				cameraTargetDescriptor.msaaSamples = 1;
				TextureHandle textureHandle = UniversalRenderer.CreateRenderGraphTexture(renderGraph, cameraTargetDescriptor, "_FlickerTempTexture", clear: false);
				PassData passData;
				using (IRasterRenderGraphBuilder rasterRenderGraphBuilder = renderGraph.AddRasterRenderPass<PassData>("Flicker_Apply", out passData, "C:\\Dev\\UnityProjects\\RubberArmsNew\\RubberArmsPrototype\\Assets\\Features\\PostProcessingModule\\Scripts\\Rendering\\FlickerFeature.cs", 55))
				{
					passData.material = _material;
					passData.source = universalResourceData.activeColorTexture;
					passData.destination = textureHandle;
					passData.frequency = component.frequency.value;
					passData.effectWeight = component.EffectWeight;
					passData.randomization = component.randomization.value;
					passData.minPostExposure = component.minPostExposure.value;
					passData.maxPostExposure = component.maxPostExposure.value;
					passData.minSaturation = component.minSaturation.value;
					passData.maxSaturation = component.maxSaturation.value;
					rasterRenderGraphBuilder.UseTexture(in passData.source);
					rasterRenderGraphBuilder.SetRenderAttachment(passData.destination, 0);
					rasterRenderGraphBuilder.AllowPassCulling(value: false);
					rasterRenderGraphBuilder.AllowGlobalStateModification(value: true);
					rasterRenderGraphBuilder.SetRenderFunc(delegate(PassData data, RasterGraphContext context)
					{
						data.material.SetFloat(FrequencyId, data.frequency);
						data.material.SetFloat(EffectWeightId, data.effectWeight);
						data.material.SetFloat(RandomizationId, data.randomization);
						data.material.SetFloat(MinPostExposureId, data.minPostExposure);
						data.material.SetFloat(MaxPostExposureId, data.maxPostExposure);
						data.material.SetFloat(MinSaturationId, data.minSaturation);
						data.material.SetFloat(MaxSaturationId, data.maxSaturation);
						Blitter.BlitTexture(context.cmd, data.source, BlitScaleBias, data.material, 0);
					});
				}
				PassData passData2;
				using IRasterRenderGraphBuilder rasterRenderGraphBuilder2 = renderGraph.AddRasterRenderPass<PassData>("Flicker_CopyBack", out passData2, "C:\\Dev\\UnityProjects\\RubberArmsNew\\RubberArmsPrototype\\Assets\\Features\\PostProcessingModule\\Scripts\\Rendering\\FlickerFeature.cs", 84);
				passData2.source = textureHandle;
				passData2.destination = universalResourceData.activeColorTexture;
				rasterRenderGraphBuilder2.UseTexture(in passData2.source);
				rasterRenderGraphBuilder2.SetRenderAttachment(passData2.destination, 0);
				rasterRenderGraphBuilder2.AllowPassCulling(value: false);
				rasterRenderGraphBuilder2.SetRenderFunc(delegate(PassData data, RasterGraphContext context)
				{
					Blitter.BlitTexture(context.cmd, data.source, BlitScaleBias, 0f, bilinear: false);
				});
			}
		}

		private const float MinEffectWeight = 0.0001f;

		[SerializeField]
		private Shader _shader;

		[SerializeField]
		private RenderPassEvent _renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

		private Material _material;

		private FlickerPass _pass;

		private static bool _wasDrivingSaturation;

		private static bool _wasDrivingPostExposure;

		private static bool _wasDrivingFilmGrain;

		public override void Create()
		{
			if (_shader == null)
			{
				_shader = Shader.Find("Hidden/Custom/Flicker");
			}
			if (_shader != null && _material == null)
			{
				_material = CoreUtils.CreateEngineMaterial(_shader);
			}
			_pass = new FlickerPass(_material, _renderPassEvent);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (!renderingData.cameraData.postProcessEnabled)
			{
				ReleaseStackOverrides();
			}
			else
			{
				if (renderingData.cameraData.cameraType != CameraType.Game && renderingData.cameraData.cameraType != CameraType.SceneView)
				{
					return;
				}
				FlickerVolume flickerVolume = VolumeManager.instance.stack?.GetComponent<FlickerVolume>();
				if (flickerVolume == null || !flickerVolume.IsActive())
				{
					ReleaseStackOverrides();
					return;
				}
				bool flag = false;
				if (flickerVolume.ShouldDriveColorAdjustments)
				{
					flag |= DriveColorAdjustments(flickerVolume);
				}
				else
				{
					ReleaseColorAdjustmentOverrides();
				}
				if (flickerVolume.syncFilmGrain.value && flickerVolume.EffectiveFilmGrainIntensity >= 0.0001f)
				{
					DriveFilmGrain(flickerVolume);
					flag = true;
				}
				else
				{
					ReleaseFilmGrainOverrides();
				}
				if (!flag && !(_material == null))
				{
					renderer.EnqueuePass(_pass);
				}
			}
		}

		private static bool DriveColorAdjustments(FlickerVolume volume)
		{
			ColorAdjustments component = VolumeManager.instance.stack.GetComponent<ColorAdjustments>();
			if (component == null)
			{
				return false;
			}
			float t = FlickerMath.Evaluate(Time.time, volume.frequency.value, volume.randomization.value);
			float effectWeight = volume.EffectWeight;
			float value = component.saturation.value;
			float value2 = component.postExposure.value;
			bool result = false;
			if (volume.syncSaturation.value)
			{
				float b = Mathf.Lerp(volume.minSaturation.value, volume.maxSaturation.value, t);
				component.saturation.Override(Mathf.Lerp(value, b, effectWeight));
				_wasDrivingSaturation = true;
				result = true;
			}
			if (volume.syncPostExposure.value)
			{
				float b2 = Mathf.Lerp(volume.minPostExposure.value, volume.maxPostExposure.value, t);
				component.postExposure.Override(Mathf.Lerp(value2, b2, effectWeight));
				_wasDrivingPostExposure = true;
				result = true;
			}
			return result;
		}

		private static void DriveFilmGrain(FlickerVolume volume)
		{
			FilmGrain component = VolumeManager.instance.stack.GetComponent<FilmGrain>();
			if (!(component == null))
			{
				float value = component.intensity.value;
				float t = Mathf.Clamp01(volume.blend.value);
				component.type.Override(volume.type.value);
				component.intensity.Override(Mathf.Lerp(value, volume.intensity.value, t));
				_wasDrivingFilmGrain = true;
			}
		}

		private static void ReleaseColorAdjustmentOverrides()
		{
			ColorAdjustments colorAdjustments = VolumeManager.instance.stack?.GetComponent<ColorAdjustments>();
			if (colorAdjustments == null)
			{
				_wasDrivingSaturation = false;
				_wasDrivingPostExposure = false;
				return;
			}
			if (_wasDrivingSaturation)
			{
				colorAdjustments.saturation.overrideState = false;
				_wasDrivingSaturation = false;
			}
			if (_wasDrivingPostExposure)
			{
				colorAdjustments.postExposure.overrideState = false;
				_wasDrivingPostExposure = false;
			}
		}

		private static void ReleaseFilmGrainOverrides()
		{
			if (_wasDrivingFilmGrain)
			{
				FilmGrain filmGrain = VolumeManager.instance.stack?.GetComponent<FilmGrain>();
				if (filmGrain != null)
				{
					filmGrain.type.overrideState = false;
					filmGrain.intensity.overrideState = false;
				}
				_wasDrivingFilmGrain = false;
			}
		}

		private void ReleaseStackOverrides()
		{
			ReleaseColorAdjustmentOverrides();
			ReleaseFilmGrainOverrides();
		}

		protected override void Dispose(bool disposing)
		{
			ReleaseStackOverrides();
			CoreUtils.Destroy(_material);
		}
	}
}
