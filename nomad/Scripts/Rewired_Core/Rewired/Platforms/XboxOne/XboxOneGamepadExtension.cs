using System;
using Rewired.Interfaces;
using Rewired.Utils;
using Rewired.Utils.Classes.Utility;

namespace Rewired.Platforms.XboxOne
{
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	public sealed class XboxOneGamepadExtension : Controller.Extension, IControllerVibrator
	{
		private class DjmesPgVzPqQTlchLfLjaMidftMab : IControllerExtensionSource
		{
			public const int jjMhoKUnRlcFvdgvysGmlUVsjbWrA = 4;

			public gPBXUmUByuNZDOTYEcbeSlXNYIzu FFCyDFhKXkZFaTFNXBfqBcsiwOkRA;

			public readonly IXboxOneInputSource tHaCAuWtfPMGVfPUXqUmLEkbhtIQ;

			public readonly bool qrtrxfQWXRmciwLQqRYaagesCLwIA;

			public DjmesPgVzPqQTlchLfLjaMidftMab(bool P_0, IXboxOneInputSource P_1, gPBXUmUByuNZDOTYEcbeSlXNYIzu P_2)
			{
				FFCyDFhKXkZFaTFNXBfqBcsiwOkRA = P_2;
				tHaCAuWtfPMGVfPUXqUmLEkbhtIQ = P_1;
				qrtrxfQWXRmciwLQqRYaagesCLwIA = P_0;
			}
		}

		private DjmesPgVzPqQTlchLfLjaMidftMab onvTuAFMtrFubXWeiYjWWHDtbMtEA;

		private TimerAbs[] BFYMUQyFIDoPhinaXzAcDxWSriFm;

		private Joystick joystick => GetController<Joystick>();

		public int xboxOneUserId
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return -1;
				}
				if (onvTuAFMtrFubXWeiYjWWHDtbMtEA.tHaCAuWtfPMGVfPUXqUmLEkbhtIQ == null || joystick == null)
				{
					return -1;
				}
				return onvTuAFMtrFubXWeiYjWWHDtbMtEA.tHaCAuWtfPMGVfPUXqUmLEkbhtIQ.GetXboxOneUserIdFromUnityJoystick(joystick.unityId);
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
				if (joystick == null)
				{
					return 0uL;
				}
				long? systemId = joystick.systemId;
				if (!systemId.HasValue)
				{
					return 0uL;
				}
				return (ulong)systemId.Value;
			}
		}

		int IControllerVibrator.vibrationMotorCount
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
			: base(new DjmesPgVzPqQTlchLfLjaMidftMab(P_0, P_1, default(gPBXUmUByuNZDOTYEcbeSlXNYIzu)))
		{
			if (P_1 == null)
			{
				throw new ArgumentNullException("xboxOneInputSource");
			}
			BFYMUQyFIDoPhinaXzAcDxWSriFm = new TimerAbs[4];
			ArrayTools.Populate(BFYMUQyFIDoPhinaXzAcDxWSriFm, 0, BFYMUQyFIDoPhinaXzAcDxWSriFm.Length);
		}

		private XboxOneGamepadExtension(XboxOneGamepadExtension P_0)
			: base(P_0)
		{
			BFYMUQyFIDoPhinaXzAcDxWSriFm = new TimerAbs[4];
			ArrayTools.Populate(BFYMUQyFIDoPhinaXzAcDxWSriFm, 0, BFYMUQyFIDoPhinaXzAcDxWSriFm.Length);
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
			if (!onvTuAFMtrFubXWeiYjWWHDtbMtEA.qrtrxfQWXRmciwLQqRYaagesCLwIA)
			{
				return 0f;
			}
			return motorIndex switch
			{
				0 => onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.oAhdgdFZXLtTlGjVOObkzwuLPbLJA, 
				1 => onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.rKnIuNfuTPgoanctbQTxEjgfLyVO, 
				2 => onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.fQBycGTqeYBbidpSCIjrrxuOvEUsA, 
				3 => onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.JoWYsigeMoXdMyZBJMcXhTJOMoWp, 
				_ => 0f, 
			};
		}

		float IControllerVibrator.GetVibration(int motorIndex)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetVibration
			return this.GetVibration(motorIndex);
		}

		public float GetVibration(XboxOneGamepadMotorType motor)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return 0f;
			}
			if (!onvTuAFMtrFubXWeiYjWWHDtbMtEA.qrtrxfQWXRmciwLQqRYaagesCLwIA)
			{
				return 0f;
			}
			return motor switch
			{
				XboxOneGamepadMotorType.LeftMotor => onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.oAhdgdFZXLtTlGjVOObkzwuLPbLJA, 
				XboxOneGamepadMotorType.RightMotor => onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.rKnIuNfuTPgoanctbQTxEjgfLyVO, 
				XboxOneGamepadMotorType.LeftTriggerMotor => onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.fQBycGTqeYBbidpSCIjrrxuOvEUsA, 
				XboxOneGamepadMotorType.RightTriggerMotor => onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.JoWYsigeMoXdMyZBJMcXhTJOMoWp, 
				_ => throw new NotImplementedException(), 
			};
		}

		public void StopVibration()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (onvTuAFMtrFubXWeiYjWWHDtbMtEA.qrtrxfQWXRmciwLQqRYaagesCLwIA)
			{
				onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.LsGHZpjOjHFhvXDppYzEHGncaQx();
				for (int i = 0; i < 4; i++)
				{
					BFYMUQyFIDoPhinaXzAcDxWSriFm[i].Clear();
				}
				GYcSSESopobHUpxPzhLMiZMKSMRY();
			}
		}

		void IControllerVibrator.StopVibration()
		{
			//ILSpy generated this explicit interface implementation from .override directive in StopVibration
			this.StopVibration();
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
				if (!onvTuAFMtrFubXWeiYjWWHDtbMtEA.qrtrxfQWXRmciwLQqRYaagesCLwIA)
				{
					return;
				}
				if (stopOtherMotors)
				{
					onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.LsGHZpjOjHFhvXDppYzEHGncaQx();
					for (int i = 0; i < 4; i++)
					{
						BFYMUQyFIDoPhinaXzAcDxWSriFm[i].Clear();
					}
				}
				motorLevel = MathTools.Clamp01(motorLevel);
				switch (motor)
				{
				case XboxOneGamepadMotorType.LeftMotor:
					onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.oAhdgdFZXLtTlGjVOObkzwuLPbLJA = motorLevel;
					break;
				case XboxOneGamepadMotorType.RightMotor:
					onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.rKnIuNfuTPgoanctbQTxEjgfLyVO = motorLevel;
					break;
				case XboxOneGamepadMotorType.LeftTriggerMotor:
					onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.fQBycGTqeYBbidpSCIjrrxuOvEUsA = motorLevel;
					break;
				case XboxOneGamepadMotorType.RightTriggerMotor:
					onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.JoWYsigeMoXdMyZBJMcXhTJOMoWp = motorLevel;
					break;
				default:
					throw new NotImplementedException();
				}
				lrlUlNngGLALxagKSMmjwpatQQEDb(motor, motorLevel, duration);
				GYcSSESopobHUpxPzhLMiZMKSMRY();
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
				if (!onvTuAFMtrFubXWeiYjWWHDtbMtEA.qrtrxfQWXRmciwLQqRYaagesCLwIA)
				{
					return;
				}
				if (stopOtherMotors)
				{
					onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.LsGHZpjOjHFhvXDppYzEHGncaQx();
					for (int i = 0; i < 4; i++)
					{
						BFYMUQyFIDoPhinaXzAcDxWSriFm[i].Clear();
					}
				}
				onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.PvCnrDHAfTZoFwbHbUzoxfzmAxTd = xboxOneJoystickId;
				onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.oAhdgdFZXLtTlGjVOObkzwuLPbLJA = MathTools.Clamp01(leftMotorLevel);
				onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.rKnIuNfuTPgoanctbQTxEjgfLyVO = MathTools.Clamp01(rightMotorLevel);
				BFYMUQyFIDoPhinaXzAcDxWSriFm[0].Clear();
				BFYMUQyFIDoPhinaXzAcDxWSriFm[1].Clear();
				GYcSSESopobHUpxPzhLMiZMKSMRY();
			}
		}

		public void SetVibration(float leftMotorLevel, float rightMotorLevel, float leftTriggerLevel, float rightTriggerLevel)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (onvTuAFMtrFubXWeiYjWWHDtbMtEA.qrtrxfQWXRmciwLQqRYaagesCLwIA)
			{
				onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.PvCnrDHAfTZoFwbHbUzoxfzmAxTd = xboxOneJoystickId;
				onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.oAhdgdFZXLtTlGjVOObkzwuLPbLJA = MathTools.Clamp01(leftMotorLevel);
				onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.rKnIuNfuTPgoanctbQTxEjgfLyVO = MathTools.Clamp01(rightMotorLevel);
				onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.fQBycGTqeYBbidpSCIjrrxuOvEUsA = MathTools.Clamp01(leftTriggerLevel);
				onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA.JoWYsigeMoXdMyZBJMcXhTJOMoWp = MathTools.Clamp01(rightTriggerLevel);
				for (int i = 0; i < 4; i++)
				{
					BFYMUQyFIDoPhinaXzAcDxWSriFm[i].Clear();
				}
				GYcSSESopobHUpxPzhLMiZMKSMRY();
			}
		}

		public void PulseVibrateMotor(XboxOneGamepadMotorType motor, float startLevel, float endLevel, float duration)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (base.isJoystickConnected && onvTuAFMtrFubXWeiYjWWHDtbMtEA.qrtrxfQWXRmciwLQqRYaagesCLwIA)
			{
				lrlUlNngGLALxagKSMmjwpatQQEDb(motor, 0f, 0f);
				onvTuAFMtrFubXWeiYjWWHDtbMtEA.tHaCAuWtfPMGVfPUXqUmLEkbhtIQ.PulseVibrateMotor(xboxOneJoystickId, motor, startLevel, endLevel, duration);
			}
		}

		internal override void UpdateData(UpdateLoopType updateLoop)
		{
			IHFlWYTSctjzmMFVPoeAxAufcojn();
		}

		internal override void SourceUpdated(IControllerExtensionSource source)
		{
			onvTuAFMtrFubXWeiYjWWHDtbMtEA = source as DjmesPgVzPqQTlchLfLjaMidftMab;
		}

		internal override Controller.Extension Clone()
		{
			return new XboxOneGamepadExtension(this);
		}

		private void IHFlWYTSctjzmMFVPoeAxAufcojn()
		{
			if (!onvTuAFMtrFubXWeiYjWWHDtbMtEA.qrtrxfQWXRmciwLQqRYaagesCLwIA)
			{
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				if (BFYMUQyFIDoPhinaXzAcDxWSriFm[i].Update())
				{
					SetVibration(i, 0f, stopOtherMotors: false);
				}
			}
		}

		private void lrlUlNngGLALxagKSMmjwpatQQEDb(XboxOneGamepadMotorType P_0, float P_1, float P_2)
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
				BFYMUQyFIDoPhinaXzAcDxWSriFm[num].Clear();
			}
			else
			{
				BFYMUQyFIDoPhinaXzAcDxWSriFm[num].Start(P_2);
			}
		}

		private void GYcSSESopobHUpxPzhLMiZMKSMRY()
		{
			if (base.isJoystickConnected)
			{
				onvTuAFMtrFubXWeiYjWWHDtbMtEA.tHaCAuWtfPMGVfPUXqUmLEkbhtIQ.SetXboxOneVibration(xboxOneJoystickId, onvTuAFMtrFubXWeiYjWWHDtbMtEA.FFCyDFhKXkZFaTFNXBfqBcsiwOkRA);
			}
		}
	}
}
