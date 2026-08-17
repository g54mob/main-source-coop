using NomadDrive.Features.Vehicle.Modules.Slots;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Parts.Doors
{
	public class VehicleDoorSlot : VehicleSlot
	{
		public UnityEvent<VehicleDoor> OnDoorAttached = new UnityEvent<VehicleDoor>();

		public UnityEvent OnDoorDetached = new UnityEvent();

		public VehicleDoor AttachedDoor { get; set; }

		protected override void OnEnable()
		{
			base.OnEnable();
			base.OnObjectAttached.AddListener(AttachDoor);
			base.OnObjectDetached.AddListener(DetachDoor);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.OnObjectAttached.RemoveListener(AttachDoor);
			base.OnObjectDetached.RemoveListener(DetachDoor);
		}

		private void AttachDoor()
		{
			VehicleDoor arg = (AttachedDoor = attachedObject.GetComponent<VehicleDoor>());
			OnDoorAttached.Invoke(arg);
		}

		private void DetachDoor()
		{
			if (AttachedDoor != null)
			{
				AttachedDoor = null;
				OnDoorDetached.Invoke();
			}
		}
	}
}
