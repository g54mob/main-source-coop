using NomadDrive.Features.Vehicle.Modules.Slots;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Parts.Handbrake
{
	public class HandbrakeSlot : VehicleSlot
	{
		public UnityEvent<Handbrake> OnHandbrakeInstalled;

		public UnityEvent<Handbrake> OnHandbrakeRemoved;

		public Handbrake InstalledHandbrake { get; set; }

		protected override void OnEnable()
		{
			base.OnEnable();
			base.OnObjectAttached.AddListener(InstallHandbrake);
			base.OnObjectDetached.AddListener(RemoveHandbrake);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.OnObjectAttached.RemoveListener(InstallHandbrake);
			base.OnObjectDetached.RemoveListener(RemoveHandbrake);
		}

		private void InstallHandbrake()
		{
			Handbrake component = attachedObject.GetComponent<Handbrake>();
			component.HandbrakeSlot = this;
			InstalledHandbrake = component;
			InstalledHandbrake.OnAttachedActions();
			OnHandbrakeInstalled.Invoke(component);
		}

		private void RemoveHandbrake()
		{
			OnHandbrakeRemoved.Invoke(InstalledHandbrake);
			InstalledHandbrake.OnDetachedActions();
			InstalledHandbrake = null;
		}
	}
}
