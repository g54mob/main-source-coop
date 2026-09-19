using UnityEngine;

namespace StinkySteak.SimulationTimer
{
	public struct PauseableSimulationTimer
	{
		private float _targetTime;

		private bool _isPaused;

		private float _pauseAtTime;

		public static PauseableSimulationTimer None => default(PauseableSimulationTimer);

		public float TargetTime => GetTargetTime();

		public bool IsPaused => _isPaused;

		public bool IsRunning => _targetTime > 0f;

		public float RemainingSeconds => Mathf.Max(TargetTime - Time.time, 0f);

		private float GetTargetTime()
		{
			if (!_isPaused)
			{
				return _targetTime;
			}
			return _targetTime + Time.time - _pauseAtTime;
		}

		public static PauseableSimulationTimer CreateFromSeconds(float duration)
		{
			return new PauseableSimulationTimer
			{
				_targetTime = duration + Time.time
			};
		}

		public void Pause()
		{
			if (!_isPaused)
			{
				_isPaused = true;
				_pauseAtTime = Time.time;
			}
		}

		public void Resume()
		{
			if (_isPaused)
			{
				_targetTime = GetTargetTime();
				_isPaused = false;
				_pauseAtTime = 0f;
			}
		}

		public bool IsExpired()
		{
			if (Time.time >= TargetTime)
			{
				return IsRunning;
			}
			return false;
		}

		public bool IsExpiredOrNotRunning()
		{
			return Time.time >= TargetTime;
		}
	}
}
