using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LimitlessVHSTapeRewind : ScriptableRendererFeature
{
	public class LimitlessVHSTapeRewindPass : ScriptableRenderPass
	{
		private static readonly string k_RenderTag = "Render Glitch1 Effect";

		private static readonly int MainTexId = Shader.PropertyToID("_MainTex");

		private static readonly int TempTargetId = Shader.PropertyToID("Glitch1");

		private static readonly int NOISE_STATIC = Shader.PropertyToID("NOISE_STATIC");

		private static readonly int intencity = Shader.PropertyToID("intencity");

		private static readonly int fade = Shader.PropertyToID("fade");

		private VHSTapeRewind m_VHSNoise;

		private Material VHSNoiseMaterial;

		private RenderTargetIdentifier currentTarget;

		public LimitlessVHSTapeRewindPass(RenderPassEvent evt)
		{
			base.renderPassEvent = evt;
			Shader shader = Shader.Find("Hidden/Shader/VHS_Tape_Rewind");
			if (shader == null)
			{
				Debug.LogError("Shader not found.");
			}
			else
			{
				VHSNoiseMaterial = CoreUtils.CreateEngineMaterial(shader);
			}
		}

		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			ScriptableRenderer renderer = renderingData.cameraData.renderer;
			currentTarget = renderer.cameraColorTargetHandle;
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (VHSNoiseMaterial == null)
			{
				Debug.LogError("Material not created.");
				return;
			}
			VolumeStack stack = VolumeManager.instance.stack;
			m_VHSNoise = stack.GetComponent<VHSTapeRewind>();
			if ((renderingData.cameraData.postProcessEnabled || !m_VHSNoise.GlobalPostProcessingSettings.value) && !(m_VHSNoise == null) && m_VHSNoise.IsActive())
			{
				CommandBuffer commandBuffer = CommandBufferPool.Get(k_RenderTag);
				Render(commandBuffer, ref renderingData);
				context.ExecuteCommandBuffer(commandBuffer);
				CommandBufferPool.Release(commandBuffer);
			}
		}

		public void Setup(in RenderTargetIdentifier currentTarget)
		{
			this.currentTarget = currentTarget;
		}

		private void Render(CommandBuffer cmd, ref RenderingData renderingData)
		{
			RenderTargetIdentifier renderTargetIdentifier = currentTarget;
			int tempTargetId = TempTargetId;
			VHSNoiseMaterial.SetFloat(intencity, m_VHSNoise.intencity.value);
			VHSNoiseMaterial.SetFloat(fade, m_VHSNoise.fade.value);
			cmd.GetTemporaryRT(tempTargetId, Screen.width, Screen.height, 0, FilterMode.Bilinear, RenderTextureFormat.Default);
			cmd.SetGlobalTexture(MainTexId, renderTargetIdentifier);
			cmd.Blit(renderTargetIdentifier, tempTargetId);
			cmd.Blit(tempTargetId, renderTargetIdentifier, VHSNoiseMaterial, 0);
		}
	}

	private LimitlessVHSTapeRewindPass Pass;

	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;

	public override void Create()
	{
		Pass = new LimitlessVHSTapeRewindPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(Pass);
	}
}
