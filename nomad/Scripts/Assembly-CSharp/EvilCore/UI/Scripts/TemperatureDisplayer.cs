using NomadDrive.Managers.GameTime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EvilCore.UI.Scripts
{
	public class TemperatureDisplayer : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI temperatureText;

		[Header("Weather Icon")]
		[SerializeField]
		private Image weatherIcon;

		[SerializeField]
		private Sprite clearSprite;

		[SerializeField]
		private Sprite cloudySprite;

		[SerializeField]
		private Sprite rainySprite;

		[SerializeField]
		private Sprite stormSprite;

		[SerializeField]
		private Sprite snowySprite;

		private ITimeManager _timeManager;

		[Inject]
		public void Construct(ITimeManager timeManager)
		{
			_timeManager = timeManager;
		}

		private void Start()
		{
			_timeManager.OnMinutePassed.AddListener(DisplayTemperature);
			_timeManager.OnWeatherChanged += ApplyWeatherIcon;
			DisplayTemperature();
			ApplyWeatherIcon(_timeManager.CurrentWeather);
		}

		private void OnDisable()
		{
			_timeManager.OnMinutePassed.RemoveListener(DisplayTemperature);
			_timeManager.OnWeatherChanged -= ApplyWeatherIcon;
		}

		private void DisplayTemperature()
		{
			temperatureText.text = _timeManager.Temperature.ToString("0") + "°C";
		}

		private void ApplyWeatherIcon(WeatherType weather)
		{
			if (!(weatherIcon == null))
			{
				Sprite sprite = weather switch
				{
					WeatherType.Snowy => snowySprite, 
					WeatherType.Storm => stormSprite, 
					WeatherType.Rainy => rainySprite, 
					WeatherType.Cloudy => cloudySprite, 
					_ => clearSprite, 
				};
				if (sprite != null)
				{
					weatherIcon.sprite = sprite;
				}
			}
		}
	}
}
