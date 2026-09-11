using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	public class OverlayRenderer : ScriptableRendererFeature
	{
		private class OverlayRenderPass : PostEffectRenderer<Overlay>
		{
			public OverlayRenderPass(EffectBaseSettings settings)
			{
				base.settings = settings;
				base.renderPassEvent = settings.GetInjectionPoint();
				shaderName = "Hidden/SC Post Effects/Overlay";
				ProfilerTag = GetProfilerTag();
			}

			public override void Setup(ScriptableRenderer renderer, RenderingData renderingData)
			{
				volumeSettings = VolumeManager.instance.stack.GetComponent<Overlay>();
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
				if ((bool)volumeSettings.overlayTex.value)
				{
					Material.SetTexture("_OverlayTex", volumeSettings.overlayTex.value);
				}
				Material.SetVector("_Params", new Vector4(volumeSettings.intensity.value, Mathf.Pow(volumeSettings.tiling.value + 1f, 2f), volumeSettings.autoAspect.value ? 1f : 0f, (float)volumeSettings.blendMode.value));
				float value = ((QualitySettings.activeColorSpace == ColorSpace.Gamma) ? Mathf.LinearToGammaSpace(volumeSettings.luminanceThreshold.value) : volumeSettings.luminanceThreshold.value);
				Material.SetFloat("_LuminanceThreshold", value);
				FinalBlit(this, context, commandBuffer, renderingData, 0);
			}
		}

		private OverlayRenderPass m_ScriptablePass;

		[SerializeField]
		public EffectBaseSettings settings = new EffectBaseSettings();

		public override void Create()
		{
			m_ScriptablePass = new OverlayRenderPass(settings);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			m_ScriptablePass.Setup(renderer, renderingData);
		}
	}
}
