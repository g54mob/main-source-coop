using Mirror;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.Interaction;

namespace NomadDrive.Features.Tools
{
	public class RepairKit : HeldItem
	{
		public float repairAmount = 50f;

		public ObjectSlotType targetMechanicalPart;

		public void Destroy()
		{
			NetworkServer.Destroy(base.gameObject);
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
