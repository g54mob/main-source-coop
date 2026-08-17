using NomadDrive.Features.Vehicle.Modules.Slots;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Parts.SteeringWheel
{
	public class SteeringWheelSlot : VehicleSlot
	{
		public UnityEvent<SteeringWheel> OnSteeringWheelAttached;

		public UnityEvent<SteeringWheel> OnSteeringWheelDetached;

		public SteeringWheel InstalledSteeringWheel { get; set; }

		protected override void OnEnable()
		{
			base.OnEnable();
			base.OnObjectAttached.AddListener(InstallSteeringWheel);
			base.OnObjectDetached.AddListener(RemoveSteeringWheel);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.OnObjectAttached.RemoveListener(InstallSteeringWheel);
			base.OnObjectDetached.RemoveListener(RemoveSteeringWheel);
		}

		private void InstallSteeringWheel()
		{
			SteeringWheel component = attachedObject.GetComponent<SteeringWheel>();
			component.SteeringWheelSlot = this;
			InstalledSteeringWheel = component;
			OnSteeringWheelAttached.Invoke(component);
		}

		private void RemoveSteeringWheel()
		{
			OnSteeringWheelDetached.Invoke(InstalledSteeringWheel);
			InstalledSteeringWheel = null;
		}

		public override void DriveAttachedPartPose(Transform vehicleRoot)
		{
		}
	}
}
