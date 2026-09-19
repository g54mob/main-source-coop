using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Features.LevelLightModule.Scripts.Rendering
{
	public class CustomFogFeature : ScriptableRendererFeature
	{
		private class CustomFogPass : ScriptableRenderPass
		{
			private class MaskPassData
			{
				public RendererListHandle rendererList;
			}

			private class FogPassData
			{
				public Material material;

				public TextureHandle maskTexture;
			}

			private static readonly int FogColorId = Shader.PropertyToID("_FogColor");

			private static readonly int FogParamsId = Shader.PropertyToID("_FogParams");

			private readonly Material _fogMaterial;

			private readonly ShaderTagId maskShaderTag = new ShaderTagId("FogMask");

			public CustomFogPass(Material mat, RenderPassEvent renderPass)
			{
				_fogMaterial = mat;
				base.renderPassEvent = renderPass;
			}

			public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
			{
				CustomFogVolume component = VolumeManager.instance.stack.GetComponent<CustomFogVolume>();
				if (component == null || !component.IsActive())
				{
					return;
				}
				Shader.SetGlobalColor(FogColorId, component.fogColor.value);
				Shader.SetGlobalVector(FogParamsId, new Vector4(component.fogStart.value, component.fogEnd.value, component.fogPushEnd.value, 0f));
				UniversalResourceData universalResourceData = frameData.Get<UniversalResourceData>();
				UniversalCameraData universalCameraData = frameData.Get<UniversalCameraData>();
				UniversalRenderingData universalRenderingData = frameData.Get<UniversalRenderingData>();
				RenderTextureDescriptor cameraTargetDescriptor = universalCameraData.cameraTargetDescriptor;
				cameraTargetDescriptor.colorFormat = RenderTextureFormat.ARGBHalf;
				cameraTargetDescriptor.depthBufferBits = 0;
				cameraTargetDescriptor.msaaSamples = 1;
				RenderTextureDescriptor cameraTargetDescriptor2 = universalCameraData.cameraTargetDescriptor;
				cameraTargetDescriptor2.colorFormat = RenderTextureFormat.Depth;
				cameraTargetDescriptor2.depthBufferBits = 32;
				cameraTargetDescriptor2.msaaSamples = 1;
				TextureHandle textureHandle = UniversalRenderer.CreateRenderGraphTexture(renderGraph, cameraTargetDescriptor, "_FogMaskTexture", clear: true);
				TextureHandle tex = UniversalRenderer.CreateRenderGraphTexture(renderGraph, cameraTargetDescriptor2, "_FogMaskDepth", clear: true);
				SortingSettings sortingSettings = new SortingSettings(universalCameraData.camera);
				sortingSettings.criteria = universalCameraData.defaultOpaqueSortFlags;
				SortingSettings sortingSettings2 = sortingSettings;
				RendererListHandle rendererList = renderGraph.CreateRendererList(new RendererListParams(drawSettings: new DrawingSettings(maskShaderTag, sortingSettings2), filteringSettings: new FilteringSettings(RenderQueueRange.opaque), cullingResults: universalRenderingData.cullResults));
				MaskPassData passData;
				using (IRasterRenderGraphBuilder rasterRenderGraphBuilder = renderGraph.AddRasterRenderPass<MaskPassData>("CustomFog_RenderMasks", out passData, "C:\\Dev\\UnityProjects\\RubberArmsNew\\RubberArmsPrototype\\Assets\\Features\\LevelLightModule\\Scripts\\Rendering\\CustomFogFeature.cs", 72))
				{
					passData.rendererList = rendererList;
					rasterRenderGraphBuilder.UseRendererList(in rendererList);
					rasterRenderGraphBuilder.SetRenderAttachment(textureHandle, 0);
					rasterRenderGraphBuilder.SetRenderAttachmentDepth(tex);
					rasterRenderGraphBuilder.AllowPassCulling(value: false);
					rasterRenderGraphBuilder.SetRenderFunc(delegate(MaskPassData data, RasterGraphContext context)
					{
						context.cmd.ClearRenderTarget(clearDepth: true, clearColor: true, Color.clear);
						context.cmd.DrawRendererList(data.rendererList);
					});
				}
				FogPassData passData2;
				using IRasterRenderGraphBuilder rasterRenderGraphBuilder2 = renderGraph.AddRasterRenderPass<FogPassData>("CustomFog_Apply", out passData2, "C:\\Dev\\UnityProjects\\RubberArmsNew\\RubberArmsPrototype\\Assets\\Features\\LevelLightModule\\Scripts\\Rendering\\CustomFogFeature.cs", 91);
				passData2.material = _fogMaterial;
				passData2.maskTexture = textureHandle;
				rasterRenderGraphBuilder2.UseTexture(in textureHandle);
				rasterRenderGraphBuilder2.SetRenderAttachment(universalResourceData.activeColorTexture, 0, AccessFlags.ReadWrite);
				rasterRenderGraphBuilder2.AllowPassCulling(value: false);
				rasterRenderGraphBuilder2.AllowGlobalStateModification(value: true);
				rasterRenderGraphBuilder2.SetRenderFunc(delegate(FogPassData data, RasterGraphContext context)
				{
					context.cmd.SetGlobalTexture("_FogMaskTexture", data.maskTexture);
					context.cmd.DrawProcedural(Matrix4x4.identity, data.material, 0, MeshTopology.Triangles, 3);
				});
			}
		}

		[SerializeField]
		private Material _fogMaterial;

		[SerializeField]
		private RenderPassEvent _renderPass;

		private CustomFogPass _pass;

		public override void Create()
		{
			_pass = new CustomFogPass(_fogMaterial, _renderPass);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (!(_fogMaterial == null) && (renderingData.cameraData.cameraType == CameraType.Game || renderingData.cameraData.cameraType == CameraType.SceneView))
			{
				renderer.EnqueuePass(_pass);
			}
		}
	}
}
