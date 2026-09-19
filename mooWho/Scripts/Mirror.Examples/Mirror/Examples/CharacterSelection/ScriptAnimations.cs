using UnityEngine;

namespace Mirror.Examples.CharacterSelection
{
	public class ScriptAnimations : MonoBehaviour
	{
		public float minimum = 0.1f;

		public float maximum = 0.5f;

		private float yPos;

		public float bounceSpeed = 3f;

		private float yStartPosition;

		private void Start()
		{
			yStartPosition = base.transform.localPosition.y;
		}

		private void Update()
		{
			float num = Mathf.Sin(Time.time * bounceSpeed);
			yPos = Mathf.Lerp(maximum, minimum, Mathf.Abs((1f + num) / 2f));
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, yStartPosition + yPos, base.transform.localPosition.z);
		}
	}
}
