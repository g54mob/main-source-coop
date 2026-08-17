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
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class SDL2InputSource : IDisposable, IInputSource
	{
		public delegate void BOPCCUpPBbJtOgCpekOJqtLhQTIK(int joystickId, byte rewiredElementType, byte elementIndex, short value);

		public delegate void yfihbfvkSrPQvCcRATeXLMaLQXlk(int joystickIndex);

		public delegate void oPtCnEnxUdGOvVgrIJavNPtJjREq(int joystickId);

		public delegate void GeYcOMhlaYzKQeiqTPOmcRNscPGTA(int gameControllerId, byte rewiredElementType, byte sdlElementType, short value);

		private const int AxSEOABMkXghZmSEPKKQEQFyonAlA = 32;

		private bool APoNWtXoeYHyrbDkDswtiTxkfkUL;

		private bool BUmHgvFAJSaxNqKAkNFxTGAZejPIA;

		private bool eEerJXpIkLJjWGhWhVTVcNOaIiBAA;

		private bool ZZJerRuoZikIBFxmWuqdynzQHrgs;

		private bool obpzIVquVRQseulcTcTZaHvizTjRA;

		private ADictionary<int, RzpKZbCpWsgubaEUuUNtBGeHcjal> pfvHZfkVJHuFpTIaVPSjIEuhIisl;

		private ADictionary<int, hphINTghzNEHxUPDVroFxKuDYUSd> PVAbxWitkmowNRFVfIMGpcpwKhDb;

		private ikfePoAXHchteyoSqLTiXmtbNjbd.pZxgJRiLHTbtEDaQOLvsJOffWSZlA uqAvHWxPxhMooBQwGulIkvAnIrsK;

		private NativeBuffer TNIvOvlbHBfBTxffqblcIdyNTeAv;

		[CompilerGenerated]
		private Action DEbuznmERjiCnEQGPtRombzvjfeh;

		private bool AeWaeWamxRrERkciQkpFDWbRfZMkA;

		public bool initialized => obpzIVquVRQseulcTcTZaHvizTjRA;

		private event Action _DeviceChangedEvent
		{
			[CompilerGenerated]
			add
			{
				Action action = DEbuznmERjiCnEQGPtRombzvjfeh;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, b);
					action = Interlocked.CompareExchange(ref DEbuznmERjiCnEQGPtRombzvjfeh, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = DEbuznmERjiCnEQGPtRombzvjfeh;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value3);
					action = Interlocked.CompareExchange(ref DEbuznmERjiCnEQGPtRombzvjfeh, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public event Action DeviceChangedEvent
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
			APoNWtXoeYHyrbDkDswtiTxkfkUL = P_1;
			BUmHgvFAJSaxNqKAkNFxTGAZejPIA = P_2;
			eEerJXpIkLJjWGhWhVTVcNOaIiBAA = P_3;
			ZZJerRuoZikIBFxmWuqdynzQHrgs = P_4;
			pfvHZfkVJHuFpTIaVPSjIEuhIisl = new ADictionary<int, RzpKZbCpWsgubaEUuUNtBGeHcjal>();
			PVAbxWitkmowNRFVfIMGpcpwKhDb = new ADictionary<int, hphINTghzNEHxUPDVroFxKuDYUSd>();
			int num = ((!UnityTools.isEditor || UnityTools.editorPlatform != EditorPlatform.OSX) ? 29184 : 25088);
			try
			{
				ikfePoAXHchteyoSqLTiXmtbNjbd.LMfBsJIgIyucOWRnGnbLLelWrNTwA(UnityTools.effectivePlatform);
				if (ikfePoAXHchteyoSqLTiXmtbNjbd.JOdNHgndKGtMwDrBcSSYaefhlOEO((uint)num) < 0)
				{
					throw new Exception("Failed initialize SDL2!");
				}
				obpzIVquVRQseulcTcTZaHvizTjRA = true;
				if (P_2)
				{
					nafLQIiEsnsNNjBRiGfkhykETqeQ();
				}
				bXSliKMHXPBxpbZBblVKTWOVAoDn();
				TNIvOvlbHBfBTxffqblcIdyNTeAv = new NativeBuffer(56);
			}
			catch
			{
				obpzIVquVRQseulcTcTZaHvizTjRA = false;
				Dispose();
				throw;
			}
		}

		public void SystemDeviceConnected()
		{
			throw new NotImplementedException();
		}

		public void SystemDeviceDisconnected()
		{
			throw new NotImplementedException();
		}

		public void Update()
		{
			_ = obpzIVquVRQseulcTcTZaHvizTjRA;
		}

		public void UpdateDevices(UpdateLoopType updateLoop)
		{
			if (obpzIVquVRQseulcTcTZaHvizTjRA)
			{
				oJttfTtVdWGnUshHrBdcJQnschXi();
			}
		}

		public void UpdateFinished()
		{
			_ = obpzIVquVRQseulcTcTZaHvizTjRA;
		}

		public IList<T> GetJoysticks<T>() where T : class
		{
			if (!obpzIVquVRQseulcTcTZaHvizTjRA)
			{
				return null;
			}
			List<YnTfoKfozPExlkgidDzpkPNmatxBb> list = new List<YnTfoKfozPExlkgidDzpkPNmatxBb>();
			if (APoNWtXoeYHyrbDkDswtiTxkfkUL)
			{
				foreach (KeyValuePair<int, RzpKZbCpWsgubaEUuUNtBGeHcjal> item in pfvHZfkVJHuFpTIaVPSjIEuhIisl)
				{
					if (item.Value.MIFGEQkClVgkOHFLeCwtLNUzKzlIB)
					{
						list.Add(item.Value);
					}
				}
			}
			if (BUmHgvFAJSaxNqKAkNFxTGAZejPIA)
			{
				foreach (KeyValuePair<int, hphINTghzNEHxUPDVroFxKuDYUSd> item2 in PVAbxWitkmowNRFVfIMGpcpwKhDb)
				{
					hphINTghzNEHxUPDVroFxKuDYUSd value = item2.Value;
					if (value.MIFGEQkClVgkOHFLeCwtLNUzKzlIB)
					{
						list.Add(value);
					}
				}
			}
			return list as IList<T>;
		}

		private int JroAzOpjwsDYKzrwpdynudJALgYw()
		{
			if (!obpzIVquVRQseulcTcTZaHvizTjRA)
			{
				return 0;
			}
			return Math.Min(ikfePoAXHchteyoSqLTiXmtbNjbd.daZGGZyIcFMUAJXlJOUGnWCjazRo(), 32);
		}

		private int VPZfqiCRRsfizgMcmRRCszzsSSDJ()
		{
			if (!obpzIVquVRQseulcTcTZaHvizTjRA)
			{
				return 0;
			}
			int num = JroAzOpjwsDYKzrwpdynudJALgYw();
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				if (!ikfePoAXHchteyoSqLTiXmtbNjbd.KLBLbZIMpcihsgXmCAKsBRrqyJXD(i))
				{
					num2++;
				}
			}
			return num2;
		}

		private RzpKZbCpWsgubaEUuUNtBGeHcjal sPkUKVZmchFVNidkQRboCROhnvCCb(int P_0)
		{
			IntPtr intPtr = ikfePoAXHchteyoSqLTiXmtbNjbd.npjHpcNaARrNuFOWuPkNXwxlgeIM(P_0);
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			ADyAksuGELvZwlagWICRBzcTKbTy aDyAksuGELvZwlagWICRBzcTKbTy = new ADyAksuGELvZwlagWICRBzcTKbTy(intPtr);
			wHGpGKjtVGRHisAxykuOCasKYqsT wHGpGKjtVGRHisAxykuOCasKYqsT2 = hVeGBaBzltexdRYAcQljPYVxCajV(P_0, aDyAksuGELvZwlagWICRBzcTKbTy);
			if (wHGpGKjtVGRHisAxykuOCasKYqsT2 == null)
			{
				ikfePoAXHchteyoSqLTiXmtbNjbd.eOTbDMGXrMfbSvFeOGNoXlMpXSXt(intPtr);
				return null;
			}
			return new RzpKZbCpWsgubaEUuUNtBGeHcjal(aDyAksuGELvZwlagWICRBzcTKbTy, wHGpGKjtVGRHisAxykuOCasKYqsT2);
		}

		private hphINTghzNEHxUPDVroFxKuDYUSd CjLPIfXjmqNmUlqeTaqMbkLryMPW(int P_0)
		{
			IntPtr intPtr = ikfePoAXHchteyoSqLTiXmtbNjbd.oPZPmGKaNmzRHCkCrRZWIiLtZbwd(P_0);
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			FpZXCDmwgTbyWItcpddtIBipfuYO fpZXCDmwgTbyWItcpddtIBipfuYO = new FpZXCDmwgTbyWItcpddtIBipfuYO(intPtr);
			wHGpGKjtVGRHisAxykuOCasKYqsT wHGpGKjtVGRHisAxykuOCasKYqsT2 = drHmgMPLOXLUEkgSFgWiQvgDDEVkA(P_0, fpZXCDmwgTbyWItcpddtIBipfuYO);
			if (wHGpGKjtVGRHisAxykuOCasKYqsT2 == null)
			{
				return null;
			}
			if (!wHGpGKjtVGRHisAxykuOCasKYqsT2.YpBtwmHHGBDqMXwfPbSITfkICZop)
			{
				ikfePoAXHchteyoSqLTiXmtbNjbd.onbKpKfjbfdDyzOZKQMyCGXGxJDl(intPtr);
				return null;
			}
			wHGpGKjtVGRHisAxykuOCasKYqsT2.noDyXDUdlPKBMSkRvbqYasAXqAFaA = ikfePoAXHchteyoSqLTiXmtbNjbd.QGIGuhTEmwecGJEtchVMpoxXsYdP(fpZXCDmwgTbyWItcpddtIBipfuYO);
			return new hphINTghzNEHxUPDVroFxKuDYUSd(fpZXCDmwgTbyWItcpddtIBipfuYO, wHGpGKjtVGRHisAxykuOCasKYqsT2);
		}

		private wHGpGKjtVGRHisAxykuOCasKYqsT hVeGBaBzltexdRYAcQljPYVxCajV(int P_0, ADyAksuGELvZwlagWICRBzcTKbTy P_1)
		{
			if (!obpzIVquVRQseulcTcTZaHvizTjRA)
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
			return new wHGpGKjtVGRHisAxykuOCasKYqsT
			{
				HNrdETOGKUabWLqKrsrJjjjvQQqu = P_0,
				bUXXVdXGkUesYGUEMAGNgTFtAiMsA = ikfePoAXHchteyoSqLTiXmtbNjbd.FASjyEQFUcejKbZGTeSvvmBReprIb(P_1),
				YpBtwmHHGBDqMXwfPbSITfkICZop = ikfePoAXHchteyoSqLTiXmtbNjbd.KLBLbZIMpcihsgXmCAKsBRrqyJXD(P_0),
				TyBbfACkfBSDMBUYEBOmeDAdwaFFc = ikfePoAXHchteyoSqLTiXmtbNjbd.XMdVSvHPmrhcIdXwtJbFKbwemRHWA(P_1),
				FeHpFNUpCcTBGOhdkWReGnguZnrV = ikfePoAXHchteyoSqLTiXmtbNjbd.lREfoKJvEUAgMbBYdteBQNYLehJXA(P_1),
				BkXUMuYBKghOBIdJYiPftcAEYsXQA = ikfePoAXHchteyoSqLTiXmtbNjbd.zgDNbBpWvLcjEHkXOKMMAUakfuCW(P_0),
				tpoonPbJmajQyAErVLnAOoXhqZqEb = ikfePoAXHchteyoSqLTiXmtbNjbd.XPJNRzKrnAzdaQquYRnyNJrfBgZc(P_1),
				yLLlShtixVkbYMaIfuUMijmLCboKA = ikfePoAXHchteyoSqLTiXmtbNjbd.nLFRWxKBObouWutYqGQnywlqHUAS(P_1),
				VlOBcOivgRYNVAIyWkzlGxAaDIWac = ikfePoAXHchteyoSqLTiXmtbNjbd.nlJtPryJASdprzqnPXxCRsGuPUJM(P_1),
				ZotJgQDIqTjrsClzSattVdeemzgR = ikfePoAXHchteyoSqLTiXmtbNjbd.ZcIUxelyYRqeYAEhASwKnNCFyUg(P_1)
			};
		}

		private wHGpGKjtVGRHisAxykuOCasKYqsT drHmgMPLOXLUEkgSFgWiQvgDDEVkA(int P_0, FpZXCDmwgTbyWItcpddtIBipfuYO P_1)
		{
			if (P_1 == null || !P_1.IsValid)
			{
				return null;
			}
			ADyAksuGELvZwlagWICRBzcTKbTy aDyAksuGELvZwlagWICRBzcTKbTy = new ADyAksuGELvZwlagWICRBzcTKbTy(ikfePoAXHchteyoSqLTiXmtbNjbd.yjsbquiJRrnACunbGBhtgSIzJilBA(P_1));
			if (!aDyAksuGELvZwlagWICRBzcTKbTy.IsValid)
			{
				return null;
			}
			return hVeGBaBzltexdRYAcQljPYVxCajV(P_0, aDyAksuGELvZwlagWICRBzcTKbTy);
		}

		private void bXSliKMHXPBxpbZBblVKTWOVAoDn()
		{
			for (int i = 0; i < JroAzOpjwsDYKzrwpdynudJALgYw(); i++)
			{
				if (APoNWtXoeYHyrbDkDswtiTxkfkUL)
				{
					qckZxShqtYgKZSGQmXJLvbtBGAUy(i);
				}
				if (BUmHgvFAJSaxNqKAkNFxTGAZejPIA)
				{
					sauBYMcgVeObPsfBYhmYAmBhcfFRA(i);
				}
			}
		}

		private void NIHulfYmlhxPFhjrJaZNmdqSxxeU()
		{
			if (BUmHgvFAJSaxNqKAkNFxTGAZejPIA)
			{
				foreach (KeyValuePair<int, hphINTghzNEHxUPDVroFxKuDYUSd> item in PVAbxWitkmowNRFVfIMGpcpwKhDb)
				{
					hphINTghzNEHxUPDVroFxKuDYUSd value = item.Value;
					value.GpGIRuIvoGIAHofqjWMtNRetHESX();
					value.Dispose();
				}
				PVAbxWitkmowNRFVfIMGpcpwKhDb.Clear();
			}
			if (!APoNWtXoeYHyrbDkDswtiTxkfkUL)
			{
				return;
			}
			foreach (KeyValuePair<int, RzpKZbCpWsgubaEUuUNtBGeHcjal> item2 in pfvHZfkVJHuFpTIaVPSjIEuhIisl)
			{
				RzpKZbCpWsgubaEUuUNtBGeHcjal value2 = item2.Value;
				value2.GpGIRuIvoGIAHofqjWMtNRetHESX();
				value2.Dispose();
			}
			pfvHZfkVJHuFpTIaVPSjIEuhIisl.Clear();
		}

		private bool qckZxShqtYgKZSGQmXJLvbtBGAUy(int P_0)
		{
			if (P_0 < 0 || P_0 >= 32)
			{
				return false;
			}
			if (BUmHgvFAJSaxNqKAkNFxTGAZejPIA && ikfePoAXHchteyoSqLTiXmtbNjbd.KLBLbZIMpcihsgXmCAKsBRrqyJXD(P_0))
			{
				return false;
			}
			RzpKZbCpWsgubaEUuUNtBGeHcjal rzpKZbCpWsgubaEUuUNtBGeHcjal = sPkUKVZmchFVNidkQRboCROhnvCCb(P_0);
			if (rzpKZbCpWsgubaEUuUNtBGeHcjal == null)
			{
				return false;
			}
			int kXmAeOVIoefUVTrVJWaLobNGDaae = rzpKZbCpWsgubaEUuUNtBGeHcjal.kXmAeOVIoefUVTrVJWaLobNGDaae;
			if (pfvHZfkVJHuFpTIaVPSjIEuhIisl.ContainsKey(kXmAeOVIoefUVTrVJWaLobNGDaae))
			{
				pfvHZfkVJHuFpTIaVPSjIEuhIisl[kXmAeOVIoefUVTrVJWaLobNGDaae].GpGIRuIvoGIAHofqjWMtNRetHESX();
				pfvHZfkVJHuFpTIaVPSjIEuhIisl[kXmAeOVIoefUVTrVJWaLobNGDaae] = rzpKZbCpWsgubaEUuUNtBGeHcjal;
			}
			else
			{
				pfvHZfkVJHuFpTIaVPSjIEuhIisl.Add(kXmAeOVIoefUVTrVJWaLobNGDaae, rzpKZbCpWsgubaEUuUNtBGeHcjal);
			}
			rzpKZbCpWsgubaEUuUNtBGeHcjal.zQQfvDZMmpVqPPLYlLuSJXXpwJcI();
			return true;
		}

		private void GHLFUYDRbbdpOizTVbmQosBYlpOBb(int P_0)
		{
			if (pfvHZfkVJHuFpTIaVPSjIEuhIisl.ContainsKey(P_0))
			{
				pfvHZfkVJHuFpTIaVPSjIEuhIisl[P_0].GpGIRuIvoGIAHofqjWMtNRetHESX();
				pfvHZfkVJHuFpTIaVPSjIEuhIisl.Remove(P_0);
			}
		}

		private bool sauBYMcgVeObPsfBYhmYAmBhcfFRA(int P_0)
		{
			if (P_0 < 0 || P_0 >= 32)
			{
				return false;
			}
			if (!ikfePoAXHchteyoSqLTiXmtbNjbd.KLBLbZIMpcihsgXmCAKsBRrqyJXD(P_0))
			{
				return false;
			}
			hphINTghzNEHxUPDVroFxKuDYUSd hphINTghzNEHxUPDVroFxKuDYUSd2 = CjLPIfXjmqNmUlqeTaqMbkLryMPW(P_0);
			if (hphINTghzNEHxUPDVroFxKuDYUSd2 == null)
			{
				return false;
			}
			int kXmAeOVIoefUVTrVJWaLobNGDaae = hphINTghzNEHxUPDVroFxKuDYUSd2.kXmAeOVIoefUVTrVJWaLobNGDaae;
			if (PVAbxWitkmowNRFVfIMGpcpwKhDb.ContainsKey(kXmAeOVIoefUVTrVJWaLobNGDaae))
			{
				PVAbxWitkmowNRFVfIMGpcpwKhDb[kXmAeOVIoefUVTrVJWaLobNGDaae].GpGIRuIvoGIAHofqjWMtNRetHESX();
				PVAbxWitkmowNRFVfIMGpcpwKhDb[kXmAeOVIoefUVTrVJWaLobNGDaae] = hphINTghzNEHxUPDVroFxKuDYUSd2;
			}
			else
			{
				PVAbxWitkmowNRFVfIMGpcpwKhDb.Add(kXmAeOVIoefUVTrVJWaLobNGDaae, hphINTghzNEHxUPDVroFxKuDYUSd2);
			}
			hphINTghzNEHxUPDVroFxKuDYUSd2.zQQfvDZMmpVqPPLYlLuSJXXpwJcI();
			return true;
		}

		private void UDOqbwjHuFmoHKVRbpUeSRwwxwEP(int P_0)
		{
			if (PVAbxWitkmowNRFVfIMGpcpwKhDb.ContainsKey(P_0))
			{
				PVAbxWitkmowNRFVfIMGpcpwKhDb[P_0].GpGIRuIvoGIAHofqjWMtNRetHESX();
				PVAbxWitkmowNRFVfIMGpcpwKhDb.Remove(P_0);
			}
		}

		private RzpKZbCpWsgubaEUuUNtBGeHcjal ppFBkIhRhknvjnBNoaaKkvYBrvifA(int P_0)
		{
			if (!pfvHZfkVJHuFpTIaVPSjIEuhIisl.TryGetValue(P_0, out var value))
			{
				return null;
			}
			return value;
		}

		private hphINTghzNEHxUPDVroFxKuDYUSd jluJYbeJLOPOOXhJzwEBSEwiSgyc(int P_0)
		{
			if (!PVAbxWitkmowNRFVfIMGpcpwKhDb.TryGetValue(P_0, out var value))
			{
				return null;
			}
			return value;
		}

		private void oJttfTtVdWGnUshHrBdcJQnschXi()
		{
			while (ikfePoAXHchteyoSqLTiXmtbNjbd.rAjcCGeITzkijApSEqJhVkLhgIux(TNIvOvlbHBfBTxffqblcIdyNTeAv) != 0)
			{
				uqAvHWxPxhMooBQwGulIkvAnIrsK.cngigLpFzOjOBbEMhBSfgixoNTQJA(TNIvOvlbHBfBTxffqblcIdyNTeAv);
				ikfePoAXHchteyoSqLTiXmtbNjbd.nEKayXPWUUgmGsRUApmLHZPbvsVh gwUdWoefqvdKVvJvsShVNYBtVNpSA = uqAvHWxPxhMooBQwGulIkvAnIrsK.gwUdWoefqvdKVvJvsShVNYBtVNpSA;
				double realTime = ReInput.realTime;
				switch (gwUdWoefqvdKVvJvsShVNYBtVNpSA)
				{
				case ikfePoAXHchteyoSqLTiXmtbNjbd.nEKayXPWUUgmGsRUApmLHZPbvsVh.SDL_CONTROLLERAXISMOTION:
					UZKMNAtlCaqUIsgJeTMhZrtXANEV(ref uqAvHWxPxhMooBQwGulIkvAnIrsK.qKDAZUIwtmgOTpVbwBCtKJouODtd, realTime);
					break;
				case ikfePoAXHchteyoSqLTiXmtbNjbd.nEKayXPWUUgmGsRUApmLHZPbvsVh.SDL_CONTROLLERBUTTONDOWN:
				case ikfePoAXHchteyoSqLTiXmtbNjbd.nEKayXPWUUgmGsRUApmLHZPbvsVh.SDL_CONTROLLERBUTTONUP:
					fAbcVBwBbAeGWTivPzKEUQjfdTjjA(ref uqAvHWxPxhMooBQwGulIkvAnIrsK.QWAFnNVDHSGOitXFmvFZHiVveETg, realTime);
					break;
				case ikfePoAXHchteyoSqLTiXmtbNjbd.nEKayXPWUUgmGsRUApmLHZPbvsVh.SDL_CONTROLLERDEVICEREMAPPED:
					PdOvnrteRYEkjsTlxcNZOOlZLmvt(ref uqAvHWxPxhMooBQwGulIkvAnIrsK.ZLrloeePetzoZBcRNfiGFExoYVmG);
					break;
				case ikfePoAXHchteyoSqLTiXmtbNjbd.nEKayXPWUUgmGsRUApmLHZPbvsVh.SDL_JOYAXISMOTION:
					QkDdJZMeBWGmiTXpZruuOfpBcrGl(ref uqAvHWxPxhMooBQwGulIkvAnIrsK.KmeGDhCYUWmAxLvYwnFskQPPjHtz, realTime);
					break;
				case ikfePoAXHchteyoSqLTiXmtbNjbd.nEKayXPWUUgmGsRUApmLHZPbvsVh.SDL_JOYBUTTONDOWN:
				case ikfePoAXHchteyoSqLTiXmtbNjbd.nEKayXPWUUgmGsRUApmLHZPbvsVh.SDL_JOYBUTTONUP:
					QzkzNtVdoeaHYXdXIxuZnnghGPzp(ref uqAvHWxPxhMooBQwGulIkvAnIrsK.cEZdFrcopcjAsKWRjCdqeLOmHHNm, realTime);
					break;
				case ikfePoAXHchteyoSqLTiXmtbNjbd.nEKayXPWUUgmGsRUApmLHZPbvsVh.SDL_JOYHATMOTION:
					fenyqpXVagBGimkMtqegPbByvNZ(ref uqAvHWxPxhMooBQwGulIkvAnIrsK.OOgXyNvLYVISfdTKEJElZHvHbxZO, realTime);
					break;
				case ikfePoAXHchteyoSqLTiXmtbNjbd.nEKayXPWUUgmGsRUApmLHZPbvsVh.SDL_JOYBALLMOTION:
					xEvwtCwvXpqXAQHCKqeFDfSOaEgAA(ref uqAvHWxPxhMooBQwGulIkvAnIrsK.WQNEhaDAdJiACkcEcaZrMiccQMqfb, realTime);
					break;
				case ikfePoAXHchteyoSqLTiXmtbNjbd.nEKayXPWUUgmGsRUApmLHZPbvsVh.SDL_JOYDEVICEADDED:
					xStnTqeXNNLuVqbjECVBheQIpYjL(ref uqAvHWxPxhMooBQwGulIkvAnIrsK.hBLtSBihaGgGPeSrzOIOIBcKpfWzb);
					break;
				case ikfePoAXHchteyoSqLTiXmtbNjbd.nEKayXPWUUgmGsRUApmLHZPbvsVh.SDL_JOYDEVICEREMOVED:
					tmvdtQgSLxIHkpXFVEzxWLEZERzE(ref uqAvHWxPxhMooBQwGulIkvAnIrsK.hBLtSBihaGgGPeSrzOIOIBcKpfWzb);
					break;
				case ikfePoAXHchteyoSqLTiXmtbNjbd.nEKayXPWUUgmGsRUApmLHZPbvsVh.SDL_CONTROLLERDEVICEADDED:
					QxMJfTDDqnmaAXdoNozbymRRMAEf(ref uqAvHWxPxhMooBQwGulIkvAnIrsK.ZLrloeePetzoZBcRNfiGFExoYVmG);
					break;
				case ikfePoAXHchteyoSqLTiXmtbNjbd.nEKayXPWUUgmGsRUApmLHZPbvsVh.SDL_CONTROLLERDEVICEREMOVED:
					qotXQOCpSJCmEiNAYaOCvMUFNGSEb(ref uqAvHWxPxhMooBQwGulIkvAnIrsK.ZLrloeePetzoZBcRNfiGFExoYVmG);
					break;
				}
			}
		}

		private void QkDdJZMeBWGmiTXpZruuOfpBcrGl(ref ikfePoAXHchteyoSqLTiXmtbNjbd.sFUCNuAYHSPTHylLJckTelReGUqeA P_0, double P_1)
		{
			if (APoNWtXoeYHyrbDkDswtiTxkfkUL)
			{
				SujGBkgZLmlPPzTolCwPCiDoNLgC(P_0.DKMwsMPTHXggGJzTsupzKYgnfiTp, GGbtdUzpXnCzPhpAqObQrrCgdzpo.Axis, P_0.UtJFkWMjMzuefMSZCUDoBmxxaAue, P_0.yYOUbwIbBcyPAQvjeAEXVsjzLAln, P_1);
			}
		}

		private void QzkzNtVdoeaHYXdXIxuZnnghGPzp(ref ikfePoAXHchteyoSqLTiXmtbNjbd.fmwgCVUPDchacTEIiLOOwEGTbSwbA P_0, double P_1)
		{
			if (APoNWtXoeYHyrbDkDswtiTxkfkUL)
			{
				SujGBkgZLmlPPzTolCwPCiDoNLgC(P_0.DKMwsMPTHXggGJzTsupzKYgnfiTp, GGbtdUzpXnCzPhpAqObQrrCgdzpo.Button, P_0.oOrIZgDqgkBiIeNHVJdqkqbZWomK, P_0.UFCGiNgJQTBJXbozkINBMenjSxAW, P_1);
			}
		}

		private void fenyqpXVagBGimkMtqegPbByvNZ(ref ikfePoAXHchteyoSqLTiXmtbNjbd.iiruHewHkaiIjvQfNEZCiUOhIISuA P_0, double P_1)
		{
			if (APoNWtXoeYHyrbDkDswtiTxkfkUL)
			{
				SujGBkgZLmlPPzTolCwPCiDoNLgC(P_0.DKMwsMPTHXggGJzTsupzKYgnfiTp, GGbtdUzpXnCzPhpAqObQrrCgdzpo.Hat, P_0.tQaGhzmJfUCxqxuyjovHupwfnbFD, P_0.yYOUbwIbBcyPAQvjeAEXVsjzLAln, P_1);
			}
		}

		private void xEvwtCwvXpqXAQHCKqeFDfSOaEgAA(ref ikfePoAXHchteyoSqLTiXmtbNjbd.cITSKHpEhhWXYboHxofjjtgysgrk P_0, double P_1)
		{
			_ = APoNWtXoeYHyrbDkDswtiTxkfkUL;
		}

		private void xStnTqeXNNLuVqbjECVBheQIpYjL(ref ikfePoAXHchteyoSqLTiXmtbNjbd.tChToPLypbFFSxiImMkXHnPUIRwL P_0)
		{
			if (APoNWtXoeYHyrbDkDswtiTxkfkUL)
			{
				qckZxShqtYgKZSGQmXJLvbtBGAUy(P_0.DKMwsMPTHXggGJzTsupzKYgnfiTp);
				if (DEbuznmERjiCnEQGPtRombzvjfeh != null)
				{
					DEbuznmERjiCnEQGPtRombzvjfeh();
				}
			}
		}

		private void tmvdtQgSLxIHkpXFVEzxWLEZERzE(ref ikfePoAXHchteyoSqLTiXmtbNjbd.tChToPLypbFFSxiImMkXHnPUIRwL P_0)
		{
			if (APoNWtXoeYHyrbDkDswtiTxkfkUL)
			{
				GHLFUYDRbbdpOizTVbmQosBYlpOBb(P_0.DKMwsMPTHXggGJzTsupzKYgnfiTp);
				if (DEbuznmERjiCnEQGPtRombzvjfeh != null)
				{
					DEbuznmERjiCnEQGPtRombzvjfeh();
				}
			}
		}

		private void UZKMNAtlCaqUIsgJeTMhZrtXANEV(ref ikfePoAXHchteyoSqLTiXmtbNjbd.ZEOAmaGwHsGsccptuDaoqLSgaeZab P_0, double P_1)
		{
			if (BUmHgvFAJSaxNqKAkNFxTGAZejPIA && P_0.UtJFkWMjMzuefMSZCUDoBmxxaAue != 6)
			{
				TAZBCtpkDDTJftPZOFwSbmvsVdVsA(P_0.DKMwsMPTHXggGJzTsupzKYgnfiTp, GGbtdUzpXnCzPhpAqObQrrCgdzpo.Axis, P_0.UtJFkWMjMzuefMSZCUDoBmxxaAue, P_0.yYOUbwIbBcyPAQvjeAEXVsjzLAln, P_1);
			}
		}

		private void fAbcVBwBbAeGWTivPzKEUQjfdTjjA(ref ikfePoAXHchteyoSqLTiXmtbNjbd.eXVVMJbpjbLDGKnRQxBArKeojiNbA P_0, double P_1)
		{
			if (BUmHgvFAJSaxNqKAkNFxTGAZejPIA && P_0.oOrIZgDqgkBiIeNHVJdqkqbZWomK != 15)
			{
				TAZBCtpkDDTJftPZOFwSbmvsVdVsA(P_0.DKMwsMPTHXggGJzTsupzKYgnfiTp, GGbtdUzpXnCzPhpAqObQrrCgdzpo.Button, P_0.oOrIZgDqgkBiIeNHVJdqkqbZWomK, P_0.UFCGiNgJQTBJXbozkINBMenjSxAW, P_1);
			}
		}

		private void QxMJfTDDqnmaAXdoNozbymRRMAEf(ref ikfePoAXHchteyoSqLTiXmtbNjbd.ccpFLpGYCrvnKOEpfTBuqyEAScmnA P_0)
		{
			if (BUmHgvFAJSaxNqKAkNFxTGAZejPIA)
			{
				sauBYMcgVeObPsfBYhmYAmBhcfFRA(P_0.DKMwsMPTHXggGJzTsupzKYgnfiTp);
				if (DEbuznmERjiCnEQGPtRombzvjfeh != null)
				{
					DEbuznmERjiCnEQGPtRombzvjfeh();
				}
			}
		}

		private void qotXQOCpSJCmEiNAYaOCvMUFNGSEb(ref ikfePoAXHchteyoSqLTiXmtbNjbd.ccpFLpGYCrvnKOEpfTBuqyEAScmnA P_0)
		{
			if (BUmHgvFAJSaxNqKAkNFxTGAZejPIA)
			{
				UDOqbwjHuFmoHKVRbpUeSRwwxwEP(P_0.DKMwsMPTHXggGJzTsupzKYgnfiTp);
				if (DEbuznmERjiCnEQGPtRombzvjfeh != null)
				{
					DEbuznmERjiCnEQGPtRombzvjfeh();
				}
			}
		}

		private void PdOvnrteRYEkjsTlxcNZOOlZLmvt(ref ikfePoAXHchteyoSqLTiXmtbNjbd.ccpFLpGYCrvnKOEpfTBuqyEAScmnA P_0)
		{
			_ = BUmHgvFAJSaxNqKAkNFxTGAZejPIA;
		}

		private void SujGBkgZLmlPPzTolCwPCiDoNLgC(int P_0, GGbtdUzpXnCzPhpAqObQrrCgdzpo P_1, byte P_2, short P_3, double P_4)
		{
			ppFBkIhRhknvjnBNoaaKkvYBrvifA(P_0)?.tzfYrQaBdsWHACSbQzIxoLZqqtry(P_1, P_2, P_3, P_4);
		}

		private void TAZBCtpkDDTJftPZOFwSbmvsVdVsA(int P_0, GGbtdUzpXnCzPhpAqObQrrCgdzpo P_1, byte P_2, short P_3, double P_4)
		{
			jluJYbeJLOPOOXhJzwEBSEwiSgyc(P_0)?.tzfYrQaBdsWHACSbQzIxoLZqqtry(P_1, P_2, P_3, P_4);
		}

		private void nafLQIiEsnsNNjBRiGfkhykETqeQ()
		{
			string[] array = urUSKRIWlVFAivOKmhLlbhnTAVJz.FneFglMqHMOvRkyVskTSVkGmgngBA();
			if (array == null)
			{
				return;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (!string.IsNullOrEmpty(array[i]) && array[i].Length > 32 && !(ikfePoAXHchteyoSqLTiXmtbNjbd.mTpWAsLSSIXOfUaunGwpSfmMwHTd(new Guid(array[i].Substring(0, 32))) != string.Empty))
				{
					ikfePoAXHchteyoSqLTiXmtbNjbd.XSPrvCkklhOoXCDTxTmnDHtRBbFiA(array[i]);
				}
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~SDL2InputSource()
		{
			Dispose(disposing: false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (AeWaeWamxRrERkciQkpFDWbRfZMkA)
			{
				return;
			}
			if (disposing)
			{
				if (TNIvOvlbHBfBTxffqblcIdyNTeAv != null)
				{
					TNIvOvlbHBfBTxffqblcIdyNTeAv.Dispose();
				}
				NIHulfYmlhxPFhjrJaZNmdqSxxeU();
			}
			ikfePoAXHchteyoSqLTiXmtbNjbd.PSmgpANIYhNGPztBGSVdmJuPmeHF();
			obpzIVquVRQseulcTcTZaHvizTjRA = false;
			AeWaeWamxRrERkciQkpFDWbRfZMkA = true;
		}
	}
}
