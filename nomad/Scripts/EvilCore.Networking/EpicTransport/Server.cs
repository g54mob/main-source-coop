using System;
using System.Collections.Generic;
using Epic.OnlineServices;
using Epic.OnlineServices.P2P;
using Mirror;
using UnityEngine;

namespace EpicTransport
{
	public class Server : Common
	{
		private BidirectionalDictionary<ProductUserId, int> epicToMirrorIds;

		private Dictionary<ProductUserId, SocketId> epicToSocketIds;

		private int maxConnections;

		private int nextConnectionID;

		private event Action<int> OnConnected;

		private event Action<int, byte[], int> OnReceivedData;

		private event Action<int> OnDisconnected;

		private event Action<int, Exception> OnReceivedError;

		public static Server CreateServer(EosTransport transport, int maxConnections)
		{
			Server server = new Server(transport, maxConnections);
			server.OnConnected += delegate(int id)
			{
				transport.OnServerConnected(id);
			};
			server.OnDisconnected += delegate(int id)
			{
				transport.OnServerDisconnected(id);
			};
			server.OnReceivedData += delegate(int id, byte[] data, int channel)
			{
				transport.OnServerDataReceived(id, new ArraySegment<byte>(data), channel);
			};
			server.OnReceivedError += delegate(int id, Exception exception)
			{
				transport.OnServerError(id, TransportError.Unexpected, exception.ToString());
			};
			if (!EOSSDKComponent.Initialized)
			{
				Debug.LogError("EOS not initialized.");
			}
			return server;
		}

		private Server(EosTransport transport, int maxConnections)
			: base(transport)
		{
			this.maxConnections = maxConnections;
			epicToMirrorIds = new BidirectionalDictionary<ProductUserId, int>();
			epicToSocketIds = new Dictionary<ProductUserId, SocketId>();
			nextConnectionID = 1;
		}

		protected override void OnNewConnection(ref OnIncomingConnectionRequestInfo result)
		{
			if (ignoreAllMessages)
			{
				return;
			}
			if (result.SocketId.HasValue && deadSockets.Contains(result.SocketId.Value.SocketName))
			{
				if (_loggedDeadSockets.Add(result.SocketId.Value.SocketName))
				{
					Debug.LogWarning("[EosTransport.Server] Incoming connection request from dead socket " + result.SocketId.Value.SocketName + " — ignoring (stale EOS P2P session, benign).");
				}
			}
			else
			{
				AcceptConnectionOptions options = new AcceptConnectionOptions
				{
					LocalUserId = EOSSDKComponent.LocalUserProductId,
					RemoteUserId = result.RemoteUserId,
					SocketId = result.SocketId
				};
				EOSSDKComponent.GetP2PInterface().AcceptConnection(ref options);
			}
		}

		protected override void OnReceiveInternalData(InternalMessages type, ProductUserId clientUserId, SocketId socketId)
		{
			if (ignoreAllMessages)
			{
				return;
			}
			switch (type)
			{
			case InternalMessages.CONNECT:
			{
				if (epicToMirrorIds.Count >= maxConnections)
				{
					Debug.LogError($"[EosTransport.Server] Reached max connections ({maxConnections}), rejecting PUID {clientUserId}");
					SendInternal(clientUserId, socketId, InternalMessages.DISCONNECT);
					break;
				}
				if (epicToMirrorIds.TryGetValue(clientUserId, out var value2))
				{
					Debug.LogWarning($"[EosTransport.Server] Duplicate CONNECT from PUID {clientUserId} (existing connId {value2}); replacing registration.");
					this.OnDisconnected(value2);
					epicToMirrorIds.Remove(clientUserId);
					epicToSocketIds.Remove(clientUserId);
				}
				SendInternal(clientUserId, socketId, InternalMessages.ACCEPT_CONNECT);
				int num = nextConnectionID++;
				epicToMirrorIds.Add(clientUserId, num);
				epicToSocketIds.Add(clientUserId, socketId);
				_loggedUnknownPuids.Remove(clientUserId.ToString());
				this.OnConnected(num);
				string text = clientUserId.ToString();
				Debug.Log($"[EosTransport.Server] PUID {text} connected, assigned connId {num}. (Total: {epicToMirrorIds.Count}/{maxConnections})");
				break;
			}
			case InternalMessages.DISCONNECT:
			{
				if (epicToMirrorIds.TryGetValue(clientUserId, out var value))
				{
					this.OnDisconnected(value);
					epicToMirrorIds.Remove(clientUserId);
					epicToSocketIds.Remove(clientUserId);
					Debug.Log($"[EosTransport.Server] PUID {clientUserId} disconnected (was connId {value}). (Remaining: {epicToMirrorIds.Count})");
				}
				else
				{
					this.OnReceivedError(-1, new Exception("ERROR Unknown Product User ID"));
				}
				break;
			}
			default:
				Debug.Log("Received unknown message type");
				break;
			}
		}

		protected override void OnReceiveData(byte[] data, ProductUserId clientUserId, int channel)
		{
			if (ignoreAllMessages)
			{
				return;
			}
			if (epicToMirrorIds.TryGetValue(clientUserId, out var value))
			{
				this.OnReceivedData(value, data, channel);
				return;
			}
			if (!epicToSocketIds.TryGetValue(clientUserId, out var value2) || string.IsNullOrEmpty(value2.SocketName))
			{
				_lastSocketIdByPuid.TryGetValue(clientUserId, out value2);
			}
			CloseP2PSessionWithUser(clientUserId, value2);
			string text = clientUserId.ToString();
			if (_loggedUnknownPuids.Add(text))
			{
				Debug.LogWarning($"[EosTransport.Server] Data received from unknown PUID {text} on ch{channel} — closing stale P2P session (benign late EOS packet). Subsequent packets from this PUID suppressed.");
				this.OnReceivedError(-1, new Exception("ERROR Unknown product ID"));
			}
		}

		public void Disconnect(int connectionId)
		{
			if (epicToMirrorIds.TryGetValue(connectionId, out var value))
			{
				epicToSocketIds.TryGetValue(value, out var value2);
				SendInternal(value, value2, InternalMessages.DISCONNECT);
				epicToMirrorIds.Remove(value);
				epicToSocketIds.Remove(value);
			}
			else
			{
				Debug.LogWarning("Trying to disconnect unknown connection id: " + connectionId);
			}
		}

		public void Shutdown()
		{
			ignoreAllMessages = true;
			try
			{
				List<KeyValuePair<ProductUserId, SocketId>> list = new List<KeyValuePair<ProductUserId, SocketId>>(epicToMirrorIds.Count);
				foreach (KeyValuePair<ProductUserId, int> epicToMirrorId in epicToMirrorIds)
				{
					epicToSocketIds.TryGetValue(epicToMirrorId.Key, out var value);
					list.Add(new KeyValuePair<ProductUserId, SocketId>(epicToMirrorId.Key, value));
				}
				foreach (KeyValuePair<ProductUserId, SocketId> item in list)
				{
					ProductUserId key = item.Key;
					SocketId value2 = item.Value;
					try
					{
						SendInternal(key, value2, InternalMessages.DISCONNECT);
					}
					catch (Exception ex)
					{
						Debug.LogWarning($"[EosTransport.Server] DISCONNECT send to {key} failed: {ex.Message}");
					}
					epicToMirrorIds.Remove(key);
					epicToSocketIds.Remove(key);
					WaitForClose(key, value2);
				}
				ReceiveData();
			}
			finally
			{
				Dispose();
			}
		}

		public void SendAll(int connectionId, byte[] data, int channelId)
		{
			if (epicToMirrorIds.TryGetValue(connectionId, out var value))
			{
				epicToSocketIds.TryGetValue(value, out var value2);
				Send(value, value2, data, (byte)channelId);
			}
		}

		public string ServerGetClientAddress(int connectionId)
		{
			if (epicToMirrorIds.TryGetValue(connectionId, out var value))
			{
				return value.ToString();
			}
			return string.Empty;
		}

		protected override void OnConnectionFailed(ProductUserId remoteId, SocketId failedSocketId)
		{
			if (!ignoreAllMessages)
			{
				if (!epicToMirrorIds.TryGetValue(remoteId, out var value))
				{
					Debug.LogWarning($"[EosTransport.Server] OnConnectionFailed for PUID {remoteId} (not in registry, failedSocketId={failedSocketId.SocketName}). Ignoring.");
					return;
				}
				if (!epicToSocketIds.TryGetValue(remoteId, out var value2) || value2.SocketName != failedSocketId.SocketName)
				{
					Debug.LogWarning($"[EosTransport.Server] Ignoring stale OnConnectionFailed for PUID {remoteId} " + $"(failedSocketId={failedSocketId.SocketName}, registeredSocketId={value2.SocketName}, connId={value}). " + "Live connection preserved.");
					return;
				}
				this.OnDisconnected(value);
				Debug.LogWarning($"[EosTransport.Server] OnConnectionFailed for PUID {remoteId} (connId={value}, socketId={failedSocketId.SocketName}). Removing.");
				epicToMirrorIds.Remove(remoteId);
				epicToSocketIds.Remove(remoteId);
			}
		}
	}
}
