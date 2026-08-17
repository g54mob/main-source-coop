using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

internal class UnhqUOCyEUrgMbiboDhDbPWXYvzG : SnKsqFWMcIJZUgHUHCKhGOTpadlDA
{
	[CompilerGenerated]
	private mtUCgFdfWSzHjwdMMuIhLjSVppYBA[] ZjknRCFHeghdbIMPmebvqUFUNhkG;

	public mtUCgFdfWSzHjwdMMuIhLjSVppYBA[] TBvZOCwQnlmYVTkfPmthUdDIhXhDA
	{
		[CompilerGenerated]
		get
		{
			return ZjknRCFHeghdbIMPmebvqUFUNhkG;
		}
		[CompilerGenerated]
		set
		{
			ZjknRCFHeghdbIMPmebvqUFUNhkG = zjknRCFHeghdbIMPmebvqUFUNhkG;
		}
	}

	unsafe int SnKsqFWMcIJZUgHUHCKhGOTpadlDA.wSxdShTeVjIhEFmnDeTBxdoyJeBl
	{
		get
		{
			if (TBvZOCwQnlmYVTkfPmthUdDIhXhDA == null)
			{
				return 0;
			}
			return TBvZOCwQnlmYVTkfPmthUdDIhXhDA.Length * sizeof(mtUCgFdfWSzHjwdMMuIhLjSVppYBA);
		}
	}

	protected unsafe override SnKsqFWMcIJZUgHUHCKhGOTpadlDA RHqAfuWsMbBXYmKfQKwGAcvKuhXX(int P_0, IntPtr P_1)
	{
		if (P_0 <= 0 || P_0 % sizeof(mtUCgFdfWSzHjwdMMuIhLjSVppYBA) != 0)
		{
			return null;
		}
		int num = P_0 / sizeof(mtUCgFdfWSzHjwdMMuIhLjSVppYBA);
		TBvZOCwQnlmYVTkfPmthUdDIhXhDA = new mtUCgFdfWSzHjwdMMuIhLjSVppYBA[num];
		fixed (mtUCgFdfWSzHjwdMMuIhLjSVppYBA* ptr = TBvZOCwQnlmYVTkfPmthUdDIhXhDA)
		{
			aOYtALpBiXtNUYUxrmqpvmqCyvKG.RnAWqRWOrAiuOkNLYFPejoszYblU((IntPtr)ptr, P_1, aOYtALpBiXtNUYUxrmqpvmqCyvKG.pbdlJJuDWMAxynFvrfVPNfMMBbuS<mtUCgFdfWSzHjwdMMuIhLjSVppYBA>() * TBvZOCwQnlmYVTkfPmthUdDIhXhDA.Length);
		}
		return this;
	}

	internal unsafe override IntPtr IYbxJSHaRotubmhhwSagJgZcVwfH()
	{
		if (wSxdShTeVjIhEFmnDeTBxdoyJeBl == 0)
		{
			return IntPtr.Zero;
		}
		IntPtr intPtr = Marshal.AllocHGlobal(wSxdShTeVjIhEFmnDeTBxdoyJeBl);
		fixed (mtUCgFdfWSzHjwdMMuIhLjSVppYBA* ptr = TBvZOCwQnlmYVTkfPmthUdDIhXhDA)
		{
			aOYtALpBiXtNUYUxrmqpvmqCyvKG.RnAWqRWOrAiuOkNLYFPejoszYblU(intPtr, (IntPtr)ptr, aOYtALpBiXtNUYUxrmqpvmqCyvKG.pbdlJJuDWMAxynFvrfVPNfMMBbuS<mtUCgFdfWSzHjwdMMuIhLjSVppYBA>() * TBvZOCwQnlmYVTkfPmthUdDIhXhDA.Length);
		}
		return intPtr;
	}
}
