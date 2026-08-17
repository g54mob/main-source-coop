using System.Collections.Generic;
using NomadDrive.Features.LiquidTransferSystem;
using NomadDrive.Features.Objectives;
using NomadDrive.Features.Vehicle.Enums;
using NomadDrive.Features.Vehicle.Interactables;
using NomadDrive.Features.Vehicle.Networking;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Modules
{
	public class VehicleIgnitionModule : VehicleModule
	{
		private const string CONTROLS = "Controls";

		[SerializeField]
		private IgnitionButton _ignitionButton;

		private readonly List<IgnitionResult> _hardBlockers = new List<IgnitionResult>();

		private readonly List<IgnitionResult> _warnings = new List<IgnitionResult>();

		private NetworkedNWHVehicle _networkedVehicle;

		public IgnitionButton IgnitionButton => _ignitionButton;

		public bool IsIgnited => _ignitionButton.IgnitionState.Equals(IgnitionState.Ignited);

		protected override void SubscribeEvents()
		{
			_ignitionButton.OnStartEngineRequested.AddListener(OnStartEngineRequested);
			_ignitionButton.OnStopEngineRequested.AddListener(OnStopEngineRequested);
			_ignitionButton.onHovered.AddListener(OnIgnitionHovered);
			base.VehicleManager.VehicleController.powertrain.engine.onStart.AddListener(OnEngineStartedActions);
			base.VehicleManager.VehicleController.powertrain.engine.onStop.AddListener(OnEngineStoppedActions);
			base.VehicleManager.VehicleController.powertrain.engine.onStart.AddListener(BroadcastIgniteIfAuthority);
			base.VehicleManager.VehicleController.powertrain.engine.onStop.AddListener(BroadcastExtinguishIfAuthority);
			base.EventBus.OnBatteryDepleted += OnBatteryDepleted;
		}

		protected override void UnsubscribeEvents()
		{
			_ignitionButton.OnStartEngineRequested.RemoveListener(OnStartEngineRequested);
			_ignitionButton.OnStopEngineRequested.RemoveListener(OnStopEngineRequested);
			_ignitionButton.onHovered.RemoveListener(OnIgnitionHovered);
			base.VehicleManager.VehicleController.powertrain.engine.onStart.RemoveListener(OnEngineStartedActions);
			base.VehicleManager.VehicleController.powertrain.engine.onStop.RemoveListener(OnEngineStoppedActions);
			base.VehicleManager.VehicleController.powertrain.engine.onStart.RemoveListener(BroadcastIgniteIfAuthority);
			base.VehicleManager.VehicleController.powertrain.engine.onStop.RemoveListener(BroadcastExtinguishIfAuthority);
			base.EventBus.OnBatteryDepleted -= OnBatteryDepleted;
		}

		private void OnStartEngineRequested()
		{
			base.VehicleManager.GetModule<VehicleEngineModule>()?.StartEngine();
		}

		private void OnStopEngineRequested()
		{
			base.VehicleManager.GetModule<VehicleEngineModule>()?.StopEngine();
		}

		private void OnIgnitionHovered()
		{
			EvaluateIgnition(_hardBlockers, _warnings);
			_ignitionButton.SetIgnitionState(_hardBlockers, _warnings);
		}

		private void OnEngineStartedActions()
		{
			base.EventBus.FireEngineStarted();
			ObjectivesEventBus.Raise(ObjectiveSignal.EngineStarted, base.VehicleManager);
		}

		private void OnEngineStoppedActions()
		{
			base.EventBus.FireEngineStopped();
		}

		private void OnBatteryDepleted()
		{
			if (IsIgnited)
			{
				base.VehicleManager.GetModule<VehicleEngineModule>()?.StopEngine();
			}
		}

		private bool HasEngineStateAuthority()
		{
			if (base.VehicleManager.NetworkSync != null && base.VehicleManager.NetworkSync.isServer)
			{
				return true;
			}
			if (_networkedVehicle == null)
			{
				_networkedVehicle = base.VehicleManager.GetComponentInChildren<NetworkedNWHVehicle>();
			}
			if (_networkedVehicle != null)
			{
				return _networkedVehicle.IsControlling;
			}
			return false;
		}

		private void BroadcastIgniteIfAuthority()
		{
			if (HasEngineStateAuthority())
			{
				_ignitionButton.Ignite();
			}
		}

		private void BroadcastExtinguishIfAuthority()
		{
			if (HasEngineStateAuthority())
			{
				_ignitionButton.Extinguish();
			}
		}

		private void EvaluateIgnition(List<IgnitionResult> hardBlockers, List<IgnitionResult> warnings)
		{
			hardBlockers.Clear();
			warnings.Clear();
			VehicleEngineModule module = base.VehicleManager.GetModule<VehicleEngineModule>();
			if (module == null || !module.IsEngineInstalled)
			{
				hardBlockers.Add(IgnitionResult.EngineNotInstalled);
			}
			else
			{
				if (module.IsEngineBroken)
				{
					hardBlockers.Add(IgnitionResult.EngineBroken);
				}
				if (module.IsOverheated)
				{
					hardBlockers.Add(IgnitionResult.EngineOverheated);
				}
			}
			VehicleBatteryModule module2 = base.VehicleManager.GetModule<VehicleBatteryModule>();
			if (module2 == null || !module2.IsBatteryInstalled)
			{
				hardBlockers.Add(IgnitionResult.BatteryNotInstalled);
			}
			else if (module2.IsBatteryBroken)
			{
				hardBlockers.Add(IgnitionResult.BatteryBroken);
			}
			if (IsFuelEmpty())
			{
				hardBlockers.Add(IgnitionResult.FuelEmpty);
			}
			VehicleHandbrakeModule module3 = base.VehicleManager.GetModule<VehicleHandbrakeModule>();
			if (module3 != null && module3.InstalledHandbrake != null && !module3.IsHandbrakeOn)
			{
				warnings.Add(IgnitionResult.HandbrakeEngaged);
			}
		}

		private bool IsFuelEmpty()
		{
			ILiquidContainer liquidContainer = base.VehicleManager.GasolineCap?.RoutedLiquidContainer;
			if (liquidContainer == null)
			{
				return false;
			}
			return liquidContainer.CurrentAmount <= 0.001f;
		}

		public void ToggleIgnitionFromInput()
		{
			if (IsIgnited)
			{
				_ignitionButton.RequestStopFromInput();
				return;
			}
			EvaluateIgnition(_hardBlockers, _warnings);
			if (_hardBlockers.Count > 0)
			{
				_ignitionButton.ShowStartBlockedFromInput(_hardBlockers);
			}
			else
			{
				_ignitionButton.RequestStartFromInput(_warnings);
			}
		}
	}
}
