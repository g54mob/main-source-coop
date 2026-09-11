using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	public class EdgeDetectionRenderer : ScriptableRendererFeature
	{
		private class EdgeDetectionRenderPass : PostEffectRenderer<EdgeDetection>
		{
			public EdgeDetectionRenderPass(EffectBaseSettings settings)
			{
				base.settings = settings;
				base.renderPassEvent = settings.GetInjectionPoint();
				shaderName = "Hidden/SC Post Effects/Edge Detection";
				ProfilerTag = GetProfilerTag();
			}

			public override void Setup(ScriptableRenderer renderer, RenderingData renderingData)
			{
				volumeSettings = VolumeManager.instance.stack.GetComponent<EdgeDetection>();
				base.Setup(renderer, renderingData);
				if (render && volumeSettings.IsActive())
				{
					cameraColorTarget = GetCameraTarget(renderer);
					if ((bool)volumeSettings && volumeSettings.IsActive())
					{
						renderer.EnqueuePass(this);
					}
				}
			}

			protected override void ConfigurePass(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				requiresDepth = volumeSettings.mode != EdgeDetection.EdgeDetectMode.LuminanceBased;
				requiresDepthNormals = volumeSettings.IsActive() && (volumeSettings.mode == EdgeDetection.EdgeDetectMode.CrossDepthNormals || volumeSettings.mode == EdgeDetection.EdgeDetectMode.DepthNormals);
				base.ConfigurePass(cmd, cameraTextureDescriptor);
			}

			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				base.Execute(context, ref renderingData);
				CommandBuffer commandBuffer = GetCommandBuffer(ref renderingData);
				CopyTargets(commandBuffer, renderingData);
				Vector2 vector = new Vector2(volumeSettings.sensitivityDepth.value, volumeSettings.sensitivityNormals.value);
				Material.SetVector("_Sensitivity", vector);
				Material.SetFloat("_BackgroundFade", volumeSettings.debug.value ? 1f : 0f);
				Material.SetFloat("_EdgeSize", volumeSettings.edgeSize.value);
				Material.SetFloat("_Exponent", volumeSettings.edgeExp.value);
				Material.SetFloat("_Threshold", volumeSettings.lumThreshold.value);
				Material.SetColor("_EdgeColor", volumeSettings.edgeColor.value);
				Material.SetFloat("_EdgeOpacity", volumeSettings.edgeOpacity.value);
				Material.SetVector(ShaderParameters.FadeParams, new Vector4(volumeSettings.startFadeDistance.value, volumeSettings.endFadeDistance.value, volumeSettings.invertFadeDistance.value ? 1 : 0, volumeSettings.distanceFade.value ? 1 : 0));
				Material.SetVector("_SobelParams", new Vector4(volumeSettings.sobelThin.value ? 1 : 0, 0f, 0f, 0f));
				FinalBlit(this, context, commandBuffer, renderingData, (int)volumeSettings.mode.value);
			}
		}

		[Serializable]
		public class EdgeDetectionSettings : EffectBaseSettings
		{
			[Header("Effect specific")]
			[Tooltip("Reconstruct the scene geometry's normals from the depth texture.\n\nIn Unity 2020.3+, disabling this will have the effect use the Depth-Normals prepass, which is more accurate. This will have all object re-render, if the scene isn't already optimized for draw calls, this will negatively affect performance")]
			public bool reconstructDepthNormals = true;

			[Tooltip("Executes the effect before transparent materials are rendered.")]
			public bool skipTransparents;
		}

		private EdgeDetectionRenderPass m_ScriptablePass;

		[SerializeField]
		public EdgeDetectionSettings settings = new EdgeDetectionSettings();

		public override void Create()
		{
			m_ScriptablePass = new EdgeDetectionRenderPass(settings);
			m_ScriptablePass.renderPassEvent = (settings.skipTransparents ? RenderPassEvent.BeforeRenderingTransparents : settings.GetInjectionPoint());
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			m_ScriptablePass.reconstructDepthNormals = settings.reconstructDepthNormals;
			m_ScriptablePass.Setup(renderer, renderingData);
		}
	}
}
