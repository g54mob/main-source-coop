using System;
using Rewired.Interfaces;
using Rewired.Utils;
using Rewired.Utils.Classes.Utility;

namespace Rewired.Platforms.XboxOne
{
	public sealed class XboxOneGamepadExtension : Controller.Extension, IControllerVibrator
	{
		private class rKYRURQbUzdxQHxIhEXtftnHBdHw : IControllerExtensionSource
		{
			public const int IvQuNSqDypWdalUGYcleYiqEVPmd = 4;

			public OCxnhoeHJSmdQcYpgnyghAYbUGqI althziZnBCLwxMViIeGJaEFtrKgM;

			public readonly IXboxOneInputSource GcmEfGWGpZLjOdiyIOEjPeAmuYvC;

			public readonly bool BMKmLgTjByiExXMvsepaerGfBiXH;

			public rKYRURQbUzdxQHxIhEXtftnHBdHw(bool P_0, IXboxOneInputSource P_1, OCxnhoeHJSmdQcYpgnyghAYbUGqI P_2)
			{
				althziZnBCLwxMViIeGJaEFtrKgM = P_2;
				GcmEfGWGpZLjOdiyIOEjPeAmuYvC = P_1;
				BMKmLgTjByiExXMvsepaerGfBiXH = P_0;
			}
		}

		private rKYRURQbUzdxQHxIhEXtftnHBdHw rmMLNGpqXVBSkkjZadentyrvdrgtA;

		private TimerAbs[] IfVBCqaFujeIzJMUNQBcBKnduRXLA;

		private Joystick chcOLBMXAtWUArFXncgBETOGbAnFb => GetController<Joystick>();

		public int xboxOneUserId
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return -1;
				}
				if (rmMLNGpqXVBSkkjZadentyrvdrgtA.GcmEfGWGpZLjOdiyIOEjPeAmuYvC == null || chcOLBMXAtWUArFXncgBETOGbAnFb == null)
				{
					return -1;
				}
				return rmMLNGpqXVBSkkjZadentyrvdrgtA.GcmEfGWGpZLjOdiyIOEjPeAmuYvC.GetXboxOneUserIdFromUnityJoystick(chcOLBMXAtWUArFXncgBETOGbAnFb.unityId);
			}
		}

		public ulong xboxOneJoystickId
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return 0uL;
				}
				if (chcOLBMXAtWUArFXncgBETOGbAnFb == null)
				{
					return 0uL;
				}
				long? systemId = chcOLBMXAtWUArFXncgBETOGbAnFb.systemId;
				if (!systemId.HasValue)
				{
					return 0uL;
				}
				return (ulong)systemId.Value;
			}
		}

		public int vibrationMotorCount
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return 0;
				}
				return 4;
			}
		}

		internal XboxOneGamepadExtension(bool P_0, IXboxOneInputSource P_1)
			: base(new rKYRURQbUzdxQHxIhEXtftnHBdHw(P_0, P_1, default(OCxnhoeHJSmdQcYpgnyghAYbUGqI)))
		{
			if (P_1 == null)
			{
				throw new ArgumentNullException("xboxOneInputSource");
			}
			IfVBCqaFujeIzJMUNQBcBKnduRXLA = new TimerAbs[4];
			ArrayTools.Populate(IfVBCqaFujeIzJMUNQBcBKnduRXLA, 0, IfVBCqaFujeIzJMUNQBcBKnduRXLA.Length);
		}

		private XboxOneGamepadExtension(XboxOneGamepadExtension P_0)
			: base(P_0)
		{
			IfVBCqaFujeIzJMUNQBcBKnduRXLA = new TimerAbs[4];
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
			else if (motorIndex >= 0 && motorIndex < 4)
			{
				SetVibration(motorIndex switch
				{
					0 => XboxOneGamepadMotorType.LeftMotor, 
					1 => XboxOneGamepadMotorType.RightMotor, 
					2 => XboxOneGamepadMotorType.LeftTriggerMotor, 
					3 => XboxOneGamepadMotorType.RightTriggerMotor, 
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
			if (!rmMLNGpqXVBSkkjZadentyrvdrgtA.BMKmLgTjByiExXMvsepaerGfBiXH)
			{
				return 0f;
			}
			return motorIndex switch
			{
				0 => rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.iYnwtFXqkYqnGUSQFZDESWIzVLVj, 
				1 => rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.ustXDwUALhHRVQXlEyYvEvvIQJsc, 
				2 => rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.wTCGMUooHghhIgaiqISLfqkiHMvBc, 
				3 => rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.dOBWtOmULbMIFyuANzzkOjTUWQTe, 
				_ => 0f, 
			};
		}

		public float GetVibration(XboxOneGamepadMotorType motor)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return 0f;
			}
			if (!rmMLNGpqXVBSkkjZadentyrvdrgtA.BMKmLgTjByiExXMvsepaerGfBiXH)
			{
				return 0f;
			}
			return motor switch
			{
				XboxOneGamepadMotorType.LeftMotor => rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.iYnwtFXqkYqnGUSQFZDESWIzVLVj, 
				XboxOneGamepadMotorType.RightMotor => rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.ustXDwUALhHRVQXlEyYvEvvIQJsc, 
				XboxOneGamepadMotorType.LeftTriggerMotor => rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.wTCGMUooHghhIgaiqISLfqkiHMvBc, 
				XboxOneGamepadMotorType.RightTriggerMotor => rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.dOBWtOmULbMIFyuANzzkOjTUWQTe, 
				_ => throw new NotImplementedException(), 
			};
		}

		public void StopVibration()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (rmMLNGpqXVBSkkjZadentyrvdrgtA.BMKmLgTjByiExXMvsepaerGfBiXH)
			{
				rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.SYlczOJTLMCPuGgKHdrzHezGyLRJA();
				for (int i = 0; i < 4; i++)
				{
					IfVBCqaFujeIzJMUNQBcBKnduRXLA[i].Clear();
				}
				eFuFKyJtpUZgMpxZDmhhdBiyfABcb();
			}
		}

		public void SetVibration(XboxOneGamepadMotorType motor, float motorLevel)
		{
			SetVibration(motor, motorLevel, 0f, stopOtherMotors: false);
		}

		public void SetVibration(XboxOneGamepadMotorType motor, float motorLevel, float duration)
		{
			SetVibration(motor, motorLevel, duration, stopOtherMotors: false);
		}

		public void SetVibration(XboxOneGamepadMotorType motor, float motorLevel, bool stopOtherMotors)
		{
			SetVibration(motor, motorLevel, 0f, stopOtherMotors);
		}

		public void SetVibration(XboxOneGamepadMotorType motor, float motorLevel, float duration, bool stopOtherMotors)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else
			{
				if (!rmMLNGpqXVBSkkjZadentyrvdrgtA.BMKmLgTjByiExXMvsepaerGfBiXH)
				{
					return;
				}
				if (stopOtherMotors)
				{
					rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.SYlczOJTLMCPuGgKHdrzHezGyLRJA();
					for (int i = 0; i < 4; i++)
					{
						IfVBCqaFujeIzJMUNQBcBKnduRXLA[i].Clear();
					}
				}
				motorLevel = MathTools.Clamp01(motorLevel);
				switch (motor)
				{
				case XboxOneGamepadMotorType.LeftMotor:
					rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.iYnwtFXqkYqnGUSQFZDESWIzVLVj = motorLevel;
					break;
				case XboxOneGamepadMotorType.RightMotor:
					rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.ustXDwUALhHRVQXlEyYvEvvIQJsc = motorLevel;
					break;
				case XboxOneGamepadMotorType.LeftTriggerMotor:
					rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.wTCGMUooHghhIgaiqISLfqkiHMvBc = motorLevel;
					break;
				case XboxOneGamepadMotorType.RightTriggerMotor:
					rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.dOBWtOmULbMIFyuANzzkOjTUWQTe = motorLevel;
					break;
				default:
					throw new NotImplementedException();
				}
				uLJgIUZWcYRvIJfnwylMTtjINIBA(motor, motorLevel, duration);
				eFuFKyJtpUZgMpxZDmhhdBiyfABcb();
			}
		}

		public void SetVibration(float leftMotorLevel, float rightMotorLevel)
		{
			SetVibration(leftMotorLevel, rightMotorLevel, stopOtherMotors: false);
		}

		public void SetVibration(float leftMotorLevel, float rightMotorLevel, bool stopOtherMotors)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else
			{
				if (!rmMLNGpqXVBSkkjZadentyrvdrgtA.BMKmLgTjByiExXMvsepaerGfBiXH)
				{
					return;
				}
				if (stopOtherMotors)
				{
					rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.SYlczOJTLMCPuGgKHdrzHezGyLRJA();
					for (int i = 0; i < 4; i++)
					{
						IfVBCqaFujeIzJMUNQBcBKnduRXLA[i].Clear();
					}
				}
				rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.GRdgvZNhNYNXeCgYfwXCSDrSaAXbA = xboxOneJoystickId;
				rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.iYnwtFXqkYqnGUSQFZDESWIzVLVj = MathTools.Clamp01(leftMotorLevel);
				rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.ustXDwUALhHRVQXlEyYvEvvIQJsc = MathTools.Clamp01(rightMotorLevel);
				IfVBCqaFujeIzJMUNQBcBKnduRXLA[0].Clear();
				IfVBCqaFujeIzJMUNQBcBKnduRXLA[1].Clear();
				eFuFKyJtpUZgMpxZDmhhdBiyfABcb();
			}
		}

		public void SetVibration(float leftMotorLevel, float rightMotorLevel, float leftTriggerLevel, float rightTriggerLevel)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (rmMLNGpqXVBSkkjZadentyrvdrgtA.BMKmLgTjByiExXMvsepaerGfBiXH)
			{
				rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.GRdgvZNhNYNXeCgYfwXCSDrSaAXbA = xboxOneJoystickId;
				rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.iYnwtFXqkYqnGUSQFZDESWIzVLVj = MathTools.Clamp01(leftMotorLevel);
				rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.ustXDwUALhHRVQXlEyYvEvvIQJsc = MathTools.Clamp01(rightMotorLevel);
				rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.wTCGMUooHghhIgaiqISLfqkiHMvBc = MathTools.Clamp01(leftTriggerLevel);
				rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM.dOBWtOmULbMIFyuANzzkOjTUWQTe = MathTools.Clamp01(rightTriggerLevel);
				for (int i = 0; i < 4; i++)
				{
					IfVBCqaFujeIzJMUNQBcBKnduRXLA[i].Clear();
				}
				eFuFKyJtpUZgMpxZDmhhdBiyfABcb();
			}
		}

		public void PulseVibrateMotor(XboxOneGamepadMotorType motor, float startLevel, float endLevel, float duration)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (base.isJoystickConnected && rmMLNGpqXVBSkkjZadentyrvdrgtA.BMKmLgTjByiExXMvsepaerGfBiXH)
			{
				uLJgIUZWcYRvIJfnwylMTtjINIBA(motor, 0f, 0f);
				rmMLNGpqXVBSkkjZadentyrvdrgtA.GcmEfGWGpZLjOdiyIOEjPeAmuYvC.PulseVibrateMotor(xboxOneJoystickId, motor, startLevel, endLevel, duration);
			}
		}

		internal void PwSVyxiNOmkyuuXUHWyuymUyabog(UpdateLoopType P_0)
		{
			RkjJWtCPffqNHKYZiSpumVKUPDFy();
		}

		internal void EJfWkXMMYTRfEHNEnCEaDzBnFICoA(IControllerExtensionSource P_0)
		{
			rmMLNGpqXVBSkkjZadentyrvdrgtA = P_0 as rKYRURQbUzdxQHxIhEXtftnHBdHw;
		}

		internal Controller.Extension vpHGhDFXfrFPnKJBKRFCgbudEJvBB()
		{
			return new XboxOneGamepadExtension(this);
		}

		private void RkjJWtCPffqNHKYZiSpumVKUPDFy()
		{
			if (!rmMLNGpqXVBSkkjZadentyrvdrgtA.BMKmLgTjByiExXMvsepaerGfBiXH)
			{
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				if (IfVBCqaFujeIzJMUNQBcBKnduRXLA[i].Update())
				{
					SetVibration(i, 0f, stopOtherMotors: false);
				}
			}
		}

		private void uLJgIUZWcYRvIJfnwylMTtjINIBA(XboxOneGamepadMotorType P_0, float P_1, float P_2)
		{
			int num = P_0 switch
			{
				XboxOneGamepadMotorType.LeftMotor => 0, 
				XboxOneGamepadMotorType.RightMotor => 1, 
				XboxOneGamepadMotorType.LeftTriggerMotor => 2, 
				XboxOneGamepadMotorType.RightTriggerMotor => 3, 
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

		private void eFuFKyJtpUZgMpxZDmhhdBiyfABcb()
		{
			if (base.isJoystickConnected)
			{
				rmMLNGpqXVBSkkjZadentyrvdrgtA.GcmEfGWGpZLjOdiyIOEjPeAmuYvC.SetXboxOneVibration(xboxOneJoystickId, rmMLNGpqXVBSkkjZadentyrvdrgtA.althziZnBCLwxMViIeGJaEFtrKgM);
			}
		}
	}
}
