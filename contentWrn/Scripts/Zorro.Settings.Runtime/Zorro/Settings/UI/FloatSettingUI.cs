using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Zorro.Settings.UI
{
	public class FloatSettingUI : SettingInputUICell
	{
		public TMP_InputField inputField;

		public Slider slider;

		public override void Setup(Setting setting, ISettingHandler settingHandler)
		{
			FloatSetting floatSetting = setting as FloatSetting;
			if (floatSetting != null)
			{
				slider.maxValue = floatSetting.MaxValue;
				slider.minValue = floatSetting.MinValue;
				slider.value = floatSetting.Value;
				inputField.SetTextWithoutNotify(floatSetting.Expose(floatSetting.Value));
				inputField.onValueChanged.AddListener(OnChanged);
				slider.onValueChanged.AddListener(OnSliderChanged);
			}
			void OnChanged(string str)
			{
				if (float.TryParse(str, out var result))
				{
					floatSetting.SetValue(result, settingHandler);
					inputField.SetTextWithoutNotify(floatSetting.Expose(floatSetting.Value));
					slider.SetValueWithoutNotify(floatSetting.Value);
				}
			}
			void OnSliderChanged(float value)
			{
				value = Mathf.Clamp(value, floatSetting.MinValue, floatSetting.MaxValue);
				floatSetting.SetValue(value, settingHandler);
				inputField.SetTextWithoutNotify(floatSetting.Expose(floatSetting.Value));
				slider.SetValueWithoutNotify(floatSetting.Value);
			}
		}
	}
}
