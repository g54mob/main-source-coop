using UnityEngine;

namespace Obi.Samples
{
	[RequireComponent(typeof(Rigidbody))]
	public class RigidbodyMaxAngularVel : MonoBehaviour
	{
		public float maxAngularVelocity = 20f;

		private void Start()
		{
			GetComponent<Rigidbody>().maxAngularVelocity = maxAngularVelocity;
		}

		private void Update()
		{
		}
	}
}
