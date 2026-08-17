using System;
using UnityEngine.Events;

namespace NomadDrive.Managers.GameTime
{
	public interface ITimeManager
	{
		UnityEvent OnMinutePassed { get; }

		string CurrentTimeString { get; set; }

		float Temperature { get; set; }

		int CurrentHour { get; }

		bool IsRaining { get; }

		float RainIntensity { get; }

		WeatherType CurrentWeather { get; }

		event Action<WeatherType> OnWeatherChanged;

		void ForwardTimeSmoothly(DayTimePart dayTimePart, Action completeAction = null);

		void ForwardTimeSmoothly(DayTimePart dayTimePart, float speedMultiplier, Action completeAction = null);
	}
}
