using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EdgeStretch_RLPRO : ScriptableRendererFeature
{
	public class EdgeStretch_RLPROPass : ScriptableRenderPass
	{
		private static readonly string k_RenderTag = "Renderr Glitch1 Effect";

		private static readonly int MainTexId = Shader.PropertyToID("_MainTex");

		private static readonly int TimeV = Shader.PropertyToID("Time");

		private static readonly int _NoiseBottomHeightV = Shader.PropertyToID("_NoiseBottomHeight");

		private static readonly int frequencyV = Shader.PropertyToID("frequency");

		private static readonly int amplitudeV = Shader.PropertyToID("amplitude");

		private static readonly int speedV = Shader.PropertyToID("speed");

		private static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");

		private EdgeStretch retroEffect;

		private Material RetroEffectMaterial;

		private RenderTargetIdentifier currentTarget;

		private float T;

		public EdgeStretch_RLPROPass(RenderPassEvent evt)
		{
			base.renderPassEvent = evt;
			Shader shader = Shader.Find("Hidden/Shader/EdgeStretchEffect_RLPRO");
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
			retroEffect = stack.GetComponent<EdgeStretch>();
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
			int num = 0;
			num = ((!retroEffect.distort.value) ? 2 : (retroEffect.distortRandomly.value ? 1 : 0));
			T += Time.deltaTime;
			RetroEffectMaterial.SetFloat(TimeV, T);
			RetroEffectMaterial.SetFloat(_NoiseBottomHeightV, retroEffect.height.value);
			RetroEffectMaterial.SetFloat(frequencyV, retroEffect.frequency.value);
			RetroEffectMaterial.SetFloat(amplitudeV, retroEffect.amplitude.value);
			RetroEffectMaterial.SetFloat(speedV, retroEffect.speed.value);
			ParamSwitch(RetroEffectMaterial, retroEffect.top.value, "top_ON");
			ParamSwitch(RetroEffectMaterial, retroEffect.bottom.value, "bottom_ON");
			ParamSwitch(RetroEffectMaterial, retroEffect.left.value, "left_ON");
			ParamSwitch(RetroEffectMaterial, retroEffect.right.value, "right_ON");
			cmd.SetGlobalTexture(MainTexId, renderTargetIdentifier);
			cmd.GetTemporaryRT(tempTargetId, Screen.width, Screen.height, 0, FilterMode.Point, RenderTextureFormat.Default);
			cmd.Blit(renderTargetIdentifier, tempTargetId);
			cmd.Blit(tempTargetId, renderTargetIdentifier, RetroEffectMaterial, num);
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

	private EdgeStretch_RLPROPass RetroPass;

	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;

	public override void Create()
	{
		RetroPass = new EdgeStretch_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(RetroPass);
	}
}
