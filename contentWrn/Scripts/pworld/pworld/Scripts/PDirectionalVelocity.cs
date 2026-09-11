using UnityEngine;

namespace pworld.Scripts
{
	public class PDirectionalVelocity : MonoBehaviour
	{
		[SerializeField]
		private float turnRate;

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
			float magnitude = rig_g.linearVelocity.magnitude;
			Vector3 vector = Vector3.RotateTowards(rig_g.linearVelocity.normalized, base.transform.forward, 0f, turnRate * Time.fixedDeltaTime);
			rig_g.linearVelocity = vector * magnitude;
		}

		private void OnDestroy()
		{
		}
	}
}
