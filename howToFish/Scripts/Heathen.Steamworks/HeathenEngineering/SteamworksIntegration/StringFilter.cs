using System;
using Steamworks;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public struct StringFilter
	{
		public string key;

		public string value;

		public ELobbyComparison comparison;
	}
}
