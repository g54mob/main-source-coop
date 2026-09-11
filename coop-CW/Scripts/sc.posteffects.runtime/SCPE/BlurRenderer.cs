using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	public class BlurRenderer : ScriptableRendererFeature
	{
		private class BlurRenderPass : PostEffectRenderer<Blur>
		{
			private enum Pass
			{
				Blend = 0,
				BlendDepthFade = 1,
				Gaussian = 2,
				Box = 3
			}

			private RTHandle blurBuffer1;

			private RTHandle blurBuffer2;

			public BlurRenderPass(EffectBaseSettings settings)
			{
				base.settings = settings;
				base.renderPassEvent = settings.GetInjectionPoint();
				shaderName = "Hidden/SC Post Effects/Blur";
				ProfilerTag = GetProfilerTag();
			}

			public override void Setup(ScriptableRenderer renderer, RenderingData renderingData)
			{
				volumeSettings = VolumeManager.instance.stack.GetComponent<Blur>();
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
				blurBuffer1 = PostEffectRenderer<Blur>.GetTemporaryRT(ref blurBuffer1, cameraTextureDescriptor, cameraTextureDescriptor.graphicsFormat, FilterMode.Bilinear, "BlurBuffer1", volumeSettings.downscaling.value);
				blurBuffer2 = PostEffectRenderer<Blur>.GetTemporaryRT(ref blurBuffer2, cameraTextureDescriptor, cameraTextureDescriptor.graphicsFormat, FilterMode.Bilinear, "BlurBuffer2", volumeSettings.downscaling.value);
			}

			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				CommandBuffer commandBuffer = GetCommandBuffer(ref renderingData);
				CopyTargets(commandBuffer, renderingData);
				BlitCopy(commandBuffer, cameraColorTarget, blurBuffer1);
				int passIndex = ((volumeSettings.mode == Blur.BlurMethod.Gaussian) ? 2 : 3);
				for (int i = 0; i < volumeSettings.iterations.value; i++)
				{
					if (volumeSettings.iterations.value > 12)
					{
						return;
					}
					commandBuffer.SetGlobalVector(ShaderParameters.BlurOffsets, new Vector4(volumeSettings.amount.value / (float)renderingData.cameraData.camera.scaledPixelWidth, 0f, 0f, 0f));
					Blit(this, commandBuffer, blurBuffer1, blurBuffer2, Material, passIndex);
					commandBuffer.SetGlobalVector(ShaderParameters.BlurOffsets, new Vector4(0f, volumeSettings.amount.value / (float)renderingData.cameraData.camera.scaledPixelHeight, 0f, 0f));
					Blit(this, commandBuffer, blurBuffer2, blurBuffer1, Material, passIndex);
					if (volumeSettings.highQuality.value)
					{
						commandBuffer.SetGlobalVector(ShaderParameters.BlurOffsets, new Vector4(volumeSettings.amount.value / (float)renderingData.cameraData.camera.scaledPixelWidth, 0f, 0f, 0f));
						Blit(this, commandBuffer, blurBuffer1, blurBuffer2, Material, passIndex);
						commandBuffer.SetGlobalVector(ShaderParameters.BlurOffsets, new Vector4(0f, volumeSettings.amount.value / (float)renderingData.cameraData.camera.scaledPixelHeight, 0f, 0f));
						Blit(this, commandBuffer, blurBuffer2, blurBuffer1, Material, passIndex);
					}
				}
				commandBuffer.SetGlobalTexture("_BlurredTex", blurBuffer1);
				if (volumeSettings.distanceFade.value)
				{
					commandBuffer.SetGlobalVector(ShaderParameters.FadeParams, new Vector4(volumeSettings.startFadeDistance.value, volumeSettings.endFadeDistance.value, 0f, volumeSettings.distanceFade.value ? 1 : 0));
				}
				FinalBlit(this, context, commandBuffer, renderingData, volumeSettings.distanceFade.value ? 1 : 0);
			}

			public override void OnCameraCleanup(CommandBuffer cmd)
			{
				base.OnCameraCleanup(cmd);
				if (ShouldReleaseRT())
				{
					PostEffectRenderer<Blur>.ReleaseRT(blurBuffer1);
					PostEffectRenderer<Blur>.ReleaseRT(blurBuffer2);
				}
			}
		}

		private BlurRenderPass m_ScriptablePass;

		[SerializeField]
		public EffectBaseSettings settings = new EffectBaseSettings(enableInSceneView: false);

		public override void Create()
		{
			m_ScriptablePass = new BlurRenderPass(settings);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			m_ScriptablePass.Setup(renderer, renderingData);
		}
	}
}
