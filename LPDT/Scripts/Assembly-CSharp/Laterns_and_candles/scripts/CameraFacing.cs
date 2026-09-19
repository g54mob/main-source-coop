using Features.CameraModelModule;
using UnityEngine;
using Zenject;

namespace Laterns_and_candles.scripts
{
	public class CameraFacing : MonoBehaviour
	{
		private CameraModel _cameraModel;

		private Camera _targetCamera;

		[Inject]
		public void InjectDependencies(CameraModel cameraModel)
		{
			_cameraModel = cameraModel;
		}

		private void OnEnable()
		{
			_cameraModel.OnCameraObjectChanged += SetCameraTarget;
		}

		private void OnDisable()
		{
			_cameraModel.OnCameraObjectChanged -= SetCameraTarget;
		}

		private void Update()
		{
			if (!(_targetCamera == null) && !(_cameraModel.CameraObject == null))
			{
				Vector3 vector = _cameraModel.CameraObject.transform.position - base.transform.position;
				vector.x = (vector.z = 0f);
				base.transform.LookAt(_cameraModel.CameraObject.transform.position - vector);
			}
		}

		private void SetCameraTarget(Camera targetCamera)
		{
			_targetCamera = targetCamera;
		}
	}
}
