using UnityEngine;
using UnityEngine.UI;

namespace Fusion.Statistics
{
	public class RadialChart : MonoBehaviour
	{
		[SerializeField]
		protected Image _sourceImage;

		[SerializeField]
		protected Text _titleText;

		[SerializeField]
		protected Text _percentText;

		private int _fillShaderPropertyID = Shader.PropertyToID("_Fill");

		private float _value;

		private float _maxValue;

		private Material _material;

		public string Title
		{
			get
			{
				return _titleText?.text;
			}
			set
			{
				if ((bool)_titleText)
				{
					_titleText.text = value;
				}
			}
		}

		protected void SetShaderValues(float value, float totalValue)
		{
			float num = ((totalValue != 0f) ? Mathf.Clamp01(value / totalValue) : 0f);
			_material.SetFloat(_fillShaderPropertyID, num);
			_percentText.text = FusionStatsLookup.GetValueText(num * 100f, FusionStatsLookup.LOOKUP_TABLE_0_PERCENT, "{0}%");
		}

		public void RefreshDisplay()
		{
			SetShaderValues(_value, _maxValue);
		}

		public void Setup(string title)
		{
			_material = new Material(_sourceImage.material);
			_sourceImage.material = _material;
			Title = title;
		}

		public void SetValue(float value, float maxValue)
		{
			_value = value;
			_maxValue = maxValue;
		}
	}
}
