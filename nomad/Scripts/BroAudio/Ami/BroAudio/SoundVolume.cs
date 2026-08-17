using System;
using System.Collections.Generic;
using Ami.BroAudio.Runtime;
using Ami.BroAudio.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Ami.BroAudio
{
	[HelpURL("https://man572142s-organization.gitbook.io/broaudio/core-features/no-code-components/sound-volume")]
	[AddComponentMenu("BroAudio/SoundVolume")]
	public class SoundVolume : MonoBehaviour
	{
		public delegate(SliderType sliderType, bool allowBoost) GetSliderSetting();

		[Serializable]
		public class Setting
		{
			public static class NameOf
			{
				public const string AudioType = "_audioType";

				public const string Volume = "_volume";

				public const string Slider = "_slider";
			}

			[SerializeField]
			private BroAudioType _audioType = BroAudioType.Music;

			[SerializeField]
			private float _volume = 1f;

			[SerializeField]
			private Slider _slider;

			private GetSliderSetting _onGetSliderSetting;

			private float _originalVolume;

			private Dictionary<BroAudioType, float> _systemOriginalVolumes;

			private SliderType SliderType => _onGetSliderSetting().sliderType;

			private bool AllowBoost => _onGetSliderSetting().allowBoost;

			public bool IsInit => _onGetSliderSetting != null;

			public void Init(GetSliderSetting onGetSliderSetting)
			{
				_onGetSliderSetting = onGetSliderSetting;
			}

			public void ApplyVolumeToSystem(float fadeTime)
			{
				BroAudio.SetVolume(_audioType, _volume, fadeTime);
			}

			public void SetVolumeToSlider(bool notify)
			{
				if ((bool)_slider)
				{
					float num = Utility.VolumeToSlider(SliderType, _volume, AllowBoost);
					num = (float)Math.Round(num, 3);
					if (notify)
					{
						_slider.normalizedValue = num;
					}
					else
					{
						_slider.SetValueWithoutNotify(Mathf.Lerp(_slider.minValue, _slider.maxValue, num));
					}
				}
			}

			public void RecordOrigin()
			{
				_originalVolume = _volume;
				_systemOriginalVolumes = new Dictionary<BroAudioType, float>();
				Utility.ForeachConcreteAudioType(new OriginVolumeRecorder
				{
					TargetType = _audioType,
					SystemOriginalVolumes = _systemOriginalVolumes
				});
			}

			public void ResetToOrigin(float fadeTime)
			{
				_volume = _originalVolume;
				SetVolumeToSlider(notify: false);
				if (_systemOriginalVolumes == null)
				{
					return;
				}
				foreach (KeyValuePair<BroAudioType, float> systemOriginalVolume in _systemOriginalVolumes)
				{
					BroAudio.SetVolume(systemOriginalVolume.Key, systemOriginalVolume.Value, fadeTime);
				}
				_systemOriginalVolumes = null;
			}

			public void AddSliderListener()
			{
				if ((bool)_slider)
				{
					_slider.onValueChanged.AddListener(OnValueChanged);
				}
			}

			public void RemoveSliderListener()
			{
				if ((bool)_slider)
				{
					_slider.onValueChanged.RemoveListener(OnValueChanged);
				}
			}

			private void OnValueChanged(float sliderValue)
			{
				BroAudio.SetVolume(vol: _volume = Utility.SliderToVolume(SliderType, _slider.normalizedValue, AllowBoost), audioType: _audioType);
			}
		}

		public static class NameOf
		{
			public const string ApplyOnEnable = "_applyOnEnable";

			public const string OnlyApplyOnce = "_onlyApplyOnce";

			public const string ResetOnDisable = "_resetOnDisable";

			public const string FadeTime = "_fadeTime";

			public const string SliderType = "_sliderType";

			public const string AllowBoost = "_allowBoost";

			public const string Settings = "_settings";
		}

		public const int RoundingDigits = 3;

		[SerializeField]
		private bool _applyOnEnable;

		[SerializeField]
		private bool _onlyApplyOnce;

		[SerializeField]
		private bool _resetOnDisable;

		[SerializeField]
		private float _fadeTime;

		[SerializeField]
		private SliderType _sliderType = SliderType.BroVolume;

		[SerializeField]
		private bool _allowBoost;

		[SerializeField]
		private Setting[] _settings;

		private bool _hasApplyOnce;

		public void SetFadeTime(float fadeTime)
		{
			_fadeTime = fadeTime;
		}

		private void OnEnable()
		{
			Setting[] settings = _settings;
			foreach (Setting setting in settings)
			{
				if (!setting.IsInit)
				{
					setting.Init(() => (sliderType: _sliderType, allowBoost: _allowBoost));
				}
				setting.AddSliderListener();
				if (_resetOnDisable)
				{
					setting.RecordOrigin();
				}
				if (_applyOnEnable && (!_onlyApplyOnce || !_hasApplyOnce))
				{
					setting.ApplyVolumeToSystem(_fadeTime);
					setting.SetVolumeToSlider(notify: false);
					_hasApplyOnce = true;
				}
			}
		}

		private void OnDisable()
		{
			Setting[] settings = _settings;
			foreach (Setting setting in settings)
			{
				if (_resetOnDisable)
				{
					setting.ResetToOrigin(_fadeTime);
				}
				setting.RemoveSliderListener();
			}
		}
	}
}
