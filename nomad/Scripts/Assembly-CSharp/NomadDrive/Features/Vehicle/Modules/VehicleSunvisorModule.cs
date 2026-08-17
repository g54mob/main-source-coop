using NomadDrive.Features.Vehicle.Parts.Sunvisor;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Modules
{
	public class VehicleSunvisorModule : VehicleModule
	{
		private const string SUNVISORS = "Sunvisors";

		[SerializeField]
		public Sunvisor InstalledLeftSunvisor;

		[SerializeField]
		public Sunvisor InstalledRightSunVisor;

		[SerializeField]
		public SunvisorSlot LeftSunvisorSlotRef;

		[SerializeField]
		public SunvisorSlot RightSunvisorSlotRef;

		protected override void SubscribeEvents()
		{
			LeftSunvisorSlotRef.OnSunvisorInstalled.AddListener(OnSunvisorInstalled);
			RightSunvisorSlotRef.OnSunvisorInstalled.AddListener(OnSunvisorInstalled);
			LeftSunvisorSlotRef.OnSunvisorRemoved.AddListener(OnSunvisorRemoved);
			RightSunvisorSlotRef.OnSunvisorRemoved.AddListener(OnSunvisorRemoved);
		}

		protected override void UnsubscribeEvents()
		{
			LeftSunvisorSlotRef.OnSunvisorInstalled.RemoveListener(OnSunvisorInstalled);
			RightSunvisorSlotRef.OnSunvisorInstalled.RemoveListener(OnSunvisorInstalled);
			LeftSunvisorSlotRef.OnSunvisorRemoved.RemoveListener(OnSunvisorRemoved);
			RightSunvisorSlotRef.OnSunvisorRemoved.RemoveListener(OnSunvisorRemoved);
		}

		private void OnSunvisorInstalled(Sunvisor sunvisor)
		{
			if (sunvisor.SunvisorSlot == LeftSunvisorSlotRef)
			{
				InstalledLeftSunvisor = sunvisor;
			}
			else if (sunvisor.SunvisorSlot == RightSunvisorSlotRef)
			{
				InstalledRightSunVisor = sunvisor;
			}
		}

		private void OnSunvisorRemoved(Sunvisor sunvisor)
		{
			if (sunvisor == InstalledLeftSunvisor)
			{
				InstalledLeftSunvisor = null;
			}
			else if (sunvisor == InstalledRightSunVisor)
			{
				InstalledRightSunVisor = null;
			}
		}
	}
}
