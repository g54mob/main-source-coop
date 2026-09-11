using UnityEngine;

namespace pworld.Scripts
{
	public class PFlyCam : MonoBehaviour
	{
		public Vector3 dirVelocity;

		public Vector3 rotationVelocity;

		public float moveSpeed = 100f;

		private void Start()
		{
		}

		private void Update()
		{
			base.transform.position += PSingleton<PInput>.Me.daeqws * moveSpeed * Time.deltaTime;
		}
	}
}
