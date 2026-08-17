using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using Rewired;
using Rewired.Config;
using Rewired.Data;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;

internal sealed class pmhdIRhCoLlQZffSYgOmsLkpxsjN : IDisposable
{
	public enum cRwcHsJJSwdOXPswvlQsCSGPrDri
	{
		Connected = 0,
		Disconnected = 1
	}

	private class PJMZIbSFLOAsjEebtpNfFsEvvMVbb
	{
		public ADictionary<int, InputBehavior> gtOwGaSStjjjqPkwkkQpbgQQomCn;

		public List<InputBehavior> MkIQcLiiGSSdDGDnhPcZzTRYKLoi;

		public IList<InputBehavior> REVMeXqqKzfxWRkzieBTIdVcitWF;

		public PJMZIbSFLOAsjEebtpNfFsEvvMVbb(List<InputBehavior> P_0)
		{
			MkIQcLiiGSSdDGDnhPcZzTRYKLoi = new List<InputBehavior>(P_0.Count);
			gtOwGaSStjjjqPkwkkQpbgQQomCn = new ADictionary<int, InputBehavior>();
			int num = 0;
			for (int i = 0; i < P_0.Count; i++)
			{
				InputBehavior inputBehavior = P_0[i].Clone();
				gtOwGaSStjjjqPkwkkQpbgQQomCn.Add(P_0[i].id, inputBehavior);
				MkIQcLiiGSSdDGDnhPcZzTRYKLoi.Add(inputBehavior);
				num++;
			}
			REVMeXqqKzfxWRkzieBTIdVcitWF = new ReadOnlyCollection<InputBehavior>(MkIQcLiiGSSdDGDnhPcZzTRYKLoi);
		}

		public InputBehavior rtrPDnldpcAJqYNVDRuNTPUDYHqF(int P_0)
		{
			if (MkIQcLiiGSSdDGDnhPcZzTRYKLoi.Count == 0)
			{
				return null;
			}
			gtOwGaSStjjjqPkwkkQpbgQQomCn.TryGetValue(P_0, out var value);
			if (value == null)
			{
				return MkIQcLiiGSSdDGDnhPcZzTRYKLoi[0];
			}
			return value;
		}
	}

	private sealed class OVlTUCSeRUMfdObDTYgcgZLKedyv : IEnumerable<CustomController>, IDisposable, IEnumerator<CustomController>, IEnumerable, IEnumerator
	{
		private int RxAoyfYzYDsYonLGXsvUgwChukLk;

		private CustomController VqEePGSMyrGKIqkWibsjeHcWPSIx;

		private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

		public pmhdIRhCoLlQZffSYgOmsLkpxsjN TtytLoUfsgUyhsklaKccrnoMiiek;

		private int ksLEMLchGFQdNnoxKqqFSBMtwKHz;

		public int xqTcIBbgIpiyNCXcOBWDlWIEYFYDA;

		private int FoUBVFvJgCwuxjyvzlOOmezIEkFS;

		private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

		CustomController IEnumerator<CustomController>.Current
		{
			[DebuggerHidden]
			get
			{
				return VqEePGSMyrGKIqkWibsjeHcWPSIx;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return VqEePGSMyrGKIqkWibsjeHcWPSIx;
			}
		}

		[DebuggerHidden]
		public OVlTUCSeRUMfdObDTYgcgZLKedyv(int P_0)
		{
			RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
			wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
			pmhdIRhCoLlQZffSYgOmsLkpxsjN ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
			if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
			{
				if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
				{
					return false;
				}
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				goto IL_007d;
			}
			RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
			FoUBVFvJgCwuxjyvzlOOmezIEkFS = ttytLoUfsgUyhsklaKccrnoMiiek.vUFoteRIgwMjMHesIRSyNjZtxfgn.Count;
			fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
			goto IL_008d;
			IL_007d:
			fIMVaffCgsuIJcnrkMmGGKfPwwel++;
			goto IL_008d;
			IL_008d:
			if (fIMVaffCgsuIJcnrkMmGGKfPwwel < FoUBVFvJgCwuxjyvzlOOmezIEkFS)
			{
				if (ttytLoUfsgUyhsklaKccrnoMiiek.vUFoteRIgwMjMHesIRSyNjZtxfgn[fIMVaffCgsuIJcnrkMmGGKfPwwel].sourceControllerId == ksLEMLchGFQdNnoxKqqFSBMtwKHz)
				{
					VqEePGSMyrGKIqkWibsjeHcWPSIx = ttytLoUfsgUyhsklaKccrnoMiiek.vUFoteRIgwMjMHesIRSyNjZtxfgn[fIMVaffCgsuIJcnrkMmGGKfPwwel];
					RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
					return true;
				}
				goto IL_007d;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<CustomController> IEnumerable<CustomController>.GetEnumerator()
		{
			OVlTUCSeRUMfdObDTYgcgZLKedyv oVlTUCSeRUMfdObDTYgcgZLKedyv;
			if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
				oVlTUCSeRUMfdObDTYgcgZLKedyv = this;
			}
			else
			{
				oVlTUCSeRUMfdObDTYgcgZLKedyv = new OVlTUCSeRUMfdObDTYgcgZLKedyv(0);
				oVlTUCSeRUMfdObDTYgcgZLKedyv.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
			}
			oVlTUCSeRUMfdObDTYgcgZLKedyv.ksLEMLchGFQdNnoxKqqFSBMtwKHz = xqTcIBbgIpiyNCXcOBWDlWIEYFYDA;
			return oVlTUCSeRUMfdObDTYgcgZLKedyv;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<CustomController>)this).GetEnumerator();
		}
	}

	private sealed class KcXqtzOffZGfigpnGLYcUqHCtZINA : IEnumerable<CustomController>, IDisposable, IEnumerator<CustomController>, IEnumerable, IEnumerator
	{
		private int RxAoyfYzYDsYonLGXsvUgwChukLk;

		private CustomController VqEePGSMyrGKIqkWibsjeHcWPSIx;

		private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

		public pmhdIRhCoLlQZffSYgOmsLkpxsjN TtytLoUfsgUyhsklaKccrnoMiiek;

		private string eBfDTHTrxuRklzIaThUTCseJJiEeb;

		public string NgUGOCgMAPNuMsxXPWsrQJXFnwujA;

		private int FoUBVFvJgCwuxjyvzlOOmezIEkFS;

		private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

		CustomController IEnumerator<CustomController>.Current
		{
			[DebuggerHidden]
			get
			{
				return VqEePGSMyrGKIqkWibsjeHcWPSIx;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return VqEePGSMyrGKIqkWibsjeHcWPSIx;
			}
		}

		[DebuggerHidden]
		public KcXqtzOffZGfigpnGLYcUqHCtZINA(int P_0)
		{
			RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
			wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
			pmhdIRhCoLlQZffSYgOmsLkpxsjN ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
			if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
			{
				if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
				{
					return false;
				}
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				goto IL_0083;
			}
			RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
			FoUBVFvJgCwuxjyvzlOOmezIEkFS = ttytLoUfsgUyhsklaKccrnoMiiek.vUFoteRIgwMjMHesIRSyNjZtxfgn.Count;
			fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
			goto IL_0093;
			IL_0083:
			fIMVaffCgsuIJcnrkMmGGKfPwwel++;
			goto IL_0093;
			IL_0093:
			if (fIMVaffCgsuIJcnrkMmGGKfPwwel < FoUBVFvJgCwuxjyvzlOOmezIEkFS)
			{
				if (ttytLoUfsgUyhsklaKccrnoMiiek.vUFoteRIgwMjMHesIRSyNjZtxfgn[fIMVaffCgsuIJcnrkMmGGKfPwwel].tag.Equals(eBfDTHTrxuRklzIaThUTCseJJiEeb, StringComparison.OrdinalIgnoreCase))
				{
					VqEePGSMyrGKIqkWibsjeHcWPSIx = ttytLoUfsgUyhsklaKccrnoMiiek.vUFoteRIgwMjMHesIRSyNjZtxfgn[fIMVaffCgsuIJcnrkMmGGKfPwwel];
					RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
					return true;
				}
				goto IL_0083;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<CustomController> IEnumerable<CustomController>.GetEnumerator()
		{
			KcXqtzOffZGfigpnGLYcUqHCtZINA kcXqtzOffZGfigpnGLYcUqHCtZINA;
			if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
				kcXqtzOffZGfigpnGLYcUqHCtZINA = this;
			}
			else
			{
				kcXqtzOffZGfigpnGLYcUqHCtZINA = new KcXqtzOffZGfigpnGLYcUqHCtZINA(0);
				kcXqtzOffZGfigpnGLYcUqHCtZINA.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
			}
			kcXqtzOffZGfigpnGLYcUqHCtZINA.eBfDTHTrxuRklzIaThUTCseJJiEeb = NgUGOCgMAPNuMsxXPWsrQJXFnwujA;
			return kcXqtzOffZGfigpnGLYcUqHCtZINA;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<CustomController>)this).GetEnumerator();
		}
	}

	private List<Joystick> YYasSHCtpzGGHqjStqqVkdXZiWHH;

	private List<Joystick> hXYIgyCpgDGjSSuuYxDApkRKvfGq;

	private List<CustomController> vUFoteRIgwMjMHesIRSyNjZtxfgn;

	private List<Controller> xLmeyCEvHjFDowSsezuNCUAMXWmL;

	private ReadOnlyCollection<Controller> zAapaVyddVmRriQiWinbIMUoxoDW;

	private Keyboard OlktKbOjbgGrTJEsAkUzsiYeMHYb;

	private Mouse dEcTjrSkAbFekEIeQDAfncJOVAhdA;

	private ConfigVars dNgrfdLJdsfeJmlYYJJiwGpIkluS;

	private ItaEjuKtoATtafYloiocFOanqPQc[] ibSPyLhzomBLVYeqxJcktMghzOgj;

	private ItaEjuKtoATtafYloiocFOanqPQc[] tgNELAiNXZMjZMcUZhoHllhenZUfA;

	private ItaEjuKtoATtafYloiocFOanqPQc[,] OUADEDsGMLnIILsOyroWSEdclIJD;

	private YsOrrlNIumifyOKxnwVmvgJjFiYZ tDEfcwuerNXHdNWhqfUXcHaLJUBh;

	private gTyYgmXURpDluqEDVAzYahaASSNCb YIYErldVrijfYfrevxuAbluBNlnI;

	private gTyYgmXURpDluqEDVAzYahaASSNCb[] RfSHmwHwAbxbYEnRZzPmlzoVqRaV;

	private KxFbllcCGygjmUeLFqyAeiTIGWMf<ActiveControllerChangedDelegate> fyHVWbKFunKAyypmzpTWUaRdcnKjA;

	private KxFbllcCGygjmUeLFqyAeiTIGWMf<PlayerActiveControllerChangedDelegate> xiubogzFXvIuQeBbfawPsDyKfmce;

	private KxFbllcCGygjmUeLFqyAeiTIGWMf<PlayerActiveControllerChangedDelegate>[] diKlwWVZnsNvkfCPbFhcQDkJDIsO;

	private ADictionary<int, PJMZIbSFLOAsjEebtpNfFsEvvMVbb> hmyGBQfyaWazZLqcjhHmFczCcGjfB;

	private readonly jwwEVOVklpzvRHhBdNqsduEELqpv sOyJxxGPTWlVyzPJIvrZyYyRatmE;

	private IList<Joystick> EZkeaGNjmfUUVrykZmLhuXIZgwOf;

	private IList<CustomController> YhufnUJBrjmuLslaygcAwWOOSGscA;

	private int dVSeywEvtXvpViovnwUfHUKkKgtbb;

	private bool sXLLyWhVtnTsJUdLYDsshzdInxBV;

	private bool hnvMKVcKgNVmSXgWGXLrQdCHStBg;

	private bool nzKAgdkNjptUfxogWPtGXJvoIweb;

	private IUnifiedKeyboardSource tSzApJiwheMYhjgvAaTrNbSHdHgP;

	private IUnifiedMouseSource CDQvdxrBYeaibeuILXSWZTLflVSS;

	private int IagzVHFlWCLJUEfGubXiUScVfGmCA;

	private nWObnmDQDyPAcKuVnyCIqGPjixDdb GAwkqrvWlvpikRXvCLOCwZfKeMsC;

	private BBctKivJxEjGKRzyEkHSRjJoJqnW hqGXmYxZUrSrCxDobJTGEPCWvHAsA;

	private int CQIAQFpzVmISknegYFOIxRunxPpK;

	private int aLLlpNUjcfjiKpSIORbqbGLhIBol;

	private Action<int, ControllerDataUpdater> xeKrMCpTCMtvKGhOQAkmgoChmmzib;

	private Action<bool, int, int> jTGnCWWqtRRsUxstEqWASkKUAfYd;

	private Action<ControllerStatusChangedEventArgs> kGVOImGbVNwKiMTMLDkVUZdzwWAI;

	private Action<ControllerType, int> lJrmwhYszLAVKekZLdGKVsHAVsdp;

	private bool AeWaeWamxRrERkciQkpFDWbRfZMkA;

	public IList<Joystick> FMULSNPMOjtlSugUpKDpvqOlAjyl => EZkeaGNjmfUUVrykZmLhuXIZgwOf;

	public List<Joystick> TDmdTDkWePpymjgPxyvYlCXtgfEh => YYasSHCtpzGGHqjStqqVkdXZiWHH;

	public int OeeHBecrguVtJOJkdEjZKpQgYwls => YYasSHCtpzGGHqjStqqVkdXZiWHH.Count;

	public Mouse rgqCDZHatmAjYfIausJQUnRSqVpAA => dEcTjrSkAbFekEIeQDAfncJOVAhdA;

	public Keyboard OurrBprwSKVEMTXHNSeMGLuBHall => OlktKbOjbgGrTJEsAkUzsiYeMHYb;

	public IList<CustomController> XoVatgitzcrXSMTZlyGxtxxDoZSr => YhufnUJBrjmuLslaygcAwWOOSGscA;

	public List<CustomController> WeUtdHzFWLInOtLnGANlKjgSOlFtA => vUFoteRIgwMjMHesIRSyNjZtxfgn;

	public int GAMwoDaktPkGWjOteLxMlJnoastv => vUFoteRIgwMjMHesIRSyNjZtxfgn.Count;

	public IList<Controller> FXqycrxiIdSgUWNefithenzRnAiDb => zAapaVyddVmRriQiWinbIMUoxoDW;

	public int XAxgQwEvifPgbHUznCFCtyEbWwOtA => xLmeyCEvHjFDowSsezuNCUAMXWmL.Count;

	private int HMPKfiNcRoeRZGxImsvhaFHEInIC
	{
		get
		{
			int iagzVHFlWCLJUEfGubXiUScVfGmCA = IagzVHFlWCLJUEfGubXiUScVfGmCA;
			IagzVHFlWCLJUEfGubXiUScVfGmCA++;
			if (IagzVHFlWCLJUEfGubXiUScVfGmCA >= int.MaxValue)
			{
				IagzVHFlWCLJUEfGubXiUScVfGmCA = 0;
			}
			return iagzVHFlWCLJUEfGubXiUScVfGmCA;
		}
	}

	public event Action<ControllerStatusChangedEventArgs> iQyOLDUnjPHgzcBvmppBkkalWfrl
	{
		add
		{
			kGVOImGbVNwKiMTMLDkVUZdzwWAI = (Action<ControllerStatusChangedEventArgs>)Delegate.Combine(kGVOImGbVNwKiMTMLDkVUZdzwWAI, b);
		}
		remove
		{
			kGVOImGbVNwKiMTMLDkVUZdzwWAI = (Action<ControllerStatusChangedEventArgs>)Delegate.Remove(kGVOImGbVNwKiMTMLDkVUZdzwWAI, value2);
		}
	}

	public event Action<ControllerType, int> qGgZpjGtlrLnQGOPypQsiBrlekEz
	{
		add
		{
			lJrmwhYszLAVKekZLdGKVsHAVsdp = (Action<ControllerType, int>)Delegate.Combine(lJrmwhYszLAVKekZLdGKVsHAVsdp, b);
		}
		remove
		{
			lJrmwhYszLAVKekZLdGKVsHAVsdp = (Action<ControllerType, int>)Delegate.Remove(lJrmwhYszLAVKekZLdGKVsHAVsdp, value2);
		}
	}

	public pmhdIRhCoLlQZffSYgOmsLkpxsjN(ConfigVars P_0, PlatformInputManager P_1)
	{
		dNgrfdLJdsfeJmlYYJJiwGpIkluS = P_0;
		dVSeywEvtXvpViovnwUfHUKkKgtbb = 0;
		sXLLyWhVtnTsJUdLYDsshzdInxBV = UnityTools.isAndroidPlatform;
		xLmeyCEvHjFDowSsezuNCUAMXWmL = new List<Controller>(10);
		zAapaVyddVmRriQiWinbIMUoxoDW = new ReadOnlyCollection<Controller>(xLmeyCEvHjFDowSsezuNCUAMXWmL);
		IUnifiedKeyboardSource unifiedKeyboardSource = P_1.GetUnifiedKeyboardSource();
		if (unifiedKeyboardSource == null)
		{
			unifiedKeyboardSource = (tSzApJiwheMYhjgvAaTrNbSHdHgP = new UnityUnifiedKeyboardSource());
		}
		OlktKbOjbgGrTJEsAkUzsiYeMHYb = new Keyboard("Keyboard", unifiedKeyboardSource);
		xLmeyCEvHjFDowSsezuNCUAMXWmL.Add(OlktKbOjbgGrTJEsAkUzsiYeMHYb);
		IUnifiedMouseSource unifiedMouseSource = P_1.GetUnifiedMouseSource();
		if (unifiedMouseSource == null)
		{
			unifiedMouseSource = (CDQvdxrBYeaibeuILXSWZTLflVSS = new UnityUnifiedMouseSource());
		}
		dEcTjrSkAbFekEIeQDAfncJOVAhdA = new Mouse("Mouse", unifiedMouseSource);
		xLmeyCEvHjFDowSsezuNCUAMXWmL.Add(dEcTjrSkAbFekEIeQDAfncJOVAhdA);
		tDEfcwuerNXHdNWhqfUXcHaLJUBh = new YsOrrlNIumifyOKxnwVmvgJjFiYZ(P_0.updateLoop, OlktKbOjbgGrTJEsAkUzsiYeMHYb);
		OlktKbOjbgGrTJEsAkUzsiYeMHYb.XzlJaAqgaEXTyCbUKbmUFIQZYLfT += SfHweRCGppaJxMUeKCMqXVNgaQzCA;
		OlktKbOjbgGrTJEsAkUzsiYeMHYb.enabled = !P_0.GetPlatformVar_disableKeyboard();
		dEcTjrSkAbFekEIeQDAfncJOVAhdA.enabled = !P_0.GetPlatformVar_disableMouse();
		gDqyZLpNcQdFqLgOmEFQbYwoWlEy.jpwugzufXqktYbXkMYboQpqCbQgL();
		sOyJxxGPTWlVyzPJIvrZyYyRatmE = new jwwEVOVklpzvRHhBdNqsduEELqpv(UnityTools.externalTools.GetControllerTemplateTypes(), UnityTools.externalTools.GetControllerTemplateInterfaceTypes());
		sOyJxxGPTWlVyzPJIvrZyYyRatmE.ycndZKmKnXmWelVDbcsLODEZWhMV(OlktKbOjbgGrTJEsAkUzsiYeMHYb);
		sOyJxxGPTWlVyzPJIvrZyYyRatmE.ycndZKmKnXmWelVDbcsLODEZWhMV(dEcTjrSkAbFekEIeQDAfncJOVAhdA);
		ReInput.ApplicationFocusChangedEvent += xKXEQmiiNXKorUsTsPlIOtwcwnfr;
	}

	public void zQQfvDZMmpVqPPLYlLuSJXXpwJcI(Action<int, ControllerDataUpdater> P_0, List<InputBehavior> P_1)
	{
		xeKrMCpTCMtvKGhOQAkmgoChmmzib = P_0;
		zQQfvDZMmpVqPPLYlLuSJXXpwJcI(P_1);
	}

	public void jRaYtHNVcykNMAbqOnSGaKIIGSEaA(UpdateLoopType P_0)
	{
		gDqyZLpNcQdFqLgOmEFQbYwoWlEy.zFgskLGCZhfDtwaydFtWOrswGxzB(P_0);
		if (OlktKbOjbgGrTJEsAkUzsiYeMHYb.enabled)
		{
			tDEfcwuerNXHdNWhqfUXcHaLJUBh.jRaYtHNVcykNMAbqOnSGaKIIGSEaA(P_0);
		}
		hBilpTcOigSnyvaPUXGaPrYstwEJ(P_0);
		YkzWSxoRkwVnuhNLyISugAdxcHKJA(P_0);
		gDqyZLpNcQdFqLgOmEFQbYwoWlEy.JFPDzpFPVYxyqXpnZhoBGcTRgDUr(P_0, ReInput.currentFrame);
		if (nzKAgdkNjptUfxogWPtGXJvoIweb)
		{
			EGwFtKcKdrirdnDWIAunJKfRTxueA();
		}
	}

	public ItaEjuKtoATtafYloiocFOanqPQc IULWFKQGEhckhDtQiOajvpmQfmsO(int P_0, string P_1, bool P_2)
	{
		int num = GAwkqrvWlvpikRXvCLOCwZfKeMsC.lsWdiPZgHHfEfIiesCpnLAlcpBgUA(P_1, P_2);
		if (num < 0)
		{
			return null;
		}
		if (P_0 == 9999999)
		{
			return tgNELAiNXZMjZMcUZhoHllhenZUfA[num];
		}
		if (P_0 < 0 || P_0 >= CQIAQFpzVmISknegYFOIxRunxPpK)
		{
			return null;
		}
		return OUADEDsGMLnIILsOyroWSEdclIJD[P_0, num];
	}

	public ItaEjuKtoATtafYloiocFOanqPQc IULWFKQGEhckhDtQiOajvpmQfmsO(int P_0, int P_1, bool P_2)
	{
		int num = GAwkqrvWlvpikRXvCLOCwZfKeMsC.lsWdiPZgHHfEfIiesCpnLAlcpBgUA(P_1, P_2);
		if (num < 0)
		{
			return null;
		}
		if (P_0 == 9999999)
		{
			return tgNELAiNXZMjZMcUZhoHllhenZUfA[num];
		}
		return OUADEDsGMLnIILsOyroWSEdclIJD[P_0, num];
	}

	public void bGjYPyDQFpioBExhyERNSZtsiMQQA(UpdateControllerInfoEventArgs P_0)
	{
		if (P_0 != null && P_0.sourceJoystick != null)
		{
			cRwcHsJJSwdOXPswvlQsCSGPrDri cRwcHsJJSwdOXPswvlQsCSGPrDri2 = cRwcHsJJSwdOXPswvlQsCSGPrDri.Connected;
			int num = bDJkdzIZfPLqSPLEjCUWkRdPnCwx(P_0.sourceJoystick.rewiredId, cRwcHsJJSwdOXPswvlQsCSGPrDri2);
			if (num < 0)
			{
				cRwcHsJJSwdOXPswvlQsCSGPrDri2 = cRwcHsJJSwdOXPswvlQsCSGPrDri.Disconnected;
				num = bDJkdzIZfPLqSPLEjCUWkRdPnCwx(P_0.sourceJoystick.rewiredId, cRwcHsJJSwdOXPswvlQsCSGPrDri2);
			}
			if (num >= 0)
			{
				((cRwcHsJJSwdOXPswvlQsCSGPrDri2 == cRwcHsJJSwdOXPswvlQsCSGPrDri.Connected) ? YYasSHCtpzGGHqjStqqVkdXZiWHH[num] : hXYIgyCpgDGjSSuuYxDApkRKvfGq[num]).UByBUxdeeRfyWdxWnNFetplCMBqd(P_0);
			}
		}
	}

	public bool EpxBFcxyCsxcjqFPquXeXPUJECSb(int P_0, cRwcHsJJSwdOXPswvlQsCSGPrDri P_1)
	{
		if (bDJkdzIZfPLqSPLEjCUWkRdPnCwx(P_0, P_1) < 0)
		{
			return false;
		}
		return true;
	}

	public int bDJkdzIZfPLqSPLEjCUWkRdPnCwx(int P_0, cRwcHsJJSwdOXPswvlQsCSGPrDri P_1)
	{
		switch (P_1)
		{
		case cRwcHsJJSwdOXPswvlQsCSGPrDri.Connected:
		{
			int count2 = YYasSHCtpzGGHqjStqqVkdXZiWHH.Count;
			for (int j = 0; j < count2; j++)
			{
				if (YYasSHCtpzGGHqjStqqVkdXZiWHH[j].id == P_0)
				{
					return j;
				}
			}
			break;
		}
		case cRwcHsJJSwdOXPswvlQsCSGPrDri.Disconnected:
		{
			int count = hXYIgyCpgDGjSSuuYxDApkRKvfGq.Count;
			for (int i = 0; i < count; i++)
			{
				if (hXYIgyCpgDGjSSuuYxDApkRKvfGq[i].id == P_0)
				{
					return i;
				}
			}
			break;
		}
		}
		return -1;
	}

	public int bDJkdzIZfPLqSPLEjCUWkRdPnCwx(Guid P_0, cRwcHsJJSwdOXPswvlQsCSGPrDri P_1)
	{
		switch (P_1)
		{
		case cRwcHsJJSwdOXPswvlQsCSGPrDri.Connected:
		{
			int count2 = YYasSHCtpzGGHqjStqqVkdXZiWHH.Count;
			for (int j = 0; j < count2; j++)
			{
				if (YYasSHCtpzGGHqjStqqVkdXZiWHH[j].deviceInstanceGuid == P_0)
				{
					return j;
				}
			}
			break;
		}
		case cRwcHsJJSwdOXPswvlQsCSGPrDri.Disconnected:
		{
			int count = hXYIgyCpgDGjSSuuYxDApkRKvfGq.Count;
			for (int i = 0; i < count; i++)
			{
				if (hXYIgyCpgDGjSSuuYxDApkRKvfGq[i].deviceInstanceGuid == P_0)
				{
					return i;
				}
			}
			break;
		}
		}
		return -1;
	}

	public bool EWNlxMCwoFnTFbzpLKyTDpcdvMwn(int P_0)
	{
		if (XhuoaRUWIZSJnbYqbPGkxpakeBGCA(P_0) < 0)
		{
			return false;
		}
		return true;
	}

	public int XhuoaRUWIZSJnbYqbPGkxpakeBGCA(int P_0)
	{
		int count = vUFoteRIgwMjMHesIRSyNjZtxfgn.Count;
		for (int i = 0; i < count; i++)
		{
			if (vUFoteRIgwMjMHesIRSyNjZtxfgn[i].id == P_0)
			{
				return i;
			}
		}
		return -1;
	}

	public int XhuoaRUWIZSJnbYqbPGkxpakeBGCA(Guid P_0)
	{
		int count = vUFoteRIgwMjMHesIRSyNjZtxfgn.Count;
		for (int i = 0; i < count; i++)
		{
			if (vUFoteRIgwMjMHesIRSyNjZtxfgn[i].deviceInstanceGuid == P_0)
			{
				return i;
			}
		}
		return -1;
	}

	public void bHsYHPUicNEFXDZZhoWGRxKnRVDu(BridgedController P_0)
	{
		ZbcDSPekYnlStRgRugzyetSbWkxBc(P_0);
	}

	public void PqJEIUwQDbRjQsunXAtcvOSSVLxX(int P_0)
	{
		int num = bDJkdzIZfPLqSPLEjCUWkRdPnCwx(P_0, cRwcHsJJSwdOXPswvlQsCSGPrDri.Connected);
		KdhLmBFFAJHZLrHKkMIuhTdMdfAv(num);
	}

	public int vLiLmJTosNNvyFgxkOlaPhnFocg()
	{
		return dVSeywEvtXvpViovnwUfHUKkKgtbb++;
	}

	public IList<InputBehavior> hrFktKFZmNkMtXheIgPWJTjSvNng(int P_0)
	{
		if (!hmyGBQfyaWazZLqcjhHmFczCcGjfB.ContainsKey(P_0))
		{
			return new List<InputBehavior>();
		}
		return hmyGBQfyaWazZLqcjhHmFczCcGjfB[P_0].REVMeXqqKzfxWRkzieBTIdVcitWF;
	}

	public InputBehavior uwwouvQdjxpfTIRrJNyUxsHLWDAC(int P_0, string P_1)
	{
		if (P_1 == null || P_1 == string.Empty)
		{
			return null;
		}
		int inputBehaviorId = ReInput.mapping.GetInputBehaviorId(P_1);
		return uwwouvQdjxpfTIRrJNyUxsHLWDAC(P_0, inputBehaviorId);
	}

	public InputBehavior uwwouvQdjxpfTIRrJNyUxsHLWDAC(int P_0, int P_1)
	{
		if (!hmyGBQfyaWazZLqcjhHmFczCcGjfB.ContainsKey(P_0))
		{
			return null;
		}
		IList<InputBehavior> rEVMeXqqKzfxWRkzieBTIdVcitWF = hmyGBQfyaWazZLqcjhHmFczCcGjfB[P_0].REVMeXqqKzfxWRkzieBTIdVcitWF;
		for (int i = 0; i < rEVMeXqqKzfxWRkzieBTIdVcitWF.Count; i++)
		{
			if (rEVMeXqqKzfxWRkzieBTIdVcitWF[i].id == P_1)
			{
				return rEVMeXqqKzfxWRkzieBTIdVcitWF[i];
			}
		}
		return null;
	}

	public Joystick ojzheXTjdCUaEzozjVQUFinqCht(int P_0, bool P_1 = false)
	{
		int num = bDJkdzIZfPLqSPLEjCUWkRdPnCwx(P_0, cRwcHsJJSwdOXPswvlQsCSGPrDri.Connected);
		if (num >= 0)
		{
			return YYasSHCtpzGGHqjStqqVkdXZiWHH[num];
		}
		if (P_1)
		{
			num = bDJkdzIZfPLqSPLEjCUWkRdPnCwx(P_0, cRwcHsJJSwdOXPswvlQsCSGPrDri.Disconnected);
			if (num >= 0)
			{
				return hXYIgyCpgDGjSSuuYxDApkRKvfGq[num];
			}
		}
		return null;
	}

	public Joystick ojzheXTjdCUaEzozjVQUFinqCht(Guid P_0, bool P_1 = false)
	{
		int num = bDJkdzIZfPLqSPLEjCUWkRdPnCwx(P_0, cRwcHsJJSwdOXPswvlQsCSGPrDri.Connected);
		if (num >= 0)
		{
			return YYasSHCtpzGGHqjStqqVkdXZiWHH[num];
		}
		if (P_1)
		{
			num = bDJkdzIZfPLqSPLEjCUWkRdPnCwx(P_0, cRwcHsJJSwdOXPswvlQsCSGPrDri.Disconnected);
			if (num >= 0)
			{
				return hXYIgyCpgDGjSSuuYxDApkRKvfGq[num];
			}
		}
		return null;
	}

	public Joystick[] FlyseoUFWWdQIBiWngLFaaOSOfwHb()
	{
		int count = YYasSHCtpzGGHqjStqqVkdXZiWHH.Count;
		if (count == 0)
		{
			return EmptyObjects<Joystick>.array;
		}
		Joystick[] array = new Joystick[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = YYasSHCtpzGGHqjStqqVkdXZiWHH[i];
		}
		return array;
	}

	public string[] gtXizTltiHBZLUKHFdFDkemIjcZY()
	{
		int count = YYasSHCtpzGGHqjStqqVkdXZiWHH.Count;
		if (count == 0)
		{
			return EmptyObjects<string>.array;
		}
		string[] array = new string[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = YYasSHCtpzGGHqjStqqVkdXZiWHH[i].name;
		}
		return array;
	}

	public CustomController UhFBEoKYbICWKGrfPbNBAMYDdcqzA(int P_0)
	{
		int num = XhuoaRUWIZSJnbYqbPGkxpakeBGCA(P_0);
		if (num < 0)
		{
			return null;
		}
		return vUFoteRIgwMjMHesIRSyNjZtxfgn[num];
	}

	public CustomController UhFBEoKYbICWKGrfPbNBAMYDdcqzA(Guid P_0)
	{
		int num = XhuoaRUWIZSJnbYqbPGkxpakeBGCA(P_0);
		if (num < 0)
		{
			return null;
		}
		return vUFoteRIgwMjMHesIRSyNjZtxfgn[num];
	}

	public CustomController[] xRsdIcKLBoMJoRCkGuUDaFVkcLvs()
	{
		int count = vUFoteRIgwMjMHesIRSyNjZtxfgn.Count;
		if (count == 0)
		{
			return EmptyObjects<CustomController>.array;
		}
		CustomController[] array = new CustomController[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = vUFoteRIgwMjMHesIRSyNjZtxfgn[i];
		}
		return array;
	}

	public string[] zDkghtyeRYxdcAfWYOgOCoOMPalb()
	{
		int count = vUFoteRIgwMjMHesIRSyNjZtxfgn.Count;
		if (count == 0)
		{
			return EmptyObjects<string>.array;
		}
		string[] array = new string[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = vUFoteRIgwMjMHesIRSyNjZtxfgn[i].name;
		}
		return array;
	}

	public CustomController labQcDriOISpTvNogGqpTBqIeJDA(int P_0)
	{
		CustomController_Editor customControllerById = ReInput.UserData.GetCustomControllerById(P_0);
		if (customControllerById == null)
		{
			return null;
		}
		int vLaDHvfmijzLwCtDfpDRfeHfsWbqc = HMPKfiNcRoeRZGxImsvhaFHEInIC;
		CustomController customController = new CustomController(new LDGblTyVuNxcqdmvXxXJqAlXAiYR
		{
			rmMLNGpqXVBSkkjZadentyrvdrgtA = InputSource.Custom,
			rCJyseVRiKOjaaVRmjlxeXUUOXNJ = customControllerById.descriptiveName,
			LIPsbVuyBvODEWtXBDzVIGmLLUAS = customControllerById.name,
			yLLlShtixVkbYMaIfuUMijmLCboKA = customControllerById.axisCount,
			tpoonPbJmajQyAErVLnAOoXhqZqEb = customControllerById.buttonCount,
			vLaDHvfmijzLwCtDfpDRfeHfsWbqc = vLaDHvfmijzLwCtDfpDRfeHfsWbqc,
			IkfUAiUlOToIXPDSQCjvytoLhPaV = customControllerById.id,
			AdvgSiYWsKyfjHPRtouNwdStlsJq = customControllerById.typeGuid,
			WncgGowlsDGmmJANqyvwLIbwHAtqA = customControllerById.id.ToString(),
			uAnjbTFgypgjVDFVAgoVQDncZkGd = customControllerById.CreateGameHardwareMap()
		});
		mzykFmvMViYixIPuHDidbTcTViYtA(customController);
		return customController;
	}

	public bool hgJXJmdxNMqcjJmsiLmfzAnuBfpbA(CustomController P_0)
	{
		if (P_0 == null)
		{
			return false;
		}
		return iuacuYVnEcyuHqNzXCqSIHTRrksK(P_0);
	}

	public CustomController weNaberKoEhMzGELeiozzgiHRPtHA(int P_0)
	{
		int count = vUFoteRIgwMjMHesIRSyNjZtxfgn.Count;
		for (int i = 0; i < count; i++)
		{
			if (vUFoteRIgwMjMHesIRSyNjZtxfgn[i].sourceControllerId == P_0)
			{
				return vUFoteRIgwMjMHesIRSyNjZtxfgn[i];
			}
		}
		return null;
	}

	public CustomController pCJeiYSRNApAXrfmZBHqQVEAIQzg(string P_0)
	{
		int count = vUFoteRIgwMjMHesIRSyNjZtxfgn.Count;
		for (int i = 0; i < count; i++)
		{
			if (vUFoteRIgwMjMHesIRSyNjZtxfgn[i].tag.Equals(P_0, StringComparison.OrdinalIgnoreCase))
			{
				return vUFoteRIgwMjMHesIRSyNjZtxfgn[i];
			}
		}
		return null;
	}

	public IEnumerable<CustomController> whXOtSlXGtPSNIikcNUrRlaeaFKIA(int P_0)
	{
		return new OVlTUCSeRUMfdObDTYgcgZLKedyv(-2)
		{
			TtytLoUfsgUyhsklaKccrnoMiiek = this,
			xqTcIBbgIpiyNCXcOBWDlWIEYFYDA = P_0
		};
	}

	public IEnumerable<CustomController> UZaCrMKFZHYrqgcALBkFZJwZoOsi(string P_0)
	{
		return new KcXqtzOffZGfigpnGLYcUqHCtZINA(-2)
		{
			TtytLoUfsgUyhsklaKccrnoMiiek = this,
			NgUGOCgMAPNuMsxXPWsrQJXFnwujA = P_0
		};
	}

	public Controller IzoAZhLSJPdQcJxvWLgBrvgaJNMKA(ControllerType P_0, int P_1, bool P_2 = false)
	{
		return P_0 switch
		{
			ControllerType.Joystick => ojzheXTjdCUaEzozjVQUFinqCht(P_1, P_2), 
			ControllerType.Keyboard => OlktKbOjbgGrTJEsAkUzsiYeMHYb, 
			ControllerType.Mouse => dEcTjrSkAbFekEIeQDAfncJOVAhdA, 
			ControllerType.Custom => UhFBEoKYbICWKGrfPbNBAMYDdcqzA(P_1), 
			_ => throw new NotImplementedException(), 
		};
	}

	public Controller IzoAZhLSJPdQcJxvWLgBrvgaJNMKA(ControllerIdentifier P_0, bool P_1 = false)
	{
		if (P_0.deviceInstanceGuid != Guid.Empty)
		{
			return IzoAZhLSJPdQcJxvWLgBrvgaJNMKA(P_0.deviceInstanceGuid);
		}
		if (P_0.controllerId >= 0)
		{
			return IzoAZhLSJPdQcJxvWLgBrvgaJNMKA(P_0.controllerType, P_0.controllerId, P_1);
		}
		return null;
	}

	public Controller IzoAZhLSJPdQcJxvWLgBrvgaJNMKA(Guid P_0, bool P_1 = false)
	{
		if (P_0 == Guid.Empty)
		{
			return null;
		}
		if (OlktKbOjbgGrTJEsAkUzsiYeMHYb.deviceInstanceGuid == P_0)
		{
			return OlktKbOjbgGrTJEsAkUzsiYeMHYb;
		}
		if (dEcTjrSkAbFekEIeQDAfncJOVAhdA.deviceInstanceGuid == P_0)
		{
			return dEcTjrSkAbFekEIeQDAfncJOVAhdA;
		}
		Controller result;
		if ((result = ojzheXTjdCUaEzozjVQUFinqCht(P_0, P_1)) != null)
		{
			return result;
		}
		if ((result = UhFBEoKYbICWKGrfPbNBAMYDdcqzA(P_0)) != null)
		{
			return result;
		}
		return null;
	}

	public Controller[] nzQQovtPuMkcgOGwQubivSJhAFlx(ControllerType P_0)
	{
		return P_0 switch
		{
			ControllerType.Joystick => FlyseoUFWWdQIBiWngLFaaOSOfwHb(), 
			ControllerType.Keyboard => new Controller[1] { OlktKbOjbgGrTJEsAkUzsiYeMHYb }, 
			ControllerType.Mouse => new Controller[1] { dEcTjrSkAbFekEIeQDAfncJOVAhdA }, 
			ControllerType.Custom => xRsdIcKLBoMJoRCkGuUDaFVkcLvs(), 
			_ => throw new NotImplementedException(), 
		};
	}

	public string[] IyVdxTVDGtYxFqHRAhFWyOfLJNWm(ControllerType P_0)
	{
		return P_0 switch
		{
			ControllerType.Joystick => gtXizTltiHBZLUKHFdFDkemIjcZY(), 
			ControllerType.Keyboard => new string[1] { OlktKbOjbgGrTJEsAkUzsiYeMHYb.name }, 
			ControllerType.Mouse => new string[1] { dEcTjrSkAbFekEIeQDAfncJOVAhdA.name }, 
			ControllerType.Custom => zDkghtyeRYxdcAfWYOgOCoOMPalb(), 
			_ => throw new NotImplementedException(), 
		};
	}

	public void vUrsReZzXqfhXvHmNIAleprzJmKO(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2)
	{
		if (!hnvMKVcKgNVmSXgWGXLrQdCHStBg)
		{
			hnvMKVcKgNVmSXgWGXLrQdCHStBg = true;
		}
		LwOPFbIjZVapvmlQStJRsGcCHOhV(P_0)?.BcNRXlUcIeCeQoTBnFpegDhuaAap(P_1, P_2, InputActionEventType.Update, null);
	}

	public void vUrsReZzXqfhXvHmNIAleprzJmKO(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2, int P_3)
	{
		if (!hnvMKVcKgNVmSXgWGXLrQdCHStBg)
		{
			hnvMKVcKgNVmSXgWGXLrQdCHStBg = true;
		}
		LwOPFbIjZVapvmlQStJRsGcCHOhV(P_0)?.BcNRXlUcIeCeQoTBnFpegDhuaAap(P_1, P_2, InputActionEventType.Update, P_3, null);
	}

	public void vUrsReZzXqfhXvHmNIAleprzJmKO(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2, string P_3)
	{
		if (!hnvMKVcKgNVmSXgWGXLrQdCHStBg)
		{
			hnvMKVcKgNVmSXgWGXLrQdCHStBg = true;
		}
		int num = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(P_3);
		if (num >= 0)
		{
			vUrsReZzXqfhXvHmNIAleprzJmKO(P_0, P_1, P_2, num);
		}
	}

	public void vUrsReZzXqfhXvHmNIAleprzJmKO(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2, InputActionEventType P_3, object[] P_4)
	{
		if (!hnvMKVcKgNVmSXgWGXLrQdCHStBg)
		{
			hnvMKVcKgNVmSXgWGXLrQdCHStBg = true;
		}
		LwOPFbIjZVapvmlQStJRsGcCHOhV(P_0)?.BcNRXlUcIeCeQoTBnFpegDhuaAap(P_1, P_2, P_3, P_4);
	}

	public void vUrsReZzXqfhXvHmNIAleprzJmKO(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2, InputActionEventType P_3, int P_4, object[] P_5)
	{
		if (!hnvMKVcKgNVmSXgWGXLrQdCHStBg)
		{
			hnvMKVcKgNVmSXgWGXLrQdCHStBg = true;
		}
		LwOPFbIjZVapvmlQStJRsGcCHOhV(P_0)?.BcNRXlUcIeCeQoTBnFpegDhuaAap(P_1, P_2, P_3, P_4, P_5);
	}

	public void vUrsReZzXqfhXvHmNIAleprzJmKO(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2, InputActionEventType P_3, string P_4, object[] P_5)
	{
		if (!hnvMKVcKgNVmSXgWGXLrQdCHStBg)
		{
			hnvMKVcKgNVmSXgWGXLrQdCHStBg = true;
		}
		int num = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(P_4);
		if (num >= 0)
		{
			vUrsReZzXqfhXvHmNIAleprzJmKO(P_0, P_1, P_2, P_3, num, P_5);
		}
	}

	public void stOESscrTenbysqLWMUiwOZCcVyh(int P_0, Action<InputActionEventData> P_1)
	{
		LwOPFbIjZVapvmlQStJRsGcCHOhV(P_0)?.qZjUkljUkFefeYpopqopvDZOAIkDA(P_1);
	}

	public void stOESscrTenbysqLWMUiwOZCcVyh(int P_0, Action<InputActionEventData> P_1, int P_2)
	{
		LwOPFbIjZVapvmlQStJRsGcCHOhV(P_0)?.qZjUkljUkFefeYpopqopvDZOAIkDA(P_1, P_2);
	}

	public void stOESscrTenbysqLWMUiwOZCcVyh(int P_0, Action<InputActionEventData> P_1, string P_2)
	{
		int num = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(P_2);
		if (num >= 0)
		{
			stOESscrTenbysqLWMUiwOZCcVyh(P_0, P_1, num);
		}
	}

	public void stOESscrTenbysqLWMUiwOZCcVyh(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2)
	{
		LwOPFbIjZVapvmlQStJRsGcCHOhV(P_0)?.qZjUkljUkFefeYpopqopvDZOAIkDA(P_1, P_2);
	}

	public void stOESscrTenbysqLWMUiwOZCcVyh(int P_0, Action<InputActionEventData> P_1, InputActionEventType P_2)
	{
		LwOPFbIjZVapvmlQStJRsGcCHOhV(P_0)?.qZjUkljUkFefeYpopqopvDZOAIkDA(P_1, P_2);
	}

	public void stOESscrTenbysqLWMUiwOZCcVyh(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2, int P_3)
	{
		LwOPFbIjZVapvmlQStJRsGcCHOhV(P_0)?.qZjUkljUkFefeYpopqopvDZOAIkDA(P_1, P_2, P_3);
	}

	public void stOESscrTenbysqLWMUiwOZCcVyh(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2, string P_3)
	{
		int num = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(P_3);
		if (num >= 0)
		{
			stOESscrTenbysqLWMUiwOZCcVyh(P_0, P_1, P_2, num);
		}
	}

	public void stOESscrTenbysqLWMUiwOZCcVyh(int P_0, Action<InputActionEventData> P_1, InputActionEventType P_2, int P_3)
	{
		LwOPFbIjZVapvmlQStJRsGcCHOhV(P_0)?.qZjUkljUkFefeYpopqopvDZOAIkDA(P_1, P_2, P_3);
	}

	public void stOESscrTenbysqLWMUiwOZCcVyh(int P_0, Action<InputActionEventData> P_1, InputActionEventType P_2, string P_3)
	{
		int num = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(P_3);
		if (num >= 0)
		{
			stOESscrTenbysqLWMUiwOZCcVyh(P_0, P_1, P_2, num);
		}
	}

	public void stOESscrTenbysqLWMUiwOZCcVyh(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2, InputActionEventType P_3)
	{
		LwOPFbIjZVapvmlQStJRsGcCHOhV(P_0)?.qZjUkljUkFefeYpopqopvDZOAIkDA(P_1, P_2, P_3);
	}

	public void stOESscrTenbysqLWMUiwOZCcVyh(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2, InputActionEventType P_3, int P_4)
	{
		LwOPFbIjZVapvmlQStJRsGcCHOhV(P_0)?.qZjUkljUkFefeYpopqopvDZOAIkDA(P_1, P_2, P_3, P_4);
	}

	public void stOESscrTenbysqLWMUiwOZCcVyh(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2, InputActionEventType P_3, string P_4)
	{
		int num = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(P_4);
		if (num >= 0)
		{
			stOESscrTenbysqLWMUiwOZCcVyh(P_0, P_1, P_2, P_3, num);
		}
	}

	public void rIXpseirsOAvJaoyyaejvCyGhFgZA(int P_0)
	{
		LwOPFbIjZVapvmlQStJRsGcCHOhV(P_0)?.SPGTRPyvIslcMdbPTItsewSLRPxx();
	}

	public bool IRCnsSkdfDsQRseHgJojFdXPOaqp(int P_0)
	{
		if (P_0 == 9999999)
		{
			for (int i = 0; i < tgNELAiNXZMjZMcUZhoHllhenZUfA.Length; i++)
			{
				if (tgNELAiNXZMjZMcUZhoHllhenZUfA[i].IKCLDPTiIKTdmSlZPgUWaOkbUbhVA())
				{
					return true;
				}
			}
			return false;
		}
		if (P_0 < 0 || P_0 >= CQIAQFpzVmISknegYFOIxRunxPpK)
		{
			return false;
		}
		int num = GAwkqrvWlvpikRXvCLOCwZfKeMsC.aLLlpNUjcfjiKpSIORbqbGLhIBol;
		for (int j = 0; j < num; j++)
		{
			if (OUADEDsGMLnIILsOyroWSEdclIJD[P_0, j].IKCLDPTiIKTdmSlZPgUWaOkbUbhVA())
			{
				return true;
			}
		}
		return false;
	}

	public bool IVjAOTmckhslFoPOpudSfEsYJoJd(int P_0)
	{
		if (P_0 == 9999999)
		{
			for (int i = 0; i < tgNELAiNXZMjZMcUZhoHllhenZUfA.Length; i++)
			{
				if (tgNELAiNXZMjZMcUZhoHllhenZUfA[i].LbzwmtxTiWvoFmsIMWGeWeOGjmIg())
				{
					return true;
				}
			}
			return false;
		}
		if (P_0 < 0 || P_0 >= CQIAQFpzVmISknegYFOIxRunxPpK)
		{
			return false;
		}
		int num = GAwkqrvWlvpikRXvCLOCwZfKeMsC.aLLlpNUjcfjiKpSIORbqbGLhIBol;
		for (int j = 0; j < num; j++)
		{
			if (OUADEDsGMLnIILsOyroWSEdclIJD[P_0, j].LbzwmtxTiWvoFmsIMWGeWeOGjmIg())
			{
				return true;
			}
		}
		return false;
	}

	public bool VjopfXmpXZLVGJQICuEfyNhOalVX(int P_0)
	{
		if (P_0 == 9999999)
		{
			for (int i = 0; i < tgNELAiNXZMjZMcUZhoHllhenZUfA.Length; i++)
			{
				if (tgNELAiNXZMjZMcUZhoHllhenZUfA[i].vJpqJdBUhGAMATiWyyWbrqfEhKUg())
				{
					return true;
				}
			}
			return false;
		}
		if (P_0 < 0 || P_0 >= CQIAQFpzVmISknegYFOIxRunxPpK)
		{
			return false;
		}
		int num = GAwkqrvWlvpikRXvCLOCwZfKeMsC.aLLlpNUjcfjiKpSIORbqbGLhIBol;
		for (int j = 0; j < num; j++)
		{
			if (OUADEDsGMLnIILsOyroWSEdclIJD[P_0, j].vJpqJdBUhGAMATiWyyWbrqfEhKUg())
			{
				return true;
			}
		}
		return false;
	}

	public bool OrVbdoGcXTDnFDLCtyrVPufMrBTc(int P_0)
	{
		if (P_0 == 9999999)
		{
			for (int i = 0; i < tgNELAiNXZMjZMcUZhoHllhenZUfA.Length; i++)
			{
				if (tgNELAiNXZMjZMcUZhoHllhenZUfA[i].OQBBxsOrfqCVjzijqOkpltRYRQId())
				{
					return true;
				}
			}
			return false;
		}
		if (P_0 < 0 || P_0 >= CQIAQFpzVmISknegYFOIxRunxPpK)
		{
			return false;
		}
		int num = GAwkqrvWlvpikRXvCLOCwZfKeMsC.aLLlpNUjcfjiKpSIORbqbGLhIBol;
		for (int j = 0; j < num; j++)
		{
			if (OUADEDsGMLnIILsOyroWSEdclIJD[P_0, j].OQBBxsOrfqCVjzijqOkpltRYRQId())
			{
				return true;
			}
		}
		return false;
	}

	public bool ycLpDiqeaYQSXPETCyDMUtFZLmcJ(int P_0)
	{
		if (P_0 == 9999999)
		{
			for (int i = 0; i < tgNELAiNXZMjZMcUZhoHllhenZUfA.Length; i++)
			{
				if (tgNELAiNXZMjZMcUZhoHllhenZUfA[i].tfsYXedjKMNWjMtDSqWFOsogLsAD())
				{
					return true;
				}
			}
			return false;
		}
		if (P_0 < 0 || P_0 >= CQIAQFpzVmISknegYFOIxRunxPpK)
		{
			return false;
		}
		int num = GAwkqrvWlvpikRXvCLOCwZfKeMsC.aLLlpNUjcfjiKpSIORbqbGLhIBol;
		for (int j = 0; j < num; j++)
		{
			if (OUADEDsGMLnIILsOyroWSEdclIJD[P_0, j].tfsYXedjKMNWjMtDSqWFOsogLsAD())
			{
				return true;
			}
		}
		return false;
	}

	public bool sJQjFIqOpNYyVTuSfzUIBMplXbCJ(int P_0)
	{
		if (P_0 == 9999999)
		{
			for (int i = 0; i < tgNELAiNXZMjZMcUZhoHllhenZUfA.Length; i++)
			{
				if (tgNELAiNXZMjZMcUZhoHllhenZUfA[i].fFQjdmlZoEThlEPanCBBkaXvvEJo())
				{
					return true;
				}
			}
			return false;
		}
		if (P_0 < 0 || P_0 >= CQIAQFpzVmISknegYFOIxRunxPpK)
		{
			return false;
		}
		int num = GAwkqrvWlvpikRXvCLOCwZfKeMsC.aLLlpNUjcfjiKpSIORbqbGLhIBol;
		for (int j = 0; j < num; j++)
		{
			if (OUADEDsGMLnIILsOyroWSEdclIJD[P_0, j].fFQjdmlZoEThlEPanCBBkaXvvEJo())
			{
				return true;
			}
		}
		return false;
	}

	public bool TwJpIdDDJCYpPXAjPGWYoEzFhWsc(int P_0)
	{
		if (P_0 == 9999999)
		{
			for (int i = 0; i < tgNELAiNXZMjZMcUZhoHllhenZUfA.Length; i++)
			{
				if (tgNELAiNXZMjZMcUZhoHllhenZUfA[i].qgryZPkzxqeyRJeMpPEmurhtTyMm())
				{
					return true;
				}
			}
			return false;
		}
		if (P_0 < 0 || P_0 >= CQIAQFpzVmISknegYFOIxRunxPpK)
		{
			return false;
		}
		int num = GAwkqrvWlvpikRXvCLOCwZfKeMsC.aLLlpNUjcfjiKpSIORbqbGLhIBol;
		for (int j = 0; j < num; j++)
		{
			if (OUADEDsGMLnIILsOyroWSEdclIJD[P_0, j].qgryZPkzxqeyRJeMpPEmurhtTyMm())
			{
				return true;
			}
		}
		return false;
	}

	public bool HFVhQshmXoeRkKBunSvVvgNFEvYz(int P_0)
	{
		if (P_0 == 9999999)
		{
			for (int i = 0; i < tgNELAiNXZMjZMcUZhoHllhenZUfA.Length; i++)
			{
				if (tgNELAiNXZMjZMcUZhoHllhenZUfA[i].wSIRQBVAPRDurkpeGuQPDPRCpMgm())
				{
					return true;
				}
			}
			return false;
		}
		if (P_0 < 0 || P_0 >= CQIAQFpzVmISknegYFOIxRunxPpK)
		{
			return false;
		}
		int num = GAwkqrvWlvpikRXvCLOCwZfKeMsC.aLLlpNUjcfjiKpSIORbqbGLhIBol;
		for (int j = 0; j < num; j++)
		{
			if (OUADEDsGMLnIILsOyroWSEdclIJD[P_0, j].wSIRQBVAPRDurkpeGuQPDPRCpMgm())
			{
				return true;
			}
		}
		return false;
	}

	public bool BFKZawnovHBiifPPnuuSRDawTwze()
	{
		if (!BFKZawnovHBiifPPnuuSRDawTwze(dEcTjrSkAbFekEIeQDAfncJOVAhdA) && !BFKZawnovHBiifPPnuuSRDawTwze(YYasSHCtpzGGHqjStqqVkdXZiWHH) && !BFKZawnovHBiifPPnuuSRDawTwze(OlktKbOjbgGrTJEsAkUzsiYeMHYb))
		{
			return BFKZawnovHBiifPPnuuSRDawTwze(vUFoteRIgwMjMHesIRSyNjZtxfgn);
		}
		return true;
	}

	public bool BFKZawnovHBiifPPnuuSRDawTwze(ControllerType P_0)
	{
		return P_0 switch
		{
			ControllerType.Joystick => BFKZawnovHBiifPPnuuSRDawTwze(YYasSHCtpzGGHqjStqqVkdXZiWHH), 
			ControllerType.Keyboard => BFKZawnovHBiifPPnuuSRDawTwze(OlktKbOjbgGrTJEsAkUzsiYeMHYb), 
			ControllerType.Mouse => BFKZawnovHBiifPPnuuSRDawTwze(dEcTjrSkAbFekEIeQDAfncJOVAhdA), 
			ControllerType.Custom => BFKZawnovHBiifPPnuuSRDawTwze(vUFoteRIgwMjMHesIRSyNjZtxfgn), 
			_ => throw new NotImplementedException(), 
		};
	}

	public bool VSTjcPRPJuIMVohKVeAMxLRVDISe()
	{
		if (!VSTjcPRPJuIMVohKVeAMxLRVDISe(dEcTjrSkAbFekEIeQDAfncJOVAhdA) && !VSTjcPRPJuIMVohKVeAMxLRVDISe(YYasSHCtpzGGHqjStqqVkdXZiWHH) && !VSTjcPRPJuIMVohKVeAMxLRVDISe(OlktKbOjbgGrTJEsAkUzsiYeMHYb))
		{
			return VSTjcPRPJuIMVohKVeAMxLRVDISe(vUFoteRIgwMjMHesIRSyNjZtxfgn);
		}
		return true;
	}

	public bool VSTjcPRPJuIMVohKVeAMxLRVDISe(ControllerType P_0)
	{
		return P_0 switch
		{
			ControllerType.Joystick => VSTjcPRPJuIMVohKVeAMxLRVDISe(YYasSHCtpzGGHqjStqqVkdXZiWHH), 
			ControllerType.Keyboard => VSTjcPRPJuIMVohKVeAMxLRVDISe(OlktKbOjbgGrTJEsAkUzsiYeMHYb), 
			ControllerType.Mouse => VSTjcPRPJuIMVohKVeAMxLRVDISe(dEcTjrSkAbFekEIeQDAfncJOVAhdA), 
			ControllerType.Custom => VSTjcPRPJuIMVohKVeAMxLRVDISe(vUFoteRIgwMjMHesIRSyNjZtxfgn), 
			_ => throw new NotImplementedException(), 
		};
	}

	public bool huUGSZvDeDGtAwKwPyflEhkiDTSU()
	{
		if (!huUGSZvDeDGtAwKwPyflEhkiDTSU(dEcTjrSkAbFekEIeQDAfncJOVAhdA) && !huUGSZvDeDGtAwKwPyflEhkiDTSU(YYasSHCtpzGGHqjStqqVkdXZiWHH) && !huUGSZvDeDGtAwKwPyflEhkiDTSU(OlktKbOjbgGrTJEsAkUzsiYeMHYb))
		{
			return huUGSZvDeDGtAwKwPyflEhkiDTSU(vUFoteRIgwMjMHesIRSyNjZtxfgn);
		}
		return true;
	}

	public bool huUGSZvDeDGtAwKwPyflEhkiDTSU(ControllerType P_0)
	{
		return P_0 switch
		{
			ControllerType.Joystick => huUGSZvDeDGtAwKwPyflEhkiDTSU(YYasSHCtpzGGHqjStqqVkdXZiWHH), 
			ControllerType.Keyboard => huUGSZvDeDGtAwKwPyflEhkiDTSU(OlktKbOjbgGrTJEsAkUzsiYeMHYb), 
			ControllerType.Mouse => huUGSZvDeDGtAwKwPyflEhkiDTSU(dEcTjrSkAbFekEIeQDAfncJOVAhdA), 
			ControllerType.Custom => huUGSZvDeDGtAwKwPyflEhkiDTSU(vUFoteRIgwMjMHesIRSyNjZtxfgn), 
			_ => throw new NotImplementedException(), 
		};
	}

	public bool cwGisSwHogQrGVvEJHgYIEyuPaOv()
	{
		if (!cwGisSwHogQrGVvEJHgYIEyuPaOv(dEcTjrSkAbFekEIeQDAfncJOVAhdA) && !cwGisSwHogQrGVvEJHgYIEyuPaOv(YYasSHCtpzGGHqjStqqVkdXZiWHH) && !cwGisSwHogQrGVvEJHgYIEyuPaOv(OlktKbOjbgGrTJEsAkUzsiYeMHYb))
		{
			return cwGisSwHogQrGVvEJHgYIEyuPaOv(vUFoteRIgwMjMHesIRSyNjZtxfgn);
		}
		return true;
	}

	public bool cwGisSwHogQrGVvEJHgYIEyuPaOv(ControllerType P_0)
	{
		return P_0 switch
		{
			ControllerType.Joystick => cwGisSwHogQrGVvEJHgYIEyuPaOv(YYasSHCtpzGGHqjStqqVkdXZiWHH), 
			ControllerType.Keyboard => cwGisSwHogQrGVvEJHgYIEyuPaOv(OlktKbOjbgGrTJEsAkUzsiYeMHYb), 
			ControllerType.Mouse => cwGisSwHogQrGVvEJHgYIEyuPaOv(dEcTjrSkAbFekEIeQDAfncJOVAhdA), 
			ControllerType.Custom => cwGisSwHogQrGVvEJHgYIEyuPaOv(vUFoteRIgwMjMHesIRSyNjZtxfgn), 
			_ => throw new NotImplementedException(), 
		};
	}

	public bool tCLNIawgRirkkeTAwgbgLCflRZH()
	{
		if (!tCLNIawgRirkkeTAwgbgLCflRZH(dEcTjrSkAbFekEIeQDAfncJOVAhdA) && !tCLNIawgRirkkeTAwgbgLCflRZH(YYasSHCtpzGGHqjStqqVkdXZiWHH) && !tCLNIawgRirkkeTAwgbgLCflRZH(OlktKbOjbgGrTJEsAkUzsiYeMHYb))
		{
			return tCLNIawgRirkkeTAwgbgLCflRZH(vUFoteRIgwMjMHesIRSyNjZtxfgn);
		}
		return true;
	}

	public bool tCLNIawgRirkkeTAwgbgLCflRZH(ControllerType P_0)
	{
		return P_0 switch
		{
			ControllerType.Joystick => tCLNIawgRirkkeTAwgbgLCflRZH(YYasSHCtpzGGHqjStqqVkdXZiWHH), 
			ControllerType.Keyboard => tCLNIawgRirkkeTAwgbgLCflRZH(OlktKbOjbgGrTJEsAkUzsiYeMHYb), 
			ControllerType.Mouse => tCLNIawgRirkkeTAwgbgLCflRZH(dEcTjrSkAbFekEIeQDAfncJOVAhdA), 
			ControllerType.Custom => tCLNIawgRirkkeTAwgbgLCflRZH(vUFoteRIgwMjMHesIRSyNjZtxfgn), 
			_ => throw new NotImplementedException(), 
		};
	}

	private bool BFKZawnovHBiifPPnuuSRDawTwze<_0001>(IList<_0001> P_0) where _0001 : Controller
	{
		if (P_0 == null)
		{
			return false;
		}
		int count = P_0.Count;
		for (int i = 0; i < count; i++)
		{
			_0001 val = P_0[i];
			if (val != null && val.GetAnyButton())
			{
				return true;
			}
		}
		return false;
	}

	private bool BFKZawnovHBiifPPnuuSRDawTwze(Controller P_0)
	{
		return P_0?.GetAnyButton() ?? false;
	}

	private bool VSTjcPRPJuIMVohKVeAMxLRVDISe<_0001>(IList<_0001> P_0) where _0001 : Controller
	{
		if (P_0 == null)
		{
			return false;
		}
		int count = P_0.Count;
		for (int i = 0; i < count; i++)
		{
			_0001 val = P_0[i];
			if (val != null && val.GetAnyButtonDown())
			{
				return true;
			}
		}
		return false;
	}

	private bool VSTjcPRPJuIMVohKVeAMxLRVDISe(Controller P_0)
	{
		return P_0?.GetAnyButtonDown() ?? false;
	}

	private bool huUGSZvDeDGtAwKwPyflEhkiDTSU<_0001>(IList<_0001> P_0) where _0001 : Controller
	{
		if (P_0 == null)
		{
			return false;
		}
		int count = P_0.Count;
		for (int i = 0; i < count; i++)
		{
			_0001 val = P_0[i];
			if (val != null && val.GetAnyButtonUp())
			{
				return true;
			}
		}
		return false;
	}

	private bool huUGSZvDeDGtAwKwPyflEhkiDTSU(Controller P_0)
	{
		return P_0?.GetAnyButtonUp() ?? false;
	}

	private bool cwGisSwHogQrGVvEJHgYIEyuPaOv<_0001>(IList<_0001> P_0) where _0001 : Controller
	{
		if (P_0 == null)
		{
			return false;
		}
		int count = P_0.Count;
		for (int i = 0; i < count; i++)
		{
			_0001 val = P_0[i];
			if (val != null && val.GetAnyButtonChanged())
			{
				return true;
			}
		}
		return false;
	}

	private bool cwGisSwHogQrGVvEJHgYIEyuPaOv(Controller P_0)
	{
		return P_0?.GetAnyButtonChanged() ?? false;
	}

	private bool tCLNIawgRirkkeTAwgbgLCflRZH<_0001>(IList<_0001> P_0) where _0001 : Controller
	{
		if (P_0 == null)
		{
			return false;
		}
		int count = P_0.Count;
		for (int i = 0; i < count; i++)
		{
			_0001 val = P_0[i];
			if (val != null && val.GetAnyButtonPrev())
			{
				return true;
			}
		}
		return false;
	}

	private bool tCLNIawgRirkkeTAwgbgLCflRZH(Controller P_0)
	{
		return P_0?.GetAnyButtonPrev() ?? false;
	}

	public Controller hVkRNMWgAYioJtcYTeDknaJRusNi()
	{
		Controller lastController = null;
		double lastTime = 0.0;
		InputTools.CompareLastActiveController(dEcTjrSkAbFekEIeQDAfncJOVAhdA, ref lastController, ref lastTime);
		InputTools.CompareLastActiveController(OlktKbOjbgGrTJEsAkUzsiYeMHYb, ref lastController, ref lastTime);
		IList<Joystick> yYasSHCtpzGGHqjStqqVkdXZiWHH = YYasSHCtpzGGHqjStqqVkdXZiWHH;
		for (int i = 0; i < OeeHBecrguVtJOJkdEjZKpQgYwls; i++)
		{
			InputTools.CompareLastActiveController(yYasSHCtpzGGHqjStqqVkdXZiWHH[i], ref lastController, ref lastTime);
		}
		IList<CustomController> list = vUFoteRIgwMjMHesIRSyNjZtxfgn;
		for (int j = 0; j < GAMwoDaktPkGWjOteLxMlJnoastv; j++)
		{
			InputTools.CompareLastActiveController(list[j], ref lastController, ref lastTime);
		}
		if (lastController == null)
		{
			lastController = OlktKbOjbgGrTJEsAkUzsiYeMHYb;
		}
		return lastController;
	}

	public Controller hVkRNMWgAYioJtcYTeDknaJRusNi(ControllerType P_0)
	{
		Controller lastController = null;
		double lastTime = 0.0;
		switch (P_0)
		{
		case ControllerType.Joystick:
		{
			int count = YYasSHCtpzGGHqjStqqVkdXZiWHH.Count;
			for (int j = 0; j < count; j++)
			{
				InputTools.CompareLastActiveController(YYasSHCtpzGGHqjStqqVkdXZiWHH[j], ref lastController, ref lastTime);
			}
			break;
		}
		case ControllerType.Keyboard:
			return OurrBprwSKVEMTXHNSeMGLuBHall;
		case ControllerType.Mouse:
			return rgqCDZHatmAjYfIausJQUnRSqVpAA;
		case ControllerType.Custom:
		{
			int count = vUFoteRIgwMjMHesIRSyNjZtxfgn.Count;
			for (int i = 0; i < count; i++)
			{
				InputTools.CompareLastActiveController(vUFoteRIgwMjMHesIRSyNjZtxfgn[i], ref lastController, ref lastTime);
			}
			break;
		}
		default:
			throw new NotImplementedException();
		}
		return lastController;
	}

	public _0001 hVkRNMWgAYioJtcYTeDknaJRusNi<_0001>() where _0001 : Controller
	{
		Type typeFromHandle = typeof(_0001);
		if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Joystick)))
		{
			return hVkRNMWgAYioJtcYTeDknaJRusNi(ControllerType.Joystick) as _0001;
		}
		if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Keyboard)))
		{
			return hVkRNMWgAYioJtcYTeDknaJRusNi(ControllerType.Keyboard) as _0001;
		}
		if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(CustomController)))
		{
			return hVkRNMWgAYioJtcYTeDknaJRusNi(ControllerType.Custom) as _0001;
		}
		if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Mouse)))
		{
			return hVkRNMWgAYioJtcYTeDknaJRusNi(ControllerType.Mouse) as _0001;
		}
		throw new NotImplementedException();
	}

	public ControllerType xqJfJvckYUGwxCjPVXdZBHEjcqvrA()
	{
		return hVkRNMWgAYioJtcYTeDknaJRusNi()?.type ?? ControllerType.Keyboard;
	}

	public void ffjZabrkykFoYthIJjudFzxTDcXdA(ActiveControllerChangedDelegate P_0)
	{
		if (P_0 != null)
		{
			nzKAgdkNjptUfxogWPtGXJvoIweb = true;
			fyHVWbKFunKAyypmzpTWUaRdcnKjA.vPLgXremUaeOIHOoicNoEutpTYnUb(P_0);
		}
	}

	public void ffjZabrkykFoYthIJjudFzxTDcXdA(ActiveControllerChangedDelegate P_0, ControllerType P_1)
	{
		if (P_0 != null)
		{
			nzKAgdkNjptUfxogWPtGXJvoIweb = true;
			fyHVWbKFunKAyypmzpTWUaRdcnKjA.vPLgXremUaeOIHOoicNoEutpTYnUb(P_0, P_1);
		}
	}

	public void vGCWQiJKsuezzBAZLoskYhZekgebb(ActiveControllerChangedDelegate P_0)
	{
		if (P_0 != null)
		{
			fyHVWbKFunKAyypmzpTWUaRdcnKjA.ICNHcVAorEPdfEFBIbbgdHyoVbAgB(P_0);
		}
	}

	public void AKcoZthCUaHCaaCsWVsXriIBiHuY(ActiveControllerChangedDelegate P_0, ControllerType P_1)
	{
		if (P_0 != null)
		{
			fyHVWbKFunKAyypmzpTWUaRdcnKjA.ICNHcVAorEPdfEFBIbbgdHyoVbAgB(P_0, P_1);
		}
	}

	public void QNWANzBRZPmxOCLzNtEOxRAVONPaA()
	{
		fyHVWbKFunKAyypmzpTWUaRdcnKjA.SPGTRPyvIslcMdbPTItsewSLRPxx();
	}

	public void ffjZabrkykFoYthIJjudFzxTDcXdA(int P_0, PlayerActiveControllerChangedDelegate P_1)
	{
		if (P_1 == null)
		{
			return;
		}
		if (P_0 == 9999999)
		{
			xiubogzFXvIuQeBbfawPsDyKfmce.vPLgXremUaeOIHOoicNoEutpTYnUb(P_1);
		}
		else
		{
			if ((uint)P_0 >= (uint)CQIAQFpzVmISknegYFOIxRunxPpK)
			{
				return;
			}
			diKlwWVZnsNvkfCPbFhcQDkJDIsO[P_0].vPLgXremUaeOIHOoicNoEutpTYnUb(P_1);
		}
		nzKAgdkNjptUfxogWPtGXJvoIweb = true;
	}

	public void ffjZabrkykFoYthIJjudFzxTDcXdA(int P_0, PlayerActiveControllerChangedDelegate P_1, ControllerType P_2)
	{
		if (P_1 == null)
		{
			return;
		}
		if (P_0 == 9999999)
		{
			xiubogzFXvIuQeBbfawPsDyKfmce.vPLgXremUaeOIHOoicNoEutpTYnUb(P_1, P_2);
		}
		else
		{
			if ((uint)P_0 >= (uint)CQIAQFpzVmISknegYFOIxRunxPpK)
			{
				return;
			}
			diKlwWVZnsNvkfCPbFhcQDkJDIsO[P_0].vPLgXremUaeOIHOoicNoEutpTYnUb(P_1, P_2);
		}
		nzKAgdkNjptUfxogWPtGXJvoIweb = true;
	}

	public void vGCWQiJKsuezzBAZLoskYhZekgebb(int P_0, PlayerActiveControllerChangedDelegate P_1)
	{
		if (P_1 != null)
		{
			if (P_0 == 9999999)
			{
				xiubogzFXvIuQeBbfawPsDyKfmce.ICNHcVAorEPdfEFBIbbgdHyoVbAgB(P_1);
			}
			else if ((uint)P_0 < (uint)CQIAQFpzVmISknegYFOIxRunxPpK)
			{
				diKlwWVZnsNvkfCPbFhcQDkJDIsO[P_0].ICNHcVAorEPdfEFBIbbgdHyoVbAgB(P_1);
			}
		}
	}

	public void vGCWQiJKsuezzBAZLoskYhZekgebb(int P_0, PlayerActiveControllerChangedDelegate P_1, ControllerType P_2)
	{
		if (P_1 != null)
		{
			if (P_0 == 9999999)
			{
				xiubogzFXvIuQeBbfawPsDyKfmce.ICNHcVAorEPdfEFBIbbgdHyoVbAgB(P_1, P_2);
			}
			else if ((uint)P_0 < (uint)CQIAQFpzVmISknegYFOIxRunxPpK)
			{
				diKlwWVZnsNvkfCPbFhcQDkJDIsO[P_0].ICNHcVAorEPdfEFBIbbgdHyoVbAgB(P_1, P_2);
			}
		}
	}

	public void QNWANzBRZPmxOCLzNtEOxRAVONPaA(int P_0)
	{
		if (P_0 == 9999999)
		{
			xiubogzFXvIuQeBbfawPsDyKfmce.SPGTRPyvIslcMdbPTItsewSLRPxx();
		}
		else if ((uint)P_0 < (uint)CQIAQFpzVmISknegYFOIxRunxPpK)
		{
			diKlwWVZnsNvkfCPbFhcQDkJDIsO[P_0].SPGTRPyvIslcMdbPTItsewSLRPxx();
		}
	}

	private void EGwFtKcKdrirdnDWIAunJKfRTxueA()
	{
		if (fyHVWbKFunKAyypmzpTWUaRdcnKjA.zdzEDGAaFnidWsIlSukpUKbTQuINA > 0)
		{
			fyHVWbKFunKAyypmzpTWUaRdcnKjA.cUIVJwCvDuwByLhTCDIAFypmcoKK(-1, hVkRNMWgAYioJtcYTeDknaJRusNi(), hVkRNMWgAYioJtcYTeDknaJRusNi(ControllerType.Joystick), hVkRNMWgAYioJtcYTeDknaJRusNi(ControllerType.Custom));
		}
		if (xiubogzFXvIuQeBbfawPsDyKfmce.zdzEDGAaFnidWsIlSukpUKbTQuINA > 0)
		{
			Player.ControllerHelper controllers = hqGXmYxZUrSrCxDobJTGEPCWvHAsA.blBVoZZhgxiIOOqkchCQmhLeDxsEA().controllers;
			xiubogzFXvIuQeBbfawPsDyKfmce.cUIVJwCvDuwByLhTCDIAFypmcoKK(9999999, controllers.GetLastActiveController(), controllers.GetLastActiveController(ControllerType.Joystick), controllers.GetLastActiveController(ControllerType.Custom));
		}
		for (int i = 0; i < CQIAQFpzVmISknegYFOIxRunxPpK; i++)
		{
			if (diKlwWVZnsNvkfCPbFhcQDkJDIsO[i].zdzEDGAaFnidWsIlSukpUKbTQuINA != 0)
			{
				Player.ControllerHelper controllers2 = hqGXmYxZUrSrCxDobJTGEPCWvHAsA.DUOCbsXCfPFpdfdngPcfPQmvfCwTA[i].controllers;
				diKlwWVZnsNvkfCPbFhcQDkJDIsO[i].cUIVJwCvDuwByLhTCDIAFypmcoKK(i, controllers2.GetLastActiveController(), controllers2.GetLastActiveController(ControllerType.Joystick), controllers2.GetLastActiveController(ControllerType.Custom));
			}
		}
	}

	public void bScBilCiPympXpnYaJzxzFWvePuLA(ThrottleCalibrationMode P_0)
	{
		for (int i = 0; i < YYasSHCtpzGGHqjStqqVkdXZiWHH.Count; i++)
		{
			if (YYasSHCtpzGGHqjStqqVkdXZiWHH[i] != null)
			{
				bScBilCiPympXpnYaJzxzFWvePuLA(YYasSHCtpzGGHqjStqqVkdXZiWHH[i], P_0);
			}
		}
		for (int j = 0; j < hXYIgyCpgDGjSSuuYxDApkRKvfGq.Count; j++)
		{
			if (hXYIgyCpgDGjSSuuYxDApkRKvfGq[j] != null)
			{
				bScBilCiPympXpnYaJzxzFWvePuLA(hXYIgyCpgDGjSSuuYxDApkRKvfGq[j], P_0);
			}
		}
		for (int k = 0; k < GAMwoDaktPkGWjOteLxMlJnoastv; k++)
		{
			if (vUFoteRIgwMjMHesIRSyNjZtxfgn[k] != null)
			{
				bScBilCiPympXpnYaJzxzFWvePuLA(vUFoteRIgwMjMHesIRSyNjZtxfgn[k], P_0);
			}
		}
		bScBilCiPympXpnYaJzxzFWvePuLA(dEcTjrSkAbFekEIeQDAfncJOVAhdA, P_0);
	}

	private void bScBilCiPympXpnYaJzxzFWvePuLA(ControllerWithAxes P_0, ThrottleCalibrationMode P_1)
	{
		IList<Controller.Axis> axes = P_0.Axes;
		for (int i = 0; i < P_0.axisCount; i++)
		{
			if (axes[i].EKLxpZvxKyeifPSbgLSzhhhiYuui._specialAxisType == SpecialAxisType.Throttle)
			{
				P_0.calibrationMap.Axes[i].calibrationMode = EnumConverter.ToAlternateAxisCalibrationType(P_1);
			}
		}
	}

	public IList<_0001> yiiCShGioGtoKQqhkzTTNUynLASJ<_0001>() where _0001 : IControllerTemplate
	{
		return sOyJxxGPTWlVyzPJIvrZyYyRatmE.eSISbadMeLSkFsmVxVmjgTxXfofi<_0001>();
	}

	private void zQQfvDZMmpVqPPLYlLuSJXXpwJcI(List<InputBehavior> P_0)
	{
		GAwkqrvWlvpikRXvCLOCwZfKeMsC = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC;
		hqGXmYxZUrSrCxDobJTGEPCWvHAsA = ReInput.hqGXmYxZUrSrCxDobJTGEPCWvHAsA;
		YYasSHCtpzGGHqjStqqVkdXZiWHH = new List<Joystick>();
		hXYIgyCpgDGjSSuuYxDApkRKvfGq = new List<Joystick>();
		vUFoteRIgwMjMHesIRSyNjZtxfgn = new List<CustomController>();
		aLLlpNUjcfjiKpSIORbqbGLhIBol = GAwkqrvWlvpikRXvCLOCwZfKeMsC.aLLlpNUjcfjiKpSIORbqbGLhIBol;
		CQIAQFpzVmISknegYFOIxRunxPpK = hqGXmYxZUrSrCxDobJTGEPCWvHAsA.CQIAQFpzVmISknegYFOIxRunxPpK;
		jTGnCWWqtRRsUxstEqWASkKUAfYd = qhlUBnGLgjQhLXJrucHkgATYWhwEA;
		IagzVHFlWCLJUEfGubXiUScVfGmCA = 0;
		hmyGBQfyaWazZLqcjhHmFczCcGjfB = new ADictionary<int, PJMZIbSFLOAsjEebtpNfFsEvvMVbb>();
		hmyGBQfyaWazZLqcjhHmFczCcGjfB.Add(ReInput.players.GetSystemPlayer().id, new PJMZIbSFLOAsjEebtpNfFsEvvMVbb(P_0));
		IList<Player> players = ReInput.players.Players;
		for (int i = 0; i < players.Count; i++)
		{
			hmyGBQfyaWazZLqcjhHmFczCcGjfB.Add(players[i].id, new PJMZIbSFLOAsjEebtpNfFsEvvMVbb(P_0));
		}
		EZkeaGNjmfUUVrykZmLhuXIZgwOf = new ReadOnlyCollection<Joystick>(YYasSHCtpzGGHqjStqqVkdXZiWHH);
		YhufnUJBrjmuLslaygcAwWOOSGscA = new ReadOnlyCollection<CustomController>(vUFoteRIgwMjMHesIRSyNjZtxfgn);
		ItaEjuKtoATtafYloiocFOanqPQc.GJnvKxyTFiqeqoVWKfGaRKIAppiM(dNgrfdLJdsfeJmlYYJJiwGpIkluS);
		ibSPyLhzomBLVYeqxJcktMghzOgj = new ItaEjuKtoATtafYloiocFOanqPQc[(CQIAQFpzVmISknegYFOIxRunxPpK + 1) * aLLlpNUjcfjiKpSIORbqbGLhIBol];
		int num = 0;
		tgNELAiNXZMjZMcUZhoHllhenZUfA = new ItaEjuKtoATtafYloiocFOanqPQc[aLLlpNUjcfjiKpSIORbqbGLhIBol];
		for (int j = 0; j < aLLlpNUjcfjiKpSIORbqbGLhIBol; j++)
		{
			InputAction inputAction = GAwkqrvWlvpikRXvCLOCwZfKeMsC.qGTnUvTooyDyvIZphuDcznGqwKrR(j);
			InputBehavior inputBehavior = hmyGBQfyaWazZLqcjhHmFczCcGjfB[9999999].rtrPDnldpcAJqYNVDRuNTPUDYHqF(inputAction.behaviorId);
			ItaEjuKtoATtafYloiocFOanqPQc itaEjuKtoATtafYloiocFOanqPQc = new ItaEjuKtoATtafYloiocFOanqPQc(9999999, inputAction, inputBehavior, dNgrfdLJdsfeJmlYYJJiwGpIkluS);
			tgNELAiNXZMjZMcUZhoHllhenZUfA[j] = itaEjuKtoATtafYloiocFOanqPQc;
			ibSPyLhzomBLVYeqxJcktMghzOgj[num] = itaEjuKtoATtafYloiocFOanqPQc;
			num++;
		}
		OUADEDsGMLnIILsOyroWSEdclIJD = new ItaEjuKtoATtafYloiocFOanqPQc[CQIAQFpzVmISknegYFOIxRunxPpK, aLLlpNUjcfjiKpSIORbqbGLhIBol];
		for (int k = 0; k < CQIAQFpzVmISknegYFOIxRunxPpK; k++)
		{
			for (int l = 0; l < aLLlpNUjcfjiKpSIORbqbGLhIBol; l++)
			{
				InputAction inputAction2 = GAwkqrvWlvpikRXvCLOCwZfKeMsC.qGTnUvTooyDyvIZphuDcznGqwKrR(l);
				InputBehavior inputBehavior2 = hmyGBQfyaWazZLqcjhHmFczCcGjfB[players[k].id].rtrPDnldpcAJqYNVDRuNTPUDYHqF(inputAction2.behaviorId);
				ItaEjuKtoATtafYloiocFOanqPQc itaEjuKtoATtafYloiocFOanqPQc2 = new ItaEjuKtoATtafYloiocFOanqPQc(k, inputAction2, inputBehavior2, dNgrfdLJdsfeJmlYYJJiwGpIkluS);
				OUADEDsGMLnIILsOyroWSEdclIJD[k, l] = itaEjuKtoATtafYloiocFOanqPQc2;
				ibSPyLhzomBLVYeqxJcktMghzOgj[num] = itaEjuKtoATtafYloiocFOanqPQc2;
				num++;
			}
		}
		IList<Player_Editor> list = ReInput.UserData.WEZyqeTcxBEnozDVUGnNzVZBqwqJ;
		if (list == null)
		{
			throw new ArgumentNullException("Players cannot be null!");
		}
		for (int m = 0; m < list.Count; m++)
		{
			List<Player_Editor.CreateControllerInfo> startingCustomControllers = list[m].startingCustomControllers;
			if (startingCustomControllers == null)
			{
				continue;
			}
			for (int n = 0; n < startingCustomControllers.Count; n++)
			{
				CustomController customController = labQcDriOISpTvNogGqpTBqIeJDA(startingCustomControllers[n].sourceId);
				if (customController != null)
				{
					customController.tag = startingCustomControllers[n].tag;
					int num2 = ((m == 0) ? 9999999 : (m - 1));
					hqGXmYxZUrSrCxDobJTGEPCWvHAsA.ynWFEmrsktqGecVuJbofDaNeQFxn(num2)?.controllers.mzykFmvMViYixIPuHDidbTcTViYtA(customController, false);
				}
			}
		}
		YIYErldVrijfYfrevxuAbluBNlnI = new gTyYgmXURpDluqEDVAzYahaASSNCb();
		RfSHmwHwAbxbYEnRZzPmlzoVqRaV = new gTyYgmXURpDluqEDVAzYahaASSNCb[CQIAQFpzVmISknegYFOIxRunxPpK];
		for (int num3 = 0; num3 < CQIAQFpzVmISknegYFOIxRunxPpK; num3++)
		{
			RfSHmwHwAbxbYEnRZzPmlzoVqRaV[num3] = new gTyYgmXURpDluqEDVAzYahaASSNCb();
		}
		fyHVWbKFunKAyypmzpTWUaRdcnKjA = new KxFbllcCGygjmUeLFqyAeiTIGWMf<ActiveControllerChangedDelegate>();
		xiubogzFXvIuQeBbfawPsDyKfmce = new KxFbllcCGygjmUeLFqyAeiTIGWMf<PlayerActiveControllerChangedDelegate>();
		diKlwWVZnsNvkfCPbFhcQDkJDIsO = new KxFbllcCGygjmUeLFqyAeiTIGWMf<PlayerActiveControllerChangedDelegate>[hqGXmYxZUrSrCxDobJTGEPCWvHAsA.CQIAQFpzVmISknegYFOIxRunxPpK];
		ArrayTools.Populate(diKlwWVZnsNvkfCPbFhcQDkJDIsO);
	}

	private void hBilpTcOigSnyvaPUXGaPrYstwEJ(UpdateLoopType P_0)
	{
		int count = YYasSHCtpzGGHqjStqqVkdXZiWHH.Count;
		for (int i = 0; i < count; i++)
		{
			Joystick joystick = YYasSHCtpzGGHqjStqqVkdXZiWHH[i];
			if (joystick.enabled)
			{
				xeKrMCpTCMtvKGhOQAkmgoChmmzib(joystick.zfwMqMcMdqXTQMwrxKsBjTZHvIF, joystick.BkAqQtJmzNvLobJflzLPUhNYRphu);
				joystick.PwSVyxiNOmkyuuXUHWyuymUyabog(P_0);
			}
		}
		if (OlktKbOjbgGrTJEsAkUzsiYeMHYb.enabled)
		{
			OlktKbOjbgGrTJEsAkUzsiYeMHYb.PwSVyxiNOmkyuuXUHWyuymUyabog(P_0);
		}
		else if (sXLLyWhVtnTsJUdLYDsshzdInxBV)
		{
			OlktKbOjbgGrTJEsAkUzsiYeMHYb.DtgpVXPyTPhskVKHNnJAXrucMrXt(P_0);
		}
		if (dEcTjrSkAbFekEIeQDAfncJOVAhdA.enabled)
		{
			dEcTjrSkAbFekEIeQDAfncJOVAhdA.PwSVyxiNOmkyuuXUHWyuymUyabog(P_0);
		}
		int count2 = vUFoteRIgwMjMHesIRSyNjZtxfgn.Count;
		for (int j = 0; j < count2; j++)
		{
			CustomController customController = vUFoteRIgwMjMHesIRSyNjZtxfgn[j];
			if (customController.enabled)
			{
				customController.pPRJisySYxBqxxleQgjLYfheiyTx();
				customController.PwSVyxiNOmkyuuXUHWyuymUyabog(P_0);
			}
		}
	}

	private void YkzWSxoRkwVnuhNLyISugAdxcHKJA(UpdateLoopType P_0)
	{
		ItaEjuKtoATtafYloiocFOanqPQc.TgGIZddcnIcjkrnvuFBsfzzerktR(P_0);
		Player[] array = hqGXmYxZUrSrCxDobJTGEPCWvHAsA.JfwqcvUYPSLnmJKPxcgsgsLzOVvp;
		int num = array.Length;
		bool enabled = OlktKbOjbgGrTJEsAkUzsiYeMHYb.enabled;
		if (enabled)
		{
			for (int i = 0; i < num; i++)
			{
				IList<KeyboardMap> maps = array[i].controllers.maps.GetMaps<KeyboardMap>(0);
				int count = maps.Count;
				for (int j = 0; j < count; j++)
				{
					if (maps[j].enabled)
					{
						tDEfcwuerNXHdNWhqfUXcHaLJUBh.BSeGhuIEzhduphsRnoNEHUwZyfSw(maps[j]);
					}
				}
			}
		}
		bool enabled2 = dEcTjrSkAbFekEIeQDAfncJOVAhdA.enabled;
		for (int k = 0; k < num; k++)
		{
			Player.ControllerHelper controllers = array[k].controllers;
			controllers.KHujJUDVCavZXgaNEjdghqhUcmyP(jTGnCWWqtRRsUxstEqWASkKUAfYd);
			if (enabled || sXLLyWhVtnTsJUdLYDsshzdInxBV)
			{
				controllers.bIHunaRKeVWuyrOragQBInAticEfA(OlktKbOjbgGrTJEsAkUzsiYeMHYb, tDEfcwuerNXHdNWhqfUXcHaLJUBh, jTGnCWWqtRRsUxstEqWASkKUAfYd);
			}
			if (enabled2)
			{
				controllers.cOlvUlHpEvAaKKCzZhAPbRillpOuA(dEcTjrSkAbFekEIeQDAfncJOVAhdA, jTGnCWWqtRRsUxstEqWASkKUAfYd);
			}
			controllers.WmzEOiiLRPquVDkqmowgbepGMeJFc(jTGnCWWqtRRsUxstEqWASkKUAfYd);
		}
		for (int l = 0; l < ibSPyLhzomBLVYeqxJcktMghzOgj.Length; l++)
		{
			if (ibSPyLhzomBLVYeqxJcktMghzOgj[l].kQHeClDIlBGPmjlYBLdULwBWzqLWb != ItaEjuKtoATtafYloiocFOanqPQc.xjKfyCGuYDcbrJYexAtAnkygtlwsA.Disabled)
			{
				ibSPyLhzomBLVYeqxJcktMghzOgj[l].AkRVFMcwbJpEzFTqnbHSXGJzEJIG();
			}
		}
		ItaEjuKtoATtafYloiocFOanqPQc.GGnfmqgvBLvqYJQcSVGrTbzpFyTs();
		if (!hnvMKVcKgNVmSXgWGXLrQdCHStBg)
		{
			return;
		}
		if (YIYErldVrijfYfrevxuAbluBNlnI.VulcQkzEFSldNXBvZXTerjMDDble > 0)
		{
			for (int m = 0; m < aLLlpNUjcfjiKpSIORbqbGLhIBol; m++)
			{
				ItaEjuKtoATtafYloiocFOanqPQc itaEjuKtoATtafYloiocFOanqPQc = tgNELAiNXZMjZMcUZhoHllhenZUfA[m];
				if (itaEjuKtoATtafYloiocFOanqPQc.kQHeClDIlBGPmjlYBLdULwBWzqLWb != ItaEjuKtoATtafYloiocFOanqPQc.xjKfyCGuYDcbrJYexAtAnkygtlwsA.Disabled)
				{
					YIYErldVrijfYfrevxuAbluBNlnI.mAMBMpqTbYtzNRCEtBamaAccLfJGb(itaEjuKtoATtafYloiocFOanqPQc, P_0);
				}
			}
		}
		for (int n = 0; n < CQIAQFpzVmISknegYFOIxRunxPpK; n++)
		{
			gTyYgmXURpDluqEDVAzYahaASSNCb gTyYgmXURpDluqEDVAzYahaASSNCb2 = RfSHmwHwAbxbYEnRZzPmlzoVqRaV[n];
			if (gTyYgmXURpDluqEDVAzYahaASSNCb2.VulcQkzEFSldNXBvZXTerjMDDble == 0)
			{
				continue;
			}
			for (int num2 = 0; num2 < aLLlpNUjcfjiKpSIORbqbGLhIBol; num2++)
			{
				ItaEjuKtoATtafYloiocFOanqPQc itaEjuKtoATtafYloiocFOanqPQc2 = OUADEDsGMLnIILsOyroWSEdclIJD[n, num2];
				if (itaEjuKtoATtafYloiocFOanqPQc2.kQHeClDIlBGPmjlYBLdULwBWzqLWb != ItaEjuKtoATtafYloiocFOanqPQc.xjKfyCGuYDcbrJYexAtAnkygtlwsA.Disabled)
				{
					gTyYgmXURpDluqEDVAzYahaASSNCb2.mAMBMpqTbYtzNRCEtBamaAccLfJGb(itaEjuKtoATtafYloiocFOanqPQc2, P_0);
				}
			}
		}
	}

	private void qhlUBnGLgjQhLXJrucHkgATYWhwEA(bool P_0, int P_1, int P_2)
	{
		int num = GAwkqrvWlvpikRXvCLOCwZfKeMsC.lsWdiPZgHHfEfIiesCpnLAlcpBgUA(P_2);
		if (num >= 0)
		{
			if (P_1 == 9999999)
			{
				tgNELAiNXZMjZMcUZhoHllhenZUfA[num].DzBUiSxwHAdhIazTMHCVUnJeeZGe(P_0);
			}
			else
			{
				OUADEDsGMLnIILsOyroWSEdclIJD[P_1, num].DzBUiSxwHAdhIazTMHCVUnJeeZGe(P_0);
			}
		}
	}

	private void ZbcDSPekYnlStRgRugzyetSbWkxBc(BridgedController P_0)
	{
		int num = bDJkdzIZfPLqSPLEjCUWkRdPnCwx(P_0.sourceJoystick.rewiredId, cRwcHsJJSwdOXPswvlQsCSGPrDri.Connected);
		if (num >= 0)
		{
			Logger.LogError("Controller was already in connected list!");
			return;
		}
		num = bDJkdzIZfPLqSPLEjCUWkRdPnCwx(P_0.sourceJoystick.rewiredId, cRwcHsJJSwdOXPswvlQsCSGPrDri.Disconnected);
		Joystick joystick;
		if (num >= 0)
		{
			joystick = hXYIgyCpgDGjSSuuYxDApkRKvfGq[num];
			hXYIgyCpgDGjSSuuYxDApkRKvfGq.RemoveAt(num);
			joystick.UByBUxdeeRfyWdxWnNFetplCMBqd(P_0);
			joystick.isConnected = true;
		}
		else
		{
			joystick = new Joystick(P_0);
		}
		YYasSHCtpzGGHqjStqqVkdXZiWHH.Add(joystick);
		xLmeyCEvHjFDowSsezuNCUAMXWmL.Add(joystick);
		YYasSHCtpzGGHqjStqqVkdXZiWHH.Sort(Joystick.BLBeFCaTjYjfxFliEgaztNfvQKfCA);
		sOyJxxGPTWlVyzPJIvrZyYyRatmE.ycndZKmKnXmWelVDbcsLODEZWhMV(joystick);
	}

	private void KdhLmBFFAJHZLrHKkMIuhTdMdfAv(int P_0)
	{
		if (P_0 < 0)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (P_0 >= YYasSHCtpzGGHqjStqqVkdXZiWHH.Count)
		{
			Logger.LogError("Device was not in connected list! Cannot remove!");
			return;
		}
		Joystick joystick = YYasSHCtpzGGHqjStqqVkdXZiWHH[P_0];
		joystick.isConnected = false;
		if (kGVOImGbVNwKiMTMLDkVUZdzwWAI != null)
		{
			kGVOImGbVNwKiMTMLDkVUZdzwWAI(new ControllerStatusChangedEventArgs(joystick.name, joystick.id, joystick.type));
		}
		if (lJrmwhYszLAVKekZLdGKVsHAVsdp != null)
		{
			lJrmwhYszLAVKekZLdGKVsHAVsdp(joystick.type, joystick.id);
		}
		YYasSHCtpzGGHqjStqqVkdXZiWHH.RemoveAt(P_0);
		hXYIgyCpgDGjSSuuYxDApkRKvfGq.Add(joystick);
		xLmeyCEvHjFDowSsezuNCUAMXWmL.Remove(joystick);
		sOyJxxGPTWlVyzPJIvrZyYyRatmE.FcypqzUtVfbvdvFSJGJKEXpZVBLaA(joystick);
		joystick.SPGTRPyvIslcMdbPTItsewSLRPxx();
	}

	private void tJtYoywWNrcxhKllHLiKeQJoxVSP()
	{
		for (int num = YYasSHCtpzGGHqjStqqVkdXZiWHH.Count - 1; num >= 0; num--)
		{
			KdhLmBFFAJHZLrHKkMIuhTdMdfAv(num);
		}
	}

	private bool mzykFmvMViYixIPuHDidbTcTViYtA(CustomController P_0)
	{
		if (P_0 == null)
		{
			return false;
		}
		for (int i = 0; i < vUFoteRIgwMjMHesIRSyNjZtxfgn.Count; i++)
		{
			if (vUFoteRIgwMjMHesIRSyNjZtxfgn[i] == P_0)
			{
				return true;
			}
		}
		vUFoteRIgwMjMHesIRSyNjZtxfgn.Add(P_0);
		xLmeyCEvHjFDowSsezuNCUAMXWmL.Add(P_0);
		sOyJxxGPTWlVyzPJIvrZyYyRatmE.ycndZKmKnXmWelVDbcsLODEZWhMV(P_0);
		return true;
	}

	private bool iuacuYVnEcyuHqNzXCqSIHTRrksK(CustomController P_0)
	{
		if (P_0 == null)
		{
			return false;
		}
		sOyJxxGPTWlVyzPJIvrZyYyRatmE.FcypqzUtVfbvdvFSJGJKEXpZVBLaA(P_0);
		xLmeyCEvHjFDowSsezuNCUAMXWmL.Remove(P_0);
		return vUFoteRIgwMjMHesIRSyNjZtxfgn.Remove(P_0);
	}

	private gTyYgmXURpDluqEDVAzYahaASSNCb LwOPFbIjZVapvmlQStJRsGcCHOhV(int P_0)
	{
		if (P_0 == 9999999)
		{
			return YIYErldVrijfYfrevxuAbluBNlnI;
		}
		if (P_0 < 0 || P_0 >= ReInput.hqGXmYxZUrSrCxDobJTGEPCWvHAsA.CQIAQFpzVmISknegYFOIxRunxPpK)
		{
			return null;
		}
		return RfSHmwHwAbxbYEnRZzPmlzoVqRaV[P_0];
	}

	private void SfHweRCGppaJxMUeKCMqXVNgaQzCA(bool P_0)
	{
		if (!P_0)
		{
			tDEfcwuerNXHdNWhqfUXcHaLJUBh.HPdhUycvZEuKWkzUokzqofIAeNbfb();
		}
	}

	private void xKXEQmiiNXKorUsTsPlIOtwcwnfr(bool P_0)
	{
		OlktKbOjbgGrTJEsAkUzsiYeMHYb.xKXEQmiiNXKorUsTsPlIOtwcwnfr(P_0);
		dEcTjrSkAbFekEIeQDAfncJOVAhdA.xKXEQmiiNXKorUsTsPlIOtwcwnfr(P_0);
		for (int i = 0; i < YYasSHCtpzGGHqjStqqVkdXZiWHH.Count; i++)
		{
			YYasSHCtpzGGHqjStqqVkdXZiWHH[i].xKXEQmiiNXKorUsTsPlIOtwcwnfr(P_0);
		}
		for (int j = 0; j < vUFoteRIgwMjMHesIRSyNjZtxfgn.Count; j++)
		{
			vUFoteRIgwMjMHesIRSyNjZtxfgn[j].xKXEQmiiNXKorUsTsPlIOtwcwnfr(P_0);
		}
	}

	public void Dispose()
	{
		oTWKNdZEHnDTTJoUPKlJFjuBkKdNb(true);
		GC.SuppressFinalize(this);
	}

	protected void iMcWPzJbivbQRVFtjrscsWpjuUvv()
	{
		try
		{
			oTWKNdZEHnDTTJoUPKlJFjuBkKdNb(false);
		}
		finally
		{
			base.Finalize();
		}
	}

	private void oTWKNdZEHnDTTJoUPKlJFjuBkKdNb(bool P_0)
	{
		if (AeWaeWamxRrERkciQkpFDWbRfZMkA)
		{
			return;
		}
		if (P_0)
		{
			if (tSzApJiwheMYhjgvAaTrNbSHdHgP is IDisposable)
			{
				(tSzApJiwheMYhjgvAaTrNbSHdHgP as IDisposable).Dispose();
			}
			if (CDQvdxrBYeaibeuILXSWZTLflVSS is IDisposable)
			{
				(CDQvdxrBYeaibeuILXSWZTLflVSS as IDisposable).Dispose();
			}
		}
		AeWaeWamxRrERkciQkpFDWbRfZMkA = true;
	}
}
