using UnityEngine;
using UnityEngine.UI;

namespace Features.SettingsMenuModule.Scripts.HubSettings
{
	public class HubSettingsButtonsView : HubSettingsButtonsViewBase
	{
		[SerializeField]
		private Button _settingsButton;

		protected override void OnEnable()
		{
			_settingsButton.onClick.AddListener(base.InvokeOnClickSettings);
		}

		protected override void OnDisable()
		{
			_settingsButton.onClick.RemoveListener(base.InvokeOnClickSettings);
		}

		public override void SetOpenSettingsButtonInteractable(bool interactable)
		{
			_settingsButton.interactable = interactable;
		}
	}
}
