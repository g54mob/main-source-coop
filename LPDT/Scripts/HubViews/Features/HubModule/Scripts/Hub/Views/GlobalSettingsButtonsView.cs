using UnityEngine;
using UnityEngine.UI;

namespace Features.HubModule.Scripts.Hub.Views
{
	public class GlobalSettingsButtonsView : GlobalSettingsButtonsViewBase
	{
		[SerializeField]
		private Button _settingsButton;

		[SerializeField]
		private bool _isMenu;

		public override bool IsMenu => _isMenu;

		protected override void OnEnable()
		{
			_settingsButton.onClick.AddListener(base.InvokeOnClickSettings);
		}

		protected override void OnDisable()
		{
			_settingsButton.onClick.RemoveListener(base.InvokeOnClickSettings);
		}
	}
}
