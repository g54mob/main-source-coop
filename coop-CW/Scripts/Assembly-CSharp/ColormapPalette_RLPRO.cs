using RetroLookPro.Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColormapPalette_RLPRO : ScriptableRendererFeature
{
	public class ColormapPalette_RLPROPass : ScriptableRenderPass
	{
		private static readonly string k_RenderTag = "Renderr Glitch1 Effect";

		private static readonly int MainTexId = Shader.PropertyToID("_MainTex");

		private static readonly int heightV = Shader.PropertyToID("height");

		private static readonly int widthV = Shader.PropertyToID("width");

		private static readonly int _DitherV = Shader.PropertyToID("_Dither");

		private static readonly int _OpacityV = Shader.PropertyToID("_Opacity");

		private static readonly int _BlueNoiseV = Shader.PropertyToID("_BlueNoise");

		private static readonly int _PaletteV = Shader.PropertyToID("_Palette");

		private static readonly int _ColormapV = Shader.PropertyToID("_Colormap");

		private static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");

		private static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");

		private static readonly int _Mask = Shader.PropertyToID("_Mask");

		private ColormapPalette retroEffect;

		private Material RetroEffectMaterial;

		private RenderTargetIdentifier currentTarget;

		public int tempPresetIndex;

		private bool m_Init;

		private Texture2D colormapPalette;

		private Texture3D colormapTexture;

		private Vector2 m_Res;

		private int m_TempPixelSize;

		public ColormapPalette_RLPROPass(RenderPassEvent evt)
		{
			base.renderPassEvent = evt;
			Shader shader = Shader.Find("Hidden/Shader/ColormapPaletteEffect_RLPRO");
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
			retroEffect = stack.GetComponent<ColormapPalette>();
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
			ref CameraData cameraData = ref renderingData.cameraData;
			RenderTargetIdentifier renderTargetIdentifier = currentTarget;
			int tempTargetId = TempTargetId;
			int pass = 0;
			ApplyMaterialVariables(RetroEffectMaterial, out m_Res);
			if (m_Init || intHasChanged(tempPresetIndex, retroEffect.presetIndex.value) || m_TempPixelSize != retroEffect.pixelSize.value)
			{
				tempPresetIndex = retroEffect.presetIndex.value;
				ApplyColormapToMaterial(RetroEffectMaterial);
				m_Init = false;
				m_TempPixelSize = retroEffect.pixelSize.value;
			}
			float num = (float)cameraData.camera.scaledPixelWidth / (float)cameraData.camera.scaledPixelHeight;
			int scaledPixelWidth = cameraData.camera.scaledPixelWidth;
			int scaledPixelHeight = cameraData.camera.scaledPixelHeight;
			RetroEffectMaterial.SetInt(heightV, retroEffect.pixelSize.value);
			RetroEffectMaterial.SetInt(widthV, Mathf.RoundToInt((float)retroEffect.pixelSize.value * num));
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
			cmd.GetTemporaryRT(tempTargetId, scaledPixelWidth, scaledPixelHeight, 0, FilterMode.Point, RenderTextureFormat.Default);
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

		public void ApplyMaterialVariables(Material bl, out Vector2 res)
		{
			res.x = Screen.width / retroEffect.pixelSize.value;
			res.y = Screen.height / retroEffect.pixelSize.value;
			retroEffect.opacity.value = Mathf.Clamp01(retroEffect.opacity.value);
			retroEffect.dither.value = Mathf.Clamp01(retroEffect.dither.value);
			bl.SetFloat(_DitherV, retroEffect.dither.value);
			bl.SetFloat(_OpacityV, retroEffect.opacity.value);
		}

		public void ApplyColormapToMaterial(Material bl)
		{
			if (retroEffect.presetsList.value != null)
			{
				if (retroEffect.bluenoise.value != null)
				{
					bl.SetTexture(_BlueNoiseV, retroEffect.bluenoise.value);
				}
				ApplyPalette(bl);
				ApplyMap(bl);
			}
		}

		private void ApplyPalette(Material bl)
		{
			colormapPalette = new Texture2D(256, 1, TextureFormat.RGB24, mipChain: false);
			colormapPalette.filterMode = FilterMode.Point;
			colormapPalette.wrapMode = TextureWrapMode.Clamp;
			for (int i = 0; i < retroEffect.presetsList.value.presetsList[retroEffect.presetIndex.value].preset.numberOfColors; i++)
			{
				colormapPalette.SetPixel(i, 0, retroEffect.presetsList.value.presetsList[retroEffect.presetIndex.value].preset.palette[i]);
			}
			colormapPalette.Apply();
			bl.SetTexture(_PaletteV, colormapPalette);
		}

		public void ApplyMap(Material bl)
		{
			int num = 64;
			colormapTexture = new Texture3D(num, num, num, TextureFormat.RGB24, mipChain: false)
			{
				filterMode = FilterMode.Point,
				wrapMode = TextureWrapMode.Clamp
			};
			colormapTexture.SetPixels32(retroEffect.presetsList.value.presetsList[retroEffect.presetIndex.value].preset.pixels);
			colormapTexture.Apply();
			bl.SetTexture(_ColormapV, colormapTexture);
		}

		public bool intHasChanged(int A, int B)
		{
			bool result = false;
			if (B != A)
			{
				A = B;
				result = true;
			}
			return result;
		}
	}

	private ColormapPalette_RLPROPass RetroPass;

	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;

	public override void Create()
	{
		RetroPass = new ColormapPalette_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(RetroPass);
	}
}
