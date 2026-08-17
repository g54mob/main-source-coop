using System;
using System.Runtime.InteropServices;

internal static class NrQfQnkqCzbdhahnYmfVCnDUJLLm
{
	public unsafe static int TtXCLBIgIxbCbqldyxqiMNzepooM(oavWbmfgJapTKculwWsDMRdHdvld[] P_0, ref int P_1, int P_2)
	{
		int result;
		fixed (oavWbmfgJapTKculwWsDMRdHdvld* ptr = P_0)
		{
			void* ptr2 = ptr;
			fixed (int* ptr3 = &P_1)
			{
				void* ptr4 = ptr3;
				result = EgLZFKyXCLvbUhSzNGyIHcjavNsj(ptr2, ptr4, P_2);
			}
		}
		return result;
	}

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetRawInputDeviceList")]
	private unsafe static extern int EgLZFKyXCLvbUhSzNGyIHcjavNsj(void* P_0, void* P_1, int P_2);

	public unsafe static int PncDRzJXHnGSyNcLARdWRYDGQiuBA(IntPtr P_0, OyazSCDHmjCcxWOxttgKIaujvoGj P_1, IntPtr P_2, ref int P_3)
	{
		int result;
		fixed (int* ptr = &P_3)
		{
			void* ptr2 = ptr;
			result = lswFwSWFmfkbSrYROcphmpulnLDn((void*)P_0, (int)P_1, (void*)P_2, ptr2);
		}
		return result;
	}

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetRawInputDeviceInfoW")]
	private unsafe static extern int lswFwSWFmfkbSrYROcphmpulnLDn(void* P_0, int P_1, void* P_2, void* P_3);

	public unsafe static IyNzWgENqThPgJqwJUbyZoBSDLqz GtKCPkKfHmOfnBWOXXAxDHKHbkFmb(aNUlwnfSpteWhCxjVAlPAAjcddIi[] P_0, int P_1, int P_2)
	{
		IyNzWgENqThPgJqwJUbyZoBSDLqz result;
		fixed (aNUlwnfSpteWhCxjVAlPAAjcddIi* ptr = P_0)
		{
			void* ptr2 = ptr;
			result = DhnemEcVEXEhdhyQgwDidOCiFNtLc(ptr2, P_1, P_2);
		}
		return result;
	}

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "RegisterRawInputDevices")]
	private unsafe static extern IyNzWgENqThPgJqwJUbyZoBSDLqz DhnemEcVEXEhdhyQgwDidOCiFNtLc(void* P_0, int P_1, int P_2);

	public unsafe static int YZGpyKagokbNyTBsNetlxUQgNKAd(IntPtr P_0, JCRwMEbRHHiXvfpUFofbmixTImNhA P_1, IntPtr P_2, ref int P_3, int P_4)
	{
		int result;
		fixed (int* ptr = &P_3)
		{
			void* ptr2 = ptr;
			result = zlPeUqFEUQFhlnTZCuBdnjVkyTtFA((void*)P_0, (int)P_1, (void*)P_2, ptr2, P_4);
		}
		return result;
	}

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetRawInputData")]
	private unsafe static extern int zlPeUqFEUQFhlnTZCuBdnjVkyTtFA(void* P_0, int P_1, void* P_2, void* P_3, int P_4);
}
