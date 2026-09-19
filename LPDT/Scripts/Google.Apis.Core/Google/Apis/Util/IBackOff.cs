using System;

namespace Google.Apis.Util
{
	public interface IBackOff
	{
		int MaxNumOfRetries { get; }

		TimeSpan GetNextBackOff(int currentRetry);
	}
}
