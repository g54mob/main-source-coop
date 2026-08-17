using Mirror.RemoteCalls;
using UnityEngine;

namespace Mirror.Examples.CouchCoop
{
	public class CouchPlayerManager : NetworkBehaviour
	{
		public CanvasScript canvasScript;

		public GameObject[] playerPrefabs;

		public int totalCouchPlayers;

		public KeyCode[] playerKeyJump;

		public KeyCode[] playerKeyLeft;

		public KeyCode[] playerKeyRight;

		private readonly SyncList<GameObject> couchPlayersList = new SyncList<GameObject>();

		public override void OnStartAuthority()
		{
			canvasScript = Object.FindAnyObjectByType<CanvasScript>();
			canvasScript.couchPlayerManager = this;
		}

		[Command]
		public void CmdAddPlayer()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void Mirror.Examples.CouchCoop.CouchPlayerManager::CmdAddPlayer()", -1689906266, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[Command]
		public void CmdRemovePlayer()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void Mirror.Examples.CouchCoop.CouchPlayerManager::CmdRemovePlayer()", -778285693, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		public CouchPlayerManager()
		{
			InitSyncObject(couchPlayersList);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdAddPlayer()
		{
			if (totalCouchPlayers >= playerKeyJump.Length - 1)
			{
				Debug.Log(base.name + " - No controls setup for further players.");
				return;
			}
			totalCouchPlayers++;
			Transform transform = NetworkManager.startPositions[Random.Range(0, NetworkManager.startPositions.Count)];
			GameObject gameObject = Object.Instantiate(playerPrefabs[0], transform.position, transform.rotation);
			gameObject.GetComponent<CouchPlayer>().NetworkplayerNumber = totalCouchPlayers;
			NetworkServer.Spawn(gameObject, base.connectionToClient);
			couchPlayersList.Add(gameObject);
		}

		protected static void InvokeUserCode_CmdAddPlayer(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdAddPlayer called on client.");
			}
			else
			{
				((CouchPlayerManager)obj).UserCode_CmdAddPlayer();
			}
		}

		protected void UserCode_CmdRemovePlayer()
		{
			if (totalCouchPlayers <= 0)
			{
				Debug.Log(base.name + " - No players to remove for that connection.");
				return;
			}
			totalCouchPlayers--;
			NetworkServer.Destroy(couchPlayersList[couchPlayersList.Count - 1]);
			couchPlayersList.RemoveAt(couchPlayersList.Count - 1);
		}

		protected static void InvokeUserCode_CmdRemovePlayer(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRemovePlayer called on client.");
			}
			else
			{
				((CouchPlayerManager)obj).UserCode_CmdRemovePlayer();
			}
		}

		static CouchPlayerManager()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(CouchPlayerManager), "System.Void Mirror.Examples.CouchCoop.CouchPlayerManager::CmdAddPlayer()", InvokeUserCode_CmdAddPlayer, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(CouchPlayerManager), "System.Void Mirror.Examples.CouchCoop.CouchPlayerManager::CmdRemovePlayer()", InvokeUserCode_CmdRemovePlayer, requiresAuthority: true);
		}
	}
}
