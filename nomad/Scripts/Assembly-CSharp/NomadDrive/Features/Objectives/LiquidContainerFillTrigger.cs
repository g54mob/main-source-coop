using System;
using UnityEngine;

namespace NomadDrive.Features.Objectives
{
	[Serializable]
	public class LiquidContainerFillTrigger : ObjectiveTrigger
	{
		[SerializeField]
		[Tooltip("Signal key the target container raises. The producer (e.g. LiquidContainerComponent with objectivesSignalKey) must call ObjectivesEventBus.RaiseRatio / RaiseAmount with the same key whenever its fill changes.")]
		private string signalKey;

		[SerializeField]
		[Min(0f)]
		[Tooltip("Target amount in absolute units (e.g. liters). When > 0, the trigger uses the absolute amount channel: progress = min(currentAmount / targetAmount, 1) and the step completes once currentAmount >= targetAmount, regardless of container capacity. When 0, the trigger falls back to the legacy ratio channel and completes at FillRatio >= 1 (i.e. container fully full).")]
		private float targetAmount;

		protected override void OnActivate()
		{
			if (targetAmount > 0f)
			{
				ObjectivesEventBus.AmountRaised += HandleAmount;
				if (ObjectivesEventBus.TryGetLastAmount(signalKey, out var amount))
				{
					HandleAmount(signalKey, amount);
				}
			}
			else
			{
				ObjectivesEventBus.RatioRaised += HandleRatio;
				if (ObjectivesEventBus.TryGetLastRatio(signalKey, out var ratio))
				{
					HandleRatio(signalKey, ratio);
				}
			}
		}

		protected override void OnDeactivate()
		{
			ObjectivesEventBus.AmountRaised -= HandleAmount;
			ObjectivesEventBus.RatioRaised -= HandleRatio;
		}

		private void HandleRatio(string key, float ratio)
		{
			if (!string.IsNullOrEmpty(signalKey) && !(key != signalKey))
			{
				Report?.Invoke(ratio);
				if (ratio >= 1f)
				{
					Fire?.Invoke();
				}
			}
		}

		private void HandleAmount(string key, float amount)
		{
			if (!string.IsNullOrEmpty(signalKey) && !(key != signalKey))
			{
				float obj = Mathf.Clamp01(amount / targetAmount);
				Report?.Invoke(obj);
				if (amount >= targetAmount)
				{
					Fire?.Invoke();
				}
			}
		}
	}
}
