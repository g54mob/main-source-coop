using UnityEngine;

namespace Mirror.Examples.Basic
{
	[AddComponentMenu("")]
	public class BasicNetManager : NetworkManager
	{
		public override void OnServerAddPlayer(NetworkConnectionToClient conn)
		{
			base.OnServerAddPlayer(conn);
			Player.ResetPlayerNumbers();
		}

		public override void OnServerDisconnect(NetworkConnectionToClient conn)
		{
			base.OnServerDisconnect(conn);
			Player.ResetPlayerNumbers();
		}
	}
}
