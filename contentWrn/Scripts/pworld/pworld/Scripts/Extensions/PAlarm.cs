using System;
using System.Collections;
using UnityEngine;

namespace pworld.Scripts.Extensions
{
	public class PAlarm
	{
		private readonly MonoBehaviour host;

		private float counter;

		public Action end;

		public Action start;

		private Coroutine update;

		public bool IsSet { get; private set; }

		public PAlarm(MonoBehaviour host_, Action start_, Action end_)
		{
			start = start_;
			end = end_;
			host = host_;
			IsSet = false;
		}

		public void Set(float time)
		{
			if (!IsSet)
			{
				start?.Invoke();
				IsSet = true;
				counter = time;
				update = host.StartCoroutine(Update());
			}
			else
			{
				counter = Mathf.Max(counter, time);
			}
		}

		private IEnumerator Update()
		{
			while (IsSet)
			{
				Tick();
				yield return null;
			}
		}

		private void Tick()
		{
			if (counter <= 0f)
			{
				IsSet = false;
				counter = 0f;
				end?.Invoke();
			}
			else
			{
				counter -= Time.deltaTime;
			}
		}
	}
}
