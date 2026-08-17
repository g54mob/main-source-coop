using System;
using System.Threading;
using System.Threading.Tasks;
using Epic.OnlineServices;
using Epic.OnlineServices.P2P;
using UnityEngine;

namespace EpicTransport
{
	public class Client : Common
	{
		public SocketId socketId;

		public ProductUserId serverId;

		private TimeSpan ConnectionTimeout;

		public bool isConnecting;

		public string hostAddress = "";

		private ProductUserId hostProductId;

		private TaskCompletionSource<Task> connectedComplete;

		private CancellationTokenSource cancelToken;

		public bool Connected { get; private set; }

		public bool Error { get; private set; }

		private event Action<byte[], int> OnReceivedData;

		private event Action OnConnected;

		public event Action OnDisconnected;

		private Client(EosTransport transport)
			: base(transport)
		{
			ConnectionTimeout = TimeSpan.FromSeconds(Math.Max(1, transport.timeout));
		}

		public static Client CreateClient(EosTransport transport, string host)
		{
			Client client = new Client(transport);
			client.hostAddress = host;
			client.socketId = new SocketId
			{
				SocketName = RandomString.Generate(20)
			};
			client.OnConnected += delegate
			{
				transport.OnClientConnected?.Invoke();
			};
			client.OnDisconnected += delegate
			{
				transport.OnClientDisconnected?.Invoke();
			};
			client.OnReceivedData += delegate(byte[] data, int channel)
			{
				transport.OnClientDataReceived?.Invoke(new ArraySegment<byte>(data), channel);
			};
			return client;
		}

		public async void Connect(string host)
		{
			cancelToken = new CancellationTokenSource();
			try
			{
				hostProductId = ProductUserId.FromString(host);
				serverId = hostProductId;
				connectedComplete = new TaskCompletionSource<Task>();
				OnConnected += SetConnectedComplete;
				SendInternal(hostProductId, socketId, InternalMessages.CONNECT);
				Task connectedCompleteTask = connectedComplete.Task;
				if (await Task.WhenAny(connectedCompleteTask, Task.Delay(ConnectionTimeout, cancelToken.Token)) != connectedCompleteTask)
				{
					OnConnected -= SetConnectedComplete;
					if (!cancelToken.IsCancellationRequested)
					{
						Debug.LogError("Connection to " + host + " timed out.");
						this.OnDisconnected?.Invoke();
					}
				}
				else
				{
					OnConnected -= SetConnectedComplete;
				}
			}
			catch (OperationCanceledException)
			{
				OnConnected -= SetConnectedComplete;
			}
			catch (FormatException)
			{
				Debug.LogError("Connection string was not in the right format. Did you enter a ProductId?");
				Error = true;
				OnConnected -= SetConnectedComplete;
				this.OnDisconnected?.Invoke();
			}
			catch (Exception ex3)
			{
				Debug.LogError(ex3.Message);
				Error = true;
				OnConnected -= SetConnectedComplete;
				this.OnDisconnected?.Invoke();
			}
		}

		public void Disconnect()
		{
			cancelToken?.Cancel();
			if (serverId != null)
			{
				CloseP2PSessionWithUser(serverId, socketId);
				serverId = null;
				SendInternal(hostProductId, socketId, InternalMessages.DISCONNECT);
				Dispose();
				WaitForClose(hostProductId, socketId);
			}
		}

		private void SetConnectedComplete()
		{
			connectedComplete.SetResult(connectedComplete.Task);
		}

		protected override void OnReceiveData(byte[] data, ProductUserId clientUserId, int channel)
		{
			if (!ignoreAllMessages)
			{
				if (clientUserId != hostProductId)
				{
					Debug.LogError("Received a message from an unknown");
				}
				else
				{
					this.OnReceivedData(data, channel);
				}
			}
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
					Debug.LogWarning("[EosTransport.Client] Incoming connection request from dead socket " + result.SocketId.Value.SocketName + " — ignoring (stale EOS P2P session, benign).");
				}
			}
			else if (hostProductId == result.RemoteUserId)
			{
				AcceptConnectionOptions options = new AcceptConnectionOptions
				{
					LocalUserId = EOSSDKComponent.LocalUserProductId,
					RemoteUserId = result.RemoteUserId,
					SocketId = result.SocketId
				};
				EOSSDKComponent.GetP2PInterface().AcceptConnection(ref options);
			}
			else
			{
				Debug.LogError("P2P Acceptance Request from unknown host ID.");
			}
		}

		protected override void OnReceiveInternalData(InternalMessages type, ProductUserId clientUserId, SocketId socketId)
		{
			if (!ignoreAllMessages)
			{
				switch (type)
				{
				case InternalMessages.ACCEPT_CONNECT:
					Connected = true;
					this.OnConnected();
					Debug.Log("Connection established.");
					break;
				case InternalMessages.DISCONNECT:
					Connected = false;
					Debug.Log("Disconnected.");
					this.OnDisconnected();
					break;
				default:
					Debug.Log("Received unknown message type");
					break;
				}
			}
		}

		public void Send(byte[] data, int channelId)
		{
			Send(hostProductId, socketId, data, (byte)channelId);
		}

		protected override void OnConnectionFailed(ProductUserId remoteId, SocketId failedSocketId)
		{
			if (!(hostProductId == null) && !(remoteId != hostProductId))
			{
				if (failedSocketId.SocketName != socketId.SocketName)
				{
					Debug.LogWarning($"[EosTransport.Client] Ignoring stale OnConnectionFailed for host PUID {remoteId} " + "(failedSocketId=" + failedSocketId.SocketName + ", mySocketId=" + socketId.SocketName + ").");
				}
				else
				{
					this.OnDisconnected();
				}
			}
		}

		public void EosNotInitialized()
		{
			this.OnDisconnected();
		}
	}
}
