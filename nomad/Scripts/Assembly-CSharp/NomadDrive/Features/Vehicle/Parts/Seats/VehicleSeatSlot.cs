using Ami.BroAudio;
using NomadDrive.Features.Furnitures;
using NomadDrive.Features.Vehicle.Modules.Slots;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Parts.Seats
{
	public class VehicleSeatSlot : VehicleSlot
	{
		public UnityEvent<Seat> OnSeatInstalled;

		public UnityEvent OnSeatRemoved;

		[Header("Audio")]
		[SerializeField]
		private SoundID attachSound;

		[SerializeField]
		private SoundID detachSound;

		public Seat InstalledSeat { get; set; }

		[field: SerializeField]
		public bool IsDriverSlot { get; set; }

		protected override void OnEnable()
		{
			base.OnEnable();
			base.OnObjectAttached.AddListener(InstallSeat);
			base.OnObjectDetached.AddListener(RemoveSeat);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.OnObjectAttached.RemoveListener(InstallSeat);
			base.OnObjectDetached.RemoveListener(RemoveSeat);
		}

		private void InstallSeat()
		{
			Seat seat = (InstalledSeat = attachedObject.GetComponent<Seat>());
			((seat != null) ? seat.GetComponentInChildren<SittableSurface>(includeInactive: true) : null)?.ConfigureAsVehicleSeat(this, IsDriverSlot);
			OnSeatInstalled.Invoke(seat);
			if (attachSound.IsValid())
			{
				AudioManager?.PlayOneShot(attachSound, base.transform.position);
			}
		}

		private void RemoveSeat()
		{
			if (InstalledSeat != null)
			{
				InstalledSeat.GetComponentInChildren<SittableSurface>(includeInactive: true)?.ClearVehicleSeat();
			}
			InstalledSeat = null;
			OnSeatRemoved.Invoke();
			if (detachSound.IsValid())
			{
				AudioManager?.PlayOneShot(detachSound, base.transform.position);
			}
		}

		public override void DriveAttachedPartPose(Transform vehicleRoot)
		{
		}
	}
}
