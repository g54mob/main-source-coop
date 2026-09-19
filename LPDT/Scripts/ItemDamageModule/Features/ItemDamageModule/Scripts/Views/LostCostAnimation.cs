using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Features.ItemDamageModule.Scripts.Views
{
	public class LostCostAnimation : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup _canvasGroup;

		[SerializeField]
		private TMP_Text _lostCostText;

		[SerializeField]
		private float _animationDuration = 1f;

		[SerializeField]
		private float _whiteColorDuration = 0.3f;

		[SerializeField]
		private Color _whiteColor = Color.white;

		[SerializeField]
		private Color _redColor = Color.red;

		[SerializeField]
		private float _minFontSize = 50f;

		[SerializeField]
		private float _maxFontSize = 65f;

		[SerializeField]
		private float _minDistance = 50f;

		[SerializeField]
		private float _maxDistance = 500f;

		private RectTransform _rectTransform;

		private Camera _camera;

		private Vector3 _worldPosition;

		private Sequence _animationSequence;

		public bool IsActive { get; private set; }

		private void Awake()
		{
			_rectTransform = base.transform as RectTransform;
		}

		private void OnDestroy()
		{
			KillAnimation();
		}

		public void Activate(float lostCost, Vector3 hitPosition, Camera targetCamera, float distance)
		{
			Vector3 rhs = hitPosition - targetCamera.transform.position;
			if (!(Vector3.Dot(targetCamera.transform.forward, rhs) < 0f))
			{
				_worldPosition = hitPosition;
				_camera = targetCamera;
				_lostCostText.text = $"-{lostCost}";
				_lostCostText.fontSize = Mathf.Lerp(_maxFontSize, _minFontSize, Mathf.InverseLerp(_minDistance, _maxDistance, distance));
				IsActive = true;
				KillAnimation();
				PlayAnimation();
			}
		}

		public void Deactivate()
		{
			IsActive = false;
			KillAnimation();
			_canvasGroup.alpha = 0f;
		}

		private void KillAnimation()
		{
			if (_animationSequence != null && _animationSequence.IsActive())
			{
				_animationSequence.Kill();
			}
			_animationSequence = null;
		}

		private void PlayAnimation()
		{
			_canvasGroup.alpha = 1f;
			_lostCostText.color = _whiteColor;
			float num = _animationDuration - _whiteColorDuration;
			float duration = 0.2f;
			float num2 = _animationDuration - _whiteColorDuration - num;
			_animationSequence = DOTween.Sequence();
			_animationSequence.AppendInterval(_whiteColorDuration);
			_animationSequence.Append(DOTween.To(() => _lostCostText.color, delegate(Color color)
			{
				_lostCostText.color = color;
			}, _redColor, num));
			if (num2 > 0f)
			{
				_animationSequence.AppendInterval(num2);
			}
			_animationSequence.Append(_canvasGroup.DOFade(0f, duration));
			_animationSequence.OnComplete(Deactivate);
		}

		private void LateUpdate()
		{
			if (IsActive)
			{
				UpdatePosition();
			}
		}

		private void UpdatePosition()
		{
			if (_camera == null || _rectTransform == null)
			{
				return;
			}
			Canvas componentInParent = GetComponentInParent<Canvas>();
			if (componentInParent == null)
			{
				return;
			}
			RectTransform rectTransform = componentInParent.rootCanvas.transform as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(_camera, _worldPosition);
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, null, out var localPoint))
			{
				RectTransform rectTransform2 = _rectTransform.parent as RectTransform;
				if (rectTransform2 != null)
				{
					Vector2 vector = rectTransform.TransformPoint(localPoint);
					Vector2 anchoredPosition = rectTransform2.InverseTransformPoint(vector);
					_rectTransform.anchoredPosition = anchoredPosition;
				}
				else
				{
					_rectTransform.anchoredPosition = localPoint;
				}
			}
		}
	}
}
