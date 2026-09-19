using System;
using Google.Apis.Logging;

namespace Google
{
	public static class ApplicationContext
	{
		private static ILogger logger;

		public static ILogger Logger => logger ?? (logger = new NullLogger());

		internal static void Reset()
		{
			logger = null;
		}

		public static void RegisterLogger(ILogger loggerToRegister)
		{
			if (logger != null && !(logger is NullLogger))
			{
				throw new InvalidOperationException("A logger was already registered with this context.");
			}
			logger = loggerToRegister;
		}
	}
}
