using System;

namespace Features.UsersStatsModule.Scripts
{
	public interface IUserStatsService
	{
		event Action OnGlobalStatsReceived;

		bool TryGetGlobalStat(string pchStatName, out long pData);

		bool IncrementUserStat(string pchStatName, int delta = 1);

		void RequestGlobalStats(int historyDays);
	}
}
