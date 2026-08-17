using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[DefaultMember("Item")]
internal class XDbjydBaRflbbcCWXMsNJdiGwFsu : IDisposable
{
	private unsafe byte* RyiVQDdIcIIJGhAIxaTpTcpMTLYTA;

	private int GYQHOPynfgYJaNItkxIxPTHRBagy;

	private bool ajbOPLKuqHWQiJIGbSSxeGlqImQV;

	public unsafe IntPtr XPoUgrQanfDKnELyjemeETOoeCpEb => (IntPtr)RyiVQDdIcIIJGhAIxaTpTcpMTLYTA;

	public XDbjydBaRflbbcCWXMsNJdiGwFsu(int P_0)
	{
		pLqOMuCrHehXdQqUjazBUKinDWyR(P_0);
	}

	public unsafe byte ewVgdDoBxqqrnNqfzsJunNPhmXbD(int P_0)
	{
		if (1 + P_0 > GYQHOPynfgYJaNItkxIxPTHRBagy || P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		return RyiVQDdIcIIJGhAIxaTpTcpMTLYTA[P_0];
	}

	public unsafe int uTmENDANNVnxYNZlWbDCcQValJGX(byte* P_0, int P_1, int P_2, int P_3 = 0, int P_4 = 0)
	{
		if (P_0 == null || P_2 <= 0)
		{
			return 0;
		}
		if (P_3 >= GYQHOPynfgYJaNItkxIxPTHRBagy)
		{
			return 0;
		}
		if (P_4 >= P_1)
		{
			return 0;
		}
		if (P_3 < 0)
		{
			P_3 = 0;
		}
		if (P_4 < 0)
		{
			P_4 = 0;
		}
		if (P_3 + P_2 > GYQHOPynfgYJaNItkxIxPTHRBagy)
		{
			P_2 = GYQHOPynfgYJaNItkxIxPTHRBagy - P_3;
		}
		if (P_4 + P_2 > P_1)
		{
			P_2 = P_1 - P_4;
		}
		TbAntJIfSSmAyfyyGIMdtrdGaNBT.NyfaVQAUWOlhoNLqqrnFuAjfaNfr(RyiVQDdIcIIJGhAIxaTpTcpMTLYTA, P_0, P_3, P_4, P_2);
		return P_2;
	}

	public unsafe int uxXfzWekZkEErxQMfmOfTlhGaPsw(byte* P_0, int P_1, int P_2, int P_3 = 0, int P_4 = 0)
	{
		if (P_0 == null || P_1 <= 0 || P_2 <= 0 || P_4 >= P_1 || P_3 >= GYQHOPynfgYJaNItkxIxPTHRBagy)
		{
			return 0;
		}
		if (P_4 < 0)
		{
			P_4 = 0;
		}
		if (P_3 < 0)
		{
			P_3 = 0;
		}
		if (P_4 + P_2 > P_1)
		{
			P_2 = P_1 - P_4;
		}
		if (P_2 + P_3 > GYQHOPynfgYJaNItkxIxPTHRBagy)
		{
			P_2 = GYQHOPynfgYJaNItkxIxPTHRBagy - P_3;
		}
		TbAntJIfSSmAyfyyGIMdtrdGaNBT.NyfaVQAUWOlhoNLqqrnFuAjfaNfr(P_0, RyiVQDdIcIIJGhAIxaTpTcpMTLYTA, P_4, P_3, P_2);
		return P_2;
	}

	public unsafe int aaZDAvvyycdhPsOBkPSXFPVjpDqu(IntPtr P_0, int P_1, int P_2, int P_3 = 0, int P_4 = 0)
	{
		return uxXfzWekZkEErxQMfmOfTlhGaPsw((byte*)(void*)P_0, P_1, P_2, P_3, P_4);
	}

	public unsafe bool pLqOMuCrHehXdQqUjazBUKinDWyR(int P_0)
	{
		if (P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("size");
		}
		if (GYQHOPynfgYJaNItkxIxPTHRBagy == P_0)
		{
			return true;
		}
		MoytzVbxYwklwooyGYcGSOBZQZdO();
		if (P_0 == 0)
		{
			return true;
		}
		GYQHOPynfgYJaNItkxIxPTHRBagy = P_0;
		RyiVQDdIcIIJGhAIxaTpTcpMTLYTA = (byte*)(void*)Marshal.AllocHGlobal(P_0);
		qIHTGPyXAFeIvekIkInZUpXVBhoU();
		return true;
	}

	public unsafe void qIHTGPyXAFeIvekIkInZUpXVBhoU()
	{
		if (GYQHOPynfgYJaNItkxIxPTHRBagy != 0)
		{
			TbAntJIfSSmAyfyyGIMdtrdGaNBT.oZggRQgOfULUnGHOdGGtmCQIzwaVe(RyiVQDdIcIIJGhAIxaTpTcpMTLYTA, GYQHOPynfgYJaNItkxIxPTHRBagy);
		}
	}

	public unsafe void MoytzVbxYwklwooyGYcGSOBZQZdO()
	{
		if (GYQHOPynfgYJaNItkxIxPTHRBagy == 0)
		{
			return;
		}
		try
		{
			if (RyiVQDdIcIIJGhAIxaTpTcpMTLYTA != null)
			{
				Marshal.FreeHGlobal(XPoUgrQanfDKnELyjemeETOoeCpEb);
			}
		}
		catch
		{
		}
		RyiVQDdIcIIJGhAIxaTpTcpMTLYTA = null;
		GYQHOPynfgYJaNItkxIxPTHRBagy = 0;
	}

	public virtual string VjmaihnBKMMBUsSpzfENDGBUkaSk()
	{
		string text = "";
		for (int i = 0; i < GYQHOPynfgYJaNItkxIxPTHRBagy; i++)
		{
			text = text + ewVgdDoBxqqrnNqfzsJunNPhmXbD(i).ToString("x2") + " ";
		}
		return text;
	}

	public void Dispose()
	{
		xSwupOfJnpkNaWoBQWeJXVYlnuzD(true);
		GC.SuppressFinalize(this);
	}

	void IDisposable.Dispose()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Dispose
		this.Dispose();
	}

	protected virtual void IMlnmSmpdDzUGITkFDcBMUVaiLyK()
	{
		try
		{
			xSwupOfJnpkNaWoBQWeJXVYlnuzD(false);
		}
		finally
		{
			base.Finalize();
		}
	}

	protected virtual void xSwupOfJnpkNaWoBQWeJXVYlnuzD(bool P_0)
	{
		if (!ajbOPLKuqHWQiJIGbSSxeGlqImQV)
		{
			MoytzVbxYwklwooyGYcGSOBZQZdO();
			ajbOPLKuqHWQiJIGbSSxeGlqImQV = true;
		}
	}

	[SpecialName]
	public unsafe static IntPtr ppDCbGJKoMCeiEeyUIfgpJPpsDxF(XDbjydBaRflbbcCWXMsNJdiGwFsu P_0)
	{
		if (P_0 == null)
		{
			return IntPtr.Zero;
		}
		return (IntPtr)P_0.RyiVQDdIcIIJGhAIxaTpTcpMTLYTA;
	}
}
