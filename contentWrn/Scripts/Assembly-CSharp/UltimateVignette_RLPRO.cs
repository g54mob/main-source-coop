using RetroLookPro.Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UltimateVignette_RLPRO : ScriptableRendererFeature
{
	public class UltimateVignette_RLPROPass : ScriptableRenderPass
	{
		private static readonly string k_RenderTag = "Renderr Glitch1 Effect";

		private static readonly int MainTexId = Shader.PropertyToID("_InputTexture");

		private static readonly int _Params = Shader.PropertyToID("_Params");

		private static readonly int _InnerColor = Shader.PropertyToID("_InnerColor");

		private static readonly int _Center = Shader.PropertyToID("_Center");

		private static readonly int _Params1 = Shader.PropertyToID("_Params1");

		private static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");

		private static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");

		private static readonly int _Mask = Shader.PropertyToID("_Mask");

		private UltimateVignette retroEffect;

		private Material RetroEffectMaterial;

		private RenderTargetIdentifier currentTarget;

		public UltimateVignette_RLPROPass(RenderPassEvent evt)
		{
			base.renderPassEvent = evt;
			Shader shader = Shader.Find("Hidden/Shader/UltimateVignetteEffect_RLPRO");
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
			retroEffect = stack.GetComponent<UltimateVignette>();
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
			RetroEffectMaterial.DisableKeyword("VIGNETTE_CIRCLE");
			RetroEffectMaterial.DisableKeyword("VIGNETTE_ROUNDEDCORNERS");
			switch (retroEffect.vignetteShape.value)
			{
			case VignetteShape.circle:
				RetroEffectMaterial.EnableKeyword("VIGNETTE_CIRCLE");
				break;
			case VignetteShape.roundedCorners:
				RetroEffectMaterial.EnableKeyword("VIGNETTE_ROUNDEDCORNERS");
				break;
			}
			RetroEffectMaterial.SetVector(_Params, new Vector4(retroEffect.edgeSoftness.value * 0.01f, retroEffect.vignetteAmount.value * 0.02f, retroEffect.innerColorAlpha.value * 0.01f, retroEffect.edgeBlend.value * 0.01f));
			RetroEffectMaterial.SetColor(_InnerColor, retroEffect.innerColor.value);
			RetroEffectMaterial.SetVector(_Center, retroEffect.center.value);
			RetroEffectMaterial.SetVector(_Params1, new Vector2(retroEffect.vignetteFineTune.value, 0.8f));
			cmd.Blit(tempTargetId, renderTargetIdentifier, RetroEffectMaterial, 0);
		}
	}

	private UltimateVignette_RLPROPass RetroPass;

	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;

	public override void Create()
	{
		RetroPass = new UltimateVignette_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(RetroPass);
	}
}
