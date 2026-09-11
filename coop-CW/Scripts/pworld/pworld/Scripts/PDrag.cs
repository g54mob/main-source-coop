using UnityEngine;

namespace pworld.Scripts
{
	public class PDrag : MonoBehaviour
	{
		public float dragAmount = 1f;

		public float dragPow = 2f;

		private Rigidbody rig_g;

		private void Awake()
		{
			rig_g = GetComponent<Rigidbody>();
		}

		private void Start()
		{
		}

		private void FixedUpdate()
		{
			float value = Mathf.Pow(rig_g.linearVelocity.magnitude + 1f, dragPow) * dragAmount;
			value = Mathf.Clamp(value, 0f, rig_g.linearVelocity.magnitude);
			Vector3 vector = rig_g.linearVelocity.normalized * value;
			rig_g.linearVelocity -= vector;
		}
	}
}
