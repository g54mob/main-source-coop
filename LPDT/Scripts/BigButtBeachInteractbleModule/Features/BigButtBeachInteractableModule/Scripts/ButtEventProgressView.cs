using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.BigButtBeachInteractableModule.Scripts
{
	public class ButtEventProgressView : MonoBehaviour
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
		private TMP_Text _counterText;

		[SerializeField]
		private Color _normalColor;

		[SerializeField]
		private Color _completedColor;

		[SerializeField]
		private List<Image> _progressImages;

		[SerializeField]
		private ParticleSystem _fireworksParticle;

		[SerializeField]
		[Range(0f, 1f)]
		private float _debugNormalizedValue = 1f;

		public Camera ActiveCamera { get; set; }

		private void LateUpdate()
		{
			if (!(ActiveCamera == null))
			{
				base.transform.rotation = ActiveCamera.transform.rotation;
			}
		}

		public void SetCompletedStatus(bool completed)
		{
			foreach (Image progressImage in _progressImages)
			{
				progressImage.color = (completed ? _completedColor : _normalColor);
			}
		}

		public void TriggerFireworks()
		{
			_fireworksParticle.Play();
		}

		public void SetProgress(long currentValue, long maxValue)
		{
			if (_counterText != null)
			{
				_counterText.SetText("{0}/{1}", currentValue, maxValue);
			}
			SetNormalizedValue((maxValue > 0) ? ((float)((double)currentValue / (double)maxValue)) : 0f);
		}

		public void SetProgress(float progress)
		{
			SetNormalizedValue(progress);
		}

		public void SetNormalizedValue(float normalized)
		{
			if (!(_indicator == null))
			{
				float x = Mathf.Lerp(_minX, _maxX, Mathf.Clamp01(normalized));
				Vector2 anchoredPosition = _indicator.anchoredPosition;
				anchoredPosition.x = x;
				_indicator.anchoredPosition = anchoredPosition;
			}
		}

		public void SetVisible(bool visible)
		{
			if (!(_canvasGroup == null))
			{
				_canvasGroup.alpha = (visible ? 1f : 0f);
				_canvasGroup.blocksRaycasts = visible;
				_canvasGroup.interactable = visible;
			}
		}

		[ContextMenu("Debug/Apply Normalized Value")]
		private void ApplyDebugNormalizedValue()
		{
			SetNormalizedValue(_debugNormalizedValue);
		}
	}
}
