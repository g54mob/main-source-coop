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
		private const uint QOZNKTcUAswGBtxBixwaxQlAoMI = 750u;

		private readonly Stopwatch iRmlHWzqMyOzWizPjKawEVzqnmnA;

		private Thread SDWRoAydnweorhdNaUaMNqumjHBn;

		private ManualResetEvent wQLhJViaxXRjCwrqEzkAefEjSRaP;

		private ManualResetEvent tfbogMNMGTnPQxGCXWltAtetmPTj;

		private AutoResetEvent kMcPvKvaNtQafLhBZoJcLsuWpWJV;

		private bool AuDUroxOkNhCEILUjOQlXknzLpXq;

		private bool JcSOJGqlEfppAGIajtcZvcxleNQEA;

		private int rsEtpujLAKkrCauZLxSYAANdrHpB;

		private bool HmbDZGcbUcfnPYStZCxRanVXeirvA;

		private int QuejbxSpgdfDbHCFVfBJZEJEocOE;

		private long jyGCuKPaQwhcOEilogUCEUAxhrdU;

		private bool VibCwvSpQvmeyxgwQUQIRaOqNqhM;

		private int UUSPUkRBsJIyqRununeizCgEiwIc;

		private long XByanqjyYqVXUsIqHFDiqFFitSTy;

		private uint lxnTuoIZXwmHWoHJdMNViIxHpZDV;

		private readonly object sAyleNeQceINNqeahDvwkoKEewUN;

		private Queue<Action> wrqLioyoqGKHndPanqROqkoxgTQgA;

		private Queue<Action> HmqdiZiUBBcCIEtZGFrcHNeTMLuk;

		private bool htCfhzAavMhbaqvqkbiNnWIAYJbx;

		private Action GXugdreVJAFfCPaKDjepAVXVAERkA;

		[CompilerGenerated]
		private Action fASLChwpkOublZawptjIDVHbQBRf;

		[CompilerGenerated]
		private Action jdbcNtITOUlNGJylYGOqqcrUiVBCA;

		private bool AeWaeWamxRrERkciQkpFDWbRfZMkA;

		public bool isRunning => JcSOJGqlEfppAGIajtcZvcxleNQEA;

		public bool isStopped
		{
			get
			{
				if (!JcSOJGqlEfppAGIajtcZvcxleNQEA)
				{
					if (SDWRoAydnweorhdNaUaMNqumjHBn == null)
					{
						return true;
					}
					return !SDWRoAydnweorhdNaUaMNqumjHBn.IsAlive;
				}
				return false;
			}
		}

		public bool useHighPrecitionTimer
		{
			get
			{
				if (!HmbDZGcbUcfnPYStZCxRanVXeirvA)
				{
					return (long)QuejbxSpgdfDbHCFVfBJZEJEocOE >= 750L;
				}
				return true;
			}
			set
			{
				if (value != HmbDZGcbUcfnPYStZCxRanVXeirvA)
				{
					HmbDZGcbUcfnPYStZCxRanVXeirvA = value;
					QRGuzioveUXCfBGGaFwgXABFPueG();
				}
			}
		}

		public bool useFixedTimeStep => VibCwvSpQvmeyxgwQUQIRaOqNqhM;

		public int fixedTimeStepFPS
		{
			get
			{
				return QuejbxSpgdfDbHCFVfBJZEJEocOE;
			}
			set
			{
				QuejbxSpgdfDbHCFVfBJZEJEocOE = ((value > 0) ? value : 0);
				QRGuzioveUXCfBGGaFwgXABFPueG();
			}
		}

		public int timeoutMS
		{
			get
			{
				return UUSPUkRBsJIyqRununeizCgEiwIc;
			}
			set
			{
				UUSPUkRBsJIyqRununeizCgEiwIc = ((value > 0) ? value : 0);
				QRGuzioveUXCfBGGaFwgXABFPueG();
			}
		}

		public uint tick => lxnTuoIZXwmHWoHJdMNViIxHpZDV;

		public event Action ThreadUpdateEvent
		{
			add
			{
				GXugdreVJAFfCPaKDjepAVXVAERkA = (Action)Delegate.Combine(GXugdreVJAFfCPaKDjepAVXVAERkA, value);
			}
			remove
			{
				GXugdreVJAFfCPaKDjepAVXVAERkA = (Action)Delegate.Remove(GXugdreVJAFfCPaKDjepAVXVAERkA, value);
			}
		}

		private event Action _ThreadStartedEvent
		{
			[CompilerGenerated]
			add
			{
				Action action = fASLChwpkOublZawptjIDVHbQBRf;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, b);
					action = Interlocked.CompareExchange(ref fASLChwpkOublZawptjIDVHbQBRf, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = fASLChwpkOublZawptjIDVHbQBRf;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value3);
					action = Interlocked.CompareExchange(ref fASLChwpkOublZawptjIDVHbQBRf, value2, action2);
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
				Action action = jdbcNtITOUlNGJylYGOqqcrUiVBCA;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, b);
					action = Interlocked.CompareExchange(ref jdbcNtITOUlNGJylYGOqqcrUiVBCA, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = jdbcNtITOUlNGJylYGOqqcrUiVBCA;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value3);
					action = Interlocked.CompareExchange(ref jdbcNtITOUlNGJylYGOqqcrUiVBCA, value2, action2);
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
			iRmlHWzqMyOzWizPjKawEVzqnmnA = Stopwatch.Global;
			if (P_0 < 0)
			{
				P_0 = 0;
			}
			if (P_2 < 0)
			{
				P_2 = 0;
			}
			UUSPUkRBsJIyqRununeizCgEiwIc = P_2;
			QuejbxSpgdfDbHCFVfBJZEJEocOE = P_0;
			HmbDZGcbUcfnPYStZCxRanVXeirvA = P_1;
			QRGuzioveUXCfBGGaFwgXABFPueG();
			wQLhJViaxXRjCwrqEzkAefEjSRaP = new ManualResetEvent(initialState: false);
			tfbogMNMGTnPQxGCXWltAtetmPTj = new ManualResetEvent(initialState: false);
			kMcPvKvaNtQafLhBZoJcLsuWpWJV = new AutoResetEvent(initialState: false);
			sAyleNeQceINNqeahDvwkoKEewUN = new object();
			wrqLioyoqGKHndPanqROqkoxgTQgA = new Queue<Action>();
			HmqdiZiUBBcCIEtZGFrcHNeTMLuk = new Queue<Action>();
		}

		public bool Start(bool wait)
		{
			if (JcSOJGqlEfppAGIajtcZvcxleNQEA)
			{
				return false;
			}
			try
			{
				wQLhJViaxXRjCwrqEzkAefEjSRaP.Reset();
				kMcPvKvaNtQafLhBZoJcLsuWpWJV.Reset();
				SDWRoAydnweorhdNaUaMNqumjHBn = new Thread(TwvNADwmMBWRUGonPQZWuUuzfycl);
				SDWRoAydnweorhdNaUaMNqumjHBn.Start();
				if (wait)
				{
					wQLhJViaxXRjCwrqEzkAefEjSRaP.WaitOne();
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
			if (SDWRoAydnweorhdNaUaMNqumjHBn != null && JcSOJGqlEfppAGIajtcZvcxleNQEA && AuDUroxOkNhCEILUjOQlXknzLpXq)
			{
				wQLhJViaxXRjCwrqEzkAefEjSRaP.Reset();
				AuDUroxOkNhCEILUjOQlXknzLpXq = false;
				kMcPvKvaNtQafLhBZoJcLsuWpWJV.Set();
				if (wait)
				{
					wQLhJViaxXRjCwrqEzkAefEjSRaP.WaitOne();
				}
				HPdhUycvZEuKWkzUokzqofIAeNbfb();
			}
		}

		public bool EnqueueAction(Action action)
		{
			if (action == null)
			{
				return false;
			}
			if (!JcSOJGqlEfppAGIajtcZvcxleNQEA)
			{
				return false;
			}
			if (!AuDUroxOkNhCEILUjOQlXknzLpXq)
			{
				return false;
			}
			ResetTimeout();
			lock (sAyleNeQceINNqeahDvwkoKEewUN)
			{
				wrqLioyoqGKHndPanqROqkoxgTQgA.Enqueue(action);
				htCfhzAavMhbaqvqkbiNnWIAYJbx = true;
				kMcPvKvaNtQafLhBZoJcLsuWpWJV.Set();
			}
			return true;
		}

		public bool InvokeActionSync(Action action)
		{
			if (!JcSOJGqlEfppAGIajtcZvcxleNQEA)
			{
				return false;
			}
			if (!AuDUroxOkNhCEILUjOQlXknzLpXq)
			{
				return false;
			}
			EnqueueAction(action);
			WaitForActionQueueToFinish();
			return true;
		}

		public void WaitForActionQueueToFinish()
		{
			if (!JcSOJGqlEfppAGIajtcZvcxleNQEA || !AuDUroxOkNhCEILUjOQlXknzLpXq)
			{
				return;
			}
			ResetTimeout();
			lock (sAyleNeQceINNqeahDvwkoKEewUN)
			{
				tfbogMNMGTnPQxGCXWltAtetmPTj.Reset();
				rsEtpujLAKkrCauZLxSYAANdrHpB++;
			}
			kMcPvKvaNtQafLhBZoJcLsuWpWJV.Set();
			tfbogMNMGTnPQxGCXWltAtetmPTj.WaitOne();
			lock (sAyleNeQceINNqeahDvwkoKEewUN)
			{
				rsEtpujLAKkrCauZLxSYAANdrHpB--;
			}
		}

		public void ResetTimeout()
		{
			XByanqjyYqVXUsIqHFDiqFFitSTy = ((UUSPUkRBsJIyqRununeizCgEiwIc > 0) ? (iRmlHWzqMyOzWizPjKawEVzqnmnA.elapsedMillisecondsRaw + UUSPUkRBsJIyqRununeizCgEiwIc) : 0);
		}

		private void TwvNADwmMBWRUGonPQZWuUuzfycl()
		{
			ResetTimeout();
			JcSOJGqlEfppAGIajtcZvcxleNQEA = true;
			AuDUroxOkNhCEILUjOQlXknzLpXq = true;
			wQLhJViaxXRjCwrqEzkAefEjSRaP.Set();
			if (fASLChwpkOublZawptjIDVHbQBRf != null)
			{
				lock (fASLChwpkOublZawptjIDVHbQBRf)
				{
					try
					{
						fASLChwpkOublZawptjIDVHbQBRf();
					}
					catch (Exception ex)
					{
						Logger.LogError("Caught exception in thread start event callback.\n" + ex, requiredThreadSafety: true);
					}
				}
			}
			while (AuDUroxOkNhCEILUjOQlXknzLpXq)
			{
				long num = iRmlHWzqMyOzWizPjKawEVzqnmnA.elapsedTicksRaw + jyGCuKPaQwhcOEilogUCEUAxhrdU;
				zeksCKPXIKfCTtiTKRUTjwdMlxHT();
				lock (sAyleNeQceINNqeahDvwkoKEewUN)
				{
					if (!htCfhzAavMhbaqvqkbiNnWIAYJbx && rsEtpujLAKkrCauZLxSYAANdrHpB > 0)
					{
						tfbogMNMGTnPQxGCXWltAtetmPTj.Set();
					}
				}
				if (GXugdreVJAFfCPaKDjepAVXVAERkA != null)
				{
					try
					{
						GXugdreVJAFfCPaKDjepAVXVAERkA();
					}
					catch (Exception ex2)
					{
						Logger.LogError("Exception occurred in a Thread Update Event callback.\n" + ex2, requiredThreadSafety: true);
					}
				}
				if (VibCwvSpQvmeyxgwQUQIRaOqNqhM)
				{
					if (HmbDZGcbUcfnPYStZCxRanVXeirvA || (long)QuejbxSpgdfDbHCFVfBJZEJEocOE >= 750L)
					{
						while (iRmlHWzqMyOzWizPjKawEVzqnmnA.elapsedTicksRaw < num)
						{
						}
					}
					else
					{
						long num2 = num - iRmlHWzqMyOzWizPjKawEVzqnmnA.elapsedTicksRaw;
						if (num2 > 0)
						{
							kMcPvKvaNtQafLhBZoJcLsuWpWJV.WaitOne(TimeSpan.FromTicks(Stopwatch.ConvertTo100NSTicks(num2)));
						}
					}
				}
				lxnTuoIZXwmHWoHJdMNViIxHpZDV = ((lxnTuoIZXwmHWoHJdMNViIxHpZDV != uint.MaxValue) ? (lxnTuoIZXwmHWoHJdMNViIxHpZDV + 1) : 0u);
				if (UUSPUkRBsJIyqRununeizCgEiwIc > 0 && iRmlHWzqMyOzWizPjKawEVzqnmnA.elapsedMillisecondsRaw >= XByanqjyYqVXUsIqHFDiqFFitSTy)
				{
					AuDUroxOkNhCEILUjOQlXknzLpXq = false;
				}
			}
			if (jdbcNtITOUlNGJylYGOqqcrUiVBCA != null)
			{
				lock (jdbcNtITOUlNGJylYGOqqcrUiVBCA)
				{
					try
					{
						jdbcNtITOUlNGJylYGOqqcrUiVBCA();
					}
					catch (Exception ex3)
					{
						Logger.LogError("Caught exception in thread pre-stop event event callback.\n" + ex3, requiredThreadSafety: true);
					}
				}
			}
			JcSOJGqlEfppAGIajtcZvcxleNQEA = false;
			wQLhJViaxXRjCwrqEzkAefEjSRaP.Set();
		}

		private void zeksCKPXIKfCTtiTKRUTjwdMlxHT()
		{
			if (!htCfhzAavMhbaqvqkbiNnWIAYJbx)
			{
				return;
			}
			lock (sAyleNeQceINNqeahDvwkoKEewUN)
			{
				MiscTools.Swap(ref wrqLioyoqGKHndPanqROqkoxgTQgA, ref HmqdiZiUBBcCIEtZGFrcHNeTMLuk);
				htCfhzAavMhbaqvqkbiNnWIAYJbx = false;
			}
			while (HmqdiZiUBBcCIEtZGFrcHNeTMLuk.Count > 0)
			{
				Action action = HmqdiZiUBBcCIEtZGFrcHNeTMLuk.Dequeue();
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

		private void QRGuzioveUXCfBGGaFwgXABFPueG()
		{
			if (QuejbxSpgdfDbHCFVfBJZEJEocOE <= 0)
			{
				VibCwvSpQvmeyxgwQUQIRaOqNqhM = false;
			}
			else
			{
				VibCwvSpQvmeyxgwQUQIRaOqNqhM = true;
				jyGCuKPaQwhcOEilogUCEUAxhrdU = Stopwatch.frequency / QuejbxSpgdfDbHCFVfBJZEJEocOE;
			}
			ResetTimeout();
		}

		private void HPdhUycvZEuKWkzUokzqofIAeNbfb()
		{
			SDWRoAydnweorhdNaUaMNqumjHBn = null;
			JcSOJGqlEfppAGIajtcZvcxleNQEA = false;
			AuDUroxOkNhCEILUjOQlXknzLpXq = false;
			wrqLioyoqGKHndPanqROqkoxgTQgA.Clear();
			HmqdiZiUBBcCIEtZGFrcHNeTMLuk.Clear();
			htCfhzAavMhbaqvqkbiNnWIAYJbx = false;
			rsEtpujLAKkrCauZLxSYAANdrHpB = 0;
			wQLhJViaxXRjCwrqEzkAefEjSRaP.Reset();
			tfbogMNMGTnPQxGCXWltAtetmPTj.Reset();
			XByanqjyYqVXUsIqHFDiqFFitSTy = 0L;
			lxnTuoIZXwmHWoHJdMNViIxHpZDV = 0u;
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~ThreadHelper()
		{
			Dispose(disposing: false);
		}

		protected void Dispose(bool disposing)
		{
			if (!AeWaeWamxRrERkciQkpFDWbRfZMkA)
			{
				if (disposing)
				{
					Stop(wait: true);
				}
				else
				{
					AuDUroxOkNhCEILUjOQlXknzLpXq = false;
				}
				AeWaeWamxRrERkciQkpFDWbRfZMkA = true;
			}
		}

		[Conditional("DEBUG_THREAD_HELPER")]
		private static void YqTEREeAmudmGZOuDtwYJIZalkJpA(object P_0)
		{
			if (P_0 != null)
			{
				Logger.Log(P_0, requiredThreadSafety: true);
			}
		}
	}
}
