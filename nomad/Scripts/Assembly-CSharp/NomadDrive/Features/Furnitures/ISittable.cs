using EvilCore.Networking.Parenting;
using NomadDrive.Features.Vehicle;
using NomadDrive.Features.Vehicle.Parts.Seats;
using UnityEngine;

namespace NomadDrive.Features.Furnitures
{
	public interface ISittable
	{
		uint NetId { get; }

		bool IsDriverSeat { get; }

		VehicleSeatSlot VehicleSeatSlot { get; }

		Transform CurrentStandingPoint { get; }

		NetworkedTransform NetworkedTransform { get; }

		Transform LeftHandTarget { get; }

		Transform RightHandTarget { get; }

		Transform LeftFootTarget { get; }

		Transform RightFootTarget { get; }

		Transform LeftElbowBendGoal { get; }

		Transform RightElbowBendGoal { get; }

		Transform LeftKneeBendGoal { get; }

		Transform RightKneeBendGoal { get; }

		void SetOccupied(bool value);

		bool CanStandUpNow();

		bool TryGetMountVehicle(out VehicleManager vehicle);
	}
}
