using System;
using System.Runtime.InteropServices;

internal class SnKsqFWMcIJZUgHUHCKhGOTpadlDA
{
	private int sboloENHqDaKmErJCxlUCTbdlWDe;

	private byte[] hqjaEUASFjOXvvoGsKkZpjKAdZUFb;

	public virtual int wSxdShTeVjIhEFmnDeTBxdoyJeBl => sboloENHqDaKmErJCxlUCTbdlWDe;

	protected SnKsqFWMcIJZUgHUHCKhGOTpadlDA()
	{
	}

	internal SnKsqFWMcIJZUgHUHCKhGOTpadlDA(int P_0, IntPtr P_1)
	{
		QXdiXCMNmtICDXJiLzJcnlEDgLLHA(P_0, P_1);
	}

	private unsafe void QXdiXCMNmtICDXJiLzJcnlEDgLLHA(int P_0, IntPtr P_1)
	{
		sboloENHqDaKmErJCxlUCTbdlWDe = P_0;
		if (sboloENHqDaKmErJCxlUCTbdlWDe > 0 && P_1 != IntPtr.Zero)
		{
			hqjaEUASFjOXvvoGsKkZpjKAdZUFb = new byte[P_0];
			fixed (byte* ptr = hqjaEUASFjOXvvoGsKkZpjKAdZUFb)
			{
				aOYtALpBiXtNUYUxrmqpvmqCyvKG.RnAWqRWOrAiuOkNLYFPejoszYblU((IntPtr)ptr, P_1, sboloENHqDaKmErJCxlUCTbdlWDe);
			}
		}
	}

	protected virtual SnKsqFWMcIJZUgHUHCKhGOTpadlDA RHqAfuWsMbBXYmKfQKwGAcvKuhXX(int P_0, IntPtr P_1)
	{
		QXdiXCMNmtICDXJiLzJcnlEDgLLHA(P_0, P_1);
		return this;
	}

	internal virtual void rKfSxXvvhrwYQfyUKZlEjAZcyoYj(IntPtr P_0)
	{
		if (P_0 != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(P_0);
		}
	}

	internal unsafe virtual IntPtr IYbxJSHaRotubmhhwSagJgZcVwfH()
	{
		IntPtr intPtr = IntPtr.Zero;
		if (sboloENHqDaKmErJCxlUCTbdlWDe > 0 && hqjaEUASFjOXvvoGsKkZpjKAdZUFb != null)
		{
			intPtr = Marshal.AllocHGlobal(sboloENHqDaKmErJCxlUCTbdlWDe);
			fixed (byte* ptr = hqjaEUASFjOXvvoGsKkZpjKAdZUFb)
			{
				aOYtALpBiXtNUYUxrmqpvmqCyvKG.RnAWqRWOrAiuOkNLYFPejoszYblU(intPtr, (IntPtr)ptr, sboloENHqDaKmErJCxlUCTbdlWDe);
			}
		}
		return intPtr;
	}

	public unsafe _0001 mDURCbDHtMQgIjbretrBmzVdzPXD<_0001>() where _0001 : SnKsqFWMcIJZUgHUHCKhGOTpadlDA, new()
	{
		if ((object)GetType() == typeof(_0001))
		{
			return (_0001)this;
		}
		if ((object)GetType() == typeof(SnKsqFWMcIJZUgHUHCKhGOTpadlDA))
		{
			fixed (byte* ptr = hqjaEUASFjOXvvoGsKkZpjKAdZUFb)
			{
				void* ptr2 = ptr;
				return (_0001)new _0001().RHqAfuWsMbBXYmKfQKwGAcvKuhXX(sboloENHqDaKmErJCxlUCTbdlWDe, (IntPtr)ptr2);
			}
		}
		return null;
	}
}
