using UnityEngine;

namespace pworld.Scripts
{
	public class PFreeCam : MonoBehaviour
	{
		public float pitch;

		public float jaw;

		public float speed;

		public float stabilizer;

		public Rigidbody rig_g;

		private void Awake()
		{
			rig_g = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			Vector3 vector = PSingleton<PInput>.Me.daeqws * (speed * Time.fixedDeltaTime);
			Vector3 vector2 = Vector3.Cross(base.transform.forward, base.transform.right) * (PSingleton<PInput>.Me.mouseD.x * pitch);
			Vector3 vector3 = Vector3.Cross(base.transform.forward, base.transform.up) * (PSingleton<PInput>.Me.mouseD.y * jaw);
			Vector3 vector4 = Vector3.Cross(base.transform.up, base.transform.right);
			Vector3 vector5 = vector4 * (Vector3.SignedAngle(base.transform.up, Vector3.up, vector4) * stabilizer);
			rig_g.AddTorque(vector3 + vector2 + vector5, ForceMode.Acceleration);
			vector = base.transform.TransformVector(vector);
			rig_g.AddForce(vector, ForceMode.Acceleration);
		}
	}
}
