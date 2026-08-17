using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace ShadedTechnology.WindshieldRainAsset
{
	public class WindshieldBlurHdrpPass : CustomPass
	{
		public int iterations = 3;

		public float blurStrength = 2f;

		private Material blurMaterial;

		private int _iterations;

		private RTHandle tempRT_Grab;

		private RTHandle[] tempRTsH;

		private RTHandle[] tempRTsV;

		private void AllocTextures()
		{
			float num = 2f;
			tempRT_Grab = RTHandles.Alloc(Vector2.one, TextureXR.slices, DepthBits.None, GraphicsFormat.R16G16B16A16_SFloat, FilterMode.Bilinear, TextureWrapMode.Repeat, TextureXR.dimension, enableRandomWrite: false, useMipMap: false, autoGenerateMips: true, isShadowMap: false, 1, 0f, MSAASamples.None, bindTextureMS: false, useDynamicScale: false, useDynamicScaleExplicit: false, RenderTextureMemoryless.None, VRTextureUsage.None, "WindshieldBlur_Grab");
			for (int i = 0; i < tempRTsH.Length; i++)
			{
				tempRTsH[i] = RTHandles.Alloc(Vector2.one / num, TextureXR.slices, DepthBits.None, GraphicsFormat.R16G16B16A16_SFloat, FilterMode.Bilinear, TextureWrapMode.Repeat, TextureXR.dimension, enableRandomWrite: false, useMipMap: false, autoGenerateMips: true, isShadowMap: false, 1, 0f, MSAASamples.None, bindTextureMS: false, useDynamicScale: false, useDynamicScaleExplicit: false, RenderTextureMemoryless.None, VRTextureUsage.None, $"WindshieldBlur_H_{i}");
				tempRTsV[i] = RTHandles.Alloc(Vector2.one / num, TextureXR.slices, DepthBits.None, GraphicsFormat.R16G16B16A16_SFloat, FilterMode.Bilinear, TextureWrapMode.Repeat, TextureXR.dimension, enableRandomWrite: false, useMipMap: false, autoGenerateMips: true, isShadowMap: false, 1, 0f, MSAASamples.None, bindTextureMS: false, useDynamicScale: false, useDynamicScaleExplicit: false, RenderTextureMemoryless.None, VRTextureUsage.None, $"WindshieldBlur_V_{i}");
				num *= 2f;
			}
		}

		private void DeallocTextures()
		{
			if (tempRT_Grab != null)
			{
				RTHandles.Release(tempRT_Grab);
			}
			for (int i = 0; i < tempRTsH.Length; i++)
			{
				if (tempRTsH != null && tempRTsH[i] != null)
				{
					RTHandles.Release(tempRTsH[i]);
				}
				if (tempRTsV != null && tempRTsV[i] != null)
				{
					RTHandles.Release(tempRTsV[i]);
				}
			}
		}

		protected override void Setup(ScriptableRenderContext ctx, CommandBuffer cmd)
		{
			iterations = Mathf.Clamp(iterations, 1, 7);
			_iterations = iterations;
			blurMaterial = new Material(Shader.Find("Hidden/SeparableGlassBlurHDRP"));
			tempRTsH = new RTHandle[_iterations];
			tempRTsV = new RTHandle[_iterations];
			AllocTextures();
		}

		protected override void Execute(CustomPassContext ctx)
		{
			if (!(blurMaterial == null))
			{
				RTHandle cameraColorBuffer = ctx.cameraColorBuffer;
				HDUtils.BlitCameraTexture(ctx.cmd, cameraColorBuffer, tempRT_Grab);
				cameraColorBuffer = tempRT_Grab;
				ctx.cmd.SetGlobalTexture("_WindshieldGrabTexture", tempRT_Grab);
				for (int i = 0; i < _iterations; i++)
				{
					ctx.cmd.SetGlobalVector("_WindshieldRain_Blur_Offsets", new Vector4(blurStrength / (float)tempRTsH[i].rt.width, 0f, 0f, 0f));
					RTHandle destination = tempRTsH[i];
					HDUtils.BlitCameraTexture(ctx.cmd, cameraColorBuffer, destination, blurMaterial, 0);
					ctx.cmd.SetGlobalVector("_WindshieldRain_Blur_Offsets", new Vector4(0f, blurStrength / (float)tempRTsV[i].rt.height, 0f, 0f));
					destination = tempRTsV[i];
					HDUtils.BlitCameraTexture(ctx.cmd, tempRTsH[i], destination, blurMaterial, 0);
					ctx.cmd.SetGlobalTexture($"_GrabBlurTexture_{i}", tempRTsV[i]);
					cameraColorBuffer = tempRTsV[i];
				}
			}
		}

		protected override void Cleanup()
		{
			DeallocTextures();
		}
	}
}
