using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	public class SunshaftsRenderer : ScriptableRendererFeature
	{
		private class SunshaftsRenderPass : PostEffectRenderer<Sunshafts>
		{
			public enum Pass
			{
				SkySource = 0,
				RadialBlur = 1,
				Blend = 2
			}

			private int skyboxBufferID = Shader.PropertyToID("_SunshaftBuffer");

			private RTHandle blurBuffer1;

			private RTHandle blurBuffer2;

			public SunshaftsRenderPass(EffectBaseSettings settings)
			{
				base.settings = settings;
				base.renderPassEvent = settings.GetInjectionPoint();
				shaderName = "Hidden/SC Post Effects/Sun Shafts";
				requiresDepth = true;
				ProfilerTag = GetProfilerTag();
			}

			public override void Setup(ScriptableRenderer renderer, RenderingData renderingData)
			{
				volumeSettings = VolumeManager.instance.stack.GetComponent<Sunshafts>();
				base.Setup(renderer, renderingData);
				if (render && volumeSettings.IsActive())
				{
					cameraColorTarget = GetCameraTarget(renderer);
					renderer.EnqueuePass(this);
				}
			}

			public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
			{
				base.OnCameraSetup(cmd, ref renderingData);
				int value = (int)volumeSettings.resolution.value;
				RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
				blurBuffer1 = PostEffectRenderer<Sunshafts>.GetTemporaryRT(ref blurBuffer1, cameraTargetDescriptor, cameraTargetDescriptor.graphicsFormat, FilterMode.Bilinear, "SunshaftsBlurBuffer1", value);
				blurBuffer2 = PostEffectRenderer<Sunshafts>.GetTemporaryRT(ref blurBuffer2, cameraTargetDescriptor, cameraTargetDescriptor.graphicsFormat, FilterMode.Bilinear, "SunshaftsBlurBuffer2", value);
			}

			protected override void ConfigurePass(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				base.ConfigurePass(cmd, cameraTextureDescriptor);
			}

			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				CommandBuffer commandBuffer = GetCommandBuffer(ref renderingData);
				CopyTargets(commandBuffer, renderingData);
				float x = ((volumeSettings.useCasterIntensity.value && (bool)RenderSettings.sun) ? RenderSettings.sun.intensity : volumeSettings.sunShaftIntensity.value);
				commandBuffer.SetGlobalVector("_SunPosition", -RenderSettings.sun.transform.forward * 1E+10f);
				commandBuffer.SetGlobalFloat("_BlendMode", (float)volumeSettings.blendMode.value);
				commandBuffer.SetGlobalColor("_SunColor", (volumeSettings.useCasterColor.value && (bool)RenderSettings.sun) ? RenderSettings.sun.color : volumeSettings.sunColor.value);
				commandBuffer.SetGlobalColor("_SunThreshold", volumeSettings.sunThreshold.value);
				commandBuffer.SetGlobalVector(ShaderParameters.Params, new Vector4(x, volumeSettings.falloff.value, 0f, 0f));
				SetViewProjectionMatrixUniforms(commandBuffer, in renderingData.cameraData);
				Blit(this, commandBuffer, cameraColorTarget, blurBuffer1, Material, 0);
				commandBuffer.BeginSample("Sunshafts blur");
				float num = volumeSettings.length.value * 0.0013020834f;
				int num2 = ((!volumeSettings.highQuality.value) ? 1 : 2);
				float num3 = (volumeSettings.highQuality.value ? (volumeSettings.length.value / 2.5f) : volumeSettings.length.value);
				for (int i = 0; i < num2; i++)
				{
					Blit(this, commandBuffer, blurBuffer1, blurBuffer2, Material, 1);
					num = num3 * (((float)i * 2f + 1f) * 6f) / (float)renderingData.cameraData.camera.pixelWidth;
					commandBuffer.SetGlobalFloat(ShaderParameters.BlurRadius, num);
					Blit(this, commandBuffer, blurBuffer2, blurBuffer1, Material, 1);
					num = num3 * (((float)i * 2f + 1f) * 6f) / (float)renderingData.cameraData.camera.pixelHeight;
					commandBuffer.SetGlobalFloat(ShaderParameters.BlurRadius, num);
				}
				commandBuffer.EndSample("Sunshafts blur");
				commandBuffer.SetGlobalTexture(skyboxBufferID, blurBuffer1);
				FinalBlit(this, context, commandBuffer, renderingData, 2);
			}

			public override void OnCameraCleanup(CommandBuffer cmd)
			{
				base.OnCameraCleanup(cmd);
				if (ShouldReleaseRT())
				{
					PostEffectRenderer<Sunshafts>.ReleaseRT(blurBuffer1);
					PostEffectRenderer<Sunshafts>.ReleaseRT(blurBuffer2);
				}
			}
		}

		private SunshaftsRenderPass m_ScriptablePass;

		[SerializeField]
		public EffectBaseSettings settings = new EffectBaseSettings();

		public override void Create()
		{
			m_ScriptablePass = new SunshaftsRenderPass(settings);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			m_ScriptablePass.Setup(renderer, renderingData);
		}
	}
}
