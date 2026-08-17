using System;
using UnityEngine;

namespace FishNet.Managing.Statistic
{
	[Serializable]
	public class NetworkTraficStatistics
	{
		[Tooltip("How often to update traffic statistics.")]
		[SerializeField]
		[Range(0f, 10f)]
		private float _updateInteval = 1f;

		[Tooltip("True to update client statistics.")]
		[SerializeField]
		private bool _updateClient;

		[Tooltip("True to update server statistics.")]
		[SerializeField]
		private bool _updateServer;

		private NetworkManager _networkManager;

		private ulong _client_toServerBytes;

		private ulong _client_fromServerBytes;

		private ulong _server_toClientsBytes;

		private ulong _server_fromClientsBytes;

		private float _nextUpdateTime;

		private static readonly string[] _sizeSuffixes = new string[9] { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

		public bool UpdateClient
		{
			get
			{
				return _updateClient;
			}
			private set
			{
				_updateClient = value;
			}
		}

		public bool UpdateServer
		{
			get
			{
				return _updateServer;
			}
			private set
			{
				_updateServer = value;
			}
		}

		public event Action<NetworkTrafficArgs> OnClientNetworkTraffic;

		public event Action<NetworkTrafficArgs> OnServerNetworkTraffic;

		public void SetUpdateClient(bool update)
		{
			UpdateClient = update;
		}

		public void SetUpdateServer(bool update)
		{
			UpdateServer = update;
		}

		internal void InitializeOnce_Internal(NetworkManager manager)
		{
			_networkManager = manager;
			manager.TimeManager.OnPreTick += TimeManager_OnPreTick;
		}

		private void TimeManager_OnPreTick()
		{
			if (!(Time.unscaledTime < _nextUpdateTime))
			{
				_nextUpdateTime = Time.unscaledTime + _updateInteval;
				if (UpdateClient && _networkManager.IsClient)
				{
					this.OnClientNetworkTraffic?.Invoke(new NetworkTrafficArgs(_client_toServerBytes, _client_fromServerBytes));
				}
				if (UpdateServer && _networkManager.IsServer)
				{
					this.OnServerNetworkTraffic?.Invoke(new NetworkTrafficArgs(_server_fromClientsBytes, _server_toClientsBytes));
				}
				_client_toServerBytes = 0uL;
				_client_fromServerBytes = 0uL;
				_server_toClientsBytes = 0uL;
				_server_fromClientsBytes = 0uL;
			}
		}

		internal void LocalClientSentData(ulong dataLength)
		{
			_client_toServerBytes = Math.Min(_client_toServerBytes + dataLength, ulong.MaxValue);
		}

		public void LocalClientReceivedData(ulong dataLength)
		{
			_client_fromServerBytes = Math.Min(_client_fromServerBytes + dataLength, ulong.MaxValue);
		}

		internal void LocalServerSentData(ulong dataLength)
		{
			_server_toClientsBytes = Math.Min(_server_toClientsBytes + dataLength, ulong.MaxValue);
		}

		public void LocalServerReceivedData(ulong dataLength)
		{
			_server_fromClientsBytes = Math.Min(_server_fromClientsBytes + dataLength, ulong.MaxValue);
		}

		public static string FormatBytesToLargest(ulong bytes)
		{
			int decimals = 2;
			if (bytes == 0L)
			{
				return string.Format("{0:n" + 0 + "} bytes", 0);
			}
			int num = (int)Math.Log(bytes, 1024.0);
			decimal num2 = (decimal)bytes / (decimal)(1L << num * 10);
			if (Math.Round(num2, decimals) >= 1000m)
			{
				num++;
				num2 /= 1024m;
			}
			if (num == 0)
			{
				decimals = 0;
			}
			return string.Format("{0:n" + decimals + "} {1}", num2, _sizeSuffixes[num]);
		}
	}
}
