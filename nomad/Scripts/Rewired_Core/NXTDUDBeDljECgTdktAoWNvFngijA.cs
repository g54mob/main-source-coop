using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rewired;
using Rewired.Config;
using Rewired.Data;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;

internal sealed class NXTDUDBeDljECgTdktAoWNvFngijA : IDisposable
{
	public enum QIwlYaqDyNkycuuFpHTnXgkLkQvM
	{
		Connected = 0,
		Disconnected = 1
	}

	private class PMmWeSKIIthraMbmySYLmpDneVdh
	{
		public ADictionary<int, InputBehavior> SqycuPxyLqojCaThePdBZuBNHill;

		public List<InputBehavior> gjVZFLBbwEDEyWXKFcJxHBuHNmRx;

		public IList<InputBehavior> jSAPMexwxCRLqTeunZlegmZKaMDT;

		public PMmWeSKIIthraMbmySYLmpDneVdh(List<InputBehavior> P_0)
		{
			gjVZFLBbwEDEyWXKFcJxHBuHNmRx = new List<InputBehavior>(P_0.Count);
			SqycuPxyLqojCaThePdBZuBNHill = new ADictionary<int, InputBehavior>();
			int num = 0;
			for (int i = 0; i < P_0.Count; i++)
			{
				InputBehavior inputBehavior = P_0[i].Clone();
				SqycuPxyLqojCaThePdBZuBNHill.Add(P_0[i].id, inputBehavior);
				gjVZFLBbwEDEyWXKFcJxHBuHNmRx.Add(inputBehavior);
				num++;
			}
			jSAPMexwxCRLqTeunZlegmZKaMDT = new ReadOnlyCollection<InputBehavior>(gjVZFLBbwEDEyWXKFcJxHBuHNmRx);
		}

		public InputBehavior qmwkmQRlOiYPAqLQMIfGwHwmAlYIA(int P_0)
		{
			if (gjVZFLBbwEDEyWXKFcJxHBuHNmRx.Count == 0)
			{
				return null;
			}
			SqycuPxyLqojCaThePdBZuBNHill.TryGetValue(P_0, out var value);
			if (value == null)
			{
				return gjVZFLBbwEDEyWXKFcJxHBuHNmRx[0];
			}
			return value;
		}
	}

	private sealed class JkcUaJSrjlOrNvKovejJHJOScGvbb : IEnumerable<CustomController>, IEnumerable, IEnumerator<CustomController>, IEnumerator, IDisposable
	{
		private int LDEVYYhcXzfbaFdBKjFXACkxrXZMA;

		private CustomController tKLyNCQgKWLFTowMksOOULhMafDgA;

		private int VwLwEplAscqAuLxDsfjWUpjhcFLs;

		public NXTDUDBeDljECgTdktAoWNvFngijA YnmItmIbMhIdxNGiMiihiSVhJCXLA;

		private int lnLtUWsLBFOdwUEBqDcyGFrCydVTA;

		public int SjLVBwimhDBpiHLjcMnNQpSZNGJw;

		private int dGmBMSEgzintjdLNBaxPldXfLadP;

		private int ZMfLjgFpqUCpSZZReVjNvVEVGOWf;

		CustomController IEnumerator<CustomController>.Current
		{
			[DebuggerHidden]
			get
			{
				return tKLyNCQgKWLFTowMksOOULhMafDgA;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return tKLyNCQgKWLFTowMksOOULhMafDgA;
			}
		}

		[DebuggerHidden]
		public JkcUaJSrjlOrNvKovejJHJOScGvbb(int P_0)
		{
			LDEVYYhcXzfbaFdBKjFXACkxrXZMA = P_0;
			VwLwEplAscqAuLxDsfjWUpjhcFLs = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			LDEVYYhcXzfbaFdBKjFXACkxrXZMA = -2;
		}

		private bool MoveNext()
		{
			int lDEVYYhcXzfbaFdBKjFXACkxrXZMA = LDEVYYhcXzfbaFdBKjFXACkxrXZMA;
			NXTDUDBeDljECgTdktAoWNvFngijA ynmItmIbMhIdxNGiMiihiSVhJCXLA = YnmItmIbMhIdxNGiMiihiSVhJCXLA;
			if (lDEVYYhcXzfbaFdBKjFXACkxrXZMA != 0)
			{
				if (lDEVYYhcXzfbaFdBKjFXACkxrXZMA != 1)
				{
					return false;
				}
				LDEVYYhcXzfbaFdBKjFXACkxrXZMA = -1;
				goto IL_007d;
			}
			LDEVYYhcXzfbaFdBKjFXACkxrXZMA = -1;
			dGmBMSEgzintjdLNBaxPldXfLadP = ynmItmIbMhIdxNGiMiihiSVhJCXLA.WaJIozXsYamtRkLzfYkBjnjxfrzl.Count;
			ZMfLjgFpqUCpSZZReVjNvVEVGOWf = 0;
			goto IL_008d;
			IL_007d:
			ZMfLjgFpqUCpSZZReVjNvVEVGOWf++;
			goto IL_008d;
			IL_008d:
			if (ZMfLjgFpqUCpSZZReVjNvVEVGOWf < dGmBMSEgzintjdLNBaxPldXfLadP)
			{
				if (ynmItmIbMhIdxNGiMiihiSVhJCXLA.WaJIozXsYamtRkLzfYkBjnjxfrzl[ZMfLjgFpqUCpSZZReVjNvVEVGOWf].sourceControllerId == lnLtUWsLBFOdwUEBqDcyGFrCydVTA)
				{
					tKLyNCQgKWLFTowMksOOULhMafDgA = ynmItmIbMhIdxNGiMiihiSVhJCXLA.WaJIozXsYamtRkLzfYkBjnjxfrzl[ZMfLjgFpqUCpSZZReVjNvVEVGOWf];
					LDEVYYhcXzfbaFdBKjFXACkxrXZMA = 1;
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
			JkcUaJSrjlOrNvKovejJHJOScGvbb jkcUaJSrjlOrNvKovejJHJOScGvbb;
			if (LDEVYYhcXzfbaFdBKjFXACkxrXZMA == -2 && VwLwEplAscqAuLxDsfjWUpjhcFLs == Environment.CurrentManagedThreadId)
			{
				LDEVYYhcXzfbaFdBKjFXACkxrXZMA = 0;
				jkcUaJSrjlOrNvKovejJHJOScGvbb = this;
			}
			else
			{
				jkcUaJSrjlOrNvKovejJHJOScGvbb = new JkcUaJSrjlOrNvKovejJHJOScGvbb(0);
				jkcUaJSrjlOrNvKovejJHJOScGvbb.YnmItmIbMhIdxNGiMiihiSVhJCXLA = YnmItmIbMhIdxNGiMiihiSVhJCXLA;
			}
			jkcUaJSrjlOrNvKovejJHJOScGvbb.lnLtUWsLBFOdwUEBqDcyGFrCydVTA = SjLVBwimhDBpiHLjcMnNQpSZNGJw;
			return jkcUaJSrjlOrNvKovejJHJOScGvbb;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<CustomController>)this).GetEnumerator();
		}
	}

	private sealed class XZgpaUYpyQyRnniYLLQVyzyyEqGI : IEnumerable<CustomController>, IEnumerable, IEnumerator<CustomController>, IEnumerator, IDisposable
	{
		private int yvxtuhmqZZpVGhKPknUuHdPIEanA;

		private CustomController SAyNTXPzKQgVhGMiwcdfhENvnDzfb;

		private int DhmjBlGGjTdAamvDEifAaTgHTbNlA;

		public NXTDUDBeDljECgTdktAoWNvFngijA qLfchNdPDyAJEbYegzlEQxjTOqpRB;

		private string VcrGtLImJNAUMNmjrgyFxylMcQyW;

		public string yrRFZEKYXTTimJBaoVcwwBWDoFaM;

		private int pTjNLJddHIGhNKksKWSXLfBAWJbBA;

		private int boDbsAGKoVTEhwMQXIyYTIMeabBc;

		CustomController IEnumerator<CustomController>.Current
		{
			[DebuggerHidden]
			get
			{
				return SAyNTXPzKQgVhGMiwcdfhENvnDzfb;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return SAyNTXPzKQgVhGMiwcdfhENvnDzfb;
			}
		}

		[DebuggerHidden]
		public XZgpaUYpyQyRnniYLLQVyzyyEqGI(int P_0)
		{
			yvxtuhmqZZpVGhKPknUuHdPIEanA = P_0;
			DhmjBlGGjTdAamvDEifAaTgHTbNlA = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			yvxtuhmqZZpVGhKPknUuHdPIEanA = -2;
		}

		private bool MoveNext()
		{
			int num = yvxtuhmqZZpVGhKPknUuHdPIEanA;
			NXTDUDBeDljECgTdktAoWNvFngijA nXTDUDBeDljECgTdktAoWNvFngijA = qLfchNdPDyAJEbYegzlEQxjTOqpRB;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				yvxtuhmqZZpVGhKPknUuHdPIEanA = -1;
				goto IL_0083;
			}
			yvxtuhmqZZpVGhKPknUuHdPIEanA = -1;
			pTjNLJddHIGhNKksKWSXLfBAWJbBA = nXTDUDBeDljECgTdktAoWNvFngijA.WaJIozXsYamtRkLzfYkBjnjxfrzl.Count;
			boDbsAGKoVTEhwMQXIyYTIMeabBc = 0;
			goto IL_0093;
			IL_0083:
			boDbsAGKoVTEhwMQXIyYTIMeabBc++;
			goto IL_0093;
			IL_0093:
			if (boDbsAGKoVTEhwMQXIyYTIMeabBc < pTjNLJddHIGhNKksKWSXLfBAWJbBA)
			{
				if (nXTDUDBeDljECgTdktAoWNvFngijA.WaJIozXsYamtRkLzfYkBjnjxfrzl[boDbsAGKoVTEhwMQXIyYTIMeabBc].tag.Equals(VcrGtLImJNAUMNmjrgyFxylMcQyW, StringComparison.OrdinalIgnoreCase))
				{
					SAyNTXPzKQgVhGMiwcdfhENvnDzfb = nXTDUDBeDljECgTdktAoWNvFngijA.WaJIozXsYamtRkLzfYkBjnjxfrzl[boDbsAGKoVTEhwMQXIyYTIMeabBc];
					yvxtuhmqZZpVGhKPknUuHdPIEanA = 1;
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
			XZgpaUYpyQyRnniYLLQVyzyyEqGI xZgpaUYpyQyRnniYLLQVyzyyEqGI;
			if (yvxtuhmqZZpVGhKPknUuHdPIEanA == -2 && DhmjBlGGjTdAamvDEifAaTgHTbNlA == Environment.CurrentManagedThreadId)
			{
				yvxtuhmqZZpVGhKPknUuHdPIEanA = 0;
				xZgpaUYpyQyRnniYLLQVyzyyEqGI = this;
			}
			else
			{
				xZgpaUYpyQyRnniYLLQVyzyyEqGI = new XZgpaUYpyQyRnniYLLQVyzyyEqGI(0);
				xZgpaUYpyQyRnniYLLQVyzyyEqGI.qLfchNdPDyAJEbYegzlEQxjTOqpRB = qLfchNdPDyAJEbYegzlEQxjTOqpRB;
			}
			xZgpaUYpyQyRnniYLLQVyzyyEqGI.VcrGtLImJNAUMNmjrgyFxylMcQyW = yrRFZEKYXTTimJBaoVcwwBWDoFaM;
			return xZgpaUYpyQyRnniYLLQVyzyyEqGI;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<CustomController>)this).GetEnumerator();
		}
	}

	private List<Joystick> EJCFAvjXToHxuQxXFDPFBZPZTRHdA;

	private List<Joystick> xsGIfXaWUCexIVjJhaiEUfjZlERB;

	private List<CustomController> WaJIozXsYamtRkLzfYkBjnjxfrzl;

	private List<Controller> irVFTHDbZhyARZSjxyajpjzdWWZl;

	private ReadOnlyCollection<Controller> sYfKfyGoCeCLvjAEvCQgLhtZMbCcA;

	private Keyboard OHoFrtAsBjDYugoYVclSaeWlifdaA;

	private Mouse nsRtkUIduvbzdzdONcpKgCDGweMrA;

	private ConfigVars DMfdqwHStwxCULmnZnMMbiIggxxJA;

	private kBOilrfmQspwwsLlQucgVePHzaAKA[] rUhUYPJOLiCyZtlCTemjqjIZJfrU;

	private kBOilrfmQspwwsLlQucgVePHzaAKA[] IleKFPIiyXDJpfDeagGbBQbAXdpDA;

	private kBOilrfmQspwwsLlQucgVePHzaAKA[,] XNnuTExVERvjPlpjlkLPRRqEpuBO;

	private ixyiAxzBBALGbgKSZMZuYYGBvaTW ZvLODOAERMicBraxEvmPYgBDZJwj;

	private IgUVpafhmNYDnMTcltiGEUbuIaMm wouoRPfYwKqZonCFTQkUdmSvVYxE;

	private IgUVpafhmNYDnMTcltiGEUbuIaMm[] ugMqSNXIkwbyvDgnkncjpPoxddXwA;

	private global::mXvUKnAftAGBresozjoMzBMkfSTeb<ActiveControllerChangedDelegate> VUJGoplhMfmICFGAnBidkUoaFRwx;

	private global::mXvUKnAftAGBresozjoMzBMkfSTeb<PlayerActiveControllerChangedDelegate> ylLFDsMXVVVkCVuYwhTWhbBitGVU;

	private global::mXvUKnAftAGBresozjoMzBMkfSTeb<PlayerActiveControllerChangedDelegate>[] wbRAShiNEXppEJXmExAHzYiezVrh;

	private ADictionary<int, PMmWeSKIIthraMbmySYLmpDneVdh> BjRtfcwyrXeCCeKWHcpZMhuRYVRE;

	private readonly BfSuWOtYIJOEShfeXemgQlZkXemn mExieEPzUampKVqNAfqEMTjoTkEP;

	private IList<Joystick> vsCLkOFAmDrSrsKodGFlfXpkAveBA;

	private IList<CustomController> lZohoZfREIwhjTfuQsOdHmreXtok;

	private int cOfTxFQBiJUysoydlMEsSOPnEjFA;

	private bool EACdwryTdNpQNmXOUQUmQMuoSWoo;

	private bool HcvCFkoIMMBtsbTAqcthMvdnyZIGA;

	private bool FXWsOYCOLSmYSQuKOCtwrBHvxYFO;

	private IUnifiedKeyboardSource ljfcKoeBmRxbHsmfiYxvdianzTUQA;

	private IUnifiedMouseSource hUpaWWWbfVwVtttgzQQiNPaISJMP;

	private int NIEiRyFcMCAmnSWBVgKgaXhFUlTV;

	private DckEcfkkHGpczSsaRQGOdUFFpVWD dPqQHnBvkeSkRzJiKdMphfDpwkds;

	private vVGxMaRQSgzzFjtXaZTAmFOGsEmm uZSHzuPrMOGzQpgnxdAvgFnQUdKD;

	private int szFjEAeNlWQWOZoZvkhMqQSzhEgd;

	private int BcGHmmMcpMXMZJLkbFRclNEtxSzU;

	private Action<int, ControllerDataUpdater> mRBrNjRyufqsdvnsXAgOYDzMhgBp;

	private Action<bool, int, int> NUQDPccRpdVUVcpmuuHHIDMlZnwf;

	private Action<ControllerStatusChangedEventArgs> zCgRpqvmjCwzcLEOnWUFUXoZiyM;

	private Action<ControllerType, int> LiOxSUgiRaGswMIKrGqSPfjNCYOd;

	private bool fTIymgMtzYCMvyTanTOcuYUrPtWs;

	public IList<Joystick> vSofOXqlhEhhaEStqlEuVapoCWcU => vsCLkOFAmDrSrsKodGFlfXpkAveBA;

	public List<Joystick> dlyhsOzpuZSaZiMnFGhfDRfxZuhK => EJCFAvjXToHxuQxXFDPFBZPZTRHdA;

	public int tMmOaRdQZinbpzIJqOsQpkLeJiKf => EJCFAvjXToHxuQxXFDPFBZPZTRHdA.Count;

	public Mouse RVEYLKIoOydxyctXJKWZflgnfQyi => nsRtkUIduvbzdzdONcpKgCDGweMrA;

	public Keyboard TVvLxBfEgOqnloHdRFcagvmpmnZT => OHoFrtAsBjDYugoYVclSaeWlifdaA;

	public IList<CustomController> XyyXBqESkHDJwVnEjfVUWHBlFRAaA => lZohoZfREIwhjTfuQsOdHmreXtok;

	public List<CustomController> vLrfsQaNRQZYophCUghOAEVNDjbYA => WaJIozXsYamtRkLzfYkBjnjxfrzl;

	public int OaArgfysJnEWrVVscGbEedXuKtYDA => WaJIozXsYamtRkLzfYkBjnjxfrzl.Count;

	public IList<Controller> qxItpjGgcTPOulCPqnbsIgGhWdnn => sYfKfyGoCeCLvjAEvCQgLhtZMbCcA;

	public int yLjTfJFmvSvPPOwImgrFOhMEynMR => irVFTHDbZhyARZSjxyajpjzdWWZl.Count;

	private int GVQqMztLukeojeTiPawITcgnzxEf
	{
		get
		{
			int nIEiRyFcMCAmnSWBVgKgaXhFUlTV = NIEiRyFcMCAmnSWBVgKgaXhFUlTV;
			NIEiRyFcMCAmnSWBVgKgaXhFUlTV++;
			if (NIEiRyFcMCAmnSWBVgKgaXhFUlTV >= 2147483647)
			{
				NIEiRyFcMCAmnSWBVgKgaXhFUlTV = 0;
			}
			return nIEiRyFcMCAmnSWBVgKgaXhFUlTV;
		}
	}

	public event Action<ControllerStatusChangedEventArgs> lCbtGzMeNwCOvlLkHCUAZALCxwY
	{
		add
		{
			zCgRpqvmjCwzcLEOnWUFUXoZiyM = (Action<ControllerStatusChangedEventArgs>)Delegate.Combine(zCgRpqvmjCwzcLEOnWUFUXoZiyM, b);
		}
		remove
		{
			zCgRpqvmjCwzcLEOnWUFUXoZiyM = (Action<ControllerStatusChangedEventArgs>)Delegate.Remove(zCgRpqvmjCwzcLEOnWUFUXoZiyM, value2);
		}
	}

	public event Action<ControllerType, int> HBrXrEzBGOMoCLqyxxqezaDoNCZO
	{
		add
		{
			LiOxSUgiRaGswMIKrGqSPfjNCYOd = (Action<ControllerType, int>)Delegate.Combine(LiOxSUgiRaGswMIKrGqSPfjNCYOd, b);
		}
		remove
		{
			LiOxSUgiRaGswMIKrGqSPfjNCYOd = (Action<ControllerType, int>)Delegate.Remove(LiOxSUgiRaGswMIKrGqSPfjNCYOd, value2);
		}
	}

	public NXTDUDBeDljECgTdktAoWNvFngijA(ConfigVars P_0, PlatformInputManager P_1)
	{
		DMfdqwHStwxCULmnZnMMbiIggxxJA = P_0;
		cOfTxFQBiJUysoydlMEsSOPnEjFA = 0;
		EACdwryTdNpQNmXOUQUmQMuoSWoo = UnityTools.isAndroidPlatform;
		irVFTHDbZhyARZSjxyajpjzdWWZl = new List<Controller>(10);
		sYfKfyGoCeCLvjAEvCQgLhtZMbCcA = new ReadOnlyCollection<Controller>(irVFTHDbZhyARZSjxyajpjzdWWZl);
		IUnifiedKeyboardSource unifiedKeyboardSource = P_1.GetUnifiedKeyboardSource();
		if (unifiedKeyboardSource == null)
		{
			unifiedKeyboardSource = (ljfcKoeBmRxbHsmfiYxvdianzTUQA = new UnityUnifiedKeyboardSource());
		}
		OHoFrtAsBjDYugoYVclSaeWlifdaA = new Keyboard("Keyboard", unifiedKeyboardSource);
		irVFTHDbZhyARZSjxyajpjzdWWZl.Add(OHoFrtAsBjDYugoYVclSaeWlifdaA);
		IUnifiedMouseSource unifiedMouseSource = P_1.GetUnifiedMouseSource();
		if (unifiedMouseSource == null)
		{
			unifiedMouseSource = (hUpaWWWbfVwVtttgzQQiNPaISJMP = new UnityUnifiedMouseSource());
		}
		nsRtkUIduvbzdzdONcpKgCDGweMrA = new Mouse("Mouse", unifiedMouseSource);
		irVFTHDbZhyARZSjxyajpjzdWWZl.Add(nsRtkUIduvbzdzdONcpKgCDGweMrA);
		ZvLODOAERMicBraxEvmPYgBDZJwj = new ixyiAxzBBALGbgKSZMZuYYGBvaTW(P_0.updateLoop, OHoFrtAsBjDYugoYVclSaeWlifdaA);
		OHoFrtAsBjDYugoYVclSaeWlifdaA.CTMeOjKJARUNRwcCVPoKffFMNnHY += IyKZVGGdqREdMKjAmpGsVQRPsVop;
		OHoFrtAsBjDYugoYVclSaeWlifdaA.enabled = !P_0.GetPlatformVar_disableKeyboard();
		nsRtkUIduvbzdzdONcpKgCDGweMrA.enabled = !P_0.GetPlatformVar_disableMouse();
		SJMiMZJZkwuyvjnzAVoAtfGYlqFC.SdtZiaiGiOBRkWhsDyUVqFZZHOuT();
		mExieEPzUampKVqNAfqEMTjoTkEP = new BfSuWOtYIJOEShfeXemgQlZkXemn(UnityTools.externalTools.GetControllerTemplateTypes(), UnityTools.externalTools.GetControllerTemplateInterfaceTypes());
		mExieEPzUampKVqNAfqEMTjoTkEP.twQjFuHMXyfPWoSHuycASCLyVoAM(OHoFrtAsBjDYugoYVclSaeWlifdaA);
		mExieEPzUampKVqNAfqEMTjoTkEP.twQjFuHMXyfPWoSHuycASCLyVoAM(nsRtkUIduvbzdzdONcpKgCDGweMrA);
		ReInput.ApplicationFocusChangedEvent += YxlgtAhlAmWelGsMVrnDXhQvkODQ;
	}

	public void oFhsHQXkmNwYyEyVythlXdYyuaJh(Action<int, ControllerDataUpdater> P_0, List<InputBehavior> P_1)
	{
		mRBrNjRyufqsdvnsXAgOYDzMhgBp = P_0;
		IWqzYMEJcopMpgBBDUTyDGJcpXFR(P_1);
	}

	public void GpGfvrbUiJpEQCprYpRlROLBPWqD(UpdateLoopType P_0)
	{
		SJMiMZJZkwuyvjnzAVoAtfGYlqFC.JggHihlzjjGVGuslzmppVdouDItL(P_0);
		if (OHoFrtAsBjDYugoYVclSaeWlifdaA.enabled)
		{
			ZvLODOAERMicBraxEvmPYgBDZJwj.UyfgZMOktGaqnEJnEFQlZWQlpsmh(P_0);
		}
		uSrQsbirMlpKCyDCCVIUUAVSSlpM(P_0);
		MMaaWJerGyxPlnjLyfgUHjEEKsCKb(P_0);
		SJMiMZJZkwuyvjnzAVoAtfGYlqFC.JGDtVXAHzmzdeUnWsenokwdpxrorA(P_0, ReInput.currentFrame);
		if (FXWsOYCOLSmYSQuKOCtwrBHvxYFO)
		{
			TPtEskRMPFinQaMmxNTLIziNwgSRA();
		}
	}

	public kBOilrfmQspwwsLlQucgVePHzaAKA hofxhLckICmzdddUmWnGeMyCMRVm(int P_0, string P_1, bool P_2)
	{
		int num = dPqQHnBvkeSkRzJiKdMphfDpwkds.XYovkROfWrjUvrhXaljRAUvHHDbU(P_1, P_2);
		if (num < 0)
		{
			return null;
		}
		if (P_0 == 9999999)
		{
			return IleKFPIiyXDJpfDeagGbBQbAXdpDA[num];
		}
		if (P_0 < 0 || P_0 >= szFjEAeNlWQWOZoZvkhMqQSzhEgd)
		{
			return null;
		}
		return XNnuTExVERvjPlpjlkLPRRqEpuBO[P_0, num];
	}

	public kBOilrfmQspwwsLlQucgVePHzaAKA FfgVHeyzXYOBalgpoIeyNyHpAHaO(int P_0, int P_1, bool P_2)
	{
		int num = dPqQHnBvkeSkRzJiKdMphfDpwkds.CKEcLorxcAHKtKQePRXUkBRZbKYWA(P_1, P_2);
		if (num < 0)
		{
			return null;
		}
		if (P_0 == 9999999)
		{
			return IleKFPIiyXDJpfDeagGbBQbAXdpDA[num];
		}
		return XNnuTExVERvjPlpjlkLPRRqEpuBO[P_0, num];
	}

	public void WBFGWdYfgxHoAguKSomSYudqfBve(UpdateControllerInfoEventArgs P_0)
	{
		if (P_0 != null && P_0.sourceJoystick != null)
		{
			QIwlYaqDyNkycuuFpHTnXgkLkQvM qIwlYaqDyNkycuuFpHTnXgkLkQvM = QIwlYaqDyNkycuuFpHTnXgkLkQvM.Connected;
			int num = FMePnYvlBloZbURmoJROONDxBmHDA(P_0.sourceJoystick.rewiredId, qIwlYaqDyNkycuuFpHTnXgkLkQvM);
			if (num < 0)
			{
				qIwlYaqDyNkycuuFpHTnXgkLkQvM = QIwlYaqDyNkycuuFpHTnXgkLkQvM.Disconnected;
				num = FMePnYvlBloZbURmoJROONDxBmHDA(P_0.sourceJoystick.rewiredId, qIwlYaqDyNkycuuFpHTnXgkLkQvM);
			}
			if (num >= 0)
			{
				((qIwlYaqDyNkycuuFpHTnXgkLkQvM == QIwlYaqDyNkycuuFpHTnXgkLkQvM.Connected) ? EJCFAvjXToHxuQxXFDPFBZPZTRHdA[num] : xsGIfXaWUCexIVjJhaiEUfjZlERB[num]).YnsJhbtxxMkNJRWEyLbLNCgXzish(P_0);
			}
		}
	}

	public bool OAYewipMVNfsJcyulFBwftBEQMOIb(int P_0, QIwlYaqDyNkycuuFpHTnXgkLkQvM P_1)
	{
		return FMePnYvlBloZbURmoJROONDxBmHDA(P_0, P_1) >= 0;
	}

	public int FMePnYvlBloZbURmoJROONDxBmHDA(int P_0, QIwlYaqDyNkycuuFpHTnXgkLkQvM P_1)
	{
		switch (P_1)
		{
		case QIwlYaqDyNkycuuFpHTnXgkLkQvM.Connected:
		{
			int count2 = EJCFAvjXToHxuQxXFDPFBZPZTRHdA.Count;
			for (int j = 0; j < count2; j++)
			{
				if (EJCFAvjXToHxuQxXFDPFBZPZTRHdA[j].id == P_0)
				{
					return j;
				}
			}
			break;
		}
		case QIwlYaqDyNkycuuFpHTnXgkLkQvM.Disconnected:
		{
			int count = xsGIfXaWUCexIVjJhaiEUfjZlERB.Count;
			for (int i = 0; i < count; i++)
			{
				if (xsGIfXaWUCexIVjJhaiEUfjZlERB[i].id == P_0)
				{
					return i;
				}
			}
			break;
		}
		}
		return -1;
	}

	public int ENZSPXFLDAxKTHHQerdFKyqWgcDc(Guid P_0, QIwlYaqDyNkycuuFpHTnXgkLkQvM P_1)
	{
		switch (P_1)
		{
		case QIwlYaqDyNkycuuFpHTnXgkLkQvM.Connected:
		{
			int count2 = EJCFAvjXToHxuQxXFDPFBZPZTRHdA.Count;
			for (int j = 0; j < count2; j++)
			{
				if (EJCFAvjXToHxuQxXFDPFBZPZTRHdA[j].deviceInstanceGuid == P_0)
				{
					return j;
				}
			}
			break;
		}
		case QIwlYaqDyNkycuuFpHTnXgkLkQvM.Disconnected:
		{
			int count = xsGIfXaWUCexIVjJhaiEUfjZlERB.Count;
			for (int i = 0; i < count; i++)
			{
				if (xsGIfXaWUCexIVjJhaiEUfjZlERB[i].deviceInstanceGuid == P_0)
				{
					return i;
				}
			}
			break;
		}
		}
		return -1;
	}

	public bool HgncBDmfRjaORZdDxfmsNvjjpivx(int P_0)
	{
		return RHbykCvRAuPVUcXwKGgGkryOiUnN(P_0) >= 0;
	}

	public int RHbykCvRAuPVUcXwKGgGkryOiUnN(int P_0)
	{
		int count = WaJIozXsYamtRkLzfYkBjnjxfrzl.Count;
		for (int i = 0; i < count; i++)
		{
			if (WaJIozXsYamtRkLzfYkBjnjxfrzl[i].id == P_0)
			{
				return i;
			}
		}
		return -1;
	}

	public int SUkKRDGRaHApOeQHYKBOfyYisUNFb(Guid P_0)
	{
		int count = WaJIozXsYamtRkLzfYkBjnjxfrzl.Count;
		for (int i = 0; i < count; i++)
		{
			if (WaJIozXsYamtRkLzfYkBjnjxfrzl[i].deviceInstanceGuid == P_0)
			{
				return i;
			}
		}
		return -1;
	}

	public void sASGoPhpiZXntIFnupiKVlrNeegWA(BridgedController P_0)
	{
		xdhenbjXFsNdCpAIrAWTaDaaBWtE(P_0);
	}

	public void hNqZsrGJVouRHrpaNkjKKedTLbTi(int P_0)
	{
		int num = FMePnYvlBloZbURmoJROONDxBmHDA(P_0, QIwlYaqDyNkycuuFpHTnXgkLkQvM.Connected);
		eCNPGVPUpbsMJIemEKezujJJdtKv(num);
	}

	public int OJXbLxHHcgVefAVbbgBrlhzSUErv()
	{
		return cOfTxFQBiJUysoydlMEsSOPnEjFA++;
	}

	public IList<InputBehavior> UkGZIfhaCReBibQxxKNQcxuisNEb(int P_0)
	{
		if (!BjRtfcwyrXeCCeKWHcpZMhuRYVRE.ContainsKey(P_0))
		{
			return new List<InputBehavior>();
		}
		return BjRtfcwyrXeCCeKWHcpZMhuRYVRE[P_0].jSAPMexwxCRLqTeunZlegmZKaMDT;
	}

	public InputBehavior LAjEtOLXxuEGCJYVpCdAEIAbisdo(int P_0, string P_1)
	{
		if (P_1 == null || P_1 == string.Empty)
		{
			return null;
		}
		int inputBehaviorId = ReInput.mapping.GetInputBehaviorId(P_1);
		return AUVupqHUEheIGgrZDnacEevMQjxL(P_0, inputBehaviorId);
	}

	public InputBehavior AUVupqHUEheIGgrZDnacEevMQjxL(int P_0, int P_1)
	{
		if (!BjRtfcwyrXeCCeKWHcpZMhuRYVRE.ContainsKey(P_0))
		{
			return null;
		}
		IList<InputBehavior> jSAPMexwxCRLqTeunZlegmZKaMDT = BjRtfcwyrXeCCeKWHcpZMhuRYVRE[P_0].jSAPMexwxCRLqTeunZlegmZKaMDT;
		for (int i = 0; i < jSAPMexwxCRLqTeunZlegmZKaMDT.Count; i++)
		{
			if (jSAPMexwxCRLqTeunZlegmZKaMDT[i].id == P_1)
			{
				return jSAPMexwxCRLqTeunZlegmZKaMDT[i];
			}
		}
		return null;
	}

	public Joystick ypqDskzQOCOIVJAyamsKXzVhMSuK(int P_0, bool P_1 = false)
	{
		int num = FMePnYvlBloZbURmoJROONDxBmHDA(P_0, QIwlYaqDyNkycuuFpHTnXgkLkQvM.Connected);
		if (num >= 0)
		{
			return EJCFAvjXToHxuQxXFDPFBZPZTRHdA[num];
		}
		if (P_1)
		{
			num = FMePnYvlBloZbURmoJROONDxBmHDA(P_0, QIwlYaqDyNkycuuFpHTnXgkLkQvM.Disconnected);
			if (num >= 0)
			{
				return xsGIfXaWUCexIVjJhaiEUfjZlERB[num];
			}
		}
		return null;
	}

	public Joystick dIGXUIKMXbuiJHypfgnqkadxbKtM(Guid P_0, bool P_1 = false)
	{
		int num = ENZSPXFLDAxKTHHQerdFKyqWgcDc(P_0, QIwlYaqDyNkycuuFpHTnXgkLkQvM.Connected);
		if (num >= 0)
		{
			return EJCFAvjXToHxuQxXFDPFBZPZTRHdA[num];
		}
		if (P_1)
		{
			num = ENZSPXFLDAxKTHHQerdFKyqWgcDc(P_0, QIwlYaqDyNkycuuFpHTnXgkLkQvM.Disconnected);
			if (num >= 0)
			{
				return xsGIfXaWUCexIVjJhaiEUfjZlERB[num];
			}
		}
		return null;
	}

	public Joystick[] idLfsOlrRwIOAwtLOCiEiUsbHLTo()
	{
		int count = EJCFAvjXToHxuQxXFDPFBZPZTRHdA.Count;
		if (count == 0)
		{
			return EmptyObjects<Joystick>.array;
		}
		Joystick[] array = new Joystick[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = EJCFAvjXToHxuQxXFDPFBZPZTRHdA[i];
		}
		return array;
	}

	public string[] lbnmTCPcLjbLEFbatcILIzDwyaaFA()
	{
		int count = EJCFAvjXToHxuQxXFDPFBZPZTRHdA.Count;
		if (count == 0)
		{
			return EmptyObjects<string>.array;
		}
		string[] array = new string[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = EJCFAvjXToHxuQxXFDPFBZPZTRHdA[i].name;
		}
		return array;
	}

	public CustomController ANdTvUmEGdGDfhVQXzoelxqNrFDv(int P_0)
	{
		int num = RHbykCvRAuPVUcXwKGgGkryOiUnN(P_0);
		if (num < 0)
		{
			return null;
		}
		return WaJIozXsYamtRkLzfYkBjnjxfrzl[num];
	}

	public CustomController VsbUKexUoYjOZsWMoMqJCeoeIteV(Guid P_0)
	{
		int num = SUkKRDGRaHApOeQHYKBOfyYisUNFb(P_0);
		if (num < 0)
		{
			return null;
		}
		return WaJIozXsYamtRkLzfYkBjnjxfrzl[num];
	}

	public CustomController[] FxxKDGBXOHFOjsKatpBkZYlwIcqK()
	{
		int count = WaJIozXsYamtRkLzfYkBjnjxfrzl.Count;
		if (count == 0)
		{
			return EmptyObjects<CustomController>.array;
		}
		CustomController[] array = new CustomController[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = WaJIozXsYamtRkLzfYkBjnjxfrzl[i];
		}
		return array;
	}

	public string[] yCalGqhSGCRQTFkwwGPZCSKbajIbA()
	{
		int count = WaJIozXsYamtRkLzfYkBjnjxfrzl.Count;
		if (count == 0)
		{
			return EmptyObjects<string>.array;
		}
		string[] array = new string[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = WaJIozXsYamtRkLzfYkBjnjxfrzl[i].name;
		}
		return array;
	}

	public CustomController WWEUAIfEGDhZVmEWMiuGLFXZCtBI(int P_0)
	{
		CustomController_Editor customControllerById = ReInput.UserData.GetCustomControllerById(P_0);
		if (customControllerById == null)
		{
			return null;
		}
		int xeXxvGYvjFirovAVsEbKHSBiTfze = GVQqMztLukeojeTiPawITcgnzxEf;
		CustomController customController = new CustomController(new fwgKdBUKTlRPdLgAbAbZZfczAgRhA
		{
			mqFrAXIklmBKtLvNbaeTzicAlAZS = InputSource.Custom,
			DWKTTUsUiQIxbXclGunmDxAsRdJn = customControllerById.descriptiveName,
			vBFQqEGJmQCmxRemiFRJGbhmcLjX = customControllerById.name,
			huYUOMQqeSPrNGNymoPdGbxrIQqq = customControllerById.axisCount,
			SrHkRIbnCJqDjhkAMIJyjEDwzLCtA = customControllerById.buttonCount,
			xeXxvGYvjFirovAVsEbKHSBiTfze = xeXxvGYvjFirovAVsEbKHSBiTfze,
			drwTiMlBCJtrHnnBOjUoUmEsSCIc = customControllerById.id,
			SLSuArTGSHNkNzkmWeYRkDMdTXBk = customControllerById.typeGuid,
			lZKPTLcOrnPBoHHSBfJsQQAfWYeV = customControllerById.id.ToString(),
			WuHueMVJjpDMMEfXPbHTeCKBXytE = customControllerById.CreateGameHardwareMap()
		});
		KiexqVutEOZvhujudxRuUsrukCNE(customController);
		return customController;
	}

	public bool vfsQiSVhBXImAuqUKHhFsLuzejdB(CustomController P_0)
	{
		if (P_0 == null)
		{
			return false;
		}
		return OEQxVhgGzvkDOQydtvNSTUZDsxtT(P_0);
	}

	public CustomController lVeUTSmCPUqbQipxtaNxiuEOuyyH(int P_0)
	{
		int count = WaJIozXsYamtRkLzfYkBjnjxfrzl.Count;
		for (int i = 0; i < count; i++)
		{
			if (WaJIozXsYamtRkLzfYkBjnjxfrzl[i].sourceControllerId == P_0)
			{
				return WaJIozXsYamtRkLzfYkBjnjxfrzl[i];
			}
		}
		return null;
	}

	public CustomController CEyjqrKQwjuACHSruGveGpxtwOju(string P_0)
	{
		int count = WaJIozXsYamtRkLzfYkBjnjxfrzl.Count;
		for (int i = 0; i < count; i++)
		{
			if (WaJIozXsYamtRkLzfYkBjnjxfrzl[i].tag.Equals(P_0, StringComparison.OrdinalIgnoreCase))
			{
				return WaJIozXsYamtRkLzfYkBjnjxfrzl[i];
			}
		}
		return null;
	}

	[IteratorStateMachine(typeof(JkcUaJSrjlOrNvKovejJHJOScGvbb))]
	public IEnumerable<CustomController> sifPkQqpzlkQXgCHDCjVKbyzrFGwA(int P_0)
	{
		return new JkcUaJSrjlOrNvKovejJHJOScGvbb(-2)
		{
			YnmItmIbMhIdxNGiMiihiSVhJCXLA = this,
			SjLVBwimhDBpiHLjcMnNQpSZNGJw = P_0
		};
	}

	[IteratorStateMachine(typeof(XZgpaUYpyQyRnniYLLQVyzyyEqGI))]
	public IEnumerable<CustomController> txQQUttZxiiPPsPtUvpInElVchaw(string P_0)
	{
		return new XZgpaUYpyQyRnniYLLQVyzyyEqGI(-2)
		{
			qLfchNdPDyAJEbYegzlEQxjTOqpRB = this,
			yrRFZEKYXTTimJBaoVcwwBWDoFaM = P_0
		};
	}

	public Controller QaXmawDzDviOFGGVAiudAlfjSkMM(ControllerType P_0, int P_1, bool P_2 = false)
	{
		return P_0 switch
		{
			ControllerType.Joystick => ypqDskzQOCOIVJAyamsKXzVhMSuK(P_1, P_2), 
			ControllerType.Keyboard => OHoFrtAsBjDYugoYVclSaeWlifdaA, 
			ControllerType.Mouse => nsRtkUIduvbzdzdONcpKgCDGweMrA, 
			ControllerType.Custom => ANdTvUmEGdGDfhVQXzoelxqNrFDv(P_1), 
			_ => throw new NotImplementedException(), 
		};
	}

	public Controller UpNCLkybyrfyzcgOPVjvhyzXROTtA(ControllerIdentifier P_0, bool P_1 = false)
	{
		if (P_0.deviceInstanceGuid != Guid.Empty)
		{
			return vBejDsAdZnJlzbwLCQPjiJmjswwI(P_0.deviceInstanceGuid);
		}
		if (P_0.controllerId >= 0)
		{
			return QaXmawDzDviOFGGVAiudAlfjSkMM(P_0.controllerType, P_0.controllerId, P_1);
		}
		return null;
	}

	public Controller vBejDsAdZnJlzbwLCQPjiJmjswwI(Guid P_0, bool P_1 = false)
	{
		if (P_0 == Guid.Empty)
		{
			return null;
		}
		if (OHoFrtAsBjDYugoYVclSaeWlifdaA.deviceInstanceGuid == P_0)
		{
			return OHoFrtAsBjDYugoYVclSaeWlifdaA;
		}
		if (nsRtkUIduvbzdzdONcpKgCDGweMrA.deviceInstanceGuid == P_0)
		{
			return nsRtkUIduvbzdzdONcpKgCDGweMrA;
		}
		Controller result;
		if ((result = dIGXUIKMXbuiJHypfgnqkadxbKtM(P_0, P_1)) != null)
		{
			return result;
		}
		if ((result = VsbUKexUoYjOZsWMoMqJCeoeIteV(P_0)) != null)
		{
			return result;
		}
		return null;
	}

	public Controller[] jskyfxOoiQdVrOHqoqaLZAUxDpKF(ControllerType P_0)
	{
		return P_0 switch
		{
			ControllerType.Joystick => idLfsOlrRwIOAwtLOCiEiUsbHLTo(), 
			ControllerType.Keyboard => new Controller[1] { OHoFrtAsBjDYugoYVclSaeWlifdaA }, 
			ControllerType.Mouse => new Controller[1] { nsRtkUIduvbzdzdONcpKgCDGweMrA }, 
			ControllerType.Custom => FxxKDGBXOHFOjsKatpBkZYlwIcqK(), 
			_ => throw new NotImplementedException(), 
		};
	}

	public string[] rsywZcHxTMzhaMfBitMrospUvwJF(ControllerType P_0)
	{
		return P_0 switch
		{
			ControllerType.Joystick => lbnmTCPcLjbLEFbatcILIzDwyaaFA(), 
			ControllerType.Keyboard => new string[1] { OHoFrtAsBjDYugoYVclSaeWlifdaA.name }, 
			ControllerType.Mouse => new string[1] { nsRtkUIduvbzdzdONcpKgCDGweMrA.name }, 
			ControllerType.Custom => yCalGqhSGCRQTFkwwGPZCSKbajIbA(), 
			_ => throw new NotImplementedException(), 
		};
	}

	public void nvJlMegypofMmlmFYEsDwGUnCemY(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2)
	{
		if (!HcvCFkoIMMBtsbTAqcthMvdnyZIGA)
		{
			HcvCFkoIMMBtsbTAqcthMvdnyZIGA = true;
		}
		jaZqVMJDOCHNDKHlwnOBdtLuAoBnA(P_0)?.tPHDsXZajMrnvPPwRkxODRerbYhI(P_1, P_2, InputActionEventType.Update, null);
	}

	public void PHXuIjmHwdTPbdhoeqfeVmTeizJG(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2, int P_3)
	{
		if (!HcvCFkoIMMBtsbTAqcthMvdnyZIGA)
		{
			HcvCFkoIMMBtsbTAqcthMvdnyZIGA = true;
		}
		jaZqVMJDOCHNDKHlwnOBdtLuAoBnA(P_0)?.VvtznjEucSiQYaSyphkmDjBERHJg(P_1, P_2, InputActionEventType.Update, P_3, null);
	}

	public void NWlzoNoZlOARGOQnhanMPuPodqrb(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2, string P_3)
	{
		if (!HcvCFkoIMMBtsbTAqcthMvdnyZIGA)
		{
			HcvCFkoIMMBtsbTAqcthMvdnyZIGA = true;
		}
		int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_3);
		if (num >= 0)
		{
			PHXuIjmHwdTPbdhoeqfeVmTeizJG(P_0, P_1, P_2, num);
		}
	}

	public void NnaeZEblCldBQiyWCRhsFaLrGwU(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2, InputActionEventType P_3, object[] P_4)
	{
		if (!HcvCFkoIMMBtsbTAqcthMvdnyZIGA)
		{
			HcvCFkoIMMBtsbTAqcthMvdnyZIGA = true;
		}
		jaZqVMJDOCHNDKHlwnOBdtLuAoBnA(P_0)?.tPHDsXZajMrnvPPwRkxODRerbYhI(P_1, P_2, P_3, P_4);
	}

	public void IKejPHmuKzHkCOQkYgfnOGyUgnJT(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2, InputActionEventType P_3, int P_4, object[] P_5)
	{
		if (!HcvCFkoIMMBtsbTAqcthMvdnyZIGA)
		{
			HcvCFkoIMMBtsbTAqcthMvdnyZIGA = true;
		}
		jaZqVMJDOCHNDKHlwnOBdtLuAoBnA(P_0)?.VvtznjEucSiQYaSyphkmDjBERHJg(P_1, P_2, P_3, P_4, P_5);
	}

	public void LUgdJIBKeQXwHgZbBktqcgBdrrAv(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2, InputActionEventType P_3, string P_4, object[] P_5)
	{
		if (!HcvCFkoIMMBtsbTAqcthMvdnyZIGA)
		{
			HcvCFkoIMMBtsbTAqcthMvdnyZIGA = true;
		}
		int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_4);
		if (num >= 0)
		{
			IKejPHmuKzHkCOQkYgfnOGyUgnJT(P_0, P_1, P_2, P_3, num, P_5);
		}
	}

	public void FKgiYbNkJHIQITZiFQdKUgsHcBhm(int P_0, Action<InputActionEventData> P_1)
	{
		jaZqVMJDOCHNDKHlwnOBdtLuAoBnA(P_0)?.oEHjHFaHgVgZEiiPvrBYpoEJRcgM(P_1);
	}

	public void LVMWrgHmqKvKmNKZQcFyajfLWXQw(int P_0, Action<InputActionEventData> P_1, int P_2)
	{
		jaZqVMJDOCHNDKHlwnOBdtLuAoBnA(P_0)?.aAssgIcQTwezjASHDINKEhcluwYm(P_1, P_2);
	}

	public void aYcMZSFngUFLTwJNBmpYRitONZUf(int P_0, Action<InputActionEventData> P_1, string P_2)
	{
		int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_2);
		if (num >= 0)
		{
			LVMWrgHmqKvKmNKZQcFyajfLWXQw(P_0, P_1, num);
		}
	}

	public void rDCAMuHlglAoxLFUgqBGJvyKdzJJ(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2)
	{
		jaZqVMJDOCHNDKHlwnOBdtLuAoBnA(P_0)?.mpsatPBiZqhdudDDVpvZnTUipvyV(P_1, P_2);
	}

	public void iUjCLpCkpwAMkxFUDUoNeajknXMy(int P_0, Action<InputActionEventData> P_1, InputActionEventType P_2)
	{
		jaZqVMJDOCHNDKHlwnOBdtLuAoBnA(P_0)?.xjMhVdFmEdfITamEREHhHeVInjHyA(P_1, P_2);
	}

	public void OuADhETpQnfqbOCDmUNwkNkuJGPP(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2, int P_3)
	{
		jaZqVMJDOCHNDKHlwnOBdtLuAoBnA(P_0)?.rbAbnRXMZlhoYgOXMyQXMmxDjYQU(P_1, P_2, P_3);
	}

	public void vhAKeFhklzLAiatuQHOkXuCrEiDw(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2, string P_3)
	{
		int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_3);
		if (num >= 0)
		{
			OuADhETpQnfqbOCDmUNwkNkuJGPP(P_0, P_1, P_2, num);
		}
	}

	public void yUbxkEEdrvcYADVfImSCiWfsAaDDA(int P_0, Action<InputActionEventData> P_1, InputActionEventType P_2, int P_3)
	{
		jaZqVMJDOCHNDKHlwnOBdtLuAoBnA(P_0)?.PjrnfjaNJUJGnxbZKkcahMKRIcPs(P_1, P_2, P_3);
	}

	public void vOwSbfmwJayepVdnXhpQhrXrTsdm(int P_0, Action<InputActionEventData> P_1, InputActionEventType P_2, string P_3)
	{
		int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_3);
		if (num >= 0)
		{
			yUbxkEEdrvcYADVfImSCiWfsAaDDA(P_0, P_1, P_2, num);
		}
	}

	public void cRMczdFKdAtFYIphgLBMqeufjlfsA(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2, InputActionEventType P_3)
	{
		jaZqVMJDOCHNDKHlwnOBdtLuAoBnA(P_0)?.HDEpMWesyAgcRghVGTzGaEsbqbAf(P_1, P_2, P_3);
	}

	public void mlVvArjbyBthhbBWEPpwDyRkhLHy(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2, InputActionEventType P_3, int P_4)
	{
		jaZqVMJDOCHNDKHlwnOBdtLuAoBnA(P_0)?.hJodpAkBUzfITDVxbrWhIwqPPrfvB(P_1, P_2, P_3, P_4);
	}

	public void OGcuRewpBSepFcjcUiXxxSJTBgVgA(int P_0, Action<InputActionEventData> P_1, UpdateLoopType P_2, InputActionEventType P_3, string P_4)
	{
		int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_4);
		if (num >= 0)
		{
			mlVvArjbyBthhbBWEPpwDyRkhLHy(P_0, P_1, P_2, P_3, num);
		}
	}

	public void ARQGcvGEsYwHGQbeFaZtLOjBluWW(int P_0)
	{
		jaZqVMJDOCHNDKHlwnOBdtLuAoBnA(P_0)?.SWXmZDJMjvvJfIqiUJcHmCVSNfXG();
	}

	public bool dOWihLBVqbhasnWxUedOOgLvhhVq(int P_0)
	{
		if (P_0 == 9999999)
		{
			for (int i = 0; i < IleKFPIiyXDJpfDeagGbBQbAXdpDA.Length; i++)
			{
				if (IleKFPIiyXDJpfDeagGbBQbAXdpDA[i].BRgJAcEyxDdRJtEmfVewtjEoYNqt())
				{
					return true;
				}
			}
			return false;
		}
		if (P_0 < 0 || P_0 >= szFjEAeNlWQWOZoZvkhMqQSzhEgd)
		{
			return false;
		}
		int num = dPqQHnBvkeSkRzJiKdMphfDpwkds.mlHXwbZsAMqBDxgLMneFMJrnnMfX;
		for (int j = 0; j < num; j++)
		{
			if (XNnuTExVERvjPlpjlkLPRRqEpuBO[P_0, j].BRgJAcEyxDdRJtEmfVewtjEoYNqt())
			{
				return true;
			}
		}
		return false;
	}

	public bool dvuVTLKANvgVhLQedePpWnZXhoEH(int P_0)
	{
		if (P_0 == 9999999)
		{
			for (int i = 0; i < IleKFPIiyXDJpfDeagGbBQbAXdpDA.Length; i++)
			{
				if (IleKFPIiyXDJpfDeagGbBQbAXdpDA[i].ulNzhMMaXtAXOxcBFAnWyCDHeqoN())
				{
					return true;
				}
			}
			return false;
		}
		if (P_0 < 0 || P_0 >= szFjEAeNlWQWOZoZvkhMqQSzhEgd)
		{
			return false;
		}
		int num = dPqQHnBvkeSkRzJiKdMphfDpwkds.mlHXwbZsAMqBDxgLMneFMJrnnMfX;
		for (int j = 0; j < num; j++)
		{
			if (XNnuTExVERvjPlpjlkLPRRqEpuBO[P_0, j].ulNzhMMaXtAXOxcBFAnWyCDHeqoN())
			{
				return true;
			}
		}
		return false;
	}

	public bool mcYMPIxhRtIeYNfHJLoaJqOkHYyh(int P_0)
	{
		if (P_0 == 9999999)
		{
			for (int i = 0; i < IleKFPIiyXDJpfDeagGbBQbAXdpDA.Length; i++)
			{
				if (IleKFPIiyXDJpfDeagGbBQbAXdpDA[i].aOLFHKiGReYtLuVqzyyHLHbfKQYab())
				{
					return true;
				}
			}
			return false;
		}
		if (P_0 < 0 || P_0 >= szFjEAeNlWQWOZoZvkhMqQSzhEgd)
		{
			return false;
		}
		int num = dPqQHnBvkeSkRzJiKdMphfDpwkds.mlHXwbZsAMqBDxgLMneFMJrnnMfX;
		for (int j = 0; j < num; j++)
		{
			if (XNnuTExVERvjPlpjlkLPRRqEpuBO[P_0, j].aOLFHKiGReYtLuVqzyyHLHbfKQYab())
			{
				return true;
			}
		}
		return false;
	}

	public bool axUBQajEqZhmhvhoGZWqojMdEKLG(int P_0)
	{
		if (P_0 == 9999999)
		{
			for (int i = 0; i < IleKFPIiyXDJpfDeagGbBQbAXdpDA.Length; i++)
			{
				if (IleKFPIiyXDJpfDeagGbBQbAXdpDA[i].ooKSDMuVHpVciXXBbIhumspcSpRM())
				{
					return true;
				}
			}
			return false;
		}
		if (P_0 < 0 || P_0 >= szFjEAeNlWQWOZoZvkhMqQSzhEgd)
		{
			return false;
		}
		int num = dPqQHnBvkeSkRzJiKdMphfDpwkds.mlHXwbZsAMqBDxgLMneFMJrnnMfX;
		for (int j = 0; j < num; j++)
		{
			if (XNnuTExVERvjPlpjlkLPRRqEpuBO[P_0, j].ooKSDMuVHpVciXXBbIhumspcSpRM())
			{
				return true;
			}
		}
		return false;
	}

	public bool wQLEuWyfJafvNqoWDFzeSidJbbMW(int P_0)
	{
		if (P_0 == 9999999)
		{
			for (int i = 0; i < IleKFPIiyXDJpfDeagGbBQbAXdpDA.Length; i++)
			{
				if (IleKFPIiyXDJpfDeagGbBQbAXdpDA[i].VRiBjNoimKLqMJcOigBnTGHHrHub())
				{
					return true;
				}
			}
			return false;
		}
		if (P_0 < 0 || P_0 >= szFjEAeNlWQWOZoZvkhMqQSzhEgd)
		{
			return false;
		}
		int num = dPqQHnBvkeSkRzJiKdMphfDpwkds.mlHXwbZsAMqBDxgLMneFMJrnnMfX;
		for (int j = 0; j < num; j++)
		{
			if (XNnuTExVERvjPlpjlkLPRRqEpuBO[P_0, j].VRiBjNoimKLqMJcOigBnTGHHrHub())
			{
				return true;
			}
		}
		return false;
	}

	public bool ndmftXNSUwiLKVuQmztrMrsknIAj(int P_0)
	{
		if (P_0 == 9999999)
		{
			for (int i = 0; i < IleKFPIiyXDJpfDeagGbBQbAXdpDA.Length; i++)
			{
				if (IleKFPIiyXDJpfDeagGbBQbAXdpDA[i].VYSikZzuXqPelkspEaGEPYtFZADJ())
				{
					return true;
				}
			}
			return false;
		}
		if (P_0 < 0 || P_0 >= szFjEAeNlWQWOZoZvkhMqQSzhEgd)
		{
			return false;
		}
		int num = dPqQHnBvkeSkRzJiKdMphfDpwkds.mlHXwbZsAMqBDxgLMneFMJrnnMfX;
		for (int j = 0; j < num; j++)
		{
			if (XNnuTExVERvjPlpjlkLPRRqEpuBO[P_0, j].VYSikZzuXqPelkspEaGEPYtFZADJ())
			{
				return true;
			}
		}
		return false;
	}

	public bool vOGOUBImYulJQCmtkfSujuqdRRfGA(int P_0)
	{
		if (P_0 == 9999999)
		{
			for (int i = 0; i < IleKFPIiyXDJpfDeagGbBQbAXdpDA.Length; i++)
			{
				if (IleKFPIiyXDJpfDeagGbBQbAXdpDA[i].KOICBttxgehAkePueGwrcVfAlxWCb())
				{
					return true;
				}
			}
			return false;
		}
		if (P_0 < 0 || P_0 >= szFjEAeNlWQWOZoZvkhMqQSzhEgd)
		{
			return false;
		}
		int num = dPqQHnBvkeSkRzJiKdMphfDpwkds.mlHXwbZsAMqBDxgLMneFMJrnnMfX;
		for (int j = 0; j < num; j++)
		{
			if (XNnuTExVERvjPlpjlkLPRRqEpuBO[P_0, j].KOICBttxgehAkePueGwrcVfAlxWCb())
			{
				return true;
			}
		}
		return false;
	}

	public bool SfZAmeeXiCsbCpACbjdIjcditpuv(int P_0)
	{
		if (P_0 == 9999999)
		{
			for (int i = 0; i < IleKFPIiyXDJpfDeagGbBQbAXdpDA.Length; i++)
			{
				if (IleKFPIiyXDJpfDeagGbBQbAXdpDA[i].tnXlVtxyaxIfLqFVXAJLDtiGTaUf())
				{
					return true;
				}
			}
			return false;
		}
		if (P_0 < 0 || P_0 >= szFjEAeNlWQWOZoZvkhMqQSzhEgd)
		{
			return false;
		}
		int num = dPqQHnBvkeSkRzJiKdMphfDpwkds.mlHXwbZsAMqBDxgLMneFMJrnnMfX;
		for (int j = 0; j < num; j++)
		{
			if (XNnuTExVERvjPlpjlkLPRRqEpuBO[P_0, j].tnXlVtxyaxIfLqFVXAJLDtiGTaUf())
			{
				return true;
			}
		}
		return false;
	}

	public bool TLjbPoFCLLhzNTmYRGBcjxgqMOhkA()
	{
		if (!ecfzDXPDKbBOwEeWUXRDcshClEsnA(nsRtkUIduvbzdzdONcpKgCDGweMrA) && !EAGGgnRXmhCAwiWbeHoBOpBSMrAwA(EJCFAvjXToHxuQxXFDPFBZPZTRHdA) && !ecfzDXPDKbBOwEeWUXRDcshClEsnA(OHoFrtAsBjDYugoYVclSaeWlifdaA))
		{
			return EAGGgnRXmhCAwiWbeHoBOpBSMrAwA(WaJIozXsYamtRkLzfYkBjnjxfrzl);
		}
		return true;
	}

	public bool sVKvRLKlFDzkBvsqLuyhswblQmUy(ControllerType P_0)
	{
		return P_0 switch
		{
			ControllerType.Joystick => EAGGgnRXmhCAwiWbeHoBOpBSMrAwA(EJCFAvjXToHxuQxXFDPFBZPZTRHdA), 
			ControllerType.Keyboard => ecfzDXPDKbBOwEeWUXRDcshClEsnA(OHoFrtAsBjDYugoYVclSaeWlifdaA), 
			ControllerType.Mouse => ecfzDXPDKbBOwEeWUXRDcshClEsnA(nsRtkUIduvbzdzdONcpKgCDGweMrA), 
			ControllerType.Custom => EAGGgnRXmhCAwiWbeHoBOpBSMrAwA(WaJIozXsYamtRkLzfYkBjnjxfrzl), 
			_ => throw new NotImplementedException(), 
		};
	}

	public bool wXiZvBpmRjyecgotDrIryUpYwQOS()
	{
		if (!cyrxDwdNQwGtYeKKExqUUTmLPoKFA(nsRtkUIduvbzdzdONcpKgCDGweMrA) && !wpXCCzEeOjWDCVZAqydVgbtoLyzK(EJCFAvjXToHxuQxXFDPFBZPZTRHdA) && !cyrxDwdNQwGtYeKKExqUUTmLPoKFA(OHoFrtAsBjDYugoYVclSaeWlifdaA))
		{
			return wpXCCzEeOjWDCVZAqydVgbtoLyzK(WaJIozXsYamtRkLzfYkBjnjxfrzl);
		}
		return true;
	}

	public bool vyPESacQRtUmMAbFZfKGXdzaquYr(ControllerType P_0)
	{
		return P_0 switch
		{
			ControllerType.Joystick => wpXCCzEeOjWDCVZAqydVgbtoLyzK(EJCFAvjXToHxuQxXFDPFBZPZTRHdA), 
			ControllerType.Keyboard => cyrxDwdNQwGtYeKKExqUUTmLPoKFA(OHoFrtAsBjDYugoYVclSaeWlifdaA), 
			ControllerType.Mouse => cyrxDwdNQwGtYeKKExqUUTmLPoKFA(nsRtkUIduvbzdzdONcpKgCDGweMrA), 
			ControllerType.Custom => wpXCCzEeOjWDCVZAqydVgbtoLyzK(WaJIozXsYamtRkLzfYkBjnjxfrzl), 
			_ => throw new NotImplementedException(), 
		};
	}

	public bool LiEOVesscDjMRDFNZBWJmysCuUXdA()
	{
		if (!iYVBExKjDvrJZakZFooBlQXmYuQp(nsRtkUIduvbzdzdONcpKgCDGweMrA) && !XcDftBVkKpOujYpYoakUYmJMnBxC(EJCFAvjXToHxuQxXFDPFBZPZTRHdA) && !iYVBExKjDvrJZakZFooBlQXmYuQp(OHoFrtAsBjDYugoYVclSaeWlifdaA))
		{
			return XcDftBVkKpOujYpYoakUYmJMnBxC(WaJIozXsYamtRkLzfYkBjnjxfrzl);
		}
		return true;
	}

	public bool pDbFNUqNXfoiiwKvhYVNdsICejxd(ControllerType P_0)
	{
		return P_0 switch
		{
			ControllerType.Joystick => XcDftBVkKpOujYpYoakUYmJMnBxC(EJCFAvjXToHxuQxXFDPFBZPZTRHdA), 
			ControllerType.Keyboard => iYVBExKjDvrJZakZFooBlQXmYuQp(OHoFrtAsBjDYugoYVclSaeWlifdaA), 
			ControllerType.Mouse => iYVBExKjDvrJZakZFooBlQXmYuQp(nsRtkUIduvbzdzdONcpKgCDGweMrA), 
			ControllerType.Custom => XcDftBVkKpOujYpYoakUYmJMnBxC(WaJIozXsYamtRkLzfYkBjnjxfrzl), 
			_ => throw new NotImplementedException(), 
		};
	}

	public bool QDgFRpBowsehhtyTlZrdUcsuBpfdb()
	{
		if (!MmCzkMWcEqCusGMiENWbcoibGNWv(nsRtkUIduvbzdzdONcpKgCDGweMrA) && !qxdCruBRaMKLbHPvKRiJDchOzOVfb(EJCFAvjXToHxuQxXFDPFBZPZTRHdA) && !MmCzkMWcEqCusGMiENWbcoibGNWv(OHoFrtAsBjDYugoYVclSaeWlifdaA))
		{
			return qxdCruBRaMKLbHPvKRiJDchOzOVfb(WaJIozXsYamtRkLzfYkBjnjxfrzl);
		}
		return true;
	}

	public bool NmCDtfhQJzNrgVEGzUvuPHKSwRvP(ControllerType P_0)
	{
		return P_0 switch
		{
			ControllerType.Joystick => qxdCruBRaMKLbHPvKRiJDchOzOVfb(EJCFAvjXToHxuQxXFDPFBZPZTRHdA), 
			ControllerType.Keyboard => MmCzkMWcEqCusGMiENWbcoibGNWv(OHoFrtAsBjDYugoYVclSaeWlifdaA), 
			ControllerType.Mouse => MmCzkMWcEqCusGMiENWbcoibGNWv(nsRtkUIduvbzdzdONcpKgCDGweMrA), 
			ControllerType.Custom => qxdCruBRaMKLbHPvKRiJDchOzOVfb(WaJIozXsYamtRkLzfYkBjnjxfrzl), 
			_ => throw new NotImplementedException(), 
		};
	}

	public bool tLYadFJuQsHesxdaaUeLWVUAwbKpA()
	{
		if (!dBwHBcNTCuUmjGMMImYChfozBUjB(nsRtkUIduvbzdzdONcpKgCDGweMrA) && !LVSBplIPDtHRDANzUJGJTgGtpPvlA(EJCFAvjXToHxuQxXFDPFBZPZTRHdA) && !dBwHBcNTCuUmjGMMImYChfozBUjB(OHoFrtAsBjDYugoYVclSaeWlifdaA))
		{
			return LVSBplIPDtHRDANzUJGJTgGtpPvlA(WaJIozXsYamtRkLzfYkBjnjxfrzl);
		}
		return true;
	}

	public bool eAqZhGncDoHktPpvlezDGaYYEEMq(ControllerType P_0)
	{
		return P_0 switch
		{
			ControllerType.Joystick => LVSBplIPDtHRDANzUJGJTgGtpPvlA(EJCFAvjXToHxuQxXFDPFBZPZTRHdA), 
			ControllerType.Keyboard => dBwHBcNTCuUmjGMMImYChfozBUjB(OHoFrtAsBjDYugoYVclSaeWlifdaA), 
			ControllerType.Mouse => dBwHBcNTCuUmjGMMImYChfozBUjB(nsRtkUIduvbzdzdONcpKgCDGweMrA), 
			ControllerType.Custom => LVSBplIPDtHRDANzUJGJTgGtpPvlA(WaJIozXsYamtRkLzfYkBjnjxfrzl), 
			_ => throw new NotImplementedException(), 
		};
	}

	private bool EAGGgnRXmhCAwiWbeHoBOpBSMrAwA<_0001>(IList<_0001> P_0) where _0001 : Controller
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

	private bool ecfzDXPDKbBOwEeWUXRDcshClEsnA(Controller P_0)
	{
		return P_0?.GetAnyButton() ?? false;
	}

	private bool wpXCCzEeOjWDCVZAqydVgbtoLyzK<_0001>(IList<_0001> P_0) where _0001 : Controller
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

	private bool cyrxDwdNQwGtYeKKExqUUTmLPoKFA(Controller P_0)
	{
		return P_0?.GetAnyButtonDown() ?? false;
	}

	private bool XcDftBVkKpOujYpYoakUYmJMnBxC<_0001>(IList<_0001> P_0) where _0001 : Controller
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

	private bool iYVBExKjDvrJZakZFooBlQXmYuQp(Controller P_0)
	{
		return P_0?.GetAnyButtonUp() ?? false;
	}

	private bool qxdCruBRaMKLbHPvKRiJDchOzOVfb<_0001>(IList<_0001> P_0) where _0001 : Controller
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

	private bool MmCzkMWcEqCusGMiENWbcoibGNWv(Controller P_0)
	{
		return P_0?.GetAnyButtonChanged() ?? false;
	}

	private bool LVSBplIPDtHRDANzUJGJTgGtpPvlA<_0001>(IList<_0001> P_0) where _0001 : Controller
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

	private bool dBwHBcNTCuUmjGMMImYChfozBUjB(Controller P_0)
	{
		return P_0?.GetAnyButtonPrev() ?? false;
	}

	public Controller OzkDYWaprGZkxpnnegeOlsUIfTGV()
	{
		Controller lastController = null;
		double lastTime = 0.0;
		InputTools.CompareLastActiveController(nsRtkUIduvbzdzdONcpKgCDGweMrA, ref lastController, ref lastTime);
		InputTools.CompareLastActiveController(OHoFrtAsBjDYugoYVclSaeWlifdaA, ref lastController, ref lastTime);
		IList<Joystick> eJCFAvjXToHxuQxXFDPFBZPZTRHdA = EJCFAvjXToHxuQxXFDPFBZPZTRHdA;
		for (int i = 0; i < tMmOaRdQZinbpzIJqOsQpkLeJiKf; i++)
		{
			InputTools.CompareLastActiveController(eJCFAvjXToHxuQxXFDPFBZPZTRHdA[i], ref lastController, ref lastTime);
		}
		IList<CustomController> waJIozXsYamtRkLzfYkBjnjxfrzl = WaJIozXsYamtRkLzfYkBjnjxfrzl;
		for (int j = 0; j < OaArgfysJnEWrVVscGbEedXuKtYDA; j++)
		{
			InputTools.CompareLastActiveController(waJIozXsYamtRkLzfYkBjnjxfrzl[j], ref lastController, ref lastTime);
		}
		if (lastController == null)
		{
			lastController = OHoFrtAsBjDYugoYVclSaeWlifdaA;
		}
		return lastController;
	}

	public Controller kVMKJXGrpERpuLPyWEHnJICCwiPnA(ControllerType P_0)
	{
		Controller lastController = null;
		double lastTime = 0.0;
		switch (P_0)
		{
		case ControllerType.Joystick:
		{
			int count = EJCFAvjXToHxuQxXFDPFBZPZTRHdA.Count;
			for (int j = 0; j < count; j++)
			{
				InputTools.CompareLastActiveController(EJCFAvjXToHxuQxXFDPFBZPZTRHdA[j], ref lastController, ref lastTime);
			}
			break;
		}
		case ControllerType.Keyboard:
			return TVvLxBfEgOqnloHdRFcagvmpmnZT;
		case ControllerType.Mouse:
			return RVEYLKIoOydxyctXJKWZflgnfQyi;
		case ControllerType.Custom:
		{
			int count = WaJIozXsYamtRkLzfYkBjnjxfrzl.Count;
			for (int i = 0; i < count; i++)
			{
				InputTools.CompareLastActiveController(WaJIozXsYamtRkLzfYkBjnjxfrzl[i], ref lastController, ref lastTime);
			}
			break;
		}
		default:
			throw new NotImplementedException();
		}
		return lastController;
	}

	public _0001 OzkDYWaprGZkxpnnegeOlsUIfTGV<_0001>() where _0001 : Controller
	{
		Type typeFromHandle = typeof(_0001);
		if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Joystick)))
		{
			return kVMKJXGrpERpuLPyWEHnJICCwiPnA(ControllerType.Joystick) as _0001;
		}
		if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Keyboard)))
		{
			return kVMKJXGrpERpuLPyWEHnJICCwiPnA(ControllerType.Keyboard) as _0001;
		}
		if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(CustomController)))
		{
			return kVMKJXGrpERpuLPyWEHnJICCwiPnA(ControllerType.Custom) as _0001;
		}
		if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Mouse)))
		{
			return kVMKJXGrpERpuLPyWEHnJICCwiPnA(ControllerType.Mouse) as _0001;
		}
		throw new NotImplementedException();
	}

	public ControllerType uskUJcFSYKuEnjKcOhCfeTmggWLVA()
	{
		return OzkDYWaprGZkxpnnegeOlsUIfTGV()?.type ?? ControllerType.Keyboard;
	}

	public void gTuUmxdySLmMPcmDBQyoKkYuaScT(ActiveControllerChangedDelegate P_0)
	{
		if (P_0 != null)
		{
			FXWsOYCOLSmYSQuKOCtwrBHvxYFO = true;
			VUJGoplhMfmICFGAnBidkUoaFRwx.NAvhDGfHnWVhNLPgRosAtHwQqfC(P_0);
		}
	}

	public void CaFYlnfgOBGxABBsFsaafYjjWmun(ActiveControllerChangedDelegate P_0, ControllerType P_1)
	{
		if (P_0 != null)
		{
			FXWsOYCOLSmYSQuKOCtwrBHvxYFO = true;
			VUJGoplhMfmICFGAnBidkUoaFRwx.IcDKRxqYPngWWgGpRYYSrCBgRsie(P_0, P_1);
		}
	}

	public void XjVDXGzMkNqxprATLFFHesSkfjlg(ActiveControllerChangedDelegate P_0)
	{
		if (P_0 != null)
		{
			VUJGoplhMfmICFGAnBidkUoaFRwx.oUIjIgyURvRnckhgfHpqVjItHvWs(P_0);
		}
	}

	public void IfjxMKpFVkAFazHMwlJvNCxlfiZU(ActiveControllerChangedDelegate P_0, ControllerType P_1)
	{
		if (P_0 != null)
		{
			VUJGoplhMfmICFGAnBidkUoaFRwx.jTXWQgLAddXoRxmrdjQVSUtoELSt(P_0, P_1);
		}
	}

	public void eEmPpgRThzKuPBYtJSKOZzzLpUpe()
	{
		VUJGoplhMfmICFGAnBidkUoaFRwx.eFuaTPXiLSfnOAFHGwTTbCPyccXCb();
	}

	public void SdpLFiihptMZfHxZkgvTbdRFrLMb(int P_0, PlayerActiveControllerChangedDelegate P_1)
	{
		if (P_1 == null)
		{
			return;
		}
		if (P_0 == 9999999)
		{
			ylLFDsMXVVVkCVuYwhTWhbBitGVU.NAvhDGfHnWVhNLPgRosAtHwQqfC(P_1);
		}
		else
		{
			if ((uint)P_0 >= (uint)szFjEAeNlWQWOZoZvkhMqQSzhEgd)
			{
				return;
			}
			wbRAShiNEXppEJXmExAHzYiezVrh[P_0].NAvhDGfHnWVhNLPgRosAtHwQqfC(P_1);
		}
		FXWsOYCOLSmYSQuKOCtwrBHvxYFO = true;
	}

	public void xxYxXFLnjpWLAKLMCqCzPDwcTCDG(int P_0, PlayerActiveControllerChangedDelegate P_1, ControllerType P_2)
	{
		if (P_1 == null)
		{
			return;
		}
		if (P_0 == 9999999)
		{
			ylLFDsMXVVVkCVuYwhTWhbBitGVU.IcDKRxqYPngWWgGpRYYSrCBgRsie(P_1, P_2);
		}
		else
		{
			if ((uint)P_0 >= (uint)szFjEAeNlWQWOZoZvkhMqQSzhEgd)
			{
				return;
			}
			wbRAShiNEXppEJXmExAHzYiezVrh[P_0].IcDKRxqYPngWWgGpRYYSrCBgRsie(P_1, P_2);
		}
		FXWsOYCOLSmYSQuKOCtwrBHvxYFO = true;
	}

	public void DHseRPCIsZLFIZMymJMAlhDdgliT(int P_0, PlayerActiveControllerChangedDelegate P_1)
	{
		if (P_1 != null)
		{
			if (P_0 == 9999999)
			{
				ylLFDsMXVVVkCVuYwhTWhbBitGVU.oUIjIgyURvRnckhgfHpqVjItHvWs(P_1);
			}
			else if ((uint)P_0 < (uint)szFjEAeNlWQWOZoZvkhMqQSzhEgd)
			{
				wbRAShiNEXppEJXmExAHzYiezVrh[P_0].oUIjIgyURvRnckhgfHpqVjItHvWs(P_1);
			}
		}
	}

	public void PunOxnffJqmmGYdePHvHNeDGfcMCA(int P_0, PlayerActiveControllerChangedDelegate P_1, ControllerType P_2)
	{
		if (P_1 != null)
		{
			if (P_0 == 9999999)
			{
				ylLFDsMXVVVkCVuYwhTWhbBitGVU.jTXWQgLAddXoRxmrdjQVSUtoELSt(P_1, P_2);
			}
			else if ((uint)P_0 < (uint)szFjEAeNlWQWOZoZvkhMqQSzhEgd)
			{
				wbRAShiNEXppEJXmExAHzYiezVrh[P_0].jTXWQgLAddXoRxmrdjQVSUtoELSt(P_1, P_2);
			}
		}
	}

	public void YFkjSeFvBxUzGvlWAZFkKErUZjVl(int P_0)
	{
		if (P_0 == 9999999)
		{
			ylLFDsMXVVVkCVuYwhTWhbBitGVU.eFuaTPXiLSfnOAFHGwTTbCPyccXCb();
		}
		else if ((uint)P_0 < (uint)szFjEAeNlWQWOZoZvkhMqQSzhEgd)
		{
			wbRAShiNEXppEJXmExAHzYiezVrh[P_0].eFuaTPXiLSfnOAFHGwTTbCPyccXCb();
		}
	}

	private void TPtEskRMPFinQaMmxNTLIziNwgSRA()
	{
		if (VUJGoplhMfmICFGAnBidkUoaFRwx.VSWvwLunqPSvsLcHCdBaSBvBCKcGA > 0)
		{
			VUJGoplhMfmICFGAnBidkUoaFRwx.NfsXENfpiQRdYETYngBYEJslxWdJ(-1, OzkDYWaprGZkxpnnegeOlsUIfTGV(), kVMKJXGrpERpuLPyWEHnJICCwiPnA(ControllerType.Joystick), kVMKJXGrpERpuLPyWEHnJICCwiPnA(ControllerType.Custom));
		}
		if (ylLFDsMXVVVkCVuYwhTWhbBitGVU.VSWvwLunqPSvsLcHCdBaSBvBCKcGA > 0)
		{
			Player.ControllerHelper controllers = uZSHzuPrMOGzQpgnxdAvgFnQUdKD.KSOsWtiVPgllQavrnvGgcPSdahjn().controllers;
			ylLFDsMXVVVkCVuYwhTWhbBitGVU.NfsXENfpiQRdYETYngBYEJslxWdJ(9999999, controllers.GetLastActiveController(), controllers.GetLastActiveController(ControllerType.Joystick), controllers.GetLastActiveController(ControllerType.Custom));
		}
		for (int i = 0; i < szFjEAeNlWQWOZoZvkhMqQSzhEgd; i++)
		{
			if (wbRAShiNEXppEJXmExAHzYiezVrh[i].VSWvwLunqPSvsLcHCdBaSBvBCKcGA != 0)
			{
				Player.ControllerHelper controllers2 = uZSHzuPrMOGzQpgnxdAvgFnQUdKD.RNneRXekceTIrvqRONeevKWpudcSA[i].controllers;
				wbRAShiNEXppEJXmExAHzYiezVrh[i].NfsXENfpiQRdYETYngBYEJslxWdJ(i, controllers2.GetLastActiveController(), controllers2.GetLastActiveController(ControllerType.Joystick), controllers2.GetLastActiveController(ControllerType.Custom));
			}
		}
	}

	public void rYKbJGDlkVucJBbAnkMBYeAFjEQmA(ThrottleCalibrationMode P_0)
	{
		for (int i = 0; i < EJCFAvjXToHxuQxXFDPFBZPZTRHdA.Count; i++)
		{
			if (EJCFAvjXToHxuQxXFDPFBZPZTRHdA[i] != null)
			{
				ScNklfdszuMBowILBAOnAipeaXliA(EJCFAvjXToHxuQxXFDPFBZPZTRHdA[i], P_0);
			}
		}
		for (int j = 0; j < xsGIfXaWUCexIVjJhaiEUfjZlERB.Count; j++)
		{
			if (xsGIfXaWUCexIVjJhaiEUfjZlERB[j] != null)
			{
				ScNklfdszuMBowILBAOnAipeaXliA(xsGIfXaWUCexIVjJhaiEUfjZlERB[j], P_0);
			}
		}
		for (int k = 0; k < OaArgfysJnEWrVVscGbEedXuKtYDA; k++)
		{
			if (WaJIozXsYamtRkLzfYkBjnjxfrzl[k] != null)
			{
				ScNklfdszuMBowILBAOnAipeaXliA(WaJIozXsYamtRkLzfYkBjnjxfrzl[k], P_0);
			}
		}
		ScNklfdszuMBowILBAOnAipeaXliA(nsRtkUIduvbzdzdONcpKgCDGweMrA, P_0);
	}

	private void ScNklfdszuMBowILBAOnAipeaXliA(ControllerWithAxes P_0, ThrottleCalibrationMode P_1)
	{
		IList<Controller.Axis> axes = P_0.Axes;
		for (int i = 0; i < P_0.axisCount; i++)
		{
			if (axes[i].dyBDSFNRWhEclaVHduPpTilGwQgN._specialAxisType == SpecialAxisType.Throttle)
			{
				P_0.calibrationMap.Axes[i].calibrationMode = EnumConverter.ToAlternateAxisCalibrationType(P_1);
			}
		}
	}

	public IList<_0001> YmQipxvVmDsNqUDHtzzfqGORHoZl<_0001>() where _0001 : IControllerTemplate
	{
		return mExieEPzUampKVqNAfqEMTjoTkEP.VJzsOuBgNaRhPOlQAJASDbsknCBn<_0001>();
	}

	private void IWqzYMEJcopMpgBBDUTyDGJcpXFR(List<InputBehavior> P_0)
	{
		dPqQHnBvkeSkRzJiKdMphfDpwkds = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF;
		uZSHzuPrMOGzQpgnxdAvgFnQUdKD = ReInput.KTVqyytqGISutLJQbfhSaKddBlfv;
		EJCFAvjXToHxuQxXFDPFBZPZTRHdA = new List<Joystick>();
		xsGIfXaWUCexIVjJhaiEUfjZlERB = new List<Joystick>();
		WaJIozXsYamtRkLzfYkBjnjxfrzl = new List<CustomController>();
		BcGHmmMcpMXMZJLkbFRclNEtxSzU = dPqQHnBvkeSkRzJiKdMphfDpwkds.mlHXwbZsAMqBDxgLMneFMJrnnMfX;
		szFjEAeNlWQWOZoZvkhMqQSzhEgd = uZSHzuPrMOGzQpgnxdAvgFnQUdKD.VeldubbvOvGpfoCqfSOeOBOPVmbpA;
		NUQDPccRpdVUVcpmuuHHIDMlZnwf = KidYnzhQazmcfgsCXrLfgttGAzJe;
		NIEiRyFcMCAmnSWBVgKgaXhFUlTV = 0;
		BjRtfcwyrXeCCeKWHcpZMhuRYVRE = new ADictionary<int, PMmWeSKIIthraMbmySYLmpDneVdh>();
		BjRtfcwyrXeCCeKWHcpZMhuRYVRE.Add(ReInput.players.GetSystemPlayer().id, new PMmWeSKIIthraMbmySYLmpDneVdh(P_0));
		IList<Player> players = ReInput.players.Players;
		for (int i = 0; i < players.Count; i++)
		{
			BjRtfcwyrXeCCeKWHcpZMhuRYVRE.Add(players[i].id, new PMmWeSKIIthraMbmySYLmpDneVdh(P_0));
		}
		vsCLkOFAmDrSrsKodGFlfXpkAveBA = new ReadOnlyCollection<Joystick>(EJCFAvjXToHxuQxXFDPFBZPZTRHdA);
		lZohoZfREIwhjTfuQsOdHmreXtok = new ReadOnlyCollection<CustomController>(WaJIozXsYamtRkLzfYkBjnjxfrzl);
		kBOilrfmQspwwsLlQucgVePHzaAKA.KhGHkdewxHeaLKbHTCnkkXSJloYP(DMfdqwHStwxCULmnZnMMbiIggxxJA);
		rUhUYPJOLiCyZtlCTemjqjIZJfrU = new kBOilrfmQspwwsLlQucgVePHzaAKA[(szFjEAeNlWQWOZoZvkhMqQSzhEgd + 1) * BcGHmmMcpMXMZJLkbFRclNEtxSzU];
		int num = 0;
		IleKFPIiyXDJpfDeagGbBQbAXdpDA = new kBOilrfmQspwwsLlQucgVePHzaAKA[BcGHmmMcpMXMZJLkbFRclNEtxSzU];
		for (int j = 0; j < BcGHmmMcpMXMZJLkbFRclNEtxSzU; j++)
		{
			InputAction inputAction = dPqQHnBvkeSkRzJiKdMphfDpwkds.tOrbWEggKrehwrEPReOQzyywMhaE(j);
			InputBehavior inputBehavior = BjRtfcwyrXeCCeKWHcpZMhuRYVRE[9999999].qmwkmQRlOiYPAqLQMIfGwHwmAlYIA(inputAction.behaviorId);
			kBOilrfmQspwwsLlQucgVePHzaAKA kBOilrfmQspwwsLlQucgVePHzaAKA2 = new kBOilrfmQspwwsLlQucgVePHzaAKA(9999999, inputAction, inputBehavior, DMfdqwHStwxCULmnZnMMbiIggxxJA);
			IleKFPIiyXDJpfDeagGbBQbAXdpDA[j] = kBOilrfmQspwwsLlQucgVePHzaAKA2;
			rUhUYPJOLiCyZtlCTemjqjIZJfrU[num] = kBOilrfmQspwwsLlQucgVePHzaAKA2;
			num++;
		}
		XNnuTExVERvjPlpjlkLPRRqEpuBO = new kBOilrfmQspwwsLlQucgVePHzaAKA[szFjEAeNlWQWOZoZvkhMqQSzhEgd, BcGHmmMcpMXMZJLkbFRclNEtxSzU];
		for (int k = 0; k < szFjEAeNlWQWOZoZvkhMqQSzhEgd; k++)
		{
			for (int l = 0; l < BcGHmmMcpMXMZJLkbFRclNEtxSzU; l++)
			{
				InputAction inputAction2 = dPqQHnBvkeSkRzJiKdMphfDpwkds.tOrbWEggKrehwrEPReOQzyywMhaE(l);
				InputBehavior inputBehavior2 = BjRtfcwyrXeCCeKWHcpZMhuRYVRE[players[k].id].qmwkmQRlOiYPAqLQMIfGwHwmAlYIA(inputAction2.behaviorId);
				kBOilrfmQspwwsLlQucgVePHzaAKA kBOilrfmQspwwsLlQucgVePHzaAKA3 = new kBOilrfmQspwwsLlQucgVePHzaAKA(k, inputAction2, inputBehavior2, DMfdqwHStwxCULmnZnMMbiIggxxJA);
				XNnuTExVERvjPlpjlkLPRRqEpuBO[k, l] = kBOilrfmQspwwsLlQucgVePHzaAKA3;
				rUhUYPJOLiCyZtlCTemjqjIZJfrU[num] = kBOilrfmQspwwsLlQucgVePHzaAKA3;
				num++;
			}
		}
		IList<Player_Editor> list = ReInput.UserData.JIZWSkNLBmbxmgqTylFTFDyIHEkLB;
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
				CustomController customController = WWEUAIfEGDhZVmEWMiuGLFXZCtBI(startingCustomControllers[n].sourceId);
				if (customController != null)
				{
					customController.tag = startingCustomControllers[n].tag;
					int num2 = ((m == 0) ? 9999999 : (m - 1));
					uZSHzuPrMOGzQpgnxdAvgFnQUdKD.LcqJOYavcMfniFdJGbmbfGcBCdPHA(num2)?.controllers.kEAbMXgUFCzMuOyhJNeDWOPVrEoGA(customController, false);
				}
			}
		}
		wouoRPfYwKqZonCFTQkUdmSvVYxE = new IgUVpafhmNYDnMTcltiGEUbuIaMm();
		ugMqSNXIkwbyvDgnkncjpPoxddXwA = new IgUVpafhmNYDnMTcltiGEUbuIaMm[szFjEAeNlWQWOZoZvkhMqQSzhEgd];
		for (int num3 = 0; num3 < szFjEAeNlWQWOZoZvkhMqQSzhEgd; num3++)
		{
			ugMqSNXIkwbyvDgnkncjpPoxddXwA[num3] = new IgUVpafhmNYDnMTcltiGEUbuIaMm();
		}
		VUJGoplhMfmICFGAnBidkUoaFRwx = new global::mXvUKnAftAGBresozjoMzBMkfSTeb<ActiveControllerChangedDelegate>();
		ylLFDsMXVVVkCVuYwhTWhbBitGVU = new global::mXvUKnAftAGBresozjoMzBMkfSTeb<PlayerActiveControllerChangedDelegate>();
		wbRAShiNEXppEJXmExAHzYiezVrh = new global::mXvUKnAftAGBresozjoMzBMkfSTeb<PlayerActiveControllerChangedDelegate>[uZSHzuPrMOGzQpgnxdAvgFnQUdKD.VeldubbvOvGpfoCqfSOeOBOPVmbpA];
		ArrayTools.Populate(wbRAShiNEXppEJXmExAHzYiezVrh);
	}

	private void uSrQsbirMlpKCyDCCVIUUAVSSlpM(UpdateLoopType P_0)
	{
		int count = EJCFAvjXToHxuQxXFDPFBZPZTRHdA.Count;
		for (int i = 0; i < count; i++)
		{
			Joystick joystick = EJCFAvjXToHxuQxXFDPFBZPZTRHdA[i];
			if (joystick.enabled)
			{
				mRBrNjRyufqsdvnsXAgOYDzMhgBp(joystick.BFfYMETdxToenAnLoAiBVAfvIxMw, joystick.yZwGORAVRJPjNCmxxWIIoQgNomuqA);
				joystick.PTpLZPTdIGBCXbVzlMCHCqylApVQA(P_0);
			}
		}
		if (OHoFrtAsBjDYugoYVclSaeWlifdaA.enabled)
		{
			OHoFrtAsBjDYugoYVclSaeWlifdaA.PTpLZPTdIGBCXbVzlMCHCqylApVQA(P_0);
		}
		else if (EACdwryTdNpQNmXOUQUmQMuoSWoo)
		{
			OHoFrtAsBjDYugoYVclSaeWlifdaA.WNoRRIZbwaCOlzIxscrCaTKXKxmI(P_0);
		}
		if (nsRtkUIduvbzdzdONcpKgCDGweMrA.enabled)
		{
			nsRtkUIduvbzdzdONcpKgCDGweMrA.PTpLZPTdIGBCXbVzlMCHCqylApVQA(P_0);
		}
		int count2 = WaJIozXsYamtRkLzfYkBjnjxfrzl.Count;
		for (int j = 0; j < count2; j++)
		{
			CustomController customController = WaJIozXsYamtRkLzfYkBjnjxfrzl[j];
			if (customController.enabled)
			{
				customController.mDaReUmdzWmtGgOUJOzXLErqrwKK();
				customController.PTpLZPTdIGBCXbVzlMCHCqylApVQA(P_0);
			}
		}
	}

	private void MMaaWJerGyxPlnjLyfgUHjEEKsCKb(UpdateLoopType P_0)
	{
		kBOilrfmQspwwsLlQucgVePHzaAKA.qlMNFyvSAUHOHsHEBAOEJxVwXHzk(P_0);
		Player[] array = uZSHzuPrMOGzQpgnxdAvgFnQUdKD.UTKiNgXnQuIZVkkphLwgVlBnhTkd;
		int num = array.Length;
		bool enabled = OHoFrtAsBjDYugoYVclSaeWlifdaA.enabled;
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
						ZvLODOAERMicBraxEvmPYgBDZJwj.RtoBJtCVPvOYYHQzegWYzxmGYuufA(maps[j]);
					}
				}
			}
		}
		bool enabled2 = nsRtkUIduvbzdzdONcpKgCDGweMrA.enabled;
		for (int k = 0; k < num; k++)
		{
			Player.ControllerHelper controllers = array[k].controllers;
			controllers.paEuTLolRmWivldZSfhDYGfdyUrP(NUQDPccRpdVUVcpmuuHHIDMlZnwf);
			if (enabled || EACdwryTdNpQNmXOUQUmQMuoSWoo)
			{
				controllers.KfulNBNpspCMeYlkHDTukWbvxvIf(OHoFrtAsBjDYugoYVclSaeWlifdaA, ZvLODOAERMicBraxEvmPYgBDZJwj, NUQDPccRpdVUVcpmuuHHIDMlZnwf);
			}
			if (enabled2)
			{
				controllers.OswiFrhrsUTajCLxSjQkuHVIeaPg(nsRtkUIduvbzdzdONcpKgCDGweMrA, NUQDPccRpdVUVcpmuuHHIDMlZnwf);
			}
			controllers.YWgkmvLKQVcbyGVwGnLXrRWLJmVxA(NUQDPccRpdVUVcpmuuHHIDMlZnwf);
		}
		for (int l = 0; l < rUhUYPJOLiCyZtlCTemjqjIZJfrU.Length; l++)
		{
			if (rUhUYPJOLiCyZtlCTemjqjIZJfrU[l].EwUlKdxJNDjYFOxUmtThCxBTiwFk != kBOilrfmQspwwsLlQucgVePHzaAKA.HEPRhHqiAQGPgyrCYjFjLzdlFAoj.Disabled)
			{
				rUhUYPJOLiCyZtlCTemjqjIZJfrU[l].YzWzyUUothWsLjLzMqEXcFkrUFTK();
			}
		}
		kBOilrfmQspwwsLlQucgVePHzaAKA.cKZKGgcABDCiGUOUwQuMGLoCKSFA();
		if (!HcvCFkoIMMBtsbTAqcthMvdnyZIGA)
		{
			return;
		}
		if (wouoRPfYwKqZonCFTQkUdmSvVYxE.rbDgJJKPZnOfiISIIFHbhZQEItGmD > 0)
		{
			for (int m = 0; m < BcGHmmMcpMXMZJLkbFRclNEtxSzU; m++)
			{
				kBOilrfmQspwwsLlQucgVePHzaAKA kBOilrfmQspwwsLlQucgVePHzaAKA2 = IleKFPIiyXDJpfDeagGbBQbAXdpDA[m];
				if (kBOilrfmQspwwsLlQucgVePHzaAKA2.EwUlKdxJNDjYFOxUmtThCxBTiwFk != kBOilrfmQspwwsLlQucgVePHzaAKA.HEPRhHqiAQGPgyrCYjFjLzdlFAoj.Disabled)
				{
					wouoRPfYwKqZonCFTQkUdmSvVYxE.exZDkojriUFuDCBsAcnVVOPGfVvM(kBOilrfmQspwwsLlQucgVePHzaAKA2, P_0);
				}
			}
		}
		for (int n = 0; n < szFjEAeNlWQWOZoZvkhMqQSzhEgd; n++)
		{
			IgUVpafhmNYDnMTcltiGEUbuIaMm igUVpafhmNYDnMTcltiGEUbuIaMm = ugMqSNXIkwbyvDgnkncjpPoxddXwA[n];
			if (igUVpafhmNYDnMTcltiGEUbuIaMm.rbDgJJKPZnOfiISIIFHbhZQEItGmD == 0)
			{
				continue;
			}
			for (int num2 = 0; num2 < BcGHmmMcpMXMZJLkbFRclNEtxSzU; num2++)
			{
				kBOilrfmQspwwsLlQucgVePHzaAKA kBOilrfmQspwwsLlQucgVePHzaAKA3 = XNnuTExVERvjPlpjlkLPRRqEpuBO[n, num2];
				if (kBOilrfmQspwwsLlQucgVePHzaAKA3.EwUlKdxJNDjYFOxUmtThCxBTiwFk != kBOilrfmQspwwsLlQucgVePHzaAKA.HEPRhHqiAQGPgyrCYjFjLzdlFAoj.Disabled)
				{
					igUVpafhmNYDnMTcltiGEUbuIaMm.exZDkojriUFuDCBsAcnVVOPGfVvM(kBOilrfmQspwwsLlQucgVePHzaAKA3, P_0);
				}
			}
		}
	}

	private void KidYnzhQazmcfgsCXrLfgttGAzJe(bool P_0, int P_1, int P_2)
	{
		int num = dPqQHnBvkeSkRzJiKdMphfDpwkds.CKEcLorxcAHKtKQePRXUkBRZbKYWA(P_2);
		if (num >= 0)
		{
			if (P_1 == 9999999)
			{
				IleKFPIiyXDJpfDeagGbBQbAXdpDA[num].hdUKmfKyVIfXWfiGGqfikFUUWQUq(P_0);
			}
			else
			{
				XNnuTExVERvjPlpjlkLPRRqEpuBO[P_1, num].hdUKmfKyVIfXWfiGGqfikFUUWQUq(P_0);
			}
		}
	}

	private void xdhenbjXFsNdCpAIrAWTaDaaBWtE(BridgedController P_0)
	{
		int num = FMePnYvlBloZbURmoJROONDxBmHDA(P_0.sourceJoystick.rewiredId, QIwlYaqDyNkycuuFpHTnXgkLkQvM.Connected);
		if (num >= 0)
		{
			Logger.LogError("Controller was already in connected list!");
			return;
		}
		num = FMePnYvlBloZbURmoJROONDxBmHDA(P_0.sourceJoystick.rewiredId, QIwlYaqDyNkycuuFpHTnXgkLkQvM.Disconnected);
		Joystick joystick;
		if (num >= 0)
		{
			joystick = xsGIfXaWUCexIVjJhaiEUfjZlERB[num];
			xsGIfXaWUCexIVjJhaiEUfjZlERB.RemoveAt(num);
			joystick.ZCHOihHaYWvbmwhgSXfIQcsWlmCF(P_0);
			joystick.isConnected = true;
		}
		else
		{
			joystick = new Joystick(P_0);
		}
		EJCFAvjXToHxuQxXFDPFBZPZTRHdA.Add(joystick);
		irVFTHDbZhyARZSjxyajpjzdWWZl.Add(joystick);
		EJCFAvjXToHxuQxXFDPFBZPZTRHdA.Sort(Joystick.uEfjfrrziwMGjoyYJmOwuuEXFfJt);
		mExieEPzUampKVqNAfqEMTjoTkEP.twQjFuHMXyfPWoSHuycASCLyVoAM(joystick);
	}

	private void eCNPGVPUpbsMJIemEKezujJJdtKv(int P_0)
	{
		if (P_0 < 0)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (P_0 >= EJCFAvjXToHxuQxXFDPFBZPZTRHdA.Count)
		{
			Logger.LogError("Device was not in connected list! Cannot remove!");
			return;
		}
		Joystick joystick = EJCFAvjXToHxuQxXFDPFBZPZTRHdA[P_0];
		joystick.isConnected = false;
		if (zCgRpqvmjCwzcLEOnWUFUXoZiyM != null)
		{
			zCgRpqvmjCwzcLEOnWUFUXoZiyM(new ControllerStatusChangedEventArgs(joystick.name, joystick.id, joystick.type));
		}
		if (LiOxSUgiRaGswMIKrGqSPfjNCYOd != null)
		{
			LiOxSUgiRaGswMIKrGqSPfjNCYOd(joystick.type, joystick.id);
		}
		EJCFAvjXToHxuQxXFDPFBZPZTRHdA.RemoveAt(P_0);
		xsGIfXaWUCexIVjJhaiEUfjZlERB.Add(joystick);
		irVFTHDbZhyARZSjxyajpjzdWWZl.Remove(joystick);
		mExieEPzUampKVqNAfqEMTjoTkEP.YPbLVCMIvIHWRQTVZLgUrWLBevvq(joystick);
		joystick.bglRweWaaTFfEiIQjwyzpBARhNXC();
	}

	private void xyjIHhNTsToANAWKCcuRKvYSveLc()
	{
		for (int num = EJCFAvjXToHxuQxXFDPFBZPZTRHdA.Count - 1; num >= 0; num--)
		{
			eCNPGVPUpbsMJIemEKezujJJdtKv(num);
		}
	}

	private bool KiexqVutEOZvhujudxRuUsrukCNE(CustomController P_0)
	{
		if (P_0 == null)
		{
			return false;
		}
		for (int i = 0; i < WaJIozXsYamtRkLzfYkBjnjxfrzl.Count; i++)
		{
			if (WaJIozXsYamtRkLzfYkBjnjxfrzl[i] == P_0)
			{
				return true;
			}
		}
		WaJIozXsYamtRkLzfYkBjnjxfrzl.Add(P_0);
		irVFTHDbZhyARZSjxyajpjzdWWZl.Add(P_0);
		mExieEPzUampKVqNAfqEMTjoTkEP.twQjFuHMXyfPWoSHuycASCLyVoAM(P_0);
		return true;
	}

	private bool OEQxVhgGzvkDOQydtvNSTUZDsxtT(CustomController P_0)
	{
		if (P_0 == null)
		{
			return false;
		}
		mExieEPzUampKVqNAfqEMTjoTkEP.YPbLVCMIvIHWRQTVZLgUrWLBevvq(P_0);
		irVFTHDbZhyARZSjxyajpjzdWWZl.Remove(P_0);
		return WaJIozXsYamtRkLzfYkBjnjxfrzl.Remove(P_0);
	}

	private IgUVpafhmNYDnMTcltiGEUbuIaMm jaZqVMJDOCHNDKHlwnOBdtLuAoBnA(int P_0)
	{
		if (P_0 == 9999999)
		{
			return wouoRPfYwKqZonCFTQkUdmSvVYxE;
		}
		if (P_0 < 0 || P_0 >= ReInput.KTVqyytqGISutLJQbfhSaKddBlfv.VeldubbvOvGpfoCqfSOeOBOPVmbpA)
		{
			return null;
		}
		return ugMqSNXIkwbyvDgnkncjpPoxddXwA[P_0];
	}

	private void IyKZVGGdqREdMKjAmpGsVQRPsVop(bool P_0)
	{
		if (!P_0)
		{
			ZvLODOAERMicBraxEvmPYgBDZJwj.zubwDlyUfCTHwFahVNTMFyvWCfhi();
		}
	}

	private void YxlgtAhlAmWelGsMVrnDXhQvkODQ(bool P_0)
	{
		OHoFrtAsBjDYugoYVclSaeWlifdaA.rlUNcxrpXspwUOOiDFKtvkpmClqcA(P_0);
		nsRtkUIduvbzdzdONcpKgCDGweMrA.rlUNcxrpXspwUOOiDFKtvkpmClqcA(P_0);
		for (int i = 0; i < EJCFAvjXToHxuQxXFDPFBZPZTRHdA.Count; i++)
		{
			EJCFAvjXToHxuQxXFDPFBZPZTRHdA[i].rlUNcxrpXspwUOOiDFKtvkpmClqcA(P_0);
		}
		for (int j = 0; j < WaJIozXsYamtRkLzfYkBjnjxfrzl.Count; j++)
		{
			WaJIozXsYamtRkLzfYkBjnjxfrzl[j].rlUNcxrpXspwUOOiDFKtvkpmClqcA(P_0);
		}
	}

	public void Dispose()
	{
		ovVakZcxyIRVWlzHQWGYMIeZCpsOA(true);
		GC.SuppressFinalize(this);
	}

	void IDisposable.Dispose()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Dispose
		this.Dispose();
	}

	protected void KKMNVjylcIqgoewCrUlnHBjVyXSK()
	{
		try
		{
			ovVakZcxyIRVWlzHQWGYMIeZCpsOA(false);
		}
		finally
		{
			base.Finalize();
		}
	}

	private void ovVakZcxyIRVWlzHQWGYMIeZCpsOA(bool P_0)
	{
		if (fTIymgMtzYCMvyTanTOcuYUrPtWs)
		{
			return;
		}
		if (P_0)
		{
			if (ljfcKoeBmRxbHsmfiYxvdianzTUQA is IDisposable)
			{
				(ljfcKoeBmRxbHsmfiYxvdianzTUQA as IDisposable).Dispose();
			}
			if (hUpaWWWbfVwVtttgzQQiNPaISJMP is IDisposable)
			{
				(hUpaWWWbfVwVtttgzQQiNPaISJMP as IDisposable).Dispose();
			}
		}
		fTIymgMtzYCMvyTanTOcuYUrPtWs = true;
	}
}
