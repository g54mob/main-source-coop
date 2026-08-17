using System;
using Rewired.Drivers.Interfaces;
using Rewired.Interfaces;
using Rewired.Utils;
using Rewired.Utils.Classes.Utility;
using UnityEngine;

namespace Rewired.ControllerExtensions
{
	public sealed class DualSenseExtension : Controller.Extension, IControllerVibrator, IDualShock4Extension
	{
		private class rVWncrbWZThGISFZtXCAuqdKkQAA : IControllerExtensionSource
		{
			public readonly IDriver_DualSense UVnyPZOPqXebrkDwGURsMwQhOdVT;

			public readonly bool BMKmLgTjByiExXMvsepaerGfBiXH;

			public readonly int IvQuNSqDypWdalUGYcleYiqEVPmd;

			public rVWncrbWZThGISFZtXCAuqdKkQAA(IDriver_DualSense P_0, bool P_1, int P_2)
			{
				UVnyPZOPqXebrkDwGURsMwQhOdVT = P_0;
				BMKmLgTjByiExXMvsepaerGfBiXH = P_1;
				IvQuNSqDypWdalUGYcleYiqEVPmd = P_2;
			}
		}

		private rVWncrbWZThGISFZtXCAuqdKkQAA eyyVWBZaxsCJCQMmwhCWcagfYfzWA;

		private bool OfwbAYIoZdGfvbuKLWzFcjGDPogfc;

		private TimerAbs[] IfVBCqaFujeIzJMUNQBcBKnduRXLA;

		private Joystick chcOLBMXAtWUArFXncgBETOGbAnFb => GetController<Joystick>();

		public int vibrationMotorCount
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return 0;
				}
				if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc)
				{
					return 0;
				}
				return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.IvQuNSqDypWdalUGYcleYiqEVPmd;
			}
		}

		public float lightColorRed
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return 0f;
				}
				if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled)
				{
					return 0f;
				}
				return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.LightColorR;
			}
			set
			{
				if (OfwbAYIoZdGfvbuKLWzFcjGDPogfc)
				{
					eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.LightColorR = value;
				}
			}
		}

		public float lightColorGreen
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return 0f;
				}
				if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled)
				{
					return 0f;
				}
				return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.LightColorG;
			}
			set
			{
				if (OfwbAYIoZdGfvbuKLWzFcjGDPogfc)
				{
					eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.LightColorG = value;
				}
			}
		}

		public float lightColorBlue
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return 0f;
				}
				if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled)
				{
					return 0f;
				}
				return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.LightColorB;
			}
			set
			{
				if (OfwbAYIoZdGfvbuKLWzFcjGDPogfc)
				{
					eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.LightColorB = value;
				}
			}
		}

		public DualSenseMicrophoneLightMode microphoneLightMode
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return DualSenseMicrophoneLightMode.Off;
				}
				if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled)
				{
					return DualSenseMicrophoneLightMode.Off;
				}
				return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.microphoneLightMode;
			}
			set
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
				}
				else if (OfwbAYIoZdGfvbuKLWzFcjGDPogfc && base.enabled)
				{
					eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.microphoneLightMode = value;
				}
			}
		}

		public DualSenseOtherLightBrightness otherLightBrightness
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return DualSenseOtherLightBrightness.High;
				}
				if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled)
				{
					return DualSenseOtherLightBrightness.High;
				}
				return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.otherLightBrightness;
			}
			set
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
				}
				else if (OfwbAYIoZdGfvbuKLWzFcjGDPogfc && base.enabled)
				{
					eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.otherLightBrightness = value;
				}
			}
		}

		public DualSensePlayerLightFlags playerLights
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return DualSensePlayerLightFlags.None;
				}
				if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled)
				{
					return DualSensePlayerLightFlags.None;
				}
				return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.playerLights;
			}
			set
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
				}
				else if (OfwbAYIoZdGfvbuKLWzFcjGDPogfc && base.enabled)
				{
					eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.playerLights = value;
				}
			}
		}

		public int maxTouches
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return 0;
				}
				if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc)
				{
					return 0;
				}
				return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.MaxTouches;
			}
		}

		public int touchCount
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return 0;
				}
				return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.GetTouchCount();
			}
		}

		public float batteryLevel
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return 0f;
				}
				if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc)
				{
					return 0f;
				}
				return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.BatteryLevel;
			}
		}

		public bool batteryCharging
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return false;
				}
				if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc)
				{
					return false;
				}
				return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.BatteryCharging;
			}
		}

		internal DualSenseExtension(IDriver_DualSense P_0)
			: base(new rVWncrbWZThGISFZtXCAuqdKkQAA(P_0, P_0.VibrationMotorCount > 0, P_0.VibrationMotorCount))
		{
			IfVBCqaFujeIzJMUNQBcBKnduRXLA = new TimerAbs[P_0.VibrationMotorCount];
			ArrayTools.Populate(IfVBCqaFujeIzJMUNQBcBKnduRXLA, 0, IfVBCqaFujeIzJMUNQBcBKnduRXLA.Length);
		}

		private DualSenseExtension(DualSenseExtension P_0)
			: base(P_0)
		{
			try
			{
				IfVBCqaFujeIzJMUNQBcBKnduRXLA = new TimerAbs[P_0.vibrationMotorCount];
			}
			catch
			{
				IfVBCqaFujeIzJMUNQBcBKnduRXLA = new TimerAbs[0];
			}
			ArrayTools.Populate(IfVBCqaFujeIzJMUNQBcBKnduRXLA, 0, IfVBCqaFujeIzJMUNQBcBKnduRXLA.Length);
		}

		public void SetVibration(int motorIndex, float motorLevel)
		{
			SetVibration(motorIndex, motorLevel, 0f, stopOtherMotors: false);
		}

		public void SetVibration(int motorIndex, float motorLevel, float duration)
		{
			SetVibration(motorIndex, motorLevel, duration, stopOtherMotors: false);
		}

		public void SetVibration(int motorIndex, float motorLevel, bool stopOtherMotors)
		{
			SetVibration(motorIndex, motorLevel, 0f, stopOtherMotors);
		}

		public void SetVibration(int motorIndex, float motorLevel, float duration, bool stopOtherMotors)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (OfwbAYIoZdGfvbuKLWzFcjGDPogfc && base.enabled && motorIndex >= 0 && motorIndex < eyyVWBZaxsCJCQMmwhCWcagfYfzWA.IvQuNSqDypWdalUGYcleYiqEVPmd)
			{
				SetVibration(motorIndex switch
				{
					0 => DualShock4MotorType.LeftMotor, 
					1 => DualShock4MotorType.RightMotor, 
					_ => throw new NotImplementedException(), 
				}, motorLevel, duration, stopOtherMotors);
			}
		}

		public float GetVibration(int motorIndex)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return 0f;
			}
			if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled)
			{
				return 0f;
			}
			if (!eyyVWBZaxsCJCQMmwhCWcagfYfzWA.BMKmLgTjByiExXMvsepaerGfBiXH)
			{
				return 0f;
			}
			return motorIndex switch
			{
				0 => eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.LeftMotor, 
				1 => eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.RightMotor, 
				_ => 0f, 
			};
		}

		public void StopVibration()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (OfwbAYIoZdGfvbuKLWzFcjGDPogfc && base.enabled && eyyVWBZaxsCJCQMmwhCWcagfYfzWA.BMKmLgTjByiExXMvsepaerGfBiXH)
			{
				for (int i = 0; i < eyyVWBZaxsCJCQMmwhCWcagfYfzWA.IvQuNSqDypWdalUGYcleYiqEVPmd; i++)
				{
					IfVBCqaFujeIzJMUNQBcBKnduRXLA[i].Clear();
				}
				eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.StopVibration();
			}
		}

		public float GetVibration(DualShock4MotorType motor)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return 0f;
			}
			if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled)
			{
				return 0f;
			}
			if (!eyyVWBZaxsCJCQMmwhCWcagfYfzWA.BMKmLgTjByiExXMvsepaerGfBiXH)
			{
				return 0f;
			}
			return (int)motor switch
			{
				0 => eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.LeftMotor, 
				1 => eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.RightMotor, 
				_ => throw new NotImplementedException(), 
			};
		}

		public void SetVibration(DualShock4MotorType motor, float motorLevel)
		{
			SetVibration(motor, motorLevel, 0f, stopOtherMotors: false);
		}

		public void SetVibration(DualShock4MotorType motor, float motorLevel, float duration)
		{
			SetVibration(motor, motorLevel, duration, stopOtherMotors: false);
		}

		public void SetVibration(DualShock4MotorType motor, float motorLevel, bool stopOtherMotors)
		{
			SetVibration(motor, motorLevel, 0f, stopOtherMotors);
		}

		public void SetVibration(DualShock4MotorType motor, float motorLevel, float duration, bool stopOtherMotors)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else
			{
				if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled || !eyyVWBZaxsCJCQMmwhCWcagfYfzWA.BMKmLgTjByiExXMvsepaerGfBiXH)
				{
					return;
				}
				if (stopOtherMotors)
				{
					for (int i = 0; i < eyyVWBZaxsCJCQMmwhCWcagfYfzWA.IvQuNSqDypWdalUGYcleYiqEVPmd; i++)
					{
						IfVBCqaFujeIzJMUNQBcBKnduRXLA[i].Clear();
					}
					eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.StopVibration();
				}
				motorLevel = MathTools.Clamp01(motorLevel);
				switch ((int)motor)
				{
				case 0:
					eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.LeftMotor = motorLevel;
					break;
				case 1:
					eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.RightMotor = motorLevel;
					break;
				default:
					throw new NotImplementedException();
				}
				uLJgIUZWcYRvIJfnwylMTtjINIBA(motor, motorLevel, duration);
			}
		}

		public void SetVibration(float leftMotorLevel, float rightMotorLevel)
		{
			SetVibration(leftMotorLevel, rightMotorLevel, 0f, 0f);
		}

		public void SetVibration(float leftMotorLevel, float rightMotorLevel, float leftMotorDuration, float rightMotorDuration)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (OfwbAYIoZdGfvbuKLWzFcjGDPogfc && base.enabled && eyyVWBZaxsCJCQMmwhCWcagfYfzWA.BMKmLgTjByiExXMvsepaerGfBiXH)
			{
				eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.LeftMotor = MathTools.Clamp01(leftMotorLevel);
				eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.RightMotor = MathTools.Clamp01(rightMotorLevel);
				uLJgIUZWcYRvIJfnwylMTtjINIBA(DualShock4MotorType.LeftMotor, leftMotorLevel, leftMotorDuration);
				uLJgIUZWcYRvIJfnwylMTtjINIBA(DualShock4MotorType.RightMotor, rightMotorLevel, rightMotorDuration);
			}
		}

		public Color GetLightColor()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return default(Color);
			}
			if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc)
			{
				return default(Color);
			}
			return new Color(eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.LightColorR, eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.LightColorG, eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.LightColorB, 1f);
		}

		public void SetLightColor(Color color)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (OfwbAYIoZdGfvbuKLWzFcjGDPogfc && base.enabled)
			{
				eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.LightColorR = color.r * color.a;
				eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.LightColorG = color.g * color.a;
				eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.LightColorB = color.b * color.a;
			}
		}

		public void SetLightColor(float red, float green, float blue)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else
			{
				SetLightColor(red, green, blue, 1f);
			}
		}

		public void SetLightColor(float red, float green, float blue, float intensity)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (OfwbAYIoZdGfvbuKLWzFcjGDPogfc && base.enabled)
			{
				eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.LightColorR = red * intensity;
				eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.LightColorG = green * intensity;
				eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.LightColorB = blue * intensity;
			}
		}

		public Vector3 GetAccelerometerValueRaw()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Vector3.zero;
			}
			if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return Vector3.zero;
			}
			return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.AccelerometerValueRaw;
		}

		public Vector3 GetAccelerometerValue()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Vector3.zero;
			}
			if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return Vector3.zero;
			}
			return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.AccelerometerValue;
		}

		public Vector3 GetLastGyroscopeValueRaw()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Vector3.zero;
			}
			if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return Vector3.zero;
			}
			return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.LastGyroscopeValueRaw;
		}

		public Vector3 GetLastGyroscopeValue()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Vector3.zero;
			}
			if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return Vector3.zero;
			}
			return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.LastGyroscopeValue;
		}

		public Vector3 GetGyroscopeValueRaw()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Vector3.zero;
			}
			if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return Vector3.zero;
			}
			return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.GyroscopeValueRaw;
		}

		public Vector3 GetGyroscopeValue()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Vector3.zero;
			}
			if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return Vector3.zero;
			}
			return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.GyroscopeValue;
		}

		public Quaternion GetOrientation()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Quaternion.identity;
			}
			if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return default(Quaternion);
			}
			return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.Orientation;
		}

		public void ResetOrientation()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (OfwbAYIoZdGfvbuKLWzFcjGDPogfc)
			{
				eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.ResetOrientation();
			}
		}

		public int GetTouchId(int index)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return -1;
			}
			if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return -1;
			}
			return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.GetTouchIdAtIndex(index);
		}

		public bool GetTouchPosition(int index, out Vector2 position)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				position = Vector2.zero;
				return false;
			}
			if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				position = Vector2.zero;
				return false;
			}
			return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.GetTouchPositionByIndex(index, out position);
		}

		public bool GetTouchPositionByTouchId(int touchId, out Vector2 position)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				position = Vector2.zero;
				return false;
			}
			if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				position = Vector2.zero;
				return false;
			}
			return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.GetTouchPositionByTouchId(touchId, out position);
		}

		public bool GetTouchPositionAbsolute(int index, out Vector2 position)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				position = Vector2.zero;
				return false;
			}
			if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				position = Vector2.zero;
				return false;
			}
			int positionX;
			int positionY;
			bool touchPositionAbsoluteByIndex = eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.GetTouchPositionAbsoluteByIndex(index, out positionX, out positionY);
			position = new Vector2(positionX, positionY);
			return touchPositionAbsoluteByIndex;
		}

		public bool GetTouchPositionAbsoluteByTouchId(int touchId, out Vector2 position)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				position = Vector2.zero;
				return false;
			}
			if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				position = Vector2.zero;
				return false;
			}
			int positionX;
			int positionY;
			bool touchPositionAbsoluteByTouchId = eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.GetTouchPositionAbsoluteByTouchId(touchId, out positionX, out positionY);
			position = new Vector2(positionX, positionY);
			return touchPositionAbsoluteByTouchId;
		}

		public bool IsTouching(int index)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return false;
			}
			if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return false;
			}
			return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.IsTouchingAtIndex(index);
		}

		public bool IsTouchingByTouchId(int touchId)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return false;
			}
			if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return false;
			}
			return eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT.IsTouchingAtTouchId(touchId);
		}

		Vector3 IDualShock4Extension.GetGyroscopeValue()
		{
			return GetGyroscopeValue();
		}

		Vector3 IDualShock4Extension.GetGyroscopeValueRaw()
		{
			return GetGyroscopeValueRaw();
		}

		internal void PwSVyxiNOmkyuuXUHWyuymUyabog(UpdateLoopType P_0)
		{
			if (OfwbAYIoZdGfvbuKLWzFcjGDPogfc && base.enabled)
			{
				RkjJWtCPffqNHKYZiSpumVKUPDFy();
			}
		}

		internal void EJfWkXMMYTRfEHNEnCEaDzBnFICoA(IControllerExtensionSource P_0)
		{
			eyyVWBZaxsCJCQMmwhCWcagfYfzWA = P_0 as rVWncrbWZThGISFZtXCAuqdKkQAA;
			OfwbAYIoZdGfvbuKLWzFcjGDPogfc = eyyVWBZaxsCJCQMmwhCWcagfYfzWA != null && eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UVnyPZOPqXebrkDwGURsMwQhOdVT != null;
		}

		internal Controller.Extension vpHGhDFXfrFPnKJBKRFCgbudEJvBB()
		{
			return new DualSenseExtension(this);
		}

		private void RkjJWtCPffqNHKYZiSpumVKUPDFy()
		{
			if (!OfwbAYIoZdGfvbuKLWzFcjGDPogfc || !eyyVWBZaxsCJCQMmwhCWcagfYfzWA.BMKmLgTjByiExXMvsepaerGfBiXH)
			{
				return;
			}
			for (int i = 0; i < eyyVWBZaxsCJCQMmwhCWcagfYfzWA.IvQuNSqDypWdalUGYcleYiqEVPmd; i++)
			{
				if (IfVBCqaFujeIzJMUNQBcBKnduRXLA[i].Update())
				{
					SetVibration(i, 0f, stopOtherMotors: false);
				}
			}
		}

		private void uLJgIUZWcYRvIJfnwylMTtjINIBA(DualShock4MotorType P_0, float P_1, float P_2)
		{
			int num = P_0 switch
			{
				DualShock4MotorType.LeftMotor => 0, 
				DualShock4MotorType.RightMotor => 1, 
				_ => throw new NotImplementedException(), 
			};
			if (P_1 <= 0f || P_2 <= 0f)
			{
				IfVBCqaFujeIzJMUNQBcBKnduRXLA[num].Clear();
			}
			else
			{
				IfVBCqaFujeIzJMUNQBcBKnduRXLA[num].Start(P_2);
			}
		}
	}
}
