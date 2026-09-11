using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	public class CausticsRenderer : ScriptableRendererFeature
	{
		private class CausticsRenderPass : PostEffectRenderer<Caustics>
		{
			public CausticsRenderPass(EffectBaseSettings settings)
			{
				base.settings = settings;
				base.renderPassEvent = settings.GetInjectionPoint();
				shaderName = "Hidden/SC Post Effects/Caustics";
				requiresDepth = true;
				ProfilerTag = GetProfilerTag();
			}

			public override void Setup(ScriptableRenderer renderer, RenderingData renderingData)
			{
				volumeSettings = VolumeManager.instance.stack.GetComponent<Caustics>();
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
				if ((bool)volumeSettings.causticsTexture.value)
				{
					Material.SetTexture("_CausticsTex", volumeSettings.causticsTexture.value);
				}
				Material.SetFloat("_LuminanceThreshold", Mathf.GammaToLinearSpace(volumeSettings.luminanceThreshold.value));
				if (volumeSettings.projectFromSun.value)
				{
					SetMainLightProjection(commandBuffer, renderingData);
				}
				Material.SetVector("_CausticsParams", new Vector4(volumeSettings.size.value, volumeSettings.speed.value, volumeSettings.projectFromSun.value ? 1 : 0, volumeSettings.brightness.value * volumeSettings.intensity.value));
				Material.SetVector("_HeightParams", new Vector4(volumeSettings.minHeight.value, volumeSettings.minHeightFalloff.value, volumeSettings.maxHeight.value, volumeSettings.maxHeightFalloff.value));
				commandBuffer.SetGlobalVector("_FadeParams", new Vector4(volumeSettings.startFadeDistance.value, volumeSettings.endFadeDistance.value, 0f, volumeSettings.distanceFade.value ? 1 : 0));
				FinalBlit(this, context, commandBuffer, renderingData, 0);
			}
		}

		[Serializable]
		public class Causticsettings : EffectBaseSettings
		{
			[Header("Effect specific")]
			[Tooltip("Executes the effect before transparent materials are rendered.")]
			public bool skipTransparents;
		}

		private CausticsRenderPass m_ScriptablePass;

		[SerializeField]
		public Causticsettings settings = new Causticsettings();

		public override void Create()
		{
			m_ScriptablePass = new CausticsRenderPass(settings);
			m_ScriptablePass.renderPassEvent = (settings.skipTransparents ? RenderPassEvent.BeforeRenderingTransparents : settings.GetInjectionPoint());
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			m_ScriptablePass.Setup(renderer, renderingData);
		}
	}
}
