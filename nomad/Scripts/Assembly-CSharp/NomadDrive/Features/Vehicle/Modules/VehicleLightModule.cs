using System.Collections.Generic;
using NomadDrive.Features.Vehicle.Enums;
using NomadDrive.Features.Vehicle.Interactables;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Modules
{
	public class VehicleLightModule : VehicleModule
	{
		private readonly List<DriverCabinLight> _cabinLights = new List<DriverCabinLight>();

		private readonly Dictionary<DriverCabinLight, UnityAction> _onListeners = new Dictionary<DriverCabinLight, UnityAction>();

		protected override void Awake()
		{
			base.Awake();
			_cabinLights.Clear();
			if (base.VehicleManager != null)
			{
				_cabinLights.AddRange(base.VehicleManager.GetComponentsInChildren<DriverCabinLight>(includeInactive: true));
			}
		}

		private void Start()
		{
			foreach (DriverCabinLight cabinLight in _cabinLights)
			{
				cabinLight.Init();
			}
			VehicleBatteryModule module = base.VehicleManager.GetModule<VehicleBatteryModule>();
			if (module != null)
			{
				module.RegisterConsumer(IsAnyCabinLightActive, GetCabinLightMultiplier);
			}
		}

		protected override void SubscribeEvents()
		{
			foreach (DriverCabinLight light in _cabinLights)
			{
				if (!(light == null))
				{
					UnityAction unityAction = delegate
					{
						OnCabinLightOn(light);
					};
					_onListeners[light] = unityAction;
					light.OnLightButtonOn.AddListener(unityAction);
				}
			}
			base.EventBus.OnBatteryDepleted += OnBatteryDepleted;
			base.EventBus.OnBatteryInstalled += OnBatteryInstalled;
		}

		protected override void UnsubscribeEvents()
		{
			foreach (KeyValuePair<DriverCabinLight, UnityAction> onListener in _onListeners)
			{
				if (onListener.Key != null)
				{
					onListener.Key.OnLightButtonOn.RemoveListener(onListener.Value);
				}
			}
			_onListeners.Clear();
			base.EventBus.OnBatteryDepleted -= OnBatteryDepleted;
			base.EventBus.OnBatteryInstalled -= OnBatteryInstalled;
			VehicleBatteryModule module = base.VehicleManager.GetModule<VehicleBatteryModule>();
			if (module != null)
			{
				module.UnregisterConsumer(IsAnyCabinLightActive);
			}
		}

		private bool IsBatteryUsable()
		{
			VehicleBatteryModule module = base.VehicleManager.GetModule<VehicleBatteryModule>();
			if (module != null && module.IsBatteryInstalled)
			{
				return !module.IsBatteryBroken;
			}
			return false;
		}

		private async void OnCabinLightOn(DriverCabinLight light)
		{
			if (light == null)
			{
				return;
			}
			if (!IsBatteryUsable())
			{
				light.Init();
				return;
			}
			VehicleBatteryModule module = base.VehicleManager.GetModule<VehicleBatteryModule>();
			if (module != null)
			{
				await module.ConsumeBatteryCondition();
			}
		}

		private void OnBatteryDepleted()
		{
			foreach (DriverCabinLight cabinLight in _cabinLights)
			{
				if (cabinLight != null)
				{
					cabinLight.Init();
				}
			}
		}

		private void OnBatteryInstalled()
		{
			foreach (DriverCabinLight cabinLight in _cabinLights)
			{
				if (cabinLight != null)
				{
					cabinLight.UpdateState();
				}
			}
		}

		private bool IsAnyCabinLightActive()
		{
			foreach (DriverCabinLight cabinLight in _cabinLights)
			{
				if (cabinLight != null && cabinLight.driverCabinLightState.Equals(DriverCabinLightState.On))
				{
					return true;
				}
			}
			return false;
		}

		private float GetCabinLightMultiplier()
		{
			return (base.VehicleManager.GetModule<VehicleBatteryModule>()?.InstalledBattery?.batteryConfig?.indoorLightUsageMultiplier).GetValueOrDefault();
		}
	}
}
