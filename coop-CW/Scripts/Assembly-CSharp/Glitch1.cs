using RetroLookPro.Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Glitch1 : ScriptableRendererFeature
{
	public class Glitch1Pass : ScriptableRenderPass
	{
		private static readonly string k_RenderTag = "Render Glitch1 Effect";

		private static readonly int MainTexId = Shader.PropertyToID("_MainTex");

		private static readonly int Strength = Shader.PropertyToID("Strength");

		private static readonly int x = Shader.PropertyToID("x");

		private static readonly int y = Shader.PropertyToID("y");

		private static readonly int angleY = Shader.PropertyToID("angleY");

		private static readonly int Stretch = Shader.PropertyToID("Stretch");

		private static readonly int Speed = Shader.PropertyToID("Speed");

		private static readonly int mR = Shader.PropertyToID("mR");

		private static readonly int mG = Shader.PropertyToID("mG");

		private static readonly int mB = Shader.PropertyToID("mB");

		private static readonly int Fade = Shader.PropertyToID("Fade");

		private static readonly int m_T = Shader.PropertyToID("T");

		private static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");

		private static readonly int _Mask = Shader.PropertyToID("_Mask");

		private static readonly int TempTargetId = Shader.PropertyToID("Glitch1");

		private LimitlessGlitch1 glitch1;

		private Material Glitch1Material;

		private RenderTargetIdentifier currentTarget;

		private float T;

		public Glitch1Pass(RenderPassEvent evt)
		{
			base.renderPassEvent = evt;
			Shader shader = Shader.Find("Hidden/Shader/Glitch1");
			if (shader == null)
			{
				Debug.LogError("Shader not found.");
			}
			else
			{
				Glitch1Material = CoreUtils.CreateEngineMaterial(shader);
			}
		}

		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			ScriptableRenderer renderer = renderingData.cameraData.renderer;
			currentTarget = renderer.cameraColorTargetHandle;
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (Glitch1Material == null)
			{
				Debug.LogError("Material not created.");
				return;
			}
			VolumeStack stack = VolumeManager.instance.stack;
			glitch1 = stack.GetComponent<LimitlessGlitch1>();
			if ((renderingData.cameraData.postProcessEnabled || !glitch1.GlobalPostProcessingSettings.value) && !(glitch1 == null) && glitch1.IsActive())
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
			T += Time.deltaTime;
			Glitch1Material.SetFloat(Strength, glitch1.amount.value);
			Glitch1Material.SetFloat(x, glitch1.x.value);
			Glitch1Material.SetFloat(y, glitch1.y.value);
			Glitch1Material.SetFloat(angleY, glitch1.z.value);
			Glitch1Material.SetFloat(Stretch, glitch1.stretch.value);
			Glitch1Material.SetFloat(Speed, glitch1.speed.value);
			Glitch1Material.SetFloat(mR, glitch1.rMultiplier.value);
			Glitch1Material.SetFloat(mG, glitch1.gMultiplier.value);
			Glitch1Material.SetFloat(mB, glitch1.bMultiplier.value);
			if (glitch1.mask.value != null)
			{
				Glitch1Material.SetTexture(_Mask, glitch1.mask.value);
				Glitch1Material.SetFloat(_FadeMultiplier, 1f);
				ParamSwitch(Glitch1Material, glitch1.maskChannel.value == maskChannelMode.alphaChannel, "ALPHA_CHANNEL");
			}
			else
			{
				Glitch1Material.SetFloat(_FadeMultiplier, 0f);
			}
			Glitch1Material.SetFloat(Fade, glitch1.fade.value);
			Glitch1Material.SetFloat(m_T, T);
			cmd.SetGlobalTexture(MainTexId, renderTargetIdentifier);
			cmd.GetTemporaryRT(tempTargetId, Screen.width, Screen.height, 0, FilterMode.Point, RenderTextureFormat.Default);
			cmd.Blit(renderTargetIdentifier, tempTargetId);
			cmd.Blit(tempTargetId, renderTargetIdentifier, Glitch1Material, pass);
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

	private Glitch1Pass GlitchPass;

	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;

	public override void Create()
	{
		GlitchPass = new Glitch1Pass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(GlitchPass);
	}
}
