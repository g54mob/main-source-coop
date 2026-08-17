using System.Collections.Generic;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.LiquidTransferSystem;
using NomadDrive.Features.Vehicle.Networking;
using NomadDrive.Features.Vehicle.Parts.Battery;
using NomadDrive.Features.Vehicle.Parts.Engine;
using NomadDrive.Features.Vehicle.Parts.Radiator;
using NomadDrive.Features.Vehicle.WheelSystem;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace NomadDrive.Features.Vehicle.Modules
{
	public class VehiclePartFailureWarningModule : VehicleModule
	{
		[Inject]
		private UIFeedbackManager _uiFeedbackManager;

		private NetworkedNWHVehicle _networkedVehicle;

		private EngineSlot _engineSlot;

		private BatterySlot _batterySlot;

		private RadiatorSlot _radiatorSlot;

		private ConditionComponent _engineCondition;

		private ConditionComponent _batteryCondition;

		private ConditionComponent _radiatorCondition;

		private LiquidContainerComponent _radiatorCoolant;

		private ILiquidContainer _fuelTank;

		private UnityAction<float, float> _engineConditionListener;

		private UnityAction<float, float> _batteryConditionListener;

		private UnityAction<float, float> _radiatorConditionListener;

		private UnityAction<float, float> _radiatorCoolantListener;

		private UnityAction<float, float> _fuelListener;

		private UnityAction<Engine> _engineInstalledListener;

		private UnityAction _engineRemovedListener;

		private UnityAction<Battery> _batteryInstalledListener;

		private UnityAction _batteryRemovedListener;

		private UnityAction<Radiator> _radiatorInstalledListener;

		private UnityAction _radiatorRemovedListener;

		private readonly Dictionary<WheelLocation, WheelSlot> _wheelSlots = new Dictionary<WheelLocation, WheelSlot>();

		private readonly Dictionary<WheelLocation, UnityAction> _wheelInstalledListeners = new Dictionary<WheelLocation, UnityAction>();

		private readonly Dictionary<WheelLocation, UnityAction> _wheelRemovedListeners = new Dictionary<WheelLocation, UnityAction>();

		private readonly Dictionary<WheelLocation, ConditionComponent> _wheelConditions = new Dictionary<WheelLocation, ConditionComponent>();

		private readonly Dictionary<WheelLocation, UnityAction<float, float>> _wheelConditionListeners = new Dictionary<WheelLocation, UnityAction<float, float>>();

		private bool _sourcesWired;

		private bool IsLocalDriver
		{
			get
			{
				if (_networkedVehicle != null && _networkedVehicle.HasDriver)
				{
					return _networkedVehicle.isOwned;
				}
				return false;
			}
		}

		protected override void SubscribeEvents()
		{
			base.EventBus.OnEngineOverheated += OnEngineOverheated;
		}

		protected override void UnsubscribeEvents()
		{
			base.EventBus.OnEngineOverheated -= OnEngineOverheated;
			UnwireSources();
		}

		private void Start()
		{
			WireSources();
		}

		private void WireSources()
		{
			if (!_sourcesWired)
			{
				_sourcesWired = true;
				_networkedVehicle = base.VehicleManager.GetComponentInChildren<NetworkedNWHVehicle>();
				_engineConditionListener = OnEngineConditionChanged;
				_batteryConditionListener = OnBatteryConditionChanged;
				_radiatorConditionListener = OnRadiatorConditionChanged;
				_radiatorCoolantListener = OnRadiatorCoolantChanged;
				_fuelListener = OnFuelAmountChanged;
				WireEngine();
				WireBattery();
				WireRadiator();
				WireWheels();
				WireFuel();
			}
		}

		private void WireEngine()
		{
			VehicleEngineModule module = base.VehicleManager.GetModule<VehicleEngineModule>();
			if (!(module == null))
			{
				_engineSlot = module.EngineSlotRef;
				if (_engineSlot != null)
				{
					_engineInstalledListener = OnEngineInstalled;
					_engineRemovedListener = OnEngineRemoved;
					_engineSlot.onEngineInstalled.AddListener(_engineInstalledListener);
					_engineSlot.onEngineRemoved.AddListener(_engineRemovedListener);
				}
				if (module.InstalledEngine != null)
				{
					AttachEngine(module.InstalledEngine);
				}
			}
		}

		private void WireBattery()
		{
			VehicleBatteryModule module = base.VehicleManager.GetModule<VehicleBatteryModule>();
			if (!(module == null))
			{
				_batterySlot = module.BatterySlotRef;
				if (_batterySlot != null)
				{
					_batteryInstalledListener = OnBatteryInstalled;
					_batteryRemovedListener = OnBatteryRemoved;
					_batterySlot.OnBatteryInstalled.AddListener(_batteryInstalledListener);
					_batterySlot.OnBatteryRemoved.AddListener(_batteryRemovedListener);
				}
				if (module.InstalledBattery != null)
				{
					AttachBattery(module.InstalledBattery);
				}
			}
		}

		private void WireRadiator()
		{
			VehicleRadiatorModule module = base.VehicleManager.GetModule<VehicleRadiatorModule>();
			if (!(module == null))
			{
				_radiatorSlot = module.RadiatorSlotRef;
				if (_radiatorSlot != null)
				{
					_radiatorInstalledListener = OnRadiatorInstalled;
					_radiatorRemovedListener = OnRadiatorRemoved;
					_radiatorSlot.OnRadiatorInstalled.AddListener(_radiatorInstalledListener);
					_radiatorSlot.OnRadiatorRemoved.AddListener(_radiatorRemovedListener);
				}
				if (module.InstalledRadiator != null)
				{
					AttachRadiator(module.InstalledRadiator);
				}
			}
		}

		private void WireWheels()
		{
			WheelsManager wheelsManager = base.VehicleManager.WheelsManager;
			if (wheelsManager == null)
			{
				return;
			}
			foreach (WheelLocation activeWheelLocation in wheelsManager.ActiveWheelLocations)
			{
				WheelSlot slot = wheelsManager.GetWheelSlot(activeWheelLocation);
				if (!(slot == null))
				{
					WheelLocation captured = activeWheelLocation;
					UnityAction unityAction = delegate
					{
						OnTireInstalled(captured, slot);
					};
					UnityAction unityAction2 = delegate
					{
						OnTireRemoved(captured);
					};
					slot.OnTireInstalled.AddListener(unityAction);
					slot.OnTireRemoved.AddListener(unityAction2);
					_wheelSlots[captured] = slot;
					_wheelInstalledListeners[captured] = unityAction;
					_wheelRemovedListeners[captured] = unityAction2;
					if (slot.InstalledTire != null)
					{
						AttachWheel(captured, slot.InstalledTire);
					}
				}
			}
		}

		private void WireFuel()
		{
			_fuelTank = ((base.VehicleManager.GasolineCap != null) ? base.VehicleManager.GasolineCap.RoutedLiquidContainer : null);
			_fuelTank?.OnLiquidAmountChangedEvent.AddListener(_fuelListener);
		}

		private void UnwireSources()
		{
			if (!_sourcesWired)
			{
				return;
			}
			_sourcesWired = false;
			if (_engineSlot != null)
			{
				if (_engineInstalledListener != null)
				{
					_engineSlot.onEngineInstalled.RemoveListener(_engineInstalledListener);
				}
				if (_engineRemovedListener != null)
				{
					_engineSlot.onEngineRemoved.RemoveListener(_engineRemovedListener);
				}
			}
			if (_batterySlot != null)
			{
				if (_batteryInstalledListener != null)
				{
					_batterySlot.OnBatteryInstalled.RemoveListener(_batteryInstalledListener);
				}
				if (_batteryRemovedListener != null)
				{
					_batterySlot.OnBatteryRemoved.RemoveListener(_batteryRemovedListener);
				}
			}
			if (_radiatorSlot != null)
			{
				if (_radiatorInstalledListener != null)
				{
					_radiatorSlot.OnRadiatorInstalled.RemoveListener(_radiatorInstalledListener);
				}
				if (_radiatorRemovedListener != null)
				{
					_radiatorSlot.OnRadiatorRemoved.RemoveListener(_radiatorRemovedListener);
				}
			}
			foreach (KeyValuePair<WheelLocation, WheelSlot> wheelSlot in _wheelSlots)
			{
				WheelSlot value = wheelSlot.Value;
				if (!(value == null))
				{
					if (_wheelInstalledListeners.TryGetValue(wheelSlot.Key, out var value2))
					{
						value.OnTireInstalled.RemoveListener(value2);
					}
					if (_wheelRemovedListeners.TryGetValue(wheelSlot.Key, out var value3))
					{
						value.OnTireRemoved.RemoveListener(value3);
					}
				}
			}
			_wheelSlots.Clear();
			_wheelInstalledListeners.Clear();
			_wheelRemovedListeners.Clear();
			_fuelTank?.OnLiquidAmountChangedEvent.RemoveListener(_fuelListener);
			_fuelTank = null;
			DetachEngine();
			DetachBattery();
			DetachRadiator();
			DetachAllWheels();
		}

		private void OnEngineInstalled(Engine engine)
		{
			AttachEngine(engine);
		}

		private void OnEngineRemoved()
		{
			DetachEngine();
		}

		private void OnBatteryInstalled(Battery battery)
		{
			AttachBattery(battery);
		}

		private void OnBatteryRemoved()
		{
			DetachBattery();
		}

		private void OnRadiatorInstalled(Radiator radiator)
		{
			AttachRadiator(radiator);
		}

		private void OnRadiatorRemoved()
		{
			DetachRadiator();
		}

		private void OnTireInstalled(WheelLocation location, WheelSlot slot)
		{
			AttachWheel(location, (slot != null) ? slot.InstalledTire : null);
		}

		private void OnTireRemoved(WheelLocation location)
		{
			DetachWheel(location);
		}

		private void AttachEngine(Engine engine)
		{
			DetachEngine();
			_engineCondition = ((engine != null) ? engine.ConditionComponent : null);
			_engineCondition?.OnConditionChanged.AddListener(_engineConditionListener);
		}

		private void DetachEngine()
		{
			_engineCondition?.OnConditionChanged.RemoveListener(_engineConditionListener);
			_engineCondition = null;
		}

		private void AttachBattery(Battery battery)
		{
			DetachBattery();
			_batteryCondition = ((battery != null) ? battery.ConditionComponent : null);
			_batteryCondition?.OnConditionChanged.AddListener(_batteryConditionListener);
		}

		private void DetachBattery()
		{
			_batteryCondition?.OnConditionChanged.RemoveListener(_batteryConditionListener);
			_batteryCondition = null;
		}

		private void AttachRadiator(Radiator radiator)
		{
			DetachRadiator();
			if (!(radiator == null))
			{
				_radiatorCondition = radiator.ConditionComponent;
				_radiatorCondition?.OnConditionChanged.AddListener(_radiatorConditionListener);
				_radiatorCoolant = radiator.LiquidContainer;
				_radiatorCoolant?.OnLiquidAmountChangedEvent.AddListener(_radiatorCoolantListener);
			}
		}

		private void DetachRadiator()
		{
			_radiatorCondition?.OnConditionChanged.RemoveListener(_radiatorConditionListener);
			_radiatorCondition = null;
			_radiatorCoolant?.OnLiquidAmountChangedEvent.RemoveListener(_radiatorCoolantListener);
			_radiatorCoolant = null;
		}

		private void AttachWheel(WheelLocation location, Tire tire)
		{
			DetachWheel(location);
			ConditionComponent conditionComponent = ((tire != null) ? tire.ConditionComponent : null);
			if (!(conditionComponent == null))
			{
				UnityAction<float, float> unityAction = delegate(float oldValue, float newValue)
				{
					OnConditionCrossedZero(oldValue, newValue, WheelKey(location));
				};
				conditionComponent.OnConditionChanged.AddListener(unityAction);
				_wheelConditions[location] = conditionComponent;
				_wheelConditionListeners[location] = unityAction;
			}
		}

		private void DetachWheel(WheelLocation location)
		{
			if (_wheelConditions.TryGetValue(location, out var value) && _wheelConditionListeners.TryGetValue(location, out var value2) && value != null)
			{
				value.OnConditionChanged.RemoveListener(value2);
			}
			_wheelConditions.Remove(location);
			_wheelConditionListeners.Remove(location);
		}

		private void DetachAllWheels()
		{
			foreach (WheelLocation item in new List<WheelLocation>(_wheelConditions.Keys))
			{
				DetachWheel(item);
			}
		}

		private void OnEngineConditionChanged(float oldValue, float newValue)
		{
			OnConditionCrossedZero(oldValue, newValue, "@vehicle.engine_broken");
		}

		private void OnBatteryConditionChanged(float oldValue, float newValue)
		{
			OnConditionCrossedZero(oldValue, newValue, "@vehicle.battery_broken");
		}

		private void OnRadiatorConditionChanged(float oldValue, float newValue)
		{
			OnConditionCrossedZero(oldValue, newValue, "@vehicle.radiator_broken");
		}

		private void OnRadiatorCoolantChanged(float oldValue, float newValue)
		{
			OnConditionCrossedZero(oldValue, newValue, "@vehicle.radiator_coolant_empty");
		}

		private void OnFuelAmountChanged(float oldValue, float newValue)
		{
			OnConditionCrossedZero(oldValue, newValue, "@vehicle.fuel_empty");
		}

		private void OnEngineOverheated()
		{
			Warn("@vehicle.engine_overheated");
		}

		private void OnConditionCrossedZero(float oldValue, float newValue, string localizationKey)
		{
			if (oldValue > 0f && newValue <= 0f)
			{
				Warn(localizationKey);
			}
		}

		private void Warn(string localizationKey)
		{
			if (IsLocalDriver)
			{
				if (_uiFeedbackManager == null)
				{
					_uiFeedbackManager = Object.FindFirstObjectByType<UIFeedbackManager>();
				}
				_uiFeedbackManager?.CreateFloatingMessage(localizationKey, FeedbackType.Error);
			}
		}

		private static string WheelKey(WheelLocation location)
		{
			return location switch
			{
				WheelLocation.FrontLeft => "@vehicle.wheel_broken_front_left", 
				WheelLocation.FrontRight => "@vehicle.wheel_broken_front_right", 
				WheelLocation.RearLeft => "@vehicle.wheel_broken_rear_left", 
				WheelLocation.RearRight => "@vehicle.wheel_broken_rear_right", 
				_ => "@vehicle.wheel_broken_front_left", 
			};
		}
	}
}
