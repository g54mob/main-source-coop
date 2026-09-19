using System;
using Features.GrabModule.Scripts.PhysGrab;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy
{
	[Serializable]
	public class AnchorPendant
	{
		[SerializeField]
		private Transform _transform;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private GrabObject _grabObject;

		[SerializeField]
		private Transform _grabPoint;

		[SerializeField]
		private SpringJoint _returnSpringJoint;

		[SerializeField]
		private Collider[] _colliders;

		[SerializeField]
		private Renderer[] _renderers;

		[SerializeField]
		private Vector3 _restLocalPosition = new Vector3(0f, -0.5f, 0f);

		[SerializeField]
		private Vector3 _restLocalEuler;

		[SerializeField]
		[FormerlySerializedAs("_flightWorldEuler")]
		private Vector3 _flightLocalEuler = new Vector3(-90f, 0f, 0f);

		private RigidbodyConstraints _constraintsAtRest;

		public Transform Transform => _transform;

		public Rigidbody Rigidbody => _rigidbody;

		public GrabObject GrabObject => _grabObject;

		public Transform GrabPoint => _grabPoint;

		public Collider[] Colliders => _colliders;

		public Renderer[] Renderers => _renderers;

		public Vector3 WorldPosition
		{
			get
			{
				if (!(_transform != null))
				{
					return Vector3.zero;
				}
				return _transform.position;
			}
		}

		public void Assign(Transform transform, Rigidbody rigidbody, GrabObject grabObject, Transform grabPoint, SpringJoint returnSpringJoint, Collider[] colliders, Renderer[] renderers, Vector3 restLocalPosition, Vector3 restLocalEuler, Vector3 flightLocalEuler)
		{
			_transform = transform;
			_rigidbody = rigidbody;
			_grabObject = grabObject;
			_grabPoint = grabPoint;
			_returnSpringJoint = returnSpringJoint;
			_colliders = colliders;
			_renderers = renderers;
			_restLocalPosition = restLocalPosition;
			_restLocalEuler = restLocalEuler;
			_flightLocalEuler = flightLocalEuler;
		}

		public void Initialize()
		{
			if (_rigidbody != null)
			{
				_constraintsAtRest = _rigidbody.constraints;
			}
		}

		public void TeleportTo(Vector3 worldPosition)
		{
			AnchorPhysicsUtil.ApplyWorldPosition(_rigidbody, _transform, worldPosition);
		}

		public void SnapRelativeTo(Vector3 spherePosition, Quaternion sphereRotation)
		{
			if (!(_transform == null))
			{
				Quaternion rotation = sphereRotation * Quaternion.Euler(_restLocalEuler);
				Vector3 position = spherePosition + sphereRotation * _restLocalPosition;
				AnchorPhysicsUtil.ApplyPose(_rigidbody, _transform, position, rotation);
			}
		}

		public void SnapFlightRelativeTo(Vector3 spherePosition, Quaternion sphereRotation)
		{
			if (!(_transform == null))
			{
				Quaternion rotation = sphereRotation * Quaternion.Euler(_flightLocalEuler);
				Vector3 position = spherePosition + sphereRotation * _restLocalPosition;
				AnchorPhysicsUtil.ApplyPose(_rigidbody, _transform, position, rotation);
			}
		}

		public void SetReturnSpring(float spring, float damper, float massScale)
		{
			if (!(_returnSpringJoint == null))
			{
				bool flag = spring > 0f;
				_returnSpringJoint.spring = spring;
				_returnSpringJoint.damper = (flag ? damper : 0f);
				_returnSpringJoint.massScale = (flag ? massScale : 1f);
			}
		}

		public void LockFlightRotation()
		{
			if (_rigidbody != null)
			{
				_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			}
		}

		public void UnlockFlightRotation()
		{
			if (_rigidbody != null)
			{
				_rigidbody.constraints = _constraintsAtRest;
			}
		}

		public float GetSpeed()
		{
			if (_rigidbody != null && !_rigidbody.isKinematic)
			{
				return _rigidbody.linearVelocity.magnitude;
			}
			return 0f;
		}
	}
}
