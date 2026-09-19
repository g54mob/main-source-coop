using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class SoundSettingsItemView : SoundSettingsItemViewBase
	{
		[SerializeField]
		private SoundSettingsType _soundSettingsType;

		[SerializeField]
		private SettingsSlider _soundsVolumeSlider;

		protected override void OnEnable()
		{
			base.OnEnable();
			_soundsVolumeSlider.OnValueChanged += OnSoundChanged;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_soundsVolumeSlider.OnValueChanged -= OnSoundChanged;
		}

		public override void SetValue(float value)
		{
			_soundsVolumeSlider.Slider.maxValue = 100f;
			_soundsVolumeSlider.Slider.value = value;
			UpdateVolumeText(value);
		}

		private void OnSoundChanged(float volume)
		{
			OnValueChanged?.Invoke(volume, _soundSettingsType);
			UpdateVolumeText(volume);
		}

		private void UpdateVolumeText(float value)
		{
			_soundsVolumeSlider.Value.SetText((int)value + "%");
		}

		public override SoundSettingsType GetSoundType()
		{
			return _soundSettingsType;
		}
	}
}
