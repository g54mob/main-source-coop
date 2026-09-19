using UnityEngine;

namespace Features.RigidbodyVelocityClampModule.Scripts
{
	public class RigidbodyVelocityClamper : MonoBehaviour
	{
		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private float _maxVelocity = 10f;

		private void FixedUpdate()
		{
			if (!(_rigidbody == null))
			{
				Vector3 linearVelocity = _rigidbody.linearVelocity;
				if (linearVelocity.sqrMagnitude > _maxVelocity * _maxVelocity)
				{
					_rigidbody.linearVelocity = linearVelocity.normalized * _maxVelocity;
				}
			}
		}
	}
}
