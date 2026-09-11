using RetroLookPro.Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PictureCorrection_RLPRO : ScriptableRendererFeature
{
	public class PictureCorrection_RLPROPass : ScriptableRenderPass
	{
		private static readonly string k_RenderTag = "Renderr Glitch1 Effect";

		private static readonly int MainTexId = Shader.PropertyToID("_MainTex");

		private static readonly int signalAdjustY = Shader.PropertyToID("signalAdjustY");

		private static readonly int signalAdjustI = Shader.PropertyToID("signalAdjustI");

		private static readonly int signalAdjustQ = Shader.PropertyToID("signalAdjustQ");

		private static readonly int signalShiftY = Shader.PropertyToID("signalShiftY");

		private static readonly int signalShiftI = Shader.PropertyToID("signalShiftI");

		private static readonly int signalShiftQ = Shader.PropertyToID("signalShiftQ");

		private static readonly int gammaCorection = Shader.PropertyToID("gammaCorection");

		private static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");

		private static readonly int _Mask = Shader.PropertyToID("_Mask");

		private static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");

		private PictureCorrection retroEffect;

		private Material RetroEffectMaterial;

		private RenderTargetIdentifier currentTarget;

		public PictureCorrection_RLPROPass(RenderPassEvent evt)
		{
			base.renderPassEvent = evt;
			Shader shader = Shader.Find("Hidden/Shader/PictureCorrectionEffect_RLPRO");
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
			retroEffect = stack.GetComponent<PictureCorrection>();
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
			RetroEffectMaterial.SetFloat(signalAdjustY, retroEffect.signalAdjustY.value);
			RetroEffectMaterial.SetFloat(signalAdjustI, retroEffect.signalAdjustI.value);
			RetroEffectMaterial.SetFloat(signalAdjustQ, retroEffect.signalAdjustQ.value);
			RetroEffectMaterial.SetFloat(signalShiftY, retroEffect.signalShiftY.value);
			RetroEffectMaterial.SetFloat(signalShiftI, retroEffect.signalShiftI.value);
			RetroEffectMaterial.SetFloat(signalShiftQ, retroEffect.signalShiftQ.value);
			RetroEffectMaterial.SetFloat(gammaCorection, retroEffect.gammaCorection.value);
			cmd.SetGlobalTexture(MainTexId, renderTargetIdentifier);
			cmd.GetTemporaryRT(tempTargetId, Screen.width, Screen.height, 0, FilterMode.Point, RenderTextureFormat.Default);
			cmd.Blit(renderTargetIdentifier, tempTargetId);
			cmd.Blit(tempTargetId, renderTargetIdentifier, RetroEffectMaterial, 0);
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

	private PictureCorrection_RLPROPass RetroPass;

	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;

	public override void Create()
	{
		RetroPass = new PictureCorrection_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(RetroPass);
	}
}
