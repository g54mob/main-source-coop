using System;
using DG.Tweening;
using UnityEngine;

namespace Features.UIAnimationsModule.Scripts
{
	[Serializable]
	public class UIAnimationPreset
	{
		[SerializeField]
		private float _duration = 0.25f;

		[SerializeField]
		private float _delay;

		[SerializeField]
		private Ease _ease = Ease.OutCubic;

		[SerializeField]
		private AnimationCurve _customCurve;

		[SerializeField]
		private int _loops;

		[SerializeField]
		private LoopType _loopType;

		[SerializeField]
		private bool _useUnscaledTime = true;

		[Header("Alpha (requires CanvasGroup on target; component is added automatically if missing)")]
		[SerializeField]
		private bool _animateAlpha;

		[SerializeField]
		private float _alphaFrom;

		[SerializeField]
		private float _alphaTo = 1f;

		[Header("Scale (localScale)")]
		[SerializeField]
		private bool _animateScale;

		[SerializeField]
		private Vector3 _scaleFrom = Vector3.one * 0.9f;

		[SerializeField]
		private Vector3 _scaleTo = Vector3.one;

		[Header("Anchored Position Offset (relative to current anchoredPosition)")]
		[SerializeField]
		private bool _animatePosition;

		[SerializeField]
		private Vector2 _positionFromOffset;

		[SerializeField]
		private Vector2 _positionToOffset;

		[Header("Rotation (local euler)")]
		[SerializeField]
		private bool _animateRotation;

		[SerializeField]
		private Vector3 _rotationFrom;

		[SerializeField]
		private Vector3 _rotationTo;

		[SerializeField]
		private RotateMode _rotateMode;

		public float Duration => _duration;

		public float Delay => _delay;

		public Ease Ease => _ease;

		public AnimationCurve CustomCurve => _customCurve;

		public int Loops => _loops;

		public LoopType LoopType => _loopType;

		public bool UseUnscaledTime => _useUnscaledTime;

		public bool AnimateAlpha => _animateAlpha;

		public float AlphaFrom => _alphaFrom;

		public float AlphaTo => _alphaTo;

		public bool AnimateScale => _animateScale;

		public Vector3 ScaleFrom => _scaleFrom;

		public Vector3 ScaleTo => _scaleTo;

		public bool AnimatePosition => _animatePosition;

		public Vector2 PositionFromOffset => _positionFromOffset;

		public Vector2 PositionToOffset => _positionToOffset;

		public bool AnimateRotation => _animateRotation;

		public Vector3 RotationFrom => _rotationFrom;

		public Vector3 RotationTo => _rotationTo;

		public RotateMode RotateMode => _rotateMode;
	}
}
