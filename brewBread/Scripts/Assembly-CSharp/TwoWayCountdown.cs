using System.Collections.Generic;
using UnityEngine;

public class TwoWayCountdown : StaticInstance<TwoWayCountdown>
{
	private List<Countdown> _countdowns = new List<Countdown>();

	public Countdown StartNewTimer(float time)
	{
		Countdown countdown = new Countdown(time, running: true);
		_countdowns.Add(countdown);
		return countdown;
	}

	public void StartTimer(Countdown countdown)
	{
		_countdowns.Find((Countdown x) => x.ID == countdown?.ID)?.StartTimer();
	}

	public void RestartTimer(Countdown countdown)
	{
		_countdowns.Find((Countdown x) => x.ID == countdown?.ID)?.Reset();
	}

	public void StopTimer(Countdown countdown)
	{
		_countdowns.Find((Countdown x) => x.ID == countdown?.ID)?.StopTimer();
	}

	private void Update()
	{
		for (int i = 0; i < _countdowns.Count; i++)
		{
			if (_countdowns[i].IsRunning)
			{
				_countdowns[i] -= Time.deltaTime;
			}
			else
			{
				_countdowns[i] += Time.deltaTime;
			}
			if (_countdowns[i].CurrentTime <= 0f)
			{
				_countdowns[i].Ended?.Invoke();
			}
		}
	}
}
