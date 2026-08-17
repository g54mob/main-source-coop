using UnityEngine;
using UnityEngine.Rendering;

namespace Enviro
{
	[ExecuteInEditMode]
	[ImageEffectAllowedInSceneView]
	public class EnviroRenderer : MonoBehaviour
	{
		[Tooltip("Assign a quality here if you want to use different settings for this camera. Otherwise it takes settings from Enviro Manager.")]
		private EnviroQuality myQuality;

		private Camera myCam;

		private EnviroVolumetricCloudRenderer volumetricCloudsRender;

		private Vector3 floatingPointOriginMod = Vector3.zero;

		private void OnEnable()
		{
			myCam = GetComponent<Camera>();
			base.enabled = false;
		}

		private void OnDisable()
		{
			CleanupVolumetricRenderer();
		}

		private void CleanupVolumetricRenderer()
		{
			if (volumetricCloudsRender == null)
			{
				return;
			}
			if (volumetricCloudsRender.raymarchMat != null)
			{
				Object.DestroyImmediate(volumetricCloudsRender.raymarchMat);
			}
			if (volumetricCloudsRender.blendAndLightingMat != null)
			{
				Object.DestroyImmediate(volumetricCloudsRender.blendAndLightingMat);
			}
			if (volumetricCloudsRender.reprojectMat != null)
			{
				Object.DestroyImmediate(volumetricCloudsRender.reprojectMat);
			}
			if (volumetricCloudsRender.undersampleBuffer != null)
			{
				Object.DestroyImmediate(volumetricCloudsRender.undersampleBuffer);
			}
			if (volumetricCloudsRender.fullBuffer == null || volumetricCloudsRender.fullBuffer.Length == 0)
			{
				return;
			}
			for (int i = 0; i < volumetricCloudsRender.fullBuffer.Length; i++)
			{
				if (volumetricCloudsRender.fullBuffer[i] != null)
				{
					Object.DestroyImmediate(volumetricCloudsRender.fullBuffer[i]);
				}
			}
		}

		private void SetMatrix()
		{
			if (myCam.stereoEnabled)
			{
				Matrix4x4 inverse = myCam.GetStereoViewMatrix(Camera.StereoscopicEye.Left).inverse;
				Matrix4x4 inverse2 = myCam.GetStereoViewMatrix(Camera.StereoscopicEye.Right).inverse;
				Matrix4x4 stereoProjectionMatrix = myCam.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
				Matrix4x4 stereoProjectionMatrix2 = myCam.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right);
				Matrix4x4 inverse3 = GL.GetGPUProjectionMatrix(stereoProjectionMatrix, renderIntoTexture: true).inverse;
				Matrix4x4 inverse4 = GL.GetGPUProjectionMatrix(stereoProjectionMatrix2, renderIntoTexture: true).inverse;
				if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLCore && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES3)
				{
					inverse3[1, 1] *= -1f;
					inverse4[1, 1] *= -1f;
				}
				Shader.SetGlobalMatrix("_LeftWorldFromView", inverse);
				Shader.SetGlobalMatrix("_RightWorldFromView", inverse2);
				Shader.SetGlobalMatrix("_LeftViewFromScreen", inverse3);
				Shader.SetGlobalMatrix("_RightViewFromScreen", inverse4);
			}
			else
			{
				Matrix4x4 cameraToWorldMatrix = myCam.cameraToWorldMatrix;
				Matrix4x4 inverse5 = GL.GetGPUProjectionMatrix(myCam.projectionMatrix, renderIntoTexture: true).inverse;
				if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLCore && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES3)
				{
					inverse5[1, 1] *= -1f;
				}
				Shader.SetGlobalMatrix("_LeftWorldFromView", cameraToWorldMatrix);
				Shader.SetGlobalMatrix("_LeftViewFromScreen", inverse5);
			}
		}

		private void Update()
		{
		}

		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture src, RenderTexture dest)
		{
			if (EnviroManager.instance == null || !EnviroManager.instance.gameObject.activeSelf)
			{
				Graphics.Blit(src, dest);
				return;
			}
			if (myCam == null)
			{
				myCam = GetComponent<Camera>();
			}
			if (myCam.actualRenderingPath == RenderingPath.Forward)
			{
				myCam.depthTextureMode |= DepthTextureMode.Depth;
			}
			if (EnviroHelper.ResetMatrix(myCam))
			{
				myCam.ResetProjectionMatrix();
			}
			myQuality = EnviroHelper.GetQualityForCamera(myCam);
			bool flag = false;
			bool flag2 = false;
			if (EnviroManager.instance.Quality != null)
			{
				if (EnviroManager.instance.VolumetricClouds != null)
				{
					flag = myQuality.volumetricCloudsOverride.volumetricClouds;
				}
				if (EnviroManager.instance.Fog != null)
				{
					flag2 = myQuality.fogOverride.fog;
				}
			}
			else
			{
				if (EnviroManager.instance.VolumetricClouds != null)
				{
					flag = EnviroManager.instance.VolumetricClouds.settingsQuality.volumetricClouds;
				}
				if (EnviroManager.instance.Fog != null)
				{
					flag2 = EnviroManager.instance.Fog.Settings.fog;
				}
			}
			if (EnviroManager.instance.Objects.worldAnchor != null)
			{
				floatingPointOriginMod = EnviroManager.instance.Objects.worldAnchor.transform.position;
			}
			else
			{
				floatingPointOriginMod = Vector3.zero;
			}
			SetMatrix();
			if (volumetricCloudsRender == null)
			{
				volumetricCloudsRender = new EnviroVolumetricCloudRenderer();
			}
			volumetricCloudsRender.camera = myCam;
			if (EnviroManager.instance.Fog != null && flag2)
			{
				EnviroManager.instance.Fog.RenderVolumetrics(myCam, src);
			}
			if (!flag)
			{
				Shader.SetGlobalTexture("_EnviroClouds", Texture2D.blackTexture);
			}
			if (EnviroManager.instance.Fog != null && EnviroManager.instance.VolumetricClouds != null && flag && flag2)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(src.descriptor);
				RenderTexture temporary2 = RenderTexture.GetTemporary(src.descriptor);
				if (myCam.transform.position.y - floatingPointOriginMod.y < EnviroManager.instance.VolumetricClouds.settingsVolume.bottomCloudsHeight)
				{
					EnviroManager.instance.VolumetricClouds.RenderVolumetricClouds(myCam, src, temporary, volumetricCloudsRender, myQuality);
					if (EnviroManager.instance.VolumetricClouds.settingsGlobal.cloudShadows && myCam.cameraType != CameraType.Reflection)
					{
						EnviroManager.instance.VolumetricClouds.RenderCloudsShadows(temporary, temporary2, volumetricCloudsRender);
						EnviroManager.instance.Fog.RenderHeightFog(myCam, temporary2, dest);
					}
					else
					{
						EnviroManager.instance.Fog.RenderHeightFog(myCam, temporary, dest);
					}
				}
				else
				{
					EnviroManager.instance.Fog.RenderHeightFog(myCam, src, temporary);
					if (EnviroManager.instance.VolumetricClouds.settingsGlobal.cloudShadows && myCam.cameraType != CameraType.Reflection)
					{
						EnviroManager.instance.VolumetricClouds.RenderCloudsShadows(temporary, temporary2, volumetricCloudsRender);
						EnviroManager.instance.VolumetricClouds.RenderVolumetricClouds(myCam, temporary2, dest, volumetricCloudsRender, myQuality);
					}
					else
					{
						EnviroManager.instance.VolumetricClouds.RenderVolumetricClouds(myCam, temporary, dest, volumetricCloudsRender, myQuality);
					}
				}
				RenderTexture.ReleaseTemporary(temporary);
				RenderTexture.ReleaseTemporary(temporary2);
			}
			else if (EnviroManager.instance.VolumetricClouds != null && flag && !flag2)
			{
				if (EnviroManager.instance.VolumetricClouds.settingsGlobal.cloudShadows && myCam.cameraType != CameraType.Reflection)
				{
					RenderTexture temporary3 = RenderTexture.GetTemporary(src.descriptor);
					EnviroManager.instance.VolumetricClouds.RenderCloudsShadows(src, temporary3, volumetricCloudsRender);
					EnviroManager.instance.VolumetricClouds.RenderVolumetricClouds(myCam, temporary3, dest, volumetricCloudsRender, myQuality);
					RenderTexture.ReleaseTemporary(temporary3);
				}
				else
				{
					EnviroManager.instance.VolumetricClouds.RenderVolumetricClouds(myCam, src, dest, volumetricCloudsRender, myQuality);
				}
			}
			else if (EnviroManager.instance.Fog != null && flag2)
			{
				EnviroManager.instance.Fog.RenderHeightFog(myCam, src, dest);
			}
			else
			{
				Graphics.Blit(src, dest);
			}
			if (!flag)
			{
				Shader.SetGlobalTexture("_EnviroClouds", Texture2D.blackTexture);
			}
		}
	}
}
