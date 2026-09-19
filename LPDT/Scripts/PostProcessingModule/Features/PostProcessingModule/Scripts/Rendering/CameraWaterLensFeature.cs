using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Features.PostProcessingModule.Scripts.Rendering
{
	public class CameraWaterLensFeature : ScriptableRendererFeature
	{
		private class CameraWaterLensPass : ScriptableRenderPass
		{
			private class PassData
			{
				public Material material;

				public TextureHandle source;

				public TextureHandle destination;

				public float intensity;

				public float distortion;

				public float blur;

				public float flowSpeed;
			}

			private static readonly int _intensityId = Shader.PropertyToID("_CameraWaterIntensity");

			private static readonly int _distortionId = Shader.PropertyToID("_CameraWaterDistortion");

			private static readonly int _blurId = Shader.PropertyToID("_CameraWaterBlur");

			private static readonly int _flowSpeedId = Shader.PropertyToID("_CameraWaterFlowSpeed");

			private static readonly Vector4 _blitScaleBias = new Vector4(1f, 1f, 0f, 0f);

			private readonly Material _material;

			public CameraWaterLensPass(Material material, RenderPassEvent renderPassEvent)
			{
				_material = material;
				base.renderPassEvent = renderPassEvent;
			}

			public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
			{
				CameraWaterLensVolume component = VolumeManager.instance.stack.GetComponent<CameraWaterLensVolume>();
				if (component == null || !component.IsActive())
				{
					return;
				}
				UniversalResourceData universalResourceData = frameData.Get<UniversalResourceData>();
				RenderTextureDescriptor cameraTargetDescriptor = frameData.Get<UniversalCameraData>().cameraTargetDescriptor;
				cameraTargetDescriptor.depthBufferBits = 0;
				cameraTargetDescriptor.msaaSamples = 1;
				TextureHandle textureHandle = UniversalRenderer.CreateRenderGraphTexture(renderGraph, cameraTargetDescriptor, "_CameraWaterLensTempTexture", clear: false);
				PassData passData;
				using (IRasterRenderGraphBuilder rasterRenderGraphBuilder = renderGraph.AddRasterRenderPass<PassData>("CameraWaterLens_Apply", out passData, "C:\\Dev\\UnityProjects\\RubberArmsNew\\RubberArmsPrototype\\Assets\\Features\\PostProcessingModule\\Scripts\\Rendering\\CameraWaterLensFeature.cs", 47))
				{
					passData.material = _material;
					passData.source = universalResourceData.activeColorTexture;
					passData.destination = textureHandle;
					passData.intensity = component.intensity.value;
					passData.distortion = component.distortionStrength.value;
					passData.blur = component.blurStrength.value;
					passData.flowSpeed = component.flowSpeed.value;
					rasterRenderGraphBuilder.UseTexture(in passData.source);
					rasterRenderGraphBuilder.SetRenderAttachment(passData.destination, 0);
					rasterRenderGraphBuilder.AllowPassCulling(value: false);
					rasterRenderGraphBuilder.AllowGlobalStateModification(value: true);
					rasterRenderGraphBuilder.SetRenderFunc(delegate(PassData data, RasterGraphContext context)
					{
						data.material.SetFloat(_intensityId, data.intensity);
						data.material.SetFloat(_distortionId, data.distortion);
						data.material.SetFloat(_blurId, data.blur);
						data.material.SetFloat(_flowSpeedId, data.flowSpeed);
						Blitter.BlitTexture(context.cmd, data.source, _blitScaleBias, data.material, 0);
					});
				}
				PassData passData2;
				using IRasterRenderGraphBuilder rasterRenderGraphBuilder2 = renderGraph.AddRasterRenderPass<PassData>("CameraWaterLens_CopyBack", out passData2, "C:\\Dev\\UnityProjects\\RubberArmsNew\\RubberArmsPrototype\\Assets\\Features\\PostProcessingModule\\Scripts\\Rendering\\CameraWaterLensFeature.cs", 70);
				passData2.source = textureHandle;
				passData2.destination = universalResourceData.activeColorTexture;
				rasterRenderGraphBuilder2.UseTexture(in passData2.source);
				rasterRenderGraphBuilder2.SetRenderAttachment(passData2.destination, 0);
				rasterRenderGraphBuilder2.AllowPassCulling(value: false);
				rasterRenderGraphBuilder2.SetRenderFunc(delegate(PassData data, RasterGraphContext context)
				{
					Blitter.BlitTexture(context.cmd, data.source, _blitScaleBias, 0f, bilinear: false);
				});
			}
		}

		[SerializeField]
		private Shader _shader;

		[SerializeField]
		private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;

		private Material _material;

		private CameraWaterLensPass _pass;

		public override void Create()
		{
			if (_shader == null)
			{
				_shader = Shader.Find("Hidden/Custom/CameraWaterLens");
			}
			if (_shader != null && _material == null)
			{
				_material = CoreUtils.CreateEngineMaterial(_shader);
			}
			_pass = new CameraWaterLensPass(_material, _renderPassEvent);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (renderingData.cameraData.postProcessEnabled && (renderingData.cameraData.cameraType == CameraType.Game || renderingData.cameraData.cameraType == CameraType.SceneView) && !(_material == null))
			{
				CameraWaterLensVolume cameraWaterLensVolume = VolumeManager.instance.stack?.GetComponent<CameraWaterLensVolume>();
				if (!(cameraWaterLensVolume == null) && cameraWaterLensVolume.IsActive())
				{
					renderer.EnqueuePass(_pass);
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			CoreUtils.Destroy(_material);
		}
	}
}
