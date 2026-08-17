using System;
using Rewired.ControllerExtensions;
using Rewired.Drivers.Interfaces;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using UnityEngine;

namespace Rewired.HID.Drivers
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class DualSenseDriver : HIDDeviceDriver, IDisposable, IControllerDriver, IDriver_DualSense
	{
		private enum rbpsDScoOmbWLehGUBTzhryusOEsA
		{
			X = 0,
			Y = 1,
			Z = 2
		}

		private enum tzEKFlRMaxNimwXFiEHhnbrVOTfe
		{
			None = 0,
			XZ = 1,
			Y = 2
		}

		public enum gcBgeujMQKwjpvqLScilprYNLiTU : byte
		{
			High = 0,
			Medium = 1,
			Low = 2
		}

		private const float oYhgiVHWYRhSmetHbOEAAZfNDkpbA = 4f;

		private const int VnQiGrFElxZQckFfuctAXoBRfYio = 15;

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

		private const int lrjcXzfDvsNTHOKVKDoqKLcENPiZ = 49;

		private const int BSgTgtWhVGzvVhrIaomwrbjTAIyAA = 0;

		private const int LPWjIJitCdNlEBhxYLPaOgbUmMrC = 1;

		private const int YZYFKODfnQIXvcrIaaTAZiohWjbL = 64;

		private const int dLRdbNzqhLfNuDkperkcgIZkBAubA = 48;

		private const int hQuddMKEnNKoqXUgFKpvtPlTVXUu = 547;

		private const int SfJTLEYGzlMOknmIKIkeLSfbxiWi = 64;

		private const int eYRCyzOdNNQbebJVUbglaVIwdkCA = 547;

		private const int rXQhAqeXVjEtdaeohSLphBdJANCM = 1;

		private const int QfYdZcwkayAvZdCDuugpgmfzkbceA = 2;

		private const int DzaRRIWzKJwwHSxpeNLYohrEcSZjA = 3;

		private const int NETxQtxDbDuQNWjqBPmdHLROaXvL = 4;

		private const int jtJcMoQIPZrUcmdDViDBhevaNHuTA = 5;

		private const int krEYJDRWNlIYBAldKnbhfHfJswKk = 6;

		private const int ydojHvsUOrPlORFlxbxbMfjpcCnW = 8;

		private const int eGdFoEVAwMwMNaWPeoSYZtLeeKuX = 22;

		private const int OPGozUEAdfpTIweLaFTzamiYgLBAb = 16;

		private const int FCWcSYGGJCXkICHoMvNyzceBCAAZ = 33;

		private const int wRpaEgloiUtSIMnBUQqQgXAtvGQj = 8;

		private const int RmlhUvxQfUSYHRWkRbcNUtkNZKqk = 9;

		private const int pqAJxQPmqEXXwSQDvbeFoDBGBCTu = 10;

		private const int BShCzWEUMMkNRRISgHeYrgyAKeBxA = 28;

		private const int LEhfkRSLRqzNEAdmEzcXvwiRXZlh = 54;

		private const int JaghLkDwHXUZsDMazPYvsmWqQFeNA = 55;

		private const int UjrVdJYpKpJHnpQBvGYmhcCerWMs = 54;

		private const bool rYQEgqARilSnhOeGWjvUtVLbisqPA = true;

		private const int SOpLCXhgrxApahqNSlYFZBeDBlzN = 25;

		private const int euZbolgWQrVReHjPVBQUSaUHTexhA = 3000000;

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

		private OutputReport BuQEXisXIqFXvdwXReGSUsjHrfng;

		private readonly Func<OutputReport, bool> nUfHkEItsYftJzYkqNavhDPHdlCe;

		private readonly Action<OutputReport> KMtNyOhjxErEtpBItWsLNqmherMHA;

		private bool KwzpPuaDrCLkgOgTRCqggBvGhEBh;

		private bool ULLBncarwIzAwpoBsvQVRaOwwRtvA;

		private double ttLTFCslQtbuRHfbjcFpXrsRMUGfb;

		private byte JrgUdyTuBsdMhTzLXfuRKnFdAHNz;

		private bool SjxtcqPdmhjLyDbGJOzLGCnAThMCB;

		private bool DsGAqceMmOTkIADNeQypgRLAnzon;

		private bool MmgGhpCDtCBhYFPtcFhoPBAaIBmiB;

		private Quaternion ihPCDXHqXKildPsQIzfxuLnxeoPg = Quaternion.identity;

		private DualSenseMicrophoneLightMode SZuYIRtPktAdSPkPrPiJxBrsTyXV;

		private gcBgeujMQKwjpvqLScilprYNLiTU atNJRaLRKabUieTrDlvmqCbduEsFA;

		private DualSensePlayerLightFlags zjOLnWOrhBGvnKUKtjLfCeKbBKXe;

		private bool HLtUeXlgAUlbQvlddcCQGGeUHXmI;

		private bool GVzcvSBgBaoaWVuyUcVevLKKEFVHb;

		private uint JXRJRwsAtReVqNSrQgxDsJidJLwn;

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

		private static uint[] cBNiFZRLhxIRKBaIlkMzCnogdNgUb = new uint[256]
		{
			3523407757u, 2768625435u, 1007455905u, 1259060791u, 3580832660u, 2724731650u, 996231864u, 1281784366u, 3705235391u, 2883475241u,
			852952723u, 1171273221u, 3686048678u, 2897449776u, 901431946u, 1119744540u, 3484811241u, 3098726271u, 565944005u, 1455205971u,
			3369614320u, 3219065702u, 651582172u, 1372678730u, 3245242331u, 3060352845u, 794826487u, 1483155041u, 3322131394u, 2969862996u,
			671994606u, 1594548856u, 3916222277u, 2657877971u, 123907689u, 1885708031u, 3993045852u, 2567322570u, 1010288u, 1997036262u,
			3887548279u, 2427484129u, 163128923u, 2126386893u, 3772416878u, 2547889144u, 248832578u, 2043925204u, 4108050209u, 2212294583u,
			450215437u, 1842515611u, 4088798008u, 2226203566u, 498629140u, 1790921346u, 4194326291u, 2366072709u, 336475711u, 1661535913u,
			4251816714u, 2322244508u, 325317158u, 1684325040u, 2766056989u, 3554254475u, 1255198513u, 1037565863u, 2746444292u, 3568589458u,
			1304234792u, 985283518u, 2852464175u, 3707901625u, 1141589763u, 856455061u, 2909332022u, 3664761504u, 1130791706u, 878818188u,
			3110715001u, 3463352047u, 1466425173u, 543223747u, 3187964512u, 3372436214u, 1342839628u, 655174618u, 3081909835u, 3233089245u,
			1505515367u, 784033777u, 2967466578u, 3352871620u, 1590793086u, 701932520u, 2679148245u, 3904355907u, 1908338681u, 112844655u,
			2564639436u, 4024072794u, 1993550816u, 30677878u, 2439710439u, 3865851505u, 2137352139u, 140662621u, 2517025534u, 3775001192u,
			2013832146u, 252678980u, 2181537457u, 4110462503u, 1812594589u, 453955339u, 2238339752u, 4067256894u, 1801730948u, 476252946u,
			2363233923u, 4225443349u, 1657960367u, 366298937u, 2343686810u, 4239843852u, 1707062198u, 314082080u, 1069182125u, 1220369467u,
			3518238081u, 2796764439u, 953657524u, 1339070498u, 3604597144u, 2715744526u, 828499103u, 1181144073u, 3748627891u, 2825434405u,
			906764422u, 1091244048u, 3624026538u, 2936369468u, 571309257u, 1426738271u, 3422756325u, 3137613171u, 627095760u, 1382516806u,
			3413039612u, 3161057642u, 752284923u, 1540473965u, 3268974039u, 3051332929u, 733688034u, 1555824756u, 3316994510u, 2998034776u,
			81022053u, 1943239923u, 3940166985u, 2648514015u, 62490748u, 1958656234u, 3988253008u, 2595281350u, 168805463u, 2097738945u,
			3825313147u, 2466682349u, 224526414u, 2053451992u, 3815530850u, 2490061300u, 425942017u, 1852075159u, 4151131437u, 2154433979u,
			504272920u, 1762240654u, 4026595636u, 2265434530u, 397988915u, 1623188645u, 4189500703u, 2393998729u, 282398762u, 1741824188u,
			4275794182u, 2312913296u, 1231433021u, 1046551979u, 2808630289u, 3496967303u, 1309403428u, 957143474u, 2684717064u, 3607279774u,
			1203610895u, 817534361u, 2847130659u, 3736401077u, 1087398166u, 936857984u, 2933784634u, 3654889644u, 1422998873u, 601230799u,
			3135200373u, 3453512931u, 1404893504u, 616286678u, 3182598252u, 3400902906u, 1510651243u, 755860989u, 3020215367u, 3271812305u,
			1567060338u, 710951396u, 3010007134u, 3295551688u, 1913130485u, 84884835u, 2617666777u, 3942734927u, 1969605100u, 40040826u,
			2607524032u, 3966539862u, 2094237127u, 198489425u, 2464015595u, 3856323709u, 2076066270u, 213479752u, 2511347954u, 3803648100u,
			1874795921u, 414723335u, 2175892669u, 4139142187u, 1758648712u, 534112542u, 2262612132u, 4057696306u, 1633981859u, 375629109u,
			2406151311u, 4167943193u, 1711886778u, 286155052u, 2282172566u, 4278190080u
		};

		private const uint vHwHJilSzpRcGcMHINkIEjBqHKxr = 3940166985u;

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

		public bool BatteryCharging => SjxtcqPdmhjLyDbGJOzLGCnAThMCB;

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

		public DualSenseMicrophoneLightMode microphoneLightMode
		{
			get
			{
				return SZuYIRtPktAdSPkPrPiJxBrsTyXV;
			}
			set
			{
				SZuYIRtPktAdSPkPrPiJxBrsTyXV = value;
				KwzpPuaDrCLkgOgTRCqggBvGhEBh = true;
			}
		}

		public DualSenseOtherLightBrightness otherLightBrightness
		{
			get
			{
				return kZYQpUjvSPcqgvKMUlOJQXTGbRUl(atNJRaLRKabUieTrDlvmqCbduEsFA);
			}
			set
			{
				atNJRaLRKabUieTrDlvmqCbduEsFA = bqWHaXcNPMgilkqEbcMMRdqDJmKV(value);
				KwzpPuaDrCLkgOgTRCqggBvGhEBh = true;
			}
		}

		public DualSensePlayerLightFlags playerLights
		{
			get
			{
				return zjOLnWOrhBGvnKUKtjLfCeKbBKXe;
			}
			set
			{
				zjOLnWOrhBGvnKUKtjLfCeKbBKXe = value;
				KwzpPuaDrCLkgOgTRCqggBvGhEBh = true;
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

		public DualSenseDriver(InitArgs P_0)
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
			fDAHDJbARjWStNNVeAQVzofIxULQ = P_0.connectionType == DeviceConnectionType.Bluetooth;
			if (fDAHDJbARjWStNNVeAQVzofIxULQ)
			{
				RUJKBthdvVQqUCHNDAtXmuJoumgV = 547;
			}
			else
			{
				RUJKBthdvVQqUCHNDAtXmuJoumgV = 48;
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
				byte[] array = P_0.getFeatureReportDelegate(5);
				mNDdAkFBUZZNqoZtuijPtQcAcaOk = array != null && array.Length != 0;
				if (mNDdAkFBUZZNqoZtuijPtQcAcaOk)
				{
					bROjQYFTsjmSyLGlwglxdUhGIYpT(patCHGFpzQFWiFajobvxWhTzPZfhA.Synchronous);
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
				vPZjGKKCZiTkmypxFRYxoULOigAv = 49;
				htyqSPpfIfxkTMvTRsUkdMOHtgNH = 1;
			}
			tFjliRmVNqRJBLgdGqbeGULvFMdU = 8 + htyqSPpfIfxkTMvTRsUkdMOHtgNH;
			uwDjQiabEjNvefpNjTtmGelTYOGR = 9 + htyqSPpfIfxkTMvTRsUkdMOHtgNH;
			UCQcafhwMxDbvyQzpJLDHdHejqygA = 10 + htyqSPpfIfxkTMvTRsUkdMOHtgNH;
			buttons = new HIDButton[15];
			for (int i = 0; i < 15; i++)
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
					dataIndex = 5 + htyqSPpfIfxkTMvTRsUkdMOHtgNH,
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
					dataIndex = 6 + htyqSPpfIfxkTMvTRsUkdMOHtgNH,
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
					dataIndex = 8 + htyqSPpfIfxkTMvTRsUkdMOHtgNH,
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
					dataIndex = 22 + htyqSPpfIfxkTMvTRsUkdMOHtgNH,
					bitSize = 48
				}, 3, fVJsvXRmLprokhErKbSaAJNmRHGE)
			};
			gyroscopes = new HIDGyroscope[1]
			{
				new HIDGyroscope(P_0.updateLoopSetting, vPZjGKKCZiTkmypxFRYxoULOigAv, new HIDControllerElement.HIDInfo
				{
					usagePage = 1,
					dataIndex = 16 + htyqSPpfIfxkTMvTRsUkdMOHtgNH,
					bitSize = 48
				}, 3, 25, uhYotKaDyBCRSdIqqcJnRRCGUgrF, zeLdUqyICWRtPyuZDrgACpSMiWgI)
			};
			touchpads = new HIDTouchpad[1]
			{
				new HIDTouchpad(vPZjGKKCZiTkmypxFRYxoULOigAv, new HIDTouchpad.TouchpadInfo(2, 0, 1912, 0, 941, false, true), new HIDControllerElement.HIDInfo
				{
					usagePage = 1,
					dataIndex = 33 + htyqSPpfIfxkTMvTRsUkdMOHtgNH,
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
			SjxtcqPdmhjLyDbGJOzLGCnAThMCB = (HLsWXNxbuFUDCLhxfUYoppWgHBGi[54 + htyqSPpfIfxkTMvTRsUkdMOHtgNH] & 8) != 0;
			DsGAqceMmOTkIADNeQypgRLAnzon = (HLsWXNxbuFUDCLhxfUYoppWgHBGi[55 + htyqSPpfIfxkTMvTRsUkdMOHtgNH] & 0x20) != 0;
			JrgUdyTuBsdMhTzLXfuRKnFdAHNz = (byte)(HLsWXNxbuFUDCLhxfUYoppWgHBGi[55 + htyqSPpfIfxkTMvTRsUkdMOHtgNH] & 0xF);
			MmgGhpCDtCBhYFPtcFhoPBAaIBmiB = (HLsWXNxbuFUDCLhxfUYoppWgHBGi[54 + htyqSPpfIfxkTMvTRsUkdMOHtgNH] & 1) != 0;
			HQnmtoMCtxgtOxBfNBvbJDyhwEno();
			return true;
		}

		public override Controller.Extension CreateControllerExtension()
		{
			return new DualSenseExtension(this);
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
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[0] = 49;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[1] = 2;
				WQGtVGagMjJCQPcACQAcaZKqfuhJ(CAPBXhcnJamPmJsEbuPqwfyjKPHdb, 2);
				uint num = QNCDBbBfyyTHOkMjLALUFgAgMHmFB(CAPBXhcnJamPmJsEbuPqwfyjKPHdb, 74);
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[74] = (byte)(num & 0xFF);
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[75] = (byte)((num & 0xFF00) >> 8);
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[76] = (byte)((num & 0xFF0000) >> 16);
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[77] = (byte)((num & 0xFF000000u) >> 24);
			}
			else
			{
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[0] = 2;
				WQGtVGagMjJCQPcACQAcaZKqfuhJ(CAPBXhcnJamPmJsEbuPqwfyjKPHdb, 1);
			}
		}

		private void WQGtVGagMjJCQPcACQAcaZKqfuhJ(NativeBuffer P_0, int P_1)
		{
			P_0[P_1] = byte.MaxValue;
			P_0[1 + P_1] = 247;
			P_0[2 + P_1] = (byte)vibrationMotors[1].SpeedRaw;
			P_0[3 + P_1] = (byte)vibrationMotors[0].SpeedRaw;
			P_0[8 + P_1] = (byte)SZuYIRtPktAdSPkPrPiJxBrsTyXV;
			P_0[43 + P_1] = (byte)zjOLnWOrhBGvnKUKtjLfCeKbBKXe;
			if (HLtUeXlgAUlbQvlddcCQGGeUHXmI)
			{
				P_0[43 + P_1] = (byte)(P_0[43 + P_1] & -33);
			}
			else
			{
				P_0[43 + P_1] |= 32;
			}
			P_0[38 + P_1] = 3;
			P_0[41 + P_1] = (byte)(GVzcvSBgBaoaWVuyUcVevLKKEFVHb ? 1 : 2);
			P_0[42 + P_1] = (byte)atNJRaLRKabUieTrDlvmqCbduEsFA;
			P_0[44 + P_1] = lights[0].ColorRRaw;
			P_0[45 + P_1] = lights[0].ColorGRaw;
			P_0[46 + P_1] = lights[0].ColorBRaw;
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
			if (mNDdAkFBUZZNqoZtuijPtQcAcaOk)
			{
				buttons[14].SetValue((b & 4) != 0, P_1);
			}
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
				uint num = HLsWXNxbuFUDCLhxfUYoppWgHBGi.ReadUInt(28 + htyqSPpfIfxkTMvTRsUkdMOHtgNH);
				float num3;
				if (num != JXRJRwsAtReVqNSrQgxDsJidJLwn)
				{
					uint num2 = (uint)((num >= JXRJRwsAtReVqNSrQgxDsJidJLwn) ? (num - JXRJRwsAtReVqNSrQgxDsJidJLwn) : ((long)num + 4294967295L - JXRJRwsAtReVqNSrQgxDsJidJLwn));
					num3 = (float)num2 / 3000000f;
				}
				else
				{
					uint num2 = 0u;
					num3 = 0f;
				}
				JXRJRwsAtReVqNSrQgxDsJidJLwn = num;
				wLqvEwIHNMZuhtrqhsAlwwIFQXrI = num3;
			}
		}

		private void HQnmtoMCtxgtOxBfNBvbJDyhwEno()
		{
			if (mNDdAkFBUZZNqoZtuijPtQcAcaOk && !(wLqvEwIHNMZuhtrqhsAlwwIFQXrI <= 0f))
			{
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
			if (sqrMagnitude > 16777216f && sqrMagnitude < 268435460f && jsgdUxVZaFxxIIRNWogijkrfDfPJA(P_0, out var tzEKFlRMaxNimwXFiEHhnbrVOTfe2))
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
				if ((tzEKFlRMaxNimwXFiEHhnbrVOTfe2 & tzEKFlRMaxNimwXFiEHhnbrVOTfe.XZ) != tzEKFlRMaxNimwXFiEHhnbrVOTfe.None)
				{
					b = UZYBKJeXZMJQnHhrvwMfWfkYtObOA(P_0, a.eulerAngles.y);
				}
				else if ((tzEKFlRMaxNimwXFiEHhnbrVOTfe2 & tzEKFlRMaxNimwXFiEHhnbrVOTfe.Y) != tzEKFlRMaxNimwXFiEHhnbrVOTfe.None)
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

		private Quaternion tdWybmfuyQIgdRqzbNuSROxQbyUAA(Quaternion P_0, rbpsDScoOmbWLehGUBTzhryusOEsA P_1)
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

		private bool jsgdUxVZaFxxIIRNWogijkrfDfPJA(Vector3 P_0, out tzEKFlRMaxNimwXFiEHhnbrVOTfe P_1)
		{
			P_0.Normalize();
			P_1 = tzEKFlRMaxNimwXFiEHhnbrVOTfe.None;
			bool result = false;
			if (NejbHGFhHdfCNjRCCpqPDhfawqZHc(P_0))
			{
				result = true;
				P_1 |= tzEKFlRMaxNimwXFiEHhnbrVOTfe.XZ;
			}
			if (uwHidFBblQHYYjLIuLbxsFLDWiBr(P_0))
			{
				result = true;
				P_1 |= tzEKFlRMaxNimwXFiEHhnbrVOTfe.Y;
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
			int num = 33 + htyqSPpfIfxkTMvTRsUkdMOHtgNH;
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

		~DualSenseDriver()
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
			if (vid == 1356 && pid == 3302)
			{
				return true;
			}
			return false;
		}

		private static uint QNCDBbBfyyTHOkMjLALUFgAgMHmFB(NativeBuffer P_0, int P_1)
		{
			uint num = 3940166985u;
			for (int i = 0; i < P_1; i++)
			{
				num = cBNiFZRLhxIRKBaIlkMzCnogdNgUb[(byte)num ^ P_0[i]] ^ (num >> 8);
			}
			return num;
		}

		private static gcBgeujMQKwjpvqLScilprYNLiTU bqWHaXcNPMgilkqEbcMMRdqDJmKV(DualSenseOtherLightBrightness P_0)
		{
			return P_0 switch
			{
				DualSenseOtherLightBrightness.High => gcBgeujMQKwjpvqLScilprYNLiTU.High, 
				DualSenseOtherLightBrightness.Medium => gcBgeujMQKwjpvqLScilprYNLiTU.Medium, 
				DualSenseOtherLightBrightness.Low => gcBgeujMQKwjpvqLScilprYNLiTU.Low, 
				_ => throw new NotImplementedException(), 
			};
		}

		private static DualSenseOtherLightBrightness kZYQpUjvSPcqgvKMUlOJQXTGbRUl(gcBgeujMQKwjpvqLScilprYNLiTU P_0)
		{
			return P_0 switch
			{
				gcBgeujMQKwjpvqLScilprYNLiTU.High => DualSenseOtherLightBrightness.High, 
				gcBgeujMQKwjpvqLScilprYNLiTU.Medium => DualSenseOtherLightBrightness.Medium, 
				gcBgeujMQKwjpvqLScilprYNLiTU.Low => DualSenseOtherLightBrightness.Low, 
				_ => throw new NotImplementedException(), 
			};
		}
	}
}
