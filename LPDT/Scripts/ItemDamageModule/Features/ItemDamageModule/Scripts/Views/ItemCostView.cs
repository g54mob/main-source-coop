using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace Features.ItemDamageModule.Scripts.Views
{
	public class ItemCostView : ItemCostViewBase
	{
		[SerializeField]
		private GameObject _damageDisplayRoot;

		[SerializeField]
		private TMP_Text _costText;

		[SerializeField]
		private RectTransform _rectTransform;

		[SerializeField]
		private float _minSize;

		[SerializeField]
		private float _maxSize = 1f;

		[SerializeField]
		private float _openAnimationDuration = 0.3f;

		[SerializeField]
		private float _closeAnimationDuration = 0.3f;

		[SerializeField]
		private string _currencyString = "¢";

		[SerializeField]
		private float _costUpdateSpeed = 1f;

		[SerializeField]
		private float _costUpdateMaxDuration = 2f;

		[SerializeField]
		private AnimationCurve _costUpdateCurve;

		[SerializeField]
		private float _costScaleMultiplier = 0.01f;

		[SerializeField]
		private float _maxScaleFromCost = 2f;

		private Coroutine _sizeCoroutine;

		private Coroutine _costUpdateCoroutine;

		private bool _isCurrentlyShowed = true;

		private float _currentCost;

		public override void UpdateCostDisplay(float cost, bool isWithAnimation)
		{
			if (isWithAnimation)
			{
				if (_costUpdateCoroutine != null)
				{
					StopCoroutine(_costUpdateCoroutine);
				}
				_costUpdateCoroutine = StartCoroutine(UpdateCostCoroutine(cost));
			}
			else
			{
				_costText.text = cost.ToString(CultureInfo.InvariantCulture) + _currencyString;
			}
		}

		private IEnumerator UpdateCostCoroutine(float targetCost)
		{
			float elapsed = 0f;
			float initialCost = _currentCost;
			float duration = Mathf.Min(Mathf.Abs(targetCost - initialCost) / _costUpdateSpeed, _costUpdateMaxDuration);
			while (elapsed < duration)
			{
				elapsed += Time.deltaTime;
				float num = Mathf.Clamp01(elapsed / duration);
				if (_costUpdateCurve != null)
				{
					num = _costUpdateCurve.Evaluate(num);
				}
				float num2 = Mathf.Lerp(initialCost, targetCost, num);
				_costText.text = ((int)num2).ToString(CultureInfo.InvariantCulture) + _currencyString;
				_currentCost = num2;
				float num3 = Mathf.Min(1f + num2 * _costScaleMultiplier, _maxScaleFromCost);
				_costText.transform.localScale = Vector3.one * num3;
				yield return null;
			}
			_costText.text = targetCost.ToString(CultureInfo.InvariantCulture) + _currencyString;
			float num4 = Mathf.Min(1f + targetCost * _costScaleMultiplier, _maxScaleFromCost);
			_costText.transform.localScale = Vector3.one * num4;
			_currentCost = targetCost;
		}

		public override void SwitchCostDisplay(bool show)
		{
			if (_isCurrentlyShowed == show)
			{
				return;
			}
			_isCurrentlyShowed = show;
			if (_damageDisplayRoot != null)
			{
				if (_sizeCoroutine != null)
				{
					StopCoroutine(_sizeCoroutine);
				}
				_sizeCoroutine = StartCoroutine(show ? IncreaseSizeCoroutine() : DecreaseSizeCoroutine());
			}
		}

		private IEnumerator IncreaseSizeCoroutine()
		{
			float elapsed = 0f;
			_damageDisplayRoot.SetActive(value: true);
			_costText.transform.localScale = new Vector3(_minSize, _minSize, _minSize);
			while (elapsed < _openAnimationDuration)
			{
				elapsed += Time.deltaTime;
				float t = Mathf.Clamp01(elapsed / _openAnimationDuration);
				float num = Mathf.Lerp(_minSize, _maxSize, t);
				_costText.transform.localScale = new Vector3(num, num, num);
				yield return null;
			}
			_costText.transform.localScale = new Vector3(_maxSize, _maxSize, _maxSize);
		}

		private IEnumerator DecreaseSizeCoroutine()
		{
			float elapsed = 0f;
			_costText.transform.localScale = new Vector3(_maxSize, _maxSize, _maxSize);
			while (elapsed < _closeAnimationDuration)
			{
				elapsed += Time.deltaTime;
				float t = Mathf.Clamp01(elapsed / _closeAnimationDuration);
				float num = Mathf.Lerp(_maxSize, _minSize, t);
				_costText.transform.localScale = new Vector3(num, num, num);
				yield return null;
			}
			_costText.transform.localScale = new Vector3(_minSize, _minSize, _minSize);
			_damageDisplayRoot.SetActive(value: false);
		}

		public override void SetLocalPosition(Vector3 localPoint)
		{
			_rectTransform.anchoredPosition = localPoint;
		}

		public override void ClearCurrency()
		{
			_currentCost = 0f;
		}
	}
}
