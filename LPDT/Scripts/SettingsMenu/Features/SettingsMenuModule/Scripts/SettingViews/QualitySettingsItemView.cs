using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class QualitySettingsItemView : QualitySettingsItemViewBase
	{
		[SerializeField]
		private SettingsMultipleItems _qualitySettings;

		private List<string> _qualityLocalizationKeys = new List<string>();

		protected override void OnEnable()
		{
			base.OnEnable();
			_qualitySettings.OnValueChanged += OnValueChangedHandler;
			UpdateLocalization();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_qualitySettings.OnValueChanged -= OnValueChangedHandler;
		}

		private void OnValueChangedHandler(int index)
		{
			OnValueChanged?.Invoke(index);
			UpdateLocalization();
		}

		public override void SetQualityLocalizationKeys(List<string> qualityLocalizationKeys)
		{
			_qualityLocalizationKeys = qualityLocalizationKeys;
			_qualitySettings.SetValues(_qualityLocalizationKeys.ToList());
			UpdateLocalization();
		}

		public override void SetQualityIndex(int index)
		{
			_qualitySettings.SetIndex(index);
		}

		private void UpdateLocalization()
		{
			if (_qualityLocalizationKeys.Any())
			{
				_qualitySettings.Value.SetText(_qualityLocalizationKeys[_qualitySettings.ValueIndex]);
			}
		}
	}
}
