using RetroLookPro.Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VHSEffect_RLPRO : ScriptableRendererFeature
{
	public class VHSEffect_RLPROPass : ScriptableRenderPass
	{
		private static readonly string k_RenderTag = "Render VHS Effect";

		private static readonly int MainTexId = Shader.PropertyToID("_MainTex");

		private static readonly int TimeV = Shader.PropertyToID("Time");

		private static readonly int _OffsetPosY = Shader.PropertyToID("_OffsetPosY");

		private static readonly int smoothSize = Shader.PropertyToID("smoothSize");

		private static readonly int _StandardDeviation = Shader.PropertyToID("_StandardDeviation");

		private static readonly int iterations = Shader.PropertyToID("iterations");

		private static readonly int tileX = Shader.PropertyToID("tileX");

		private static readonly int smooth = Shader.PropertyToID("smooth1");

		private static readonly int tileY = Shader.PropertyToID("tileY");

		private static readonly int _OffsetDistortion = Shader.PropertyToID("_OffsetDistortion");

		private static readonly int _Stripes = Shader.PropertyToID("_Stripes");

		private static readonly int _OffsetColorAngle = Shader.PropertyToID("_OffsetColorAngle");

		private static readonly int _OffsetColor = Shader.PropertyToID("_OffsetColor");

		private static readonly int _OffsetNoiseX = Shader.PropertyToID("_OffsetNoiseX");

		private static readonly int _SecondaryTex = Shader.PropertyToID("_SecondaryTex");

		private static readonly int _OffsetNoiseY = Shader.PropertyToID("_OffsetNoiseY");

		private static readonly int _TexIntensity = Shader.PropertyToID("_TexIntensity");

		private static readonly int _TexCut = Shader.PropertyToID("_TexCut");

		private static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");

		private static readonly int _Mask = Shader.PropertyToID("_Mask");

		private static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");

		private float T;

		private VHSEffect retroEffect;

		private Material RetroEffectMaterial;

		private RenderTargetIdentifier currentTarget;

		public VHSEffect_RLPROPass(RenderPassEvent evt)
		{
			base.renderPassEvent = evt;
			Shader shader = Shader.Find("Hidden/Shader/VHSEffect_RLPRO");
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
			retroEffect = stack.GetComponent<VHSEffect>();
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
			if (!retroEffect.unscaledTime.value)
			{
				T += Time.deltaTime;
			}
			else
			{
				T += Time.unscaledDeltaTime;
			}
			RetroEffectMaterial.SetFloat(TimeV, T);
			if (Random.Range(0f, 100f - retroEffect.verticalOffsetFrequency.value) <= 5f)
			{
				if (retroEffect.verticalOffset == 0f)
				{
					RetroEffectMaterial.SetFloat(_OffsetPosY, retroEffect.verticalOffset.value);
				}
				if (retroEffect.verticalOffset.value > 0f)
				{
					RetroEffectMaterial.SetFloat(_OffsetPosY, retroEffect.verticalOffset.value - Random.Range(0f, retroEffect.verticalOffset.value));
				}
				else if (retroEffect.verticalOffset.value < 0f)
				{
					RetroEffectMaterial.SetFloat(_OffsetPosY, retroEffect.verticalOffset.value + Random.Range(0f, 0f - retroEffect.verticalOffset.value));
				}
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
			RetroEffectMaterial.SetFloat(iterations, retroEffect.iterations.value);
			RetroEffectMaterial.SetFloat(smoothSize, retroEffect.smoothSize.value);
			RetroEffectMaterial.SetFloat(_StandardDeviation, retroEffect.deviation.value);
			RetroEffectMaterial.SetFloat(tileX, retroEffect.tile.value.x);
			RetroEffectMaterial.SetFloat(smooth, retroEffect.smoothCut.value ? 1 : 0);
			RetroEffectMaterial.SetFloat(tileY, retroEffect.tile.value.y);
			RetroEffectMaterial.SetFloat(_OffsetDistortion, retroEffect.offsetDistortion.value);
			RetroEffectMaterial.SetFloat(_Stripes, 0.51f - retroEffect.stripes.value);
			RetroEffectMaterial.SetVector(_OffsetColorAngle, new Vector2(Mathf.Sin(retroEffect.colorOffsetAngle.value), Mathf.Cos(retroEffect.colorOffsetAngle.value)));
			RetroEffectMaterial.SetFloat(_OffsetColor, retroEffect.colorOffset.value * 0.001f);
			RetroEffectMaterial.SetFloat(_OffsetNoiseX, Random.Range(-0.4f, 0.4f));
			if (retroEffect.noiseTexture.value != null)
			{
				RetroEffectMaterial.SetTexture(_SecondaryTex, retroEffect.noiseTexture.value);
			}
			if (RetroEffectMaterial.HasProperty(_OffsetNoiseY))
			{
				float num = RetroEffectMaterial.GetFloat(_OffsetNoiseY);
				RetroEffectMaterial.SetFloat(_OffsetNoiseY, num + Random.Range(-0.03f, 0.03f));
			}
			RetroEffectMaterial.SetFloat(_TexIntensity, retroEffect._textureIntensity.value);
			RetroEffectMaterial.SetFloat(_TexCut, retroEffect._textureCutOff.value);
			cmd.Blit(renderTargetIdentifier, tempTargetId);
			cmd.Blit(tempTargetId, renderTargetIdentifier, RetroEffectMaterial, (int)retroEffect.blendMode.value);
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

	private VHSEffect_RLPROPass RetroPass;

	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;

	public override void Create()
	{
		RetroPass = new VHSEffect_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(RetroPass);
	}
}
