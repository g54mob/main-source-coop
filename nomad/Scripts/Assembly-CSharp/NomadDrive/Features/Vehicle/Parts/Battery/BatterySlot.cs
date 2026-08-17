using NomadDrive.Features.Vehicle.Modules.Slots;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Parts.Battery
{
	public class BatterySlot : VehicleSlot
	{
		public UnityEvent<Battery> OnBatteryInstalled { get; } = new UnityEvent<Battery>();

		public UnityEvent OnBatteryRemoved { get; } = new UnityEvent();

		public Battery InstalledBattery { get; set; }

		public bool IsInitialized { get; set; }

		protected override void OnEnable()
		{
			base.OnEnable();
			base.OnObjectAttached.AddListener(InstallBattery);
			base.OnObjectDetached.AddListener(RemoveBattery);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.OnObjectAttached.RemoveListener(InstallBattery);
			base.OnObjectDetached.RemoveListener(RemoveBattery);
		}

		private void InstallBattery()
		{
			Battery arg = (InstalledBattery = attachedObject.GetComponent<Battery>());
			OnBatteryInstalled.Invoke(arg);
		}

		private void RemoveBattery()
		{
			InstalledBattery = null;
			OnBatteryRemoved.Invoke();
		}
	}
}
