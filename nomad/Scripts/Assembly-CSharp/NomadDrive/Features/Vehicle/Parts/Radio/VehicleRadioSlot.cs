using NomadDrive.Features.Vehicle.Modules.Slots;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Parts.Radio
{
	public class VehicleRadioSlot : VehicleSlot
	{
		public UnityEvent<VehicleRadio> OnRadioInstalled { get; } = new UnityEvent<VehicleRadio>();

		public UnityEvent OnRadioRemoved { get; } = new UnityEvent();

		public VehicleRadio InstalledRadio { get; set; }

		protected override void OnEnable()
		{
			base.OnEnable();
			base.OnObjectAttached.AddListener(InstallRadio);
			base.OnObjectDetached.AddListener(RemoveRadio);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.OnObjectAttached.RemoveListener(InstallRadio);
			base.OnObjectDetached.RemoveListener(RemoveRadio);
		}

		private void InstallRadio()
		{
			VehicleRadio component = attachedObject.GetComponent<VehicleRadio>();
			component.RadioSlot = this;
			InstalledRadio = component;
			OnRadioInstalled.Invoke(component);
		}

		private void RemoveRadio()
		{
			InstalledRadio = null;
			OnRadioRemoved.Invoke();
		}
	}
}
