using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Photon.Client
{
	public abstract class PhotonSocket
	{
		protected internal PeerBase peerBase;

		protected readonly ConnectionProtocol Protocol;

		public bool PollReceive;

		public string ConnectAddress;

		protected IPhotonPeerListener Listener => peerBase.Listener;

		protected internal int MTU => peerBase.mtu;

		public PhotonSocketState State { get; protected set; }

		public int SocketErrorCode { get; protected set; }

		public bool Connected => State == PhotonSocketState.Connected;

		protected LogLevel LogLevel => peerBase.LogLevel;

		public string ServerAddress { get; protected set; }

		public string ProxyServerAddress { get; protected set; }

		public string ServerIpAddress { get; protected set; }

		public int ServerPort { get; protected set; }

		public bool AddressResolvedAsIpv6 { get; protected internal set; }

		public string UrlProtocol { get; protected set; }

		public string UrlPath { get; protected set; }

		protected internal string SerializationProtocol
		{
			get
			{
				if (peerBase == null || peerBase.photonPeer == null)
				{
					return "GpBinaryV18";
				}
				return Enum.GetName(typeof(SerializationProtocol), peerBase.photonPeer.SerializationProtocolType);
			}
		}

		public PhotonSocket(PeerBase peerBase)
		{
			if (peerBase == null)
			{
				throw new Exception("Can't init without peer");
			}
			this.peerBase = peerBase;
			Protocol = peerBase.usedTransportProtocol;
			ConnectAddress = this.peerBase.ServerAddress;
		}

		public virtual bool Connect()
		{
			if (State != PhotonSocketState.Disconnected)
			{
				if ((int)LogLevel >= 1)
				{
					peerBase.Listener.DebugReturn(LogLevel.Error, $"Connect() failed: connection in State: {State}");
				}
				return false;
			}
			if (peerBase == null || Protocol != peerBase.usedTransportProtocol)
			{
				return false;
			}
			if (!TryParseAddress(peerBase.ServerAddress, out var host, out var port, out var scheme, out var absolutePath))
			{
				if ((int)LogLevel >= 1)
				{
					peerBase.Listener.DebugReturn(LogLevel.Error, "Failed parsing address: " + peerBase.ServerAddress);
				}
				return false;
			}
			ServerIpAddress = string.Empty;
			ServerAddress = host;
			ServerPort = port;
			UrlProtocol = scheme;
			UrlPath = absolutePath;
			if ((int)LogLevel >= 4)
			{
				Listener.DebugReturn(LogLevel.Debug, $"PhotonSocket.Connect() {ServerAddress}:{ServerPort} this.Protocol: {Protocol}");
			}
			return true;
		}

		public abstract bool Disconnect();

		public abstract PhotonSocketError Send(byte[] data, int length);

		public abstract PhotonSocketError Receive(out byte[] data);

		public void HandleReceivedDatagram(byte[] inBuffer, int length, bool willBeReused)
		{
			peerBase.photonPeer.TrafficRecorder?.Record(inBuffer, length, incoming: true, peerBase.peerID, this);
			if (peerBase.NetworkSimulationSettings.IsSimulationEnabled)
			{
				if (willBeReused)
				{
					byte[] array = new byte[length];
					Buffer.BlockCopy(inBuffer, 0, array, 0, length);
					peerBase.ReceiveNetworkSimulated(array);
				}
				else
				{
					peerBase.ReceiveNetworkSimulated(inBuffer);
				}
			}
			else
			{
				peerBase.ReceiveIncomingCommands(inBuffer, length);
			}
		}

		public bool ReportDebugOfLevel(LogLevel levelOfMessage)
		{
			return (int)LogLevel >= (int)levelOfMessage;
		}

		public void EnqueueDebugReturn(LogLevel logLevel, string message)
		{
			peerBase.EnqueueDebugReturn(logLevel, message);
		}

		protected internal void HandleException(StatusCode statusCode)
		{
			State = PhotonSocketState.Disconnecting;
			peerBase.EnqueueStatusCallback(statusCode);
			peerBase.EnqueueActionForDispatch(delegate
			{
				peerBase.Disconnect();
			});
		}

		protected internal bool TryParseAddress(string url, out string host, out ushort port, out string scheme, out string absolutePath)
		{
			host = string.Empty;
			port = 0;
			scheme = string.Empty;
			absolutePath = string.Empty;
			if (string.IsNullOrEmpty(url))
			{
				return false;
			}
			bool flag = url.Contains("://");
			string uriString = (flag ? url : ("net.tcp://" + url));
			Uri result;
			bool flag2 = Uri.TryCreate(uriString, UriKind.Absolute, out result);
			if (flag2)
			{
				host = result.Host;
				port = (ushort)((flag || url.Contains($":{result.Port}")) ? ((ushort)result.Port) : 0);
				scheme = (flag ? result.Scheme : string.Empty);
				absolutePath = ("/".Equals(result.AbsolutePath) ? string.Empty : result.AbsolutePath);
			}
			return flag2;
		}

		private bool IpAddressTryParse(string strIP, out IPAddress address)
		{
			address = null;
			if (string.IsNullOrEmpty(strIP))
			{
				return false;
			}
			string[] array = strIP.Split(new char[1] { '.' });
			if (array.Length != 4)
			{
				return false;
			}
			byte[] array2 = new byte[4];
			for (int i = 0; i < array.Length; i++)
			{
				string s = array[i];
				byte result = 0;
				if (!byte.TryParse(s, out result))
				{
					return false;
				}
				array2[i] = result;
			}
			if (array2[0] == 0)
			{
				return false;
			}
			address = new IPAddress(array2);
			return true;
		}

		protected internal IPAddress[] GetIpAddresses(string hostname)
		{
			IPAddress address = null;
			if (IPAddress.TryParse(hostname, out address))
			{
				if (address.AddressFamily == AddressFamily.InterNetworkV6 || IpAddressTryParse(hostname, out address))
				{
					return new IPAddress[1] { address };
				}
				HandleException(StatusCode.ServerAddressInvalid);
				return null;
			}
			IPAddress[] array;
			try
			{
				array = Dns.GetHostAddresses(ServerAddress);
			}
			catch (Exception arg)
			{
				try
				{
					IPHostEntry hostByName = Dns.GetHostByName(ServerAddress);
					array = hostByName.AddressList;
				}
				catch (Exception arg2)
				{
					if (ReportDebugOfLevel(LogLevel.Warning))
					{
						EnqueueDebugReturn(LogLevel.Warning, $"GetHostAddresses and GetHostEntry() failed for: {ServerAddress}. Caught and handled exceptions:\n{arg}\n{arg2}");
					}
					HandleException(StatusCode.DnsExceptionOnConnect);
					return null;
				}
			}
			Array.Sort(array, AddressSortComparer);
			if (ReportDebugOfLevel(LogLevel.Info))
			{
				string[] array2 = array.Select((IPAddress x) => $"{x} ({x.AddressFamily}({(int)x.AddressFamily}))").ToArray();
				string arg3 = string.Join(", ", array2);
				if (ReportDebugOfLevel(LogLevel.Info))
				{
					EnqueueDebugReturn(LogLevel.Info, $"{ServerAddress} resolved to {array2.Length} address(es): {arg3}");
				}
			}
			return array;
		}

		private int AddressSortComparer(IPAddress x, IPAddress y)
		{
			if (x.AddressFamily == y.AddressFamily)
			{
				return 0;
			}
			return (x.AddressFamily != AddressFamily.InterNetworkV6) ? 1 : (-1);
		}
	}
}
