using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	public class TiltShiftRenderer : ScriptableRendererFeature
	{
		private class TiltShiftRenderPass : PostEffectRenderer<TiltShift>
		{
			private enum Pass
			{
				FragHorizontal = 0,
				FragHorizontalHQ = 1,
				FragRadial = 2,
				FragRadialHQ = 3,
				FragDebug = 4
			}

			public TiltShiftRenderPass(EffectBaseSettings settings)
			{
				base.settings = settings;
				base.renderPassEvent = settings.GetInjectionPoint();
				shaderName = "Hidden/SC Post Effects/Tilt Shift";
				ProfilerTag = GetProfilerTag();
			}

			public override void Setup(ScriptableRenderer renderer, RenderingData renderingData)
			{
				volumeSettings = VolumeManager.instance.stack.GetComponent<TiltShift>();
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
				Material.SetVector(ShaderParameters.Params, new Vector4(volumeSettings.areaSize.value, volumeSettings.areaFalloff.value, volumeSettings.amount.value, (float)volumeSettings.mode.value));
				Material.SetFloat("_Offset", volumeSettings.offset.value);
				Material.SetFloat("_Angle", volumeSettings.angle.value);
				int num = (int)volumeSettings.mode.value + (int)volumeSettings.quality.value;
				switch ((int)volumeSettings.mode.value)
				{
				case 0:
					num = (int)volumeSettings.quality.value;
					break;
				case 1:
					num = (int)(2 + volumeSettings.quality.value);
					break;
				}
				FinalBlit(this, context, commandBuffer, renderingData, TiltShift.debug ? 4 : num);
			}

			public override void OnCameraCleanup(CommandBuffer cmd)
			{
				base.OnCameraCleanup(cmd);
			}
		}

		private TiltShiftRenderPass m_ScriptablePass;

		[SerializeField]
		public EffectBaseSettings settings = new EffectBaseSettings();

		public override void Create()
		{
			m_ScriptablePass = new TiltShiftRenderPass(settings);
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
