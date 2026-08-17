using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet.Managing;
using GameKit.Utilities;
using UnityEngine;

namespace FishNet.Transporting.Multipass
{
	[AddComponentMenu("FishNet/Transport/Multipass")]
	public class Multipass : Transport
	{
		public struct TransportIdData
		{
			public int TransportId;

			public int TransportIndex;

			public TransportIdData(int transportId, int transportIndex)
			{
				TransportId = transportId;
				TransportIndex = transportIndex;
			}
		}

		[Tooltip("While true server actions such as starting or stopping the server will run on all transport.")]
		public bool GlobalServerActions = true;

		private Transport _clientTransport;

		[Tooltip("Transports to use.")]
		[SerializeField]
		private List<Transport> _transports = new List<Transport>();

		private Dictionary<int, TransportIdData> _multipassToTransport = new Dictionary<int, TransportIdData>();

		private List<Dictionary<int, int>> _transportToMultipass = new List<Dictionary<int, int>>();

		private Queue<int> _availableIds = new Queue<int>();

		internal const int CLIENT_HOST_ID = 32767;

		[HideInInspector]
		public Transport ClientTransport
		{
			get
			{
				if (_clientTransport == null)
				{
					if (_transports.Count != 0)
					{
						_clientTransport = _transports[0];
					}
					if (_clientTransport == null)
					{
						base.NetworkManager.LogError("ClientTransport in Multipass could not be set to the first transport. This can occur if no trnasports are specified or if the first entry is null.");
					}
					else
					{
						base.NetworkManager.LogError($"ClientTransport in Multipass is being automatically set to {_clientTransport.GetType()}. For production use SetClientTransport before attempting to access the ClientTransport.");
					}
				}
				return _clientTransport;
			}
			private set
			{
				_clientTransport = value;
			}
		}

		public IList<Transport> Transports => _transports;

		public override event Action<ClientConnectionStateArgs> OnClientConnectionState;

		public override event Action<ServerConnectionStateArgs> OnServerConnectionState;

		public override event Action<RemoteConnectionStateArgs> OnRemoteConnectionState;

		public override event Action<ClientReceivedDataArgs> OnClientReceivedData;

		public override event Action<ServerReceivedDataArgs> OnServerReceivedData;

		public override void Initialize(NetworkManager networkManager, int transportIndex)
		{
			base.Initialize(networkManager, transportIndex);
			for (int i = 0; i < _transports.Count; i++)
			{
				if (_transports[i] == null)
				{
					base.NetworkManager.LogWarning($"Transports contains a null entry on index {i}.");
					_transports.RemoveAt(i);
					i--;
				}
			}
			if (_transports.Count == 0)
			{
				base.NetworkManager.LogError("No transports are set within Multipass.");
				return;
			}
			for (int j = 0; j < _transports.Count; j++)
			{
				Dictionary<int, int> item = new Dictionary<int, int>();
				_transportToMultipass.Add(item);
			}
			for (int k = 0; k < _transports.Count; k++)
			{
				_transports[k].Initialize(networkManager, k);
				_transports[k].OnClientConnectionState += Multipass_OnClientConnectionState;
				_transports[k].OnServerConnectionState += Multipass_OnServerConnectionState;
				_transports[k].OnRemoteConnectionState += Multipass_OnRemoteConnectionState;
				_transports[k].OnClientReceivedData += Multipass_OnClientReceivedData;
				_transports[k].OnServerReceivedData += Multipass_OnServerReceivedData;
			}
		}

		private void OnDestroy()
		{
			foreach (Transport transport in _transports)
			{
				transport.Shutdown();
			}
		}

		private void TryResetClientIds(bool force)
		{
			if (!force)
			{
				foreach (Transport transport in _transports)
				{
					if (transport.GetConnectionState(server: true) == LocalConnectionState.Started)
					{
						return;
					}
				}
			}
			_multipassToTransport.Clear();
			foreach (Dictionary<int, int> item in _transportToMultipass)
			{
				item.Clear();
			}
			CreateAvailableIds();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool GetMultipassId(int transportIndex, int transportId, out int multipassId)
		{
			if (!_transportToMultipass[transportIndex].TryGetValueIL2CPP(transportId, out multipassId))
			{
				multipassId = -1;
				base.NetworkManager.LogError($"Multipass connectionId could not be found for transportIndex {transportIndex}, transportId of {transportId}.");
				return false;
			}
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool GetTransportIdData(int multipassId, out TransportIdData data)
		{
			if (!_multipassToTransport.TryGetValueIL2CPP(multipassId, out data))
			{
				base.NetworkManager.LogError($"TransportIdData could not be found for Multipass connectionId of {multipassId}.");
				return false;
			}
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string GetConnectionAddress(int connectionId)
		{
			if (!GetTransportIdData(connectionId, out var data))
			{
				return string.Empty;
			}
			return _transports[data.TransportIndex].GetConnectionAddress(data.TransportId);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override LocalConnectionState GetConnectionState(bool server)
		{
			if (server)
			{
				base.NetworkManager.LogError("This method is not supported for server. Use GetConnectionState(server, transportIndex) instead.");
				return LocalConnectionState.Stopped;
			}
			if (IsClientTransportSetWithError("GetConnectionState"))
			{
				return GetConnectionState(server, ClientTransport.Index);
			}
			return LocalConnectionState.Stopped;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LocalConnectionState GetConnectionState(bool server, int index)
		{
			if (!IndexInRange(index, error: true))
			{
				return LocalConnectionState.Stopped;
			}
			return _transports[index].GetConnectionState(server);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override RemoteConnectionState GetConnectionState(int connectionId)
		{
			if (!GetTransportIdData(connectionId, out var data))
			{
				return RemoteConnectionState.Stopped;
			}
			return _transports[data.TransportIndex].GetConnectionState(data.TransportId);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public RemoteConnectionState GetConnectionState(int connectionId, int index)
		{
			if (!IndexInRange(index, error: true))
			{
				return RemoteConnectionState.Stopped;
			}
			return _transports[index].GetConnectionState(connectionId);
		}

		private void Multipass_OnClientConnectionState(ClientConnectionStateArgs connectionStateArgs)
		{
			OnClientConnectionState?.Invoke(connectionStateArgs);
		}

		private void Multipass_OnServerConnectionState(ServerConnectionStateArgs connectionStateArgs)
		{
			OnServerConnectionState?.Invoke(connectionStateArgs);
			TryResetClientIds(force: false);
		}

		private void Multipass_OnRemoteConnectionState(RemoteConnectionStateArgs connectionStateArgs)
		{
			int transportIndex = connectionStateArgs.TransportIndex;
			int connectionId = connectionStateArgs.ConnectionId;
			Dictionary<int, int> dictionary = _transportToMultipass[transportIndex];
			int multipassId;
			if (connectionStateArgs.ConnectionState == RemoteConnectionState.Started)
			{
				multipassId = (dictionary[connectionId] = _availableIds.Dequeue());
				_multipassToTransport[multipassId] = new TransportIdData(connectionId, transportIndex);
			}
			else
			{
				if (!GetMultipassId(transportIndex, connectionId, out multipassId))
				{
					return;
				}
				_availableIds.Enqueue(multipassId);
				dictionary.Remove(connectionId);
				_multipassToTransport.Remove(multipassId);
			}
			connectionStateArgs.ConnectionId = multipassId;
			OnRemoteConnectionState?.Invoke(connectionStateArgs);
		}

		public override void IterateIncoming(bool server)
		{
			foreach (Transport transport in _transports)
			{
				transport.IterateIncoming(server);
			}
		}

		public override void IterateOutgoing(bool server)
		{
			foreach (Transport transport in _transports)
			{
				transport.IterateOutgoing(server);
			}
		}

		private void Multipass_OnClientReceivedData(ClientReceivedDataArgs receivedDataArgs)
		{
			OnClientReceivedData?.Invoke(receivedDataArgs);
		}

		private void Multipass_OnServerReceivedData(ServerReceivedDataArgs receivedDataArgs)
		{
			if (GetMultipassId(receivedDataArgs.TransportIndex, receivedDataArgs.ConnectionId, out var multipassId))
			{
				receivedDataArgs.ConnectionId = multipassId;
				OnServerReceivedData?.Invoke(receivedDataArgs);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override void SendToServer(byte channelId, ArraySegment<byte> segment)
		{
			if (ClientTransport != null)
			{
				ClientTransport.SendToServer(channelId, segment);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override void SendToClient(byte channelId, ArraySegment<byte> segment, int connectionId)
		{
			if (GetTransportIdData(connectionId, out var data))
			{
				_transports[data.TransportIndex].SendToClient(channelId, segment, data.TransportId);
			}
		}

		private bool UseGlobalServerActionsWithError(string methodText)
		{
			if (!GlobalServerActions)
			{
				base.NetworkManager.LogError("Method " + methodText + " is not supported while GlobalServerActions is false.");
				return false;
			}
			return true;
		}

		private bool IsClientTransportSetWithError(string methodText)
		{
			if (ClientTransport == null)
			{
				base.NetworkManager.LogError("ClientTransport is not set. Use SetClientTransport before calling " + methodText + ".");
				return false;
			}
			return true;
		}

		private void CreateAvailableIds()
		{
			_availableIds.Clear();
			for (int i = 0; i < 32767; i++)
			{
				_availableIds.Enqueue(i);
			}
		}

		public void SetClientTransport<T>()
		{
			int clientTransport = -1;
			for (int i = 0; i < _transports.Count; i++)
			{
				if (_transports[i].GetType() == typeof(T))
				{
					clientTransport = i;
					break;
				}
			}
			SetClientTransport(clientTransport);
		}

		public void SetClientTransport(Type type)
		{
			int clientTransport = -1;
			for (int i = 0; i < _transports.Count; i++)
			{
				if (_transports[i].GetType() == type)
				{
					clientTransport = i;
					break;
				}
			}
			SetClientTransport(clientTransport);
		}

		public void SetClientTransport(Transport transport)
		{
			int clientTransport = -1;
			for (int i = 0; i < _transports.Count; i++)
			{
				if (_transports[i] == transport)
				{
					clientTransport = i;
					break;
				}
			}
			SetClientTransport(clientTransport);
		}

		public void SetClientTransport(int index)
		{
			if (IndexInRange(index, error: true))
			{
				ClientTransport = _transports[index];
			}
		}

		public Transport GetTransport(int index)
		{
			if (!IndexInRange(index, error: true))
			{
				return null;
			}
			return _transports[index];
		}

		public T GetTransport<T>()
		{
			foreach (Transport transport in _transports)
			{
				if (transport.GetType() == typeof(T))
				{
					return (T)(object)transport;
				}
			}
			return default(T);
		}

		public override bool IsLocalTransport(int connectionid)
		{
			if (GetTransportIdData(connectionid, out var data))
			{
				return _transports[data.TransportIndex].IsLocalTransport(data.TransportId);
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetMaximumClients()
		{
			base.NetworkManager.LogError("This method is not supported. Use GetMaximumClients(transportIndex) instead.");
			return -1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetMaximumClients(int transportIndex)
		{
			if (!IndexInRange(transportIndex, error: true))
			{
				return -1;
			}
			return _transports[transportIndex].GetMaximumClients();
		}

		public override void SetMaximumClients(int value)
		{
			base.NetworkManager.LogError("This method is not supported. Use SetMaximumClients(value, transportIndex) instead.");
		}

		public void SetMaximumClients(int value, int transportIndex)
		{
			if (IndexInRange(transportIndex, error: true))
			{
				_transports[transportIndex].SetMaximumClients(value);
			}
		}

		public override void SetClientAddress(string address)
		{
			foreach (Transport transport in _transports)
			{
				transport.SetClientAddress(address);
			}
		}

		public override void SetServerBindAddress(string address, IPAddressType addressType)
		{
			base.NetworkManager.LogError("This method is not supported. Use SetServerBindAddress(address, transportIndex) instead.");
		}

		public void SetServerBindAddress(string address, IPAddressType addressType, int index)
		{
			if (IndexInRange(index, error: true))
			{
				_transports[index].SetServerBindAddress(address, addressType);
			}
		}

		public override void SetPort(ushort port)
		{
			base.NetworkManager.LogError("This method is not supported. Use SetPort(port, transportIndex) instead.");
		}

		public void SetPort(ushort port, int index)
		{
			if (IndexInRange(index, error: true))
			{
				_transports[index].SetPort(port);
			}
		}

		public override bool StartConnection(bool server)
		{
			if (server)
			{
				if (!UseGlobalServerActionsWithError("StartConnection"))
				{
					return false;
				}
				bool result = true;
				for (int i = 0; i < _transports.Count; i++)
				{
					if (!StartConnection(server: true, i))
					{
						result = false;
					}
				}
				return result;
			}
			if (IsClientTransportSetWithError("StartConnection"))
			{
				return StartConnection(server: false, ClientTransport.Index);
			}
			return false;
		}

		public bool StartConnection(bool server, int index)
		{
			if (server)
			{
				return StartServer(index);
			}
			if (IsClientTransportSetWithError("StartConnection"))
			{
				return StartClient();
			}
			return false;
		}

		public override bool StopConnection(bool server)
		{
			if (server)
			{
				if (!UseGlobalServerActionsWithError("StopConnection"))
				{
					return false;
				}
				bool result = true;
				for (int i = 0; i < _transports.Count; i++)
				{
					if (!StopConnection(server: true, i))
					{
						result = false;
					}
				}
				return result;
			}
			if (IsClientTransportSetWithError("StopConnection"))
			{
				return StopConnection(server: false, ClientTransport.Index);
			}
			return false;
		}

		public bool StopConnection(bool server, int index)
		{
			if (server)
			{
				return StopServer(index);
			}
			if (IsClientTransportSetWithError("StopConnection"))
			{
				return StopClient();
			}
			return false;
		}

		public override bool StopConnection(int connectionId, bool immediately)
		{
			return StopClient(connectionId, immediately);
		}

		public bool StopServerConnection(bool sendDisconnectMessage, int transportIndex)
		{
			if (sendDisconnectMessage)
			{
				int[] connectionIds = _transportToMultipass[transportIndex].Keys.ToArray();
				base.NetworkManager.ServerManager.SendDisconnectMessages(connectionIds);
				_transports[transportIndex].IterateOutgoing(server: true);
			}
			return StopConnection(server: true, transportIndex);
		}

		public override void Shutdown()
		{
			foreach (Transport transport in _transports)
			{
				transport.StopConnection(server: false);
				transport.StopConnection(server: true);
			}
		}

		private bool StartServer(int index)
		{
			if (!IndexInRange(index, error: true))
			{
				return false;
			}
			return _transports[index].StartConnection(server: true);
		}

		private bool StopServer(int index)
		{
			if (!IndexInRange(index, error: true))
			{
				return false;
			}
			return _transports[index].StopConnection(server: true);
		}

		private bool StartClient()
		{
			return ClientTransport.StartConnection(server: false);
		}

		private bool StopClient()
		{
			return ClientTransport.StopConnection(server: false);
		}

		private bool StopClient(int connectionId, bool immediately)
		{
			if (!GetTransportIdData(connectionId, out var data))
			{
				return false;
			}
			return _transports[data.TransportIndex].StopConnection(data.TransportId, immediately);
		}

		public override int GetMTU(byte channel)
		{
			return GetMTU(channel, 0);
		}

		public int GetMTU(byte channel, int index)
		{
			if (!IndexInRange(index, error: true))
			{
				return -1;
			}
			return _transports[index].GetMTU(channel);
		}

		private bool IndexInRange(int index, bool error)
		{
			if (index >= _transports.Count || index < 0)
			{
				if (error)
				{
					base.NetworkManager.LogError($"Index of {index} is out of Transports range.");
				}
				return false;
			}
			return true;
		}

		public override void HandleServerConnectionState(ServerConnectionStateArgs connectionStateArgs)
		{
		}

		public override void HandleRemoteConnectionState(RemoteConnectionStateArgs connectionStateArgs)
		{
		}

		public override void HandleClientReceivedDataArgs(ClientReceivedDataArgs receivedDataArgs)
		{
		}

		public override void HandleServerReceivedDataArgs(ServerReceivedDataArgs receivedDataArgs)
		{
		}

		public override void HandleClientConnectionState(ClientConnectionStateArgs connectionStateArgs)
		{
		}
	}
}
