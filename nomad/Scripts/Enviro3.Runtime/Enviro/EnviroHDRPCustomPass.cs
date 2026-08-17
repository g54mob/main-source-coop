using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Enviro
{
	internal class EnviroHDRPCustomPass : CustomPass
	{
		private Material blitTrough;

		private List<EnviroVolumetricCloudRenderer> volumetricCloudsRender = new List<EnviroVolumetricCloudRenderer>();

		private Vector3 floatingPointOriginMod = Vector3.zero;

		private RTHandle sourceHandle;

		private RTHandle temp1Handle;

		private RTHandle temp2Handle;

		protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
		{
			if (blitTrough == null)
			{
				blitTrough = new Material(Shader.Find("Hidden/Enviro/BlitTroughHDRP"));
			}
			sourceHandle = null;
			temp1Handle = null;
			temp2Handle = null;
		}

		public RTHandle ReallocateIfNeeded(RTHandle handle, RenderTextureDescriptor desc, string name)
		{
			bool flag = handle == null || handle.rt == null;
			if (!flag)
			{
				RenderTextureDescriptor descriptor = handle.rt.descriptor;
				if (descriptor.graphicsFormat != desc.graphicsFormat || descriptor.dimension != desc.dimension)
				{
					flag = true;
				}
			}
			if (flag)
			{
				if (handle != null)
				{
					RTHandles.Release(handle);
				}
				handle = RTHandles.Alloc(Vector2.one, 1, DepthBits.None, desc.graphicsFormat, FilterMode.Point, TextureWrapMode.Repeat, desc.dimension, enableRandomWrite: false, desc.useMipMap, autoGenerateMips: true, isShadowMap: false, 1, 0f, MSAASamples.None, bindTextureMS: false, useDynamicScale: true, useDynamicScaleExplicit: false, RenderTextureMemoryless.None, VRTextureUsage.None, name);
			}
			return handle;
		}

		protected override void Execute(CustomPassContext ctx)
		{
			HDCamera hdCamera = ctx.hdCamera;
			if (ctx.cameraColorBuffer == null || ctx.cameraColorBuffer.rt == null || hdCamera.camera.cameraType == CameraType.Preview || !EnviroHelper.CanRenderOnCamera(hdCamera.camera) || (EnviroManager.instance == null && EnviroManager.instance.configuration != null))
			{
				return;
			}
			RenderTextureDescriptor descriptor = ctx.cameraColorBuffer.rt.descriptor;
			sourceHandle = ReallocateIfNeeded(sourceHandle, descriptor, "Enviro Source");
			temp1Handle = ReallocateIfNeeded(temp1Handle, descriptor, "Enviro Temp1");
			if (EnviroManager.instance.VolumetricClouds != null && EnviroManager.instance.VolumetricClouds.settingsGlobal.cloudShadows)
			{
				temp2Handle = ReallocateIfNeeded(temp2Handle, descriptor, "Enviro Temp2");
			}
			HDUtils.BlitCameraTexture(ctx.cmd, ctx.cameraColorBuffer, sourceHandle);
			EnviroQuality qualityForCamera = EnviroHelper.GetQualityForCamera(hdCamera.camera);
			bool flag = false;
			bool flag2 = false;
			if (qualityForCamera == null)
			{
				flag = EnviroManager.instance.VolumetricClouds != null && EnviroManager.instance.VolumetricClouds.settingsQuality.volumetricClouds;
				flag2 = EnviroManager.instance.Fog != null && EnviroManager.instance.Fog.Settings.fog;
			}
			else
			{
				flag = EnviroManager.instance.VolumetricClouds != null && qualityForCamera.volumetricCloudsOverride.volumetricClouds;
				flag2 = EnviroManager.instance.Fog != null && qualityForCamera.fogOverride.fog;
			}
			floatingPointOriginMod = ((EnviroManager.instance.Objects?.worldAnchor != null) ? EnviroManager.instance.Objects.worldAnchor.transform.position : Vector3.zero);
			if (flag && GetCloudsRenderer(hdCamera.camera) == null)
			{
				CreateCloudsRenderer(hdCamera.camera);
			}
			SetMatrix(hdCamera.camera);
			EnviroVolumetricCloudRenderer cloudsRenderer = GetCloudsRenderer(hdCamera.camera);
			if (!flag)
			{
				Shader.SetGlobalTexture("_EnviroClouds", Texture2D.blackTexture);
			}
			if (flag && flag2)
			{
				if (hdCamera.camera.transform.position.y - floatingPointOriginMod.y < EnviroManager.instance.VolumetricClouds.settingsVolume.bottomCloudsHeight)
				{
					EnviroManager.instance.VolumetricClouds.RenderVolumetricCloudsHDRP(hdCamera.camera, ctx.cmd, sourceHandle, temp1Handle, cloudsRenderer, qualityForCamera);
					if (EnviroManager.instance.VolumetricClouds.settingsGlobal.cloudShadows && hdCamera.camera.cameraType != CameraType.Reflection)
					{
						EnviroManager.instance.VolumetricClouds.RenderCloudsShadowsHDRP(hdCamera.camera, ctx.cmd, temp1Handle, temp2Handle, cloudsRenderer);
						EnviroManager.instance.Fog.RenderHeightFogHDRP(hdCamera.camera, ctx.cmd, temp2Handle, ctx.cameraColorBuffer);
					}
					else
					{
						EnviroManager.instance.Fog.RenderHeightFogHDRP(hdCamera.camera, ctx.cmd, temp1Handle, ctx.cameraColorBuffer);
					}
				}
				else
				{
					EnviroManager.instance.Fog.RenderHeightFogHDRP(hdCamera.camera, ctx.cmd, sourceHandle, temp1Handle);
					if (EnviroManager.instance.VolumetricClouds.settingsGlobal.cloudShadows && hdCamera.camera.cameraType != CameraType.Reflection)
					{
						EnviroManager.instance.VolumetricClouds.RenderCloudsShadowsHDRP(hdCamera.camera, ctx.cmd, temp1Handle, temp2Handle, cloudsRenderer);
						EnviroManager.instance.VolumetricClouds.RenderVolumetricCloudsHDRP(hdCamera.camera, ctx.cmd, temp2Handle, ctx.cameraColorBuffer, cloudsRenderer, qualityForCamera);
					}
					else
					{
						EnviroManager.instance.VolumetricClouds.RenderVolumetricCloudsHDRP(hdCamera.camera, ctx.cmd, temp1Handle, ctx.cameraColorBuffer, cloudsRenderer, qualityForCamera);
					}
				}
			}
			else if (flag)
			{
				if (EnviroManager.instance.VolumetricClouds.settingsGlobal.cloudShadows && hdCamera.camera.cameraType != CameraType.Reflection)
				{
					EnviroManager.instance.VolumetricClouds.RenderCloudsShadowsHDRP(hdCamera.camera, ctx.cmd, sourceHandle, temp1Handle, cloudsRenderer);
					EnviroManager.instance.VolumetricClouds.RenderVolumetricCloudsHDRP(hdCamera.camera, ctx.cmd, temp1Handle, ctx.cameraColorBuffer, cloudsRenderer, qualityForCamera);
				}
				else
				{
					EnviroManager.instance.VolumetricClouds.RenderVolumetricCloudsHDRP(hdCamera.camera, ctx.cmd, sourceHandle, ctx.cameraColorBuffer, cloudsRenderer, qualityForCamera);
				}
			}
			else if (flag2)
			{
				EnviroManager.instance.Fog.RenderHeightFogHDRP(hdCamera.camera, ctx.cmd, sourceHandle, ctx.cameraColorBuffer);
			}
			if (!flag)
			{
				Shader.SetGlobalTexture("_EnviroClouds", Texture2D.blackTexture);
			}
		}

		protected override void Cleanup()
		{
			if (blitTrough != null)
			{
				CoreUtils.Destroy(blitTrough);
			}
			if (sourceHandle != null)
			{
				RTHandles.Release(sourceHandle);
			}
			if (temp1Handle != null)
			{
				RTHandles.Release(temp1Handle);
			}
			if (temp2Handle != null)
			{
				RTHandles.Release(temp2Handle);
			}
			for (int i = 0; i < volumetricCloudsRender.Count; i++)
			{
				CleanCloudsRenderer(volumetricCloudsRender[i]);
			}
		}

		private EnviroVolumetricCloudRenderer CreateCloudsRenderer(Camera cam)
		{
			EnviroVolumetricCloudRenderer enviroVolumetricCloudRenderer = new EnviroVolumetricCloudRenderer();
			enviroVolumetricCloudRenderer.camera = cam;
			volumetricCloudsRender.Add(enviroVolumetricCloudRenderer);
			return enviroVolumetricCloudRenderer;
		}

		private void CleanCloudsRenderer(EnviroVolumetricCloudRenderer renderer)
		{
			if (renderer.fullBuffer != null)
			{
				RenderTexture[] fullBuffer = renderer.fullBuffer;
				foreach (RenderTexture renderTexture in fullBuffer)
				{
					if (renderTexture != null)
					{
						CoreUtils.Destroy(renderTexture);
					}
				}
			}
			if (renderer.undersampleBuffer != null)
			{
				CoreUtils.Destroy(renderer.undersampleBuffer);
			}
			if (renderer.downsampledDepth != null)
			{
				CoreUtils.Destroy(renderer.downsampledDepth);
			}
			if (renderer.raymarchMat != null)
			{
				CoreUtils.Destroy(renderer.raymarchMat);
			}
			if (renderer.reprojectMat != null)
			{
				CoreUtils.Destroy(renderer.reprojectMat);
			}
			if (renderer.blendAndLightingMat != null)
			{
				CoreUtils.Destroy(renderer.blendAndLightingMat);
			}
			if (renderer.depthMat != null)
			{
				CoreUtils.Destroy(renderer.depthMat);
			}
			if (renderer.shadowMat != null)
			{
				CoreUtils.Destroy(renderer.shadowMat);
			}
		}

		private EnviroVolumetricCloudRenderer GetCloudsRenderer(Camera cam)
		{
			foreach (EnviroVolumetricCloudRenderer item in volumetricCloudsRender)
			{
				if (item.camera == cam)
				{
					return item;
				}
			}
			return CreateCloudsRenderer(cam);
		}

		private void SetMatrix(Camera myCam)
		{
			Matrix4x4 cameraToWorldMatrix = myCam.cameraToWorldMatrix;
			Matrix4x4 inverse = GL.GetGPUProjectionMatrix(myCam.projectionMatrix, renderIntoTexture: true).inverse;
			if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLCore && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES3)
			{
				inverse[1, 1] *= -1f;
			}
			Shader.SetGlobalMatrix("_LeftWorldFromView", cameraToWorldMatrix);
			Shader.SetGlobalMatrix("_LeftViewFromScreen", inverse);
		}
	}
}
