using UnityEngine;

namespace CorgiGodRays
{
	public class DemoRotator : MonoBehaviour
	{
		public float speed = 10f;

		private void Update()
		{
			base.transform.RotateAround(base.transform.position, Vector3.up, Time.deltaTime * speed);
		}
	}
}
