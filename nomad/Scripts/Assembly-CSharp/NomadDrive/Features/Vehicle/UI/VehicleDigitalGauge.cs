using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NomadDrive.Features.Vehicle.UI
{
	public class VehicleDigitalGauge : MonoBehaviour
	{
		[SerializeField]
		private DashboardDisplayMode displayMode;

		[SerializeField]
		private DigitalGaugeType gaugeType;

		[SerializeField]
		private string format = "0.0";

		[SerializeField]
		private float maxValue;

		[SerializeField]
		[Range(0f, 1f)]
		private float numericalSmoothing = 0.5f;

		[SerializeField]
		private string unit;

		[SerializeField]
		private Text uiText;

		[SerializeField]
		private TMP_Text uiTmpText;

		[SerializeField]
		private TMP_Text meshText;

		[SerializeField]
		private bool showProgressBar;

		[SerializeField]
		private Image progressBarImage;

		private float _numericalValue;

		private float _prevNumericalValue;

		private string _stringValue;

		private StringBuilder _stringBuilder;

		private float _fullLineWidth;

		public float NumericalValue
		{
			get
			{
				return _numericalValue;
			}
			set
			{
				_numericalValue = value;
			}
		}

		public string StringValue
		{
			get
			{
				return _stringValue;
			}
			set
			{
				_stringValue = value;
			}
		}

		private void Start()
		{
			_stringBuilder = new StringBuilder();
			if (displayMode == DashboardDisplayMode.UI && uiText == null)
			{
				Transform transform = base.transform.Find("Readout");
				if (transform != null)
				{
					uiText = transform.GetComponent<Text>();
				}
			}
			if (gaugeType == DigitalGaugeType.Textual)
			{
				showProgressBar = false;
			}
			if (showProgressBar && progressBarImage != null)
			{
				_fullLineWidth = progressBarImage.rectTransform.sizeDelta.x;
			}
		}

		private void Update()
		{
			_stringBuilder.Clear();
			if (gaugeType == DigitalGaugeType.Numerical)
			{
				_numericalValue = Mathf.SmoothStep(_prevNumericalValue, _numericalValue, 1.01f - numericalSmoothing);
				string text = "{0:" + format + "}";
				_stringBuilder.AppendFormat(text, _numericalValue);
				_prevNumericalValue = _numericalValue;
			}
			if (!string.IsNullOrEmpty(_stringValue))
			{
				_stringBuilder.Append(_stringValue);
			}
			if (!string.IsNullOrEmpty(unit))
			{
				_stringBuilder.Append(' ');
				_stringBuilder.Append(unit);
			}
			string text2 = _stringBuilder.ToString();
			if (displayMode == DashboardDisplayMode.UI)
			{
				if (uiTmpText != null)
				{
					uiTmpText.text = text2;
				}
				else if (uiText != null)
				{
					uiText.text = text2;
				}
			}
			else if (meshText != null)
			{
				meshText.text = text2;
			}
			if (showProgressBar && progressBarImage != null)
			{
				float num = Mathf.Clamp01(_numericalValue / maxValue);
				progressBarImage.rectTransform.sizeDelta = new Vector2(num * _fullLineWidth, progressBarImage.rectTransform.sizeDelta.y);
			}
		}
	}
}
