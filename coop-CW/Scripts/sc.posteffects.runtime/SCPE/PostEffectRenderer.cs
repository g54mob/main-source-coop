using System.Reflection;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	[DisallowMultipleRendererFeature(null)]
	public class PostEffectRenderer<T> : ScriptableRenderPass
	{
		public bool render;

		public T volumeSettings;

		public EffectBaseSettings settings;

		private static bool is2D;

		private static bool isDeferred;

		private bool xrRendering;

		protected bool requiresDepth;

		protected bool requiresDepthNormals;

		protected string shaderName;

		private Shader shader;

		protected string ProfilerTag;

		protected Material Material;

		private static Material _BlitMaterial;

		private static RTHandle cameraColorSource;

		protected RTHandle cameraColorTarget;

		private RenderTextureDescriptor cameraTargetRtDsc;

		private static RenderTextureDescriptor tempRTDesc;

		private int mainTexID = Shader.PropertyToID("_MainTex");

		public bool reconstructDepthNormals;

		private int depthNormalsID = Shader.PropertyToID("_CameraDepthNormalsTexture");

		private static RTHandle cameraNormalsTexture;

		private readonly ProfilingSampler bufferCopyProfiler = new ProfilingSampler("Copy color");

		private readonly ProfilingSampler depthNormalsProfiler = new ProfilingSampler("Reconstruct normals from depth");

		private static int prevRendererID = -1;

		private static int currentRendererID = -1;

		private ScriptableRenderPassInput requirements;

		private Material DepthNormalsMat;

		private static Shader DepthNormalsShader;

		private static Vector4 ScaleBias = new Vector4(1f, 1f, 0f, 0f);

		private static bool isPlaying;

		private static Matrix4x4 lightToLocalMatrix;

		private static readonly int viewProjection = Shader.PropertyToID("viewProjection");

		private static readonly int viewMatrix = Shader.PropertyToID("viewMatrix");

		private static Matrix4x4[] s_viewProjectionMatrices = new Matrix4x4[2];

		private static readonly int viewProjectionArray = Shader.PropertyToID("viewProjectionArray");

		private bool RequireBufferCopy
		{
			get
			{
				if (!is2D && !xrRendering)
				{
					return base.renderPassEvent == RenderPassEvent.BeforeRenderingTransparents;
				}
				return true;
			}
		}

		private static Material BlitMaterial
		{
			get
			{
				if (!_BlitMaterial)
				{
					_BlitMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Hidden/Universal Render Pipeline/Blit"));
				}
				return _BlitMaterial;
			}
		}

		internal string GetProfilerTag()
		{
			return shaderName.Replace("Hidden/SC Post Effects/", "SCPE ");
		}

		private void DetermineRendererType(ScriptableRenderer renderer)
		{
			currentRendererID = renderer.GetHashCode();
			if (currentRendererID != prevRendererID)
			{
				prevRendererID = currentRendererID;
				is2D = renderer.GetType() != typeof(UniversalRenderer);
				if (!is2D)
				{
					ScriptableRendererData[] obj = (ScriptableRendererData[])typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(UniversalRenderPipeline.asset);
					int num = (int)typeof(UniversalRenderPipelineAsset).GetField("m_DefaultRendererIndex", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(UniversalRenderPipeline.asset);
					isDeferred = ((UniversalRendererData)obj[num]).renderingMode == RenderingMode.Deferred;
				}
			}
		}

		public virtual void Setup(ScriptableRenderer renderer, RenderingData renderingData)
		{
			render = true;
			if (volumeSettings == null)
			{
				render = false;
				return;
			}
			if (!shader)
			{
				shader = Shader.Find(shaderName);
			}
			if (!ShouldRenderForCamera(renderingData))
			{
				render = false;
			}
			else if (render)
			{
				DetermineRendererType(renderer);
			}
		}

		public RTHandle GetCameraTarget(ScriptableRenderer renderer)
		{
			return cameraColorTarget;
		}

		public void SetCameraTarget(ScriptableRenderer renderer)
		{
			cameraColorTarget = renderer.cameraColorTargetHandle;
		}

		private bool IsAllowedCameraType(EffectBaseSettings.CameraTypeFlags flag)
		{
			return (settings.cameraTypes & flag) == flag;
		}

		public bool ShouldRenderForCamera(RenderingData renderingData)
		{
			if (!renderingData.postProcessingEnabled && !settings.alwaysEnable)
			{
				return false;
			}
			if (renderingData.cameraData.camera.cameraType == CameraType.Game)
			{
				if (renderingData.cameraData.camera.hideFlags != HideFlags.None && !IsAllowedCameraType(EffectBaseSettings.CameraTypeFlags.Hidden))
				{
					return false;
				}
				if (renderingData.cameraData.renderType == CameraRenderType.Base && !IsAllowedCameraType(EffectBaseSettings.CameraTypeFlags.GameBase))
				{
					return false;
				}
				if (renderingData.cameraData.renderType == CameraRenderType.Overlay && !IsAllowedCameraType(EffectBaseSettings.CameraTypeFlags.GameOverlay))
				{
					return false;
				}
			}
			if (renderingData.cameraData.camera.cameraType == CameraType.Reflection && !IsAllowedCameraType(EffectBaseSettings.CameraTypeFlags.Reflection))
			{
				return false;
			}
			if (renderingData.cameraData.camera.cameraType == CameraType.Preview && !IsAllowedCameraType(EffectBaseSettings.CameraTypeFlags.Preview))
			{
				return false;
			}
			return true;
		}

		private void CreateMaterialIfNull(ref Material material, Shader m_shader)
		{
			if (!material)
			{
				material = CoreUtils.CreateEngineMaterial(m_shader);
				material.hideFlags = HideFlags.DontSave;
				material.name = m_shader.name;
			}
		}

		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			xrRendering = renderingData.cameraData.xrRendering;
		}

		public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
		{
			cameraTargetRtDsc = cameraTextureDescriptor;
			cameraTargetRtDsc.msaaSamples = 1;
			ConfigurePass(cmd, cameraTargetRtDsc);
		}

		private static RTHandle AllocateRT(RenderTextureDescriptor cameraTextureDescriptor, GraphicsFormat format, FilterMode filterMode, string name, int downsampling = 1)
		{
			return RTHandles.Alloc(cameraTextureDescriptor.width / downsampling, cameraTextureDescriptor.height / downsampling, cameraTextureDescriptor.volumeDepth, DepthBits.None, format, filterMode, TextureWrapMode.Clamp, cameraTextureDescriptor.dimension, enableRandomWrite: false, useMipMap: false, autoGenerateMips: true, isShadowMap: false, 1, 0f, MSAASamples.None, bindTextureMS: false, useDynamicScale: false, useDynamicScaleExplicit: false, RenderTextureMemoryless.None, VRTextureUsage.None, name);
		}

		public static void ReleaseRT(RTHandle handle)
		{
			RTHandles.Release(handle);
		}

		private static bool RTHandleNeedsReAlloc(RTHandle handle, in RenderTextureDescriptor descriptor, in string name)
		{
			return true;
		}

		public static RTHandle GetTemporaryRT(ref RTHandle handle, RenderTextureDescriptor cameraTextureDescriptor, GraphicsFormat format, FilterMode filterMode, string name, int downsampling = 1)
		{
			tempRTDesc = cameraTextureDescriptor;
			if (downsampling > 1)
			{
				tempRTDesc.width /= downsampling;
				tempRTDesc.height /= downsampling;
			}
			if (RTHandleNeedsReAlloc(handle, in tempRTDesc, in name))
			{
				if (handle != null)
				{
					ReleaseRT(handle);
				}
				handle = AllocateRT(cameraTextureDescriptor, format, filterMode, name, downsampling);
			}
			return handle;
		}

		protected virtual void ConfigurePass(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
		{
			CreateMaterialIfNull(ref Material, shader);
			if (RequireBufferCopy)
			{
				cameraColorSource = GetTemporaryRT(ref cameraColorSource, cameraTextureDescriptor, cameraTextureDescriptor.graphicsFormat, FilterMode.Point, ProfilerTag + "_CameraColorSource");
			}
			requirements = ScriptableRenderPassInput.None;
			if (requiresDepth)
			{
				requirements = ScriptableRenderPassInput.Depth;
			}
			if (requiresDepthNormals && !reconstructDepthNormals)
			{
				requirements |= ScriptableRenderPassInput.Normal;
			}
			ConfigureInput(requirements);
			CoreUtils.SetKeyword(Material, "_RECONSTRUCT_NORMAL", reconstructDepthNormals);
			cmd.SetGlobalInt(ShaderParameters._DeferredRendering, isDeferred ? 1 : 0);
			if (requiresDepthNormals && reconstructDepthNormals && !isDeferred)
			{
				if (!DepthNormalsShader)
				{
					DepthNormalsShader = Shader.Find("Hidden/SC Post Effects/DepthNormals");
				}
				CreateMaterialIfNull(ref DepthNormalsMat, DepthNormalsShader);
				cameraNormalsTexture = GetTemporaryRT(ref cameraNormalsTexture, cameraTextureDescriptor, GraphicsFormat.R8G8_UNorm, FilterMode.Point, "_CameraDepthNormalsTexture");
				cmd.SetGlobalTexture(depthNormalsID, cameraNormalsTexture);
			}
		}

		protected CommandBuffer GetCommandBuffer(ref RenderingData renderingData)
		{
			return CommandBufferPool.Get(ProfilerTag);
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
		}

		private void GetCameraColorTarget(RenderingData renderingData)
		{
			cameraColorTarget = renderingData.cameraData.renderer.cameraColorTargetHandle;
		}

		protected void CopyTargets(CommandBuffer cmd, RenderingData renderingData)
		{
			GetCameraColorTarget(renderingData);
			if (RequireBufferCopy)
			{
				using (new ProfilingScope(cmd, bufferCopyProfiler))
				{
					BlitCopy(cmd, cameraColorTarget, cameraColorSource);
				}
			}
			GenerateDepthNormals(this, cmd);
		}

		private void GenerateDepthNormals(ScriptableRenderPass pass, CommandBuffer cmd)
		{
			if (!requiresDepthNormals || isDeferred || !reconstructDepthNormals)
			{
				return;
			}
			using (new ProfilingScope(cmd, depthNormalsProfiler))
			{
				Blit(pass, cmd, cameraNormalsTexture, cameraNormalsTexture, DepthNormalsMat, 0);
			}
		}

		protected void BlitCopy(CommandBuffer cmd, RTHandle source, RTHandle dest)
		{
			cmd.SetGlobalTexture("_BlitTexture", source);
			Blit(this, cmd, source, dest, BlitMaterial, 0);
		}

		protected void Blit(ScriptableRenderPass pass, CommandBuffer cmd, RTHandle source, RTHandle target, Material mat, int passIndex, bool clearColor = false)
		{
			cmd.SetGlobalTexture(mainTexID, source);
			cmd.SetRenderTarget(target, 0, CubemapFace.Unknown, -1);
			if (clearColor)
			{
				cmd.ClearRenderTarget(clearDepth: true, clearColor: true, Color.clear);
			}
			cmd.SetGlobalVector(ShaderParameters._BlitScaleBiasRt, ScaleBias);
			cmd.SetGlobalVector(ShaderParameters._BlitScaleBias, ScaleBias);
			if (xrRendering)
			{
				cmd.DrawProcedural(Matrix4x4.identity, mat, passIndex, MeshTopology.Quads, 4, 1, null);
			}
			else
			{
				cmd.Blit(source, target, mat, passIndex);
			}
		}

		protected void FinalBlit(ScriptableRenderPass pass, ScriptableRenderContext context, CommandBuffer cmd, RenderingData renderingData, int passIndex)
		{
			if (RequireBufferCopy)
			{
				Blit(pass, cmd, cameraColorSource, cameraColorTarget, Material, passIndex);
			}
			else
			{
				cmd.SetGlobalTexture(mainTexID, cameraColorTarget);
				pass.Blit(cmd, ref renderingData, Material, passIndex);
			}
			context.ExecuteCommandBuffer(cmd);
			cmd.Clear();
			CommandBufferPool.Release(cmd);
		}

		public override void OnCameraCleanup(CommandBuffer cmd)
		{
			if (RequireBufferCopy && ShouldReleaseRT())
			{
				ReleaseRT(cameraColorSource);
			}
			if (requiresDepthNormals && ShouldReleaseRT())
			{
				ReleaseRT(cameraNormalsTexture);
			}
		}

		protected bool ShouldReleaseRT()
		{
			isPlaying = Application.isPlaying;
			return isPlaying;
		}

		public void Dispose()
		{
			CoreUtils.Destroy(Material);
			if (requiresDepthNormals)
			{
				CoreUtils.Destroy(DepthNormalsMat);
			}
		}

		public void SetMainLightProjection(CommandBuffer cmd, RenderingData renderingData)
		{
			if (renderingData.lightData.mainLightIndex > -1)
			{
				VisibleLight visibleLight = renderingData.lightData.visibleLights[renderingData.lightData.mainLightIndex];
				if (visibleLight.lightType == LightType.Directional)
				{
					lightToLocalMatrix = visibleLight.light.transform.worldToLocalMatrix;
					cmd.SetGlobalMatrix(ShaderParameters.unity_WorldToLight, lightToLocalMatrix);
				}
			}
		}

		protected void SetViewProjectionMatrixUniforms(CommandBuffer cmd, in CameraData cameraData)
		{
			cmd.SetGlobalMatrix(viewProjection, GL.GetGPUProjectionMatrix(cameraData.GetProjectionMatrix(), cameraData.IsCameraProjectionMatrixFlipped()) * cameraData.GetViewMatrix());
			cmd.SetGlobalMatrix(viewMatrix, cameraData.camera.cameraToWorldMatrix);
		}
	}
}
