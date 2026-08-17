using NomadDrive.Features.Vehicle.Modules.Slots;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Parts.TowKnob
{
	public class TowKnobSlot : VehicleSlot
	{
		public UnityEvent<TowKnob> OnTowKnobInstalled { get; } = new UnityEvent<TowKnob>();

		public UnityEvent OnTowKnobRemoved { get; } = new UnityEvent();

		public TowKnob InstalledTowKnob { get; set; }

		protected override void OnEnable()
		{
			base.OnEnable();
			base.OnObjectAttached.AddListener(InstallTowKnob);
			base.OnObjectDetached.AddListener(RemoveTowKnob);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.OnObjectAttached.RemoveListener(InstallTowKnob);
			base.OnObjectDetached.RemoveListener(RemoveTowKnob);
		}

		private void InstallTowKnob()
		{
			TowKnob component = attachedObject.GetComponent<TowKnob>();
			component.TowKnobSlot = this;
			InstalledTowKnob = component;
			OnTowKnobInstalled.Invoke(component);
		}

		private void RemoveTowKnob()
		{
			InstalledTowKnob = null;
			OnTowKnobRemoved.Invoke();
		}
	}
}
