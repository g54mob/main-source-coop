using UnityEngine;

namespace Boxophobic.Utility
{
	public class CamController : MonoBehaviour
	{
		public float movementSpeed = 5f;

		public float accelerationMultiplier = 2f;

		public float sensitivity = 2f;

		private float yaw;

		private float pitch;

		private void Start()
		{
			yaw = base.transform.eulerAngles.y;
			pitch = base.transform.eulerAngles.x;
		}

		private void Update()
		{
			float num = movementSpeed;
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				num *= accelerationMultiplier;
			}
			float x = Input.GetAxis("Horizontal") * num * Time.deltaTime;
			float z = Input.GetAxis("Vertical") * num * Time.deltaTime;
			base.transform.Translate(x, 0f, z);
			yaw += sensitivity * Input.GetAxis("Mouse X");
			pitch -= sensitivity * Input.GetAxis("Mouse Y");
			pitch = Mathf.Clamp(pitch, -90f, 90f);
			base.transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
		}
	}
}
