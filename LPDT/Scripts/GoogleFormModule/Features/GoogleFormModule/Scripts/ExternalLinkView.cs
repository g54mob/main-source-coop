using UnityEngine;
using UnityEngine.UI;

namespace Features.GoogleFormModule.Scripts
{
	public class ExternalLinkView : ExternalLinkViewBase
	{
		[SerializeField]
		private Button _externalLinkButton;

		[SerializeField]
		private ExternalLinkType _externalLinkType;

		protected override void OnEnable()
		{
			base.OnEnable();
			_externalLinkButton.onClick.AddListener(OnExternalLinkButtonClicked);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_externalLinkButton.onClick.RemoveListener(OnExternalLinkButtonClicked);
		}

		private void OnExternalLinkButtonClicked()
		{
			InvokeExternalLinkButtonClick(_externalLinkType);
		}
	}
}
