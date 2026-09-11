using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zorro.Core;

public class TimeOfDayToggler : Singleton<TimeOfDayToggler>, IFindIfNull
{
	public List<ITimeOfDayListener> listeners = new List<ITimeOfDayListener>();

	public static void AddListener(ITimeOfDayListener listener)
	{
		if (Singleton<TimeOfDayToggler>.Instance == null)
		{
			if (!SceneManager.GetActiveScene().name.Contains("MainMenu"))
			{
				Debug.LogError("TimeOfDayToggler is null");
			}
		}
		else
		{
			Singleton<TimeOfDayToggler>.Instance.listeners.Add(listener);
			Debug.Log("Listener added: " + listener.GetType().Name);
		}
	}

	private void Start()
	{
		UpdateListeners();
	}

	public void UpdateListeners()
	{
		foreach (ITimeOfDayListener listener in listeners)
		{
			listener.DayTimeChanged(TimeOfDayHandler.TimeOfDay);
		}
	}
}
