using System;
using Photon.Client;

namespace Photon.Realtime
{
	public class ProtocolPorts
	{
		public ServerPorts Udp = new ServerPorts
		{
			NameServer = 27000,
			MasterServer = 27001,
			GameServer = 27002
		};

		public ServerPorts Tcp = new ServerPorts
		{
			NameServer = 4533
		};

		public ServerPorts Ws = new ServerPorts
		{
			NameServer = 80
		};

		public ServerPorts Wss = new ServerPorts
		{
			NameServer = 443
		};

		public ushort Get(ConnectionProtocol protocol, ServerConnection serverType)
		{
			return protocol switch
			{
				ConnectionProtocol.Udp => Udp.Get(serverType), 
				ConnectionProtocol.Tcp => Tcp.Get(serverType), 
				ConnectionProtocol.WebSocket => Ws.Get(serverType), 
				ConnectionProtocol.WebSocketSecure => Wss.Get(serverType), 
				_ => throw new ArgumentOutOfRangeException("protocol", protocol, null), 
			};
		}

		public void SetUdpDefault()
		{
			Udp = new ServerPorts
			{
				NameServer = 27000,
				MasterServer = 27001,
				GameServer = 27002
			};
		}

		public void SetUdpDefaultOld()
		{
			Udp = new ServerPorts
			{
				NameServer = 5058,
				MasterServer = 5055,
				GameServer = 5056
			};
		}

		public void SetTcpDefault()
		{
			Tcp = new ServerPorts
			{
				NameServer = 4533,
				MasterServer = 4530,
				GameServer = 4531
			};
		}

		public void SetWsDefault()
		{
			Ws = new ServerPorts
			{
				NameServer = 80,
				MasterServer = 80,
				GameServer = 80
			};
		}

		public void SetWsDefaultOld()
		{
			Ws = new ServerPorts
			{
				NameServer = 9093,
				MasterServer = 9090,
				GameServer = 9091
			};
		}

		public void SetWssDefault()
		{
			Wss = new ServerPorts
			{
				NameServer = 433,
				MasterServer = 443,
				GameServer = 443
			};
		}

		public void SetWssDefaultOld()
		{
			Wss = new ServerPorts
			{
				NameServer = 19093,
				MasterServer = 19090,
				GameServer = 19091
			};
		}

		public void SetOldDefaults()
		{
			SetUdpDefaultOld();
			SetTcpDefault();
			SetWsDefaultOld();
			SetWssDefaultOld();
		}
	}
}
