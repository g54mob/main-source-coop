using System;
using Edgegap;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.EdgegapLobby
{
	public class UILobbyStatus : MonoBehaviour
	{
		private enum Status
		{
			Offline = 0,
			Server = 1,
			Host = 2,
			Client = 3
		}

		public GameObject[] ShowDisconnected;

		public GameObject[] ShowServer;

		public GameObject[] ShowHost;

		public GameObject[] ShowClient;

		public Button StopServer;

		public Button StopHost;

		public Button StopClient;

		public Text StatusText;

		private Status _status;

		private EdgegapLobbyKcpTransport _transport;

		private void Awake()
		{
			Refresh();
			StopServer.onClick.AddListener(delegate
			{
				NetworkManager.singleton.StopServer();
			});
			StopHost.onClick.AddListener(delegate
			{
				NetworkManager.singleton.StopHost();
			});
			StopClient.onClick.AddListener(delegate
			{
				NetworkManager.singleton.StopClient();
			});
		}

		private void Start()
		{
			_transport = (EdgegapLobbyKcpTransport)NetworkManager.singleton.transport;
		}

		private void Update()
		{
			Status status = GetStatus();
			if (_status != status)
			{
				_status = status;
				Refresh();
			}
			if ((bool)_transport)
			{
				StatusText.text = _transport.Status.ToString();
			}
			else
			{
				StatusText.text = "";
			}
		}

		private void Refresh()
		{
			switch (_status)
			{
			case Status.Offline:
				SetUI(ShowServer, active: false);
				SetUI(ShowHost, active: false);
				SetUI(ShowClient, active: false);
				SetUI(ShowDisconnected, active: true);
				break;
			case Status.Server:
				SetUI(ShowDisconnected, active: false);
				SetUI(ShowHost, active: false);
				SetUI(ShowClient, active: false);
				SetUI(ShowServer, active: true);
				break;
			case Status.Host:
				SetUI(ShowDisconnected, active: false);
				SetUI(ShowServer, active: false);
				SetUI(ShowClient, active: false);
				SetUI(ShowHost, active: true);
				break;
			case Status.Client:
				SetUI(ShowDisconnected, active: false);
				SetUI(ShowServer, active: false);
				SetUI(ShowHost, active: false);
				SetUI(ShowClient, active: true);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private void SetUI(GameObject[] gos, bool active)
		{
			for (int i = 0; i < gos.Length; i++)
			{
				gos[i].SetActive(active);
			}
		}

		private Status GetStatus()
		{
			if (NetworkServer.active && NetworkClient.active)
			{
				return Status.Host;
			}
			if (NetworkServer.active)
			{
				return Status.Server;
			}
			if (NetworkClient.active)
			{
				return Status.Client;
			}
			return Status.Offline;
		}
	}
}
