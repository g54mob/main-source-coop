using System;
using System.Collections.Generic;
using System.Net.Http;
using Cysharp.Threading.Tasks;
using EvilCore.EvilSave;

namespace EvilCore.Networking
{
	public sealed class RegionService : IRegionService
	{
		private const string CacheKey = "network.region.code";

		private const string GeoIpUrl = "http://ip-api.com/json/?fields=status,continentCode,countryCode";

		private static readonly HttpClient Http = new HttpClient
		{
			Timeout = TimeSpan.FromSeconds(5.0)
		};

		private static readonly Dictionary<string, RegionCode> CountryOverrides = new Dictionary<string, RegionCode>
		{
			{
				"TR",
				RegionCode.Europe
			},
			{
				"CY",
				RegionCode.Europe
			},
			{
				"GE",
				RegionCode.Europe
			},
			{
				"AM",
				RegionCode.Europe
			},
			{
				"AZ",
				RegionCode.Europe
			},
			{
				"RU",
				RegionCode.Europe
			}
		};

		private static readonly int[,] PingMatrix = new int[6, 6]
		{
			{ -1, -1, -1, -1, -1, -1 },
			{ -1, 35, 140, 110, 160, 170 },
			{ -1, 140, 40, 230, 320, 290 },
			{ -1, 110, 230, 35, 180, 290 },
			{ -1, 160, 320, 180, 40, 140 },
			{ -1, 170, 290, 290, 140, 45 }
		};

		private RegionCode _localRegion;

		private bool _isResolved;

		private bool _resolving;

		private UniTask<RegionCode> _runningLookup;

		public RegionCode LocalRegion => _localRegion;

		public bool IsResolved => _isResolved;

		public event Action<RegionCode> OnRegionResolved;

		public RegionService()
		{
			LoadCache();
			ResolveLocalRegionAsync(forceRefresh: true).Forget();
		}

		public UniTask<RegionCode> ResolveLocalRegionAsync(bool forceRefresh = false)
		{
			if (_isResolved && !forceRefresh)
			{
				return UniTask.FromResult(_localRegion);
			}
			if (_resolving)
			{
				return _runningLookup;
			}
			_resolving = true;
			_runningLookup = RunLookupAsync();
			return _runningLookup;
		}

		public int EstimatePingMs(RegionCode hostRegion)
		{
			if (_localRegion == RegionCode.Unknown || hostRegion == RegionCode.Unknown)
			{
				return -1;
			}
			return PingMatrix[(uint)_localRegion, (uint)hostRegion];
		}

		public string ToAttribute(RegionCode region)
		{
			return region switch
			{
				RegionCode.NorthAmerica => "na", 
				RegionCode.SouthAmerica => "sa", 
				RegionCode.Europe => "eu", 
				RegionCode.Asia => "as", 
				RegionCode.Oceania => "oc", 
				_ => "unknown", 
			};
		}

		public RegionCode FromAttribute(string attribute)
		{
			return attribute switch
			{
				"na" => RegionCode.NorthAmerica, 
				"sa" => RegionCode.SouthAmerica, 
				"eu" => RegionCode.Europe, 
				"as" => RegionCode.Asia, 
				"oc" => RegionCode.Oceania, 
				_ => RegionCode.Unknown, 
			};
		}

		private void LoadCache()
		{
			RegionCode regionCode = (RegionCode)EvilCore.EvilSave.EvilSave.Prefs.GetInt("network.region.code");
			if (regionCode != RegionCode.Unknown)
			{
				_localRegion = regionCode;
				_isResolved = true;
			}
		}

		private async UniTask<RegionCode> RunLookupAsync()
		{
			_ = 1;
			try
			{
				RegionCode resolved = await FetchRegionAsync();
				await UniTask.SwitchToMainThread();
				if (resolved == RegionCode.Unknown)
				{
					return _localRegion;
				}
				bool num = resolved != _localRegion;
				_localRegion = resolved;
				_isResolved = true;
				EvilCore.EvilSave.EvilSave.Prefs.SetInt("network.region.code", (int)resolved);
				if (num)
				{
					this.OnRegionResolved?.Invoke(resolved);
				}
				return resolved;
			}
			finally
			{
				_resolving = false;
			}
		}

		private static async UniTask<RegionCode> FetchRegionAsync()
		{
			try
			{
				string json = await Http.GetStringAsync("http://ip-api.com/json/?fields=status,continentCode,countryCode").AsUniTask();
				if (ExtractJsonValue(json, "status") != "success")
				{
					return RegionCode.Unknown;
				}
				string text = ExtractJsonValue(json, "countryCode");
				if (!string.IsNullOrEmpty(text) && CountryOverrides.TryGetValue(text, out var value))
				{
					return value;
				}
				return ContinentToRegion(ExtractJsonValue(json, "continentCode"));
			}
			catch (Exception)
			{
				return RegionCode.Unknown;
			}
		}

		private static RegionCode ContinentToRegion(string continentCode)
		{
			return continentCode switch
			{
				"EU" => RegionCode.Europe, 
				"NA" => RegionCode.NorthAmerica, 
				"SA" => RegionCode.SouthAmerica, 
				"AS" => RegionCode.Asia, 
				"OC" => RegionCode.Oceania, 
				"AF" => RegionCode.Europe, 
				_ => RegionCode.Unknown, 
			};
		}

		private static string ExtractJsonValue(string json, string key)
		{
			if (string.IsNullOrEmpty(json))
			{
				return null;
			}
			string text = "\"" + key + "\":\"";
			int num = json.IndexOf(text, StringComparison.Ordinal);
			if (num < 0)
			{
				return null;
			}
			num += text.Length;
			int num2 = json.IndexOf('"', num);
			if (num2 >= 0)
			{
				return json.Substring(num, num2 - num);
			}
			return null;
		}
	}
}
