using System;
using NomadDrive.Features.Vehicle.Collision;
using NomadDrive.Features.Vehicle.Enums;

namespace NomadDrive.Features.Vehicle
{
	public class VehicleEventBus
	{
		public event Action<bool> OnBatteryUsefulChanged;

		public event Action OnBatteryDepleted;

		public event Action OnBatteryInstalled;

		public event Action OnBatteryRemoved;

		public event Action<bool> OnEngineUsefulChanged;

		public event Action OnEngineStarted;

		public event Action OnEngineStopped;

		public event Action OnEngineOverheated;

		public event Action OnEngineCooledDown;

		public event Action OnUnderHoodPartsActivate;

		public event Action OnUnderHoodPartsDeactivate;

		public event Action<HeadLightState> OnHeadlightStateChanged;

		public event Action<bool> OnHandbrakeStateChanged;

		public event Action OnBrakePressed;

		public event Action OnBrakeReleased;

		public event Action OnDashboardRefreshRequested;

		public event Action OnFrontSeatsTaken;

		public event Action OnFrontSeatsVacated;

		public event Action<VehicleCollisionData> OnCollisionServer;

		public event Action<VehicleCollisionData> OnCollisionAllClients;

		public void FireBatteryUsefulChanged(bool isBatteryUseful)
		{
			this.OnBatteryUsefulChanged?.Invoke(isBatteryUseful);
		}

		public void FireBatteryDepleted()
		{
			this.OnBatteryDepleted?.Invoke();
		}

		public void FireBatteryInstalled()
		{
			this.OnBatteryInstalled?.Invoke();
		}

		public void FireBatteryRemoved()
		{
			this.OnBatteryRemoved?.Invoke();
		}

		public void FireEngineUsefulChanged(bool isEngineUseful)
		{
			this.OnEngineUsefulChanged?.Invoke(isEngineUseful);
		}

		public void FireEngineStarted()
		{
			this.OnEngineStarted?.Invoke();
		}

		public void FireEngineStopped()
		{
			this.OnEngineStopped?.Invoke();
		}

		public void FireEngineOverheated()
		{
			this.OnEngineOverheated?.Invoke();
		}

		public void FireEngineCooledDown()
		{
			this.OnEngineCooledDown?.Invoke();
		}

		public void FireUnderHoodPartsActivate()
		{
			this.OnUnderHoodPartsActivate?.Invoke();
		}

		public void FireUnderHoodPartsDeactivate()
		{
			this.OnUnderHoodPartsDeactivate?.Invoke();
		}

		public void FireHeadlightStateChanged(HeadLightState state)
		{
			this.OnHeadlightStateChanged?.Invoke(state);
		}

		public void FireHandbrakeStateChanged(bool isOn)
		{
			this.OnHandbrakeStateChanged?.Invoke(isOn);
		}

		public void FireBrakePressed()
		{
			this.OnBrakePressed?.Invoke();
		}

		public void FireBrakeReleased()
		{
			this.OnBrakeReleased?.Invoke();
		}

		public void FireDashboardRefreshRequested()
		{
			this.OnDashboardRefreshRequested?.Invoke();
		}

		public void FireFrontSeatsTaken()
		{
			this.OnFrontSeatsTaken?.Invoke();
		}

		public void FireFrontSeatsVacated()
		{
			this.OnFrontSeatsVacated?.Invoke();
		}

		public void FireCollisionServer(VehicleCollisionData data)
		{
			this.OnCollisionServer?.Invoke(data);
		}

		public void FireCollisionAllClients(VehicleCollisionData data)
		{
			this.OnCollisionAllClients?.Invoke(data);
		}
	}
}
