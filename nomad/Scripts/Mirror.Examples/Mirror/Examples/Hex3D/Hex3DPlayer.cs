using UnityEngine;

namespace Mirror.Examples.Hex3D
{
	[AddComponentMenu("")]
	public class Hex3DPlayer : NetworkBehaviour
	{
		[Range(1f, 20f)]
		public float speed = 10f;

		private void Update()
		{
			if (base.isLocalPlayer)
			{
				float axis = Input.GetAxis("Horizontal");
				float axis2 = Input.GetAxis("Vertical");
				if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
				{
					Vector3 vector = new Vector3(axis, axis2, 0f);
					base.transform.position += vector.normalized * (Time.deltaTime * speed);
				}
				else
				{
					Vector3 vector2 = new Vector3(axis, 0f, axis2);
					base.transform.position += vector2.normalized * (Time.deltaTime * speed);
				}
				if (Input.GetKey(KeyCode.Q))
				{
					base.transform.Rotate(Vector3.up, -90f * Time.deltaTime);
				}
				if (Input.GetKey(KeyCode.E))
				{
					base.transform.Rotate(Vector3.up, 90f * Time.deltaTime);
				}
			}
		}

		private void OnGUI()
		{
			if (base.isLocalPlayer)
			{
				GUILayout.BeginArea(new Rect(10f, Screen.height - 50, 300f, 300f));
				GUILayout.Label("Use WASD+QE to move and rotate\nHold Shift with W/S to move up/down");
				GUILayout.EndArea();
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
