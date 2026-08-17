using System;
using System.Collections.Generic;
using System.Threading;
using Rewired;
using Rewired.Utils;

internal class gxiqikdujHobFnpuuAGPldgYhOvdA<_0001>
{
	private enum rUpGIVqqRfQthevNTKfPPjADktCI
	{
		Idle = 0,
		AwaitingResult = 1,
		ResultReceived = 2
	}

	private sealed class xSlQKqOAhXLJTDlwvyKCGHehGKLFA
	{
		private class ntytBNqxSoxWKXMwBsuzornkkXvD : IDisposable
		{
			private sealed class xVxbZzKqQgNtzZVZrrncSgZTDWyi
			{
				public ntytBNqxSoxWKXMwBsuzornkkXvD fjLwwvpiLifeQVNDncqaWwPnQHgH;

				public ManualResetEvent HYyPCuufEYlfbGChcFshMgeUKivF;

				internal void ZHvGbSgfJTmLsnVTCrrwEapleFjHA()
				{
					HYyPCuufEYlfbGChcFshMgeUKivF.Set();
					fjLwwvpiLifeQVNDncqaWwPnQHgH.tnCMEescxwjFUUtXwTPSCAhUPMSo();
				}
			}

			private readonly object RiBDvskkQjFxfkkSTnZBmjaSJGbP;

			private List<WaitCallback> IbCaHzhAoKdEkfBbeCJLgXJbLJmbB;

			private List<WaitCallback> TJTgyhIGHKJndWkiLAXobQNJnuPZA;

			private Thread PgKTUSczmxAohGdpToIrGFqHKSCO;

			private AutoResetEvent ZUsKVGHqbRMRRvtHfHLoVxpxHEbr;

			private bool yuXqAnTqrsVYudPdvhdPHTkVbnIN;

			private bool UcyOuvkXJMoZwQnRrmyjzAvOXdbJ;

			private bool hZZzsYgjSOFxwGfxHEJrtFXQCImq;

			private bool zynyXAcZyazvtaCFCKdBJGVwpHWo;

			public ntytBNqxSoxWKXMwBsuzornkkXvD()
			{
				RiBDvskkQjFxfkkSTnZBmjaSJGbP = new object();
				IbCaHzhAoKdEkfBbeCJLgXJbLJmbB = new List<WaitCallback>();
				TJTgyhIGHKJndWkiLAXobQNJnuPZA = new List<WaitCallback>();
				ZUsKVGHqbRMRRvtHfHLoVxpxHEbr = new AutoResetEvent(initialState: false);
			}

			public void VIBICPngjieouCDGwijDjrEhxnrtA(WaitCallback P_0)
			{
				if (JHBuhbmoThtPKiFmWLlbOKmBhFYy())
				{
					if (P_0 == null)
					{
						throw new ArgumentNullException("waitCallback");
					}
					lock (RiBDvskkQjFxfkkSTnZBmjaSJGbP)
					{
						IbCaHzhAoKdEkfBbeCJLgXJbLJmbB.Add(P_0);
					}
					ZUsKVGHqbRMRRvtHfHLoVxpxHEbr.Set();
				}
			}

			private bool JHBuhbmoThtPKiFmWLlbOKmBhFYy()
			{
				if (hZZzsYgjSOFxwGfxHEJrtFXQCImq)
				{
					return false;
				}
				if (UcyOuvkXJMoZwQnRrmyjzAvOXdbJ)
				{
					return false;
				}
				if (yuXqAnTqrsVYudPdvhdPHTkVbnIN)
				{
					return true;
				}
				if (PgKTUSczmxAohGdpToIrGFqHKSCO != null)
				{
					return true;
				}
				return xTPvwElaLrMIZrdfHWvWaPyBeOFg();
			}

			private bool xTPvwElaLrMIZrdfHWvWaPyBeOFg()
			{
				xVxbZzKqQgNtzZVZrrncSgZTDWyi xVxbZzKqQgNtzZVZrrncSgZTDWyi2 = new xVxbZzKqQgNtzZVZrrncSgZTDWyi();
				xVxbZzKqQgNtzZVZrrncSgZTDWyi2.fjLwwvpiLifeQVNDncqaWwPnQHgH = this;
				try
				{
					xVxbZzKqQgNtzZVZrrncSgZTDWyi2.HYyPCuufEYlfbGChcFshMgeUKivF = new ManualResetEvent(initialState: false);
					PgKTUSczmxAohGdpToIrGFqHKSCO = new Thread(xVxbZzKqQgNtzZVZrrncSgZTDWyi2.ZHvGbSgfJTmLsnVTCrrwEapleFjHA);
					PgKTUSczmxAohGdpToIrGFqHKSCO.Start();
					xVxbZzKqQgNtzZVZrrncSgZTDWyi2.HYyPCuufEYlfbGChcFshMgeUKivF.WaitOne();
					return true;
				}
				catch (Exception ex)
				{
					Logger.LogError("An exception occurred trying to initialize the thread pool.\n" + ex, requiredThreadSafety: true);
					PgKTUSczmxAohGdpToIrGFqHKSCO = null;
					hZZzsYgjSOFxwGfxHEJrtFXQCImq = true;
					return false;
				}
			}

			private void tnCMEescxwjFUUtXwTPSCAhUPMSo()
			{
				yuXqAnTqrsVYudPdvhdPHTkVbnIN = true;
				while (!UcyOuvkXJMoZwQnRrmyjzAvOXdbJ)
				{
					ZUsKVGHqbRMRRvtHfHLoVxpxHEbr.WaitOne();
					if (UcyOuvkXJMoZwQnRrmyjzAvOXdbJ)
					{
						break;
					}
					lock (RiBDvskkQjFxfkkSTnZBmjaSJGbP)
					{
						MiscTools.Swap(ref IbCaHzhAoKdEkfBbeCJLgXJbLJmbB, ref TJTgyhIGHKJndWkiLAXobQNJnuPZA);
					}
					List<WaitCallback> tJTgyhIGHKJndWkiLAXobQNJnuPZA = TJTgyhIGHKJndWkiLAXobQNJnuPZA;
					int count = tJTgyhIGHKJndWkiLAXobQNJnuPZA.Count;
					if (count == 0)
					{
						continue;
					}
					for (int i = 0; i < count; i++)
					{
						try
						{
							tJTgyhIGHKJndWkiLAXobQNJnuPZA[i](null);
						}
						catch (Exception ex)
						{
							Logger.LogError("Exception occurred in thread pool callback.\n" + ex, requiredThreadSafety: true);
						}
					}
					tJTgyhIGHKJndWkiLAXobQNJnuPZA.Clear();
				}
				lock (RiBDvskkQjFxfkkSTnZBmjaSJGbP)
				{
					IbCaHzhAoKdEkfBbeCJLgXJbLJmbB.Clear();
					TJTgyhIGHKJndWkiLAXobQNJnuPZA.Clear();
				}
				UcyOuvkXJMoZwQnRrmyjzAvOXdbJ = false;
				yuXqAnTqrsVYudPdvhdPHTkVbnIN = false;
			}

			private void quUfRYIKWFMlzuGroJeDTHbumZFnA()
			{
				PgKTUSczmxAohGdpToIrGFqHKSCO = null;
				hZZzsYgjSOFxwGfxHEJrtFXQCImq = false;
				UcyOuvkXJMoZwQnRrmyjzAvOXdbJ = true;
			}

			private void kvMHHJsdmzXczOoOBfCqhrJKxbvJ()
			{
				quUfRYIKWFMlzuGroJeDTHbumZFnA();
				try
				{
					ZUsKVGHqbRMRRvtHfHLoVxpxHEbr.Set();
				}
				catch (ObjectDisposedException)
				{
				}
			}

			public void Dispose()
			{
				HMiGQjIrmevipRQXHuAkWYzsXOcjA(true);
				GC.SuppressFinalize(this);
			}

			void IDisposable.Dispose()
			{
				//ILSpy generated this explicit interface implementation from .override directive in Dispose
				this.Dispose();
			}

			protected virtual void HEeFMuKzMdDDXlnaJwWQliyWPlvW()
			{
				try
				{
					HMiGQjIrmevipRQXHuAkWYzsXOcjA(false);
				}
				finally
				{
					base.Finalize();
				}
			}

			protected virtual void HMiGQjIrmevipRQXHuAkWYzsXOcjA(bool P_0)
			{
				if (!zynyXAcZyazvtaCFCKdBJGVwpHWo)
				{
					kvMHHJsdmzXczOoOBfCqhrJKxbvJ();
					zynyXAcZyazvtaCFCKdBJGVwpHWo = true;
				}
			}
		}

		private static xSlQKqOAhXLJTDlwvyKCGHehGKLFA MLzvrVMQsidoMxsWUDNzKqQCJyKr;

		private ntytBNqxSoxWKXMwBsuzornkkXvD XURYhnvTgJDCRWtDEgnVuITkStNO;

		private int VxFMTNwuFPQaSdccdQvELLWFcLaI;

		private bool QYtNHdebcUbSRrHEvkMlspYTgIsIA;

		private static xSlQKqOAhXLJTDlwvyKCGHehGKLFA fwbfsgKMXmJOKEhSBSAoBTXWhohIb => MLzvrVMQsidoMxsWUDNzKqQCJyKr ?? new xSlQKqOAhXLJTDlwvyKCGHehGKLFA();

		private ntytBNqxSoxWKXMwBsuzornkkXvD ZyJbqRInErQXRILNPCplKtjYsgdD => XURYhnvTgJDCRWtDEgnVuITkStNO ?? (XURYhnvTgJDCRWtDEgnVuITkStNO = new ntytBNqxSoxWKXMwBsuzornkkXvD());

		private xSlQKqOAhXLJTDlwvyKCGHehGKLFA()
		{
			MLzvrVMQsidoMxsWUDNzKqQCJyKr?.xCnWxIyzQCIsumVTqYEokdEfmAMk();
			MLzvrVMQsidoMxsWUDNzKqQCJyKr = this;
		}

		private void kxfDtTRjeSNcHTwfgZfQhfINqXCG()
		{
			VxFMTNwuFPQaSdccdQvELLWFcLaI++;
		}

		private void lpEPbuaCklstwjwvEVbBssvnffce()
		{
			VxFMTNwuFPQaSdccdQvELLWFcLaI--;
			if (VxFMTNwuFPQaSdccdQvELLWFcLaI < 0)
			{
				Logger.LogError("SharedThread: Too many calls to Unregister.", requiredThreadSafety: true);
			}
			if (VxFMTNwuFPQaSdccdQvELLWFcLaI == 0)
			{
				xCnWxIyzQCIsumVTqYEokdEfmAMk();
			}
		}

		private void JTVUSdCdoinkMyXyPkHsvEXVhdBE(WaitCallback P_0)
		{
			ZyJbqRInErQXRILNPCplKtjYsgdD.VIBICPngjieouCDGwijDjrEhxnrtA(P_0);
		}

		private void xCnWxIyzQCIsumVTqYEokdEfmAMk()
		{
			lFjfnoBOHXiQvLniLNKHkOGIvyEe(true);
			GC.SuppressFinalize(this);
		}

		protected void lBIlejwpPINxICiNRmKeUhLRtKqu()
		{
			try
			{
				lFjfnoBOHXiQvLniLNKHkOGIvyEe(false);
			}
			finally
			{
				base.Finalize();
			}
		}

		private void lFjfnoBOHXiQvLniLNKHkOGIvyEe(bool P_0)
		{
			if (!QYtNHdebcUbSRrHEvkMlspYTgIsIA)
			{
				if (P_0 && XURYhnvTgJDCRWtDEgnVuITkStNO != null)
				{
					XURYhnvTgJDCRWtDEgnVuITkStNO.Dispose();
					XURYhnvTgJDCRWtDEgnVuITkStNO = null;
				}
				VxFMTNwuFPQaSdccdQvELLWFcLaI = 0;
				if (MLzvrVMQsidoMxsWUDNzKqQCJyKr == this)
				{
					MLzvrVMQsidoMxsWUDNzKqQCJyKr = null;
				}
				QYtNHdebcUbSRrHEvkMlspYTgIsIA = true;
			}
		}

		public static void zeIUgzWeFYjjUDHLwORchafZkeRL()
		{
			fwbfsgKMXmJOKEhSBSAoBTXWhohIb.kxfDtTRjeSNcHTwfgZfQhfINqXCG();
		}

		public static void vPpERdAkBywOuvveYFZJLLwpKuMdA()
		{
			MLzvrVMQsidoMxsWUDNzKqQCJyKr?.lpEPbuaCklstwjwvEVbBssvnffce();
		}

		public static void hcRdTMcvNSjDZQSZlDqKCDhkCAzaA(WaitCallback P_0)
		{
			fwbfsgKMXmJOKEhSBSAoBTXWhohIb.JTVUSdCdoinkMyXyPkHsvEXVhdBE(P_0);
		}
	}

	private rUpGIVqqRfQthevNTKfPPjADktCI KtmXkFOyfIndJgxvVOPgRYjMENJn;

	private _0001 VGyKBBZSIcOnodEBefZIinaOtnPQ;

	private WaitCallback WquGUwOnohdoHWRRLdkFMeXQjImE;

	private object NIVDiBEowKgfIxErrJnNDPNyytgm;

	private Func<_0001> oEAkYcClRRuelBkKaWOKiqXRzPlx;

	private bool MnhEbQCsVHMMSFUEgBzbeOaLLTlCc;

	private bool yPCfCwPsOApamlfVaYxKfsYdWTBc;

	public bool LYkqHsINOrRmqDzoBVFvzhFfDHwO
	{
		get
		{
			if (KtmXkFOyfIndJgxvVOPgRYjMENJn != rUpGIVqqRfQthevNTKfPPjADktCI.AwaitingResult)
			{
				return KtmXkFOyfIndJgxvVOPgRYjMENJn == rUpGIVqqRfQthevNTKfPPjADktCI.ResultReceived;
			}
			return true;
		}
	}

	public _0001 JNANzwwQdbCSWIOPljErVgGJvfwO => VGyKBBZSIcOnodEBefZIinaOtnPQ;

	public bool XrInaWDtMKIvqgrHsTdTFhcYalgT()
	{
		bool num = KtmXkFOyfIndJgxvVOPgRYjMENJn == rUpGIVqqRfQthevNTKfPPjADktCI.ResultReceived;
		if (num)
		{
			KtmXkFOyfIndJgxvVOPgRYjMENJn = rUpGIVqqRfQthevNTKfPPjADktCI.Idle;
		}
		return num;
	}

	public gxiqikdujHobFnpuuAGPldgYhOvdA(bool P_0, Func<_0001> P_1)
	{
		MnhEbQCsVHMMSFUEgBzbeOaLLTlCc = P_0;
		if (P_1 == null)
		{
			throw new ArgumentNullException("resultDelegate");
		}
		oEAkYcClRRuelBkKaWOKiqXRzPlx = P_1;
		WquGUwOnohdoHWRRLdkFMeXQjImE = SfmDdBkHsUdRuuDyPZkWUdINyMSf;
		NIVDiBEowKgfIxErrJnNDPNyytgm = new object();
		KtmXkFOyfIndJgxvVOPgRYjMENJn = rUpGIVqqRfQthevNTKfPPjADktCI.Idle;
		if (P_0)
		{
			xSlQKqOAhXLJTDlwvyKCGHehGKLFA.zeIUgzWeFYjjUDHLwORchafZkeRL();
		}
	}

	public bool dLArMTCEuzcNhWTwyvoEESZrqiGi()
	{
		lock (NIVDiBEowKgfIxErrJnNDPNyytgm)
		{
			if (KtmXkFOyfIndJgxvVOPgRYjMENJn == rUpGIVqqRfQthevNTKfPPjADktCI.AwaitingResult)
			{
				return false;
			}
			VGyKBBZSIcOnodEBefZIinaOtnPQ = default(_0001);
			KtmXkFOyfIndJgxvVOPgRYjMENJn = rUpGIVqqRfQthevNTKfPPjADktCI.AwaitingResult;
		}
		if (MnhEbQCsVHMMSFUEgBzbeOaLLTlCc)
		{
			xSlQKqOAhXLJTDlwvyKCGHehGKLFA.hcRdTMcvNSjDZQSZlDqKCDhkCAzaA(WquGUwOnohdoHWRRLdkFMeXQjImE);
		}
		else
		{
			ThreadPool.QueueUserWorkItem(WquGUwOnohdoHWRRLdkFMeXQjImE, this);
		}
		return true;
	}

	public void LmbHGGTdiCfvkXltfXtklzlyoGD()
	{
		lock (NIVDiBEowKgfIxErrJnNDPNyytgm)
		{
			VGyKBBZSIcOnodEBefZIinaOtnPQ = default(_0001);
			KtmXkFOyfIndJgxvVOPgRYjMENJn = rUpGIVqqRfQthevNTKfPPjADktCI.Idle;
		}
	}

	private void SfmDdBkHsUdRuuDyPZkWUdINyMSf(object P_0)
	{
		lock (NIVDiBEowKgfIxErrJnNDPNyytgm)
		{
			if (KtmXkFOyfIndJgxvVOPgRYjMENJn == rUpGIVqqRfQthevNTKfPPjADktCI.AwaitingResult)
			{
				VGyKBBZSIcOnodEBefZIinaOtnPQ = oEAkYcClRRuelBkKaWOKiqXRzPlx();
				KtmXkFOyfIndJgxvVOPgRYjMENJn = rUpGIVqqRfQthevNTKfPPjADktCI.ResultReceived;
			}
		}
	}

	public void WvLYkoCkCYjVtJGzHiicpMYRreXw()
	{
		TLVZukHBsUEQCckkkkjOpJeOxwQW(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void pngyQolDyGtrhzeFQCuACTvzZQwdA()
	{
		try
		{
			TLVZukHBsUEQCckkkkjOpJeOxwQW(false);
		}
		finally
		{
			base.Finalize();
		}
	}

	protected virtual void TLVZukHBsUEQCckkkkjOpJeOxwQW(bool P_0)
	{
		if (!yPCfCwPsOApamlfVaYxKfsYdWTBc)
		{
			if (P_0)
			{
				LmbHGGTdiCfvkXltfXtklzlyoGD();
			}
			if (MnhEbQCsVHMMSFUEgBzbeOaLLTlCc)
			{
				xSlQKqOAhXLJTDlwvyKCGHehGKLFA.vPpERdAkBywOuvveYFZJLLwpKuMdA();
			}
			yPCfCwPsOApamlfVaYxKfsYdWTBc = true;
		}
	}
}
