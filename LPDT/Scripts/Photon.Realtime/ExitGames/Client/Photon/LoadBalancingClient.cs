using System;
using Photon.Client;
using Photon.Realtime;

namespace ExitGames.Client.Photon
{
	[Obsolete("Use the RealtimeClient class instead. This was just renamed.")]
	public class LoadBalancingClient : RealtimeClient
	{
		[Obsolete("Use the RealtimeClient class instead. This was just renamed.")]
		public LoadBalancingClient(ConnectionProtocol protocol = ConnectionProtocol.Udp)
			: base(protocol)
		{
		}
	}
}
