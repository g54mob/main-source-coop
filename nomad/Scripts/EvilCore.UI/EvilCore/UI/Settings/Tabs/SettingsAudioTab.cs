using System.Collections.Generic;
using EvilCore.Localization;
using EvilCore.Networking;
using EvilCore.Settings;
using UnityEngine;
using VContainer;

namespace EvilCore.UI.Settings.Tabs
{
	public class SettingsAudioTab : MonoBehaviour
	{
		[Header("Volume")]
		[SerializeField]
		private OptionSelector masterVolumeSelector;

		[SerializeField]
		private OptionSelector musicVolumeSelector;

		[SerializeField]
		private OptionSelector sfxVolumeSelector;

		[SerializeField]
		private OptionSelector ambienceVolumeSelector;

		[Header("Voice Chat")]
		[SerializeField]
		private OptionSelector voiceMuteSelector;

		[Header("Devices")]
		[SerializeField]
		private OptionSelector inputDeviceSelector;

		[SerializeField]
		private OptionSelector outputDeviceSelector;

		[Inject]
		private ISettingsManager _settingsManager;

		[Inject]
		private ILocalizationService _localizationService;

		[Inject]
		private IVoiceDeviceController _voiceDevices;

		private bool _isRefreshing;

		private bool _initialized;

		private readonly List<VoiceAudioDevice> _inputDevices = new List<VoiceAudioDevice>();

		private readonly List<VoiceAudioDevice> _outputDevices = new List<VoiceAudioDevice>();

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
				SubscribeDeviceEvents();
				_voiceDevices?.RefreshAudioDevices();
				Refresh();
			}
		}

		private void OnDisable()
		{
			UnsubscribeDeviceEvents();
		}

		public void Initialize()
		{
			if (!_initialized)
			{
				_initialized = true;
				ConfigureSelectors();
				BindListeners();
				SubscribeDeviceEvents();
				if (_localizationService != null)
				{
					_localizationService.OnLocaleChanged += OnLocaleChanged;
				}
				_voiceDevices?.RefreshAudioDevices();
				Refresh();
			}
		}

		private void OnLocaleChanged()
		{
			voiceMuteSelector?.SetOptions(new List<string>
			{
				Loc("@settings.off", "Off"),
				Loc("@settings.on", "On")
			});
			_isRefreshing = true;
			voiceMuteSelector?.SetValueWithoutNotify((_settingsManager != null && _settingsManager.VoiceMuted) ? 1 : 0);
			RefreshDeviceSelectors();
			_isRefreshing = false;
		}

		private void OnDestroy()
		{
			if (masterVolumeSelector != null)
			{
				masterVolumeSelector.OnRangeValueChanged -= OnMasterVolumeChanged;
			}
			if (musicVolumeSelector != null)
			{
				musicVolumeSelector.OnRangeValueChanged -= OnMusicVolumeChanged;
			}
			if (sfxVolumeSelector != null)
			{
				sfxVolumeSelector.OnRangeValueChanged -= OnSfxVolumeChanged;
			}
			if (ambienceVolumeSelector != null)
			{
				ambienceVolumeSelector.OnRangeValueChanged -= OnAmbienceVolumeChanged;
			}
			if (voiceMuteSelector != null)
			{
				voiceMuteSelector.OnValueChanged -= OnVoiceMuteChanged;
			}
			if (inputDeviceSelector != null)
			{
				inputDeviceSelector.OnValueChanged -= OnInputDeviceChanged;
			}
			if (outputDeviceSelector != null)
			{
				outputDeviceSelector.OnValueChanged -= OnOutputDeviceChanged;
			}
			UnsubscribeDeviceEvents();
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged -= OnLocaleChanged;
			}
		}

		public void Refresh()
		{
			if (_settingsManager != null)
			{
				_isRefreshing = true;
				masterVolumeSelector?.SetRangeValueWithoutNotify(_settingsManager.MasterVolume * 100f);
				musicVolumeSelector?.SetRangeValueWithoutNotify(_settingsManager.MusicVolume * 100f);
				sfxVolumeSelector?.SetRangeValueWithoutNotify(_settingsManager.SfxVolume * 100f);
				ambienceVolumeSelector?.SetRangeValueWithoutNotify(_settingsManager.AmbienceVolume * 100f);
				voiceMuteSelector?.SetValueWithoutNotify(_settingsManager.VoiceMuted ? 1 : 0);
				RefreshDeviceSelectors();
				_isRefreshing = false;
			}
		}

		private void ConfigureSelectors()
		{
			string suffix = Loc("@unit.percent", "%");
			masterVolumeSelector?.SetRange(0f, 100f, 5f, suffix);
			musicVolumeSelector?.SetRange(0f, 100f, 5f, suffix);
			sfxVolumeSelector?.SetRange(0f, 100f, 5f, suffix);
			ambienceVolumeSelector?.SetRange(0f, 100f, 5f, suffix);
			voiceMuteSelector?.SetOptions(new List<string>
			{
				Loc("@settings.off", "Off"),
				Loc("@settings.on", "On")
			});
		}

		private void BindListeners()
		{
			if (masterVolumeSelector != null)
			{
				masterVolumeSelector.OnRangeValueChanged += OnMasterVolumeChanged;
			}
			if (musicVolumeSelector != null)
			{
				musicVolumeSelector.OnRangeValueChanged += OnMusicVolumeChanged;
			}
			if (sfxVolumeSelector != null)
			{
				sfxVolumeSelector.OnRangeValueChanged += OnSfxVolumeChanged;
			}
			if (ambienceVolumeSelector != null)
			{
				ambienceVolumeSelector.OnRangeValueChanged += OnAmbienceVolumeChanged;
			}
			if (voiceMuteSelector != null)
			{
				voiceMuteSelector.OnValueChanged += OnVoiceMuteChanged;
			}
			if (inputDeviceSelector != null)
			{
				inputDeviceSelector.OnValueChanged += OnInputDeviceChanged;
			}
			if (outputDeviceSelector != null)
			{
				outputDeviceSelector.OnValueChanged += OnOutputDeviceChanged;
			}
		}

		private void OnMasterVolumeChanged(float value)
		{
			if (!_isRefreshing)
			{
				_settingsManager.SetMasterVolume(value / 100f);
			}
		}

		private void OnMusicVolumeChanged(float value)
		{
			if (!_isRefreshing)
			{
				_settingsManager.SetMusicVolume(value / 100f);
			}
		}

		private void OnSfxVolumeChanged(float value)
		{
			if (!_isRefreshing)
			{
				_settingsManager.SetSfxVolume(value / 100f);
			}
		}

		private void OnAmbienceVolumeChanged(float value)
		{
			if (!_isRefreshing)
			{
				_settingsManager.SetAmbienceVolume(value / 100f);
			}
		}

		private void OnVoiceMuteChanged(int index)
		{
			if (!_isRefreshing)
			{
				_settingsManager.SetVoiceMuted(index == 1);
			}
		}

		private void SubscribeDeviceEvents()
		{
			if (_voiceDevices != null)
			{
				_voiceDevices.OnAudioDevicesChanged += OnAudioDevicesChanged;
			}
		}

		private void UnsubscribeDeviceEvents()
		{
			if (_voiceDevices != null)
			{
				_voiceDevices.OnAudioDevicesChanged -= OnAudioDevicesChanged;
			}
		}

		private void OnAudioDevicesChanged()
		{
			_isRefreshing = true;
			RefreshDeviceSelectors();
			_isRefreshing = false;
		}

		private void RefreshDeviceSelectors()
		{
			bool ready = _voiceDevices != null;
			RefreshInputDevices(ready);
			RefreshOutputDevices(ready);
		}

		private static bool IsVirtualDevice(string deviceName)
		{
			if (string.IsNullOrEmpty(deviceName))
			{
				return true;
			}
			string text = deviceName.ToLowerInvariant();
			if (!text.Contains("virtual") && !text.Contains("voice changer") && !text.Contains("screenshare") && !text.Contains("screen share") && !text.Contains("cable input") && !text.Contains("cable output") && !text.Contains("voicemod") && !text.Contains("vb-audio") && !text.Contains("vb audio"))
			{
				if (text.Contains("mix"))
				{
					return text.Contains("stereo");
				}
				return false;
			}
			return true;
		}

		private void RefreshInputDevices(bool ready)
		{
			if (inputDeviceSelector == null)
			{
				return;
			}
			_inputDevices.Clear();
			IReadOnlyList<VoiceAudioDevice> readOnlyList = (ready ? _voiceDevices.InputDevices : null);
			if (readOnlyList == null || readOnlyList.Count == 0)
			{
				inputDeviceSelector.SetOptions(new List<string> { Loc("@settings.no_devices", "No devices available") });
				inputDeviceSelector.SetInteractable(interactable: false);
				return;
			}
			inputDeviceSelector.SetInteractable(interactable: true);
			List<string> list = new List<string>();
			string text = _settingsManager?.InputDeviceName ?? "";
			string activeInputDeviceId = _voiceDevices.ActiveInputDeviceId;
			foreach (VoiceAudioDevice item in readOnlyList)
			{
				if (item.IsValid)
				{
					if (item.IsDefault)
					{
						_inputDevices.Insert(0, item);
						list.Insert(0, Loc("@settings.default_device", "Default"));
					}
					else if (!IsVirtualDevice(item.Name))
					{
						_inputDevices.Add(item);
						list.Add(item.Name);
					}
				}
			}
			if (list.Count == 0)
			{
				inputDeviceSelector.SetOptions(new List<string> { Loc("@settings.no_devices", "No devices available") });
				inputDeviceSelector.SetInteractable(interactable: false);
				return;
			}
			int valueWithoutNotify = 0;
			for (int i = 0; i < _inputDevices.Count; i++)
			{
				if (!string.IsNullOrEmpty(text) && _inputDevices[i].Name == text)
				{
					valueWithoutNotify = i;
					break;
				}
				if (!string.IsNullOrEmpty(activeInputDeviceId) && _inputDevices[i].Id == activeInputDeviceId)
				{
					valueWithoutNotify = i;
				}
			}
			inputDeviceSelector.SetOptions(list);
			inputDeviceSelector.SetValueWithoutNotify(valueWithoutNotify);
		}

		private void RefreshOutputDevices(bool ready)
		{
			if (outputDeviceSelector == null)
			{
				return;
			}
			_outputDevices.Clear();
			IReadOnlyList<VoiceAudioDevice> readOnlyList = (ready ? _voiceDevices.OutputDevices : null);
			if (readOnlyList == null || readOnlyList.Count == 0)
			{
				outputDeviceSelector.SetOptions(new List<string> { Loc("@settings.no_devices", "No devices available") });
				outputDeviceSelector.SetInteractable(interactable: false);
				return;
			}
			outputDeviceSelector.SetInteractable(interactable: true);
			List<string> list = new List<string>();
			string text = _settingsManager?.OutputDeviceName ?? "";
			string activeOutputDeviceId = _voiceDevices.ActiveOutputDeviceId;
			foreach (VoiceAudioDevice item in readOnlyList)
			{
				if (item.IsValid)
				{
					if (item.IsDefault)
					{
						_outputDevices.Insert(0, item);
						list.Insert(0, Loc("@settings.default_device", "Default"));
					}
					else if (!IsVirtualDevice(item.Name))
					{
						_outputDevices.Add(item);
						list.Add(item.Name);
					}
				}
			}
			if (list.Count == 0)
			{
				outputDeviceSelector.SetOptions(new List<string> { Loc("@settings.no_devices", "No devices available") });
				outputDeviceSelector.SetInteractable(interactable: false);
				return;
			}
			int valueWithoutNotify = 0;
			for (int i = 0; i < _outputDevices.Count; i++)
			{
				if (!string.IsNullOrEmpty(text) && _outputDevices[i].Name == text)
				{
					valueWithoutNotify = i;
					break;
				}
				if (!string.IsNullOrEmpty(activeOutputDeviceId) && _outputDevices[i].Id == activeOutputDeviceId)
				{
					valueWithoutNotify = i;
				}
			}
			outputDeviceSelector.SetOptions(list);
			outputDeviceSelector.SetValueWithoutNotify(valueWithoutNotify);
		}

		private void OnInputDeviceChanged(int index)
		{
			if (!_isRefreshing && index >= 0 && index < _inputDevices.Count)
			{
				VoiceAudioDevice voiceAudioDevice = _inputDevices[index];
				_settingsManager?.SetInputDeviceName(voiceAudioDevice.Name);
				_voiceDevices?.SetInputDevice(voiceAudioDevice.Id);
			}
		}

		private void OnOutputDeviceChanged(int index)
		{
			if (!_isRefreshing && index >= 0 && index < _outputDevices.Count)
			{
				VoiceAudioDevice voiceAudioDevice = _outputDevices[index];
				_settingsManager?.SetOutputDeviceName(voiceAudioDevice.Name);
				_voiceDevices?.SetOutputDevice(voiceAudioDevice.Id);
			}
		}
	}
}
