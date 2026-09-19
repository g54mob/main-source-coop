using System;
using Features.GrabModule.Scripts.PhysGrab;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy
{
	[Serializable]
	public class AnchorThrowSphere
	{
		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private GrabObject _grabObject;

		[SerializeField]
		private Transform _restReference;

		[SerializeField]
		private Collider[] _colliders;

		[SerializeField]
		private Renderer[] _renderers;

		private Transform _hostTransform;

		public Rigidbody Rigidbody => _rigidbody;

		public GrabObject GrabObject => _grabObject;

		public Transform RestReference => _restReference;

		public Collider[] Colliders => _colliders;

		public Renderer[] Renderers => _renderers;

		public Transform HostTransform => _hostTransform;

		public bool HasRestReference => _restReference != null;

		public float RestWorldY
		{
			get
			{
				if (!(_restReference != null))
				{
					if (!(_hostTransform != null))
					{
						return 0f;
					}
					return _hostTransform.position.y;
				}
				return _restReference.position.y;
			}
		}

		public void BindHost(Transform hostTransform)
		{
			_hostTransform = hostTransform;
		}

		public void Assign(Rigidbody rigidbody, GrabObject grabObject, Transform restReference, Collider[] colliders, Renderer[] renderers)
		{
			_rigidbody = rigidbody;
			_grabObject = grabObject;
			_restReference = restReference;
			_colliders = colliders;
			_renderers = renderers;
		}

		public void TeleportTo(Vector3 worldPosition)
		{
			AnchorPhysicsUtil.ApplyWorldPosition(_rigidbody, _hostTransform, worldPosition);
		}

		public void ApplyPose(Vector3 position, Quaternion rotation)
		{
			AnchorPhysicsUtil.ApplyPose(_rigidbody, _hostTransform, position, rotation);
		}

		public bool TryGetRestPose(out Vector3 position, out Quaternion rotation)
		{
			if (_restReference == null)
			{
				position = default(Vector3);
				rotation = default(Quaternion);
				return false;
			}
			position = _restReference.position;
			rotation = _restReference.rotation;
			return true;
		}
	}
}
