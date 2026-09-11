using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PulsatingVignette_RLPRO : ScriptableRendererFeature
{
	public class PulsatingVignette_RLPROPass : ScriptableRenderPass
	{
		private static readonly string k_RenderTag = "Renderr Glitch1 Effect";

		private static readonly int MainTexId = Shader.PropertyToID("_InputTexture");

		private static readonly int TimeV = Shader.PropertyToID("Time");

		private static readonly int vignetteSpeed = Shader.PropertyToID("vignetteSpeed");

		private static readonly int vignetteAmount = Shader.PropertyToID("vignetteAmount");

		private static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");

		private PulsatingVignette retroEffect;

		private Material RetroEffectMaterial;

		private RenderTargetIdentifier currentTarget;

		private float T;

		public PulsatingVignette_RLPROPass(RenderPassEvent evt)
		{
			base.renderPassEvent = evt;
			Shader shader = Shader.Find("Hidden/Shader/PulsatingVignetteEffect_RLPRO");
			if (shader == null)
			{
				Debug.LogError("Shader not found.");
			}
			else
			{
				RetroEffectMaterial = CoreUtils.CreateEngineMaterial(shader);
			}
		}

		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			ScriptableRenderer renderer = renderingData.cameraData.renderer;
			currentTarget = renderer.cameraColorTargetHandle;
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (RetroEffectMaterial == null)
			{
				Debug.LogError("Material not created.");
				return;
			}
			VolumeStack stack = VolumeManager.instance.stack;
			retroEffect = stack.GetComponent<PulsatingVignette>();
			if ((renderingData.cameraData.postProcessEnabled || !retroEffect.GlobalPostProcessingSettings.value) && !(retroEffect == null) && retroEffect.IsActive())
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
			cmd.SetGlobalTexture(MainTexId, renderTargetIdentifier);
			cmd.GetTemporaryRT(tempTargetId, Screen.width, Screen.height, 0, FilterMode.Point, RenderTextureFormat.Default);
			T += Time.deltaTime;
			RetroEffectMaterial.SetFloat(TimeV, T);
			RetroEffectMaterial.SetFloat(vignetteSpeed, retroEffect.speed.value);
			RetroEffectMaterial.SetFloat(vignetteAmount, retroEffect.amount.value);
			cmd.Blit(tempTargetId, renderTargetIdentifier, RetroEffectMaterial, 0);
		}
	}

	private PulsatingVignette_RLPROPass RetroPass;

	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;

	public override void Create()
	{
		RetroPass = new PulsatingVignette_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(RetroPass);
	}
}
