using UnityEngine;

namespace Highlighters
{
	public class ObjectOscilate : MonoBehaviour
	{
		public float minX;

		public float maxX;

		public float speed = 1f;

		private void Update()
		{
			float t = Mathf.PingPong(Time.time * speed, 1f);
			float x = Mathf.Lerp(minX, maxX, t);
			base.transform.localPosition = new Vector3(x, base.transform.localPosition.y, base.transform.localPosition.z);
		}
	}
}
