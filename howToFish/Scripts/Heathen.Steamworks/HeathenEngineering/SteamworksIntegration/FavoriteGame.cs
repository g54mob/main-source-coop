using System;
using HeathenEngineering.SteamworksIntegration.API;
using Steamworks;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public struct FavoriteGame
	{
		public AppId_t appId;

		public uint ipAddress;

		public ushort connectionPort;

		public ushort queryPort;

		public DateTime lastPlayedOnServer;

		public bool isHistory;

		public string IpAddress
		{
			get
			{
				return Utilities.IPUintToString(ipAddress);
			}
			set
			{
				ipAddress = Utilities.IPStringToUint(value);
			}
		}
	}
}
