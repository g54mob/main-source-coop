using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using Mirror;
using NWH.VehiclePhysics2.Modules.Fuel;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.LiquidTransferSystem;
using NomadDrive.Features.SaveSystem;
using NomadDrive.Features.Vehicle.Modules.Slots;
using NomadDrive.Features.Vehicle.Parts.Battery;
using NomadDrive.Features.Vehicle.Parts.Brakelight;
using NomadDrive.Features.Vehicle.Parts.Doors;
using NomadDrive.Features.Vehicle.Parts.Engine;
using NomadDrive.Features.Vehicle.Parts.Generator;
using NomadDrive.Features.Vehicle.Parts.Handbrake;
using NomadDrive.Features.Vehicle.Parts.Headlights;
using NomadDrive.Features.Vehicle.Parts.Hood;
using NomadDrive.Features.Vehicle.Parts.Seats;
using NomadDrive.Features.Vehicle.Parts.SteeringWheel;
using NomadDrive.Features.Vehicle.Parts.Sunvisor;
using NomadDrive.Features.Vehicle.WheelSystem;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NomadDrive.Features.Vehicle.Testing
{
	public class VehiclePartSpawner : NetworkBehaviour
	{
		[SerializeField]
		private VehiclePartConfig _config;

		[SerializeField]
		private EngineSlot _engineSlot;

		[SerializeField]
		private BatterySlot _batterySlot;

		[SerializeField]
		private HoodSlot _hoodSlot;

		[SerializeField]
		private GeneratorSlot _generatorSlot;

		[SerializeField]
		private VehicleDoorSlot _leftDoorSlot;

		[SerializeField]
		private VehicleDoorSlot _rightDoorSlot;

		[SerializeField]
		private VehicleSeatSlot _frontLeftSeatSlot;

		[SerializeField]
		private VehicleSeatSlot _frontRightSeatSlot;

		[SerializeField]
		private HeadlightSlot _leftHeadlightSlot;

		[SerializeField]
		private HeadlightSlot _rightHeadlightSlot;

		[SerializeField]
		private BrakelightSlot _leftBrakelightSlot;

		[SerializeField]
		private BrakelightSlot _rightBrakelightSlot;

		[SerializeField]
		private SteeringWheelSlot _steeringWheelSlot;

		[SerializeField]
		private HandbrakeSlot _handbrakeSlot;

		[SerializeField]
		private SunvisorSlot _leftSunvisorSlot;

		[SerializeField]
		private SunvisorSlot _rightSunvisorSlot;

		[SerializeField]
		private WheelsManager _wheelsManager;

		[SerializeField]
		private FuelModuleWrapper _fuelModuleWrapper;

		[SerializeField]
		private GasolineCap _gasolineCap;

		[SerializeField]
		private List<GameObject> _spawnedParts = new List<GameObject>();

		[SerializeField]
		private bool _isInjecting;

		private bool IsServerActive => NetworkServer.active;

		private void InjectDependenciesFromChildren()
		{
			_engineSlot = GetComponentInChildren<EngineSlot>();
			_batterySlot = GetComponentInChildren<BatterySlot>();
			_hoodSlot = GetComponentInChildren<HoodSlot>();
			_generatorSlot = GetComponentInChildren<GeneratorSlot>();
			_steeringWheelSlot = GetComponentInChildren<SteeringWheelSlot>();
			_handbrakeSlot = GetComponentInChildren<HandbrakeSlot>();
			_wheelsManager = GetComponentInChildren<WheelsManager>();
			_fuelModuleWrapper = GetComponentInChildren<FuelModuleWrapper>();
			_gasolineCap = GetComponentInChildren<GasolineCap>();
			VehicleDoorSlot[] componentsInChildren = GetComponentsInChildren<VehicleDoorSlot>();
			if (componentsInChildren.Length >= 2)
			{
				_leftDoorSlot = componentsInChildren[0];
				_rightDoorSlot = componentsInChildren[1];
			}
			VehicleSeatSlot[] componentsInChildren2 = GetComponentsInChildren<VehicleSeatSlot>();
			if (componentsInChildren2.Length >= 2)
			{
				_frontLeftSeatSlot = componentsInChildren2[0];
				_frontRightSeatSlot = componentsInChildren2[1];
			}
			HeadlightSlot[] componentsInChildren3 = GetComponentsInChildren<HeadlightSlot>();
			if (componentsInChildren3.Length >= 2)
			{
				_leftHeadlightSlot = componentsInChildren3[0];
				_rightHeadlightSlot = componentsInChildren3[1];
			}
			BrakelightSlot[] componentsInChildren4 = GetComponentsInChildren<BrakelightSlot>();
			if (componentsInChildren4.Length >= 2)
			{
				_leftBrakelightSlot = componentsInChildren4[0];
				_rightBrakelightSlot = componentsInChildren4[1];
			}
			SunvisorSlot[] componentsInChildren5 = GetComponentsInChildren<SunvisorSlot>();
			if (componentsInChildren5.Length >= 2)
			{
				_leftSunvisorSlot = componentsInChildren5[0];
				_rightSunvisorSlot = componentsInChildren5[1];
			}
		}

		private void InjectParts()
		{
			if (!(_config == null))
			{
				InjectPartsAsync(_config).Forget();
			}
		}

		private void DetachAllParts()
		{
			DetachAllPartsAsync().Forget();
		}

		private async UniTaskVoid InjectPartsAsync(VehiclePartConfig config)
		{
			if (!base.isServer || _isInjecting)
			{
				return;
			}
			_isInjecting = true;
			try
			{
				await InjectPartIfEnabled(config.engine, _engineSlot);
				await InjectPartIfEnabled(config.battery, _batterySlot);
				await InjectPartIfEnabled(config.hood, _hoodSlot);
				if (_wheelsManager != null)
				{
					await InjectPartIfEnabled(config.frontLeftTire, _wheelsManager.GetWheelSlot(WheelLocation.FrontLeft));
					await InjectPartIfEnabled(config.frontRightTire, _wheelsManager.GetWheelSlot(WheelLocation.FrontRight));
					await InjectPartIfEnabled(config.rearLeftTire, _wheelsManager.GetWheelSlot(WheelLocation.RearLeft));
					await InjectPartIfEnabled(config.rearRightTire, _wheelsManager.GetWheelSlot(WheelLocation.RearRight));
				}
				await InjectPartIfEnabled(config.leftDoor, _leftDoorSlot);
				await InjectPartIfEnabled(config.rightDoor, _rightDoorSlot);
				await InjectPartIfEnabled(config.frontLeftSeat, _frontLeftSeatSlot);
				await InjectPartIfEnabled(config.frontRightSeat, _frontRightSeatSlot);
				await InjectPartIfEnabled(config.leftHeadlight, _leftHeadlightSlot);
				await InjectPartIfEnabled(config.rightHeadlight, _rightHeadlightSlot);
				await InjectPartIfEnabled(config.leftBrakelight, _leftBrakelightSlot);
				await InjectPartIfEnabled(config.rightBrakelight, _rightBrakelightSlot);
				await InjectPartIfEnabled(config.steeringWheel, _steeringWheelSlot);
				await InjectPartIfEnabled(config.handbrake, _handbrakeSlot);
				await InjectPartIfEnabled(config.leftSunvisor, _leftSunvisorSlot);
				await InjectPartIfEnabled(config.rightSunvisor, _rightSunvisorSlot);
				await InjectPartIfEnabled(config.generator, _generatorSlot);
				if (_gasolineCap != null && _gasolineCap.RoutedLiquidContainer != null)
				{
					float amount = _gasolineCap.RoutedLiquidContainer.Capacity * (config.fuelPercentage / 100f);
					_gasolineCap.RoutedLiquidContainer.CmdSetAmount(amount);
				}
				else if (_fuelModuleWrapper != null && _fuelModuleWrapper.module != null)
				{
					float capacity = _fuelModuleWrapper.module.capacity;
					_fuelModuleWrapper.module.amount = capacity * (config.fuelPercentage / 100f);
				}
				GetComponentInParent<VehicleManager>()?.EventBus.FireDashboardRefreshRequested();
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("[VehiclePartSpawner] Error during part injection: " + ex.Message, "InjectPartsAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Driving\\_Core\\Testing\\VehiclePartSpawner.cs", 288);
			}
			finally
			{
				_isInjecting = false;
			}
		}

		private async UniTask InjectPartIfEnabled(SlotPartConfig partConfig, VehicleSlot slot)
		{
			if (partConfig == null || !partConfig.enabled || slot == null || slot.IsOccupied || partConfig.prefab == null || !partConfig.prefab.RuntimeKeyIsValid())
			{
				return;
			}
			try
			{
				GameObject original;
				if (partConfig.prefab.Asset != null)
				{
					original = partConfig.prefab.Asset as GameObject;
				}
				else
				{
					AsyncOperationHandle<GameObject> handle = partConfig.prefab.LoadAssetAsync<GameObject>();
					await handle.ToUniTask();
					if (handle.Status != AsyncOperationStatus.Succeeded)
					{
						EvilLogger.LogError("[VehiclePartSpawner] Failed to load prefab for slot " + slot.name, "InjectPartIfEnabled", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Driving\\_Core\\Testing\\VehiclePartSpawner.cs", 334);
						return;
					}
					original = handle.Result;
				}
				Vector3 position = slot.transform.position;
				Quaternion rotation = slot.transform.rotation;
				GameObject instance = UnityEngine.Object.Instantiate(original, position, rotation);
				NetworkServer.Spawn(instance);
				_spawnedParts.Add(instance);
				PersistentObject.ServerEnsure(instance, (partConfig.prefab != null) ? partConfig.prefab.AssetGUID : null);
				await UniTask.Delay(100);
				if (!instance.TryGetComponent<AttachableObject>(out var attachable))
				{
					EvilLogger.LogError("[VehiclePartSpawner] Spawned object has no AttachableObject component", "InjectPartIfEnabled", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Driving\\_Core\\Testing\\VehiclePartSpawner.cs", 361);
					return;
				}
				int attempts = 0;
				while (!attachable.IsLateJoinCompleted && attempts < 50)
				{
					await UniTask.Delay(50);
					attempts++;
				}
				_ = attachable.IsLateJoinCompleted;
				if (partConfig.condition < 100f && attachable.TryGetComponent<ConditionComponent>(out var component))
				{
					component.ServerSetCondition(partConfig.condition);
				}
				slot.ServerAttach(attachable.netId);
				int attachWaitAttempts = 0;
				while (!slot.IsOccupied && attachWaitAttempts < 50)
				{
					await UniTask.Delay(100);
					attachWaitAttempts++;
				}
				_ = slot.IsOccupied;
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("[VehiclePartSpawner] Error injecting part into " + slot.name + ": " + ex.Message, "InjectPartIfEnabled", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Driving\\_Core\\Testing\\VehiclePartSpawner.cs", 404);
			}
		}

		private async UniTaskVoid DetachAllPartsAsync()
		{
			if (!base.isServer || _isInjecting)
			{
				return;
			}
			_isInjecting = true;
			try
			{
				DetachSlot(_engineSlot);
				DetachSlot(_batterySlot);
				DetachSlot(_hoodSlot);
				DetachSlot(_generatorSlot);
				DetachSlot(_leftDoorSlot);
				DetachSlot(_rightDoorSlot);
				DetachSlot(_frontLeftSeatSlot);
				DetachSlot(_frontRightSeatSlot);
				DetachSlot(_leftHeadlightSlot);
				DetachSlot(_rightHeadlightSlot);
				DetachSlot(_leftBrakelightSlot);
				DetachSlot(_rightBrakelightSlot);
				DetachSlot(_steeringWheelSlot);
				DetachSlot(_handbrakeSlot);
				DetachSlot(_leftSunvisorSlot);
				DetachSlot(_rightSunvisorSlot);
				if (_wheelsManager != null)
				{
					DetachSlot(_wheelsManager.GetWheelSlot(WheelLocation.FrontLeft));
					DetachSlot(_wheelsManager.GetWheelSlot(WheelLocation.FrontRight));
					DetachSlot(_wheelsManager.GetWheelSlot(WheelLocation.RearLeft));
					DetachSlot(_wheelsManager.GetWheelSlot(WheelLocation.RearRight));
				}
				await UniTask.Delay(100);
				foreach (GameObject spawnedPart in _spawnedParts)
				{
					if (spawnedPart != null)
					{
						NetworkServer.Destroy(spawnedPart);
					}
				}
				_spawnedParts.Clear();
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("[VehiclePartSpawner] Error detaching parts: " + ex.Message, "DetachAllPartsAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Driving\\_Core\\Testing\\VehiclePartSpawner.cs", 470);
			}
			finally
			{
				_isInjecting = false;
			}
		}

		private void DetachSlot(VehicleSlot slot)
		{
			if (!(slot == null) && slot.IsOccupied)
			{
				slot.ServerDetach();
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
