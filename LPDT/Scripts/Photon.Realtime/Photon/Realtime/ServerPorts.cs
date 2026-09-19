using System;

namespace Photon.Realtime
{
	public struct ServerPorts
	{
		public ushort NameServer;

		public ushort MasterServer;

		public ushort GameServer;

		public ushort Get(ServerConnection serverType)
		{
			return serverType switch
			{
				ServerConnection.NameServer => NameServer, 
				ServerConnection.MasterServer => MasterServer, 
				ServerConnection.GameServer => GameServer, 
				_ => throw new ArgumentOutOfRangeException("serverType", serverType, null), 
			};
		}
	}
}
