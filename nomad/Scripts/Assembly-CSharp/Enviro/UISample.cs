using UnityEngine;
using UnityEngine.UI;

namespace Enviro
{
	public class UISample : MonoBehaviour
	{
		[Header("Time")]
		public Slider hourSlider;

		public Text hourText;

		public Text dateText;

		[Header("Weather")]
		public Text currentWeatherText;

		[Header("Environment")]
		public Text seasonText;

		public Text temperatureText;

		public Text wetnessText;

		public Text snowText;

		[Header("Quality")]
		public Text currentQualityText;

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void LateUpdate()
		{
			if (EnviroManager.instance.Time != null)
			{
				hourText.text = EnviroManager.instance.Time.GetTimeStringWithSeconds();
				dateText.text = $"{EnviroManager.instance.Time.days:00}/{EnviroManager.instance.Time.months:00}/{EnviroManager.instance.Time.years:0000}";
			}
			if (EnviroManager.instance.Weather != null)
			{
				currentWeatherText.text = "Current Weather: " + EnviroManager.instance.Weather.targetWeatherType.name;
			}
			if (EnviroManager.instance.Environment != null)
			{
				temperatureText.text = "Temperature: " + $"{EnviroManager.instance.Environment.Settings.temperature:0.0} °C";
				wetnessText.text = "Wetness: " + $"{EnviroManager.instance.Environment.Settings.wetness:0.00}";
				snowText.text = "Snow: " + $"{EnviroManager.instance.Environment.Settings.snow:0.00}";
				string text = "";
				switch (EnviroManager.instance.Environment.Settings.season)
				{
				case EnviroEnvironment.Seasons.Spring:
					text = "Current Season: Spring";
					break;
				case EnviroEnvironment.Seasons.Summer:
					text = "Current Season: Summer";
					break;
				case EnviroEnvironment.Seasons.Autumn:
					text = "Current Season: Autumn";
					break;
				case EnviroEnvironment.Seasons.Winter:
					text = "Current Season: Winter";
					break;
				}
				seasonText.text = text;
			}
			if (EnviroManager.instance.Quality != null)
			{
				currentQualityText.text = "Current Quality: " + EnviroManager.instance.Quality.Settings.defaultQuality.name;
			}
		}

		public void ChangeHourSlider()
		{
			if (!(EnviroManager.instance.Time == null))
			{
				if (hourSlider.value < 0f)
				{
					hourSlider.value = 0f;
				}
				EnviroManager.instance.Time.SetTimeOfDay(hourSlider.value * 24f);
			}
		}

		public void ChangeQuality(int q)
		{
			if (EnviroManager.instance.Quality != null && EnviroManager.instance.Quality.Settings.Qualities.Count >= q)
			{
				EnviroManager.instance.Quality.Settings.defaultQuality = EnviroManager.instance.Quality.Settings.Qualities[q];
			}
		}

		public void ChangeWeather(int w)
		{
			if (EnviroManager.instance.Weather != null && EnviroManager.instance.Weather.Settings.weatherTypes.Count >= w)
			{
				EnviroManager.instance.Weather.ChangeWeather(EnviroManager.instance.Weather.Settings.weatherTypes[w]);
			}
		}

		public void ChangeTimeSimulation(bool t)
		{
			if (EnviroManager.instance.Time != null)
			{
				EnviroManager.instance.Time.Settings.simulate = t;
			}
		}
	}
}
