using System;
using System.Collections.Generic;

namespace Photon.Realtime
{
	public interface IConnectionCallbacks
	{
		[Obsolete("Use OnConnectedToMaster or check the debug logging if you need to know if the client ever connected.")]
		void OnConnected();

		void OnConnectedToMaster();

		void OnDisconnected(DisconnectCause cause);

		void OnRegionListReceived(RegionHandler regionHandler);

		void OnCustomAuthenticationResponse(Dictionary<string, object> data);

		void OnCustomAuthenticationFailed(string debugMessage);
	}
}
