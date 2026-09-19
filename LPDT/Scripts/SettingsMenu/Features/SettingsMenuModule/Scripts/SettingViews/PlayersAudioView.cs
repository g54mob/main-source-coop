using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class PlayersAudioView : PlayersAudioViewBase
	{
		[SerializeField]
		private Transform _settingsItemContainer;

		[SerializeField]
		private SelectableSettingsTab _selectableSettingsTab;

		public override Transform SettingsItemContainer => _settingsItemContainer;

		public override SelectableSettingsTab SelectableSettingsTab => _selectableSettingsTab;
	}
}
