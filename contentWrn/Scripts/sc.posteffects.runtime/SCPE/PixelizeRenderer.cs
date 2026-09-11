using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	public class PixelizeRenderer : ScriptableRendererFeature
	{
		private class PixelizeRenderPass : PostEffectRenderer<Pixelize>
		{
			private static readonly int _PixelizeParams = Shader.PropertyToID("_PixelizeParams");

			private static Vector4 pixelizeParams;

			public PixelizeRenderPass(EffectBaseSettings settings)
			{
				base.settings = settings;
				base.renderPassEvent = settings.GetInjectionPoint();
				shaderName = "Hidden/SC Post Effects/Pixelize";
				ProfilerTag = GetProfilerTag();
			}

			public override void Setup(ScriptableRenderer renderer, RenderingData renderingData)
			{
				volumeSettings = VolumeManager.instance.stack.GetComponent<Pixelize>();
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
				int num = ((volumeSettings.resolutionPreset.value == Pixelize.Resolution.Custom) ? volumeSettings.resolution.value : ((int)volumeSettings.resolutionPreset.value));
				pixelizeParams.x = (float)(volumeSettings.preserveAspectRatio.value ? renderingData.cameraData.camera.scaledPixelWidth : renderingData.cameraData.camera.scaledPixelHeight) / (float)num;
				pixelizeParams.y = (float)renderingData.cameraData.camera.scaledPixelHeight / (float)num;
				pixelizeParams.z = volumeSettings.amount.value;
				pixelizeParams.w = (volumeSettings.centerPixel.value ? 1 : 0);
				Material.SetVector(_PixelizeParams, pixelizeParams);
				FinalBlit(this, context, commandBuffer, renderingData, 0);
			}
		}

		private PixelizeRenderPass m_ScriptablePass;

		[SerializeField]
		public EffectBaseSettings settings = new EffectBaseSettings();

		public override void Create()
		{
			m_ScriptablePass = new PixelizeRenderPass(settings);
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
