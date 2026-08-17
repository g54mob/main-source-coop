using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Player
{
	[Serializable]
	public class PlayerStat
	{
		[SerializeField]
		private float currentValue;

		[SerializeField]
		private float maxValue;

		[SerializeField]
		private float consumeSpeedPerMinute;

		[SerializeField]
		private float criticalLowThreshold;

		[SerializeField]
		private float sleepingMultiplier;

		[SerializeField]
		private float consumingMultiplier = 1f;

		public readonly UnityEvent OnValueChanged = new UnityEvent();

		public readonly UnityEvent OnValueZero = new UnityEvent();

		public readonly UnityEvent OnValueMax = new UnityEvent();

		public readonly UnityEvent OnValueCriticalLow = new UnityEvent();

		public readonly UnityEvent OnValueExitCriticalLow = new UnityEvent();

		private bool _isCriticalForTween;

		public float CurrentValue
		{
			get
			{
				return currentValue;
			}
			set
			{
				currentValue = value;
				OnValueChanged?.Invoke();
			}
		}

		public float MaxValue => maxValue;

		public float CriticalLowThreshold => criticalLowThreshold;

		public PlayerStat(float currentValue, float maxValue, float consumeSpeedPerMinute, float criticalLowThreshold, float sleepingMultiplier)
		{
			CurrentValue = currentValue;
			this.maxValue = maxValue;
			this.consumeSpeedPerMinute = consumeSpeedPerMinute;
			this.criticalLowThreshold = criticalLowThreshold;
			this.sleepingMultiplier = sleepingMultiplier;
		}

		public void SetMax()
		{
			currentValue = maxValue;
			OnValueChanged?.Invoke();
			OnValueMax?.Invoke();
		}

		public void SetZero()
		{
			currentValue = 0f;
			OnValueChanged?.Invoke();
			OnValueZero?.Invoke();
		}

		public void ConsumeByMinute()
		{
			if (!(CurrentValue <= 0f))
			{
				float num = CurrentValue - consumeSpeedPerMinute * consumingMultiplier;
				if (CurrentValue > criticalLowThreshold && num <= criticalLowThreshold)
				{
					OnValueCriticalLow?.Invoke();
				}
				if (num <= 0f)
				{
					CurrentValue = 0f;
					OnValueZero?.Invoke();
				}
				else
				{
					CurrentValue = num;
				}
			}
		}

		public void SetSleepingConsumingMultiplier()
		{
			consumingMultiplier = sleepingMultiplier;
		}

		public void ResetConsumingMultiplier()
		{
			consumingMultiplier = 1f;
		}

		public void ResetValue(float value)
		{
			Tween.StopAll(this);
			_isCriticalForTween = false;
			consumingMultiplier = 1f;
			currentValue = Mathf.Clamp(value, 0f, maxValue);
			OnValueChanged?.Invoke();
		}

		public void IncreaseValue(float value)
		{
			float num = CurrentValue + value;
			if (CurrentValue <= criticalLowThreshold && num > criticalLowThreshold)
			{
				OnValueExitCriticalLow?.Invoke();
			}
			if (num >= maxValue)
			{
				CurrentValue = maxValue;
				OnValueMax?.Invoke();
			}
			else
			{
				CurrentValue = num;
			}
		}

		public void IncreaseValueSmoothly(float increaseAmount, float duration)
		{
			float num = CurrentValue + increaseAmount;
			if (num >= maxValue)
			{
				num = maxValue;
			}
			_isCriticalForTween = CurrentValue <= criticalLowThreshold;
			Tween.Custom(this, CurrentValue, num, duration, delegate(PlayerStat target, float val)
			{
				target.CurrentValue = val;
				if (target._isCriticalForTween && target.CurrentValue > target.criticalLowThreshold)
				{
					target.OnValueExitCriticalLow?.Invoke();
					target._isCriticalForTween = false;
				}
			});
		}

		public void DecreaseValue(float consumeAmount)
		{
			float num = CurrentValue - consumeAmount;
			if (CurrentValue > criticalLowThreshold && num <= criticalLowThreshold)
			{
				OnValueCriticalLow?.Invoke();
			}
			if (num <= 0f)
			{
				CurrentValue = 0f;
				OnValueZero?.Invoke();
			}
			else
			{
				CurrentValue = num;
			}
		}

		public float GetValueRatio()
		{
			return CurrentValue / maxValue;
		}
	}
}
