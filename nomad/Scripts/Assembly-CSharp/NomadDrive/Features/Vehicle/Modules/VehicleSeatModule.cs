using NomadDrive.Features.Furnitures;
using NomadDrive.Features.Vehicle.Parts.Seats;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Modules
{
	public class VehicleSeatModule : VehicleModule
	{
		private const string SEATS = "Seats";

		[SerializeField]
		public VehicleSeatSlot FrontLeftSeatSlotRef;

		[SerializeField]
		public VehicleSeatSlot FrontRightSeatSlotRef;

		[SerializeField]
		public Seat InstalledFrontLeftSeat;

		[SerializeField]
		public Seat InstalledFrontRightSeat;

		protected override void SubscribeEvents()
		{
			FrontLeftSeatSlotRef.OnSeatInstalled.AddListener(OnFrontLeftSeatInstalled);
			FrontLeftSeatSlotRef.OnSeatRemoved.AddListener(OnFrontLeftSeatRemoved);
			FrontRightSeatSlotRef.OnSeatInstalled.AddListener(OnFrontRightSeatInstalled);
			FrontRightSeatSlotRef.OnSeatRemoved.AddListener(OnFrontRightSeatRemoved);
		}

		protected override void UnsubscribeEvents()
		{
			FrontLeftSeatSlotRef.OnSeatInstalled.RemoveListener(OnFrontLeftSeatInstalled);
			FrontLeftSeatSlotRef.OnSeatRemoved.RemoveListener(OnFrontLeftSeatRemoved);
			FrontRightSeatSlotRef.OnSeatInstalled.RemoveListener(OnFrontRightSeatInstalled);
			FrontRightSeatSlotRef.OnSeatRemoved.RemoveListener(OnFrontRightSeatRemoved);
		}

		private void OnFrontLeftSeatInstalled(Seat seat)
		{
			InstalledFrontLeftSeat = seat;
		}

		private void OnFrontLeftSeatRemoved()
		{
			InstalledFrontLeftSeat = null;
		}

		private void OnFrontRightSeatInstalled(Seat seat)
		{
			InstalledFrontRightSeat = seat;
		}

		private void OnFrontRightSeatRemoved()
		{
			InstalledFrontRightSeat = null;
		}
	}
}
