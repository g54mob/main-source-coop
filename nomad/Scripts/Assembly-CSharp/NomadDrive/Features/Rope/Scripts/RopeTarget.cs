using UnityEngine;

namespace NomadDrive.Features.Rope.Scripts
{
	public class RopeTarget : MonoBehaviour
	{
		public Rope rope;

		public float minLength = 1f;

		public float maxLength = 10f;

		private float clength;

		public float length;

		private float vel;

		public float damp = 0.25f;

		public float speed = 1f;

		private void Start()
		{
			if ((bool)rope)
			{
				clength = (length = rope.length);
			}
		}

		private void Update()
		{
			if ((bool)rope)
			{
				length = Mathf.Clamp(length, minLength, maxLength);
				clength = Mathf.SmoothDamp(clength, length, ref vel, damp);
				rope.length = clength;
				rope.SetDirty();
			}
		}
	}
}
