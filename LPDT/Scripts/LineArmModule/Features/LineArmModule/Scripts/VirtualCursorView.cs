using UnityEngine;

namespace Features.LineArmModule.Scripts
{
	public class VirtualCursorView : VirtualCursorViewBase
	{
		[SerializeField]
		private RectTransform _cursorRectTransform;

		[SerializeField]
		private CanvasGroup _canvasGroup;

		[SerializeField]
		private GameObject _visualRoot;

		[SerializeField]
		private float _positionSmoothTime = 0.05f;

		private RectTransform _parentRectTransform;

		private Vector2 _targetAnchoredPosition;

		private Vector2 _positionVelocity;

		private bool _hasTargetPosition;

		private bool _isVisible;

		private void Awake()
		{
			if (_cursorRectTransform == null)
			{
				_cursorRectTransform = base.transform as RectTransform;
			}
			_parentRectTransform = ((_cursorRectTransform != null) ? (_cursorRectTransform.parent as RectTransform) : null);
		}

		public override void SetVirtualCursorVisibility(bool isVisible)
		{
			_isVisible = isVisible;
			if (_canvasGroup != null)
			{
				_canvasGroup.alpha = (isVisible ? 1f : 0f);
			}
			if (_visualRoot != null)
			{
				_visualRoot.SetActive(isVisible);
			}
		}

		public override void SetVirtualCursorScreenPosition(Vector2 screenPosition)
		{
			if (_cursorRectTransform == null)
			{
				return;
			}
			if (_parentRectTransform == null)
			{
				_cursorRectTransform.position = screenPosition;
				return;
			}
			RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentRectTransform, screenPosition, null, out var localPoint);
			_targetAnchoredPosition = localPoint;
			if (!_hasTargetPosition)
			{
				_cursorRectTransform.anchoredPosition = localPoint;
				_hasTargetPosition = true;
			}
		}

		private void Update()
		{
			if (_isVisible && _hasTargetPosition && !(_cursorRectTransform == null) && !(_parentRectTransform == null))
			{
				float smoothTime = Mathf.Max(0.001f, _positionSmoothTime);
				_cursorRectTransform.anchoredPosition = Vector2.SmoothDamp(_cursorRectTransform.anchoredPosition, _targetAnchoredPosition, ref _positionVelocity, smoothTime, float.PositiveInfinity, Time.deltaTime);
			}
		}
	}
}
