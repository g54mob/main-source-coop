using Fusion;
using UnityEngine;

namespace Features.BasketballHoopModule.Scripts
{
	public interface IBasketballService
	{
		void RegisterBall(NetworkObject ball, Vector3 spawnPosition, Vector3 trackPosition, BasketballBallTeleport teleport);

		void UnregisterBall(NetworkObject ball);
	}
}
