using NomadDrive.Features.Attachables;

namespace NomadDrive.Features.Vehicle.Parts.TowKnob
{
	public class TowKnob : AttachableObject
	{
		public TowKnobSlot TowKnobSlot { get; set; }

		public override bool Weaved()
		{
			return true;
		}
	}
}
