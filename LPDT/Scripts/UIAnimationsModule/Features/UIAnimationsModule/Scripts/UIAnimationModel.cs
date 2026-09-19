using System;
using UnityEngine;

namespace Features.UIAnimationsModule.Scripts
{
	public class UIAnimationModel
	{
		public int ActiveAnimationsCount { get; private set; }

		public bool IsAnyAnimationActive => ActiveAnimationsCount > 0;

		public event Action<UIAnimationEventData> OnAnimationStarted;

		public event Action<UIAnimationEventData> OnAnimationEnded;

		public void InvokeStarted(UIAnimationEventData data)
		{
			ActiveAnimationsCount++;
			this.OnAnimationStarted?.Invoke(data);
		}

		public void InvokeEnded(UIAnimationEventData data)
		{
			ActiveAnimationsCount = Mathf.Max(0, ActiveAnimationsCount - 1);
			this.OnAnimationEnded?.Invoke(data);
		}
	}
}
