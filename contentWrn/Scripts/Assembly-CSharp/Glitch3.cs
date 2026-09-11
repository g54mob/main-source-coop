using RetroLookPro.Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Glitch3 : ScriptableRendererFeature
{
	public class Glitch3Pass : ScriptableRenderPass
	{
		private static readonly string k_RenderTag = "Render Glitch3 Effect";

		private static readonly int MainTexId = Shader.PropertyToID("_MainTex");

		private static readonly int maxOffsetY = Shader.PropertyToID("maxOffsetY");

		private static readonly int maxOffsetX = Shader.PropertyToID("maxOffsetX");

		private static readonly int blockSize = Shader.PropertyToID("blockSize");

		private static readonly int speed = Shader.PropertyToID("speed");

		private static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");

		private static readonly int _Mask = Shader.PropertyToID("_Mask");

		private static readonly int TempTargetId = Shader.PropertyToID("Glitch3");

		private LimitlessGlitch3 Glitch3;

		private Material Glitch3Material;

		private RenderTargetIdentifier currentTarget;

		public Glitch3Pass(RenderPassEvent evt)
		{
			base.renderPassEvent = evt;
			Shader shader = Shader.Find("RetroLookPro/Glitch3");
			if (shader == null)
			{
				Debug.LogError("Shader not found.");
			}
			else
			{
				Glitch3Material = CoreUtils.CreateEngineMaterial(shader);
			}
		}

		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			ScriptableRenderer renderer = renderingData.cameraData.renderer;
			currentTarget = renderer.cameraColorTargetHandle;
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (Glitch3Material == null)
			{
				Debug.LogError("Material not created.");
				return;
			}
			VolumeStack stack = VolumeManager.instance.stack;
			Glitch3 = stack.GetComponent<LimitlessGlitch3>();
			if ((renderingData.cameraData.postProcessEnabled || !Glitch3.GlobalPostProcessingSettings.value) && !(Glitch3 == null) && Glitch3.IsActive())
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
			if (Glitch3.mask.value != null)
			{
				Glitch3Material.SetTexture(_Mask, Glitch3.mask.value);
				Glitch3Material.SetFloat(_FadeMultiplier, 1f);
				ParamSwitch(Glitch3Material, Glitch3.maskChannel.value == maskChannelMode.alphaChannel, "ALPHA_CHANNEL");
			}
			else
			{
				Glitch3Material.SetFloat(_FadeMultiplier, 0f);
			}
			Glitch3Material.SetFloat(speed, Glitch3.speed.value);
			Glitch3Material.SetFloat(blockSize, Glitch3.blockSize.value);
			Glitch3Material.SetFloat(maxOffsetX, Glitch3.maxOffsetX.value);
			Glitch3Material.SetFloat(maxOffsetY, Glitch3.maxOffsetY.value);
			cmd.SetGlobalTexture(MainTexId, renderTargetIdentifier);
			cmd.GetTemporaryRT(tempTargetId, Screen.width, Screen.height, 0, FilterMode.Point, RenderTextureFormat.Default);
			cmd.Blit(renderTargetIdentifier, tempTargetId);
			cmd.Blit(tempTargetId, renderTargetIdentifier, Glitch3Material, pass);
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

	private Glitch3Pass GlitchPass;

	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;

	public override void Create()
	{
		GlitchPass = new Glitch3Pass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		LimitlessGlitch3 component = VolumeManager.instance.stack.GetComponent<LimitlessGlitch3>();
		if (!(component == null) && component.IsActive())
		{
			renderer.EnqueuePass(GlitchPass);
		}
	}
}
