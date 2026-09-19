using System.Collections;
using System.Collections.Generic;
using Mirror.Discovery;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.AutoLANClientController
{
	public class CanvasHUD : MonoBehaviour
	{
		public bool alwaysAutoStart;

		public AutoLANNetworkDiscovery networkDiscovery;

		private readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();

		public bool runAsPlayerHost;

		public GameObject PanelStart;

		public GameObject PanelStop;

		public Button buttonHost;

		public Button buttonServer;

		public Button buttonClient;

		public Button buttonStop;

		public Button buttonAuto;

		public Text infoText;

		public InputField inputFieldAddress;

		private void Start()
		{
			buttonHost.onClick.AddListener(ButtonHost);
			buttonServer.onClick.AddListener(ButtonServer);
			buttonClient.onClick.AddListener(ButtonClient);
			buttonStop.onClick.AddListener(ButtonStop);
			buttonAuto.onClick.AddListener(ButtonAuto);
			inputFieldAddress.text = NetworkManager.singleton.networkAddress;
			inputFieldAddress.onValueChanged.AddListener(delegate
			{
				OnValueChangedAddress();
			});
			if (networkDiscovery == null)
			{
				networkDiscovery = Object.FindAnyObjectByType<AutoLANNetworkDiscovery>();
			}
			if (alwaysAutoStart)
			{
				StartCoroutine(Waiter());
			}
		}

		public IEnumerator Waiter()
		{
			infoText.text = "Discovering servers..";
			discoveredServers.Clear();
			networkDiscovery.StartDiscovery();
			yield return new WaitForSeconds(3.1f);
			if (discoveredServers == null || discoveredServers.Count <= 0)
			{
				if (runAsPlayerHost)
				{
					infoText.text = "No Servers found, starting as Host.";
				}
				else
				{
					infoText.text = "No Servers found, starting as Server.";
				}
				yield return new WaitForSeconds(1f);
				discoveredServers.Clear();
				if (runAsPlayerHost)
				{
					NetworkManager.singleton.StartHost();
				}
				else
				{
					NetworkManager.singleton.StartServer();
				}
				networkDiscovery.AdvertiseServer();
			}
		}

		private void Connect(ServerResponse info)
		{
			infoText.text = "Connecting to: " + info.serverId;
			networkDiscovery.StopDiscovery();
			NetworkManager.singleton.StartClient(info.uri);
		}

		public void OnDiscoveredServer(ServerResponse info)
		{
			discoveredServers[info.serverId] = info;
			Connect(info);
		}

		public void ButtonHost()
		{
			SetupInfoText("Starting as host");
			discoveredServers.Clear();
			NetworkManager.singleton.StartHost();
			networkDiscovery.AdvertiseServer();
		}

		public void ButtonServer()
		{
			SetupInfoText("Starting as server.");
			discoveredServers.Clear();
			NetworkManager.singleton.StartServer();
			networkDiscovery.AdvertiseServer();
		}

		public void ButtonClient()
		{
			SetupInfoText("Starting as client.");
			discoveredServers.Clear();
			networkDiscovery.StartDiscovery();
		}

		public void ButtonStop()
		{
			SetupInfoText("Stopping.");
			if (NetworkServer.active && NetworkClient.isConnected)
			{
				NetworkManager.singleton.StopHost();
			}
			else if (NetworkClient.isConnected)
			{
				NetworkManager.singleton.StopClient();
			}
			else if (NetworkServer.active)
			{
				NetworkManager.singleton.StopServer();
			}
			networkDiscovery.StopDiscovery();
			SetupCanvas();
		}

		public void ButtonAuto()
		{
			SetupInfoText("Auto Starting.");
			StartCoroutine(Waiter());
		}

		public void SetupCanvas()
		{
			if (NetworkManager.singleton == null)
			{
				SetupInfoText("NetworkManager null");
			}
			else if (!NetworkClient.isConnected && !NetworkServer.active)
			{
				if (NetworkClient.active)
				{
					PanelStart.SetActive(value: false);
					PanelStop.SetActive(value: true);
				}
				else
				{
					PanelStart.SetActive(value: true);
					PanelStop.SetActive(value: false);
				}
			}
			else
			{
				PanelStart.SetActive(value: false);
				PanelStop.SetActive(value: true);
			}
		}

		public void SetupInfoText(string _info)
		{
			infoText.text = _info;
			SetupCanvas();
		}

		public void OnValueChangedAddress()
		{
			NetworkManager.singleton.networkAddress = inputFieldAddress.text;
		}
	}
}
