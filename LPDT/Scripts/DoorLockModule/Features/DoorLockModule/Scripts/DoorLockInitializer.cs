using Fusion;
using UnityEngine;

namespace Features.DoorLockModule.Scripts
{
	public class DoorLockInitializer : MonoBehaviour
	{
		[SerializeField]
		private NetworkObject _doorLockObject;

		[SerializeField]
		private Rigidbody _doorLockRigidBody;

		[SerializeField]
		private float _initialLockRotation;

		private void Start()
		{
			if (_doorLockObject.HasStateAuthority)
			{
				_doorLockRigidBody.rotation = Quaternion.Euler(_doorLockRigidBody.rotation.eulerAngles.x, _doorLockRigidBody.rotation.eulerAngles.y, _initialLockRotation);
			}
		}
	}
}
