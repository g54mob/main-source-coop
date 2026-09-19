using UnityEngine;

namespace INab.Demo
{
	public class FloatingCoin : MonoBehaviour
	{
		public float rotationSpeed = 100f;

		public float floatSpeed = 0.5f;

		public float floatHeight = 0.5f;

		private Vector3 startPosition;

		private void Start()
		{
			startPosition = base.transform.position;
		}

		private void Update()
		{
			base.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
			Vector3 position = startPosition + new Vector3(0f, Mathf.Sin(Time.time * floatSpeed) * floatHeight, 0f);
			base.transform.position = position;
		}
	}
}
