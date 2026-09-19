using System;

namespace Mirror.Examples.PlayerTest
{
	public class PlayerTestNetMan : NetworkManager
	{
		public new static PlayerTestNetMan singleton => (PlayerTestNetMan)NetworkManager.singleton;

		public override void Awake()
		{
			base.Awake();
		}

		public override void OnValidate()
		{
			base.OnValidate();
		}

		public override void Start()
		{
			base.Start();
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
			base.OnServerDisconnect(conn);
		}

		public override void OnServerError(NetworkConnectionToClient conn, TransportError transportError, string message)
		{
		}

		public override void OnServerTransportException(NetworkConnectionToClient conn, Exception exception)
		{
		}

		public override void OnClientConnect()
		{
			base.OnClientConnect();
		}

		public override void OnClientDisconnect()
		{
		}

		public override void OnClientNotReady()
		{
		}

		public override void OnClientError(TransportError transportError, string message)
		{
		}

		public override void OnClientTransportException(Exception exception)
		{
		}

		public override void OnStartHost()
		{
		}

		public override void OnStartServer()
		{
		}

		public override void OnStartClient()
		{
		}

		public override void OnStopHost()
		{
		}

		public override void OnStopServer()
		{
		}

		public override void OnStopClient()
		{
		}
	}
}
