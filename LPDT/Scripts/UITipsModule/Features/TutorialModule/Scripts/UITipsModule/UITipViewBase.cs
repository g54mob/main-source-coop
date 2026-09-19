using System;
using System.Collections;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.TutorialModule.Scripts.UITipsModule
{
	public abstract class UITipViewBase : ViewBehaviour
	{
		[SerializeField]
		private CanvasGroup _containerCanvasGroup;

		[SerializeField]
		private RectTransform _containerRect;

		[SerializeField]
		private AnimationCurve _showCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[SerializeField]
		private AnimationCurve _hideCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

		[SerializeField]
		private float _animationDuration = 0.25f;

		private Coroutine _animationCoroutine;

		public RectTransform RectTransform => (RectTransform)base.transform;

		public void SetParent(Transform parent)
		{
			base.transform.SetParent(parent, worldPositionStays: false);
		}

		public void SetAnchoredPosition(Vector2 anchoredPosition)
		{
			RectTransform.anchoredPosition = anchoredPosition;
		}

		public void SetVisibleInstant(bool visible)
		{
			StopAnimation();
			Apply(visible ? 1f : 0f);
		}

		public void Appear(Action onComplete = null)
		{
			PlayAnimation(_showCurve, onComplete);
		}

		public void Disappear(Action onComplete = null)
		{
			PlayAnimation(_hideCurve, onComplete);
		}

		private void PlayAnimation(AnimationCurve curve, Action onComplete)
		{
			StopAnimation();
			_animationCoroutine = StartCoroutine(AnimateRoutine(curve, onComplete));
		}

		private void StopAnimation()
		{
			if (_animationCoroutine != null)
			{
				StopCoroutine(_animationCoroutine);
				_animationCoroutine = null;
			}
		}

		private IEnumerator AnimateRoutine(AnimationCurve curve, Action onComplete)
		{
			float elapsed = 0f;
			Apply(curve.Evaluate(0f));
			while (elapsed < _animationDuration)
			{
				elapsed += Time.unscaledDeltaTime;
				float time = ((_animationDuration <= 0f) ? 1f : Mathf.Clamp01(elapsed / _animationDuration));
				Apply(curve.Evaluate(time));
				yield return null;
			}
			Apply(curve.Evaluate(1f));
			_animationCoroutine = null;
			onComplete?.Invoke();
		}

		private void Apply(float value)
		{
			if (_containerCanvasGroup != null)
			{
				_containerCanvasGroup.alpha = value;
			}
			if (_containerRect != null)
			{
				_containerRect.localScale = Vector3.one * value;
			}
		}
	}
}
