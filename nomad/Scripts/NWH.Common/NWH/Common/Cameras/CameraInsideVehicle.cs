using NWH.Common.Vehicles;
using UnityEngine;

namespace NWH.Common.Cameras
{
	public class CameraInsideVehicle : MonoBehaviour
	{
		[Tooltip("    Is the camera inside vehicle?")]
		public bool isInsideVehicle = true;

		private Vehicle _vehicle;

		private void Awake()
		{
			_vehicle = GetComponentInParent<Vehicle>();
		}

		private void Update()
		{
			_vehicle.CameraInsideVehicle = isInsideVehicle;
		}
	}
}
