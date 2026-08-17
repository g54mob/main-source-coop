using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Rewired.Utils.Classes.Utility
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class ThreadHelper : IDisposable
	{
		private const uint OlriTOIiieNaOHKWGhoklIMRZTTE = 750u;

		private readonly Stopwatch QlRxqQimbQbqpyJTEYxRmcZkfKaCA;

		private Thread EbRHMbdtsvtIjMjOSMykEVysYQmMA;

		private ManualResetEvent VTgAwFEtNUjvWpidWFocuhxIajyP;

		private ManualResetEvent htymIYwbLKNoeJcBlpnDMPNTkfiE;

		private AutoResetEvent hGktpADuBlnCOKLCLdmgdmUYknevA;

		private bool mNDciKKMxhCHhYavVQKyLDBvRnBGA;

		private bool kZZDZwHdGSsdyJROrWMtKZIGwCOQ;

		private int lzGgsslikWZsQOarIqTEmXLZwHyi;

		private bool ncjPrbQulEjiCssjHHchFeuOnVpq;

		private int zDtrQYQOnvRShvWnorLXdPmKaTpdA;

		private long GeECNMkEIieMtdzVMrLwdMaizJToB;

		private bool HdlfBlWESYakoDkxabVbcBoKczcT;

		private int liVzExsOiSCHNhLxVFDdKuNvJGipA;

		private long XhHdaGkgmbkZUTEqHPNfehdXGIMP;

		private uint FOrwgpSCEfPtfdinooKAmKYwlBsP;

		private readonly object MBaCTcelMiCcAUdNLBodAYlhlgXDB;

		private Queue<Action> DpoYcapUJZeucxsIeBQgGHYsgFAS;

		private Queue<Action> QEzfWVuZPKrSulEqXVvIpNQHrngi;

		private bool uTitnoCZPseieZfglLunlsiEexCl;

		private Action JLwggAurcOsmgrSNKskBucfYBTmIA;

		[CompilerGenerated]
		private Action SHnYNIalklTrGaNSMbUJsmWCEnReA;

		[CompilerGenerated]
		private Action OpRgRqqVLlLUDDoAlXWhWnAIGLjP;

		private bool mjtbktfPEnuUVvKGsQRntoxgCjAO;

		public bool isRunning => kZZDZwHdGSsdyJROrWMtKZIGwCOQ;

		public bool isStopped
		{
			get
			{
				if (!kZZDZwHdGSsdyJROrWMtKZIGwCOQ)
				{
					if (EbRHMbdtsvtIjMjOSMykEVysYQmMA == null)
					{
						return true;
					}
					return !EbRHMbdtsvtIjMjOSMykEVysYQmMA.IsAlive;
				}
				return false;
			}
		}

		public bool useHighPrecitionTimer
		{
			get
			{
				if (!ncjPrbQulEjiCssjHHchFeuOnVpq)
				{
					return (long)zDtrQYQOnvRShvWnorLXdPmKaTpdA >= 750L;
				}
				return true;
			}
			set
			{
				if (value != ncjPrbQulEjiCssjHHchFeuOnVpq)
				{
					ncjPrbQulEjiCssjHHchFeuOnVpq = value;
					mPQFzJiqvNTWElbAmZQIEsuHbbPq();
				}
			}
		}

		public bool useFixedTimeStep => HdlfBlWESYakoDkxabVbcBoKczcT;

		public int fixedTimeStepFPS
		{
			get
			{
				return zDtrQYQOnvRShvWnorLXdPmKaTpdA;
			}
			set
			{
				zDtrQYQOnvRShvWnorLXdPmKaTpdA = ((value > 0) ? value : 0);
				mPQFzJiqvNTWElbAmZQIEsuHbbPq();
			}
		}

		public int timeoutMS
		{
			get
			{
				return liVzExsOiSCHNhLxVFDdKuNvJGipA;
			}
			set
			{
				liVzExsOiSCHNhLxVFDdKuNvJGipA = ((value > 0) ? value : 0);
				mPQFzJiqvNTWElbAmZQIEsuHbbPq();
			}
		}

		public uint tick => FOrwgpSCEfPtfdinooKAmKYwlBsP;

		public event Action ThreadUpdateEvent
		{
			add
			{
				JLwggAurcOsmgrSNKskBucfYBTmIA = (Action)Delegate.Combine(JLwggAurcOsmgrSNKskBucfYBTmIA, value);
			}
			remove
			{
				JLwggAurcOsmgrSNKskBucfYBTmIA = (Action)Delegate.Remove(JLwggAurcOsmgrSNKskBucfYBTmIA, value);
			}
		}

		private event Action _ThreadStartedEvent
		{
			[CompilerGenerated]
			add
			{
				Action action = SHnYNIalklTrGaNSMbUJsmWCEnReA;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, b);
					action = Interlocked.CompareExchange(ref SHnYNIalklTrGaNSMbUJsmWCEnReA, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = SHnYNIalklTrGaNSMbUJsmWCEnReA;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value3);
					action = Interlocked.CompareExchange(ref SHnYNIalklTrGaNSMbUJsmWCEnReA, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public event Action ThreadStartedEvent
		{
			add
			{
				_ThreadStartedEvent += value;
			}
			remove
			{
				_ThreadStartedEvent -= value;
			}
		}

		private event Action _ThreadPreStopEvent
		{
			[CompilerGenerated]
			add
			{
				Action action = OpRgRqqVLlLUDDoAlXWhWnAIGLjP;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, b);
					action = Interlocked.CompareExchange(ref OpRgRqqVLlLUDDoAlXWhWnAIGLjP, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = OpRgRqqVLlLUDDoAlXWhWnAIGLjP;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value3);
					action = Interlocked.CompareExchange(ref OpRgRqqVLlLUDDoAlXWhWnAIGLjP, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public event Action ThreadPreStopEvent
		{
			add
			{
				_ThreadPreStopEvent += value;
			}
			remove
			{
				_ThreadPreStopEvent -= value;
			}
		}

		public static ThreadHelper Create(bool fixedTimeStep = false, int fixedTimeStepFPS = 100, bool useHighPrecisionTimer = false, int timeoutMS = 0)
		{
			if (fixedTimeStep)
			{
				return new ThreadHelper(fixedTimeStepFPS, useHighPrecisionTimer, timeoutMS);
			}
			return new ThreadHelper(timeoutMS);
		}

		public static ThreadHelper CreateFixedTimeStep(int timeStepFPS, int timeoutMS = 0)
		{
			return CreateFixedTimeStep(timeStepFPS, useHighPrecisionTimer: false, timeoutMS);
		}

		public static ThreadHelper CreateFixedTimeStep(int timeStepFPS, bool useHighPrecisionTimer = false, int timeoutMS = 0)
		{
			return new ThreadHelper(timeStepFPS, useHighPrecisionTimer, timeoutMS);
		}

		private ThreadHelper()
			: this(0)
		{
		}

		private ThreadHelper(int P_0)
			: this(0, false, P_0)
		{
		}

		private ThreadHelper(int P_0, bool P_1, int P_2)
		{
			QlRxqQimbQbqpyJTEYxRmcZkfKaCA = Stopwatch.Global;
			if (P_0 < 0)
			{
				P_0 = 0;
			}
			if (P_2 < 0)
			{
				P_2 = 0;
			}
			liVzExsOiSCHNhLxVFDdKuNvJGipA = P_2;
			zDtrQYQOnvRShvWnorLXdPmKaTpdA = P_0;
			ncjPrbQulEjiCssjHHchFeuOnVpq = P_1;
			mPQFzJiqvNTWElbAmZQIEsuHbbPq();
			VTgAwFEtNUjvWpidWFocuhxIajyP = new ManualResetEvent(initialState: false);
			htymIYwbLKNoeJcBlpnDMPNTkfiE = new ManualResetEvent(initialState: false);
			hGktpADuBlnCOKLCLdmgdmUYknevA = new AutoResetEvent(initialState: false);
			MBaCTcelMiCcAUdNLBodAYlhlgXDB = new object();
			DpoYcapUJZeucxsIeBQgGHYsgFAS = new Queue<Action>();
			QEzfWVuZPKrSulEqXVvIpNQHrngi = new Queue<Action>();
		}

		public bool Start(bool wait)
		{
			if (kZZDZwHdGSsdyJROrWMtKZIGwCOQ)
			{
				return false;
			}
			try
			{
				VTgAwFEtNUjvWpidWFocuhxIajyP.Reset();
				hGktpADuBlnCOKLCLdmgdmUYknevA.Reset();
				EbRHMbdtsvtIjMjOSMykEVysYQmMA = new Thread(HDmjbvtEGyrvyjIuQWEOwjZelfzx);
				EbRHMbdtsvtIjMjOSMykEVysYQmMA.Start();
				if (wait)
				{
					VTgAwFEtNUjvWpidWFocuhxIajyP.WaitOne();
				}
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public void Stop(bool wait)
		{
			if (EbRHMbdtsvtIjMjOSMykEVysYQmMA != null && kZZDZwHdGSsdyJROrWMtKZIGwCOQ && mNDciKKMxhCHhYavVQKyLDBvRnBGA)
			{
				VTgAwFEtNUjvWpidWFocuhxIajyP.Reset();
				mNDciKKMxhCHhYavVQKyLDBvRnBGA = false;
				hGktpADuBlnCOKLCLdmgdmUYknevA.Set();
				if (wait)
				{
					VTgAwFEtNUjvWpidWFocuhxIajyP.WaitOne();
				}
				jcWGpczhzcAfjSSbdxPbUQSVbGdIA();
			}
		}

		public bool EnqueueAction(Action action)
		{
			if (action == null)
			{
				return false;
			}
			if (!kZZDZwHdGSsdyJROrWMtKZIGwCOQ)
			{
				return false;
			}
			if (!mNDciKKMxhCHhYavVQKyLDBvRnBGA)
			{
				return false;
			}
			ResetTimeout();
			lock (MBaCTcelMiCcAUdNLBodAYlhlgXDB)
			{
				DpoYcapUJZeucxsIeBQgGHYsgFAS.Enqueue(action);
				uTitnoCZPseieZfglLunlsiEexCl = true;
				hGktpADuBlnCOKLCLdmgdmUYknevA.Set();
			}
			return true;
		}

		public bool InvokeActionSync(Action action)
		{
			if (!kZZDZwHdGSsdyJROrWMtKZIGwCOQ)
			{
				return false;
			}
			if (!mNDciKKMxhCHhYavVQKyLDBvRnBGA)
			{
				return false;
			}
			EnqueueAction(action);
			WaitForActionQueueToFinish();
			return true;
		}

		public void WaitForActionQueueToFinish()
		{
			if (!kZZDZwHdGSsdyJROrWMtKZIGwCOQ || !mNDciKKMxhCHhYavVQKyLDBvRnBGA)
			{
				return;
			}
			ResetTimeout();
			lock (MBaCTcelMiCcAUdNLBodAYlhlgXDB)
			{
				htymIYwbLKNoeJcBlpnDMPNTkfiE.Reset();
				lzGgsslikWZsQOarIqTEmXLZwHyi++;
			}
			hGktpADuBlnCOKLCLdmgdmUYknevA.Set();
			htymIYwbLKNoeJcBlpnDMPNTkfiE.WaitOne();
			lock (MBaCTcelMiCcAUdNLBodAYlhlgXDB)
			{
				lzGgsslikWZsQOarIqTEmXLZwHyi--;
			}
		}

		public void ResetTimeout()
		{
			XhHdaGkgmbkZUTEqHPNfehdXGIMP = ((liVzExsOiSCHNhLxVFDdKuNvJGipA > 0) ? (QlRxqQimbQbqpyJTEYxRmcZkfKaCA.elapsedMillisecondsRaw + liVzExsOiSCHNhLxVFDdKuNvJGipA) : 0);
		}

		private void HDmjbvtEGyrvyjIuQWEOwjZelfzx()
		{
			ResetTimeout();
			kZZDZwHdGSsdyJROrWMtKZIGwCOQ = true;
			mNDciKKMxhCHhYavVQKyLDBvRnBGA = true;
			VTgAwFEtNUjvWpidWFocuhxIajyP.Set();
			if (SHnYNIalklTrGaNSMbUJsmWCEnReA != null)
			{
				lock (SHnYNIalklTrGaNSMbUJsmWCEnReA)
				{
					try
					{
						SHnYNIalklTrGaNSMbUJsmWCEnReA();
					}
					catch (Exception ex)
					{
						Logger.LogError("Caught exception in thread start event callback.\n" + ex, requiredThreadSafety: true);
					}
				}
			}
			while (mNDciKKMxhCHhYavVQKyLDBvRnBGA)
			{
				long num = QlRxqQimbQbqpyJTEYxRmcZkfKaCA.elapsedTicksRaw + GeECNMkEIieMtdzVMrLwdMaizJToB;
				gzIeUueqblSBFDpnjfJELENYSXpjB();
				lock (MBaCTcelMiCcAUdNLBodAYlhlgXDB)
				{
					if (!uTitnoCZPseieZfglLunlsiEexCl && lzGgsslikWZsQOarIqTEmXLZwHyi > 0)
					{
						htymIYwbLKNoeJcBlpnDMPNTkfiE.Set();
					}
				}
				if (JLwggAurcOsmgrSNKskBucfYBTmIA != null)
				{
					try
					{
						JLwggAurcOsmgrSNKskBucfYBTmIA();
					}
					catch (Exception ex2)
					{
						Logger.LogError("Exception occurred in a Thread Update Event callback.\n" + ex2, requiredThreadSafety: true);
					}
				}
				if (HdlfBlWESYakoDkxabVbcBoKczcT)
				{
					if (ncjPrbQulEjiCssjHHchFeuOnVpq || (long)zDtrQYQOnvRShvWnorLXdPmKaTpdA >= 750L)
					{
						while (QlRxqQimbQbqpyJTEYxRmcZkfKaCA.elapsedTicksRaw < num)
						{
						}
					}
					else
					{
						long num2 = num - QlRxqQimbQbqpyJTEYxRmcZkfKaCA.elapsedTicksRaw;
						if (num2 > 0)
						{
							hGktpADuBlnCOKLCLdmgdmUYknevA.WaitOne(TimeSpan.FromTicks(Stopwatch.ConvertTo100NSTicks(num2)));
						}
					}
				}
				FOrwgpSCEfPtfdinooKAmKYwlBsP = ((FOrwgpSCEfPtfdinooKAmKYwlBsP != 4294967295u) ? (FOrwgpSCEfPtfdinooKAmKYwlBsP + 1) : 0u);
				if (liVzExsOiSCHNhLxVFDdKuNvJGipA > 0 && QlRxqQimbQbqpyJTEYxRmcZkfKaCA.elapsedMillisecondsRaw >= XhHdaGkgmbkZUTEqHPNfehdXGIMP)
				{
					mNDciKKMxhCHhYavVQKyLDBvRnBGA = false;
				}
			}
			if (OpRgRqqVLlLUDDoAlXWhWnAIGLjP != null)
			{
				lock (OpRgRqqVLlLUDDoAlXWhWnAIGLjP)
				{
					try
					{
						OpRgRqqVLlLUDDoAlXWhWnAIGLjP();
					}
					catch (Exception ex3)
					{
						Logger.LogError("Caught exception in thread pre-stop event event callback.\n" + ex3, requiredThreadSafety: true);
					}
				}
			}
			kZZDZwHdGSsdyJROrWMtKZIGwCOQ = false;
			VTgAwFEtNUjvWpidWFocuhxIajyP.Set();
		}

		private void gzIeUueqblSBFDpnjfJELENYSXpjB()
		{
			if (!uTitnoCZPseieZfglLunlsiEexCl)
			{
				return;
			}
			lock (MBaCTcelMiCcAUdNLBodAYlhlgXDB)
			{
				MiscTools.Swap(ref DpoYcapUJZeucxsIeBQgGHYsgFAS, ref QEzfWVuZPKrSulEqXVvIpNQHrngi);
				uTitnoCZPseieZfglLunlsiEexCl = false;
			}
			while (QEzfWVuZPKrSulEqXVvIpNQHrngi.Count > 0)
			{
				Action action = QEzfWVuZPKrSulEqXVvIpNQHrngi.Dequeue();
				try
				{
					action();
				}
				catch (Exception ex)
				{
					Logger.LogError("Exception occurred while processing thread Action queue.\n" + ex, requiredThreadSafety: true);
				}
			}
		}

		private void mPQFzJiqvNTWElbAmZQIEsuHbbPq()
		{
			if (zDtrQYQOnvRShvWnorLXdPmKaTpdA <= 0)
			{
				HdlfBlWESYakoDkxabVbcBoKczcT = false;
			}
			else
			{
				HdlfBlWESYakoDkxabVbcBoKczcT = true;
				GeECNMkEIieMtdzVMrLwdMaizJToB = Stopwatch.frequency / zDtrQYQOnvRShvWnorLXdPmKaTpdA;
			}
			ResetTimeout();
		}

		private void jcWGpczhzcAfjSSbdxPbUQSVbGdIA()
		{
			EbRHMbdtsvtIjMjOSMykEVysYQmMA = null;
			kZZDZwHdGSsdyJROrWMtKZIGwCOQ = false;
			mNDciKKMxhCHhYavVQKyLDBvRnBGA = false;
			DpoYcapUJZeucxsIeBQgGHYsgFAS.Clear();
			QEzfWVuZPKrSulEqXVvIpNQHrngi.Clear();
			uTitnoCZPseieZfglLunlsiEexCl = false;
			lzGgsslikWZsQOarIqTEmXLZwHyi = 0;
			VTgAwFEtNUjvWpidWFocuhxIajyP.Reset();
			htymIYwbLKNoeJcBlpnDMPNTkfiE.Reset();
			XhHdaGkgmbkZUTEqHPNfehdXGIMP = 0L;
			FOrwgpSCEfPtfdinooKAmKYwlBsP = 0u;
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

		~ThreadHelper()
		{
			Dispose(disposing: false);
		}

		protected void Dispose(bool disposing)
		{
			if (!mjtbktfPEnuUVvKGsQRntoxgCjAO)
			{
				if (disposing)
				{
					Stop(wait: true);
				}
				else
				{
					mNDciKKMxhCHhYavVQKyLDBvRnBGA = false;
				}
				mjtbktfPEnuUVvKGsQRntoxgCjAO = true;
			}
		}

		[Conditional("DEBUG_THREAD_HELPER")]
		private static void WCZBgtjugzVPtZUBiIHieXIelvSBA(object P_0)
		{
			if (P_0 != null)
			{
				Logger.Log(P_0, requiredThreadSafety: true);
			}
		}
	}
}
