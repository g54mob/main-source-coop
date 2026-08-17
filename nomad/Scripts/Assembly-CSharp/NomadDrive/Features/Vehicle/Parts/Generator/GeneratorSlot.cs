using NomadDrive.Features.Vehicle.Modules.Slots;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Parts.Generator
{
	public class GeneratorSlot : VehicleSlot
	{
		public UnityEvent<Generator> OnGeneratorInstalled { get; } = new UnityEvent<Generator>();

		public UnityEvent OnGeneratorRemoved { get; } = new UnityEvent();

		public Generator InstalledGenerator { get; set; }

		public bool IsInitialized { get; set; }

		protected override void OnEnable()
		{
			base.OnEnable();
			base.OnObjectAttached.AddListener(InstallGenerator);
			base.OnObjectDetached.AddListener(RemoveGenerator);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.OnObjectAttached.RemoveListener(InstallGenerator);
			base.OnObjectDetached.RemoveListener(RemoveGenerator);
		}

		private void InstallGenerator()
		{
			Generator arg = (InstalledGenerator = attachedObject.GetComponent<Generator>());
			OnGeneratorInstalled.Invoke(arg);
		}

		private void RemoveGenerator()
		{
			InstalledGenerator = null;
			OnGeneratorRemoved.Invoke();
		}
	}
}
