using System;

namespace Mirror.BouncyCastle.Utilities.Date
{
	public static class DateTimeUtilities
	{
		public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static readonly long MaxUnixMs;

		public static readonly long MinUnixMs;

		public static long DateTimeToUnixMs(DateTime dateTime)
		{
			DateTime dateTime2 = dateTime.ToUniversalTime();
			if (dateTime2.CompareTo(UnixEpoch) < 0)
			{
				throw new ArgumentOutOfRangeException("dateTime", "DateTime value may not be before the epoch");
			}
			long ticks = dateTime2.Ticks;
			DateTime unixEpoch = UnixEpoch;
			return (ticks - unixEpoch.Ticks) / 10000;
		}

		public static long CurrentUnixMs()
		{
			return DateTimeToUnixMs(DateTime.UtcNow);
		}

		static DateTimeUtilities()
		{
			DateTime maxValue = DateTime.MaxValue;
			MaxUnixMs = (maxValue.Ticks - UnixEpoch.Ticks) / 10000;
			MinUnixMs = 0L;
		}
	}
}
