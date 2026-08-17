using System;
using System.Threading.Tasks;
using EvilAnalytics.SDK.Network;
using EvilAnalytics.Shared.Leaderboards;

namespace EvilAnalytics.SDK.Leaderboards
{
	public class LeaderboardManager
	{
		private readonly ApiClient _apiClient;

		private readonly string _deviceId;

		internal LeaderboardManager(ApiClient apiClient, string deviceId)
		{
			_apiClient = apiClient ?? throw new ArgumentNullException("apiClient");
			_deviceId = deviceId ?? throw new ArgumentNullException("deviceId");
		}

		public async Task<SubmitScoreResponse> SubmitScoreAsync(Guid leaderboardId, decimal score)
		{
			SubmitScoreRequest request = new SubmitScoreRequest
			{
				DeviceId = _deviceId,
				Score = score
			};
			return await _apiClient.SubmitLeaderboardScoreAsync(leaderboardId, request);
		}

		public async Task<RankingsResponse> GetRankingsAsync(Guid leaderboardId, string timeWindow = "AllTime", int limit = 10)
		{
			return await _apiClient.GetLeaderboardRankingsAsync(leaderboardId, timeWindow, limit);
		}

		public async Task<PlayerRankResponse> GetMyRankAsync(Guid leaderboardId, string timeWindow = "AllTime", int nearbyCount = 3)
		{
			return await _apiClient.GetMyLeaderboardRankAsync(leaderboardId, _deviceId, timeWindow, nearbyCount);
		}
	}
}
