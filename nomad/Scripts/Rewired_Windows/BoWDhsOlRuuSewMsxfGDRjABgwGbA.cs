using System;
using System.Runtime.CompilerServices;

internal abstract class BoWDhsOlRuuSewMsxfGDRjABgwGbA : IDisposable
{
	[CompilerGenerated]
	private EventHandler<EventArgs> DGYaUJareCUTxPPyJjfAhJFGaqATB;

	[CompilerGenerated]
	private EventHandler<EventArgs> OSFLbjyXfRabDUECFBpnGOBQjHkZ;

	[CompilerGenerated]
	private bool aJCMbgPRHrsqBQLTVfIlBhFhnEYDA;

	public bool PhVlQZEjQrXjFiADScOSilfqQCReb
	{
		[CompilerGenerated]
		get
		{
			return aJCMbgPRHrsqBQLTVfIlBhFhnEYDA;
		}
		[CompilerGenerated]
		private set
		{
			aJCMbgPRHrsqBQLTVfIlBhFhnEYDA = flag;
		}
	}

	protected virtual void TKtjRQUoyelLfsvTEcewRLHlveof()
	{
		try
		{
			wUCvOhgJfupOWCVTsZUyGobOnrnf(false);
		}
		finally
		{
			base.Finalize();
		}
	}

	public void Dispose()
	{
		wUCvOhgJfupOWCVTsZUyGobOnrnf(true);
	}

	void IDisposable.Dispose()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Dispose
		this.Dispose();
	}

	private void wUCvOhgJfupOWCVTsZUyGobOnrnf(bool P_0)
	{
		if (!PhVlQZEjQrXjFiADScOSilfqQCReb)
		{
			DGYaUJareCUTxPPyJjfAhJFGaqATB?.Invoke(this, EventArgs.Empty);
			rRsMTzOJJvcyZcAJeGMleLmNeBkx(P_0);
			GC.SuppressFinalize(this);
			PhVlQZEjQrXjFiADScOSilfqQCReb = true;
			OSFLbjyXfRabDUECFBpnGOBQjHkZ?.Invoke(this, EventArgs.Empty);
		}
	}

	protected abstract void rRsMTzOJJvcyZcAJeGMleLmNeBkx(bool P_0);
}
