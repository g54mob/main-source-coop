using System.Collections.Generic;
using System.Threading.Tasks;
using EvilAnalytics.SDK.Core;
using EvilAnalytics.Shared.Common;

namespace EvilCore.Networking
{
	public sealed class EvilAnalyticsNetworkTelemetry : INetworkTelemetry
	{
		public void RecordAttempt(string op)
		{
			Fire("net_" + op + "_attempt", EventCategory.Custom, null);
		}

		public void RecordSuccess(string op, int attempts)
		{
			Fire("net_" + op + "_success", EventCategory.Custom, new Dictionary<string, object> { { "attempts", attempts } });
		}

		public void RecordFailure(string op, NetworkErrorType type, string resultCode, int attempts, bool rateLimited)
		{
			Fire("net_" + op + "_fail", EventCategory.Error, new Dictionary<string, object>
			{
				{
					"reason",
					type.ToString()
				},
				{
					"result_code",
					resultCode ?? string.Empty
				},
				{ "attempts", attempts },
				{ "rate_limited", rateLimited }
			});
		}

		public void RecordReportedError(NetworkErrorType type, string detail)
		{
			Fire("net_error", EventCategory.Error, new Dictionary<string, object>
			{
				{
					"type",
					type.ToString()
				},
				{
					"detail",
					detail ?? string.Empty
				}
			});
		}

		private static void Fire(string eventName, EventCategory category, Dictionary<string, object> properties)
		{
			if (Analytics.IsInitialized)
			{
				FireAsync(eventName, category, properties);
			}
		}

		private static async Task FireAsync(string eventName, EventCategory category, Dictionary<string, object> properties)
		{
			try
			{
				await Analytics.TrackEventAsync(eventName, category, properties);
			}
			catch
			{
			}
		}
	}
}
