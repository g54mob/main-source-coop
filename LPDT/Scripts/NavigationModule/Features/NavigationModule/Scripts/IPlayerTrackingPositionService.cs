using Fusion;
using UnityEngine;

namespace Features.NavigationModule.Scripts
{
	public interface IPlayerTrackingPositionService
	{
		bool TryGetTrackingPosition(PlayerRef player, out Vector3 position);
	}
}
