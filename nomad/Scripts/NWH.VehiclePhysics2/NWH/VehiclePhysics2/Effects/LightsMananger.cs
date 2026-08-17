using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace NWH.VehiclePhysics2.Effects
{
	[Serializable]
	public class LightsMananger : Effect
	{
		[FormerlySerializedAs("stopLights")]
		[Tooltip("    Rear lights that will light up when brake is pressed. Always red.")]
		public VehicleLight brakeLights = new VehicleLight();

		[Tooltip("    Can be used for any type of special lights, e.g. beacons.")]
		public VehicleLight extraLights = new VehicleLight();

		[FormerlySerializedAs("fullBeams")]
		[Tooltip("    High (full) beam lights.")]
		public VehicleLight highBeamLights = new VehicleLight();

		[Tooltip("    Blinkers on the left side of the vehicle.")]
		public VehicleLight leftBlinkers = new VehicleLight();

		[FormerlySerializedAs("headLights")]
		[Tooltip("    Low beam lights.")]
		public VehicleLight lowBeamLights = new VehicleLight();

		[Tooltip("    Rear Lights that will light up when vehicle is in reverse gear(s). Usually white.")]
		public VehicleLight reverseLights = new VehicleLight();

		[Tooltip("    Blinkers on the right side of the vehicle.")]
		public VehicleLight rightBlinkers = new VehicleLight();

		[FormerlySerializedAs("rearLights")]
		[Tooltip("    Rear Lights that will light up when headlights are on. Always red.")]
		public VehicleLight tailLights = new VehicleLight();

		private bool _hazardLightsOn;

		private bool _leftBlinkersOn;

		private bool _rightBlinkersOn;

		private float _leftBlinkerTurnOnTime;

		private float _rightBlinkerTurnOnTime;

		public bool LeftBlinkerState => (int)((vehicleController.realtimeSinceStartup - _leftBlinkerTurnOnTime) * 2f) % 2 == 0;

		public bool RightBlinkerState => (int)((vehicleController.realtimeSinceStartup - _rightBlinkerTurnOnTime) * 2f) % 2 == 0;

		public override void VC_Update()
		{
			base.VC_Update();
			if (vehicleController.MultiplayerIsRemote)
			{
				return;
			}
			if (brakeLights != null)
			{
				if (vehicleController.brakes.IsBraking)
				{
					brakeLights.TurnOn();
				}
				else
				{
					bool num = tailLights.On;
					brakeLights.TurnOff();
					if (num)
					{
						tailLights.TurnOff();
						tailLights.TurnOn();
					}
				}
			}
			if (reverseLights != null)
			{
				if (vehicleController.powertrain.transmission.Gear < 0)
				{
					reverseLights.TurnOn();
				}
				else
				{
					reverseLights.TurnOff();
				}
			}
			if (lowBeamLights != null && vehicleController.input.states.lowBeamLights)
			{
				lowBeamLights.Toggle();
				if (lowBeamLights.On)
				{
					tailLights.TurnOn();
				}
				else
				{
					if (brakeLights != null)
					{
						bool num2 = brakeLights.On;
						tailLights.TurnOff();
						if (num2)
						{
							brakeLights.TurnOff();
							brakeLights.TurnOn();
						}
					}
					else
					{
						tailLights.TurnOff();
					}
					if (highBeamLights != null)
					{
						highBeamLights.TurnOff();
					}
				}
				vehicleController.input.states.lowBeamLights = false;
			}
			if (highBeamLights != null && lowBeamLights != null && vehicleController.input.states.highBeamLights)
			{
				bool flag = highBeamLights.On;
				highBeamLights.Toggle();
				if (highBeamLights.On && !flag)
				{
					lowBeamLights.TurnOn();
					tailLights.TurnOn();
				}
				else if (!highBeamLights.On && !lowBeamLights.On)
				{
					tailLights.TurnOff();
				}
				vehicleController.input.states.highBeamLights = false;
			}
			if (leftBlinkers != null && rightBlinkers != null)
			{
				if (vehicleController.input.states.hazardLights)
				{
					_hazardLightsOn = !_hazardLightsOn;
					_leftBlinkersOn = (_rightBlinkersOn = _hazardLightsOn);
					if (_hazardLightsOn)
					{
						_leftBlinkerTurnOnTime = (_rightBlinkerTurnOnTime = vehicleController.realtimeSinceStartup);
					}
					else
					{
						_leftBlinkersOn = false;
						_rightBlinkersOn = false;
					}
					vehicleController.input.states.hazardLights = false;
				}
				if (!_hazardLightsOn)
				{
					if (vehicleController.input.states.leftBlinker)
					{
						_leftBlinkersOn = !_leftBlinkersOn;
						if (_leftBlinkersOn)
						{
							_leftBlinkerTurnOnTime = vehicleController.realtimeSinceStartup;
							_rightBlinkersOn = false;
						}
						vehicleController.input.states.leftBlinker = false;
					}
					if (vehicleController.input.states.rightBlinker)
					{
						_rightBlinkersOn = !_rightBlinkersOn;
						if (_rightBlinkersOn)
						{
							_rightBlinkerTurnOnTime = vehicleController.realtimeSinceStartup;
							_leftBlinkersOn = false;
						}
						vehicleController.input.states.rightBlinker = false;
					}
				}
				else
				{
					vehicleController.input.states.leftBlinker = false;
					vehicleController.input.states.rightBlinker = false;
				}
				leftBlinkers.SetState(_leftBlinkersOn && LeftBlinkerState);
				rightBlinkers.SetState(_rightBlinkersOn && RightBlinkerState);
			}
			if (extraLights != null && vehicleController.input.states.extraLights)
			{
				extraLights.Toggle();
				vehicleController.input.states.extraLights = false;
			}
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				TurnOffAllLights();
				return true;
			}
			return false;
		}

		public int GetIntState()
		{
			int target = 0;
			SetBit(ref target, brakeLights.On, 0);
			SetBit(ref target, tailLights.On, 1);
			SetBit(ref target, reverseLights.On, 2);
			SetBit(ref target, lowBeamLights.On, 3);
			SetBit(ref target, highBeamLights.On, 4);
			SetBit(ref target, leftBlinkers.On, 5);
			SetBit(ref target, rightBlinkers.On, 6);
			SetBit(ref target, extraLights.On, 7);
			return target;
		}

		public void SetStateFromInt(int intState)
		{
			brakeLights.SetState(GetBit(intState, 0));
			tailLights.SetState(GetBit(intState, 1));
			reverseLights.SetState(GetBit(intState, 2));
			lowBeamLights.SetState(GetBit(intState, 3));
			highBeamLights.SetState(GetBit(intState, 4));
			leftBlinkers.SetState(GetBit(intState, 5));
			rightBlinkers.SetState(GetBit(intState, 6));
			extraLights.SetState(GetBit(intState, 7));
		}

		private void SetBit(ref int target, bool value, int position)
		{
			if (value)
			{
				target |= 1 << position;
			}
			else
			{
				target &= ~(1 << position);
			}
		}

		private bool GetBit(int source, int position)
		{
			return ((source >> position) & 1) == 1;
		}

		public void TurnOffAllLights()
		{
			brakeLights.TurnOff();
			lowBeamLights.TurnOff();
			tailLights.TurnOff();
			reverseLights.TurnOff();
			highBeamLights.TurnOff();
			leftBlinkers.TurnOff();
			rightBlinkers.TurnOff();
			extraLights.TurnOff();
		}
	}
}
