using System;
using System.Threading;
using Rewired;
using Rewired.Utils;
using Rewired.Utils.Classes.Utility;

internal class ndmePBZINOpKdaHMvHWxzZcCpwyn : IDisposable
{
	public delegate void xsyVpaJBPcVaZJyQGhsynwNQJgNS(IntPtr reportPointer, int reportByteLength, int reportCount, double timestamp);

	private const int qmgHLoOqTiCcRVPodhbdGvnzgbmQA = 512;

	private const int hULByfsFRNqYYKqQnzvODdMJCVRH = 250;

	private readonly xsyVpaJBPcVaZJyQGhsynwNQJgNS npERdIqMBwgvDdeQBjWRpujnDZJDA;

	private readonly dXlctfYTaIJgQQIMXYbceYWYtRZE eUvTglKkMpvHfdvNgCBVXrHiRrwO;

	private readonly ThreadHelper JfvVzDxSnZQCYpMfzRpeANZVDaGz;

	private readonly int aYjtMarksUcoLaLCwYSusKVTQbkGA;

	private readonly int UicgRqcVZvODZBBEUPSprPSsPsBu;

	private readonly string gXRFkOegGkZLZEgryjAWxRYjkhqqA;

	private readonly byte[] xZHHRDkEmqYVCJiwOxtPrwLshreL;

	private readonly byte[] VRukyDjnvqiIVeVbCOAegJLAqtYL;

	private int cpZsxtQgPgcYyGRSJJUwuBoXcJaJA;

	private btTtcDDuGnqMmSyRYKBhUvGLqQwD DICfyPHtihXhIQgqRhWNBBNGqokp;

	private btTtcDDuGnqMmSyRYKBhUvGLqQwD ymhGURJfUkLAwkfoixSIcCqquuBd;

	private bool NchdYNbKzqsssgcQJdenZuGqXgLo;

	public ndmePBZINOpKdaHMvHWxzZcCpwyn(string P_0, int P_1, string P_2, xsyVpaJBPcVaZJyQGhsynwNQJgNS P_3)
	{
		if (string.IsNullOrEmpty(P_0))
		{
			throw new ArgumentNullException("hidDevicePath");
		}
		if (P_3 == null)
		{
			throw new ArgumentNullException("processReportDelegate");
		}
		UicgRqcVZvODZBBEUPSprPSsPsBu = P_1;
		if (UicgRqcVZvODZBBEUPSprPSsPsBu <= 0)
		{
			UicgRqcVZvODZBBEUPSprPSsPsBu = 512;
		}
		aYjtMarksUcoLaLCwYSusKVTQbkGA = P_1 + 8;
		gXRFkOegGkZLZEgryjAWxRYjkhqqA = P_2;
		npERdIqMBwgvDdeQBjWRpujnDZJDA = P_3;
		int num = aYjtMarksUcoLaLCwYSusKVTQbkGA * 60;
		if (num <= 0)
		{
			Logger.LogError("Invalid report buffer size. This device \"" + P_2 + "\" will not function.");
			throw new Exception();
		}
		try
		{
			eUvTglKkMpvHfdvNgCBVXrHiRrwO = new dXlctfYTaIJgQQIMXYbceYWYtRZE(P_0, P_1, 250);
		}
		catch (Exception)
		{
			Logger.LogError("Out of memory. This device \"" + P_2 + "\" will not function.");
			throw;
		}
		try
		{
			DICfyPHtihXhIQgqRhWNBBNGqokp = new btTtcDDuGnqMmSyRYKBhUvGLqQwD(num);
			ymhGURJfUkLAwkfoixSIcCqquuBd = new btTtcDDuGnqMmSyRYKBhUvGLqQwD(num);
			xZHHRDkEmqYVCJiwOxtPrwLshreL = new byte[aYjtMarksUcoLaLCwYSusKVTQbkGA];
			VRukyDjnvqiIVeVbCOAegJLAqtYL = new byte[aYjtMarksUcoLaLCwYSusKVTQbkGA];
		}
		catch (Exception)
		{
			Logger.LogError("Out of memory. This device \"" + P_2 + "\" will not function.");
			throw;
		}
		try
		{
			JfvVzDxSnZQCYpMfzRpeANZVDaGz = ThreadHelper.Create();
			JfvVzDxSnZQCYpMfzRpeANZVDaGz.ThreadUpdateEvent += vtJjkpbMGzWoCuAWhQbXIkbbgcuU;
			JfvVzDxSnZQCYpMfzRpeANZVDaGz.Start(wait: false);
		}
		catch (Exception)
		{
			Logger.LogError("Error creating thread. This device \"" + P_2 + "\" will not function.");
			throw;
		}
	}

	public unsafe void mPVLsAWcaTKfbIPORRXexWhffTDm()
	{
		try
		{
			if (rrduSlBTBelAQQvqqbJXnJlZahtb())
			{
				return;
			}
			iJVBDhuVldFcOWXKpegOVehItSZu();
			int num = 0;
			byte[] array = xZHHRDkEmqYVCJiwOxtPrwLshreL;
			fixed (byte* ptr = array)
			{
				while (DICfyPHtihXhIQgqRhWNBBNGqokp.pkpJIXUPRvEEdtemqnOqHyayGnzb(array, aYjtMarksUcoLaLCwYSusKVTQbkGA) > 0)
				{
					npERdIqMBwgvDdeQBjWRpujnDZJDA((IntPtr)ptr, UicgRqcVZvODZBBEUPSprPSsPsBu, 1, *(double*)(ptr + UicgRqcVZvODZBBEUPSprPSsPsBu));
					num++;
				}
			}
		}
		catch
		{
		}
	}

	private void iJVBDhuVldFcOWXKpegOVehItSZu()
	{
		lock (DICfyPHtihXhIQgqRhWNBBNGqokp)
		{
			lock (ymhGURJfUkLAwkfoixSIcCqquuBd)
			{
				MiscTools.Swap(ref DICfyPHtihXhIQgqRhWNBBNGqokp, ref ymhGURJfUkLAwkfoixSIcCqquuBd);
			}
		}
	}

	private void vtJjkpbMGzWoCuAWhQbXIkbbgcuU()
	{
		if (cpZsxtQgPgcYyGRSJJUwuBoXcJaJA != 0)
		{
			Thread.Sleep(500);
			return;
		}
		try
		{
			byte[] vRukyDjnvqiIVeVbCOAegJLAqtYL = VRukyDjnvqiIVeVbCOAegJLAqtYL;
			if (!JxTDGRFivoAbnpauyqpEFmsHdspf(vRukyDjnvqiIVeVbCOAegJLAqtYL))
			{
				return;
			}
			lock (ymhGURJfUkLAwkfoixSIcCqquuBd)
			{
				ymhGURJfUkLAwkfoixSIcCqquuBd.KjttXAEzsRtECEzfsLykXNbscFEq(vRukyDjnvqiIVeVbCOAegJLAqtYL, vRukyDjnvqiIVeVbCOAegJLAqtYL.Length);
			}
		}
		catch
		{
		}
	}

	private bool JxTDGRFivoAbnpauyqpEFmsHdspf(byte[] P_0)
	{
		switch (eUvTglKkMpvHfdvNgCBVXrHiRrwO.pkpJIXUPRvEEdtemqnOqHyayGnzb(P_0))
		{
		case dXlctfYTaIJgQQIMXYbceYWYtRZE.tVvrxFHOVpLzqIiIrdPzxthuGXVj.Success:
			return true;
		case dXlctfYTaIJgQQIMXYbceYWYtRZE.tVvrxFHOVpLzqIiIrdPzxthuGXVj.Error:
			Thread.Sleep(500);
			break;
		case dXlctfYTaIJgQQIMXYbceYWYtRZE.tVvrxFHOVpLzqIiIrdPzxthuGXVj.CriticalError:
			cpZsxtQgPgcYyGRSJJUwuBoXcJaJA = 1;
			break;
		}
		return false;
	}

	private bool rrduSlBTBelAQQvqqbJXnJlZahtb()
	{
		if (cpZsxtQgPgcYyGRSJJUwuBoXcJaJA != 0)
		{
			if (cpZsxtQgPgcYyGRSJJUwuBoXcJaJA == 1)
			{
				Logger.LogError("Error communicating with HID device. This device \"" + gXRFkOegGkZLZEgryjAWxRYjkhqqA + "\" will not function.");
				cpZsxtQgPgcYyGRSJJUwuBoXcJaJA = 2;
				try
				{
					JfvVzDxSnZQCYpMfzRpeANZVDaGz.Stop(wait: false);
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

	protected virtual void lDxnsjCDTQrmresvWgbliNUVruIc(bool P_0)
	{
		if (NchdYNbKzqsssgcQJdenZuGqXgLo)
		{
			return;
		}
		if (P_0)
		{
			if (JfvVzDxSnZQCYpMfzRpeANZVDaGz != null)
			{
				JfvVzDxSnZQCYpMfzRpeANZVDaGz.Dispose();
			}
			if (eUvTglKkMpvHfdvNgCBVXrHiRrwO != null)
			{
				eUvTglKkMpvHfdvNgCBVXrHiRrwO.Dispose();
			}
		}
		NchdYNbKzqsssgcQJdenZuGqXgLo = true;
	}
}
