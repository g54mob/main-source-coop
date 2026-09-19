using Features.KrakenModule.Scripts.Data;
using Fusion;
using UnityEngine;

namespace Features.KrakenModule.Scripts.Core
{
	public interface IDeadPartThrowTargetService
	{
		bool HasDeadPlayers(NetworkRunner runner);

		bool TryFindNearestBeachAliveTarget(Vector3 fromPosition, out KrakenDeadPartThrowTarget target);
	}
}
