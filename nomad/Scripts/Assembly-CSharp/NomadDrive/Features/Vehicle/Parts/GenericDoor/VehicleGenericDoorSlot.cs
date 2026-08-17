using NomadDrive.Features.Vehicle.Modules.Slots;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Parts.GenericDoor
{
	public class VehicleGenericDoorSlot : VehicleSlot
	{
		public UnityEvent<VehicleGenericDoor> OnGenericDoorInstalled { get; } = new UnityEvent<VehicleGenericDoor>();

		public UnityEvent OnGenericDoorRemoved { get; } = new UnityEvent();

		public VehicleGenericDoor InstalledDoor { get; set; }

		protected override void OnEnable()
		{
			base.OnEnable();
			base.OnObjectAttached.AddListener(InstallDoor);
			base.OnObjectDetached.AddListener(RemoveDoor);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.OnObjectAttached.RemoveListener(InstallDoor);
			base.OnObjectDetached.RemoveListener(RemoveDoor);
		}

		private void InstallDoor()
		{
			VehicleGenericDoor component = attachedObject.GetComponent<VehicleGenericDoor>();
			component.GenericDoorSlot = this;
			InstalledDoor = component;
			OnGenericDoorInstalled.Invoke(component);
		}

		private void RemoveDoor()
		{
			InstalledDoor = null;
			OnGenericDoorRemoved.Invoke();
		}
	}
}
