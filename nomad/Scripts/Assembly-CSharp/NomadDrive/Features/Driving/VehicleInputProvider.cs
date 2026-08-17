using Mirror;
using NWH.VehiclePhysics2.Input;
using NomadDrive.Features.Inputs;
using NomadDrive.Features.Vehicle.Networking;

namespace NomadDrive.Features.Driving
{
	public class VehicleInputProvider : VehicleInputProviderBase
	{
		private float _throttle;

		private float _brakes;

		private float _steering;

		private float _handbrake;

		private bool _horn;

		private NetworkedNWHVehicle _networkedVehicle;

		private void Start()
		{
			_networkedVehicle = GetComponentInParent<NetworkedNWHVehicle>();
		}

		private void Update()
		{
			_throttle = DrivingInputs.GetThrottle();
			_brakes = DrivingInputs.GetBrakes();
			_steering = DrivingInputs.GetSteering();
			_handbrake = DrivingInputs.GetHandbrake();
			_horn = DrivingInputs.IsHornButton();
		}

		private bool IsLocalDriverActive()
		{
			if (_networkedVehicle == null)
			{
				return true;
			}
			if (!_networkedVehicle.IsControlling)
			{
				return false;
			}
			if (NetworkServer.active && !_networkedVehicle.HasDriver)
			{
				return false;
			}
			return true;
		}

		public override float Throttle()
		{
			if (!IsLocalDriverActive())
			{
				return 0f;
			}
			return _throttle;
		}

		public override float Brakes()
		{
			if (!IsLocalDriverActive())
			{
				return 0f;
			}
			return _brakes;
		}

		public override float Steering()
		{
			if (!IsLocalDriverActive())
			{
				return 0f;
			}
			return _steering;
		}

		public override float Handbrake()
		{
			if (!IsLocalDriverActive())
			{
				return 0f;
			}
			return _handbrake;
		}

		public override bool Horn()
		{
			if (IsLocalDriverActive())
			{
				return _horn;
			}
			return false;
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
			_throttle = 0f;
			_brakes = 0f;
			_steering = 0f;
			_handbrake = 0f;
			_horn = false;
		}
	}
}
