using Boxophobic.StyledGUI;
using UnityEngine;
using UnityEngine.Serialization;

namespace AtmosphericHeightFog
{
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshFilter))]
	[ExecuteInEditMode]
	public class HeightFogGlobal : StyledMonoBehaviour
	{
		[StyledBanner(0.55f, 0.75f, 1f, "Height Fog Global", "", "https://docs.google.com/document/d/1pIzIHIZ-cSh2ykODSZCbAPtScJ4Jpuu7lS3rNEHCLbc/edit#heading=h.kfvqsi6kusw4")]
		public bool styledBanner;

		[StyledCategory("Scene Settings", 5f, 10f)]
		public bool categoryScene;

		public Camera mainCamera;

		public Light mainDirectional;

		[StyledCategory("Preset Settings")]
		public bool categoryMode;

		public FogMode fogMode = FogMode.UseScriptSettings;

		[StyledMessage("Info", "The Preset feature requires a material using the BOXOPHOBIC > Atmospherics > Fog Preset shader.", 10f, 0f)]
		public bool messagePreset;

		[StyledMessage("Info", "The Time Of Day feature works by interpolating two Fog Preset materials using the BOXOPHOBIC > Atmospherics > Fog Preset shader. Please note that not all material properties can be interpolated properly!", 10f, 0f)]
		public bool messageTimeOfDay;

		[Space(10f)]
		public Material presetMaterial;

		[Space(10f)]
		public Material presetDay;

		public Material presetNight;

		[Space(10f)]
		[Range(0f, 1f)]
		public float timeOfDay;

		[StyledCategory("Fog Settings")]
		public bool categoryFog;

		[Range(0f, 1f)]
		public float fogIntensity = 1f;

		[Space(10f)]
		public FogAxisMode fogAxisMode = FogAxisMode.YAxis;

		public FogLayersMode fogLayersMode = FogLayersMode.MultiplyDistanceAndHeight;

		public FogCameraMode fogCameraMode;

		[Space(10f)]
		[FormerlySerializedAs("fogColor")]
		[ColorUsage(false, true)]
		public Color fogColorStart = new Color(0.5f, 0.75f, 1f, 1f);

		[ColorUsage(false, true)]
		public Color fogColorEnd = new Color(0.75f, 1f, 1.25f, 1f);

		[Range(0f, 1f)]
		public float fogColorDuo;

		[Space(10f)]
		public float fogDistanceStart;

		public float fogDistanceEnd = 100f;

		[Range(1f, 8f)]
		public float fogDistanceFalloff = 1f;

		[Space(10f)]
		public float fogHeightStart;

		public float fogHeightEnd = 100f;

		[Range(1f, 8f)]
		public float fogHeightFalloff = 1f;

		[Space(10f)]
		public float farDistanceHeight;

		public float farDistanceOffset;

		[StyledCategory("Skybox Settings")]
		public bool categorySkybox;

		[Range(0f, 1f)]
		public float skyboxFogIntensity = 1f;

		[Range(0f, 8f)]
		public float skyboxFogHeight = 1f;

		[Range(1f, 8f)]
		public float skyboxFogFalloff = 1f;

		[Range(-1f, 1f)]
		public float skyboxFogOffset;

		[Range(0f, 1f)]
		public float skyboxFogBottom;

		[Range(0f, 1f)]
		public float skyboxFogFill;

		[StyledCategory("Directional Settings")]
		public bool categoryDirectional;

		[Range(0f, 1f)]
		public float directionalIntensity = 1f;

		[Range(1f, 8f)]
		public float directionalFalloff = 1f;

		[ColorUsage(false, true)]
		public Color directionalColor = new Color(1f, 0.75f, 0.5f, 1f);

		[StyledCategory("Noise Settings")]
		public bool categoryNoise;

		[Range(0f, 1f)]
		public float noiseIntensity = 1f;

		[Range(0f, 1f)]
		public float noiseMin;

		[Range(0f, 1f)]
		public float noiseMax = 1f;

		public float noiseScale = 30f;

		public Vector3 noiseSpeed = new Vector3(0.5f, 0f, 0.5f);

		[Space(10f)]
		public float noiseDistanceEnd = 200f;

		[StyledCategory("Advanced Settings")]
		public bool categoryAdvanced;

		public float jitterIntensity;

		public int renderPriority = 1;

		[Space(10f)]
		public bool manualPositionAndScale;

		[StyledSpace(5)]
		public bool styledSpace0;

		private Material localMaterial;

		private Material blendMaterial;

		private Material globalMaterial;

		private Material missingMaterial;

		private Material currentMaterial;

		[HideInInspector]
		public Material overrideMaterial;

		[HideInInspector]
		public float overrideCamToVolumeDistance = 1f;

		[HideInInspector]
		public float overrideVolumeDistanceFade;

		[HideInInspector]
		public int version;

		private void OnEnable()
		{
			base.gameObject.name = "Height Fog Global";
			if (!manualPositionAndScale)
			{
				base.gameObject.transform.position = Vector3.zero;
				base.gameObject.transform.rotation = Quaternion.identity;
			}
			GetCamera();
			GetDirectional();
			if (mainCamera != null)
			{
				if (mainCamera.depthTextureMode != DepthTextureMode.Depth || mainCamera.depthTextureMode != DepthTextureMode.DepthNormals)
				{
					mainCamera.depthTextureMode = DepthTextureMode.Depth;
				}
			}
			else
			{
				Debug.Log("[Atmospheric Height Fog] Camera not found! Make sure you have a camera in the scene or your camera has the MainCamera tag!");
			}
			Mesh builtinResource = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");
			base.gameObject.GetComponent<MeshFilter>().sharedMesh = builtinResource;
			localMaterial = new Material(Shader.Find("BOXOPHOBIC/Atmospherics/Height Fog Preset"));
			localMaterial.name = "Local";
			overrideMaterial = new Material(localMaterial);
			overrideMaterial.name = "Override";
			blendMaterial = new Material(localMaterial);
			blendMaterial.name = "Blend";
			globalMaterial = new Material(Shader.Find("Hidden/BOXOPHOBIC/Atmospherics/Height Fog Global"));
			globalMaterial.name = "Height Fog Global";
			missingMaterial = Resources.Load<Material>("Height Fog Preset");
			base.gameObject.GetComponent<MeshRenderer>().sharedMaterial = globalMaterial;
			base.gameObject.GetComponent<MeshRenderer>().enabled = true;
			Shader.SetGlobalFloat("AHF_Enabled", 1f);
		}

		private void OnDisable()
		{
			base.gameObject.GetComponent<MeshRenderer>().enabled = false;
			Shader.SetGlobalFloat("AHF_Enabled", 0f);
		}

		private void Update()
		{
			if (mainCamera == null)
			{
				return;
			}
			if (!manualPositionAndScale)
			{
				SetFogSphereSize();
				SetFogSpherePosition();
			}
			currentMaterial = localMaterial;
			if (fogMode == FogMode.UseScriptSettings)
			{
				SetLocalMaterial();
				messageTimeOfDay = false;
				messagePreset = false;
			}
			else if (fogMode == FogMode.UsePresetSettings)
			{
				if (presetMaterial != null && presetMaterial.HasProperty("_IsHeightFogPreset"))
				{
					currentMaterial = presetMaterial;
					messagePreset = false;
				}
				else
				{
					currentMaterial = missingMaterial;
					messagePreset = true;
				}
				messageTimeOfDay = false;
			}
			else if (fogMode == FogMode.UseTimeOfDay)
			{
				if (presetDay != null && presetDay.HasProperty("_IsHeightFogPreset") && presetNight != null && presetNight.HasProperty("_IsHeightFogPreset"))
				{
					currentMaterial.Lerp(presetDay, presetNight, timeOfDay);
					messageTimeOfDay = false;
				}
				else
				{
					currentMaterial = missingMaterial;
					messageTimeOfDay = true;
				}
				messagePreset = false;
			}
			if (mainDirectional != null)
			{
				currentMaterial.SetVector("_DirectionalDir", -mainDirectional.transform.forward);
			}
			else
			{
				currentMaterial.SetVector("_DirectionalDir", Vector4.zero);
			}
			if (overrideCamToVolumeDistance > overrideVolumeDistanceFade)
			{
				blendMaterial.CopyPropertiesFromMaterial(currentMaterial);
			}
			else if (overrideCamToVolumeDistance < overrideVolumeDistanceFade)
			{
				float t = 1f - overrideCamToVolumeDistance / overrideVolumeDistanceFade;
				blendMaterial.Lerp(currentMaterial, overrideMaterial, t);
			}
			SetGlobalMaterials();
			SetRenderQueue();
		}

		private void GetCamera()
		{
			if (mainCamera == null)
			{
				mainCamera = Camera.main;
			}
		}

		private void GetDirectional()
		{
			if (!(mainDirectional == null))
			{
				return;
			}
			Light[] array = Object.FindObjectsOfType<Light>();
			float num = 0f;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].type == LightType.Directional && array[i].intensity > num)
				{
					mainDirectional = array[i];
				}
			}
		}

		private void SetLocalMaterial()
		{
			localMaterial.SetFloat("_FogIntensity", fogIntensity);
			localMaterial.SetColor("_FogColorStart", fogColorStart);
			localMaterial.SetColor("_FogColorEnd", fogColorEnd);
			localMaterial.SetFloat("_FogColorDuo", fogColorDuo);
			localMaterial.SetFloat("_FogDistanceStart", fogDistanceStart);
			localMaterial.SetFloat("_FogDistanceEnd", fogDistanceEnd);
			localMaterial.SetFloat("_FogDistanceFalloff", fogDistanceFalloff);
			localMaterial.SetFloat("_FogHeightStart", fogHeightStart);
			localMaterial.SetFloat("_FogHeightEnd", fogHeightEnd);
			localMaterial.SetFloat("_FogHeightFalloff", fogHeightFalloff);
			localMaterial.SetFloat("_FarDistanceHeight", farDistanceHeight);
			localMaterial.SetFloat("_FarDistanceOffset", farDistanceOffset);
			localMaterial.SetFloat("_SkyboxFogIntensity", skyboxFogIntensity);
			localMaterial.SetFloat("_SkyboxFogHeight", skyboxFogHeight);
			localMaterial.SetFloat("_SkyboxFogFalloff", skyboxFogFalloff);
			localMaterial.SetFloat("_SkyboxFogOffset", skyboxFogOffset);
			localMaterial.SetFloat("_SkyboxFogBottom", skyboxFogBottom);
			localMaterial.SetFloat("_SkyboxFogFill", skyboxFogFill);
			localMaterial.SetFloat("_DirectionalIntensity", directionalIntensity);
			localMaterial.SetFloat("_DirectionalFalloff", directionalFalloff);
			localMaterial.SetColor("_DirectionalColor", directionalColor);
			localMaterial.SetFloat("_NoiseIntensity", noiseIntensity);
			localMaterial.SetFloat("_NoiseMin", noiseMin);
			localMaterial.SetFloat("_NoiseMax", noiseMax);
			localMaterial.SetFloat("_NoiseScale", noiseScale);
			localMaterial.SetVector("_NoiseSpeed", noiseSpeed);
			localMaterial.SetFloat("_NoiseDistanceEnd", noiseDistanceEnd);
			localMaterial.SetFloat("_JitterIntensity", jitterIntensity);
			if (fogAxisMode == FogAxisMode.XAxis)
			{
				localMaterial.SetVector("_FogAxisOption", new Vector4(1f, 0f, 0f, 0f));
			}
			else if (fogAxisMode == FogAxisMode.YAxis)
			{
				localMaterial.SetVector("_FogAxisOption", new Vector4(0f, 1f, 0f, 0f));
			}
			else if (fogAxisMode == FogAxisMode.ZAxis)
			{
				localMaterial.SetVector("_FogAxisOption", new Vector4(0f, 0f, 1f, 0f));
			}
			if (fogLayersMode == FogLayersMode.MultiplyDistanceAndHeight)
			{
				localMaterial.SetFloat("_FogLayersMode", 0f);
			}
			else
			{
				localMaterial.SetFloat("_FogLayersMode", 1f);
			}
			if (fogCameraMode == FogCameraMode.Perspective)
			{
				localMaterial.SetFloat("_FogCameraMode", 0f);
			}
			else if (fogCameraMode == FogCameraMode.Orthographic)
			{
				localMaterial.SetFloat("_FogCameraMode", 1f);
			}
			else if (fogCameraMode == FogCameraMode.Both)
			{
				localMaterial.SetFloat("_FogCameraMode", 2f);
			}
		}

		private void SetGlobalMaterials()
		{
			if (blendMaterial.HasProperty("_IsHeightFogPreset"))
			{
				Shader.SetGlobalFloat("AHF_FogIntensity", blendMaterial.GetFloat("_FogIntensity"));
				Shader.SetGlobalVector("AHF_FogAxisOption", blendMaterial.GetVector("_FogAxisOption"));
				Shader.SetGlobalFloat("AHF_FogLayersMode", blendMaterial.GetFloat("_FogLayersMode"));
				Shader.SetGlobalColor("AHF_FogColorStart", blendMaterial.GetColor("_FogColorStart"));
				Shader.SetGlobalColor("AHF_FogColorEnd", blendMaterial.GetColor("_FogColorEnd"));
				Shader.SetGlobalFloat("AHF_FogColorDuo", blendMaterial.GetFloat("_FogColorDuo"));
				Shader.SetGlobalFloat("AHF_FogDistanceStart", blendMaterial.GetFloat("_FogDistanceStart"));
				Shader.SetGlobalFloat("AHF_FogDistanceEnd", blendMaterial.GetFloat("_FogDistanceEnd"));
				Shader.SetGlobalFloat("AHF_FogDistanceFalloff", blendMaterial.GetFloat("_FogDistanceFalloff"));
				Shader.SetGlobalFloat("AHF_FogHeightStart", blendMaterial.GetFloat("_FogHeightStart"));
				Shader.SetGlobalFloat("AHF_FogHeightEnd", blendMaterial.GetFloat("_FogHeightEnd"));
				Shader.SetGlobalFloat("AHF_FogHeightFalloff", blendMaterial.GetFloat("_FogHeightFalloff"));
				Shader.SetGlobalFloat("AHF_FarDistanceHeight", blendMaterial.GetFloat("_FarDistanceHeight"));
				Shader.SetGlobalFloat("AHF_FarDistanceOffset", blendMaterial.GetFloat("_FarDistanceOffset"));
				Shader.SetGlobalFloat("AHF_SkyboxFogIntensity", blendMaterial.GetFloat("_SkyboxFogIntensity"));
				Shader.SetGlobalFloat("AHF_SkyboxFogHeight", blendMaterial.GetFloat("_SkyboxFogHeight"));
				Shader.SetGlobalFloat("AHF_SkyboxFogFalloff", blendMaterial.GetFloat("_SkyboxFogFalloff"));
				Shader.SetGlobalFloat("AHF_SkyboxFogOffset", blendMaterial.GetFloat("_SkyboxFogOffset"));
				Shader.SetGlobalFloat("AHF_SkyboxFogBottom", blendMaterial.GetFloat("_SkyboxFogBottom"));
				Shader.SetGlobalFloat("AHF_SkyboxFogFill", blendMaterial.GetFloat("_SkyboxFogFill"));
				Shader.SetGlobalVector("AHF_DirectionalDir", blendMaterial.GetVector("_DirectionalDir"));
				Shader.SetGlobalFloat("AHF_DirectionalIntensity", blendMaterial.GetFloat("_DirectionalIntensity"));
				Shader.SetGlobalFloat("AHF_DirectionalFalloff", blendMaterial.GetFloat("_DirectionalFalloff"));
				Shader.SetGlobalColor("AHF_DirectionalColor", blendMaterial.GetColor("_DirectionalColor"));
				Shader.SetGlobalFloat("AHF_NoiseIntensity", blendMaterial.GetFloat("_NoiseIntensity"));
				Shader.SetGlobalFloat("AHF_NoiseMin", blendMaterial.GetFloat("_NoiseMin"));
				Shader.SetGlobalFloat("AHF_NoiseMax", blendMaterial.GetFloat("_NoiseMax"));
				Shader.SetGlobalFloat("AHF_NoiseScale", blendMaterial.GetFloat("_NoiseScale"));
				Shader.SetGlobalVector("AHF_NoiseSpeed", blendMaterial.GetVector("_NoiseSpeed"));
				Shader.SetGlobalFloat("AHF_NoiseDistanceEnd", blendMaterial.GetFloat("_NoiseDistanceEnd"));
				Shader.SetGlobalFloat("AHF_JitterIntensity", blendMaterial.GetFloat("_JitterIntensity"));
				switch (blendMaterial.GetInt("_FogCameraMode"))
				{
				case 0:
					Shader.EnableKeyword("AHF_CAMERAMODE_PERSPECTIVE");
					Shader.DisableKeyword("AHF_CAMERAMODE_ORTHOGRAPHIC");
					Shader.DisableKeyword("AHF_CAMERAMODE_BOTH");
					break;
				case 1:
					Shader.DisableKeyword("AHF_CAMERAMODE_PERSPECTIVE");
					Shader.EnableKeyword("AHF_CAMERAMODE_ORTHOGRAPHIC");
					Shader.DisableKeyword("AHF_CAMERAMODE_BOTH");
					break;
				case 2:
					Shader.DisableKeyword("AHF_CAMERAMODE_ORTHOGRAPHIC");
					Shader.DisableKeyword("AHF_CAMERAMODE_PERSPECTIVE");
					Shader.EnableKeyword("AHF_CAMERAMODE_BOTH");
					break;
				}
			}
		}

		private void SetFogSphereSize()
		{
			float num = mainCamera.farClipPlane - 1f;
			base.gameObject.transform.localScale = new Vector3(num, num, num);
		}

		private void SetFogSpherePosition()
		{
			base.transform.position = mainCamera.transform.position;
		}

		private void SetRenderQueue()
		{
			globalMaterial.renderQueue = 3000 + renderPriority;
		}
	}
}
