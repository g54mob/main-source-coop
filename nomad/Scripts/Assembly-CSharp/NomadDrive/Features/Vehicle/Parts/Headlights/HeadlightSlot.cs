using NomadDrive.Features.Vehicle.Modules.Slots;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Parts.Headlights
{
	public class HeadlightSlot : VehicleSlot
	{
		public UnityEvent<Headlight> OnHeadlightInstalled;

		public UnityEvent OnHeadlightRemoved;

		public Headlight InstalledHeadlight { get; set; }

		protected override void OnEnable()
		{
			base.OnEnable();
			base.OnObjectAttached.AddListener(InstallHeadlight);
			base.OnObjectDetached.AddListener(RemoveHeadlight);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.OnObjectAttached.RemoveListener(InstallHeadlight);
			base.OnObjectDetached.RemoveListener(RemoveHeadlight);
		}

		private void InstallHeadlight()
		{
			Headlight component = attachedObject.GetComponent<Headlight>();
			component.HeadlightSlot = this;
			InstalledHeadlight = component;
			OnHeadlightInstalled.Invoke(component);
		}

		private void RemoveHeadlight()
		{
			InstalledHeadlight = null;
			OnHeadlightRemoved.Invoke();
		}
	}
}
