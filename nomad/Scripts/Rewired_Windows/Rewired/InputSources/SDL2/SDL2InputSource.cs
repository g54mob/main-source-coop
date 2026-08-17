using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Rewired.Config;
using Rewired.Interfaces;
using Rewired.Platforms;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;

namespace Rewired.InputSources.SDL2
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class SDL2InputSource : IInputSource, IDisposable
	{
		private bool NvaDFSSsEMOethDaxOoJiEWxXNwD;

		private bool GzIpgcIyRZSOUsQWVPStSCrxGxpq;

		private bool SjKqVRuOEMDkjeHtYUhkavzoGdhgA;

		private bool ZNShvOKkoYZWLfOrdCGwdGLGSbDc;

		private bool OUtHtUEbYUyLhXKfEWnCRAJIfGlk;

		private ADictionary<int, XGGQMyZVNBKhRaGnErwNnBmbPgkE> RnrfyFtWoZVUJQZOPSgTUGspWfBg;

		private ADictionary<int, fMAKGMfYqqmFRMCFdbAbLpznnXQL> xJccXZHsCeYdOFkUoUzIOIYVNarnA;

		private qbAgQjRXIPTuMaDgGyMMhTVDcNdG.dRCDXOlEIehBuiqEuMSCokARrlNgb cnTgKXISCeyRCiuLxapTdGTfaWkdA;

		private NativeBuffer JDctVvlsvLALDeWIYvSnBUGxhpWq;

		[CompilerGenerated]
		private Action JcDbePCxjnJKRiNXRQfxQMvUAqBZA;

		private bool ztTxyRQlkIMQNZGZSqIocFadIMOgA;

		private event Action _DeviceChangedEvent
		{
			[CompilerGenerated]
			add
			{
				Action action = JcDbePCxjnJKRiNXRQfxQMvUAqBZA;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, b);
					action = Interlocked.CompareExchange(ref JcDbePCxjnJKRiNXRQfxQMvUAqBZA, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = JcDbePCxjnJKRiNXRQfxQMvUAqBZA;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value3);
					action = Interlocked.CompareExchange(ref JcDbePCxjnJKRiNXRQfxQMvUAqBZA, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		event Action IInputSource.DeviceChangedEvent
		{
			add
			{
				_DeviceChangedEvent += value;
			}
			remove
			{
				_DeviceChangedEvent -= value;
			}
		}

		public SDL2InputSource(UpdateLoopSetting P_0, bool P_1, bool P_2, bool P_3, bool P_4)
		{
			NvaDFSSsEMOethDaxOoJiEWxXNwD = P_1;
			GzIpgcIyRZSOUsQWVPStSCrxGxpq = P_2;
			SjKqVRuOEMDkjeHtYUhkavzoGdhgA = P_3;
			ZNShvOKkoYZWLfOrdCGwdGLGSbDc = P_4;
			RnrfyFtWoZVUJQZOPSgTUGspWfBg = new ADictionary<int, XGGQMyZVNBKhRaGnErwNnBmbPgkE>();
			xJccXZHsCeYdOFkUoUzIOIYVNarnA = new ADictionary<int, fMAKGMfYqqmFRMCFdbAbLpznnXQL>();
			int num = ((!UnityTools.isEditor || UnityTools.editorPlatform != EditorPlatform.OSX) ? 29184 : 25088);
			try
			{
				qbAgQjRXIPTuMaDgGyMMhTVDcNdG.qBUCxFNizzRvwJVoRwfBaygLISvW(UnityTools.effectivePlatform);
				if (qbAgQjRXIPTuMaDgGyMMhTVDcNdG.RlvdlvsFVdBpVqqkvTfWDnGSDKqt((uint)num) < 0)
				{
					throw new Exception("Failed initialize SDL2!");
				}
				OUtHtUEbYUyLhXKfEWnCRAJIfGlk = true;
				if (P_2)
				{
					YdlfseJaCyQYCBYjyMeEqNaBUBxd();
				}
				EHSdngwuJEqyRRYixEhkZWkBIRfs();
				JDctVvlsvLALDeWIYvSnBUGxhpWq = new NativeBuffer(56);
			}
			catch
			{
				OUtHtUEbYUyLhXKfEWnCRAJIfGlk = false;
				Dispose();
				throw;
			}
		}

		public void SystemDeviceConnected()
		{
			throw new NotImplementedException();
		}

		void IInputSource.SystemDeviceConnected()
		{
			//ILSpy generated this explicit interface implementation from .override directive in SystemDeviceConnected
			this.SystemDeviceConnected();
		}

		public void SystemDeviceDisconnected()
		{
			throw new NotImplementedException();
		}

		void IInputSource.SystemDeviceDisconnected()
		{
			//ILSpy generated this explicit interface implementation from .override directive in SystemDeviceDisconnected
			this.SystemDeviceDisconnected();
		}

		public void Update()
		{
			_ = OUtHtUEbYUyLhXKfEWnCRAJIfGlk;
		}

		void IInputSource.Update()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Update
			this.Update();
		}

		public void UpdateDevices(UpdateLoopType updateLoop)
		{
			if (OUtHtUEbYUyLhXKfEWnCRAJIfGlk)
			{
				zGURNWjKPfSluhaQhCoIVODqXoBE();
			}
		}

		void IInputSource.UpdateDevices(UpdateLoopType updateLoop)
		{
			//ILSpy generated this explicit interface implementation from .override directive in UpdateDevices
			this.UpdateDevices(updateLoop);
		}

		public void UpdateFinished()
		{
			_ = OUtHtUEbYUyLhXKfEWnCRAJIfGlk;
		}

		void IInputSource.UpdateFinished()
		{
			//ILSpy generated this explicit interface implementation from .override directive in UpdateFinished
			this.UpdateFinished();
		}

		public IList<T> GetJoysticks<T>() where T : class
		{
			if (!OUtHtUEbYUyLhXKfEWnCRAJIfGlk)
			{
				return null;
			}
			List<MnsFyTzecaNZBeNkLkANtMeQRWvs> list = new List<MnsFyTzecaNZBeNkLkANtMeQRWvs>();
			if (NvaDFSSsEMOethDaxOoJiEWxXNwD)
			{
				foreach (KeyValuePair<int, XGGQMyZVNBKhRaGnErwNnBmbPgkE> item in RnrfyFtWoZVUJQZOPSgTUGspWfBg)
				{
					if (item.Value.wSKSFDMVyLjVDgLPjDCkFXMYWChOA)
					{
						list.Add(item.Value);
					}
				}
			}
			if (GzIpgcIyRZSOUsQWVPStSCrxGxpq)
			{
				foreach (KeyValuePair<int, fMAKGMfYqqmFRMCFdbAbLpznnXQL> item2 in xJccXZHsCeYdOFkUoUzIOIYVNarnA)
				{
					fMAKGMfYqqmFRMCFdbAbLpznnXQL value = item2.Value;
					if (value.wSKSFDMVyLjVDgLPjDCkFXMYWChOA)
					{
						list.Add(value);
					}
				}
			}
			return list as IList<T>;
		}

		IList<T> IInputSource.GetJoysticks<T>()
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetJoysticks
			return this.GetJoysticks<T>();
		}

		private int oHLpwtwFjCcVqUqqXcMJUIdFRSaE()
		{
			if (!OUtHtUEbYUyLhXKfEWnCRAJIfGlk)
			{
				return 0;
			}
			return Math.Min(qbAgQjRXIPTuMaDgGyMMhTVDcNdG.icDqidmbdvmKRWaypreivoypaXXW(), 32);
		}

		private XGGQMyZVNBKhRaGnErwNnBmbPgkE gAerpeOGNNbmPFPzgngXGEHoTyqLA(int P_0)
		{
			IntPtr intPtr = qbAgQjRXIPTuMaDgGyMMhTVDcNdG.qZzFEtwaVPbEiHKlUjQtILqBeoHLA(P_0);
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			OfHcchtNNgrqQpaRylQnzBsxMNFD ofHcchtNNgrqQpaRylQnzBsxMNFD = new OfHcchtNNgrqQpaRylQnzBsxMNFD(intPtr);
			ettUIVyZApbfOsMrELHoexHuUXcN ettUIVyZApbfOsMrELHoexHuUXcN2 = tUWMCgyIwxgtoHONWhmNCLqEtyHZ(P_0, ofHcchtNNgrqQpaRylQnzBsxMNFD);
			if (ettUIVyZApbfOsMrELHoexHuUXcN2 == null)
			{
				qbAgQjRXIPTuMaDgGyMMhTVDcNdG.TdrdxKgHjrwNcweBeXrcFsWTCCNG(intPtr);
				return null;
			}
			return new XGGQMyZVNBKhRaGnErwNnBmbPgkE(ofHcchtNNgrqQpaRylQnzBsxMNFD, ettUIVyZApbfOsMrELHoexHuUXcN2);
		}

		private fMAKGMfYqqmFRMCFdbAbLpznnXQL OcnUtFXTkcUQapZyGXCsJIQDQzBB(int P_0)
		{
			IntPtr intPtr = qbAgQjRXIPTuMaDgGyMMhTVDcNdG.ZxNdHOnXHGhRUtttHTufuTJWUMlo(P_0);
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			NmwHhSpefsDRqUaqZUiLoTTFNxKg nmwHhSpefsDRqUaqZUiLoTTFNxKg = new NmwHhSpefsDRqUaqZUiLoTTFNxKg(intPtr);
			ettUIVyZApbfOsMrELHoexHuUXcN ettUIVyZApbfOsMrELHoexHuUXcN2 = GbbEOCHDZenVZAysFsrqjrQeuowPB(P_0, nmwHhSpefsDRqUaqZUiLoTTFNxKg);
			if (ettUIVyZApbfOsMrELHoexHuUXcN2 == null)
			{
				return null;
			}
			if (!ettUIVyZApbfOsMrELHoexHuUXcN2.GLrCbizCVIdjJNQlrNzUXdTUyJYs)
			{
				qbAgQjRXIPTuMaDgGyMMhTVDcNdG.XckdhTfPyGmBCEBieIxsxGjhiAri(intPtr);
				return null;
			}
			ettUIVyZApbfOsMrELHoexHuUXcN2.MmmKcHslyLeNdcFDErupuZipZFY = qbAgQjRXIPTuMaDgGyMMhTVDcNdG.BFZJEgDfxLpioUGyAenzbalgdPMSA(nmwHhSpefsDRqUaqZUiLoTTFNxKg);
			return new fMAKGMfYqqmFRMCFdbAbLpznnXQL(nmwHhSpefsDRqUaqZUiLoTTFNxKg, ettUIVyZApbfOsMrELHoexHuUXcN2);
		}

		private ettUIVyZApbfOsMrELHoexHuUXcN tUWMCgyIwxgtoHONWhmNCLqEtyHZ(int P_0, OfHcchtNNgrqQpaRylQnzBsxMNFD P_1)
		{
			if (!OUtHtUEbYUyLhXKfEWnCRAJIfGlk)
			{
				return null;
			}
			if (P_0 < 0 || P_0 >= 32)
			{
				return null;
			}
			if (P_1 == null || !P_1.IsValid)
			{
				return null;
			}
			return new ettUIVyZApbfOsMrELHoexHuUXcN
			{
				DoWnkeloRxSemxvueBCZnmhLIqtS = P_0,
				KjRormcdGxhvdWbQmSpZaQRujKKN = qbAgQjRXIPTuMaDgGyMMhTVDcNdG.BFuideImFJwbsvxsZFHiEBhppCMcA(P_1),
				GLrCbizCVIdjJNQlrNzUXdTUyJYs = qbAgQjRXIPTuMaDgGyMMhTVDcNdG.NcSEgMcXoinkicCbQndMIGyJSxOmB(P_0),
				uATcQBVLkatxNzIoYaEaljmiwIhB = qbAgQjRXIPTuMaDgGyMMhTVDcNdG.oIHCupCHwgDKPXfiSnFnZNcWhSFV(P_1),
				uBQxBBQjIUIzUDjNIsbWycLGtKnt = qbAgQjRXIPTuMaDgGyMMhTVDcNdG.fpJOyIgXpyPQSzrYZGYahfOfZIRw(P_1),
				nPASzgADMvDUNpZroJVRAPDvtzpf = qbAgQjRXIPTuMaDgGyMMhTVDcNdG.AYOdKKLtPxIiQNhejvWVvCQQpeIo(P_0),
				kruIBIEKmDWLUjnMGjJUQFYlSNFx = qbAgQjRXIPTuMaDgGyMMhTVDcNdG.ZkKuQSwWOmFnjtznQuGCAxYFVBED(P_1),
				QRGPSUGAgdaBSsLOqeCkDiNnMMdGb = qbAgQjRXIPTuMaDgGyMMhTVDcNdG.zPkxNzcSjMuskTBKMZaRiEsxeyVbA(P_1),
				gARdLCOFuGIqRIoDvTCsYmNSZuAM = qbAgQjRXIPTuMaDgGyMMhTVDcNdG.vVfNfOvdaAxLgoYCrTwXHaiArjty(P_1),
				KvDEREhlzoVOVCaKPhQeEuSqmmZU = qbAgQjRXIPTuMaDgGyMMhTVDcNdG.deqCJDEfqDHMjRwmPYjpfkjzKrfy(P_1)
			};
		}

		private ettUIVyZApbfOsMrELHoexHuUXcN GbbEOCHDZenVZAysFsrqjrQeuowPB(int P_0, NmwHhSpefsDRqUaqZUiLoTTFNxKg P_1)
		{
			if (P_1 == null || !P_1.IsValid)
			{
				return null;
			}
			OfHcchtNNgrqQpaRylQnzBsxMNFD ofHcchtNNgrqQpaRylQnzBsxMNFD = new OfHcchtNNgrqQpaRylQnzBsxMNFD(qbAgQjRXIPTuMaDgGyMMhTVDcNdG.jDcoLPuBYefeHAsfMLyAVjrIXFhr(P_1));
			if (!ofHcchtNNgrqQpaRylQnzBsxMNFD.IsValid)
			{
				return null;
			}
			return tUWMCgyIwxgtoHONWhmNCLqEtyHZ(P_0, ofHcchtNNgrqQpaRylQnzBsxMNFD);
		}

		private void EHSdngwuJEqyRRYixEhkZWkBIRfs()
		{
			for (int i = 0; i < oHLpwtwFjCcVqUqqXcMJUIdFRSaE(); i++)
			{
				if (NvaDFSSsEMOethDaxOoJiEWxXNwD)
				{
					ecyUsQThWqdZgHDxWZtlqSDhawhm(i);
				}
				if (GzIpgcIyRZSOUsQWVPStSCrxGxpq)
				{
					JyBuMGSbLJJeEGwFWvFDMgifOCGP(i);
				}
			}
		}

		private void NSCsifByDSiKRRMemxlcvGCNIUYBA()
		{
			if (GzIpgcIyRZSOUsQWVPStSCrxGxpq)
			{
				foreach (KeyValuePair<int, fMAKGMfYqqmFRMCFdbAbLpznnXQL> item in xJccXZHsCeYdOFkUoUzIOIYVNarnA)
				{
					fMAKGMfYqqmFRMCFdbAbLpznnXQL value = item.Value;
					value.LgoasxmQxWSQstHVwLCqZHNyBzhk();
					value.Dispose();
				}
				xJccXZHsCeYdOFkUoUzIOIYVNarnA.Clear();
			}
			if (!NvaDFSSsEMOethDaxOoJiEWxXNwD)
			{
				return;
			}
			foreach (KeyValuePair<int, XGGQMyZVNBKhRaGnErwNnBmbPgkE> item2 in RnrfyFtWoZVUJQZOPSgTUGspWfBg)
			{
				XGGQMyZVNBKhRaGnErwNnBmbPgkE value2 = item2.Value;
				value2.LgoasxmQxWSQstHVwLCqZHNyBzhk();
				value2.Dispose();
			}
			RnrfyFtWoZVUJQZOPSgTUGspWfBg.Clear();
		}

		private bool ecyUsQThWqdZgHDxWZtlqSDhawhm(int P_0)
		{
			if (P_0 < 0 || P_0 >= 32)
			{
				return false;
			}
			if (GzIpgcIyRZSOUsQWVPStSCrxGxpq && qbAgQjRXIPTuMaDgGyMMhTVDcNdG.NcSEgMcXoinkicCbQndMIGyJSxOmB(P_0))
			{
				return false;
			}
			XGGQMyZVNBKhRaGnErwNnBmbPgkE xGGQMyZVNBKhRaGnErwNnBmbPgkE = gAerpeOGNNbmPFPzgngXGEHoTyqLA(P_0);
			if (xGGQMyZVNBKhRaGnErwNnBmbPgkE == null)
			{
				return false;
			}
			int ifpVnuiAcuCRPvvZrdwkbEinzjysA = xGGQMyZVNBKhRaGnErwNnBmbPgkE.IfpVnuiAcuCRPvvZrdwkbEinzjysA;
			if (RnrfyFtWoZVUJQZOPSgTUGspWfBg.ContainsKey(ifpVnuiAcuCRPvvZrdwkbEinzjysA))
			{
				RnrfyFtWoZVUJQZOPSgTUGspWfBg[ifpVnuiAcuCRPvvZrdwkbEinzjysA].LgoasxmQxWSQstHVwLCqZHNyBzhk();
				RnrfyFtWoZVUJQZOPSgTUGspWfBg[ifpVnuiAcuCRPvvZrdwkbEinzjysA] = xGGQMyZVNBKhRaGnErwNnBmbPgkE;
			}
			else
			{
				RnrfyFtWoZVUJQZOPSgTUGspWfBg.Add(ifpVnuiAcuCRPvvZrdwkbEinzjysA, xGGQMyZVNBKhRaGnErwNnBmbPgkE);
			}
			xGGQMyZVNBKhRaGnErwNnBmbPgkE.wtMgiaYQimXpYkeCsmfpspMHblsO();
			return true;
		}

		private void RqLUXrphFZfrHSdHSJXKMvbwVeDH(int P_0)
		{
			if (RnrfyFtWoZVUJQZOPSgTUGspWfBg.ContainsKey(P_0))
			{
				RnrfyFtWoZVUJQZOPSgTUGspWfBg[P_0].LgoasxmQxWSQstHVwLCqZHNyBzhk();
				RnrfyFtWoZVUJQZOPSgTUGspWfBg.Remove(P_0);
			}
		}

		private bool JyBuMGSbLJJeEGwFWvFDMgifOCGP(int P_0)
		{
			if (P_0 < 0 || P_0 >= 32)
			{
				return false;
			}
			if (!qbAgQjRXIPTuMaDgGyMMhTVDcNdG.NcSEgMcXoinkicCbQndMIGyJSxOmB(P_0))
			{
				return false;
			}
			fMAKGMfYqqmFRMCFdbAbLpznnXQL fMAKGMfYqqmFRMCFdbAbLpznnXQL2 = OcnUtFXTkcUQapZyGXCsJIQDQzBB(P_0);
			if (fMAKGMfYqqmFRMCFdbAbLpznnXQL2 == null)
			{
				return false;
			}
			int ifpVnuiAcuCRPvvZrdwkbEinzjysA = fMAKGMfYqqmFRMCFdbAbLpznnXQL2.IfpVnuiAcuCRPvvZrdwkbEinzjysA;
			if (xJccXZHsCeYdOFkUoUzIOIYVNarnA.ContainsKey(ifpVnuiAcuCRPvvZrdwkbEinzjysA))
			{
				xJccXZHsCeYdOFkUoUzIOIYVNarnA[ifpVnuiAcuCRPvvZrdwkbEinzjysA].LgoasxmQxWSQstHVwLCqZHNyBzhk();
				xJccXZHsCeYdOFkUoUzIOIYVNarnA[ifpVnuiAcuCRPvvZrdwkbEinzjysA] = fMAKGMfYqqmFRMCFdbAbLpznnXQL2;
			}
			else
			{
				xJccXZHsCeYdOFkUoUzIOIYVNarnA.Add(ifpVnuiAcuCRPvvZrdwkbEinzjysA, fMAKGMfYqqmFRMCFdbAbLpznnXQL2);
			}
			fMAKGMfYqqmFRMCFdbAbLpznnXQL2.wtMgiaYQimXpYkeCsmfpspMHblsO();
			return true;
		}

		private void tIXPOPAosSMlrjqIVePSTBPbEpWBA(int P_0)
		{
			if (xJccXZHsCeYdOFkUoUzIOIYVNarnA.ContainsKey(P_0))
			{
				xJccXZHsCeYdOFkUoUzIOIYVNarnA[P_0].LgoasxmQxWSQstHVwLCqZHNyBzhk();
				xJccXZHsCeYdOFkUoUzIOIYVNarnA.Remove(P_0);
			}
		}

		private XGGQMyZVNBKhRaGnErwNnBmbPgkE xKmujLUSkGdGSKcaAlUJiLHmvnWH(int P_0)
		{
			if (!RnrfyFtWoZVUJQZOPSgTUGspWfBg.TryGetValue(P_0, out var value))
			{
				return null;
			}
			return value;
		}

		private fMAKGMfYqqmFRMCFdbAbLpznnXQL yZoZNbAmJncQpbIyMwDmqcGhBWpA(int P_0)
		{
			if (!xJccXZHsCeYdOFkUoUzIOIYVNarnA.TryGetValue(P_0, out var value))
			{
				return null;
			}
			return value;
		}

		private void zGURNWjKPfSluhaQhCoIVODqXoBE()
		{
			while (qbAgQjRXIPTuMaDgGyMMhTVDcNdG.iyduDPgRLtffsuSzsEsRDePvuuBD(JDctVvlsvLALDeWIYvSnBUGxhpWq) != 0)
			{
				cnTgKXISCeyRCiuLxapTdGTfaWkdA.EhLmclJAGyPQyemKGilRCBzVNVKV(JDctVvlsvLALDeWIYvSnBUGxhpWq);
				qbAgQjRXIPTuMaDgGyMMhTVDcNdG.cTVmRLFzrmgXlWoMsvytuoyBQiLm iagibpPKMQWvSinLOUYGtKIgjJgk = cnTgKXISCeyRCiuLxapTdGTfaWkdA.iagibpPKMQWvSinLOUYGtKIgjJgk;
				double realTime = ReInput.realTime;
				switch (iagibpPKMQWvSinLOUYGtKIgjJgk)
				{
				case qbAgQjRXIPTuMaDgGyMMhTVDcNdG.cTVmRLFzrmgXlWoMsvytuoyBQiLm.SDL_CONTROLLERAXISMOTION:
					qZylktUwXHRUzIkghOcpyRBvzKdm(ref cnTgKXISCeyRCiuLxapTdGTfaWkdA.xdDdrpqhlnoFwNppRQMWnqSsqhxP, realTime);
					break;
				case qbAgQjRXIPTuMaDgGyMMhTVDcNdG.cTVmRLFzrmgXlWoMsvytuoyBQiLm.SDL_CONTROLLERBUTTONDOWN:
				case qbAgQjRXIPTuMaDgGyMMhTVDcNdG.cTVmRLFzrmgXlWoMsvytuoyBQiLm.SDL_CONTROLLERBUTTONUP:
					BfnKVcLsZTHdypbxdiCxOUFbAOhb(ref cnTgKXISCeyRCiuLxapTdGTfaWkdA.yJXBMhEFRANjXINVbFgEOIeomxZeB, realTime);
					break;
				case qbAgQjRXIPTuMaDgGyMMhTVDcNdG.cTVmRLFzrmgXlWoMsvytuoyBQiLm.SDL_CONTROLLERDEVICEREMAPPED:
					lwkvCHgFSIqdUwlmgrAKRoEnLuLA(ref cnTgKXISCeyRCiuLxapTdGTfaWkdA.pcCeLuWaYzujxGJKDZLiMqIUJfcx);
					break;
				case qbAgQjRXIPTuMaDgGyMMhTVDcNdG.cTVmRLFzrmgXlWoMsvytuoyBQiLm.SDL_JOYAXISMOTION:
					hIMiHxeiMBsyYZecGLKOFrgmJWpg(ref cnTgKXISCeyRCiuLxapTdGTfaWkdA.psNPBJIRnEoVdcdWHZfSoBdWPpMD, realTime);
					break;
				case qbAgQjRXIPTuMaDgGyMMhTVDcNdG.cTVmRLFzrmgXlWoMsvytuoyBQiLm.SDL_JOYBUTTONDOWN:
				case qbAgQjRXIPTuMaDgGyMMhTVDcNdG.cTVmRLFzrmgXlWoMsvytuoyBQiLm.SDL_JOYBUTTONUP:
					RTsXbYWIaapuBNSukDHSCWgBODGm(ref cnTgKXISCeyRCiuLxapTdGTfaWkdA.oivWVOLdFWGboCExodGbYNNFApDz, realTime);
					break;
				case qbAgQjRXIPTuMaDgGyMMhTVDcNdG.cTVmRLFzrmgXlWoMsvytuoyBQiLm.SDL_JOYHATMOTION:
					pOFsuKKpyLBEjZPFMvryBFSRRvTU(ref cnTgKXISCeyRCiuLxapTdGTfaWkdA.rqoCdGYwpNmbQyLrLOVLuUBjaFvu, realTime);
					break;
				case qbAgQjRXIPTuMaDgGyMMhTVDcNdG.cTVmRLFzrmgXlWoMsvytuoyBQiLm.SDL_JOYBALLMOTION:
					xZhSmmAxhXvlKNXzxoUOzvxtilGm(ref cnTgKXISCeyRCiuLxapTdGTfaWkdA.ViIzLaxlJEPeIWjMxAxwMwrMGFHu, realTime);
					break;
				case qbAgQjRXIPTuMaDgGyMMhTVDcNdG.cTVmRLFzrmgXlWoMsvytuoyBQiLm.SDL_JOYDEVICEADDED:
					hAraDlXBnqQBZiYtefPDzVWQAvRW(ref cnTgKXISCeyRCiuLxapTdGTfaWkdA.VXTMWktiZHlNpSDvCerZYVLIzytG);
					break;
				case qbAgQjRXIPTuMaDgGyMMhTVDcNdG.cTVmRLFzrmgXlWoMsvytuoyBQiLm.SDL_JOYDEVICEREMOVED:
					IBsgWlOilALRYCYqemihzuOhcPlg(ref cnTgKXISCeyRCiuLxapTdGTfaWkdA.VXTMWktiZHlNpSDvCerZYVLIzytG);
					break;
				case qbAgQjRXIPTuMaDgGyMMhTVDcNdG.cTVmRLFzrmgXlWoMsvytuoyBQiLm.SDL_CONTROLLERDEVICEADDED:
					RidvZCDnUflYHUJntKUJekpzlErJ(ref cnTgKXISCeyRCiuLxapTdGTfaWkdA.pcCeLuWaYzujxGJKDZLiMqIUJfcx);
					break;
				case qbAgQjRXIPTuMaDgGyMMhTVDcNdG.cTVmRLFzrmgXlWoMsvytuoyBQiLm.SDL_CONTROLLERDEVICEREMOVED:
					sPraTLffYeoOgKDMMdBsuoAeNWrM(ref cnTgKXISCeyRCiuLxapTdGTfaWkdA.pcCeLuWaYzujxGJKDZLiMqIUJfcx);
					break;
				}
			}
		}

		private void hIMiHxeiMBsyYZecGLKOFrgmJWpg(ref qbAgQjRXIPTuMaDgGyMMhTVDcNdG.HSOwncKLxikQmztxIbxxzTvjaWyE P_0, double P_1)
		{
			if (NvaDFSSsEMOethDaxOoJiEWxXNwD)
			{
				vaEiJshgxPENqjjWDvypHSNUXLUNb(P_0.gGOYecFVKippQbsrmLTTonKkCRhH, MwKsnJyPOGEFvGzQAmSgbJnKNOltA.Axis, P_0.pllxyqdXiKAHtHFZVFfQTNytrgaB, P_0.OicgOlewuRpBLTOnSEiaFePAvhsV, P_1);
			}
		}

		private void RTsXbYWIaapuBNSukDHSCWgBODGm(ref qbAgQjRXIPTuMaDgGyMMhTVDcNdG.pYDDKYkFSNvAOPTSQzvqMPnvqtyS P_0, double P_1)
		{
			if (NvaDFSSsEMOethDaxOoJiEWxXNwD)
			{
				vaEiJshgxPENqjjWDvypHSNUXLUNb(P_0.QhgefbJOjuBGJKSDmbzNBETEqbvJB, MwKsnJyPOGEFvGzQAmSgbJnKNOltA.Button, P_0.wimEEYCKPKMnNbPraVUzcHWPhypU, P_0.NfohweOUiRKHBKgqXJzwxZdzMJVC, P_1);
			}
		}

		private void pOFsuKKpyLBEjZPFMvryBFSRRvTU(ref qbAgQjRXIPTuMaDgGyMMhTVDcNdG.kaMgiqfNFRsBxvfPUzZmUgQPjrGCA P_0, double P_1)
		{
			if (NvaDFSSsEMOethDaxOoJiEWxXNwD)
			{
				vaEiJshgxPENqjjWDvypHSNUXLUNb(P_0.chvonafIKHUXhJqIRGPXVBQfexkeA, MwKsnJyPOGEFvGzQAmSgbJnKNOltA.Hat, P_0.EImRTxOnKwBTqMQvwSrZqpCfFGBF, P_0.rTygsJfviPopOBNVddDpPZxyIGyBB, P_1);
			}
		}

		private void xZhSmmAxhXvlKNXzxoUOzvxtilGm(ref qbAgQjRXIPTuMaDgGyMMhTVDcNdG.VFyRxmAbaWZSkYeELiXKMFoVqhDl P_0, double P_1)
		{
			_ = NvaDFSSsEMOethDaxOoJiEWxXNwD;
		}

		private void hAraDlXBnqQBZiYtefPDzVWQAvRW(ref qbAgQjRXIPTuMaDgGyMMhTVDcNdG.bmQSiQOUqSLhejFUQlJpvnoewzwR P_0)
		{
			if (NvaDFSSsEMOethDaxOoJiEWxXNwD)
			{
				ecyUsQThWqdZgHDxWZtlqSDhawhm(P_0.pkVpyXSmFFTRFNqRgztrWhREPXU);
				if (JcDbePCxjnJKRiNXRQfxQMvUAqBZA != null)
				{
					JcDbePCxjnJKRiNXRQfxQMvUAqBZA();
				}
			}
		}

		private void IBsgWlOilALRYCYqemihzuOhcPlg(ref qbAgQjRXIPTuMaDgGyMMhTVDcNdG.bmQSiQOUqSLhejFUQlJpvnoewzwR P_0)
		{
			if (NvaDFSSsEMOethDaxOoJiEWxXNwD)
			{
				RqLUXrphFZfrHSdHSJXKMvbwVeDH(P_0.pkVpyXSmFFTRFNqRgztrWhREPXU);
				if (JcDbePCxjnJKRiNXRQfxQMvUAqBZA != null)
				{
					JcDbePCxjnJKRiNXRQfxQMvUAqBZA();
				}
			}
		}

		private void qZylktUwXHRUzIkghOcpyRBvzKdm(ref qbAgQjRXIPTuMaDgGyMMhTVDcNdG.NMvMsfLWGFLMMbClOWHKdVbGFPDV P_0, double P_1)
		{
			if (GzIpgcIyRZSOUsQWVPStSCrxGxpq && P_0.lkvjevHRCZRojnVdTFlJlrNmRmRe != 6)
			{
				qYbdvliCuVrFSxHkjHVMsmPcTeFj(P_0.nMTFmYCJUmzarLTIOpyqiYuCLZruA, MwKsnJyPOGEFvGzQAmSgbJnKNOltA.Axis, P_0.lkvjevHRCZRojnVdTFlJlrNmRmRe, P_0.YauUGbCsabNLdSLeFYRSoaPvbATy, P_1);
			}
		}

		private void BfnKVcLsZTHdypbxdiCxOUFbAOhb(ref qbAgQjRXIPTuMaDgGyMMhTVDcNdG.mWepiFweeUBtWCqbtzLTtEweAkKA P_0, double P_1)
		{
			if (GzIpgcIyRZSOUsQWVPStSCrxGxpq && P_0.jYKpIxrKUeytxpmXedQKtcnDEfrW != 15)
			{
				qYbdvliCuVrFSxHkjHVMsmPcTeFj(P_0.VSUGqBCFJeGizBLvcOvXfqzcRkzu, MwKsnJyPOGEFvGzQAmSgbJnKNOltA.Button, P_0.jYKpIxrKUeytxpmXedQKtcnDEfrW, P_0.jxNmHoKBpShZcZabLpJNvRcSvDey, P_1);
			}
		}

		private void RidvZCDnUflYHUJntKUJekpzlErJ(ref qbAgQjRXIPTuMaDgGyMMhTVDcNdG.YGKoUSHKxKGumGdAmIDvQUDGmYYxA P_0)
		{
			if (GzIpgcIyRZSOUsQWVPStSCrxGxpq)
			{
				JyBuMGSbLJJeEGwFWvFDMgifOCGP(P_0.AkyJfZMSJRgNtSHuSzFGaizgNWeS);
				if (JcDbePCxjnJKRiNXRQfxQMvUAqBZA != null)
				{
					JcDbePCxjnJKRiNXRQfxQMvUAqBZA();
				}
			}
		}

		private void sPraTLffYeoOgKDMMdBsuoAeNWrM(ref qbAgQjRXIPTuMaDgGyMMhTVDcNdG.YGKoUSHKxKGumGdAmIDvQUDGmYYxA P_0)
		{
			if (GzIpgcIyRZSOUsQWVPStSCrxGxpq)
			{
				tIXPOPAosSMlrjqIVePSTBPbEpWBA(P_0.AkyJfZMSJRgNtSHuSzFGaizgNWeS);
				if (JcDbePCxjnJKRiNXRQfxQMvUAqBZA != null)
				{
					JcDbePCxjnJKRiNXRQfxQMvUAqBZA();
				}
			}
		}

		private void lwkvCHgFSIqdUwlmgrAKRoEnLuLA(ref qbAgQjRXIPTuMaDgGyMMhTVDcNdG.YGKoUSHKxKGumGdAmIDvQUDGmYYxA P_0)
		{
			_ = GzIpgcIyRZSOUsQWVPStSCrxGxpq;
		}

		private void vaEiJshgxPENqjjWDvypHSNUXLUNb(int P_0, MwKsnJyPOGEFvGzQAmSgbJnKNOltA P_1, byte P_2, short P_3, double P_4)
		{
			xKmujLUSkGdGSKcaAlUJiLHmvnWH(P_0)?.tfetQISCmyhJuXrFFfsAecThqogsA(P_1, P_2, P_3, P_4);
		}

		private void qYbdvliCuVrFSxHkjHVMsmPcTeFj(int P_0, MwKsnJyPOGEFvGzQAmSgbJnKNOltA P_1, byte P_2, short P_3, double P_4)
		{
			yZoZNbAmJncQpbIyMwDmqcGhBWpA(P_0)?.tfetQISCmyhJuXrFFfsAecThqogsA(P_1, P_2, P_3, P_4);
		}

		private void YdlfseJaCyQYCBYjyMeEqNaBUBxd()
		{
			string[] array = cOfrEWLVmoLyUzNQMDqBNSGhsqRv.eEfjlsGYEMxQdgLAduIFyLDRJxKr();
			if (array == null)
			{
				return;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (!string.IsNullOrEmpty(array[i]) && array[i].Length > 32 && !(qbAgQjRXIPTuMaDgGyMMhTVDcNdG.kNXnrfnjuNKfVPqGdmvSsPkRjYhr(new Guid(array[i].Substring(0, 32))) != string.Empty))
				{
					qbAgQjRXIPTuMaDgGyMMhTVDcNdG.ZbgamPLlqtAKFJFcMSvYqFTaILgx(array[i]);
				}
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Dispose
			this.Dispose();
		}

		~SDL2InputSource()
		{
			Dispose(disposing: false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (ztTxyRQlkIMQNZGZSqIocFadIMOgA)
			{
				return;
			}
			if (disposing)
			{
				if (JDctVvlsvLALDeWIYvSnBUGxhpWq != null)
				{
					JDctVvlsvLALDeWIYvSnBUGxhpWq.Dispose();
				}
				NSCsifByDSiKRRMemxlcvGCNIUYBA();
			}
			qbAgQjRXIPTuMaDgGyMMhTVDcNdG.SrkyCtYMRQchKrJRwAljzCVKhjVt();
			OUtHtUEbYUyLhXKfEWnCRAJIfGlk = false;
			ztTxyRQlkIMQNZGZSqIocFadIMOgA = true;
		}
	}
}
