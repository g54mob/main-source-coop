using UnityEngine;

namespace Enviro
{
	public class EventTest : MonoBehaviour
	{
		private void Start()
		{
			EnviroManager.instance.OnHourPassed += delegate
			{
				Debug.Log("Hour Passed!");
			};
			EnviroManager.instance.OnDayPassed += delegate
			{
				Debug.Log("New Day!");
			};
			EnviroManager.instance.OnYearPassed += delegate
			{
				Debug.Log("New Year!");
			};
			EnviroManager.instance.OnDayTime += delegate
			{
				Debug.Log("Day!");
			};
			EnviroManager.instance.OnNightTime += delegate
			{
				Debug.Log("Night!");
			};
			EnviroManager.instance.OnSeasonChanged += delegate(EnviroEnvironment.Seasons s)
			{
				Debug.Log("Season changed to: " + s);
			};
			EnviroManager.instance.OnWeatherChanged += delegate(EnviroWeatherType w)
			{
				Debug.Log("Weather changed to: " + w.name + " from:" + EnviroManager.instance.Weather.targetWeatherType.name);
			};
			EnviroManager.instance.OnZoneWeatherChanged += delegate(EnviroWeatherType w, EnviroZone z)
			{
				Debug.Log("Weather changed to: " + w.name.ToString() + " in zone:" + z.name);
			};
		}
	}
}
