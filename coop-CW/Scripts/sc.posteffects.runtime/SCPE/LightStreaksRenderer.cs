using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	public class LightStreaksRenderer : ScriptableRendererFeature
	{
		private class LightStreaksRenderPass : PostEffectRenderer<LightStreaks>
		{
			private enum Pass
			{
				LuminanceDiff = 0,
				BlurFast = 1,
				Blur = 2,
				Blend = 3,
				Debug = 4
			}

			private readonly int emissionTexID = Shader.PropertyToID("_BloomTex");

			private RTHandle emissionTex;

			private RTHandle blurBuffer1;

			private RTHandle blurBuffer2;

			public LightStreaksRenderPass(EffectBaseSettings settings)
			{
				base.settings = settings;
				base.renderPassEvent = settings.GetInjectionPoint();
				shaderName = "Hidden/SC Post Effects/Light Streaks";
				ProfilerTag = GetProfilerTag();
			}

			public override void Setup(ScriptableRenderer renderer, RenderingData renderingData)
			{
				volumeSettings = VolumeManager.instance.stack.GetComponent<LightStreaks>();
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
				emissionTex = PostEffectRenderer<LightStreaks>.GetTemporaryRT(ref emissionTex, cameraTextureDescriptor, cameraTextureDescriptor.graphicsFormat, FilterMode.Bilinear, "LightStreaks");
				blurBuffer1 = PostEffectRenderer<LightStreaks>.GetTemporaryRT(ref blurBuffer1, cameraTextureDescriptor, cameraTextureDescriptor.graphicsFormat, FilterMode.Bilinear, "LightStreaksBlurBuffer1", volumeSettings.downscaling.value);
				blurBuffer2 = PostEffectRenderer<LightStreaks>.GetTemporaryRT(ref blurBuffer2, cameraTextureDescriptor, cameraTextureDescriptor.graphicsFormat, FilterMode.Bilinear, "LightStreaksBlurBuffer2", volumeSettings.downscaling.value);
			}

			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				CommandBuffer commandBuffer = GetCommandBuffer(ref renderingData);
				int passIndex = ((volumeSettings.quality.value == LightStreaks.Quality.Performance) ? 1 : 2);
				float x = Mathf.GammaToLinearSpace(volumeSettings.luminanceThreshold.value);
				Material.SetVector(ShaderParameters.Params, new Vector4(x, volumeSettings.intensity.value, 0f, 0f));
				CopyTargets(commandBuffer, renderingData);
				Blit(this, commandBuffer, cameraColorTarget, emissionTex, Material, 0);
				BlitCopy(commandBuffer, emissionTex, blurBuffer1);
				float num = Mathf.Clamp(volumeSettings.direction.value, -1f, 1f);
				float num2 = ((num < 0f) ? ((0f - num) * 1f) : 0f);
				float num3 = ((num > 0f) ? (num * 4f) : 0f);
				int num4 = ((volumeSettings.quality.value == LightStreaks.Quality.Performance) ? (volumeSettings.iterations.value * 3) : volumeSettings.iterations.value);
				for (int i = 0; i < num4; i++)
				{
					commandBuffer.SetGlobalVector(ShaderParameters.BlurOffsets, new Vector4(num2 * volumeSettings.blur.value / (float)renderingData.cameraData.camera.scaledPixelWidth, num3 / (float)renderingData.cameraData.camera.scaledPixelHeight, 0f, 0f));
					Blit(this, commandBuffer, blurBuffer1, blurBuffer2, Material, passIndex);
					commandBuffer.SetGlobalVector(ShaderParameters.BlurOffsets, new Vector4(num2 * volumeSettings.blur.value * 2f / (float)renderingData.cameraData.camera.scaledPixelWidth, num3 * 2f / (float)renderingData.cameraData.camera.scaledPixelHeight, 0f, 0f));
					Blit(this, commandBuffer, blurBuffer2, blurBuffer1, Material, passIndex);
				}
				commandBuffer.SetGlobalTexture(emissionTexID, blurBuffer1);
				FinalBlit(this, context, commandBuffer, renderingData, volumeSettings.debug.value ? 4 : 3);
			}

			public override void OnCameraCleanup(CommandBuffer cmd)
			{
				base.OnCameraCleanup(cmd);
				if (ShouldReleaseRT())
				{
					PostEffectRenderer<LightStreaks>.ReleaseRT(emissionTex);
					PostEffectRenderer<LightStreaks>.ReleaseRT(blurBuffer1);
					PostEffectRenderer<LightStreaks>.ReleaseRT(blurBuffer2);
				}
			}
		}

		private LightStreaksRenderPass m_ScriptablePass;

		[SerializeField]
		public EffectBaseSettings settings = new EffectBaseSettings();

		public override void Create()
		{
			m_ScriptablePass = new LightStreaksRenderPass(settings);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			m_ScriptablePass.Setup(renderer, renderingData);
		}
	}
}
