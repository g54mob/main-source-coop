using System;
using UnityEngine;

namespace Rewired.Utils.Classes.Utility
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal sealed class UnityStopwatch : StopwatchBase
	{
		private class FLoQUSxgYMBWwDGjPwIYbvXubyuZA
		{
			public const long wCmVZVVnyNAhVbdqtGqFCIuhmuGeb = 10000000L;

			private double rKxbHpKgUzBfNWvbQHBuUoLiHqJGb;

			private bool vcAxoNbQZMGTVhlkSeKLNpzirRDK;

			private double REcxwCBnouHBfcmTGQrvSIllxuXi;

			private double ZEzWOMlqDxOOcFLQviNpMhrYZVul;

			public bool XQVETgFgOCvMbBtPTvTMhTNJincfb => vcAxoNbQZMGTVhlkSeKLNpzirRDK;

			public double zGphhqpqPgmxNBBNqAmrGaZDdszzA
			{
				get
				{
					if (!vcAxoNbQZMGTVhlkSeKLNpzirRDK)
					{
						return ZEzWOMlqDxOOcFLQviNpMhrYZVul;
					}
					return (double)Time.realtimeSinceStartup - REcxwCBnouHBfcmTGQrvSIllxuXi;
				}
			}

			public void XPdUvsVLWySABNRpgYdhhlRuWpdQ()
			{
				rKxbHpKgUzBfNWvbQHBuUoLiHqJGb = Time.realtimeSinceStartup;
			}

			public void YvndsCdiQwbDKDoQaLXEzJEDGMREB()
			{
				if (!vcAxoNbQZMGTVhlkSeKLNpzirRDK)
				{
					vcAxoNbQZMGTVhlkSeKLNpzirRDK = true;
					REcxwCBnouHBfcmTGQrvSIllxuXi = rKxbHpKgUzBfNWvbQHBuUoLiHqJGb;
				}
			}

			public void XhUDoLCWmuNLoMusfQIuPQGxxkCCA()
			{
				if (vcAxoNbQZMGTVhlkSeKLNpzirRDK)
				{
					vcAxoNbQZMGTVhlkSeKLNpzirRDK = false;
					ZEzWOMlqDxOOcFLQviNpMhrYZVul += rKxbHpKgUzBfNWvbQHBuUoLiHqJGb - REcxwCBnouHBfcmTGQrvSIllxuXi;
				}
			}

			public void aZJRFLlUcAPaHeMRgCmAeTIOqLsZA()
			{
				REcxwCBnouHBfcmTGQrvSIllxuXi = 0.0;
				ZEzWOMlqDxOOcFLQviNpMhrYZVul = 0.0;
				bool num = vcAxoNbQZMGTVhlkSeKLNpzirRDK;
				vcAxoNbQZMGTVhlkSeKLNpzirRDK = false;
				if (num)
				{
					YvndsCdiQwbDKDoQaLXEzJEDGMREB();
				}
			}
		}

		private const long WnckrwwAuZbOxokzspZQXZoEvbSC = 10000000L;

		private static UnityStopwatch vkpoykmiDajWzPYhCoubqfTmFpcz;

		private readonly FLoQUSxgYMBWwDGjPwIYbvXubyuZA NWLwQvKnQhOHCxxqLQfBoKlECklaA;

		private readonly bool altUEMsxUmSVorcOAtYUQKHqAxVc;

		private double XOekhoHlXZfVLSjnyZhgvsFDhMee;

		public static UnityStopwatch Global => vkpoykmiDajWzPYhCoubqfTmFpcz ?? (vkpoykmiDajWzPYhCoubqfTmFpcz = new UnityStopwatch(true));

		public static long frequency => 10000000L;

		double StopwatchBase.offsetSeconds
		{
			get
			{
				return XOekhoHlXZfVLSjnyZhgvsFDhMee;
			}
			set
			{
				XOekhoHlXZfVLSjnyZhgvsFDhMee = value;
			}
		}

		long StopwatchBase.offsetTicks
		{
			get
			{
				return (long)(XOekhoHlXZfVLSjnyZhgvsFDhMee * 10000000.0);
			}
			set
			{
				XOekhoHlXZfVLSjnyZhgvsFDhMee = (double)value / 10000000.0;
			}
		}

		double StopwatchBase.elapsedSeconds => NWLwQvKnQhOHCxxqLQfBoKlECklaA.zGphhqpqPgmxNBBNqAmrGaZDdszzA + offsetSeconds;

		double StopwatchBase.elapsedSecondsRaw => NWLwQvKnQhOHCxxqLQfBoKlECklaA.zGphhqpqPgmxNBBNqAmrGaZDdszzA;

		long StopwatchBase.elapsedMilliseconds => (long)((NWLwQvKnQhOHCxxqLQfBoKlECklaA.zGphhqpqPgmxNBBNqAmrGaZDdszzA + XOekhoHlXZfVLSjnyZhgvsFDhMee) * 1000.0);

		long StopwatchBase.elapsedMillisecondsRaw => (long)(NWLwQvKnQhOHCxxqLQfBoKlECklaA.zGphhqpqPgmxNBBNqAmrGaZDdszzA * 1000.0);

		long StopwatchBase.elapsedTicks => (long)(elapsedSeconds * 10000000.0);

		long StopwatchBase.elapsedTicksRaw => (long)(elapsedSecondsRaw * 10000000.0);

		bool StopwatchBase.isRunning => NWLwQvKnQhOHCxxqLQfBoKlECklaA.XQVETgFgOCvMbBtPTvTMhTNJincfb;

		public static UnityStopwatch StartNew()
		{
			UnityStopwatch unityStopwatch = new UnityStopwatch(false);
			unityStopwatch.Start();
			return unityStopwatch;
		}

		public static long ConvertTo100NSTicks(long ticks)
		{
			return ticks;
		}

		public UnityStopwatch()
			: this(false)
		{
		}

		private UnityStopwatch(bool P_0)
		{
			NWLwQvKnQhOHCxxqLQfBoKlECklaA = new FLoQUSxgYMBWwDGjPwIYbvXubyuZA();
			mJrvXBSTmGmbFrDqFUHWQxONNtzB();
			if (P_0)
			{
				Start();
			}
			altUEMsxUmSVorcOAtYUQKHqAxVc = P_0;
		}

		~UnityStopwatch()
		{
			PNOorPKmsmlrrPFXdboCWagopeDi();
		}

		public override void Stop()
		{
			if (altUEMsxUmSVorcOAtYUQKHqAxVc)
			{
				throw new Exception("The Global Stopwatch cannot be stopped.");
			}
			NWLwQvKnQhOHCxxqLQfBoKlECklaA.XhUDoLCWmuNLoMusfQIuPQGxxkCCA();
		}

		public override void Start()
		{
			if (!altUEMsxUmSVorcOAtYUQKHqAxVc)
			{
				NWLwQvKnQhOHCxxqLQfBoKlECklaA.YvndsCdiQwbDKDoQaLXEzJEDGMREB();
			}
		}

		public override void Reset()
		{
			if (altUEMsxUmSVorcOAtYUQKHqAxVc)
			{
				throw new Exception("The Global Stopwatch cannot be reset.");
			}
			NWLwQvKnQhOHCxxqLQfBoKlECklaA.aZJRFLlUcAPaHeMRgCmAeTIOqLsZA();
		}

		private void mJrvXBSTmGmbFrDqFUHWQxONNtzB()
		{
			PNOorPKmsmlrrPFXdboCWagopeDi();
			ReInput.BeforeTimeManagerUpdateEvent += HVnuqLyFzvgfKeKRTtAMwTKlIoAA;
		}

		private void PNOorPKmsmlrrPFXdboCWagopeDi()
		{
			ReInput.BeforeTimeManagerUpdateEvent -= HVnuqLyFzvgfKeKRTtAMwTKlIoAA;
		}

		private void HVnuqLyFzvgfKeKRTtAMwTKlIoAA(UpdateLoopType P_0)
		{
			NWLwQvKnQhOHCxxqLQfBoKlECklaA.XPdUvsVLWySABNRpgYdhhlRuWpdQ();
		}
	}
}
