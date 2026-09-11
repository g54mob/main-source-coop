using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	public class LUTRenderer : ScriptableRendererFeature
	{
		private class LUTRenderPass : PostEffectRenderer<LUT>
		{
			public LUTRenderPass(EffectBaseSettings settings)
			{
				base.settings = settings;
				base.renderPassEvent = settings.GetInjectionPoint();
				shaderName = "Hidden/SC Post Effects/LUT";
				ProfilerTag = GetProfilerTag();
			}

			public override void Setup(ScriptableRenderer renderer, RenderingData renderingData)
			{
				volumeSettings = VolumeManager.instance.stack.GetComponent<LUT>();
				base.Setup(renderer, renderingData);
				if (render && volumeSettings.IsActive())
				{
					cameraColorTarget = GetCameraTarget(renderer);
					renderer.EnqueuePass(this);
				}
			}

			protected override void ConfigurePass(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				requiresDepth = volumeSettings.mode == LUT.Mode.DistanceBased;
				base.ConfigurePass(cmd, cameraTextureDescriptor);
			}

			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				if (LUT.Bypass || !render || !volumeSettings.IsActive())
				{
					return;
				}
				CommandBuffer commandBuffer = GetCommandBuffer(ref renderingData);
				CopyTargets(commandBuffer, renderingData);
				Material.SetVector("_LUT_Params", new Vector4(volumeSettings.lutNear.value ? volumeSettings.intensity.value : 0f, volumeSettings.invert.value));
				if ((bool)volumeSettings.lutNear.value)
				{
					Material.SetTexture("_LUT_Near", volumeSettings.lutNear.value);
				}
				if (volumeSettings.mode.value == LUT.Mode.DistanceBased)
				{
					Material.SetVector("_FadeParams", new Vector4(volumeSettings.startFadeDistance.value, volumeSettings.endFadeDistance.value, 0f, 0f));
					if ((bool)volumeSettings.lutFar.value)
					{
						Material.SetTexture("_LUT_Far", volumeSettings.lutFar.value);
					}
				}
				Material.SetVector(ShaderParameters.Params, new Vector4(volumeSettings.vibranceRGBBalance.value.r, volumeSettings.vibranceRGBBalance.value.g, volumeSettings.vibranceRGBBalance.value.b, volumeSettings.vibrance.value));
				FinalBlit(this, context, commandBuffer, renderingData, (int)volumeSettings.mode.value);
			}
		}

		private LUTRenderPass m_ScriptablePass;

		[SerializeField]
		public EffectBaseSettings settings = new EffectBaseSettings();

		public override void Create()
		{
			m_ScriptablePass = new LUTRenderPass(settings);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			m_ScriptablePass.Setup(renderer, renderingData);
		}
	}
}
