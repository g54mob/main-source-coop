using NomadDrive.Features.Vehicle.Modules.Slots;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Parts.Radiator
{
	public class RadiatorSlot : VehicleSlot
	{
		public UnityEvent<Radiator> OnRadiatorInstalled { get; } = new UnityEvent<Radiator>();

		public UnityEvent OnRadiatorRemoved { get; } = new UnityEvent();

		public Radiator InstalledRadiator { get; set; }

		protected override void OnEnable()
		{
			base.OnEnable();
			base.OnObjectAttached.AddListener(InstallRadiator);
			base.OnObjectDetached.AddListener(RemoveRadiator);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.OnObjectAttached.RemoveListener(InstallRadiator);
			base.OnObjectDetached.RemoveListener(RemoveRadiator);
		}

		private void InstallRadiator()
		{
			Radiator arg = (InstalledRadiator = attachedObject.GetComponent<Radiator>());
			OnRadiatorInstalled.Invoke(arg);
		}

		private void RemoveRadiator()
		{
			InstalledRadiator = null;
			OnRadiatorRemoved.Invoke();
		}
	}
}
