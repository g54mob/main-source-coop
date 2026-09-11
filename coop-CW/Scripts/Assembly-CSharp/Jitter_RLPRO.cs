using RetroLookPro.Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Jitter_RLPRO : ScriptableRendererFeature
{
	public class Jitter_RLPROPass : ScriptableRenderPass
	{
		private static readonly string k_RenderTag = "Renderr Glitch1 Effect";

		private static readonly int MainTexId = Shader.PropertyToID("_MainTex");

		private static readonly int screenLinesNumV = Shader.PropertyToID("screenLinesNum");

		private static readonly int time_V = Shader.PropertyToID("time_");

		private static readonly int twitchHFreqV = Shader.PropertyToID("twitchHFreq");

		private static readonly int twitchVFreqV = Shader.PropertyToID("twitchVFreq");

		private static readonly int jitterHAmountV = Shader.PropertyToID("jitterHAmount");

		private static readonly int jitterVAmountV = Shader.PropertyToID("jitterVAmount");

		private static readonly int jitterVSpeedV = Shader.PropertyToID("jitterVSpeed");

		private static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");

		private static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");

		private static readonly int _Mask = Shader.PropertyToID("_Mask");

		private Jitter retroEffect;

		private Material RetroEffectMaterial;

		private RenderTargetIdentifier currentTarget;

		private float _time;

		public Jitter_RLPROPass(RenderPassEvent evt)
		{
			base.renderPassEvent = evt;
			Shader shader = Shader.Find("Hidden/Shader/JitterEffect_RLPRO");
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
			retroEffect = stack.GetComponent<Jitter>();
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
			if (retroEffect.unscaledTime.value)
			{
				_time = Time.unscaledTime;
			}
			else
			{
				_time = Time.time;
			}
			RetroEffectMaterial.SetFloat(screenLinesNumV, retroEffect.stretchResolution.value);
			RetroEffectMaterial.SetFloat(time_V, _time);
			ParamSwitch(RetroEffectMaterial, retroEffect.twitchHorizontal.value, "VHS_TWITCH_H_ON");
			RetroEffectMaterial.SetFloat(twitchHFreqV, retroEffect.horizontalFreq.value);
			ParamSwitch(RetroEffectMaterial, retroEffect.twitchVertical.value, "VHS_TWITCH_V_ON");
			RetroEffectMaterial.SetFloat(twitchVFreqV, retroEffect.verticalFreq.value);
			ParamSwitch(RetroEffectMaterial, retroEffect.stretch.value, "VHS_STRETCH_ON");
			ParamSwitch(RetroEffectMaterial, retroEffect.jitterHorizontal.value, "VHS_JITTER_H_ON");
			RetroEffectMaterial.SetFloat(jitterHAmountV, retroEffect.jitterHorizontalAmount.value);
			ParamSwitch(RetroEffectMaterial, retroEffect.jitterVertical.value, "VHS_JITTER_V_ON");
			RetroEffectMaterial.SetFloat(jitterVAmountV, retroEffect.jitterVerticalAmount.value);
			RetroEffectMaterial.SetFloat(jitterVSpeedV, retroEffect.jitterVerticalSpeed.value);
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

	private Jitter_RLPROPass RetroPass;

	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;

	public override void Create()
	{
		RetroPass = new Jitter_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(RetroPass);
	}
}
