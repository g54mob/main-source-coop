using System;
using System.Collections;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.FlipOver
{
	[Serializable]
	public class FlipOverModule : VehicleComponent
	{
		public enum FlipOverType
		{
			Gradual = 0,
			Instant = 1
		}

		public enum FlipOverActivation
		{
			Manual = 0,
			Automatic = 1
		}

		[Tooltip("Determines how the vehicle will be flipped over. ")]
		public FlipOverType flipOverType = FlipOverType.Instant;

		public FlipOverActivation flipOverActivation;

		[Tooltip("    Minimum angle that the vehicle needs to be at for it to be detected as flipped over.")]
		public float allowedAngle = 70f;

		[Tooltip("If using instant (not gradual) flip over this value will be applied to the transform.y position to prevent rotating\r\nthe object to a position that is underground.")]
		public float instantFlipOverVerticalOffset = 1f;

		[Tooltip("    Is the vehicle flipped over?")]
		public bool flippedOver;

		[Tooltip("    Flip over detection will be disabled if velocity is above this value [m/s].")]
		public float maxDetectionSpeed = 0.6f;

		[Tooltip("Time after detecting flip over after which vehicle will be flipped back or the manual button can be used.")]
		public float timeout = 1f;

		[Tooltip("How long the flip over process will take if using gradual flip over.")]
		public float flipOverDuration = 5f;

		private bool _flipOverInProgress;

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				vehicleController.StartCoroutine(FlipOverCheckCoroutine());
				return true;
			}
			return false;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				vehicleController.StopCoroutine(FlipOverCheckCoroutine());
				return true;
			}
			return false;
		}

		private IEnumerator FlipOverCheckCoroutine()
		{
			while (true)
			{
				float num = Vector3.Angle(vehicleController.transform.up, -Physics.gravity.normalized);
				flippedOver = vehicleController.Speed < maxDetectionSpeed && vehicleController.vehicleRigidbody.angularVelocity.magnitude < maxDetectionSpeed && num > allowedAngle;
				if (!_flipOverInProgress && flippedOver && ((vehicleController.input.FlipOver && flipOverActivation == FlipOverActivation.Manual) || flipOverActivation == FlipOverActivation.Automatic))
				{
					if (flipOverType == FlipOverType.Gradual)
					{
						vehicleController.StartCoroutine(FlipOverGraduallyCoroutine());
					}
					else
					{
						FlipOverInstantly();
						yield return new WaitForSeconds(1f);
					}
				}
				vehicleController.input.FlipOver = false;
				yield return new WaitForSeconds(timeout);
			}
		}

		private IEnumerator FlipOverGraduallyCoroutine()
		{
			float timer = 0f;
			RigidbodyConstraints initConstraints = vehicleController.vehicleRigidbody.constraints;
			Quaternion initRotation = vehicleController.transform.rotation;
			Quaternion targetRotation = ((Mathf.Abs(Vector3.Dot(vehicleController.transform.forward, Vector3.up)) < 0.7f) ? Quaternion.LookRotation(vehicleController.transform.forward, Vector3.up) : Quaternion.LookRotation(vehicleController.transform.up, Vector3.up));
			float initialDrag = vehicleController.vehicleRigidbody.linearDamping;
			float initialAngularDrag = vehicleController.vehicleRigidbody.angularDamping;
			vehicleController.vehicleRigidbody.linearDamping = 30f;
			vehicleController.vehicleRigidbody.angularDamping = 30f;
			while (timer < 20f)
			{
				float num = timer / flipOverDuration;
				if (num > 1f)
				{
					vehicleController.vehicleRigidbody.constraints = initConstraints;
					_flipOverInProgress = false;
					break;
				}
				vehicleController.vehicleRigidbody.constraints = (RigidbodyConstraints)10;
				vehicleController.vehicleRigidbody.MoveRotation(Quaternion.Slerp(initRotation, targetRotation, num));
				_flipOverInProgress = true;
				timer += Time.fixedDeltaTime;
				yield return new WaitForFixedUpdate();
			}
			timer = 0f;
			while (timer < 1f)
			{
				vehicleController.vehicleRigidbody.linearDamping = Mathf.Lerp(30f, initialDrag, timer);
				vehicleController.vehicleRigidbody.angularDamping = Mathf.Lerp(30f, initialAngularDrag, timer);
				timer += Time.fixedDeltaTime;
				yield return new WaitForFixedUpdate();
			}
			vehicleController.vehicleRigidbody.linearDamping = initialDrag;
			_flipOverInProgress = false;
			yield return null;
		}

		private void FlipOverInstantly()
		{
			Quaternion rot = ((Mathf.Abs(Vector3.Dot(vehicleController.transform.forward, Vector3.up)) < 0.7f) ? Quaternion.LookRotation(vehicleController.transform.forward, Vector3.up) : Quaternion.LookRotation(vehicleController.transform.up, Vector3.up));
			vehicleController.vehicleRigidbody.MoveRotation(rot);
			vehicleController.vehicleRigidbody.MovePosition(vehicleController.transform.position + Vector3.up * instantFlipOverVerticalOffset);
		}
	}
}
