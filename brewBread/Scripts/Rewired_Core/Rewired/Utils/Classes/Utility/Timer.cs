using System;
using UnityEngine;

namespace Rewired.Utils.Classes.Utility
{
	[Serializable]
	[CustomObfuscation(rename = false)]
	internal class Timer
	{
		public bool running;

		[SerializeField]
		private double timer;

		public double length;

		public Timer()
		{
		}

		public Timer(double P_0)
		{
			length = P_0;
		}

		public void edGQWybkvEgdpdpFVqgmUdPGZTqV()
		{
			running = true;
			timer = length;
		}

		public void edGQWybkvEgdpdpFVqgmUdPGZTqV(double P_0)
		{
			running = true;
			length = P_0;
			timer = length;
		}

		public void VsFiwwWjBjBmpAxpxJrxioIwcJno()
		{
			SPGTRPyvIslcMdbPTItsewSLRPxx();
			edGQWybkvEgdpdpFVqgmUdPGZTqV();
		}

		public bool jRaYtHNVcykNMAbqOnSGaKIIGSEaA(double P_0)
		{
			if (!running)
			{
				return false;
			}
			timer -= P_0;
			if (timer <= 0.0)
			{
				running = false;
				return true;
			}
			return false;
		}

		public void SPGTRPyvIslcMdbPTItsewSLRPxx()
		{
			running = false;
			timer = 0.0;
		}

		public void ldaNExqXJaJlPtgnaHlvtaPGGJxdA(double P_0)
		{
			length = P_0;
		}

		public Timer vpHGhDFXfrFPnKJBKRFCgbudEJvBB()
		{
			return (Timer)MemberwiseClone();
		}
	}
}
