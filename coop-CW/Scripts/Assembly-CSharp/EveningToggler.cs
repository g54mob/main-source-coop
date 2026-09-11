using UnityEngine;

public class EveningToggler : MonoBehaviour, ITimeOfDayListener
{
	public GameObject evening;

	public GameObject day;

	private void Awake()
	{
		TimeOfDayToggler.AddListener(this);
	}

	public void DayTimeChanged(TimeOfDay timeOfDay)
	{
		evening.SetActive(value: false);
		day.SetActive(value: false);
		if (timeOfDay == TimeOfDay.Evening)
		{
			evening.SetActive(value: true);
			evening.GetComponent<SetTheme>().Start();
		}
		else
		{
			day.SetActive(value: true);
			day.GetComponent<SetTheme>().Start();
		}
	}
}
