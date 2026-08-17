using System.Collections.Generic;
using NWH.Common.Input;
using NWH.Common.Vehicles;
using UnityEngine;
using UnityEngine.Serialization;

namespace NWH.Common.Cameras
{
	[DefaultExecutionOrder(20)]
	public class CameraChanger : MonoBehaviour
	{
		[Tooltip("    If true vehicleCameras list will be filled through cameraTag.")]
		public bool autoFindCameras = true;

		[Tooltip("    Index of the camera from vehicle cameras list that will be active first.")]
		public int currentCameraIndex;

		[FormerlySerializedAs("vehicleCameras")]
		[Tooltip("List of cameras that the changer will cycle through. Leave empty if you want cameras to be automatically detected. To be detected cameras need to have camera tag and be children of the object this script is attached to.")]
		public List<GameObject> cameras = new List<GameObject>();

		private Vehicle _vehicle;

		private void Awake()
		{
			_vehicle = GetComponentInParent<Vehicle>();
			if (_vehicle == null)
			{
				Debug.LogError("None of the parent objects of CameraChanger contain VehicleController.");
			}
			_vehicle.onEnable.AddListener(EnableCurrentDisableOthers);
			_vehicle.onDisable.AddListener(DisableAllCameras);
			_vehicle.onMultiplayerStatusChanged.AddListener(OnMultiplayerInstanceTypeChanged);
			if (_vehicle == null)
			{
				Debug.Log("None of the parents of camera changer contain VehicleController component. Make sure that the camera changer is amongst the children of VehicleController object.");
			}
			if (autoFindCameras)
			{
				cameras = new List<GameObject>();
				Camera[] componentsInChildren = GetComponentsInChildren<Camera>(includeInactive: true);
				foreach (Camera camera in componentsInChildren)
				{
					cameras.Add(camera.gameObject);
				}
			}
			if (cameras.Count == 0)
			{
				Debug.LogWarning("No cameras could be found by CameraChanger. Either add cameras manually or add them as children to the game object this script is attached to.");
			}
		}

		private void Update()
		{
			if (_vehicle.enabled && !_vehicle.MultiplayerIsRemote && InputProvider.Instances.Count > 0 && InputProvider.CombinedInput((SceneInputProviderBase i) => i.ChangeCamera()))
			{
				NextCamera();
				CheckIfInside();
			}
		}

		private void OnMultiplayerInstanceTypeChanged(bool isRemote)
		{
			if (isRemote)
			{
				DisableAllCameras();
			}
		}

		private void EnableCurrentDisableOthers()
		{
			if (_vehicle.MultiplayerIsRemote)
			{
				return;
			}
			int count = cameras.Count;
			for (int i = 0; i < count; i++)
			{
				if (cameras[i] == null)
				{
					continue;
				}
				if (i == currentCameraIndex)
				{
					cameras[i].SetActive(value: true);
					AudioListener component = cameras[i].GetComponent<AudioListener>();
					if (component != null)
					{
						component.enabled = true;
					}
				}
				else
				{
					cameras[i].SetActive(value: false);
					AudioListener component2 = cameras[i].GetComponent<AudioListener>();
					if (component2 != null)
					{
						component2.enabled = false;
					}
				}
			}
		}

		private void DisableAllCameras()
		{
			int count = cameras.Count;
			for (int i = 0; i < count; i++)
			{
				cameras[i].SetActive(value: false);
				AudioListener component = cameras[i].GetComponent<AudioListener>();
				if (component != null)
				{
					component.enabled = true;
				}
			}
		}

		public void NextCamera()
		{
			if (cameras.Count > 0)
			{
				currentCameraIndex++;
				if (currentCameraIndex >= cameras.Count)
				{
					currentCameraIndex = 0;
				}
				EnableCurrentDisableOthers();
			}
		}

		public void PreviousCamera()
		{
			if (cameras.Count > 0)
			{
				currentCameraIndex--;
				if (currentCameraIndex < 0)
				{
					currentCameraIndex = cameras.Count - 1;
				}
				EnableCurrentDisableOthers();
			}
		}

		private void CheckIfInside()
		{
			if (cameras.Count != 0 && !(cameras[currentCameraIndex] == null))
			{
				CameraInsideVehicle cameraInsideVehicle = cameras[currentCameraIndex]?.GetComponent<CameraInsideVehicle>();
				if (cameraInsideVehicle != null)
				{
					_vehicle.CameraInsideVehicle = cameraInsideVehicle.isInsideVehicle;
				}
				else
				{
					_vehicle.CameraInsideVehicle = false;
				}
			}
		}
	}
}
