using System;
using Rewired.Internal;
using Rewired.Internal.Glyphs;
using Rewired.Utils.Classes.Data;

internal sealed class vNAwabfVGwBmZXFoOexLVfwdqKMS : IPrefetch, IDisposable
{
	private Action ElcnkyUYeGuxVaVoZujGmdsELEmA;

	private Id asAREUJuSOWCxkNXvUoydGGykpHF;

	private bool RmKMtGGPtxdexQNXFGHfOlfIFEEH;

	public vNAwabfVGwBmZXFoOexLVfwdqKMS(Action P_0)
	{
		ElcnkyUYeGuxVaVoZujGmdsELEmA = P_0;
		asAREUJuSOWCxkNXvUoydGGykpHF = 0u;
		GlyphManager.Add(this, ref asAREUJuSOWCxkNXvUoydGGykpHF);
	}

	void IPrefetch.Prefetch()
	{
		ElcnkyUYeGuxVaVoZujGmdsELEmA();
	}

	private void AkEygUcDCQdxscuSFGaCHDMglpSiA(bool P_0)
	{
		if (!RmKMtGGPtxdexQNXFGHfOlfIFEEH)
		{
			if (P_0)
			{
				GlyphManager.Remove(ref asAREUJuSOWCxkNXvUoydGGykpHF);
			}
			RmKMtGGPtxdexQNXFGHfOlfIFEEH = true;
		}
	}

	public void Dispose()
	{
		AkEygUcDCQdxscuSFGaCHDMglpSiA(true);
		GC.SuppressFinalize(this);
	}

	void IDisposable.Dispose()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Dispose
		this.Dispose();
	}
}
