using Edgegap;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.EdgegapLobby
{
	public class UILobbyCreate : MonoBehaviour
	{
		public UILobbyList List;

		public Button CancelButton;

		public InputField LobbyName;

		public Text SlotCount;

		public Slider SlotSlider;

		public Button HostButton;

		public Button ServerButton;

		private EdgegapLobbyKcpTransport _transport => (EdgegapLobbyKcpTransport)NetworkManager.singleton.transport;

		private void Awake()
		{
			ValidateName();
			LobbyName.onValueChanged.AddListener(delegate
			{
				ValidateName();
			});
			CancelButton.onClick.AddListener(delegate
			{
				List.gameObject.SetActive(value: true);
				base.gameObject.SetActive(value: false);
			});
			SlotSlider.onValueChanged.AddListener(delegate(float arg0)
			{
				SlotCount.text = ((int)arg0).ToString();
			});
			HostButton.onClick.AddListener(delegate
			{
				base.gameObject.SetActive(value: false);
				_transport.SetServerLobbyParams(LobbyName.text, (int)SlotSlider.value);
				NetworkManager.singleton.StartHost();
			});
			ServerButton.onClick.AddListener(delegate
			{
				base.gameObject.SetActive(value: false);
				_transport.SetServerLobbyParams(LobbyName.text, (int)SlotSlider.value);
				NetworkManager.singleton.StartServer();
			});
		}

		private void ValidateName()
		{
			bool interactable = !string.IsNullOrWhiteSpace(LobbyName.text);
			HostButton.interactable = interactable;
			ServerButton.interactable = interactable;
		}
	}
}
