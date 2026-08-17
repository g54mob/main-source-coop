using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NWH.Common.Utility;
using NWH.Common.Vehicles;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace NWH.VehiclePhysics2.Powertrain
{
	[Serializable]
	public class TransmissionComponent : PowertrainComponent
	{
		public delegate void Shift(VehicleController vc);

		public enum AutomaticTransmissionDNRShiftType
		{
			Auto = 0,
			RequireShiftInput = 1,
			RepeatInput = 2
		}

		public enum TransmissionShiftType
		{
			Manual = 0,
			Automatic = 1,
			AutomaticSequential_Obsolete = 2,
			CVT = 3,
			External = 4
		}

		private float _referenceShiftRPM;

		[Tooltip("If true the gear input has to be held for the transmission to stay in gear, otherwise it goes to neutral.\r\nUsed for hardware H-shifters.")]
		public bool holdToKeepInGear;

		[Tooltip("    Final gear multiplier. Each gear gets multiplied by this value.\r\n    Equivalent to axle/differential ratio in real life.")]
		[ShowInSettings("Final Ratio", 1f, 20f, 1f)]
		public float finalGearRatio = 6f;

		[Tooltip("    Currently active gearing profile.\r\n    Final gear ratio will be determined from this and final gear ratio.")]
		[SerializeField]
		public TransmissionGearingProfile gearingProfile;

		[SerializeField]
		public List<float> gears = new List<float>();

		public int forwardGearCount;

		public int reverseGearCount;

		[Range(0f, 4f)]
		[Tooltip("How much inclines affect shift point position. Higher value will push the shift up and shift down RPM up depending \r\non the current incline to prevent vehicle from upshifting at the wrong time.")]
		public float inclineEffectCoeff;

		[SerializeField]
		[Tooltip("    Event that gets triggered when transmission shifts down.")]
		public UnityEvent onDownshift = new UnityEvent();

		[SerializeField]
		[Tooltip("    Event that gets triggered when transmission shifts (up or down).")]
		public UnityEvent onShift = new UnityEvent();

		[SerializeField]
		[Tooltip("    Event that gets triggered when transmission shifts up.")]
		public UnityEvent onUpshift = new UnityEvent();

		[Tooltip("    Time after shifting in which shifting can not be done again.")]
		public float postShiftBan = 0.5f;

		[FormerlySerializedAs("automaticTransmissionReverseType")]
		[FormerlySerializedAs("reverseType")]
		[Tooltip("    Behavior when switching from neutral to forward or reverse gear.")]
		public AutomaticTransmissionDNRShiftType automaticTransmissionDNRShiftType;

		public float dnrSpeedThreshold = 0.4f;

		public float clutchInputShiftThreshold = 1f;

		[Tooltip("Function that changes the gears as required.\r\nUse transmissionType External and assign this delegate to use your own gear shift code.")]
		public Shift shiftDelegate;

		[Tooltip("    Time it takes transmission to shift between gears.")]
		[ShowInSettings("Shift Duration", 0.001f, 0.5f, 0.05f)]
		public float shiftDuration = 0.2f;

		[Range(0f, 1f)]
		[Tooltip("Intensity of variable shift point. Higher value will result in shift point moving higher up with higher engine load.")]
		public float variableShiftIntensity = 0.3f;

		[Tooltip("    If enabled shifting when in manual transmission will be instant, ignoring post shift ban.")]
		public bool ignorePostShiftBanInManual = true;

		[Tooltip("    If enabled transmission will adjust both shift up and down points to match current load.")]
		[ShowInSettings("Variable Shift Point")]
		public bool variableShiftPoint = true;

		[Tooltip("    Should the transmission shift while the vehicle is fully in air. All wheels must be off the ground for it to be considered as in air.")]
		public bool shiftInAir = true;

		[Tooltip("Current gear ratio.")]
		[ShowInTelemetry]
		public float currentGearRatio;

		public bool isPostShiftBanActive;

		public bool isShifting;

		public float shiftProgress;

		[SerializeField]
		private float _downshiftRPM = 1400f;

		[ShowInTelemetry]
		private float _targetDownshiftRPM;

		[Tooltip("RPM at which automatic transmission will shift up. If dynamic shift point is enabled this value will change depending on load.")]
		[SerializeField]
		[ShowInTelemetry]
		private float _upshiftRPM = 2800f;

		[ShowInTelemetry]
		private float _targetUpshiftRPM;

		[SerializeField]
		[Tooltip("Manual - gears can only be shifted by manual user input. Automatic - automatic gear changing. Allows for gear skipping (e.g. 3rd->5th) which can be useful in trucks and other high gear count vehicles. AutomaticSequential - automatic gear changing but only one gear at the time can be shifted (e.g. 3rd->4th)")]
		[ShowInSettings("Type")]
		[FormerlySerializedAs("_transmissionType")]
		public TransmissionShiftType transmissionType = TransmissionShiftType.Automatic;

		private TransmissionShiftType _prevTransmissionType;

		[ShowInSettings("Sequential")]
		[Tooltip("Is the automatic gearbox sequential?\r\nHas no effect on manual transmission.")]
		public bool isSequential;

		public bool allowUpshiftGearSkipping;

		public bool allowDownshiftGearSkipping = true;

		private bool _repeatInputFlag;

		private float _smoothedThrottleInput;

		private float _slipOutOfGearTimer = -999f;

		[NonSerialized]
		[ShowInTelemetry]
		public int gearIndex;

		public UnityEvent triedToShiftWithoutClutch = new UnityEvent();

		private Coroutine _shiftCoroutine;

		private bool _isShiftCoroutineRunning;

		private Dictionary<int, string> _gearNameCache = new Dictionary<int, string>();

		public float ReferenceShiftRPM => _referenceShiftRPM;

		public float DownshiftRPM
		{
			get
			{
				return _downshiftRPM;
			}
			set
			{
				_downshiftRPM = Mathf.Clamp(value, 0f, 1f / 0f);
			}
		}

		public float TargetDownshiftRPM => _targetDownshiftRPM;

		public float UpshiftRPM
		{
			get
			{
				return _upshiftRPM;
			}
			set
			{
				_upshiftRPM = Mathf.Clamp(value, 0f, 1f / 0f);
			}
		}

		public float TargetUpshiftRPM => _targetUpshiftRPM;

		public int Gear
		{
			get
			{
				return IndexToGear(gearIndex);
			}
			set
			{
				gearIndex = GearToIndex(value);
			}
		}

		public string GearName
		{
			get
			{
				int gear = Gear;
				if (_gearNameCache.TryGetValue(gear, out var value))
				{
					return value;
				}
				value = ((gear == 0) ? "N" : ((gear <= 0) ? ("R" + -gear) : Gear.ToString()));
				_gearNameCache[gear] = value;
				return value;
			}
		}

		protected override void VC_Initialize()
		{
			UpdateGearCounts();
			Gear = 0;
			if (transmissionType == TransmissionShiftType.AutomaticSequential_Obsolete)
			{
				transmissionType = TransmissionShiftType.Automatic;
			}
			AssignShiftDelegate();
			base.VC_Initialize();
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				Gear = 0;
				currentGearRatio = 0f;
				if (_shiftCoroutine != null)
				{
					vehicleController.StopCoroutine(_shiftCoroutine);
				}
				isShifting = false;
				isPostShiftBanActive = false;
				return true;
			}
			return false;
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			inertia = 0.02f;
			gears = new List<float> { -2.216f, 0f, 3.274f, 2.093f, 1.439f, 1.084f, 0.817f, 0.651f };
			triedToShiftWithoutClutch = new UnityEvent();
		}

		public override void VC_Validate(VehicleController vc)
		{
			base.VC_Validate(vc);
			if (gears == null || gears.Count == 0)
			{
				PC_LogWarning(vc, "Gears list is empty on " + vc.name + ". Open the VehicleController > PWR > Transmission tab in Unity editor to load the gears from the (now obsolete) gearing profile.");
				return;
			}
			if (gears.Count > 2)
			{
				if (!gears.Any((float g) => g > 0f))
				{
					PC_LogWarning(vc, "Gears list does not have any reverse gears. Reverse gears should be added first to the gears list and should be negative. Example gears list: -3, 0, 3, 2, 1.");
				}
				if (gears.All((float g) => g != 0f))
				{
					PC_LogWarning(vc, "There is no neutral gear. There should be one neutral gear in the gears list, placed in between the reverse (negative) and forward (positive) gears. Example gears list: -3, 0, 3, 2, 1.");
				}
				if (!gears.Any((float g) => g < 0f))
				{
					PC_LogWarning(vc, "Gears list does not have any forward gears. Forward gears should be added after the reverse (negative) and neutral (0) gears and should be positive. Example gears list: -3, 0, 3, 2, 1.");
				}
			}
			if (transmissionType == TransmissionShiftType.CVT && gears.Count != 3)
			{
				PC_LogWarning(vc, "CVT Transmission type requires 3 gears in the gears list, one reverse, one neutral and one forward. E.g. -3, 0, 2.");
			}
			if (_upshiftRPM > vc.powertrain.engine.revLimiterRPM || _upshiftRPM > vc.powertrain.engine.revLimiterRPM)
			{
				PC_LogWarning(vc, "Upshift RPM set to higher RPM than the engine can achieve (check revLimiterRPM).");
			}
			if (vc.powertrain.engine.engineType == EngineComponent.EngineType.ICE && DownshiftRPM < vc.powertrain.engine.idleRPM)
			{
				PC_LogWarning(vc, $"Downshift RPM ({DownshiftRPM}) set to a lower value than the engine idle RPM ({vc.powertrain.engine.idleRPM}).");
			}
		}

		public void LoadGearsFromGearingProfile()
		{
			if (!(gearingProfile == null))
			{
				int capacity = gearingProfile.reverseGears.Count + 1 + gearingProfile.forwardGears.Count;
				if (gears == null)
				{
					gears = new List<float>(capacity);
				}
				else
				{
					gears.Clear();
					gears.Capacity = capacity;
				}
				gears.AddRange(gearingProfile.reverseGears);
				gears.Add(0f);
				gears.AddRange(gearingProfile.forwardGears);
			}
		}

		private float CalculateTotalGearRatio()
		{
			if (transmissionType == TransmissionShiftType.CVT)
			{
				float num = gears[gearIndex];
				float a = num * 40f;
				float t = Mathf.Clamp01(vehicleController.powertrain.engine.RPMPercent + (1f - vehicleController.powertrain.engine.ThrottlePosition));
				float b = Mathf.Lerp(a, num, t) * finalGearRatio;
				return Mathf.Lerp(currentGearRatio, b, Time.fixedDeltaTime * 5f);
			}
			return gears[gearIndex] * finalGearRatio;
		}

		private float CalculateNoSlipRPM()
		{
			float localForwardVelocity = vehicleController.LocalForwardVelocity;
			float num = 0f;
			foreach (WheelComponent wheel in vehicleController.powertrain.wheels)
			{
				num += localForwardVelocity / wheel.wheelUAPI.Radius;
			}
			return UnitConverter.AngularVelocityToRPM(num / (float)vehicleController.powertrain.wheelCount) * currentGearRatio;
		}

		public float GetGearRatio(int g)
		{
			return gears[GearToIndex(g)] * finalGearRatio;
		}

		public override float QueryAngularVelocity(float angularVelocity, float dt)
		{
			inputAngularVelocity = angularVelocity;
			if (outputNameHash == 0 || (currentGearRatio < 1E-05f && currentGearRatio > -1E-05f))
			{
				outputAngularVelocity = 0f;
				return angularVelocity;
			}
			outputAngularVelocity = inputAngularVelocity / currentGearRatio;
			return _output.QueryAngularVelocity(outputAngularVelocity, dt) * currentGearRatio;
		}

		public override float QueryInertia()
		{
			if (outputNameHash == 0 || (currentGearRatio < 1E-05f && currentGearRatio > -1E-05f))
			{
				return inertia;
			}
			return inertia + _output.QueryInertia() / (currentGearRatio * currentGearRatio);
		}

		public float ReverseTransmitRPM(float inputRPM, int g)
		{
			return Mathf.Abs(inputRPM * gears[GearToIndex(g)] * finalGearRatio);
		}

		private void AssignShiftDelegate()
		{
			if (transmissionType == TransmissionShiftType.Manual)
			{
				shiftDelegate = ManualShift;
			}
			else if (transmissionType == TransmissionShiftType.Automatic)
			{
				shiftDelegate = AutomaticShift;
			}
			else if (transmissionType == TransmissionShiftType.CVT)
			{
				shiftDelegate = CVTShift;
			}
		}

		private void UpdateGearCounts()
		{
			forwardGearCount = 0;
			reverseGearCount = 0;
			int count = gears.Count;
			for (int i = 0; i < count; i++)
			{
				float num = gears[i];
				if (num > 0f)
				{
					forwardGearCount++;
				}
				else if (num < 0f)
				{
					reverseGearCount++;
				}
			}
		}

		public override float ForwardStep(float torque, float inertiaSum, float dt)
		{
			inputTorque = torque;
			inputInertia = inertiaSum;
			if (_prevTransmissionType != transmissionType)
			{
				AssignShiftDelegate();
			}
			_prevTransmissionType = transmissionType;
			UpdateGearCounts();
			if (_output == null)
			{
				PC_LogWarning(vehicleController, "Transmission output is null.");
				return inputTorque;
			}
			currentGearRatio = CalculateTotalGearRatio();
			_referenceShiftRPM = CalculateNoSlipRPM();
			shiftDelegate(vehicleController);
			vehicleController.input.ResetShiftFlags();
			if (outputNameHash == 0)
			{
				return torque;
			}
			if (currentGearRatio < 1E-05f && currentGearRatio > -1E-05f)
			{
				outputTorque = 0f;
				outputInertia = inputInertia;
				_output.ForwardStep(outputTorque, outputInertia, dt);
				return torque;
			}
			outputTorque = torque * currentGearRatio;
			outputInertia = (inertiaSum + inertia) * (currentGearRatio * currentGearRatio);
			return _output.ForwardStep(torque * currentGearRatio, outputInertia, dt) / currentGearRatio;
		}

		public void ShiftInto(int targetGear, bool instant = false)
		{
			if (vehicleController.input.Clutch > clutchInputShiftThreshold)
			{
				triedToShiftWithoutClutch.Invoke();
				return;
			}
			int gear = Gear;
			bool flag = targetGear == 0 || gear == 0;
			if (targetGear == gear || targetGear < -100 || _damage == 1f)
			{
				return;
			}
			int num = GearToIndex(targetGear);
			if (num >= 0 && num < gears.Count && !isShifting && (flag || !isPostShiftBanActive))
			{
				_shiftCoroutine = vehicleController.StartCoroutine(ShiftCoroutine(gear, targetGear, flag || instant));
				if (targetGear == 0)
				{
					_repeatInputFlag = false;
				}
			}
		}

		private IEnumerator ShiftCoroutine(int currentGear, int targetGear, bool instant)
		{
			if (_isShiftCoroutineRunning)
			{
				vehicleController.StopCoroutine(_shiftCoroutine);
				_isShiftCoroutineRunning = false;
			}
			if (isShifting)
			{
				yield return null;
			}
			if (!shiftInAir && !vehicleController.IsGrounded())
			{
				yield return null;
			}
			_isShiftCoroutineRunning = true;
			float dt = 0.02f;
			bool isManual = transmissionType == TransmissionShiftType.Manual;
			if (!isManual)
			{
				isPostShiftBanActive = true;
			}
			isShifting = true;
			shiftProgress = 0f;
			float shiftTimer = 0f;
			float halfDuration = shiftDuration * 0.5f;
			if (!instant)
			{
				while (shiftTimer < halfDuration)
				{
					shiftProgress = shiftTimer / shiftDuration;
					shiftTimer += dt;
					yield return new WaitForSeconds(dt);
				}
			}
			Gear = targetGear;
			if (currentGear < targetGear)
			{
				onUpshift.Invoke();
			}
			else
			{
				onDownshift.Invoke();
			}
			onShift.Invoke();
			if (!instant)
			{
				while (shiftTimer < shiftDuration)
				{
					shiftProgress = shiftTimer / shiftDuration;
					shiftTimer += dt;
					yield return new WaitForSeconds(dt);
				}
			}
			shiftProgress = 1f;
			isShifting = false;
			if (!isManual)
			{
				float postShiftBanTimer = 0f;
				while (postShiftBanTimer < postShiftBan)
				{
					postShiftBanTimer += dt;
					yield return new WaitForSeconds(dt);
				}
				isPostShiftBanActive = false;
			}
			_isShiftCoroutineRunning = false;
		}

		private void CVTShift(VehicleController vc)
		{
			AutomaticShift(vc);
		}

		private void AutomaticShift(VehicleController vc)
		{
			float speed = vc.Speed;
			float inputSwappedThrottle = vc.input.InputSwappedThrottle;
			float inputSwappedBrakes = vc.input.InputSwappedBrakes;
			int gear = Gear;
			_targetDownshiftRPM = _downshiftRPM;
			_targetUpshiftRPM = _upshiftRPM;
			if (variableShiftPoint)
			{
				_smoothedThrottleInput = Mathf.Lerp(_smoothedThrottleInput, inputSwappedThrottle, vc.fixedDeltaTime * 2f);
				float revLimiterRPM = vc.powertrain.engine.revLimiterRPM;
				_targetUpshiftRPM = _upshiftRPM + Mathf.Clamp01(_smoothedThrottleInput * variableShiftIntensity) * _upshiftRPM;
				_targetUpshiftRPM = Mathf.Clamp(_targetUpshiftRPM, _upshiftRPM, revLimiterRPM * 0.97f);
				_targetDownshiftRPM = _downshiftRPM + Mathf.Clamp01(_smoothedThrottleInput * variableShiftIntensity) * _downshiftRPM;
				_targetDownshiftRPM = Mathf.Clamp(_targetDownshiftRPM, vc.powertrain.engine.idleRPM * 1.1f, _targetUpshiftRPM * 0.7f);
				float num = Mathf.Clamp01(Vector3.Dot(vc.vehicleTransform.forward, Vector3.up) * inclineEffectCoeff);
				_targetUpshiftRPM += revLimiterRPM * num;
				_targetDownshiftRPM += revLimiterRPM * num;
			}
			if (gear == 0)
			{
				if (automaticTransmissionDNRShiftType == AutomaticTransmissionDNRShiftType.Auto)
				{
					if (inputSwappedThrottle > 0.02f)
					{
						ShiftInto(1);
					}
					else if (inputSwappedBrakes > 0.02f)
					{
						ShiftInto(-1);
					}
				}
				else if (automaticTransmissionDNRShiftType == AutomaticTransmissionDNRShiftType.RequireShiftInput)
				{
					if (vc.input.ShiftUp || vc.input.ShiftInto == 1)
					{
						ShiftInto(1);
					}
					else if (vc.input.ShiftDown || vc.input.ShiftInto == -1)
					{
						ShiftInto(-1);
					}
				}
				else
				{
					if (automaticTransmissionDNRShiftType != AutomaticTransmissionDNRShiftType.RepeatInput)
					{
						return;
					}
					if (!_repeatInputFlag && inputSwappedThrottle < 0.02f && inputSwappedBrakes < 0.02f)
					{
						_repeatInputFlag = true;
					}
					if (_repeatInputFlag)
					{
						if (inputSwappedThrottle > 0.02f)
						{
							ShiftInto(1);
						}
						else if (inputSwappedBrakes > 0.02f)
						{
							ShiftInto(-1);
						}
					}
				}
			}
			else if (gear < 0)
			{
				if (automaticTransmissionDNRShiftType == AutomaticTransmissionDNRShiftType.RequireShiftInput)
				{
					if (vc.input.ShiftUp || vc.input.ShiftInto == 0)
					{
						ShiftInto(0);
					}
					else if (vc.input.ShiftInto == 1)
					{
						ShiftInto(1);
					}
				}
				else if (speed < dnrSpeedThreshold && (inputSwappedBrakes > 0.02f || inputSwappedThrottle < 0.02f))
				{
					ShiftInto(0);
				}
				float num2 = gear - 1;
				num2 = ((num2 < 0f) ? (0f - num2) : num2);
				if (_referenceShiftRPM > TargetUpshiftRPM && num2 <= (float)reverseGearCount)
				{
					ShiftInto(gear - 1);
				}
				else if (_referenceShiftRPM < TargetDownshiftRPM && gear < -1)
				{
					ShiftInto(gear + 1);
				}
			}
			else if (speed > 0.4f)
			{
				if (gear < forwardGearCount && _referenceShiftRPM > TargetUpshiftRPM)
				{
					if (!isSequential && allowUpshiftGearSkipping)
					{
						int num3 = gear;
						while (num3 < forwardGearCount)
						{
							num3++;
							float num4 = ReverseTransmitRPM(_referenceShiftRPM / currentGearRatio, num3);
							float num5 = Mathf.Clamp01(shiftDuration) * (_targetUpshiftRPM - _targetDownshiftRPM) * 0.25f;
							if (num4 < _targetDownshiftRPM + num5)
							{
								num3--;
								break;
							}
						}
						if (num3 != gear)
						{
							ShiftInto(num3);
						}
					}
					else
					{
						ShiftInto(gear + 1);
					}
				}
				else
				{
					if (!(_referenceShiftRPM < TargetDownshiftRPM))
					{
						return;
					}
					if (!isSequential && allowDownshiftGearSkipping)
					{
						if (gear != 1)
						{
							int num6 = gear;
							while (num6 > 1)
							{
								num6--;
								if (ReverseTransmitRPM(_referenceShiftRPM / currentGearRatio, num6) > _targetUpshiftRPM)
								{
									num6++;
									break;
								}
							}
							if (num6 != gear)
							{
								ShiftInto(num6);
							}
						}
						else if (speed < dnrSpeedThreshold && inputSwappedThrottle < 0.02f && automaticTransmissionDNRShiftType != AutomaticTransmissionDNRShiftType.RequireShiftInput)
						{
							ShiftInto(0);
						}
					}
					else if (gear != 1)
					{
						ShiftInto(gear - 1);
					}
					else if (speed < dnrSpeedThreshold && inputSwappedThrottle < 0.02f && inputSwappedBrakes < 0.02f && automaticTransmissionDNRShiftType != AutomaticTransmissionDNRShiftType.RequireShiftInput)
					{
						ShiftInto(0);
					}
				}
			}
			else if (automaticTransmissionDNRShiftType != AutomaticTransmissionDNRShiftType.RequireShiftInput)
			{
				if (inputSwappedThrottle < 0.02f)
				{
					ShiftInto(0);
				}
			}
			else if (vc.input.ShiftDown || vc.input.ShiftInto == 0)
			{
				ShiftInto(0);
			}
			else if (vc.input.ShiftInto == -1 && vc.Speed < dnrSpeedThreshold)
			{
				ShiftInto(-1);
			}
		}

		private int GearToIndex(int g)
		{
			return g + reverseGearCount;
		}

		private int IndexToGear(int g)
		{
			return g - reverseGearCount;
		}

		private void ManualShift(VehicleController vc)
		{
			if (vc.input.ShiftUp)
			{
				ShiftInto(Gear + 1);
				return;
			}
			if (vc.input.ShiftDown)
			{
				ShiftInto(Gear - 1);
				return;
			}
			int shiftInto = vc.input.ShiftInto;
			if (shiftInto > -100)
			{
				ShiftInto(shiftInto);
				_slipOutOfGearTimer = 0f;
			}
			else if (holdToKeepInGear)
			{
				_slipOutOfGearTimer += vc.fixedDeltaTime;
				if (Gear != 0 && _slipOutOfGearTimer > 0.1f)
				{
					ShiftInto(0);
				}
			}
		}
	}
}
