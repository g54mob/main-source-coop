using UnityEngine;

namespace Features.GrabModule.Scripts.PhysGrab
{
	public class CartAngularLock : MonoBehaviour
	{
		private const RigidbodyConstraints ANGULAR_LOCK = (RigidbodyConstraints)80;

		[SerializeField]
		private Rigidbody _rigidbody;

		public bool IsAngleLocked => (_rigidbody.constraints & (RigidbodyConstraints)80) == (RigidbodyConstraints)80;

		private void Awake()
		{
			if (_rigidbody == null)
			{
				TryGetComponent<Rigidbody>(out _rigidbody);
			}
		}

		public void UnlockAngle()
		{
			_rigidbody.constraints &= (RigidbodyConstraints)(-81);
		}

		public void LockAngle()
		{
			_rigidbody.constraints |= (RigidbodyConstraints)80;
			Vector3 angularVelocity = _rigidbody.angularVelocity;
			_rigidbody.angularVelocity = new Vector3(0f, angularVelocity.y, 0f);
			Vector3 eulerAngles = _rigidbody.rotation.eulerAngles;
			_rigidbody.rotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
		}
	}
}
