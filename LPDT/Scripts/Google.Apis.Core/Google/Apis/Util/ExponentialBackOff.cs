using System;

namespace Google.Apis.Util
{
	public class ExponentialBackOff : IBackOff
	{
		private const int MaxAllowedNumRetries = 20;

		private readonly TimeSpan deltaBackOff;

		private readonly int maxNumOfRetries;

		private Random random = new Random();

		public TimeSpan DeltaBackOff => deltaBackOff;

		public int MaxNumOfRetries => maxNumOfRetries;

		public ExponentialBackOff()
			: this(TimeSpan.FromMilliseconds(250.0))
		{
		}

		public ExponentialBackOff(TimeSpan deltaBackOff, int maximumNumOfRetries = 10)
		{
			if (deltaBackOff < TimeSpan.Zero || deltaBackOff > TimeSpan.FromSeconds(1.0))
			{
				throw new ArgumentOutOfRangeException("deltaBackOff");
			}
			if (maximumNumOfRetries < 0 || maximumNumOfRetries > 20)
			{
				throw new ArgumentOutOfRangeException("deltaBackOff");
			}
			this.deltaBackOff = deltaBackOff;
			maxNumOfRetries = maximumNumOfRetries;
		}

		public TimeSpan GetNextBackOff(int currentRetry)
		{
			if (currentRetry <= 0)
			{
				throw new ArgumentOutOfRangeException("currentRetry");
			}
			if (currentRetry > MaxNumOfRetries)
			{
				return TimeSpan.MinValue;
			}
			double num = random.Next((int)(DeltaBackOff.TotalMilliseconds * -1.0), (int)(DeltaBackOff.TotalMilliseconds * 1.0));
			return TimeSpan.FromMilliseconds((int)(Math.Pow(2.0, (double)currentRetry - 1.0) * 1000.0 + num));
		}
	}
}
