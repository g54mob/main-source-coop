using Steamworks;

namespace HeathenEngineering.SteamworksIntegration
{
	public struct SteamServerConnectFailure
	{
		public SteamServerConnectFailure_t data;

		public EResult Result => data.m_eResult;

		public bool Retrying => data.m_bStillRetrying;

		public static implicit operator SteamServerConnectFailure(SteamServerConnectFailure_t native)
		{
			return new SteamServerConnectFailure
			{
				data = native
			};
		}

		public static implicit operator SteamServerConnectFailure_t(SteamServerConnectFailure heathen)
		{
			return heathen.data;
		}
	}
}
