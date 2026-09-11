using RetroLookPro.Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Glitch2 : ScriptableRendererFeature
{
	public class Glitch2Pass : ScriptableRenderPass
	{
		private static readonly string k_RenderTag = "Render Glitch2 Effect";

		private static readonly int MainTexId = Shader.PropertyToID("_MainTex");

		private static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");

		private static readonly int _Mask = Shader.PropertyToID("_Mask");

		private static readonly int TempTargetId = Shader.PropertyToID("Glitch2");

		private static readonly int _trashFrame1Id = Shader.PropertyToID("_trashFrame1");

		private static readonly int _trashFrame2Id = Shader.PropertyToID("_trashFrame2");

		private static readonly int _trashFrameId = Shader.PropertyToID("_trashFrame");

		private bool done;

		private LimitlessGlitch2 Glitch2;

		private Material Glitch2Material;

		private RenderTargetIdentifier currentTarget;

		private Texture2D _noiseTexture;

		public Glitch2Pass(RenderPassEvent evt)
		{
			base.renderPassEvent = evt;
			Shader shader = Shader.Find("Hidden/Shader/Glitch2");
			if (shader == null)
			{
				Debug.LogError("Shader not found.");
			}
			else
			{
				Glitch2Material = CoreUtils.CreateEngineMaterial(shader);
			}
		}

		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			ScriptableRenderer renderer = renderingData.cameraData.renderer;
			currentTarget = renderer.cameraColorTargetHandle;
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (Glitch2Material == null)
			{
				Debug.LogError("Material not created.");
				return;
			}
			VolumeStack stack = VolumeManager.instance.stack;
			Glitch2 = stack.GetComponent<LimitlessGlitch2>();
			if ((renderingData.cameraData.postProcessEnabled || !Glitch2.GlobalPostProcessingSettings.value) && !(Glitch2 == null) && Glitch2.IsActive())
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
			int trashFrame1Id = _trashFrame1Id;
			int trashFrame2Id = _trashFrame2Id;
			int trashFrameId = _trashFrameId;
			int pass = 0;
			cmd.GetTemporaryRT(trashFrame1Id, Screen.width, Screen.height, 0, FilterMode.Point, RenderTextureFormat.Default);
			cmd.GetTemporaryRT(trashFrame2Id, Screen.width, Screen.height, 0, FilterMode.Point, RenderTextureFormat.Default);
			if (!done)
			{
				SetUpResources(Glitch2.resolutionMultiplier.value);
			}
			if (Random.value > Mathf.Lerp(0.9f, 0.5f, Glitch2.speed.value))
			{
				SetUpResources(Glitch2.resolutionMultiplier.value);
				UpdateNoiseTexture(Glitch2.resolutionMultiplier.value);
			}
			int frameCount = Time.frameCount;
			if (frameCount % 13 == 0)
			{
				cmd.Blit(renderTargetIdentifier, trashFrame1Id);
			}
			if (frameCount % 73 == 0)
			{
				cmd.Blit(renderTargetIdentifier, trashFrame2Id);
			}
			trashFrameId = ((Random.value > 0.5f) ? trashFrame1Id : trashFrame2Id);
			Glitch2Material.SetFloat("_ColorIntensity", Glitch2.intensity.value);
			if (_noiseTexture == null)
			{
				UpdateNoiseTexture(Glitch2.resolutionMultiplier.value);
			}
			if (Glitch2.mask.value != null)
			{
				Glitch2Material.SetTexture(_Mask, Glitch2.mask.value);
				Glitch2Material.SetFloat(_FadeMultiplier, 1f);
				ParamSwitch(Glitch2Material, Glitch2.maskChannel.value == maskChannelMode.alphaChannel, "ALPHA_CHANNEL");
			}
			else
			{
				Glitch2Material.SetFloat(_FadeMultiplier, 0f);
			}
			Glitch2Material.SetTexture("_NoiseTex", _noiseTexture);
			cmd.SetGlobalTexture("_TrashTex", trashFrameId);
			cmd.SetGlobalTexture(MainTexId, renderTargetIdentifier);
			cmd.GetTemporaryRT(tempTargetId, Screen.width, Screen.height, 0, FilterMode.Point, RenderTextureFormat.Default);
			cmd.Blit(renderTargetIdentifier, tempTargetId);
			cmd.Blit(tempTargetId, renderTargetIdentifier, Glitch2Material, pass);
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

		private void SetUpResources(float g_2Res)
		{
			if (!done)
			{
				Vector2Int vector2Int = new Vector2Int((int)(g_2Res * 64f), (int)(g_2Res * 62f));
				_noiseTexture = new Texture2D(vector2Int.x, vector2Int.y, TextureFormat.ARGB32, mipChain: false)
				{
					hideFlags = HideFlags.DontSave,
					wrapMode = TextureWrapMode.Clamp,
					filterMode = FilterMode.Point
				};
				UpdateNoiseTexture(g_2Res);
				done = true;
			}
		}

		private void UpdateNoiseTexture(float g_2Res)
		{
			Color color = RandomColor();
			if (_noiseTexture == null)
			{
				Vector2Int vector2Int = new Vector2Int((int)(g_2Res * 64f), (int)(g_2Res * 32f));
				_noiseTexture = new Texture2D(vector2Int.x, vector2Int.y, TextureFormat.ARGB32, mipChain: false);
			}
			for (int i = 0; i < _noiseTexture.height; i++)
			{
				for (int j = 0; j < _noiseTexture.width; j++)
				{
					if (Random.value > Glitch2.stretchMultiplier.value)
					{
						color = RandomColor();
					}
					_noiseTexture.SetPixel(j, i, color);
				}
			}
			_noiseTexture.Apply();
		}

		private static Color RandomColor()
		{
			return new Color(Random.value, Random.value, Random.value, Random.value);
		}
	}

	private Glitch2Pass GlitchPass;

	public RenderPassEvent Event = RenderPassEvent.AfterRenderingOpaques;

	public override void Create()
	{
		GlitchPass = new Glitch2Pass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(GlitchPass);
	}
}
