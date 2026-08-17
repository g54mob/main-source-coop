using NomadDrive.Features.Vehicle.Modules.Slots;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Parts.Brakelight
{
	public class BrakelightSlot : VehicleSlot
	{
		public UnityEvent<Brakelight> OnBrakelightInstalled { get; } = new UnityEvent<Brakelight>();

		public UnityEvent<Brakelight> OnBrakelightRemoved { get; } = new UnityEvent<Brakelight>();

		public Brakelight InstalledBrakelight { get; set; }

		protected override void OnEnable()
		{
			base.OnEnable();
			base.OnObjectAttached.AddListener(InstallBrakelight);
			base.OnObjectDetached.AddListener(RemoveBrakelight);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.OnObjectAttached.RemoveListener(InstallBrakelight);
			base.OnObjectDetached.RemoveListener(RemoveBrakelight);
		}

		private void InstallBrakelight()
		{
			Brakelight component = attachedObject.GetComponent<Brakelight>();
			component.BrakelightSlot = this;
			InstalledBrakelight = component;
			OnBrakelightInstalled.Invoke(component);
		}

		private void RemoveBrakelight()
		{
			OnBrakelightRemoved.Invoke(InstalledBrakelight);
			InstalledBrakelight = null;
		}
	}
}
