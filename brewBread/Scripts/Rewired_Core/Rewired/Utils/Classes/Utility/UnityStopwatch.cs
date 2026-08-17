using System;
using UnityEngine;

namespace Rewired.Utils.Classes.Utility
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal sealed class UnityStopwatch : StopwatchBase
	{
		private class lzQzLGBNlkdSlyWGnWMQYCGMyntL
		{
			public const long mYclhanKzYDkSLRjuGvuHZoAvEvoA = 10000000L;

			private double IjwtBFxKlmRsYFwipWqacUjFfObj;

			private bool JcSOJGqlEfppAGIajtcZvcxleNQEA;

			private double ExyPsGsshISNmqeXwjckKDdtxeWL;

			private double XWPYglQXCXFIeMihMCXpCkTwPqhG;

			public bool XWdcjkkYyzrqvvuauNalniSKBNdJA => JcSOJGqlEfppAGIajtcZvcxleNQEA;

			public double tlsAeMueVgQecAbCerdWrNsuCezl
			{
				get
				{
					if (!JcSOJGqlEfppAGIajtcZvcxleNQEA)
					{
						return XWPYglQXCXFIeMihMCXpCkTwPqhG;
					}
					return (double)Time.realtimeSinceStartup - ExyPsGsshISNmqeXwjckKDdtxeWL;
				}
			}

			public void jRaYtHNVcykNMAbqOnSGaKIIGSEaA()
			{
				IjwtBFxKlmRsYFwipWqacUjFfObj = Time.realtimeSinceStartup;
			}

			public void edGQWybkvEgdpdpFVqgmUdPGZTqV()
			{
				if (!JcSOJGqlEfppAGIajtcZvcxleNQEA)
				{
					JcSOJGqlEfppAGIajtcZvcxleNQEA = true;
					ExyPsGsshISNmqeXwjckKDdtxeWL = IjwtBFxKlmRsYFwipWqacUjFfObj;
				}
			}

			public void neftIrgqNxhCwszXDdZtxDjrjvobA()
			{
				if (JcSOJGqlEfppAGIajtcZvcxleNQEA)
				{
					JcSOJGqlEfppAGIajtcZvcxleNQEA = false;
					XWPYglQXCXFIeMihMCXpCkTwPqhG += IjwtBFxKlmRsYFwipWqacUjFfObj - ExyPsGsshISNmqeXwjckKDdtxeWL;
				}
			}

			public void jpwugzufXqktYbXkMYboQpqCbQgL()
			{
				ExyPsGsshISNmqeXwjckKDdtxeWL = 0.0;
				XWPYglQXCXFIeMihMCXpCkTwPqhG = 0.0;
				bool jcSOJGqlEfppAGIajtcZvcxleNQEA = JcSOJGqlEfppAGIajtcZvcxleNQEA;
				JcSOJGqlEfppAGIajtcZvcxleNQEA = false;
				if (jcSOJGqlEfppAGIajtcZvcxleNQEA)
				{
					edGQWybkvEgdpdpFVqgmUdPGZTqV();
				}
			}
		}

		private const long xvvFAJSCYdpicCuWQflaSHQDxZvJ = 10000000L;

		private static UnityStopwatch mXZKyyEjAeUJuNQoiHqkVhTWLRFf;

		private readonly lzQzLGBNlkdSlyWGnWMQYCGMyntL iRmlHWzqMyOzWizPjKawEVzqnmnA;

		private readonly bool pgSNZWRlIzWpKIojmChdKuLsINgEA;

		private double SBCUNRppGZiwfEIgHHfvcplDxbZVB;

		public static UnityStopwatch Global => mXZKyyEjAeUJuNQoiHqkVhTWLRFf ?? (mXZKyyEjAeUJuNQoiHqkVhTWLRFf = new UnityStopwatch(true));

		public static long frequency => 10000000L;

		public override double offsetSeconds
		{
			get
			{
				return SBCUNRppGZiwfEIgHHfvcplDxbZVB;
			}
			set
			{
				SBCUNRppGZiwfEIgHHfvcplDxbZVB = value;
			}
		}

		public override long offsetTicks
		{
			get
			{
				return (long)(SBCUNRppGZiwfEIgHHfvcplDxbZVB * 10000000.0);
			}
			set
			{
				SBCUNRppGZiwfEIgHHfvcplDxbZVB = (double)value / 10000000.0;
			}
		}

		public override double elapsedSeconds => iRmlHWzqMyOzWizPjKawEVzqnmnA.tlsAeMueVgQecAbCerdWrNsuCezl + offsetSeconds;

		public override double elapsedSecondsRaw => iRmlHWzqMyOzWizPjKawEVzqnmnA.tlsAeMueVgQecAbCerdWrNsuCezl;

		public override long elapsedMilliseconds => (long)((iRmlHWzqMyOzWizPjKawEVzqnmnA.tlsAeMueVgQecAbCerdWrNsuCezl + SBCUNRppGZiwfEIgHHfvcplDxbZVB) * 1000.0);

		public override long elapsedMillisecondsRaw => (long)(iRmlHWzqMyOzWizPjKawEVzqnmnA.tlsAeMueVgQecAbCerdWrNsuCezl * 1000.0);

		public override long elapsedTicks => (long)(elapsedSeconds * 10000000.0);

		public override long elapsedTicksRaw => (long)(elapsedSecondsRaw * 10000000.0);

		public override bool isRunning => iRmlHWzqMyOzWizPjKawEVzqnmnA.XWdcjkkYyzrqvvuauNalniSKBNdJA;

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
			iRmlHWzqMyOzWizPjKawEVzqnmnA = new lzQzLGBNlkdSlyWGnWMQYCGMyntL();
			ZKOFEwLsWkYLqkSbZmhCrbFIuhGX();
			if (P_0)
			{
				Start();
			}
			pgSNZWRlIzWpKIojmChdKuLsINgEA = P_0;
		}

		~UnityStopwatch()
		{
			ZkbMPnXIgfuzLtZDINNVeIcdeuQx();
		}

		public override void Stop()
		{
			if (pgSNZWRlIzWpKIojmChdKuLsINgEA)
			{
				throw new Exception("The Global Stopwatch cannot be stopped.");
			}
			iRmlHWzqMyOzWizPjKawEVzqnmnA.neftIrgqNxhCwszXDdZtxDjrjvobA();
		}

		public override void Start()
		{
			if (!pgSNZWRlIzWpKIojmChdKuLsINgEA)
			{
				iRmlHWzqMyOzWizPjKawEVzqnmnA.edGQWybkvEgdpdpFVqgmUdPGZTqV();
			}
		}

		public override void Reset()
		{
			if (pgSNZWRlIzWpKIojmChdKuLsINgEA)
			{
				throw new Exception("The Global Stopwatch cannot be reset.");
			}
			iRmlHWzqMyOzWizPjKawEVzqnmnA.jpwugzufXqktYbXkMYboQpqCbQgL();
		}

		private void ZKOFEwLsWkYLqkSbZmhCrbFIuhGX()
		{
			ZkbMPnXIgfuzLtZDINNVeIcdeuQx();
			ReInput.BeforeTimeManagerUpdateEvent += ZCGETbjMQZUkyflRtYAqwQNUBPQIb;
		}

		private void ZkbMPnXIgfuzLtZDINNVeIcdeuQx()
		{
			ReInput.BeforeTimeManagerUpdateEvent -= ZCGETbjMQZUkyflRtYAqwQNUBPQIb;
		}

		private void ZCGETbjMQZUkyflRtYAqwQNUBPQIb(UpdateLoopType P_0)
		{
			iRmlHWzqMyOzWizPjKawEVzqnmnA.jRaYtHNVcykNMAbqOnSGaKIIGSEaA();
		}
	}
}
