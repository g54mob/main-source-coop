using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	public class DitheringRenderer : ScriptableRendererFeature
	{
		private class DitheringRenderPass : PostEffectRenderer<Dithering>
		{
			public DitheringRenderPass(EffectBaseSettings settings)
			{
				base.settings = settings;
				base.renderPassEvent = settings.GetInjectionPoint();
				shaderName = "Hidden/SC Post Effects/Dithering";
				ProfilerTag = GetProfilerTag();
			}

			public override void Setup(ScriptableRenderer renderer, RenderingData renderingData)
			{
				volumeSettings = VolumeManager.instance.stack.GetComponent<Dithering>();
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
				Texture value = ((volumeSettings.lut.value == null) ? Texture2D.blackTexture : volumeSettings.lut.value);
				Material.SetTexture("_LUT", value);
				float z = ((QualitySettings.activeColorSpace == ColorSpace.Gamma) ? Mathf.LinearToGammaSpace(volumeSettings.luminanceThreshold.value) : volumeSettings.luminanceThreshold.value);
				Vector4 value2 = new Vector4(0f, volumeSettings.tiling.value, z, volumeSettings.intensity.value);
				Material.SetVector("_Dithering_Coords", value2);
				FinalBlit(this, context, commandBuffer, renderingData, 0);
			}
		}

		private DitheringRenderPass m_ScriptablePass;

		[SerializeField]
		public EffectBaseSettings settings = new EffectBaseSettings();

		public override void Create()
		{
			m_ScriptablePass = new DitheringRenderPass(settings);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			m_ScriptablePass.Setup(renderer, renderingData);
		}
	}
}
