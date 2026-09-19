using UnityEngine;

namespace Features.GrabModule.Scripts.PhysGrab
{
	public sealed class EnemyCarryHandle : MonoBehaviour
	{
		[SerializeField]
		private Transform _handle;

		[SerializeField]
		private bool _lockToCarryAnchor = true;

		[SerializeField]
		private Rigidbody _rigidbody;

		private bool _hasActiveLock;

		private bool _previousIsKinematic;

		public Transform Handle
		{
			get
			{
				if (!(_handle != null))
				{
					return base.transform;
				}
				return _handle;
			}
		}

		public bool LockToCarryAnchor => _lockToCarryAnchor;

		public void ApplyCarryPhysicsLock()
		{
			if (!_lockToCarryAnchor)
			{
				return;
			}
			Rigidbody rigidbody = ResolveRigidbody();
			if (!(rigidbody == null))
			{
				if (!_hasActiveLock)
				{
					_previousIsKinematic = rigidbody.isKinematic;
					_hasActiveLock = true;
				}
				rigidbody.linearVelocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;
				rigidbody.isKinematic = true;
			}
		}

		public void ReleaseCarryPhysicsLock()
		{
			Rigidbody rigidbody = ResolveRigidbody();
			if (!(rigidbody == null))
			{
				rigidbody.linearVelocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;
				if (_hasActiveLock)
				{
					rigidbody.isKinematic = _previousIsKinematic;
				}
				_hasActiveLock = false;
			}
		}

		private Rigidbody ResolveRigidbody()
		{
			if (_rigidbody != null)
			{
				return _rigidbody;
			}
			_rigidbody = GetComponentInParent<Rigidbody>();
			return _rigidbody;
		}
	}
}
