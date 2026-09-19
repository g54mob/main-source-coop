using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Steamworks;
using UnityEngine;

namespace Mirror.FizzySteam
{
	public class NextClient : NextCommon, IClient
	{
		private TimeSpan ConnectionTimeout;

		private Callback<SteamNetConnectionStatusChangedCallback_t> c_onConnectionChange;

		private CancellationTokenSource cancelToken;

		private TaskCompletionSource<Task> connectedComplete;

		private CSteamID hostSteamID = CSteamID.Nil;

		private HSteamNetConnection HostConnection;

		private List<Action> BufferedData;

		public bool Connected { get; private set; }

		public bool Error { get; private set; }

		private event Action<byte[], int> OnReceivedData;

		private event Action OnConnected;

		private event Action OnDisconnected;

		private NextClient(FizzySteamworks transport)
		{
			ConnectionTimeout = TimeSpan.FromSeconds(Math.Max(1, transport.Timeout));
			BufferedData = new List<Action>();
		}

		public static NextClient CreateClient(FizzySteamworks transport, string host)
		{
			NextClient nextClient = new NextClient(transport);
			nextClient.OnConnected += delegate
			{
				transport.OnClientConnected();
			};
			nextClient.OnDisconnected += delegate
			{
				transport.OnClientDisconnected();
			};
			nextClient.OnReceivedData += delegate(byte[] data, int ch)
			{
				transport.OnClientDataReceived(new ArraySegment<byte>(data), ch);
			};
			try
			{
				SteamNetworkingUtils.InitRelayNetworkAccess();
				nextClient.Connect(host);
			}
			catch (FormatException)
			{
				Debug.LogError("Connection string was not in the right format. Did you enter a SteamId?");
				nextClient.Error = true;
				nextClient.OnConnectionFailed();
			}
			catch (Exception ex2)
			{
				Debug.LogError("Unexpected exception: " + ex2.Message);
				nextClient.Error = true;
				nextClient.OnConnectionFailed();
			}
			return nextClient;
		}

		private async void Connect(string host)
		{
			cancelToken = new CancellationTokenSource();
			c_onConnectionChange = Callback<SteamNetConnectionStatusChangedCallback_t>.Create(OnConnectionStatusChanged);
			try
			{
				hostSteamID = new CSteamID(ulong.Parse(host));
				connectedComplete = new TaskCompletionSource<Task>();
				OnConnected += SetConnectedComplete;
				SteamNetworkingIdentity identityRemote = default(SteamNetworkingIdentity);
				identityRemote.SetSteamID(hostSteamID);
				SteamNetworkingConfigValue_t[] array = new SteamNetworkingConfigValue_t[0];
				HostConnection = SteamNetworkingSockets.ConnectP2P(ref identityRemote, 0, array.Length, array);
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
					OnConnectionFailed();
				}
				OnConnected -= SetConnectedComplete;
			}
			catch (FormatException)
			{
				Debug.LogError("Connection string was not in the right format. Did you enter a SteamId?");
				Error = true;
				OnConnectionFailed();
			}
			catch (Exception ex2)
			{
				Debug.LogError("Unexpected exception: " + ex2.Message);
				Error = true;
				OnConnectionFailed();
			}
			finally
			{
				if (Error)
				{
					Debug.LogError("Connection failed.");
					OnConnectionFailed();
				}
			}
		}

		private void OnConnectionStatusChanged(SteamNetConnectionStatusChangedCallback_t param)
		{
			param.m_info.m_identityRemote.GetSteamID64();
			if (param.m_info.m_eState == ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_Connected)
			{
				Connected = true;
				this.OnConnected();
				Debug.Log("Connection established.");
				if (BufferedData.Count <= 0)
				{
					return;
				}
				Debug.Log($"{BufferedData.Count} received before connection was established. Processing now.");
				{
					foreach (Action bufferedDatum in BufferedData)
					{
						bufferedDatum();
					}
					return;
				}
			}
			if (param.m_info.m_eState == ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_ClosedByPeer || param.m_info.m_eState == ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_ProblemDetectedLocally)
			{
				Debug.Log("Connection was closed by peer, " + param.m_info.m_szEndDebug);
				Disconnect();
			}
			else
			{
				Debug.Log("Connection state changed: " + param.m_info.m_eState.ToString() + " - " + param.m_info.m_szEndDebug);
			}
		}

		public void Disconnect()
		{
			cancelToken?.Cancel();
			Dispose();
			if (Connected)
			{
				InternalDisconnect();
			}
			if (HostConnection.m_HSteamNetConnection != 0)
			{
				Debug.Log("Sending Disconnect message");
				SteamNetworkingSockets.CloseConnection(HostConnection, 0, "Graceful disconnect", bEnableLinger: false);
				HostConnection.m_HSteamNetConnection = 0u;
			}
		}

		protected void Dispose()
		{
			if (c_onConnectionChange != null)
			{
				c_onConnectionChange.Dispose();
				c_onConnectionChange = null;
			}
		}

		private void InternalDisconnect()
		{
			Connected = false;
			this.OnDisconnected();
			Debug.Log("Disconnected.");
			SteamNetworkingSockets.CloseConnection(HostConnection, 0, "Disconnected", bEnableLinger: false);
		}

		public void ReceiveData()
		{
			IntPtr[] array = new IntPtr[256];
			int num;
			if ((num = SteamNetworkingSockets.ReceiveMessagesOnConnection(HostConnection, array, 256)) <= 0)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				var (data, ch) = ProcessMessage(array[i]);
				if (Connected)
				{
					this.OnReceivedData(data, ch);
					continue;
				}
				BufferedData.Add(delegate
				{
					this.OnReceivedData(data, ch);
				});
			}
		}

		public void Send(byte[] data, int channelId)
		{
			try
			{
				EResult eResult = SendSocket(HostConnection, data, channelId);
				switch (eResult)
				{
				case EResult.k_EResultNoConnection:
				case EResult.k_EResultInvalidParam:
					Debug.Log("Connection to server was lost.");
					InternalDisconnect();
					break;
				default:
					Debug.LogError("Could not send: " + eResult);
					break;
				case EResult.k_EResultOK:
					break;
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("SteamNetworking exception during Send: " + ex.Message);
				InternalDisconnect();
			}
		}

		private void SetConnectedComplete()
		{
			connectedComplete.SetResult(connectedComplete.Task);
		}

		private void OnConnectionFailed()
		{
			this.OnDisconnected();
		}

		public void FlushData()
		{
			SteamNetworkingSockets.FlushMessagesOnConnection(HostConnection);
		}
	}
}
