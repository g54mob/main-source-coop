using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.MainMenuModule.Scripts
{
	public class JoinCrewPopupView : JoinCrewPopupViewBase
	{
		[SerializeField]
		private GameObject _container;

		[SerializeField]
		private Button _closeButton;

		[SerializeField]
		private TMP_InputField _roomCodeInput;

		[SerializeField]
		private Button _joinViaCodeButton;

		[SerializeField]
		private GameObject _joinViaSteamSection;

		[SerializeField]
		private Button _joinViaSteamButton;

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

		public override void SetSteamJoinVisible(bool isVisible)
		{
			if (_joinViaSteamSection != null)
			{
				_joinViaSteamSection.SetActive(isVisible);
			}
		}

		public override void SetJoinViaCodeInteractable(bool interactable)
		{
			if (_joinViaCodeButton != null)
			{
				_joinViaCodeButton.interactable = interactable;
			}
		}

		public override void SetRoomIdentifier(string roomIdentifier)
		{
			if (_roomCodeInput != null)
			{
				_roomCodeInput.SetTextWithoutNotify(roomIdentifier);
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (_closeButton != null)
			{
				_closeButton.onClick.AddListener(base.InvokeCloseClicked);
			}
			if (_joinViaCodeButton != null)
			{
				_joinViaCodeButton.onClick.AddListener(base.InvokeJoinViaCodeClicked);
			}
			if (_joinViaSteamButton != null)
			{
				_joinViaSteamButton.onClick.AddListener(base.InvokeJoinViaSteamClicked);
			}
			if (_roomCodeInput != null)
			{
				_roomCodeInput.onValueChanged.AddListener(base.InvokeRoomIdentifierChanged);
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			if (_closeButton != null)
			{
				_closeButton.onClick.RemoveListener(base.InvokeCloseClicked);
			}
			if (_joinViaCodeButton != null)
			{
				_joinViaCodeButton.onClick.RemoveListener(base.InvokeJoinViaCodeClicked);
			}
			if (_joinViaSteamButton != null)
			{
				_joinViaSteamButton.onClick.RemoveListener(base.InvokeJoinViaSteamClicked);
			}
			if (_roomCodeInput != null)
			{
				_roomCodeInput.onValueChanged.RemoveListener(base.InvokeRoomIdentifierChanged);
			}
		}

		public override void ClearInputField()
		{
			_roomCodeInput.text = string.Empty;
		}
	}
}
