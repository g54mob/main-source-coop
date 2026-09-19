using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.MainMenuModule.Scripts
{
	public class MatchmakingPreviewPopupView : MatchmakingPreviewPopupViewBase
	{
		[SerializeField]
		private GameObject _container;

		[SerializeField]
		private GameObject _infoSection;

		[SerializeField]
		private GameObject _noGamesSection;

		[SerializeField]
		private TMP_Text _hostNameText;

		[SerializeField]
		private TMP_Text _playersText;

		[SerializeField]
		private TMP_Text _regionText;

		[SerializeField]
		private TMP_Text _pingText;

		[SerializeField]
		private TMP_Text _foundGameText;

		[SerializeField]
		private TMP_Text _noGamesText;

		[SerializeField]
		private TMP_Text _noGamesDescriptionText;

		[SerializeField]
		private Button _joinButton;

		[SerializeField]
		private Button _searchAgainButton;

		[SerializeField]
		private Button _closeButton;

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
			if (isVisible)
			{
				ResetSelectableAnimator(_joinButton);
				ResetSelectableAnimator(_searchAgainButton);
				ResetSelectableAnimator(_closeButton);
			}
		}

		private static void ResetSelectableAnimator(Button button)
		{
			if (!(button == null) && button.TryGetComponent<Animator>(out var component) && component.isActiveAndEnabled)
			{
				component.Rebind();
				component.Update(0f);
			}
		}

		public override void ShowSessionInfo(string header, string hostName, string playersText, string region, string ping)
		{
			if (_infoSection != null)
			{
				_infoSection.SetActive(value: true);
			}
			if (_noGamesSection != null)
			{
				_noGamesSection.SetActive(value: false);
			}
			if (_joinButton != null)
			{
				_joinButton.gameObject.SetActive(value: true);
			}
			if (_hostNameText != null)
			{
				_hostNameText.text = hostName;
			}
			if (_playersText != null)
			{
				_playersText.text = playersText;
			}
			if (_regionText != null)
			{
				_regionText.text = region;
			}
			if (_pingText != null)
			{
				_pingText.text = ping;
			}
			if (_foundGameText != null)
			{
				_foundGameText.text = header;
			}
		}

		public override void ShowNoGamesFound(string header, string description)
		{
			if (_infoSection != null)
			{
				_infoSection.SetActive(value: false);
			}
			if (_noGamesSection != null)
			{
				_noGamesSection.SetActive(value: true);
			}
			if (_joinButton != null)
			{
				_joinButton.gameObject.SetActive(value: false);
			}
			if (_noGamesText != null)
			{
				_noGamesText.text = header;
			}
			if (_noGamesDescriptionText != null)
			{
				_noGamesDescriptionText.text = description;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (_joinButton != null)
			{
				_joinButton.onClick.AddListener(base.InvokeJoinClicked);
			}
			if (_searchAgainButton != null)
			{
				_searchAgainButton.onClick.AddListener(base.InvokeSearchAgainClicked);
			}
			if (_closeButton != null)
			{
				_closeButton.onClick.AddListener(base.InvokeCloseClicked);
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			if (_joinButton != null)
			{
				_joinButton.onClick.RemoveListener(base.InvokeJoinClicked);
			}
			if (_searchAgainButton != null)
			{
				_searchAgainButton.onClick.RemoveListener(base.InvokeSearchAgainClicked);
			}
			if (_closeButton != null)
			{
				_closeButton.onClick.RemoveListener(base.InvokeCloseClicked);
			}
		}
	}
}
