using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

internal class mAoXlxZILjcMJKicdBJPgflDaCklb
{
	private readonly List<Delegate> yxGHbkqGosMHZxDoDufGoPngHzqG;

	private readonly IntPtr VTtPcHeacfDiCBOqyGUXoJQKinvPA;

	public IntPtr qhEAlcCfypKkEIlVORqfPYQDtvaj => VTtPcHeacfDiCBOqyGUXoJQKinvPA;

	public mAoXlxZILjcMJKicdBJPgflDaCklb(int P_0)
	{
		VTtPcHeacfDiCBOqyGUXoJQKinvPA = Marshal.AllocHGlobal(IntPtr.Size * P_0);
		yxGHbkqGosMHZxDoDufGoPngHzqG = new List<Delegate>();
	}

	public unsafe void QkEIblOCcBSvhdcllJrkxZstaSxX(Delegate P_0)
	{
		int count = yxGHbkqGosMHZxDoDufGoPngHzqG.Count;
		yxGHbkqGosMHZxDoDufGoPngHzqG.Add(P_0);
		((IntPtr*)(void*)VTtPcHeacfDiCBOqyGUXoJQKinvPA)[count] = Marshal.GetFunctionPointerForDelegate(P_0);
	}
}
