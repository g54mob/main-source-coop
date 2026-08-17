using NomadDrive.Features.Attachables;

namespace NomadDrive.Features.Vehicle.Parts.Radio
{
	public class VehicleRadio : AttachableObject
	{
		public VehicleRadioSlot RadioSlot { get; set; }

		public override bool Weaved()
		{
			return true;
		}
	}
}
