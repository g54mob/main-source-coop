using System;
using UnityEngine;

namespace HSVPicker
{
	[DefaultExecutionOrder(0)]
	public class ColorPicker : MonoBehaviour
	{
		private float _hue;

		private float _saturation;

		private float _brightness;

		[SerializeField]
		private Color _color = Color.red;

		[Header("Setup")]
		public ColorPickerSetup Setup;

		[Header("Event")]
		public ColorChangedEvent onValueChanged = new ColorChangedEvent();

		public HSVChangedEvent onHSVChanged = new HSVChangedEvent();

		public Color CurrentColor
		{
			get
			{
				return _color;
			}
			set
			{
				if (!(CurrentColor == value))
				{
					_color = value;
					RGBChanged();
					SendChangedEvent();
				}
			}
		}

		public float H
		{
			get
			{
				return _hue;
			}
			set
			{
				if (_hue != value)
				{
					_hue = value;
					HSVChanged();
					SendChangedEvent();
				}
			}
		}

		public float S
		{
			get
			{
				return _saturation;
			}
			set
			{
				if (_saturation != value)
				{
					_saturation = value;
					HSVChanged();
					SendChangedEvent();
				}
			}
		}

		public float V
		{
			get
			{
				return _brightness;
			}
			set
			{
				if (_brightness != value)
				{
					_brightness = value;
					HSVChanged();
					SendChangedEvent();
				}
			}
		}

		public float R
		{
			get
			{
				return _color.r;
			}
			set
			{
				if (_color.r != value)
				{
					_color.r = value;
					RGBChanged();
					SendChangedEvent();
				}
			}
		}

		public float G
		{
			get
			{
				return _color.g;
			}
			set
			{
				if (_color.g != value)
				{
					_color.g = value;
					RGBChanged();
					SendChangedEvent();
				}
			}
		}

		public float B
		{
			get
			{
				return _color.b;
			}
			set
			{
				if (_color.b != value)
				{
					_color.b = value;
					RGBChanged();
					SendChangedEvent();
				}
			}
		}

		private float A
		{
			get
			{
				return _color.a;
			}
			set
			{
				if (_color.a != value)
				{
					_color.a = value;
					SendChangedEvent();
				}
			}
		}

		private void Awake()
		{
			Regenerate();
		}

		private void OnEnable()
		{
			if (Setup.RegenerateOnOpen)
			{
				Regenerate();
			}
		}

		private void Regenerate()
		{
			Setup.AlphaSlidiers.Toggle(Setup.ShowAlpha);
			Setup.ColorToggleElement.Toggle(Setup.ShowColorSliderToggle);
			Setup.RgbSliders.Toggle(Setup.ShowRgb);
			Setup.HsvSliders.Toggle(Setup.ShowHsv);
			Setup.ColorBox.Toggle(Setup.ShowColorBox);
			HandleHeaderSetting(Setup.ShowHeader);
			UpdateColorToggleText();
			RGBChanged();
		}

		private void RGBChanged()
		{
			HsvColor hsvColor = HSVUtil.ConvertRgbToHsv(CurrentColor);
			_hue = hsvColor.normalizedH;
			_saturation = hsvColor.normalizedS;
			_brightness = hsvColor.normalizedV;
		}

		private void HSVChanged()
		{
			Color color = HSVUtil.ConvertHsvToRgb(_hue * 360f, _saturation, _brightness, _color.a);
			_color = color;
		}

		private void SendChangedEvent()
		{
			onValueChanged.Invoke(CurrentColor);
			onHSVChanged.Invoke(_hue, _saturation, _brightness);
		}

		public void AssignColor(ColorValues type, float value)
		{
			switch (type)
			{
			case ColorValues.R:
				R = value;
				break;
			case ColorValues.G:
				G = value;
				break;
			case ColorValues.B:
				B = value;
				break;
			case ColorValues.A:
				A = value;
				break;
			case ColorValues.Hue:
				H = value;
				break;
			case ColorValues.Saturation:
				S = value;
				break;
			case ColorValues.Value:
				V = value;
				break;
			}
		}

		public void AssignColor(Color color)
		{
			CurrentColor = color;
		}

		public float GetValue(ColorValues type)
		{
			return type switch
			{
				ColorValues.R => R, 
				ColorValues.G => G, 
				ColorValues.B => B, 
				ColorValues.A => A, 
				ColorValues.Hue => H, 
				ColorValues.Saturation => S, 
				ColorValues.Value => V, 
				_ => throw new NotImplementedException(""), 
			};
		}

		public void ToggleColorSliders()
		{
			Setup.ShowHsv = !Setup.ShowHsv;
			Setup.ShowRgb = !Setup.ShowRgb;
			Setup.HsvSliders.Toggle(Setup.ShowHsv);
			Setup.RgbSliders.Toggle(Setup.ShowRgb);
			onHSVChanged.Invoke(_hue, _saturation, _brightness);
			UpdateColorToggleText();
		}

		private void UpdateColorToggleText()
		{
			if (!(Setup.SliderToggleButtonText == null))
			{
				if (Setup.ShowRgb)
				{
					Setup.SliderToggleButtonText.text = "RGB";
				}
				if (Setup.ShowHsv)
				{
					Setup.SliderToggleButtonText.text = "HSV";
				}
			}
		}

		private void HandleHeaderSetting(ColorPickerSetup.ColorHeaderShowing setupShowHeader)
		{
			if (setupShowHeader == ColorPickerSetup.ColorHeaderShowing.Hide)
			{
				Setup.ColorHeader.Toggle(active: false);
				return;
			}
			Setup.ColorHeader.Toggle(active: true);
			Setup.ColorPreview.Toggle(setupShowHeader != ColorPickerSetup.ColorHeaderShowing.ShowColorCode);
			Setup.ColorCode.Toggle(setupShowHeader != ColorPickerSetup.ColorHeaderShowing.ShowColor);
		}
	}
}
