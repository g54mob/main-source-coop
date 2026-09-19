namespace Features.AIModuleStateMachine.Scripts.Core.SafeZones
{
	public readonly struct EnemySafeZoneInteractionPointOccupant
	{
		public readonly int OwnerType;

		public readonly int OwnerInstanceId;

		public EnemySafeZoneInteractionPointOccupant(int ownerType, int ownerInstanceId)
		{
			OwnerType = ownerType;
			OwnerInstanceId = ownerInstanceId;
		}

		public bool IsOwnedBy(int ownerType, int ownerInstanceId)
		{
			if (OwnerType == ownerType)
			{
				return OwnerInstanceId == ownerInstanceId;
			}
			return false;
		}
	}
}
