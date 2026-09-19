using System;

namespace Features.DeathHandlingModule.Scripts
{
	public class PreDeadTimerModel
	{
		public float CurrentTimer { get; private set; }

		public float MaxTimer { get; private set; }

		public bool IsTimerRunning { get; private set; }

		public event Action<float, float> OnTimerChanged;

		public event Action<bool> OnTimerRunningChanged;

		public void StartTimer(float duration)
		{
			MaxTimer = duration;
			CurrentTimer = duration;
			this.OnTimerChanged?.Invoke(CurrentTimer, MaxTimer);
			SetRunning(running: true);
		}

		public void SetCurrent(float current)
		{
			CurrentTimer = current;
			this.OnTimerChanged?.Invoke(CurrentTimer, MaxTimer);
		}

		public void StopTimer()
		{
			CurrentTimer = 0f;
			MaxTimer = 0f;
			this.OnTimerChanged?.Invoke(CurrentTimer, MaxTimer);
			SetRunning(running: false);
		}

		private void SetRunning(bool running)
		{
			if (IsTimerRunning != running)
			{
				IsTimerRunning = running;
				this.OnTimerRunningChanged?.Invoke(running);
			}
		}
	}
}
