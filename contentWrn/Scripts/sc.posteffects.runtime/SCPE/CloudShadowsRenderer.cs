using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	public class CloudShadowsRenderer : ScriptableRendererFeature
	{
		private class CloudShadowsRenderPass : PostEffectRenderer<CloudShadows>
		{
			public CloudShadowsRenderPass(EffectBaseSettings settings)
			{
				base.settings = settings;
				base.renderPassEvent = settings.GetInjectionPoint();
				shaderName = "Hidden/SC Post Effects/Cloud Shadows";
				requiresDepth = true;
				ProfilerTag = GetProfilerTag();
			}

			public override void Setup(ScriptableRenderer renderer, RenderingData renderingData)
			{
				volumeSettings = VolumeManager.instance.stack.GetComponent<CloudShadows>();
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
			}

			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				CommandBuffer commandBuffer = GetCommandBuffer(ref renderingData);
				CopyTargets(commandBuffer, renderingData);
				Texture value = ((volumeSettings.texture.value == null) ? Texture2D.whiteTexture : volumeSettings.texture.value);
				Material.SetTexture("_NoiseTex", value);
				float num = volumeSettings.speed.value * 0.1f;
				Material.SetVector("_CloudParams", new Vector4(volumeSettings.size.value * 0.01f, volumeSettings.direction.value.x * num, volumeSettings.direction.value.y * num, volumeSettings.density.value));
				if (volumeSettings.projectFromSun.value)
				{
					SetMainLightProjection(commandBuffer, renderingData);
				}
				Material.SetFloat("_ProjectionEnabled", volumeSettings.projectFromSun.value ? 1 : 0);
				commandBuffer.SetGlobalVector("_FadeParams", new Vector4(volumeSettings.startFadeDistance.value, volumeSettings.endFadeDistance.value, 0f, 0f));
				FinalBlit(this, context, commandBuffer, renderingData, 0);
			}
		}

		private CloudShadowsRenderPass m_ScriptablePass;

		[SerializeField]
		public EffectBaseSettings settings = new EffectBaseSettings();

		public override void Create()
		{
			m_ScriptablePass = new CloudShadowsRenderPass(settings);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			m_ScriptablePass.Setup(renderer, renderingData);
		}
	}
}
