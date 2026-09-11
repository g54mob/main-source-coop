using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	public class KuwaharaRenderer : ScriptableRendererFeature
	{
		private class KuwaharaRenderPass : PostEffectRenderer<Kuwahara>
		{
			private int mode;

			public KuwaharaRenderPass(EffectBaseSettings settings)
			{
				base.settings = settings;
				base.renderPassEvent = settings.GetInjectionPoint();
				shaderName = "Hidden/SC Post Effects/Kuwahara";
				ProfilerTag = GetProfilerTag();
			}

			public override void Setup(ScriptableRenderer renderer, RenderingData renderingData)
			{
				volumeSettings = VolumeManager.instance.stack.GetComponent<Kuwahara>();
				base.Setup(renderer, renderingData);
				if (render && volumeSettings.IsActive())
				{
					cameraColorTarget = GetCameraTarget(renderer);
					renderer.EnqueuePass(this);
				}
			}

			protected override void ConfigurePass(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				requiresDepth = volumeSettings.mode == Kuwahara.KuwaharaMode.DepthFade;
				base.ConfigurePass(cmd, cameraTextureDescriptor);
			}

			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				CommandBuffer commandBuffer = GetCommandBuffer(ref renderingData);
				mode = (int)volumeSettings.mode.value;
				if (renderingData.cameraData.camera.orthographic)
				{
					mode = 0;
				}
				CopyTargets(commandBuffer, renderingData);
				Material.SetFloat("_Radius", (int)volumeSettings.radius);
				if (mode == 1)
				{
					Material.SetVector("_FadeParams", new Vector4(volumeSettings.startFadeDistance.value, volumeSettings.endFadeDistance.value, 0f, 0f));
				}
				FinalBlit(this, context, commandBuffer, renderingData, mode);
			}
		}

		private KuwaharaRenderPass m_ScriptablePass;

		[SerializeField]
		public EffectBaseSettings settings = new EffectBaseSettings();

		public override void Create()
		{
			m_ScriptablePass = new KuwaharaRenderPass(settings);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			m_ScriptablePass.Setup(renderer, renderingData);
		}
	}
}
