using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
internal struct MfzatynuFTZcaumUqgpvALYfiEpbb
{
	[FieldOffset(0)]
	private int SUtqYQgvkgkvjjOKxGybHWhKTPHjA;

	[FieldOffset(0)]
	private long BnIAaWAgHoKhxxBPPxNicOQYlFuCb;

	[FieldOffset(0)]
	private IntPtr iaBBRokDVcipiOqeUxCsnqONhzRLA;

	private static readonly bool fCRsBzRKNQUIAqRKbGuzamLePKcbA;

	public static readonly int QQQAKQvZqrHpvcOaaZCyIvgHKHGM;

	static MfzatynuFTZcaumUqgpvALYfiEpbb()
	{
		QQQAKQvZqrHpvcOaaZCyIvgHKHGM = IntPtr.Size;
		fCRsBzRKNQUIAqRKbGuzamLePKcbA = QQQAKQvZqrHpvcOaaZCyIvgHKHGM == 8;
	}

	public static MfzatynuFTZcaumUqgpvALYfiEpbb nsVSFpYykHpmEhuOWHBXrLenwrTS(byte[] P_0, int P_1)
	{
		MfzatynuFTZcaumUqgpvALYfiEpbb result = default(MfzatynuFTZcaumUqgpvALYfiEpbb);
		if (fCRsBzRKNQUIAqRKbGuzamLePKcbA)
		{
			result.BnIAaWAgHoKhxxBPPxNicOQYlFuCb = BitConverter.ToInt64(P_0, P_1);
			result.iaBBRokDVcipiOqeUxCsnqONhzRLA = new IntPtr(result.BnIAaWAgHoKhxxBPPxNicOQYlFuCb);
		}
		else
		{
			result.SUtqYQgvkgkvjjOKxGybHWhKTPHjA = BitConverter.ToInt32(P_0, P_1);
			result.iaBBRokDVcipiOqeUxCsnqONhzRLA = new IntPtr(result.SUtqYQgvkgkvjjOKxGybHWhKTPHjA);
		}
		return result;
	}

	[SpecialName]
	public static MfzatynuFTZcaumUqgpvALYfiEpbb yXqnlWRVYEFdDzolmhhrejoSQllY(IntPtr P_0)
	{
		MfzatynuFTZcaumUqgpvALYfiEpbb result = new MfzatynuFTZcaumUqgpvALYfiEpbb
		{
			iaBBRokDVcipiOqeUxCsnqONhzRLA = P_0
		};
		if (fCRsBzRKNQUIAqRKbGuzamLePKcbA)
		{
			result.BnIAaWAgHoKhxxBPPxNicOQYlFuCb = P_0.ToInt64();
		}
		else
		{
			result.SUtqYQgvkgkvjjOKxGybHWhKTPHjA = P_0.ToInt32();
		}
		return result;
	}

	[SpecialName]
	public static IntPtr gUPJhcGKfXWpuxczBwNYekvTSsyb(MfzatynuFTZcaumUqgpvALYfiEpbb P_0)
	{
		return P_0.iaBBRokDVcipiOqeUxCsnqONhzRLA;
	}

	public string SdciNuERyslsiYHDVbaVOFSwxkvQ()
	{
		if (fCRsBzRKNQUIAqRKbGuzamLePKcbA)
		{
			return BnIAaWAgHoKhxxBPPxNicOQYlFuCb.ToString();
		}
		return SUtqYQgvkgkvjjOKxGybHWhKTPHjA.ToString();
	}
}
