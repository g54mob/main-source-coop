using PrimeTween;
using UnityEngine;

namespace EvilCore.UI.Scripts
{
	public class UIHoverRotate : UIPointerEffect
	{
		[Header("Rotate")]
		[SerializeField]
		private float tiltAngle = 8f;

		private Quaternion _baseRotation;

		protected override void CacheState()
		{
			_baseRotation = base.transform.localRotation;
		}

		protected override void OnHoverBegin()
		{
			Tween.LocalRotation(base.transform, _baseRotation * Quaternion.Euler(0f, 0f, tiltAngle), duration, enterEase, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
		}

		protected override void OnHoverEnd()
		{
			Tween.LocalRotation(base.transform, _baseRotation, duration, exitEase, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
		}

		protected override void ResetImmediate()
		{
			Tween.CompleteAll(base.transform);
			base.transform.localRotation = _baseRotation;
		}
	}
}
