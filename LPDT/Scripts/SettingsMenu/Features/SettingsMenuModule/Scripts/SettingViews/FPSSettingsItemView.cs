using System.Collections.Generic;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class FPSSettingsItemView : FPSSettingsItemViewBase
	{
		[SerializeField]
		private SettingsFillableMultipleValues _fpsSlider;

		protected override void OnEnable()
		{
			base.OnEnable();
			_fpsSlider.OnValueChanged += OnValueChanged;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_fpsSlider.OnValueChanged -= OnValueChanged;
		}

		public override void SetValueIndex(int index)
		{
			_fpsSlider.SetIndex(index);
		}

		public override void SetFpsValues(List<string> fpsValues)
		{
			_fpsSlider.SetValues(fpsValues);
		}

		public override void SetFpsLimitText(int value)
		{
			if (_fpsSlider.Value != null)
			{
				_fpsSlider.Value.SetText(value.ToString());
			}
		}
	}
}
