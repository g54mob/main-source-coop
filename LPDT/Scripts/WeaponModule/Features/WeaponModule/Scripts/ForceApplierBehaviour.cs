using UnityEngine;

namespace Features.WeaponModule.Scripts
{
	[RequireComponent(typeof(Rigidbody))]
	public class ForceApplierBehaviour : MonoBehaviour
	{
		[SerializeField]
		private Rigidbody _rigidBody;

		public void ApplyForceCustom(Vector3 direction, float magnitude, ForceMode forceMode)
		{
			if (!(_rigidBody == null))
			{
				Vector3 force = direction.normalized * magnitude;
				_rigidBody.AddForce(force, forceMode);
			}
		}
	}
}
