using UnityEngine;

namespace Enviro
{
	public class EnviroManagerBase : MonoBehaviour
	{
		public enum ModuleType
		{
			Time = 0,
			Lighting = 1,
			Reflections = 2,
			Sky = 3,
			Fog = 4,
			VolumetricClouds = 5,
			FlatClouds = 6,
			Weather = 7,
			Aurora = 8,
			Effects = 9,
			Lightning = 10,
			Environment = 11,
			Audio = 12,
			Quality = 13
		}

		public EnviroConfiguration configuration;

		[SerializeField]
		private EnviroConfiguration lastConfiguration;

		public EnviroTimeModule Time;

		public EnviroLightingModule Lighting;

		public EnviroReflectionsModule Reflections;

		public EnviroSkyModule Sky;

		public EnviroFogModule Fog;

		public EnviroVolumetricCloudsModule VolumetricClouds;

		public EnviroFlatCloudsModule FlatClouds;

		public EnviroWeatherModule Weather;

		public EnviroAuroraModule Aurora;

		public EnviroAudioModule Audio;

		public EnviroEffectsModule Effects;

		public EnviroLightningModule Lightning;

		public EnviroQualityModule Quality;

		public EnviroEnvironmentModule Environment;

		public string version => "3.3.2";

		private string defaultConfig => "Default Enviro Configuration 3_3_2";

		public void EnableModules()
		{
			if (Time != null)
			{
				Time.Enable();
			}
			if (Sky != null)
			{
				Sky.Enable();
			}
			if (Lighting != null)
			{
				Lighting.Enable();
			}
			if (Reflections != null)
			{
				Reflections.Enable();
			}
			if (VolumetricClouds != null)
			{
				VolumetricClouds.Enable();
			}
			if (FlatClouds != null)
			{
				FlatClouds.Enable();
			}
			if (Fog != null)
			{
				Fog.Enable();
			}
			if (Weather != null)
			{
				Weather.Enable();
			}
			if (Aurora != null)
			{
				Aurora.Enable();
			}
			if (Environment != null)
			{
				Environment.Enable();
			}
			if (Audio != null)
			{
				Audio.Enable();
			}
			if (Effects != null)
			{
				Effects.Enable();
			}
			if (Lightning != null)
			{
				Lightning.Enable();
			}
			if (Quality != null)
			{
				Quality.Enable();
			}
		}

		public void DisableModules()
		{
			if (Time != null)
			{
				Time.Disable();
			}
			if (Sky != null)
			{
				Sky.Disable();
			}
			if (Lighting != null)
			{
				Lighting.Disable();
			}
			if (Reflections != null)
			{
				Reflections.Disable();
			}
			if (VolumetricClouds != null)
			{
				VolumetricClouds.Disable();
			}
			if (FlatClouds != null)
			{
				FlatClouds.Disable();
			}
			if (Fog != null)
			{
				Fog.Disable();
			}
			if (Weather != null)
			{
				Weather.Disable();
			}
			if (Aurora != null)
			{
				Aurora.Disable();
			}
			if (Environment != null)
			{
				Environment.Disable();
			}
			if (Audio != null)
			{
				Audio.Disable();
			}
			if (Effects != null)
			{
				Effects.Disable();
			}
			if (Lightning != null)
			{
				Lightning.Disable();
			}
			if (Quality != null)
			{
				Quality.Disable();
			}
		}

		public void DisableAndRemoveModules()
		{
			if (Time != null)
			{
				Time.Disable();
				Time = null;
			}
			if (Sky != null)
			{
				Sky.Disable();
				Sky = null;
			}
			if (Lighting != null)
			{
				Lighting.Disable();
				Lighting = null;
			}
			if (Reflections != null)
			{
				Reflections.Disable();
				Reflections = null;
			}
			if (VolumetricClouds != null)
			{
				VolumetricClouds.Disable();
				VolumetricClouds = null;
			}
			if (FlatClouds != null)
			{
				FlatClouds.Disable();
				FlatClouds = null;
			}
			if (Fog != null)
			{
				Fog.Disable();
				Fog = null;
			}
			if (Weather != null)
			{
				Weather.Disable();
				Weather = null;
			}
			if (Aurora != null)
			{
				Aurora.Disable();
				Aurora = null;
			}
			if (Environment != null)
			{
				Environment.Disable();
				Environment = null;
			}
			if (Audio != null)
			{
				Audio.Disable();
				Audio = null;
			}
			if (Effects != null)
			{
				Effects.Disable();
				Effects = null;
			}
			if (Lightning != null)
			{
				Lightning.Disable();
				Lightning = null;
			}
			if (Quality != null)
			{
				Quality.Disable();
				Quality = null;
			}
		}

		public void StartModules()
		{
			if (Time != null)
			{
				Time = Object.Instantiate(Time);
			}
			if (Sky != null)
			{
				Sky = Object.Instantiate(Sky);
			}
			if (Lighting != null)
			{
				Lighting = Object.Instantiate(Lighting);
			}
			if (Reflections != null)
			{
				Reflections = Object.Instantiate(Reflections);
			}
			if (Fog != null)
			{
				Fog = Object.Instantiate(Fog);
			}
			if (VolumetricClouds != null)
			{
				VolumetricClouds = Object.Instantiate(VolumetricClouds);
			}
			if (FlatClouds != null)
			{
				FlatClouds = Object.Instantiate(FlatClouds);
			}
			if (Weather != null)
			{
				Weather = Object.Instantiate(Weather);
			}
			if (Aurora != null)
			{
				Aurora = Object.Instantiate(Aurora);
			}
			if (Environment != null)
			{
				Environment = Object.Instantiate(Environment);
			}
			if (Audio != null)
			{
				Audio = Object.Instantiate(Audio);
			}
			if (Effects != null)
			{
				Effects = Object.Instantiate(Effects);
			}
			if (Lightning != null)
			{
				Lightning = Object.Instantiate(Lightning);
			}
			if (Quality != null)
			{
				Quality = Object.Instantiate(Quality);
			}
		}

		public void UpdateModules()
		{
			if (Time != null)
			{
				Time.UpdateModule();
			}
			if (Sky != null)
			{
				Sky.UpdateModule();
			}
			if (Lighting != null)
			{
				Lighting.UpdateModule();
			}
			if (Reflections != null)
			{
				Reflections.UpdateModule();
			}
			if (Fog != null)
			{
				Fog.UpdateModule();
			}
			if (VolumetricClouds != null)
			{
				VolumetricClouds.UpdateModule();
			}
			if (FlatClouds != null)
			{
				FlatClouds.UpdateModule();
			}
			if (Weather != null)
			{
				Weather.UpdateModule();
			}
			if (Aurora != null)
			{
				Aurora.UpdateModule();
			}
			if (Environment != null)
			{
				Environment.UpdateModule();
			}
			if (Audio != null)
			{
				Audio.UpdateModule();
			}
			if (Effects != null)
			{
				Effects.UpdateModule();
			}
			if (Lightning != null)
			{
				Lightning.UpdateModule();
			}
			if (Quality != null)
			{
				Quality.UpdateModule();
			}
		}

		public void SaveAllModules()
		{
			if (Time != null && Time.preset != null)
			{
				Time.SaveModuleValues(Time.preset);
			}
			if (Sky != null && Sky.preset != null)
			{
				Sky.SaveModuleValues(Sky.preset);
			}
			if (Lighting != null && Lighting.preset != null)
			{
				Lighting.SaveModuleValues(Lighting.preset);
			}
			if (Reflections != null && Reflections.preset != null)
			{
				Reflections.SaveModuleValues(Reflections.preset);
			}
			if (Fog != null && Fog.preset != null)
			{
				Fog.SaveModuleValues(Fog.preset);
			}
			if (VolumetricClouds != null && VolumetricClouds.preset != null)
			{
				VolumetricClouds.SaveModuleValues(VolumetricClouds.preset);
			}
			if (FlatClouds != null && FlatClouds.preset != null)
			{
				FlatClouds.SaveModuleValues(FlatClouds.preset);
			}
			if (Weather != null && Weather.preset != null)
			{
				Weather.SaveModuleValues(Weather.preset);
			}
			if (Aurora != null && Aurora.preset != null)
			{
				Aurora.SaveModuleValues(Aurora.preset);
			}
			if (Environment != null && Environment.preset != null)
			{
				Environment.SaveModuleValues(Environment.preset);
			}
			if (Audio != null && Audio.preset != null)
			{
				Audio.SaveModuleValues(Audio.preset);
			}
			if (Effects != null && Effects.preset != null)
			{
				Effects.SaveModuleValues(Effects.preset);
			}
			if (Lightning != null && Lightning.preset != null)
			{
				Lightning.SaveModuleValues(Lightning.preset);
			}
			if (Quality != null && Quality.preset != null)
			{
				Quality.SaveModuleValues(Quality.preset);
			}
		}

		public void LoadAllModules()
		{
			if (Time != null)
			{
				Time.LoadModuleValues();
			}
			if (Sky != null)
			{
				Sky.LoadModuleValues();
			}
			if (Lighting != null)
			{
				Lighting.LoadModuleValues();
			}
			if (Reflections != null)
			{
				Reflections.LoadModuleValues();
			}
			if (Fog != null)
			{
				Fog.LoadModuleValues();
			}
			if (VolumetricClouds != null)
			{
				VolumetricClouds.LoadModuleValues();
			}
			if (FlatClouds != null)
			{
				FlatClouds.LoadModuleValues();
			}
			if (Weather != null)
			{
				Weather.LoadModuleValues();
			}
			if (Aurora != null)
			{
				Aurora.LoadModuleValues();
			}
			if (Environment != null)
			{
				Environment.LoadModuleValues();
			}
			if (Audio != null)
			{
				Audio.LoadModuleValues();
			}
			if (Effects != null)
			{
				Effects.LoadModuleValues();
			}
			if (Lightning != null)
			{
				Lightning.LoadModuleValues();
			}
			if (Quality != null)
			{
				Quality.LoadModuleValues();
			}
		}

		public void LoadConfiguration()
		{
			if (configuration != null)
			{
				if (configuration != lastConfiguration)
				{
					DisableModules();
				}
				Time = configuration.timeModule;
				Sky = configuration.Sky;
				Lighting = configuration.lightingModule;
				Reflections = configuration.reflectionsModule;
				VolumetricClouds = configuration.volumetricCloudModule;
				FlatClouds = configuration.flatCloudModule;
				Fog = configuration.fogModule;
				Weather = configuration.Weather;
				Aurora = configuration.Aurora;
				Environment = configuration.Environment;
				Audio = configuration.Audio;
				Effects = configuration.Effects;
				Lightning = configuration.Lightning;
				Quality = configuration.Quality;
				if (configuration != lastConfiguration)
				{
					EnableModules();
				}
				lastConfiguration = configuration;
			}
			else if (configuration == null)
			{
				DisableAndRemoveModules();
			}
		}

		public void AddModule(ModuleType type)
		{
			switch (type)
			{
			case ModuleType.Time:
				if (Time != null)
				{
					Debug.Log("Time module already attached!");
					break;
				}
				Time = ScriptableObject.CreateInstance<EnviroTimeModule>();
				Time.name = "Time Module";
				Time.preset = (EnviroTimeModule)EnviroHelper.GetDefaultPreset("Default Time Preset");
				Time.LoadModuleValues();
				Time.Enable();
				break;
			case ModuleType.Sky:
				if (Sky != null)
				{
					Debug.Log("Sky module already attached!");
					break;
				}
				Sky = ScriptableObject.CreateInstance<EnviroSkyModule>();
				Sky.name = "Sky Module";
				Sky.preset = (EnviroSkyModule)EnviroHelper.GetDefaultPreset("Default Sky Preset");
				Sky.LoadModuleValues();
				Sky.Enable();
				if (configuration != null && !Application.isPlaying)
				{
					configuration.Sky = Sky;
				}
				break;
			case ModuleType.Lighting:
				if (Lighting != null)
				{
					Debug.Log("Lighting module already attached!");
					break;
				}
				Lighting = ScriptableObject.CreateInstance<EnviroLightingModule>();
				Lighting.name = "Lighting Module";
				Lighting.preset = (EnviroLightingModule)EnviroHelper.GetDefaultPreset("Default Lighting Preset");
				Lighting.LoadModuleValues();
				Lighting.Enable();
				if (configuration != null && !Application.isPlaying)
				{
					configuration.lightingModule = Lighting;
				}
				break;
			case ModuleType.Reflections:
				if (Reflections != null)
				{
					Debug.Log("Reflections module already attached!");
					break;
				}
				Reflections = ScriptableObject.CreateInstance<EnviroReflectionsModule>();
				Reflections.name = "Reflections Module";
				Reflections.preset = (EnviroReflectionsModule)EnviroHelper.GetDefaultPreset("Default Reflections Preset");
				Reflections.LoadModuleValues();
				Reflections.Enable();
				if (configuration != null && !Application.isPlaying)
				{
					configuration.reflectionsModule = Reflections;
				}
				break;
			case ModuleType.Fog:
				if (Fog != null)
				{
					Debug.Log("Fog module already attached!");
					break;
				}
				Fog = ScriptableObject.CreateInstance<EnviroFogModule>();
				Fog.name = "Fog Module";
				Fog.preset = (EnviroFogModule)EnviroHelper.GetDefaultPreset("Default Fog Preset");
				Fog.LoadModuleValues();
				Fog.Enable();
				if (configuration != null && !Application.isPlaying)
				{
					configuration.fogModule = Fog;
				}
				break;
			case ModuleType.VolumetricClouds:
				if (VolumetricClouds != null)
				{
					Debug.Log("Volumetric clouds module already attached!");
					break;
				}
				VolumetricClouds = ScriptableObject.CreateInstance<EnviroVolumetricCloudsModule>();
				VolumetricClouds.name = "Volumetric Cloud Module";
				VolumetricClouds.preset = (EnviroVolumetricCloudsModule)EnviroHelper.GetDefaultPreset("Default Volumetric Clouds Preset");
				VolumetricClouds.LoadModuleValues();
				VolumetricClouds.Enable();
				if (configuration != null && !Application.isPlaying)
				{
					configuration.volumetricCloudModule = VolumetricClouds;
				}
				break;
			case ModuleType.FlatClouds:
				if (FlatClouds != null)
				{
					Debug.Log("Flat clouds module already attached!");
					break;
				}
				FlatClouds = ScriptableObject.CreateInstance<EnviroFlatCloudsModule>();
				FlatClouds.name = "Flat Clouds Module";
				FlatClouds.preset = (EnviroFlatCloudsModule)EnviroHelper.GetDefaultPreset("Default Flat Clouds Preset");
				FlatClouds.LoadModuleValues();
				FlatClouds.Enable();
				if (configuration != null && !Application.isPlaying)
				{
					configuration.flatCloudModule = FlatClouds;
				}
				break;
			case ModuleType.Weather:
				if (Weather != null)
				{
					Debug.Log("Weather module already attached!");
					break;
				}
				Weather = ScriptableObject.CreateInstance<EnviroWeatherModule>();
				Weather.name = "Weather Module";
				Weather.preset = (EnviroWeatherModule)EnviroHelper.GetDefaultPreset("Default Weather Preset");
				Weather.LoadModuleValues();
				Weather.Enable();
				if (configuration != null && !Application.isPlaying)
				{
					configuration.Weather = Weather;
				}
				break;
			case ModuleType.Aurora:
				if (Aurora != null)
				{
					Debug.Log("Aurora module already attached!");
					break;
				}
				Aurora = ScriptableObject.CreateInstance<EnviroAuroraModule>();
				Aurora.name = "Aurora Module";
				Aurora.preset = (EnviroAuroraModule)EnviroHelper.GetDefaultPreset("Default Aurora Preset");
				Aurora.LoadModuleValues();
				Aurora.Enable();
				if (configuration != null && !Application.isPlaying)
				{
					configuration.Aurora = Aurora;
				}
				break;
			case ModuleType.Environment:
				if (Environment != null)
				{
					Debug.Log("Environment module already attached!");
					break;
				}
				Environment = ScriptableObject.CreateInstance<EnviroEnvironmentModule>();
				Environment.name = "Environment Module";
				Environment.preset = (EnviroEnvironmentModule)EnviroHelper.GetDefaultPreset("Default Environment Preset");
				Environment.LoadModuleValues();
				Environment.Enable();
				if (configuration != null && !Application.isPlaying)
				{
					configuration.Environment = Environment;
				}
				break;
			case ModuleType.Audio:
				if (Audio != null)
				{
					Debug.Log("Audio module already attached!");
					break;
				}
				Audio = ScriptableObject.CreateInstance<EnviroAudioModule>();
				Audio.name = "Audio Module";
				Audio.preset = (EnviroAudioModule)EnviroHelper.GetDefaultPreset("Default Audio Preset");
				Audio.LoadModuleValues();
				Audio.Enable();
				if (configuration != null && !Application.isPlaying)
				{
					configuration.Audio = Audio;
				}
				break;
			case ModuleType.Effects:
				if (Effects != null)
				{
					Debug.Log("Effects module already attached!");
					break;
				}
				Effects = ScriptableObject.CreateInstance<EnviroEffectsModule>();
				Effects.name = "Effects Module";
				Effects.preset = (EnviroEffectsModule)EnviroHelper.GetDefaultPreset("Default Effects Preset");
				Effects.LoadModuleValues();
				Effects.Enable();
				if (configuration != null && !Application.isPlaying)
				{
					configuration.Effects = Effects;
				}
				break;
			case ModuleType.Lightning:
				if (Lightning != null)
				{
					Debug.Log("Lighting module already attached!");
					break;
				}
				Lightning = ScriptableObject.CreateInstance<EnviroLightningModule>();
				Lightning.name = "Lightning Module";
				Lightning.preset = (EnviroLightningModule)EnviroHelper.GetDefaultPreset("Default Lightning Preset");
				Lightning.LoadModuleValues();
				Lightning.Enable();
				if (configuration != null && !Application.isPlaying)
				{
					configuration.Lightning = Lightning;
				}
				break;
			case ModuleType.Quality:
				if (Quality != null)
				{
					Debug.Log("Quality module already attached!");
					break;
				}
				Quality = ScriptableObject.CreateInstance<EnviroQualityModule>();
				Quality.name = "Quality Module";
				Quality.preset = (EnviroQualityModule)EnviroHelper.GetDefaultPreset("Default Quality Module Preset");
				Quality.LoadModuleValues();
				Quality.Enable();
				if (configuration != null && !Application.isPlaying)
				{
					configuration.Quality = Quality;
				}
				break;
			}
		}

		public void RemoveModule(ModuleType type)
		{
			switch (type)
			{
			case ModuleType.Time:
				if (Time != null)
				{
					Time.Disable();
					Object.DestroyImmediate(Time, allowDestroyingAssets: true);
					_ = Application.isPlaying;
				}
				else
				{
					Debug.Log("No time module attached!");
				}
				break;
			case ModuleType.Sky:
				if (Sky != null)
				{
					Sky.Disable();
					Object.DestroyImmediate(Sky, allowDestroyingAssets: true);
					if (!Application.isPlaying)
					{
						Object.DestroyImmediate(configuration.Sky, allowDestroyingAssets: true);
					}
				}
				else
				{
					Debug.Log("No sky module attached!");
				}
				break;
			case ModuleType.Lighting:
				if (Lighting != null)
				{
					Lighting.Disable();
					Object.DestroyImmediate(Lighting, allowDestroyingAssets: true);
					if (!Application.isPlaying && configuration != null)
					{
						Object.DestroyImmediate(configuration.lightingModule, allowDestroyingAssets: true);
					}
				}
				else
				{
					Debug.Log("No lighting module attached!");
				}
				break;
			case ModuleType.Reflections:
				if (Reflections != null)
				{
					Reflections.Disable();
					Object.DestroyImmediate(Reflections, allowDestroyingAssets: true);
					if (!Application.isPlaying && configuration != null)
					{
						Object.DestroyImmediate(configuration.reflectionsModule, allowDestroyingAssets: true);
					}
				}
				else
				{
					Debug.Log("No reflections module attached!");
				}
				break;
			case ModuleType.Fog:
				if (Fog != null)
				{
					Fog.Disable();
					Object.DestroyImmediate(Fog, allowDestroyingAssets: true);
					if (!Application.isPlaying && configuration != null)
					{
						Object.DestroyImmediate(configuration.fogModule, allowDestroyingAssets: true);
					}
				}
				else
				{
					Debug.Log("No fog module attached!");
				}
				break;
			case ModuleType.VolumetricClouds:
				if (VolumetricClouds != null)
				{
					VolumetricClouds.Disable();
					Object.DestroyImmediate(VolumetricClouds, allowDestroyingAssets: true);
					if (!Application.isPlaying && configuration != null)
					{
						Object.DestroyImmediate(configuration.volumetricCloudModule, allowDestroyingAssets: true);
					}
				}
				else
				{
					Debug.Log("No volumetric cloud module attached!");
				}
				break;
			case ModuleType.FlatClouds:
				if (FlatClouds != null)
				{
					FlatClouds.Disable();
					Object.DestroyImmediate(FlatClouds, allowDestroyingAssets: true);
					if (!Application.isPlaying && configuration != null)
					{
						Object.DestroyImmediate(configuration.flatCloudModule, allowDestroyingAssets: true);
					}
				}
				else
				{
					Debug.Log("No flat cloud module attached!");
				}
				break;
			case ModuleType.Weather:
				if (Weather != null)
				{
					Weather.Disable();
					Object.DestroyImmediate(Weather, allowDestroyingAssets: true);
					if (!Application.isPlaying && configuration != null)
					{
						Object.DestroyImmediate(configuration.Weather, allowDestroyingAssets: true);
					}
				}
				else
				{
					Debug.Log("No weather module attached!");
				}
				break;
			case ModuleType.Aurora:
				if (Aurora != null)
				{
					Aurora.Disable();
					Object.DestroyImmediate(Aurora, allowDestroyingAssets: true);
					if (!Application.isPlaying && configuration != null)
					{
						Object.DestroyImmediate(configuration.Aurora, allowDestroyingAssets: true);
					}
				}
				else
				{
					Debug.Log("No aurora module attached!");
				}
				break;
			case ModuleType.Environment:
				if (Environment != null)
				{
					Environment.Disable();
					Object.DestroyImmediate(Environment, allowDestroyingAssets: true);
					if (!Application.isPlaying && configuration != null)
					{
						Object.DestroyImmediate(configuration.Environment, allowDestroyingAssets: true);
					}
				}
				else
				{
					Debug.Log("No environment module attached!");
				}
				break;
			case ModuleType.Audio:
				if (Audio != null)
				{
					Audio.Disable();
					Object.DestroyImmediate(Audio, allowDestroyingAssets: true);
					if (!Application.isPlaying && configuration != null)
					{
						Object.DestroyImmediate(configuration.Audio, allowDestroyingAssets: true);
					}
				}
				else
				{
					Debug.Log("No audio module attached!");
				}
				break;
			case ModuleType.Effects:
				if (Effects != null)
				{
					Effects.Disable();
					Object.DestroyImmediate(Effects, allowDestroyingAssets: true);
					if (!Application.isPlaying && configuration != null)
					{
						Object.DestroyImmediate(configuration.Effects, allowDestroyingAssets: true);
					}
				}
				else
				{
					Debug.Log("No effects module attached!");
				}
				break;
			case ModuleType.Lightning:
				if (Lightning != null)
				{
					Lightning.Disable();
					Object.DestroyImmediate(Lightning, allowDestroyingAssets: true);
					if (!Application.isPlaying && configuration != null)
					{
						Object.DestroyImmediate(configuration.Lightning, allowDestroyingAssets: true);
					}
				}
				else
				{
					Debug.Log("No lightning module attached!");
				}
				break;
			case ModuleType.Quality:
				if (Quality != null)
				{
					Quality.Disable();
					Object.DestroyImmediate(Quality, allowDestroyingAssets: true);
					if (!Application.isPlaying && configuration != null)
					{
						Object.DestroyImmediate(configuration.Quality, allowDestroyingAssets: true);
					}
				}
				else
				{
					Debug.Log("No quality module attached!");
				}
				break;
			}
		}

		public void UpdateConfiguration(string fromVersion)
		{
			EnviroConfiguration config = EnviroHelper.GetConfig(defaultConfig);
			if (config != null)
			{
				if (!(fromVersion != version))
				{
					return;
				}
				if (fromVersion == "")
				{
					if (configuration.volumetricCloudModule != null)
					{
						configuration.volumetricCloudModule.settingsGlobal.noise = config.volumetricCloudModule.settingsGlobal.noise;
						configuration.volumetricCloudModule.settingsGlobal.detailNoise = config.volumetricCloudModule.settingsGlobal.detailNoise;
						configuration.volumetricCloudModule.settingsGlobal.curlTex = config.volumetricCloudModule.settingsGlobal.curlTex;
						configuration.volumetricCloudModule.settingsGlobal.bottomsOffsetNoise = config.volumetricCloudModule.settingsGlobal.bottomsOffsetNoise;
						configuration.volumetricCloudModule.settingsGlobal.atmosphereColorSaturateDistance = config.volumetricCloudModule.settingsGlobal.atmosphereColorSaturateDistance;
						configuration.volumetricCloudModule.settingsVolume.worleyFreq1 = config.volumetricCloudModule.settingsVolume.worleyFreq1;
						configuration.volumetricCloudModule.settingsVolume.worleyFreq2 = config.volumetricCloudModule.settingsVolume.worleyFreq2;
						configuration.volumetricCloudModule.settingsVolume.baseNoiseUV = config.volumetricCloudModule.settingsVolume.baseNoiseUV;
						configuration.volumetricCloudModule.settingsVolume.detailNoiseUV = config.volumetricCloudModule.settingsVolume.detailNoiseUV;
						configuration.volumetricCloudModule.settingsVolume.windSpeedModifier = config.volumetricCloudModule.settingsVolume.windSpeedModifier;
						configuration.volumetricCloudModule.settingsVolume.windUpwards = config.volumetricCloudModule.settingsVolume.windUpwards;
						configuration.volumetricCloudModule.settingsGlobal.atmosphereColorSaturateDistance = config.volumetricCloudModule.settingsGlobal.atmosphereColorSaturateDistance;
						configuration.volumetricCloudModule.settingsGlobal.curlTex = config.volumetricCloudModule.settingsGlobal.curlTex;
						configuration.volumetricCloudModule.settingsVolume.baseNoiseUV = config.volumetricCloudModule.settingsVolume.baseNoiseUV;
						configuration.volumetricCloudModule.settingsVolume.detailNoiseUV = config.volumetricCloudModule.settingsVolume.detailNoiseUV;
						configuration.lightingModule.Settings.sceneExposure = config.lightingModule.Settings.sceneExposure;
						configuration.Sky.Settings.skyExposureHDRP = config.Sky.Settings.skyExposureHDRP;
					}
				}
				else if (fromVersion == "3.3.0" && configuration.volumetricCloudModule != null)
				{
					configuration.volumetricCloudModule.settingsGlobal.atmosphereColorSaturateDistance = config.volumetricCloudModule.settingsGlobal.atmosphereColorSaturateDistance;
					configuration.volumetricCloudModule.settingsGlobal.curlTex = config.volumetricCloudModule.settingsGlobal.curlTex;
					configuration.volumetricCloudModule.settingsVolume.baseNoiseUV = config.volumetricCloudModule.settingsVolume.baseNoiseUV;
					configuration.volumetricCloudModule.settingsVolume.detailNoiseUV = config.volumetricCloudModule.settingsVolume.detailNoiseUV;
				}
				configuration.version = version;
			}
			else
			{
				Debug.LogError("Could not find default config asset: " + defaultConfig);
			}
		}
	}
}
