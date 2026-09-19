using System;
using Features.GrabModule.Scripts.PhysGrab;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy
{
	[Serializable]
	public class AnchorSpringReel
	{
		[SerializeField]
		private PhysGrabber _pullGrabber;

		[SerializeField]
		private Transform _backGrabPoint;

		[SerializeField]
		private float _returnSpringStrength = 18f;

		[SerializeField]
		private float _returnSpringDamper = 2f;

		[SerializeField]
		private float _returnSpringMassScale = 1f;

		public void Assign(PhysGrabber pullGrabber, Transform backGrabPoint, float returnSpringStrength, float returnSpringDamper, float returnSpringMassScale)
		{
			_pullGrabber = pullGrabber;
			_backGrabPoint = backGrabPoint;
			_returnSpringStrength = returnSpringStrength;
			_returnSpringDamper = returnSpringDamper;
			_returnSpringMassScale = returnSpringMassScale;
		}

		public void Begin(AnchorThrowSphere sphere, AnchorPendant pendant, AnchorPlayerLatch latch, AnchorFlight flight, ref bool physGrabReeling)
		{
			physGrabReeling = true;
			if (latch.IsLatched)
			{
				pendant.SetReturnSpring(0f, 0f, 1f);
				ClearPullGrabbers(sphere, pendant);
				AnchorPhysicsUtil.ZeroVelocity(sphere.Rigidbody);
				AnchorPhysicsUtil.ZeroVelocity(pendant.Rigidbody);
				if (sphere.Rigidbody != null)
				{
					sphere.Rigidbody.isKinematic = true;
				}
				if (pendant.Rigidbody != null)
				{
					pendant.Rigidbody.isKinematic = true;
				}
				latch.Snap(sphere, pendant);
				return;
			}
			pendant.SetReturnSpring(_returnSpringStrength, _returnSpringDamper, _returnSpringMassScale);
			UseBackGrabPoint(sphere, pendant);
			flight.UnlockRotation(sphere, pendant);
			AddPullGrabbers(sphere);
			if (_pullGrabber != null)
			{
				_pullGrabber.IsProcessPhysGrabbing = true;
			}
			if (sphere.Rigidbody != null)
			{
				sphere.Rigidbody.isKinematic = false;
			}
			if (pendant.Rigidbody != null)
			{
				pendant.Rigidbody.isKinematic = false;
			}
		}

		public void ReturnToRest(AnchorThrowSphere sphere, AnchorPendant pendant, AnchorPlayerLatch latch, AnchorFlight flight, ref bool thrown, ref bool physGrabReeling)
		{
			latch.Clear();
			thrown = false;
			physGrabReeling = false;
			flight.ResetRuntime();
			ClearPullGrabbers(sphere, pendant);
			pendant.SetReturnSpring(0f, 0f, 1f);
			flight.UnlockRotation(sphere, pendant);
			AnchorPhysicsUtil.SetRigidbodyThrown(sphere.Rigidbody, thrown: false);
			AnchorPhysicsUtil.SetRigidbodyThrown(pendant.Rigidbody, thrown: false);
			flight.SnapPairToBone(sphere, pendant);
		}

		public void ClearPullGrabbers(AnchorThrowSphere sphere, AnchorPendant pendant)
		{
			RemoveGrabber(sphere.GrabObject);
			RemoveGrabber(pendant.GrabObject);
			if (_pullGrabber != null)
			{
				_pullGrabber.IsProcessPhysGrabbing = false;
			}
		}

		private void AddPullGrabbers(AnchorThrowSphere sphere)
		{
			AddGrabber(sphere.GrabObject);
		}

		private void AddGrabber(GrabObject grabObject)
		{
			if (!(grabObject == null) && !(_pullGrabber == null) && !grabObject.Grabbers.Contains(_pullGrabber))
			{
				grabObject.Grabbers.Add(_pullGrabber);
			}
		}

		private void RemoveGrabber(GrabObject grabObject)
		{
			if (!(grabObject == null) && !(_pullGrabber == null))
			{
				grabObject.Grabbers.Remove(_pullGrabber);
			}
		}

		private void UseBackGrabPoint(AnchorThrowSphere sphere, AnchorPendant pendant)
		{
			if (!(_pullGrabber == null) && !(_backGrabPoint == null))
			{
				if (sphere.GrabObject != null)
				{
					_pullGrabber.physGrabPoints[sphere.GrabObject] = _backGrabPoint;
				}
				if (!(pendant.GrabObject == null) && !(pendant.GrabPoint == null))
				{
					_pullGrabber.physGrabPoints[pendant.GrabObject] = pendant.GrabPoint;
				}
			}
		}
	}
}
