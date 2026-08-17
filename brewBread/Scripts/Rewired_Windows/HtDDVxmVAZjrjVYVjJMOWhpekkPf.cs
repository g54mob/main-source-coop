using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

internal abstract class HtDDVxmVAZjrjVYVjJMOWhpekkPf : cSHubWgUMcojQENLcjkzsXuQKVzU
{
	[CompilerGenerated]
	private ftHGEOBkFlDqxRXIUCGQiwAXBAveb dDtEaQkaYXEtUMfRnzYWQQsRqllcb;

	public ftHGEOBkFlDqxRXIUCGQiwAXBAveb rBpwDVVnDinSeXBnaHZBiCBsnXbPA
	{
		[CompilerGenerated]
		get
		{
			return dDtEaQkaYXEtUMfRnzYWQQsRqllcb;
		}
		[CompilerGenerated]
		private set
		{
			dDtEaQkaYXEtUMfRnzYWQQsRqllcb = ftHGEOBkFlDqxRXIUCGQiwAXBAveb2;
		}
	}

	protected abstract mAoXlxZILjcMJKicdBJPgflDaCklb hkwPyXYdUFBEwbsywFCQlzzNIJPj { get; }

	public unsafe virtual void iNjZxEWgGYMxgNiUyUmpWefCBkbC(ftHGEOBkFlDqxRXIUCGQiwAXBAveb P_0)
	{
		rBpwDVVnDinSeXBnaHZBiCBsnXbPA = P_0;
		base.KUJrOBoduRcGKIkxgMoKicmgtCeb = Marshal.AllocHGlobal(IntPtr.Size * 2);
		GCHandle value = GCHandle.Alloc(this);
		Marshal.WriteIntPtr(base.KUJrOBoduRcGKIkxgMoKicmgtCeb, hkwPyXYdUFBEwbsywFCQlzzNIJPj.qhEAlcCfypKkEIlVORqfPYQDtvaj);
		((IntPtr*)(void*)base.KUJrOBoduRcGKIkxgMoKicmgtCeb)[1] = GCHandle.ToIntPtr(value);
	}

	protected unsafe override void lDxnsjCDTQrmresvWgbliNUVruIc(bool P_0)
	{
		if (base.KUJrOBoduRcGKIkxgMoKicmgtCeb != IntPtr.Zero)
		{
			GCHandle.FromIntPtr(((IntPtr*)(void*)base.KUJrOBoduRcGKIkxgMoKicmgtCeb)[1]).Free();
			Marshal.FreeHGlobal(base.KUJrOBoduRcGKIkxgMoKicmgtCeb);
			base.KUJrOBoduRcGKIkxgMoKicmgtCeb = IntPtr.Zero;
		}
		rBpwDVVnDinSeXBnaHZBiCBsnXbPA = null;
		base.lDxnsjCDTQrmresvWgbliNUVruIc(P_0);
	}

	internal unsafe static _0001 GuJnRNRRWAIehwFdLTbSvwbFhtle<_0001>(IntPtr P_0) where _0001 : HtDDVxmVAZjrjVYVjJMOWhpekkPf
	{
		return (_0001)GCHandle.FromIntPtr(((IntPtr*)(void*)P_0)[1]).Target;
	}
}
