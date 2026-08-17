using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.Chat
{
	public class LoginUI : MonoBehaviour
	{
		[Header("UI Elements")]
		[SerializeField]
		internal InputField networkAddressInput;

		[SerializeField]
		internal InputField usernameInput;

		[SerializeField]
		internal Button hostButton;

		[SerializeField]
		internal Button clientButton;

		[SerializeField]
		internal Text errorText;

		public static LoginUI instance;

		private string originalNetworkAddress;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			if (string.IsNullOrWhiteSpace(NetworkManager.singleton.networkAddress))
			{
				NetworkManager.singleton.networkAddress = "localhost";
			}
			originalNetworkAddress = NetworkManager.singleton.networkAddress;
		}

		private void Update()
		{
			if (string.IsNullOrWhiteSpace(NetworkManager.singleton.networkAddress))
			{
				NetworkManager.singleton.networkAddress = originalNetworkAddress;
			}
			if (networkAddressInput.text != NetworkManager.singleton.networkAddress)
			{
				networkAddressInput.text = NetworkManager.singleton.networkAddress;
			}
		}

		public void ToggleButtons(string username)
		{
			hostButton.interactable = !string.IsNullOrWhiteSpace(username);
			clientButton.interactable = !string.IsNullOrWhiteSpace(username);
		}
	}
}
