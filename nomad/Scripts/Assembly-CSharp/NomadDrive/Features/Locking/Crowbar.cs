using NomadDrive.Features.Interaction;

namespace NomadDrive.Features.Locking
{
	public class Crowbar : HeldItem
	{
		public override bool Weaved()
		{
			return true;
		}
	}
}
