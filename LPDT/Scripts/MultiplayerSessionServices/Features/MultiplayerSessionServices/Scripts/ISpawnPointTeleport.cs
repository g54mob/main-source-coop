using UnityEngine;

namespace Features.MultiplayerSessionServices.Scripts
{
	public interface ISpawnPointTeleport
	{
		void TeleportToBeach();

		void TeleportLocalTo(Vector3 position, float yaw);
	}
}
