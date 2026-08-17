using System;
using System.Diagnostics;

namespace Rewired.Utils.Classes.Utility
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal sealed class Stopwatch : StopwatchBase
	{
		private const long seImlbkqoRGapdXwBjBfYzBghxXr = 10000000L;

		public static readonly Stopwatch Global;

		private static long RsthikdWkEeZciDcMAqQhnUgQkAr;

		private System.Diagnostics.Stopwatch jbaWfUtUqtwEYRosDJYPXaXIIbBn;

		private long FnbleVYPYMiRuLPCHZpqrdGLhuYy;

		public static long frequency => RsthikdWkEeZciDcMAqQhnUgQkAr;

		double StopwatchBase.offsetSeconds
		{
			get
			{
				return (double)FnbleVYPYMiRuLPCHZpqrdGLhuYy / (double)RsthikdWkEeZciDcMAqQhnUgQkAr;
			}
			set
			{
				FnbleVYPYMiRuLPCHZpqrdGLhuYy = (long)(value * (double)RsthikdWkEeZciDcMAqQhnUgQkAr);
			}
		}

		long StopwatchBase.offsetTicks
		{
			get
			{
				return FnbleVYPYMiRuLPCHZpqrdGLhuYy;
			}
			set
			{
				FnbleVYPYMiRuLPCHZpqrdGLhuYy = value;
			}
		}

		double StopwatchBase.elapsedSeconds => (double)(jbaWfUtUqtwEYRosDJYPXaXIIbBn.ElapsedTicks + offsetTicks) / (double)RsthikdWkEeZciDcMAqQhnUgQkAr;

		double StopwatchBase.elapsedSecondsRaw => (double)jbaWfUtUqtwEYRosDJYPXaXIIbBn.ElapsedTicks / (double)RsthikdWkEeZciDcMAqQhnUgQkAr;

		long StopwatchBase.elapsedMilliseconds => (long)((double)(jbaWfUtUqtwEYRosDJYPXaXIIbBn.ElapsedTicks + offsetTicks) / (double)RsthikdWkEeZciDcMAqQhnUgQkAr * 1000.0);

		long StopwatchBase.elapsedMillisecondsRaw => jbaWfUtUqtwEYRosDJYPXaXIIbBn.ElapsedMilliseconds;

		long StopwatchBase.elapsedTicks => jbaWfUtUqtwEYRosDJYPXaXIIbBn.ElapsedTicks + FnbleVYPYMiRuLPCHZpqrdGLhuYy;

		long StopwatchBase.elapsedTicksRaw => jbaWfUtUqtwEYRosDJYPXaXIIbBn.ElapsedTicks;

		bool StopwatchBase.isRunning => jbaWfUtUqtwEYRosDJYPXaXIIbBn.IsRunning;

		static Stopwatch()
		{
			RsthikdWkEeZciDcMAqQhnUgQkAr = System.Diagnostics.Stopwatch.Frequency;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			Global = stopwatch;
		}

		public static Stopwatch StartNew()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			return stopwatch;
		}

		public static long ConvertTo100NSTicks(long ticks)
		{
			if (RsthikdWkEeZciDcMAqQhnUgQkAr == 10000000)
			{
				return ticks;
			}
			return 10000000 / RsthikdWkEeZciDcMAqQhnUgQkAr;
		}

		public Stopwatch()
		{
			jbaWfUtUqtwEYRosDJYPXaXIIbBn = new System.Diagnostics.Stopwatch();
		}

		public override void Stop()
		{
			if (this == Global)
			{
				throw new Exception("The Global Stopwatch cannot be stopped.");
			}
			jbaWfUtUqtwEYRosDJYPXaXIIbBn.Stop();
		}

		public override void Start()
		{
			if (this != Global)
			{
				jbaWfUtUqtwEYRosDJYPXaXIIbBn.Start();
			}
		}

		public override void Reset()
		{
			if (this == Global)
			{
				throw new Exception("The Global Stopwatch cannot be reset.");
			}
			jbaWfUtUqtwEYRosDJYPXaXIIbBn.Reset();
		}
	}
}
