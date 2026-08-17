using System;
using System.Linq;
using NWH.Common.Vehicles;
using NWH.VehiclePhysics2;
using NWH.VehiclePhysics2.Powertrain;
using NomadDrive.Features.LiquidTransferSystem;
using NomadDrive.Features.Vehicle.Enums;
using NomadDrive.Features.Vehicle.Networking;
using NomadDrive.Features.Vehicle.UI;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Modules
{
	public class VehicleDashboardModule : VehicleModule
	{
		private const string DASHBOARD = "Dashboard";

		[SerializeField]
		public VehicleDashboard RvDashboard;

		private bool _isIgnited;

		private VehicleEngineModule _engineModule;

		private VehicleBatteryModule _batteryModule;

		private void Start()
		{
			_engineModule = base.VehicleManager.GetModule<VehicleEngineModule>();
			_batteryModule = base.VehicleManager.GetModule<VehicleBatteryModule>();
			RefreshDashboardLights();
			VehicleIgnitionModule module = base.VehicleManager.GetModule<VehicleIgnitionModule>();
			if (module != null && module.IsIgnited)
			{
				_isIgnited = true;
				SetGaugeMaxValues();
			}
		}

		protected override void SubscribeEvents()
		{
			base.EventBus.OnEngineUsefulChanged += OnEngineUsefulChanged;
			base.EventBus.OnBatteryUsefulChanged += OnBatteryUsefulChanged;
			base.EventBus.OnBatteryDepleted += OnBatteryDepleted;
			base.EventBus.OnHandbrakeStateChanged += OnHandbrakeStateChanged;
			base.EventBus.OnHeadlightStateChanged += OnHeadlightStateChanged;
			base.EventBus.OnDashboardRefreshRequested += RefreshDashboardLights;
			base.EventBus.OnEngineOverheated += OnEngineOverheated;
			base.EventBus.OnEngineCooledDown += OnEngineCooledDown;
			base.EventBus.OnEngineStarted += OnEngineStarted;
			base.EventBus.OnEngineStopped += OnEngineStopped;
		}

		protected override void UnsubscribeEvents()
		{
			base.EventBus.OnEngineUsefulChanged -= OnEngineUsefulChanged;
			base.EventBus.OnBatteryUsefulChanged -= OnBatteryUsefulChanged;
			base.EventBus.OnBatteryDepleted -= OnBatteryDepleted;
			base.EventBus.OnHandbrakeStateChanged -= OnHandbrakeStateChanged;
			base.EventBus.OnHeadlightStateChanged -= OnHeadlightStateChanged;
			base.EventBus.OnDashboardRefreshRequested -= RefreshDashboardLights;
			base.EventBus.OnEngineOverheated -= OnEngineOverheated;
			base.EventBus.OnEngineCooledDown -= OnEngineCooledDown;
			base.EventBus.OnEngineStarted -= OnEngineStarted;
			base.EventBus.OnEngineStopped -= OnEngineStopped;
		}

		private void LateUpdate()
		{
			UpdateDashboardUIGauges();
		}

		private void UpdateDashboardUIGauges()
		{
			VehicleNetworkSync networkSync = base.VehicleManager.NetworkSync;
			VehicleController vehicleController = base.VehicleManager.VehicleController;
			float rpmValue;
			float speed;
			string gear;
			if (networkSync != null && networkSync.isOwned && vehicleController != null)
			{
				rpmValue = vehicleController.powertrain.engine.OutputRPM;
				speed = vehicleController.Speed * 3.6f;
				gear = vehicleController.powertrain.transmission.GearName;
			}
			else
			{
				rpmValue = networkSync?.EngineRpm ?? 0f;
				speed = networkSync?.VehicleSpeedKmh ?? 0f;
				gear = networkSync?.GearName ?? string.Empty;
			}
			RvDashboard.UpdateSpeedGauge(speed);
			RvDashboard.UpdateRpmGauge(rpmValue);
			RvDashboard.UpdateGearGauge(gear);
			UpdateFuelGauge();
			float temperature = ((!_isIgnited) ? 0f : (_engineModule?.CurrentHeatLevel ?? 0f));
			RvDashboard.UpdateTemperatureGauge(temperature);
			RvDashboard.UpdateOdometer(base.VehicleManager.TotalDistanceKm);
		}

		private void UpdateFuelGauge()
		{
			ILiquidContainer liquidContainer = base.VehicleManager.GasolineCap?.RoutedLiquidContainer;
			if (liquidContainer != null)
			{
				bool flag = _batteryModule?.IsBatteryUseful ?? false;
				RvDashboard.SetFuelGaugeMaxValue(liquidContainer.Capacity);
				RvDashboard.UpdateFuelGauge(flag ? liquidContainer.CurrentAmount : 0f);
			}
		}

		private void RefreshDashboardLights()
		{
			VehicleEngineModule module = base.VehicleManager.GetModule<VehicleEngineModule>();
			VehicleBatteryModule module2 = base.VehicleManager.GetModule<VehicleBatteryModule>();
			VehicleHandbrakeModule module3 = base.VehicleManager.GetModule<VehicleHandbrakeModule>();
			bool flag = module?.IsEngineUseful ?? false;
			bool flag2 = module2?.IsBatteryUseful ?? false;
			bool flag3 = module3?.IsHandbrakeOn ?? false;
			bool overheatWarningLight = module?.IsOverheated ?? false;
			RvDashboard.SetEngineErrorLight(!flag);
			RvDashboard.SetBatteryErrorLight(!flag2);
			RvDashboard.SetHandbrakeErrorLight(!flag3);
			RvDashboard.SetOverheatWarningLight(overheatWarningLight);
		}

		private void OnEngineUsefulChanged(bool isEngineUseful)
		{
			RvDashboard.SetEngineErrorLight(!isEngineUseful);
			if (isEngineUseful)
			{
				SetGaugeMaxValues();
			}
		}

		private void SetGaugeMaxValues()
		{
			VehicleController vehicleController = base.VehicleManager.VehicleController;
			EngineComponent engine = vehicleController.powertrain.engine;
			TransmissionComponent transmission = vehicleController.powertrain.transmission;
			RvDashboard.SetRpmGaugeMaxValue(engine.revLimiterRPM);
			VehicleEngineModule module = base.VehicleManager.GetModule<VehicleEngineModule>();
			if (module?.InstalledEngine?.engineConfig != null)
			{
				RvDashboard.SetTemperatureGaugeMaxValue(module.InstalledEngine.engineConfig.overheatThreshold);
			}
			WheelUAPI componentInChildren = vehicleController.GetComponentInChildren<WheelUAPI>();
			if (componentInChildren != null)
			{
				float num = transmission.gears.Where((float g) => g > 0f).DefaultIfEmpty(1f).Min();
				float num2 = engine.revLimiterRPM / (num * transmission.finalGearRatio);
				float speedGaugeMaxValue = componentInChildren.Radius * ((float)Math.PI / 30f) * num2 * 3.6f;
				RvDashboard.SetSpeedGaugeMaxValue(speedGaugeMaxValue);
			}
		}

		private void OnBatteryUsefulChanged(bool isBatteryUseful)
		{
			RvDashboard.SetBatteryErrorLight(!isBatteryUseful);
		}

		private void OnBatteryDepleted()
		{
			RvDashboard.SetBatteryErrorLight(state: true);
		}

		private void OnHandbrakeStateChanged(bool isHandbrakeOn)
		{
			RvDashboard.SetHandbrakeErrorLight(!isHandbrakeOn);
		}

		private void OnEngineStarted()
		{
			_isIgnited = true;
			SetGaugeMaxValues();
		}

		private void OnEngineStopped()
		{
			_isIgnited = false;
		}

		private void OnEngineOverheated()
		{
			RvDashboard.SetOverheatWarningLight(state: true);
		}

		private void OnEngineCooledDown()
		{
			RvDashboard.SetOverheatWarningLight(state: false);
		}

		private void OnHeadlightStateChanged(HeadLightState state)
		{
			switch (state)
			{
			case HeadLightState.Off:
				RvDashboard.DeactivateHeadlightInfoLight();
				break;
			case HeadLightState.Low:
				RvDashboard.ActivateLowBeamInfoLight();
				break;
			case HeadLightState.High:
				RvDashboard.ActivateHighBeamInfoLight();
				break;
			}
		}
	}
}
