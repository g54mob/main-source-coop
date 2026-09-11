using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	public class MosaicRenderer : ScriptableRendererFeature
	{
		private class MosaicRenderPass : PostEffectRenderer<Mosaic>
		{
			public MosaicRenderPass(EffectBaseSettings settings)
			{
				base.settings = settings;
				base.renderPassEvent = settings.GetInjectionPoint();
				shaderName = "Hidden/SC Post Effects/Mosaic";
				ProfilerTag = GetProfilerTag();
			}

			public override void Setup(ScriptableRenderer renderer, RenderingData renderingData)
			{
				volumeSettings = VolumeManager.instance.stack.GetComponent<Mosaic>();
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
				float num = volumeSettings.size.value;
				switch ((Mosaic.MosaicMode)volumeSettings.mode)
				{
				case Mosaic.MosaicMode.Triangles:
					num = 10f / volumeSettings.size.value;
					break;
				case Mosaic.MosaicMode.Hexagons:
					num = volumeSettings.size.value / 10f;
					break;
				case Mosaic.MosaicMode.Circles:
					num = (1f - volumeSettings.size.value) * 100f;
					break;
				}
				Vector4 value = new Vector4(num, (float)(renderingData.cameraData.camera.scaledPixelWidth * 2 / renderingData.cameraData.camera.scaledPixelHeight) * num / Mathf.Sqrt(3f), 0f, 0f);
				Material.SetVector("_Params", value);
				FinalBlit(this, context, commandBuffer, renderingData, (int)volumeSettings.mode.value);
			}
		}

		private MosaicRenderPass m_ScriptablePass;

		[SerializeField]
		public EffectBaseSettings settings = new EffectBaseSettings();

		public override void Create()
		{
			m_ScriptablePass = new MosaicRenderPass(settings);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			m_ScriptablePass.Setup(renderer, renderingData);
		}
	}
}
