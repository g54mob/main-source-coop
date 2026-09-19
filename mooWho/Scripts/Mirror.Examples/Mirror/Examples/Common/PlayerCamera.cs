using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mirror.Examples.Common
{
	[AddComponentMenu("")]
	[DisallowMultipleComponent]
	public class PlayerCamera : NetworkBehaviour
	{
		private Camera mainCam;

		public Vector3 offset = new Vector3(0f, 3f, -8f);

		public Vector3 rotation = new Vector3(10f, 0f, 0f);

		private void Awake()
		{
			mainCam = Camera.main;
		}

		public override void OnStartLocalPlayer()
		{
			if (mainCam != null)
			{
				mainCam.orthographic = false;
				mainCam.transform.SetParent(base.transform);
				mainCam.transform.localPosition = offset;
				mainCam.transform.localEulerAngles = rotation;
			}
			else
			{
				Debug.LogWarning("PlayerCamera: Could not find a camera in scene with 'MainCamera' tag.");
			}
		}

		private void OnApplicationQuit()
		{
			ReleaseCamera();
		}

		public override void OnStopLocalPlayer()
		{
			ReleaseCamera();
		}

		private void OnDisable()
		{
			ReleaseCamera();
		}

		private void OnDestroy()
		{
			ReleaseCamera();
		}

		private void ReleaseCamera()
		{
			if (mainCam != null && mainCam.transform.parent == base.transform)
			{
				mainCam.transform.SetParent(null);
				mainCam.orthographic = true;
				mainCam.orthographicSize = 15f;
				mainCam.transform.localPosition = new Vector3(0f, 70f, 0f);
				mainCam.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
				if (mainCam.gameObject.scene != SceneManager.GetActiveScene())
				{
					SceneManager.MoveGameObjectToScene(mainCam.gameObject, SceneManager.GetActiveScene());
				}
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
