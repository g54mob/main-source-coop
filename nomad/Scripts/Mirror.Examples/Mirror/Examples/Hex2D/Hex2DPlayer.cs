using UnityEngine;

namespace Mirror.Examples.Hex2D
{
	[AddComponentMenu("")]
	public class Hex2DPlayer : NetworkBehaviour
	{
		[Range(1f, 20f)]
		public float speed = 15f;

		[Header("Diagnostics")]
		[ReadOnly]
		[SerializeField]
		private HexSpatialHash2DInterestManagement.CheckMethod checkMethod;

		private void Awake()
		{
			checkMethod = Object.FindAnyObjectByType<HexSpatialHash2DInterestManagement>().checkMethod;
		}

		private void Update()
		{
			if (base.isLocalPlayer)
			{
				float axis = Input.GetAxis("Horizontal");
				float axis2 = Input.GetAxis("Vertical");
				Vector3 vector = ((checkMethod != HexSpatialHash2DInterestManagement.CheckMethod.XY_FOR_2D) ? new Vector3(axis, 0f, axis2) : new Vector3(axis, axis2, 0f));
				base.transform.position += vector.normalized * (Time.deltaTime * speed);
			}
		}

		private void OnGUI()
		{
			if (base.isLocalPlayer)
			{
				GUILayout.BeginArea(new Rect(10f, Screen.height - 25, 300f, 300f));
				GUILayout.Label("Use WASD to move");
				GUILayout.EndArea();
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
