using UnityEngine;
using UnityEngine.Events;

public class Countdown
{
	private float _time;

	private float _timeAux;

	private bool _running;

	private int _id;

	public UnityEvent Ended;

	public UnityEvent Restarted;

	public int ID => _id;

	public bool IsRunning => _running;

	public float CurrentTime => _timeAux;

	public Countdown(float value, bool running)
	{
		_time = value;
		_timeAux = _time;
		_running = running;
		_id = (int)(Time.realtimeSinceStartup * 10000f);
		Ended = new UnityEvent();
		Restarted = new UnityEvent();
	}

	public void StartTimer()
	{
		_running = true;
	}

	public void StopTimer()
	{
		_running = false;
	}

	public static Countdown operator -(Countdown c, float t)
	{
		if (c._timeAux > 0f)
		{
			c._timeAux -= t;
		}
		if (c._timeAux < 0f)
		{
			c._timeAux = 0f;
		}
		return c;
	}

	public static Countdown operator +(Countdown c, float t)
	{
		if (c._timeAux < c._time)
		{
			c._timeAux += t;
		}
		if (c._timeAux > c._time)
		{
			c._timeAux = c._time;
		}
		return c;
	}

	public static bool operator !(Countdown c)
	{
		if (c != null)
		{
			return true;
		}
		return false;
	}

	public void RegenerateID()
	{
		_id = Random.Range(1, 50000);
	}

	public void Reset()
	{
		_running = false;
		_timeAux = _time;
		Restarted?.Invoke();
	}
}
