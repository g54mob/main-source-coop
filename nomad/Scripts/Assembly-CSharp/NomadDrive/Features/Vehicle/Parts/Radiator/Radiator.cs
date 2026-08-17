using Ami.BroAudio;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.LiquidTransferSystem;
using PrimeTween;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Parts.Radiator
{
	[RequireComponent(typeof(ConditionComponent))]
	public class Radiator : AttachableLiquidContainer
	{
		[SerializeField]
		public RadiatorConfig radiatorConfig;

		[Header("Audio")]
		[SerializeField]
		private SoundID repairSound;

		[Header("Cap Animation")]
		[SerializeField]
		private Transform capTransform;

		[SerializeField]
		private Vector3 capOpenRotation = new Vector3(0f, 0f, 125f);

		[SerializeField]
		private Vector3 capClosedRotation = Vector3.zero;

		[SerializeField]
		private float capOpenDuration = 0.5f;

		[SerializeField]
		private float capCloseDuration = 0.5f;

		[SerializeField]
		private Ease capOpenEase = Ease.OutBounce;

		[SerializeField]
		private Ease capCloseEase = Ease.OutBounce;

		private bool _isCapOpen;

		public override void OnStartServer()
		{
			base.OnStartServer();
			if (radiatorConfig != null)
			{
				base.LiquidContainer.Capacity = radiatorConfig.coolantCapacity;
			}
		}

		protected override void OnHovered()
		{
			base.OnHovered();
			OpenCap();
		}

		protected override void OnUnhovered()
		{
			base.OnUnhovered();
			CloseCap();
		}

		private void OpenCap()
		{
			if (!_isCapOpen && !(capTransform == null))
			{
				_isCapOpen = true;
				Tween.LocalRotation(capTransform, Quaternion.Euler(capOpenRotation), capOpenDuration, capOpenEase);
			}
		}

		private void CloseCap()
		{
			if (_isCapOpen && !(capTransform == null))
			{
				_isCapOpen = false;
				Tween.LocalRotation(capTransform, Quaternion.Euler(capClosedRotation), capCloseDuration, capCloseEase);
			}
		}

		protected override void OnRepair()
		{
			base.OnRepair();
			if (repairSound.IsValid())
			{
				NetworkAudioRelay?.PlayOneShot(repairSound, base.transform.position);
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
