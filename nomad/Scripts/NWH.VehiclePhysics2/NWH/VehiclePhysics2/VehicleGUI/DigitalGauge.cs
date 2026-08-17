using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace NWH.VehiclePhysics2.VehicleGUI
{
	[RequireComponent(typeof(Text))]
	public class DigitalGauge : MonoBehaviour
	{
		public enum GaugeType
		{
			Numerical = 0,
			Textual = 1
		}

		[Tooltip("    Numerical value formatting.")]
		public string format = "0.0";

		[Tooltip("Should the stringValue or numericalValue be used. String value is useful for e.g. gear (R, N, 1, 2, 3) and numerical\r\nfor\r\nspeed, RPM or similar.")]
		public GaugeType gaugeType;

		[Tooltip("    Maximum value that the gauge can display. Only used if showProgressBar enabled.")]
		public float maxValue;

		[Range(0f, 1f)]
		[Tooltip("    Time over which the numerical value will be smoothed.")]
		public float numericalSmoothing = 0.5f;

		[Tooltip("    Numerical value that will be displayed on the gauge.")]
		public float numericalValue;

		[Tooltip("    Should the progress line/bar be displayed for better visualization?")]
		public bool showProgressBar;

		[Tooltip("    String value that will be displayed on the gauge.")]
		public string stringValue;

		[Tooltip("    Unit displayed after the value, e.g. km/h.")]
		public string unit;

		private float _fullLineWidth;

		private Image _line;

		private float _prevNumericalValue;

		private Text _readout;

		private StringBuilder _stringBuilder;

		private void Start()
		{
			Transform transform = base.transform.Find("Readout");
			if (transform != null)
			{
				_readout = transform.gameObject.GetComponent<Text>();
			}
			Transform transform2 = base.transform.Find("Line");
			if (transform2 != null)
			{
				_line = transform2.gameObject.GetComponent<Image>();
			}
			if (gaugeType == GaugeType.Textual)
			{
				showProgressBar = false;
			}
			if (_line != null)
			{
				_fullLineWidth = _line.rectTransform.sizeDelta.x;
			}
			_stringBuilder = new StringBuilder();
		}

		private void Update()
		{
			if (_readout != null)
			{
				_stringBuilder.Clear();
				if (gaugeType == GaugeType.Numerical)
				{
					numericalValue = Mathf.SmoothStep(_prevNumericalValue, numericalValue, 1.01f - numericalSmoothing);
					string text = "{0:" + format + "}";
					_stringBuilder.AppendFormat(text, numericalValue);
					_prevNumericalValue = numericalValue;
				}
				_stringBuilder.Append(stringValue);
				if (!string.IsNullOrEmpty(unit))
				{
					_stringBuilder.Append(' ');
					_stringBuilder.Append(unit);
				}
				_readout.text = _stringBuilder.ToString();
			}
			if (_line != null && showProgressBar)
			{
				float num = Mathf.Clamp01(numericalValue / maxValue);
				_line.rectTransform.sizeDelta = new Vector2(num * _fullLineWidth, _line.rectTransform.sizeDelta.y);
			}
		}
	}
}
