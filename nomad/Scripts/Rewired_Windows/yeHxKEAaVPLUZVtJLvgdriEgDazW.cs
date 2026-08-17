using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

internal class yeHxKEAaVPLUZVtJLvgdriEgDazW<_0001> : IDisposable where _0001 : struct
{
	private static readonly int jQkalxLWCLfetZrCHVNNAkjGtXdq = Marshal.SizeOf(typeof(_0001));

	private XDbjydBaRflbbcCWXMsNJdiGwFsu HjeNOOJFRkzdJicysJWPwSYrqXru;

	private bool eUqJzbTfKnkZOpKCENBvHYxjYzPf;

	public XDbjydBaRflbbcCWXMsNJdiGwFsu kRwcWSbXuaICyiQzWAGAeIWfvqPeb => HjeNOOJFRkzdJicysJWPwSYrqXru;

	public bool WRCMhugiRwGuxooCLhsgkBvxpmxO
	{
		get
		{
			if (HjeNOOJFRkzdJicysJWPwSYrqXru != null)
			{
				return HjeNOOJFRkzdJicysJWPwSYrqXru.XPoUgrQanfDKnELyjemeETOoeCpEb != IntPtr.Zero;
			}
			return false;
		}
	}

	public unsafe _0001 YdWtcSRuzXyCzeeQdIauXKhuQdui
	{
		get
		{
			nQTYzYhPQrCdMkzuutvlReJmalghA();
			return System.Runtime.CompilerServices.Unsafe.Read<_0001>((void*)HjeNOOJFRkzdJicysJWPwSYrqXru.XPoUgrQanfDKnELyjemeETOoeCpEb);
		}
		set
		{
			nQTYzYhPQrCdMkzuutvlReJmalghA();
			_0001* ptr = &val;
			HjeNOOJFRkzdJicysJWPwSYrqXru.aaZDAvvyycdhPsOBkPSXFPVjpDqu((IntPtr)ptr, jQkalxLWCLfetZrCHVNNAkjGtXdq, jQkalxLWCLfetZrCHVNNAkjGtXdq);
		}
	}

	public yeHxKEAaVPLUZVtJLvgdriEgDazW()
	{
		HjeNOOJFRkzdJicysJWPwSYrqXru = new XDbjydBaRflbbcCWXMsNJdiGwFsu(jQkalxLWCLfetZrCHVNNAkjGtXdq);
	}

	private void bfCNPDLreheMohfEMUCsoXGtUBOm()
	{
		if (HjeNOOJFRkzdJicysJWPwSYrqXru == null)
		{
			HjeNOOJFRkzdJicysJWPwSYrqXru.Dispose();
			HjeNOOJFRkzdJicysJWPwSYrqXru = null;
		}
	}

	private void nQTYzYhPQrCdMkzuutvlReJmalghA()
	{
		if (!WRCMhugiRwGuxooCLhsgkBvxpmxO)
		{
			throw new Exception("Memory not allocated.");
		}
	}

	private void VKxpFNqHIZYRxVfiAqwwzedYNDIu(bool P_0)
	{
		if (!eUqJzbTfKnkZOpKCENBvHYxjYzPf)
		{
			if (P_0)
			{
				bfCNPDLreheMohfEMUCsoXGtUBOm();
			}
			eUqJzbTfKnkZOpKCENBvHYxjYzPf = true;
		}
	}

	public void Dispose()
	{
		VKxpFNqHIZYRxVfiAqwwzedYNDIu(true);
		GC.SuppressFinalize(this);
	}

	void IDisposable.Dispose()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Dispose
		this.Dispose();
	}
}
