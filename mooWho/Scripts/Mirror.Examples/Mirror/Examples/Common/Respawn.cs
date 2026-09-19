using System.Collections;
using UnityEngine;

namespace Mirror.Examples.Common
{
	public class Respawn
	{
		public static void RespawnPlayer(bool respawn, byte respawnTime, NetworkConnectionToClient conn)
		{
			NetworkManager.singleton.StartCoroutine(DoRespawn(respawn, respawnTime, conn));
		}

		public static IEnumerator DoRespawn(bool respawn, byte respawnTime, NetworkConnectionToClient conn)
		{
			yield return null;
			if (!respawn)
			{
				NetworkServer.RemovePlayerForConnection(conn, RemovePlayerOptions.Destroy);
				yield break;
			}
			GameObject playerObject = conn.identity.gameObject;
			NetworkServer.RemovePlayerForConnection(conn, RemovePlayerOptions.Unspawn);
			yield return new WaitForSeconds((int)respawnTime);
			Transform startPosition = NetworkManager.singleton.GetStartPosition();
			Vector3 position = ((startPosition != null) ? startPosition.position : (Vector3.up * 5f));
			Quaternion rotation = ((startPosition != null) ? startPosition.rotation : Quaternion.identity);
			playerObject.transform.SetPositionAndRotation(position, rotation);
			NetworkServer.AddPlayerForConnection(conn, playerObject);
		}
	}
}
