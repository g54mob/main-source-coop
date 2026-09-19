using UnityEngine;
using UnityEngine.UI;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class BackMenuSettingsView : BackMenuSettingsViewBase
	{
		[SerializeField]
		private Button _backButton;

		protected override void OnEnable()
		{
			base.OnEnable();
			_backButton.onClick.AddListener(InvokeOnBackLobbyButtonClick);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_backButton.onClick.RemoveListener(InvokeOnBackLobbyButtonClick);
		}

		private void InvokeOnBackLobbyButtonClick()
		{
			OnBackLobbyButtonClick?.Invoke();
		}
	}
}
