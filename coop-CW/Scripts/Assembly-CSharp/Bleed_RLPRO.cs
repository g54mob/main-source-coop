using RetroLookPro.Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Bleed_RLPRO : ScriptableRendererFeature
{
	public class Bleed_RLPROPass : ScriptableRenderPass
	{
		private static readonly string k_RenderTag = "Renderr Glitch1 Effect";

		private static readonly int MainTexId = Shader.PropertyToID("_MainTex");

		private static readonly int bleedAmount = Shader.PropertyToID("bleedAmount");

		private static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");

		private static readonly int _Mask = Shader.PropertyToID("_Mask");

		private static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");

		private Bleed retroEffect;

		private Material RetroEffectMaterial;

		private RenderTargetIdentifier currentTarget;

		public Bleed_RLPROPass(RenderPassEvent evt)
		{
			base.renderPassEvent = evt;
			Shader shader = Shader.Find("Hidden/Shader/Bleed_RLPRO_HDRP");
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
			retroEffect = stack.GetComponent<Bleed>();
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
			int value = (int)retroEffect.bleedMode.value;
			ParamSwitch(RetroEffectMaterial, retroEffect.bleedDebug.value, "VHS_DEBUG_BLEEDING_ON");
			RetroEffectMaterial.SetFloat(bleedAmount, retroEffect.bleedAmount.value);
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
			cmd.Blit(tempTargetId, renderTargetIdentifier, RetroEffectMaterial, value);
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

	private Bleed_RLPROPass RetroPass;

	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;

	public override void Create()
	{
		RetroPass = new Bleed_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(RetroPass);
	}
}
