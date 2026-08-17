using System;
using System.Collections.Generic;
using NWH.VehiclePhysics2;
using NWH.WheelController3D;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.Vehicle.Networking;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.WheelSystem
{
	public class WheelsManager : MonoBehaviour
	{
		[SerializeField]
		private List<WheelLocation> activeWheelLocations = new List<WheelLocation>();

		[SerializeField]
		private WheelSlot[] wheelSlots;

		[SerializeField]
		private WheelController[] wheelControllers;

		private readonly Dictionary<WheelLocation, UnityAction<float, float>> _tireConditionListeners = new Dictionary<WheelLocation, UnityAction<float, float>>();

		private readonly Dictionary<WheelLocation, int> _wheelLocationToIndex = new Dictionary<WheelLocation, int>();

		[SerializeField]
		private VehicleController _vehicleController;

		private readonly Dictionary<WheelLocation, float> _baseLngGrip = new Dictionary<WheelLocation, float>();

		private readonly Dictionary<WheelLocation, float> _baseLatGrip = new Dictionary<WheelLocation, float>();

		private readonly Dictionary<WheelLocation, TyreWear> _tyreWears = new Dictionary<WheelLocation, TyreWear>();

		private NetworkedNWHVehicle _networkedVehicle;

		private float _reportTimer;

		private const float ReportInterval = 0.25f;

		private const float ConditionReportEpsilon = 0.5f;

		private bool _wheelSimulationAsleep;

		private bool _wheelsReady;

		private int initializedWheelCount;

		public int WheelCount => activeWheelLocations.Count;

		public IReadOnlyList<WheelLocation> ActiveWheelLocations => activeWheelLocations;

		public bool IsFrontLeftWheelAttached
		{
			get
			{
				if (HasWheelLocation(WheelLocation.FrontLeft))
				{
					return GetWheelSlot(WheelLocation.FrontLeft)?.InstalledTire != null;
				}
				return false;
			}
		}

		public bool IsFrontRightWheelAttached
		{
			get
			{
				if (HasWheelLocation(WheelLocation.FrontRight))
				{
					return GetWheelSlot(WheelLocation.FrontRight)?.InstalledTire != null;
				}
				return false;
			}
		}

		public bool IsRearLeftWheelAttached
		{
			get
			{
				if (HasWheelLocation(WheelLocation.RearLeft))
				{
					return GetWheelSlot(WheelLocation.RearLeft)?.InstalledTire != null;
				}
				return false;
			}
		}

		public bool IsRearRightWheelAttached
		{
			get
			{
				if (HasWheelLocation(WheelLocation.RearRight))
				{
					return GetWheelSlot(WheelLocation.RearRight)?.InstalledTire != null;
				}
				return false;
			}
		}

		public bool HasWheelLocation(WheelLocation location)
		{
			return _wheelLocationToIndex.ContainsKey(location);
		}

		private void OnValidate()
		{
			_vehicleController = GetComponent<VehicleController>();
			wheelControllers = GetComponentsInChildren<WheelController>();
			wheelSlots = GetComponentsInChildren<WheelSlot>();
			if (wheelSlots == null || wheelSlots.Length == 0 || activeWheelLocations.Count != 0)
			{
				return;
			}
			activeWheelLocations.Clear();
			WheelSlot[] array = wheelSlots;
			foreach (WheelSlot wheelSlot in array)
			{
				if (!activeWheelLocations.Contains(wheelSlot.WheelSlotLocation))
				{
					activeWheelLocations.Add(wheelSlot.WheelSlotLocation);
				}
			}
		}

		private void Awake()
		{
			_networkedVehicle = GetComponentInParent<NetworkedNWHVehicle>();
			BuildLocationIndex();
			Init();
		}

		private void BuildLocationIndex()
		{
			_wheelLocationToIndex.Clear();
			if (wheelSlots == null)
			{
				return;
			}
			for (int i = 0; i < wheelSlots.Length; i++)
			{
				WheelLocation wheelSlotLocation = wheelSlots[i].WheelSlotLocation;
				if (!_wheelLocationToIndex.ContainsKey(wheelSlotLocation))
				{
					_wheelLocationToIndex[wheelSlotLocation] = i;
				}
			}
		}

		private void Init()
		{
			WheelSlot[] array = wheelSlots;
			foreach (WheelSlot slot in array)
			{
				if (!HasWheelLocation(slot.WheelSlotLocation))
				{
					continue;
				}
				WheelController wheelController = GetWheelController(slot);
				if (!(wheelController == null))
				{
					wheelController.OnInitialized.AddListener(IncreaseInitializedWheelCount);
					slot.OnTireInstalled.AddListener(delegate
					{
						ActivateWheelController(slot.WheelSlotLocation);
					});
					slot.OnTireRemoved.AddListener(delegate
					{
						DeactivateWheelController(slot.WheelSlotLocation);
					});
				}
			}
		}

		private void IncreaseInitializedWheelCount()
		{
			initializedWheelCount++;
			if (initializedWheelCount == WheelCount)
			{
				SetupWheels();
			}
		}

		private void SetupWheels()
		{
			CaptureBaseGrips();
			WheelSlot[] array = wheelSlots;
			foreach (WheelSlot wheelSlot in array)
			{
				if (HasWheelLocation(wheelSlot.WheelSlotLocation))
				{
					if (wheelSlot.InstalledTire == null)
					{
						DeactivateWheelController(wheelSlot.WheelSlotLocation);
					}
					else
					{
						ActivateWheelController(wheelSlot.WheelSlotLocation);
					}
				}
			}
			_wheelsReady = true;
			if (_wheelSimulationAsleep)
			{
				ApplyWheelSimulationState();
			}
		}

		public void ActivateWheelController(WheelLocation location)
		{
			if (HasWheelLocation(location))
			{
				ActivateWheel(location);
			}
		}

		public void DeactivateWheelController(WheelLocation location)
		{
			if (HasWheelLocation(location))
			{
				DeactivateWheel(location);
			}
		}

		public void ReinitializeWheel(WheelLocation location)
		{
			InitializeWheel(GetWheelController(location));
		}

		private void InitializeWheel(WheelController wheelController)
		{
			if (!(wheelController == null))
			{
				wheelController.Initialize();
				PruneDuplicateWheelColliders(wheelController);
			}
		}

		private void PruneDuplicateWheelColliders(WheelController wheelController)
		{
			MeshCollider meshCollider = wheelController.wheel?.meshCollider;
			if (meshCollider == null)
			{
				return;
			}
			MeshCollider[] componentsInChildren = wheelController.GetComponentsInChildren<MeshCollider>(includeInactive: true);
			foreach (MeshCollider meshCollider2 in componentsInChildren)
			{
				if (!(meshCollider2 == null) && !(meshCollider2 == meshCollider) && !(meshCollider2.gameObject.name != "Collider"))
				{
					UnityEngine.Object.Destroy(meshCollider2.gameObject);
				}
			}
		}

		public void ResyncAfterSettle()
		{
			if (!_wheelsReady)
			{
				return;
			}
			foreach (WheelLocation activeWheelLocation in activeWheelLocations)
			{
				WheelController wheelController = GetWheelController(activeWheelLocation);
				if (!(wheelController == null) && wheelController.enabled)
				{
					InitializeWheel(wheelController);
					if (wheelController.wheel != null)
					{
						wheelController.wheel.angularVelocity = 0f;
						wheelController.wheel.prevAngularVelocity = 0f;
					}
				}
			}
		}

		public void LogSuspensionDiag(string tag)
		{
			foreach (WheelLocation activeWheelLocation in activeWheelLocations)
			{
				WheelController wheelController = GetWheelController(activeWheelLocation);
				if (!(wheelController == null))
				{
					_ = wheelController.wheel;
				}
			}
		}

		private void ActivateWheel(WheelLocation wheelLocation)
		{
			WheelController wheelController = GetWheelController(wheelLocation);
			if (!(wheelController == null))
			{
				wheelController.enabled = !_wheelsReady || !_wheelSimulationAsleep;
				Collider componentInChildren = wheelController.GetComponentInChildren<Collider>();
				if (componentInChildren != null)
				{
					componentInChildren.enabled = true;
				}
				ConfigureTyreWear(wheelLocation);
				if (_wheelsReady)
				{
					InitializeWheel(wheelController);
				}
			}
		}

		private void DeactivateWheel(WheelLocation wheelLocation)
		{
			WheelController wheelController = GetWheelController(wheelLocation);
			if (wheelController == null)
			{
				return;
			}
			TyreWear component = wheelController.GetComponent<TyreWear>();
			if (component != null)
			{
				component.enabled = false;
			}
			wheelController.enabled = false;
			Collider[] componentsInChildren = wheelController.GetComponentsInChildren<Collider>();
			foreach (Collider collider in componentsInChildren)
			{
				if (collider != null)
				{
					collider.enabled = false;
				}
			}
			if (wheelController.wheel != null)
			{
				wheelController.wheel.angularVelocity = 0f;
				wheelController.wheel.prevAngularVelocity = 0f;
			}
			wheelController.forwardFriction.slip = 0f;
			wheelController.forwardFriction.speed = 0f;
			wheelController.sideFriction.slip = 0f;
			wheelController.sideFriction.speed = 0f;
			RestoreBaseGrip(wheelLocation);
			_tyreWears.Remove(wheelLocation);
			WheelSlot wheelSlot = GetWheelSlot(wheelLocation);
			if (wheelSlot == null || wheelSlot.InstalledTire == null)
			{
				return;
			}
			if (_tireConditionListeners.TryGetValue(wheelLocation, out var value))
			{
				wheelSlot.InstalledTire.ConditionComponent?.OnConditionChanged.RemoveListener(value);
				_tireConditionListeners.Remove(wheelLocation);
			}
			wheelSlot.InstalledTire = null;
			foreach (WheelLocation activeWheelLocation in activeWheelLocations)
			{
				_ = GetWheelController(activeWheelLocation) == null;
			}
		}

		public void SetWheelSimulation(bool active)
		{
			_wheelSimulationAsleep = !active;
			if (_wheelsReady)
			{
				ApplyWheelSimulationState();
			}
		}

		private void ApplyWheelSimulationState()
		{
			foreach (WheelLocation activeWheelLocation in activeWheelLocations)
			{
				WheelSlot wheelSlot = GetWheelSlot(activeWheelLocation);
				if (wheelSlot == null || wheelSlot.InstalledTire == null)
				{
					continue;
				}
				WheelController wheelController = GetWheelController(activeWheelLocation);
				if (!(wheelController == null))
				{
					bool flag = !_wheelSimulationAsleep;
					if (flag && !wheelController.enabled)
					{
						InitializeWheel(wheelController);
					}
					wheelController.enabled = flag;
				}
			}
		}

		private void LateUpdate()
		{
			if (_networkedVehicle != null && !_networkedVehicle.IsControlling)
			{
				return;
			}
			_reportTimer += Time.deltaTime;
			bool flag = _reportTimer >= 0.25f;
			if (flag)
			{
				_reportTimer = 0f;
			}
			foreach (WheelLocation activeWheelLocation in activeWheelLocations)
			{
				UpdateTireRuntime(activeWheelLocation, flag);
			}
		}

		private void CaptureBaseGrips()
		{
			WheelSlot[] array = wheelSlots;
			for (int i = 0; i < array.Length; i++)
			{
				WheelLocation wheelSlotLocation = array[i].WheelSlotLocation;
				if (!_baseLngGrip.ContainsKey(wheelSlotLocation))
				{
					WheelController wheelController = GetWheelController(wheelSlotLocation);
					if (!(wheelController == null))
					{
						_baseLngGrip[wheelSlotLocation] = wheelController.LongitudinalFrictionGrip;
						_baseLatGrip[wheelSlotLocation] = wheelController.LateralFrictionGrip;
					}
				}
			}
		}

		private void ConfigureTyreWear(WheelLocation location)
		{
			Tire tire = GetWheelSlot(location)?.InstalledTire;
			if (tire == null)
			{
				return;
			}
			WheelController wheelController = GetWheelController(location);
			if (wheelController == null)
			{
				return;
			}
			TyreWear component = wheelController.GetComponent<TyreWear>();
			if (component == null)
			{
				return;
			}
			_tyreWears[location] = component;
			TireConfig tireConfig = tire.tireConfig;
			if (tireConfig != null)
			{
				component.wearRate = tireConfig.wearRate;
				component.loadWearContribution = tireConfig.loadWearContribution;
				component.lateralSlipWearContribution = tireConfig.lateralSlipWearContribution;
				component.longitudinalSlipWearContribution = tireConfig.longitudinalSlipWearContribution;
				component.updateRate = tireConfig.updateRate;
			}
			component.maxGripReduction = 0f;
			ConditionComponent conditionComponent = tire.ConditionComponent;
			float num = ((conditionComponent != null) ? conditionComponent.ConditionRatio : 1f);
			component.wear = Mathf.Clamp01(1f - num);
			component.enabled = true;
			ApplyGrip(location, num, tireConfig);
			if (conditionComponent != null && !_tireConditionListeners.ContainsKey(location))
			{
				UnityAction<float, float> unityAction = delegate(float oldV, float newV)
				{
					OnTireConditionChanged(location, oldV, newV);
				};
				conditionComponent.OnConditionChanged.AddListener(unityAction);
				_tireConditionListeners[location] = unityAction;
			}
		}

		private void OnTireConditionChanged(WheelLocation location, float oldCondition, float newCondition)
		{
			Tire tire = GetWheelSlot(location)?.InstalledTire;
			if (tire?.ConditionComponent == null)
			{
				return;
			}
			float maxCondition = tire.ConditionComponent.MaxCondition;
			float num = ((maxCondition > 0f) ? Mathf.Clamp01(newCondition / maxCondition) : 1f);
			if (_tyreWears.TryGetValue(location, out var value) && value != null)
			{
				bool num2 = _networkedVehicle == null || _networkedVehicle.IsControlling;
				bool flag = newCondition > oldCondition;
				if (!num2 || flag)
				{
					value.wear = Mathf.Clamp01(1f - num);
				}
			}
			ApplyGrip(location, num, tire.tireConfig);
		}

		private void UpdateTireRuntime(WheelLocation location, bool doReport)
		{
			Tire tire = GetWheelSlot(location)?.InstalledTire;
			if (tire == null)
			{
				return;
			}
			WheelController wheelController = GetWheelController(location);
			if (wheelController == null || !wheelController.enabled || !_tyreWears.TryGetValue(location, out var value) || value == null)
			{
				return;
			}
			ConditionComponent conditionComponent = tire.ConditionComponent;
			if (conditionComponent == null)
			{
				return;
			}
			TireConfig tireConfig = tire.tireConfig;
			float num = 1f - Mathf.Clamp01(value.wear);
			ApplyGrip(location, num, tireConfig);
			if (doReport)
			{
				if (tireConfig != null && tireConfig.wearRateMultiplierByCondition != null)
				{
					value.wearRate = Mathf.Max(0f, tireConfig.wearRate * tireConfig.wearRateMultiplierByCondition.Evaluate(num));
				}
				float num2 = num * conditionComponent.MaxCondition;
				if (num2 < conditionComponent.Condition - 0.5f)
				{
					conditionComponent.SetCondition(num2);
				}
			}
		}

		private void ApplyGrip(WheelLocation location, float conditionRatio, TireConfig config)
		{
			WheelController wheelController = GetWheelController(location);
			if (!(wheelController == null) && _baseLngGrip.TryGetValue(location, out var value) && _baseLatGrip.TryGetValue(location, out var value2))
			{
				float num = ((config != null && config.gripMultiplierByCondition != null) ? Mathf.Clamp01(config.gripMultiplierByCondition.Evaluate(conditionRatio)) : 1f);
				wheelController.LongitudinalFrictionGrip = value * num;
				wheelController.LateralFrictionGrip = value2 * num;
			}
		}

		private void RestoreBaseGrip(WheelLocation location)
		{
			WheelController wheelController = GetWheelController(location);
			if (!(wheelController == null))
			{
				if (_baseLngGrip.TryGetValue(location, out var value))
				{
					wheelController.LongitudinalFrictionGrip = value;
				}
				if (_baseLatGrip.TryGetValue(location, out var value2))
				{
					wheelController.LateralFrictionGrip = value2;
				}
			}
		}

		public WheelController GetWheelController(WheelLocation wheelLocation)
		{
			if (!_wheelLocationToIndex.TryGetValue(wheelLocation, out var value))
			{
				return null;
			}
			if (value < 0 || value >= wheelControllers.Length)
			{
				return null;
			}
			return wheelControllers[value];
		}

		public WheelController GetWheelController(WheelSlot slot)
		{
			int num = Array.IndexOf(wheelSlots, slot);
			if (num < 0 || num >= wheelControllers.Length)
			{
				return null;
			}
			return wheelControllers[num];
		}

		public WheelSlot GetWheelSlot(WheelLocation wheelLocation)
		{
			if (!_wheelLocationToIndex.TryGetValue(wheelLocation, out var value))
			{
				return null;
			}
			if (value < 0 || value >= wheelSlots.Length)
			{
				return null;
			}
			return wheelSlots[value];
		}

		public Tire GetTire(WheelLocation wheelLocation)
		{
			if (!HasWheelLocation(wheelLocation))
			{
				return null;
			}
			return GetWheelSlot(wheelLocation)?.InstalledTire;
		}

		private Vector3 GetWheelPosition(WheelLocation wheelLocation)
		{
			WheelSlot wheelSlot = GetWheelSlot(wheelLocation);
			if (wheelSlot?.InstalledTire == null)
			{
				return Vector3.zero;
			}
			return wheelSlot.InstalledTire.transform.position;
		}

		private Vector3 GetWheelRotation(WheelLocation wheelLocation)
		{
			WheelSlot wheelSlot = GetWheelSlot(wheelLocation);
			if (wheelSlot?.InstalledTire == null)
			{
				return Vector3.zero;
			}
			return wheelSlot.InstalledTire.transform.rotation.eulerAngles;
		}

		public void UpdateWheelSuspension(WheelLocation location, float effectedMass, float targetCompression)
		{
			WheelController wheelController = GetWheelController(location);
			if (!(wheelController == null))
			{
				float num = Mathf.Abs(Physics.gravity.y);
				float num2 = _vehicleController.vehicleRigidbody.mass / (float)Mathf.Max(WheelCount, 1) + effectedMass;
				float num3 = num2 * num / targetCompression;
				wheelController.spring.maxForce = num3;
				float num4 = 2f * Mathf.Sqrt(num3 * num2);
				float num5 = 0.65f;
				wheelController.damper.bumpRate = num4 * num5 * 0.8f;
				wheelController.damper.reboundRate = num4 * num5 * 1f;
				if (_wheelsReady)
				{
					InitializeWheel(wheelController);
				}
			}
		}
	}
}
