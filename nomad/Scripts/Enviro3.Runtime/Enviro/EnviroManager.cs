using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Enviro
{
	[ExecuteInEditMode]
	public class EnviroManager : EnviroManagerBase
	{
		private struct ZoneParams
		{
			public float type;

			public Vector3 pos;

			public float radius;

			public Vector3 size;

			public Vector3 axis;

			public float stretch;

			public float density;

			public float feather;

			public Matrix4x4 transform;

			public float pad0;

			public float pad1;
		}

		public delegate void HourPassed();

		public delegate void DayPassed();

		public delegate void YearPassed();

		public delegate void WeatherChanged(EnviroWeatherType weatherType);

		public delegate void ZoneWeatherChanged(EnviroWeatherType weatherType, EnviroZone zone);

		public delegate void SeasonChanged(EnviroEnvironment.Seasons season);

		public delegate void isNightEvent();

		public delegate void isDayEvent();

		private static EnviroManager _instance;

		public GeneralObjects Objects = new GeneralObjects();

		public bool dontDestroyOnLoad;

		public Camera Camera;

		public string CameraTag = "MainCamera";

		public List<EnviroCameras> Cameras = new List<EnviroCameras>();

		[Tooltip("'Optional': Assign a transform here to change what object Enviro and weather effects should follow. If not set it will use the camera transform.")]
		public Transform optionalFollowTransform;

		public bool showSetup;

		public bool showModules;

		public bool showEvents;

		public bool showThirdParty;

		[Range(0.2f, 0.7f)]
		public float dayNightSwitch = 0.45f;

		public bool isNight;

		public float solarTime;

		public float lunarTime;

		public bool notFirstFrame;

		public List<EnviroEffectRemovalZone> removalZones = new List<EnviroEffectRemovalZone>();

		public ComputeBuffer clearZoneCB;

		public ComputeBuffer removeZoneParamsCB;

		public ComputeBuffer clearCBPoint;

		public ComputeBuffer clearCBSpot;

		private ZoneParams[] removalZoneParams;

		[Range(0f, 360f)]
		public float sunRotationX;

		[Range(0f, 360f)]
		public float sunRotationY;

		[Range(0f, 360f)]
		public float moonRotationX;

		[Range(0f, 360f)]
		public float moonRotationY;

		public bool showNonTimeControls;

		public EnviroEvents Events;

		public EnviroZone currentZone;

		public EnviroZone defaultZone;

		public List<EnviroZone> zones = new List<EnviroZone>();

		public bool updateSkyAndLighting = true;

		public bool updateSkyAndLightingHDRP = true;

		public Volume volumeHDRP;

		public VolumeProfile volumeProfileHDRP;

		public CustomPassVolume customPassVolumeHDRP;

		public static EnviroManager instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = Object.FindAnyObjectByType<EnviroManager>();
				}
				return _instance;
			}
		}

		public event HourPassed OnHourPassed;

		public event DayPassed OnDayPassed;

		public event YearPassed OnYearPassed;

		public event WeatherChanged OnWeatherChanged;

		public event ZoneWeatherChanged OnZoneWeatherChanged;

		public event SeasonChanged OnSeasonChanged;

		public event isNightEvent OnNightTime;

		public event isDayEvent OnDayTime;

		public virtual void NotifyHourPassed()
		{
			if (this.OnHourPassed != null)
			{
				this.OnHourPassed();
			}
		}

		public virtual void NotifyDayPassed()
		{
			if (this.OnDayPassed != null)
			{
				this.OnDayPassed();
			}
		}

		public virtual void NotifyYearPassed()
		{
			if (this.OnYearPassed != null)
			{
				this.OnYearPassed();
			}
		}

		public virtual void NotifyWeatherChanged(EnviroWeatherType type)
		{
			if (this.OnWeatherChanged != null)
			{
				this.OnWeatherChanged(type);
			}
		}

		public virtual void NotifyZoneWeatherChanged(EnviroWeatherType type, EnviroZone zone)
		{
			if (this.OnZoneWeatherChanged != null)
			{
				this.OnZoneWeatherChanged(type, zone);
			}
		}

		public virtual void NotifySeasonChanged(EnviroEnvironment.Seasons season)
		{
			if (this.OnSeasonChanged != null)
			{
				this.OnSeasonChanged(season);
			}
		}

		public virtual void NotifyIsNight()
		{
			if (this.OnNightTime != null)
			{
				this.OnNightTime();
			}
		}

		public virtual void NotifyIsDay()
		{
			if (this.OnDayTime != null)
			{
				this.OnDayTime();
			}
		}

		private void HourPassedInvoke()
		{
			Events.onHourPassedActions.Invoke();
		}

		private void DayPassedInvoke()
		{
			Events.onDayPassedActions.Invoke();
		}

		private void YearPassedInvoke()
		{
			Events.onYearPassedActions.Invoke();
		}

		private void WeatherChangedInvoke()
		{
			Events.onWeatherChangedActions.Invoke();
		}

		private void SeasonsChangedInvoke()
		{
			Events.onSeasonChangedActions.Invoke();
		}

		private void NightTimeInvoke()
		{
			Events.onNightActions.Invoke();
		}

		private void DayTimeInvoke()
		{
			Events.onDayActions.Invoke();
		}

		private void ZoneChangedInvoke()
		{
			Events.onZoneChangedActions.Invoke();
		}

		private void OnEnable()
		{
			if (configuration == null)
			{
				Debug.Log("Please create or assign a configuration asset in your Enviro Manager!");
			}
			CreateGeneralObjects();
			CreateHDRPVolume();
			UpdateManager();
			EnableModules();
			EventInit();
			SetSRPKeywords();
			if (isNight)
			{
				NotifyIsNight();
			}
			else
			{
				NotifyIsDay();
			}
		}

		private void OnDisable()
		{
			Shader.SetGlobalFloat("_EnviroActive", 0f);
			if (Fog != null)
			{
				Fog.Disable();
			}
			ReleaseZoneBuffers();
		}

		private void AddCameraComponents()
		{
			if (Camera != null && Camera.gameObject.GetComponent<EnviroRenderer>() == null)
			{
				Camera.gameObject.AddComponent<EnviroRenderer>();
			}
			for (int i = 0; i < Cameras.Count; i++)
			{
				if (Cameras[i].camera != null && Cameras[i].camera.gameObject.GetComponent<EnviroRenderer>() == null)
				{
					Cameras[i].camera.gameObject.AddComponent<EnviroRenderer>();
				}
			}
		}

		public void ChangeCamera(Camera cam)
		{
			Camera = cam;
		}

		public void AddAdditionalCamera(Camera cam, bool reset = false)
		{
			bool flag = false;
			for (int i = 0; i < Cameras.Count; i++)
			{
				if (Cameras[i].camera != null && Cameras[i].camera == cam)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				EnviroCameras enviroCameras = new EnviroCameras();
				enviroCameras.camera = cam;
				enviroCameras.resetMatrix = reset;
				Cameras.Add(enviroCameras);
			}
		}

		private void Start()
		{
			if (dontDestroyOnLoad && Application.isPlaying)
			{
				Object.DontDestroyOnLoad(base.gameObject);
			}
			notFirstFrame = false;
			StartCoroutine(FirstFrame());
			StartModules();
		}

		private void Update()
		{
			if (!Application.isPlaying)
			{
				LoadConfiguration();
			}
			UpdateManager();
			UpdateModules();
			if (Time == null)
			{
				UpdateNonTime();
			}
			Shader.SetGlobalFloat("_EnviroActive", 1f);
		}

		private void LateUpdate()
		{
			if (Camera != null)
			{
				if (optionalFollowTransform != null)
				{
					base.transform.position = optionalFollowTransform.position;
				}
				else
				{
					base.transform.position = Camera.transform.position;
				}
			}
		}

		private void CreateGeneralObjects()
		{
			if (Objects.sun == null)
			{
				Objects.sun = new GameObject();
				Objects.sun.name = "Sun";
				Objects.sun.transform.SetParent(base.transform);
				Objects.sun.transform.localPosition = Vector3.zero;
			}
			if (Objects.moon == null)
			{
				Objects.moon = new GameObject();
				Objects.moon.name = "Moon";
				Objects.moon.transform.SetParent(base.transform);
				Objects.moon.transform.localPosition = Vector3.zero;
			}
			if (Objects.stars == null)
			{
				Objects.stars = new GameObject();
				Objects.stars.name = "Stars";
				Objects.stars.transform.SetParent(base.transform);
				Objects.stars.transform.localPosition = Vector3.zero;
			}
		}

		public void UpdateNonTime()
		{
			if (Objects.sun != null)
			{
				Objects.sun.transform.eulerAngles = new Vector3(sunRotationX, sunRotationY, 0f);
				if (sunRotationX > 0f && sunRotationX <= 90f)
				{
					solarTime = EnviroHelper.Remap(sunRotationX, 0f, 90f, 0.5f, 1f);
				}
				else if (sunRotationX > 90f && sunRotationX <= 180f)
				{
					solarTime = EnviroHelper.Remap(sunRotationX, 90f, 180f, 1f, 0.5f);
				}
				else if (sunRotationX > 180f && sunRotationX <= 270f)
				{
					solarTime = EnviroHelper.Remap(sunRotationX, 180f, 270f, 0.5f, 0f);
				}
				else if (sunRotationX > 270f && sunRotationX <= 360f)
				{
					solarTime = EnviroHelper.Remap(sunRotationX, 270f, 360f, 0f, 0.5f);
				}
				else
				{
					solarTime = 0.5f;
				}
			}
			if (Objects.moon != null)
			{
				Objects.moon.transform.eulerAngles = new Vector3(moonRotationX, moonRotationY, 0f);
				if (moonRotationX > 0f && moonRotationX <= 90f)
				{
					lunarTime = EnviroHelper.Remap(moonRotationX, 0f, 90f, 0.5f, 1f);
				}
				else if (moonRotationX > 90f && moonRotationX <= 180f)
				{
					lunarTime = EnviroHelper.Remap(moonRotationX, 90f, 180f, 1f, 0.5f);
				}
				else if (moonRotationX > 180f && moonRotationX <= 270f)
				{
					lunarTime = EnviroHelper.Remap(moonRotationX, 180f, 270f, 0.5f, 0f);
				}
				else if (moonRotationX > 270f && moonRotationX <= 360f)
				{
					lunarTime = EnviroHelper.Remap(moonRotationX, 270f, 360f, 0f, 0.5f);
				}
				else
				{
					lunarTime = 0.5f;
				}
			}
		}

		public bool AddRemovalZone(EnviroEffectRemovalZone zone)
		{
			removalZones.Add(zone);
			return true;
		}

		public void RemoveRemovaleZone(EnviroEffectRemovalZone zone)
		{
			if (removalZones.Contains(zone))
			{
				removalZones.Remove(zone);
			}
		}

		private void SetupZoneBuffers()
		{
			int num = 0;
			for (int i = 0; i < removalZones.Count; i++)
			{
				if (removalZones[i] != null && removalZones[i].enabled && removalZones[i].gameObject.activeSelf)
				{
					num++;
				}
			}
			Shader.SetGlobalFloat("_EnviroRemovalZonesCount", num);
			if (num == 0)
			{
				Shader.SetGlobalBuffer("_EnviroRemovalZones", clearZoneCB);
				return;
			}
			if (removalZoneParams == null || removalZoneParams.Length != num)
			{
				removalZoneParams = new ZoneParams[num];
			}
			int num2 = 0;
			for (int j = 0; j < removalZones.Count; j++)
			{
				EnviroEffectRemovalZone enviroEffectRemovalZone = removalZones[j];
				if (!(enviroEffectRemovalZone == null) && enviroEffectRemovalZone.enabled && enviroEffectRemovalZone.gameObject.activeSelf)
				{
					Transform transform = enviroEffectRemovalZone.transform;
					removalZoneParams[num2].type = (float)enviroEffectRemovalZone.type;
					removalZoneParams[num2].pos = transform.position;
					removalZoneParams[num2].radius = enviroEffectRemovalZone.radius * enviroEffectRemovalZone.radius;
					removalZoneParams[num2].size = enviroEffectRemovalZone.size;
					removalZoneParams[num2].axis = -transform.up;
					removalZoneParams[num2].stretch = 1f / enviroEffectRemovalZone.stretch - 1f;
					removalZoneParams[num2].density = enviroEffectRemovalZone.density;
					removalZoneParams[num2].feather = 1f - enviroEffectRemovalZone.feather;
					removalZoneParams[num2].transform = transform.transform.worldToLocalMatrix;
					removalZoneParams[num2].pad0 = 0f;
					removalZoneParams[num2].pad1 = 0f;
					num2++;
				}
			}
			removeZoneParamsCB.SetData(removalZoneParams);
			Shader.SetGlobalBuffer("_EnviroRemovalZones", removeZoneParamsCB);
		}

		private void CreateZoneBuffers()
		{
			EnviroHelper.CreateBuffer(ref removeZoneParamsCB, removalZones.Count, Marshal.SizeOf(typeof(ZoneParams)));
			EnviroHelper.CreateBuffer(ref clearZoneCB, 1, 128);
		}

		private void ReleaseZoneBuffers()
		{
			if (removeZoneParamsCB != null)
			{
				EnviroHelper.ReleaseComputeBuffer(ref removeZoneParamsCB);
			}
			if (clearZoneCB != null)
			{
				EnviroHelper.ReleaseComputeBuffer(ref clearZoneCB);
			}
		}

		private IEnumerator FirstFrame()
		{
			yield return 0;
			notFirstFrame = true;
		}

		public void CreateHDRPVolume()
		{
			if (volumeProfileHDRP == null)
			{
				volumeProfileHDRP = EnviroHelper.GetDefaultSkyAndFogProfile("Enviro HDRP Sky and Fog Volume");
			}
			if (volumeHDRP == null)
			{
				GameObject gameObject = new GameObject();
				gameObject.name = "Enviro Sky and Fog Volume";
				gameObject.transform.SetParent(base.transform);
				gameObject.transform.localPosition = Vector3.zero;
				volumeHDRP = gameObject.AddComponent<Volume>();
				volumeHDRP.sharedProfile = volumeProfileHDRP;
				volumeHDRP.priority = 1f;
			}
			else
			{
				volumeHDRP.sharedProfile = volumeProfileHDRP;
			}
			if (customPassVolumeHDRP == null)
			{
				GameObject gameObject2 = new GameObject();
				gameObject2.name = "Enviro Custom Pass";
				gameObject2.transform.SetParent(base.transform);
				gameObject2.transform.localPosition = Vector3.zero;
				customPassVolumeHDRP = gameObject2.AddComponent<CustomPassVolume>();
				customPassVolumeHDRP.isGlobal = true;
				customPassVolumeHDRP.injectionPoint = CustomPassInjectionPoint.AfterOpaqueAndSky;
				customPassVolumeHDRP.AddPassOfType<EnviroHDRPCustomPass>();
				return;
			}
			bool flag = false;
			foreach (CustomPass customPass in customPassVolumeHDRP.customPasses)
			{
				if (customPass is EnviroHDRPCustomPass)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				customPassVolumeHDRP.AddPassOfType<EnviroHDRPCustomPass>();
			}
		}

		private void CheckCameraSetup()
		{
			if (!(Camera == null))
			{
				return;
			}
			for (int i = 0; i < Camera.allCameras.Length; i++)
			{
				if (Camera.allCameras[i].tag == CameraTag)
				{
					Camera = Camera.allCameras[i];
					AddCameraComponents();
				}
			}
		}

		private void SetSRPKeywords()
		{
			Shader.EnableKeyword("ENVIROHDRP");
			Shader.DisableKeyword("ENVIROURP");
		}

		public void Save()
		{
			if (Time != null)
			{
				PlayerPrefs.SetFloat("Time_Hours", Time.GetTimeOfDay());
				PlayerPrefs.SetInt("Time_Days", Time.days);
				PlayerPrefs.SetInt("Time_Months", Time.months);
				PlayerPrefs.SetInt("Time_Years", Time.years);
			}
			if (!(Weather != null))
			{
				return;
			}
			for (int i = 0; i < Weather.Settings.weatherTypes.Count; i++)
			{
				if (Weather.Settings.weatherTypes[i] == Weather.targetWeatherType)
				{
					PlayerPrefs.SetInt("currentWeather", i);
				}
			}
		}

		public void Load()
		{
			if (Time != null)
			{
				if (PlayerPrefs.HasKey("Time_Hours"))
				{
					Time.SetTimeOfDay(PlayerPrefs.GetFloat("Time_Hours"));
				}
				if (PlayerPrefs.HasKey("Time_Days"))
				{
					Time.days = PlayerPrefs.GetInt("Time_Days");
				}
				if (PlayerPrefs.HasKey("Time_Months"))
				{
					Time.months = PlayerPrefs.GetInt("Time_Months");
				}
				if (PlayerPrefs.HasKey("Time_Years"))
				{
					Time.years = PlayerPrefs.GetInt("Time_Years");
				}
			}
			if (Weather != null && PlayerPrefs.HasKey("currentWeather"))
			{
				Weather.ChangeWeatherInstant(PlayerPrefs.GetInt("currentWeather"));
			}
		}

		private void EventInit()
		{
			if (Time != null)
			{
				OnHourPassed += delegate
				{
					HourPassedInvoke();
				};
				OnDayPassed += delegate
				{
					DayPassedInvoke();
				};
				OnYearPassed += delegate
				{
					YearPassedInvoke();
				};
				OnNightTime += delegate
				{
					NightTimeInvoke();
				};
				OnDayTime += delegate
				{
					DayTimeInvoke();
				};
			}
			if (Weather != null)
			{
				OnWeatherChanged += delegate
				{
					WeatherChangedInvoke();
				};
				OnZoneWeatherChanged += delegate
				{
					ZoneChangedInvoke();
				};
			}
			if (Environment != null)
			{
				OnSeasonChanged += delegate
				{
					SeasonsChangedInvoke();
				};
			}
		}

		private void UpdateManager()
		{
			if (Application.isPlaying)
			{
				CheckCameraSetup();
			}
			if (solarTime > dayNightSwitch)
			{
				if (isNight)
				{
					NotifyIsDay();
				}
				isNight = false;
			}
			else
			{
				if (!isNight)
				{
					NotifyIsNight();
				}
				isNight = true;
			}
			if (Fog != null || Effects != null)
			{
				CreateZoneBuffers();
				SetupZoneBuffers();
			}
		}
	}
}
