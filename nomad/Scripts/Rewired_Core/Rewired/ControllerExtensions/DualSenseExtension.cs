using System;
using Rewired.HID.Drivers;
using Rewired.Interfaces;
using Rewired.Utils;
using Rewired.Utils.Classes.Utility;
using UnityEngine;

namespace Rewired.ControllerExtensions
{
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	public sealed class DualSenseExtension : Controller.Extension, IControllerVibrator, IDualShock4Extension, IDualSenseExtension, IHIDControllerExtension
	{
		private class HPzBSqILjvqYJzoolqHMMPnPmwRgA : IControllerExtensionSource
		{
			public readonly IDriver_DualSense rqZmpCcxwrqvspiWnSltfsNmMtXx;

			public readonly bool OAyWiMzLBXhunVVUBdVNvexGAXGFA;

			public readonly int SpFduwBbHQtgWRDgRrnmCBCXMILkA;

			public HPzBSqILjvqYJzoolqHMMPnPmwRgA(IDriver_DualSense P_0, bool P_1, int P_2)
			{
				rqZmpCcxwrqvspiWnSltfsNmMtXx = P_0;
				OAyWiMzLBXhunVVUBdVNvexGAXGFA = P_1;
				SpFduwBbHQtgWRDgRrnmCBCXMILkA = P_2;
			}
		}

		private HPzBSqILjvqYJzoolqHMMPnPmwRgA ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA;

		private bool ZuBtNPtLZCufQZglNIFxkEekWjlT;

		private TimerAbs[] YreTeaWnycCCYJesbyvWISqrAJRx;

		private Joystick joystick => GetController<Joystick>();

		int IControllerVibrator.vibrationMotorCount
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return 0;
				}
				if (!ZuBtNPtLZCufQZglNIFxkEekWjlT)
				{
					return 0;
				}
				return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.SpFduwBbHQtgWRDgRrnmCBCXMILkA;
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
				if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled)
				{
					return 0f;
				}
				return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.LightColorR;
			}
			set
			{
				if (ZuBtNPtLZCufQZglNIFxkEekWjlT)
				{
					ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.LightColorR = value;
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
				if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled)
				{
					return 0f;
				}
				return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.LightColorG;
			}
			set
			{
				if (ZuBtNPtLZCufQZglNIFxkEekWjlT)
				{
					ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.LightColorG = value;
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
				if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled)
				{
					return 0f;
				}
				return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.LightColorB;
			}
			set
			{
				if (ZuBtNPtLZCufQZglNIFxkEekWjlT)
				{
					ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.LightColorB = value;
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
				if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled)
				{
					return DualSenseMicrophoneLightMode.Off;
				}
				return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.microphoneLightMode;
			}
			set
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
				}
				else if (ZuBtNPtLZCufQZglNIFxkEekWjlT && base.enabled)
				{
					ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.microphoneLightMode = value;
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
				if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled)
				{
					return DualSenseOtherLightBrightness.High;
				}
				return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.otherLightBrightness;
			}
			set
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
				}
				else if (ZuBtNPtLZCufQZglNIFxkEekWjlT && base.enabled)
				{
					ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.otherLightBrightness = value;
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
				if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled)
				{
					return DualSensePlayerLightFlags.None;
				}
				return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.playerLights;
			}
			set
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
				}
				else if (ZuBtNPtLZCufQZglNIFxkEekWjlT && base.enabled)
				{
					ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.playerLights = value;
				}
			}
		}

		int IDualShock4Extension.maxTouches
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return 0;
				}
				if (!ZuBtNPtLZCufQZglNIFxkEekWjlT)
				{
					return 0;
				}
				return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.MaxTouches;
			}
		}

		int IDualShock4Extension.touchCount
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return 0;
				}
				return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.GetTouchCount();
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
				if (!ZuBtNPtLZCufQZglNIFxkEekWjlT)
				{
					return 0f;
				}
				return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.BatteryLevel;
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
				if (!ZuBtNPtLZCufQZglNIFxkEekWjlT)
				{
					return false;
				}
				return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.BatteryCharging;
			}
		}

		ushort IHIDControllerExtension.vendorId
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return 0;
				}
				return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.vendorId;
			}
		}

		ushort IHIDControllerExtension.productId
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return 0;
				}
				return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.productId;
			}
		}

		string IHIDControllerExtension.productName
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return string.Empty;
				}
				return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.productName;
			}
		}

		string IHIDControllerExtension.manufacturer
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return string.Empty;
				}
				return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.manufacturer;
			}
		}

		ushort IHIDControllerExtension.usagePage
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return 0;
				}
				return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.usagePage;
			}
		}

		ushort IHIDControllerExtension.usage
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return 0;
				}
				return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.usage;
			}
		}

		internal DualSenseExtension(IDriver_DualSense P_0)
			: base(new HPzBSqILjvqYJzoolqHMMPnPmwRgA(P_0, P_0.VibrationMotorCount > 0, P_0.VibrationMotorCount))
		{
			YreTeaWnycCCYJesbyvWISqrAJRx = new TimerAbs[P_0.VibrationMotorCount];
			ArrayTools.Populate(YreTeaWnycCCYJesbyvWISqrAJRx, 0, YreTeaWnycCCYJesbyvWISqrAJRx.Length);
		}

		private DualSenseExtension(DualSenseExtension P_0)
			: base(P_0)
		{
			try
			{
				YreTeaWnycCCYJesbyvWISqrAJRx = new TimerAbs[P_0.Rewired_002EInterfaces_002EIControllerVibrator_002EvibrationMotorCount];
			}
			catch
			{
				YreTeaWnycCCYJesbyvWISqrAJRx = new TimerAbs[0];
			}
			ArrayTools.Populate(YreTeaWnycCCYJesbyvWISqrAJRx, 0, YreTeaWnycCCYJesbyvWISqrAJRx.Length);
		}

		public void SetVibration(int motorIndex, float motorLevel)
		{
			SetVibration(motorIndex, motorLevel, 0f, stopOtherMotors: false);
		}

		void IControllerVibrator.SetVibration(int motorIndex, float motorLevel)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetVibration
			this.SetVibration(motorIndex, motorLevel);
		}

		public void SetVibration(int motorIndex, float motorLevel, float duration)
		{
			SetVibration(motorIndex, motorLevel, duration, stopOtherMotors: false);
		}

		void IControllerVibrator.SetVibration(int motorIndex, float motorLevel, float duration)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetVibration
			this.SetVibration(motorIndex, motorLevel, duration);
		}

		public void SetVibration(int motorIndex, float motorLevel, bool stopOtherMotors)
		{
			SetVibration(motorIndex, motorLevel, 0f, stopOtherMotors);
		}

		void IControllerVibrator.SetVibration(int motorIndex, float motorLevel, bool stopOtherMotors)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetVibration
			this.SetVibration(motorIndex, motorLevel, stopOtherMotors);
		}

		public void SetVibration(int motorIndex, float motorLevel, float duration, bool stopOtherMotors)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (ZuBtNPtLZCufQZglNIFxkEekWjlT && base.enabled && motorIndex >= 0 && motorIndex < ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.SpFduwBbHQtgWRDgRrnmCBCXMILkA)
			{
				SetVibration(motorIndex switch
				{
					0 => DualShock4MotorType.LeftMotor, 
					1 => DualShock4MotorType.RightMotor, 
					_ => throw new NotImplementedException(), 
				}, motorLevel, duration, stopOtherMotors);
			}
		}

		void IControllerVibrator.SetVibration(int motorIndex, float motorLevel, float duration, bool stopOtherMotors)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetVibration
			this.SetVibration(motorIndex, motorLevel, duration, stopOtherMotors);
		}

		public float GetVibration(int motorIndex)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return 0f;
			}
			if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled)
			{
				return 0f;
			}
			if (!ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.OAyWiMzLBXhunVVUBdVNvexGAXGFA)
			{
				return 0f;
			}
			return motorIndex switch
			{
				0 => ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.LeftMotor, 
				1 => ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.RightMotor, 
				_ => 0f, 
			};
		}

		float IControllerVibrator.GetVibration(int motorIndex)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetVibration
			return this.GetVibration(motorIndex);
		}

		public void StopVibration()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (ZuBtNPtLZCufQZglNIFxkEekWjlT && base.enabled && ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.OAyWiMzLBXhunVVUBdVNvexGAXGFA)
			{
				for (int i = 0; i < ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.SpFduwBbHQtgWRDgRrnmCBCXMILkA; i++)
				{
					YreTeaWnycCCYJesbyvWISqrAJRx[i].Clear();
				}
				ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.StopVibration();
			}
		}

		void IControllerVibrator.StopVibration()
		{
			//ILSpy generated this explicit interface implementation from .override directive in StopVibration
			this.StopVibration();
		}

		public DualSenseVibrationMode GetVibrationMode()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return DualSenseVibrationMode.Compatible2;
			}
			return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.vibrationMode;
		}

		public void SetVibrationMode(DualSenseVibrationMode mode)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else
			{
				ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.vibrationMode = mode;
			}
		}

		public float GetVibration(DualShock4MotorType motor)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return 0f;
			}
			if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled)
			{
				return 0f;
			}
			if (!ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.OAyWiMzLBXhunVVUBdVNvexGAXGFA)
			{
				return 0f;
			}
			return (int)motor switch
			{
				0 => ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.LeftMotor, 
				1 => ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.RightMotor, 
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
				if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled || !ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.OAyWiMzLBXhunVVUBdVNvexGAXGFA)
				{
					return;
				}
				if (stopOtherMotors)
				{
					for (int i = 0; i < ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.SpFduwBbHQtgWRDgRrnmCBCXMILkA; i++)
					{
						YreTeaWnycCCYJesbyvWISqrAJRx[i].Clear();
					}
					ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.StopVibration();
				}
				motorLevel = MathTools.Clamp01(motorLevel);
				switch ((int)motor)
				{
				case 0:
					ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.LeftMotor = motorLevel;
					break;
				case 1:
					ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.RightMotor = motorLevel;
					break;
				default:
					throw new NotImplementedException();
				}
				bRhnaVvuIwlKsmlfhIrqpgCHGQnhA(motor, motorLevel, duration);
			}
		}

		public void SetVibration(float leftMotorLevel, float rightMotorLevel)
		{
			SetVibration(leftMotorLevel, rightMotorLevel, 0f, 0f);
		}

		void IDualShock4Extension.SetVibration(float leftMotorLevel, float rightMotorLevel)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetVibration
			this.SetVibration(leftMotorLevel, rightMotorLevel);
		}

		public void SetVibration(float leftMotorLevel, float rightMotorLevel, float leftMotorDuration, float rightMotorDuration)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (ZuBtNPtLZCufQZglNIFxkEekWjlT && base.enabled && ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.OAyWiMzLBXhunVVUBdVNvexGAXGFA)
			{
				ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.LeftMotor = MathTools.Clamp01(leftMotorLevel);
				ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.RightMotor = MathTools.Clamp01(rightMotorLevel);
				bRhnaVvuIwlKsmlfhIrqpgCHGQnhA(DualShock4MotorType.LeftMotor, leftMotorLevel, leftMotorDuration);
				bRhnaVvuIwlKsmlfhIrqpgCHGQnhA(DualShock4MotorType.RightMotor, rightMotorLevel, rightMotorDuration);
			}
		}

		void IDualShock4Extension.SetVibration(float leftMotorLevel, float rightMotorLevel, float leftMotorDuration, float rightMotorDuration)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetVibration
			this.SetVibration(leftMotorLevel, rightMotorLevel, leftMotorDuration, rightMotorDuration);
		}

		public Color GetLightColor()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return default(Color);
			}
			if (!ZuBtNPtLZCufQZglNIFxkEekWjlT)
			{
				return default(Color);
			}
			return new Color(ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.LightColorR, ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.LightColorG, ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.LightColorB, 1f);
		}

		public void SetLightColor(Color color)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (ZuBtNPtLZCufQZglNIFxkEekWjlT && base.enabled)
			{
				ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.LightColorR = color.r * color.a;
				ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.LightColorG = color.g * color.a;
				ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.LightColorB = color.b * color.a;
			}
		}

		void IDualShock4Extension.SetLightColor(Color color)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetLightColor
			this.SetLightColor(color);
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

		void IDualShock4Extension.SetLightColor(float red, float green, float blue)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetLightColor
			this.SetLightColor(red, green, blue);
		}

		public void SetLightColor(float red, float green, float blue, float intensity)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (ZuBtNPtLZCufQZglNIFxkEekWjlT && base.enabled)
			{
				ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.LightColorR = red * intensity;
				ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.LightColorG = green * intensity;
				ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.LightColorB = blue * intensity;
			}
		}

		void IDualShock4Extension.SetLightColor(float red, float green, float blue, float intensity)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetLightColor
			this.SetLightColor(red, green, blue, intensity);
		}

		public Vector3 GetAccelerometerValueRaw()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Vector3.zero;
			}
			if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return Vector3.zero;
			}
			return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.AccelerometerValueRaw;
		}

		Vector3 IDualShock4Extension.GetAccelerometerValueRaw()
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetAccelerometerValueRaw
			return this.GetAccelerometerValueRaw();
		}

		public Vector3 GetAccelerometerValue()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Vector3.zero;
			}
			if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return Vector3.zero;
			}
			return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.AccelerometerValue;
		}

		Vector3 IDualShock4Extension.GetAccelerometerValue()
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetAccelerometerValue
			return this.GetAccelerometerValue();
		}

		public Vector3 GetLastGyroscopeValueRaw()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Vector3.zero;
			}
			if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return Vector3.zero;
			}
			return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.LastGyroscopeValueRaw;
		}

		public Vector3 GetLastGyroscopeValue()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Vector3.zero;
			}
			if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return Vector3.zero;
			}
			return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.LastGyroscopeValue;
		}

		public Vector3 GetGyroscopeValueRaw()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Vector3.zero;
			}
			if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return Vector3.zero;
			}
			return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.GyroscopeValueRaw;
		}

		public Vector3 GetGyroscopeValue()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Vector3.zero;
			}
			if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return Vector3.zero;
			}
			return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.GyroscopeValue;
		}

		public Quaternion GetOrientation()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Quaternion.identity;
			}
			if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return default(Quaternion);
			}
			return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.Orientation;
		}

		Quaternion IDualShock4Extension.GetOrientation()
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetOrientation
			return this.GetOrientation();
		}

		public void ResetOrientation()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (ZuBtNPtLZCufQZglNIFxkEekWjlT)
			{
				ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.ResetOrientation();
			}
		}

		void IDualShock4Extension.ResetOrientation()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ResetOrientation
			this.ResetOrientation();
		}

		public int GetTouchId(int index)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return -1;
			}
			if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return -1;
			}
			return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.GetTouchIdAtIndex(index);
		}

		int IDualShock4Extension.GetTouchId(int index)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetTouchId
			return this.GetTouchId(index);
		}

		public bool GetTouchPosition(int index, out Vector2 position)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				position = Vector2.zero;
				return false;
			}
			if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				position = Vector2.zero;
				return false;
			}
			return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.GetTouchPositionByIndex(index, out position);
		}

		bool IDualShock4Extension.GetTouchPosition(int index, out Vector2 position)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetTouchPosition
			return this.GetTouchPosition(index, out position);
		}

		public bool GetTouchPositionByTouchId(int touchId, out Vector2 position)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				position = Vector2.zero;
				return false;
			}
			if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				position = Vector2.zero;
				return false;
			}
			return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.GetTouchPositionByTouchId(touchId, out position);
		}

		bool IDualShock4Extension.GetTouchPositionByTouchId(int touchId, out Vector2 position)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetTouchPositionByTouchId
			return this.GetTouchPositionByTouchId(touchId, out position);
		}

		public bool GetTouchPositionAbsolute(int index, out Vector2 position)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				position = Vector2.zero;
				return false;
			}
			if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				position = Vector2.zero;
				return false;
			}
			int positionX;
			int positionY;
			bool touchPositionAbsoluteByIndex = ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.GetTouchPositionAbsoluteByIndex(index, out positionX, out positionY);
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
			if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				position = Vector2.zero;
				return false;
			}
			int positionX;
			int positionY;
			bool touchPositionAbsoluteByTouchId = ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.GetTouchPositionAbsoluteByTouchId(touchId, out positionX, out positionY);
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
			if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return false;
			}
			return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.IsTouchingAtIndex(index);
		}

		bool IDualShock4Extension.IsTouching(int index)
		{
			//ILSpy generated this explicit interface implementation from .override directive in IsTouching
			return this.IsTouching(index);
		}

		public bool IsTouchingByTouchId(int touchId)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return false;
			}
			if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return false;
			}
			return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.IsTouchingAtTouchId(touchId);
		}

		bool IDualShock4Extension.IsTouchingByTouchId(int touchId)
		{
			//ILSpy generated this explicit interface implementation from .override directive in IsTouchingByTouchId
			return this.IsTouchingByTouchId(touchId);
		}

		public bool SetTriggerEffect(DualSenseTriggerType trigger, IDualSenseTriggerEffect effect)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return false;
			}
			return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.SetTriggerEffect(trigger, effect);
		}

		bool IDualSenseExtension.SetTriggerEffect(DualSenseTriggerType trigger, IDualSenseTriggerEffect effect)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetTriggerEffect
			return this.SetTriggerEffect(trigger, effect);
		}

		public DualSenseTriggerEffectStates GetTriggerEffectStates()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return default(DualSenseTriggerEffectStates);
			}
			return ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx.GetTriggerEffectStates();
		}

		DualSenseTriggerEffectStates IDualSenseExtension.GetTriggerEffectStates()
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetTriggerEffectStates
			return this.GetTriggerEffectStates();
		}

		Vector3 IDualShock4Extension.GetGyroscopeValue()
		{
			return GetGyroscopeValue();
		}

		Vector3 IDualShock4Extension.GetGyroscopeValueRaw()
		{
			return GetGyroscopeValueRaw();
		}

		internal override void UpdateData(UpdateLoopType updateLoop)
		{
			if (ZuBtNPtLZCufQZglNIFxkEekWjlT && base.enabled)
			{
				sbSNeHxgZJTQLishsHOZOpJXVfmR();
			}
		}

		internal override void SourceUpdated(IControllerExtensionSource source)
		{
			ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA = source as HPzBSqILjvqYJzoolqHMMPnPmwRgA;
			ZuBtNPtLZCufQZglNIFxkEekWjlT = ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA != null && ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.rqZmpCcxwrqvspiWnSltfsNmMtXx != null;
		}

		internal override Controller.Extension Clone()
		{
			return new DualSenseExtension(this);
		}

		private void sbSNeHxgZJTQLishsHOZOpJXVfmR()
		{
			if (!ZuBtNPtLZCufQZglNIFxkEekWjlT || !ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.OAyWiMzLBXhunVVUBdVNvexGAXGFA)
			{
				return;
			}
			for (int i = 0; i < ZbRZjmGhvkgIYbTkqHOaRoVLPyAuA.SpFduwBbHQtgWRDgRrnmCBCXMILkA; i++)
			{
				if (YreTeaWnycCCYJesbyvWISqrAJRx[i].Update())
				{
					SetVibration(i, 0f, stopOtherMotors: false);
				}
			}
		}

		private void bRhnaVvuIwlKsmlfhIrqpgCHGQnhA(DualShock4MotorType P_0, float P_1, float P_2)
		{
			int num = P_0 switch
			{
				DualShock4MotorType.LeftMotor => 0, 
				DualShock4MotorType.RightMotor => 1, 
				_ => throw new NotImplementedException(), 
			};
			if (P_1 <= 0f || P_2 <= 0f)
			{
				YreTeaWnycCCYJesbyvWISqrAJRx[num].Clear();
			}
			else
			{
				YreTeaWnycCCYJesbyvWISqrAJRx[num].Start(P_2);
			}
		}
	}
}
