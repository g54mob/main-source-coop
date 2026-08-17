using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Enviro
{
	[Serializable]
	public class EnviroFogModule : EnviroModule
	{
		private struct PointLightParams
		{
			public Vector3 pos;

			public float range;

			public Vector3 color;

			private float padding;
		}

		private struct SpotLightParams
		{
			public Vector3 pos;

			public float range;

			public Vector3 color;

			public Vector3 lightDirection;

			public float lightCosHalfAngle;

			private float padding;
		}

		public EnviroFogSettings Settings;

		public EnviroFogModule preset;

		public bool showFogControls;

		public bool showVolumetricsControls;

		public bool showHDRPFogControls;

		public bool showUnityFogControls;

		public List<EnviroVolumetricFogLight> fogLights = new List<EnviroVolumetricFogLight>();

		private Light myLight;

		public float customFogDensityModifer = 1f;

		public Material fogMat;

		public Material volumetricsMat;

		public Material blurMat;

		public Material blurMat2;

		public RenderTexture volumetricsRenderTexture;

		private PointLightParams[] m_PointLightParams;

		private ComputeBuffer m_PointLightParamsCB;

		private SpotLightParams[] m_SpotLightParams;

		private ComputeBuffer m_SpotLightParamsCB;

		public Fog fogHDRP;

		private EnviroVolumetricFogLight directionaLight;

		private EnviroVolumetricFogLight additionalLight;

		public override void Enable()
		{
			_ = EnviroManager.instance == null;
		}

		public override void Disable()
		{
			if (!(EnviroManager.instance == null))
			{
				CleanupHeightFog();
				CleanupVolumetrics();
				if (EnviroManager.instance.Objects.directionalLight != null && EnviroManager.instance.Objects.directionalLight.gameObject.GetComponent<EnviroVolumetricFogLight>() != null)
				{
					UnityEngine.Object.DestroyImmediate(EnviroManager.instance.Objects.directionalLight.gameObject.GetComponent<EnviroVolumetricFogLight>());
				}
				if (EnviroManager.instance.Objects.additionalDirectionalLight != null && EnviroManager.instance.Objects.additionalDirectionalLight.gameObject.GetComponent<EnviroVolumetricFogLight>() != null)
				{
					UnityEngine.Object.DestroyImmediate(EnviroManager.instance.Objects.additionalDirectionalLight.gameObject.GetComponent<EnviroVolumetricFogLight>());
				}
			}
		}

		public override void UpdateModule()
		{
			if (!active || EnviroManager.instance == null)
			{
				return;
			}
			UpdateFogHDRP();
			if (additionalLight != null && directionaLight != null)
			{
				if (EnviroManager.instance.isNight)
				{
					additionalLight.enabled = true;
					directionaLight.enabled = false;
				}
				else
				{
					directionaLight.enabled = true;
					additionalLight.enabled = false;
				}
			}
		}

		public bool AddLight(EnviroVolumetricFogLight light)
		{
			fogLights.Add(light);
			return true;
		}

		public void RemoveLight(EnviroVolumetricFogLight light)
		{
			if (fogLights.Contains(light))
			{
				fogLights.Remove(light);
			}
		}

		private void UpdateFogHDRP()
		{
			if (!(EnviroManager.instance.volumeHDRP != null) || !(EnviroManager.instance.volumeProfileHDRP != null))
			{
				return;
			}
			if (fogHDRP == null)
			{
				if (EnviroManager.instance.volumeProfileHDRP.TryGet<Fog>(out var component))
				{
					fogHDRP = component;
					return;
				}
				EnviroManager.instance.volumeProfileHDRP.Add<Fog>();
				if (EnviroManager.instance.volumeProfileHDRP.TryGet<Fog>(out component))
				{
					fogHDRP = component;
				}
				return;
			}
			if (Settings.controlHDRPFog)
			{
				fogHDRP.active = true;
				fogHDRP.enabled.overrideState = true;
				fogHDRP.enabled.value = Settings.controlHDRPFog;
				fogHDRP.meanFreePath.overrideState = true;
				fogHDRP.meanFreePath.value = Settings.fogAttenuationDistance;
				fogHDRP.baseHeight.overrideState = true;
				fogHDRP.baseHeight.value = Settings.baseHeight;
				fogHDRP.maximumHeight.overrideState = true;
				fogHDRP.maximumHeight.value = Settings.maxHeight;
				fogHDRP.tint.overrideState = true;
				fogHDRP.tint.value = Settings.fogColorTint.Evaluate(EnviroManager.instance.solarTime);
			}
			else
			{
				fogHDRP.active = false;
			}
			if (Settings.controlHDRPVolumetrics)
			{
				fogHDRP.enableVolumetricFog.overrideState = true;
				fogHDRP.enableVolumetricFog.value = true;
				fogHDRP.albedo.overrideState = true;
				fogHDRP.albedo.value = Settings.volumetricsColorTint.Evaluate(EnviroManager.instance.solarTime);
				fogHDRP.globalLightProbeDimmer.overrideState = true;
				fogHDRP.globalLightProbeDimmer.value = Settings.ambientDimmer;
				if (EnviroManager.instance.Lighting != null && EnviroManager.instance.Lighting.directionalLightHDRP != null)
				{
					EnviroManager.instance.Lighting.directionalLightHDRP.volumetricDimmer = Settings.directLightMultiplier;
					EnviroManager.instance.Lighting.directionalLightHDRP.volumetricShadowDimmer = Settings.directLightShadowdimmer;
				}
				if (EnviroManager.instance.Lighting != null && EnviroManager.instance.Lighting.additionalLightHDRP != null)
				{
					EnviroManager.instance.Lighting.additionalLightHDRP.volumetricDimmer = Settings.directLightMultiplier;
					EnviroManager.instance.Lighting.additionalLightHDRP.volumetricShadowDimmer = Settings.directLightShadowdimmer;
				}
			}
			else
			{
				fogHDRP.enableVolumetricFog.value = false;
			}
		}

		private void UpdateUnityFog()
		{
			RenderSettings.fog = Settings.unityFog;
			RenderSettings.fogMode = Settings.unityFogMode;
			if (Settings.unityFogMode == FogMode.Linear)
			{
				RenderSettings.fogStartDistance = Settings.unityFogStartDistance;
				RenderSettings.fogEndDistance = Settings.unityFogEndDistance;
			}
			else
			{
				RenderSettings.fogDensity = Settings.unityFogDensity;
			}
			RenderSettings.fogColor = Settings.unityFogColor.Evaluate(EnviroManager.instance.solarTime) * (Settings.fogColorMod * EnviroManager.instance.solarTime);
		}

		public void UpdateFogShader(Camera cam)
		{
			if (Settings.fogQualityMode == EnviroFogSettings.FogQualityMode.Simple)
			{
				Shader.EnableKeyword("ENVIRO_SIMPLEFOG");
				Shader.SetGlobalVector("_EnviroFogParameters", new Vector4(0f, Settings.fogHeightFalloff, Settings.fogDensity * 0.01f * customFogDensityModifer, Settings.fogHeight + Settings.globalFogHeight));
			}
			else
			{
				Shader.DisableKeyword("ENVIRO_SIMPLEFOG");
				Shader.SetGlobalVector("_EnviroFogParameters", new Vector4(0f, Settings.fogHeightFalloff, Settings.fogDensity * 0.01f * customFogDensityModifer, Settings.fogHeight + Settings.globalFogHeight));
				Shader.SetGlobalVector("_EnviroFogParameters2", new Vector4(0f, Settings.fogHeightFalloff2, Settings.fogDensity2 * 0.01f * customFogDensityModifer, Settings.fogHeight2 + Settings.globalFogHeight));
			}
			Shader.SetGlobalVector("_EnviroFogParameters3", new Vector4(1f - Settings.fogMaxOpacity, Settings.startDistance, Settings.blockScattering ? 0f : 1f, Settings.fogColorBlend));
			Shader.SetGlobalColor("_EnviroFogColor", Settings.ambientColorGradient.Evaluate(EnviroManager.instance.solarTime) * (Settings.fogColorMod * EnviroManager.instance.solarTime));
			if (EnviroManager.instance.Objects.worldAnchor != null)
			{
				Settings.floatingPointOriginMod = EnviroManager.instance.Objects.worldAnchor.transform.position;
			}
			else
			{
				Settings.floatingPointOriginMod = Vector3.zero;
			}
			Shader.SetGlobalVector("_EnviroCameraPos", cam.transform.position - Settings.floatingPointOriginMod);
			Shader.SetGlobalVector("_EnviroWorldOffset", Settings.floatingPointOriginMod);
		}

		public void RenderHeightFog(Camera cam, RenderTexture source, RenderTexture destination)
		{
			if (fogMat == null)
			{
				fogMat = new Material(Shader.Find("Hidden/EnviroHeightFog"));
			}
			UpdateFogShader(cam);
			fogMat.SetTexture("_MainTex", source);
			Graphics.Blit(source, destination, fogMat);
		}

		public void RenderHeightFogHDRP(Camera cam, CommandBuffer cmd, RTHandle source, RTHandle destination)
		{
			if (fogMat == null)
			{
				fogMat = new Material(Shader.Find("Hidden/EnviroHeightFogHDRP"));
			}
			UpdateFogShader(cam);
			fogMat.SetTexture("_MainTex", source);
			cmd.Blit(source, destination, fogMat);
		}

		private void CleanupHeightFog()
		{
			if (!(EnviroManager.instance == null))
			{
				if (fogMat != null)
				{
					UnityEngine.Object.DestroyImmediate(fogMat);
				}
				if (EnviroManager.instance.removeZoneParamsCB != null)
				{
					EnviroHelper.ReleaseComputeBuffer(ref EnviroManager.instance.removeZoneParamsCB);
				}
				if (EnviroManager.instance.clearZoneCB != null)
				{
					EnviroHelper.ReleaseComputeBuffer(ref EnviroManager.instance.clearZoneCB);
				}
				if (EnviroManager.instance.clearCBPoint != null)
				{
					EnviroHelper.ReleaseComputeBuffer(ref EnviroManager.instance.clearCBPoint);
				}
				if (EnviroManager.instance.clearCBSpot != null)
				{
					EnviroHelper.ReleaseComputeBuffer(ref EnviroManager.instance.clearCBSpot);
				}
			}
		}

		public void RenderVolumetrics(Camera camera, RenderTexture source)
		{
			if (!Settings.volumetrics || camera.cameraType == CameraType.Reflection)
			{
				Shader.DisableKeyword("ENVIRO_VOLUMELIGHT");
				return;
			}
			Shader.EnableKeyword("ENVIRO_VOLUMELIGHT");
			if (volumetricsMat == null)
			{
				volumetricsMat = new Material(Shader.Find("Hidden/Volumetrics"));
			}
			if (blurMat == null)
			{
				blurMat = new Material(Shader.Find("Hidden/EnviroBlur"));
			}
			CreateVolumetricsBuffers();
			SetUpPointLightBuffers();
			SetUpSpotLightBuffers();
			UpdateVolumetricsShader(volumetricsMat);
			RenderTextureDescriptor descriptor = source.descriptor;
			descriptor.msaaSamples = 1;
			if (volumetricsRenderTexture == null || volumetricsRenderTexture.width != descriptor.width || volumetricsRenderTexture.height != descriptor.height)
			{
				if (volumetricsRenderTexture != null)
				{
					UnityEngine.Object.DestroyImmediate(volumetricsRenderTexture);
				}
				volumetricsRenderTexture = new RenderTexture(descriptor);
			}
			if (Settings.quality == EnviroFogSettings.Quality.High)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(descriptor);
				volumetricsMat.SetTexture("_MainTex", source);
				Graphics.Blit(source, temporary, volumetricsMat);
				RenderTexture temporary2 = RenderTexture.GetTemporary(descriptor);
				blurMat.SetTexture("_MainTex", temporary);
				Graphics.Blit(temporary, temporary2, blurMat, 0);
				blurMat.SetTexture("_MainTex", temporary2);
				Graphics.Blit(temporary2, temporary, blurMat, 1);
				Graphics.Blit(temporary, volumetricsRenderTexture);
				RenderTexture.ReleaseTemporary(temporary2);
				RenderTexture.ReleaseTemporary(temporary);
			}
			else if (Settings.quality == EnviroFogSettings.Quality.Medium)
			{
				descriptor.width = source.width / 2;
				descriptor.height = source.height / 2;
				RenderTexture temporary3 = RenderTexture.GetTemporary(descriptor);
				RenderTexture temporary4 = RenderTexture.GetTemporary(descriptor);
				temporary4.filterMode = FilterMode.Point;
				volumetricsMat.SetTexture("_MainTex", source);
				Graphics.Blit(source, temporary3, volumetricsMat);
				blurMat.SetTexture("_MainTex", source);
				Graphics.Blit(source, temporary4, blurMat, 4);
				blurMat.SetTexture("_HalfResDepthBuffer", temporary4);
				blurMat.SetTexture("_HalfResColor", temporary3);
				RenderTexture temporary5 = RenderTexture.GetTemporary(descriptor);
				blurMat.SetTexture("_MainTex", temporary3);
				Graphics.Blit(temporary3, temporary5, blurMat, 2);
				blurMat.SetTexture("_MainTex", temporary5);
				Graphics.Blit(temporary5, temporary3, blurMat, 3);
				blurMat.SetTexture("_MainTex", temporary3);
				Graphics.Blit(temporary3, volumetricsRenderTexture, blurMat, 5);
				RenderTexture.ReleaseTemporary(temporary5);
				RenderTexture.ReleaseTemporary(temporary3);
				RenderTexture.ReleaseTemporary(temporary4);
			}
			else if (Settings.quality == EnviroFogSettings.Quality.Low)
			{
				descriptor.width = source.width / 2;
				descriptor.height = source.height / 2;
				RenderTexture temporary6 = RenderTexture.GetTemporary(descriptor);
				temporary6.filterMode = FilterMode.Point;
				descriptor.width = source.width / 4;
				descriptor.height = source.height / 4;
				RenderTexture temporary7 = RenderTexture.GetTemporary(descriptor);
				RenderTexture temporary8 = RenderTexture.GetTemporary(descriptor);
				temporary8.filterMode = FilterMode.Point;
				volumetricsMat.SetTexture("_MainTex", source);
				Graphics.Blit(source, temporary7, volumetricsMat);
				blurMat.SetTexture("_MainTex", source);
				Graphics.Blit(source, temporary6, blurMat, 4);
				Graphics.Blit(source, temporary8, blurMat, 6);
				blurMat.SetTexture("_HalfResDepthBuffer", temporary6);
				blurMat.SetTexture("_QuarterResDepthBuffer", temporary8);
				blurMat.SetTexture("_QuarterResColor", temporary7);
				RenderTexture temporary9 = RenderTexture.GetTemporary(descriptor);
				blurMat.SetTexture("_MainTex", temporary7);
				Graphics.Blit(temporary7, temporary9, blurMat, 8);
				blurMat.SetTexture("_MainTex", temporary9);
				Graphics.Blit(temporary9, temporary7, blurMat, 9);
				blurMat.SetTexture("_MainTex", temporary7);
				Graphics.Blit(temporary7, volumetricsRenderTexture, blurMat, 7);
				RenderTexture.ReleaseTemporary(temporary9);
				RenderTexture.ReleaseTemporary(temporary7);
				RenderTexture.ReleaseTemporary(temporary6);
				RenderTexture.ReleaseTemporary(temporary8);
			}
			Shader.SetGlobalTexture("_EnviroVolumetricFogTex", volumetricsRenderTexture);
		}

		private void UpdateVolumetricsShader(Material mat)
		{
			if (EnviroManager.instance.Lighting != null)
			{
				myLight = EnviroHelper.GetDirectionalLight();
			}
			else if (myLight == null)
			{
				myLight = EnviroHelper.GetDirectionalLight();
			}
			mat.SetInt("_Steps", Settings.steps);
			if (myLight == null)
			{
				mat.SetVector("_DirLightDir", new Vector4(0f, 0f, 0f, 0.5f));
				Shader.SetGlobalColor("_EnviroDirLightColor", Color.white * 1f);
			}
			else
			{
				mat.SetVector("_DirLightDir", new Vector4(myLight.transform.forward.x, myLight.transform.forward.y, myLight.transform.forward.z, 1f / (myLight.range * myLight.range)));
				Shader.SetGlobalColor("_EnviroDirLightColor", myLight.color * myLight.intensity);
			}
			mat.SetFloat("_MaxRayLength", Settings.maxRange);
			mat.SetFloat("_MaxRayLengthLights", Settings.maxRangePointSpot);
			mat.SetVector("_WindDirection", new Vector4(Settings.windDirection.x, Settings.windDirection.y, Settings.windDirection.z));
			mat.SetVector("_NoiseData", new Vector4(Settings.noiseScale, Settings.noiseIntensity));
			mat.SetVector("_MieG", new Vector4(Settings.anistropy, 1f + Settings.anistropy * Settings.anistropy, 2f * Settings.anistropy, 1f / (4f * (float)Math.PI)));
			mat.SetVector("_VolumetricLight", new Vector4(Settings.scattering * Settings.scatteringMultiplier.Evaluate(EnviroManager.instance.solarTime), Settings.extinction, 1f, 0f));
			mat.SetTexture("_NoiseTexture", Settings.noise);
			mat.SetTexture("_DitherTexture", Settings.ditheringTex);
			mat.SetVector("_Randomness", new Vector4(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value));
		}

		private void CreateVolumetricsBuffers()
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < fogLights.Count; i++)
			{
				EnviroVolumetricFogLight enviroVolumetricFogLight = fogLights[i];
				if (enviroVolumetricFogLight == null)
				{
					continue;
				}
				bool isOn = enviroVolumetricFogLight.isOn;
				switch (enviroVolumetricFogLight.light.type)
				{
				case LightType.Point:
					if (isOn)
					{
						num++;
					}
					break;
				case LightType.Spot:
					if (isOn)
					{
						num2++;
					}
					break;
				}
			}
			EnviroHelper.CreateBuffer(ref m_PointLightParamsCB, num, Marshal.SizeOf(typeof(PointLightParams)));
			EnviroHelper.CreateBuffer(ref m_SpotLightParamsCB, num2, Marshal.SizeOf(typeof(SpotLightParams)));
			EnviroHelper.CreateBuffer(ref EnviroManager.instance.clearCBPoint, 1, Marshal.SizeOf(typeof(PointLightParams)));
			EnviroHelper.CreateBuffer(ref EnviroManager.instance.clearCBSpot, 1, Marshal.SizeOf(typeof(SpotLightParams)));
		}

		private void CleanupVolumetrics()
		{
			if (volumetricsMat != null)
			{
				UnityEngine.Object.DestroyImmediate(volumetricsMat);
			}
			if (blurMat != null)
			{
				UnityEngine.Object.DestroyImmediate(blurMat);
			}
			if (volumetricsRenderTexture != null)
			{
				UnityEngine.Object.DestroyImmediate(volumetricsRenderTexture);
			}
			EnviroHelper.ReleaseComputeBuffer(ref m_PointLightParamsCB);
			EnviroHelper.ReleaseComputeBuffer(ref m_SpotLightParamsCB);
			EnviroHelper.ReleaseComputeBuffer(ref EnviroManager.instance.clearCBSpot);
			EnviroHelper.ReleaseComputeBuffer(ref EnviroManager.instance.clearCBPoint);
		}

		private void SetUpPointLightBuffers()
		{
			int num = ((m_PointLightParamsCB != null) ? m_PointLightParamsCB.count : 0);
			volumetricsMat.SetFloat("_PointLightsCount", num);
			if (num == 0)
			{
				volumetricsMat.SetBuffer("_PointLights", EnviroManager.instance.clearCBPoint);
				return;
			}
			if (m_PointLightParams == null || m_PointLightParams.Length != num)
			{
				m_PointLightParams = new PointLightParams[num];
			}
			int num2 = 0;
			for (int i = 0; i < fogLights.Count; i++)
			{
				EnviroVolumetricFogLight enviroVolumetricFogLight = fogLights[i];
				if (!(enviroVolumetricFogLight == null) && enviroVolumetricFogLight.light.type == LightType.Point && enviroVolumetricFogLight.isOn)
				{
					Light light = enviroVolumetricFogLight.light;
					m_PointLightParams[num2].pos = light.transform.position;
					float num3 = light.range * enviroVolumetricFogLight.range;
					m_PointLightParams[num2].range = 1f / (num3 * num3);
					m_PointLightParams[num2].color = new Vector3(light.color.r, light.color.g, light.color.b) * light.intensity * enviroVolumetricFogLight.intensity;
					num2++;
				}
			}
			m_PointLightParamsCB.SetData(m_PointLightParams);
			volumetricsMat.SetBuffer("_PointLights", m_PointLightParamsCB);
		}

		private void SetUpSpotLightBuffers()
		{
			int num = ((m_SpotLightParamsCB != null) ? m_SpotLightParamsCB.count : 0);
			volumetricsMat.SetFloat("_SpotLightsCount", num);
			if (num == 0)
			{
				volumetricsMat.SetBuffer("_SpotLights", EnviroManager.instance.clearCBSpot);
				return;
			}
			if (m_SpotLightParams == null || m_SpotLightParams.Length != num)
			{
				m_SpotLightParams = new SpotLightParams[num];
			}
			int num2 = 0;
			for (int i = 0; i < fogLights.Count; i++)
			{
				EnviroVolumetricFogLight enviroVolumetricFogLight = fogLights[i];
				if (!(enviroVolumetricFogLight == null) && enviroVolumetricFogLight.light.type == LightType.Spot && enviroVolumetricFogLight.isOn)
				{
					Light light = enviroVolumetricFogLight.light;
					m_SpotLightParams[num2].pos = light.transform.position;
					float num3 = light.range * enviroVolumetricFogLight.range;
					m_SpotLightParams[num2].range = 1f / (num3 * num3);
					m_SpotLightParams[num2].color = new Vector3(light.color.r, light.color.g, light.color.b) * light.intensity * enviroVolumetricFogLight.intensity;
					m_SpotLightParams[num2].lightDirection = light.transform.forward;
					m_SpotLightParams[num2].lightCosHalfAngle = Mathf.Cos(light.spotAngle * 0.5f * ((float)Math.PI / 180f));
					num2++;
				}
			}
			m_SpotLightParamsCB.SetData(m_SpotLightParams);
			volumetricsMat.SetBuffer("_SpotLights", m_SpotLightParamsCB);
		}

		public void LoadModuleValues()
		{
			if (preset != null)
			{
				Settings = JsonUtility.FromJson<EnviroFogSettings>(JsonUtility.ToJson(preset.Settings));
			}
			else
			{
				Debug.Log("Please assign a saved module to load from!");
			}
		}

		public void SaveModuleValues()
		{
		}

		public void SaveModuleValues(EnviroFogModule module)
		{
			module.Settings = JsonUtility.FromJson<EnviroFogSettings>(JsonUtility.ToJson(Settings));
		}
	}
}
