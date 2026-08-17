using System;
using PrimeTween;
using UnityEngine;

namespace NomadDrive.Features.Interaction
{
	[Serializable]
	public class AnimationEaseConfig
	{
		[SerializeField]
		private AnimationEaseMode mode;

		[SerializeField]
		private Ease preset = Ease.OutBounce;

		[SerializeField]
		private AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		public AnimationEaseMode Mode => mode;

		public Ease Preset => preset;

		public AnimationCurve Curve => curve;

		public Tween CreateLocalPositionTween(Transform target, Vector3 endValue, float duration)
		{
			if (mode == AnimationEaseMode.CustomCurve && curve != null && curve.length > 0)
			{
				return Tween.LocalPosition(target, endValue, duration, curve);
			}
			return Tween.LocalPosition(target, endValue, duration, preset);
		}

		public Tween CreateLocalRotationTween(Transform target, Quaternion endValue, float duration)
		{
			if (mode == AnimationEaseMode.CustomCurve && curve != null && curve.length > 0)
			{
				return Tween.LocalRotation(target, endValue, duration, curve);
			}
			return Tween.LocalRotation(target, endValue, duration, preset);
		}
	}
}
