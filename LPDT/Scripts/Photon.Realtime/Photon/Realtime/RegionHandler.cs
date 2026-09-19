using System;
using System.Collections.Generic;
using System.Text;
using Photon.Client;

namespace Photon.Realtime
{
	public class RegionHandler
	{
		public static Type PingImplementation;

		private Region bestRegionCache;

		private readonly List<RegionPinger> pingerList = new List<RegionPinger>();

		private Action<RegionHandler> onCompleteCall;

		private int previousPing;

		private string previousSummaryProvided;

		private float rePingFactor = 1.2f;

		private float pingSimilarityFactor = 1.2f;

		public int BestRegionSummaryPingLimit = 90;

		protected internal static ushort UdpPortToPing;

		private MonoBehaviourEmpty emptyMonoBehavior;

		public List<Region> EnabledRegions { get; protected internal set; }

		public string AvailableRegionCodes { get; private set; }

		public Region BestRegion
		{
			get
			{
				if (EnabledRegions == null)
				{
					return null;
				}
				if (bestRegionCache != null)
				{
					return bestRegionCache;
				}
				EnabledRegions.Sort((Region a, Region b) => a.Ping.CompareTo(b.Ping));
				int num = (int)((float)EnabledRegions[0].Ping * pingSimilarityFactor);
				Region region = EnabledRegions[0];
				foreach (Region enabledRegion in EnabledRegions)
				{
					if (enabledRegion.Ping <= num && enabledRegion.Code.CompareTo(region.Code) < 0)
					{
						region = enabledRegion;
					}
				}
				bestRegionCache = region;
				return bestRegionCache;
			}
		}

		public string SummaryToCache
		{
			get
			{
				if (BestRegion != null && BestRegion.Ping < RegionPinger.MaxMillisecondsPerPing)
				{
					return $"{BestRegion.Code};{BestRegion.Ping};{AvailableRegionCodes}";
				}
				return AvailableRegionCodes;
			}
		}

		public bool IsPinging { get; private set; }

		public bool Aborted { get; private set; }

		public string GetResults()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Summary: {0}\n", SummaryToCache);
			foreach (RegionPinger pinger in pingerList)
			{
				stringBuilder.AppendLine(pinger.GetResults());
			}
			string arg = previousSummaryProvided ?? "N/A";
			stringBuilder.AppendFormat("Previous summary: {0}", arg);
			return stringBuilder.ToString();
		}

		public void SetRegions(OperationResponse opGetRegions, RealtimeClient client = null)
		{
			if (opGetRegions.OperationCode != 220 || opGetRegions.ReturnCode != 0)
			{
				return;
			}
			string[] array = opGetRegions[210] as string[];
			string[] array2 = opGetRegions[230] as string[];
			if (array == null || array2 == null || array.Length != array2.Length)
			{
				if (client != null)
				{
					Log.Error("RegionHandler.SetRegions() failed. Received regions and servers must be non null and of equal length. Could not read regions.", client.LogLevel, client.LogPrefix);
				}
				return;
			}
			bestRegionCache = null;
			EnabledRegions = new List<Region>(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				string text = array2[i];
				if (client != null && client.AddressRewriter != null)
				{
					text = client.AddressRewriter(text, ServerConnection.MasterServer);
				}
				Region region = new Region(array[i], text);
				if (!string.IsNullOrEmpty(region.Code))
				{
					EnabledRegions.Add(region);
				}
			}
			Array.Sort(array);
			AvailableRegionCodes = string.Join(",", array);
		}

		public RegionHandler(ushort masterServerUdpPort = 0)
		{
			UdpPortToPing = masterServerUdpPort;
		}

		public bool PingAvailableRegions(Action<RegionHandler> onCompleteCallback)
		{
			return PingMinimumOfRegions(onCompleteCallback, null);
		}

		public bool PingMinimumOfRegions(Action<RegionHandler> onCompleteCallback, string previousSummary)
		{
			if (EnabledRegions == null || EnabledRegions.Count == 0)
			{
				return false;
			}
			if (IsPinging)
			{
				return false;
			}
			Aborted = false;
			IsPinging = true;
			previousSummaryProvided = previousSummary;
			if (emptyMonoBehavior != null)
			{
				emptyMonoBehavior.SelfDestroy();
			}
			emptyMonoBehavior = MonoBehaviourEmpty.BuildInstance("RegionHandler");
			emptyMonoBehavior.onCompleteCall = onCompleteCallback;
			onCompleteCall = emptyMonoBehavior.CompleteOnMainThread;
			if (string.IsNullOrEmpty(previousSummary))
			{
				return PingEnabledRegions();
			}
			string[] array = previousSummary.Split(';');
			if (array.Length < 3)
			{
				return PingEnabledRegions();
			}
			if (!int.TryParse(array[1], out var result))
			{
				return PingEnabledRegions();
			}
			string prevBestRegionCode = array[0];
			string value = array[2];
			if (string.IsNullOrEmpty(prevBestRegionCode))
			{
				return PingEnabledRegions();
			}
			if (string.IsNullOrEmpty(value))
			{
				return PingEnabledRegions();
			}
			if (!AvailableRegionCodes.Equals(value) || !AvailableRegionCodes.Contains(prevBestRegionCode))
			{
				return PingEnabledRegions();
			}
			if (result >= RegionPinger.PingWhenFailed)
			{
				return PingEnabledRegions();
			}
			previousPing = result;
			RegionPinger regionPinger = new RegionPinger(EnabledRegions.Find((Region r) => r.Code.Equals(prevBestRegionCode)), OnPreferredRegionPinged);
			lock (pingerList)
			{
				pingerList.Clear();
				pingerList.Add(regionPinger);
			}
			regionPinger.Start();
			return true;
		}

		public void Abort()
		{
			if (Aborted)
			{
				return;
			}
			Aborted = true;
			lock (pingerList)
			{
				foreach (RegionPinger pinger in pingerList)
				{
					pinger.Abort();
				}
			}
			if (emptyMonoBehavior != null)
			{
				emptyMonoBehavior.SelfDestroy();
			}
		}

		private void OnPreferredRegionPinged(Region preferredRegion)
		{
			if (preferredRegion.Ping > BestRegionSummaryPingLimit || (float)preferredRegion.Ping > (float)previousPing * rePingFactor)
			{
				PingEnabledRegions();
				return;
			}
			IsPinging = false;
			onCompleteCall(this);
		}

		private bool PingEnabledRegions()
		{
			if (EnabledRegions == null || EnabledRegions.Count == 0)
			{
				return false;
			}
			lock (pingerList)
			{
				pingerList.Clear();
				foreach (Region enabledRegion in EnabledRegions)
				{
					RegionPinger regionPinger = new RegionPinger(enabledRegion, OnRegionDone);
					if (regionPinger.Start())
					{
						pingerList.Add(regionPinger);
					}
				}
			}
			return true;
		}

		private void OnRegionDone(Region region)
		{
			lock (pingerList)
			{
				if (!IsPinging)
				{
					return;
				}
				bestRegionCache = null;
				foreach (RegionPinger pinger in pingerList)
				{
					if (!pinger.Done)
					{
						return;
					}
				}
				IsPinging = false;
			}
			if (!Aborted)
			{
				onCompleteCall(this);
			}
		}
	}
}
