using UnityEngine;

namespace Features.UIAnimationsModule.Scripts
{
	public class UIAnimationEventData
	{
		public RectTransform Target { get; }

		public UIAnimationKey Key { get; }

		public UIAnimationEventReason Reason { get; }

		public UIAnimationEventData(RectTransform target, UIAnimationKey key, UIAnimationEventReason reason)
		{
			Target = target;
			Key = key;
			Reason = reason;
		}
	}
}
