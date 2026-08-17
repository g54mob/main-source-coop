using NomadDrive.Features.Vehicle.Parts.Handbrake;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Modules
{
	public class VehicleHandbrakeModule : VehicleModule
	{
		private const string HANDBRAKE = "Handbrake";

		[SerializeField]
		public HandbrakeSlot HandbrakeSlotRef;

		[SerializeField]
		public Handbrake InstalledHandbrake;

		[SerializeField]
		private float _handbrakeValue = 1f;

		public bool IsHandbrakeOn => InstalledHandbrake?.IsHandbrakeOn ?? false;

		protected override void SubscribeEvents()
		{
			HandbrakeSlotRef.OnHandbrakeInstalled.AddListener(OnHandbrakeInstalled);
			HandbrakeSlotRef.OnHandbrakeRemoved.AddListener(OnHandbrakeRemoved);
		}

		protected override void UnsubscribeEvents()
		{
			HandbrakeSlotRef.OnHandbrakeInstalled.RemoveListener(OnHandbrakeInstalled);
			HandbrakeSlotRef.OnHandbrakeRemoved.RemoveListener(OnHandbrakeRemoved);
		}

		private void LateUpdate()
		{
			base.VehicleManager.VehicleController.input.Handbrake = ((InstalledHandbrake != null) ? _handbrakeValue : 0f);
		}

		private void OnHandbrakeInstalled(Handbrake handbrake)
		{
			InstalledHandbrake = handbrake;
			InstalledHandbrake.OnHandbrakeButtonOn.AddListener(OnHandbrakeButtonOn);
			InstalledHandbrake.OnHandbrakeButtonOff.AddListener(OnHandbrakeButtonOff);
			if (InstalledHandbrake.IsHandbrakeOn)
			{
				OnHandbrakeButtonOn();
			}
			else
			{
				OnHandbrakeButtonOff();
			}
		}

		private void OnHandbrakeRemoved(Handbrake handbrake)
		{
			InstalledHandbrake.OnHandbrakeButtonOn.RemoveListener(OnHandbrakeButtonOn);
			InstalledHandbrake.OnHandbrakeButtonOff.RemoveListener(OnHandbrakeButtonOff);
			InstalledHandbrake = null;
		}

		private void OnHandbrakeButtonOn()
		{
			_handbrakeValue = 0f;
			base.EventBus.FireHandbrakeStateChanged(isOn: true);
		}

		private void OnHandbrakeButtonOff()
		{
			_handbrakeValue = 1f;
			base.EventBus.FireHandbrakeStateChanged(isOn: false);
		}
	}
}
