using System;
using Rewired.ControllerExtensions;
using Rewired.Drivers.Interfaces;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using UnityEngine;

namespace Rewired.HID.Drivers
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class DualShock4Driver : HIDDeviceDriver, IDisposable, IControllerDriver, IDriver_DualShock4
	{
		private enum jdKRWCkCgQtVyUxhMexbTsrigjYG
		{
			X = 0,
			Y = 1,
			Z = 2
		}

		private enum fstLtwIvIVaQdqeviUnLQgFKVKNN
		{
			None = 0,
			XZ = 1,
			Y = 2
		}

		private const float oYhgiVHWYRhSmetHbOEAAZfNDkpbA = 4f;

		private const int VnQiGrFElxZQckFfuctAXoBRfYio = 14;

		private const int NVDYvlUbanjoVEfCscGpuHzITBDJ = 2;

		private const int juKxFOEQbSHpWnloPcjxibbiPlJZ = 0;

		private const int LTLfLXCMXlvkqeqZQQYqFrKglkhO = 1912;

		private const int zmgUzAISQnwmcSnRCrVjvtCvuPvF = 0;

		private const int EhgtlSzhDzJREnSGfvPwucmYvDgh = 941;

		private const bool cFoaIdwmcjXIOeyDejRBFRqHIIvI = false;

		private const bool eNgwxXZtbogNSshVndAJEHBOYnzn = true;

		private const float YJODZDZyLupTvedBYtsBCQGdIJoY = 2.5f;

		private const int mhYaeNxjGUgYsdSbBfmuuzdmhYhhA = 0;

		private const int gZJWtQOzTvauGAndEdeLlLBnGCvg = 0;

		private const int JuxFedDLuHEWuJGmGjNRTXydYhcCA = 1;

		private const int gMICZRzRXJIpmXCoCKEFjRezZsrw = 0;

		private const int uUhSOAEOUjxORrPAlSiIIynVtWfS = 0;

		private const int gYdSziRAyQLNrigFtZetxtOzrQeY = 0;

		private const int BITUChKhaqrfeCbMBdqOcEOcfwBk = 1;

		private const int pqRLNyHNDfOvygwxDvsnPknabGOJ = 17;

		private const int BSgTgtWhVGzvVhrIaomwrbjTAIyAA = 0;

		private const int uUCAlPGBYDzCkzhLUIsQSxxQqnsS = 2;

		private const int YZYFKODfnQIXvcrIaaTAZiohWjbL = 64;

		private const int ETpWPjSAiLKTGwtLehLLxoAFusJq = 78;

		private const int rXQhAqeXVjEtdaeohSLphBdJANCM = 1;

		private const int QfYdZcwkayAvZdCDuugpgmfzkbceA = 2;

		private const int DzaRRIWzKJwwHSxpeNLYohrEcSZjA = 3;

		private const int NETxQtxDbDuQNWjqBPmdHLROaXvL = 4;

		private const int jtJcMoQIPZrUcmdDViDBhevaNHuTA = 8;

		private const int krEYJDRWNlIYBAldKnbhfHfJswKk = 9;

		private const int ydojHvsUOrPlORFlxbxbMfjpcCnW = 5;

		private const int eGdFoEVAwMwMNaWPeoSYZtLeeKuX = 19;

		private const int OPGozUEAdfpTIweLaFTzamiYgLBAb = 13;

		private const int FCWcSYGGJCXkICHoMvNyzceBCAAZ = 35;

		private const int wRpaEgloiUtSIMnBUQqQgXAtvGQj = 5;

		private const int RmlhUvxQfUSYHRWkRbcNUtkNZKqk = 6;

		private const int pqAJxQPmqEXXwSQDvbeFoDBGBCTu = 7;

		private const int BShCzWEUMMkNRRISgHeYrgyAKeBxA = 10;

		private const int DGIuICpMyeViBhAZzKMkCGllYlVi = 30;

		private const int kuaQKfZIaTeRFRDunmrDCRphJVaB = 27;

		private const byte NTiGXrXoDgfUERvbvatXEDyuXTuz = 200;

		private const byte bQrkWLxfTJdpugmNorusRTfphqUb = 53;

		private const byte QShqQGChiUCVxABlShCsukbzBOZCb = byte.MaxValue;

		private const byte HzxAjeyUhBKZwUWkRHvpnqObItTBA = 0;

		private const bool rYQEgqARilSnhOeGWjvUtVLbisqPA = true;

		private const int SOpLCXhgrxApahqNSlYFZBeDBlzN = 25;

		private const int euZbolgWQrVReHjPVBQUSaUHTexhA = 187500;

		private const float lDbFbqvqEufajARKdBOwBoCGxceY = 8192f;

		private const float PvdChOBTildnQTbeljDDhxEjosKP = 0.0010652969f;

		private const float DEHlxPOboOsasOXJHVQngYzlMViL = 0.06103702f;

		private const bool EVKZHjcgIhhQQcfCqDRlayphJIJVb = true;

		private const bool evdXqMDOJkHyTOqOPuwBGvOrdque = true;

		private const bool ATqMOyfMJMTbgEZAVHRQBiGtAYQG = true;

		private const bool EsBYcKLwroGxEVRIycrsfZBjAYSs = true;

		private const float igGKivmYKIaikdQMuRmXkneNhKDO = 4096f;

		private const float XhSUfvOviFvcJhVFMDHmwBfFgOLiA = 16384f;

		private const float KuDFpxZohPCygYBKECiPfxRrWDWhA = 16777216f;

		private const float HXSmVXMmuzPzQbaXEKFnalPLRDmg = 268435460f;

		private const float zisFYOTXrdFiIaamncVJBqUDcozsb = 0.01999998f;

		private const float cmqBfHZbZbxuxaOWWLKEYLvypngC = 8192f;

		private const float jaZvCygXnpRJAQtlHquIuNsjNfAT = 0.98f;

		private const float ogqnCWXpArbPdQqQpoYmUbHLDZae = 45f;

		private const float PfGMCkxaKVtJxmOzdWVzlIcXgPogA = 20f;

		private readonly bool fDAHDJbARjWStNNVeAQVzofIxULQ;

		private readonly DeviceConnectionType xqxRNEDkGGHZgGrNQEMzeUHLqoEfb;

		private readonly int ekpwlvHcPlMfYPFJKDKaXHiCkzhs;

		private readonly int uXVJskFSOanKkrhkSxOQMBeHKfbN;

		private readonly bool mNDdAkFBUZZNqoZtuijPtQcAcaOk;

		private readonly byte vPZjGKKCZiTkmypxFRYxoULOigAv;

		private readonly int pztukqONRdxCvQGBPEuNfbIMkFGDA;

		private readonly int RUJKBthdvVQqUCHNDAtXmuJoumgV;

		private readonly int htyqSPpfIfxkTMvTRsUkdMOHtgNH;

		private readonly int tFjliRmVNqRJBLgdGqbeGULvFMdU;

		private readonly int uwDjQiabEjNvefpNjTtmGelTYOGR;

		private readonly int UCQcafhwMxDbvyQzpJLDHdHejqygA;

		private readonly NativeBuffer HLsWXNxbuFUDCLhxfUYoppWgHBGi;

		private readonly NativeBuffer CAPBXhcnJamPmJsEbuPqwfyjKPHdb;

		private readonly OutputReport BuQEXisXIqFXvdwXReGSUsjHrfng;

		private readonly Func<OutputReport, bool> nUfHkEItsYftJzYkqNavhDPHdlCe;

		private readonly Action<OutputReport> KMtNyOhjxErEtpBItWsLNqmherMHA;

		private readonly GetHidFeatureData xbcZauqgqoSgkSZBEukVnAbsZDLn;

		private bool KwzpPuaDrCLkgOgTRCqggBvGhEBh;

		private bool ULLBncarwIzAwpoBsvQVRaOwwRtvA;

		private double ttLTFCslQtbuRHfbjcFpXrsRMUGfb;

		private byte JrgUdyTuBsdMhTzLXfuRKnFdAHNz;

		private Quaternion ihPCDXHqXKildPsQIzfxuLnxeoPg = Quaternion.identity;

		private ushort JXRJRwsAtReVqNSrQgxDsJidJLwn;

		private float wLqvEwIHNMZuhtrqhsAlwwIFQXrI;

		private double sMsPthWihMdOEjJCEDJfgIInICwt;

		private float PrECXiktcHTaMnkJTbhtpwfhaLwjA;

		private byte nPgWllPLDwssOmvhECfyMrqDVwxu;

		private byte ONJNyutLlCAXYqZIKEnpKSJLqjnb;

		private Quaternion ekOqcOMlzPQfEATHHqxXeKZIhArc = Quaternion.identity;

		private Quaternion puefNXgPSijNJiZwlSsoDsuSgGLbb = Quaternion.identity;

		private bool bHpRFgezjLRiIFMjYufaVCZZYYSD;

		private int aMrCHiGAjvpMXtinFAhtXyeWiqCdA;

		private int[] mqTdiUwCjTjroDyCFFzlIQkHsFmr = new int[2];

		private int[] HmgQugGcjVFKWEAAoSiGqAbRaXjJA = new int[2];

		private bool isVibrating
		{
			get
			{
				for (int i = 0; i < base.VibrationMotorCount; i++)
				{
					if (vibrationMotors[i].SpeedRaw > 0)
					{
						return true;
					}
				}
				return false;
			}
		}

		public float BatteryLevel
		{
			get
			{
				float num = 0f;
				num = ((!fDAHDJbARjWStNNVeAQVzofIxULQ) ? ((float)(JrgUdyTuBsdMhTzLXfuRKnFdAHNz - 1) * 10f) : ((float)(JrgUdyTuBsdMhTzLXfuRKnFdAHNz + 2) * 10f));
				return MathTools.Clamp(num, 0f, 100f);
			}
		}

		public float LeftMotor
		{
			get
			{
				return vibrationMotors[0].Speed;
			}
			set
			{
				vibrationMotors[0].Speed = value;
			}
		}

		public float RightMotor
		{
			get
			{
				return vibrationMotors[1].Speed;
			}
			set
			{
				vibrationMotors[1].Speed = value;
			}
		}

		public float LightColorR
		{
			get
			{
				return lights[0].ColorR;
			}
			set
			{
				lights[0].ColorR = value;
			}
		}

		public float LightColorG
		{
			get
			{
				return lights[0].ColorG;
			}
			set
			{
				lights[0].ColorG = value;
			}
		}

		public float LightColorB
		{
			get
			{
				return lights[0].ColorB;
			}
			set
			{
				lights[0].ColorB = value;
			}
		}

		public float LightFlashOnDuration
		{
			get
			{
				return (int)nPgWllPLDwssOmvhECfyMrqDVwxu;
			}
			set
			{
				nPgWllPLDwssOmvhECfyMrqDVwxu = (byte)MathTools.Clamp(MathTools.Clamp(value, 0f, 2.5f) * 100f, 0f, 255f);
				KwzpPuaDrCLkgOgTRCqggBvGhEBh = true;
				if (nPgWllPLDwssOmvhECfyMrqDVwxu == 0 && ONJNyutLlCAXYqZIKEnpKSJLqjnb == 0)
				{
					ULLBncarwIzAwpoBsvQVRaOwwRtvA = true;
				}
			}
		}

		public float LightFlashOffDuration
		{
			get
			{
				return (int)ONJNyutLlCAXYqZIKEnpKSJLqjnb;
			}
			set
			{
				ONJNyutLlCAXYqZIKEnpKSJLqjnb = (byte)MathTools.Clamp(MathTools.Clamp(value, 0f, 2.5f) * 100f, 0f, 255f);
				KwzpPuaDrCLkgOgTRCqggBvGhEBh = true;
				if (nPgWllPLDwssOmvhECfyMrqDVwxu == 0 && ONJNyutLlCAXYqZIKEnpKSJLqjnb == 0)
				{
					ULLBncarwIzAwpoBsvQVRaOwwRtvA = true;
				}
			}
		}

		public Vector3 AccelerometerValue => FWcdEzEyWBdMxiSgKrCIQaECwCHGe(accelerometers[0].rawValue);

		public Vector3 AccelerometerValueRaw => new Vector3(accelerometers[0].rawValue[0], accelerometers[0].rawValue[1], accelerometers[0].rawValue[2]);

		public Vector3 GyroscopeValue => FEikdMlyEFIBnlAWMXsBdXggLpIl(gyroscopes[0].events);

		public Vector3 GyroscopeValueRaw => new Vector3(gyroscopes[0].rawValue[0], gyroscopes[0].rawValue[1], gyroscopes[0].rawValue[2]);

		public Vector3 LastGyroscopeValue
		{
			get
			{
				Vector3 vector = new Vector3(gyroscopes[0].lastRawValue[0], gyroscopes[0].lastRawValue[1], gyroscopes[0].lastRawValue[2]);
				return FEikdMlyEFIBnlAWMXsBdXggLpIl(vector, wLqvEwIHNMZuhtrqhsAlwwIFQXrI);
			}
		}

		public Vector3 LastGyroscopeValueRaw => new Vector3(gyroscopes[0].lastRawValue[0], gyroscopes[0].lastRawValue[1], gyroscopes[0].lastRawValue[2]);

		public Quaternion Orientation => ihPCDXHqXKildPsQIzfxuLnxeoPg;

		public int MaxTouches => 2;

		public void ResetOrientation()
		{
			ihPCDXHqXKildPsQIzfxuLnxeoPg = Quaternion.identity;
			bHpRFgezjLRiIFMjYufaVCZZYYSD = false;
		}

		public int GetTouchCount()
		{
			int num = 0;
			for (int i = 0; i < 2; i++)
			{
				if (touchpads[0].values[i].isTouching)
				{
					num++;
				}
			}
			return num;
		}

		public bool IsTouchingAtIndex(int index)
		{
			if (index < 0 || index >= 2)
			{
				return false;
			}
			return touchpads[0].values[index].isTouching;
		}

		public bool IsTouchingAtTouchId(int touchId)
		{
			return touchpads[0].IsTouching(touchId);
		}

		public int GetTouchIdAtIndex(int index)
		{
			if (index < 0 || index >= 2)
			{
				return -1;
			}
			return touchpads[0].values[index].touchId;
		}

		public bool GetTouchPositionByIndex(int index, out Vector2 position)
		{
			position = default(Vector2);
			if (index < 0 || index >= 2)
			{
				return false;
			}
			HIDTouchpad.TouchData[] values = touchpads[0].values;
			if (!values[index].isTouching)
			{
				return false;
			}
			position.x = values[index].positionX;
			position.y = values[index].positionY;
			return true;
		}

		public bool GetTouchPositionByTouchId(int touchId, out Vector2 position)
		{
			position = default(Vector2);
			if (!touchpads[0].IsTouching(touchId))
			{
				return false;
			}
			HIDTouchpad.TouchData[] values = touchpads[0].values;
			for (int i = 0; i < values.Length; i++)
			{
				if (values[i].isTouching)
				{
					position.x = values[i].positionX;
					position.y = values[i].positionY;
				}
			}
			return true;
		}

		public bool GetTouchPositionAbsoluteByIndex(int index, out int positionX, out int positionY)
		{
			positionX = 0;
			positionY = 0;
			if (index < 0 || index >= 2)
			{
				return false;
			}
			HIDTouchpad.TouchData[] values = touchpads[0].values;
			if (!values[index].isTouching)
			{
				return false;
			}
			positionX = values[index].positionAbsX;
			positionY = values[index].positionAbsY;
			return true;
		}

		public bool GetTouchPositionAbsoluteByTouchId(int touchId, out int positionX, out int positionY)
		{
			positionX = 0;
			positionY = 0;
			if (!touchpads[0].IsTouching(touchId))
			{
				return false;
			}
			HIDTouchpad.TouchData[] values = touchpads[0].values;
			for (int i = 0; i < values.Length; i++)
			{
				if (values[i].isTouching)
				{
					positionX = values[i].positionAbsX;
					positionY = values[i].positionAbsY;
				}
			}
			return true;
		}

		public void StopLightFlash()
		{
			nPgWllPLDwssOmvhECfyMrqDVwxu = 0;
			ONJNyutLlCAXYqZIKEnpKSJLqjnb = 0;
			KwzpPuaDrCLkgOgTRCqggBvGhEBh = true;
			ULLBncarwIzAwpoBsvQVRaOwwRtvA = true;
		}

		public void StopVibration()
		{
			int vibrationMotorCount = base.VibrationMotorCount;
			for (int i = 0; i < vibrationMotorCount; i++)
			{
				vibrationMotors[i].SpeedRaw = 0;
			}
		}

		public DualShock4Driver(InitArgs P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("initArgs");
			}
			ekpwlvHcPlMfYPFJKDKaXHiCkzhs = P_0.hatZeroValue;
			uXVJskFSOanKkrhkSxOQMBeHKfbN = P_0.hatSpan;
			pztukqONRdxCvQGBPEuNfbIMkFGDA = P_0.inputReportLength;
			RUJKBthdvVQqUCHNDAtXmuJoumgV = P_0.outputReportLength;
			nUfHkEItsYftJzYkqNavhDPHdlCe = P_0.synchronousWriteOutputReportDelegate;
			KMtNyOhjxErEtpBItWsLNqmherMHA = P_0.asynchronousWriteOutputReportDelegate;
			xbcZauqgqoSgkSZBEukVnAbsZDLn = P_0.getFeatureReportDelegate;
			xqxRNEDkGGHZgGrNQEMzeUHLqoEfb = P_0.connectionType;
			fDAHDJbARjWStNNVeAQVzofIxULQ = xqxRNEDkGGHZgGrNQEMzeUHLqoEfb == DeviceConnectionType.Bluetooth;
			if (fDAHDJbARjWStNNVeAQVzofIxULQ)
			{
				RUJKBthdvVQqUCHNDAtXmuJoumgV = 78;
			}
			if (RUJKBthdvVQqUCHNDAtXmuJoumgV < 23)
			{
				RUJKBthdvVQqUCHNDAtXmuJoumgV = 23;
			}
			HLsWXNxbuFUDCLhxfUYoppWgHBGi = new NativeBuffer(64);
			CAPBXhcnJamPmJsEbuPqwfyjKPHdb = new NativeBuffer(RUJKBthdvVQqUCHNDAtXmuJoumgV);
			BuQEXisXIqFXvdwXReGSUsjHrfng = new OutputReport(CAPBXhcnJamPmJsEbuPqwfyjKPHdb.Pointer, CAPBXhcnJamPmJsEbuPqwfyjKPHdb.Length, RUJKBthdvVQqUCHNDAtXmuJoumgV);
			lights = new HIDLight[1]
			{
				new HIDLight(11, 24, 28)
			};
			lights[0].ValueChangedEvent += bjgDOENuUUluVNEnLSRTUNPlGDnM;
			vibrationMotors = new HIDVibrationMotor[2]
			{
				new HIDVibrationMotor(0, 255),
				new HIDVibrationMotor(0, 255)
			};
			vibrationMotors[0].ValueChangedEvent += bjgDOENuUUluVNEnLSRTUNPlGDnM;
			vibrationMotors[1].ValueChangedEvent += bjgDOENuUUluVNEnLSRTUNPlGDnM;
			if (fDAHDJbARjWStNNVeAQVzofIxULQ)
			{
				BuQEXisXIqFXvdwXReGSUsjHrfng.options |= OutputReportOptions.WriteDirect;
				mNDdAkFBUZZNqoZtuijPtQcAcaOk = true;
				mNDdAkFBUZZNqoZtuijPtQcAcaOk = bROjQYFTsjmSyLGlwglxdUhGIYpT(patCHGFpzQFWiFajobvxWhTzPZfhA.Synchronous);
				if (!mNDdAkFBUZZNqoZtuijPtQcAcaOk)
				{
					BuQEXisXIqFXvdwXReGSUsjHrfng.options &= ~OutputReportOptions.WriteDirect;
				}
			}
			else
			{
				mNDdAkFBUZZNqoZtuijPtQcAcaOk = true;
				mNDdAkFBUZZNqoZtuijPtQcAcaOk = bROjQYFTsjmSyLGlwglxdUhGIYpT(patCHGFpzQFWiFajobvxWhTzPZfhA.Synchronous);
			}
			if (!mNDdAkFBUZZNqoZtuijPtQcAcaOk)
			{
				throw new Exception("Special features not supported so just treat this as a standard HID device.");
			}
			vPZjGKKCZiTkmypxFRYxoULOigAv = 1;
			htyqSPpfIfxkTMvTRsUkdMOHtgNH = 0;
			if (fDAHDJbARjWStNNVeAQVzofIxULQ && mNDdAkFBUZZNqoZtuijPtQcAcaOk)
			{
				vPZjGKKCZiTkmypxFRYxoULOigAv = 17;
				htyqSPpfIfxkTMvTRsUkdMOHtgNH = 2;
			}
			tFjliRmVNqRJBLgdGqbeGULvFMdU = 5 + htyqSPpfIfxkTMvTRsUkdMOHtgNH;
			uwDjQiabEjNvefpNjTtmGelTYOGR = 6 + htyqSPpfIfxkTMvTRsUkdMOHtgNH;
			UCQcafhwMxDbvyQzpJLDHdHejqygA = 7 + htyqSPpfIfxkTMvTRsUkdMOHtgNH;
			buttons = new HIDButton[14];
			for (int i = 0; i < 14; i++)
			{
				buttons[i] = new HIDButton(vPZjGKKCZiTkmypxFRYxoULOigAv, new HIDControllerElement.HIDInfo
				{
					usagePage = 9,
					usage = (ushort)i
				});
			}
			axes = new HIDAxis[6]
			{
				new HIDAxis(vPZjGKKCZiTkmypxFRYxoULOigAv, new HIDControllerElement.HIDInfo
				{
					usagePage = 1,
					usage = 48,
					dataIndex = 1 + htyqSPpfIfxkTMvTRsUkdMOHtgNH,
					bitSize = 8,
					logicalMin = 0,
					logicalMax = 255,
					physicalMin = 0,
					physicalMax = 0,
					units = 0u,
					unitsExp = 0u
				}, false, 127),
				new HIDAxis(vPZjGKKCZiTkmypxFRYxoULOigAv, new HIDControllerElement.HIDInfo
				{
					usagePage = 1,
					usage = 49,
					dataIndex = 2 + htyqSPpfIfxkTMvTRsUkdMOHtgNH,
					bitSize = 8,
					logicalMin = 0,
					logicalMax = 255,
					physicalMin = 0,
					physicalMax = 0,
					units = 0u,
					unitsExp = 0u
				}, false, 127),
				new HIDAxis(vPZjGKKCZiTkmypxFRYxoULOigAv, new HIDControllerElement.HIDInfo
				{
					usagePage = 1,
					usage = 50,
					dataIndex = 3 + htyqSPpfIfxkTMvTRsUkdMOHtgNH,
					bitSize = 8,
					logicalMin = 0,
					logicalMax = 255,
					physicalMin = 0,
					physicalMax = 0,
					units = 0u,
					unitsExp = 0u
				}, false, 127),
				new HIDAxis(vPZjGKKCZiTkmypxFRYxoULOigAv, new HIDControllerElement.HIDInfo
				{
					usagePage = 1,
					usage = 53,
					dataIndex = 4 + htyqSPpfIfxkTMvTRsUkdMOHtgNH,
					bitSize = 8,
					logicalMin = 0,
					logicalMax = 255,
					physicalMin = 0,
					physicalMax = 0,
					units = 0u,
					unitsExp = 0u
				}, false, 127),
				new HIDAxis(vPZjGKKCZiTkmypxFRYxoULOigAv, new HIDControllerElement.HIDInfo
				{
					usagePage = 1,
					usage = 51,
					dataIndex = 8 + htyqSPpfIfxkTMvTRsUkdMOHtgNH,
					bitSize = 8,
					logicalMin = 0,
					logicalMax = 255,
					physicalMin = 0,
					physicalMax = 315,
					units = 0u,
					unitsExp = 0u
				}, false, 0),
				new HIDAxis(vPZjGKKCZiTkmypxFRYxoULOigAv, new HIDControllerElement.HIDInfo
				{
					usagePage = 1,
					usage = 52,
					dataIndex = 9 + htyqSPpfIfxkTMvTRsUkdMOHtgNH,
					bitSize = 8,
					logicalMin = 0,
					logicalMax = 255,
					physicalMin = 0,
					physicalMax = 315,
					units = 0u,
					unitsExp = 0u
				}, false, 0)
			};
			hats = new HIDHat[1]
			{
				new HIDHat(vPZjGKKCZiTkmypxFRYxoULOigAv, new HIDControllerElement.HIDInfo
				{
					usagePage = 1,
					usage = 57,
					dataIndex = 5 + htyqSPpfIfxkTMvTRsUkdMOHtgNH,
					bitSize = 4,
					logicalMin = 0,
					logicalMax = 7,
					physicalMin = 0,
					physicalMax = 315,
					units = 20u,
					unitsExp = 0u
				}, lRpFsmaXNdatGBxyGDTFGnutrDYEA)
			};
			accelerometers = new HIDAccelerometer[1]
			{
				new HIDAccelerometer(vPZjGKKCZiTkmypxFRYxoULOigAv, new HIDControllerElement.HIDInfo
				{
					usagePage = 1,
					dataIndex = 19 + htyqSPpfIfxkTMvTRsUkdMOHtgNH,
					bitSize = 48
				}, 3, fVJsvXRmLprokhErKbSaAJNmRHGE)
			};
			gyroscopes = new HIDGyroscope[1]
			{
				new HIDGyroscope(P_0.updateLoopSetting, vPZjGKKCZiTkmypxFRYxoULOigAv, new HIDControllerElement.HIDInfo
				{
					usagePage = 1,
					dataIndex = 13 + htyqSPpfIfxkTMvTRsUkdMOHtgNH,
					bitSize = 48
				}, 3, 25, uhYotKaDyBCRSdIqqcJnRRCGUgrF, zeLdUqyICWRtPyuZDrgACpSMiWgI)
			};
			touchpads = new HIDTouchpad[1]
			{
				new HIDTouchpad(vPZjGKKCZiTkmypxFRYxoULOigAv, new HIDTouchpad.TouchpadInfo(2, 0, 1912, 0, 941, false, true), new HIDControllerElement.HIDInfo
				{
					usagePage = 1,
					dataIndex = 35 + htyqSPpfIfxkTMvTRsUkdMOHtgNH,
					bitSize = 48
				}, ebDtzwuulRdagjvqRzmyJrWBARvf)
			};
			sMsPthWihMdOEjJCEDJfgIInICwt = ReInput.realTime;
		}

		public override void Update(UpdateLoopType updateLoop)
		{
			AmtrPsgvzwlDLKJIvYNsxdiatRhL();
			XVmxtFleoxnJXNqnhRlPbvnGVCbD(patCHGFpzQFWiFajobvxWhTzPZfhA.Asynchronous);
		}

		public override bool ParseInputReport(IntPtr inputReportPtr, int inputReportLength, double timestamp)
		{
			if (inputReportPtr == IntPtr.Zero)
			{
				return false;
			}
			if (inputReportLength < HLsWXNxbuFUDCLhxfUYoppWgHBGi.Length)
			{
				return false;
			}
			PrECXiktcHTaMnkJTbhtpwfhaLwjA = (float)(timestamp - sMsPthWihMdOEjJCEDJfgIInICwt);
			sMsPthWihMdOEjJCEDJfgIInICwt = timestamp;
			HLsWXNxbuFUDCLhxfUYoppWgHBGi.Write(inputReportPtr, inputReportLength, HLsWXNxbuFUDCLhxfUYoppWgHBGi.Length);
			VZmBjhgCYOGvBfRLcopkGwSWjcIC(HLsWXNxbuFUDCLhxfUYoppWgHBGi);
			MGEvRBxTXmrzVAbFgmVqKAAmKjlj(HLsWXNxbuFUDCLhxfUYoppWgHBGi, timestamp);
			HIDControllerElement[] array = axes;
			qWGyimoreyEdVdLhdiNleELnOMKsA(array, HLsWXNxbuFUDCLhxfUYoppWgHBGi, timestamp);
			array = hats;
			qWGyimoreyEdVdLhdiNleELnOMKsA(array, HLsWXNxbuFUDCLhxfUYoppWgHBGi, timestamp);
			array = accelerometers;
			qWGyimoreyEdVdLhdiNleELnOMKsA(array, HLsWXNxbuFUDCLhxfUYoppWgHBGi, timestamp);
			array = gyroscopes;
			qWGyimoreyEdVdLhdiNleELnOMKsA(array, HLsWXNxbuFUDCLhxfUYoppWgHBGi, timestamp);
			array = touchpads;
			qWGyimoreyEdVdLhdiNleELnOMKsA(array, HLsWXNxbuFUDCLhxfUYoppWgHBGi, timestamp);
			JrgUdyTuBsdMhTzLXfuRKnFdAHNz = (byte)(HLsWXNxbuFUDCLhxfUYoppWgHBGi[30 + htyqSPpfIfxkTMvTRsUkdMOHtgNH] & 0xF);
			HQnmtoMCtxgtOxBfNBvbJDyhwEno();
			return true;
		}

		public override Controller.Extension CreateControllerExtension()
		{
			return new DualShock4Extension(this);
		}

		private void XVmxtFleoxnJXNqnhRlPbvnGVCbD(patCHGFpzQFWiFajobvxWhTzPZfhA P_0)
		{
			if (KwzpPuaDrCLkgOgTRCqggBvGhEBh)
			{
				bROjQYFTsjmSyLGlwglxdUhGIYpT(P_0);
				KwzpPuaDrCLkgOgTRCqggBvGhEBh = false;
			}
		}

		private bool bROjQYFTsjmSyLGlwglxdUhGIYpT(patCHGFpzQFWiFajobvxWhTzPZfhA P_0)
		{
			WQGtVGagMjJCQPcACQAcaZKqfuhJ();
			bool result = zSmfRRtQOmVGUcQwVOBYunHIHatM(P_0);
			if (ULLBncarwIzAwpoBsvQVRaOwwRtvA)
			{
				result = zSmfRRtQOmVGUcQwVOBYunHIHatM(P_0);
				ULLBncarwIzAwpoBsvQVRaOwwRtvA = false;
			}
			return result;
		}

		private void WQGtVGagMjJCQPcACQAcaZKqfuhJ()
		{
			if (fDAHDJbARjWStNNVeAQVzofIxULQ && mNDdAkFBUZZNqoZtuijPtQcAcaOk)
			{
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[0] = 17;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[1] = 128;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[3] = byte.MaxValue;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[6] = (byte)vibrationMotors[1].SpeedRaw;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[7] = (byte)vibrationMotors[0].SpeedRaw;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[8] = lights[0].ColorRRaw;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[9] = lights[0].ColorGRaw;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[10] = lights[0].ColorBRaw;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[11] = nPgWllPLDwssOmvhECfyMrqDVwxu;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[12] = ONJNyutLlCAXYqZIKEnpKSJLqjnb;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[21] = 53;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[22] = 53;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[23] = byte.MaxValue;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[24] = 0;
			}
			else
			{
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[0] = 5;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[1] = byte.MaxValue;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[4] = (byte)vibrationMotors[1].SpeedRaw;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[5] = (byte)vibrationMotors[0].SpeedRaw;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[6] = lights[0].ColorRRaw;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[7] = lights[0].ColorGRaw;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[8] = lights[0].ColorBRaw;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[9] = nPgWllPLDwssOmvhECfyMrqDVwxu;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[10] = ONJNyutLlCAXYqZIKEnpKSJLqjnb;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[19] = 53;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[20] = 53;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[21] = byte.MaxValue;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[22] = 0;
			}
		}

		private bool zSmfRRtQOmVGUcQwVOBYunHIHatM(patCHGFpzQFWiFajobvxWhTzPZfhA P_0)
		{
			ttLTFCslQtbuRHfbjcFpXrsRMUGfb = ReInput.realTime + 4.0;
			switch (P_0)
			{
			case patCHGFpzQFWiFajobvxWhTzPZfhA.Synchronous:
				if (nUfHkEItsYftJzYkqNavhDPHdlCe == null)
				{
					return false;
				}
				return nUfHkEItsYftJzYkqNavhDPHdlCe(BuQEXisXIqFXvdwXReGSUsjHrfng);
			case patCHGFpzQFWiFajobvxWhTzPZfhA.Asynchronous:
				if (KMtNyOhjxErEtpBItWsLNqmherMHA == null)
				{
					return false;
				}
				KMtNyOhjxErEtpBItWsLNqmherMHA(BuQEXisXIqFXvdwXReGSUsjHrfng);
				return true;
			default:
				throw new NotImplementedException();
			}
		}

		private void MGEvRBxTXmrzVAbFgmVqKAAmKjlj(NativeBuffer P_0, double P_1)
		{
			byte b = P_0[tFjliRmVNqRJBLgdGqbeGULvFMdU];
			buttons[0].SetValue((b & 0x10) != 0, P_1);
			buttons[1].SetValue((b & 0x20) != 0, P_1);
			buttons[2].SetValue((b & 0x40) != 0, P_1);
			buttons[3].SetValue((b & 0x80) != 0, P_1);
			b = P_0[uwDjQiabEjNvefpNjTtmGelTYOGR];
			buttons[4].SetValue((b & 1) != 0, P_1);
			buttons[5].SetValue((b & 2) != 0, P_1);
			buttons[6].SetValue((b & 4) != 0, P_1);
			buttons[7].SetValue((b & 8) != 0, P_1);
			buttons[8].SetValue((b & 0x10) != 0, P_1);
			buttons[9].SetValue((b & 0x20) != 0, P_1);
			buttons[10].SetValue((b & 0x40) != 0, P_1);
			buttons[11].SetValue((b & 0x80) != 0, P_1);
			b = P_0[UCQcafhwMxDbvyQzpJLDHdHejqygA];
			buttons[12].SetValue((b & 1) != 0, P_1);
			buttons[13].SetValue((b & 2) != 0, P_1);
		}

		private void qWGyimoreyEdVdLhdiNleELnOMKsA(HIDControllerElement[] P_0, NativeBuffer P_1, double P_2)
		{
			for (int i = 0; i < P_0.Length; i++)
			{
				P_0[i].UpdateValue(P_1, P_2);
			}
		}

		private void AmtrPsgvzwlDLKJIvYNsxdiatRhL()
		{
			if (isVibrating && ReInput.realTime >= ttLTFCslQtbuRHfbjcFpXrsRMUGfb)
			{
				KwzpPuaDrCLkgOgTRCqggBvGhEBh = true;
			}
		}

		private void VZmBjhgCYOGvBfRLcopkGwSWjcIC(NativeBuffer P_0)
		{
			if (mNDdAkFBUZZNqoZtuijPtQcAcaOk)
			{
				ushort num = HLsWXNxbuFUDCLhxfUYoppWgHBGi.ReadUShort(10 + htyqSPpfIfxkTMvTRsUkdMOHtgNH);
				float num3;
				if (num != JXRJRwsAtReVqNSrQgxDsJidJLwn)
				{
					int num2 = ((num >= JXRJRwsAtReVqNSrQgxDsJidJLwn) ? (num - JXRJRwsAtReVqNSrQgxDsJidJLwn) : (num + 65535 - JXRJRwsAtReVqNSrQgxDsJidJLwn));
					num3 = (float)num2 / 187500f;
				}
				else
				{
					int num2 = 0;
					num3 = 0f;
				}
				JXRJRwsAtReVqNSrQgxDsJidJLwn = num;
				wLqvEwIHNMZuhtrqhsAlwwIFQXrI = num3;
			}
		}

		private void HQnmtoMCtxgtOxBfNBvbJDyhwEno()
		{
			if (mNDdAkFBUZZNqoZtuijPtQcAcaOk)
			{
				_ = wLqvEwIHNMZuhtrqhsAlwwIFQXrI;
				_ = 0f;
				Vector3 vector = FEikdMlyEFIBnlAWMXsBdXggLpIl(new Vector3(gyroscopes[0].lastRawValue[0], gyroscopes[0].lastRawValue[1], gyroscopes[0].lastRawValue[2]), wLqvEwIHNMZuhtrqhsAlwwIFQXrI);
				ojqxjGeRaGDRUAQvrbJhULDoReYh(ref vector);
				Vector3 vector2 = new Vector3(accelerometers[0].rawValue[0] * -1f, accelerometers[0].rawValue[1] * -1f, accelerometers[0].rawValue[2] * -1f);
				YZJgCRZYWZDCUeuOoiisJcujHnqR(vector2, vector);
			}
		}

		private static bool ojqxjGeRaGDRUAQvrbJhULDoReYh(ref Vector3 P_0)
		{
			if (P_0.magnitude < 0.004f)
			{
				P_0.x = 0f;
				P_0.y = 0f;
				P_0.z = 0f;
				return false;
			}
			return true;
		}

		private void YZJgCRZYWZDCUeuOoiisJcujHnqR(Vector3 P_0, Vector3 P_1)
		{
			Quaternion quaternion = Quaternion.Euler(P_1);
			float sqrMagnitude = P_0.sqrMagnitude;
			if (sqrMagnitude > 16777216f && sqrMagnitude < 268435460f && jsgdUxVZaFxxIIRNWogijkrfDfPJA(P_0, out var fstLtwIvIVaQdqeviUnLQgFKVKNN2))
			{
				Quaternion a = ihPCDXHqXKildPsQIzfxuLnxeoPg * quaternion;
				if (!bHpRFgezjLRiIFMjYufaVCZZYYSD)
				{
					bHpRFgezjLRiIFMjYufaVCZZYYSD = true;
					ekOqcOMlzPQfEATHHqxXeKZIhArc = Quaternion.identity * Quaternion.Euler(new Vector3(90f, 0f, 0f));
					puefNXgPSijNJiZwlSsoDsuSgGLbb = ihPCDXHqXKildPsQIzfxuLnxeoPg;
				}
				ekOqcOMlzPQfEATHHqxXeKZIhArc *= quaternion;
				puefNXgPSijNJiZwlSsoDsuSgGLbb *= quaternion;
				Quaternion b;
				if ((fstLtwIvIVaQdqeviUnLQgFKVKNN2 & fstLtwIvIVaQdqeviUnLQgFKVKNN.XZ) != fstLtwIvIVaQdqeviUnLQgFKVKNN.None)
				{
					b = UZYBKJeXZMJQnHhrvwMfWfkYtObOA(P_0, a.eulerAngles.y);
				}
				else if ((fstLtwIvIVaQdqeviUnLQgFKVKNN2 & fstLtwIvIVaQdqeviUnLQgFKVKNN.Y) != fstLtwIvIVaQdqeviUnLQgFKVKNN.None)
				{
					b = TqxKvbHqfpNSQGCPbdPaEjjUrvabA(P_0);
					Vector3 vector = puefNXgPSijNJiZwlSsoDsuSgGLbb * Vector3.right;
					float y = 0f - MathTools.SignedAngle(new Vector3(vector.x, 0f, vector.z), Vector3.right, Vector3.up);
					b = Quaternion.Euler(0f, y, 0f) * b;
				}
				else
				{
					b = Quaternion.identity;
				}
				ihPCDXHqXKildPsQIzfxuLnxeoPg = Quaternion.Lerp(a, b, 0.01999998f);
			}
			else
			{
				ihPCDXHqXKildPsQIzfxuLnxeoPg *= quaternion;
				if (bHpRFgezjLRiIFMjYufaVCZZYYSD)
				{
					bHpRFgezjLRiIFMjYufaVCZZYYSD = false;
				}
			}
		}

		private static Quaternion hbqosotFIGImTZAbOxcYBAxRLLOc(Quaternion P_0, Vector3 P_1)
		{
			Vector3 vector = RjczkChOXXGDgbUPSYEHDqscIpLfA(new Vector3(P_0.x, P_0.y, P_0.z), P_1);
			return new Quaternion(vector.x, vector.y, vector.z, P_0.w);
		}

		private static Vector3 RjczkChOXXGDgbUPSYEHDqscIpLfA(Vector3 P_0, Vector3 P_1)
		{
			float num = Vector3.Dot(P_1, P_1);
			if (num < float.Epsilon)
			{
				return Vector3.zero;
			}
			return P_1 * Vector3.Dot(P_0, P_1) / num;
		}

		private Quaternion tdWybmfuyQIgdRqzbNuSROxQbyUAA(Quaternion P_0, jdKRWCkCgQtVyUxhMexbTsrigjYG P_1)
		{
			Vector4 vector = default(Vector4);
			if (MathTools.Approximately(P_0.w, 0f) && MathTools.Approximately(P_0[(int)P_1], 0f))
			{
				P_0 = Quaternion.identity;
			}
			else
			{
				float num = P_0[(int)P_1];
				float num2 = MathTools.Sqrt(P_0.w * P_0.w + num * num);
				vector[3] = P_0.w / num2;
				vector[(int)P_1] = num / num2;
				P_0 = new Quaternion(vector[0], vector[1], vector[2], vector[3]);
			}
			return P_0;
		}

		public static Quaternion Inverse(Quaternion quaternion)
		{
			float num = quaternion.x * quaternion.x + quaternion.y * quaternion.y + quaternion.z * quaternion.z + quaternion.w * quaternion.w;
			float num2 = 1f / num;
			Quaternion result = default(Quaternion);
			result.x = (0f - quaternion.x) * num2;
			result.y = (0f - quaternion.y) * num2;
			result.z = (0f - quaternion.z) * num2;
			result.w = quaternion.w * num2;
			return result;
		}

		private float VpJECLuufDPjfLWbYRBMzCIvrhpF(float P_0, float P_1)
		{
			P_0 = MathTools.ClampAngle360(P_0);
			P_1 = MathTools.ClampAngle360(P_1);
			if (P_0 == P_1)
			{
				return 0f;
			}
			if (P_0 >= 180f)
			{
				P_0 -= 360f;
			}
			if (P_1 >= 180f)
			{
				P_1 -= 360f;
			}
			return P_0 - P_1;
		}

		private Vector3 HRryyqelNypcuxadygVGGsrwfaFj(Vector3 P_0, float P_1 = 0f)
		{
			float num = MathTools.Atan2(P_0.z, P_0.y);
			float num2 = MathTools.Atan2(x: MathTools.Sqrt(MathTools.Pow(P_0.y, 2f) + MathTools.Pow(P_0.z, 2f)), y: P_0.x);
			float x = num * 57.29578f + 180f;
			float z = (0f - num2) * 57.29578f;
			return new Vector3(x, P_1, z);
		}

		private Quaternion UZYBKJeXZMJQnHhrvwMfWfkYtObOA(Vector3 P_0, float P_1 = 0f)
		{
			float num = MathTools.Atan2(P_0.z, P_0.y);
			float num2 = MathTools.Atan2(x: MathTools.Sqrt(MathTools.Pow(P_0.y, 2f) + MathTools.Pow(P_0.z, 2f)), y: P_0.x);
			float x = num * 57.29578f + 180f;
			float z = (0f - num2) * 57.29578f;
			return Quaternion.Euler(x, P_1, z);
		}

		private Quaternion TqxKvbHqfpNSQGCPbdPaEjjUrvabA(Vector3 P_0, float P_1 = 0f)
		{
			float num = MathTools.Atan2(P_0.z, P_0.y);
			float x = MathTools.Sqrt(MathTools.Pow(P_0.y, 2f) + MathTools.Pow(P_0.z, 2f));
			float num2 = MathTools.Atan2(P_0.x, x);
			float x2 = num * 57.29578f + 180f;
			float z = (0f - num2) * 57.29578f;
			Quaternion quaternion = Quaternion.Euler(0f, 0f, z) * Quaternion.Euler(x2, 0f, 0f);
			if (P_1 != 0f)
			{
				return quaternion * Quaternion.Euler(0f, P_1, 0f);
			}
			return quaternion;
		}

		private float hulwgPoOoOqnmKITyyOCSNqmLRqX(Vector3 P_0)
		{
			return MathTools.Atan2(P_0.x, P_0.z) * 57.29578f;
		}

		private bool qhSkfSIHPzUZOCqwjliGRdMyswxU(float P_0)
		{
			if (P_0 >= 45f)
			{
				return P_0 <= 70f;
			}
			return false;
		}

		private bool jsgdUxVZaFxxIIRNWogijkrfDfPJA(Vector3 P_0, out fstLtwIvIVaQdqeviUnLQgFKVKNN P_1)
		{
			P_0.Normalize();
			P_1 = fstLtwIvIVaQdqeviUnLQgFKVKNN.None;
			bool result = false;
			if (NejbHGFhHdfCNjRCCpqPDhfawqZHc(P_0))
			{
				result = true;
				P_1 |= fstLtwIvIVaQdqeviUnLQgFKVKNN.XZ;
			}
			if (uwHidFBblQHYYjLIuLbxsFLDWiBr(P_0))
			{
				result = true;
				P_1 |= fstLtwIvIVaQdqeviUnLQgFKVKNN.Y;
			}
			return result;
		}

		private bool NejbHGFhHdfCNjRCCpqPDhfawqZHc(Vector3 P_0)
		{
			if (P_0.y > 0f)
			{
				return false;
			}
			if (Vector3.Angle(Vector3.down, P_0) > 45f)
			{
				return false;
			}
			return true;
		}

		private bool uwHidFBblQHYYjLIuLbxsFLDWiBr(Vector3 P_0)
		{
			if (P_0.z < 0f)
			{
				return false;
			}
			if (Vector3.Angle(new Vector3(0f, 0f, 1f), P_0) > 20f)
			{
				return false;
			}
			return true;
		}

		private Vector3 FWcdEzEyWBdMxiSgKrCIQaECwCHGe(float[] P_0)
		{
			return new Vector3(P_0[0] * 0.00012207031f * -1f, P_0[1] * 0.00012207031f * -1f, P_0[2] * 0.00012207031f);
		}

		private Vector3 FEikdMlyEFIBnlAWMXsBdXggLpIl(ExpandableArray_DataContainer<HIDGyroscope.vumlNYaXVVyzQKQsUZwqOQVbUSLf> P_0)
		{
			Vector3 result = default(Vector3);
			int count = P_0.Count;
			for (int i = 0; i < count; i++)
			{
				HIDGyroscope.vumlNYaXVVyzQKQsUZwqOQVbUSLf vumlNYaXVVyzQKQsUZwqOQVbUSLf = P_0[i];
				result += FEikdMlyEFIBnlAWMXsBdXggLpIl(vumlNYaXVVyzQKQsUZwqOQVbUSLf.XMLhFdSuDtJBozGJAApmtuOmajzY, vumlNYaXVVyzQKQsUZwqOQVbUSLf.cXGCxvAxxQZVPCFzsmouaVkAGZKV);
			}
			return result;
		}

		private Vector3 FEikdMlyEFIBnlAWMXsBdXggLpIl(Vector3 P_0, float P_1)
		{
			P_0.x *= -1f;
			P_0.y *= -1f;
			return P_0 * 0.06103702f * P_1;
		}

		private int lRpFsmaXNdatGBxyGDTFGnutrDYEA(int P_0)
		{
			P_0 &= 0xF;
			return P_0;
		}

		private void fVJsvXRmLprokhErKbSaAJNmRHGE(byte[] P_0, float[] P_1)
		{
			P_1[0] = BitConverter.ToInt16(P_0, 0);
			P_1[1] = BitConverter.ToInt16(P_0, 2);
			P_1[2] = BitConverter.ToInt16(P_0, 4);
		}

		private void uhYotKaDyBCRSdIqqcJnRRCGUgrF(byte[] P_0, float[] P_1)
		{
			P_1[0] = BitConverter.ToInt16(P_0, 0);
			P_1[1] = BitConverter.ToInt16(P_0, 2);
			P_1[2] = BitConverter.ToInt16(P_0, 4);
		}

		private float zeLdUqyICWRtPyuZDrgACpSMiWgI()
		{
			return wLqvEwIHNMZuhtrqhsAlwwIFQXrI;
		}

		private void ebDtzwuulRdagjvqRzmyJrWBARvf(NativeBuffer P_0, HIDTouchpad.TouchData[] P_1)
		{
			int num = 35 + htyqSPpfIfxkTMvTRsUkdMOHtgNH;
			int positionRawX = P_0[1 + num] + (P_0[2 + num] & 0xF) * 255;
			int positionRawY = ((P_0[2 + num] & 0xF0) >> 4) + P_0[3 + num] * 16;
			int positionRawX2 = P_0[5 + num] + (P_0[6 + num] & 0xF) * 255;
			int positionRawY2 = ((P_0[6 + num] & 0xF0) >> 4) + P_0[7 + num] * 16;
			byte b = P_0[num];
			bool flag = b < 128;
			byte num2 = P_0[num + 4];
			bool flag2 = num2 < 128;
			int num3 = b & 0x7F;
			int num4 = num2 & 0x7F;
			P_1[0].isTouching = flag;
			P_1[0].touchId = cjudKUuoREQsIpBKuyuHgTxGtOJq(0, flag, num3);
			P_1[0].positionRawX = positionRawX;
			P_1[0].positionRawY = positionRawY;
			P_1[1].isTouching = flag2;
			P_1[1].touchId = cjudKUuoREQsIpBKuyuHgTxGtOJq(1, flag2, num4);
			P_1[1].positionRawX = positionRawX2;
			P_1[1].positionRawY = positionRawY2;
		}

		private int cjudKUuoREQsIpBKuyuHgTxGtOJq(int P_0, bool P_1, int P_2)
		{
			if (!P_1)
			{
				mqTdiUwCjTjroDyCFFzlIQkHsFmr[P_0] = -1;
				HmgQugGcjVFKWEAAoSiGqAbRaXjJA[P_0] = P_2;
				return -1;
			}
			if (P_2 != HmgQugGcjVFKWEAAoSiGqAbRaXjJA[P_0])
			{
				int num = aMrCHiGAjvpMXtinFAhtXyeWiqCdA;
				if (aMrCHiGAjvpMXtinFAhtXyeWiqCdA == int.MaxValue)
				{
					aMrCHiGAjvpMXtinFAhtXyeWiqCdA = 0;
				}
				else
				{
					aMrCHiGAjvpMXtinFAhtXyeWiqCdA++;
				}
				HmgQugGcjVFKWEAAoSiGqAbRaXjJA[P_0] = P_2;
				mqTdiUwCjTjroDyCFFzlIQkHsFmr[P_0] = num;
				return num;
			}
			return mqTdiUwCjTjroDyCFFzlIQkHsFmr[P_0];
		}

		private void bjgDOENuUUluVNEnLSRTUNPlGDnM()
		{
			KwzpPuaDrCLkgOgTRCqggBvGhEBh = true;
		}

		~DualShock4Driver()
		{
			Dispose(disposing: false);
		}

		protected override void Dispose(bool disposing)
		{
			if (base.disposed)
			{
				return;
			}
			base.Dispose(disposing);
			if (disposing)
			{
				StopVibration();
				XVmxtFleoxnJXNqnhRlPbvnGVCbD(patCHGFpzQFWiFajobvxWhTzPZfhA.Synchronous);
				if (HLsWXNxbuFUDCLhxfUYoppWgHBGi != null)
				{
					HLsWXNxbuFUDCLhxfUYoppWgHBGi.Dispose();
				}
				if (CAPBXhcnJamPmJsEbuPqwfyjKPHdb != null)
				{
					CAPBXhcnJamPmJsEbuPqwfyjKPHdb.Dispose();
				}
			}
		}

		public static bool Matches(int vid, int pid)
		{
			for (int i = 0; i < Consts.pidVids_sony_dualShock4.Count; i++)
			{
				if (Consts.pidVids_sony_dualShock4[i].vendorId == vid && Consts.pidVids_sony_dualShock4[i].productId == pid)
				{
					return true;
				}
			}
			return false;
		}
	}
}
