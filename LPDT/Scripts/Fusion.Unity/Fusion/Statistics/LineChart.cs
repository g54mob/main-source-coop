using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Fusion.Statistics
{
	public class LineChart : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		[SerializeField]
		protected float _threshold;

		[SerializeField]
		protected RawImage _sourceImage;

		[SerializeField]
		protected Text _titleText;

		[SerializeField]
		protected Text _peakValueText;

		[SerializeField]
		protected Text _avgValueText;

		[SerializeField]
		protected Text _lastValueText;

		protected StatAccumulator _accumulator;

		private string _originalTitle;

		private string _labelFormat;

		private bool _forcePerUpdate;

		private float _lookUpTableMultiplier;

		private readonly int _valuesShaderPropertyID = Shader.PropertyToID("_Values");

		private readonly int _valueMinShaderPropertyID = Shader.PropertyToID("_ValueMin");

		private readonly int _valueMaxShaderPropertyID = Shader.PropertyToID("_ValueMax");

		private readonly int _thresholdShaderPropertyID = Shader.PropertyToID("_Threshold");

		private readonly int _baseBottomColorShaderPropertyID = Shader.PropertyToID("_BottomColor");

		private readonly int _baseTopColorShaderPropertyID = Shader.PropertyToID("_TopColor");

		private readonly int _thresholdBottomColorShaderPropertyID = Shader.PropertyToID("_ThresholdBottomColor");

		private readonly int _thresholdTopColorShaderPropertyID = Shader.PropertyToID("_ThresholdTopColor");

		private readonly int _zeroIsTransparentShaderPropertyId = Shader.PropertyToID("_ZeroIsTransparent");

		private const int BUFFER_SAMPLES = 180;

		private float[] _values;

		private float[] _valuesToDispatch;

		private int _headIndex;

		private Material _material;

		private string[][] _lookupTable;

		private float _lastNonZeroValue;

		public string Title
		{
			set
			{
				if ((bool)_titleText)
				{
					_titleText.text = value;
				}
			}
		}

		public void SetThreshold(float threshold)
		{
			threshold = (_accumulator.DisplayingPerSecond ? (threshold * (float)FusionStatistics.EstimateFusionAfterUpdatesPerSecond) : threshold);
			_material.SetFloat(_thresholdShaderPropertyID, threshold);
		}

		public void SetColors(Gradient defaultGradient, Gradient thresholdGradient, bool zeroIsTransparent)
		{
			_material.SetColor(_baseBottomColorShaderPropertyID, defaultGradient.Evaluate(0f));
			_material.SetColor(_baseTopColorShaderPropertyID, defaultGradient.Evaluate(1f));
			_material.SetColor(_thresholdBottomColorShaderPropertyID, thresholdGradient.Evaluate(0f));
			_material.SetColor(_thresholdTopColorShaderPropertyID, thresholdGradient.Evaluate(1f));
			_material.SetInteger(_zeroIsTransparentShaderPropertyId, zeroIsTransparent ? 1 : 0);
		}

		public void Clear()
		{
			for (int i = 0; i < _values.Length; i++)
			{
				_values[i] = 0f;
			}
		}

		public void DispatchValuesToShader()
		{
			float num = _values[_headIndex];
			float num2 = _values[_headIndex];
			float num3 = 0f;
			bool flag = false;
			int num4 = (_headIndex + 1) % 180;
			for (int i = 0; i < 180; i++)
			{
				_valuesToDispatch[i] = _values[num4];
				num3 += _valuesToDispatch[i];
				flag |= _valuesToDispatch[i] != 0f;
				if (_values[num4] < num)
				{
					num = _values[num4];
				}
				if (_values[num4] > num2)
				{
					num2 = _values[num4];
				}
				num4 = (num4 + 1) % 180;
			}
			num3 /= 180f;
			if (_lookupTable != FusionStatsLookup.LOOKUP_TABLE_0_00ms && num3 > 0f && num3 < 1f)
			{
				num3 = 1f;
			}
			FusionStatisticsConfig config = FusionStatistics.Config;
			float num5 = _valuesToDispatch[^1];
			if (num5 != 0f)
			{
				_lastNonZeroValue = num5;
			}
			_lastNonZeroValue = (flag ? _lastNonZeroValue : 0f);
			_peakValueText.text = FusionStatsLookup.GetValueText(num2, _lookupTable, _labelFormat, _lookUpTableMultiplier);
			_lastValueText.text = FusionStatsLookup.GetValueText((num5 == 0f && config.DontDisplayZeroOnLastValue) ? _lastNonZeroValue : num5, _lookupTable, _labelFormat, _lookUpTableMultiplier);
			_avgValueText.text = FusionStatsLookup.GetValueText(num3, _lookupTable, _labelFormat, _lookUpTableMultiplier);
			_material.SetFloatArray(_valuesShaderPropertyID, _valuesToDispatch);
			num = (_material.HasFloat(_valueMinShaderPropertyID) ? ((_material.GetFloat(_valueMinShaderPropertyID) + num) * 0.5f) : num);
			num2 = (_material.HasFloat(_valueMaxShaderPropertyID) ? ((_material.GetFloat(_valueMaxShaderPropertyID) + num2) * 0.5f) : num2);
			_material.SetFloat(_valueMinShaderPropertyID, num);
			_material.SetFloat(_valueMaxShaderPropertyID, num2);
		}

		public void RefreshDisplay()
		{
			SetThreshold(_threshold);
			DispatchValuesToShader();
		}

		public void Setup(string title, string[][] lookupTable, string labelFormat = "{0}", bool forcePerUpdate = false, float lookUpTableMultiplier = 1f)
		{
			_material = new Material(_sourceImage.material);
			_sourceImage.material = _material;
			_values = new float[180];
			_valuesToDispatch = new float[180];
			_headIndex = 0;
			_forcePerUpdate = forcePerUpdate;
			_originalTitle = title;
			Title = _originalTitle + " " + (_accumulator.DisplayingPerSecond ? "(S)" : "(U)");
			_labelFormat = labelFormat;
			_lookupTable = lookupTable;
			_lookUpTableMultiplier = lookUpTableMultiplier;
			SetThreshold(_threshold);
			FusionStatisticsConfig config = FusionStatistics.Config;
			SetColors(config.DefaultGradient, config.ThresholdGradient, config.RenderZeroAsTransparent);
		}

		public void AddValue(float value)
		{
			float lastTimeStamp = _accumulator.LastTimeStamp;
			_accumulator.Accumulate(value);
			if (!_accumulator.DisplayingPerSecond || !Mathf.Approximately(_accumulator.LastTimeStamp, lastTimeStamp))
			{
				float num = (_accumulator.DisplayingPerSecond ? _accumulator.ValuePerSecond : _accumulator.Value);
				_headIndex = (_headIndex + 1) % 180;
				_values[_headIndex] = num;
			}
		}

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			if (_forcePerUpdate)
			{
				_accumulator.DisplayingPerSecond = false;
			}
			else
			{
				_accumulator.DisplayingPerSecond = !_accumulator.DisplayingPerSecond;
			}
			Title = _originalTitle + " " + (_accumulator.DisplayingPerSecond ? "(S)" : "(U)");
		}
	}
}
