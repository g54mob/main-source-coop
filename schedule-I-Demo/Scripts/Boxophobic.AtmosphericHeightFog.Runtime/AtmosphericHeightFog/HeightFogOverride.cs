using Boxophobic.StyledGUI;
using UnityEngine;
using UnityEngine.Serialization;

namespace AtmosphericHeightFog
{
	[ExecuteInEditMode]
	[HelpURL("https://docs.google.com/document/d/1pIzIHIZ-cSh2ykODSZCbAPtScJ4Jpuu7lS3rNEHCLbc/edit#heading=h.hd5jt8lucuqq")]
	public class HeightFogOverride : StyledMonoBehaviour
	{
		[StyledBanner(0.55f, 0.75f, 1f, "Height Fog Override", "", "https://docs.google.com/document/d/1pIzIHIZ-cSh2ykODSZCbAPtScJ4Jpuu7lS3rNEHCLbc/edit#heading=h.hd5jt8lucuqq")]
		public bool styledBanner;

		[StyledMessage("Info", "The Height Fog Global object is missing from your scene! Please add it before using the Height Fog Override component!", 5f, 0f)]
		public bool messageNoHeightFogGlobal;

		[StyledCategory("Volume Settings", 5f, 10f)]
		public bool categoryVolume;

		public float volumeDistanceFade = 3f;

		public Color volumeGizmoColor = Color.white;

		[StyledCategory("Scene Settings")]
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
		public Color fogColorStart = new Color(0.5f, 0.75f, 0f, 1f);

		[ColorUsage(false, true)]
		public Color fogColorEnd = new Color(0.75f, 1f, 0f, 1f);

		[Range(0f, 1f)]
		public float fogColorDuo;

		[Space(10f)]
		public float fogDistanceStart = -100f;

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

		[Range(0f, 1f)]
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

		[StyledSpace(5)]
		public bool styledSpace0;

		private Material localMaterial;

		private Material missingMaterial;

		private Material currentMaterial;

		private Collider volumeCollider;

		private HeightFogGlobal globalFog;

		private bool distanceSent;

		[HideInInspector]
		public int version;

		private void Start()
		{
			volumeCollider = GetComponent<Collider>();
			if (volumeCollider == null)
			{
				Debug.Log("[Atmospheric Height Fog] Please create override volumes from the GameObject menu > BOXOPHOBIC > Atmospheric Height Fog > Override!");
				Object.DestroyImmediate(this);
			}
			if (GameObject.Find("Height Fog Global") != null)
			{
				GameObject gameObject = GameObject.Find("Height Fog Global");
				globalFog = gameObject.GetComponent<HeightFogGlobal>();
				messageNoHeightFogGlobal = false;
			}
			else
			{
				messageNoHeightFogGlobal = true;
			}
			GetDirectional();
			localMaterial = new Material(Shader.Find("BOXOPHOBIC/Atmospherics/Height Fog Preset"));
			localMaterial.name = "Local";
			missingMaterial = Resources.Load<Material>("Height Fog Preset");
			SetLocalMaterial();
		}

		private void OnDisable()
		{
			if (globalFog != null)
			{
				globalFog.overrideCamToVolumeDistance = 1f;
				globalFog.overrideVolumeDistanceFade = 0f;
			}
		}

		private void OnDestroy()
		{
			if (globalFog != null)
			{
				globalFog.overrideCamToVolumeDistance = 1f;
				globalFog.overrideVolumeDistanceFade = 0f;
			}
		}

		private void Update()
		{
			GetCamera();
			if (mainCamera == null || globalFog == null)
			{
				return;
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
			Vector3 position = mainCamera.transform.position;
			Vector3 b = volumeCollider.ClosestPoint(position);
			float num = Vector3.Distance(position, b);
			if (num > volumeDistanceFade && !distanceSent)
			{
				globalFog.overrideCamToVolumeDistance = float.PositiveInfinity;
				distanceSent = true;
			}
			else if (num < volumeDistanceFade)
			{
				globalFog.overrideMaterial = currentMaterial;
				globalFog.overrideCamToVolumeDistance = num;
				globalFog.overrideVolumeDistanceFade = volumeDistanceFade;
				distanceSent = false;
			}
		}

		private void OnDrawGizmos()
		{
			if (!(volumeCollider == null))
			{
				Color color = volumeGizmoColor;
				float num = 1f;
				if (volumeCollider.GetType() == typeof(BoxCollider))
				{
					BoxCollider component = GetComponent<BoxCollider>();
					Gizmos.color = new Color(color.r * num, color.g * num, color.b * num, color.a);
					Gizmos.DrawWireCube(base.transform.position, new Vector3(base.transform.lossyScale.x * component.size.x, base.transform.lossyScale.y * component.size.y, base.transform.lossyScale.z * component.size.z));
					Gizmos.color = new Color(color.r * num, color.g * num, color.b * num, color.a * 0.5f);
					Gizmos.DrawWireCube(base.transform.position, new Vector3(base.transform.lossyScale.x * component.size.x + volumeDistanceFade * 2f, base.transform.lossyScale.y * component.size.y + volumeDistanceFade * 2f, base.transform.lossyScale.z * component.size.z + volumeDistanceFade * 2f));
				}
				else
				{
					SphereCollider component2 = GetComponent<SphereCollider>();
					float num2 = Mathf.Max(Mathf.Max(base.gameObject.transform.localScale.x, base.gameObject.transform.localScale.y), base.gameObject.transform.localScale.z);
					Gizmos.color = new Color(color.r * num, color.g * num, color.b * num, color.a);
					Gizmos.DrawWireSphere(base.transform.position, component2.radius * num2);
					Gizmos.color = new Color(color.r * num, color.g * num, color.b * num, color.a * 0.5f);
					Gizmos.DrawWireSphere(base.transform.position, component2.radius * num2 + volumeDistanceFade);
				}
			}
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
			localMaterial.SetFloat("_SkyboxFogBottom", skyboxFogFill);
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
	}
}
