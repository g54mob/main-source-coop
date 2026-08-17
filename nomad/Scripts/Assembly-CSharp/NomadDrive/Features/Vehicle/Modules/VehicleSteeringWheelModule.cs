using NomadDrive.Features.Vehicle.Enums;
using NomadDrive.Features.Vehicle.Networking;
using NomadDrive.Features.Vehicle.Parts.SteeringWheel;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Modules
{
	[DefaultExecutionOrder(-20)]
	public class VehicleSteeringWheelModule : VehicleModule
	{
		private const string STEERING_WHEEL = "Steering Wheel";

		[SerializeField]
		public SteeringWheelSlot SteeringWheelSlotRef;

		[SerializeField]
		public SteeringWheel InstalledSteeringWheel;

		[Tooltip("Remote viewers smooth the steering wheel rotation toward the driver-published angle (15Hz). Higher = snappier, lower = smoother but laggier.")]
		[SerializeField]
		private float remoteSteeringLerpSpeed = 12f;

		private Transform _pivotTransform;

		private Quaternion _initialRotation;

		private Vector3 _axisVector;

		private NetworkedNWHVehicle _networkedVehicle;

		private Quaternion _smoothedRemoteRotation;

		private bool _hasSmoothedRotation;

		private void Start()
		{
			InstalledSteeringWheel = null;
			base.VehicleManager.VehicleController.steering.InitializeSteeringWheel(null);
			base.VehicleManager.VehicleController.steering.maximumSteerAngle = 0f;
			_networkedVehicle = base.VehicleManager.GetComponentInChildren<NetworkedNWHVehicle>();
		}

		private void LateUpdate()
		{
			if (_pivotTransform == null)
			{
				return;
			}
			int num;
			float num2;
			if (_networkedVehicle != null)
			{
				num = ((!_networkedVehicle.IsControlling) ? 1 : 0);
				if (num != 0)
				{
					num2 = _networkedVehicle.RemoteSteeringAngleDeg;
					goto IL_0053;
				}
			}
			else
			{
				num = 0;
			}
			num2 = base.VehicleManager.VehicleController.steering.angle;
			goto IL_0053;
			IL_0053:
			float angle = num2 * base.VehicleManager.VehicleController.steering.steeringWheelTurnRatio;
			Quaternion quaternion = _initialRotation * Quaternion.AngleAxis(angle, _axisVector);
			if (num != 0)
			{
				if (!_hasSmoothedRotation)
				{
					_smoothedRemoteRotation = quaternion;
					_hasSmoothedRotation = true;
				}
				else
				{
					_smoothedRemoteRotation = Quaternion.Slerp(_smoothedRemoteRotation, quaternion, Time.deltaTime * remoteSteeringLerpSpeed);
				}
				_pivotTransform.localRotation = _smoothedRemoteRotation;
			}
			else
			{
				_pivotTransform.localRotation = quaternion;
				_hasSmoothedRotation = false;
			}
		}

		protected override void SubscribeEvents()
		{
			SteeringWheelSlotRef.OnSteeringWheelAttached.AddListener(OnSteeringWheelInstalled);
			SteeringWheelSlotRef.OnSteeringWheelDetached.AddListener(OnSteeringWheelRemoved);
		}

		protected override void UnsubscribeEvents()
		{
			SteeringWheelSlotRef.OnSteeringWheelAttached.RemoveListener(OnSteeringWheelInstalled);
			SteeringWheelSlotRef.OnSteeringWheelDetached.RemoveListener(OnSteeringWheelRemoved);
		}

		private void OnSteeringWheelInstalled(SteeringWheel steeringWheel)
		{
			InstalledSteeringWheel = steeringWheel;
			_pivotTransform = steeringWheel.steeringWheelPivotTransform;
			_initialRotation = _pivotTransform.localRotation;
			_axisVector = GetAxisVector(steeringWheel.RotationAxis);
			_hasSmoothedRotation = false;
			base.VehicleManager.VehicleController.steering.InitializeSteeringWheel(null);
			base.VehicleManager.VehicleController.steering.maximumSteerAngle = steeringWheel.steeringWheelAngle;
		}

		private void OnSteeringWheelRemoved(SteeringWheel steeringWheel)
		{
			if (_pivotTransform != null)
			{
				_pivotTransform.localRotation = _initialRotation;
			}
			_pivotTransform = null;
			InstalledSteeringWheel = null;
			_hasSmoothedRotation = false;
			base.VehicleManager.VehicleController.steering.InitializeSteeringWheel(null);
			base.VehicleManager.VehicleController.steering.maximumSteerAngle = 0f;
		}

		private static Vector3 GetAxisVector(ControlRotationAxis axis)
		{
			return axis switch
			{
				ControlRotationAxis.X => Vector3.right, 
				ControlRotationAxis.Y => Vector3.up, 
				ControlRotationAxis.Z => Vector3.forward, 
				_ => Vector3.forward, 
			};
		}
	}
}
