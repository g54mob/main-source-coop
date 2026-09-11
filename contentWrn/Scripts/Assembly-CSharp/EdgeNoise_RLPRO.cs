using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EdgeNoise_RLPRO : ScriptableRendererFeature
{
	public class EdgeNoise_RLPROPass : ScriptableRenderPass
	{
		private static readonly string k_RenderTag = "Renderr Glitch1 Effect";

		private static readonly int MainTexId = Shader.PropertyToID("_MainTex");

		private static readonly int _OffsetNoiseYV = Shader.PropertyToID("_OffsetNoiseY");

		private static readonly int _OffsetNoiseXV = Shader.PropertyToID("_OffsetNoiseX");

		private static readonly int _NoiseBottomHeightV = Shader.PropertyToID("_NoiseBottomHeight");

		private static readonly int _NoiseBottomIntensityV = Shader.PropertyToID("_NoiseBottomIntensity");

		private static readonly int _NoiseTextureV = Shader.PropertyToID("_NoiseTexture");

		private static readonly int tileXV = Shader.PropertyToID("tileX");

		private static readonly int tileYV = Shader.PropertyToID("tileY");

		private static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");

		private EdgeNoise retroEffect;

		private Material RetroEffectMaterial;

		private RenderTargetIdentifier currentTarget;

		public EdgeNoise_RLPROPass(RenderPassEvent evt)
		{
			base.renderPassEvent = evt;
			Shader shader = Shader.Find("Hidden/Shader/EdgeNoiseEffect_RLPRO");
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
			retroEffect = stack.GetComponent<EdgeNoise>();
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
			if (RetroEffectMaterial.HasProperty(_OffsetNoiseYV))
			{
				float num = RetroEffectMaterial.GetFloat(_OffsetNoiseYV);
				RetroEffectMaterial.SetFloat(_OffsetNoiseYV, num + Random.Range(-0.05f, 0.05f));
			}
			RetroEffectMaterial.SetFloat(_OffsetNoiseXV, Random.Range(0f, 1f));
			RetroEffectMaterial.SetFloat(_NoiseBottomHeightV, retroEffect.height.value);
			RetroEffectMaterial.SetFloat(_NoiseBottomIntensityV, retroEffect.intencity.value);
			if (retroEffect.noiseTexture.value != null)
			{
				RetroEffectMaterial.SetTexture(_NoiseTextureV, retroEffect.noiseTexture.value);
			}
			RetroEffectMaterial.SetFloat(tileXV, retroEffect.tile.value.x);
			RetroEffectMaterial.SetFloat(tileYV, retroEffect.tile.value.y);
			ParamSwitch(RetroEffectMaterial, retroEffect.top.value, "top_ON");
			ParamSwitch(RetroEffectMaterial, retroEffect.bottom.value, "bottom_ON");
			ParamSwitch(RetroEffectMaterial, retroEffect.left.value, "left_ON");
			ParamSwitch(RetroEffectMaterial, retroEffect.right.value, "right_ON");
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

	private EdgeNoise_RLPROPass RetroPass;

	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;

	public override void Create()
	{
		RetroPass = new EdgeNoise_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(RetroPass);
	}
}
