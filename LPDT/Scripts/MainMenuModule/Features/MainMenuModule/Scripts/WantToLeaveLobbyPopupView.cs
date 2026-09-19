using UnityEngine;
using UnityEngine.UI;

namespace Features.MainMenuModule.Scripts
{
	public class WantToLeaveLobbyPopupView : WantToLeaveLobbyPopupViewBase
	{
		[SerializeField]
		private GameObject _container;

		[SerializeField]
		private Button _yesButton;

		[SerializeField]
		private Button _noButton;

		public override void SetVisible(bool isVisible)
		{
			if (_container != null)
			{
				_container.SetActive(isVisible);
			}
			else
			{
				base.gameObject.SetActive(isVisible);
			}
		}

		protected override void OnEnable()
		{
			_yesButton.onClick.AddListener(base.InvokeYesClicked);
			_noButton.onClick.AddListener(base.InvokeNoClicked);
		}

		protected override void OnDisable()
		{
			_yesButton.onClick.RemoveListener(base.InvokeYesClicked);
			_noButton.onClick.RemoveListener(base.InvokeNoClicked);
		}
	}
}
