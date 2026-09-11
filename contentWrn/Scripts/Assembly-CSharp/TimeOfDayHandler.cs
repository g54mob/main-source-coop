using UnityEngine;
using Zorro.Core;

public class TimeOfDayHandler : RetrievableSingleton<TimeOfDayHandler>
{
	public TimeOfDay timeOfDay;

	public static TimeOfDay TimeOfDay => RetrievableSingleton<TimeOfDayHandler>.Instance.timeOfDay;

	protected override void OnCreated()
	{
		base.OnCreated();
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public static void SetTimeOfDay(TimeOfDay timeOfDay)
	{
		RetrievableSingleton<TimeOfDayHandler>.Instance.timeOfDay = timeOfDay;
		Debug.Log("Time of day set to " + timeOfDay);
		if (Singleton<TimeOfDayToggler>.Instance != null)
		{
			Singleton<TimeOfDayToggler>.Instance.UpdateListeners();
		}
	}
}
