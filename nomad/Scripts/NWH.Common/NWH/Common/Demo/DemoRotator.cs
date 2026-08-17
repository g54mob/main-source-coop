using UnityEngine;

namespace NWH.Common.Demo
{
	public class DemoRotator : MonoBehaviour
	{
		public Vector3 rotation;

		private Rigidbody _rb;

		private void Start()
		{
			_rb = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			_rb.MoveRotation(base.transform.rotation * Quaternion.Euler(rotation * Time.fixedDeltaTime));
		}
	}
}
