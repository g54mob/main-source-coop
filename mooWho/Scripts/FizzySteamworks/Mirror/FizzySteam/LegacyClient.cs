using System;
using System.Threading;
using System.Threading.Tasks;
using Steamworks;
using UnityEngine;

namespace Mirror.FizzySteam
{
	public class LegacyClient : LegacyCommon, IClient
	{
		private TimeSpan ConnectionTimeout;

		private CSteamID hostSteamID = CSteamID.Nil;

		private TaskCompletionSource<Task> connectedComplete;

		private CancellationTokenSource cancelToken;

		public bool Connected { get; private set; }

		public bool Error { get; private set; }

		private event Action<byte[], int> OnReceivedData;

		private event Action OnConnected;

		private event Action OnDisconnected;

		private LegacyClient(FizzySteamworks transport)
			: base(transport)
		{
			ConnectionTimeout = TimeSpan.FromSeconds(Math.Max(1, transport.Timeout));
		}

		public static LegacyClient CreateClient(FizzySteamworks transport, string host)
		{
			LegacyClient legacyClient = new LegacyClient(transport);
			legacyClient.OnConnected += delegate
			{
				transport.OnClientConnected();
			};
			legacyClient.OnDisconnected += delegate
			{
				transport.OnClientDisconnected();
			};
			legacyClient.OnReceivedData += delegate(byte[] data, int channel)
			{
				transport.OnClientDataReceived(new ArraySegment<byte>(data), channel);
			};
			try
			{
				InteropHelp.TestIfAvailableClient();
				legacyClient.Connect(host);
			}
			catch (FormatException)
			{
				Debug.LogError("Connection string was not in the right format. Did you enter a SteamId?");
				legacyClient.Error = true;
				legacyClient.OnConnectionFailed(CSteamID.Nil);
			}
			catch (Exception ex2)
			{
				Debug.LogError("Unexpected exception: " + ex2.Message);
				legacyClient.Error = true;
				legacyClient.OnConnectionFailed(CSteamID.Nil);
			}
			return legacyClient;
		}

		private async void Connect(string host)
		{
			cancelToken = new CancellationTokenSource();
			try
			{
				hostSteamID = new CSteamID(ulong.Parse(host));
				connectedComplete = new TaskCompletionSource<Task>();
				OnConnected += SetConnectedComplete;
				SendInternal(hostSteamID, InternalMessages.CONNECT);
				Task connectedCompleteTask = connectedComplete.Task;
				Task timeOutTask = Task.Delay(ConnectionTimeout, cancelToken.Token);
				if (await Task.WhenAny(connectedCompleteTask, timeOutTask) != connectedCompleteTask)
				{
					if (cancelToken.IsCancellationRequested)
					{
						Debug.LogError("The connection attempt was cancelled.");
					}
					else if (timeOutTask.IsCompleted)
					{
						Debug.LogError("Connection to " + host + " timed out.");
					}
					OnConnected -= SetConnectedComplete;
					OnConnectionFailed(hostSteamID);
				}
				OnConnected -= SetConnectedComplete;
			}
			catch (FormatException)
			{
				Debug.LogError("Connection string was not in the right format. Did you enter a SteamId?");
				Error = true;
				OnConnectionFailed(hostSteamID);
			}
			catch (Exception ex2)
			{
				Debug.LogError("Unexpected exception: " + ex2.Message);
				Error = true;
				OnConnectionFailed(hostSteamID);
			}
			finally
			{
				if (Error)
				{
					Debug.LogError("Connection failed.");
					OnConnectionFailed(CSteamID.Nil);
				}
			}
		}

		public void Disconnect()
		{
			Debug.Log("Sending Disconnect message");
			SendInternal(hostSteamID, InternalMessages.DISCONNECT);
			Dispose();
			cancelToken?.Cancel();
			WaitForClose(hostSteamID);
		}

		private void SetConnectedComplete()
		{
			connectedComplete.SetResult(connectedComplete.Task);
		}

		protected override void OnReceiveData(byte[] data, CSteamID clientSteamID, int channel)
		{
			if (clientSteamID != hostSteamID)
			{
				Debug.LogError("Received a message from an unknown");
			}
			else
			{
				this.OnReceivedData(data, channel);
			}
		}

		protected override void OnNewConnection(P2PSessionRequest_t result)
		{
			if (hostSteamID == result.m_steamIDRemote)
			{
				SteamNetworking.AcceptP2PSessionWithUser(result.m_steamIDRemote);
			}
			else
			{
				Debug.LogError("P2P Acceptance Request from unknown host ID.");
			}
		}

		protected override void OnReceiveInternalData(InternalMessages type, CSteamID clientSteamID)
		{
			switch (type)
			{
			case InternalMessages.ACCEPT_CONNECT:
				if (!Connected)
				{
					Connected = true;
					this.OnConnected();
					Debug.Log("Connection established.");
				}
				break;
			case InternalMessages.DISCONNECT:
				if (Connected)
				{
					Connected = false;
					Debug.Log("Disconnected.");
					this.OnDisconnected();
				}
				break;
			default:
				Debug.Log("Received unknown message type");
				break;
			}
		}

		public void Send(byte[] data, int channelId)
		{
			Send(hostSteamID, data, channelId);
		}

		protected override void OnConnectionFailed(CSteamID remoteId)
		{
			this.OnDisconnected();
		}

		public void FlushData()
		{
		}
	}
}
