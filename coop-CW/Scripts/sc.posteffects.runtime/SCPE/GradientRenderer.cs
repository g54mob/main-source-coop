using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	public class GradientRenderer : ScriptableRendererFeature
	{
		private class GradientRenderPass : PostEffectRenderer<Gradient>
		{
			public GradientRenderPass(EffectBaseSettings settings)
			{
				base.settings = settings;
				base.renderPassEvent = settings.GetInjectionPoint();
				shaderName = "Hidden/SC Post Effects/Gradient";
				ProfilerTag = GetProfilerTag();
			}

			public override void Setup(ScriptableRenderer renderer, RenderingData renderingData)
			{
				volumeSettings = VolumeManager.instance.stack.GetComponent<Gradient>();
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
				if ((bool)volumeSettings.gradientTex.value)
				{
					Material.SetTexture("_Gradient", volumeSettings.gradientTex.value);
				}
				Material.SetColor("_Color1", volumeSettings.color1.value);
				Material.SetColor("_Color2", volumeSettings.color2.value);
				Material.SetFloat("_Rotation", volumeSettings.rotation.value * 360f);
				Material.SetFloat("_Intensity", volumeSettings.intensity.value);
				Material.SetFloat("_BlendMode", (float)volumeSettings.mode.value);
				FinalBlit(this, context, commandBuffer, renderingData, (int)volumeSettings.input.value);
			}
		}

		private GradientRenderPass m_ScriptablePass;

		[SerializeField]
		public EffectBaseSettings settings = new EffectBaseSettings();

		public override void Create()
		{
			m_ScriptablePass = new GradientRenderPass(settings);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			m_ScriptablePass.Setup(renderer, renderingData);
		}
	}
}
