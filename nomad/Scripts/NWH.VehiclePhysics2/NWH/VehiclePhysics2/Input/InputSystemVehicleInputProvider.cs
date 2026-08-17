using UnityEngine;
using UnityEngine.InputSystem;

namespace NWH.VehiclePhysics2.Input
{
	public class InputSystemVehicleInputProvider : VehicleInputProviderBase
	{
		private const int H_SHIFTER_GEAR_COUNT = 10;

		public static VehicleInputActions vehicleInputActions;

		[Tooltip("    Should mouse be used for input?")]
		public bool mouseInput;

		private readonly bool[] _shiftIntoHeld = new bool[10];

		private float _throttle;

		private float _brakes;

		private float _steering;

		private float _clutch;

		private float _handbrake;

		private bool _horn;

		private bool _boost;

		public new void Awake()
		{
			base.Awake();
			vehicleInputActions = new VehicleInputActions();
			vehicleInputActions.Enable();
			SetupGearShiftInput(vehicleInputActions.VehicleControls.ShiftIntoR1, 0);
			SetupGearShiftInput(vehicleInputActions.VehicleControls.ShiftInto0, 1);
			SetupGearShiftInput(vehicleInputActions.VehicleControls.ShiftInto1, 2);
			SetupGearShiftInput(vehicleInputActions.VehicleControls.ShiftInto2, 3);
			SetupGearShiftInput(vehicleInputActions.VehicleControls.ShiftInto3, 4);
			SetupGearShiftInput(vehicleInputActions.VehicleControls.ShiftInto4, 5);
			SetupGearShiftInput(vehicleInputActions.VehicleControls.ShiftInto5, 6);
			SetupGearShiftInput(vehicleInputActions.VehicleControls.ShiftInto6, 7);
			SetupGearShiftInput(vehicleInputActions.VehicleControls.ShiftInto7, 8);
			SetupGearShiftInput(vehicleInputActions.VehicleControls.ShiftInto8, 9);
			vehicleInputActions.VehicleControls.Horn.started += delegate
			{
				_horn = true;
			};
			vehicleInputActions.VehicleControls.Horn.canceled += delegate
			{
				_horn = false;
			};
			vehicleInputActions.VehicleControls.Boost.started += delegate
			{
				_boost = true;
			};
			vehicleInputActions.VehicleControls.Boost.canceled += delegate
			{
				_boost = false;
			};
		}

		private void SetupGearShiftInput(InputAction gearShiftAction, int index)
		{
			gearShiftAction.started += delegate
			{
				_shiftIntoHeld[index] = true;
			};
			gearShiftAction.canceled += delegate
			{
				_shiftIntoHeld[index] = false;
			};
		}

		public void Update()
		{
			_throttle = (mouseInput ? Mathf.Clamp(GetMouseVertical(), 0f, 1f) : vehicleInputActions.VehicleControls.Throttle.ReadValue<float>());
			_brakes = (mouseInput ? (0f - Mathf.Clamp(GetMouseVertical(), -1f, 0f)) : vehicleInputActions.VehicleControls.Brakes.ReadValue<float>());
			_steering = (mouseInput ? Mathf.Clamp(GetMouseHorizontal(), -1f, 1f) : vehicleInputActions.VehicleControls.Steering.ReadValue<float>());
			_clutch = vehicleInputActions.VehicleControls.Clutch.ReadValue<float>();
			_handbrake = vehicleInputActions.VehicleControls.Handbrake.ReadValue<float>();
		}

		public void OnEnable()
		{
			vehicleInputActions?.Enable();
		}

		public void OnDisable()
		{
			vehicleInputActions?.Disable();
		}

		public override float Throttle()
		{
			return _throttle;
		}

		public override float Brakes()
		{
			return _brakes;
		}

		public override float Steering()
		{
			return _steering;
		}

		public override float Clutch()
		{
			return _clutch;
		}

		public override float Handbrake()
		{
			return _handbrake;
		}

		public override bool EngineStartStop()
		{
			return vehicleInputActions.VehicleControls.EngineStartStop.triggered;
		}

		public override bool ExtraLights()
		{
			return vehicleInputActions.VehicleControls.ExtraLights.triggered;
		}

		public override bool HighBeamLights()
		{
			return vehicleInputActions.VehicleControls.HighBeamLights.triggered;
		}

		public override bool HazardLights()
		{
			return vehicleInputActions.VehicleControls.HazardLights.triggered;
		}

		public override bool Horn()
		{
			return _horn;
		}

		public override bool LeftBlinker()
		{
			return vehicleInputActions.VehicleControls.LeftBlinker.triggered;
		}

		public override bool LowBeamLights()
		{
			return vehicleInputActions.VehicleControls.LowBeamLights.triggered;
		}

		public override bool RightBlinker()
		{
			return vehicleInputActions.VehicleControls.RightBlinker.triggered;
		}

		public override bool ShiftDown()
		{
			return vehicleInputActions.VehicleControls.ShiftDown.triggered;
		}

		public override bool ShiftUp()
		{
			return vehicleInputActions.VehicleControls.ShiftUp.triggered;
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
			_throttle = 0f;
			_brakes = 0f;
			_steering = 0f;
			_clutch = 0f;
			_handbrake = 0f;
		}

		public override int ShiftInto()
		{
			for (int i = 0; i < 10; i++)
			{
				if (_shiftIntoHeld[i])
				{
					return i - 1;
				}
			}
			return -999;
		}

		public override bool TrailerAttachDetach()
		{
			return vehicleInputActions.VehicleControls.TrailerAttachDetach.triggered;
		}

		public override bool FlipOver()
		{
			return vehicleInputActions.VehicleControls.FlipOver.triggered;
		}

		public override bool Boost()
		{
			return _boost;
		}

		public override bool CruiseControl()
		{
			return vehicleInputActions.VehicleControls.CruiseControl.triggered;
		}

		private float GetMouseHorizontal()
		{
			float num = Mathf.Clamp(Mouse.current.position.ReadValue().x / (float)Screen.width, -1f, 1f);
			if (num < 0.5f)
			{
				return (0f - (0.5f - num)) * 2f;
			}
			return (num - 0.5f) * 2f;
		}

		private float GetMouseVertical()
		{
			float num = Mathf.Clamp(Mouse.current.position.ReadValue().y / (float)Screen.height, -1f, 1f);
			if (num < 0.5f)
			{
				return (0f - (0.5f - num)) * 2f;
			}
			return (num - 0.5f) * 2f;
		}
	}
}
