using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Rewired;
using Rewired.Libraries.SharpDX.RawInput;
using Rewired.Utils;
using Rewired.Utils.Attributes;
using Rewired.Utils.Classes.Data;

internal class vaMEXWmtmhgkkjmQIaKcHyMbMajS
{
	private static readonly pNjEQKwNMacTPSVRvkuPNTsvHycD qLsFbYUgGBTRsPUEBUKSaThsgBcA;

	private const uint FcMdYNuaSIAgCoXaquVOEbbeBaiQ = 8192u;

	private const uint teTQJubXaUhUMyoHJjXwhjCiplJxA = 100u;

	private const uint wDmBRaImpTUINtqHUgRqonvpHJWfb = 8192u;

	private static IntPtr RSCMNMhSJzIMVMOUahSfVpBvbLuy;

	private static bool sZTTJMDBuidjvmcbbWVBqKTbFsWL;

	private static IntPtr PTneWUErjbWOjdUUAqatZxhuOegxA;

	private static bool dfBPgqqPnHblTSdXBgYBHznPjkwOA;

	private static readonly int NXknlxLcEHMiPPCmbxMcpFvBaFgaA;

	private static readonly int yesRKLyGMsQYmrMHfwfBUpCHbXEEA;

	private static readonly NativeBuffer kVnSJqQSRWojFXHiktszxjFliVXW;

	private static readonly bool EaghbIHnDldiplwewfVhLKplXGOW;

	private static readonly byte[] InnRcubJLSsWWNBtDNrbCGLxmFgq;

	private static readonly uint[] NmCNzIPYMOAtynpfcUxIaEQGLqzR;

	private static readonly uint[] wrVcoifDOzaFYLglyOhpdveBPerbB;

	private static readonly bool uOSCzXxSiWjQXbwRgFbineaqwrQmA;

	private static ForwardRawInputEventsToUnityDelegate eVSMwNDYHANQmeEWyygsOgrkrOkV;

	[CompilerGenerated]
	private static Action<VbuwuiLmwxfrQAPwNZAKAcROJgVgb, double> m_jbuKnmPOtrWNfuHYgKEIAmicwine;

	[CompilerGenerated]
	private static Action<qsOyzcrZVgqWvqYbLoowsSqEKWkk, double> m_iIMOJGJixgmmpvCrCpDcUgUIwvFY;

	[CompilerGenerated]
	private static Action<McirdyGKrseZIDeGHFqVBkIAUEeUB, double> m_IGgDxUtolvrbybgRykaplzGUZIlB;

	[CompilerGenerated]
	private static Action<IntPtr> m_GiHRsoeRcJMqWIxwkngZkFkTcMZo;

	[CompilerGenerated]
	private static Action m_JfHzQfUlnPjlIHhEBRUpfBHGlVeQ;

	public static ForwardRawInputEventsToUnityDelegate XVyoZYJxpGsdMWWWFukkMrNneVbD
	{
		get
		{
			return eVSMwNDYHANQmeEWyygsOgrkrOkV;
		}
		set
		{
			eVSMwNDYHANQmeEWyygsOgrkrOkV = forwardRawInputEventsToUnityDelegate;
		}
	}

	public static event Action<VbuwuiLmwxfrQAPwNZAKAcROJgVgb, double> jbuKnmPOtrWNfuHYgKEIAmicwine
	{
		[CompilerGenerated]
		add
		{
			Action<VbuwuiLmwxfrQAPwNZAKAcROJgVgb, double> action = vaMEXWmtmhgkkjmQIaKcHyMbMajS.m_jbuKnmPOtrWNfuHYgKEIAmicwine;
			Action<VbuwuiLmwxfrQAPwNZAKAcROJgVgb, double> action2;
			do
			{
				action2 = action;
				Action<VbuwuiLmwxfrQAPwNZAKAcROJgVgb, double> value2 = (Action<VbuwuiLmwxfrQAPwNZAKAcROJgVgb, double>)Delegate.Combine(action2, b);
				action = Interlocked.CompareExchange(ref vaMEXWmtmhgkkjmQIaKcHyMbMajS.m_jbuKnmPOtrWNfuHYgKEIAmicwine, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<VbuwuiLmwxfrQAPwNZAKAcROJgVgb, double> action = vaMEXWmtmhgkkjmQIaKcHyMbMajS.m_jbuKnmPOtrWNfuHYgKEIAmicwine;
			Action<VbuwuiLmwxfrQAPwNZAKAcROJgVgb, double> action2;
			do
			{
				action2 = action;
				Action<VbuwuiLmwxfrQAPwNZAKAcROJgVgb, double> value2 = (Action<VbuwuiLmwxfrQAPwNZAKAcROJgVgb, double>)Delegate.Remove(action2, value3);
				action = Interlocked.CompareExchange(ref vaMEXWmtmhgkkjmQIaKcHyMbMajS.m_jbuKnmPOtrWNfuHYgKEIAmicwine, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static event Action<qsOyzcrZVgqWvqYbLoowsSqEKWkk, double> iIMOJGJixgmmpvCrCpDcUgUIwvFY
	{
		[CompilerGenerated]
		add
		{
			Action<qsOyzcrZVgqWvqYbLoowsSqEKWkk, double> action = vaMEXWmtmhgkkjmQIaKcHyMbMajS.m_iIMOJGJixgmmpvCrCpDcUgUIwvFY;
			Action<qsOyzcrZVgqWvqYbLoowsSqEKWkk, double> action2;
			do
			{
				action2 = action;
				Action<qsOyzcrZVgqWvqYbLoowsSqEKWkk, double> value2 = (Action<qsOyzcrZVgqWvqYbLoowsSqEKWkk, double>)Delegate.Combine(action2, b);
				action = Interlocked.CompareExchange(ref vaMEXWmtmhgkkjmQIaKcHyMbMajS.m_iIMOJGJixgmmpvCrCpDcUgUIwvFY, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<qsOyzcrZVgqWvqYbLoowsSqEKWkk, double> action = vaMEXWmtmhgkkjmQIaKcHyMbMajS.m_iIMOJGJixgmmpvCrCpDcUgUIwvFY;
			Action<qsOyzcrZVgqWvqYbLoowsSqEKWkk, double> action2;
			do
			{
				action2 = action;
				Action<qsOyzcrZVgqWvqYbLoowsSqEKWkk, double> value2 = (Action<qsOyzcrZVgqWvqYbLoowsSqEKWkk, double>)Delegate.Remove(action2, value3);
				action = Interlocked.CompareExchange(ref vaMEXWmtmhgkkjmQIaKcHyMbMajS.m_iIMOJGJixgmmpvCrCpDcUgUIwvFY, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static event Action<McirdyGKrseZIDeGHFqVBkIAUEeUB, double> IGgDxUtolvrbybgRykaplzGUZIlB
	{
		[CompilerGenerated]
		add
		{
			Action<McirdyGKrseZIDeGHFqVBkIAUEeUB, double> action = vaMEXWmtmhgkkjmQIaKcHyMbMajS.m_IGgDxUtolvrbybgRykaplzGUZIlB;
			Action<McirdyGKrseZIDeGHFqVBkIAUEeUB, double> action2;
			do
			{
				action2 = action;
				Action<McirdyGKrseZIDeGHFqVBkIAUEeUB, double> value2 = (Action<McirdyGKrseZIDeGHFqVBkIAUEeUB, double>)Delegate.Combine(action2, b);
				action = Interlocked.CompareExchange(ref vaMEXWmtmhgkkjmQIaKcHyMbMajS.m_IGgDxUtolvrbybgRykaplzGUZIlB, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<McirdyGKrseZIDeGHFqVBkIAUEeUB, double> action = vaMEXWmtmhgkkjmQIaKcHyMbMajS.m_IGgDxUtolvrbybgRykaplzGUZIlB;
			Action<McirdyGKrseZIDeGHFqVBkIAUEeUB, double> action2;
			do
			{
				action2 = action;
				Action<McirdyGKrseZIDeGHFqVBkIAUEeUB, double> value2 = (Action<McirdyGKrseZIDeGHFqVBkIAUEeUB, double>)Delegate.Remove(action2, value3);
				action = Interlocked.CompareExchange(ref vaMEXWmtmhgkkjmQIaKcHyMbMajS.m_IGgDxUtolvrbybgRykaplzGUZIlB, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static event Action<IntPtr> GiHRsoeRcJMqWIxwkngZkFkTcMZo
	{
		[CompilerGenerated]
		add
		{
			Action<IntPtr> action = vaMEXWmtmhgkkjmQIaKcHyMbMajS.m_GiHRsoeRcJMqWIxwkngZkFkTcMZo;
			Action<IntPtr> action2;
			do
			{
				action2 = action;
				Action<IntPtr> value2 = (Action<IntPtr>)Delegate.Combine(action2, b);
				action = Interlocked.CompareExchange(ref vaMEXWmtmhgkkjmQIaKcHyMbMajS.m_GiHRsoeRcJMqWIxwkngZkFkTcMZo, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<IntPtr> action = vaMEXWmtmhgkkjmQIaKcHyMbMajS.m_GiHRsoeRcJMqWIxwkngZkFkTcMZo;
			Action<IntPtr> action2;
			do
			{
				action2 = action;
				Action<IntPtr> value2 = (Action<IntPtr>)Delegate.Remove(action2, value3);
				action = Interlocked.CompareExchange(ref vaMEXWmtmhgkkjmQIaKcHyMbMajS.m_GiHRsoeRcJMqWIxwkngZkFkTcMZo, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static event Action JfHzQfUlnPjlIHhEBRUpfBHGlVeQ
	{
		[CompilerGenerated]
		add
		{
			Action action = vaMEXWmtmhgkkjmQIaKcHyMbMajS.m_JfHzQfUlnPjlIHhEBRUpfBHGlVeQ;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, b);
				action = Interlocked.CompareExchange(ref vaMEXWmtmhgkkjmQIaKcHyMbMajS.m_JfHzQfUlnPjlIHhEBRUpfBHGlVeQ, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = vaMEXWmtmhgkkjmQIaKcHyMbMajS.m_JfHzQfUlnPjlIHhEBRUpfBHGlVeQ;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value3);
				action = Interlocked.CompareExchange(ref vaMEXWmtmhgkkjmQIaKcHyMbMajS.m_JfHzQfUlnPjlIHhEBRUpfBHGlVeQ, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	static vaMEXWmtmhgkkjmQIaKcHyMbMajS()
	{
		qLsFbYUgGBTRsPUEBUKSaThsgBcA = tYEbojeeeJlnHbWfdBsrnOIgnOEcd;
		NXknlxLcEHMiPPCmbxMcpFvBaFgaA = aOYtALpBiXtNUYUxrmqpvmqCyvKG.pbdlJJuDWMAxynFvrfVPNfMMBbuS<BfrWUppsjgDcoMLolPjFdAkdoqjP>();
		yesRKLyGMsQYmrMHfwfBUpCHbXEEA = aOYtALpBiXtNUYUxrmqpvmqCyvKG.pbdlJJuDWMAxynFvrfVPNfMMBbuS<NXPCBEDXCQFnUVkafhIcnwaFbhfxA>();
		EaghbIHnDldiplwewfVhLKplXGOW = UnityTools.windowsStandalone_supportsRawInputForwarding;
		if (EaghbIHnDldiplwewfVhLKplXGOW)
		{
			try
			{
				kVnSJqQSRWojFXHiktszxjFliVXW = new NativeBuffer(8192);
				InnRcubJLSsWWNBtDNrbCGLxmFgq = new byte[8192];
				NmCNzIPYMOAtynpfcUxIaEQGLqzR = new uint[100];
				wrVcoifDOzaFYLglyOhpdveBPerbB = new uint[100];
			}
			catch
			{
				EaghbIHnDldiplwewfVhLKplXGOW = false;
				Logger.LogError("Could not allocate memory for Raw Input buffer.", requiredThreadSafety: true);
			}
		}
		uOSCzXxSiWjQXbwRgFbineaqwrQmA = !SystemInfo.is64Bit && hUfdZejvJYlOtHWdillPtEZZfcOAA.ADSzpqwvSxTBgtWGcBYWKMvKpntb();
	}

	public static void kRHabkyclLZhGlktJIOsrAqSjijq(IntPtr P_0, bool P_1)
	{
		sZTTJMDBuidjvmcbbWVBqKTbFsWL = P_1;
		if (!(P_0 == IntPtr.Zero) && !(P_0 == RSCMNMhSJzIMVMOUahSfVpBvbLuy))
		{
			RSCMNMhSJzIMVMOUahSfVpBvbLuy = P_0;
			dfBPgqqPnHblTSdXBgYBHznPjkwOA = true;
		}
	}

	public static void MAEDnwgMOCaZIDsbDgWtZfWlSQJUB(bool P_0)
	{
		sZTTJMDBuidjvmcbbWVBqKTbFsWL = P_0;
	}

	public static pNjEQKwNMacTPSVRvkuPNTsvHycD FCMDoqTGmOCyvNSwaCShjgdSdPaAb()
	{
		return qLsFbYUgGBTRsPUEBUKSaThsgBcA;
	}

	public unsafe static List<HQJMKfjyTMpCeMjKUmbkunFtJlDj> PcxRaKuzIVnuHUPdgfTRrZhcecFg(bool P_0)
	{
		int num = 0;
		IuSDPpsvldfjgcyHnqOBMjVVpFIlA.xJntIHwMDcSpfyEtrJoJZqCpxrBp(null, ref num, aOYtALpBiXtNUYUxrmqpvmqCyvKG.pbdlJJuDWMAxynFvrfVPNfMMBbuS<pmxSkwdYbaBrHimOZtqRnMDGDpakA>());
		if (num == 0)
		{
			return null;
		}
		pmxSkwdYbaBrHimOZtqRnMDGDpakA[] array = new pmxSkwdYbaBrHimOZtqRnMDGDpakA[num];
		IuSDPpsvldfjgcyHnqOBMjVVpFIlA.xJntIHwMDcSpfyEtrJoJZqCpxrBp(array, ref num, aOYtALpBiXtNUYUxrmqpvmqCyvKG.pbdlJJuDWMAxynFvrfVPNfMMBbuS<pmxSkwdYbaBrHimOZtqRnMDGDpakA>());
		string[] array2 = new string[num];
		int num2 = 0;
		int num3 = 0;
		List<HQJMKfjyTMpCeMjKUmbkunFtJlDj> list = new List<HQJMKfjyTMpCeMjKUmbkunFtJlDj>();
		for (int i = 0; i < num; i++)
		{
			bool flag = false;
			IntPtr lJFUFsjaUqwNXcffOswKpjKcUJpF = array[i].lJFUFsjaUqwNXcffOswKpjKcUJpF;
			int num4 = 0;
			IuSDPpsvldfjgcyHnqOBMjVVpFIlA.RIgsJRzhwmPCrFVzGGlvrztGhKXn(lJFUFsjaUqwNXcffOswKpjKcUJpF, TfwiTYPsIjeEkKIcSjHMOzaojtXBA.DeviceName, IntPtr.Zero, ref num4);
			if (num4 == 0)
			{
				flag = true;
			}
			char* ptr = stackalloc char[num4];
			IuSDPpsvldfjgcyHnqOBMjVVpFIlA.RIgsJRzhwmPCrFVzGGlvrztGhKXn(lJFUFsjaUqwNXcffOswKpjKcUJpF, TfwiTYPsIjeEkKIcSjHMOzaojtXBA.DeviceName, (IntPtr)ptr, ref num4);
			int length = ((num4 > 0) ? (num4 - 1) : 0);
			string text = new string(ptr, 0, length);
			if (text.Length == 0)
			{
				text = string.Empty;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			int num5 = 0;
			for (int j = 0; j < bytes.Length; j++)
			{
				if (bytes[j] != 0)
				{
					num5++;
				}
			}
			if (num5 != bytes.Length)
			{
				if (num5 == 0)
				{
					text = string.Empty;
				}
				else
				{
					byte[] array3 = new byte[num5];
					int num6 = 0;
					for (int k = 0; k < bytes.Length; k++)
					{
						if (bytes[k] != 0)
						{
							array3[num6] = bytes[k];
							num6++;
						}
					}
					text = Encoding.UTF8.GetString(array3);
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				bool flag2 = false;
				for (int l = 0; l < num; l++)
				{
					if (!string.IsNullOrEmpty(array2[l]) && string.Equals(array2[l], text, StringComparison.OrdinalIgnoreCase))
					{
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					continue;
				}
			}
			array2[i] = text;
			int num7 = 0;
			IuSDPpsvldfjgcyHnqOBMjVVpFIlA.RIgsJRzhwmPCrFVzGGlvrztGhKXn(lJFUFsjaUqwNXcffOswKpjKcUJpF, TfwiTYPsIjeEkKIcSjHMOzaojtXBA.DeviceInfo, IntPtr.Zero, ref num7);
			if (num7 == 0)
			{
				if (flag)
				{
					num3++;
				}
				continue;
			}
			byte* ptr2 = stackalloc byte[(int)(uint)num7];
			*(int*)ptr2 = num7;
			if (IuSDPpsvldfjgcyHnqOBMjVVpFIlA.RIgsJRzhwmPCrFVzGGlvrztGhKXn(lJFUFsjaUqwNXcffOswKpjKcUJpF, TfwiTYPsIjeEkKIcSjHMOzaojtXBA.DeviceInfo, (IntPtr)ptr2, ref num7) >= 0)
			{
				try
				{
					ZYAEOvciDtnJrGhOUJNZVXaKpkox zYAEOvciDtnJrGhOUJNZVXaKpkox = *(ZYAEOvciDtnJrGhOUJNZVXaKpkox*)ptr2;
					HQJMKfjyTMpCeMjKUmbkunFtJlDj item = HQJMKfjyTMpCeMjKUmbkunFtJlDj.qcRqSFSnRoLYyGLXIfozMZHQOZML(ref zYAEOvciDtnJrGhOUJNZVXaKpkox, text, lJFUFsjaUqwNXcffOswKpjKcUJpF);
					list.Add(item);
				}
				catch (Exception)
				{
					throw;
				}
				num2++;
			}
		}
		if (P_0 && num2 == 0 && num3 > 0)
		{
			throw new Exception("Possible sandbox detected.")
			{
				Data = { 
				{
					(object)1,
					(object)"sandbox"
				} }
			};
		}
		return list;
	}

	public static void shcDUYXZfrcgqaPEwDCQTvWoXTwr(TwiFkdkTwsNRlqvUWmbZLRhMWJsJ P_0, eHHDCrWQrfZEoaNTeobwagJZfWUeA P_1, XYfYliKgyQqSrVgbybAHHhMqxKBo P_2, IntPtr P_3)
	{
		hqChFnfhefzukKUHmruBlezpnbHo[] array = new hqChFnfhefzukKUHmruBlezpnbHo[1];
		array[0].uuNGjxmdPvmxDegJbENqZbvxonIw = (short)P_0;
		array[0].wdLEfLZxrjtcLdIntcuABrEoICsnA = (short)P_1;
		array[0].ZIvyiOSzgMRCzZiwgApXsRrDjjHn = (int)P_2;
		array[0].cEQDCCcsMFrNAezTsxbcAsxVWpNP = P_3;
		IuSDPpsvldfjgcyHnqOBMjVVpFIlA.McoDrlHYWDOBHkQAbHUBiYYMcwijA(array, 1, aOYtALpBiXtNUYUxrmqpvmqCyvKG.pbdlJJuDWMAxynFvrfVPNfMMBbuS<hqChFnfhefzukKUHmruBlezpnbHo>());
	}

	public static void BJOUPGrOEBFtrRPjMwJPWMtRNRpO(TwiFkdkTwsNRlqvUWmbZLRhMWJsJ P_0, eHHDCrWQrfZEoaNTeobwagJZfWUeA P_1)
	{
		hqChFnfhefzukKUHmruBlezpnbHo[] array = new hqChFnfhefzukKUHmruBlezpnbHo[1];
		array[0].uuNGjxmdPvmxDegJbENqZbvxonIw = (short)P_0;
		array[0].wdLEfLZxrjtcLdIntcuABrEoICsnA = (short)P_1;
		array[0].ZIvyiOSzgMRCzZiwgApXsRrDjjHn = 1;
		array[0].cEQDCCcsMFrNAezTsxbcAsxVWpNP = IntPtr.Zero;
		IuSDPpsvldfjgcyHnqOBMjVVpFIlA.McoDrlHYWDOBHkQAbHUBiYYMcwijA(array, 1, aOYtALpBiXtNUYUxrmqpvmqCyvKG.pbdlJJuDWMAxynFvrfVPNfMMBbuS<hqChFnfhefzukKUHmruBlezpnbHo>());
	}

	internal static void ZrbFhGEbWRbTzVxxQimUntnkwisKA()
	{
		vaMEXWmtmhgkkjmQIaKcHyMbMajS.jbuKnmPOtrWNfuHYgKEIAmicwine = null;
		vaMEXWmtmhgkkjmQIaKcHyMbMajS.iIMOJGJixgmmpvCrCpDcUgUIwvFY = null;
		vaMEXWmtmhgkkjmQIaKcHyMbMajS.IGgDxUtolvrbybgRykaplzGUZIlB = null;
		RSCMNMhSJzIMVMOUahSfVpBvbLuy = IntPtr.Zero;
		sZTTJMDBuidjvmcbbWVBqKTbFsWL = false;
		PTneWUErjbWOjdUUAqatZxhuOegxA = IntPtr.Zero;
		dfBPgqqPnHblTSdXBgYBHznPjkwOA = false;
	}

	public unsafe static void MPlIucqsZdFnJptGrOxdWNCahipi(IntPtr P_0, double P_1)
	{
		if (EaghbIHnDldiplwewfVhLKplXGOW)
		{
			uint num = 0u;
			uint num2 = 0u;
			uint num3 = 8192u;
			int num4 = 0;
			if (IuSDPpsvldfjgcyHnqOBMjVVpFIlA.QrtIIeijKaLqQuiEMWMamfcKifAu(P_0, AAJgrYpUaJPJmbapqPihVtMOwfYF.Input, IntPtr.Zero, ref num4, yesRKLyGMsQYmrMHfwfBUpCHbXEEA) < 0 || num4 == 0)
			{
				return;
			}
			num4 = (int)num3;
			if (IuSDPpsvldfjgcyHnqOBMjVVpFIlA.QrtIIeijKaLqQuiEMWMamfcKifAu(P_0, AAJgrYpUaJPJmbapqPihVtMOwfYF.Input, kVnSJqQSRWojFXHiktszxjFliVXW.Pointer, ref num4, yesRKLyGMsQYmrMHfwfBUpCHbXEEA) < 0)
			{
				return;
			}
			BfrWUppsjgDcoMLolPjFdAkdoqjP* ptr = (BfrWUppsjgDcoMLolPjFdAkdoqjP*)(void*)kVnSJqQSRWojFXHiktszxjFliVXW.Pointer;
			ebiBRLMnChpvZPRPevtsPhHtiwxg(ptr, P_1);
			LidfOSibRqvXbFelzdQaFenMdgBx(ptr, InnRcubJLSsWWNBtDNrbCGLxmFgq, NmCNzIPYMOAtynpfcUxIaEQGLqzR, wrVcoifDOzaFYLglyOhpdveBPerbB, ref num2, ref num);
			if (uOSCzXxSiWjQXbwRgFbineaqwrQmA)
			{
				int num5;
				while ((num5 = hUfdZejvJYlOtHWdillPtEZZfcOAA.jREMCAOOqKFEdiXCIFUaEhjIOxWhB(kVnSJqQSRWojFXHiktszxjFliVXW.Pointer, ref num3, (uint)yesRKLyGMsQYmrMHfwfBUpCHbXEEA)) > 0)
				{
					byte* ptr2 = (byte*)(void*)kVnSJqQSRWojFXHiktszxjFliVXW.Pointer;
					for (int i = 0; i < num5; i++)
					{
						int wSxdShTeVjIhEFmnDeTBxdoyJeBl = ((NXPCBEDXCQFnUVkafhIcnwaFbhfxA*)ptr2)->wSxdShTeVjIhEFmnDeTBxdoyJeBl;
						byte* ptr3 = stackalloc byte[(int)(uint)(yesRKLyGMsQYmrMHfwfBUpCHbXEEA + wSxdShTeVjIhEFmnDeTBxdoyJeBl)];
						EigdSMgqkBzyFeSpQPsmKNepnjhE.nJkgPtxUDYWhinniORZRyzsAyBWl(ptr2, ptr3, 0, 0, yesRKLyGMsQYmrMHfwfBUpCHbXEEA);
						EigdSMgqkBzyFeSpQPsmKNepnjhE.nJkgPtxUDYWhinniORZRyzsAyBWl(ptr2, ptr3, yesRKLyGMsQYmrMHfwfBUpCHbXEEA + 8, yesRKLyGMsQYmrMHfwfBUpCHbXEEA, wSxdShTeVjIhEFmnDeTBxdoyJeBl);
						ptr = (BfrWUppsjgDcoMLolPjFdAkdoqjP*)ptr3;
						ebiBRLMnChpvZPRPevtsPhHtiwxg(ptr, P_1);
						LidfOSibRqvXbFelzdQaFenMdgBx(ptr, InnRcubJLSsWWNBtDNrbCGLxmFgq, NmCNzIPYMOAtynpfcUxIaEQGLqzR, wrVcoifDOzaFYLglyOhpdveBPerbB, ref num2, ref num);
						ptr2 = (byte*)glKwXRHbpLdMBNjLJycCmguFgbzW.WJrHzpneTjWCVXaeAekbDGOUdbseb((BfrWUppsjgDcoMLolPjFdAkdoqjP*)ptr2);
					}
				}
			}
			else
			{
				int num5;
				while ((num5 = hUfdZejvJYlOtHWdillPtEZZfcOAA.jREMCAOOqKFEdiXCIFUaEhjIOxWhB(kVnSJqQSRWojFXHiktszxjFliVXW.Pointer, ref num3, (uint)yesRKLyGMsQYmrMHfwfBUpCHbXEEA)) > 0)
				{
					ptr = (BfrWUppsjgDcoMLolPjFdAkdoqjP*)(void*)kVnSJqQSRWojFXHiktszxjFliVXW.Pointer;
					for (int j = 0; j < num5; j++)
					{
						ebiBRLMnChpvZPRPevtsPhHtiwxg(ptr, P_1);
						LidfOSibRqvXbFelzdQaFenMdgBx(ptr, InnRcubJLSsWWNBtDNrbCGLxmFgq, NmCNzIPYMOAtynpfcUxIaEQGLqzR, wrVcoifDOzaFYLglyOhpdveBPerbB, ref num2, ref num);
						ptr = glKwXRHbpLdMBNjLJycCmguFgbzW.DAGLwcvaBPvsiDDKcAKfCcpzGlDl(ptr);
					}
				}
			}
			ATYbFOauvfhdknIMIHuGuhAbMRqL(InnRcubJLSsWWNBtDNrbCGLxmFgq, NmCNzIPYMOAtynpfcUxIaEQGLqzR, wrVcoifDOzaFYLglyOhpdveBPerbB, ref num2, ref num);
		}
		else
		{
			int num6 = 0;
			IuSDPpsvldfjgcyHnqOBMjVVpFIlA.QrtIIeijKaLqQuiEMWMamfcKifAu(P_0, AAJgrYpUaJPJmbapqPihVtMOwfYF.Input, IntPtr.Zero, ref num6, yesRKLyGMsQYmrMHfwfBUpCHbXEEA);
			if (num6 != 0)
			{
				byte* ptr4 = stackalloc byte[(int)(uint)num6];
				IuSDPpsvldfjgcyHnqOBMjVVpFIlA.QrtIIeijKaLqQuiEMWMamfcKifAu(P_0, AAJgrYpUaJPJmbapqPihVtMOwfYF.Input, (IntPtr)ptr4, ref num6, yesRKLyGMsQYmrMHfwfBUpCHbXEEA);
				ebiBRLMnChpvZPRPevtsPhHtiwxg((BfrWUppsjgDcoMLolPjFdAkdoqjP*)ptr4, P_1);
			}
		}
	}

	private unsafe static void LidfOSibRqvXbFelzdQaFenMdgBx(BfrWUppsjgDcoMLolPjFdAkdoqjP* P_0, byte[] P_1, uint[] P_2, uint[] P_3, ref uint P_4, ref uint P_5)
	{
		if (!dfzdcxFeWOCeEYqaoNVnlMeNAJpDA(P_0, P_1, P_2, P_3, ref P_4, ref P_5))
		{
			ATYbFOauvfhdknIMIHuGuhAbMRqL(P_1, P_2, P_3, ref P_4, ref P_5);
			dfzdcxFeWOCeEYqaoNVnlMeNAJpDA(P_0, P_1, P_2, P_3, ref P_4, ref P_5);
		}
	}

	private unsafe static bool dfzdcxFeWOCeEYqaoNVnlMeNAJpDA(BfrWUppsjgDcoMLolPjFdAkdoqjP* P_0, byte[] P_1, uint[] P_2, uint[] P_3, ref uint P_4, ref uint P_5)
	{
		NXPCBEDXCQFnUVkafhIcnwaFbhfxA* ptr = &P_0->UrAsNmnqXXrNEmAIJfPTgKWkLplib;
		uint num = (uint)(yesRKLyGMsQYmrMHfwfBUpCHbXEEA + ptr->wSxdShTeVjIhEFmnDeTBxdoyJeBl);
		if (P_4 + num > P_1.Length)
		{
			return false;
		}
		if (P_5 == P_2.Length)
		{
			return false;
		}
		Marshal.Copy((IntPtr)P_0, P_1, (int)P_4, yesRKLyGMsQYmrMHfwfBUpCHbXEEA + ptr->wSxdShTeVjIhEFmnDeTBxdoyJeBl);
		P_2[P_5] = P_4;
		P_3[P_5] = (uint)(P_4 + yesRKLyGMsQYmrMHfwfBUpCHbXEEA);
		P_5++;
		P_4 += num;
		return true;
	}

	private unsafe static void ATYbFOauvfhdknIMIHuGuhAbMRqL(byte[] P_0, uint[] P_1, uint[] P_2, ref uint P_3, ref uint P_4)
	{
		if (eVSMwNDYHANQmeEWyygsOgrkrOkV == null || P_4 == 0 || P_3 == 0)
		{
			P_3 = 0u;
			P_4 = 0u;
			return;
		}
		try
		{
			fixed (byte* ptr = P_0)
			{
				fixed (uint* ptr2 = P_1)
				{
					fixed (uint* ptr3 = P_2)
					{
						eVSMwNDYHANQmeEWyygsOgrkrOkV((IntPtr)ptr2, (IntPtr)ptr3, P_4, (IntPtr)ptr, P_3);
					}
				}
			}
		}
		catch (Exception msg)
		{
			Logger.LogError(msg, requiredThreadSafety: true);
		}
		P_3 = 0u;
		P_4 = 0u;
	}

	private unsafe static void ebiBRLMnChpvZPRPevtsPhHtiwxg(BfrWUppsjgDcoMLolPjFdAkdoqjP* P_0, double P_1)
	{
		switch (P_0->UrAsNmnqXXrNEmAIJfPTgKWkLplib.lhIrpCzAlkfgVHpeLkMrWybTDiDQA)
		{
		case thGpvMJraEkCGcLLHOTxuArPRrdh.HumanInputDevice:
			if (vaMEXWmtmhgkkjmQIaKcHyMbMajS.IGgDxUtolvrbybgRykaplzGUZIlB != null)
			{
				McirdyGKrseZIDeGHFqVBkIAUEeUB arg = new McirdyGKrseZIDeGHFqVBkIAUEeUB(ref *P_0, skVnnfTIbZXGQnqqnOsKYrVFhShV.MDAyfkwXbhVRHhiQsCStTPDFCoUO);
				if (arg.JSayXFTNziEjxHctRbOJQBhdcOwp)
				{
					vaMEXWmtmhgkkjmQIaKcHyMbMajS.IGgDxUtolvrbybgRykaplzGUZIlB(arg, P_1);
				}
			}
			break;
		case thGpvMJraEkCGcLLHOTxuArPRrdh.Keyboard:
			if (vaMEXWmtmhgkkjmQIaKcHyMbMajS.jbuKnmPOtrWNfuHYgKEIAmicwine != null)
			{
				vaMEXWmtmhgkkjmQIaKcHyMbMajS.jbuKnmPOtrWNfuHYgKEIAmicwine(new VbuwuiLmwxfrQAPwNZAKAcROJgVgb(ref *P_0), P_1);
			}
			break;
		case thGpvMJraEkCGcLLHOTxuArPRrdh.Mouse:
			if (vaMEXWmtmhgkkjmQIaKcHyMbMajS.iIMOJGJixgmmpvCrCpDcUgUIwvFY != null)
			{
				vaMEXWmtmhgkkjmQIaKcHyMbMajS.iIMOJGJixgmmpvCrCpDcUgUIwvFY(new qsOyzcrZVgqWvqYbLoowsSqEKWkk(ref *P_0), P_1);
			}
			break;
		}
	}

	private static void SSgEGKbUPAFkVsDmGJTtsDSczaHU(IntPtr P_0, IntPtr P_1)
	{
		switch (P_0.ToInt32())
		{
		case 1:
			if (vaMEXWmtmhgkkjmQIaKcHyMbMajS.GiHRsoeRcJMqWIxwkngZkFkTcMZo != null)
			{
				vaMEXWmtmhgkkjmQIaKcHyMbMajS.GiHRsoeRcJMqWIxwkngZkFkTcMZo(P_1);
			}
			break;
		case 2:
			if (vaMEXWmtmhgkkjmQIaKcHyMbMajS.JfHzQfUlnPjlIHhEBRUpfBHGlVeQ != null)
			{
				vaMEXWmtmhgkkjmQIaKcHyMbMajS.JfHzQfUlnPjlIHhEBRUpfBHGlVeQ();
			}
			break;
		}
	}

	[MonoPInvokeCallback(typeof(pNjEQKwNMacTPSVRvkuPNTsvHycD))]
	private static IntPtr tYEbojeeeJlnHbWfdBsrnOIgnOEcd(IntPtr P_0, uint P_1, IntPtr P_2, IntPtr P_3)
	{
		switch (P_1)
		{
		case 255u:
			MPlIucqsZdFnJptGrOxdWNCahipi(P_3, ReInput.realTime);
			if (sZTTJMDBuidjvmcbbWVBqKTbFsWL && !EaghbIHnDldiplwewfVhLKplXGOW)
			{
				xWXDNidElrjJlhamELWLJWCrBHAeB(P_0, P_1, P_2, P_3);
			}
			break;
		case 254u:
			SSgEGKbUPAFkVsDmGJTtsDSczaHU(P_2, P_3);
			break;
		}
		return IntPtr.Zero;
	}

	private static void xWXDNidElrjJlhamELWLJWCrBHAeB(IntPtr P_0, uint P_1, IntPtr P_2, IntPtr P_3)
	{
		if (TGhtjJFdIheEFBuvrEuzmaodtCjb.OiaxAhOcCrpxoTRQQDMjFSbySsem(RSCMNMhSJzIMVMOUahSfVpBvbLuy))
		{
			if (dfBPgqqPnHblTSdXBgYBHznPjkwOA)
			{
				PTneWUErjbWOjdUUAqatZxhuOegxA = TGhtjJFdIheEFBuvrEuzmaodtCjb.LvjxsRoEpaQBxmxMmNxbVBlGLwBq(RSCMNMhSJzIMVMOUahSfVpBvbLuy, TGhtjJFdIheEFBuvrEuzmaodtCjb.VsotYxPVhrNWFRZhLebcZIOCPJdt.WndProc);
				dfBPgqqPnHblTSdXBgYBHznPjkwOA = false;
			}
			if (PTneWUErjbWOjdUUAqatZxhuOegxA != IntPtr.Zero)
			{
				TGhtjJFdIheEFBuvrEuzmaodtCjb.ACHfxPraKCfXYMZVbcEyUdfUsKZX(PTneWUErjbWOjdUUAqatZxhuOegxA, RSCMNMhSJzIMVMOUahSfVpBvbLuy, (int)P_1, P_2, P_3);
			}
		}
	}
}
