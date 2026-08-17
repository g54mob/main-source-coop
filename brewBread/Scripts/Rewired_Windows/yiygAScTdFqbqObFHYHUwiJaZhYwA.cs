using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
internal class yiygAScTdFqbqObFHYHUwiJaZhYwA
{
	public IntPtr qhEAlcCfypKkEIlVORqfPYQDtvaj;

	public yiygAScTdFqbqObFHYHUwiJaZhYwA(IntPtr P_0)
	{
		qhEAlcCfypKkEIlVORqfPYQDtvaj = P_0;
	}

	public unsafe yiygAScTdFqbqObFHYHUwiJaZhYwA(void* P_0)
	{
		qhEAlcCfypKkEIlVORqfPYQDtvaj = new IntPtr(P_0);
	}

	[SpecialName]
	public static IntPtr WEpFdhrCknulBLmFExBPmfoKIqww(yiygAScTdFqbqObFHYHUwiJaZhYwA P_0)
	{
		return P_0.qhEAlcCfypKkEIlVORqfPYQDtvaj;
	}

	[SpecialName]
	public static yiygAScTdFqbqObFHYHUwiJaZhYwA zsNtDbJRpaGeYqGorVKjcaHZirNu(IntPtr P_0)
	{
		return new yiygAScTdFqbqObFHYHUwiJaZhYwA(P_0);
	}

	[SpecialName]
	public unsafe static void* zsNtDbJRpaGeYqGorVKjcaHZirNu(yiygAScTdFqbqObFHYHUwiJaZhYwA P_0)
	{
		return (void*)P_0.qhEAlcCfypKkEIlVORqfPYQDtvaj;
	}

	[SpecialName]
	public unsafe static yiygAScTdFqbqObFHYHUwiJaZhYwA WEpFdhrCknulBLmFExBPmfoKIqww(void* P_0)
	{
		return new yiygAScTdFqbqObFHYHUwiJaZhYwA(P_0);
	}

	public virtual string GFrJAlTMaWterKRJyEKenILZzzqq()
	{
		return string.Format(CultureInfo.CurrentCulture, "{0}", new object[1] { qhEAlcCfypKkEIlVORqfPYQDtvaj });
	}

	public string GFrJAlTMaWterKRJyEKenILZzzqq(string P_0)
	{
		if (P_0 == null)
		{
			return ToString();
		}
		return string.Format(CultureInfo.CurrentCulture, "{0}", new object[1] { qhEAlcCfypKkEIlVORqfPYQDtvaj.ToString(P_0) });
	}

	public virtual int vpCtZDiWtrmaqwDCmMjniXrnNrOD()
	{
		return qhEAlcCfypKkEIlVORqfPYQDtvaj.ToInt32();
	}

	public bool RiVeyXzIIJEVyClDkSYOlnCBsUpL(yiygAScTdFqbqObFHYHUwiJaZhYwA P_0)
	{
		return qhEAlcCfypKkEIlVORqfPYQDtvaj == P_0.qhEAlcCfypKkEIlVORqfPYQDtvaj;
	}

	public virtual bool RiVeyXzIIJEVyClDkSYOlnCBsUpL(object P_0)
	{
		if (P_0 == null)
		{
			return false;
		}
		if ((object)P_0.GetType() != typeof(yiygAScTdFqbqObFHYHUwiJaZhYwA))
		{
			return false;
		}
		return RiVeyXzIIJEVyClDkSYOlnCBsUpL((yiygAScTdFqbqObFHYHUwiJaZhYwA)P_0);
	}
}
