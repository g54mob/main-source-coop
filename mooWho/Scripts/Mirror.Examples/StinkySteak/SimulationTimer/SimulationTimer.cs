using UnityEngine;

namespace StinkySteak.SimulationTimer
{
	public struct SimulationTimer
	{
		private float _targetTime;

		public static SimulationTimer None => default(SimulationTimer);

		public float TargetTime => _targetTime;

		public bool IsRunning => _targetTime > 0f;

		public float RemainingSeconds => Mathf.Max(_targetTime - Time.time, 0f);

		public static SimulationTimer CreateFromSeconds(float duration)
		{
			return new SimulationTimer
			{
				_targetTime = duration + Time.time
			};
		}

		public bool IsExpired()
		{
			if (Time.time >= _targetTime)
			{
				return IsRunning;
			}
			return false;
		}

		public bool IsExpiredOrNotRunning()
		{
			return Time.time >= _targetTime;
		}
	}
}
