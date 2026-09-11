using UnityEngine;

namespace pworld.Scripts.PPhys
{
	public class PPhysTurnToVelocity : MonoBehaviour
	{
		private PPhysSpringBase move_g;

		private void Awake()
		{
			move_g = GetComponent<PPhysSpringBase>();
		}

		private void Update()
		{
			if (move_g.Velocity.magnitude > 0f)
			{
				base.transform.forward = move_g.Velocity.normalized;
			}
		}
	}
}
