using NomadDrive.Features.Vehicle.Parts.Hood;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Modules
{
	public class VehicleHoodModule : VehicleModule
	{
		private const string HOOD = "Hood";

		[SerializeField]
		public Hood InstalledHood;

		[SerializeField]
		public HoodSlot HoodSlotRef;

		protected override void SubscribeEvents()
		{
			HoodSlotRef.OnHoodInstalled.AddListener(OnHoodInstalled);
			HoodSlotRef.OnHoodRemoved.AddListener(OnHoodRemoved);
		}

		protected override void UnsubscribeEvents()
		{
			HoodSlotRef.OnHoodInstalled.RemoveListener(OnHoodInstalled);
			HoodSlotRef.OnHoodRemoved.RemoveListener(OnHoodRemoved);
		}

		private void OnHoodInstalled(Hood hood)
		{
			InstalledHood = hood;
			base.EventBus.FireUnderHoodPartsDeactivate();
			hood.OnHoodOpened.AddListener(OnHoodOpened);
			hood.OnHoodClosed.AddListener(OnHoodClosed);
		}

		private void OnHoodRemoved()
		{
			InstalledHood.OnHoodOpened.RemoveListener(OnHoodOpened);
			InstalledHood.OnHoodClosed.RemoveListener(OnHoodClosed);
			InstalledHood = null;
			base.EventBus.FireUnderHoodPartsActivate();
		}

		private void OnHoodOpened()
		{
			base.EventBus.FireUnderHoodPartsActivate();
		}

		private void OnHoodClosed()
		{
			base.EventBus.FireUnderHoodPartsDeactivate();
		}

		public override void OnFrontSeatsTaken()
		{
			InstalledHood?.IgnoreHovering();
		}

		public override void OnFrontSeatsVacated()
		{
			InstalledHood?.UnignoreHovering();
		}
	}
}
