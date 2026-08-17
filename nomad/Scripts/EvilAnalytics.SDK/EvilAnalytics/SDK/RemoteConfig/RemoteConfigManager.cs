using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using EvilAnalytics.SDK.Network;
using EvilAnalytics.Shared.RemoteConfig;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EvilAnalytics.SDK.RemoteConfig
{
	public class RemoteConfigManager
	{
		private readonly object _lock = new object();

		private readonly ApiClient _apiClient;

		private Dictionary<string, object> _defaults = new Dictionary<string, object>();

		private Dictionary<string, object> _fetchedConfigs = new Dictionary<string, object>();

		private int _configVersion;

		private DateTimeOffset _lastFetchTime = DateTimeOffset.MinValue;

		private bool _hasFetched;

		public int ConfigVersion
		{
			get
			{
				lock (_lock)
				{
					return _configVersion;
				}
			}
		}

		public DateTimeOffset LastFetchTime
		{
			get
			{
				lock (_lock)
				{
					return _lastFetchTime;
				}
			}
		}

		public bool HasFetched
		{
			get
			{
				lock (_lock)
				{
					return _hasFetched;
				}
			}
		}

		internal RemoteConfigManager(ApiClient apiClient)
		{
			_apiClient = apiClient ?? throw new ArgumentNullException("apiClient");
		}

		public void SetDefaults(Dictionary<string, object> defaults)
		{
			if (defaults == null)
			{
				return;
			}
			lock (_lock)
			{
				foreach (KeyValuePair<string, object> @default in defaults)
				{
					_defaults[@default.Key] = @default.Value;
				}
			}
		}

		public async Task<bool> FetchAsync()
		{
			try
			{
				LiveConfigResponse liveConfigResponse = await _apiClient.GetRemoteConfigAsync();
				if (liveConfigResponse == null || liveConfigResponse.Configs == null)
				{
					return false;
				}
				lock (_lock)
				{
					_fetchedConfigs = liveConfigResponse.Configs;
					_configVersion = liveConfigResponse.ConfigVersion;
					_lastFetchTime = liveConfigResponse.FetchedAt;
					_hasFetched = true;
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

		public string GetString(string key, string defaultValue = "")
		{
			object value = GetValue(key);
			if (value == null)
			{
				return defaultValue;
			}
			if (value is string result)
			{
				return result;
			}
			return value.ToString();
		}

		public int GetInt(string key, int defaultValue = 0)
		{
			object value = GetValue(key);
			if (value == null)
			{
				return defaultValue;
			}
			if (value is int)
			{
				return (int)value;
			}
			if (value is long num)
			{
				return (int)num;
			}
			if (value is double num2)
			{
				return (int)num2;
			}
			if (value is decimal num3)
			{
				return (int)num3;
			}
			if (int.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
			{
				return result;
			}
			return defaultValue;
		}

		public long GetLong(string key, long defaultValue = 0L)
		{
			object value = GetValue(key);
			if (value == null)
			{
				return defaultValue;
			}
			if (value is long)
			{
				return (long)value;
			}
			if (value is int num)
			{
				return num;
			}
			if (value is double num2)
			{
				return (long)num2;
			}
			if (value is decimal num3)
			{
				return (long)num3;
			}
			if (long.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
			{
				return result;
			}
			return defaultValue;
		}

		public float GetFloat(string key, float defaultValue = 0f)
		{
			object value = GetValue(key);
			if (value == null)
			{
				return defaultValue;
			}
			if (value is float)
			{
				return (float)value;
			}
			if (value is double num)
			{
				return (float)num;
			}
			if (value is decimal num2)
			{
				return (float)num2;
			}
			if (value is int num3)
			{
				return num3;
			}
			if (value is long num4)
			{
				return num4;
			}
			if (float.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
			{
				return result;
			}
			return defaultValue;
		}

		public double GetDouble(string key, double defaultValue = 0.0)
		{
			object value = GetValue(key);
			if (value == null)
			{
				return defaultValue;
			}
			if (value is double)
			{
				return (double)value;
			}
			if (value is float num)
			{
				return num;
			}
			if (value is decimal num2)
			{
				return (double)num2;
			}
			if (value is int num3)
			{
				return num3;
			}
			if (value is long num4)
			{
				return num4;
			}
			if (double.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
			{
				return result;
			}
			return defaultValue;
		}

		public bool GetBool(string key, bool defaultValue = false)
		{
			object value = GetValue(key);
			if (value == null)
			{
				return defaultValue;
			}
			if (value is bool)
			{
				return (bool)value;
			}
			switch (value.ToString().ToLowerInvariant())
			{
			case "true":
			case "1":
			case "yes":
				return true;
			case "false":
			case "0":
			case "no":
				return false;
			default:
				return defaultValue;
			}
		}

		public T GetJson<T>(string key, T defaultValue = default(T))
		{
			object value = GetValue(key);
			if (value == null)
			{
				return defaultValue;
			}
			try
			{
				if (value is T result)
				{
					return result;
				}
				if (value is JToken jToken)
				{
					return jToken.ToObject<T>();
				}
				if (value is string value2)
				{
					return JsonConvert.DeserializeObject<T>(value2);
				}
				return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(value));
			}
			catch
			{
				return defaultValue;
			}
		}

		public bool HasKey(string key)
		{
			lock (_lock)
			{
				return _fetchedConfigs.ContainsKey(key) || _defaults.ContainsKey(key);
			}
		}

		public HashSet<string> GetAllKeys()
		{
			lock (_lock)
			{
				HashSet<string> hashSet = new HashSet<string>();
				foreach (string key in _defaults.Keys)
				{
					hashSet.Add(key);
				}
				foreach (string key2 in _fetchedConfigs.Keys)
				{
					hashSet.Add(key2);
				}
				return hashSet;
			}
		}

		public void Clear()
		{
			lock (_lock)
			{
				_defaults.Clear();
				_fetchedConfigs.Clear();
				_configVersion = 0;
				_lastFetchTime = DateTimeOffset.MinValue;
				_hasFetched = false;
			}
		}

		private object GetValue(string key)
		{
			lock (_lock)
			{
				if (_fetchedConfigs.TryGetValue(key, out var value))
				{
					return value;
				}
				if (_defaults.TryGetValue(key, out var value2))
				{
					return value2;
				}
				return null;
			}
		}
	}
}
