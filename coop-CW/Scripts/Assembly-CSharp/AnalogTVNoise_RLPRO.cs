using RetroLookPro.Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AnalogTVNoise_RLPRO : ScriptableRendererFeature
{
	public class AnalogTVNoise_RLPROPass : ScriptableRenderPass
	{
		private static readonly string k_RenderTag = "Render Analog TV Noise Effect";

		private static readonly int MainTexId = Shader.PropertyToID("_MainTex");

		private static readonly int TimeXV = Shader.PropertyToID("TimeX");

		private static readonly int _PatternV = Shader.PropertyToID("_Pattern");

		private static readonly int barHeightV = Shader.PropertyToID("barHeight");

		private static readonly int barSpeedV = Shader.PropertyToID("barSpeed");

		private static readonly int cutV = Shader.PropertyToID("cut");

		private static readonly int edgeCutOffV = Shader.PropertyToID("edgeCutOff");

		private static readonly int angleV = Shader.PropertyToID("angle");

		private static readonly int tileXV = Shader.PropertyToID("tileX");

		private static readonly int tileYV = Shader.PropertyToID("tileY");

		private static readonly int horizontalV = Shader.PropertyToID("horizontal");

		private static readonly int _OffsetNoiseXV = Shader.PropertyToID("_OffsetNoiseX");

		private static readonly int _OffsetNoiseYV = Shader.PropertyToID("_OffsetNoiseY");

		private static readonly int _FadeV = Shader.PropertyToID("_Fade");

		private static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");

		private static readonly int _Mask = Shader.PropertyToID("_Mask");

		private static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");

		private AnalogTVNoise retroEffect;

		private Material RetroEffectMaterial;

		private RenderTargetIdentifier currentTarget;

		private float TimeX;

		public AnalogTVNoise_RLPROPass(RenderPassEvent evt)
		{
			base.renderPassEvent = evt;
			Shader shader = Shader.Find("Hidden/Shader/AnalogTVNoiseEffect_RLPRO");
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
			retroEffect = stack.GetComponent<AnalogTVNoise>();
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
			TimeX += Time.deltaTime;
			if (TimeX > 100f)
			{
				TimeX = 0f;
			}
			RetroEffectMaterial.SetFloat(TimeXV, TimeX);
			RetroEffectMaterial.SetFloat(_FadeV, retroEffect.Fade.value);
			if (retroEffect.texture.value != null)
			{
				RetroEffectMaterial.SetTexture(_PatternV, retroEffect.texture.value);
			}
			RetroEffectMaterial.SetFloat(barHeightV, retroEffect.barWidth.value);
			RetroEffectMaterial.SetFloat(barSpeedV, retroEffect.barSpeed.value);
			RetroEffectMaterial.SetFloat(cutV, retroEffect.CutOff.value);
			RetroEffectMaterial.SetFloat(edgeCutOffV, retroEffect.edgeCutOff.value);
			RetroEffectMaterial.SetFloat(angleV, retroEffect.textureAngle.value);
			RetroEffectMaterial.SetFloat(tileXV, retroEffect.tile.value.x);
			RetroEffectMaterial.SetFloat(tileYV, retroEffect.tile.value.y);
			RetroEffectMaterial.SetFloat(horizontalV, retroEffect.Horizontal.value ? 1 : 0);
			if (!retroEffect.staticNoise.value)
			{
				RetroEffectMaterial.SetFloat(_OffsetNoiseXV, Random.Range(0f, 0.6f));
				RetroEffectMaterial.SetFloat(_OffsetNoiseYV, Random.Range(0f, 0.6f));
			}
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

	private AnalogTVNoise_RLPROPass RetroPass;

	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;

	public override void Create()
	{
		RetroPass = new AnalogTVNoise_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(RetroPass);
	}
}
