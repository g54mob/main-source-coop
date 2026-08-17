using NomadDrive.Features.Vehicle.Modules.Slots;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Parts.Hood
{
	public class HoodSlot : VehicleSlot
	{
		public UnityEvent<Hood> OnHoodInstalled;

		public UnityEvent OnHoodRemoved;

		public Hood InstalledHood { get; set; }

		protected override void OnEnable()
		{
			base.OnEnable();
			base.OnObjectAttached.AddListener(InstallHood);
			base.OnObjectDetached.AddListener(RemoveHood);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.OnObjectAttached.RemoveListener(InstallHood);
			base.OnObjectDetached.RemoveListener(RemoveHood);
		}

		private void InstallHood()
		{
			Hood component = attachedObject.GetComponent<Hood>();
			component.HoodSlot = this;
			InstalledHood = component;
			OnHoodInstalled.Invoke(component);
		}

		private void RemoveHood()
		{
			InstalledHood = null;
			OnHoodRemoved.Invoke();
		}
	}
}
