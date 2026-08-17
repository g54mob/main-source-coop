using UnityEngine;

namespace NomadDrive.Features.Vehicle
{
	[CreateAssetMenu(menuName = "NomadDrive/Vehicle/Top Down Camera", fileName = "TopDownCameraConfig")]
	public class TopDownVehicleCameraConfig : ScriptableObject
	{
		[Header("Orbit")]
		[Tooltip("Mouse orbit sensitivity multiplier")]
		public float orbitSpeed = 2f;

		[Tooltip("Default orbit angle when camera activates (0 = behind vehicle)")]
		[Range(-180f, 180f)]
		public float defaultOrbitAngle;

		[Tooltip("Default orbit distance from target")]
		public float distance = 10f;

		[Tooltip("Minimum zoom distance")]
		public float minDistance = 7.5f;

		[Tooltip("Maximum zoom distance")]
		public float maxDistance = 20f;

		[Tooltip("Whether to clamp orbit angle within min/max range")]
		public bool limitOrbitAngle = true;

		[Tooltip("Minimum orbit angle (left limit relative to vehicle)")]
		[Range(-180f, 0f)]
		public float minOrbitAngle = -90f;

		[Tooltip("Maximum orbit angle (right limit relative to vehicle)")]
		[Range(0f, 180f)]
		public float maxOrbitAngle = 90f;

		[Header("Height")]
		[Tooltip("Default camera height above target")]
		public float height = 8f;

		[Tooltip("Minimum camera height")]
		public float minHeight = 1f;

		[Tooltip("Maximum camera height")]
		public float maxHeight = 15f;

		[Tooltip("Mouse height adjustment sensitivity")]
		public float heightSpeed = 2f;

		[Header("Input")]
		public bool useMouseForOrbit = true;

		[Tooltip("Mouse X sensitivity for orbit rotation")]
		public float mouseOrbitSensitivity = 2f;

		public bool useMouseForHeight = true;

		[Tooltip("Mouse Y sensitivity for height adjustment")]
		public float mouseHeightSensitivity = 0.3f;

		[Header("Behavior")]
		[Tooltip("Whether the camera should always look at the target")]
		public bool lookAtTarget = true;

		[Header("Smoothing")]
		[Tooltip("SmoothDamp time for camera position follow (lower = snappier)")]
		[Range(0.01f, 0.5f)]
		public float followSmoothTime = 0.05f;

		[Tooltip("Exponential decay speed for rotation smoothing (higher = snappier)")]
		[Range(1f, 50f)]
		public float rotationSmoothSpeed = 15f;

		[Tooltip("Mouse input smoothing factor (0 = no smoothing, 1 = max smoothing)")]
		[Range(0f, 0.95f)]
		public float mouseSmoothFactor = 0.3f;

		[Tooltip("Scroll zoom smoothing speed (higher = snappier)")]
		[Range(1f, 30f)]
		public float scrollSmoothSpeed = 10f;
	}
}
