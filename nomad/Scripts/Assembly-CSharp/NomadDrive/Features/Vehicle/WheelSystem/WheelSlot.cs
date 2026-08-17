using Ami.BroAudio;
using EvilCore.Networking.Parenting;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.Vehicle.Modules.Slots;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.WheelSystem
{
	public class WheelSlot : VehicleSlot
	{
		public Transform RotatingTransform;

		[SerializeField]
		private Transform nonRotatingVisual;

		[Header("Audio")]
		[SerializeField]
		private SoundID attachSound;

		[SerializeField]
		private SoundID detachSound;

		private Vector3 _calculatedLocalRotation;

		public UnityEvent OnTireInstalled { get; } = new UnityEvent();

		public UnityEvent OnTireRemoved { get; } = new UnityEvent();

		public Tire InstalledTire { get; set; }

		[field: SerializeField]
		public WheelLocation WheelSlotLocation { get; set; }

		private bool HasMissingAlternativeSyncTransform
		{
			get
			{
				if (!TryGetComponent<NetworkedTransform>(out var component))
				{
					return false;
				}
				return component.alternativeSyncTransform == null;
			}
		}

		private bool HasMissingRotatingTransform => RotatingTransform == null;

		protected override void Awake()
		{
			base.Awake();
			_ = RotatingTransform == null;
			if (WheelSlotLocation.Equals(WheelLocation.FrontLeft) || WheelSlotLocation.Equals(WheelLocation.RearLeft))
			{
				_calculatedLocalRotation = new Vector3(0f, 0f, 0f);
			}
			else
			{
				_calculatedLocalRotation = new Vector3(0f, 0f, 180f);
			}
			if (nonRotatingVisual != null)
			{
				nonRotatingVisual.gameObject.SetActive(value: true);
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			base.OnObjectAttached.AddListener(InstallTire);
			base.OnObjectDetached.AddListener(RemoveTire);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.OnObjectAttached.RemoveListener(InstallTire);
			base.OnObjectDetached.RemoveListener(RemoveTire);
		}

		private void InstallTire()
		{
			if (attachSound.IsValid())
			{
				AudioManager?.PlayOneShot(attachSound, base.transform.position);
			}
			Tire tire = (InstalledTire = attachedObject.GetComponent<Tire>());
			tire.rotatingOffset = _calculatedLocalRotation;
			tire.SetModelTransformRotation(_calculatedLocalRotation);
			if (nonRotatingVisual != null)
			{
				nonRotatingVisual.gameObject.SetActive(value: false);
			}
			if (tire.rotatingTransform != null)
			{
				tire.rotatingTransform.gameObject.SetActive(value: true);
			}
			OnTireInstalled.Invoke();
		}

		private void RemoveTire()
		{
			if (detachSound.IsValid())
			{
				AudioManager?.PlayOneShot(detachSound, base.transform.position);
			}
			InstalledTire.rotatingOffset = Vector3.zero;
			InstalledTire.ResetModelTransformRotation();
			if (nonRotatingVisual != null)
			{
				nonRotatingVisual.gameObject.SetActive(value: true);
			}
			if (InstalledTire.rotatingTransform != null)
			{
				InstalledTire.rotatingTransform.gameObject.SetActive(value: false);
			}
			OnTireRemoved.Invoke();
		}

		protected override Quaternion AdjustHighlightLocalRotation(Quaternion localRotation)
		{
			if (WheelSlotLocation != WheelLocation.FrontRight && WheelSlotLocation != WheelLocation.RearRight)
			{
				return localRotation;
			}
			return localRotation * Quaternion.Euler(0f, 180f, 0f);
		}

		public override void ApplyAttachPreview(AttachableObject attachable)
		{
			if (attachable != null && attachable.TryGetComponent<Tire>(out var component))
			{
				component.SetModelTransformRotation(_calculatedLocalRotation);
			}
		}

		public override void ClearAttachPreview(AttachableObject attachable)
		{
			if (attachable != null && attachable.TryGetComponent<Tire>(out var component))
			{
				component.ResetModelTransformRotation();
			}
		}

		public override void DriveAttachedPartPose(Transform vehicleRoot)
		{
		}
	}
}
