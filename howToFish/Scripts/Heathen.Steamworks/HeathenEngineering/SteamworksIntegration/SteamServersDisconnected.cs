using Steamworks;

namespace HeathenEngineering.SteamworksIntegration
{
	public struct SteamServersDisconnected
	{
		public SteamServersDisconnected_t data;

		public EResult Result => data.m_eResult;

		public static implicit operator SteamServersDisconnected(SteamServersDisconnected_t native)
		{
			return new SteamServersDisconnected
			{
				data = native
			};
		}

		public static implicit operator SteamServersDisconnected_t(SteamServersDisconnected heathen)
		{
			return heathen.data;
		}
	}
}
