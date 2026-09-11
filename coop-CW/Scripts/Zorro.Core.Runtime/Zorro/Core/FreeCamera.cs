using UnityEngine;
using Zorro.Core.CLI;

namespace Zorro.Core
{
	public class FreeCamera : MonoBehaviour
	{
		public float MovementForce = 50f;

		private Vector3 m_velocity;

		public float xRot;

		public float yRot;

		private void Update()
		{
			bool num = Singleton<DebugUIHandler>.Instance == null || !Singleton<DebugUIHandler>.Instance.IsOpen;
			Vector3 zero = Vector3.zero;
			float num2 = MovementForce * Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 5f : 1f);
			if (num)
			{
				if (Input.GetKey(KeyCode.W))
				{
					zero += base.transform.forward * num2;
				}
				if (Input.GetKey(KeyCode.S))
				{
					zero -= base.transform.forward * num2;
				}
				if (Input.GetKey(KeyCode.D))
				{
					zero += base.transform.right * num2;
				}
				if (Input.GetKey(KeyCode.A))
				{
					zero -= base.transform.right * num2;
				}
				if (Input.GetKey(KeyCode.E))
				{
					zero += base.transform.up * num2;
				}
				if (Input.GetKey(KeyCode.Q))
				{
					zero -= base.transform.up * num2;
				}
			}
			m_velocity += zero;
			base.transform.position += m_velocity * Time.deltaTime;
			if (num)
			{
				xRot += Input.GetAxis("Mouse X");
				yRot += Input.GetAxis("Mouse Y");
			}
			base.transform.rotation = Quaternion.Euler(0f, xRot, 0f) * Quaternion.Euler(0f - yRot, 0f, 0f);
		}

		private void FixedUpdate()
		{
			m_velocity *= 0.95f;
		}
	}
}
