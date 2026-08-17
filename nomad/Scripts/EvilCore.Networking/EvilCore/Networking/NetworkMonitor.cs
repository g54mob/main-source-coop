using System.Collections.Generic;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

namespace EvilCore.Networking
{
	public class NetworkMonitor : NetworkBehaviour
	{
		private class ClientStats
		{
			public double lastPingTime;

			public float currentPing;

			public Queue<float> lastPings = new Queue<float>();

			public float jitter;

			public const int MAX_PING_HISTORY = 30;
		}

		private Dictionary<int, ClientStats> clientStats = new Dictionary<int, ClientStats>();

		private float pingInterval = 1f;

		private float lastPingCheck;

		private float currentPing;

		private float currentJitter;

		private float minPing = 3.4028235E+38f;

		private float maxPing = -3.4028235E+38f;

		private float avgPing;

		private float packetLoss;

		private void Update()
		{
			if (!base.isServer || !(Time.time - lastPingCheck > pingInterval))
			{
				return;
			}
			lastPingCheck = Time.time;
			foreach (KeyValuePair<int, NetworkConnectionToClient> connection in NetworkServer.connections)
			{
				if (connection.Value.connectionId != 0)
				{
					SendPingToClient(connection.Value);
				}
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdPingResponse(NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void EvilCore.Networking.NetworkMonitor::CmdPingResponse(Mirror.NetworkConnectionToClient)", 1253315970, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		private void SendPingToClient(NetworkConnectionToClient conn)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void EvilCore.Networking.NetworkMonitor::SendPingToClient(Mirror.NetworkConnectionToClient)' called when server was not active");
				return;
			}
			if (conn.connectionId == 0)
			{
				TargetUpdateStats(conn, 0f, 0f);
				return;
			}
			if (!clientStats.ContainsKey(conn.connectionId))
			{
				clientStats[conn.connectionId] = new ClientStats();
			}
			clientStats[conn.connectionId].lastPingTime = NetworkTime.time;
			TargetPingRequest(conn);
		}

		[TargetRpc]
		private void TargetPingRequest(NetworkConnection target)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendTargetRPCInternal(target, "System.Void EvilCore.Networking.NetworkMonitor::TargetPingRequest(Mirror.NetworkConnection)", 972627685, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[TargetRpc]
		private void TargetUpdateStats(NetworkConnection target, float ping, float jitter)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteFloat(ping);
			writer.WriteFloat(jitter);
			SendTargetRPCInternal(target, "System.Void EvilCore.Networking.NetworkMonitor::TargetUpdateStats(Mirror.NetworkConnection,System.Single,System.Single)", 2021045492, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdPingResponse__NetworkConnectionToClient(NetworkConnectionToClient sender)
		{
			if (sender == null)
			{
				return;
			}
			if (sender.connectionId == 0)
			{
				TargetUpdateStats(sender, 0f, 0f);
				return;
			}
			if (!this.clientStats.ContainsKey(sender.connectionId))
			{
				this.clientStats[sender.connectionId] = new ClientStats();
			}
			ClientStats clientStats = this.clientStats[sender.connectionId];
			float num = (float)((NetworkTime.time - clientStats.lastPingTime) * 1000.0);
			if (num < 0f || num > 1000f)
			{
				return;
			}
			clientStats.currentPing = num;
			clientStats.lastPings.Enqueue(num);
			if (clientStats.lastPings.Count > 30)
			{
				clientStats.lastPings.Dequeue();
			}
			if (clientStats.lastPings.Count > 1)
			{
				float num2 = 0f;
				float num3 = clientStats.currentPing;
				foreach (float lastPing in clientStats.lastPings)
				{
					num2 += Mathf.Abs(lastPing - num3);
				}
				clientStats.jitter = num2 / (float)clientStats.lastPings.Count;
			}
			TargetUpdateStats(sender, num, clientStats.jitter);
		}

		protected static void InvokeUserCode_CmdPingResponse__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdPingResponse called on client.");
			}
			else
			{
				((NetworkMonitor)obj).UserCode_CmdPingResponse__NetworkConnectionToClient(senderConnection);
			}
		}

		protected void UserCode_TargetPingRequest__NetworkConnection(NetworkConnection target)
		{
			if (base.isServer && base.isClient)
			{
				currentPing = 0f;
				currentJitter = 0f;
			}
			else
			{
				CmdPingResponse();
			}
		}

		protected static void InvokeUserCode_TargetPingRequest__NetworkConnection(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC TargetPingRequest called on server.");
			}
			else
			{
				((NetworkMonitor)obj).UserCode_TargetPingRequest__NetworkConnection(null);
			}
		}

		protected void UserCode_TargetUpdateStats__NetworkConnection__Single__Single(NetworkConnection target, float ping, float jitter)
		{
			currentPing = ping;
			currentJitter = jitter;
			minPing = Mathf.Min(minPing, ping);
			maxPing = Mathf.Max(maxPing, ping);
			avgPing = avgPing * 0.95f + ping * 0.05f;
			if (ping > 200f)
			{
				packetLoss += 0.1f;
			}
			else
			{
				packetLoss = Mathf.Max(0f, packetLoss - 0.05f);
			}
			packetLoss = Mathf.Clamp(packetLoss, 0f, 100f);
		}

		protected static void InvokeUserCode_TargetUpdateStats__NetworkConnection__Single__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC TargetUpdateStats called on server.");
			}
			else
			{
				((NetworkMonitor)obj).UserCode_TargetUpdateStats__NetworkConnection__Single__Single(null, reader.ReadFloat(), reader.ReadFloat());
			}
		}

		static NetworkMonitor()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkMonitor), "System.Void EvilCore.Networking.NetworkMonitor::CmdPingResponse(Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdPingResponse__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkMonitor), "System.Void EvilCore.Networking.NetworkMonitor::TargetPingRequest(Mirror.NetworkConnection)", InvokeUserCode_TargetPingRequest__NetworkConnection);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkMonitor), "System.Void EvilCore.Networking.NetworkMonitor::TargetUpdateStats(Mirror.NetworkConnection,System.Single,System.Single)", InvokeUserCode_TargetUpdateStats__NetworkConnection__Single__Single);
		}
	}
}
