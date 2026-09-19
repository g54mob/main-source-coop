using System;

namespace Google.Apis.Util
{
	public interface IClock
	{
		[Obsolete("System local time is almost always inappropriate to use. If you really need this, call UtcNow and then call ToLocalTime on the result")]
		DateTime Now { get; }

		DateTime UtcNow { get; }
	}
}
