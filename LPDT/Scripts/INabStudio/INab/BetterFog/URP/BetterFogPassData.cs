using System;
using System.Collections.Generic;
using INab.BetterFog.Core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace INab.BetterFog.URP
{
	public class BetterFogPassData : ContextItem, IDisposable
	{
		private class CustomDepthData
		{
			public TextureHandle source;

			public TextureHandle destination;

			public Material material;

			public List<CustomRenderer> depthRenderers = new List<CustomRenderer>();
		}

		private class FogOffsetData
		{
			public TextureHandle source;

			public TextureHandle destination;

			public List<CustomRenderer> fogRenderers = new List<CustomRenderer>();

			public RendererListHandle rendererList;
		}

		private class PassData
		{
			public TextureHandle source;

			public TextureHandle destination;

			public TextureHandle m_FogFactorHandle;

			public TextureHandle customDepthHandle;

			public TextureHandle fogOffsetHandle;

			public Material m_FogFactorMaterial;

			public Material m_FogBlendMaterial;
		}

		private class PassDataSMSS
		{
			public Material material;

			public int iterations;

			public TextureHandle temporaryAfterBlendHandle;

			public TextureHandle prefiltered;

			public TextureHandle last;

			public TextureHandle cameraTarget;

			public TextureHandle fogFactor;

			public TextureHandle[] _blurBuffer1 = new TextureHandle[BetterFog.kMaxIterations];

			public TextureHandle[] _blurBuffer2 = new TextureHandle[BetterFog.kMaxIterations];
		}

		private static readonly int kFogFactor = Shader.PropertyToID("_FogFactorRT");

		private static readonly int kBlitTexture = Shader.PropertyToID("_BlitTexture");

		private static readonly int kBlitScaleBias = Shader.PropertyToID("_BlitScaleBias");

		private static MaterialPropertyBlock s_SharedPropertyBlock = new MaterialPropertyBlock();

		private RTHandle m_FogFactor;

		private TextureHandle m_FogFactorHandle;

		private RTHandle m_Temporary;

		private TextureHandle m_TemporaryHandle;

		private RTHandle m_TemporaryAfterBlend;

		private TextureHandle m_TemporaryAfterBlendHandle;

		private RTHandle m_CustomDepth;

		private TextureHandle m_CustomDepthHandle;

		private RTHandle m_FogOffset;

		private TextureHandle m_FogOffsetHandle;

		public void Init(RenderGraph renderGraph, RenderTextureDescriptor targetDescriptor, bool useSMSS)
		{
			RenderingUtils.ReAllocateHandleIfNeeded(ref m_Temporary, in targetDescriptor, FilterMode.Point, TextureWrapMode.Clamp, 1, 0f, "_TemporaryTexture");
			m_TemporaryHandle = renderGraph.ImportTexture(m_Temporary);
			if (useSMSS)
			{
				RenderingUtils.ReAllocateHandleIfNeeded(ref m_TemporaryAfterBlend, in targetDescriptor, FilterMode.Point, TextureWrapMode.Clamp, 1, 0f, "_TemporaryTextureAfterBlend");
				m_TemporaryAfterBlendHandle = renderGraph.ImportTexture(m_TemporaryAfterBlend);
			}
			RenderingUtils.ReAllocateHandleIfNeeded(ref m_FogOffset, in targetDescriptor, FilterMode.Point, TextureWrapMode.Clamp, 1, 0f, "_FogOffset");
			m_FogOffsetHandle = renderGraph.ImportTexture(m_FogOffset);
			targetDescriptor.colorFormat = RenderTextureFormat.RFloat;
			RenderingUtils.ReAllocateHandleIfNeeded(ref m_CustomDepth, in targetDescriptor, FilterMode.Point, TextureWrapMode.Clamp, 1, 0f, "_CustomDepth");
			m_CustomDepthHandle = renderGraph.ImportTexture(m_CustomDepth);
			targetDescriptor.colorFormat = RenderTextureFormat.RFloat;
			RenderingUtils.ReAllocateHandleIfNeeded(ref m_FogFactor, in targetDescriptor, FilterMode.Point, TextureWrapMode.Clamp, 1, 0f, "_FogFactor");
			m_FogFactorHandle = renderGraph.ImportTexture(m_FogFactor);
		}

		public override void Reset()
		{
			m_FogFactorHandle = TextureHandle.nullHandle;
			m_TemporaryHandle = TextureHandle.nullHandle;
			m_TemporaryAfterBlendHandle = TextureHandle.nullHandle;
		}

		private static void DrawRenderers(List<CustomRenderer> list, RasterCommandBuffer cmd)
		{
			foreach (CustomRenderer item in list)
			{
				if (!item.render)
				{
					continue;
				}
				Material material = item.material;
				Renderer renderer = item.renderer;
				if (!renderer || !material || (!item.alwaysRender && (!renderer.enabled || !renderer.gameObject.activeInHierarchy)))
				{
					continue;
				}
				if (item.drawAllSubmeshes && !(renderer is ParticleSystemRenderer))
				{
					Mesh mesh = null;
					if (renderer is SkinnedMeshRenderer)
					{
						mesh = (renderer as SkinnedMeshRenderer).sharedMesh;
					}
					else if (renderer is MeshRenderer)
					{
						mesh = renderer.GetComponent<MeshFilter>().sharedMesh;
					}
					for (int i = 0; i < mesh.subMeshCount; i++)
					{
						cmd.DrawRenderer(renderer, material, i, 0);
					}
				}
				else
				{
					cmd.DrawRenderer(renderer, material, 0, 0);
				}
			}
		}

		public void RecordCustomDepthBlit(RenderGraph renderGraph, ContextContainer frameData, Material material, List<CustomRenderer> depthRenderers)
		{
			if (depthRenderers == null)
			{
				return;
			}
			_ = frameData.Get<UniversalCameraData>().cameraTargetDescriptor;
			CustomDepthData passData;
			using IRasterRenderGraphBuilder rasterRenderGraphBuilder = renderGraph.AddRasterRenderPass<CustomDepthData>("Custom Depth Pass", out passData, "C:\\Dev\\UnityProjects\\RubberArmsNew\\RubberArmsPrototype\\Assets\\INab Studio\\Post Processing Assets\\Better Fog\\Core URP 6\\Scripts\\BetterFog.cs", 553);
			UniversalResourceData universalResourceData = frameData.Get<UniversalResourceData>();
			passData.depthRenderers = depthRenderers;
			passData.material = material;
			passData.source = universalResourceData.activeColorTexture;
			passData.destination = m_CustomDepthHandle;
			rasterRenderGraphBuilder.AllowPassCulling(value: false);
			rasterRenderGraphBuilder.SetRenderAttachment(passData.destination, 0);
			rasterRenderGraphBuilder.SetRenderFunc(delegate(CustomDepthData data, RasterGraphContext rgContext)
			{
				ExecuteCustomDepthBlitPass(data, rgContext);
			});
		}

		private static void ExecuteCustomDepthBlitPass(CustomDepthData data, RasterGraphContext rgContext)
		{
			Blitter.BlitTexture(rgContext.cmd, data.source, new Vector4(1f, 1f, 0f, 0f), data.material, 0);
			rgContext.cmd.ClearRenderTarget(clearDepth: true, clearColor: false, Color.black);
			DrawRenderers(data.depthRenderers, rgContext.cmd);
		}

		public void RecordFogOffsetBlit(RenderGraph renderGraph, ContextContainer frameData, List<CustomRenderer> fogRenderers)
		{
			UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
			UniversalRenderingData universalRenderingData = frameData.Get<UniversalRenderingData>();
			UniversalLightData lightData = frameData.Get<UniversalLightData>();
			UniversalResourceData universalResourceData = frameData.Get<UniversalResourceData>();
			FogOffsetData passData;
			using IRasterRenderGraphBuilder rasterRenderGraphBuilder = renderGraph.AddRasterRenderPass<FogOffsetData>("Fog Offset Pass", out passData, "C:\\Dev\\UnityProjects\\RubberArmsNew\\RubberArmsPrototype\\Assets\\INab Studio\\Post Processing Assets\\Better Fog\\Core URP 6\\Scripts\\BetterFog.cs", 597);
			passData.fogRenderers = fogRenderers;
			passData.source = universalResourceData.activeColorTexture;
			passData.destination = m_FogOffsetHandle;
			DrawingSettings drawSettings = RenderingUtils.CreateDrawingSettings(new ShaderTagId("FogMask"), universalRenderingData, cameraData, lightData, SortingCriteria.CommonTransparent);
			RendererListParams desc = new RendererListParams(filteringSettings: new FilteringSettings(RenderQueueRange.all), cullingResults: universalRenderingData.cullResults, drawSettings: drawSettings);
			passData.rendererList = renderGraph.CreateRendererList(in desc);
			rasterRenderGraphBuilder.UseRendererList(in passData.rendererList);
			rasterRenderGraphBuilder.AllowPassCulling(value: false);
			rasterRenderGraphBuilder.SetRenderAttachment(passData.destination, 0);
			rasterRenderGraphBuilder.SetRenderFunc(delegate(FogOffsetData data, RasterGraphContext ctx)
			{
				ExecuteFogOffsetPass(data, ctx);
			});
		}

		private static void ExecuteFogOffsetPass(FogOffsetData data, RasterGraphContext rgContext)
		{
			rgContext.cmd.ClearRenderTarget(clearDepth: true, clearColor: true, Color.black);
			if (data.fogRenderers != null && data.fogRenderers.Count > 0)
			{
				DrawRenderers(data.fogRenderers, rgContext.cmd);
			}
			if (data.rendererList.IsValid())
			{
				rgContext.cmd.DrawRendererList(data.rendererList);
			}
		}

		public void RecordTemporaryBlit(RenderGraph renderGraph, ContextContainer frameData, bool useSMSS)
		{
			RenderTextureDescriptor cameraTargetDescriptor = frameData.Get<UniversalCameraData>().cameraTargetDescriptor;
			if (!m_TemporaryHandle.IsValid())
			{
				cameraTargetDescriptor.msaaSamples = 1;
				cameraTargetDescriptor.depthBufferBits = 0;
				Init(renderGraph, cameraTargetDescriptor, useSMSS);
			}
			PassData passData;
			using IRasterRenderGraphBuilder rasterRenderGraphBuilder = renderGraph.AddRasterRenderPass<PassData>("Better Fog Temporary Blit", out passData, "C:\\Dev\\UnityProjects\\RubberArmsNew\\RubberArmsPrototype\\Assets\\INab Studio\\Post Processing Assets\\Better Fog\\Core URP 6\\Scripts\\BetterFog.cs", 661);
			UniversalResourceData universalResourceData = frameData.Get<UniversalResourceData>();
			passData.source = universalResourceData.activeColorTexture;
			passData.destination = m_TemporaryHandle;
			rasterRenderGraphBuilder.UseTexture(in passData.source);
			rasterRenderGraphBuilder.SetRenderAttachment(passData.destination, 0);
			rasterRenderGraphBuilder.SetRenderFunc(delegate(PassData data, RasterGraphContext rgContext)
			{
				ExecuteTemporaryBlitPass(data, rgContext);
			});
		}

		private static void ExecuteTemporaryBlitPass(PassData data, RasterGraphContext rgContext)
		{
			Blitter.BlitTexture(rgContext.cmd, data.source, new Vector4(1f, 1f, 0f, 0f), 0f, bilinear: false);
		}

		public void RecordFogFactorPass(RenderGraph renderGraph, ContextContainer frameData, Material material)
		{
			UniversalCameraData universalCameraData = frameData.Get<UniversalCameraData>();
			_ = universalCameraData.cameraTargetDescriptor;
			PassData passData;
			using IRasterRenderGraphBuilder rasterRenderGraphBuilder = renderGraph.AddRasterRenderPass<PassData>("Fog Factor Pass", out passData, "C:\\Dev\\UnityProjects\\RubberArmsNew\\RubberArmsPrototype\\Assets\\INab Studio\\Post Processing Assets\\Better Fog\\Core URP 6\\Scripts\\BetterFog.cs", 689);
			frameData.Get<UniversalResourceData>();
			material.SetMatrix("_InverseView", universalCameraData.camera.cameraToWorldMatrix);
			passData.m_FogFactorMaterial = material;
			passData.customDepthHandle = m_CustomDepthHandle;
			passData.fogOffsetHandle = m_FogOffsetHandle;
			passData.destination = m_FogFactorHandle;
			rasterRenderGraphBuilder.SetRenderAttachment(passData.destination, 0);
			rasterRenderGraphBuilder.SetRenderFunc(delegate(PassData data, RasterGraphContext rgContext)
			{
				ExecuteFogFactorPass(data, rgContext);
			});
		}

		private static void ExecuteFogFactorPass(PassData data, RasterGraphContext context)
		{
			s_SharedPropertyBlock.Clear();
			if (data.customDepthHandle.IsValid())
			{
				s_SharedPropertyBlock.SetTexture("_CustomDepth", data.customDepthHandle);
			}
			if (data.fogOffsetHandle.IsValid())
			{
				s_SharedPropertyBlock.SetTexture("_FogOffset", data.fogOffsetHandle);
			}
			context.cmd.DrawProcedural(Matrix4x4.identity, data.m_FogFactorMaterial, 0, MeshTopology.Triangles, 3, 1, s_SharedPropertyBlock);
		}

		public void RecordFogBlendPass(RenderGraph renderGraph, ContextContainer frameData, Material material, bool useSMSS)
		{
			UniversalCameraData universalCameraData = frameData.Get<UniversalCameraData>();
			_ = universalCameraData.cameraTargetDescriptor;
			PassData passData;
			using IRasterRenderGraphBuilder rasterRenderGraphBuilder = renderGraph.AddRasterRenderPass<PassData>("Fog Blend Pass", out passData, "C:\\Dev\\UnityProjects\\RubberArmsNew\\RubberArmsPrototype\\Assets\\INab Studio\\Post Processing Assets\\Better Fog\\Core URP 6\\Scripts\\BetterFog.cs", 727);
			UniversalResourceData universalResourceData = frameData.Get<UniversalResourceData>();
			material.SetMatrix("_InverseView", universalCameraData.camera.cameraToWorldMatrix);
			passData.m_FogBlendMaterial = material;
			passData.m_FogFactorHandle = m_FogFactorHandle;
			rasterRenderGraphBuilder.UseTexture(in m_FogFactorHandle);
			passData.source = m_TemporaryHandle;
			if (!useSMSS)
			{
				passData.destination = universalResourceData.activeColorTexture;
			}
			else
			{
				passData.destination = m_TemporaryAfterBlendHandle;
			}
			rasterRenderGraphBuilder.UseTexture(in passData.source);
			rasterRenderGraphBuilder.SetRenderAttachment(passData.destination, 0);
			rasterRenderGraphBuilder.SetRenderFunc(delegate(PassData data, RasterGraphContext rgContext)
			{
				ExecuteFogBlendPass(data, rgContext);
			});
		}

		private static void ExecuteFogBlendPass(PassData data, RasterGraphContext context)
		{
			s_SharedPropertyBlock.Clear();
			if (data.source.IsValid())
			{
				s_SharedPropertyBlock.SetTexture(kBlitTexture, data.source);
			}
			if (data.m_FogFactorHandle.IsValid())
			{
				s_SharedPropertyBlock.SetTexture(kFogFactor, data.m_FogFactorHandle);
			}
			s_SharedPropertyBlock.SetVector(kBlitScaleBias, new Vector4(1f, 1f, 0f, 0f));
			context.cmd.DrawProcedural(Matrix4x4.identity, data.m_FogBlendMaterial, 0, MeshTopology.Triangles, 3, 1, s_SharedPropertyBlock);
		}

		public void RecordSMSSPass(RenderGraph renderGraph, ContextContainer frameData, Material material, int iterations)
		{
			PassDataSMSS passData;
			using IUnsafeRenderGraphBuilder unsafeRenderGraphBuilder = renderGraph.AddUnsafePass<PassDataSMSS>("SMSS Pass", out passData, "C:\\Dev\\UnityProjects\\RubberArmsNew\\RubberArmsPrototype\\Assets\\INab Studio\\Post Processing Assets\\Better Fog\\Core URP 6\\Scripts\\BetterFog.cs", 788);
			UniversalResourceData universalResourceData = frameData.Get<UniversalResourceData>();
			RenderTextureDescriptor cameraTargetDescriptor = frameData.Get<UniversalCameraData>().cameraTargetDescriptor;
			cameraTargetDescriptor.msaaSamples = 1;
			cameraTargetDescriptor.depthBufferBits = 0;
			passData.temporaryAfterBlendHandle = m_TemporaryAfterBlendHandle;
			passData.cameraTarget = universalResourceData.activeColorTexture;
			passData.fogFactor = m_FogFactorHandle;
			passData.material = material;
			passData.iterations = iterations;
			TextureHandle prefiltered = UniversalRenderer.CreateRenderGraphTexture(renderGraph, cameraTargetDescriptor, "_Prefiltered_Unsafe", clear: false);
			passData.prefiltered = prefiltered;
			int num = cameraTargetDescriptor.width;
			int num2 = cameraTargetDescriptor.height;
			for (int i = 0; i < iterations; i++)
			{
				num = Mathf.Max(num / 2, 1);
				num2 = Mathf.Max(num2 / 2, 1);
				cameraTargetDescriptor.width = num;
				cameraTargetDescriptor.height = num2;
				TextureHandle textureHandle = UniversalRenderer.CreateRenderGraphTexture(renderGraph, cameraTargetDescriptor, "_BlurBuffer_" + i, clear: false, FilterMode.Bilinear);
				TextureHandle textureHandle2 = UniversalRenderer.CreateRenderGraphTexture(renderGraph, cameraTargetDescriptor, "_BlurBuffer_" + (i + 1), clear: false, FilterMode.Bilinear);
				passData._blurBuffer1[i] = textureHandle;
				passData._blurBuffer2[i] = textureHandle2;
				unsafeRenderGraphBuilder.UseTexture(in passData._blurBuffer1[i], AccessFlags.Write);
				unsafeRenderGraphBuilder.UseTexture(in passData._blurBuffer2[i], AccessFlags.Write);
			}
			unsafeRenderGraphBuilder.UseTexture(in passData.temporaryAfterBlendHandle);
			unsafeRenderGraphBuilder.UseTexture(in passData.cameraTarget, AccessFlags.Write);
			unsafeRenderGraphBuilder.UseTexture(in passData.prefiltered, AccessFlags.Write);
			unsafeRenderGraphBuilder.UseTexture(in passData.fogFactor);
			unsafeRenderGraphBuilder.AllowPassCulling(value: false);
			unsafeRenderGraphBuilder.SetRenderFunc(delegate(PassDataSMSS data, UnsafeGraphContext context)
			{
				ExecuteSMSSPass(data, context);
			});
		}

		private static void ExecuteSMSSPass(PassDataSMSS data, UnsafeGraphContext context)
		{
			CommandBuffer nativeCommandBuffer = CommandBufferHelpers.GetNativeCommandBuffer(context.cmd);
			context.cmd.SetGlobalTexture("_FogFactor_RT", data.fogFactor);
			int pass = 0;
			context.cmd.SetRenderTarget(data.prefiltered);
			Blitter.BlitTexture(nativeCommandBuffer, data.temporaryAfterBlendHandle, new Vector4(1f, 1f, 0f, 0f), data.material, pass);
			data.last = data.prefiltered;
			for (int i = 0; i < data.iterations; i++)
			{
				pass = ((i == 0) ? 1 : 2);
				context.cmd.SetRenderTarget(data._blurBuffer1[i]);
				Blitter.BlitTexture(nativeCommandBuffer, data.last, new Vector4(1f, 1f, 0f, 0f), data.material, pass);
				data.last = data._blurBuffer1[i];
			}
			for (int num = data.iterations - 2; num >= 0; num--)
			{
				TextureHandle value = data._blurBuffer1[num];
				context.cmd.SetGlobalTexture("_BaseTextureUpscale", value);
				pass = 3;
				context.cmd.SetRenderTarget(data._blurBuffer2[num]);
				Blitter.BlitTexture(nativeCommandBuffer, data.last, new Vector4(1f, 1f, 0f, 0f), data.material, pass);
				data.last = data._blurBuffer2[num];
			}
			context.cmd.SetRenderTarget(data.cameraTarget);
			Blitter.BlitTexture(nativeCommandBuffer, data.last, new Vector4(1f, 1f, 0f, 0f), 0f, bilinear: false);
			context.cmd.SetGlobalTexture("_BaseTexture", data.temporaryAfterBlendHandle);
			pass = 4;
			context.cmd.SetRenderTarget(data.cameraTarget);
			Blitter.BlitTexture(nativeCommandBuffer, data.last, new Vector4(1f, 1f, 0f, 0f), data.material, pass);
		}

		public void Dispose()
		{
			m_FogFactor?.Release();
			m_Temporary?.Release();
			m_TemporaryAfterBlend?.Release();
			m_CustomDepth?.Release();
			m_FogOffset?.Release();
		}
	}
}
