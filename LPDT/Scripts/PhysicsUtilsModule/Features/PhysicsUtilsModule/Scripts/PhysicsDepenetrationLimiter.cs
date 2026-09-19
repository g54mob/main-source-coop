using UnityEngine;

namespace Features.PhysicsUtilsModule.Scripts
{
	public class PhysicsDepenetrationLimiter : MonoBehaviour
	{
		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private float _depenetrationVelocity;

		private void Start()
		{
			_rigidbody.maxDepenetrationVelocity = _depenetrationVelocity;
		}
	}
}
