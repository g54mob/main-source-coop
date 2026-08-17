using System;
using NWH.VehiclePhysics2.Powertrain;

namespace NWH.VehiclePhysics2.Modules.Metrics
{
	[Serializable]
	public class MetricsModule : VehicleComponent
	{
		[Serializable]
		public class Metric
		{
			public delegate float UpdateDelegate();

			public float value;

			public void Update(UpdateDelegate del, bool increment)
			{
				if (increment)
				{
					value += del();
				}
				else
				{
					value = del();
				}
			}

			public void Reset()
			{
				value = 0f;
			}
		}

		public Metric averageSpeed = new Metric();

		public Metric continousDriftDistance = new Metric();

		public Metric continousDriftTime = new Metric();

		public Metric odometer = new Metric();

		public Metric topSpeed = new Metric();

		public Metric totalDriftDistance = new Metric();

		public Metric totalDriftTime = new Metric();

		private float _driftEndTime;

		private float _driftTimeout = 0.75f;

		public override void VC_Update()
		{
			base.VC_Update();
			odometer.Update(() => vehicleController.Speed * vehicleController.deltaTime, increment: true);
			topSpeed.Update(() => (vehicleController.Speed > topSpeed.value) ? vehicleController.Speed : topSpeed.value, increment: false);
			averageSpeed.Update(() => odometer.value / vehicleController.realtimeSinceStartup, increment: false);
			bool hasWheelSkid = false;
			foreach (WheelComponent wheel in vehicleController.powertrain.wheels)
			{
				if (wheel.wheelUAPI.IsSkiddingLaterally)
				{
					hasWheelSkid = true;
					break;
				}
			}
			totalDriftTime.Update(() => hasWheelSkid ? vehicleController.fixedDeltaTime : 0f, increment: true);
			continousDriftTime.Update(delegate
			{
				if (hasWheelSkid)
				{
					_driftEndTime = vehicleController.realtimeSinceStartup;
					return vehicleController.fixedDeltaTime;
				}
				return (vehicleController.realtimeSinceStartup < _driftEndTime + _driftTimeout) ? vehicleController.fixedDeltaTime : (0f - continousDriftTime.value);
			}, increment: true);
			totalDriftDistance.Update(() => hasWheelSkid ? (vehicleController.fixedDeltaTime * vehicleController.Speed) : 0f, increment: true);
			continousDriftDistance.Update(delegate
			{
				if (hasWheelSkid)
				{
					_driftEndTime = vehicleController.realtimeSinceStartup;
					return vehicleController.fixedDeltaTime * vehicleController.Speed;
				}
				return (vehicleController.realtimeSinceStartup < _driftEndTime + _driftTimeout) ? (vehicleController.fixedDeltaTime * vehicleController.Speed) : (0f - continousDriftDistance.value);
			}, increment: true);
		}
	}
}
