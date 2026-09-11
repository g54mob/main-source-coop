using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	public class SketchRenderer : ScriptableRendererFeature
	{
		private class SketchRenderPass : PostEffectRenderer<Sketch>
		{
			public SketchRenderPass(EffectBaseSettings settings)
			{
				base.settings = settings;
				base.renderPassEvent = settings.GetInjectionPoint();
				shaderName = "Hidden/SC Post Effects/Sketch";
				ProfilerTag = GetProfilerTag();
			}

			public override void Setup(ScriptableRenderer renderer, RenderingData renderingData)
			{
				volumeSettings = VolumeManager.instance.stack.GetComponent<Sketch>();
				base.Setup(renderer, renderingData);
				if (render && volumeSettings.IsActive())
				{
					cameraColorTarget = GetCameraTarget(renderer);
					renderer.EnqueuePass(this);
				}
			}

			protected override void ConfigurePass(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				requiresDepth = volumeSettings.projectionMode == Sketch.SketchProjectionMode.WorldSpace;
				base.ConfigurePass(cmd, cameraTextureDescriptor);
			}

			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				CommandBuffer commandBuffer = GetCommandBuffer(ref renderingData);
				CopyTargets(commandBuffer, renderingData);
				if ((bool)volumeSettings.strokeTex.value)
				{
					Material.SetTexture("_Strokes", volumeSettings.strokeTex.value);
				}
				Material.SetVector("_Params", new Vector4(0f, (float)volumeSettings.blendMode.value, volumeSettings.intensity.value, (volumeSettings.projectionMode.value == Sketch.SketchProjectionMode.ScreenSpace) ? (volumeSettings.tiling.value * 0.1f) : volumeSettings.tiling.value));
				Material.SetVector("_Brightness", volumeSettings.brightness.value);
				FinalBlit(this, context, commandBuffer, renderingData, (int)volumeSettings.projectionMode.value);
			}
		}

		private SketchRenderPass m_ScriptablePass;

		[SerializeField]
		public EffectBaseSettings settings = new EffectBaseSettings();

		public override void Create()
		{
			m_ScriptablePass = new SketchRenderPass(settings);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			m_ScriptablePass.Setup(renderer, renderingData);
		}
	}
}
