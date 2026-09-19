using System;
using System.Collections.Generic;
using GameAnalyticsSDK.Net.Device;
using GameAnalyticsSDK.Net.Events;
using GameAnalyticsSDK.Net.Http;
using GameAnalyticsSDK.Net.Logging;
using GameAnalyticsSDK.Net.Store;
using GameAnalyticsSDK.Net.Threading;
using GameAnalyticsSDK.Net.Utilities;
using GameAnalyticsSDK.Net.Validators;

namespace GameAnalyticsSDK.Net.State
{
	internal class GAState
	{
		private const string CategorySdkError = "sdk_error";

		private const int MaxCustomFieldsCount = 50;

		private const int MaxCustomFieldsKeyLength = 64;

		private const int MaxCustomFieldsValueStringLength = 256;

		private const int MaxErrorCount = 10;

		private static Dictionary<string, int> countMap = new Dictionary<string, int>();

		private static Dictionary<string, DateTime> timestampMap = new Dictionary<string, DateTime>();

		private static readonly GAState _instance = new GAState();

		private string _userId;

		private string _identifier;

		private bool _initialized;

		private long _sessionStart;

		private int _sessionNum;

		private int _transactionNum;

		private string _sessionId;

		private string _currentCustomDimension01;

		private string _currentCustomDimension02;

		private string _currentCustomDimension03;

		private IDictionary<string, object> _currentGlobalCustomEventFields = new Dictionary<string, object>();

		private string _gameKey;

		private string _gameSecret;

		private string[] _availableCustomDimensions01 = new string[0];

		private string[] _availableCustomDimensions02 = new string[0];

		private string[] _availableCustomDimensions03 = new string[0];

		private string[] _availableResourceCurrencies = new string[0];

		private string[] _availableResourceItemTypes = new string[0];

		private string _build;

		private bool _useManualSessionHandling;

		private bool _isEventSubmissionEnabled = true;

		private string _defaultUserId;

		private Dictionary<string, int> progressionTries = new Dictionary<string, int>();

		private JSONNode sdkConfigDefault = new JSONObject();

		private JSONNode sdkConfig = new JSONObject();

		private JSONNode sdkConfigCached = new JSONObject();

		private JSONNode remoteConfigs = new JSONObject();

		private bool remoteConfigsIsReady;

		private readonly List<IRemoteConfigsListener> remoteConfigsListeners = new List<IRemoteConfigsListener>();

		private readonly object remoteConfigsLock = new object();

		public const string InMemoryPrefix = "in_memory_";

		private const string DefaultUserIdKey = "default_user_id";

		public const string SessionNumKey = "session_num";

		public const string TransactionNumKey = "transaction_num";

		private const string Dimension01Key = "dimension01";

		private const string Dimension02Key = "dimension02";

		private const string Dimension03Key = "dimension03";

		private const string SdkConfigCachedKey = "sdk_config_cached";

		private static GAState Instance => _instance;

		public static string UserId
		{
			private get
			{
				return Instance._userId;
			}
			set
			{
				Instance._userId = ((value == null) ? "" : value);
				CacheIdentifier();
			}
		}

		public static string Identifier
		{
			get
			{
				return Instance._identifier;
			}
			private set
			{
				Instance._identifier = value;
			}
		}

		public static bool Initialized
		{
			get
			{
				return Instance._initialized;
			}
			private set
			{
				Instance._initialized = value;
			}
		}

		public static long SessionStart
		{
			get
			{
				return Instance._sessionStart;
			}
			private set
			{
				Instance._sessionStart = value;
			}
		}

		public static int SessionNum
		{
			get
			{
				return Instance._sessionNum;
			}
			private set
			{
				Instance._sessionNum = value;
			}
		}

		public static int TransactionNum
		{
			get
			{
				return Instance._transactionNum;
			}
			private set
			{
				Instance._transactionNum = value;
			}
		}

		public static string SessionId
		{
			get
			{
				return Instance._sessionId;
			}
			private set
			{
				Instance._sessionId = value;
			}
		}

		public static string CurrentCustomDimension01
		{
			get
			{
				return Instance._currentCustomDimension01;
			}
			private set
			{
				Instance._currentCustomDimension01 = value;
			}
		}

		public static string CurrentCustomDimension02
		{
			get
			{
				return Instance._currentCustomDimension02;
			}
			private set
			{
				Instance._currentCustomDimension02 = value;
			}
		}

		public static string CurrentCustomDimension03
		{
			get
			{
				return Instance._currentCustomDimension03;
			}
			private set
			{
				Instance._currentCustomDimension03 = value;
			}
		}

		public static IDictionary<string, object> CurrentGlobalCustomEventFields => Instance._currentGlobalCustomEventFields;

		public static string GameKey
		{
			get
			{
				return Instance._gameKey;
			}
			private set
			{
				Instance._gameKey = value;
			}
		}

		public static string GameSecret
		{
			get
			{
				return Instance._gameSecret;
			}
			private set
			{
				Instance._gameSecret = value;
			}
		}

		public static string[] AvailableCustomDimensions01
		{
			get
			{
				return Instance._availableCustomDimensions01;
			}
			set
			{
				if (GAValidator.ValidateCustomDimensions(value))
				{
					Instance._availableCustomDimensions01 = value;
					ValidateAndFixCurrentDimensions();
					GALogger.I("Set available custom01 dimension values: (" + GAUtilities.JoinStringArray(value, ", ") + ")");
				}
			}
		}

		public static string[] AvailableCustomDimensions02
		{
			get
			{
				return Instance._availableCustomDimensions02;
			}
			set
			{
				if (GAValidator.ValidateCustomDimensions(value))
				{
					Instance._availableCustomDimensions02 = value;
					ValidateAndFixCurrentDimensions();
					GALogger.I("Set available custom02 dimension values: (" + GAUtilities.JoinStringArray(value, ", ") + ")");
				}
			}
		}

		public static string[] AvailableCustomDimensions03
		{
			get
			{
				return Instance._availableCustomDimensions03;
			}
			set
			{
				if (GAValidator.ValidateCustomDimensions(value))
				{
					Instance._availableCustomDimensions03 = value;
					ValidateAndFixCurrentDimensions();
					GALogger.I("Set available custom03 dimension values: (" + GAUtilities.JoinStringArray(value, ", ") + ")");
				}
			}
		}

		public static string[] AvailableResourceCurrencies
		{
			get
			{
				return Instance._availableResourceCurrencies;
			}
			set
			{
				if (GAValidator.ValidateResourceCurrencies(value))
				{
					Instance._availableResourceCurrencies = value;
					GALogger.I("Set available resource currencies: (" + GAUtilities.JoinStringArray(value, ", ") + ")");
				}
			}
		}

		public static string[] AvailableResourceItemTypes
		{
			get
			{
				return Instance._availableResourceItemTypes;
			}
			set
			{
				if (GAValidator.ValidateResourceItemTypes(value))
				{
					Instance._availableResourceItemTypes = value;
					GALogger.I("Set available resource item types: (" + GAUtilities.JoinStringArray(value, ", ") + ")");
				}
			}
		}

		public static string Build
		{
			get
			{
				return Instance._build;
			}
			set
			{
				Instance._build = value;
			}
		}

		public static bool UseManualSessionHandling
		{
			get
			{
				return Instance._useManualSessionHandling;
			}
			private set
			{
				Instance._useManualSessionHandling = value;
			}
		}

		public static bool IsEventSubmissionEnabled
		{
			get
			{
				return Instance._isEventSubmissionEnabled;
			}
			private set
			{
				Instance._isEventSubmissionEnabled = value;
			}
		}

		private bool Enabled { get; set; }

		private JSONNode SdkConfigCached { get; set; }

		private bool InitAuthorized { get; set; }

		private long ClientServerTimeOffset { get; set; }

		private long SuspendBlockId { get; set; }

		public string ConfigsHash { get; set; }

		public string AbId { get; set; }

		public string AbVariantId { get; set; }

		private string DefaultUserId
		{
			get
			{
				return Instance._defaultUserId;
			}
			set
			{
				Instance._defaultUserId = ((value == null) ? "" : value);
				CacheIdentifier();
			}
		}

		private static JSONNode SdkConfig
		{
			get
			{
				if (Instance.sdkConfig.AsObject != null && Instance.sdkConfig.Count != 0)
				{
					return Instance.sdkConfig;
				}
				if (Instance.sdkConfigCached.AsObject != null && Instance.sdkConfigCached.Count != 0)
				{
					return Instance.sdkConfigCached;
				}
				return Instance.sdkConfigDefault;
			}
		}

		private GAState()
		{
			Enabled = false;
		}

		~GAState()
		{
			EndSessionAndStopQueue(endThread: false);
		}

		public static bool IsEnabled()
		{
			return Instance.Enabled;
		}

		public static void SetCustomDimension01(string dimension)
		{
			CurrentCustomDimension01 = dimension;
			if (GAStore.IsTableReady)
			{
				GAStore.SetState("dimension01", dimension);
			}
			GALogger.I("Set custom01 dimension value: " + dimension);
		}

		public static void SetCustomDimension02(string dimension)
		{
			CurrentCustomDimension02 = dimension;
			if (GAStore.IsTableReady)
			{
				GAStore.SetState("dimension02", dimension);
			}
			GALogger.I("Set custom02 dimension value: " + dimension);
		}

		public static void SetCustomDimension03(string dimension)
		{
			CurrentCustomDimension03 = dimension;
			if (GAStore.IsTableReady)
			{
				GAStore.SetState("dimension03", dimension);
			}
			GALogger.I("Set custom03 dimension value: " + dimension);
		}

		public static void SetGlobalCustomEventFields(IDictionary<string, object> customFields)
		{
			if (customFields == null || customFields.Count == 0)
			{
				CurrentGlobalCustomEventFields.Clear();
				return;
			}
			CurrentGlobalCustomEventFields.Clear();
			foreach (KeyValuePair<string, object> customField in customFields)
			{
				CurrentGlobalCustomEventFields.Add(customField);
			}
			GALogger.I("Set global custom event fields");
		}

		public static void IncrementSessionNum()
		{
			SessionNum++;
		}

		public static void IncrementTransactionNum()
		{
			TransactionNum++;
		}

		public static void IncrementProgressionTries(string progression)
		{
			int num = GetProgressionTries(progression) + 1;
			Instance.progressionTries[progression] = num;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("$progression", progression);
			dictionary.Add("$tries", num);
			GAStore.ExecuteQuerySync("INSERT OR REPLACE INTO ga_progression (progression, tries) VALUES($progression, $tries);", dictionary);
		}

		public static int GetProgressionTries(string progression)
		{
			if (Instance.progressionTries.ContainsKey(progression))
			{
				return Instance.progressionTries[progression];
			}
			return 0;
		}

		public static void ClearProgressionTries(string progression)
		{
			Dictionary<string, int> dictionary = Instance.progressionTries;
			if (dictionary.ContainsKey(progression))
			{
				dictionary.Remove(progression);
			}
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2.Add("$progression", progression);
			GAStore.ExecuteQuerySync("DELETE FROM ga_progression WHERE progression = $progression;", dictionary2);
		}

		public static bool HasAvailableCustomDimensions01(string dimension1)
		{
			return GAUtilities.StringArrayContainsString(AvailableCustomDimensions01, dimension1);
		}

		public static bool HasAvailableCustomDimensions02(string dimension2)
		{
			return GAUtilities.StringArrayContainsString(AvailableCustomDimensions02, dimension2);
		}

		public static bool HasAvailableCustomDimensions03(string dimension3)
		{
			return GAUtilities.StringArrayContainsString(AvailableCustomDimensions03, dimension3);
		}

		public static bool HasAvailableResourceCurrency(string currency)
		{
			return GAUtilities.StringArrayContainsString(AvailableResourceCurrencies, currency);
		}

		public static bool HasAvailableResourceItemType(string itemType)
		{
			return GAUtilities.StringArrayContainsString(AvailableResourceItemTypes, itemType);
		}

		public static void SetKeys(string gameKey, string gameSecret)
		{
			GameKey = gameKey;
			GameSecret = gameSecret;
		}

		public static void SetManualSessionHandling(bool flag)
		{
			UseManualSessionHandling = flag;
			GALogger.I("Use manual session handling: " + flag);
		}

		public static void SetEnabledEventSubmission(bool flag)
		{
			IsEventSubmissionEnabled = flag;
		}

		public static void InternalInitialize()
		{
			if (GAStore.IsTableReady)
			{
				EnsurePersistedStates();
				GAStore.SetState("default_user_id", Instance.DefaultUserId);
				Initialized = true;
				StartNewSession();
				if (IsEnabled())
				{
					GAEvents.EnsureEventQueueIsRunning();
				}
			}
		}

		public static void EndSessionAndStopQueue(bool endThread)
		{
			if (Initialized && IsEnabled() && SessionIsStarted())
			{
				GALogger.I("Ending session.");
				GAEvents.StopEventQueue();
				GAEvents.AddSessionEndEvent();
				SessionStart = 0L;
			}
			if (endThread)
			{
				GAThreading.StopThread();
			}
		}

		public static void ResumeSessionAndStartQueue()
		{
			if (Initialized)
			{
				GALogger.I("Resuming session.");
				if (!SessionIsStarted())
				{
					StartNewSession();
				}
			}
		}

		public static JSONObject GetEventAnnotations()
		{
			JSONObject jSONObject = new JSONObject();
			jSONObject.Add("v", new JSONNumber(2.0));
			jSONObject["event_uuid"] = Guid.NewGuid().ToString().ToLowerInvariant();
			jSONObject["user_id"] = Identifier;
			jSONObject.Add("client_ts", new JSONNumber(GetClientTsAdjusted()));
			jSONObject["sdk_version"] = GADevice.RelevantSdkVersion;
			jSONObject["os_version"] = GADevice.OSVersion;
			jSONObject["manufacturer"] = GADevice.DeviceManufacturer;
			jSONObject["device"] = GADevice.DeviceModel;
			jSONObject["platform"] = GADevice.BuildPlatform;
			jSONObject["session_id"] = SessionId;
			jSONObject.Add("session_num", new JSONNumber(SessionNum));
			string connectionType = GADevice.ConnectionType;
			if (GAValidator.ValidateConnectionType(connectionType))
			{
				jSONObject["connection_type"] = connectionType;
			}
			if (!string.IsNullOrEmpty(GADevice.GameEngineVersion))
			{
				jSONObject["engine_version"] = GADevice.GameEngineVersion;
			}
			if (Instance.remoteConfigs != null && Instance.remoteConfigs.Count > 0)
			{
				jSONObject["configurations"] = Instance.remoteConfigs;
			}
			if (!string.IsNullOrEmpty(Instance.AbId))
			{
				jSONObject["ab_id"] = Instance.AbId;
			}
			if (!string.IsNullOrEmpty(Instance.AbVariantId))
			{
				jSONObject["ab_variant_id"] = Instance.AbVariantId;
			}
			if (!string.IsNullOrEmpty(Build))
			{
				jSONObject["build"] = Build;
			}
			return jSONObject;
		}

		public static JSONObject GetSdkErrorEventAnnotations()
		{
			JSONObject jSONObject = new JSONObject();
			jSONObject.Add("v", new JSONNumber(2.0));
			jSONObject["event_uuid"] = Guid.NewGuid().ToString().ToLowerInvariant();
			jSONObject["category"] = "sdk_error";
			jSONObject["sdk_version"] = GADevice.RelevantSdkVersion;
			jSONObject["os_version"] = GADevice.OSVersion;
			jSONObject["manufacturer"] = GADevice.DeviceManufacturer;
			jSONObject["device"] = GADevice.DeviceModel;
			jSONObject["platform"] = GADevice.BuildPlatform;
			string connectionType = GADevice.ConnectionType;
			if (GAValidator.ValidateConnectionType(connectionType))
			{
				jSONObject["connection_type"] = connectionType;
			}
			if (!string.IsNullOrEmpty(GADevice.GameEngineVersion))
			{
				jSONObject["engine_version"] = GADevice.GameEngineVersion;
			}
			return jSONObject;
		}

		public static JSONObject GetInitAnnotations()
		{
			JSONObject jSONObject = new JSONObject();
			if (string.IsNullOrEmpty(Identifier))
			{
				CacheIdentifier();
			}
			GAStore.SetState("last_used_identifier", Identifier);
			jSONObject["user_id"] = Identifier;
			jSONObject["sdk_version"] = GADevice.RelevantSdkVersion;
			jSONObject["os_version"] = GADevice.OSVersion;
			jSONObject["platform"] = GADevice.BuildPlatform;
			if (!string.IsNullOrEmpty(Build))
			{
				jSONObject["build"] = Build;
			}
			else
			{
				jSONObject["build"] = null;
			}
			jSONObject["session_num"] = SessionNum;
			jSONObject["random_salt"] = SessionNum;
			return jSONObject;
		}

		public static long GetClientTsAdjusted()
		{
			long num = GAUtilities.TimeIntervalSince1970();
			long num2 = num + Instance.ClientServerTimeOffset;
			if (GAValidator.ValidateClientTs(num2))
			{
				return num2;
			}
			return num;
		}

		public static bool SessionIsStarted()
		{
			return SessionStart != 0;
		}

		private static void AddErrorEvent(string baseMessage, EGAErrorSeverity severity, string message)
		{
			if (!IsEventSubmissionEnabled)
			{
				return;
			}
			DateTime now = DateTime.Now;
			if (!timestampMap.ContainsKey(baseMessage))
			{
				timestampMap.Add(baseMessage, now);
			}
			if (!countMap.ContainsKey(baseMessage))
			{
				countMap.Add(baseMessage, 0);
			}
			if ((int)(now - timestampMap[baseMessage]).TotalMinutes >= 60)
			{
				countMap[baseMessage] = 0;
				timestampMap[baseMessage] = now;
			}
			if (countMap[baseMessage] < 10)
			{
				GAThreading.PerformTaskOnGAThread("addErrorEvent", delegate
				{
					GAEvents.AddErrorEvent(severity, message, null, mergeFields: false, skipAddingFields: true);
					countMap[baseMessage] += 1;
				});
			}
		}

		public static JSONObject ValidateAndCleanCustomFields(IDictionary<string, object> fields)
		{
			JSONObject jSONObject = new JSONObject();
			if (fields != null)
			{
				int num = 0;
				foreach (KeyValuePair<string, object> field in fields)
				{
					if (field.Key == null || field.Value == null)
					{
						string text = $"ValidateAndCleanCustomFields: entry with key={field.Key}, value={field.Value} has been omitted because its key or value is null";
						GALogger.W(text);
						AddErrorEvent("ValidateAndCleanCustomFields: entry with key={0}, value={1} has been omitted because its key or value is null", EGAErrorSeverity.Warning, text);
					}
					else if (num < 50)
					{
						if (GAUtilities.StringMatch(field.Key, "^[a-zA-Z0-9_]{1," + 64 + "}$"))
						{
							if (field.Value is string || field.Value is char)
							{
								string text2 = Convert.ToString(field.Value);
								if (text2.Length <= 256 && text2.Length > 0)
								{
									jSONObject[field.Key] = text2;
									num++;
								}
								else
								{
									string text3 = $"ValidateAndCleanCustomFields: entry with key={field.Key}, value={field.Value} has been omitted because its value is an empty string or exceeds the max number of characters ({256})";
									GALogger.W(text3);
									AddErrorEvent("ValidateAndCleanCustomFields: entry with key={0}, value={1} has been omitted because its value is an empty string or exceeds the max number of characters ({2})", EGAErrorSeverity.Warning, text3);
								}
							}
							else if (field.Value is double)
							{
								jSONObject[field.Key] = new JSONNumber((double)field.Value);
								num++;
							}
							else if (field.Value is float)
							{
								jSONObject[field.Key] = new JSONNumber((float)field.Value);
								num++;
							}
							else if (field.Value is long || field.Value is ulong)
							{
								jSONObject[field.Key] = new JSONNumber(Convert.ToInt64(field.Value));
								num++;
							}
							else if (field.Value is int || field.Value is byte || field.Value is sbyte || field.Value is byte || field.Value is uint || field.Value is short || field.Value is ushort)
							{
								jSONObject[field.Key] = new JSONNumber(Convert.ToInt32(field.Value));
								num++;
							}
							else
							{
								string text4 = $"ValidateAndCleanCustomFields: entry with key={field.Key}, value={field.Value} has been omitted because its value is not a string or number";
								GALogger.W(text4);
								AddErrorEvent("ValidateAndCleanCustomFields: entry with key={0}, value={1} has been omitted because its value is not a string or number", EGAErrorSeverity.Warning, text4);
							}
						}
						else
						{
							string text5 = $"ValidateAndCleanCustomFields: entry with key={field.Key}, value={field.Value} has been omitted because its key illegal characters, an empty or exceeds the max number of characters ({64})";
							GALogger.W(text5);
							AddErrorEvent("ValidateAndCleanCustomFields: entry with key={0}, value={1} has been omitted because its key illegal characters, an empty or exceeds the max number of characters ({2})", EGAErrorSeverity.Warning, text5);
						}
					}
					else
					{
						string text6 = $"ValidateAndCleanCustomFields: entry with key={field.Key}, value={field.Value} has been omitted because it exceeds the max number of custom fields ({50})";
						GALogger.W(text6);
						AddErrorEvent("ValidateAndCleanCustomFields: entry with key={0}, value={1} has been omitted because it exceeds the max number of custom fields ({2})", EGAErrorSeverity.Warning, text6);
					}
				}
			}
			return jSONObject;
		}

		public static string GetRemoteConfigsStringValue(string key, string defaultValue)
		{
			lock (Instance.remoteConfigsLock)
			{
				return (!Instance.remoteConfigs[key].IsNull) ? Instance.remoteConfigs[key].Value : defaultValue;
			}
		}

		public static bool IsRemoteConfigsReady()
		{
			return Instance.remoteConfigsIsReady;
		}

		public static void AddRemoteConfigsListener(IRemoteConfigsListener listener)
		{
			if (!Instance.remoteConfigsListeners.Contains(listener))
			{
				Instance.remoteConfigsListeners.Add(listener);
			}
		}

		public static void RemoveRemoteConfigsListener(IRemoteConfigsListener listener)
		{
			if (Instance.remoteConfigsListeners.Contains(listener))
			{
				Instance.remoteConfigsListeners.Remove(listener);
			}
		}

		public static string GetRemoteConfigsAsString()
		{
			return Instance.remoteConfigs.ToString();
		}

		public static string GetABTestingId()
		{
			return Instance.AbId;
		}

		public static string GetABTestingVariantId()
		{
			return Instance.AbVariantId;
		}

		private static void CacheIdentifier()
		{
			if (!string.IsNullOrEmpty(UserId))
			{
				Identifier = UserId;
			}
			else if (!string.IsNullOrEmpty(Instance.DefaultUserId))
			{
				Identifier = Instance.DefaultUserId;
			}
			GALogger.D("identifier, {clean:" + Identifier + "}");
		}

		private static void EnsurePersistedStates()
		{
			JSONObject jSONObject = new JSONObject();
			JSONArray jSONArray = GAStore.ExecuteQuerySync("SELECT * FROM ga_state;");
			if (jSONArray != null && jSONArray.Count != 0)
			{
				for (int i = 0; i < jSONArray.Count; i++)
				{
					JSONNode jSONNode = jSONArray[i];
					jSONObject.Add(jSONNode["key"], jSONNode["value"]);
				}
			}
			GAState instance = Instance;
			instance.DefaultUserId = ((jSONObject["default_user_id"] != null) ? jSONObject["default_user_id"].Value : Guid.NewGuid().ToString());
			SessionNum = ((jSONObject["session_num"] != null) ? jSONObject["session_num"].AsInt : 0);
			TransactionNum = ((jSONObject["transaction_num"] != null) ? jSONObject["transaction_num"].AsInt : 0);
			if (!string.IsNullOrEmpty(CurrentCustomDimension01))
			{
				GAStore.SetState("dimension01", CurrentCustomDimension01);
			}
			else
			{
				CurrentCustomDimension01 = ((jSONObject["dimension01"] != null) ? jSONObject["dimension01"].Value : "");
				if (!string.IsNullOrEmpty(CurrentCustomDimension01))
				{
					GALogger.D("Dimension01 found in cache: " + CurrentCustomDimension01);
				}
			}
			if (!string.IsNullOrEmpty(CurrentCustomDimension02))
			{
				GAStore.SetState("dimension02", CurrentCustomDimension02);
			}
			else
			{
				CurrentCustomDimension02 = ((jSONObject["dimension02"] != null) ? jSONObject["dimension02"].Value : "");
				if (!string.IsNullOrEmpty(CurrentCustomDimension02))
				{
					GALogger.D("Dimension02 found in cache: " + CurrentCustomDimension02);
				}
			}
			if (!string.IsNullOrEmpty(CurrentCustomDimension03))
			{
				GAStore.SetState("dimension03", CurrentCustomDimension03);
			}
			else
			{
				CurrentCustomDimension03 = ((jSONObject["dimension03"] != null) ? jSONObject["dimension03"].Value : "");
				if (!string.IsNullOrEmpty(CurrentCustomDimension03))
				{
					GALogger.D("Dimension03 found in cache: " + CurrentCustomDimension03);
				}
			}
			string text = ((jSONObject["sdk_config_cached"] != null) ? jSONObject["sdk_config_cached"].Value : "");
			if (!string.IsNullOrEmpty(text))
			{
				JSONNode jSONNode2 = null;
				try
				{
					jSONNode2 = JSONNode.LoadFromBinaryBase64(text);
				}
				catch (Exception)
				{
				}
				if (jSONNode2 != null && jSONNode2.Count != 0)
				{
					string text2 = ((jSONObject["last_used_identifier"] != null) ? jSONObject["last_used_identifier"].Value : "");
					if (!string.IsNullOrEmpty(text2) && text2 != Identifier)
					{
						GALogger.W("New identifier spotted compared to last one used, clearing cached configs hash!!");
						if (jSONNode2["configs_hash"] != null)
						{
							jSONNode2["configs_hash"] = null;
						}
					}
					instance.SdkConfigCached = jSONNode2;
				}
			}
			JSONNode jSONNode3 = SdkConfig;
			instance.ConfigsHash = ((jSONNode3["configs_hash"] != null && jSONNode3["configs_hash"].IsString) ? jSONNode3["configs_hash"].Value : "");
			instance.AbId = ((jSONNode3["ab_id"] != null && jSONNode3["ab_id"].IsString) ? jSONNode3["ab_id"].Value : "");
			instance.AbVariantId = ((jSONNode3["ab_variant_id"] != null && jSONNode3["ab_variant_id"].IsString) ? jSONNode3["ab_variant_id"].Value : "");
			JSONArray jSONArray2 = GAStore.ExecuteQuerySync("SELECT * FROM ga_progression;");
			if (!(jSONArray2 != null) || jSONArray2.Count == 0)
			{
				return;
			}
			for (int j = 0; j < jSONArray2.Count; j++)
			{
				JSONNode jSONNode4 = jSONArray2[j];
				if (jSONNode4 != null && jSONNode4.Count != 0)
				{
					instance.progressionTries[jSONNode4["progression"].Value] = jSONNode4["tries"].AsInt;
				}
			}
		}

		private static void StartNewSession()
		{
			GALogger.I("Starting a new session.");
			ValidateAndFixCurrentDimensions();
			KeyValuePair<EGAHTTPApiResponse, JSONObject> keyValuePair = GAHTTPApi.Instance.RequestInitReturningDict(Instance.ConfigsHash);
			StartNewSession(keyValuePair.Key, keyValuePair.Value);
		}

		public static void StartNewSession(EGAHTTPApiResponse initResponse, JSONObject initResponseDict)
		{
			if ((initResponse == EGAHTTPApiResponse.Ok || initResponse == EGAHTTPApiResponse.Created) && initResponseDict != null)
			{
				long num = 0L;
				if (initResponseDict["server_ts"] != null)
				{
					num = CalculateServerTimeOffset(initResponseDict["server_ts"].AsLong);
				}
				initResponseDict.Add("time_offset", new JSONNumber(num));
				if (initResponse != EGAHTTPApiResponse.Created)
				{
					JSONNode jSONNode = SdkConfig;
					if (jSONNode["configs"] != null && jSONNode["configs"].IsArray)
					{
						initResponseDict["configs"] = jSONNode["configs"].AsArray;
					}
					if (jSONNode["configs_hash"] != null && jSONNode["configs_hash"].IsString)
					{
						initResponseDict["configs_hash"] = jSONNode["configs_hash"].Value;
					}
					if (jSONNode["ab_id"] != null && jSONNode["ab_id"].IsString)
					{
						initResponseDict["ab_id"] = jSONNode["ab_id"].Value;
					}
					if (jSONNode["ab_variant_id"] != null && jSONNode["ab_variant_id"].IsString)
					{
						initResponseDict["ab_variant_id"] = jSONNode["ab_variant_id"].Value;
					}
				}
				Instance.ConfigsHash = ((initResponseDict["configs_hash"] != null && initResponseDict["configs_hash"].IsString) ? initResponseDict["configs_hash"].Value : "");
				Instance.AbId = ((initResponseDict["ab_id"] != null && initResponseDict["ab_id"].IsString) ? initResponseDict["ab_id"].Value : "");
				Instance.AbVariantId = ((initResponseDict["ab_variant_id"] != null && initResponseDict["ab_variant_id"].IsString) ? initResponseDict["ab_variant_id"].Value : "");
				GAStore.SetState("sdk_config_cached", initResponseDict.SaveToBinaryBase64());
				GALogger.D("initResponseDict: " + initResponseDict.ToString());
				Instance.sdkConfigCached = initResponseDict;
				Instance.sdkConfig = initResponseDict;
				Instance.InitAuthorized = true;
			}
			else if (initResponse == EGAHTTPApiResponse.Unauthorized)
			{
				GALogger.W("Initialize SDK failed - Unauthorized");
				Instance.InitAuthorized = false;
			}
			else
			{
				switch (initResponse)
				{
				case EGAHTTPApiResponse.NoResponse:
				case EGAHTTPApiResponse.RequestTimeout:
					GALogger.I("Init call (session start) failed - no response. Could be offline or timeout.");
					break;
				case EGAHTTPApiResponse.BadResponse:
				case EGAHTTPApiResponse.JsonEncodeFailed:
				case EGAHTTPApiResponse.JsonDecodeFailed:
					GALogger.I("Init call (session start) failed - bad response. Could be bad response from proxy or GA servers.");
					break;
				case EGAHTTPApiResponse.BadRequest:
				case EGAHTTPApiResponse.UnknownResponseCode:
					GALogger.I("Init call (session start) failed - bad request or unknown response.");
					break;
				}
				if (Instance.sdkConfig == null)
				{
					if (Instance.sdkConfigCached != null)
					{
						GALogger.I("Init call (session start) failed - using cached init values.");
						Instance.sdkConfig = Instance.sdkConfigCached;
					}
					else
					{
						GALogger.I("Init call (session start) failed - using default init values.");
						Instance.sdkConfig = Instance.sdkConfigDefault;
					}
				}
				else
				{
					GALogger.I("Init call (session start) failed - using cached init values.");
				}
				Instance.InitAuthorized = true;
			}
			JSONNode jSONNode2 = SdkConfig;
			if (jSONNode2["enabled"].IsBoolean && !jSONNode2["enabled"].AsBool)
			{
				Instance.Enabled = false;
			}
			else if (!Instance.InitAuthorized)
			{
				Instance.Enabled = false;
			}
			else
			{
				Instance.Enabled = true;
			}
			Instance.ClientServerTimeOffset = ((SdkConfig["time_offset"] != null) ? SdkConfig["time_offset"].AsLong : 0);
			PopulateConfigurations(SdkConfig);
			if (!IsEnabled())
			{
				GALogger.W("Could not start session: SDK is disabled.");
				GAEvents.StopEventQueue();
				return;
			}
			GAEvents.EnsureEventQueueIsRunning();
			SessionId = Guid.NewGuid().ToString().ToLowerInvariant();
			SessionStart = GetClientTsAdjusted();
			GAEvents.AddSessionStartEvent();
		}

		private static void ValidateAndFixCurrentDimensions()
		{
			if (!GAValidator.ValidateDimension01(CurrentCustomDimension01))
			{
				GALogger.D("Invalid dimension01 found in variable. Setting to nil. Invalid dimension: " + CurrentCustomDimension01);
				SetCustomDimension01("");
			}
			if (!GAValidator.ValidateDimension02(CurrentCustomDimension02))
			{
				GALogger.D("Invalid dimension02 found in variable. Setting to nil. Invalid dimension: " + CurrentCustomDimension02);
				SetCustomDimension02("");
			}
			if (!GAValidator.ValidateDimension03(CurrentCustomDimension03))
			{
				GALogger.D("Invalid dimension03 found in variable. Setting to nil. Invalid dimension: " + CurrentCustomDimension03);
				SetCustomDimension03("");
			}
		}

		private static long CalculateServerTimeOffset(long serverTs)
		{
			long num = GAUtilities.TimeIntervalSince1970();
			return serverTs - num;
		}

		private static void PopulateConfigurations(JSONNode sdkConfig)
		{
			lock (Instance.remoteConfigsLock)
			{
				JSONArray asArray = sdkConfig["configs"].AsArray;
				if (asArray != null)
				{
					Instance.remoteConfigs = new JSONObject();
					for (int i = 0; i < asArray.Count; i++)
					{
						JSONNode jSONNode = asArray[i];
						if (!(jSONNode != null))
						{
							continue;
						}
						string value = jSONNode["key"].Value;
						object obj = null;
						obj = ((!jSONNode["value"].IsNumber) ? jSONNode["value"].Value : ((object)jSONNode["value"].AsDouble));
						long num = (jSONNode["start_ts"].IsNumber ? jSONNode["start_ts"].AsLong : long.MinValue);
						long num2 = (jSONNode["end_ts"].IsNumber ? jSONNode["end_ts"].AsLong : long.MaxValue);
						long clientTsAdjusted = GetClientTsAdjusted();
						GALogger.D("PopulateConfigurations: key=" + value + ", value=" + obj?.ToString() + ", start_ts=" + num + ", end_ts=, client_ts_adjusted=" + clientTsAdjusted);
						if (value != null && obj != null && clientTsAdjusted > num && clientTsAdjusted < num2)
						{
							new JSONObject();
							if (jSONNode["value"].IsNumber)
							{
								Instance.remoteConfigs.Add(value, new JSONNumber(jSONNode["value"].AsDouble));
							}
							else
							{
								Instance.remoteConfigs.Add(value, jSONNode["value"].Value);
							}
							GALogger.D("configuration added: " + jSONNode);
						}
					}
				}
				Instance.remoteConfigsIsReady = true;
				foreach (IRemoteConfigsListener remoteConfigsListener in Instance.remoteConfigsListeners)
				{
					remoteConfigsListener.OnRemoteConfigsUpdated();
				}
			}
		}
	}
}
