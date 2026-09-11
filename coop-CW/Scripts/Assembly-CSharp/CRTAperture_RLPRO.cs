using RetroLookPro.Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CRTAperture_RLPRO : ScriptableRendererFeature
{
	public class CRTAperture_RLPROPass : ScriptableRenderPass
	{
		private static readonly string k_RenderTag = "Renderr Glitch1 Effect";

		private static readonly int MainTexId = Shader.PropertyToID("_MainTex");

		private static readonly int GLOW_HALATIONV = Shader.PropertyToID("GLOW_HALATION");

		private static readonly int GLOW_DIFFUSIONV = Shader.PropertyToID("GLOW_DIFFUSION");

		private static readonly int MASK_COLORSV = Shader.PropertyToID("MASK_COLORS");

		private static readonly int MASK_STRENGTHV = Shader.PropertyToID("MASK_STRENGTH");

		private static readonly int GAMMA_INPUTV = Shader.PropertyToID("GAMMA_INPUT");

		private static readonly int GAMMA_OUTPUTV = Shader.PropertyToID("GAMMA_OUTPUT");

		private static readonly int BRIGHTNESSV = Shader.PropertyToID("BRIGHTNESS");

		private static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");

		private static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");

		private static readonly int _Mask = Shader.PropertyToID("_Mask");

		private CRTAperture retroEffect;

		private Material RetroEffectMaterial;

		private RenderTargetIdentifier currentTarget;

		public CRTAperture_RLPROPass(RenderPassEvent evt)
		{
			base.renderPassEvent = evt;
			Shader shader = Shader.Find("Hidden/Shader/CRTAperture_RLPRO");
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
			retroEffect = stack.GetComponent<CRTAperture>();
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
			int pass = 0;
			RetroEffectMaterial.SetFloat(GLOW_HALATIONV, retroEffect.GlowHalation.value);
			RetroEffectMaterial.SetFloat(GLOW_DIFFUSIONV, retroEffect.GlowDifusion.value);
			RetroEffectMaterial.SetFloat(MASK_COLORSV, retroEffect.MaskColors.value);
			RetroEffectMaterial.SetFloat(MASK_STRENGTHV, retroEffect.MaskStrength.value);
			RetroEffectMaterial.SetFloat(GAMMA_INPUTV, retroEffect.GammaInput.value);
			RetroEffectMaterial.SetFloat(GAMMA_OUTPUTV, retroEffect.GammaOutput.value);
			RetroEffectMaterial.SetFloat(BRIGHTNESSV, retroEffect.Brightness.value);
			if (retroEffect.mask.value != null)
			{
				RetroEffectMaterial.SetTexture(_Mask, retroEffect.mask.value);
				RetroEffectMaterial.SetFloat(_FadeMultiplier, 1f);
				ParamSwitch(RetroEffectMaterial, retroEffect.maskChannel.value == maskChannelMode.alphaChannel, "ALPHA_CHANNEL");
			}
			else
			{
				RetroEffectMaterial.SetFloat(_FadeMultiplier, 0f);
			}
			cmd.SetGlobalTexture(MainTexId, renderTargetIdentifier);
			cmd.GetTemporaryRT(tempTargetId, Screen.width, Screen.height, 0, FilterMode.Point, RenderTextureFormat.Default);
			cmd.Blit(renderTargetIdentifier, tempTargetId);
			cmd.Blit(tempTargetId, renderTargetIdentifier, RetroEffectMaterial, pass);
		}

		private void ParamSwitch(Material mat, bool paramValue, string paramName)
		{
			if (paramValue)
			{
				mat.EnableKeyword(paramName);
			}
			else
			{
				mat.DisableKeyword(paramName);
			}
		}
	}

	private CRTAperture_RLPROPass RetroPass;

	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;

	public override void Create()
	{
		RetroPass = new CRTAperture_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(RetroPass);
	}
}
