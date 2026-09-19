using UnityEngine;

namespace Highlighters
{
	public class FloatingObject : MonoBehaviour
	{
		public float amplitude = 0.5f;

		public float frequency = 1f;

		private Vector3 startPos;

		private void Start()
		{
			startPos = base.transform.position;
		}

		private void Update()
		{
			float y = amplitude * Mathf.Sin(Time.time * frequency);
			base.transform.position = startPos + new Vector3(0f, y, 0f);
		}
	}
}
