using System;
using UnityEngine;

namespace Features.StruggleBarModule.Scripts
{
	public class StruggleBarModel
	{
		public float CurrentFill { get; private set; }

		public bool IsActive { get; private set; }

		public event Action<float> OnFillChanged;

		public event Action<bool> OnActiveChanged;

		public event Action OnBoostApplied;

		public void Start(float startNormalized)
		{
			CurrentFill = Mathf.Clamp01(startNormalized);
			this.OnFillChanged?.Invoke(CurrentFill);
			SetActive(isActive: true);
		}

		public void SetFill(float fill)
		{
			CurrentFill = Mathf.Clamp01(fill);
			this.OnFillChanged?.Invoke(CurrentFill);
		}

		public void NotifyBoostApplied()
		{
			this.OnBoostApplied?.Invoke();
		}

		public void Stop()
		{
			CurrentFill = 0f;
			this.OnFillChanged?.Invoke(CurrentFill);
			SetActive(isActive: false);
		}

		private void SetActive(bool isActive)
		{
			if (IsActive != isActive)
			{
				IsActive = isActive;
				this.OnActiveChanged?.Invoke(IsActive);
			}
		}
	}
}
