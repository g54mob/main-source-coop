using NomadDrive.Features.Vehicle.Modules.Slots;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Parts.Bumper
{
	public class VehicleBumperSlot : VehicleSlot
	{
		public UnityEvent<VehicleBumper> OnBumperInstalled { get; } = new UnityEvent<VehicleBumper>();

		public UnityEvent OnBumperRemoved { get; } = new UnityEvent();

		public VehicleBumper InstalledBumper { get; set; }

		protected override void OnEnable()
		{
			base.OnEnable();
			base.OnObjectAttached.AddListener(InstallBumper);
			base.OnObjectDetached.AddListener(RemoveBumper);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.OnObjectAttached.RemoveListener(InstallBumper);
			base.OnObjectDetached.RemoveListener(RemoveBumper);
		}

		private void InstallBumper()
		{
			VehicleBumper component = attachedObject.GetComponent<VehicleBumper>();
			component.BumperSlot = this;
			InstalledBumper = component;
			OnBumperInstalled.Invoke(component);
		}

		private void RemoveBumper()
		{
			InstalledBumper = null;
			OnBumperRemoved.Invoke();
		}
	}
}
