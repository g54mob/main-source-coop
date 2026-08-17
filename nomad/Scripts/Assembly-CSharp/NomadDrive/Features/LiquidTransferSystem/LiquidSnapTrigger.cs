using NomadDrive.Features.Interaction;
using NomadDrive.Features.Player;

namespace NomadDrive.Features.LiquidTransferSystem
{
	public static class LiquidSnapTrigger
	{
		public static void Set(IPlayerService playerService, ILiquidSnapTarget target, bool active)
		{
			if (target == null)
			{
				return;
			}
			HeldItem heldItem = playerService?.EquipmentManager?.EquippedEntity;
			if (!(heldItem == null) && heldItem.TryGetComponent<LiquidContainerSnapHandler>(out var component))
			{
				if (active)
				{
					component.Snap(target);
				}
				else
				{
					component.Unsnap();
				}
			}
		}
	}
}
