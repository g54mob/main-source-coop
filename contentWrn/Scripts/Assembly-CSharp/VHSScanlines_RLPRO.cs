using RetroLookPro.Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VHSScanlines_RLPRO : ScriptableRendererFeature
{
	public class VHSScanlines_RLPROPass : ScriptableRenderPass
	{
		private static readonly string k_RenderTag = "Renderr Glitch1 Effect";

		private static readonly int MainTexId = Shader.PropertyToID("_MainTex");

		private static readonly int TimeV = Shader.PropertyToID("Time");

		private static readonly int _ScanLinesV = Shader.PropertyToID("_ScanLines");

		private static readonly int speedV = Shader.PropertyToID("speed");

		private static readonly int fadeV = Shader.PropertyToID("fade");

		private static readonly int _OffsetDistortionV = Shader.PropertyToID("_OffsetDistortion");

		private static readonly int sfericalV = Shader.PropertyToID("sferical");

		private static readonly int barrelV = Shader.PropertyToID("barrel");

		private static readonly int scaleV = Shader.PropertyToID("scale");

		private static readonly int _ScanLinesColorV = Shader.PropertyToID("_ScanLinesColor");

		private static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");

		private static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");

		private static readonly int _Mask = Shader.PropertyToID("_Mask");

		private float T;

		private VHSScanlines retroEffect;

		private Material RetroEffectMaterial;

		private RenderTargetIdentifier currentTarget;

		public VHSScanlines_RLPROPass(RenderPassEvent evt)
		{
			base.renderPassEvent = evt;
			Shader shader = Shader.Find("Hidden/Shader/VHSScanlinesEffect_RLPRO");
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
			retroEffect = stack.GetComponent<VHSScanlines>();
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
			T += Time.deltaTime;
			RetroEffectMaterial.SetFloat(TimeV, T);
			RetroEffectMaterial.SetFloat(_ScanLinesV, retroEffect.scanLines.value);
			RetroEffectMaterial.SetFloat(speedV, retroEffect.speed.value);
			RetroEffectMaterial.SetFloat(_OffsetDistortionV, retroEffect.distortion.value);
			RetroEffectMaterial.SetFloat(fadeV, retroEffect.fade.value);
			RetroEffectMaterial.SetFloat(sfericalV, retroEffect.distortion1.value);
			RetroEffectMaterial.SetFloat(barrelV, retroEffect.distortion2.value);
			RetroEffectMaterial.SetFloat(scaleV, retroEffect.scale.value);
			RetroEffectMaterial.SetColor(_ScanLinesColorV, retroEffect.scanLinesColor.value);
			int pass = (retroEffect.horizontal.value ? ((retroEffect.distortion.value != 0f) ? 1 : 0) : ((retroEffect.distortion.value == 0f) ? 2 : 3));
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

	private VHSScanlines_RLPROPass RetroPass;

	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;

	public override void Create()
	{
		RetroPass = new VHSScanlines_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(RetroPass);
	}
}
