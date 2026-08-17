using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace Enviro
{
	[Serializable]
	[ExecuteInEditMode]
	public class EnviroVolumetricCloudsModule : EnviroModule
	{
		public EnviroCloudLayerSettings settingsVolume;

		public EnviroCloudGlobalSettings settingsGlobal;

		public EnviroVolumetricCloudsQuality settingsQuality;

		public EnviroVolumetricCloudsModule preset;

		public bool showGlobalControls;

		public bool showVolumeSettings;

		public bool showCoverageControls;

		public bool showLightingControls;

		public bool showDensityControls;

		public bool showTextureControls;

		public bool showWindControls;

		public Vector3 cloudAnimLayer1;

		public Vector3 cloudAnimLayer2;

		public Vector3 cloudAnimNonScaledLayer1;

		public Vector3 cloudAnimNonScaledLayer2;

		public RenderTexture weatherMap;

		private Material weatherMapMat;

		private ComputeShader weatherMapCS;

		private Light dirLight;

		private Vector3 lastOffset = Vector3.zero;

		private Texture2DArray blackArray;

		public override void UpdateModule()
		{
			if (active && !(EnviroManager.instance == null) && settingsQuality.volumetricClouds)
			{
				UpdateWind();
				weatherMap = EnviroManager.instance.VolumetricClouds.RenderWeatherMap();
			}
		}

		private void CreateBlackArray()
		{
			Color[] array = new Color[16];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Color(0f, 0f, 0f, 0f);
			}
			blackArray = new Texture2DArray(4, 4, 2, DefaultFormat.LDR, TextureCreationFlags.None);
			blackArray.SetPixels(array, 0);
			blackArray.SetPixels(array, 1);
			blackArray.Apply();
		}

		private bool IsURPCompabilityMode()
		{
			return true;
		}

		public override void Enable()
		{
			CreateBlackArray();
		}

		public override void Disable()
		{
			if (weatherMapMat != null)
			{
				UnityEngine.Object.DestroyImmediate(weatherMapMat);
			}
			if (weatherMap != null)
			{
				UnityEngine.Object.DestroyImmediate(weatherMap);
			}
		}

		public void RenderCloudsShadows(RenderTexture source, RenderTexture destination, EnviroVolumetricCloudRenderer renderer)
		{
			if (renderer.shadowMat == null)
			{
				renderer.shadowMat = new Material(Shader.Find("Hidden/EnviroApplyShadows"));
			}
			if (!(renderer.undersampleBuffer == null))
			{
				renderer.shadowMat.SetTexture("_CloudsTex", renderer.undersampleBuffer);
				renderer.shadowMat.SetFloat("_Intensity", EnviroManager.instance.VolumetricClouds.settingsGlobal.cloudShadowsIntensity);
				renderer.shadowMat.SetTexture("_MainTex", source);
				Graphics.Blit(source, destination, renderer.shadowMat);
			}
		}

		public void RenderCloudsShadowsHDRP(Camera cam, CommandBuffer cmd, RTHandle source, RTHandle destination, EnviroVolumetricCloudRenderer renderer)
		{
			if (renderer.shadowMat == null)
			{
				renderer.shadowMat = new Material(Shader.Find("Hidden/EnviroApplyShadowsHDRP"));
			}
			if (renderer.undersampleBufferHandle != null)
			{
				renderer.shadowMat.SetTexture("_MainTex", source);
				renderer.shadowMat.SetTexture("_CloudsTex", renderer.undersampleBufferHandle);
				renderer.shadowMat.SetVector("_HandleScales", new Vector4(1f / renderer.undersampleBufferHandle.rtHandleProperties.rtHandleScale.x, 1f / renderer.undersampleBufferHandle.rtHandleProperties.rtHandleScale.y, 1f, 1f));
				renderer.shadowMat.SetFloat("_Intensity", EnviroManager.instance.VolumetricClouds.settingsGlobal.cloudShadowsIntensity);
				cmd.Blit(source, destination, renderer.shadowMat);
			}
		}

		public void RenderVolumetricClouds(Camera cam, RenderTexture source, RenderTexture destination, EnviroVolumetricCloudRenderer renderer, EnviroQuality quality)
		{
			int downsampling = settingsQuality.downsampling;
			if (quality != null)
			{
				downsampling = quality.volumetricCloudsOverride.downsampling;
			}
			int width = cam.pixelWidth / downsampling;
			int height = cam.pixelHeight / downsampling;
			if (cam.cameraType != CameraType.Reflection)
			{
				if (renderer.fullBuffer == null || renderer.fullBuffer.Length != 2)
				{
					renderer.fullBuffer = new RenderTexture[2];
				}
				renderer.fullBufferIndex = (renderer.fullBufferIndex + 1) % 2;
				renderer.firstFrame |= CreateRenderTexture(ref renderer.fullBuffer[0], width, height, RenderTextureFormat.ARGBHalf, FilterMode.Bilinear, source.descriptor);
				renderer.firstFrame |= CreateRenderTexture(ref renderer.fullBuffer[1], width, height, RenderTextureFormat.ARGBHalf, FilterMode.Bilinear, source.descriptor);
			}
			renderer.firstFrame |= CreateRenderTexture(ref renderer.undersampleBuffer, width, height, RenderTextureFormat.ARGBHalf, FilterMode.Bilinear, source.descriptor);
			renderer.frame++;
			if (renderer.frame > 64)
			{
				renderer.frame = 0;
			}
			if (renderer.depthMat == null)
			{
				renderer.depthMat = new Material(Shader.Find("Hidden/EnviroVolumetricCloudsDepth"));
			}
			CreateRenderTexture(ref renderer.downsampledDepth, width, height, RenderTextureFormat.RFloat, FilterMode.Point, source.descriptor);
			renderer.depthMat.SetTexture("_MainTex", source);
			renderer.depthMat.SetVector("_CameraDepthTexture_TexelSize", new Vector4(1 / source.width, 1 / source.height, source.width, source.height));
			if (downsampling > 1)
			{
				Graphics.Blit(source, renderer.downsampledDepth, renderer.depthMat, 0);
			}
			else
			{
				Graphics.Blit(source, renderer.downsampledDepth, renderer.depthMat, 1);
			}
			SetRaymarchShader(cam, renderer, quality);
			renderer.raymarchMat.SetTexture("_MainTex", source);
			Graphics.Blit(source, renderer.undersampleBuffer, renderer.raymarchMat);
			if (cam.cameraType != CameraType.Reflection)
			{
				if (renderer.reprojectMat == null)
				{
					renderer.reprojectMat = new Material(Shader.Find("Hidden/EnviroVolumetricCloudsReproject"));
				}
				SetReprojectShader(cam, renderer, quality);
				if (renderer.firstFrame)
				{
					Graphics.Blit(renderer.undersampleBuffer, renderer.fullBuffer[renderer.fullBufferIndex]);
				}
				renderer.reprojectMat.SetTexture("_MainTex", renderer.fullBuffer[renderer.fullBufferIndex]);
				Graphics.Blit(renderer.fullBuffer[renderer.fullBufferIndex], renderer.fullBuffer[renderer.fullBufferIndex ^ 1], renderer.reprojectMat);
			}
			if (renderer.blendAndLightingMat == null)
			{
				renderer.blendAndLightingMat = new Material(Shader.Find("Hidden/EnviroVolumetricCloudsBlend"));
			}
			SetBlendShader(cam, renderer);
			renderer.blendAndLightingMat.SetTexture("_MainTex", source);
			Graphics.Blit(source, destination, renderer.blendAndLightingMat);
			if (XRSettings.enabled && XRSettings.stereoRenderingMode == XRSettings.StereoRenderingMode.SinglePassInstanced && cam.stereoEnabled)
			{
				renderer.prevV = cam.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left) * cam.worldToCameraMatrix;
				renderer.prevVRight = cam.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right) * cam.worldToCameraMatrix;
			}
			else
			{
				renderer.prevV = cam.projectionMatrix * cam.worldToCameraMatrix;
			}
			renderer.firstFrame = false;
			Shader.SetGlobalTexture("_EnviroCloudsTex", renderer.undersampleBuffer);
		}

		public void RenderVolumetricCloudsHDRP(Camera cam, CommandBuffer cmd, RTHandle source, RTHandle destination, EnviroVolumetricCloudRenderer renderer, EnviroQuality quality)
		{
			int downsampling = settingsQuality.downsampling;
			if (quality != null)
			{
				downsampling = quality.volumetricCloudsOverride.downsampling;
			}
			int width = cam.pixelWidth / downsampling;
			int height = cam.pixelHeight / downsampling;
			RenderTextureDescriptor descriptor = source.rt.descriptor;
			if (cam.cameraType != CameraType.Reflection)
			{
				if (renderer.fullBuffer == null || renderer.fullBuffer.Length != 2)
				{
					renderer.fullBuffer = new RenderTexture[2];
					renderer.fullBufferHandles = new RTHandle[2];
				}
				renderer.fullBufferIndex = (renderer.fullBufferIndex + 1) % 2;
				renderer.firstFrame |= CreateRenderTexture(ref renderer.fullBuffer[0], width, height, RenderTextureFormat.ARGBHalf, FilterMode.Bilinear, descriptor);
				renderer.firstFrame |= CreateRenderTexture(ref renderer.fullBuffer[1], width, height, RenderTextureFormat.ARGBHalf, FilterMode.Bilinear, descriptor);
				renderer.fullBufferHandles[0] = RTHandles.Alloc(renderer.fullBuffer[0]);
				renderer.fullBufferHandles[1] = RTHandles.Alloc(renderer.fullBuffer[1]);
			}
			renderer.firstFrame |= CreateRenderTexture(ref renderer.undersampleBuffer, width, height, RenderTextureFormat.ARGBHalf, FilterMode.Bilinear, descriptor);
			renderer.undersampleBufferHandle = RTHandles.Alloc(renderer.undersampleBuffer);
			renderer.frame++;
			if (renderer.frame > 64)
			{
				renderer.frame = 0;
			}
			if (renderer.depthMat == null)
			{
				renderer.depthMat = new Material(Shader.Find("Hidden/EnviroVolumetricCloudsDepthHDRP"));
			}
			CreateRenderTexture(ref renderer.downsampledDepth, width, height, RenderTextureFormat.RFloat, FilterMode.Point, descriptor);
			renderer.downsampledDepthHandle = RTHandles.Alloc(renderer.downsampledDepth);
			renderer.depthMat.SetTexture("_MainTex", source);
			if (downsampling > 1)
			{
				cmd.Blit(source, renderer.downsampledDepthHandle, renderer.depthMat, 0);
			}
			else
			{
				cmd.Blit(source, renderer.downsampledDepthHandle, renderer.depthMat, 1);
			}
			SetRaymarchShader(cam, renderer, quality);
			renderer.raymarchMat.SetTexture("_MainTex", source);
			cmd.Blit(source, renderer.undersampleBufferHandle, renderer.raymarchMat);
			if (cam.cameraType != CameraType.Reflection)
			{
				if (renderer.reprojectMat == null)
				{
					renderer.reprojectMat = new Material(Shader.Find("Hidden/EnviroVolumetricCloudsReprojectHDRP"));
				}
				SetReprojectShader(cam, renderer, quality);
				if (renderer.firstFrame)
				{
					cmd.Blit(renderer.undersampleBufferHandle, renderer.fullBufferHandles[renderer.fullBufferIndex]);
				}
				renderer.reprojectMat.SetTexture("_MainTex", renderer.fullBufferHandles[renderer.fullBufferIndex]);
				renderer.reprojectMat.SetVector("_MainTexHandleScale", new Vector4(1f / renderer.fullBufferHandles[renderer.fullBufferIndex].rtHandleProperties.rtHandleScale.x, 1f / renderer.fullBufferHandles[renderer.fullBufferIndex].rtHandleProperties.rtHandleScale.y, renderer.fullBuffer[renderer.fullBufferIndex].width, renderer.fullBuffer[renderer.fullBufferIndex].height));
				cmd.Blit(renderer.fullBufferHandles[renderer.fullBufferIndex], renderer.fullBufferHandles[renderer.fullBufferIndex ^ 1], renderer.reprojectMat);
			}
			if (renderer.blendAndLightingMat == null)
			{
				renderer.blendAndLightingMat = new Material(Shader.Find("Hidden/EnviroVolumetricCloudsBlendHDRP"));
			}
			SetBlendShader(cam, renderer);
			renderer.blendAndLightingMat.SetTexture("_MainTex", source);
			cmd.Blit(source, destination, renderer.blendAndLightingMat);
			if (XRSettings.enabled && XRSettings.stereoRenderingMode == XRSettings.StereoRenderingMode.SinglePassInstanced && cam.stereoEnabled)
			{
				renderer.prevV = cam.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left) * cam.worldToCameraMatrix;
				renderer.prevVRight = cam.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right) * cam.worldToCameraMatrix;
			}
			else
			{
				renderer.prevV = cam.projectionMatrix * cam.worldToCameraMatrix;
			}
			renderer.firstFrame = false;
		}

		private void SetRaymarchShader(Camera cam, EnviroVolumetricCloudRenderer renderer, EnviroQuality quality)
		{
			if (renderer.raymarchMat == null)
			{
				renderer.raymarchMat = new Material(Shader.Find("Hidden/EnviroCloudsRaymarchHDRP"));
			}
			if (dirLight == null)
			{
				dirLight = EnviroHelper.GetDirectionalLight();
			}
			else if (EnviroManager.instance.Lighting != null && EnviroManager.instance.Lighting.Settings.lightingMode == EnviroLighting.LightingMode.Dual)
			{
				dirLight = EnviroHelper.GetDirectionalLight();
			}
			EnviroCloudLayerSettings enviroCloudLayerSettings = settingsVolume;
			_ = settingsGlobal;
			float blueNoiseIntensity = settingsQuality.blueNoiseIntensity;
			float lodDistance = settingsQuality.lodDistance;
			Vector4 value = new Vector4(settingsQuality.stepsLayer1, settingsQuality.stepsLayer1, settingsQuality.stepsLayer2, settingsQuality.stepsLayer2);
			_ = settingsQuality.downsampling;
			bool flag = settingsQuality.lightningSupport;
			bool variableBottomNoise = settingsQuality.variableBottomNoise;
			if (quality != null)
			{
				blueNoiseIntensity = quality.volumetricCloudsOverride.blueNoiseIntensity;
				value = new Vector4(quality.volumetricCloudsOverride.stepsLayer1, quality.volumetricCloudsOverride.stepsLayer1, 0f, 0f);
				lodDistance = quality.volumetricCloudsOverride.lodDistance;
				_ = quality.volumetricCloudsOverride.downsampling;
				flag = quality.volumetricCloudsOverride.lightningSupport;
				variableBottomNoise = quality.volumetricCloudsOverride.variableBottomNoise;
			}
			if (EnviroManager.instance.Lightning == null)
			{
				flag = false;
			}
			if (flag)
			{
				renderer.raymarchMat.EnableKeyword("ENVIRO_LIGHTNING");
			}
			else
			{
				renderer.raymarchMat.DisableKeyword("ENVIRO_LIGHTNING");
			}
			if (variableBottomNoise)
			{
				renderer.raymarchMat.EnableKeyword("ENVIRO_VARIABLE_BOTTOM");
			}
			else
			{
				renderer.raymarchMat.DisableKeyword("ENVIRO_VARIABLE_BOTTOM");
			}
			renderer.raymarchMat.SetTexture("_Noise", settingsGlobal.noise);
			renderer.raymarchMat.SetTexture("_DetailNoise", settingsGlobal.detailNoise);
			renderer.raymarchMat.SetTexture("_CurlNoise", settingsGlobal.curlTex);
			if (settingsGlobal.bottomsOffsetNoise != null)
			{
				renderer.raymarchMat.SetTexture("_BottomsOffsetNoise", settingsGlobal.bottomsOffsetNoise);
			}
			if (weatherMap != null)
			{
				renderer.raymarchMat.SetTexture("_WeatherMap", weatherMap);
			}
			else if (settingsGlobal.customWeatherMap != null)
			{
				renderer.raymarchMat.SetTexture("_WeatherMap", settingsGlobal.customWeatherMap);
			}
			if (XRSettings.enabled && XRSettings.stereoRenderingMode == XRSettings.StereoRenderingMode.SinglePassInstanced && cam.stereoEnabled)
			{
				renderer.raymarchMat.SetMatrix("_InverseProjection", cam.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left).inverse);
				renderer.raymarchMat.SetMatrix("_InverseRotation", cam.GetStereoViewMatrix(Camera.StereoscopicEye.Left).inverse);
				renderer.raymarchMat.SetMatrix("_InverseProjectionRight", cam.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right).inverse);
				renderer.raymarchMat.SetMatrix("_InverseRotationRight", cam.GetStereoViewMatrix(Camera.StereoscopicEye.Right).inverse);
			}
			else
			{
				renderer.raymarchMat.SetMatrix("_InverseProjection", cam.projectionMatrix.inverse);
				renderer.raymarchMat.SetMatrix("_InverseRotation", cam.cameraToWorldMatrix);
			}
			if (EnviroManager.instance.Objects.worldAnchor != null)
			{
				settingsGlobal.floatingPointOriginMod = EnviroManager.instance.Objects.worldAnchor.transform.position;
			}
			else
			{
				settingsGlobal.floatingPointOriginMod = Vector3.zero;
			}
			settingsGlobal.floatingPointOriginMod += settingsGlobal.cloudScrollOffset;
			renderer.raymarchMat.SetVector("_CameraPosition", cam.transform.position - settingsGlobal.floatingPointOriginMod);
			renderer.raymarchMat.SetVector("_WorldOffset", settingsGlobal.floatingPointOriginMod);
			renderer.raymarchMat.SetVector("_Steps", value);
			if (dirLight != null)
			{
				renderer.raymarchMat.SetVector("_LightDir", -dirLight.transform.forward);
			}
			else
			{
				renderer.raymarchMat.SetVector("_LightDir", Vector3.zero);
			}
			renderer.raymarchMat.SetVector("_CloudsNoiseSettings", new Vector4(enviroCloudLayerSettings.baseNoiseUV * enviroCloudLayerSettings.baseNoiseUVMultiplier, enviroCloudLayerSettings.detailNoiseUV * enviroCloudLayerSettings.detailNoiseUVMultiplier, enviroCloudLayerSettings.baseNoiseMultiplier, enviroCloudLayerSettings.detailNoiseMultiplier));
			renderer.raymarchMat.SetVector("_CloudsLighting", new Vector4(enviroCloudLayerSettings.scatteringIntensity, enviroCloudLayerSettings.silverLiningIntensity, enviroCloudLayerSettings.edgeHighlightStrength, enviroCloudLayerSettings.silverLiningSpread));
			renderer.raymarchMat.SetVector("_CloudsLightingExtended", new Vector4(enviroCloudLayerSettings.lightningIntensity, enviroCloudLayerSettings.curlIntensity, enviroCloudLayerSettings.lightStepModifier, enviroCloudLayerSettings.absorbtion));
			renderer.raymarchMat.SetVector("_CloudsMultiScattering", new Vector4(enviroCloudLayerSettings.multiScatterStrength, enviroCloudLayerSettings.multiScatterFalloff, enviroCloudLayerSettings.ambientFloor, enviroCloudLayerSettings.exposure));
			renderer.raymarchMat.SetVector("_CloudsShape1", new Vector4(enviroCloudLayerSettings.bottomShape, enviroCloudLayerSettings.midShape, enviroCloudLayerSettings.topShape, enviroCloudLayerSettings.topLayer));
			renderer.raymarchMat.SetVector("_CloudsParameter", new Vector4(enviroCloudLayerSettings.bottomCloudsHeight, enviroCloudLayerSettings.topCloudsHeight, 1f / (enviroCloudLayerSettings.topCloudsHeight - enviroCloudLayerSettings.bottomCloudsHeight), settingsGlobal.cloudsWorldScale));
			renderer.raymarchMat.SetFloat("_BlueNoiseIntensity", blueNoiseIntensity);
			renderer.raymarchMat.SetVector("_CloudDensityScale", new Vector4(enviroCloudLayerSettings.density, 0f, enviroCloudLayerSettings.densitySmoothness, 0f));
			renderer.raymarchMat.SetVector("_CloudsCoverageSettings", new Vector4(enviroCloudLayerSettings.coverage, settingsGlobal.maxRenderDistance, enviroCloudLayerSettings.cloudTypeShaping, enviroCloudLayerSettings.rampShape));
			renderer.raymarchMat.SetVector("_CloudsAnimation", new Vector4(cloudAnimLayer1.x, cloudAnimLayer1.y, cloudAnimLayer1.z, 0f));
			if (EnviroManager.instance.Environment != null)
			{
				renderer.raymarchMat.SetVector("_CloudsWindDirection", new Vector4(EnviroManager.instance.Environment.Settings.windDirectionX * settingsVolume.cloudsWindDirectionXModifier, EnviroManager.instance.Environment.Settings.windDirectionY * settingsVolume.cloudsWindDirectionYModifier, cloudAnimNonScaledLayer1.x, cloudAnimNonScaledLayer1.y));
			}
			else
			{
				renderer.raymarchMat.SetVector("_CloudsWindDirection", new Vector4(settingsVolume.cloudsWindDirectionXModifier, settingsVolume.cloudsWindDirectionYModifier, cloudAnimNonScaledLayer1.x, cloudAnimNonScaledLayer1.y));
			}
			renderer.raymarchMat.SetVector("_CloudsErosionIntensity", new Vector4(1f - enviroCloudLayerSettings.baseErosionIntensity, enviroCloudLayerSettings.detailErosionIntensity, 0f, 0f));
			renderer.raymarchMat.SetFloat("_LODDistance", lodDistance);
			renderer.raymarchMat.SetTexture("_DownsampledDepth", renderer.downsampledDepthHandle);
			renderer.raymarchMat.SetVector("_DepthHandleScale", new Vector4(1f / renderer.downsampledDepthHandle.rtHandleProperties.rtHandleScale.x, 1f / renderer.downsampledDepthHandle.rtHandleProperties.rtHandleScale.y, renderer.downsampledDepth.width, renderer.downsampledDepth.height));
			renderer.raymarchMat.SetInt("_Frame", renderer.frame);
			renderer.raymarchMat.SetTexture("_BlueNoise", settingsGlobal.blueNoise);
			renderer.raymarchMat.SetVector("_Randomness", new Vector4(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value));
			renderer.raymarchMat.SetVector("_Resolution", new Vector4(cam.pixelWidth, cam.pixelHeight, 0f, 0f));
			if (settingsGlobal.cloudShadows)
			{
				renderer.raymarchMat.EnableKeyword("ENVIRO_CLOUD_SHADOWS");
			}
			else
			{
				renderer.raymarchMat.DisableKeyword("ENVIRO_CLOUD_SHADOWS");
			}
			renderer.raymarchMat.SetFloat("_DepthTest", settingsGlobal.depthTest ? 1f : 0f);
			renderer.raymarchMat.SetFloat("_SolarTime", EnviroManager.instance.solarTime);
			SetDepthBlending(renderer.raymarchMat);
		}

		private void SetReprojectShader(Camera cam, EnviroVolumetricCloudRenderer renderer, EnviroQuality quality)
		{
			float reprojectionBlendTime = settingsQuality.reprojectionBlendTime;
			if (quality != null)
			{
				reprojectionBlendTime = quality.volumetricCloudsOverride.reprojectionBlendTime;
			}
			SetDepthBlending(renderer.reprojectMat);
			renderer.reprojectMat.SetTexture("_DownsampledDepth", renderer.downsampledDepthHandle);
			renderer.reprojectMat.SetVector("_DepthHandleScale", new Vector4(1f / renderer.downsampledDepthHandle.rtHandleProperties.rtHandleScale.x, 1f / renderer.downsampledDepthHandle.rtHandleProperties.rtHandleScale.y, renderer.downsampledDepth.width, renderer.downsampledDepth.height));
			renderer.reprojectMat.SetTexture("_UndersampleCloudTex", renderer.undersampleBufferHandle);
			renderer.reprojectMat.SetVector("_UndersampleCloudTexScale", new Vector4(1f / renderer.undersampleBufferHandle.rtHandleProperties.rtHandleScale.x, 1f / renderer.undersampleBufferHandle.rtHandleProperties.rtHandleScale.y, renderer.undersampleBuffer.width, renderer.undersampleBuffer.height));
			if (XRSettings.enabled && XRSettings.stereoRenderingMode == XRSettings.StereoRenderingMode.SinglePassInstanced)
			{
				renderer.reprojectMat.SetMatrix("_PrevVP", renderer.prevV);
				renderer.reprojectMat.SetMatrix("_PrevVPRight", renderer.prevVRight);
				renderer.reprojectMat.SetVector("_ProjectionExtents", EnviroHelper.GetProjectionExtents(cam, Camera.StereoscopicEye.Left));
				renderer.reprojectMat.SetVector("_ProjectionExtentsRight", EnviroHelper.GetProjectionExtents(cam, Camera.StereoscopicEye.Right));
			}
			else
			{
				renderer.reprojectMat.SetMatrix("_PrevVP", renderer.prevV);
				renderer.reprojectMat.SetVector("_ProjectionExtents", EnviroHelper.GetProjectionExtents(cam));
			}
			if (lastOffset != settingsGlobal.floatingPointOriginMod)
			{
				Matrix4x4 value = Matrix4x4.TRS(cam.transform.position - (settingsGlobal.floatingPointOriginMod - lastOffset), cam.transform.rotation, Vector3.one);
				renderer.reprojectMat.SetMatrix("_CamToWorld", value);
				lastOffset = settingsGlobal.floatingPointOriginMod;
			}
			else
			{
				Matrix4x4 value = Matrix4x4.TRS(cam.transform.position, cam.transform.rotation, Vector3.one);
				renderer.reprojectMat.SetMatrix("_CamToWorld", value);
			}
			renderer.reprojectMat.SetFloat("_BlendTime", reprojectionBlendTime);
		}

		private void SetBlendShader(Camera cam, EnviroVolumetricCloudRenderer renderer)
		{
			SetDepthBlending(renderer.blendAndLightingMat);
			if (XRSettings.enabled && XRSettings.stereoRenderingMode == XRSettings.StereoRenderingMode.SinglePassInstanced)
			{
				renderer.blendAndLightingMat.SetVector("_ProjectionExtents", EnviroHelper.GetProjectionExtents(cam, Camera.StereoscopicEye.Left));
				renderer.blendAndLightingMat.SetVector("_ProjectionExtentsRight", EnviroHelper.GetProjectionExtents(cam, Camera.StereoscopicEye.Right));
			}
			else
			{
				renderer.blendAndLightingMat.SetVector("_ProjectionExtents", EnviroHelper.GetProjectionExtents(cam));
			}
			renderer.blendAndLightingMat.SetTexture("_DownsampledDepth", renderer.downsampledDepthHandle);
			renderer.blendAndLightingMat.SetVector("_DepthHandleScale", new Vector4(1f / renderer.downsampledDepthHandle.rtHandleProperties.rtHandleScale.x, 1f / renderer.downsampledDepthHandle.rtHandleProperties.rtHandleScale.y, renderer.downsampledDepth.width, renderer.downsampledDepth.height));
			Matrix4x4 value = Matrix4x4.TRS(cam.transform.position, cam.transform.rotation, Vector3.one);
			renderer.blendAndLightingMat.SetMatrix("_CamToWorld", value);
			Color value2 = (EnviroManager.instance.isNight ? settingsGlobal.moonLightColorGradient.Evaluate(EnviroManager.instance.lunarTime) : settingsGlobal.sunLightColorGradient.Evaluate(EnviroManager.instance.solarTime));
			Shader.SetGlobalColor("_DirectLightColor", value2);
			Shader.SetGlobalColor("_AmbientColor", settingsGlobal.ambientColorGradient.Evaluate(EnviroManager.instance.solarTime) * settingsGlobal.ambientLighIntensity);
			Shader.SetGlobalFloat("_AtmosphereColorSaturateDistance", settingsGlobal.atmosphereColorSaturateDistance);
			Shader.SetGlobalVector("_CloudsParameter", new Vector4(settingsVolume.bottomCloudsHeight, settingsVolume.topCloudsHeight, 1f / (settingsVolume.topCloudsHeight - settingsVolume.bottomCloudsHeight), settingsGlobal.cloudsWorldScale));
			Shader.SetGlobalFloat("_SolarTime", EnviroManager.instance.solarTime);
			if (cam.cameraType == CameraType.Reflection)
			{
				renderer.blendAndLightingMat.SetTexture("_CloudTex", renderer.undersampleBufferHandle);
				renderer.blendAndLightingMat.SetVector("_HandleScales", new Vector4(1f / renderer.undersampleBufferHandle.rtHandleProperties.rtHandleScale.x, 1f / renderer.undersampleBufferHandle.rtHandleProperties.rtHandleScale.y, 1f, 1f));
			}
			else
			{
				renderer.blendAndLightingMat.SetTexture("_CloudTex", renderer.fullBufferHandles[renderer.fullBufferIndex ^ 1]);
				renderer.blendAndLightingMat.SetVector("_HandleScales", new Vector4(1f / renderer.fullBufferHandles[renderer.fullBufferIndex ^ 1].rtHandleProperties.rtHandleScale.x, 1f / renderer.fullBufferHandles[renderer.fullBufferIndex ^ 1].rtHandleProperties.rtHandleScale.y, 1f, 1f));
			}
			if (renderer.camera != null && renderer.camera.transform.position.y - settingsGlobal.floatingPointOriginMod.y <= settingsVolume.bottomCloudsHeight)
			{
				if (blackArray == null)
				{
					CreateBlackArray();
				}
				Shader.SetGlobalTexture("_EnviroClouds", blackArray);
			}
			else if (renderer != null && renderer.fullBufferHandles != null && renderer.fullBufferHandles.Length >= 2 && renderer.fullBufferHandles[renderer.fullBufferIndex ^ 1] != null)
			{
				Shader.SetGlobalTexture("_EnviroClouds", renderer.fullBufferHandles[renderer.fullBufferIndex ^ 1]);
			}
		}

		private void SetDepthBlending(Material mat)
		{
			if (settingsGlobal.depthBlending)
			{
				mat.EnableKeyword("ENVIRO_DEPTH_BLENDING");
			}
			else
			{
				mat.DisableKeyword("ENVIRO_DEPTH_BLENDING");
			}
		}

		private void SetToURP(Material mat)
		{
			mat.EnableKeyword("ENVIROURP");
		}

		public bool CreateRenderTexture(ref RenderTexture texture, int width, int height, RenderTextureFormat format, FilterMode filterMode, RenderTextureDescriptor dsc)
		{
			if (texture != null && (texture.width != width || texture.height != height || texture.vrUsage != dsc.vrUsage))
			{
				UnityEngine.Object.DestroyImmediate(texture);
				texture = null;
			}
			if (texture == null)
			{
				RenderTextureDescriptor desc = dsc;
				desc.width = width;
				desc.height = height;
				desc.colorFormat = format;
				desc.depthBufferBits = 0;
				texture = new RenderTexture(desc);
				texture.antiAliasing = 1;
				texture.useMipMap = false;
				texture.filterMode = filterMode;
				texture.Create();
				return true;
			}
			return false;
		}

		public RenderTexture RenderWeatherMap()
		{
			if (settingsGlobal.customWeatherMap != null)
			{
				return null;
			}
			if (weatherMapMat == null)
			{
				weatherMapMat = new Material(Shader.Find("Enviro3/Standard/WeatherTexture"));
			}
			if (weatherMap == null)
			{
				RenderTextureFormat format = RenderTextureFormat.ARGBFloat;
				weatherMap = new RenderTexture(512, 512, 0, format);
				weatherMap.wrapMode = TextureWrapMode.Repeat;
			}
			weatherMapMat.SetFloat("_CoverageLayer1", settingsVolume.coverage);
			weatherMapMat.SetFloat("_WorleyFreq1Layer1", settingsVolume.worleyFreq1);
			weatherMapMat.SetFloat("_WorleyFreq2Layer1", settingsVolume.worleyFreq2);
			weatherMapMat.SetFloat("_DilateCoverageLayer1", settingsVolume.dilateCoverage);
			weatherMapMat.SetFloat("_DilateTypeLayer1", settingsVolume.dilateType);
			weatherMapMat.SetFloat("_CloudsTypeModifierLayer1", settingsVolume.cloudsTypeModifier);
			weatherMapMat.SetVector("_LocationOffset", new Vector4(settingsVolume.locationOffset.x, settingsVolume.locationOffset.y, 0f, 0f));
			weatherMapMat.SetVector("_WindDirectionLayer1", cloudAnimNonScaledLayer1);
			weatherMapMat.SetVector("_WindDirectionLayer2", cloudAnimNonScaledLayer2);
			Graphics.Blit(null, weatherMap, weatherMapMat);
			return weatherMap;
		}

		private void UpdateWind()
		{
			if (EnviroManager.instance.Environment != null)
			{
				cloudAnimLayer1 += new Vector3(EnviroManager.instance.Environment.Settings.windSpeed * settingsVolume.windSpeedModifier * EnviroManager.instance.Environment.Settings.windDirectionX * settingsVolume.cloudsWindDirectionXModifier * Time.deltaTime, EnviroManager.instance.Environment.Settings.windSpeed * settingsVolume.windSpeedModifier * EnviroManager.instance.Environment.Settings.windDirectionY * settingsVolume.cloudsWindDirectionYModifier * Time.deltaTime, -1f * settingsVolume.windUpwards * Time.deltaTime);
				cloudAnimLayer1 = EnviroHelper.PingPong(cloudAnimLayer1);
				cloudAnimNonScaledLayer1 += new Vector3(settingsVolume.windSpeedModifier * EnviroManager.instance.Environment.Settings.windSpeed * EnviroManager.instance.Environment.Settings.windDirectionX * settingsVolume.cloudsWindDirectionXModifier * Time.deltaTime * 4f, settingsVolume.windSpeedModifier * EnviroManager.instance.Environment.Settings.windSpeed * EnviroManager.instance.Environment.Settings.windDirectionY * settingsVolume.cloudsWindDirectionYModifier * Time.deltaTime * 4f, -1f * EnviroManager.instance.Environment.Settings.windSpeed * Time.deltaTime) * settingsGlobal.cloudsTravelSpeed * 0.2f;
			}
			else
			{
				cloudAnimLayer1 += new Vector3(settingsVolume.windSpeedModifier * settingsVolume.cloudsWindDirectionXModifier * Time.deltaTime, settingsVolume.windSpeedModifier * settingsVolume.cloudsWindDirectionYModifier * Time.deltaTime, -1f * settingsVolume.windUpwards * Time.deltaTime);
				cloudAnimLayer1 = EnviroHelper.PingPong(cloudAnimLayer1);
				cloudAnimNonScaledLayer1 += new Vector3(settingsVolume.windSpeedModifier * settingsVolume.cloudsWindDirectionXModifier * Time.deltaTime * 4f, settingsVolume.windSpeedModifier * settingsVolume.cloudsWindDirectionYModifier * Time.deltaTime * 4f, -1f * settingsVolume.windUpwards * Time.deltaTime) * settingsGlobal.cloudsTravelSpeed * 0.2f;
			}
		}

		public void LoadModuleValues()
		{
			if (preset != null)
			{
				settingsVolume = JsonUtility.FromJson<EnviroCloudLayerSettings>(JsonUtility.ToJson(preset.settingsVolume));
				settingsGlobal = JsonUtility.FromJson<EnviroCloudGlobalSettings>(JsonUtility.ToJson(preset.settingsGlobal));
			}
			else
			{
				Debug.Log("Please assign a saved module to load from!");
			}
		}

		public void SaveModuleValues()
		{
		}

		public void SaveModuleValues(EnviroVolumetricCloudsModule module)
		{
			module.settingsVolume = JsonUtility.FromJson<EnviroCloudLayerSettings>(JsonUtility.ToJson(settingsVolume));
			module.settingsGlobal = JsonUtility.FromJson<EnviroCloudGlobalSettings>(JsonUtility.ToJson(settingsGlobal));
		}
	}
}
