using System.Collections;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.HUDModule.Scripts
{
	public class PlayerStatView : PlayerStatViewBase
	{
		private static readonly int Increase = Animator.StringToHash("Increase");

		[SerializeField]
		private EntityStatType _trackedStatType;

		[SerializeField]
		private TMP_Text _maxStatValueText;

		[SerializeField]
		private TMP_Text _currentStatValueText;

		[SerializeField]
		private RectTransform _rectTransformToUpdate;

		[SerializeField]
		private Slider _statSlider;

		[SerializeField]
		private CanvasGroup _canvasGroup;

		[SerializeField]
		private float _fadeDuration = 0.3f;

		[SerializeField]
		private Animator _statAnimator;

		private Coroutine _fadeCoroutine;

		public override EntityStatType TrackedStatType => _trackedStatType;

		public override void SetMaxValue(float health)
		{
			_maxStatValueText.SetText("/" + FormatValue(health));
			LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransformToUpdate);
			if (_statSlider != null)
			{
				_statSlider.maxValue = health;
			}
		}

		public override void SetCurrentValue(float health)
		{
			_currentStatValueText.SetText(FormatValue(health));
			LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransformToUpdate);
			if (_statSlider != null)
			{
				_statSlider.value = health;
			}
		}

		public override void SetVisible(bool visible)
		{
			if (_fadeCoroutine != null)
			{
				StopCoroutine(_fadeCoroutine);
			}
			_fadeCoroutine = StartCoroutine(FadeCoroutine(visible ? 1f : 0f));
		}

		public override void TriggerIncreaseAnimation()
		{
			_statAnimator.SetTrigger(Increase);
		}

		private static string FormatValue(float value)
		{
			int num = Mathf.RoundToInt(value);
			if (num < 1000)
			{
				return num.ToString();
			}
			return $"{num / 1000}k";
		}

		private IEnumerator FadeCoroutine(float targetAlpha)
		{
			float startAlpha = _canvasGroup.alpha;
			float elapsed = 0f;
			while (elapsed < _fadeDuration)
			{
				elapsed += Time.deltaTime;
				_canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / _fadeDuration);
				yield return null;
			}
			_canvasGroup.alpha = targetAlpha;
			_fadeCoroutine = null;
		}
	}
}
