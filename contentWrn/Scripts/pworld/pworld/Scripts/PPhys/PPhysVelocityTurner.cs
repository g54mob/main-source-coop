using UnityEngine;

namespace pworld.Scripts.PPhys
{
	public class PPhysVelocityTurner : MonoBehaviour
	{
		public float amount = 1f;

		private PPhysSpringBase move_g;

		private void Awake()
		{
			move_g = GetComponent<PPhysSpringBase>();
		}

		private void FixedUpdate()
		{
			float magnitude = move_g.Velocity.magnitude;
			Vector3 vector = FRILerp.Lerp(move_g.Velocity.normalized, base.transform.forward, amount);
			move_g.Velocity = magnitude * vector * Time.fixedDeltaTime;
		}
	}
}
