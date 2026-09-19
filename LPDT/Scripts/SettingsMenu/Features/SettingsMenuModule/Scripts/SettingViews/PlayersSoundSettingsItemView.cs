using PlayerCustomization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class PlayersSoundSettingsItemView : PlayersSoundSettingsItemViewBase
	{
		[SerializeField]
		private SettingsSlider _soundsVolumeSlider;

		[SerializeField]
		private TMP_Text _playerVolumeOwner;

		[SerializeField]
		private Selectable _prioritySelectable;

		public override Selectable PrioritySelectable => _prioritySelectable;

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
			_soundsVolumeSlider.Slider.maxValue = 1f;
			_soundsVolumeSlider.Slider.value = value;
			UpdateVolumeText(value);
		}

		public override void SetPlayerInfo(PlayerCustomizationSlotData playerCustomization)
		{
			_playerVolumeOwner.text = playerCustomization.Nickname;
		}

		private void OnSoundChanged(float volume)
		{
			OnValueChanged?.Invoke(volume);
			UpdateVolumeText(volume);
		}

		private void UpdateVolumeText(float value)
		{
			float num = value * 200f;
			_soundsVolumeSlider.Value.SetText((int)num + "%");
		}
	}
}
