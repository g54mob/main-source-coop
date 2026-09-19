using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Tools.SafeZoneInteractionPoints
{
	public readonly struct PlayerSafeZoneInteractionPointValidationSettings
	{
		public readonly LayerMask ObstacleMask;

		public readonly float FloorRayMaxDistance;

		public readonly float ObstacleProbeRadius;

		public readonly bool IgnoreTriggers;

		public readonly bool IgnoreOwnSafeZoneColliders;

		public PlayerSafeZoneInteractionPointValidationSettings(LayerMask obstacleMask, float floorRayMaxDistance, float obstacleProbeRadius, bool ignoreTriggers, bool ignoreOwnSafeZoneColliders)
		{
			ObstacleMask = obstacleMask;
			FloorRayMaxDistance = floorRayMaxDistance;
			ObstacleProbeRadius = obstacleProbeRadius;
			IgnoreTriggers = ignoreTriggers;
			IgnoreOwnSafeZoneColliders = ignoreOwnSafeZoneColliders;
		}
	}
}
