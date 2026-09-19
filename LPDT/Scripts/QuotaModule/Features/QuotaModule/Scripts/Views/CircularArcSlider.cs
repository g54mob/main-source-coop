using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.QuotaModule.Scripts.Views
{
	public class CircularArcSlider : MonoBehaviour
	{
		[SerializeField]
		private RectTransform _handle;

		[SerializeField]
		private RectTransform _fill;

		[SerializeField]
		private Image _fillImage;

		[SerializeField]
		private float _radius = 150f;

		[SerializeField]
		private Vector2 _centerOfCircle;

		[SerializeField]
		private float _minAngle = -10f;

		[SerializeField]
		private float _maxAngle = 10f;

		[SerializeField]
		private float _additionalAngle = 180f;

		[Range(0f, 1f)]
		[SerializeField]
		private float _normalizedValue;

		[SerializeField]
		private float _rotationOffset;

		[SerializeField]
		private float _handleOffset = -0.2f;

		public UnityEvent<float> OnValueChanged;

		public float NormalizedValue
		{
			get
			{
				return _normalizedValue;
			}
			set
			{
				_normalizedValue = value;
				UpdateTransform();
				OnValueChanged?.Invoke(_normalizedValue);
			}
		}

		private void Awake()
		{
			if (_handle == null)
			{
				Debug.LogError("[CircularArcSlider] Handle is not assigned in the Inspector!", this);
			}
		}

		private void OnValidate()
		{
			if (!(_handle == null))
			{
				if (_fill != null)
				{
					_fill.sizeDelta = Vector2.one * (_radius * 3f);
					_fill.localPosition = _centerOfCircle;
				}
				UpdateTransform();
			}
		}

		private void UpdateTransform()
		{
			float angleDeg = Mathf.Lerp(_minAngle + _handleOffset, _maxAngle + _handleOffset, Mathf.Clamp01(_normalizedValue));
			ApplyHandleTransform(angleDeg);
			ApplyFill();
		}

		private void ApplyHandleTransform(float angleDeg)
		{
			if (!(_handle == null))
			{
				float f = (0f - angleDeg + _additionalAngle) * (MathF.PI / 180f);
				_handle.anchoredPosition = _centerOfCircle + new Vector2(Mathf.Cos(f) * _radius, Mathf.Sin(f) * _radius);
				_handle.localRotation = Quaternion.Euler(0f, 0f, 0f - angleDeg + _additionalAngle - 90f + _rotationOffset);
			}
		}

		private void ApplyFill()
		{
			if (!(_fill == null) && !(_fillImage == null))
			{
				float num = 0.5f - (180f - _maxAngle) / 360f;
				float num2 = 0.5f - (180f - _minAngle) / 360f;
				_fillImage.fillAmount = (num - num2) * _normalizedValue + num2;
			}
		}
	}
}
