using UnityEngine;
using UnityEngine.UI;

namespace Fusion.Statistics
{
	[RequireComponent(typeof(RawImage))]
	public class MultilineGraph : MonoBehaviour
	{
		public enum RangeType
		{
			DefaultOnly = 0,
			Adaptive = 1,
			Sticky = 2
		}

		[Header("Shader")]
		[SerializeField]
		private Color _backgroundColor = Color.black;

		[SerializeField]
		[Range(0.01f, 0.1f)]
		private float _lineWidth = 0.01f;

		[SerializeField]
		[Range(0.01f, 0.1f)]
		private float _thresholdWidth = 0.01f;

		[SerializeField]
		private bool _useFullPrecision;

		[Header("Range Settings")]
		[SerializeField]
		private float _defaultMin;

		[SerializeField]
		private float _defaultMax = 1f;

		[SerializeField]
		private RangeType _rangeType = RangeType.Adaptive;

		[SerializeField]
		private float _rangePadding = 0.1f;

		[Header("UI")]
		[SerializeField]
		private Text HeaderText;

		[SerializeField]
		private Text RangeUpper;

		[SerializeField]
		private Text RangeLower;

		[SerializeField]
		private GameObject _legendItemPrefab;

		[SerializeField]
		private VerticalLayoutGroup _linesLegendHolder;

		[SerializeField]
		private VerticalLayoutGroup _thresholdLegendHolder;

		private Material _material;

		private MultilineGraphData _graphData;

		private RawImage _rawImage;

		private void Awake()
		{
			_rawImage = GetComponent<RawImage>();
			_material = new Material(_rawImage.material);
			_material.SetColor("_BackgroundColor", _backgroundColor);
			_material.SetFloat("_LineWidth", _lineWidth);
			_material.SetFloat("_ThresholdWidth", _thresholdWidth);
			_rawImage.material = _material;
			_graphData = new MultilineGraphData(_useFullPrecision);
			_graphData.SetRange(_defaultMin, _defaultMax, _rangeType, _rangePadding);
			if (HeaderText != null && HeaderText.text.Length == 0)
			{
				HeaderText.gameObject.SetActive(value: false);
			}
		}

		private void OnDestroy()
		{
			_graphData?.Dispose();
			if (_material != null)
			{
				Object.Destroy(_material);
			}
		}

		public int AddLine(Color color, string label = null)
		{
			int index = _graphData.AddLine();
			if (index < 0)
			{
				return index;
			}
			_graphData.SetLineColor(index, color);
			if (_linesLegendHolder != null && _legendItemPrefab != null)
			{
				MultilineGraphLegendItem component = Object.Instantiate(_legendItemPrefab, _linesLegendHolder.transform).GetComponent<MultilineGraphLegendItem>();
				component.Set(label, color, usesDashedSwatch: false);
				component.OnToggled = delegate(bool visible)
				{
					_graphData.SetLineVisible(index, visible);
				};
			}
			return index;
		}

		public int AddThreshold(float value, Color color, string label)
		{
			int index = _graphData.AddThreshold();
			if (index < 0)
			{
				return index;
			}
			_graphData.SetThreshold(index, value, color);
			if (_thresholdLegendHolder != null && _legendItemPrefab != null)
			{
				MultilineGraphLegendItem component = Object.Instantiate(_legendItemPrefab, _thresholdLegendHolder.transform).GetComponent<MultilineGraphLegendItem>();
				component.Set($"{label}{value}", color, usesDashedSwatch: true);
				component.OnToggled = delegate(bool visible)
				{
					_graphData.SetThresholdVisible(index, visible);
				};
			}
			return index;
		}

		public void SetLineVisible(int index, bool visible)
		{
			_graphData.SetLineVisible(index, visible);
		}

		public void SetThresholdVisible(int index, bool visible)
		{
			_graphData.SetThresholdVisible(index, visible);
		}

		public bool AddValue(int lineIndex, float value)
		{
			return _graphData.AddValue(lineIndex, value);
		}

		public bool SetThresholdValue(int index, float value)
		{
			return _graphData.SetThresholdValue(index, value);
		}

		public void RefreshDisplay()
		{
			_graphData.Apply();
			_graphData.ApplyToMaterial(_material);
			if (RangeUpper != null)
			{
				RangeUpper.text = _graphData.MaxValue.ToString("G4");
			}
			if (RangeLower != null)
			{
				RangeLower.text = _graphData.MinValue.ToString("G4");
			}
		}
	}
}
