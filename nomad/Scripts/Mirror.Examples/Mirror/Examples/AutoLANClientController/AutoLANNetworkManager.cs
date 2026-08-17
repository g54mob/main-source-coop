using System.Linq;
using UnityEngine;

namespace Mirror.Examples.AutoLANClientController
{
	public class AutoLANNetworkManager : NetworkManager
	{
		public CanvasHUD canvasHUD;

		private NetworkIdentity[] copyOfOwnedObjects;

		public new static AutoLANNetworkManager singleton { get; private set; }

		public override void Awake()
		{
			base.Awake();
			singleton = this;
		}

		public override void OnValidate()
		{
			base.OnValidate();
		}

		public override void Start()
		{
			base.Start();
			if (canvasHUD == null)
			{
				canvasHUD = Object.FindAnyObjectByType<CanvasHUD>();
			}
		}

		public override void LateUpdate()
		{
			base.LateUpdate();
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
		}

		public override void ConfigureHeadlessFrameRate()
		{
			base.ConfigureHeadlessFrameRate();
		}

		public override void OnApplicationQuit()
		{
			base.OnApplicationQuit();
		}

		public override void ServerChangeScene(string newSceneName)
		{
			base.ServerChangeScene(newSceneName);
		}

		public override void OnServerChangeScene(string newSceneName)
		{
		}

		public override void OnServerSceneChanged(string sceneName)
		{
		}

		public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
		{
		}

		public override void OnClientSceneChanged()
		{
			base.OnClientSceneChanged();
		}

		public override void OnServerConnect(NetworkConnectionToClient conn)
		{
			canvasHUD.SetupInfoText("A client connected.");
		}

		public override void OnServerReady(NetworkConnectionToClient conn)
		{
			base.OnServerReady(conn);
		}

		public override void OnServerAddPlayer(NetworkConnectionToClient conn)
		{
			base.OnServerAddPlayer(conn);
		}

		public override void OnServerDisconnect(NetworkConnectionToClient conn)
		{
			copyOfOwnedObjects = conn.owned.ToArray();
			NetworkIdentity[] array = copyOfOwnedObjects;
			foreach (NetworkIdentity networkIdentity in array)
			{
				if (networkIdentity != conn.identity)
				{
					networkIdentity.RemoveClientAuthority();
				}
			}
			base.OnServerDisconnect(conn);
			canvasHUD.SetupInfoText("A client disconnected.");
		}

		public override void OnServerError(NetworkConnectionToClient conn, TransportError transportError, string message)
		{
			Debug.Log("OnServerError");
			canvasHUD.SetupInfoText("OnServerError: " + message);
		}

		public override void OnClientConnect()
		{
			base.OnClientConnect();
			canvasHUD.SetupInfoText("Connected to server.");
		}

		public override void OnClientDisconnect()
		{
			canvasHUD.SetupInfoText("Disconnected from server.");
		}

		public override void OnClientNotReady()
		{
		}

		public override void OnClientError(TransportError transportError, string message)
		{
			Debug.Log("OnClientError");
			canvasHUD.SetupInfoText("OnClientError: " + message);
		}

		public override void OnStartHost()
		{
			canvasHUD.SetupInfoText("Started Hosting.");
		}

		public override void OnStartServer()
		{
			canvasHUD.SetupInfoText("Started server.");
		}

		public override void OnStartClient()
		{
			canvasHUD.SetupInfoText("Client started.");
		}

		public override void OnStopHost()
		{
			canvasHUD.SetupInfoText("Hosting stopped.");
		}

		public override void OnStopServer()
		{
			canvasHUD.SetupInfoText("Server stopped.");
		}

		public override void OnStopClient()
		{
			canvasHUD.SetupInfoText("Client stopped.");
		}
	}
}
