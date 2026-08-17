using System;
using Rewired.HID.Drivers;
using Rewired.Interfaces;
using Rewired.Utils;
using Rewired.Utils.Classes.Utility;
using UnityEngine;

namespace Rewired.ControllerExtensions
{
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	public sealed class DualShock4Extension : Controller.Extension, IControllerVibrator, IDualShock4Extension, IHIDControllerExtension
	{
		private class PuPSULpqCwHknKcNOcbajCyCwCsJ : IControllerExtensionSource
		{
			public readonly IDriver_DualShock4 TiyYdkJnHnIhVCQQNOnLAVWpbtkcA;

			public readonly bool MHgqgefIVFtJlxGfwuDoQyZExgkk;

			public readonly int sKpJSNLgmBvbWLHCmFhhNjydjODq;

			public PuPSULpqCwHknKcNOcbajCyCwCsJ(IDriver_DualShock4 P_0, bool P_1, int P_2)
			{
				TiyYdkJnHnIhVCQQNOnLAVWpbtkcA = P_0;
				MHgqgefIVFtJlxGfwuDoQyZExgkk = P_1;
				sKpJSNLgmBvbWLHCmFhhNjydjODq = P_2;
			}
		}

		private PuPSULpqCwHknKcNOcbajCyCwCsJ tOoNCxGnMgTmYwizBCCQKIvEJRUXA;

		private bool BgZKzYeNnIYuJgEpnNszjEGwuCYf;

		private TimerAbs[] RNTBmtHBPMWaYiaasImTOoMzsBedA;

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
				if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf)
				{
					return 0;
				}
				return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.sKpJSNLgmBvbWLHCmFhhNjydjODq;
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
				if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf || !base.enabled)
				{
					return 0f;
				}
				return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LightColorR;
			}
			set
			{
				if (BgZKzYeNnIYuJgEpnNszjEGwuCYf)
				{
					tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LightColorR = value;
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
				if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf || !base.enabled)
				{
					return 0f;
				}
				return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LightColorG;
			}
			set
			{
				if (BgZKzYeNnIYuJgEpnNszjEGwuCYf)
				{
					tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LightColorG = value;
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
				if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf || !base.enabled)
				{
					return 0f;
				}
				return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LightColorB;
			}
			set
			{
				if (BgZKzYeNnIYuJgEpnNszjEGwuCYf)
				{
					tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LightColorB = value;
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
				if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf)
				{
					return 0;
				}
				return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.MaxTouches;
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
				return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.GetTouchCount();
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
				if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf)
				{
					return 0f;
				}
				return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.BatteryLevel;
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
				if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf)
				{
					return false;
				}
				return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.BatteryCharging;
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
				return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.vendorId;
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
				return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.productId;
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
				return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.productName;
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
				return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.manufacturer;
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
				return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.usagePage;
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
				return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.usage;
			}
		}

		internal DualShock4Extension(IDriver_DualShock4 P_0)
			: base(new PuPSULpqCwHknKcNOcbajCyCwCsJ(P_0, P_0.VibrationMotorCount > 0, P_0.VibrationMotorCount))
		{
			RNTBmtHBPMWaYiaasImTOoMzsBedA = new TimerAbs[P_0.VibrationMotorCount];
			ArrayTools.Populate(RNTBmtHBPMWaYiaasImTOoMzsBedA, 0, RNTBmtHBPMWaYiaasImTOoMzsBedA.Length);
		}

		private DualShock4Extension(DualShock4Extension P_0)
			: base(P_0)
		{
			try
			{
				RNTBmtHBPMWaYiaasImTOoMzsBedA = new TimerAbs[P_0.Rewired_002EInterfaces_002EIControllerVibrator_002EvibrationMotorCount];
			}
			catch
			{
				RNTBmtHBPMWaYiaasImTOoMzsBedA = new TimerAbs[0];
			}
			ArrayTools.Populate(RNTBmtHBPMWaYiaasImTOoMzsBedA, 0, RNTBmtHBPMWaYiaasImTOoMzsBedA.Length);
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
			else if (BgZKzYeNnIYuJgEpnNszjEGwuCYf && base.enabled && motorIndex >= 0 && motorIndex < tOoNCxGnMgTmYwizBCCQKIvEJRUXA.sKpJSNLgmBvbWLHCmFhhNjydjODq)
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
			if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf || !base.enabled)
			{
				return 0f;
			}
			if (!tOoNCxGnMgTmYwizBCCQKIvEJRUXA.MHgqgefIVFtJlxGfwuDoQyZExgkk)
			{
				return 0f;
			}
			return motorIndex switch
			{
				0 => tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LeftMotor, 
				1 => tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.RightMotor, 
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
			else if (BgZKzYeNnIYuJgEpnNszjEGwuCYf && base.enabled && tOoNCxGnMgTmYwizBCCQKIvEJRUXA.MHgqgefIVFtJlxGfwuDoQyZExgkk)
			{
				for (int i = 0; i < tOoNCxGnMgTmYwizBCCQKIvEJRUXA.sKpJSNLgmBvbWLHCmFhhNjydjODq; i++)
				{
					RNTBmtHBPMWaYiaasImTOoMzsBedA[i].Clear();
				}
				tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.StopVibration();
			}
		}

		void IControllerVibrator.StopVibration()
		{
			//ILSpy generated this explicit interface implementation from .override directive in StopVibration
			this.StopVibration();
		}

		public float GetVibration(DualShock4MotorType motor)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return 0f;
			}
			if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf || !base.enabled)
			{
				return 0f;
			}
			if (!tOoNCxGnMgTmYwizBCCQKIvEJRUXA.MHgqgefIVFtJlxGfwuDoQyZExgkk)
			{
				return 0f;
			}
			return (int)motor switch
			{
				0 => tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LeftMotor, 
				1 => tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.RightMotor, 
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
				if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf || !base.enabled || !tOoNCxGnMgTmYwizBCCQKIvEJRUXA.MHgqgefIVFtJlxGfwuDoQyZExgkk)
				{
					return;
				}
				if (stopOtherMotors)
				{
					for (int i = 0; i < tOoNCxGnMgTmYwizBCCQKIvEJRUXA.sKpJSNLgmBvbWLHCmFhhNjydjODq; i++)
					{
						RNTBmtHBPMWaYiaasImTOoMzsBedA[i].Clear();
					}
					tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.StopVibration();
				}
				motorLevel = MathTools.Clamp01(motorLevel);
				switch ((int)motor)
				{
				case 0:
					tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LeftMotor = motorLevel;
					break;
				case 1:
					tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.RightMotor = motorLevel;
					break;
				default:
					throw new NotImplementedException();
				}
				tWAvPaOwwkYPzGwXDTjXhfsGcBHg(motor, motorLevel, duration);
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
			else if (BgZKzYeNnIYuJgEpnNszjEGwuCYf && base.enabled && tOoNCxGnMgTmYwizBCCQKIvEJRUXA.MHgqgefIVFtJlxGfwuDoQyZExgkk)
			{
				tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LeftMotor = MathTools.Clamp01(leftMotorLevel);
				tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.RightMotor = MathTools.Clamp01(rightMotorLevel);
				tWAvPaOwwkYPzGwXDTjXhfsGcBHg(DualShock4MotorType.LeftMotor, leftMotorLevel, leftMotorDuration);
				tWAvPaOwwkYPzGwXDTjXhfsGcBHg(DualShock4MotorType.RightMotor, rightMotorLevel, rightMotorDuration);
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
			if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf)
			{
				return default(Color);
			}
			return new Color(tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LightColorR, tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LightColorG, tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LightColorB, 1f);
		}

		public void SetLightColor(Color color)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (BgZKzYeNnIYuJgEpnNszjEGwuCYf && base.enabled)
			{
				tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LightColorR = color.r * color.a;
				tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LightColorG = color.g * color.a;
				tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LightColorB = color.b * color.a;
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
			else if (BgZKzYeNnIYuJgEpnNszjEGwuCYf && base.enabled)
			{
				tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LightColorR = red * intensity;
				tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LightColorG = green * intensity;
				tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LightColorB = blue * intensity;
			}
		}

		void IDualShock4Extension.SetLightColor(float red, float green, float blue, float intensity)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetLightColor
			this.SetLightColor(red, green, blue, intensity);
		}

		public void SetLightFlash(float onDuration, float offDuration)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (BgZKzYeNnIYuJgEpnNszjEGwuCYf && base.enabled)
			{
				tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LightFlashOnDuration = onDuration;
				tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LightFlashOffDuration = offDuration;
			}
		}

		public void StopLightFlash()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (BgZKzYeNnIYuJgEpnNszjEGwuCYf && base.enabled)
			{
				tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.StopLightFlash();
			}
		}

		public Vector3 GetAccelerometerValueRaw()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Vector3.zero;
			}
			if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return Vector3.zero;
			}
			return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.AccelerometerValueRaw;
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
			if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return Vector3.zero;
			}
			return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.AccelerometerValue;
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
			if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return Vector3.zero;
			}
			return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LastGyroscopeValueRaw;
		}

		public Vector3 GetLastGyroscopeValue()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Vector3.zero;
			}
			if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return Vector3.zero;
			}
			return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.LastGyroscopeValue;
		}

		public Vector3 GetGyroscopeValueRaw()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Vector3.zero;
			}
			if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return Vector3.zero;
			}
			return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.GyroscopeValueRaw;
		}

		public Vector3 GetGyroscopeValue()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Vector3.zero;
			}
			if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return Vector3.zero;
			}
			return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.GyroscopeValue;
		}

		public Quaternion GetOrientation()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Quaternion.identity;
			}
			if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return default(Quaternion);
			}
			return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.Orientation;
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
			else if (BgZKzYeNnIYuJgEpnNszjEGwuCYf)
			{
				tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.ResetOrientation();
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
			if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return -1;
			}
			return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.GetTouchIdAtIndex(index);
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
			if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				position = Vector2.zero;
				return false;
			}
			return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.GetTouchPositionByIndex(index, out position);
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
			if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				position = Vector2.zero;
				return false;
			}
			return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.GetTouchPositionByTouchId(touchId, out position);
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
			if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				position = Vector2.zero;
				return false;
			}
			int positionX;
			int positionY;
			bool touchPositionAbsoluteByIndex = tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.GetTouchPositionAbsoluteByIndex(index, out positionX, out positionY);
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
			if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				position = Vector2.zero;
				return false;
			}
			int positionX;
			int positionY;
			bool touchPositionAbsoluteByTouchId = tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.GetTouchPositionAbsoluteByTouchId(touchId, out positionX, out positionY);
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
			if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return false;
			}
			return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.IsTouchingAtIndex(index);
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
			if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf || !base.enabled || !ReInput.IsInputAllowed(ControllerType.Joystick))
			{
				return false;
			}
			return tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA.IsTouchingAtTouchId(touchId);
		}

		bool IDualShock4Extension.IsTouchingByTouchId(int touchId)
		{
			//ILSpy generated this explicit interface implementation from .override directive in IsTouchingByTouchId
			return this.IsTouchingByTouchId(touchId);
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
			if (BgZKzYeNnIYuJgEpnNszjEGwuCYf && base.enabled)
			{
				zWFLLrVfMRCPqiJrNVNTJKokOvnpA();
			}
		}

		internal override void SourceUpdated(IControllerExtensionSource source)
		{
			tOoNCxGnMgTmYwizBCCQKIvEJRUXA = source as PuPSULpqCwHknKcNOcbajCyCwCsJ;
			BgZKzYeNnIYuJgEpnNszjEGwuCYf = tOoNCxGnMgTmYwizBCCQKIvEJRUXA != null && tOoNCxGnMgTmYwizBCCQKIvEJRUXA.TiyYdkJnHnIhVCQQNOnLAVWpbtkcA != null;
		}

		internal override Controller.Extension Clone()
		{
			return new DualShock4Extension(this);
		}

		private void zWFLLrVfMRCPqiJrNVNTJKokOvnpA()
		{
			if (!BgZKzYeNnIYuJgEpnNszjEGwuCYf || !tOoNCxGnMgTmYwizBCCQKIvEJRUXA.MHgqgefIVFtJlxGfwuDoQyZExgkk)
			{
				return;
			}
			for (int i = 0; i < tOoNCxGnMgTmYwizBCCQKIvEJRUXA.sKpJSNLgmBvbWLHCmFhhNjydjODq; i++)
			{
				if (RNTBmtHBPMWaYiaasImTOoMzsBedA[i].Update())
				{
					SetVibration(i, 0f, stopOtherMotors: false);
				}
			}
		}

		private void tWAvPaOwwkYPzGwXDTjXhfsGcBHg(DualShock4MotorType P_0, float P_1, float P_2)
		{
			int num = P_0 switch
			{
				DualShock4MotorType.LeftMotor => 0, 
				DualShock4MotorType.RightMotor => 1, 
				_ => throw new NotImplementedException(), 
			};
			if (P_1 <= 0f || P_2 <= 0f)
			{
				RNTBmtHBPMWaYiaasImTOoMzsBedA[num].Clear();
			}
			else
			{
				RNTBmtHBPMWaYiaasImTOoMzsBedA[num].Start(P_2);
			}
		}
	}
}
