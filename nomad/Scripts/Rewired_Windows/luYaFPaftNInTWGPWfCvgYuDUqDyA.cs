using System;
using System.Runtime.InteropServices;
using System.Text;

internal static class luYaFPaftNInTWGPWfCvgYuDUqDyA
{
	[DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "memcpy")]
	private unsafe static extern void* cMIBmiabrrJmgIiUIuHQEcUOdIcBb(void* P_0, void* P_1, UIntPtr P_2);

	private unsafe static void* xyNwdRqSAGGEuniYBqlzNIgKgZkv(void* P_0, void* P_1, int P_2)
	{
		return cMIBmiabrrJmgIiUIuHQEcUOdIcBb(P_0, P_1, new UIntPtr((uint)P_2));
	}

	public unsafe static void JJGgrpAvfYRUJflNOROgzAByeTGgb(IntPtr P_0, IntPtr P_1, int P_2)
	{
		xyNwdRqSAGGEuniYBqlzNIgKgZkv((void*)P_0, (void*)P_1, P_2);
	}

	public static int glwGxVPzunhdOUxGIRKtVKPYTQvO<_0001>() where _0001 : struct
	{
		return Marshal.SizeOf(typeof(_0001));
	}

	public static Guid ibLCaFPZPldpHvHDpPZoCKbwMDSe(Type P_0)
	{
		return P_0.GUID;
	}

	public unsafe static string wZrBMjjiFsbbUUJbIaFasMIMAoju(IntPtr P_0, int P_1)
	{
		char* ptr = (char*)(void*)P_0;
		for (int i = 0; i < P_1; i++)
		{
			if (*(ptr++) == '\0')
			{
				return new string((char*)(void*)P_0);
			}
		}
		return new string((char*)(void*)P_0, 0, P_1);
	}

	public static string HnYFyVXKcwLkAIfmaeKpbkTtNkpZ<_0001>(string P_0, _0001[] P_1)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (P_1 != null)
		{
			for (int i = 0; i < P_1.Length; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(P_0);
				}
				stringBuilder.Append(P_1[i]);
			}
		}
		return stringBuilder.ToString();
	}
}
