using UnityEngine;

namespace pworld.Scripts.PPhys
{
	public class RotTest : MonoBehaviour
	{
		public Vector3 Target;

		public Vector3 Velocity;

		private float spring = 15f;

		private float damp = 15f;

		private void Update()
		{
			RotPhys();
		}

		private void RotPhys()
		{
			float deltaTime = Time.deltaTime;
			_ = Target;
			Vector3 vector = Vector3.Cross(base.transform.up, Target).normalized * Vector3.Angle(base.transform.up, Target);
			Vector3 vector2 = Vector3.Cross(base.transform.forward, Vector3.forward).normalized * Vector3.Angle(base.transform.forward, Vector3.forward);
			Velocity = FRILerp.PLerp(Velocity, (vector + vector2) * spring, damp, deltaTime);
			base.transform.Rotate(Velocity * deltaTime);
		}
	}
}
