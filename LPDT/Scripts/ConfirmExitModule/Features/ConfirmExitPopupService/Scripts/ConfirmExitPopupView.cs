using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.ConfirmExitPopupService.Scripts
{
	public class ConfirmExitPopupView : ConfirmExitPopupViewBase
	{
		[SerializeField]
		private GameObject _container;

		[SerializeField]
		private Button _yesButton;

		[SerializeField]
		private Button _noButton;

		[SerializeField]
		private TMP_Text _title;

		[SerializeField]
		private TMP_Text _description;

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

		public override void SetTitleText(string text)
		{
			_title.text = text;
		}

		public override void SetDescriptionText(string text)
		{
			_description.text = text;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			_yesButton.onClick.AddListener(base.InvokeYesClicked);
			_noButton.onClick.AddListener(base.InvokeNoClicked);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_yesButton.onClick.RemoveListener(base.InvokeYesClicked);
			_noButton.onClick.RemoveListener(base.InvokeNoClicked);
		}
	}
}
