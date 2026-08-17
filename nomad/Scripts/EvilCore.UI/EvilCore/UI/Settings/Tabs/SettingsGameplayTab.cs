using System.Collections.Generic;
using EvilCore.Localization;
using EvilCore.Settings;
using UnityEngine;
using VContainer;

namespace EvilCore.UI.Settings.Tabs
{
	public class SettingsGameplayTab : MonoBehaviour
	{
		[Header("Sensitivity")]
		[SerializeField]
		private OptionSelector sensitivityXSelector;

		[SerializeField]
		private OptionSelector sensitivityYSelector;

		[Header("Mouse")]
		[SerializeField]
		private OptionSelector invertMouseYSelector;

		[Header("Field of View")]
		[SerializeField]
		private OptionSelector fovSelector;

		[Header("Motion Blur")]
		[SerializeField]
		private OptionSelector motionBlurSelector;

		[Inject]
		private ISettingsManager _settingsManager;

		[Inject]
		private ILocalizationService _localizationService;

		private bool _isRefreshing;

		private bool _initialized;

		private string Loc(string key, string english)
		{
			if (_localizationService == null)
			{
				return english;
			}
			return _localizationService.Localize(key);
		}

		private void OnEnable()
		{
			if (_initialized)
			{
				Refresh();
			}
		}

		public void Initialize()
		{
			if (!_initialized)
			{
				_initialized = true;
				ConfigureSelectors();
				BindListeners();
				if (_localizationService != null)
				{
					_localizationService.OnLocaleChanged += OnLocaleChanged;
				}
				Refresh();
			}
		}

		private void OnDestroy()
		{
			if (sensitivityXSelector != null)
			{
				sensitivityXSelector.OnRangeValueChanged -= OnSensitivityXChanged;
			}
			if (sensitivityYSelector != null)
			{
				sensitivityYSelector.OnRangeValueChanged -= OnSensitivityYChanged;
			}
			if (invertMouseYSelector != null)
			{
				invertMouseYSelector.OnValueChanged -= OnInvertMouseYChanged;
			}
			if (fovSelector != null)
			{
				fovSelector.OnRangeValueChanged -= OnFovChanged;
			}
			if (motionBlurSelector != null)
			{
				motionBlurSelector.OnValueChanged -= OnMotionBlurChanged;
			}
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged -= OnLocaleChanged;
			}
		}

		private void OnLocaleChanged()
		{
			invertMouseYSelector?.SetOptions(new List<string>
			{
				Loc("@settings.off", "Off"),
				Loc("@settings.on", "On")
			});
			motionBlurSelector?.SetOptions(new List<string>
			{
				Loc("@settings.off", "Off"),
				Loc("@settings.on", "On")
			});
			Refresh();
		}

		public void Refresh()
		{
			if (_settingsManager != null)
			{
				_isRefreshing = true;
				sensitivityXSelector?.SetRangeValueWithoutNotify(_settingsManager.SensitivityX);
				sensitivityYSelector?.SetRangeValueWithoutNotify(_settingsManager.SensitivityY);
				invertMouseYSelector?.SetValueWithoutNotify(_settingsManager.InvertMouseY ? 1 : 0);
				fovSelector?.SetRangeValueWithoutNotify(_settingsManager.Fov);
				motionBlurSelector?.SetValueWithoutNotify(_settingsManager.MotionBlur ? 1 : 0);
				_isRefreshing = false;
			}
		}

		private void ConfigureSelectors()
		{
			sensitivityXSelector?.SetRange(1f, 10f, 1f);
			sensitivityYSelector?.SetRange(1f, 10f, 1f);
			invertMouseYSelector?.SetOptions(new List<string>
			{
				Loc("@settings.off", "Off"),
				Loc("@settings.on", "On")
			});
			fovSelector?.SetRange(60f, 90f, 5f, Loc("@unit.degree", "°"));
			motionBlurSelector?.SetOptions(new List<string>
			{
				Loc("@settings.off", "Off"),
				Loc("@settings.on", "On")
			});
		}

		private void BindListeners()
		{
			if (sensitivityXSelector != null)
			{
				sensitivityXSelector.OnRangeValueChanged += OnSensitivityXChanged;
			}
			if (sensitivityYSelector != null)
			{
				sensitivityYSelector.OnRangeValueChanged += OnSensitivityYChanged;
			}
			if (invertMouseYSelector != null)
			{
				invertMouseYSelector.OnValueChanged += OnInvertMouseYChanged;
			}
			if (fovSelector != null)
			{
				fovSelector.OnRangeValueChanged += OnFovChanged;
			}
			if (motionBlurSelector != null)
			{
				motionBlurSelector.OnValueChanged += OnMotionBlurChanged;
			}
		}

		private void OnSensitivityXChanged(float value)
		{
			if (!_isRefreshing)
			{
				_settingsManager.SetSensitivity(value, _settingsManager.SensitivityY);
			}
		}

		private void OnSensitivityYChanged(float value)
		{
			if (!_isRefreshing)
			{
				_settingsManager.SetSensitivity(_settingsManager.SensitivityX, value);
			}
		}

		private void OnInvertMouseYChanged(int index)
		{
			if (!_isRefreshing)
			{
				_settingsManager.SetInvertMouseY(index == 1);
			}
		}

		private void OnFovChanged(float value)
		{
			if (!_isRefreshing)
			{
				_settingsManager.SetFov(value);
			}
		}

		private void OnMotionBlurChanged(int index)
		{
			if (!_isRefreshing)
			{
				_settingsManager.SetMotionBlur(index == 1);
			}
		}
	}
}
