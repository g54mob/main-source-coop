using UnityEngine;

namespace pworld.Scripts
{
	public class PRotationalShake : MonoBehaviour
	{
		public Vector3 velocity;

		public float spring = 15f;

		public float damp = 15f;

		private void Awake()
		{
		}

		private void Update()
		{
			Vector3 up = Vector3.up;
			Vector3 forward = Vector3.forward;
			Vector3 vector = Vector3.Cross(base.transform.up, up).normalized * Vector3.Angle(base.transform.up, up);
			Vector3 vector2 = Vector3.Cross(base.transform.forward, forward).normalized * Vector3.Angle(base.transform.forward, forward);
			velocity = FRILerp.Lerp(velocity, (vector2 + vector) * spring, damp, useTimeScale: false);
			base.transform.Rotate(velocity * Time.deltaTime, Space.World);
		}
	}
}
