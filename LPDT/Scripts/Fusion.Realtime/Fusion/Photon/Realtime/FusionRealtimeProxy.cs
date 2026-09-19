#define DEBUG
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Photon.Realtime;
using UnityEngine;

namespace Fusion.Photon.Realtime
{
	internal static class FusionRealtimeProxy
	{
		private const float REGION_INFO_CACHE_TIME = 10f;

		private static float _lastRegionRequestTime;

		private static List<RegionInfo> _cachedRegionInfo;

		internal static async Task<List<RegionInfo>> GetEnabledRegions(string appId, CancellationToken cancellationToken)
		{
			AppSettings appSettings = new AppSettings();
			PhotonAppSettings global;
			if (appId != null)
			{
				appSettings.AppIdFusion = appId;
			}
			else if (PhotonAppSettings.TryGetGlobal(out global))
			{
				appSettings.AppIdFusion = global.AppSettings.AppIdFusion;
			}
			if (appSettings.AppIdFusion == null)
			{
				InternalLogStreams.LogDebug?.Warn("Could not get enabled regions. Provided App id is not valid.");
				return null;
			}
			if (Time.time <= _lastRegionRequestTime + 10f && _cachedRegionInfo != null)
			{
				return await Task.FromResult(_cachedRegionInfo);
			}
			RealtimeClient client = new RealtimeClient();
			RegionHandler regionHandler = await client.ConnectToNameserverAndWaitForRegionsAsync(appSettings);
			await client.DisconnectAsync();
			List<RegionInfo> list = new List<RegionInfo>();
			if (regionHandler == null)
			{
				return list;
			}
			foreach (Region region in regionHandler.EnabledRegions)
			{
				list.Add(new RegionInfo
				{
					RegionCode = region.Code,
					RegionPing = region.Ping
				});
			}
			_cachedRegionInfo = new List<RegionInfo>(list);
			_lastRegionRequestTime = Time.unscaledTime;
			return list;
		}
	}
}
