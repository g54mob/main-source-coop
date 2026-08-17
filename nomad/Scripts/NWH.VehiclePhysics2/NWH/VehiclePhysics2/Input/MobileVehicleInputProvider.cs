using NWH.Common.Input;
using NWH.VehiclePhysics2.VehicleGUI;
using UnityEngine;
using UnityEngine.Serialization;

namespace NWH.VehiclePhysics2.Input
{
	[RequireComponent(typeof(MobileSceneInputProvider))]
	public class MobileVehicleInputProvider : VehicleInputProviderBase
	{
		public enum HorizontalAxisType
		{
			None = 0,
			Accelerometer = 1,
			SteeringWheel = 2,
			Button = 3,
			Screen = 4
		}

		public enum VerticalAxisType
		{
			None = 0,
			Accelerometer = 1,
			Button = 2,
			Screen = 3
		}

		public MobileInputButton boostButton;

		public MobileInputButton brakeButton;

		public MobileInputButton cruiseControlButton;

		public MobileInputButton engineStartStopButton;

		public MobileInputButton extraLightsButton;

		public MobileInputButton flipOverButton;

		public MobileInputButton handbrakeButton;

		public MobileInputButton hazardLightsButton;

		public MobileInputButton highBeamLightsButton;

		public MobileInputButton hornButton;

		public MobileInputButton leftBlinkerButton;

		public MobileInputButton lowBeamLightsButton;

		public MobileInputButton rightBlinkerButton;

		public MobileInputButton shiftDownButton;

		public MobileInputButton shiftUpButton;

		public MobileInputButton steerLeftButton;

		public MobileInputButton steerRightButton;

		public MobileInputButton throttleButton;

		[FormerlySerializedAs("horizontalInputType")]
		[Tooltip("    Active steer devices.")]
		public HorizontalAxisType steeringInputType = HorizontalAxisType.SteeringWheel;

		[Tooltip("    Steering wheel script. Optional and not needed if SteeringWheel option is not used.")]
		public SteeringWheel steeringWheel;

		[Tooltip("    Higher value will result in higher steer angle for same tilt.")]
		public float tiltSensitivity = 1.5f;

		public MobileInputButton trailerAttachDetachButton;

		[Tooltip("    Active steer devices.")]
		public VerticalAxisType verticalInputType = VerticalAxisType.Button;

		public override void Awake()
		{
			base.Awake();
			if (steeringInputType == HorizontalAxisType.SteeringWheel && steeringWheel == null)
			{
				Debug.LogWarning("HorizontalAxisType is set to SteeringWheel but no Steering Wheel has been assigned.");
			}
			if (steeringInputType == HorizontalAxisType.Button && (steerLeftButton == null || steerRightButton == null))
			{
				Debug.LogWarning("HorizontalAxisType is set to Button but buttons have not been assigned.");
			}
		}

		public override bool EngineStartStop()
		{
			if (engineStartStopButton != null)
			{
				return engineStartStopButton.hasBeenClicked;
			}
			return false;
		}

		public override float Clutch()
		{
			return 0f;
		}

		public override bool ExtraLights()
		{
			if (extraLightsButton != null)
			{
				return extraLightsButton.hasBeenClicked;
			}
			return false;
		}

		public override bool HighBeamLights()
		{
			if (highBeamLightsButton != null)
			{
				return highBeamLightsButton.hasBeenClicked;
			}
			return false;
		}

		public override float Handbrake()
		{
			return (!(handbrakeButton == null)) ? (handbrakeButton.isPressed ? 1 : 0) : 0;
		}

		public override bool HazardLights()
		{
			if (hazardLightsButton != null)
			{
				return hazardLightsButton.hasBeenClicked;
			}
			return false;
		}

		public override float Steering()
		{
			switch (steeringInputType)
			{
			case HorizontalAxisType.SteeringWheel:
				if (!(steeringWheel != null))
				{
					return 0f;
				}
				return steeringWheel.GetClampedValue();
			case HorizontalAxisType.Accelerometer:
				return UnityEngine.Input.acceleration.x * tiltSensitivity;
			case HorizontalAxisType.Button:
				if (steerLeftButton != null && steerRightButton != null)
				{
					if (!steerLeftButton.isPressed)
					{
						if (!steerRightButton.isPressed)
						{
							return 0f;
						}
						return 1f;
					}
					return -1f;
				}
				return 0f;
			default:
				return 0f;
			}
		}

		public override bool Horn()
		{
			if (hornButton != null)
			{
				return hornButton.isPressed;
			}
			return false;
		}

		public override bool LeftBlinker()
		{
			if (leftBlinkerButton != null)
			{
				return leftBlinkerButton.hasBeenClicked;
			}
			return false;
		}

		public override bool LowBeamLights()
		{
			if (lowBeamLightsButton != null)
			{
				return lowBeamLightsButton.hasBeenClicked;
			}
			return false;
		}

		public override bool RightBlinker()
		{
			if (rightBlinkerButton != null)
			{
				return rightBlinkerButton.hasBeenClicked;
			}
			return false;
		}

		public override bool ShiftDown()
		{
			if (shiftDownButton != null)
			{
				return shiftDownButton.hasBeenClicked;
			}
			return false;
		}

		public override int ShiftInto()
		{
			return -999;
		}

		public override bool ShiftUp()
		{
			if (shiftUpButton != null)
			{
				return shiftUpButton.hasBeenClicked;
			}
			return false;
		}

		public override bool TrailerAttachDetach()
		{
			if (trailerAttachDetachButton != null)
			{
				return trailerAttachDetachButton.hasBeenClicked;
			}
			return false;
		}

		public override float Throttle()
		{
			if (verticalInputType == VerticalAxisType.Accelerometer)
			{
				return Mathf.Clamp01(UnityEngine.Input.acceleration.y * tiltSensitivity);
			}
			if (verticalInputType == VerticalAxisType.Button)
			{
				if (throttleButton != null)
				{
					if (!throttleButton.isPressed)
					{
						return 0f;
					}
					return 1f;
				}
				Debug.LogWarning("VerticalAxisType is set to button but buttons have not been assigned.");
				return 0f;
			}
			return 0f;
		}

		public override float Brakes()
		{
			if (verticalInputType == VerticalAxisType.Accelerometer)
			{
				return Mathf.Clamp01((0f - UnityEngine.Input.acceleration.y) * tiltSensitivity);
			}
			if (verticalInputType == VerticalAxisType.Button)
			{
				if (brakeButton != null)
				{
					if (!brakeButton.isPressed)
					{
						return 0f;
					}
					return 1f;
				}
				Debug.LogWarning("VerticalAxisType is set to button but buttons have not been assigned.");
				return 0f;
			}
			return 0f;
		}

		public override bool FlipOver()
		{
			if (flipOverButton != null)
			{
				return flipOverButton.hasBeenClicked;
			}
			return false;
		}

		public override bool Boost()
		{
			if (boostButton != null)
			{
				return boostButton.isPressed;
			}
			return false;
		}

		public override bool CruiseControl()
		{
			if (cruiseControlButton != null)
			{
				return cruiseControlButton.hasBeenClicked;
			}
			return false;
		}
	}
}
