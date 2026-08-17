using System;
using System.Runtime.InteropServices;
using Rewired;
using Rewired.Platforms;

internal class sfTKOSJHTxLKQElXFevvOTjQPpQt : nnmvcIglSseYrHJNmHceaCkDJbn, IDisposable
{
	private static class kExyroDIXYlxoAtXoZBfPhqPkKQX
	{
		private struct gXhVaSmdIQeJekhZwKCrgiexxIyXA
		{
			internal int jOIcVMVFyEvjTOhuchzcalKkoFWx;

			internal int EVsaHlsBytxEpQAMFdsitLQErFgd;

			internal int PYhWwDaeovhFJnLETfuWdLwFicwcA;

			internal Guid hLJIsTLDKKQvxQDqsaXOLVmNaYGf;

			internal short CCCCOyKgHOPJiBCKXLaVQIqSUmgI;
		}

		private static readonly Guid hLtsMxqqpGXZnniIybzVUgFmwMJt = new Guid("4D1E55B2-F16F-11CF-88CB-001111000030");

		private static IntPtr vglIqoCBhvzYRvpTvsDIbnYzJSBE;

		private static bool BXRRZcdPckilxUDfvLggDUzOgdWs;

		public static void NScVVvcLJipCjDhrLMwTBDbtWwul(IntPtr P_0)
		{
			gXhVaSmdIQeJekhZwKCrgiexxIyXA structure = new gXhVaSmdIQeJekhZwKCrgiexxIyXA
			{
				EVsaHlsBytxEpQAMFdsitLQErFgd = 5,
				PYhWwDaeovhFJnLETfuWdLwFicwcA = 0,
				hLJIsTLDKKQvxQDqsaXOLVmNaYGf = hLtsMxqqpGXZnniIybzVUgFmwMJt,
				CCCCOyKgHOPJiBCKXLaVQIqSUmgI = 0
			};
			structure.jOIcVMVFyEvjTOhuchzcalKkoFWx = Marshal.SizeOf(structure);
			IntPtr intPtr = Marshal.AllocHGlobal(structure.jOIcVMVFyEvjTOhuchzcalKkoFWx);
			Marshal.StructureToPtr(structure, intPtr, fDeleteOld: true);
			vglIqoCBhvzYRvpTvsDIbnYzJSBE = ZOdduDdZkyWlMMILriIFhWECVUhd(P_0, intPtr, 0);
			BXRRZcdPckilxUDfvLggDUzOgdWs = true;
		}

		public static void bfGCMItBYDSXvltkhqMNrcwPXVvE()
		{
			if (!(vglIqoCBhvzYRvpTvsDIbnYzJSBE == IntPtr.Zero))
			{
				VEQyYWCLkPjDRbXEjPbvkkQfHriI(vglIqoCBhvzYRvpTvsDIbnYzJSBE);
				BXRRZcdPckilxUDfvLggDUzOgdWs = false;
			}
		}

		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "RegisterDeviceNotification", SetLastError = true)]
		private static extern IntPtr ZOdduDdZkyWlMMILriIFhWECVUhd(IntPtr P_0, IntPtr P_1, int P_2);

		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "UnregisterDeviceNotification")]
		private static extern bool VEQyYWCLkPjDRbXEjPbvkkQfHriI(IntPtr P_0);
	}

	private Action<EventArgs> DKCgACYrUEHvbHoIOafECefuEDjPA;

	private Action<EventArgs> PFogwQHgRnbKfssGmbJdJZdOEyhU;

	private Action<EventArgs> OlEfCiaaSqyhcDYQuqjSellnNvegb;

	private Action<PfnQbhAAztkGebiJJBwStuolfJCF, fBMrrsvbWMcOcdDbjcFUuOyQnpTbb> zYMdsojmYAbelvSNSzQYmhhwFsXG;

	private IntPtr slIOOPLLmWsJjtiPCjJtbvipjFEOA;

	private GOVyHsiOMdNQbzSSGNCrEnMIOYXd VQkszfpIxdFDUlvumOqFtcwkGRaO;

	private readonly bool tUWtWtoDQkLEHLtAvNShKighRepG;

	private static dVTqWOZMXYmqKBirNLNYnAJtvPUC qLHaOiJwbjKsCVHpJhSfQMRagsnaA;

	private GOVyHsiOMdNQbzSSGNCrEnMIOYXd MuOeuraVJfwdlUJvugFkoTPqGLnd;

	private bool KDViqgaitKnhIQznoibEvenJdaSqA;

	public IntPtr pZKmIspXNDWgFcvoFKDrZtQYBPAH => slIOOPLLmWsJjtiPCjJtbvipjFEOA;

	event Action<EventArgs> nnmvcIglSseYrHJNmHceaCkDJbn.xznaairANVFFyaYloAxTdaphRlvzB
	{
		add
		{
			DKCgACYrUEHvbHoIOafECefuEDjPA = (Action<EventArgs>)Delegate.Combine(DKCgACYrUEHvbHoIOafECefuEDjPA, b);
		}
		remove
		{
			DKCgACYrUEHvbHoIOafECefuEDjPA = (Action<EventArgs>)Delegate.Remove(DKCgACYrUEHvbHoIOafECefuEDjPA, value2);
		}
	}

	event Action<EventArgs> nnmvcIglSseYrHJNmHceaCkDJbn.hbgWbHlpwcDWGLiViBalBPzBdlNjb
	{
		add
		{
			PFogwQHgRnbKfssGmbJdJZdOEyhU = (Action<EventArgs>)Delegate.Combine(PFogwQHgRnbKfssGmbJdJZdOEyhU, b);
		}
		remove
		{
			PFogwQHgRnbKfssGmbJdJZdOEyhU = (Action<EventArgs>)Delegate.Remove(PFogwQHgRnbKfssGmbJdJZdOEyhU, value2);
		}
	}

	public event Action<PfnQbhAAztkGebiJJBwStuolfJCF, fBMrrsvbWMcOcdDbjcFUuOyQnpTbb> ApnQUOZcBTcxrbfTIjJXFTyaNxFBB
	{
		add
		{
			zYMdsojmYAbelvSNSzQYmhhwFsXG = (Action<PfnQbhAAztkGebiJJBwStuolfJCF, fBMrrsvbWMcOcdDbjcFUuOyQnpTbb>)Delegate.Combine(zYMdsojmYAbelvSNSzQYmhhwFsXG, b);
		}
		remove
		{
			zYMdsojmYAbelvSNSzQYmhhwFsXG = (Action<PfnQbhAAztkGebiJJBwStuolfJCF, fBMrrsvbWMcOcdDbjcFUuOyQnpTbb>)Delegate.Remove(zYMdsojmYAbelvSNSzQYmhhwFsXG, value2);
		}
	}

	public sfTKOSJHTxLKQElXFevvOTjQPpQt()
	{
		tUWtWtoDQkLEHLtAvNShKighRepG = ReInput.editorPlatform != EditorPlatform.None;
		try
		{
			zMvLTGwhzetdyetWHBMJoABIUCVl();
		}
		catch
		{
			fgfETyhHaFUoCIBmVzZjipzQqILDb();
			throw;
		}
	}

	public void fgfETyhHaFUoCIBmVzZjipzQqILDb()
	{
		Dispose();
	}

	void nnmvcIglSseYrHJNmHceaCkDJbn.PSvqvLnjddCOdKaAIomsdSMUdRwWA()
	{
		//ILSpy generated this explicit interface implementation from .override directive in fgfETyhHaFUoCIBmVzZjipzQqILDb
		this.fgfETyhHaFUoCIBmVzZjipzQqILDb();
	}

	private void zMvLTGwhzetdyetWHBMJoABIUCVl()
	{
		KjMlPYAeAxKLjONpjJklUatTggbH();
		QwZSYsJyiCkXTeJoYIUanhXMCVVGA();
		if (tUWtWtoDQkLEHLtAvNShKighRepG)
		{
			MuOeuraVJfwdlUJvugFkoTPqGLnd = new GOVyHsiOMdNQbzSSGNCrEnMIOYXd();
			MuOeuraVJfwdlUJvugFkoTPqGLnd.ZaeAONefLBhpRGYekpJmRbMfpOQab(otcVtmQEAofGChwfQbPNuNbGvpGV, true);
		}
	}

	public void Dispose()
	{
		YtFqpWkHRGplRkifGFoUXnPcnKOX(true);
		GC.SuppressFinalize(this);
	}

	void IDisposable.Dispose()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Dispose
		this.Dispose();
	}

	protected virtual void RmsRtyzadQGwzbIXKLzNMbGqiGCnA()
	{
		try
		{
			YtFqpWkHRGplRkifGFoUXnPcnKOX(false);
		}
		finally
		{
			base.Finalize();
		}
	}

	private void YtFqpWkHRGplRkifGFoUXnPcnKOX(bool P_0)
	{
		if (KDViqgaitKnhIQznoibEvenJdaSqA)
		{
			return;
		}
		if (tUWtWtoDQkLEHLtAvNShKighRepG)
		{
			rsifTbWKtCAFBKKRIzgaUiSEIpkr();
			if (MuOeuraVJfwdlUJvugFkoTPqGLnd != null)
			{
				MuOeuraVJfwdlUJvugFkoTPqGLnd.Dispose();
			}
			if (qLHaOiJwbjKsCVHpJhSfQMRagsnaA != null)
			{
				qLHaOiJwbjKsCVHpJhSfQMRagsnaA.Dispose();
				qLHaOiJwbjKsCVHpJhSfQMRagsnaA = null;
			}
		}
		else
		{
			rsifTbWKtCAFBKKRIzgaUiSEIpkr();
			if (VQkszfpIxdFDUlvumOqFtcwkGRaO != null)
			{
				VQkszfpIxdFDUlvumOqFtcwkGRaO.Dispose();
			}
		}
		KDViqgaitKnhIQznoibEvenJdaSqA = true;
	}

	private void QwZSYsJyiCkXTeJoYIUanhXMCVVGA()
	{
		kExyroDIXYlxoAtXoZBfPhqPkKQX.NScVVvcLJipCjDhrLMwTBDbtWwul(slIOOPLLmWsJjtiPCjJtbvipjFEOA);
	}

	private void rsifTbWKtCAFBKKRIzgaUiSEIpkr()
	{
		kExyroDIXYlxoAtXoZBfPhqPkKQX.bfGCMItBYDSXvltkhqMNrcwPXVvE();
	}

	private void MThjcRARngmQACxivTednVRaqWnR(MfzatynuFTZcaumUqgpvALYfiEpbb P_0, PfnQbhAAztkGebiJJBwStuolfJCF P_1, uint P_2, IntPtr P_3)
	{
		if (P_2 != 537)
		{
			return;
		}
		int num = P_1.CEvgadGvxvYSMaYrYtdcrsqgKzTLA();
		if (P_3 == slIOOPLLmWsJjtiPCjJtbvipjFEOA)
		{
			switch (num)
			{
			case 32768:
				DKCgACYrUEHvbHoIOafECefuEDjPA?.Invoke(null);
				break;
			case 32772:
				PFogwQHgRnbKfssGmbJdJZdOEyhU?.Invoke(null);
				break;
			case 32771:
				OlEfCiaaSqyhcDYQuqjSellnNvegb?.Invoke(null);
				break;
			}
		}
	}

	private void otcVtmQEAofGChwfQbPNuNbGvpGV(MfzatynuFTZcaumUqgpvALYfiEpbb P_0, PfnQbhAAztkGebiJJBwStuolfJCF P_1, uint P_2, IntPtr P_3)
	{
		if (tUWtWtoDQkLEHLtAvNShKighRepG && (P_2 == 6 || P_2 == 28))
		{
			fBMrrsvbWMcOcdDbjcFUuOyQnpTbb fBMrrsvbWMcOcdDbjcFUuOyQnpTbb2 = ukOapkBndpfhzrsQGpDKSpCquzAqA.YpfcnAiPGIcplppqFKAqZQDUkNZPA(P_1.CEvgadGvxvYSMaYrYtdcrsqgKzTLA());
			if (fBMrrsvbWMcOcdDbjcFUuOyQnpTbb2 != fBMrrsvbWMcOcdDbjcFUuOyQnpTbb.None && zYMdsojmYAbelvSNSzQYmhhwFsXG != null)
			{
				zYMdsojmYAbelvSNSzQYmhhwFsXG(P_1, fBMrrsvbWMcOcdDbjcFUuOyQnpTbb2);
			}
		}
	}

	private void KjMlPYAeAxKLjONpjJklUatTggbH()
	{
		if (qLHaOiJwbjKsCVHpJhSfQMRagsnaA == null)
		{
			qLHaOiJwbjKsCVHpJhSfQMRagsnaA = new dVTqWOZMXYmqKBirNLNYnAJtvPUC("RewiredWDMWindow", true, zHZLRwMjBuHMbXGfGlsEVzUqbpEj);
			if (qLHaOiJwbjKsCVHpJhSfQMRagsnaA.qqeNEhvZzMQkqjFUFQSmwOmLiuvb == IntPtr.Zero)
			{
				throw new Exception("Error creating window.");
			}
		}
		else
		{
			if (qLHaOiJwbjKsCVHpJhSfQMRagsnaA.qqeNEhvZzMQkqjFUFQSmwOmLiuvb == IntPtr.Zero)
			{
				throw new Exception("Message window has invalid handle.");
			}
			qLHaOiJwbjKsCVHpJhSfQMRagsnaA.MbMVPTZuzKvQpZbnyBlsxPvSMUBt(zHZLRwMjBuHMbXGfGlsEVzUqbpEj);
		}
		slIOOPLLmWsJjtiPCjJtbvipjFEOA = qLHaOiJwbjKsCVHpJhSfQMRagsnaA.qqeNEhvZzMQkqjFUFQSmwOmLiuvb;
	}

	private IntPtr zHZLRwMjBuHMbXGfGlsEVzUqbpEj(IntPtr P_0, uint P_1, IntPtr P_2, IntPtr P_3)
	{
		MThjcRARngmQACxivTednVRaqWnR(MfzatynuFTZcaumUqgpvALYfiEpbb.yXqnlWRVYEFdDzolmhhrejoSQllY(P_3), PfnQbhAAztkGebiJJBwStuolfJCF.WpWHfjulCUuweammBUmNUoEZbQoH(P_2), P_1, P_0);
		return IntPtr.Zero;
	}
}
