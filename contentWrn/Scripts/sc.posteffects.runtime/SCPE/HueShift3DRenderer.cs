using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	public class HueShift3DRenderer : ScriptableRendererFeature
	{
		private class HueShift3DRenderPass : PostEffectRenderer<HueShift3D>
		{
			private enum Pass
			{
				ColorSpectrum = 0,
				GradientTexture = 1
			}

			public HueShift3DRenderPass(EffectBaseSettings settings)
			{
				base.settings = settings;
				base.renderPassEvent = settings.GetInjectionPoint();
				shaderName = "Hidden/SC Post Effects/3D Hue Shift";
				ProfilerTag = GetProfilerTag();
			}

			public override void Setup(ScriptableRenderer renderer, RenderingData renderingData)
			{
				volumeSettings = VolumeManager.instance.stack.GetComponent<HueShift3D>();
				base.Setup(renderer, renderingData);
				if (render && volumeSettings.IsActive())
				{
					cameraColorTarget = GetCameraTarget(renderer);
					renderer.EnqueuePass(this);
				}
			}

			protected override void ConfigurePass(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				requiresDepthNormals = volumeSettings.geoInfluence.value > 0f;
				base.ConfigurePass(cmd, cameraTextureDescriptor);
			}

			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				CommandBuffer commandBuffer = GetCommandBuffer(ref renderingData);
				CopyTargets(commandBuffer, renderingData);
				HueShift3D.isOrtho = renderingData.cameraData.camera.orthographic;
				Material.SetVector("_Params", new Vector4(volumeSettings.speed.value, volumeSettings.size.value, volumeSettings.geoInfluence.value, volumeSettings.intensity.value));
				if ((bool)volumeSettings.gradientTex.value)
				{
					Material.SetTexture("_GradientTex", volumeSettings.gradientTex.value);
				}
				FinalBlit(this, context, commandBuffer, renderingData, (volumeSettings.colorSource.value != HueShift3D.ColorSource.RGBSpectrum) ? 1 : 0);
			}
		}

		[Serializable]
		public class HueShift3DSettings : EffectBaseSettings
		{
			[Header("Effect specific")]
			[Tooltip("Reconstruct the scene geometry's normals from the depth texture.\n\nIn Unity 2020.3+, disabling this will have the effect use the Depth-Normals prepass, which is more accurate. This will have all object re-render, if the scene isn't already optimized for draw calls, this will negatively affect performance")]
			public bool reconstructDepthNormals;
		}

		private HueShift3DRenderPass m_ScriptablePass;

		[SerializeField]
		public HueShift3DSettings settings = new HueShift3DSettings();

		public override void Create()
		{
			m_ScriptablePass = new HueShift3DRenderPass(settings);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			m_ScriptablePass.reconstructDepthNormals = settings.reconstructDepthNormals;
			m_ScriptablePass.Setup(renderer, renderingData);
		}
	}
}
