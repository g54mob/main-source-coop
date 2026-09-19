using UnityEngine;

namespace Fusion.Statistics
{
	public struct StatAccumulator
	{
		public bool DisplayingPerSecond;

		private float _completedSecondSum;

		private float _currentSecondSum;

		private float _lastTimeStamp;

		public float Value { get; private set; }

		public float ValuePerSecond => _completedSecondSum;

		public float LastTimeStamp => _lastTimeStamp;

		public void Accumulate(float value)
		{
			Value = value;
			float time = Time.time;
			if (time - _lastTimeStamp >= 1f)
			{
				_completedSecondSum = _currentSecondSum;
				_currentSecondSum = value;
				_lastTimeStamp = time;
			}
			else
			{
				_currentSecondSum += value;
			}
		}
	}
}
