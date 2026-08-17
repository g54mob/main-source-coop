using System;
using System.Threading;
using Rewired;
using Rewired.Utils;
using Rewired.Utils.Classes.Utility;

internal class ufubKZLQCAcjcGccOHsfXYkPkynTA : IDisposable
{
	public delegate void qSDjYHuEwvdUhQhDXvomMaRBCMpjA(IntPtr reportPointer, int reportByteLength, int reportCount, double timestamp);

	private readonly qSDjYHuEwvdUhQhDXvomMaRBCMpjA woxltfuEmJDtmHLtULAohmWiNKfNb;

	private readonly gEdIOxSuKIfJTgYgkUkkmVYXKnOFA ythRetlxzeqhYHUSyvOguXQHotnj;

	private readonly ThreadHelper PVniwZfInMoMNQdfTcxWaYtSqjYd;

	private readonly int hLWrEkGPhdKFFZUdoiATEjpGoMzq;

	private readonly int IprXLvrRbiEebFdcGxdJQfYwsuFf;

	private readonly string NWOAJRMWfHwYvDAsJepcyEclDEdV;

	private readonly byte[] RnAfDormVVJUTdldzCMoKnNpzChc;

	private readonly byte[] omVTDwjflLASbkxLIDEvfMgBDszqc;

	private int SQEpRsYtlcQSDolfqfkmipiOGkcD;

	private ZzPGoDBJildkPPHmulNuiungQFoV XGjAuhfzzjNdDdXrXgeDvyNUZVkH;

	private ZzPGoDBJildkPPHmulNuiungQFoV gIrpQuoJShgkSKTKBCVSNRqGCzDEb;

	private bool KLoYTfzGtkZrIHqyIdkGtVLecnH;

	public ufubKZLQCAcjcGccOHsfXYkPkynTA(string P_0, int P_1, string P_2, qSDjYHuEwvdUhQhDXvomMaRBCMpjA P_3)
	{
		if (string.IsNullOrEmpty(P_0))
		{
			throw new ArgumentNullException("hidDevicePath");
		}
		if (P_3 == null)
		{
			throw new ArgumentNullException("processReportDelegate");
		}
		IprXLvrRbiEebFdcGxdJQfYwsuFf = P_1;
		if (IprXLvrRbiEebFdcGxdJQfYwsuFf <= 0)
		{
			IprXLvrRbiEebFdcGxdJQfYwsuFf = 512;
		}
		hLWrEkGPhdKFFZUdoiATEjpGoMzq = P_1 + 8;
		NWOAJRMWfHwYvDAsJepcyEclDEdV = P_2;
		woxltfuEmJDtmHLtULAohmWiNKfNb = P_3;
		int num = hLWrEkGPhdKFFZUdoiATEjpGoMzq * 60;
		if (num <= 0)
		{
			Logger.LogError("Invalid report buffer size. This device \"" + P_2 + "\" will not function.");
			throw new Exception();
		}
		try
		{
			ythRetlxzeqhYHUSyvOguXQHotnj = new gEdIOxSuKIfJTgYgkUkkmVYXKnOFA(P_0, P_1, 250);
		}
		catch (Exception)
		{
			Logger.LogError("Out of memory. This device \"" + P_2 + "\" will not function.");
			throw;
		}
		try
		{
			XGjAuhfzzjNdDdXrXgeDvyNUZVkH = new ZzPGoDBJildkPPHmulNuiungQFoV(num);
			gIrpQuoJShgkSKTKBCVSNRqGCzDEb = new ZzPGoDBJildkPPHmulNuiungQFoV(num);
			RnAfDormVVJUTdldzCMoKnNpzChc = new byte[hLWrEkGPhdKFFZUdoiATEjpGoMzq];
			omVTDwjflLASbkxLIDEvfMgBDszqc = new byte[hLWrEkGPhdKFFZUdoiATEjpGoMzq];
		}
		catch (Exception)
		{
			Logger.LogError("Out of memory. This device \"" + P_2 + "\" will not function.");
			throw;
		}
		try
		{
			PVniwZfInMoMNQdfTcxWaYtSqjYd = ThreadHelper.Create();
			PVniwZfInMoMNQdfTcxWaYtSqjYd.ThreadUpdateEvent += llrzyEUooSJuyhpPKjwmFWZjuZIEA;
			PVniwZfInMoMNQdfTcxWaYtSqjYd.Start(wait: false);
		}
		catch (Exception)
		{
			Logger.LogError("Error creating thread. This device \"" + P_2 + "\" will not function.");
			throw;
		}
	}

	public unsafe void olhAVCAgKmHsozKfCeCEQctAizWZ()
	{
		try
		{
			if (oBnFWPDWaNumEDAIqkVvRwmgfICT())
			{
				return;
			}
			USwLvCHkpcCezjzNWnwJukdkRPaQ();
			int num = 0;
			byte[] rnAfDormVVJUTdldzCMoKnNpzChc = RnAfDormVVJUTdldzCMoKnNpzChc;
			fixed (byte* ptr = rnAfDormVVJUTdldzCMoKnNpzChc)
			{
				while (XGjAuhfzzjNdDdXrXgeDvyNUZVkH.mbZiiJKMpleSyWlfMidOugAMlXXp(rnAfDormVVJUTdldzCMoKnNpzChc, hLWrEkGPhdKFFZUdoiATEjpGoMzq) > 0)
				{
					woxltfuEmJDtmHLtULAohmWiNKfNb((IntPtr)ptr, IprXLvrRbiEebFdcGxdJQfYwsuFf, 1, *(double*)(ptr + IprXLvrRbiEebFdcGxdJQfYwsuFf));
					num++;
				}
			}
		}
		catch
		{
		}
	}

	private void USwLvCHkpcCezjzNWnwJukdkRPaQ()
	{
		lock (XGjAuhfzzjNdDdXrXgeDvyNUZVkH)
		{
			lock (gIrpQuoJShgkSKTKBCVSNRqGCzDEb)
			{
				MiscTools.Swap(ref XGjAuhfzzjNdDdXrXgeDvyNUZVkH, ref gIrpQuoJShgkSKTKBCVSNRqGCzDEb);
			}
		}
	}

	private void llrzyEUooSJuyhpPKjwmFWZjuZIEA()
	{
		if (SQEpRsYtlcQSDolfqfkmipiOGkcD != 0)
		{
			Thread.Sleep(500);
			return;
		}
		try
		{
			byte[] array = omVTDwjflLASbkxLIDEvfMgBDszqc;
			if (!eyoTsuMmClyYRuAqwDiIcPeyZMGU(array))
			{
				return;
			}
			lock (gIrpQuoJShgkSKTKBCVSNRqGCzDEb)
			{
				gIrpQuoJShgkSKTKBCVSNRqGCzDEb.tOVowfYdZvrOWRwKzvzzOyfLIIYn(array, array.Length);
			}
		}
		catch
		{
		}
	}

	private bool eyoTsuMmClyYRuAqwDiIcPeyZMGU(byte[] P_0)
	{
		switch (ythRetlxzeqhYHUSyvOguXQHotnj.DkPpsvALKIWITZLnuzUMOyGUBLWT(P_0))
		{
		case gEdIOxSuKIfJTgYgkUkkmVYXKnOFA.RnQCTXuoxpSKPcwrSLLHUuqznWnf.Success:
			return true;
		case gEdIOxSuKIfJTgYgkUkkmVYXKnOFA.RnQCTXuoxpSKPcwrSLLHUuqznWnf.Error:
			Thread.Sleep(500);
			break;
		case gEdIOxSuKIfJTgYgkUkkmVYXKnOFA.RnQCTXuoxpSKPcwrSLLHUuqznWnf.CriticalError:
			SQEpRsYtlcQSDolfqfkmipiOGkcD = 1;
			break;
		}
		return false;
	}

	private bool oBnFWPDWaNumEDAIqkVvRwmgfICT()
	{
		if (SQEpRsYtlcQSDolfqfkmipiOGkcD != 0)
		{
			if (SQEpRsYtlcQSDolfqfkmipiOGkcD == 1)
			{
				Logger.LogError("Error communicating with HID device. This device \"" + NWOAJRMWfHwYvDAsJepcyEclDEdV + "\" will not function.");
				SQEpRsYtlcQSDolfqfkmipiOGkcD = 2;
				try
				{
					PVniwZfInMoMNQdfTcxWaYtSqjYd.Stop(wait: false);
				}
				catch
				{
				}
			}
			return true;
		}
		return false;
	}

	public void Dispose()
	{
		DVFgGEHSnJBiuGzfDMYbkoOOOUGBb(true);
		GC.SuppressFinalize(this);
	}

	void IDisposable.Dispose()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Dispose
		this.Dispose();
	}

	protected virtual void IWwmWZBCBlxvxjwmYnTZLHMifuLT()
	{
		try
		{
			DVFgGEHSnJBiuGzfDMYbkoOOOUGBb(false);
		}
		finally
		{
			base.Finalize();
		}
	}

	protected virtual void DVFgGEHSnJBiuGzfDMYbkoOOOUGBb(bool P_0)
	{
		if (KLoYTfzGtkZrIHqyIdkGtVLecnH)
		{
			return;
		}
		if (P_0)
		{
			if (PVniwZfInMoMNQdfTcxWaYtSqjYd != null)
			{
				PVniwZfInMoMNQdfTcxWaYtSqjYd.Dispose();
			}
			if (ythRetlxzeqhYHUSyvOguXQHotnj != null)
			{
				ythRetlxzeqhYHUSyvOguXQHotnj.Dispose();
			}
		}
		KLoYTfzGtkZrIHqyIdkGtVLecnH = true;
	}
}
