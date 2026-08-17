using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.Common
{
	public class CanvasNetworkManagerHUD : MonoBehaviour
	{
		[SerializeField]
		private GameObject startButtonsGroup;

		[SerializeField]
		private GameObject statusLabelsGroup;

		[SerializeField]
		private Button startHostButton;

		[SerializeField]
		private Button startServerOnlyButton;

		[SerializeField]
		private Button startClientButton;

		[SerializeField]
		private Button mainStopButton;

		[SerializeField]
		private Text mainStopButtonText;

		[SerializeField]
		private Button secondaryStopButton;

		[SerializeField]
		private Text statusText;

		[SerializeField]
		private InputField inputNetworkAddress;

		[SerializeField]
		private InputField inputPort;

		private void Start()
		{
			inputNetworkAddress.text = NetworkManager.singleton.networkAddress;
			GetPort();
			RegisterListeners();
			CheckWebGLPlayer();
		}

		private void RegisterListeners()
		{
			startHostButton.onClick.AddListener(OnClickStartHostButton);
			startServerOnlyButton.onClick.AddListener(OnClickStartServerButton);
			startClientButton.onClick.AddListener(OnClickStartClientButton);
			mainStopButton.onClick.AddListener(OnClickMainStopButton);
			secondaryStopButton.onClick.AddListener(OnClickSecondaryStopButton);
			inputNetworkAddress.onValueChanged.AddListener(delegate
			{
				OnNetworkAddressChange();
			});
			inputPort.onValueChanged.AddListener(delegate
			{
				OnPortChange();
			});
		}

		private void CheckWebGLPlayer()
		{
			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				startHostButton.interactable = false;
				startServerOnlyButton.interactable = false;
			}
		}

		private void RefreshHUD()
		{
			if (!NetworkServer.active && !NetworkClient.isConnected)
			{
				StartButtons();
			}
			else
			{
				StatusLabelsAndStopButtons();
			}
		}

		private void StartButtons()
		{
			if (!NetworkClient.active)
			{
				statusLabelsGroup.SetActive(value: false);
				startButtonsGroup.SetActive(value: true);
			}
			else
			{
				ShowConnectingStatus();
			}
		}

		private void StatusLabelsAndStopButtons()
		{
			startButtonsGroup.SetActive(value: false);
			statusLabelsGroup.SetActive(value: true);
			if (NetworkServer.active && NetworkClient.active)
			{
				statusText.text = $"<b>Host</b>: running via {Transport.active}";
				mainStopButtonText.text = "Stop Client";
			}
			else if (NetworkServer.active)
			{
				statusText.text = $"<b>Server</b>: running via {Transport.active}";
				mainStopButtonText.text = "Stop Server";
			}
			else if (NetworkClient.isConnected)
			{
				statusText.text = $"<b>Client</b>: connected to {NetworkManager.singleton.networkAddress} via {Transport.active}";
				mainStopButtonText.text = "Stop Client";
			}
			secondaryStopButton.gameObject.SetActive(NetworkServer.active && NetworkClient.active);
		}

		private void ShowConnectingStatus()
		{
			startButtonsGroup.SetActive(value: false);
			statusLabelsGroup.SetActive(value: true);
			secondaryStopButton.gameObject.SetActive(value: false);
			statusText.text = "Connecting to " + NetworkManager.singleton.networkAddress + "..";
			mainStopButtonText.text = "Cancel Connection Attempt";
		}

		private void OnClickStartHostButton()
		{
			NetworkManager.singleton.StartHost();
		}

		private void OnClickStartServerButton()
		{
			NetworkManager.singleton.StartServer();
		}

		private void OnClickStartClientButton()
		{
			NetworkManager.singleton.StartClient();
		}

		private void OnClickMainStopButton()
		{
			if (NetworkClient.active)
			{
				NetworkManager.singleton.StopClient();
			}
			else
			{
				NetworkManager.singleton.StopServer();
			}
		}

		private void OnClickSecondaryStopButton()
		{
			NetworkManager.singleton.StopHost();
		}

		private void OnNetworkAddressChange()
		{
			NetworkManager.singleton.networkAddress = inputNetworkAddress.text;
		}

		private void OnPortChange()
		{
			SetPort(inputPort.text);
		}

		private void SetPort(string _port)
		{
			if (Transport.active is PortTransport portTransport && ushort.TryParse(_port, out var result))
			{
				portTransport.Port = result;
			}
		}

		private void GetPort()
		{
			if (Transport.active is PortTransport portTransport)
			{
				inputPort.text = portTransport.Port.ToString();
			}
		}

		private void Update()
		{
			RefreshHUD();
		}

		private void Reset()
		{
			if (!Object.FindAnyObjectByType<NetworkManager>())
			{
				Debug.LogError("This component requires a NetworkManager component to be present in the scene. Please add!");
			}
		}
	}
}
