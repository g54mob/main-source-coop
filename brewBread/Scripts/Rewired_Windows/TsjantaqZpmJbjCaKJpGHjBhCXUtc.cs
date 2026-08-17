using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using Rewired;
using Rewired.Config;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Libraries.SharpDX.RawInput;
using Rewired.Libraries.SharpDX.Windows.Forms;
using Rewired.Utils;
using UnityEngine;

internal class TsjantaqZpmJbjCaKJpGHjBhCXUtc : IUnifiedKeyboardSource, IDisposable, IGetSetEnabled
{
	private class AsOWrTtbFCPxYjtzdfCvmmzkDRmi
	{
		private enum fUlerQguEvXZGgbXxeQmVZrwApLP
		{
			None = 0,
			Down = 1,
			Up = 2
		}

		private const int vZCeMYJxzapNIgYzbgKbWULUdJyTA = 2;

		private static readonly KeyCode[] SwLlJcxsiUfPMmgYUecgjlsaYRLmA = new KeyCode[2];

		private readonly UpdateLoopType SwvlHouiAxpGNIGJRiRIhvplhZZrA;

		private bool[] ZWjKYwlRQnaLQOLzxkWTBtgjoYPc;

		private bool[] wTpFNghuQUjzqFFJFhfCDmnjotGKc;

		private uint VsVFvODqNDFPGTMISQZITRVWdOAv;

		public AsOWrTtbFCPxYjtzdfCvmmzkDRmi(UpdateLoopType P_0)
		{
			SwvlHouiAxpGNIGJRiRIhvplhZZrA = P_0;
			ZWjKYwlRQnaLQOLzxkWTBtgjoYPc = new bool[132];
			wTpFNghuQUjzqFFJFhfCDmnjotGKc = new bool[132];
		}

		public void zVhDdHImPurLcCtmQAZyuNCpgEwYA(ZNecuqdorOgCTOYpxtLApfFiXkvKA P_0)
		{
			int num = sCwBwcJghcdECNGeTCkTMKyTGvYl(P_0, SwLlJcxsiUfPMmgYUecgjlsaYRLmA);
			for (int i = 0; i < num; i++)
			{
				int num2 = (int)SwLlJcxsiUfPMmgYUecgjlsaYRLmA[i];
				if (num2 >= 0 && num2 < rBPCmueFJfqSxuIDllJSGCupejfq.Length)
				{
					KeyState tfxSQSbEpohFyiBotCWhdCRGWZRd = P_0.TfxSQSbEpohFyiBotCWhdCRGWZRd;
					bool flag = ((tfxSQSbEpohFyiBotCWhdCRGWZRd == KeyState.KeyFirst || tfxSQSbEpohFyiBotCWhdCRGWZRd == KeyState.SystemKeyDown) ? true : false);
					int num3 = rBPCmueFJfqSxuIDllJSGCupejfq[num2];
					bool num4 = ZWjKYwlRQnaLQOLzxkWTBtgjoYPc[num3];
					ZWjKYwlRQnaLQOLzxkWTBtgjoYPc[num3] = flag;
					if (!num4 && flag)
					{
						wTpFNghuQUjzqFFJFhfCDmnjotGKc[num3] = true;
					}
				}
			}
		}

		public void ieMLFOneTNvwpyelecSLKzRFbIpQ(ControllerDataUpdater P_0)
		{
			bool[] buttonValues = P_0.buttonValues;
			for (int i = 0; i < 132; i++)
			{
				buttonValues[i] = ZWjKYwlRQnaLQOLzxkWTBtgjoYPc[i] || wTpFNghuQUjzqFFJFhfCDmnjotGKc[i];
			}
			PPXvJziBoAcRNKtWRVpDUpMyePREb();
		}

		public void UXsGlgdEPfSLHGRZMVqvnDmuNwHcA()
		{
			PPXvJziBoAcRNKtWRVpDUpMyePREb();
		}

		private void PPXvJziBoAcRNKtWRVpDUpMyePREb()
		{
			if (VsVFvODqNDFPGTMISQZITRVWdOAv != ReInput.absFrame)
			{
				rAjXfayQlUVgggdklCqVIRKjtMhg();
				VsVFvODqNDFPGTMISQZITRVWdOAv = ReInput.absFrame;
			}
		}

		public void rAjXfayQlUVgggdklCqVIRKjtMhg()
		{
			Array.Clear(wTpFNghuQUjzqFFJFhfCDmnjotGKc, 0, 132);
		}

		public void woKyyTBPSBeTJzbsBHrQfbqfolrf()
		{
			Array.Clear(ZWjKYwlRQnaLQOLzxkWTBtgjoYPc, 0, 132);
			Array.Clear(wTpFNghuQUjzqFFJFhfCDmnjotGKc, 0, 132);
		}
	}

	private const int NftkrjAQAsVBWwhzBcKmetcCKQTfA = 132;

	private const int DVYxeATTVvGPjKZIvmviEuHAKqdJ = 256;

	private readonly object ifhEwCkIVuTGOnfpubaTrgsqjFiQ = new object();

	private UpdateLoopDataSet<AsOWrTtbFCPxYjtzdfCvmmzkDRmi> QVREeTVFnkMxcshLfAilBnFGqurw;

	private HardwareControllerMap_Game YCOJSLmiUXWAYbZCerZTfDshiiq;

	private bool rtzCYjEVafvMDStvvUswFKRWbCcp;

	private int ZbGFYqQWGvjSPkFGlWESiqlHFYnZA;

	private bool[] gUjxDtOWkfREoHGOpfuaFhHBCnIHb = new bool[256];

	private readonly ZNecuqdorOgCTOYpxtLApfFiXkvKA kYGqLGSpWDzhtLJZipSCCucidqao = new ZNecuqdorOgCTOYpxtLApfFiXkvKA();

	private bool bygchBiNKpgguCZJXkLMcmYodbqaA;

	private static readonly int[] rBPCmueFJfqSxuIDllJSGCupejfq;

	private static readonly int nouqLhBMMwAaPfQwMvRcbalcMpBD;

	private bool NchdYNbKzqsssgcQJdenZuGqXgLo;

	private static IntPtr pPRcwBvkzxddelndCMkHjOArgbbiA;

	private static vQxfanyYJnehFMFEjGpxIpuLKpqg.jjJtlGNMTGHJbirWcDLCGlAeYbHk nQUApixvtzIOEpoHofVXbThyGkkxA;

	private static readonly int[] masHRyPieaTzASXwREBBnfDUfoQDA;

	private static Dictionary<int, Dictionary<int, KeyCode>> YXgTSTQOZFHUaFjxYBpajyKHCvYfb;

	private static readonly int[] gIqdunMGSWiAXzlzwgKkEgnPcpoT;

	public bool enabled
	{
		get
		{
			return bygchBiNKpgguCZJXkLMcmYodbqaA;
		}
		set
		{
			if (bygchBiNKpgguCZJXkLMcmYodbqaA != value)
			{
				bygchBiNKpgguCZJXkLMcmYodbqaA = value;
			}
		}
	}

	public InputSource inputSource => InputSource.RawInput;

	public HardwareControllerMap_Game hardwareMap
	{
		get
		{
			if (YCOJSLmiUXWAYbZCerZTfDshiiq == null)
			{
				YCOJSLmiUXWAYbZCerZTfDshiiq = MElmyMZScdpAijYFRRwUzvkpPpAv();
			}
			return YCOJSLmiUXWAYbZCerZTfDshiiq;
		}
	}

	public int buttonCount => 132;

	public Controller.Extension controllerExtension => null;

	static TsjantaqZpmJbjCaKJpGHjBhCXUtc()
	{
		nQUApixvtzIOEpoHofVXbThyGkkxA = vQxfanyYJnehFMFEjGpxIpuLKpqg.jjJtlGNMTGHJbirWcDLCGlAeYbHk.United_States_English;
		masHRyPieaTzASXwREBBnfDUfoQDA = (int[])Enum.GetValues(typeof(vQxfanyYJnehFMFEjGpxIpuLKpqg.jjJtlGNMTGHJbirWcDLCGlAeYbHk));
		YXgTSTQOZFHUaFjxYBpajyKHCvYfb = new Dictionary<int, Dictionary<int, KeyCode>>
		{
			{
				1033,
				new Dictionary<int, KeyCode>
				{
					{
						222,
						KeyCode.Quote
					},
					{
						188,
						KeyCode.Comma
					},
					{
						189,
						KeyCode.Minus
					},
					{
						190,
						KeyCode.Period
					},
					{
						191,
						KeyCode.Slash
					},
					{
						186,
						KeyCode.Semicolon
					},
					{
						187,
						KeyCode.Equals
					},
					{
						219,
						KeyCode.LeftBracket
					},
					{
						220,
						KeyCode.Backslash
					},
					{
						221,
						KeyCode.RightBracket
					},
					{
						192,
						KeyCode.BackQuote
					},
					{
						223,
						KeyCode.BackQuote
					}
				}
			},
			{
				2057,
				new Dictionary<int, KeyCode>
				{
					{
						223,
						KeyCode.BackQuote
					},
					{
						192,
						KeyCode.Quote
					}
				}
			},
			{
				1106,
				new Dictionary<int, KeyCode>
				{
					{
						223,
						KeyCode.BackQuote
					},
					{
						192,
						KeyCode.Quote
					}
				}
			},
			{
				1031,
				new Dictionary<int, KeyCode>
				{
					{
						219,
						KeyCode.Backslash
					},
					{
						221,
						KeyCode.BackQuote
					}
				}
			}
		};
		gIqdunMGSWiAXzlzwgKkEgnPcpoT = new int[22]
		{
			186, 191, 192, 219, 220, 221, 222, 223, 226, 226,
			254, 221, 188, 189, 219, 190, 220, 187, 191, 222,
			186, 192
		};
		int[] keyboardKeyValues = Consts._keyboardKeyValues;
		int num = keyboardKeyValues.Length;
		for (int i = 0; i < num; i++)
		{
			if (keyboardKeyValues[i] > nouqLhBMMwAaPfQwMvRcbalcMpBD)
			{
				nouqLhBMMwAaPfQwMvRcbalcMpBD = keyboardKeyValues[i];
			}
		}
		rBPCmueFJfqSxuIDllJSGCupejfq = new int[nouqLhBMMwAaPfQwMvRcbalcMpBD + 1];
		ArrayTools.Fill(rBPCmueFJfqSxuIDllJSGCupejfq, -1);
		for (int j = 0; j < num; j++)
		{
			rBPCmueFJfqSxuIDllJSGCupejfq[keyboardKeyValues[j]] = j;
		}
	}

	public TsjantaqZpmJbjCaKJpGHjBhCXUtc(UpdateLoopSetting P_0)
	{
		oainXtfsDHsThgBAcsZwCIsprygU();
		QVREeTVFnkMxcshLfAilBnFGqurw = new UpdateLoopDataSet<AsOWrTtbFCPxYjtzdfCvmmzkDRmi>(P_0);
		using (TempListPool.TList<UpdateLoopType> tList = TempListPool.GetTList<UpdateLoopType>(3))
		{
			List<UpdateLoopType> list = tList.list;
			EnumConverter.ToUpdateLoopTypes(P_0, list);
			for (int i = 0; i < list.Count; i++)
			{
				QVREeTVFnkMxcshLfAilBnFGqurw[i] = new AsOWrTtbFCPxYjtzdfCvmmzkDRmi(list[i]);
			}
		}
		rtzCYjEVafvMDStvvUswFKRWbCcp = ReInput.IsInputAllowed(ControllerType.Keyboard);
		enabled = true;
		ReInput.ApplicationFocusChangedEvent += avyeaphbZibEAshhtYkwPMXPQIgq;
		ReInput.EditorPauseChangedEvent += pDYfMVFoAOSNVbRGpQChmkUUIjrv;
		ReInput.UpdateEndedEvent += OgmPqENlNCgyBQmzcAocbaOTDmQj;
		ReInput.TimeScalePauseChangedEvent += FgSLXZBjBCJWqEXhRfqbKXTvMlpe;
	}

	public unsafe void mPVLsAWcaTKfbIPORRXexWhffTDm(UpdateLoopType P_0)
	{
		QVREeTVFnkMxcshLfAilBnFGqurw.SetUpdateLoop(P_0);
		rtzCYjEVafvMDStvvUswFKRWbCcp = ReInput.IsInputAllowed(ControllerType.Keyboard);
		lock (ifhEwCkIVuTGOnfpubaTrgsqjFiQ)
		{
			try
			{
				byte* ptr = stackalloc byte[256];
				if (!hUfdZejvJYlOtHWdillPtEZZfcOAA.WBRfNwKGANBNqdomITSeotxcRSyOA((IntPtr)ptr))
				{
					return;
				}
				for (int i = 0; i < 256; i++)
				{
					switch (i)
					{
					case 1:
					case 2:
					case 4:
					case 5:
					case 6:
					case 16:
					case 17:
					case 18:
					case 65536:
					case 131072:
						continue;
					}
					if ((ptr[i] & 0x80) == 0)
					{
						if (gUjxDtOWkfREoHGOpfuaFhHBCnIHb[i])
						{
							kYGqLGSpWDzhtLJZipSCCucidqao.QXdiXCMNmtICDXJiLzJcnlEDgLLHA();
							kYGqLGSpWDzhtLJZipSCCucidqao.MJdDiECDHUfxfbOtmAXdwUYFcFVk = ReInput.realTime;
							kYGqLGSpWDzhtLJZipSCCucidqao.yJgdRbTGdtMerEkKyqvlcAQQKALn = IntPtr.Zero;
							kYGqLGSpWDzhtLJZipSCCucidqao.dIZEGTkBlsFKhTipifdHNJqcAiBjA = (Keys)i;
							kYGqLGSpWDzhtLJZipSCCucidqao.ZScKzKzOtTsACiQevdIwgAUVTWKP = 0;
							kYGqLGSpWDzhtLJZipSCCucidqao.CpjaomfEmCQxAfNIHdYHnwmSnUAW = ScanCodeFlags.Break;
							kYGqLGSpWDzhtLJZipSCCucidqao.TfxSQSbEpohFyiBotCWhdCRGWZRd = KeyState.KeyUp;
							kYGqLGSpWDzhtLJZipSCCucidqao.BejYRFXYWCaBKvpnUYEToHQVYGFv = 0;
							QSrdMgzEIZiOTuryCFRIEhNVtPjsA(kYGqLGSpWDzhtLJZipSCCucidqao);
						}
					}
					else if (!gUjxDtOWkfREoHGOpfuaFhHBCnIHb[i])
					{
						kYGqLGSpWDzhtLJZipSCCucidqao.QXdiXCMNmtICDXJiLzJcnlEDgLLHA();
						kYGqLGSpWDzhtLJZipSCCucidqao.MJdDiECDHUfxfbOtmAXdwUYFcFVk = ReInput.realTime;
						kYGqLGSpWDzhtLJZipSCCucidqao.yJgdRbTGdtMerEkKyqvlcAQQKALn = IntPtr.Zero;
						kYGqLGSpWDzhtLJZipSCCucidqao.dIZEGTkBlsFKhTipifdHNJqcAiBjA = (Keys)i;
						kYGqLGSpWDzhtLJZipSCCucidqao.ZScKzKzOtTsACiQevdIwgAUVTWKP = 0;
						kYGqLGSpWDzhtLJZipSCCucidqao.CpjaomfEmCQxAfNIHdYHnwmSnUAW = ScanCodeFlags.Make;
						kYGqLGSpWDzhtLJZipSCCucidqao.TfxSQSbEpohFyiBotCWhdCRGWZRd = KeyState.KeyFirst;
						kYGqLGSpWDzhtLJZipSCCucidqao.BejYRFXYWCaBKvpnUYEToHQVYGFv = 0;
						QSrdMgzEIZiOTuryCFRIEhNVtPjsA(kYGqLGSpWDzhtLJZipSCCucidqao);
					}
				}
			}
			catch
			{
			}
		}
	}

	public void QSrdMgzEIZiOTuryCFRIEhNVtPjsA(ZNecuqdorOgCTOYpxtLApfFiXkvKA P_0)
	{
		if (!rtzCYjEVafvMDStvvUswFKRWbCcp)
		{
			return;
		}
		switch (P_0.dIZEGTkBlsFKhTipifdHNJqcAiBjA)
		{
		case Keys.ControlKey:
		{
			Keys keys = (Keys)hUfdZejvJYlOtHWdillPtEZZfcOAA.dTHamguEmaecjVsZKWWQZXqaYLOh((uint)P_0.ZScKzKzOtTsACiQevdIwgAUVTWKP, vQxfanyYJnehFMFEjGpxIpuLKpqg.GKkbansLMSQgcHJxeXciepmlqrKU);
			if (keys != Keys.LControlKey && keys != Keys.RControlKey)
			{
				return;
			}
			P_0.dIZEGTkBlsFKhTipifdHNJqcAiBjA = (((P_0.CpjaomfEmCQxAfNIHdYHnwmSnUAW & ScanCodeFlags.E0) != ScanCodeFlags.Make) ? Keys.RControlKey : Keys.LControlKey);
			break;
		}
		case Keys.Menu:
			P_0.dIZEGTkBlsFKhTipifdHNJqcAiBjA = (((P_0.CpjaomfEmCQxAfNIHdYHnwmSnUAW & ScanCodeFlags.E0) != ScanCodeFlags.Make) ? Keys.RMenu : Keys.LMenu);
			break;
		case Keys.ShiftKey:
		{
			P_0.dIZEGTkBlsFKhTipifdHNJqcAiBjA = (Keys)hUfdZejvJYlOtHWdillPtEZZfcOAA.dTHamguEmaecjVsZKWWQZXqaYLOh((uint)P_0.ZScKzKzOtTsACiQevdIwgAUVTWKP, vQxfanyYJnehFMFEjGpxIpuLKpqg.GKkbansLMSQgcHJxeXciepmlqrKU);
			if (P_0.dIZEGTkBlsFKhTipifdHNJqcAiBjA == Keys.LShiftKey || P_0.dIZEGTkBlsFKhTipifdHNJqcAiBjA == Keys.RShiftKey)
			{
				break;
			}
			KeyState tfxSQSbEpohFyiBotCWhdCRGWZRd = P_0.TfxSQSbEpohFyiBotCWhdCRGWZRd;
			bool flag = ((tfxSQSbEpohFyiBotCWhdCRGWZRd == KeyState.KeyFirst || tfxSQSbEpohFyiBotCWhdCRGWZRd == KeyState.SystemKeyDown || tfxSQSbEpohFyiBotCWhdCRGWZRd == KeyState.KeyLast) ? true : false);
			bool flag2 = (hUfdZejvJYlOtHWdillPtEZZfcOAA.WKSBsWJmlgVnqTuukSAoGjnUBIMe(160) & 0x8000) != 0;
			bool flag3 = (hUfdZejvJYlOtHWdillPtEZZfcOAA.WKSBsWJmlgVnqTuukSAoGjnUBIMe(161) & 0x8000) != 0;
			if (flag)
			{
				bool num = (hUfdZejvJYlOtHWdillPtEZZfcOAA.veqWkKOWloyHtLljMViwvgYSEeJJ(160) & 0x8000) != 0;
				bool flag4 = (hUfdZejvJYlOtHWdillPtEZZfcOAA.veqWkKOWloyHtLljMViwvgYSEeJJ(161) & 0x8000) != 0;
				if (num)
				{
					P_0.dIZEGTkBlsFKhTipifdHNJqcAiBjA = Keys.LShiftKey;
					QSrdMgzEIZiOTuryCFRIEhNVtPjsA(P_0);
				}
				if (flag4)
				{
					P_0.dIZEGTkBlsFKhTipifdHNJqcAiBjA = Keys.RShiftKey;
					QSrdMgzEIZiOTuryCFRIEhNVtPjsA(P_0);
				}
				return;
			}
			if (flag2 && flag3)
			{
				return;
			}
			if (flag2)
			{
				P_0.dIZEGTkBlsFKhTipifdHNJqcAiBjA = Keys.LShiftKey;
				break;
			}
			if (flag3)
			{
				P_0.dIZEGTkBlsFKhTipifdHNJqcAiBjA = Keys.RShiftKey;
				break;
			}
			P_0.dIZEGTkBlsFKhTipifdHNJqcAiBjA = Keys.LShiftKey;
			QSrdMgzEIZiOTuryCFRIEhNVtPjsA(P_0);
			P_0.dIZEGTkBlsFKhTipifdHNJqcAiBjA = Keys.RShiftKey;
			QSrdMgzEIZiOTuryCFRIEhNVtPjsA(P_0);
			return;
		}
		}
		lock (ifhEwCkIVuTGOnfpubaTrgsqjFiQ)
		{
			KeyState tfxSQSbEpohFyiBotCWhdCRGWZRd = P_0.TfxSQSbEpohFyiBotCWhdCRGWZRd;
			if (tfxSQSbEpohFyiBotCWhdCRGWZRd == KeyState.KeyFirst || tfxSQSbEpohFyiBotCWhdCRGWZRd == KeyState.SystemKeyDown)
			{
				gUjxDtOWkfREoHGOpfuaFhHBCnIHb[(int)P_0.dIZEGTkBlsFKhTipifdHNJqcAiBjA] = true;
			}
			else
			{
				gUjxDtOWkfREoHGOpfuaFhHBCnIHb[(int)P_0.dIZEGTkBlsFKhTipifdHNJqcAiBjA] = false;
			}
			int count = QVREeTVFnkMxcshLfAilBnFGqurw.Count;
			for (int i = 0; i < count; i++)
			{
				QVREeTVFnkMxcshLfAilBnFGqurw[i].zVhDdHImPurLcCtmQAZyuNCpgEwYA(P_0);
			}
		}
	}

	public void RHgfvdegRaclghLQGGXSxvRjTVmJA(bool P_0)
	{
		hUOvVcEXTZabBXDtcXaOnWiiuOjK();
	}

	public void gGXcVSZRUaZCAvwMZVjHsmeVTsJJ(bool P_0)
	{
		if (oainXtfsDHsThgBAcsZwCIsprygU() < 0)
		{
			hUOvVcEXTZabBXDtcXaOnWiiuOjK();
		}
	}

	private int oainXtfsDHsThgBAcsZwCIsprygU()
	{
		int zbGFYqQWGvjSPkFGlWESiqlHFYnZA = ZbGFYqQWGvjSPkFGlWESiqlHFYnZA;
		if (pYDDNhBibHfSZzyMuFjJSOUJKDWk.UFYWIAYzCAWXNVscTkIIdREHdVye(thGpvMJraEkCGcLLHOTxuArPRrdh.Keyboard, out var zbGFYqQWGvjSPkFGlWESiqlHFYnZA2))
		{
			ZbGFYqQWGvjSPkFGlWESiqlHFYnZA = zbGFYqQWGvjSPkFGlWESiqlHFYnZA2;
		}
		else
		{
			ZbGFYqQWGvjSPkFGlWESiqlHFYnZA = 1;
		}
		return ZbGFYqQWGvjSPkFGlWESiqlHFYnZA - zbGFYqQWGvjSPkFGlWESiqlHFYnZA;
	}

	private void avyeaphbZibEAshhtYkwPMXPQIgq(bool P_0)
	{
		rtzCYjEVafvMDStvvUswFKRWbCcp = ReInput.IsInputAllowed(ControllerType.Keyboard);
		if (!P_0 && !rtzCYjEVafvMDStvvUswFKRWbCcp)
		{
			hUOvVcEXTZabBXDtcXaOnWiiuOjK();
		}
	}

	private void pDYfMVFoAOSNVbRGpQChmkUUIjrv(bool P_0)
	{
	}

	private void FgSLXZBjBCJWqEXhRfqbKXTvMlpe(bool P_0)
	{
		if ((ReInput.configVars.updateLoop & UpdateLoopSetting.FixedUpdate) == 0)
		{
			return;
		}
		rtzCYjEVafvMDStvvUswFKRWbCcp = ReInput.IsInputAllowed(ControllerType.Keyboard);
		lock (ifhEwCkIVuTGOnfpubaTrgsqjFiQ)
		{
			QVREeTVFnkMxcshLfAilBnFGqurw[QVREeTVFnkMxcshLfAilBnFGqurw.fixedUpdateSetIndex].rAjXfayQlUVgggdklCqVIRKjtMhg();
		}
	}

	private void OgmPqENlNCgyBQmzcAocbaOTDmQj(UpdateLoopType P_0)
	{
		lock (ifhEwCkIVuTGOnfpubaTrgsqjFiQ)
		{
			QVREeTVFnkMxcshLfAilBnFGqurw.Get(P_0).UXsGlgdEPfSLHGRZMVqvnDmuNwHcA();
		}
	}

	private void hUOvVcEXTZabBXDtcXaOnWiiuOjK()
	{
		lock (ifhEwCkIVuTGOnfpubaTrgsqjFiQ)
		{
			int count = QVREeTVFnkMxcshLfAilBnFGqurw.Count;
			for (int i = 0; i < count; i++)
			{
				QVREeTVFnkMxcshLfAilBnFGqurw[i].woKyyTBPSBeTJzbsBHrQfbqfolrf();
			}
		}
	}

	public void UpdateInputData(ControllerDataUpdater dataUpdater)
	{
		QVREeTVFnkMxcshLfAilBnFGqurw.Current.ieMLFOneTNvwpyelecSLKzRFbIpQ(dataUpdater);
	}

	public void Clear()
	{
		hUOvVcEXTZabBXDtcXaOnWiiuOjK();
	}

	private static HardwareControllerMap_Game MElmyMZScdpAijYFRRwUzvkpPpAv()
	{
		ControllerElementIdentifier[] array = new ControllerElementIdentifier[132];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new ControllerElementIdentifier(i, Consts.keyboardKeyNames[i], Consts.keyboardKeyNames[i], string.Empty, ControllerElementType.Button, true);
		}
		int[] array2 = new int[132];
		for (int j = 0; j < 132; j++)
		{
			array2[j] = array[j].id;
		}
		HardwareButtonInfo[] array3 = new HardwareButtonInfo[132];
		for (int k = 0; k < 132; k++)
		{
			array3[k] = new HardwareButtonInfo();
		}
		return new HardwareControllerMap_Game("Keyboard", default(HardwareControllerMapIdentifier), array, array2, new int[0], new AxisCalibrationData[0], new AxisRange[0], new HardwareAxisInfo[0], array3, null);
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
			ReInput.ApplicationFocusChangedEvent -= avyeaphbZibEAshhtYkwPMXPQIgq;
			ReInput.EditorPauseChangedEvent -= pDYfMVFoAOSNVbRGpQChmkUUIjrv;
			ReInput.UpdateEndedEvent -= OgmPqENlNCgyBQmzcAocbaOTDmQj;
			ReInput.TimeScalePauseChangedEvent -= FgSLXZBjBCJWqEXhRfqbKXTvMlpe;
			NchdYNbKzqsssgcQJdenZuGqXgLo = true;
		}
	}

	public static int sCwBwcJghcdECNGeTCkTMKyTGvYl(ZNecuqdorOgCTOYpxtLApfFiXkvKA P_0, KeyCode[] P_1)
	{
		Keys dIZEGTkBlsFKhTipifdHNJqcAiBjA = P_0.dIZEGTkBlsFKhTipifdHNJqcAiBjA;
		int result = 0;
		vQxfanyYJnehFMFEjGpxIpuLKpqg.jjJtlGNMTGHJbirWcDLCGlAeYbHk jjJtlGNMTGHJbirWcDLCGlAeYbHk = yFDgDgeydyEPnTLQsbyuzVTXGVSVA();
		_ = pPRcwBvkzxddelndCMkHjOArgbbiA;
		hUfdZejvJYlOtHWdillPtEZZfcOAA.dTHamguEmaecjVsZKWWQZXqaYLOh((uint)P_0.dIZEGTkBlsFKhTipifdHNJqcAiBjA, vQxfanyYJnehFMFEjGpxIpuLKpqg.FUsNfqwJLwbdvSFumPSofSkqOpEt);
		if (HJxSQbifZRPljZUPiinZkGbzDKky(dIZEGTkBlsFKhTipifdHNJqcAiBjA))
		{
			if (PxNkRQAKIiUABPxJZIKkJicOetKDA(dIZEGTkBlsFKhTipifdHNJqcAiBjA, jjJtlGNMTGHJbirWcDLCGlAeYbHk, out var keyCode))
			{
				P_1[result++] = keyCode;
			}
		}
		else
		{
			switch (dIZEGTkBlsFKhTipifdHNJqcAiBjA)
			{
			case Keys.None:
				P_1[result++] = KeyCode.None;
				break;
			case Keys.A:
				P_1[result++] = KeyCode.A;
				break;
			case Keys.B:
				P_1[result++] = KeyCode.B;
				break;
			case Keys.C:
				P_1[result++] = KeyCode.C;
				break;
			case Keys.D:
				P_1[result++] = KeyCode.D;
				break;
			case Keys.E:
				P_1[result++] = KeyCode.E;
				break;
			case Keys.F:
				P_1[result++] = KeyCode.F;
				break;
			case Keys.G:
				P_1[result++] = KeyCode.G;
				break;
			case Keys.H:
				P_1[result++] = KeyCode.H;
				break;
			case Keys.I:
				P_1[result++] = KeyCode.I;
				break;
			case Keys.J:
				P_1[result++] = KeyCode.J;
				break;
			case Keys.K:
				P_1[result++] = KeyCode.K;
				break;
			case Keys.L:
				P_1[result++] = KeyCode.L;
				break;
			case Keys.M:
				P_1[result++] = KeyCode.M;
				break;
			case Keys.N:
				P_1[result++] = KeyCode.N;
				break;
			case Keys.O:
				P_1[result++] = KeyCode.O;
				break;
			case Keys.P:
				P_1[result++] = KeyCode.P;
				break;
			case Keys.Q:
				P_1[result++] = KeyCode.Q;
				break;
			case Keys.R:
				P_1[result++] = KeyCode.R;
				break;
			case Keys.S:
				P_1[result++] = KeyCode.S;
				break;
			case Keys.T:
				P_1[result++] = KeyCode.T;
				break;
			case Keys.U:
				P_1[result++] = KeyCode.U;
				break;
			case Keys.V:
				P_1[result++] = KeyCode.V;
				break;
			case Keys.W:
				P_1[result++] = KeyCode.W;
				break;
			case Keys.X:
				P_1[result++] = KeyCode.X;
				break;
			case Keys.Y:
				P_1[result++] = KeyCode.Y;
				break;
			case Keys.Z:
				P_1[result++] = KeyCode.Z;
				break;
			case Keys.D0:
				P_1[result++] = KeyCode.Alpha0;
				break;
			case Keys.D1:
				P_1[result++] = KeyCode.Alpha1;
				break;
			case Keys.D2:
				P_1[result++] = KeyCode.Alpha2;
				break;
			case Keys.D3:
				P_1[result++] = KeyCode.Alpha3;
				break;
			case Keys.D4:
				P_1[result++] = KeyCode.Alpha4;
				break;
			case Keys.D5:
				P_1[result++] = KeyCode.Alpha5;
				break;
			case Keys.D6:
				P_1[result++] = KeyCode.Alpha6;
				break;
			case Keys.D7:
				P_1[result++] = KeyCode.Alpha7;
				break;
			case Keys.D8:
				P_1[result++] = KeyCode.Alpha8;
				break;
			case Keys.D9:
				P_1[result++] = KeyCode.Alpha9;
				break;
			case Keys.NumPad0:
				P_1[result++] = KeyCode.Keypad0;
				break;
			case Keys.NumPad1:
				P_1[result++] = KeyCode.Keypad1;
				break;
			case Keys.NumPad2:
				P_1[result++] = KeyCode.Keypad2;
				break;
			case Keys.NumPad3:
				P_1[result++] = KeyCode.Keypad3;
				break;
			case Keys.NumPad4:
				P_1[result++] = KeyCode.Keypad4;
				break;
			case Keys.NumPad5:
				P_1[result++] = KeyCode.Keypad5;
				break;
			case Keys.NumPad6:
				P_1[result++] = KeyCode.Keypad6;
				break;
			case Keys.NumPad7:
				P_1[result++] = KeyCode.Keypad7;
				break;
			case Keys.NumPad8:
				P_1[result++] = KeyCode.Keypad8;
				break;
			case Keys.NumPad9:
				P_1[result++] = KeyCode.Keypad9;
				break;
			case Keys.Decimal:
				P_1[result++] = KeyCode.KeypadPeriod;
				break;
			case Keys.Divide:
				P_1[result++] = KeyCode.KeypadDivide;
				break;
			case Keys.Multiply:
				P_1[result++] = KeyCode.KeypadMultiply;
				break;
			case Keys.Subtract:
				P_1[result++] = KeyCode.KeypadMinus;
				break;
			case Keys.Add:
				P_1[result++] = KeyCode.KeypadPlus;
				break;
			case Keys.Return:
				if ((P_0.CpjaomfEmCQxAfNIHdYHnwmSnUAW & ScanCodeFlags.E0) != ScanCodeFlags.Make)
				{
					P_1[result++] = KeyCode.KeypadEnter;
				}
				else
				{
					P_1[result++] = KeyCode.Return;
				}
				break;
			case Keys.Back:
				P_1[result++] = KeyCode.Backspace;
				break;
			case Keys.Tab:
				P_1[result++] = KeyCode.Tab;
				break;
			case Keys.Clear:
				P_1[result++] = KeyCode.Clear;
				break;
			case Keys.Pause:
				P_1[result++] = KeyCode.Pause;
				break;
			case Keys.Escape:
				P_1[result++] = KeyCode.Escape;
				break;
			case Keys.Space:
				P_1[result++] = KeyCode.Space;
				break;
			case Keys.Delete:
				P_1[result++] = KeyCode.Delete;
				break;
			case Keys.Up:
				P_1[result++] = KeyCode.UpArrow;
				break;
			case Keys.Down:
				P_1[result++] = KeyCode.DownArrow;
				break;
			case Keys.Right:
				P_1[result++] = KeyCode.RightArrow;
				break;
			case Keys.Left:
				P_1[result++] = KeyCode.LeftArrow;
				break;
			case Keys.Insert:
				P_1[result++] = KeyCode.Insert;
				break;
			case Keys.Home:
				P_1[result++] = KeyCode.Home;
				break;
			case Keys.End:
				P_1[result++] = KeyCode.End;
				break;
			case Keys.Prior:
				P_1[result++] = KeyCode.PageUp;
				break;
			case Keys.Next:
				P_1[result++] = KeyCode.PageDown;
				break;
			case Keys.F1:
				P_1[result++] = KeyCode.F1;
				break;
			case Keys.F2:
				P_1[result++] = KeyCode.F2;
				break;
			case Keys.F3:
				P_1[result++] = KeyCode.F3;
				break;
			case Keys.F4:
				P_1[result++] = KeyCode.F4;
				break;
			case Keys.F5:
				P_1[result++] = KeyCode.F5;
				break;
			case Keys.F6:
				P_1[result++] = KeyCode.F6;
				break;
			case Keys.F7:
				P_1[result++] = KeyCode.F7;
				break;
			case Keys.F8:
				P_1[result++] = KeyCode.F8;
				break;
			case Keys.F9:
				P_1[result++] = KeyCode.F9;
				break;
			case Keys.F10:
				P_1[result++] = KeyCode.F10;
				break;
			case Keys.F11:
				P_1[result++] = KeyCode.F11;
				break;
			case Keys.F12:
				P_1[result++] = KeyCode.F12;
				break;
			case Keys.F13:
				P_1[result++] = KeyCode.F13;
				break;
			case Keys.F14:
				P_1[result++] = KeyCode.F14;
				break;
			case Keys.F15:
				P_1[result++] = KeyCode.F15;
				break;
			case Keys.NumLock:
				P_1[result++] = KeyCode.Numlock;
				break;
			case Keys.Capital:
				P_1[result++] = KeyCode.CapsLock;
				break;
			case Keys.Scroll:
				P_1[result++] = KeyCode.ScrollLock;
				break;
			case Keys.RShiftKey:
				P_1[result++] = KeyCode.RightShift;
				break;
			case Keys.LShiftKey:
				P_1[result++] = KeyCode.LeftShift;
				break;
			case Keys.RControlKey:
				P_1[result++] = KeyCode.RightControl;
				break;
			case Keys.LControlKey:
				P_1[result++] = KeyCode.LeftControl;
				break;
			case Keys.RMenu:
				P_1[result++] = KeyCode.AltGr;
				P_1[result++] = KeyCode.RightAlt;
				break;
			case Keys.LMenu:
				P_1[result++] = KeyCode.LeftAlt;
				break;
			case Keys.RWin:
				P_1[result++] = KeyCode.RightCommand;
				break;
			case Keys.LWin:
				P_1[result++] = KeyCode.LeftCommand;
				break;
			case Keys.Help:
				P_1[result++] = KeyCode.Help;
				break;
			case Keys.Print:
				P_1[result++] = KeyCode.Print;
				break;
			case Keys.Apps:
				P_1[result++] = KeyCode.Menu;
				break;
			}
		}
		return result;
	}

	private unsafe static vQxfanyYJnehFMFEjGpxIpuLKpqg.jjJtlGNMTGHJbirWcDLCGlAeYbHk yFDgDgeydyEPnTLQsbyuzVTXGVSVA()
	{
		IntPtr intPtr = hUfdZejvJYlOtHWdillPtEZZfcOAA.xVinStRXVsoEQAHEqKYJhotMMwUE(0);
		if (intPtr == pPRcwBvkzxddelndCMkHjOArgbbiA)
		{
			return nQUApixvtzIOEpoHofVXbThyGkkxA;
		}
		vQxfanyYJnehFMFEjGpxIpuLKpqg.jjJtlGNMTGHJbirWcDLCGlAeYbHk result = vQxfanyYJnehFMFEjGpxIpuLKpqg.jjJtlGNMTGHJbirWcDLCGlAeYbHk.United_States_English;
		byte* intPtr2 = stackalloc byte[128];
		hUfdZejvJYlOtHWdillPtEZZfcOAA.MWKAPeYxRKQNMHPETsyGSUaYEaph((IntPtr)intPtr2);
		if (int.TryParse(Marshal.PtrToStringUni((IntPtr)intPtr2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result2))
		{
			int num = ArrayTools.IndexOf(masHRyPieaTzASXwREBBnfDUfoQDA, result2);
			if (num >= 0)
			{
				result = (vQxfanyYJnehFMFEjGpxIpuLKpqg.jjJtlGNMTGHJbirWcDLCGlAeYbHk)masHRyPieaTzASXwREBBnfDUfoQDA[num];
			}
		}
		pPRcwBvkzxddelndCMkHjOArgbbiA = intPtr;
		nQUApixvtzIOEpoHofVXbThyGkkxA = result;
		return result;
	}

	private static bool PxNkRQAKIiUABPxJZIKkJicOetKDA(Keys P_0, vQxfanyYJnehFMFEjGpxIpuLKpqg.jjJtlGNMTGHJbirWcDLCGlAeYbHk P_1, out KeyCode P_2)
	{
		P_2 = KeyCode.None;
		if (!YXgTSTQOZFHUaFjxYBpajyKHCvYfb.TryGetValue((int)P_1, out var value))
		{
			value = YXgTSTQOZFHUaFjxYBpajyKHCvYfb[1033];
		}
		bool flag = value.TryGetValue((int)P_0, out P_2);
		if (!flag && P_1 != vQxfanyYJnehFMFEjGpxIpuLKpqg.jjJtlGNMTGHJbirWcDLCGlAeYbHk.United_States_English)
		{
			value = YXgTSTQOZFHUaFjxYBpajyKHCvYfb[1033];
			flag = value.TryGetValue((int)P_0, out P_2);
		}
		return flag;
	}

	private static bool HJxSQbifZRPljZUPiinZkGbzDKky(Keys P_0)
	{
		return ArrayTools.Contains(gIqdunMGSWiAXzlzwgKkEgnPcpoT, (int)P_0);
	}
}
