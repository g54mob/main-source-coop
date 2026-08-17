using System;
using System.Runtime.InteropServices;
using System.Security;

internal static class FOWbSUBUopNTWcFCvjeVoFBwLHmk
{
	public unsafe static int yInTsBLKMwHEMpLpNSDZXgTaFIbCA(int P_0, int P_1, out AesEIIaObsChICQBoLLfnjVNyLHtA P_2)
	{
		if (nidrBTyyTsEkugfveLPskTbfMKqg.FHrERVHlRvkinucpdkdkvVfgWxzi >= nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_4)
		{
			P_2 = default(AesEIIaObsChICQBoLLfnjVNyLHtA);
			return 0;
		}
		P_2 = default(AesEIIaObsChICQBoLLfnjVNyLHtA);
		int result;
		fixed (AesEIIaObsChICQBoLLfnjVNyLHtA* ptr = &P_2)
		{
			void* ptr2 = ptr;
			result = vDxWRXNBLHgnWDGyHcGauJzxrgXJA(P_0, P_1, ptr2);
		}
		return result;
	}

	private unsafe static int vDxWRXNBLHgnWDGyHcGauJzxrgXJA(int P_0, int P_1, void* P_2)
	{
		return nidrBTyyTsEkugfveLPskTbfMKqg.FHrERVHlRvkinucpdkdkvVfgWxzi switch
		{
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_3 => MmCIaTeCLHPAxiShMVUsiDRlmEIV(P_0, P_1, P_2), 
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_2 => ysRXmfeaGZZZwLrpwhrAzeQLXoiU(P_0, P_1, P_2), 
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_1 => PVUDKKcSAwApAcvRoBjfKYJJmLNsA(P_0, P_1, P_2), 
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_9_1_0 => gwVCNekteVbRikgYiYeUMuuvoeSyA(P_0, P_1, P_2), 
			_ => 0, 
		};
	}

	[DllImport("xinput9_1_0.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetKeystroke")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int gwVCNekteVbRikgYiYeUMuuvoeSyA(int P_0, int P_1, void* P_2);

	[DllImport("xinput1_1.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetKeystroke")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int PVUDKKcSAwApAcvRoBjfKYJJmLNsA(int P_0, int P_1, void* P_2);

	[DllImport("xinput1_2.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetKeystroke")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int ysRXmfeaGZZZwLrpwhrAzeQLXoiU(int P_0, int P_1, void* P_2);

	[DllImport("xinput1_3.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetKeystroke")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int MmCIaTeCLHPAxiShMVUsiDRlmEIV(int P_0, int P_1, void* P_2);

	public unsafe static int TycOhdOLJqQfcMAXfNYEVMrsdomi(int P_0, nSQKzuFgkuUgePhnvOTkeuGuQDqo P_1)
	{
		return DYAlgMmdzrDZThEZnBuAgQCIkPJC(P_0, &P_1);
	}

	private unsafe static int DYAlgMmdzrDZThEZnBuAgQCIkPJC(int P_0, void* P_1)
	{
		return nidrBTyyTsEkugfveLPskTbfMKqg.FHrERVHlRvkinucpdkdkvVfgWxzi switch
		{
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_4 => dNsBXMabWAhDYGcxfpzZAJYAlYtKB(P_0, P_1), 
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_3 => IzcgwmoDUscMyggxAkcdEVCNfZPwA(P_0, P_1), 
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_2 => NckakVwyVQMCQkGHblvdIikgPzbN(P_0, P_1), 
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_1 => NmZJSOQoFfPjArinhZtriaOidpRW(P_0, P_1), 
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_9_1_0 => QaklHZYYSrraEvefZDEEAOVPqfLd(P_0, P_1), 
			_ => 0, 
		};
	}

	[DllImport("xinput9_1_0.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputSetState")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int QaklHZYYSrraEvefZDEEAOVPqfLd(int P_0, void* P_1);

	[DllImport("xinput1_1.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputSetState")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int NmZJSOQoFfPjArinhZtriaOidpRW(int P_0, void* P_1);

	[DllImport("xinput1_2.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputSetState")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int NckakVwyVQMCQkGHblvdIikgPzbN(int P_0, void* P_1);

	[DllImport("xinput1_3.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputSetState")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int IzcgwmoDUscMyggxAkcdEVCNfZPwA(int P_0, void* P_1);

	[DllImport("xinput1_4.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputSetState")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int dNsBXMabWAhDYGcxfpzZAJYAlYtKB(int P_0, void* P_1);

	public unsafe static int jvlCEKVAUSccqcgxsXwdjZzlopLJ(int P_0, out Guid P_1, out Guid P_2)
	{
		P_1 = default(Guid);
		P_2 = default(Guid);
		int result;
		fixed (Guid* ptr = &P_1)
		{
			void* ptr2 = ptr;
			fixed (Guid* ptr3 = &P_2)
			{
				void* ptr4 = ptr3;
				result = ctxYptuenVeHlJoCBWqzGKdNmdNab(P_0, ptr2, ptr4);
			}
		}
		return result;
	}

	private unsafe static int ctxYptuenVeHlJoCBWqzGKdNmdNab(int P_0, void* P_1, void* P_2)
	{
		return nidrBTyyTsEkugfveLPskTbfMKqg.FHrERVHlRvkinucpdkdkvVfgWxzi switch
		{
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_3 => VEnkOLjCywBJbIRCOKNxNBRUJexhA(P_0, P_1, P_2), 
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_2 => obTJpqCVyjaQOEonqiSfgHsTUGUIA(P_0, P_1, P_2), 
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_1 => radybgchpiTuTQvuRLObMzgpplYw(P_0, P_1, P_2), 
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_9_1_0 => GJgogMBieBbgKgqspmyukItmPybl(P_0, P_1, P_2), 
			_ => 0, 
		};
	}

	[DllImport("xinput9_1_0.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetDSoundAudioDeviceGuids")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int GJgogMBieBbgKgqspmyukItmPybl(int P_0, void* P_1, void* P_2);

	[DllImport("xinput1_1.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetDSoundAudioDeviceGuids")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int radybgchpiTuTQvuRLObMzgpplYw(int P_0, void* P_1, void* P_2);

	[DllImport("xinput1_2.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetDSoundAudioDeviceGuids")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int obTJpqCVyjaQOEonqiSfgHsTUGUIA(int P_0, void* P_1, void* P_2);

	[DllImport("xinput1_3.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetDSoundAudioDeviceGuids")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int VEnkOLjCywBJbIRCOKNxNBRUJexhA(int P_0, void* P_1, void* P_2);

	[SuppressUnmanagedCodeSecurity]
	public unsafe static int IXKvTUGSrhoEVzocTiwiTtZHEvao(int P_0, out aomOZkICjBSElBGTrxaITNTycEuQ P_1)
	{
		P_1 = default(aomOZkICjBSElBGTrxaITNTycEuQ);
		int result;
		fixed (aomOZkICjBSElBGTrxaITNTycEuQ* ptr = &P_1)
		{
			void* ptr2 = ptr;
			result = XnTsXRQWnqFtlheNCGAYkIZUIyTn(P_0, ptr2);
		}
		return result;
	}

	private unsafe static int XnTsXRQWnqFtlheNCGAYkIZUIyTn(int P_0, void* P_1)
	{
		if (nidrBTyyTsEkugfveLPskTbfMKqg.ZfujBxINdJzqcgKNcFdTZfcMhRbh && nidrBTyyTsEkugfveLPskTbfMKqg.KUlSXjWPKVdvveUSRbmtknRzERW != null)
		{
			return nidrBTyyTsEkugfveLPskTbfMKqg.KUlSXjWPKVdvveUSRbmtknRzERW(P_0, P_1);
		}
		return nidrBTyyTsEkugfveLPskTbfMKqg.FHrERVHlRvkinucpdkdkvVfgWxzi switch
		{
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_4 => kegDZRDFpFTgWUyhEqSIzdeTcyEfA(P_0, P_1), 
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_3 => FMHecgXWUUigfuOHDEypDxvCaNQk(P_0, P_1), 
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_2 => EpvHxzlfkFfNnGmtjFeREpwOqNYFb(P_0, P_1), 
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_1 => TbTdQiIaqsGlFzNBzIWUFCPJqfjk(P_0, P_1), 
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_9_1_0 => POzkIfyuxhmYQYSrtdzPHlYiDgOH(P_0, P_1), 
			_ => 0, 
		};
	}

	[DllImport("xinput9_1_0.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetState")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int POzkIfyuxhmYQYSrtdzPHlYiDgOH(int P_0, void* P_1);

	[DllImport("xinput1_1.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetState")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int TbTdQiIaqsGlFzNBzIWUFCPJqfjk(int P_0, void* P_1);

	[DllImport("xinput1_2.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetState")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int EpvHxzlfkFfNnGmtjFeREpwOqNYFb(int P_0, void* P_1);

	[DllImport("xinput1_3.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetState")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int FMHecgXWUUigfuOHDEypDxvCaNQk(int P_0, void* P_1);

	[DllImport("xinput1_4.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetState")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int kegDZRDFpFTgWUyhEqSIzdeTcyEfA(int P_0, void* P_1);

	public unsafe static int WFvmHwRjJewKcMwwipvNsGgJinVh(int P_0, CBcLxjxrGQlvGCLKnksXbuQbLsqS P_1, out vkLQdmECkuLRduGdNYgGdjIRmYkU P_2)
	{
		P_2 = default(vkLQdmECkuLRduGdNYgGdjIRmYkU);
		int result;
		fixed (vkLQdmECkuLRduGdNYgGdjIRmYkU* ptr = &P_2)
		{
			void* ptr2 = ptr;
			result = XbgIuMfKsYHAxqVBiJeEJVhOIMYiA(P_0, (int)P_1, ptr2);
		}
		return result;
	}

	private unsafe static int XbgIuMfKsYHAxqVBiJeEJVhOIMYiA(int P_0, int P_1, void* P_2)
	{
		return nidrBTyyTsEkugfveLPskTbfMKqg.FHrERVHlRvkinucpdkdkvVfgWxzi switch
		{
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_4 => SodSVhcEAKbqOtSbcIEkeRYYCnir(P_0, P_1, P_2), 
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_3 => rKwGwLSbvLwlMKiHCazguRiZZkAV(P_0, P_1, P_2), 
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_2 => ZiCbBlBLKDOzpsWIjPdWvSBYrIPJA(P_0, P_1, P_2), 
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_1 => pBESTCLaNFBKskKzKpqNIqtMeoBYA(P_0, P_1, P_2), 
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_9_1_0 => XTzGjsfxUJeDlNGkULNOpmuNuwUGA(P_0, P_1, P_2), 
			_ => 0, 
		};
	}

	[DllImport("xinput9_1_0.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetCapabilities")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int XTzGjsfxUJeDlNGkULNOpmuNuwUGA(int P_0, int P_1, void* P_2);

	[DllImport("xinput1_1.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetCapabilities")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int pBESTCLaNFBKskKzKpqNIqtMeoBYA(int P_0, int P_1, void* P_2);

	[DllImport("xinput1_2.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetCapabilities")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int ZiCbBlBLKDOzpsWIjPdWvSBYrIPJA(int P_0, int P_1, void* P_2);

	[DllImport("xinput1_3.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetCapabilities")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int rKwGwLSbvLwlMKiHCazguRiZZkAV(int P_0, int P_1, void* P_2);

	[DllImport("xinput1_4.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetCapabilities")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int SodSVhcEAKbqOtSbcIEkeRYYCnir(int P_0, int P_1, void* P_2);

	public unsafe static int IgtHzTSWwPwkLdEoNHtYBhXsembd(int P_0, fVQbsjEznWdNgvqKzYiQegvznIDVA P_1, out kkCYLDBZFCSLtthvRWjfqsUbCjLk P_2)
	{
		P_2 = default(kkCYLDBZFCSLtthvRWjfqsUbCjLk);
		int result;
		fixed (kkCYLDBZFCSLtthvRWjfqsUbCjLk* ptr = &P_2)
		{
			void* ptr2 = ptr;
			result = KtyARphvNQMZyxQDoVuyzvizlmfX(P_0, (int)P_1, ptr2);
		}
		return result;
	}

	private unsafe static int KtyARphvNQMZyxQDoVuyzvizlmfX(int P_0, int P_1, void* P_2)
	{
		return nidrBTyyTsEkugfveLPskTbfMKqg.FHrERVHlRvkinucpdkdkvVfgWxzi switch
		{
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_3 => ftVOhVUSGEsEKfHRjNTRMKOBhdgf(P_0, P_1, P_2), 
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_2 => rMhlbwfhmTgZKbwULxciAWoqMukq(P_0, P_1, P_2), 
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_1 => WHhkspKDYdxTgOagDOnwaaGPYnDJ(P_0, P_1, P_2), 
			nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_9_1_0 => BdOpGYNIKyThowxjOLnQJWQqpJpV(P_0, P_1, P_2), 
			_ => 0, 
		};
	}

	[DllImport("xinput9_1_0.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetBatteryInformation")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int BdOpGYNIKyThowxjOLnQJWQqpJpV(int P_0, int P_1, void* P_2);

	[DllImport("xinput1_1.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetBatteryInformation")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int WHhkspKDYdxTgOagDOnwaaGPYnDJ(int P_0, int P_1, void* P_2);

	[DllImport("xinput1_2.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetBatteryInformation")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int rMhlbwfhmTgZKbwULxciAWoqMukq(int P_0, int P_1, void* P_2);

	[DllImport("xinput1_3.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputGetBatteryInformation")]
	[SuppressUnmanagedCodeSecurity]
	private unsafe static extern int ftVOhVUSGEsEKfHRjNTRMKOBhdgf(int P_0, int P_1, void* P_2);

	public static void BSChNLHKXHWoNUMixKtnOdyZjbtA(DBRasKQfdBHalDCRoBBuaVANJUde P_0)
	{
		dArGDAkBnjNtrzznmMLpOpjzvuDG(P_0);
	}

	private static void dArGDAkBnjNtrzznmMLpOpjzvuDG(DBRasKQfdBHalDCRoBBuaVANJUde P_0)
	{
		switch (nidrBTyyTsEkugfveLPskTbfMKqg.FHrERVHlRvkinucpdkdkvVfgWxzi)
		{
		case nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_3:
			vtWWTYUIiHRPyUOoNGIfHEJPrxgPA(P_0);
			break;
		case nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_2:
			EENhEAeqOTVqodvpfcVWcoyNjuggb(P_0);
			break;
		case nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_1:
			hTkHfNnpvshZvvWkPXRWXhhqvONi(P_0);
			break;
		case nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_9_1_0:
			jYOXavXsQuhmyWFwSKjeKaOIsTzf(P_0);
			break;
		}
	}

	[DllImport("xinput9_1_0.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputEnable")]
	[SuppressUnmanagedCodeSecurity]
	private static extern void jYOXavXsQuhmyWFwSKjeKaOIsTzf(DBRasKQfdBHalDCRoBBuaVANJUde P_0);

	[DllImport("xinput1_1.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputEnable")]
	[SuppressUnmanagedCodeSecurity]
	private static extern void hTkHfNnpvshZvvWkPXRWXhhqvONi(DBRasKQfdBHalDCRoBBuaVANJUde P_0);

	[DllImport("xinput1_2.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputEnable")]
	[SuppressUnmanagedCodeSecurity]
	private static extern void EENhEAeqOTVqodvpfcVWcoyNjuggb(DBRasKQfdBHalDCRoBBuaVANJUde P_0);

	[DllImport("xinput1_3.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "XInputEnable")]
	[SuppressUnmanagedCodeSecurity]
	private static extern void vtWWTYUIiHRPyUOoNGIfHEJPrxgPA(DBRasKQfdBHalDCRoBBuaVANJUde P_0);
}
