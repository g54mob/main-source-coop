using PrimeTween;
using UnityEngine;

namespace EvilCore.UI.Scripts
{
	public class UIHoverSlide : UIPointerEffect
	{
		[Header("Slide")]
		[SerializeField]
		private RectTransform target;

		[SerializeField]
		private Vector2 offset = new Vector2(10f, 0f);

		private Vector2 _basePosition;

		protected override void CacheState()
		{
			if (target == null)
			{
				target = base.transform as RectTransform;
			}
			if (target != null)
			{
				_basePosition = target.anchoredPosition;
			}
		}

		protected override void OnHoverBegin()
		{
			if (target != null)
			{
				Tween.UIAnchoredPosition(target, _basePosition + offset, duration, enterEase, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			}
		}

		protected override void OnHoverEnd()
		{
			if (target != null)
			{
				Tween.UIAnchoredPosition(target, _basePosition, duration, exitEase, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			}
		}

		protected override void ResetImmediate()
		{
			if (!(target == null))
			{
				Tween.CompleteAll(target);
				target.anchoredPosition = _basePosition;
			}
		}
	}
}
