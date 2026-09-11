using System;
using UnityEngine;

namespace pworld.Scripts.Extensions
{
	[Serializable]
	public class PHourGlass
	{
		public float timeToFill;

		private float startTime;

		public bool Filled => startTime + timeToFill < Time.time;

		public PHourGlass(float timeToFill)
		{
			this.timeToFill = timeToFill;
		}

		public void Flip(float newTimeToFill = -1f)
		{
			if (newTimeToFill != -1f)
			{
				timeToFill = newTimeToFill;
			}
			startTime = Time.time;
		}
	}
}
