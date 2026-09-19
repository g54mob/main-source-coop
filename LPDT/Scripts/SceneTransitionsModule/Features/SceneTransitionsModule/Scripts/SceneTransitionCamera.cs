using System.Linq;
using UnityEngine;

namespace Features.SceneTransitionsModule.Scripts
{
	public class SceneTransitionCamera : MonoBehaviour
	{
		[SerializeField]
		private Camera _camera;

		private bool _noCameraInScene;

		private void Awake()
		{
			Object.DontDestroyOnLoad(this);
			_camera.enabled = false;
		}

		private void Update()
		{
			if (_noCameraInScene)
			{
				if (Camera.allCameras.Length > 1)
				{
					_camera.enabled = false;
					_noCameraInScene = false;
				}
			}
			else if (!Camera.allCameras.Any())
			{
				_camera.enabled = true;
				_noCameraInScene = true;
			}
		}
	}
}
