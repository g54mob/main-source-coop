using System;
using System.Linq;
using Rewired.ControllerExtensions;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using UnityEngine;

namespace Rewired.HID.Drivers
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class DualSenseDriver : HIDDeviceDriver, IDriver_DualSense, IControllerDriver, IHIDControllerExtension, IDisposable
	{
		private enum xxfCKEQgVKfvKeqXIqrTJAAtaxhBA
		{
			None = 0,
			XZ = 1,
			Y = 2
		}

		private enum JDCDCcBEnhvdVErEgATkBpGJfPGfb : byte
		{
			Off = 0,
			Feedback = 1,
			Weapon = 2,
			Vibration = 3,
			SlopeFeedback = 4
		}

		private enum aIwgqjDyNnaPFxVBqcJTPSbxIDRdA : byte
		{
			High = 0,
			Medium = 1,
			Low = 2
		}

		private enum laHiMsXIheMUuubRXCQPdLaHfqVu : byte
		{
			Discharging = 0,
			Charging = 1,
			Full = 2,
			TemperatureOutOfRange = 10,
			TemperatureError = 11,
			ChargingError = 15
		}

		private enum DhGzVLCsLKQImEQBhLvwfmiKEEebA
		{
			NotCharging = 0,
			Discharging = 1,
			Charging = 2,
			Full = 3,
			Unknown = 4
		}

		private enum WCdzkDrrxMKpWaFgQbxIskTxYhFd : byte
		{
			None = 0,
			CompatibleVibrationMode1 = 1,
			HapticsSelect = 2,
			RightTriggerEffect = 4,
			LeftTriggerEffect = 8,
			AudioVolume = 0x10,
			ToggleInternalSpeaker = 0x20,
			MicrophoneVolume = 0x40,
			ToggleInternalMicOrExternalSpeaker = 0x80
		}

		private enum xTFKlrBsWaerOMPRIJPSjYDTlCMq : byte
		{
			None = 0,
			MicrophoneLEDControl = 1,
			PowerSaveControl = 2,
			LightbarControl = 4,
			TurnOffLEDs = 8,
			PlayerIndicatorLEDControl = 0x10,
			Unknown1 = 0x20,
			ChangeOverallMotorEffectPower = 0x40,
			Unknown2 = 0x80
		}

		private enum UBBwVUGzhwMRjjVzKisTLefcYmFc : byte
		{
			None = 0,
			OtherLightBrightnessControl = 1,
			LightbarSetupControl = 2,
			CompatibleVibrationMode2 = 4
		}

		private struct hjTCZuEfWUjEIEmfyaDgGaZDTELnB
		{
			public byte fckTkvgwDMeEmGzBJnFrvSVpjXGP;

			public byte jAVnrampYCrFfuNCUCBvbOCSRLEo => (byte)(fckTkvgwDMeEmGzBJnFrvSVpjXGP & 0xF);

			public byte TBsoBwBGoOUixuWdKQsSSRtqrzoL => (byte)(fckTkvgwDMeEmGzBJnFrvSVpjXGP >> 4);

			public hjTCZuEfWUjEIEmfyaDgGaZDTELnB(byte P_0)
			{
				fckTkvgwDMeEmGzBJnFrvSVpjXGP = P_0;
			}
		}

		private static class EJzFSrsMNbGNzubpPeFdJsAJcOLRA
		{
			public static class UIRTnphNpaFOERJnDwUKhkRiacZi
			{
				[Serializable]
				private sealed class WXNaRXxcICzFKGjGEGrLBqbHMSgzA
				{
					public static readonly WXNaRXxcICzFKGjGEGrLBqbHMSgzA _003C_003E9 = new WXNaRXxcICzFKGjGEGrLBqbHMSgzA();

					public static Func<byte, bool> _003C_003E9__4_0;

					public static Func<byte, bool> _003C_003E9__6_0;

					internal bool XCRjnKFWLPtHCTLYuOZkxhXbBElHb(byte P_0)
					{
						return P_0 > 0;
					}

					internal bool IJpcdBOrWgkysLhlzRzYYkydArrl(byte P_0)
					{
						return P_0 > 0;
					}
				}

				public static bool GReSEOpmUWZKBSgBujFYCxgLzrdR(byte[] P_0, int P_1)
				{
					P_0[P_1] = 5;
					P_0[P_1 + 1] = 0;
					P_0[P_1 + 2] = 0;
					P_0[P_1 + 3] = 0;
					P_0[P_1 + 4] = 0;
					P_0[P_1 + 5] = 0;
					P_0[P_1 + 6] = 0;
					P_0[P_1 + 7] = 0;
					P_0[P_1 + 8] = 0;
					P_0[P_1 + 9] = 0;
					P_0[P_1 + 10] = 0;
					return true;
				}

				public static bool ktyBdcvadDVJGMlHsmWffgfuPlQh(byte[] P_0, int P_1, byte P_2, byte P_3)
				{
					if (P_2 > 9)
					{
						return false;
					}
					if (P_3 > 8)
					{
						return false;
					}
					if (P_3 > 0)
					{
						byte b = (byte)((P_3 - 1) & 7);
						uint num = 0u;
						ushort num2 = 0;
						for (int i = P_2; i < 10; i++)
						{
							num |= (uint)(b << 3 * i);
							num2 |= (ushort)(1 << i);
						}
						P_0[P_1] = 33;
						P_0[P_1 + 1] = (byte)(num2 & 0xFF);
						P_0[P_1 + 2] = (byte)((num2 >> 8) & 0xFF);
						P_0[P_1 + 3] = (byte)(num & 0xFF);
						P_0[P_1 + 4] = (byte)((num >> 8) & 0xFF);
						P_0[P_1 + 5] = (byte)((num >> 16) & 0xFF);
						P_0[P_1 + 6] = (byte)((num >> 24) & 0xFF);
						P_0[P_1 + 7] = 0;
						P_0[P_1 + 8] = 0;
						P_0[P_1 + 9] = 0;
						P_0[P_1 + 10] = 0;
						return true;
					}
					return GReSEOpmUWZKBSgBujFYCxgLzrdR(P_0, P_1);
				}

				public static bool kRLCTTPotfKidDADCGtRHxoKRTwF(byte[] P_0, int P_1, byte P_2, byte P_3, byte P_4)
				{
					if (P_2 > 7 || P_2 < 2)
					{
						return false;
					}
					if (P_3 > 8)
					{
						return false;
					}
					if (P_3 <= P_2)
					{
						return false;
					}
					if (P_4 > 8)
					{
						return false;
					}
					if (P_4 > 0)
					{
						ushort num = (ushort)((1 << (int)P_2) | (1 << (int)P_3));
						P_0[P_1] = 37;
						P_0[P_1 + 1] = (byte)(num & 0xFF);
						P_0[P_1 + 2] = (byte)((num >> 8) & 0xFF);
						P_0[P_1 + 3] = (byte)(P_4 - 1);
						P_0[P_1 + 4] = 0;
						P_0[P_1 + 5] = 0;
						P_0[P_1 + 6] = 0;
						P_0[P_1 + 7] = 0;
						P_0[P_1 + 8] = 0;
						P_0[P_1 + 9] = 0;
						P_0[P_1 + 10] = 0;
						return true;
					}
					return GReSEOpmUWZKBSgBujFYCxgLzrdR(P_0, P_1);
				}

				public static bool QzylqyVnMwflmifRYKBPLNcngKQL(byte[] P_0, int P_1, byte P_2, byte P_3, byte P_4)
				{
					if (P_2 > 9)
					{
						return false;
					}
					if (P_3 > 8)
					{
						return false;
					}
					if (P_3 > 0 && P_4 > 0)
					{
						byte b = (byte)((P_3 - 1) & 7);
						uint num = 0u;
						ushort num2 = 0;
						for (int i = P_2; i < 10; i++)
						{
							num |= (uint)(b << 3 * i);
							num2 |= (ushort)(1 << i);
						}
						P_0[P_1] = 38;
						P_0[P_1 + 1] = (byte)(num2 & 0xFF);
						P_0[P_1 + 2] = (byte)((num2 >> 8) & 0xFF);
						P_0[P_1 + 3] = (byte)(num & 0xFF);
						P_0[P_1 + 4] = (byte)((num >> 8) & 0xFF);
						P_0[P_1 + 5] = (byte)((num >> 16) & 0xFF);
						P_0[P_1 + 6] = (byte)((num >> 24) & 0xFF);
						P_0[P_1 + 7] = 0;
						P_0[P_1 + 8] = 0;
						P_0[P_1 + 9] = P_4;
						P_0[P_1 + 10] = 0;
						return true;
					}
					return GReSEOpmUWZKBSgBujFYCxgLzrdR(P_0, P_1);
				}

				public static bool aJXiAbSRLLezAetUPJIZGDOccujlc(byte[] P_0, int P_1, byte[] P_2)
				{
					if (P_2.Length != 10)
					{
						return false;
					}
					if (P_2.Any(WXNaRXxcICzFKGjGEGrLBqbHMSgzA._003C_003E9.XCRjnKFWLPtHCTLYuOZkxhXbBElHb))
					{
						uint num = 0u;
						ushort num2 = 0;
						for (int i = 0; i < 10; i++)
						{
							if (P_2[i] > 0)
							{
								byte b = (byte)((P_2[i] - 1) & 7);
								num |= (uint)(b << 3 * i);
								num2 |= (ushort)(1 << i);
							}
						}
						P_0[P_1] = 33;
						P_0[P_1 + 1] = (byte)(num2 & 0xFF);
						P_0[P_1 + 2] = (byte)((num2 >> 8) & 0xFF);
						P_0[P_1 + 3] = (byte)(num & 0xFF);
						P_0[P_1 + 4] = (byte)((num >> 8) & 0xFF);
						P_0[P_1 + 5] = (byte)((num >> 16) & 0xFF);
						P_0[P_1 + 6] = (byte)((num >> 24) & 0xFF);
						P_0[P_1 + 7] = 0;
						P_0[P_1 + 8] = 0;
						P_0[P_1 + 9] = 0;
						P_0[P_1 + 10] = 0;
						return true;
					}
					return GReSEOpmUWZKBSgBujFYCxgLzrdR(P_0, P_1);
				}

				public static bool ivlaolcKrkGwxtlErxaDQIUfMjAoA(byte[] P_0, int P_1, byte P_2, byte P_3, byte P_4, byte P_5)
				{
					if (P_2 > 8 || P_2 < 0)
					{
						return false;
					}
					if (P_3 > 9)
					{
						return false;
					}
					if (P_3 <= P_2)
					{
						return false;
					}
					if (P_4 > 8)
					{
						return false;
					}
					if (P_4 < 1)
					{
						return false;
					}
					if (P_5 > 8)
					{
						return false;
					}
					if (P_5 < 1)
					{
						return false;
					}
					byte[] array = new byte[10];
					float num = 1f * (float)(P_5 - P_4) / (float)(P_3 - P_2);
					for (int i = P_2; i < 10; i++)
					{
						if (i <= P_3)
						{
							array[i] = (byte)Math.Round((float)(int)P_4 + num * (float)(i - P_2));
						}
						else
						{
							array[i] = P_5;
						}
					}
					return aJXiAbSRLLezAetUPJIZGDOccujlc(P_0, P_1, array);
				}

				public static bool AcaUVfcbLabzaUOqJwGbbGADRJyG(byte[] P_0, int P_1, byte P_2, byte[] P_3)
				{
					if (P_3.Length != 10)
					{
						return false;
					}
					if (P_2 > 0 && P_3.Any(WXNaRXxcICzFKGjGEGrLBqbHMSgzA._003C_003E9.IJpcdBOrWgkysLhlzRzYYkydArrl))
					{
						uint num = 0u;
						ushort num2 = 0;
						for (int i = 0; i < 10; i++)
						{
							if (P_3[i] > 0)
							{
								byte b = (byte)((P_3[i] - 1) & 7);
								num |= (uint)(b << 3 * i);
								num2 |= (ushort)(1 << i);
							}
						}
						P_0[P_1] = 38;
						P_0[P_1 + 1] = (byte)(num2 & 0xFF);
						P_0[P_1 + 2] = (byte)((num2 >> 8) & 0xFF);
						P_0[P_1 + 3] = (byte)(num & 0xFF);
						P_0[P_1 + 4] = (byte)((num >> 8) & 0xFF);
						P_0[P_1 + 5] = (byte)((num >> 16) & 0xFF);
						P_0[P_1 + 6] = (byte)((num >> 24) & 0xFF);
						P_0[P_1 + 7] = 0;
						P_0[P_1 + 8] = 0;
						P_0[P_1 + 9] = P_2;
						P_0[P_1 + 10] = 0;
						return true;
					}
					return GReSEOpmUWZKBSgBujFYCxgLzrdR(P_0, P_1);
				}
			}
		}

		private readonly IHIDDevice FingbDpFDpIsWbuwXtHuWqWRuqHc;

		private readonly HIDProperties lJGgSuKIrRxvpMXUnmuVCeVIHKJVA;

		private readonly bool BsXbtMXxpzmhGuljuFLrJzEFSAOb;

		private readonly int qEgjjGZKJzzllhpQBcwGqGBWCLqL;

		private readonly int ywSehgkiEnmZuEYBLOyvelEaYIcK;

		private readonly bool zLSJTubeqKfYYddzoxwkzNoQUBFs;

		private readonly byte miZOKXMToTJtVGlqgdlrpnmpUoEe;

		private readonly int FbKEhhOeQqEzFzmexhJOIPLVpYhdb;

		private readonly int UWmFaEVMSAOGZzAVltYRLSZYcELX;

		private readonly int xYUlpMqLMPjCfGRSlIKTKpJYFGEfA;

		private readonly int CBBRBvZHmxcsoYpRrzStbnjczKad;

		private readonly NativeBuffer kSDlSTSxKEWsjBuQXBuYndWzNbCC;

		private readonly NativeBuffer EdXCodfulaYKaAkNDfgESYNmUdQQ;

		private aMZqdyjJERTAUbjSZWzzHWVxTEnF jVjfvBdAVQAbCbGwqzkDczYoMSwE;

		private int gBvJZYxygBRKEIwqTvuVbNgjaTaaA;

		private bool oPkkGDvhUNGYAfZtPmhFrGGNYMRDA;

		private bool aUOCwOCAqBnFTDPgaCPnBCTqtnAfA;

		private double firSKKwlMnvjOjJTLaRbCfqnLwKJ;

		private int ldrELImcxfRgkeJEmTxXDlHZTjSL;

		private DhGzVLCsLKQImEQBhLvwfmiKEEebA tDpqyWvRNikOizhKqbcrJJDTCtmR;

		private bool EHXKHfgOHXIHufnOtultDSLVxlST;

		private Quaternion jzwEYubZtNyIWPqMlJovBwHpcZIg = Quaternion.identity;

		private DualSenseMicrophoneLightMode zmHCrfeLSmvptoolIlJLWVETjqbhb;

		private aIwgqjDyNnaPFxVBqcJTPSbxIDRdA ZAJtaEzeCwHNmwkvIyHqKnGgWdwX;

		private DualSensePlayerLightFlags EPgmhWLeGMzSwpXjouvJpTNpMGLE;

		private bool KydkujeNcQvxtoqlXFDtAZOsHZmK;

		private uint OWgzBoMVcBvrtLAQDwNjRJLaXduW;

		private float cjmVAzYHgkXMZAbeetYBVXkAGEOd;

		private double kLMfGQMZOVJyjTdXGAOnTLcAFHPo;

		private float BSWyYMlrppASMBiYfhdZeSvZiAad;

		private readonly IDualSenseTriggerEffect[] AfycKPjGSkxgfXjzGrxBOuWEmgje = new IDualSenseTriggerEffect[2];

		private readonly byte[] vtYWhyGuuPAxkKjMdqbSZTLlhdc = new byte[10];

		private readonly byte[] xgoUmwxPcdiTTihziXEllQjoUghAA = new byte[11];

		private DualSenseTriggerEffectState[] JrhNmYokfViKaNHIPZkevqxeRFLh = new DualSenseTriggerEffectState[2];

		private DualSenseVibrationMode XgWbvciWbhfcgueTgSHMpnZUquDYA;

		private byte gvuWfTKuZlsEcwFppAgAAZsOYzWc;

		private bool KYVrNCSXAQNwxwqsADVWNIAkBIvi;

		private bool JSDdpkdIhZgTBevUzPZreWabKaTcc;

		private bool ZvlvNaCYQSIVFPMSJCScbzutFGlOA;

		private bool QdjYRnQWXJTqSsfxhokyjNhFqhmy;

		private bool MzMqDgvuxyowjAHRvCWttelqDjOn;

		private bool vZqGJejnRMcOQibDhFaPsvtfZlso;

		private bool xSKOKiANxDIDxoFwVryhTjtAUuYL;

		private bool XQBvpupfKfYkJCkcPQBMebqSmBQw;

		private bool CIkRAvVlxNwxnaopYhITcsWChkwg;

		private byte ZjmugIayNfrewEXwPPeUZqukPetn;

		private byte fWjSfPZlfHKeZGiBkcLLYVekhBHj;

		private Quaternion ZmVYfzOldHzoPHzUkkZsHlKfyzHG = Quaternion.identity;

		private Quaternion rsmgSbCEBKlsmjPOWdHIwfayGpNw = Quaternion.identity;

		private bool rLWRwJuRJtzTiLIeeRCOmLXoxwJ;

		private int ecVBbklpuikHRewxBeiTqFHZkjfM;

		private int[] IQECmmkTEpXHUQjyvOKcuweGcnSv = new int[2];

		private int[] tYzcliEPOJbUHwUGVRAFCDixheImA = new int[2];

		private static uint[] lUXGBMRBHrhLHVodUElIdDXSAmBeb = new uint[256]
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

		private bool isVibrating
		{
			get
			{
				for (int i = 0; i < base.Rewired_002EHID_002EDrivers_002EIControllerDriver_002EVibrationMotorCount; i++)
				{
					if (vibrationMotors[i].ZcjoZwbIDbbFlaWQFjFKWrESBVuu > 0)
					{
						return true;
					}
				}
				return false;
			}
		}

		float IDriver_DualSense.BatteryLevel => ldrELImcxfRgkeJEmTxXDlHZTjSL;

		bool IDriver_DualSense.BatteryCharging => tDpqyWvRNikOizhKqbcrJJDTCtmR == DhGzVLCsLKQImEQBhLvwfmiKEEebA.Charging;

		DualSenseVibrationMode IDriver_DualSense.vibrationMode
		{
			get
			{
				return XgWbvciWbhfcgueTgSHMpnZUquDYA;
			}
			set
			{
				XgWbvciWbhfcgueTgSHMpnZUquDYA = value;
				UDIiOMasWGczisiwnUFhftrBRTAkA();
			}
		}

		float IDriver_DualSense.LeftMotor
		{
			get
			{
				return vibrationMotors[0].IzilEZFnKKPoEpcKyoPmGolsUlOt;
			}
			set
			{
				vibrationMotors[0].IzilEZFnKKPoEpcKyoPmGolsUlOt = value;
			}
		}

		float IDriver_DualSense.RightMotor
		{
			get
			{
				return vibrationMotors[1].IzilEZFnKKPoEpcKyoPmGolsUlOt;
			}
			set
			{
				vibrationMotors[1].IzilEZFnKKPoEpcKyoPmGolsUlOt = value;
			}
		}

		float IDriver_DualSense.LightColorR
		{
			get
			{
				return lights[0].mQPaVmdRqYozYExrtihYfahdYhPF;
			}
			set
			{
				lights[0].mQPaVmdRqYozYExrtihYfahdYhPF = value;
			}
		}

		float IDriver_DualSense.LightColorG
		{
			get
			{
				return lights[0].ffwrCveDusqznMzxjnoAkStTGZyeA;
			}
			set
			{
				lights[0].ffwrCveDusqznMzxjnoAkStTGZyeA = value;
			}
		}

		float IDriver_DualSense.LightColorB
		{
			get
			{
				return lights[0].DnwgdpganiBQCDljGtGXWAzaoHBmB;
			}
			set
			{
				lights[0].DnwgdpganiBQCDljGtGXWAzaoHBmB = value;
			}
		}

		float IDriver_DualSense.LightFlashOnDuration
		{
			get
			{
				return (int)ZjmugIayNfrewEXwPPeUZqukPetn;
			}
			set
			{
				ZjmugIayNfrewEXwPPeUZqukPetn = (byte)MathTools.Clamp(MathTools.Clamp(value, 0f, 2.5f) * 100f, 0f, 255f);
				PSxDGSEpWckTXsvxntiolsbkGGeR();
				if (ZjmugIayNfrewEXwPPeUZqukPetn == 0 && fWjSfPZlfHKeZGiBkcLLYVekhBHj == 0)
				{
					aUOCwOCAqBnFTDPgaCPnBCTqtnAfA = true;
				}
			}
		}

		float IDriver_DualSense.LightFlashOffDuration
		{
			get
			{
				return (int)fWjSfPZlfHKeZGiBkcLLYVekhBHj;
			}
			set
			{
				fWjSfPZlfHKeZGiBkcLLYVekhBHj = (byte)MathTools.Clamp(MathTools.Clamp(value, 0f, 2.5f) * 100f, 0f, 255f);
				PSxDGSEpWckTXsvxntiolsbkGGeR();
				if (ZjmugIayNfrewEXwPPeUZqukPetn == 0 && fWjSfPZlfHKeZGiBkcLLYVekhBHj == 0)
				{
					aUOCwOCAqBnFTDPgaCPnBCTqtnAfA = true;
				}
			}
		}

		DualSenseMicrophoneLightMode IDriver_DualSense.microphoneLightMode
		{
			get
			{
				return zmHCrfeLSmvptoolIlJLWVETjqbhb;
			}
			set
			{
				zmHCrfeLSmvptoolIlJLWVETjqbhb = value;
				UDIiOMasWGczisiwnUFhftrBRTAkA();
				QdjYRnQWXJTqSsfxhokyjNhFqhmy = true;
			}
		}

		DualSenseOtherLightBrightness IDriver_DualSense.otherLightBrightness
		{
			get
			{
				return RRmmbySSUVcrpNJooNwhEIHEUdAE(ZAJtaEzeCwHNmwkvIyHqKnGgWdwX);
			}
			set
			{
				ZAJtaEzeCwHNmwkvIyHqKnGgWdwX = aXQscBWSmdiSSyLcSsHGCDNGjynL(value);
				UDIiOMasWGczisiwnUFhftrBRTAkA();
				vZqGJejnRMcOQibDhFaPsvtfZlso = true;
			}
		}

		DualSensePlayerLightFlags IDriver_DualSense.playerLights
		{
			get
			{
				return EPgmhWLeGMzSwpXjouvJpTNpMGLE;
			}
			set
			{
				EPgmhWLeGMzSwpXjouvJpTNpMGLE = value;
				UDIiOMasWGczisiwnUFhftrBRTAkA();
				MzMqDgvuxyowjAHRvCWttelqDjOn = true;
			}
		}

		Vector3 IDriver_DualSense.AccelerometerValue => mPVqelTISgJkSgPsxBZdPcVEdbBm(accelerometers[0].SytbxvDfrRdLDWckugtMRDSBscWP);

		Vector3 IDriver_DualSense.AccelerometerValueRaw => new Vector3(accelerometers[0].SytbxvDfrRdLDWckugtMRDSBscWP[0], accelerometers[0].SytbxvDfrRdLDWckugtMRDSBscWP[1], accelerometers[0].SytbxvDfrRdLDWckugtMRDSBscWP[2]);

		Vector3 IDriver_DualSense.GyroscopeValue => BToLyliABckLcpXhpWbglObHGmSQ(gyroscopes[0].garhibHNwyDACbuxuiOfayIWtbZD);

		Vector3 IDriver_DualSense.GyroscopeValueRaw => new Vector3(gyroscopes[0].OrthfcEpPRtmJfLlFdtCctIoezeQ[0], gyroscopes[0].OrthfcEpPRtmJfLlFdtCctIoezeQ[1], gyroscopes[0].OrthfcEpPRtmJfLlFdtCctIoezeQ[2]);

		Vector3 IDriver_DualSense.LastGyroscopeValue
		{
			get
			{
				Vector3 vector = new Vector3(gyroscopes[0].HViVbfIrDiBECuxWlqkhZscDKxn[0], gyroscopes[0].HViVbfIrDiBECuxWlqkhZscDKxn[1], gyroscopes[0].HViVbfIrDiBECuxWlqkhZscDKxn[2]);
				return iidbXhYuwXwMuryOnvOwHyhZCZge(vector, cjmVAzYHgkXMZAbeetYBVXkAGEOd);
			}
		}

		Vector3 IDriver_DualSense.LastGyroscopeValueRaw => new Vector3(gyroscopes[0].HViVbfIrDiBECuxWlqkhZscDKxn[0], gyroscopes[0].HViVbfIrDiBECuxWlqkhZscDKxn[1], gyroscopes[0].HViVbfIrDiBECuxWlqkhZscDKxn[2]);

		Quaternion IDriver_DualSense.Orientation => jzwEYubZtNyIWPqMlJovBwHpcZIg;

		int IDriver_DualSense.MaxTouches => 2;

		ushort IHIDControllerExtension.vendorId => lJGgSuKIrRxvpMXUnmuVCeVIHKJVA.vendorId;

		ushort IHIDControllerExtension.productId => lJGgSuKIrRxvpMXUnmuVCeVIHKJVA.productId;

		string IHIDControllerExtension.productName => lJGgSuKIrRxvpMXUnmuVCeVIHKJVA.productName;

		string IHIDControllerExtension.manufacturer => lJGgSuKIrRxvpMXUnmuVCeVIHKJVA.manufacturer;

		ushort IHIDControllerExtension.usagePage => lJGgSuKIrRxvpMXUnmuVCeVIHKJVA.usagePage;

		ushort IHIDControllerExtension.usage => lJGgSuKIrRxvpMXUnmuVCeVIHKJVA.usage;

		public void ResetOrientation()
		{
			jzwEYubZtNyIWPqMlJovBwHpcZIg = Quaternion.identity;
			rLWRwJuRJtzTiLIeeRCOmLXoxwJ = false;
		}

		void IDriver_DualSense.ResetOrientation()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ResetOrientation
			this.ResetOrientation();
		}

		public int GetTouchCount()
		{
			int num = 0;
			for (int i = 0; i < 2; i++)
			{
				if (touchpads[0].SBWbRIEBtbRxLkclWCpSvIwxSXTqA[i].isTouching)
				{
					num++;
				}
			}
			return num;
		}

		int IDriver_DualSense.GetTouchCount()
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetTouchCount
			return this.GetTouchCount();
		}

		public bool IsTouchingAtIndex(int index)
		{
			if (index < 0 || index >= 2)
			{
				return false;
			}
			return touchpads[0].SBWbRIEBtbRxLkclWCpSvIwxSXTqA[index].isTouching;
		}

		bool IDriver_DualSense.IsTouchingAtIndex(int index)
		{
			//ILSpy generated this explicit interface implementation from .override directive in IsTouchingAtIndex
			return this.IsTouchingAtIndex(index);
		}

		public bool IsTouchingAtTouchId(int touchId)
		{
			return touchpads[0].VRQDYrjowDqtUNGMQEXSGOOHLRDj(touchId);
		}

		bool IDriver_DualSense.IsTouchingAtTouchId(int touchId)
		{
			//ILSpy generated this explicit interface implementation from .override directive in IsTouchingAtTouchId
			return this.IsTouchingAtTouchId(touchId);
		}

		public int GetTouchIdAtIndex(int index)
		{
			if (index < 0 || index >= 2)
			{
				return -1;
			}
			return touchpads[0].SBWbRIEBtbRxLkclWCpSvIwxSXTqA[index].touchId;
		}

		int IDriver_DualSense.GetTouchIdAtIndex(int index)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetTouchIdAtIndex
			return this.GetTouchIdAtIndex(index);
		}

		public bool GetTouchPositionByIndex(int index, out Vector2 position)
		{
			position = default(Vector2);
			if (index < 0 || index >= 2)
			{
				return false;
			}
			JeEihaxNGDZUEopEZTyRorKoTSAm.TouchData[] sBWbRIEBtbRxLkclWCpSvIwxSXTqA = touchpads[0].SBWbRIEBtbRxLkclWCpSvIwxSXTqA;
			if (!sBWbRIEBtbRxLkclWCpSvIwxSXTqA[index].isTouching)
			{
				return false;
			}
			position.x = sBWbRIEBtbRxLkclWCpSvIwxSXTqA[index].positionX;
			position.y = sBWbRIEBtbRxLkclWCpSvIwxSXTqA[index].positionY;
			return true;
		}

		bool IDriver_DualSense.GetTouchPositionByIndex(int index, out Vector2 position)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetTouchPositionByIndex
			return this.GetTouchPositionByIndex(index, out position);
		}

		public bool GetTouchPositionByTouchId(int touchId, out Vector2 position)
		{
			position = default(Vector2);
			if (!touchpads[0].VRQDYrjowDqtUNGMQEXSGOOHLRDj(touchId))
			{
				return false;
			}
			JeEihaxNGDZUEopEZTyRorKoTSAm.TouchData[] sBWbRIEBtbRxLkclWCpSvIwxSXTqA = touchpads[0].SBWbRIEBtbRxLkclWCpSvIwxSXTqA;
			for (int i = 0; i < sBWbRIEBtbRxLkclWCpSvIwxSXTqA.Length; i++)
			{
				if (sBWbRIEBtbRxLkclWCpSvIwxSXTqA[i].isTouching)
				{
					position.x = sBWbRIEBtbRxLkclWCpSvIwxSXTqA[i].positionX;
					position.y = sBWbRIEBtbRxLkclWCpSvIwxSXTqA[i].positionY;
				}
			}
			return true;
		}

		bool IDriver_DualSense.GetTouchPositionByTouchId(int touchId, out Vector2 position)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetTouchPositionByTouchId
			return this.GetTouchPositionByTouchId(touchId, out position);
		}

		public bool GetTouchPositionAbsoluteByIndex(int index, out int positionX, out int positionY)
		{
			positionX = 0;
			positionY = 0;
			if (index < 0 || index >= 2)
			{
				return false;
			}
			JeEihaxNGDZUEopEZTyRorKoTSAm.TouchData[] sBWbRIEBtbRxLkclWCpSvIwxSXTqA = touchpads[0].SBWbRIEBtbRxLkclWCpSvIwxSXTqA;
			if (!sBWbRIEBtbRxLkclWCpSvIwxSXTqA[index].isTouching)
			{
				return false;
			}
			positionX = sBWbRIEBtbRxLkclWCpSvIwxSXTqA[index].positionAbsX;
			positionY = sBWbRIEBtbRxLkclWCpSvIwxSXTqA[index].positionAbsY;
			return true;
		}

		bool IDriver_DualSense.GetTouchPositionAbsoluteByIndex(int index, out int positionX, out int positionY)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetTouchPositionAbsoluteByIndex
			return this.GetTouchPositionAbsoluteByIndex(index, out positionX, out positionY);
		}

		public bool GetTouchPositionAbsoluteByTouchId(int touchId, out int positionX, out int positionY)
		{
			positionX = 0;
			positionY = 0;
			if (!touchpads[0].VRQDYrjowDqtUNGMQEXSGOOHLRDj(touchId))
			{
				return false;
			}
			JeEihaxNGDZUEopEZTyRorKoTSAm.TouchData[] sBWbRIEBtbRxLkclWCpSvIwxSXTqA = touchpads[0].SBWbRIEBtbRxLkclWCpSvIwxSXTqA;
			for (int i = 0; i < sBWbRIEBtbRxLkclWCpSvIwxSXTqA.Length; i++)
			{
				if (sBWbRIEBtbRxLkclWCpSvIwxSXTqA[i].isTouching)
				{
					positionX = sBWbRIEBtbRxLkclWCpSvIwxSXTqA[i].positionAbsX;
					positionY = sBWbRIEBtbRxLkclWCpSvIwxSXTqA[i].positionAbsY;
				}
			}
			return true;
		}

		bool IDriver_DualSense.GetTouchPositionAbsoluteByTouchId(int touchId, out int positionX, out int positionY)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetTouchPositionAbsoluteByTouchId
			return this.GetTouchPositionAbsoluteByTouchId(touchId, out positionX, out positionY);
		}

		public void StopLightFlash()
		{
			ZjmugIayNfrewEXwPPeUZqukPetn = 0;
			fWjSfPZlfHKeZGiBkcLLYVekhBHj = 0;
			UDIiOMasWGczisiwnUFhftrBRTAkA();
			aUOCwOCAqBnFTDPgaCPnBCTqtnAfA = true;
			xSKOKiANxDIDxoFwVryhTjtAUuYL = true;
		}

		void IDriver_DualSense.StopLightFlash()
		{
			//ILSpy generated this explicit interface implementation from .override directive in StopLightFlash
			this.StopLightFlash();
		}

		public void StopVibration()
		{
			int num = base.Rewired_002EHID_002EDrivers_002EIControllerDriver_002EVibrationMotorCount;
			for (int i = 0; i < num; i++)
			{
				vibrationMotors[i].ZcjoZwbIDbbFlaWQFjFKWrESBVuu = 0;
			}
		}

		void IDriver_DualSense.StopVibration()
		{
			//ILSpy generated this explicit interface implementation from .override directive in StopVibration
			this.StopVibration();
		}

		public bool SetTriggerEffect(DualSenseTriggerType trigger, IDualSenseTriggerEffect effect)
		{
			switch (trigger)
			{
			case DualSenseTriggerType.Left:
				AfycKPjGSkxgfXjzGrxBOuWEmgje[0] = effect;
				UDIiOMasWGczisiwnUFhftrBRTAkA();
				JSDdpkdIhZgTBevUzPZreWabKaTcc = true;
				return true;
			case DualSenseTriggerType.Right:
				AfycKPjGSkxgfXjzGrxBOuWEmgje[1] = effect;
				UDIiOMasWGczisiwnUFhftrBRTAkA();
				ZvlvNaCYQSIVFPMSJCScbzutFGlOA = true;
				return true;
			default:
				return false;
			}
		}

		bool IDriver_DualSense.SetTriggerEffect(DualSenseTriggerType trigger, IDualSenseTriggerEffect effect)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetTriggerEffect
			return this.SetTriggerEffect(trigger, effect);
		}

		public DualSenseTriggerEffectStates GetTriggerEffectStates()
		{
			return new DualSenseTriggerEffectStates
			{
				leftTrigger = JrhNmYokfViKaNHIPZkevqxeRFLh[0],
				rightTrigger = JrhNmYokfViKaNHIPZkevqxeRFLh[1]
			};
		}

		DualSenseTriggerEffectStates IDriver_DualSense.GetTriggerEffectStates()
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetTriggerEffectStates
			return this.GetTriggerEffectStates();
		}

		public DualSenseDriver(InitArgs P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("initArgs");
			}
			FingbDpFDpIsWbuwXtHuWqWRuqHc = P_0.hidDevice;
			lJGgSuKIrRxvpMXUnmuVCeVIHKJVA = FingbDpFDpIsWbuwXtHuWqWRuqHc.properties;
			qEgjjGZKJzzllhpQBcwGqGBWCLqL = P_0.hatZeroValue;
			ywSehgkiEnmZuEYBLOyvelEaYIcK = P_0.hatSpan;
			BsXbtMXxpzmhGuljuFLrJzEFSAOb = P_0.connectionType == YDvFqJokstcNyQQOYydcruGncmeb.Bluetooth;
			if (BsXbtMXxpzmhGuljuFLrJzEFSAOb)
			{
				gBvJZYxygBRKEIwqTvuVbNgjaTaaA = 78;
			}
			else
			{
				gBvJZYxygBRKEIwqTvuVbNgjaTaaA = 48;
			}
			kSDlSTSxKEWsjBuQXBuYndWzNbCC = new NativeBuffer(64);
			EdXCodfulaYKaAkNDfgESYNmUdQQ = new NativeBuffer(gBvJZYxygBRKEIwqTvuVbNgjaTaaA);
			jVjfvBdAVQAbCbGwqzkDczYoMSwE = new aMZqdyjJERTAUbjSZWzzHWVxTEnF(EdXCodfulaYKaAkNDfgESYNmUdQQ.Pointer, EdXCodfulaYKaAkNDfgESYNmUdQQ.Length, gBvJZYxygBRKEIwqTvuVbNgjaTaaA);
			lights = new dRxYZKovikdvFiOlZLmFiKpaWUdu[1]
			{
				new dRxYZKovikdvFiOlZLmFiKpaWUdu(11, 24, 28)
			};
			lights[0].KhdFqLHnkQpyjokVAndSadBMcFSRA += hWttkVDokzaZjWaKkqcWXIoXxWPH;
			vibrationMotors = new iwnZquMFWHwhZjzckYkHRPdcqkIc[2]
			{
				new iwnZquMFWHwhZjzckYkHRPdcqkIc(0, 255),
				new iwnZquMFWHwhZjzckYkHRPdcqkIc(0, 255)
			};
			vibrationMotors[0].JbLUwmUKfnDCvYnjJuByJLLCsxze += wrXPLXbIIeeWtwcUQUKqPzWVrVvR;
			vibrationMotors[1].JbLUwmUKfnDCvYnjJuByJLLCsxze += wrXPLXbIIeeWtwcUQUKqPzWVrVvR;
			XgWbvciWbhfcgueTgSHMpnZUquDYA = DualSenseVibrationMode.Compatible2;
			KYVrNCSXAQNwxwqsADVWNIAkBIvi = true;
			JSDdpkdIhZgTBevUzPZreWabKaTcc = true;
			ZvlvNaCYQSIVFPMSJCScbzutFGlOA = true;
			QdjYRnQWXJTqSsfxhokyjNhFqhmy = true;
			MzMqDgvuxyowjAHRvCWttelqDjOn = true;
			vZqGJejnRMcOQibDhFaPsvtfZlso = true;
			xSKOKiANxDIDxoFwVryhTjtAUuYL = true;
			XQBvpupfKfYkJCkcPQBMebqSmBQw = true;
			CIkRAvVlxNwxnaopYhITcsWChkwg = true;
			gvuWfTKuZlsEcwFppAgAAZsOYzWc = 2;
			if (BsXbtMXxpzmhGuljuFLrJzEFSAOb)
			{
				byte[] hidFeatureData = FingbDpFDpIsWbuwXtHuWqWRuqHc.GetHidFeatureData(5, 41, 1000, 3);
				zLSJTubeqKfYYddzoxwkzNoQUBFs = hidFeatureData != null && hidFeatureData.Length != 0;
				if (zLSJTubeqKfYYddzoxwkzNoQUBFs)
				{
					aDODkAiyXsJZWqnhdaeZosGszebVA(NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Synchronous);
				}
			}
			else
			{
				zLSJTubeqKfYYddzoxwkzNoQUBFs = true;
				zLSJTubeqKfYYddzoxwkzNoQUBFs = aDODkAiyXsJZWqnhdaeZosGszebVA(NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Synchronous);
			}
			if (!zLSJTubeqKfYYddzoxwkzNoQUBFs)
			{
				throw new Exception("Special features not supported so just treat this as a standard HID device.");
			}
			miZOKXMToTJtVGlqgdlrpnmpUoEe = 1;
			FbKEhhOeQqEzFzmexhJOIPLVpYhdb = 0;
			if (BsXbtMXxpzmhGuljuFLrJzEFSAOb && zLSJTubeqKfYYddzoxwkzNoQUBFs)
			{
				miZOKXMToTJtVGlqgdlrpnmpUoEe = 49;
				FbKEhhOeQqEzFzmexhJOIPLVpYhdb = 1;
			}
			UWmFaEVMSAOGZzAVltYRLSZYcELX = 8 + FbKEhhOeQqEzFzmexhJOIPLVpYhdb;
			xYUlpMqLMPjCfGRSlIKTKpJYFGEfA = 9 + FbKEhhOeQqEzFzmexhJOIPLVpYhdb;
			CBBRBvZHmxcsoYpRrzStbnjczKad = 10 + FbKEhhOeQqEzFzmexhJOIPLVpYhdb;
			buttons = new RyDagBEfRFfkQlRDvQAHmQXROhrtA[15];
			for (int i = 0; i < 15; i++)
			{
				buttons[i] = new RyDagBEfRFfkQlRDvQAHmQXROhrtA(miZOKXMToTJtVGlqgdlrpnmpUoEe, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 9,
					usage = (ushort)i
				});
			}
			axes = new eTBgDLAnVcEreaYiOpvDFMeVVuExA[6]
			{
				new eTBgDLAnVcEreaYiOpvDFMeVVuExA(miZOKXMToTJtVGlqgdlrpnmpUoEe, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					usage = 48,
					dataIndex = 1 + FbKEhhOeQqEzFzmexhJOIPLVpYhdb,
					bitSize = 8,
					logicalMin = 0,
					logicalMax = 255,
					physicalMin = 0,
					physicalMax = 0,
					units = 0u,
					unitsExp = 0u
				}, false, 127),
				new eTBgDLAnVcEreaYiOpvDFMeVVuExA(miZOKXMToTJtVGlqgdlrpnmpUoEe, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					usage = 49,
					dataIndex = 2 + FbKEhhOeQqEzFzmexhJOIPLVpYhdb,
					bitSize = 8,
					logicalMin = 0,
					logicalMax = 255,
					physicalMin = 0,
					physicalMax = 0,
					units = 0u,
					unitsExp = 0u
				}, false, 127),
				new eTBgDLAnVcEreaYiOpvDFMeVVuExA(miZOKXMToTJtVGlqgdlrpnmpUoEe, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					usage = 50,
					dataIndex = 3 + FbKEhhOeQqEzFzmexhJOIPLVpYhdb,
					bitSize = 8,
					logicalMin = 0,
					logicalMax = 255,
					physicalMin = 0,
					physicalMax = 0,
					units = 0u,
					unitsExp = 0u
				}, false, 127),
				new eTBgDLAnVcEreaYiOpvDFMeVVuExA(miZOKXMToTJtVGlqgdlrpnmpUoEe, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					usage = 53,
					dataIndex = 4 + FbKEhhOeQqEzFzmexhJOIPLVpYhdb,
					bitSize = 8,
					logicalMin = 0,
					logicalMax = 255,
					physicalMin = 0,
					physicalMax = 0,
					units = 0u,
					unitsExp = 0u
				}, false, 127),
				new eTBgDLAnVcEreaYiOpvDFMeVVuExA(miZOKXMToTJtVGlqgdlrpnmpUoEe, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					usage = 51,
					dataIndex = 5 + FbKEhhOeQqEzFzmexhJOIPLVpYhdb,
					bitSize = 8,
					logicalMin = 0,
					logicalMax = 255,
					physicalMin = 0,
					physicalMax = 315,
					units = 0u,
					unitsExp = 0u
				}, false, 0),
				new eTBgDLAnVcEreaYiOpvDFMeVVuExA(miZOKXMToTJtVGlqgdlrpnmpUoEe, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					usage = 52,
					dataIndex = 6 + FbKEhhOeQqEzFzmexhJOIPLVpYhdb,
					bitSize = 8,
					logicalMin = 0,
					logicalMax = 255,
					physicalMin = 0,
					physicalMax = 315,
					units = 0u,
					unitsExp = 0u
				}, false, 0)
			};
			hats = new AlQQSkDXAKgzPiahlYVsHmMBdhGkA[1]
			{
				new AlQQSkDXAKgzPiahlYVsHmMBdhGkA(miZOKXMToTJtVGlqgdlrpnmpUoEe, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					usage = 57,
					dataIndex = 8 + FbKEhhOeQqEzFzmexhJOIPLVpYhdb,
					bitSize = 4,
					logicalMin = 0,
					logicalMax = 7,
					physicalMin = 0,
					physicalMax = 315,
					units = 20u,
					unitsExp = 0u
				}, IAmPQlGDVLihHddhiePkkpdunlcIb)
			};
			accelerometers = new fcgInupHfYVLlnSfBDoHscyUgTsEA[1]
			{
				new fcgInupHfYVLlnSfBDoHscyUgTsEA(miZOKXMToTJtVGlqgdlrpnmpUoEe, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					dataIndex = 22 + FbKEhhOeQqEzFzmexhJOIPLVpYhdb,
					bitSize = 48
				}, 3, jzcdVtMgAeRFLAEDNsQXdUzNaPkg)
			};
			gyroscopes = new zeduVYzSnJpVQGxDoGRFMdphEaCi[1]
			{
				new zeduVYzSnJpVQGxDoGRFMdphEaCi(P_0.updateLoopSetting, miZOKXMToTJtVGlqgdlrpnmpUoEe, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					dataIndex = 16 + FbKEhhOeQqEzFzmexhJOIPLVpYhdb,
					bitSize = 48
				}, 3, 60, vkhmvkHwHqSArnlDQQpeumQBTJav, fLTurVzWIHTQilKqxQXXhYDlsxqq)
			};
			touchpads = new JeEihaxNGDZUEopEZTyRorKoTSAm[1]
			{
				new JeEihaxNGDZUEopEZTyRorKoTSAm(miZOKXMToTJtVGlqgdlrpnmpUoEe, new JeEihaxNGDZUEopEZTyRorKoTSAm.TouchpadInfo(2, 0, 1912, 0, 941, false, true), new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					dataIndex = 33 + FbKEhhOeQqEzFzmexhJOIPLVpYhdb,
					bitSize = 48
				}, 60, dGPhqgmFmYZDQZlJZeusGxpLfGyA)
			};
			kLMfGQMZOVJyjTdXGAOnTLcAFHPo = ReInput.realTime;
		}

		public override void Update(UpdateLoopType updateLoop)
		{
			mVnrAdpwkzHDGTEcbCCihPvfuMrb();
			GfUWgzXJEGxBewHsIWqANzUIZGCA(NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Asynchronous);
		}

		public unsafe override bool ParseInputReport(IntPtr inputReportPtr, int inputReportLength, double timestamp)
		{
			if (inputReportPtr == IntPtr.Zero)
			{
				return false;
			}
			if (inputReportLength < kSDlSTSxKEWsjBuQXBuYndWzNbCC.Length)
			{
				return false;
			}
			if (BsXbtMXxpzmhGuljuFLrJzEFSAOb && zLSJTubeqKfYYddzoxwkzNoQUBFs && *(byte*)(void*)inputReportPtr == 1)
			{
				return false;
			}
			BSWyYMlrppASMBiYfhdZeSvZiAad = (float)(timestamp - kLMfGQMZOVJyjTdXGAOnTLcAFHPo);
			kLMfGQMZOVJyjTdXGAOnTLcAFHPo = timestamp;
			kSDlSTSxKEWsjBuQXBuYndWzNbCC.Write(inputReportPtr, inputReportLength, kSDlSTSxKEWsjBuQXBuYndWzNbCC.Length);
			KycRkxUEkLJaeDQVDZiBNCdqFJaW(kSDlSTSxKEWsjBuQXBuYndWzNbCC);
			yEEymumRmZHgaVeDMRAgYJOxZmbU(kSDlSTSxKEWsjBuQXBuYndWzNbCC, timestamp);
			LDJGvqLnFydDhJMnXduxzIERUQI[] array = axes;
			TvINHnXPlmzTNPdylBZwpQjRHHQDA(array, kSDlSTSxKEWsjBuQXBuYndWzNbCC, timestamp);
			array = hats;
			TvINHnXPlmzTNPdylBZwpQjRHHQDA(array, kSDlSTSxKEWsjBuQXBuYndWzNbCC, timestamp);
			array = accelerometers;
			TvINHnXPlmzTNPdylBZwpQjRHHQDA(array, kSDlSTSxKEWsjBuQXBuYndWzNbCC, timestamp);
			array = gyroscopes;
			TvINHnXPlmzTNPdylBZwpQjRHHQDA(array, kSDlSTSxKEWsjBuQXBuYndWzNbCC, timestamp);
			array = touchpads;
			TvINHnXPlmzTNPdylBZwpQjRHHQDA(array, kSDlSTSxKEWsjBuQXBuYndWzNbCC, timestamp);
			byte b = kSDlSTSxKEWsjBuQXBuYndWzNbCC[53 + FbKEhhOeQqEzFzmexhJOIPLVpYhdb];
			laHiMsXIheMUuubRXCQPdLaHfqVu laHiMsXIheMUuubRXCQPdLaHfqVu2 = (laHiMsXIheMUuubRXCQPdLaHfqVu)((b & 0xF0) >> 4);
			if (laHiMsXIheMUuubRXCQPdLaHfqVu2 <= laHiMsXIheMUuubRXCQPdLaHfqVu.Full)
			{
				if (laHiMsXIheMUuubRXCQPdLaHfqVu2 > laHiMsXIheMUuubRXCQPdLaHfqVu.Charging)
				{
					if (laHiMsXIheMUuubRXCQPdLaHfqVu2 != laHiMsXIheMUuubRXCQPdLaHfqVu.Full)
					{
						goto IL_0171;
					}
					ldrELImcxfRgkeJEmTxXDlHZTjSL = 100;
					tDpqyWvRNikOizhKqbcrJJDTCtmR = DhGzVLCsLKQImEQBhLvwfmiKEEebA.Full;
				}
				else
				{
					ldrELImcxfRgkeJEmTxXDlHZTjSL = MathTools.Clamp((b & 0xF) * 10 + 5, 0, 100);
					tDpqyWvRNikOizhKqbcrJJDTCtmR = ((laHiMsXIheMUuubRXCQPdLaHfqVu2 != laHiMsXIheMUuubRXCQPdLaHfqVu.Charging) ? DhGzVLCsLKQImEQBhLvwfmiKEEebA.Discharging : DhGzVLCsLKQImEQBhLvwfmiKEEebA.Charging);
				}
			}
			else
			{
				if (laHiMsXIheMUuubRXCQPdLaHfqVu2 - 10 > laHiMsXIheMUuubRXCQPdLaHfqVu.Charging)
				{
					if (laHiMsXIheMUuubRXCQPdLaHfqVu2 == laHiMsXIheMUuubRXCQPdLaHfqVu.ChargingError)
					{
					}
					goto IL_0171;
				}
				ldrELImcxfRgkeJEmTxXDlHZTjSL = 0;
				tDpqyWvRNikOizhKqbcrJJDTCtmR = DhGzVLCsLKQImEQBhLvwfmiKEEebA.Charging;
			}
			goto IL_017f;
			IL_0171:
			ldrELImcxfRgkeJEmTxXDlHZTjSL = 0;
			tDpqyWvRNikOizhKqbcrJJDTCtmR = DhGzVLCsLKQImEQBhLvwfmiKEEebA.Unknown;
			goto IL_017f;
			IL_017f:
			EHXKHfgOHXIHufnOtultDSLVxlST = (kSDlSTSxKEWsjBuQXBuYndWzNbCC[54 + FbKEhhOeQqEzFzmexhJOIPLVpYhdb] & 1) != 0;
			JrhNmYokfViKaNHIPZkevqxeRFLh[0] = vlkNpuUhbSGYYkwbxOHJwTpqFheN(DualSenseTriggerType.Left, kSDlSTSxKEWsjBuQXBuYndWzNbCC[43 + FbKEhhOeQqEzFzmexhJOIPLVpYhdb], kSDlSTSxKEWsjBuQXBuYndWzNbCC[48 + FbKEhhOeQqEzFzmexhJOIPLVpYhdb]);
			JrhNmYokfViKaNHIPZkevqxeRFLh[1] = vlkNpuUhbSGYYkwbxOHJwTpqFheN(DualSenseTriggerType.Right, kSDlSTSxKEWsjBuQXBuYndWzNbCC[42 + FbKEhhOeQqEzFzmexhJOIPLVpYhdb], kSDlSTSxKEWsjBuQXBuYndWzNbCC[48 + FbKEhhOeQqEzFzmexhJOIPLVpYhdb]);
			TxvZmIHEEqASgaWSCJEByROFPkOS();
			return true;
		}

		public override Controller.Extension CreateControllerExtension()
		{
			return new DualSenseExtension(this);
		}

		private void GfUWgzXJEGxBewHsIWqANzUIZGCA(NTgeZKbzmGIqlMGAIOSUBklVGTkNA P_0)
		{
			if (oPkkGDvhUNGYAfZtPmhFrGGNYMRDA)
			{
				aDODkAiyXsJZWqnhdaeZosGszebVA(P_0);
				oPkkGDvhUNGYAfZtPmhFrGGNYMRDA = false;
			}
		}

		private bool aDODkAiyXsJZWqnhdaeZosGszebVA(NTgeZKbzmGIqlMGAIOSUBklVGTkNA P_0)
		{
			HWJPKCVQvtUGnlvvKSDpkjQCOzlq();
			bool result = NJGAsBFVunPKgDWhriFhrgUZsWADA(P_0);
			if (aUOCwOCAqBnFTDPgaCPnBCTqtnAfA)
			{
				result = NJGAsBFVunPKgDWhriFhrgUZsWADA(P_0);
				aUOCwOCAqBnFTDPgaCPnBCTqtnAfA = false;
			}
			return result;
		}

		private void HWJPKCVQvtUGnlvvKSDpkjQCOzlq()
		{
			if (BsXbtMXxpzmhGuljuFLrJzEFSAOb && zLSJTubeqKfYYddzoxwkzNoQUBFs)
			{
				EdXCodfulaYKaAkNDfgESYNmUdQQ[0] = 49;
				EdXCodfulaYKaAkNDfgESYNmUdQQ[1] = 2;
				aAIaYywFpHloqzPaopYtPYHtdOD(EdXCodfulaYKaAkNDfgESYNmUdQQ, 2);
				uint num = GiuawUmfXSYLrfkvobyKwTxxEjaf(EdXCodfulaYKaAkNDfgESYNmUdQQ, 74);
				EdXCodfulaYKaAkNDfgESYNmUdQQ[74] = (byte)(num & 0xFF);
				EdXCodfulaYKaAkNDfgESYNmUdQQ[75] = (byte)((num & 0xFF00) >> 8);
				EdXCodfulaYKaAkNDfgESYNmUdQQ[76] = (byte)((num & 0xFF0000) >> 16);
				EdXCodfulaYKaAkNDfgESYNmUdQQ[77] = (byte)((num & 0xFF000000u) >> 24);
			}
			else
			{
				EdXCodfulaYKaAkNDfgESYNmUdQQ[0] = 2;
				aAIaYywFpHloqzPaopYtPYHtdOD(EdXCodfulaYKaAkNDfgESYNmUdQQ, 1);
			}
		}

		private void aAIaYywFpHloqzPaopYtPYHtdOD(NativeBuffer P_0, int P_1)
		{
			WCdzkDrrxMKpWaFgQbxIskTxYhFd wCdzkDrrxMKpWaFgQbxIskTxYhFd = WCdzkDrrxMKpWaFgQbxIskTxYhFd.None;
			xTFKlrBsWaerOMPRIJPSjYDTlCMq xTFKlrBsWaerOMPRIJPSjYDTlCMq2 = xTFKlrBsWaerOMPRIJPSjYDTlCMq.None;
			wCdzkDrrxMKpWaFgQbxIskTxYhFd |= WCdzkDrrxMKpWaFgQbxIskTxYhFd.HapticsSelect;
			if (XgWbvciWbhfcgueTgSHMpnZUquDYA == DualSenseVibrationMode.Compatible)
			{
				wCdzkDrrxMKpWaFgQbxIskTxYhFd |= WCdzkDrrxMKpWaFgQbxIskTxYhFd.CompatibleVibrationMode1;
			}
			KYVrNCSXAQNwxwqsADVWNIAkBIvi = false;
			wCdzkDrrxMKpWaFgQbxIskTxYhFd |= WCdzkDrrxMKpWaFgQbxIskTxYhFd.LeftTriggerEffect;
			JSDdpkdIhZgTBevUzPZreWabKaTcc = false;
			wCdzkDrrxMKpWaFgQbxIskTxYhFd |= WCdzkDrrxMKpWaFgQbxIskTxYhFd.RightTriggerEffect;
			ZvlvNaCYQSIVFPMSJCScbzutFGlOA = false;
			xTFKlrBsWaerOMPRIJPSjYDTlCMq2 |= xTFKlrBsWaerOMPRIJPSjYDTlCMq.MicrophoneLEDControl;
			QdjYRnQWXJTqSsfxhokyjNhFqhmy = false;
			xTFKlrBsWaerOMPRIJPSjYDTlCMq2 |= xTFKlrBsWaerOMPRIJPSjYDTlCMq.PlayerIndicatorLEDControl;
			MzMqDgvuxyowjAHRvCWttelqDjOn = false;
			xTFKlrBsWaerOMPRIJPSjYDTlCMq2 |= xTFKlrBsWaerOMPRIJPSjYDTlCMq.LightbarControl;
			xSKOKiANxDIDxoFwVryhTjtAUuYL = false;
			xTFKlrBsWaerOMPRIJPSjYDTlCMq2 |= xTFKlrBsWaerOMPRIJPSjYDTlCMq.ChangeOverallMotorEffectPower;
			CIkRAvVlxNwxnaopYhITcsWChkwg = false;
			P_0[P_1] = (byte)wCdzkDrrxMKpWaFgQbxIskTxYhFd;
			P_0[1 + P_1] = (byte)xTFKlrBsWaerOMPRIJPSjYDTlCMq2;
			P_0[2 + P_1] = (byte)vibrationMotors[1].ZcjoZwbIDbbFlaWQFjFKWrESBVuu;
			P_0[3 + P_1] = (byte)vibrationMotors[0].ZcjoZwbIDbbFlaWQFjFKWrESBVuu;
			P_0[8 + P_1] = (byte)zmHCrfeLSmvptoolIlJLWVETjqbhb;
			UBBwVUGzhwMRjjVzKisTLefcYmFc uBBwVUGzhwMRjjVzKisTLefcYmFc = UBBwVUGzhwMRjjVzKisTLefcYmFc.None;
			uBBwVUGzhwMRjjVzKisTLefcYmFc |= UBBwVUGzhwMRjjVzKisTLefcYmFc.OtherLightBrightnessControl;
			vZqGJejnRMcOQibDhFaPsvtfZlso = false;
			if (XgWbvciWbhfcgueTgSHMpnZUquDYA == DualSenseVibrationMode.Compatible2)
			{
				uBBwVUGzhwMRjjVzKisTLefcYmFc |= UBBwVUGzhwMRjjVzKisTLefcYmFc.CompatibleVibrationMode2;
			}
			uBBwVUGzhwMRjjVzKisTLefcYmFc |= UBBwVUGzhwMRjjVzKisTLefcYmFc.LightbarSetupControl;
			XQBvpupfKfYkJCkcPQBMebqSmBQw = false;
			P_0[38 + P_1] = (byte)uBBwVUGzhwMRjjVzKisTLefcYmFc;
			P_0[41 + P_1] = gvuWfTKuZlsEcwFppAgAAZsOYzWc;
			P_0[42 + P_1] = (byte)ZAJtaEzeCwHNmwkvIyHqKnGgWdwX;
			P_0[43 + P_1] = (byte)EPgmhWLeGMzSwpXjouvJpTNpMGLE;
			if (KydkujeNcQvxtoqlXFDtAZOsHZmK)
			{
				P_0[43 + P_1] = (byte)(P_0[43 + P_1] & -33);
			}
			else
			{
				P_0[43 + P_1] |= 32;
			}
			P_0[44 + P_1] = lights[0].dzlPvBalHRSfegtkxkAECZRZUliD;
			P_0[45 + P_1] = lights[0].pUNnpXbqlHMdMbFBrwAbNRJiZxKR;
			P_0[46 + P_1] = lights[0].ZfhhhzCONloJjmcuIfFhItNGYTyBc;
			IdPalUklSyCqFOZPBUUOZDQKLVYe(ref AfycKPjGSkxgfXjzGrxBOuWEmgje[1], P_0, 10 + P_1);
			IdPalUklSyCqFOZPBUUOZDQKLVYe(ref AfycKPjGSkxgfXjzGrxBOuWEmgje[0], P_0, 21 + P_1);
			P_0[36 + P_1] = 0;
		}

		private void IdPalUklSyCqFOZPBUUOZDQKLVYe(ref IDualSenseTriggerEffect P_0, NativeBuffer P_1, int P_2)
		{
			if (P_0 == null)
			{
				P_1[P_2] = 0;
				return;
			}
			switch (P_0.triggerEffectType)
			{
			case DualSenseTriggerEffectType.Off:
				EJzFSrsMNbGNzubpPeFdJsAJcOLRA.UIRTnphNpaFOERJnDwUKhkRiacZi.GReSEOpmUWZKBSgBujFYCxgLzrdR(xgoUmwxPcdiTTihziXEllQjoUghAA, 0);
				break;
			case DualSenseTriggerEffectType.Feedback:
			{
				DualSenseTriggerEffectFeedback dualSenseTriggerEffectFeedback = (DualSenseTriggerEffectFeedback)(object)P_0;
				EJzFSrsMNbGNzubpPeFdJsAJcOLRA.UIRTnphNpaFOERJnDwUKhkRiacZi.ktyBdcvadDVJGMlHsmWffgfuPlQh(xgoUmwxPcdiTTihziXEllQjoUghAA, 0, dualSenseTriggerEffectFeedback.position, dualSenseTriggerEffectFeedback.strength);
				break;
			}
			case DualSenseTriggerEffectType.Weapon:
			{
				DualSenseTriggerEffectWeapon dualSenseTriggerEffectWeapon = (DualSenseTriggerEffectWeapon)(object)P_0;
				EJzFSrsMNbGNzubpPeFdJsAJcOLRA.UIRTnphNpaFOERJnDwUKhkRiacZi.kRLCTTPotfKidDADCGtRHxoKRTwF(xgoUmwxPcdiTTihziXEllQjoUghAA, 0, dualSenseTriggerEffectWeapon.startPosition, dualSenseTriggerEffectWeapon.endPosition, dualSenseTriggerEffectWeapon.strength);
				break;
			}
			case DualSenseTriggerEffectType.Vibration:
			{
				DualSenseTriggerEffectVibration dualSenseTriggerEffectVibration = (DualSenseTriggerEffectVibration)(object)P_0;
				EJzFSrsMNbGNzubpPeFdJsAJcOLRA.UIRTnphNpaFOERJnDwUKhkRiacZi.QzylqyVnMwflmifRYKBPLNcngKQL(xgoUmwxPcdiTTihziXEllQjoUghAA, 0, dualSenseTriggerEffectVibration.position, dualSenseTriggerEffectVibration.amplitude, dualSenseTriggerEffectVibration.frequency);
				break;
			}
			case DualSenseTriggerEffectType.MultiplePositionFeedback:
				((DualSenseTriggerEffectMultiplePositionFeedback)(object)P_0).strength.CopyTo(vtYWhyGuuPAxkKjMdqbSZTLlhdc);
				EJzFSrsMNbGNzubpPeFdJsAJcOLRA.UIRTnphNpaFOERJnDwUKhkRiacZi.aJXiAbSRLLezAetUPJIZGDOccujlc(xgoUmwxPcdiTTihziXEllQjoUghAA, 0, vtYWhyGuuPAxkKjMdqbSZTLlhdc);
				break;
			case DualSenseTriggerEffectType.SlopeFeedback:
			{
				DualSenseTriggerEffectSlopeFeedback dualSenseTriggerEffectSlopeFeedback = (DualSenseTriggerEffectSlopeFeedback)(object)P_0;
				EJzFSrsMNbGNzubpPeFdJsAJcOLRA.UIRTnphNpaFOERJnDwUKhkRiacZi.ivlaolcKrkGwxtlErxaDQIUfMjAoA(xgoUmwxPcdiTTihziXEllQjoUghAA, 0, dualSenseTriggerEffectSlopeFeedback.startPosition, dualSenseTriggerEffectSlopeFeedback.endPosition, dualSenseTriggerEffectSlopeFeedback.startStrength, dualSenseTriggerEffectSlopeFeedback.endStrength);
				break;
			}
			case DualSenseTriggerEffectType.MultiplePositionVibration:
			{
				DualSenseTriggerEffectMultiplePositionVibration dualSenseTriggerEffectMultiplePositionVibration = (DualSenseTriggerEffectMultiplePositionVibration)(object)P_0;
				dualSenseTriggerEffectMultiplePositionVibration.amplitude.CopyTo(vtYWhyGuuPAxkKjMdqbSZTLlhdc);
				EJzFSrsMNbGNzubpPeFdJsAJcOLRA.UIRTnphNpaFOERJnDwUKhkRiacZi.AcaUVfcbLabzaUOqJwGbbGADRJyG(xgoUmwxPcdiTTihziXEllQjoUghAA, 0, dualSenseTriggerEffectMultiplePositionVibration.frequency, vtYWhyGuuPAxkKjMdqbSZTLlhdc);
				break;
			}
			default:
				Logger.LogWarning("Unknown trigger effect type: 0x" + ((byte)P_0.triggerEffectType).ToString("x2"));
				return;
			}
			P_1.Write(xgoUmwxPcdiTTihziXEllQjoUghAA, xgoUmwxPcdiTTihziXEllQjoUghAA.Length, P_2);
		}

		private bool NJGAsBFVunPKgDWhriFhrgUZsWADA(NTgeZKbzmGIqlMGAIOSUBklVGTkNA P_0)
		{
			firSKKwlMnvjOjJTLaRbCfqnLwKJ = ReInput.realTime + 4.0;
			switch (P_0)
			{
			case NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Synchronous:
				return FingbDpFDpIsWbuwXtHuWqWRuqHc.WriteSync(jVjfvBdAVQAbCbGwqzkDczYoMSwE, 0);
			case NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Asynchronous:
				FingbDpFDpIsWbuwXtHuWqWRuqHc.WriteAsync(jVjfvBdAVQAbCbGwqzkDczYoMSwE, 1000);
				return true;
			default:
				throw new NotImplementedException();
			}
		}

		private void yEEymumRmZHgaVeDMRAgYJOxZmbU(NativeBuffer P_0, double P_1)
		{
			byte b = P_0[UWmFaEVMSAOGZzAVltYRLSZYcELX];
			buttons[0].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 0x10) != 0, P_1);
			buttons[1].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 0x20) != 0, P_1);
			buttons[2].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 0x40) != 0, P_1);
			buttons[3].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 0x80) != 0, P_1);
			b = P_0[xYUlpMqLMPjCfGRSlIKTKpJYFGEfA];
			buttons[4].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 1) != 0, P_1);
			buttons[5].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 2) != 0, P_1);
			buttons[6].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 4) != 0, P_1);
			buttons[7].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 8) != 0, P_1);
			buttons[8].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 0x10) != 0, P_1);
			buttons[9].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 0x20) != 0, P_1);
			buttons[10].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 0x40) != 0, P_1);
			buttons[11].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 0x80) != 0, P_1);
			b = P_0[CBBRBvZHmxcsoYpRrzStbnjczKad];
			buttons[12].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 1) != 0, P_1);
			buttons[13].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 2) != 0, P_1);
			if (zLSJTubeqKfYYddzoxwkzNoQUBFs)
			{
				buttons[14].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 4) != 0, P_1);
			}
		}

		private void TvINHnXPlmzTNPdylBZwpQjRHHQDA(LDJGvqLnFydDhJMnXduxzIERUQI[] P_0, NativeBuffer P_1, double P_2)
		{
			for (int i = 0; i < P_0.Length; i++)
			{
				P_0[i].asArJiunXbfpvgEDUosbEuyCYgWWA(P_1, P_2);
			}
		}

		private void mVnrAdpwkzHDGTEcbCCihPvfuMrb()
		{
			if (isVibrating && ReInput.realTime >= firSKKwlMnvjOjJTLaRbCfqnLwKJ)
			{
				UDIiOMasWGczisiwnUFhftrBRTAkA();
				KYVrNCSXAQNwxwqsADVWNIAkBIvi = true;
			}
		}

		private void KycRkxUEkLJaeDQVDZiBNCdqFJaW(NativeBuffer P_0)
		{
			if (zLSJTubeqKfYYddzoxwkzNoQUBFs)
			{
				uint num = kSDlSTSxKEWsjBuQXBuYndWzNbCC.ReadUInt(28 + FbKEhhOeQqEzFzmexhJOIPLVpYhdb);
				float num3;
				if (num != OWgzBoMVcBvrtLAQDwNjRJLaXduW)
				{
					uint num2 = (uint)((num >= OWgzBoMVcBvrtLAQDwNjRJLaXduW) ? (num - OWgzBoMVcBvrtLAQDwNjRJLaXduW) : ((long)num + 4294967295L - OWgzBoMVcBvrtLAQDwNjRJLaXduW));
					num3 = (float)num2 / 3000000f;
				}
				else
				{
					uint num2 = 0u;
					num3 = 0f;
				}
				OWgzBoMVcBvrtLAQDwNjRJLaXduW = num;
				cjmVAzYHgkXMZAbeetYBVXkAGEOd = num3;
			}
		}

		private void TxvZmIHEEqASgaWSCJEByROFPkOS()
		{
			if (zLSJTubeqKfYYddzoxwkzNoQUBFs && !(cjmVAzYHgkXMZAbeetYBVXkAGEOd <= 0f))
			{
				Vector3 vector = iidbXhYuwXwMuryOnvOwHyhZCZge(new Vector3(gyroscopes[0].HViVbfIrDiBECuxWlqkhZscDKxn[0], gyroscopes[0].HViVbfIrDiBECuxWlqkhZscDKxn[1], gyroscopes[0].HViVbfIrDiBECuxWlqkhZscDKxn[2]), cjmVAzYHgkXMZAbeetYBVXkAGEOd);
				YMBINWNaGeFZPGsOuFsisbsyWpeH(ref vector);
				Vector3 vector2 = new Vector3(accelerometers[0].SytbxvDfrRdLDWckugtMRDSBscWP[0] * -1f, accelerometers[0].SytbxvDfrRdLDWckugtMRDSBscWP[1] * -1f, accelerometers[0].SytbxvDfrRdLDWckugtMRDSBscWP[2] * -1f);
				kzWLUkQdQlHZxJpkaQuWvPIqIxcs(vector2, vector);
			}
		}

		private static bool YMBINWNaGeFZPGsOuFsisbsyWpeH(ref Vector3 P_0)
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

		private void kzWLUkQdQlHZxJpkaQuWvPIqIxcs(Vector3 P_0, Vector3 P_1)
		{
			Quaternion quaternion = Quaternion.Euler(P_1);
			float sqrMagnitude = P_0.sqrMagnitude;
			if (sqrMagnitude > 16777216f && sqrMagnitude < 268435460f && oKCdqGqdTweZkCulFlpKdfFdcpgZA(P_0, out var xxfCKEQgVKfvKeqXIqrTJAAtaxhBA2))
			{
				Quaternion a = jzwEYubZtNyIWPqMlJovBwHpcZIg * quaternion;
				if (!rLWRwJuRJtzTiLIeeRCOmLXoxwJ)
				{
					rLWRwJuRJtzTiLIeeRCOmLXoxwJ = true;
					ZmVYfzOldHzoPHzUkkZsHlKfyzHG = Quaternion.identity * Quaternion.Euler(new Vector3(90f, 0f, 0f));
					rsmgSbCEBKlsmjPOWdHIwfayGpNw = jzwEYubZtNyIWPqMlJovBwHpcZIg;
				}
				ZmVYfzOldHzoPHzUkkZsHlKfyzHG *= quaternion;
				rsmgSbCEBKlsmjPOWdHIwfayGpNw *= quaternion;
				Quaternion b;
				if ((xxfCKEQgVKfvKeqXIqrTJAAtaxhBA2 & xxfCKEQgVKfvKeqXIqrTJAAtaxhBA.XZ) != xxfCKEQgVKfvKeqXIqrTJAAtaxhBA.None)
				{
					b = oPiEYlEJqgIbEEVwoQbKiVqIZNcX(P_0, a.eulerAngles.y);
				}
				else if ((xxfCKEQgVKfvKeqXIqrTJAAtaxhBA2 & xxfCKEQgVKfvKeqXIqrTJAAtaxhBA.Y) != xxfCKEQgVKfvKeqXIqrTJAAtaxhBA.None)
				{
					b = tErSNhzONcnqSpYmpMJUckimqhgR(P_0);
					Vector3 vector = rsmgSbCEBKlsmjPOWdHIwfayGpNw * Vector3.right;
					float y = 0f - MathTools.SignedAngle(new Vector3(vector.x, 0f, vector.z), Vector3.right, Vector3.up);
					b = Quaternion.Euler(0f, y, 0f) * b;
				}
				else
				{
					b = Quaternion.identity;
				}
				jzwEYubZtNyIWPqMlJovBwHpcZIg = Quaternion.Lerp(a, b, 0.01999998f);
			}
			else
			{
				jzwEYubZtNyIWPqMlJovBwHpcZIg *= quaternion;
				if (rLWRwJuRJtzTiLIeeRCOmLXoxwJ)
				{
					rLWRwJuRJtzTiLIeeRCOmLXoxwJ = false;
				}
			}
		}

		private Quaternion oPiEYlEJqgIbEEVwoQbKiVqIZNcX(Vector3 P_0, float P_1 = 0f)
		{
			float num = MathTools.Atan2(P_0.z, P_0.y);
			float num2 = MathTools.Atan2(x: MathTools.Sqrt(MathTools.Pow(P_0.y, 2f) + MathTools.Pow(P_0.z, 2f)), y: P_0.x);
			float x = num * 57.29578f + 180f;
			float z = (0f - num2) * 57.29578f;
			return Quaternion.Euler(x, P_1, z);
		}

		private Quaternion tErSNhzONcnqSpYmpMJUckimqhgR(Vector3 P_0, float P_1 = 0f)
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

		private bool oKCdqGqdTweZkCulFlpKdfFdcpgZA(Vector3 P_0, out xxfCKEQgVKfvKeqXIqrTJAAtaxhBA P_1)
		{
			P_0.Normalize();
			P_1 = xxfCKEQgVKfvKeqXIqrTJAAtaxhBA.None;
			bool result = false;
			if (yIeFlYIQSvPjElvIFomwfPeErquwB(P_0))
			{
				result = true;
				P_1 |= xxfCKEQgVKfvKeqXIqrTJAAtaxhBA.XZ;
			}
			if (ulDAvUgFMKJoTeWexxBcLRVvxqPKA(P_0))
			{
				result = true;
				P_1 |= xxfCKEQgVKfvKeqXIqrTJAAtaxhBA.Y;
			}
			return result;
		}

		private bool yIeFlYIQSvPjElvIFomwfPeErquwB(Vector3 P_0)
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

		private bool ulDAvUgFMKJoTeWexxBcLRVvxqPKA(Vector3 P_0)
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

		private Vector3 mPVqelTISgJkSgPsxBZdPcVEdbBm(float[] P_0)
		{
			return new Vector3(P_0[0] * 0.00012207031f * -1f, P_0[1] * 0.00012207031f * -1f, P_0[2] * 0.00012207031f);
		}

		private Vector3 BToLyliABckLcpXhpWbglObHGmSQ(RingBuffer<zeduVYzSnJpVQGxDoGRFMdphEaCi.hOJhFTpGFkIeuGuGckkEoiyPlXuc> P_0)
		{
			Vector3 result = default(Vector3);
			int count = P_0.Count;
			for (int i = 0; i < count; i++)
			{
				zeduVYzSnJpVQGxDoGRFMdphEaCi.hOJhFTpGFkIeuGuGckkEoiyPlXuc hOJhFTpGFkIeuGuGckkEoiyPlXuc = P_0[i];
				result += iidbXhYuwXwMuryOnvOwHyhZCZge(hOJhFTpGFkIeuGuGckkEoiyPlXuc.ZCNwuekgJmAkwEDhmlrFhlleBLIy, hOJhFTpGFkIeuGuGckkEoiyPlXuc.pEjIWtERgNgCAQHSarniWniWPwXdb);
			}
			return result;
		}

		private Vector3 iidbXhYuwXwMuryOnvOwHyhZCZge(Vector3 P_0, float P_1)
		{
			P_0.x *= -1f;
			P_0.y *= -1f;
			return P_0 * 0.06103702f * P_1;
		}

		private int IAmPQlGDVLihHddhiePkkpdunlcIb(int P_0)
		{
			P_0 &= 0xF;
			return P_0;
		}

		private void jzcdVtMgAeRFLAEDNsQXdUzNaPkg(byte[] P_0, float[] P_1)
		{
			P_1[0] = BitConverter.ToInt16(P_0, 0);
			P_1[1] = BitConverter.ToInt16(P_0, 2);
			P_1[2] = BitConverter.ToInt16(P_0, 4);
		}

		private void vkhmvkHwHqSArnlDQQpeumQBTJav(byte[] P_0, float[] P_1)
		{
			P_1[0] = BitConverter.ToInt16(P_0, 0);
			P_1[1] = BitConverter.ToInt16(P_0, 2);
			P_1[2] = BitConverter.ToInt16(P_0, 4);
		}

		private float fLTurVzWIHTQilKqxQXXhYDlsxqq()
		{
			return cjmVAzYHgkXMZAbeetYBVXkAGEOd;
		}

		private void dGPhqgmFmYZDQZlJZeusGxpLfGyA(NativeBuffer P_0, JeEihaxNGDZUEopEZTyRorKoTSAm.TouchData[] P_1)
		{
			int num = 33 + FbKEhhOeQqEzFzmexhJOIPLVpYhdb;
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
			P_1[0].touchId = JlAPYmBIyAcIukAZTAXCkGWeDIlAA(0, flag, num3);
			P_1[0].positionRawX = positionRawX;
			P_1[0].positionRawY = positionRawY;
			P_1[1].isTouching = flag2;
			P_1[1].touchId = JlAPYmBIyAcIukAZTAXCkGWeDIlAA(1, flag2, num4);
			P_1[1].positionRawX = positionRawX2;
			P_1[1].positionRawY = positionRawY2;
		}

		private int JlAPYmBIyAcIukAZTAXCkGWeDIlAA(int P_0, bool P_1, int P_2)
		{
			if (!P_1)
			{
				IQECmmkTEpXHUQjyvOKcuweGcnSv[P_0] = -1;
				tYzcliEPOJbUHwUGVRAFCDixheImA[P_0] = P_2;
				return -1;
			}
			if (P_2 != tYzcliEPOJbUHwUGVRAFCDixheImA[P_0])
			{
				int num = ecVBbklpuikHRewxBeiTqFHZkjfM;
				if (ecVBbklpuikHRewxBeiTqFHZkjfM == 2147483647)
				{
					ecVBbklpuikHRewxBeiTqFHZkjfM = 0;
				}
				else
				{
					ecVBbklpuikHRewxBeiTqFHZkjfM++;
				}
				tYzcliEPOJbUHwUGVRAFCDixheImA[P_0] = P_2;
				IQECmmkTEpXHUQjyvOKcuweGcnSv[P_0] = num;
				return num;
			}
			return IQECmmkTEpXHUQjyvOKcuweGcnSv[P_0];
		}

		private void hWttkVDokzaZjWaKkqcWXIoXxWPH()
		{
			xSKOKiANxDIDxoFwVryhTjtAUuYL = true;
			UDIiOMasWGczisiwnUFhftrBRTAkA();
		}

		private void PSxDGSEpWckTXsvxntiolsbkGGeR()
		{
			xSKOKiANxDIDxoFwVryhTjtAUuYL = true;
			UDIiOMasWGczisiwnUFhftrBRTAkA();
		}

		private void wrXPLXbIIeeWtwcUQUKqPzWVrVvR()
		{
			KYVrNCSXAQNwxwqsADVWNIAkBIvi = true;
			UDIiOMasWGczisiwnUFhftrBRTAkA();
		}

		private void UDIiOMasWGczisiwnUFhftrBRTAkA()
		{
			oPkkGDvhUNGYAfZtPmhFrGGNYMRDA = true;
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
				GfUWgzXJEGxBewHsIWqANzUIZGCA(NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Synchronous);
				if (kSDlSTSxKEWsjBuQXBuYndWzNbCC != null)
				{
					kSDlSTSxKEWsjBuQXBuYndWzNbCC.Dispose();
				}
				if (EdXCodfulaYKaAkNDfgESYNmUdQQ != null)
				{
					EdXCodfulaYKaAkNDfgESYNmUdQQ.Dispose();
				}
			}
		}

		public static bool Matches(int vid, int pid)
		{
			if (pid == 3302)
			{
				return vid == 1356;
			}
			return false;
		}

		private static uint GiuawUmfXSYLrfkvobyKwTxxEjaf(NativeBuffer P_0, int P_1)
		{
			uint num = 3940166985u;
			for (int i = 0; i < P_1; i++)
			{
				num = lUXGBMRBHrhLHVodUElIdDXSAmBeb[(byte)num ^ P_0[i]] ^ (num >> 8);
			}
			return num;
		}

		private static aIwgqjDyNnaPFxVBqcJTPSbxIDRdA aXQscBWSmdiSSyLcSsHGCDNGjynL(DualSenseOtherLightBrightness P_0)
		{
			return P_0 switch
			{
				DualSenseOtherLightBrightness.High => aIwgqjDyNnaPFxVBqcJTPSbxIDRdA.High, 
				DualSenseOtherLightBrightness.Medium => aIwgqjDyNnaPFxVBqcJTPSbxIDRdA.Medium, 
				DualSenseOtherLightBrightness.Low => aIwgqjDyNnaPFxVBqcJTPSbxIDRdA.Low, 
				_ => throw new NotImplementedException(), 
			};
		}

		private static DualSenseOtherLightBrightness RRmmbySSUVcrpNJooNwhEIHEUdAE(aIwgqjDyNnaPFxVBqcJTPSbxIDRdA P_0)
		{
			return P_0 switch
			{
				aIwgqjDyNnaPFxVBqcJTPSbxIDRdA.High => DualSenseOtherLightBrightness.High, 
				aIwgqjDyNnaPFxVBqcJTPSbxIDRdA.Medium => DualSenseOtherLightBrightness.Medium, 
				aIwgqjDyNnaPFxVBqcJTPSbxIDRdA.Low => DualSenseOtherLightBrightness.Low, 
				_ => throw new NotImplementedException(), 
			};
		}

		private static JDCDCcBEnhvdVErEgATkBpGJfPGfb UqweyChSFABYSAeTfdQZVGzdDluFA(DualSenseTriggerType P_0, byte P_1)
		{
			byte b;
			switch (P_0)
			{
			case DualSenseTriggerType.Left:
				b = new hjTCZuEfWUjEIEmfyaDgGaZDTELnB(P_1).TBsoBwBGoOUixuWdKQsSSRtqrzoL;
				break;
			case DualSenseTriggerType.Right:
				b = new hjTCZuEfWUjEIEmfyaDgGaZDTELnB(P_1).jAVnrampYCrFfuNCUCBvbOCSRLEo;
				break;
			default:
				return JDCDCcBEnhvdVErEgATkBpGJfPGfb.Off;
			}
			return b switch
			{
				0 => JDCDCcBEnhvdVErEgATkBpGJfPGfb.Off, 
				1 => JDCDCcBEnhvdVErEgATkBpGJfPGfb.Feedback, 
				2 => JDCDCcBEnhvdVErEgATkBpGJfPGfb.Weapon, 
				3 => JDCDCcBEnhvdVErEgATkBpGJfPGfb.Vibration, 
				4 => JDCDCcBEnhvdVErEgATkBpGJfPGfb.SlopeFeedback, 
				_ => JDCDCcBEnhvdVErEgATkBpGJfPGfb.Off, 
			};
		}

		private static DualSenseTriggerEffectState vlkNpuUhbSGYYkwbxOHJwTpqFheN(DualSenseTriggerType P_0, byte P_1, byte P_2)
		{
			byte b = new hjTCZuEfWUjEIEmfyaDgGaZDTELnB(P_1).TBsoBwBGoOUixuWdKQsSSRtqrzoL;
			return UqweyChSFABYSAeTfdQZVGzdDluFA(P_0, P_2) switch
			{
				JDCDCcBEnhvdVErEgATkBpGJfPGfb.Off => DualSenseTriggerEffectState.Off, 
				JDCDCcBEnhvdVErEgATkBpGJfPGfb.Feedback => b switch
				{
					0 => DualSenseTriggerEffectState.FeedbackIdle, 
					1 => DualSenseTriggerEffectState.FeedbackApplyingForce, 
					_ => DualSenseTriggerEffectState.FeedbackIdle, 
				}, 
				JDCDCcBEnhvdVErEgATkBpGJfPGfb.Weapon => b switch
				{
					0 => DualSenseTriggerEffectState.WeaponIdle, 
					1 => DualSenseTriggerEffectState.WeaponFiring, 
					2 => DualSenseTriggerEffectState.WeaponFired, 
					_ => DualSenseTriggerEffectState.WeaponIdle, 
				}, 
				JDCDCcBEnhvdVErEgATkBpGJfPGfb.Vibration => b switch
				{
					0 => DualSenseTriggerEffectState.VibrationIdle, 
					1 => DualSenseTriggerEffectState.VibrationVibrating, 
					_ => DualSenseTriggerEffectState.VibrationIdle, 
				}, 
				JDCDCcBEnhvdVErEgATkBpGJfPGfb.SlopeFeedback => b switch
				{
					0 => (DualSenseTriggerEffectState)8, 
					1 => (DualSenseTriggerEffectState)9, 
					2 => (DualSenseTriggerEffectState)10, 
					_ => (DualSenseTriggerEffectState)8, 
				}, 
				_ => DualSenseTriggerEffectState.Off, 
			};
		}
	}
}
