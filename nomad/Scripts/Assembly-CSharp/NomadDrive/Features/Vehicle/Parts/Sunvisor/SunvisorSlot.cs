using NomadDrive.Features.Vehicle.Modules.Slots;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Parts.Sunvisor
{
	public class SunvisorSlot : VehicleSlot
	{
		public UnityEvent<Sunvisor> OnSunvisorInstalled;

		public UnityEvent<Sunvisor> OnSunvisorRemoved;

		public Sunvisor InstalledSunvisor { get; set; }

		protected override void OnEnable()
		{
			base.OnEnable();
			base.OnObjectAttached.AddListener(InstallSunvisor);
			base.OnObjectDetached.AddListener(RemoveSunvisor);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.OnObjectAttached.RemoveListener(InstallSunvisor);
			base.OnObjectDetached.RemoveListener(RemoveSunvisor);
		}

		private void InstallSunvisor()
		{
			Sunvisor component = attachedObject.GetComponent<Sunvisor>();
			component.SunvisorSlot = this;
			InstalledSunvisor = component;
			OnSunvisorInstalled.Invoke(component);
		}

		private void RemoveSunvisor()
		{
			OnSunvisorRemoved.Invoke(InstalledSunvisor);
			InstalledSunvisor = null;
		}
	}
}
