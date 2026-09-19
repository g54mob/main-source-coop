using System;

namespace Google.Apis.Http
{
	[Flags]
	public enum ExponentialBackOffPolicy
	{
		None = 0,
		Exception = 1,
		UnsuccessfulResponse503 = 2
	}
}
