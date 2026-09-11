using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Fisheye_RLPRO : ScriptableRendererFeature
{
	public class Fisheye_RLPROPass : ScriptableRenderPass
	{
		private static readonly string k_RenderTag = "Renderr Glitch1 Effect";

		private static readonly int MainTexId = Shader.PropertyToID("_InputTexture");

		private static readonly int cutoffXV = Shader.PropertyToID("cutoffX");

		private static readonly int cutoffYV = Shader.PropertyToID("cutoffY");

		private static readonly int cutoffFadeXV = Shader.PropertyToID("cutoffFadeX");

		private static readonly int cutoffFadeYV = Shader.PropertyToID("cutoffFadeY");

		private static readonly int fisheyeBendV = Shader.PropertyToID("fisheyeBend");

		private static readonly int fisheyeSizeV = Shader.PropertyToID("fisheyeSize");

		private static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");

		private Fisheye retroEffect;

		private Material RetroEffectMaterial;

		private RenderTargetIdentifier currentTarget;

		public Fisheye_RLPROPass(RenderPassEvent evt)
		{
			base.renderPassEvent = evt;
			Shader shader = Shader.Find("Hidden/Shader/FisheyeEffect_RLPRO");
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
			retroEffect = stack.GetComponent<Fisheye>();
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
			ParamSwitch(RetroEffectMaterial, paramValue: true, "VHS_FISHEYE_ON");
			RetroEffectMaterial.SetFloat(cutoffXV, retroEffect.cutOffX.value);
			RetroEffectMaterial.SetFloat(cutoffYV, retroEffect.cutOffY.value);
			RetroEffectMaterial.SetFloat(cutoffFadeXV, retroEffect.fadeX.value);
			RetroEffectMaterial.SetFloat(cutoffFadeYV, retroEffect.fadeY.value);
			ParamSwitch(RetroEffectMaterial, retroEffect.fisheyeType.value == FisheyeTypeEnum.Hyperspace, "VHS_FISHEYE_HYPERSPACE");
			RetroEffectMaterial.SetFloat(fisheyeBendV, retroEffect.bend.value);
			RetroEffectMaterial.SetFloat(fisheyeSizeV, retroEffect.size.value);
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

	private Fisheye_RLPROPass RetroPass;

	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;

	public override void Create()
	{
		RetroPass = new Fisheye_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(RetroPass);
	}
}
