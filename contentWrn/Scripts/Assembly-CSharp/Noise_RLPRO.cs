using RetroLookPro.Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Noise_RLPRO : ScriptableRendererFeature
{
	public class Noise_RLPROPass : ScriptableRenderPass
	{
		private static readonly string k_RenderTag = "Renderr Glitch1 Effect";

		private static readonly int MainTexId = Shader.PropertyToID("_MainTex");

		private static readonly int alphaTexV = Shader.PropertyToID("alphaTex");

		private static readonly int _AlphaMapTexV = Shader.PropertyToID("_AlphaMapTex");

		private static readonly int tapeLinesAmountV = Shader.PropertyToID("tapeLinesAmount");

		private static readonly int time_V = Shader.PropertyToID("time_");

		private static readonly int screenLinesNumV = Shader.PropertyToID("screenLinesNum");

		private static readonly int noiseLinesNumV = Shader.PropertyToID("noiseLinesNum");

		private static readonly int noiseQuantizeXV = Shader.PropertyToID("noiseQuantizeX");

		private static readonly int signalNoisePowerV = Shader.PropertyToID("signalNoisePower");

		private static readonly int signalNoiseAmountV = Shader.PropertyToID("signalNoiseAmount");

		private static readonly int filmGrainAmountV = Shader.PropertyToID("filmGrainAmount");

		private static readonly int tapeNoiseTHV = Shader.PropertyToID("tapeNoiseTH");

		private static readonly int tapeNoiseAmountV = Shader.PropertyToID("tapeNoiseAmount");

		private static readonly int tapeNoiseSpeedV = Shader.PropertyToID("tapeNoiseSpeed");

		private static readonly int lineNoiseAmountV = Shader.PropertyToID("lineNoiseAmount");

		private static readonly int lineNoiseSpeedV = Shader.PropertyToID("lineNoiseSpeed");

		private static readonly int _TapeTexV = Shader.PropertyToID("_TapeTex");

		private static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");

		private static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");

		private static readonly int _Mask = Shader.PropertyToID("_Mask");

		private Noise retroEffect;

		private Material RetroEffectMaterial;

		private RenderTargetIdentifier currentTarget;

		private float _time;

		private RenderTexture texTape;

		public Noise_RLPROPass(RenderPassEvent evt)
		{
			base.renderPassEvent = evt;
			Shader shader = Shader.Find("Hidden/Shader/NoiseEffects_RLPRO");
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
			retroEffect = stack.GetComponent<Noise>();
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
			if (retroEffect.unscaledTime.value)
			{
				_time = Time.unscaledTime;
			}
			else
			{
				_time = Time.time;
			}
			cmd.GetTemporaryRT(tempTargetId, Screen.width, Screen.height, 0, FilterMode.Point, RenderTextureFormat.Default);
			float num = retroEffect.stretchResolution.value;
			if (num <= 0f)
			{
				num = Screen.height;
			}
			if (texTape == null || (float)texTape.height != Mathf.Min(retroEffect.VerticalResolution.value, num))
			{
				int num2 = (int)Mathf.Min(retroEffect.VerticalResolution.value, num);
				int width = (int)((float)num2 * (float)Screen.width / (float)Screen.height);
				Object.Destroy(texTape);
				texTape = new RenderTexture(width, num2, 0);
				texTape.hideFlags = HideFlags.HideAndDontSave;
				texTape.filterMode = FilterMode.Point;
				texTape.Create();
				cmd.Blit(tempTargetId, texTape, RetroEffectMaterial, 0);
			}
			cmd.SetGlobalTexture(MainTexId, renderTargetIdentifier);
			cmd.Blit(renderTargetIdentifier, tempTargetId);
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
			RetroEffectMaterial.SetFloat(tapeLinesAmountV, 1f - retroEffect.tapeLinesAmount.value);
			RetroEffectMaterial.SetFloat(time_V, _time);
			RetroEffectMaterial.SetFloat(screenLinesNumV, num);
			RetroEffectMaterial.SetFloat(noiseLinesNumV, retroEffect.VerticalResolution.value);
			RetroEffectMaterial.SetFloat(noiseQuantizeXV, retroEffect.TapeNoiseSignalProcessing.value);
			ParamSwitch(RetroEffectMaterial, retroEffect.Granularity.value, "VHS_FILMGRAIN_ON");
			ParamSwitch(RetroEffectMaterial, retroEffect.TapeNoise.value, "VHS_TAPENOISE_ON");
			ParamSwitch(RetroEffectMaterial, retroEffect.LineNoise.value, "VHS_LINENOISE_ON");
			ParamSwitch(RetroEffectMaterial, retroEffect.SignalNoise.value, "VHS_YIQNOISE_ON");
			RetroEffectMaterial.SetFloat(signalNoisePowerV, retroEffect.SignalNoisePower.value);
			RetroEffectMaterial.SetFloat(signalNoiseAmountV, retroEffect.SignalNoiseAmount.value);
			RetroEffectMaterial.SetFloat(filmGrainAmountV, retroEffect.GranularityAmount.value);
			RetroEffectMaterial.SetFloat(tapeNoiseTHV, retroEffect.TapeNoiseAmount.value);
			RetroEffectMaterial.SetFloat(tapeNoiseAmountV, retroEffect.TapeNoiseFade.value);
			RetroEffectMaterial.SetFloat(tapeNoiseSpeedV, retroEffect.TapeNoiseSpeed.value);
			RetroEffectMaterial.SetFloat(lineNoiseAmountV, retroEffect.LineNoiseAmount.value);
			RetroEffectMaterial.SetFloat(lineNoiseSpeedV, retroEffect.LineNoiseSpeed.value);
			cmd.Blit(texTape, texTape, RetroEffectMaterial, 1);
			RetroEffectMaterial.SetTexture(_TapeTexV, texTape);
			cmd.Blit(texTape, texTape, RetroEffectMaterial, 1);
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

	private Noise_RLPROPass RetroPass;

	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;

	public override void Create()
	{
		RetroPass = new Noise_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(RetroPass);
	}
}
