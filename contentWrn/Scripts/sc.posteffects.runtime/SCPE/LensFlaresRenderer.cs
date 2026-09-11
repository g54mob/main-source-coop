using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SCPE
{
	public class LensFlaresRenderer : ScriptableRendererFeature
	{
		private class LensFlaresRenderPass : PostEffectRenderer<LensFlares>
		{
			private enum Pass
			{
				LuminanceDiff = 0,
				Ghosting = 1,
				Blur = 2,
				Blend = 3,
				Debug = 4
			}

			private int flaresTexID;

			private int emissionTexID;

			private RTHandle emissionTex;

			private RTHandle flaresTex;

			private RTHandle blurBuffer1;

			private RTHandle blurBuffer2;

			public LensFlaresRenderPass(EffectBaseSettings settings)
			{
				base.settings = settings;
				base.renderPassEvent = settings.GetInjectionPoint();
				shaderName = "Hidden/SC Post Effects/Lensflares";
				ProfilerTag = GetProfilerTag();
				emissionTexID = Shader.PropertyToID("_BloomTex");
				flaresTexID = Shader.PropertyToID("_FlaresTex");
			}

			public override void Setup(ScriptableRenderer renderer, RenderingData renderingData)
			{
				volumeSettings = VolumeManager.instance.stack.GetComponent<LensFlares>();
				base.Setup(renderer, renderingData);
				if (render && volumeSettings.IsActive())
				{
					cameraColorTarget = GetCameraTarget(renderer);
					renderer.EnqueuePass(this);
				}
			}

			protected override void ConfigurePass(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				base.ConfigurePass(cmd, cameraTextureDescriptor);
				emissionTex = PostEffectRenderer<LensFlares>.GetTemporaryRT(ref emissionTex, cameraTextureDescriptor, cameraTextureDescriptor.graphicsFormat, FilterMode.Bilinear, "emissionTex", 2);
				cmd.SetGlobalTexture(emissionTexID, emissionTex);
				flaresTex = PostEffectRenderer<LensFlares>.GetTemporaryRT(ref flaresTex, cameraTextureDescriptor, cameraTextureDescriptor.graphicsFormat, FilterMode.Bilinear, "flaresTex", 2);
				cmd.SetGlobalTexture(flaresTexID, flaresTex);
				blurBuffer1 = PostEffectRenderer<LensFlares>.GetTemporaryRT(ref blurBuffer1, cameraTextureDescriptor, cameraTextureDescriptor.graphicsFormat, FilterMode.Bilinear, "LensFlareBlurBuffer1", 2);
				blurBuffer2 = PostEffectRenderer<LensFlares>.GetTemporaryRT(ref blurBuffer2, cameraTextureDescriptor, cameraTextureDescriptor.graphicsFormat, FilterMode.Bilinear, "LensFlareBlurBuffer2", 2);
			}

			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				CommandBuffer commandBuffer = GetCommandBuffer(ref renderingData);
				CopyTargets(commandBuffer, renderingData);
				Material.SetFloat("_Intensity", volumeSettings.intensity.value);
				float value = Mathf.GammaToLinearSpace(volumeSettings.luminanceThreshold.value);
				Material.SetFloat("_Threshold", value);
				Material.SetFloat("_Distance", volumeSettings.distance.value);
				Material.SetFloat("_Falloff", volumeSettings.falloff.value);
				Material.SetFloat("_Ghosts", volumeSettings.iterations.value);
				Material.SetFloat("_HaloSize", volumeSettings.haloSize.value);
				Material.SetFloat("_HaloWidth", volumeSettings.haloWidth.value);
				Material.SetFloat("_ChromaticAbberation", volumeSettings.chromaticAbberation.value);
				Material.SetTexture("_ColorTex", volumeSettings.colorTex.value ? volumeSettings.colorTex.value : Texture2D.whiteTexture);
				Material.SetTexture("_MaskTex", volumeSettings.maskTex.value ? volumeSettings.maskTex.value : Texture2D.whiteTexture);
				Blit(this, commandBuffer, cameraColorTarget, emissionTex, Material, 0);
				Blit(this, commandBuffer, emissionTex, flaresTex, Material, 1);
				BlitCopy(commandBuffer, flaresTex, blurBuffer1);
				for (int i = 0; i < volumeSettings.passes.value; i++)
				{
					commandBuffer.SetGlobalVector(ShaderParameters.BlurOffsets, new Vector4(volumeSettings.blur.value / (float)renderingData.cameraData.camera.scaledPixelWidth, 0f, 0f, 0f));
					Blit(this, commandBuffer, blurBuffer1, blurBuffer2, Material, 2);
					commandBuffer.SetGlobalVector(ShaderParameters.BlurOffsets, new Vector4(0f, volumeSettings.blur.value / (float)renderingData.cameraData.camera.scaledPixelHeight, 0f, 0f));
					Blit(this, commandBuffer, blurBuffer2, blurBuffer1, Material, 2);
				}
				commandBuffer.SetGlobalTexture(flaresTexID, blurBuffer1);
				FinalBlit(this, context, commandBuffer, renderingData, volumeSettings.debug.value ? 4 : 3);
			}

			public override void OnCameraCleanup(CommandBuffer cmd)
			{
				base.OnCameraCleanup(cmd);
				if (ShouldReleaseRT())
				{
					PostEffectRenderer<LensFlares>.ReleaseRT(emissionTex);
					PostEffectRenderer<LensFlares>.ReleaseRT(flaresTex);
					PostEffectRenderer<LensFlares>.ReleaseRT(blurBuffer1);
					PostEffectRenderer<LensFlares>.ReleaseRT(blurBuffer2);
				}
			}
		}

		private LensFlaresRenderPass m_ScriptablePass;

		[SerializeField]
		public EffectBaseSettings settings = new EffectBaseSettings();

		public override void Create()
		{
			m_ScriptablePass = new LensFlaresRenderPass(settings);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			m_ScriptablePass.Setup(renderer, renderingData);
		}
	}
}
