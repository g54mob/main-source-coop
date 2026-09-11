using RetroLookPro.Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Phosphor_RLPRO : ScriptableRendererFeature
{
	public class Phosphor_RLPROPass : ScriptableRenderPass
	{
		private static readonly string k_RenderTag = "Renderr Glitch1 Effect";

		private static readonly int MainTexId = Shader.PropertyToID("_InputTexture");

		private static readonly int TV = Shader.PropertyToID("T");

		private static readonly int speedV = Shader.PropertyToID("speed");

		private static readonly int fadeV = Shader.PropertyToID("fade");

		private static readonly int _TexV = Shader.PropertyToID("_Tex");

		private static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");

		private static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");

		private static readonly int _Mask = Shader.PropertyToID("_Mask");

		private Phosphor retroEffect;

		private Material RetroEffectMaterial;

		private RenderTargetIdentifier currentTarget;

		private RenderTexture texTape;

		private float T;

		public Phosphor_RLPROPass(RenderPassEvent evt)
		{
			base.renderPassEvent = evt;
			Shader shader = Shader.Find("Hidden/Shader/Phosphor_RLPRO");
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
			retroEffect = stack.GetComponent<Phosphor>();
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
			cmd.Blit(renderTargetIdentifier, tempTargetId);
			if (texTape == null)
			{
				texTape = new RenderTexture(Screen.width, Screen.height, 1);
			}
			T = Time.time;
			RetroEffectMaterial.SetFloat(TV, T);
			RetroEffectMaterial.SetFloat(speedV, retroEffect.width.value);
			RetroEffectMaterial.SetFloat(fadeV, retroEffect.fade.value);
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
			cmd.Blit(tempTargetId, texTape, RetroEffectMaterial, 1);
			RetroEffectMaterial.SetTexture(_TexV, texTape);
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

	private Phosphor_RLPROPass RetroPass;

	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;

	public override void Create()
	{
		RetroPass = new Phosphor_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(RetroPass);
	}
}
