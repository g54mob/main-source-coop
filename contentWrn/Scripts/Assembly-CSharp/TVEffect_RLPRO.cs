using RetroLookPro.Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TVEffect_RLPRO : ScriptableRendererFeature
{
	public class TVEffect_RLPROPass : ScriptableRenderPass
	{
		private static readonly string k_RenderTag = "Renderr Glitch1 Effect";

		private static readonly int MainTexId = Shader.PropertyToID("_MainTex");

		private static readonly int fade = Shader.PropertyToID("fade");

		private static readonly int scale = Shader.PropertyToID("scale");

		private static readonly int hardScan = Shader.PropertyToID("hardScan");

		private static readonly int hardPix = Shader.PropertyToID("hardPix");

		private static readonly int resScale = Shader.PropertyToID("resScale");

		private static readonly int maskDark = Shader.PropertyToID("maskDark");

		private static readonly int maskLight = Shader.PropertyToID("maskLight");

		private static readonly int warp = Shader.PropertyToID("warp");

		private static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");

		private static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");

		private static readonly int _Mask = Shader.PropertyToID("_Mask");

		private TVEffect retroEffect;

		private Material RetroEffectMaterial;

		private RenderTargetIdentifier currentTarget;

		private float T;

		private float scaler;

		public TVEffect_RLPROPass(RenderPassEvent evt)
		{
			base.renderPassEvent = evt;
			Shader shader = Shader.Find("Hidden/Shader/TV_RLPRO");
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
			retroEffect = stack.GetComponent<TVEffect>();
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
			RetroEffectMaterial.SetFloat(fade, retroEffect.fade.value);
			RetroEffectMaterial.SetFloat(scale, retroEffect.scale.value);
			RetroEffectMaterial.SetFloat(hardScan, retroEffect.hardScan.value);
			RetroEffectMaterial.SetFloat(hardPix, retroEffect.hardPix.value);
			if (retroEffect.ScaleWithActualScreenSize.value)
			{
				scaler = retroEffect.resScale.value * ((float)(Screen.height * (Screen.width / Screen.height)) / 1000f);
			}
			else
			{
				scaler = retroEffect.resScale.value;
			}
			RetroEffectMaterial.SetFloat(resScale, scaler);
			RetroEffectMaterial.SetFloat(maskDark, retroEffect.maskDark.value);
			RetroEffectMaterial.SetFloat(maskLight, retroEffect.maskLight.value);
			RetroEffectMaterial.SetVector(warp, retroEffect.warp.value);
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
			cmd.Blit(renderTargetIdentifier, tempTargetId);
			cmd.Blit(tempTargetId, renderTargetIdentifier, RetroEffectMaterial, (!(retroEffect.warpMode == WarpMode.SimpleWarp)) ? 1 : 0);
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

		private float GetScale(int width, int height, Vector2 scalerReferenceResolution, float scalerMatchWidthOrHeight)
		{
			return Mathf.Pow((float)width / scalerReferenceResolution.x, 1f - scalerMatchWidthOrHeight) * Mathf.Pow((float)height / scalerReferenceResolution.y, scalerMatchWidthOrHeight);
		}
	}

	private TVEffect_RLPROPass RetroPass;

	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;

	public override void Create()
	{
		RetroPass = new TVEffect_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(RetroPass);
	}
}
