using System;
using System.Runtime.InteropServices;

internal class KttgBXysILZXRjsYwEUDaxPRNIGo : IDisposable
{
	public struct aNUuebpblySUIzWSLcwPOYQekgEW
	{
		private byte RmURcIOicjlkYjxlIGYakaMDDnJJ;

		private uint DBTIqRqgnQtLROrrbUYXifwkegid;

		private int CTVzpgRhTiDxbgXTcpWPhKXADeUxA;

		private static aNUuebpblySUIzWSLcwPOYQekgEW HzKsaIEegGyJenGmyAouldmOrILh;

		public byte NeFVkUPJYydDkIGpUyliekECCOUd => RmURcIOicjlkYjxlIGYakaMDDnJJ;

		public uint JmAZYMViSGEWnFrhuhbUaAHYgGqCA => DBTIqRqgnQtLROrrbUYXifwkegid;

		public int SHtQBbZniKBdCJZwlwvZKEBlYPmZ => CTVzpgRhTiDxbgXTcpWPhKXADeUxA;

		public static aNUuebpblySUIzWSLcwPOYQekgEW CeZAEKBprvSIItPnbiNVTeUvCrnZA => HzKsaIEegGyJenGmyAouldmOrILh;

		public aNUuebpblySUIzWSLcwPOYQekgEW(byte P_0, uint P_1, int P_2)
		{
			RmURcIOicjlkYjxlIGYakaMDDnJJ = P_0;
			DBTIqRqgnQtLROrrbUYXifwkegid = P_1;
			CTVzpgRhTiDxbgXTcpWPhKXADeUxA = P_2;
			if (CTVzpgRhTiDxbgXTcpWPhKXADeUxA < 0)
			{
				CTVzpgRhTiDxbgXTcpWPhKXADeUxA = 0;
			}
		}
	}

	private const byte XBGrnfObzfPClBhAXpslQvFgrmeK = 254;

	private uint bWogGebqnSldaQkCqqjphRHclXPu;

	private int tplIFjInGlcRaVoEQkZUbBmDtpgy;

	private unsafe byte* WSxAfwfNyeVZUviowmCvpWuNfVy;

	private byte RmURcIOicjlkYjxlIGYakaMDDnJJ;

	private bool TCqVPpBZmTenqpFXQcVYAlwKYVDtA;

	private bool NchdYNbKzqsssgcQJdenZuGqXgLo;

	public int ZHEQuImTVvNEkKAXcGUIvuGvaloA => tplIFjInGlcRaVoEQkZUbBmDtpgy;

	public unsafe KttgBXysILZXRjsYwEUDaxPRNIGo(int P_0)
	{
		if (P_0 <= 0)
		{
			throw new Exception("size must be > 0!");
		}
		tplIFjInGlcRaVoEQkZUbBmDtpgy = P_0;
		bWogGebqnSldaQkCqqjphRHclXPu = 0u;
		WSxAfwfNyeVZUviowmCvpWuNfVy = (byte*)(void*)Marshal.AllocHGlobal(P_0);
	}

	public unsafe bool KjttXAEzsRtECEzfsLykXNbscFEq(IntPtr P_0, int P_1, out aNUuebpblySUIzWSLcwPOYQekgEW P_2)
	{
		if (WSxAfwfNyeVZUviowmCvpWuNfVy == null || P_1 <= 0)
		{
			P_2 = default(aNUuebpblySUIzWSLcwPOYQekgEW);
			return false;
		}
		if (P_1 > tplIFjInGlcRaVoEQkZUbBmDtpgy)
		{
			throw new Exception("Length is larger than the buffer.");
		}
		if ((uint)((int)bWogGebqnSldaQkCqqjphRHclXPu + P_1) >= tplIFjInGlcRaVoEQkZUbBmDtpgy)
		{
			bWogGebqnSldaQkCqqjphRHclXPu = 0u;
			if (RmURcIOicjlkYjxlIGYakaMDDnJJ == 254)
			{
				RmURcIOicjlkYjxlIGYakaMDDnJJ = 0;
				TCqVPpBZmTenqpFXQcVYAlwKYVDtA = true;
			}
			else
			{
				RmURcIOicjlkYjxlIGYakaMDDnJJ++;
			}
		}
		hUfdZejvJYlOtHWdillPtEZZfcOAA.FZwJwYXvSsXioalMbVsJdESnycGk(WSxAfwfNyeVZUviowmCvpWuNfVy + bWogGebqnSldaQkCqqjphRHclXPu, (void*)P_0, new UIntPtr((uint)P_1));
		P_2 = new aNUuebpblySUIzWSLcwPOYQekgEW(RmURcIOicjlkYjxlIGYakaMDDnJJ, bWogGebqnSldaQkCqqjphRHclXPu, P_1);
		bWogGebqnSldaQkCqqjphRHclXPu += (uint)P_1;
		return true;
	}

	public int pkpJIXUPRvEEdtemqnOqHyayGnzb(aNUuebpblySUIzWSLcwPOYQekgEW P_0, byte[] P_1)
	{
		if (P_1 == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (P_1.Length < P_0.SHtQBbZniKBdCJZwlwvZKEBlYPmZ)
		{
			throw new Exception("Buffer is not large enough to hold the data.");
		}
		if (!ctgeAhWpBOwsIJvcWvmfRuDIDuZl(ref P_0))
		{
			return -1;
		}
		Marshal.Copy(BsAAwuYuChqUlaCpaXMrtHWPukNX(P_0), P_1, 0, P_0.SHtQBbZniKBdCJZwlwvZKEBlYPmZ);
		return P_0.SHtQBbZniKBdCJZwlwvZKEBlYPmZ;
	}

	public unsafe int pkpJIXUPRvEEdtemqnOqHyayGnzb(aNUuebpblySUIzWSLcwPOYQekgEW P_0, IntPtr P_1, int P_2)
	{
		if (P_1 == IntPtr.Zero)
		{
			throw new Exception("Buffer pointer is invalid.");
		}
		if (P_2 <= 0)
		{
			return -1;
		}
		if (P_2 < P_0.SHtQBbZniKBdCJZwlwvZKEBlYPmZ)
		{
			throw new Exception("Buffer is not large enough to hold the data.");
		}
		if (!ctgeAhWpBOwsIJvcWvmfRuDIDuZl(ref P_0))
		{
			return -1;
		}
		hUfdZejvJYlOtHWdillPtEZZfcOAA.FZwJwYXvSsXioalMbVsJdESnycGk((void*)P_1, WSxAfwfNyeVZUviowmCvpWuNfVy, new UIntPtr((uint)P_0.SHtQBbZniKBdCJZwlwvZKEBlYPmZ));
		return P_0.SHtQBbZniKBdCJZwlwvZKEBlYPmZ;
	}

	public unsafe IntPtr BsAAwuYuChqUlaCpaXMrtHWPukNX(aNUuebpblySUIzWSLcwPOYQekgEW P_0)
	{
		if (WSxAfwfNyeVZUviowmCvpWuNfVy == null || !ctgeAhWpBOwsIJvcWvmfRuDIDuZl(ref P_0))
		{
			return IntPtr.Zero;
		}
		return (IntPtr)(WSxAfwfNyeVZUviowmCvpWuNfVy + P_0.JmAZYMViSGEWnFrhuhbUaAHYgGqCA);
	}

	public unsafe bool DdgeaTTcGBRrnDlwclGBZUkhxhAx(aNUuebpblySUIzWSLcwPOYQekgEW P_0, out IntPtr P_1)
	{
		if (WSxAfwfNyeVZUviowmCvpWuNfVy == null || !ctgeAhWpBOwsIJvcWvmfRuDIDuZl(ref P_0))
		{
			P_1 = IntPtr.Zero;
			return false;
		}
		P_1 = (IntPtr)(WSxAfwfNyeVZUviowmCvpWuNfVy + P_0.JmAZYMViSGEWnFrhuhbUaAHYgGqCA);
		return true;
	}

	private bool ctgeAhWpBOwsIJvcWvmfRuDIDuZl(ref aNUuebpblySUIzWSLcwPOYQekgEW P_0)
	{
		int num = P_0.SHtQBbZniKBdCJZwlwvZKEBlYPmZ;
		if (num <= 0)
		{
			return false;
		}
		uint num2 = P_0.NeFVkUPJYydDkIGpUyliekECCOUd;
		if (num2 > 254)
		{
			return false;
		}
		if (num2 != RmURcIOicjlkYjxlIGYakaMDDnJJ)
		{
			if (!TCqVPpBZmTenqpFXQcVYAlwKYVDtA)
			{
				if (num2 + 1 != RmURcIOicjlkYjxlIGYakaMDDnJJ)
				{
					return false;
				}
			}
			else if (num2 > RmURcIOicjlkYjxlIGYakaMDDnJJ)
			{
				if (RmURcIOicjlkYjxlIGYakaMDDnJJ != 0 || num2 != 254)
				{
					return false;
				}
			}
			else if (num2 + 1 != RmURcIOicjlkYjxlIGYakaMDDnJJ)
			{
				return false;
			}
			if (P_0.JmAZYMViSGEWnFrhuhbUaAHYgGqCA < bWogGebqnSldaQkCqqjphRHclXPu)
			{
				return false;
			}
		}
		else if (P_0.JmAZYMViSGEWnFrhuhbUaAHYgGqCA + num > bWogGebqnSldaQkCqqjphRHclXPu)
		{
			return false;
		}
		if (P_0.JmAZYMViSGEWnFrhuhbUaAHYgGqCA + num > tplIFjInGlcRaVoEQkZUbBmDtpgy)
		{
			return false;
		}
		return true;
	}

	public void Dispose()
	{
		lDxnsjCDTQrmresvWgbliNUVruIc(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void zNJVymYugIbeeZuNgMrKxyYWbziV()
	{
		try
		{
			lDxnsjCDTQrmresvWgbliNUVruIc(false);
		}
		finally
		{
			base.Finalize();
		}
	}

	protected unsafe virtual void lDxnsjCDTQrmresvWgbliNUVruIc(bool P_0)
	{
		if (!NchdYNbKzqsssgcQJdenZuGqXgLo)
		{
			if (WSxAfwfNyeVZUviowmCvpWuNfVy != null)
			{
				Marshal.FreeHGlobal((IntPtr)WSxAfwfNyeVZUviowmCvpWuNfVy);
			}
			NchdYNbKzqsssgcQJdenZuGqXgLo = true;
		}
	}
}
