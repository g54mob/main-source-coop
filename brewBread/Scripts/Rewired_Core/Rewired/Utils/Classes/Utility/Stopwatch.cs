using System;
using System.Diagnostics;

namespace Rewired.Utils.Classes.Utility
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal sealed class Stopwatch : StopwatchBase
	{
		private const long xvvFAJSCYdpicCuWQflaSHQDxZvJ = 10000000L;

		public static readonly Stopwatch Global;

		private static long QyomXXAXACNueRhngANHebGqialcb;

		private System.Diagnostics.Stopwatch iRmlHWzqMyOzWizPjKawEVzqnmnA;

		private long cZfzhhIsSTMFmUiAjFhQGjuimmKUA;

		public static long frequency => QyomXXAXACNueRhngANHebGqialcb;

		public override double offsetSeconds
		{
			get
			{
				return (double)cZfzhhIsSTMFmUiAjFhQGjuimmKUA / (double)QyomXXAXACNueRhngANHebGqialcb;
			}
			set
			{
				cZfzhhIsSTMFmUiAjFhQGjuimmKUA = (long)(value * (double)QyomXXAXACNueRhngANHebGqialcb);
			}
		}

		public override long offsetTicks
		{
			get
			{
				return cZfzhhIsSTMFmUiAjFhQGjuimmKUA;
			}
			set
			{
				cZfzhhIsSTMFmUiAjFhQGjuimmKUA = value;
			}
		}

		public override double elapsedSeconds => (double)(iRmlHWzqMyOzWizPjKawEVzqnmnA.ElapsedTicks + offsetTicks) / (double)QyomXXAXACNueRhngANHebGqialcb;

		public override double elapsedSecondsRaw => (double)iRmlHWzqMyOzWizPjKawEVzqnmnA.ElapsedTicks / (double)QyomXXAXACNueRhngANHebGqialcb;

		public override long elapsedMilliseconds => (long)((double)(iRmlHWzqMyOzWizPjKawEVzqnmnA.ElapsedTicks + offsetTicks) / (double)QyomXXAXACNueRhngANHebGqialcb * 1000.0);

		public override long elapsedMillisecondsRaw => iRmlHWzqMyOzWizPjKawEVzqnmnA.ElapsedMilliseconds;

		public override long elapsedTicks => iRmlHWzqMyOzWizPjKawEVzqnmnA.ElapsedTicks + cZfzhhIsSTMFmUiAjFhQGjuimmKUA;

		public override long elapsedTicksRaw => iRmlHWzqMyOzWizPjKawEVzqnmnA.ElapsedTicks;

		public override bool isRunning => iRmlHWzqMyOzWizPjKawEVzqnmnA.IsRunning;

		static Stopwatch()
		{
			QyomXXAXACNueRhngANHebGqialcb = System.Diagnostics.Stopwatch.Frequency;
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
			if (QyomXXAXACNueRhngANHebGqialcb == 10000000)
			{
				return ticks;
			}
			return 10000000 / QyomXXAXACNueRhngANHebGqialcb;
		}

		public Stopwatch()
		{
			iRmlHWzqMyOzWizPjKawEVzqnmnA = new System.Diagnostics.Stopwatch();
		}

		public override void Stop()
		{
			if (this == Global)
			{
				throw new Exception("The Global Stopwatch cannot be stopped.");
			}
			iRmlHWzqMyOzWizPjKawEVzqnmnA.Stop();
		}

		public override void Start()
		{
			if (this != Global)
			{
				iRmlHWzqMyOzWizPjKawEVzqnmnA.Start();
			}
		}

		public override void Reset()
		{
			if (this == Global)
			{
				throw new Exception("The Global Stopwatch cannot be reset.");
			}
			iRmlHWzqMyOzWizPjKawEVzqnmnA.Reset();
		}
	}
}
