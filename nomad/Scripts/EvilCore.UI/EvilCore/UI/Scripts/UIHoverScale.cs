using PrimeTween;
using UnityEngine;

namespace EvilCore.UI.Scripts
{
	public class UIHoverScale : UIPointerEffect
	{
		[Header("Scale")]
		[SerializeField]
		private float scaleMultiplier = 1.1f;

		private Vector3 _defaultScale;

		protected override void CacheState()
		{
			_defaultScale = base.transform.localScale;
		}

		protected override void OnHoverBegin()
		{
			Tween.Scale(base.transform, _defaultScale * scaleMultiplier, duration, enterEase, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
		}

		protected override void OnHoverEnd()
		{
			Tween.Scale(base.transform, _defaultScale, duration, exitEase, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
		}

		protected override void ResetImmediate()
		{
			Tween.CompleteAll(base.transform);
			base.transform.localScale = _defaultScale;
		}
	}
}
