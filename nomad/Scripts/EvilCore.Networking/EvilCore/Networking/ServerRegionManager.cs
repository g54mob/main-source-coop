using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Extensions;
using UnityEngine;
using VContainer;

namespace EvilCore.Networking
{
	public class ServerRegionManager : MonoBehaviour
	{
		[Serializable]
		public class RegionServer
		{
			public string regionName;

			public string serverAddress;

			public int estimatedPing;

			public bool isActive;
		}

		[Header("Regional Servers")]
		public List<RegionServer> regionServers = new List<RegionServer>();

		[Header("Auto-Select Best Region")]
		public bool autoSelectBestRegion = true;

		public int pingTestTimeout = 3000;

		[Inject]
		private INetworkManager _networkManager;

		[Inject]
		private IEOSLobbyManager _lobbyManager;

		private RegionServer bestRegion;

		private void Awake()
		{
			base.gameObject.InjectGameObject();
		}

		private void Start()
		{
			if (autoSelectBestRegion)
			{
				FindBestRegionAsync();
			}
		}

		public async Task<RegionServer> FindBestRegionAsync()
		{
			RegionServer fastest = null;
			int lowestPing = 2147483647;
			foreach (RegionServer region in regionServers)
			{
				if (region.isActive)
				{
					int num = await PingServerAsync(region.serverAddress);
					if (num < lowestPing && num > 0)
					{
						lowestPing = num;
						fastest = region;
					}
				}
			}
			if (fastest != null)
			{
				bestRegion = fastest;
				if (_networkManager != null)
				{
					_networkManager.NetworkAddress = bestRegion.serverAddress;
				}
			}
			return bestRegion;
		}

		private async Task<int> PingServerAsync(string address)
		{
			try
			{
				PingReply pingReply = await new Ping().SendPingAsync(address, pingTestTimeout);
				return (int)((pingReply.Status == IPStatus.Success) ? pingReply.RoundtripTime : (-1));
			}
			catch
			{
				return -1;
			}
		}

		public void ConnectToBestRegion()
		{
			if (bestRegion != null)
			{
				if (_networkManager != null)
				{
					_networkManager.NetworkAddress = bestRegion.serverAddress;
					_networkManager.StartClient();
				}
			}
			else
			{
				EvilLogger.LogError("No best region found! Falling back to EOS P2P...", "ConnectToBestRegion", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\_Core\\ServerRegionManager.cs", 113);
				_lobbyManager?.CreateGameWithLobby();
			}
		}

		[ContextMenu("Test All Regions")]
		public void TestAllRegions()
		{
			FindBestRegionAsync();
		}

		public int GetCurrentPing()
		{
			return bestRegion?.estimatedPing ?? (-1);
		}

		public string GetCurrentRegion()
		{
			return bestRegion?.regionName ?? "EOS P2P";
		}
	}
}
