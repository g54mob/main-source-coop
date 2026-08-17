using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroEnvironmentModule : EnviroModule
	{
		public EnviroEnvironment Settings;

		public EnviroEnvironmentModule preset;

		public bool showSeasonControls;

		public bool showTemperatureControls;

		public bool showWeatherStateControls;

		public bool showWindControls;

		public override void Enable()
		{
			if (!(EnviroManager.instance == null))
			{
				CreateWindZone();
			}
		}

		public override void Disable()
		{
			if (!(EnviroManager.instance == null) && EnviroManager.instance.Objects.windZone != null)
			{
				UnityEngine.Object.DestroyImmediate(EnviroManager.instance.Objects.windZone.gameObject);
			}
		}

		private void CreateWindZone()
		{
			if (EnviroManager.instance.Objects.windZone == null)
			{
				GameObject gameObject = new GameObject();
				gameObject.name = "Wind Zone";
				gameObject.transform.SetParent(EnviroManager.instance.transform);
				gameObject.transform.localPosition = Vector3.zero;
				EnviroManager.instance.Objects.windZone = gameObject.AddComponent<WindZone>();
			}
		}

		public override void UpdateModule()
		{
			if (active && !(EnviroManager.instance == null))
			{
				if (EnviroManager.instance.Time != null)
				{
					UpdateTemperature(EnviroManager.instance.Time.GetUniversalTimeOfDay() / 24f);
					UpdateSeason();
				}
				else
				{
					UpdateTemperature(1f);
				}
				UpdateWindZone();
				UpdateWeatherState();
			}
		}

		public void UpdateSeason()
		{
			if (Settings.changeSeason)
			{
				int num = EnviroManager.instance.Time.Settings.date.DayOfYear;
				if (EnviroManager.instance.Time.Settings.calenderType == EnviroTime.CalenderType.Custom)
				{
					num = EnviroManager.instance.Time.days + (EnviroManager.instance.Time.months - 1) * EnviroManager.instance.Time.Settings.daysInMonth;
				}
				if (num >= Settings.springStart && num <= Settings.springEnd)
				{
					ChangeSeason(EnviroEnvironment.Seasons.Spring);
				}
				else if (num >= Settings.summerStart && num <= Settings.summerEnd)
				{
					ChangeSeason(EnviroEnvironment.Seasons.Summer);
				}
				else if (num >= Settings.autumnStart && num <= Settings.autumnEnd)
				{
					ChangeSeason(EnviroEnvironment.Seasons.Autumn);
				}
				else if (num >= Settings.winterStart || num <= Settings.winterEnd)
				{
					ChangeSeason(EnviroEnvironment.Seasons.Winter);
				}
			}
		}

		public void ChangeSeason(EnviroEnvironment.Seasons season)
		{
			if (Settings.season != season)
			{
				EnviroManager.instance.NotifySeasonChanged(season);
				Settings.season = season;
			}
		}

		public void UpdateTemperature(float timeOfDay)
		{
			float num = 0f;
			switch (Settings.season)
			{
			case EnviroEnvironment.Seasons.Spring:
				num = Settings.springBaseTemperature.Evaluate(timeOfDay);
				break;
			case EnviroEnvironment.Seasons.Summer:
				num = Settings.summerBaseTemperature.Evaluate(timeOfDay);
				break;
			case EnviroEnvironment.Seasons.Autumn:
				num = Settings.autumnBaseTemperature.Evaluate(timeOfDay);
				break;
			case EnviroEnvironment.Seasons.Winter:
				num = Settings.winterBaseTemperature.Evaluate(timeOfDay);
				break;
			}
			num += Settings.temperatureWeatherMod;
			num += Settings.temperatureCustomMod;
			Settings.temperature = Mathf.Lerp(Settings.temperature, num, Time.deltaTime * Settings.temperatureChangingSpeed);
		}

		public void UpdateWeatherState()
		{
			if (Settings.wetness < Settings.wetnessTarget)
			{
				Settings.wetness = Mathf.Lerp(Settings.wetness, Settings.wetnessTarget, Settings.wetnessAccumulationSpeed * Time.deltaTime);
			}
			else
			{
				Settings.wetness = Mathf.Lerp(Settings.wetness, Settings.wetnessTarget, Settings.wetnessDrySpeed * Time.deltaTime);
			}
			if (Settings.wetness < 0.0001f)
			{
				Settings.wetness = 0f;
			}
			Settings.wetness = Mathf.Clamp(Settings.wetness, 0f, 1f);
			if (Settings.snow < Settings.snowTarget)
			{
				Settings.snow = Mathf.Lerp(Settings.snow, Settings.snowTarget, Settings.snowAccumulationSpeed * Time.deltaTime);
			}
			else if (Settings.temperature > Settings.snowMeltingTresholdTemperature)
			{
				Settings.snow = Mathf.Lerp(Settings.snow, Settings.snowTarget, Settings.snowMeltSpeed * Time.deltaTime);
			}
			if (Settings.snow < 0.0001f)
			{
				Settings.snow = 0f;
			}
			Settings.snow = Mathf.Clamp(Settings.snow, 0f, 1f);
		}

		private void UpdateWindZone()
		{
			if (EnviroManager.instance.Objects.windZone != null)
			{
				EnviroManager.instance.Objects.windZone.windMain = Settings.windSpeed;
				EnviroManager.instance.Objects.windZone.windTurbulence = Settings.windTurbulence;
				Vector3 forward = new Vector3(0f - Settings.windDirectionX, 0f, 0f - Settings.windDirectionY);
				EnviroManager.instance.Objects.windZone.transform.forward = forward;
			}
		}

		public void LoadModuleValues()
		{
			if (preset != null)
			{
				Settings = JsonUtility.FromJson<EnviroEnvironment>(JsonUtility.ToJson(preset.Settings));
			}
			else
			{
				Debug.Log("Please assign a saved module to load from!");
			}
		}

		public void SaveModuleValues()
		{
		}

		public void SaveModuleValues(EnviroEnvironmentModule module)
		{
			module.Settings = JsonUtility.FromJson<EnviroEnvironment>(JsonUtility.ToJson(Settings));
		}
	}
}
