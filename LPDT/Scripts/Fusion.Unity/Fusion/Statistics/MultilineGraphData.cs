using System;
using UnityEngine;

namespace Fusion.Statistics
{
	public class MultilineGraphData : IDisposable
	{
		private readonly int _dataTextureShaderPropertyID = Shader.PropertyToID("_DataTex");

		private readonly int _thresholdsShaderPropertyID = Shader.PropertyToID("_Thresholds");

		private readonly int _maxSamplesShaderPropertyID = Shader.PropertyToID("_MaxSamples");

		private readonly int _lineCountShaderPropertyID = Shader.PropertyToID("_LineCount");

		private readonly int _thresholdCountShaderPropertyID = Shader.PropertyToID("_ThresholdCount");

		private readonly int _minValueShaderPropertyID = Shader.PropertyToID("_MinValue");

		private readonly int _maxValueShaderPropertyID = Shader.PropertyToID("_MaxValue");

		private readonly int _lineColorsShaderPropertyID = Shader.PropertyToID("_LineColors");

		private readonly int _thresholdColorsShaderPropertyID = Shader.PropertyToID("_ThresholdColors");

		private readonly int _writeIndicesShaderPropertyID = Shader.PropertyToID("_WriteIndices");

		private readonly int _sampleCountsShaderPropertyID = Shader.PropertyToID("_SampleCounts");

		private readonly int _lineVisibleShaderPropertyID = Shader.PropertyToID("_LineVisible");

		private readonly int _thresholdVisibleShaderPropertyID = Shader.PropertyToID("_ThresholdVisible");

		public const int MaxSamples = 512;

		public const int MaxLines = 4;

		public const int MaxThresholds = 4;

		private Texture2D _dataTexture;

		private Color[] _dataPixels;

		private readonly int[] _writeIndices = new int[4];

		private readonly int[] _sampleCounts = new int[4];

		private readonly float[] _writeIndicesFloat = new float[4];

		private readonly float[] _sampleCountsFloat = new float[4];

		private readonly Color[] _lineColors = new Color[4];

		private readonly float[] _thresholds = new float[4];

		private readonly Color[] _thresholdColors = new Color[4];

		private readonly float[] _lineVisible = new float[4] { 1f, 1f, 1f, 1f };

		private readonly float[] _thresholdVisible = new float[4] { 1f, 1f, 1f, 1f };

		private int _lineCount;

		private int _thresholdCount;

		private float _minValue;

		private float _maxValue;

		private float _defaultMinValue;

		private float _defaultMaxValue = 1f;

		private MultilineGraph.RangeType _rangeType = MultilineGraph.RangeType.Adaptive;

		private float _rangePadding = 0.1f;

		private float _dataMin = float.MaxValue;

		private float _dataMax = float.MinValue;

		private bool _rangeDirty = true;

		private bool _dataDirty;

		public int LineCount => _lineCount;

		public int ThresholdCount => _thresholdCount;

		public float MinValue => _minValue;

		public float MaxValue => _maxValue;

		public MultilineGraphData(bool useFullPrecision)
		{
			_dataTexture = new Texture2D(512, 4, useFullPrecision ? TextureFormat.RFloat : TextureFormat.RHalf, mipChain: false);
			_dataTexture.filterMode = FilterMode.Point;
			_dataTexture.wrapMode = TextureWrapMode.Clamp;
			_dataPixels = new Color[2048];
		}

		public void Dispose()
		{
			if (_dataTexture != null)
			{
				UnityEngine.Object.Destroy(_dataTexture);
				_dataTexture = null;
			}
		}

		public int AddLine()
		{
			if (_lineCount == 4)
			{
				return -1;
			}
			_lineCount++;
			return _lineCount - 1;
		}

		public int AddThreshold()
		{
			if (_thresholdCount == 4)
			{
				return -1;
			}
			_thresholdCount++;
			return _thresholdCount - 1;
		}

		public void SetLineColor(int index, Color color)
		{
			if (index >= 0 && index < _lineCount)
			{
				_lineColors[index] = color;
			}
		}

		public void SetLineVisible(int index, bool visible)
		{
			if (index >= 0 && index < _lineCount)
			{
				_lineVisible[index] = (visible ? 1f : 0f);
				_rangeDirty = true;
				_dataDirty = true;
			}
		}

		public void SetThresholdVisible(int index, bool visible)
		{
			if (index >= 0 && index < _thresholdCount)
			{
				_thresholdVisible[index] = (visible ? 1f : 0f);
				_rangeDirty = true;
				_dataDirty = true;
			}
		}

		public void SetThreshold(int index, float value, Color color)
		{
			if (index >= 0 && index < _thresholdCount)
			{
				_thresholds[index] = value;
				_thresholdColors[index] = color;
				_rangeDirty = true;
			}
		}

		public bool SetThresholdValue(int index, float value)
		{
			if (index < 0 || index >= _thresholdCount)
			{
				return false;
			}
			_thresholds[index] = value;
			_rangeDirty = true;
			return true;
		}

		public void SetRange(float minValue, float maxValue, MultilineGraph.RangeType rangeType = MultilineGraph.RangeType.Adaptive, float padding = 0.1f)
		{
			_defaultMinValue = (_minValue = minValue);
			_defaultMaxValue = (_maxValue = maxValue);
			_rangeType = rangeType;
			_rangePadding = padding;
			_rangeDirty = true;
		}

		public bool AddValue(int lineIndex, float value)
		{
			if (lineIndex < 0 || lineIndex >= _lineCount)
			{
				return false;
			}
			int num = _writeIndices[lineIndex];
			int num2 = lineIndex * 512 + num;
			float r = _dataPixels[num2].r;
			bool flag = _sampleCounts[lineIndex] == 512 && (r == _dataMin || r == _dataMax);
			_dataPixels[num2].r = value;
			_writeIndices[lineIndex] = (num + 1) % 512;
			_sampleCounts[lineIndex] = Mathf.Min(_sampleCounts[lineIndex] + 1, 512);
			if (!float.IsNaN(value) && !float.IsInfinity(value))
			{
				if (value < _dataMin)
				{
					_dataMin = value;
				}
				if (value > _dataMax)
				{
					_dataMax = value;
				}
				if (flag)
				{
					_rangeDirty = true;
				}
			}
			_dataDirty = true;
			return true;
		}

		private void RecalculateDataRange()
		{
			_dataMin = float.MaxValue;
			_dataMax = float.MinValue;
			for (int i = 0; i < _lineCount; i++)
			{
				if (_lineVisible[i] < 0.5f)
				{
					continue;
				}
				int num = _sampleCounts[i];
				int num2 = i * 512;
				for (int j = 0; j < num; j++)
				{
					float r = _dataPixels[num2 + j].r;
					if (!float.IsNaN(r) && !float.IsInfinity(r))
					{
						if (r < _dataMin)
						{
							_dataMin = r;
						}
						if (r > _dataMax)
						{
							_dataMax = r;
						}
					}
				}
			}
			for (int k = 0; k < _thresholdCount; k++)
			{
				if (!(_thresholdVisible[k] < 0.5f))
				{
					float num3 = _thresholds[k];
					if (num3 < _dataMin)
					{
						_dataMin = num3;
					}
					if (num3 > _dataMax)
					{
						_dataMax = num3;
					}
				}
			}
			_rangeDirty = false;
		}

		public void Apply()
		{
			if (!_dataDirty)
			{
				return;
			}
			if (_rangeDirty)
			{
				RecalculateDataRange();
			}
			float dataMin = _dataMin;
			float dataMax = _dataMax;
			switch (_rangeType)
			{
			case MultilineGraph.RangeType.DefaultOnly:
				_minValue = _defaultMinValue;
				_maxValue = _defaultMaxValue;
				break;
			case MultilineGraph.RangeType.Adaptive:
				_minValue = _defaultMinValue;
				_maxValue = _defaultMaxValue;
				if (dataMin < _minValue)
				{
					float num3 = _minValue - dataMin;
					_minValue = dataMin - num3 * _rangePadding;
				}
				if (dataMax > _maxValue)
				{
					float num4 = dataMax - _maxValue;
					_maxValue = dataMax + num4 * _rangePadding;
				}
				break;
			case MultilineGraph.RangeType.Sticky:
				if (dataMin < _minValue)
				{
					float num = _minValue - dataMin;
					_minValue = dataMin - num * _rangePadding;
				}
				if (dataMax > _maxValue)
				{
					float num2 = dataMax - _maxValue;
					_maxValue = dataMax + num2 * _rangePadding;
				}
				break;
			}
			if (_minValue >= _maxValue)
			{
				_minValue = _defaultMinValue;
				_maxValue = _defaultMaxValue;
				if (_minValue >= _maxValue)
				{
					_minValue = 0f;
					_maxValue = 1f;
				}
			}
			_dataTexture.SetPixels(_dataPixels);
			_dataTexture.Apply();
			_dataDirty = false;
		}

		public void ApplyToMaterial(Material mat)
		{
			for (int i = 0; i < 4; i++)
			{
				_writeIndicesFloat[i] = _writeIndices[i];
				_sampleCountsFloat[i] = _sampleCounts[i];
			}
			mat.SetTexture(_dataTextureShaderPropertyID, _dataTexture);
			mat.SetInt(_maxSamplesShaderPropertyID, 512);
			mat.SetInt(_lineCountShaderPropertyID, _lineCount);
			mat.SetInt(_thresholdCountShaderPropertyID, _thresholdCount);
			mat.SetFloat(_minValueShaderPropertyID, _minValue);
			mat.SetFloat(_maxValueShaderPropertyID, _maxValue);
			mat.SetColorArray(_lineColorsShaderPropertyID, _lineColors);
			mat.SetFloatArray(_thresholdsShaderPropertyID, _thresholds);
			mat.SetColorArray(_thresholdColorsShaderPropertyID, _thresholdColors);
			mat.SetFloatArray(_writeIndicesShaderPropertyID, _writeIndicesFloat);
			mat.SetFloatArray(_sampleCountsShaderPropertyID, _sampleCountsFloat);
			mat.SetFloatArray(_lineVisibleShaderPropertyID, _lineVisible);
			mat.SetFloatArray(_thresholdVisibleShaderPropertyID, _thresholdVisible);
		}
	}
}
