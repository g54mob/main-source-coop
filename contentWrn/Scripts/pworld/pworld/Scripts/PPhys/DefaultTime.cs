using UnityEngine;

namespace pworld.Scripts.PPhys
{
	public class DefaultTime : ITimeSource
	{
		private static float previousTimeScale = 1f;

		public float TimeScale
		{
			get
			{
				return Time.timeScale;
			}
			set
			{
				previousTimeScale = Time.timeScale;
				Time.timeScale = value;
			}
		}

		public float DeltaTime => Time.deltaTime;

		public void GoToPreviousTimeScale()
		{
			TimeScale = previousTimeScale;
		}
	}
}
