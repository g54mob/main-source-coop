using UnityEngine;
using UnityEngine.UI;

namespace Features.MainMenuModule.Scripts
{
	public class ThankPlayingPopupView : ThankPlayingPopupViewBase
	{
		[SerializeField]
		private Button _backButton;

		protected override void OnEnable()
		{
			base.OnEnable();
			_backButton.onClick.AddListener(base.InvokeCloseClicked);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_backButton.onClick.RemoveListener(base.InvokeCloseClicked);
		}
	}
}
