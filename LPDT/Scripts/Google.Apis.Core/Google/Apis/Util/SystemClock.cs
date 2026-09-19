using System;

namespace Google.Apis.Util
{
	public class SystemClock : IClock
	{
		public static readonly IClock Default = new SystemClock();

		public DateTime Now => DateTime.Now;

		public DateTime UtcNow => DateTime.UtcNow;

		protected SystemClock()
		{
		}
	}
}
