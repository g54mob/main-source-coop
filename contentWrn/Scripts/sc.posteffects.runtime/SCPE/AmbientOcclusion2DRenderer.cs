using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	public class AmbientOcclusion2DRenderer : ScriptableRendererFeature
	{
		private class AmbientOcclusion2DRenderPass : PostEffectRenderer<AmbientOcclusion2D>
		{
			private enum Pass
			{
				LuminanceDiff = 0,
				Blur = 1,
				Blend = 2,
				Debug = 3
			}

			private int aoTexID = Shader.PropertyToID("_AO");

			private RTHandle ao;

			private RTHandle blurBuffer1;

			private RTHandle blurBuffer2;

			public AmbientOcclusion2DRenderPass(EffectBaseSettings settings)
			{
				base.settings = settings;
				base.renderPassEvent = settings.GetInjectionPoint();
				shaderName = "Hidden/SC Post Effects/Ambient Occlusion 2D";
				ProfilerTag = GetProfilerTag();
			}

			public override void Setup(ScriptableRenderer renderer, RenderingData renderingData)
			{
				volumeSettings = VolumeManager.instance.stack.GetComponent<AmbientOcclusion2D>();
				base.Setup(renderer, renderingData);
				if (render && volumeSettings.IsActive())
				{
					cameraColorTarget = GetCameraTarget(renderer);
					renderer.EnqueuePass(this);
				}
			}

			protected override void ConfigurePass(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				base.ConfigurePass(cmd, cameraTextureDescriptor);
				ao = PostEffectRenderer<AmbientOcclusion2D>.GetTemporaryRT(ref ao, cameraTextureDescriptor, GraphicsFormat.R8_UNorm, FilterMode.Bilinear, "ao", volumeSettings.downscaling.value);
				blurBuffer1 = PostEffectRenderer<AmbientOcclusion2D>.GetTemporaryRT(ref blurBuffer1, cameraTextureDescriptor, GraphicsFormat.R8_UNorm, FilterMode.Bilinear, "BlurBuffer1", volumeSettings.downscaling.value);
				blurBuffer2 = PostEffectRenderer<AmbientOcclusion2D>.GetTemporaryRT(ref blurBuffer2, cameraTextureDescriptor, GraphicsFormat.R8_UNorm, FilterMode.Bilinear, "BlurBuffer2", volumeSettings.downscaling.value);
			}

			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				CommandBuffer commandBuffer = GetCommandBuffer(ref renderingData);
				CopyTargets(commandBuffer, renderingData);
				commandBuffer.SetGlobalFloat("_SampleDistance", volumeSettings.distance.value);
				float value = ((QualitySettings.activeColorSpace == ColorSpace.Gamma) ? Mathf.GammaToLinearSpace(volumeSettings.luminanceThreshold.value) : volumeSettings.luminanceThreshold.value);
				commandBuffer.SetGlobalFloat("_Threshold", value);
				commandBuffer.SetGlobalFloat("_Blur", volumeSettings.blurAmount.value);
				commandBuffer.SetGlobalFloat("_Intensity", volumeSettings.intensity.value);
				Blit(commandBuffer, cameraColorTarget, ao, Material);
				BlitCopy(commandBuffer, ao, blurBuffer1);
				for (int i = 0; i < volumeSettings.iterations.value; i++)
				{
					commandBuffer.SetGlobalVector(ShaderParameters.BlurOffsets, new Vector4(volumeSettings.blurAmount.value / (float)renderingData.cameraData.camera.scaledPixelWidth, 0f, 0f, 0f));
					Blit(this, commandBuffer, blurBuffer1, blurBuffer2, Material, 1);
					commandBuffer.SetGlobalVector(ShaderParameters.BlurOffsets, new Vector4(0f, volumeSettings.blurAmount.value / (float)renderingData.cameraData.camera.scaledPixelHeight, 0f, 0f));
					Blit(this, commandBuffer, blurBuffer2, blurBuffer1, Material, 1);
				}
				commandBuffer.SetGlobalTexture(aoTexID, blurBuffer1);
				FinalBlit(this, context, commandBuffer, renderingData, volumeSettings.aoOnly.value ? 3 : 2);
			}

			public override void OnCameraCleanup(CommandBuffer cmd)
			{
				base.OnCameraCleanup(cmd);
				if (ShouldReleaseRT())
				{
					PostEffectRenderer<AmbientOcclusion2D>.ReleaseRT(ao);
					PostEffectRenderer<AmbientOcclusion2D>.ReleaseRT(blurBuffer1);
					PostEffectRenderer<AmbientOcclusion2D>.ReleaseRT(blurBuffer2);
				}
			}
		}

		private AmbientOcclusion2DRenderPass m_ScriptablePass;

		[SerializeField]
		public EffectBaseSettings settings = new EffectBaseSettings();

		public override void Create()
		{
			m_ScriptablePass = new AmbientOcclusion2DRenderPass(settings);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			m_ScriptablePass.Setup(renderer, renderingData);
		}
	}
}
