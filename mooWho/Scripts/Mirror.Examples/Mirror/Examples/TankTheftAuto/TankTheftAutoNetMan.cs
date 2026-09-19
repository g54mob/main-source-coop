using Mirror.Examples.Common.Controllers.Tank;
using UnityEngine;

namespace Mirror.Examples.TankTheftAuto
{
	[AddComponentMenu("")]
	public class TankTheftAutoNetMan : NetworkManager
	{
		public override void OnServerDisconnect(NetworkConnectionToClient conn)
		{
			if (conn.authenticationData is GameObject obj)
			{
				NetworkServer.Destroy(obj);
			}
			if (conn.identity != null)
			{
				if (conn.identity.TryGetComponent<TankTurretBase>(out var component))
				{
					component.NetworkplayerColor = Color.black;
				}
				if (conn.identity.TryGetComponent<TankAuthority>(out var component2))
				{
					component2.NetworkisControlled = false;
					NetworkServer.RemovePlayerForConnection(conn);
				}
			}
			base.OnServerDisconnect(conn);
		}
	}
}
