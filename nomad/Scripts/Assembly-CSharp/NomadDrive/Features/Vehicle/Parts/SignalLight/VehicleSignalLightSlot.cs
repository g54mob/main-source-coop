using NomadDrive.Features.Vehicle.Modules.Slots;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Parts.SignalLight
{
	public class VehicleSignalLightSlot : VehicleSlot
	{
		public UnityEvent<VehicleSignalLight> OnSignalLightInstalled { get; } = new UnityEvent<VehicleSignalLight>();

		public UnityEvent OnSignalLightRemoved { get; } = new UnityEvent();

		public VehicleSignalLight InstalledSignalLight { get; set; }

		protected override void OnEnable()
		{
			base.OnEnable();
			base.OnObjectAttached.AddListener(InstallSignalLight);
			base.OnObjectDetached.AddListener(RemoveSignalLight);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.OnObjectAttached.RemoveListener(InstallSignalLight);
			base.OnObjectDetached.RemoveListener(RemoveSignalLight);
		}

		private void InstallSignalLight()
		{
			VehicleSignalLight component = attachedObject.GetComponent<VehicleSignalLight>();
			component.SignalLightSlot = this;
			InstalledSignalLight = component;
			OnSignalLightInstalled.Invoke(component);
		}

		private void RemoveSignalLight()
		{
			InstalledSignalLight = null;
			OnSignalLightRemoved.Invoke();
		}
	}
}
