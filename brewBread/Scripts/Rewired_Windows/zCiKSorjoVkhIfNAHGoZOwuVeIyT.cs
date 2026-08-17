using System;
using System.Collections.Generic;
using System.Threading;
using Rewired;
using Rewired.Utils;

internal class zCiKSorjoVkhIfNAHGoZOwuVeIyT<_0001>
{
	private enum whpWgNqMfvNIqejZeOsJozGImuJS
	{
		Idle = 0,
		AwaitingResult = 1,
		ResultReceived = 2
	}

	private sealed class mjruVmOamPiPKVmYKeeUatakPUQZA
	{
		private class oCwHZRmDRmPjNNTAkGYjJnbfVcqx : IDisposable
		{
			private sealed class jPvyqWFUbJHEvyCwsfZmeJDVLhjf
			{
				public oCwHZRmDRmPjNNTAkGYjJnbfVcqx GhFKJpFgmZouKwSNnkDGgmPjdPls;

				public ManualResetEvent vGJGLCobxrlqACjafcTmsrrQKoqU;

				internal void pTEmbRQLJcHGheunxTjvBPzeqhHJ()
				{
					vGJGLCobxrlqACjafcTmsrrQKoqU.Set();
					GhFKJpFgmZouKwSNnkDGgmPjdPls.FUFQKrznafREyWHUNbAbtebJvlKP();
				}
			}

			private readonly object ifhEwCkIVuTGOnfpubaTrgsqjFiQ;

			private List<WaitCallback> wsKYerpFPyGzidgkTciJHxoTzsyN;

			private List<WaitCallback> qavnEuCACfoaIoMSEEgjOxJQWezG;

			private Thread JfvVzDxSnZQCYpMfzRpeANZVDaGz;

			private AutoResetEvent zVyNVgOJOCKGAPjKSCAWGBhOvQgb;

			private bool KtPpLlPEIbFfGQAykznawQYsoNsA;

			private bool vOmgQpFpZDQkWejpaMYTXfZsWyMfB;

			private bool qoCfYrHBBsLeuRyKNyMRnYRiVKaIA;

			private bool NchdYNbKzqsssgcQJdenZuGqXgLo;

			public oCwHZRmDRmPjNNTAkGYjJnbfVcqx()
			{
				ifhEwCkIVuTGOnfpubaTrgsqjFiQ = new object();
				wsKYerpFPyGzidgkTciJHxoTzsyN = new List<WaitCallback>();
				qavnEuCACfoaIoMSEEgjOxJQWezG = new List<WaitCallback>();
				zVyNVgOJOCKGAPjKSCAWGBhOvQgb = new AutoResetEvent(initialState: false);
			}

			public void zlkqWFcglMAejJcHSErmFZifzcuMB(WaitCallback P_0)
			{
				if (iNjZxEWgGYMxgNiUyUmpWefCBkbC())
				{
					if (P_0 == null)
					{
						throw new ArgumentNullException("waitCallback");
					}
					lock (ifhEwCkIVuTGOnfpubaTrgsqjFiQ)
					{
						wsKYerpFPyGzidgkTciJHxoTzsyN.Add(P_0);
					}
					zVyNVgOJOCKGAPjKSCAWGBhOvQgb.Set();
				}
			}

			public void ubSyhizRlMmgRwtaMAVDgQvAGmzd()
			{
				RlsjDbwBNnHcrJshPjDecAYNLdvBA();
			}

			public bool hXtqHxopDjDyKrvrUxISDuLjcnbe()
			{
				return iNjZxEWgGYMxgNiUyUmpWefCBkbC();
			}

			private bool iNjZxEWgGYMxgNiUyUmpWefCBkbC()
			{
				jPvyqWFUbJHEvyCwsfZmeJDVLhjf jPvyqWFUbJHEvyCwsfZmeJDVLhjf2 = new jPvyqWFUbJHEvyCwsfZmeJDVLhjf();
				jPvyqWFUbJHEvyCwsfZmeJDVLhjf2.GhFKJpFgmZouKwSNnkDGgmPjdPls = this;
				if (qoCfYrHBBsLeuRyKNyMRnYRiVKaIA)
				{
					return false;
				}
				if (vOmgQpFpZDQkWejpaMYTXfZsWyMfB)
				{
					return false;
				}
				if (KtPpLlPEIbFfGQAykznawQYsoNsA)
				{
					return true;
				}
				if (JfvVzDxSnZQCYpMfzRpeANZVDaGz != null)
				{
					return true;
				}
				try
				{
					jPvyqWFUbJHEvyCwsfZmeJDVLhjf2.vGJGLCobxrlqACjafcTmsrrQKoqU = new ManualResetEvent(initialState: false);
					JfvVzDxSnZQCYpMfzRpeANZVDaGz = new Thread(jPvyqWFUbJHEvyCwsfZmeJDVLhjf2.pTEmbRQLJcHGheunxTjvBPzeqhHJ);
					JfvVzDxSnZQCYpMfzRpeANZVDaGz.Start();
					jPvyqWFUbJHEvyCwsfZmeJDVLhjf2.vGJGLCobxrlqACjafcTmsrrQKoqU.WaitOne();
					return true;
				}
				catch (Exception ex)
				{
					Logger.LogError("An exception occurred trying to initialize the thread pool.\n" + ex, requiredThreadSafety: true);
					JfvVzDxSnZQCYpMfzRpeANZVDaGz = null;
					qoCfYrHBBsLeuRyKNyMRnYRiVKaIA = true;
					return false;
				}
			}

			private void FUFQKrznafREyWHUNbAbtebJvlKP()
			{
				KtPpLlPEIbFfGQAykznawQYsoNsA = true;
				while (!vOmgQpFpZDQkWejpaMYTXfZsWyMfB)
				{
					zVyNVgOJOCKGAPjKSCAWGBhOvQgb.WaitOne();
					if (vOmgQpFpZDQkWejpaMYTXfZsWyMfB)
					{
						break;
					}
					lock (ifhEwCkIVuTGOnfpubaTrgsqjFiQ)
					{
						MiscTools.Swap(ref wsKYerpFPyGzidgkTciJHxoTzsyN, ref qavnEuCACfoaIoMSEEgjOxJQWezG);
					}
					List<WaitCallback> list = qavnEuCACfoaIoMSEEgjOxJQWezG;
					int count = list.Count;
					if (count == 0)
					{
						continue;
					}
					for (int i = 0; i < count; i++)
					{
						try
						{
							list[i](null);
						}
						catch (Exception ex)
						{
							Logger.LogError("Exception occurred in thread pool callback.\n" + ex, requiredThreadSafety: true);
						}
					}
					list.Clear();
				}
				lock (ifhEwCkIVuTGOnfpubaTrgsqjFiQ)
				{
					wsKYerpFPyGzidgkTciJHxoTzsyN.Clear();
					qavnEuCACfoaIoMSEEgjOxJQWezG.Clear();
				}
				vOmgQpFpZDQkWejpaMYTXfZsWyMfB = false;
				KtPpLlPEIbFfGQAykznawQYsoNsA = false;
			}

			private void BSXLhXftuKQllkPbJqfhimFPaEzU()
			{
				JfvVzDxSnZQCYpMfzRpeANZVDaGz = null;
				qoCfYrHBBsLeuRyKNyMRnYRiVKaIA = false;
				vOmgQpFpZDQkWejpaMYTXfZsWyMfB = true;
			}

			private void RlsjDbwBNnHcrJshPjDecAYNLdvBA()
			{
				BSXLhXftuKQllkPbJqfhimFPaEzU();
				try
				{
					zVyNVgOJOCKGAPjKSCAWGBhOvQgb.Set();
				}
				catch (ObjectDisposedException)
				{
				}
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
				if (!NchdYNbKzqsssgcQJdenZuGqXgLo)
				{
					RlsjDbwBNnHcrJshPjDecAYNLdvBA();
					NchdYNbKzqsssgcQJdenZuGqXgLo = true;
				}
			}
		}

		private static mjruVmOamPiPKVmYKeeUatakPUQZA CUMjiYgOsfRNPymJKKmVCqeHmAEM;

		private oCwHZRmDRmPjNNTAkGYjJnbfVcqx RoVAMBEROyITsncQELtUZxDwVfQe;

		private int cACtvzzrEVArAGDIFpSeAygeirOh;

		private bool NchdYNbKzqsssgcQJdenZuGqXgLo;

		private static mjruVmOamPiPKVmYKeeUatakPUQZA HVRiWsLCvmvdlCzibTjWzIjfSPzK => CUMjiYgOsfRNPymJKKmVCqeHmAEM ?? new mjruVmOamPiPKVmYKeeUatakPUQZA();

		private oCwHZRmDRmPjNNTAkGYjJnbfVcqx KtUklsxSYDJZxJQSNFslyzQyJfCj => RoVAMBEROyITsncQELtUZxDwVfQe ?? (RoVAMBEROyITsncQELtUZxDwVfQe = new oCwHZRmDRmPjNNTAkGYjJnbfVcqx());

		private mjruVmOamPiPKVmYKeeUatakPUQZA()
		{
			CUMjiYgOsfRNPymJKKmVCqeHmAEM?.lDxnsjCDTQrmresvWgbliNUVruIc();
			CUMjiYgOsfRNPymJKKmVCqeHmAEM = this;
		}

		private void yhQbbSBqXhCBVxaKeIKCqCIyinKuA()
		{
			cACtvzzrEVArAGDIFpSeAygeirOh++;
		}

		private void YQEEeDWkOzhyuIbbIAcIGekzbFyBA()
		{
			cACtvzzrEVArAGDIFpSeAygeirOh--;
			if (cACtvzzrEVArAGDIFpSeAygeirOh < 0)
			{
				Logger.LogError("SharedThread: Too many calls to Unregister.", requiredThreadSafety: true);
			}
			if (cACtvzzrEVArAGDIFpSeAygeirOh == 0)
			{
				lDxnsjCDTQrmresvWgbliNUVruIc();
			}
		}

		private void deyRafsrJDywYPTawqqfWIjMmGry(WaitCallback P_0)
		{
			KtUklsxSYDJZxJQSNFslyzQyJfCj.zlkqWFcglMAejJcHSErmFZifzcuMB(P_0);
		}

		private void kaSYVeNAizCBRzJTJRThIHXTLVRF()
		{
			KtUklsxSYDJZxJQSNFslyzQyJfCj.ubSyhizRlMmgRwtaMAVDgQvAGmzd();
		}

		private bool MkdPLOQFeggCLfjzsaaUCxKuPUQoA()
		{
			return KtUklsxSYDJZxJQSNFslyzQyJfCj.hXtqHxopDjDyKrvrUxISDuLjcnbe();
		}

		private void lDxnsjCDTQrmresvWgbliNUVruIc()
		{
			lDxnsjCDTQrmresvWgbliNUVruIc(true);
			GC.SuppressFinalize(this);
		}

		protected void zNJVymYugIbeeZuNgMrKxyYWbziV()
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

		private void lDxnsjCDTQrmresvWgbliNUVruIc(bool P_0)
		{
			if (!NchdYNbKzqsssgcQJdenZuGqXgLo)
			{
				if (P_0 && RoVAMBEROyITsncQELtUZxDwVfQe != null)
				{
					RoVAMBEROyITsncQELtUZxDwVfQe.Dispose();
					RoVAMBEROyITsncQELtUZxDwVfQe = null;
				}
				cACtvzzrEVArAGDIFpSeAygeirOh = 0;
				if (CUMjiYgOsfRNPymJKKmVCqeHmAEM == this)
				{
					CUMjiYgOsfRNPymJKKmVCqeHmAEM = null;
				}
				NchdYNbKzqsssgcQJdenZuGqXgLo = true;
			}
		}

		public static void HxihYqGtqvUDAetBGfqShAUoiLms()
		{
			HVRiWsLCvmvdlCzibTjWzIjfSPzK.yhQbbSBqXhCBVxaKeIKCqCIyinKuA();
		}

		public static void vpYIokYHvEMeYKpUDDFgmrWPzLSy()
		{
			CUMjiYgOsfRNPymJKKmVCqeHmAEM?.YQEEeDWkOzhyuIbbIAcIGekzbFyBA();
		}

		public static void zlkqWFcglMAejJcHSErmFZifzcuMB(WaitCallback P_0)
		{
			HVRiWsLCvmvdlCzibTjWzIjfSPzK.deyRafsrJDywYPTawqqfWIjMmGry(P_0);
		}
	}

	private whpWgNqMfvNIqejZeOsJozGImuJS YBVLHhuEMfAdNIdivBJanHRaJQaM;

	private _0001 TzxgiwAmsezBHlJhnNRRkIJteFpY;

	private WaitCallback SiXixQSMucyuJvNwhknxJlowTWAA;

	private object PdLebfCotqexuGgniZIhMvALVeSjB;

	private Func<_0001> WcOlqlWHdVZXMWMHBWLvgwsXCteu;

	private bool rEkAjSkJQsJIwYRoeExNyXIfzLep;

	private bool NchdYNbKzqsssgcQJdenZuGqXgLo;

	public bool ZkLDDCubwAduGHcHaTbJRgqUXFzZ
	{
		get
		{
			if (YBVLHhuEMfAdNIdivBJanHRaJQaM != whpWgNqMfvNIqejZeOsJozGImuJS.AwaitingResult)
			{
				return YBVLHhuEMfAdNIdivBJanHRaJQaM == whpWgNqMfvNIqejZeOsJozGImuJS.ResultReceived;
			}
			return true;
		}
	}

	public _0001 mvQRpybjrKbIrEJNHDrCeYnFFAAPc => TzxgiwAmsezBHlJhnNRRkIJteFpY;

	public bool JXQYCqxIFkDGZeLuFZlXEfMklrZtA()
	{
		bool num = YBVLHhuEMfAdNIdivBJanHRaJQaM == whpWgNqMfvNIqejZeOsJozGImuJS.ResultReceived;
		if (num)
		{
			YBVLHhuEMfAdNIdivBJanHRaJQaM = whpWgNqMfvNIqejZeOsJozGImuJS.Idle;
		}
		return num;
	}

	public zCiKSorjoVkhIfNAHGoZOwuVeIyT(bool P_0, Func<_0001> P_1)
	{
		rEkAjSkJQsJIwYRoeExNyXIfzLep = P_0;
		if (P_1 == null)
		{
			throw new ArgumentNullException("resultDelegate");
		}
		WcOlqlWHdVZXMWMHBWLvgwsXCteu = P_1;
		SiXixQSMucyuJvNwhknxJlowTWAA = YnXqJKSXpGAWUEZqqLOFfvlYcBEw;
		PdLebfCotqexuGgniZIhMvALVeSjB = new object();
		YBVLHhuEMfAdNIdivBJanHRaJQaM = whpWgNqMfvNIqejZeOsJozGImuJS.Idle;
		if (P_0)
		{
			mjruVmOamPiPKVmYKeeUatakPUQZA.HxihYqGtqvUDAetBGfqShAUoiLms();
		}
	}

	public bool uRxHPttoThKrBNCurCuvwPhMcGUfA()
	{
		lock (PdLebfCotqexuGgniZIhMvALVeSjB)
		{
			if (YBVLHhuEMfAdNIdivBJanHRaJQaM == whpWgNqMfvNIqejZeOsJozGImuJS.AwaitingResult)
			{
				return false;
			}
			TzxgiwAmsezBHlJhnNRRkIJteFpY = default(_0001);
			YBVLHhuEMfAdNIdivBJanHRaJQaM = whpWgNqMfvNIqejZeOsJozGImuJS.AwaitingResult;
		}
		if (rEkAjSkJQsJIwYRoeExNyXIfzLep)
		{
			mjruVmOamPiPKVmYKeeUatakPUQZA.zlkqWFcglMAejJcHSErmFZifzcuMB(SiXixQSMucyuJvNwhknxJlowTWAA);
		}
		else
		{
			ThreadPool.QueueUserWorkItem(SiXixQSMucyuJvNwhknxJlowTWAA, this);
		}
		return true;
	}

	public void ZrbFhGEbWRbTzVxxQimUntnkwisKA()
	{
		lock (PdLebfCotqexuGgniZIhMvALVeSjB)
		{
			TzxgiwAmsezBHlJhnNRRkIJteFpY = default(_0001);
			YBVLHhuEMfAdNIdivBJanHRaJQaM = whpWgNqMfvNIqejZeOsJozGImuJS.Idle;
		}
	}

	private void YnXqJKSXpGAWUEZqqLOFfvlYcBEw(object P_0)
	{
		lock (PdLebfCotqexuGgniZIhMvALVeSjB)
		{
			if (YBVLHhuEMfAdNIdivBJanHRaJQaM == whpWgNqMfvNIqejZeOsJozGImuJS.AwaitingResult)
			{
				TzxgiwAmsezBHlJhnNRRkIJteFpY = WcOlqlWHdVZXMWMHBWLvgwsXCteu();
				YBVLHhuEMfAdNIdivBJanHRaJQaM = whpWgNqMfvNIqejZeOsJozGImuJS.ResultReceived;
			}
		}
	}

	public void lDxnsjCDTQrmresvWgbliNUVruIc()
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
		if (!NchdYNbKzqsssgcQJdenZuGqXgLo)
		{
			if (P_0)
			{
				ZrbFhGEbWRbTzVxxQimUntnkwisKA();
			}
			if (rEkAjSkJQsJIwYRoeExNyXIfzLep)
			{
				mjruVmOamPiPKVmYKeeUatakPUQZA.vpYIokYHvEMeYKpUDDFgmrWPzLSy();
			}
			NchdYNbKzqsssgcQJdenZuGqXgLo = true;
		}
	}
}
