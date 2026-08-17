using System;
using System.Collections.Generic;
using NWH.Common.Input;
using NWH.Common.Vehicles;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace NWH.VehiclePhysics2.Input
{
	[Serializable]
	public class VehicleInputHandler : VehicleComponent
	{
		private delegate bool BinaryInputDelegate();

		[FormerlySerializedAs("autoSettable")]
		[Tooltip("    When enabled input will be auto-retrieved from the InputProviders present in the scene.\r\n Disable to manualy set the input through external scripts, i.e. AI controller.")]
		public bool autoSetInput = true;

		[NonSerialized]
		[Tooltip("All the input states of the vehicle. Can be used to set input through scripting or copy the inputs\r\nover from other vehicle, such as truck to trailer.")]
		public VehicleInputStates states;

		[Tooltip("    Swaps throttle and brake axes when vehicle is in reverse.")]
		[ShowInSettings("Swap Input In R")]
		public bool swapInputInReverse = true;

		public UnityEvent inputModifyCallback = new UnityEvent();

		private List<InputProvider> _inputProviders = new List<InputProvider>();

		public float Vertical
		{
			get
			{
				return states.throttle - states.brakes;
			}
			set
			{
				float num = ((value < -1f) ? (-1f) : ((value > 1f) ? 1f : value));
				if (value > 0f)
				{
					states.throttle = num;
					states.brakes = 0f;
				}
				else
				{
					states.throttle = 0f;
					states.brakes = 0f - num;
				}
			}
		}

		public float Throttle
		{
			get
			{
				return states.throttle;
			}
			set
			{
				value = ((value < 0f) ? 0f : ((value > 1f) ? 1f : value));
				states.throttle = value;
			}
		}

		public float Brakes
		{
			get
			{
				return states.brakes;
			}
			set
			{
				value = ((value < 0f) ? 0f : ((value > 1f) ? 1f : value));
				states.brakes = value;
			}
		}

		public float InputSwappedThrottle => states.inputSwappedThrottle;

		public float InputSwappedBrakes => states.inputSwappedBrakes;

		public float Steering
		{
			get
			{
				return states.steering;
			}
			set
			{
				value = ((value < -1f) ? (-1f) : ((value > 1f) ? 1f : value));
				states.steering = value;
			}
		}

		public float Clutch
		{
			get
			{
				return states.clutch;
			}
			set
			{
				value = ((value < 0f) ? 0f : ((value > 1f) ? 1f : value));
				states.clutch = value;
			}
		}

		public bool EngineStartStop
		{
			get
			{
				return states.engineStartStop;
			}
			set
			{
				states.engineStartStop = value;
			}
		}

		public bool ExtraLights
		{
			get
			{
				return states.extraLights;
			}
			set
			{
				states.extraLights = value;
			}
		}

		public bool HighBeamLights
		{
			get
			{
				return states.highBeamLights;
			}
			set
			{
				states.highBeamLights = value;
			}
		}

		public float Handbrake
		{
			get
			{
				return states.handbrake;
			}
			set
			{
				value = ((value < 0f) ? 0f : ((value > 1f) ? 1f : value));
				states.handbrake = value;
			}
		}

		public bool HazardLights
		{
			get
			{
				return states.hazardLights;
			}
			set
			{
				states.hazardLights = value;
			}
		}

		public bool Horn
		{
			get
			{
				return states.horn;
			}
			set
			{
				states.horn = value;
			}
		}

		public bool LeftBlinker
		{
			get
			{
				return states.leftBlinker;
			}
			set
			{
				states.leftBlinker = value;
			}
		}

		public bool LowBeamLights
		{
			get
			{
				return states.lowBeamLights;
			}
			set
			{
				states.lowBeamLights = value;
			}
		}

		public bool RightBlinker
		{
			get
			{
				return states.rightBlinker;
			}
			set
			{
				states.rightBlinker = value;
			}
		}

		public bool ShiftDown
		{
			get
			{
				return states.shiftDown;
			}
			set
			{
				states.shiftDown = value;
			}
		}

		public int ShiftInto
		{
			get
			{
				return states.shiftInto;
			}
			set
			{
				states.shiftInto = value;
			}
		}

		public bool ShiftUp
		{
			get
			{
				return states.shiftUp;
			}
			set
			{
				states.shiftUp = value;
			}
		}

		public bool TrailerAttachDetach
		{
			get
			{
				return states.trailerAttachDetach;
			}
			set
			{
				states.trailerAttachDetach = value;
			}
		}

		public bool CruiseControl
		{
			get
			{
				return states.cruiseControl;
			}
			set
			{
				states.cruiseControl = value;
			}
		}

		public bool Boost
		{
			get
			{
				return states.boost;
			}
			set
			{
				states.boost = value;
			}
		}

		public bool FlipOver
		{
			get
			{
				return states.flipOver;
			}
			set
			{
				states.flipOver = value;
			}
		}

		public bool IsInputSwapped
		{
			get
			{
				if (swapInputInReverse)
				{
					return vehicleController.powertrain.transmission.Gear < 0;
				}
				return false;
			}
		}

		protected override void VC_Initialize()
		{
			_inputProviders = InputProvider.Instances;
			if (autoSetInput && (_inputProviders == null || _inputProviders.Count == 0))
			{
				Debug.LogWarning("No InputProviders are present in the scene. Make sure that one or more InputProviders are present (DesktopInputProvider, MobileInputProvider, etc.).");
			}
			vehicleController.powertrain.transmission.onShift.AddListener(CalculateInputSwappedValues);
			states.Reset();
			base.VC_Initialize();
		}

		public override void VC_Update()
		{
			base.VC_Update();
			if (!autoSetInput)
			{
				CalculateInputSwappedValues();
				return;
			}
			Throttle = InputProvider.CombinedInput((VehicleInputProviderBase i) => i.Throttle());
			states.throttleRaw = Throttle;
			Brakes = InputProvider.CombinedInput((VehicleInputProviderBase i) => i.Brakes());
			states.brakesRaw = Brakes;
			Steering = InputProvider.CombinedInput((VehicleInputProviderBase i) => i.Steering());
			states.steeringRaw = Steering;
			Clutch = InputProvider.CombinedInput((VehicleInputProviderBase i) => i.Clutch());
			states.clutchRaw = Clutch;
			Handbrake = InputProvider.CombinedInput((VehicleInputProviderBase i) => i.Handbrake());
			states.handbrakeRaw = Handbrake;
			ShiftInto = CombinedInputGear((VehicleInputProviderBase i) => i.ShiftInto());
			ShiftUp |= InputProvider.CombinedInput((VehicleInputProviderBase i) => i.ShiftUp());
			ShiftDown |= InputProvider.CombinedInput((VehicleInputProviderBase i) => i.ShiftDown());
			LeftBlinker |= InputProvider.CombinedInput((VehicleInputProviderBase i) => i.LeftBlinker());
			RightBlinker |= InputProvider.CombinedInput((VehicleInputProviderBase i) => i.RightBlinker());
			LowBeamLights |= InputProvider.CombinedInput((VehicleInputProviderBase i) => i.LowBeamLights());
			HighBeamLights |= InputProvider.CombinedInput((VehicleInputProviderBase i) => i.HighBeamLights());
			HazardLights |= InputProvider.CombinedInput((VehicleInputProviderBase i) => i.HazardLights());
			ExtraLights |= InputProvider.CombinedInput((VehicleInputProviderBase i) => i.ExtraLights());
			Horn = InputProvider.CombinedInput((VehicleInputProviderBase i) => i.Horn());
			EngineStartStop |= InputProvider.CombinedInput((VehicleInputProviderBase i) => i.EngineStartStop());
			Boost = InputProvider.CombinedInput((VehicleInputProviderBase i) => i.Boost());
			TrailerAttachDetach = TrailerAttachDetach || InputProvider.CombinedInput((VehicleInputProviderBase i) => i.TrailerAttachDetach());
			CruiseControl |= InputProvider.CombinedInput((VehicleInputProviderBase i) => i.CruiseControl());
			FlipOver = FlipOver || InputProvider.CombinedInput((VehicleInputProviderBase i) => i.FlipOver());
			inputModifyCallback.Invoke();
			CalculateInputSwappedValues();
		}

		private void CalculateInputSwappedValues()
		{
			bool isInputSwapped = IsInputSwapped;
			states.inputSwappedThrottle = (isInputSwapped ? states.brakes : states.throttle);
			states.inputSwappedThrottleRaw = (isInputSwapped ? states.brakesRaw : states.throttleRaw);
			states.inputSwappedBrakes = (isInputSwapped ? states.throttle : states.brakes);
			states.inputSwappedBrakesRaw = (isInputSwapped ? states.throttleRaw : states.brakesRaw);
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				states.Reset();
				return true;
			}
			return false;
		}

		public static int CombinedInputGear<T>(Func<T, int> selector) where T : InputProvider
		{
			int num = -999;
			for (int i = 0; i < InputProvider.Instances.Count; i++)
			{
				InputProvider inputProvider = InputProvider.Instances[i];
				if (inputProvider is T)
				{
					int num2 = selector(inputProvider as T);
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		public void ResetShiftFlags()
		{
			states.shiftUp = false;
			states.shiftDown = false;
			states.shiftInto = -999;
		}
	}
}
