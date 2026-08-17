using System.Collections.Generic;
using EvilCore;
using EvilCore.Audio;
using EvilCore.Extensions;
using NWH.VehiclePhysics2;
using NWH.VehiclePhysics2.Modules.Fuel;
using NomadDrive.Features.Driving;
using NomadDrive.Features.LiquidTransferSystem;
using NomadDrive.Features.Vehicle.Interactables;
using NomadDrive.Features.Vehicle.Modules;
using NomadDrive.Features.Vehicle.Modules.Slots;
using NomadDrive.Features.Vehicle.Networking;
using NomadDrive.Features.Vehicle.WheelSystem;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Vehicle
{
	public class VehicleManager : MonoBehaviour, IInitialize, IVehicleManager
	{
		[SerializeField]
		public VehicleNetworkSync NetworkSync;

		[SerializeField]
		public VehicleController VehicleController;

		[SerializeField]
		public FuelModuleWrapper FuelModuleWrapper;

		[SerializeField]
		public WheelsManager WheelsManager;

		[SerializeField]
		public VehiclePhysiscsManager VehiclePhysiscsManager;

		[SerializeField]
		public GasolineCap GasolineCap;

		[SerializeField]
		public TopDownVehicleCameraController TopDownVehicleCamera;

		[Inject]
		private IAudioManager _audioManager;

		private VehicleInputProvider _vehicleInputProvider;

		private Rigidbody _vehicleRigidbody;

		private readonly List<VehicleModule> _modules = new List<VehicleModule>();

		private float _prevFuelAmount;

		private bool _syncingFuel;

		private const float FuelSyncEpsilon = 0.01f;

		private VehicleSlot[] _attachedPartSlots;

		public VehicleEventBus EventBus { get; private set; }

		public int TotalDistanceKm
		{
			get
			{
				if (!(NetworkSync != null))
				{
					return 0;
				}
				return NetworkSync.OdometerKm;
			}
		}

		public bool IsInitialized { get; set; }

		public void RegisterModule(VehicleModule module)
		{
			if (!_modules.Contains(module))
			{
				_modules.Add(module);
			}
		}

		public void UnregisterModule(VehicleModule module)
		{
			_modules.Remove(module);
		}

		public T GetModule<T>() where T : VehicleModule
		{
			foreach (VehicleModule module in _modules)
			{
				if (module is T result)
				{
					return result;
				}
			}
			return null;
		}

		private void Awake()
		{
			base.gameObject.InjectGameObject();
			_vehicleInputProvider = GetComponentInChildren<VehicleInputProvider>();
			EventBus = new VehicleEventBus();
			_vehicleRigidbody = GetComponent<Rigidbody>();
			VehiclePlayerContactFilter.RegisterVehicle(_vehicleRigidbody);
		}

		private void OnDestroy()
		{
			VehiclePlayerContactFilter.UnregisterVehicle(_vehicleRigidbody);
		}

		private void Start()
		{
			Init();
		}

		public void Init()
		{
			InitGasTank();
			IsInitialized = true;
		}

		private void InitGasTank()
		{
			GasolineCap.SetVehicleManager(this);
			FuelModuleWrapper.module.onAmountChanged.AddListener(OnFuelModuleAmountChanged);
			GasolineCap.RoutedLiquidContainer.OnLiquidAmountChangedEvent.AddListener(OnGasTankAmountChanged);
			FuelModuleWrapper.module.amount = GasolineCap.RoutedLiquidContainer.CurrentAmount;
			_prevFuelAmount = FuelModuleWrapper.module.amount;
		}

		private void OnFuelModuleAmountChanged()
		{
			if (_syncingFuel)
			{
				return;
			}
			float amount = FuelModuleWrapper.module.amount;
			if (float.IsNaN(amount) || float.IsInfinity(amount))
			{
				_syncingFuel = true;
				float currentAmount = GasolineCap.RoutedLiquidContainer.CurrentAmount;
				FuelModuleWrapper.module.amount = currentAmount;
				_prevFuelAmount = currentAmount;
				_syncingFuel = false;
			}
			else
			{
				float num = _prevFuelAmount - amount;
				if (!(num <= 0.01f))
				{
					_prevFuelAmount = amount;
					GasolineCap.RoutedLiquidContainer.Drain(num);
				}
			}
		}

		private void OnGasTankAmountChanged(float oldAmount, float newAmount)
		{
			_syncingFuel = true;
			FuelModuleWrapper.module.amount = newAmount;
			_prevFuelAmount = FuelModuleWrapper.module.amount;
			_syncingFuel = false;
		}

		public void DeactivateInteractablesForFrontSeatsTaken()
		{
			foreach (VehicleModule module in _modules)
			{
				module.OnFrontSeatsTaken();
			}
			WheelsManager.GetTire(WheelLocation.FrontLeft)?.IgnoreHovering();
			WheelsManager.GetTire(WheelLocation.FrontRight)?.IgnoreHovering();
		}

		public void ActivateInteractablesForFrontSeatsTaken()
		{
			foreach (VehicleModule module in _modules)
			{
				module.OnFrontSeatsVacated();
			}
			WheelsManager.GetTire(WheelLocation.FrontLeft)?.UnignoreHovering();
			WheelsManager.GetTire(WheelLocation.FrontRight)?.UnignoreHovering();
		}

		public void SetPhysics(bool isPhysics)
		{
			GetComponent<Rigidbody>().isKinematic = !isPhysics;
		}

		public void RefreshAttachedPartsInteraction()
		{
			VehicleSlot[] componentsInChildren = GetComponentsInChildren<VehicleSlot>(includeInactive: true);
			foreach (VehicleSlot vehicleSlot in componentsInChildren)
			{
				if (vehicleSlot != null)
				{
					vehicleSlot.RefreshAttachedPartInteraction();
				}
			}
			VehicleInteractable[] componentsInChildren2 = GetComponentsInChildren<VehicleInteractable>(includeInactive: true);
			foreach (VehicleInteractable vehicleInteractable in componentsInChildren2)
			{
				if (vehicleInteractable != null)
				{
					vehicleInteractable.RefreshInteractionState();
				}
			}
		}

		public void DriveAttachedParts(Transform vehicleRoot)
		{
			if (_attachedPartSlots == null)
			{
				_attachedPartSlots = GetComponentsInChildren<VehicleSlot>(includeInactive: true);
			}
			for (int i = 0; i < _attachedPartSlots.Length; i++)
			{
				VehicleSlot vehicleSlot = _attachedPartSlots[i];
				if (vehicleSlot != null && vehicleSlot.IsOccupied)
				{
					vehicleSlot.DriveAttachedPartPose(vehicleRoot);
				}
			}
		}

		public void InjectDependenciesFromChildren()
		{
			NetworkSync = GetComponentInChildren<VehicleNetworkSync>();
			VehicleController = GetComponentInChildren<VehicleController>();
			FuelModuleWrapper = GetComponentInChildren<FuelModuleWrapper>();
			WheelsManager = GetComponentInChildren<WheelsManager>();
			GasolineCap = GetComponentInChildren<GasolineCap>();
			VehiclePhysiscsManager = GetComponentInChildren<VehiclePhysiscsManager>();
			TopDownVehicleCamera = GetComponentInChildren<TopDownVehicleCameraController>();
		}
	}
}
