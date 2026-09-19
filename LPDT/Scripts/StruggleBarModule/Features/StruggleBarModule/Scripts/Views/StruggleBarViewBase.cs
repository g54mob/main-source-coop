using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.StruggleBarModule.Scripts.Views
{
	public abstract class StruggleBarViewBase : ViewBehaviour
	{
		[SerializeField]
		private RectTransform _indicator;

		[SerializeField]
		private float _minX;

		[SerializeField]
		private float _maxX;

		[SerializeField]
		private CanvasGroup _canvasGroup;

		[SerializeField]
		private RectTransform _scaleTarget;

		[SerializeField]
		private float _scalePulseAmount = 0.2f;

		[SerializeField]
		private float _scaleFollowSpeed = 28f;

		[SerializeField]
		private float _scaleReturnSpeed = 10f;

		[SerializeField]
		private float _maxScaleMultiplier = 1.6f;

		private Vector3 _initialScale;

		private float _currentScaleMultiplier = 1f;

		private float _targetScaleMultiplier = 1f;

		private bool _hasCachedInitialScale;

		private void Awake()
		{
			CacheInitialScale();
		}

		private void Update()
		{
			if (!(_scaleTarget == null))
			{
				float deltaTime = Time.deltaTime;
				_targetScaleMultiplier = Mathf.Lerp(_targetScaleMultiplier, 1f, 1f - Mathf.Exp((0f - _scaleReturnSpeed) * deltaTime));
				_currentScaleMultiplier = Mathf.Lerp(_currentScaleMultiplier, _targetScaleMultiplier, 1f - Mathf.Exp((0f - _scaleFollowSpeed) * deltaTime));
				_scaleTarget.localScale = _initialScale * _currentScaleMultiplier;
			}
		}

		public virtual void SetNormalizedValue(float normalized)
		{
			if (!(_indicator == null))
			{
				float x = Mathf.Lerp(_minX, _maxX, Mathf.Clamp01(normalized));
				Vector2 anchoredPosition = _indicator.anchoredPosition;
				anchoredPosition.x = x;
				_indicator.anchoredPosition = anchoredPosition;
			}
		}

		public virtual void SetVisible(bool visible)
		{
			if (!(_canvasGroup == null))
			{
				_canvasGroup.alpha = (visible ? 1f : 0f);
				_canvasGroup.blocksRaycasts = visible;
				_canvasGroup.interactable = visible;
				if (!visible)
				{
					ResetScale();
				}
			}
		}

		public virtual void PlayPressScalePulse()
		{
			CacheInitialScale();
			if (!(_scaleTarget == null))
			{
				_targetScaleMultiplier = Mathf.Min(_targetScaleMultiplier + _scalePulseAmount, _maxScaleMultiplier);
			}
		}

		private void CacheInitialScale()
		{
			if (!_hasCachedInitialScale && !(_scaleTarget == null))
			{
				_initialScale = _scaleTarget.localScale;
				_hasCachedInitialScale = true;
			}
		}

		private void ResetScale()
		{
			_targetScaleMultiplier = 1f;
			_currentScaleMultiplier = 1f;
			if (!(_scaleTarget == null))
			{
				CacheInitialScale();
				_scaleTarget.localScale = _initialScale;
			}
		}
	}
}
