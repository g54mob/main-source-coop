using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	public class SpeedLinesRenderer : ScriptableRendererFeature
	{
		private class SpeedLinesRenderPass : PostEffectRenderer<SpeedLines>
		{
			public SpeedLinesRenderPass(EffectBaseSettings settings)
			{
				base.settings = settings;
				base.renderPassEvent = settings.GetInjectionPoint();
				shaderName = "Hidden/SC Post Effects/SpeedLines";
				ProfilerTag = GetProfilerTag();
			}

			public override void Setup(ScriptableRenderer renderer, RenderingData renderingData)
			{
				volumeSettings = VolumeManager.instance.stack.GetComponent<SpeedLines>();
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
				float y = 2f + (volumeSettings.falloff.value - 0f) * 14f / 1f;
				Material.SetVector("_Params", new Vector4(volumeSettings.intensity.value, y, volumeSettings.size.value * 2f, 0f));
				if ((bool)volumeSettings.noiseTex.value)
				{
					Material.SetTexture("_NoiseTex", volumeSettings.noiseTex.value);
				}
				FinalBlit(this, context, commandBuffer, renderingData, 0);
			}
		}

		private SpeedLinesRenderPass m_ScriptablePass;

		[SerializeField]
		public EffectBaseSettings settings = new EffectBaseSettings();

		public override void Create()
		{
			m_ScriptablePass = new SpeedLinesRenderPass(settings);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			m_ScriptablePass.Setup(renderer, renderingData);
		}
	}
}
