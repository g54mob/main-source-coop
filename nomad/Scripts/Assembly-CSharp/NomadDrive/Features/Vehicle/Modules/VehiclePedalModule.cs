using NomadDrive.Features.Vehicle.Networking;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Modules
{
	[DefaultExecutionOrder(-20)]
	public class VehiclePedalModule : VehicleModule
	{
		private const string PEDALS = "Pedals";

		[Header("Gas Pedal")]
		[SerializeField]
		private Transform gasPedalTransform;

		[Tooltip("Local-space Euler delta applied on top of the pedal's authored rest rotation when fully pressed.")]
		[SerializeField]
		private Vector3 gasPedalPressedRotation = new Vector3(25f, 0f, 0f);

		[Header("Brake Pedal")]
		[SerializeField]
		private Transform brakePedalTransform;

		[Tooltip("Local-space Euler delta applied on top of the pedal's authored rest rotation when fully pressed.")]
		[SerializeField]
		private Vector3 brakePedalPressedRotation = new Vector3(25f, 0f, 0f);

		[Header("Foot IK Targets")]
		[Tooltip("Static rest transform for the left foot. Left foot stays idle here while driving.")]
		[SerializeField]
		private Transform leftFootIdleTarget;

		[Tooltip("Right foot pose when resting on / pressing the gas pedal.")]
		[SerializeField]
		private Transform rightFootGasTarget;

		[Tooltip("Right foot pose when resting on / pressing the brake pedal.")]
		[SerializeField]
		private Transform rightFootBrakeTarget;

		[Tooltip("Brake input above this triggers the right foot to slide onto the brake target.")]
		[SerializeField]
		[Range(0f, 0.5f)]
		private float brakeFootSwitchThreshold = 0.05f;

		[Tooltip("Right foot transition responsiveness between gas and brake (higher = snappier).")]
		[SerializeField]
		private float rightFootSwitchSpeed = 12f;

		[Header("Settings")]
		[SerializeField]
		private float lerpSpeed = 10f;

		private Quaternion _gasPedalRestLocalRotation = Quaternion.identity;

		private Quaternion _brakePedalRestLocalRotation = Quaternion.identity;

		private Transform _rightFootDynamicTarget;

		private NetworkedNWHVehicle _networkedVehicle;

		public Transform GasPedalTransform => gasPedalTransform;

		public Transform BrakePedalTransform => brakePedalTransform;

		public Transform LeftFootTarget => leftFootIdleTarget;

		public Transform RightFootTarget => _rightFootDynamicTarget;

		protected override void Awake()
		{
			base.Awake();
			if (gasPedalTransform != null)
			{
				_gasPedalRestLocalRotation = gasPedalTransform.localRotation;
			}
			if (brakePedalTransform != null)
			{
				_brakePedalRestLocalRotation = brakePedalTransform.localRotation;
			}
			if (rightFootGasTarget != null || rightFootBrakeTarget != null)
			{
				GameObject gameObject = new GameObject("RightFootDynamicTarget");
				_rightFootDynamicTarget = gameObject.transform;
				_rightFootDynamicTarget.SetParent(base.transform, worldPositionStays: false);
				Transform transform = ((rightFootGasTarget != null) ? rightFootGasTarget : rightFootBrakeTarget);
				_rightFootDynamicTarget.position = transform.position;
				_rightFootDynamicTarget.rotation = transform.rotation;
			}
		}

		protected override void SubscribeEvents()
		{
		}

		protected override void UnsubscribeEvents()
		{
		}

		private void Start()
		{
			_networkedVehicle = base.VehicleManager.GetComponentInChildren<NetworkedNWHVehicle>();
		}

		private void LateUpdate()
		{
			int num;
			float num2;
			if (_networkedVehicle != null)
			{
				num = ((!_networkedVehicle.IsControlling) ? 1 : 0);
				if (num != 0)
				{
					num2 = _networkedVehicle.RemoteThrottle;
					goto IL_0044;
				}
			}
			else
			{
				num = 0;
			}
			num2 = base.VehicleManager.VehicleController.input.Throttle;
			goto IL_0044;
			IL_0044:
			float num3 = num2;
			float num4 = ((num != 0) ? _networkedVehicle.RemoteBrakes : base.VehicleManager.VehicleController.input.Brakes);
			if (gasPedalTransform != null)
			{
				Quaternion b = _gasPedalRestLocalRotation * Quaternion.Euler(gasPedalPressedRotation);
				Quaternion b2 = Quaternion.Slerp(_gasPedalRestLocalRotation, b, num3);
				gasPedalTransform.localRotation = Quaternion.Slerp(gasPedalTransform.localRotation, b2, Time.deltaTime * lerpSpeed);
			}
			if (brakePedalTransform != null)
			{
				Quaternion b3 = _brakePedalRestLocalRotation * Quaternion.Euler(brakePedalPressedRotation);
				Quaternion b4 = Quaternion.Slerp(_brakePedalRestLocalRotation, b3, num4);
				brakePedalTransform.localRotation = Quaternion.Slerp(brakePedalTransform.localRotation, b4, Time.deltaTime * lerpSpeed);
			}
			UpdateRightFootDynamicTarget(num3, num4);
		}

		private void UpdateRightFootDynamicTarget(float throttle, float brakes)
		{
			if (!(_rightFootDynamicTarget == null))
			{
				Transform transform = ((brakes > brakeFootSwitchThreshold) ? rightFootBrakeTarget : rightFootGasTarget);
				if (!(transform == null))
				{
					float t = 1f - Mathf.Exp((0f - rightFootSwitchSpeed) * Time.deltaTime);
					_rightFootDynamicTarget.position = Vector3.Lerp(_rightFootDynamicTarget.position, transform.position, t);
					_rightFootDynamicTarget.rotation = Quaternion.Slerp(_rightFootDynamicTarget.rotation, transform.rotation, t);
				}
			}
		}
	}
}
