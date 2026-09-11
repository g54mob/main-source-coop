using UnityEngine;

namespace pworld.Scripts.Extensions
{
	public class NullAfterTime<T> where T : class
	{
		private readonly float timeToNull = 0.5f;

		private bool paused;

		private float pausedStart;

		private T t;

		private float timePaused;

		private float timeWhenSet;

		public T Value
		{
			get
			{
				if (!paused && Time.time > timeWhenSet + timeToNull + timePaused)
				{
					t = null;
				}
				return t;
			}
			set
			{
				timeWhenSet = Time.time;
				t = value;
				Paused = false;
				timePaused = 0f;
				pausedStart = 0f;
			}
		}

		public bool Paused
		{
			get
			{
				return paused;
			}
			set
			{
				if (value != paused)
				{
					if (value)
					{
						timePaused = 0f;
						pausedStart = Time.time;
					}
					if (!value)
					{
						timePaused = Time.time - pausedStart;
					}
					paused = value;
				}
			}
		}

		public NullAfterTime(float timeToNull_ = 0.5f)
		{
			timeToNull = timeToNull_;
		}

		public void ResetTimer()
		{
			timeWhenSet = Time.time;
			timePaused = 0f;
		}
	}
}
