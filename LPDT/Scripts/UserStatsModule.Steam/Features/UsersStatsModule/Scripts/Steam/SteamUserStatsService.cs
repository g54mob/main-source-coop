using System;
using RSG.Muffin.SteamSubmodule.SteamModule.Scripts.API;
using Steamworks;

namespace Features.UsersStatsModule.Scripts.Steam
{
	public class SteamUserStatsService : IUserStatsService
	{
		private readonly SteamModel _steamModel;

		private CallResult<GlobalStatsReceived_t> _globalStatsReceivedCallResult;

		public event Action OnGlobalStatsReceived;

		public SteamUserStatsService(SteamModel steamModel)
		{
			_steamModel = steamModel;
		}

		public bool TryGetGlobalStat(string pchStatName, out long pData)
		{
			pData = 0L;
			if (!_steamModel.IsSteamInitialized)
			{
				return false;
			}
			return SteamUserStats.GetGlobalStat(pchStatName, out pData);
		}

		public bool IncrementUserStat(string pchStatName, int delta = 1)
		{
			if (!_steamModel.IsSteamInitialized)
			{
				return false;
			}
			if (!SteamUserStats.GetStat(pchStatName, out int pData))
			{
				return false;
			}
			int nData = pData + delta;
			if (!SteamUserStats.SetStat(pchStatName, nData))
			{
				return false;
			}
			return SteamUserStats.StoreStats();
		}

		public void RequestGlobalStats(int historyDays)
		{
			if (_steamModel.IsSteamInitialized)
			{
				_globalStatsReceivedCallResult?.Dispose();
				_globalStatsReceivedCallResult = CallResult<GlobalStatsReceived_t>.Create(OnGlobalStatsReceivedCallback);
				SteamAPICall_t hAPICall = SteamUserStats.RequestGlobalStats(historyDays);
				_globalStatsReceivedCallResult.Set(hAPICall);
			}
		}

		private void OnGlobalStatsReceivedCallback(GlobalStatsReceived_t callback, bool ioFailure)
		{
			if (!ioFailure && callback.m_eResult == EResult.k_EResultOK)
			{
				this.OnGlobalStatsReceived?.Invoke();
			}
		}
	}
}
