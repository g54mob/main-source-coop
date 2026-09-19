using Features.AIModuleStateMachine.Scripts.Core.SafeZones;

namespace Features.AIModuleStateMachine.Scripts.Tools.SafeZoneInteractionPoints
{
	public static class PlayerSafeZoneInteractionPointsToolResolver
	{
		public static PlayerSafeZoneInteractionPointsTool FindForSafeZone(PlayerSafeZone safeZone)
		{
			if (safeZone == null)
			{
				return null;
			}
			if (safeZone.TryGetComponent<PlayerSafeZoneInteractionPointsTool>(out var component))
			{
				return component;
			}
			PlayerSafeZoneInteractionPointsTool componentInChildren = safeZone.GetComponentInChildren<PlayerSafeZoneInteractionPointsTool>(includeInactive: true);
			if (componentInChildren != null)
			{
				return componentInChildren;
			}
			return safeZone.GetComponentInParent<PlayerSafeZoneInteractionPointsTool>(includeInactive: true);
		}

		public static PlayerSafeZone FindSafeZoneForTool(PlayerSafeZoneInteractionPointsTool config)
		{
			if (config == null)
			{
				return null;
			}
			if (config.TryGetComponent<PlayerSafeZone>(out var component))
			{
				return component;
			}
			PlayerSafeZone componentInParent = config.GetComponentInParent<PlayerSafeZone>(includeInactive: true);
			if (componentInParent != null && FindForSafeZone(componentInParent) == config)
			{
				return componentInParent;
			}
			PlayerSafeZone componentInChildren = config.GetComponentInChildren<PlayerSafeZone>(includeInactive: true);
			if (componentInChildren != null && FindForSafeZone(componentInChildren) == config)
			{
				return componentInChildren;
			}
			return null;
		}
	}
}
