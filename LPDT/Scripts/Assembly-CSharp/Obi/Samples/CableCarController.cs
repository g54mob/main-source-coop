using UnityEngine;

namespace Obi.Samples
{
	public class CableCarController : MonoBehaviour
	{
		public ObiPinhole pinhole;

		public float carSpeed = 1f;

		private void Update()
		{
			float motorSpeed = 0f;
			if (Input.GetKey(KeyCode.W))
			{
				motorSpeed = carSpeed;
			}
			if (Input.GetKey(KeyCode.S))
			{
				motorSpeed = 0f - carSpeed;
			}
			pinhole.motorSpeed = motorSpeed;
			if (Input.GetKeyDown(KeyCode.Space))
			{
				pinhole.friction = ((!(pinhole.friction > 0.5f)) ? 1 : 0);
			}
		}
	}
}
