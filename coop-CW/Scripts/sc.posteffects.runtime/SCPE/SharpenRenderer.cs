using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	public class SharpenRenderer : ScriptableRendererFeature
	{
		private class SharpenRenderPass : PostEffectRenderer<Sharpen>
		{
			public SharpenRenderPass(EffectBaseSettings settings)
			{
				base.settings = settings;
				base.renderPassEvent = settings.GetInjectionPoint();
				shaderName = "Hidden/SC Post Effects/Sharpen";
				ProfilerTag = GetProfilerTag();
			}

			public override void Setup(ScriptableRenderer renderer, RenderingData renderingData)
			{
				volumeSettings = VolumeManager.instance.stack.GetComponent<Sharpen>();
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
				Material.SetVector(ShaderParameters.Params, new Vector3(volumeSettings.amount.value, volumeSettings.radius.value, volumeSettings.contrast.value));
				FinalBlit(this, context, commandBuffer, renderingData, (int)volumeSettings.mode.value);
			}
		}

		private SharpenRenderPass m_ScriptablePass;

		[SerializeField]
		public EffectBaseSettings settings = new EffectBaseSettings();

		public override void Create()
		{
			m_ScriptablePass = new SharpenRenderPass(settings);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			m_ScriptablePass.Setup(renderer, renderingData);
		}

		public void OnDestroy()
		{
			m_ScriptablePass.Dispose();
		}
	}
}
