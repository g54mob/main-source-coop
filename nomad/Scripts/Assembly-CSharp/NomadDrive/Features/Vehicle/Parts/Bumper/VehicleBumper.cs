using NomadDrive.Features.Attachables;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Parts.Bumper
{
	public class VehicleBumper : AttachableObject
	{
		[SerializeField]
		private BumperPosition bumperPosition;

		public BumperPosition BumperPosition => bumperPosition;

		public VehicleBumperSlot BumperSlot { get; set; }

		public override bool Weaved()
		{
			return true;
		}
	}
}
