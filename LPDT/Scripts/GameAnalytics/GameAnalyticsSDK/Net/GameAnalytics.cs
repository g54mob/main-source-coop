using System;
using System.Collections.Generic;
using GameAnalyticsSDK.Net.Device;
using GameAnalyticsSDK.Net.Events;
using GameAnalyticsSDK.Net.Logging;
using GameAnalyticsSDK.Net.State;
using GameAnalyticsSDK.Net.Store;
using GameAnalyticsSDK.Net.Threading;
using GameAnalyticsSDK.Net.Validators;

namespace GameAnalyticsSDK.Net
{
	public static class GameAnalytics
	{
		private static bool _endThread;

		static GameAnalytics()
		{
			_endThread = false;
			GADevice.Touch();
		}

		public static void ConfigureAvailableCustomDimensions01(params string[] customDimensions)
		{
			if (_endThread)
			{
				return;
			}
			GAThreading.PerformTaskOnGAThread("configureAvailableCustomDimensions01", delegate
			{
				if (IsSdkReady(needsInitialized: true, warn: false))
				{
					GALogger.W("Available custom dimensions must be set before SDK is initialized");
				}
				else
				{
					GAState.AvailableCustomDimensions01 = customDimensions;
				}
			});
		}

		public static void ConfigureAvailableCustomDimensions02(params string[] customDimensions)
		{
			if (_endThread)
			{
				return;
			}
			GAThreading.PerformTaskOnGAThread("configureAvailableCustomDimensions02", delegate
			{
				if (IsSdkReady(needsInitialized: true, warn: false))
				{
					GALogger.W("Available custom dimensions must be set before SDK is initialized");
				}
				else
				{
					GAState.AvailableCustomDimensions02 = customDimensions;
				}
			});
		}

		public static void ConfigureAvailableCustomDimensions03(params string[] customDimensions)
		{
			if (_endThread)
			{
				return;
			}
			GAThreading.PerformTaskOnGAThread("configureAvailableCustomDimensions03", delegate
			{
				if (IsSdkReady(needsInitialized: true, warn: false))
				{
					GALogger.W("Available custom dimensions must be set before SDK is initialized");
				}
				else
				{
					GAState.AvailableCustomDimensions03 = customDimensions;
				}
			});
		}

		public static void ConfigureAvailableResourceCurrencies(params string[] resourceCurrencies)
		{
			if (_endThread)
			{
				return;
			}
			GAThreading.PerformTaskOnGAThread("configureAvailableResourceCurrencies", delegate
			{
				if (IsSdkReady(needsInitialized: true, warn: false))
				{
					GALogger.W("Available resource currencies must be set before SDK is initialized");
				}
				else
				{
					GAState.AvailableResourceCurrencies = resourceCurrencies;
				}
			});
		}

		public static void ConfigureAvailableResourceItemTypes(params string[] resourceItemTypes)
		{
			if (_endThread)
			{
				return;
			}
			GAThreading.PerformTaskOnGAThread("configureAvailableResourceItemTypes", delegate
			{
				if (IsSdkReady(needsInitialized: true, warn: false))
				{
					GALogger.W("Available resource item types must be set before SDK is initialized");
				}
				else
				{
					GAState.AvailableResourceItemTypes = resourceItemTypes;
				}
			});
		}

		public static void ConfigureBuild(string build)
		{
			if (_endThread)
			{
				return;
			}
			GAThreading.PerformTaskOnGAThread("configureBuild", delegate
			{
				if (IsSdkReady(needsInitialized: true, warn: false))
				{
					GALogger.W("Build version must be set before SDK is initialized.");
				}
				else if (!GAValidator.ValidateBuild(build))
				{
					GALogger.I("Validation fail - configure build: Cannot be null, empty or above 32 length. String: " + build);
				}
				else
				{
					GAState.Build = build;
				}
			});
		}

		public static void ConfigureSdkGameEngineVersion(string sdkGameEngineVersion)
		{
			if (_endThread)
			{
				return;
			}
			GAThreading.PerformTaskOnGAThread("configureSdkGameEngineVersion", delegate
			{
				if (!IsSdkReady(needsInitialized: true, warn: false))
				{
					if (!GAValidator.ValidateSdkWrapperVersion(sdkGameEngineVersion))
					{
						GALogger.I("Validation fail - configure sdk version: Sdk version not supported. String: " + sdkGameEngineVersion);
					}
					else
					{
						GADevice.SdkGameEngineVersion = sdkGameEngineVersion;
					}
				}
			});
		}

		public static void ConfigureGameEngineVersion(string gameEngineVersion)
		{
			if (_endThread)
			{
				return;
			}
			GAThreading.PerformTaskOnGAThread("configureGameEngineVersion", delegate
			{
				if (!IsSdkReady(needsInitialized: true, warn: false))
				{
					if (!GAValidator.ValidateEngineVersion(gameEngineVersion))
					{
						GALogger.I("Validation fail - configure sdk version: Sdk version not supported. String: " + gameEngineVersion);
					}
					else
					{
						GADevice.GameEngineVersion = gameEngineVersion;
					}
				}
			});
		}

		public static void ConfigureUserId(string uId)
		{
			if (_endThread)
			{
				return;
			}
			GAThreading.PerformTaskOnGAThread("configureUserId", delegate
			{
				if (IsSdkReady(needsInitialized: true, warn: false))
				{
					GALogger.W("A custom user id must be set before SDK is initialized.");
				}
				else if (!GAValidator.ValidateUserId(uId))
				{
					GALogger.I("Validation fail - configure user_id: Cannot be null, empty or above 64 length. Will use default user_id method. Used string: " + uId);
				}
				else
				{
					GAState.UserId = uId;
				}
			});
		}

		public static void Initialize(string gameKey, string gameSecret)
		{
			if (_endThread)
			{
				return;
			}
			GADevice.UpdateConnectionType();
			GAThreading.PerformTaskOnGAThread("initialize", delegate
			{
				if (IsSdkReady(needsInitialized: true, warn: false))
				{
					GALogger.W("SDK already initialized. Can only be called once.");
				}
				else if (!GAValidator.ValidateKeys(gameKey, gameSecret))
				{
					GALogger.W("SDK failed initialize. Game key or secret key is invalid. Can only contain characters A-z 0-9, gameKey is 32 length, gameSecret is 40 length. Failed keys - gameKey: " + gameKey + ", secretKey: " + gameSecret);
				}
				else
				{
					GAState.SetKeys(gameKey, gameSecret);
					if (!GAStore.EnsureDatabase(dropDatabase: false, gameKey))
					{
						GALogger.W("Could not ensure/validate local event database: " + GADevice.WritablePath);
					}
					GAState.InternalInitialize();
				}
			});
		}

		public static void AddBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType, IDictionary<string, object> customFields = null, bool mergeFields = false)
		{
			if (_endThread)
			{
				return;
			}
			GADevice.UpdateConnectionType();
			GAThreading.PerformTaskOnGAThread("addBusinessEvent", delegate
			{
				if (IsSdkReady(needsInitialized: true, warn: true, "Could not add business event"))
				{
					GAEvents.AddBusinessEvent(currency, amount, itemType, itemId, cartType, customFields, mergeFields);
				}
			});
		}

		public static void AddResourceEvent(EGAResourceFlowType flowType, string currency, float amount, string itemType, string itemId, IDictionary<string, object> customFields = null, bool mergeFields = false)
		{
			if (_endThread)
			{
				return;
			}
			GADevice.UpdateConnectionType();
			GAThreading.PerformTaskOnGAThread("addResourceEvent", delegate
			{
				if (IsSdkReady(needsInitialized: true, warn: true, "Could not add resource event"))
				{
					GAEvents.AddResourceEvent(flowType, currency, amount, itemType, itemId, customFields, mergeFields);
				}
			});
		}

		public static void AddProgressionEvent(EGAProgressionStatus progressionStatus, string progression01, IDictionary<string, object> customFields = null, bool mergeFields = false)
		{
			AddProgressionEvent(progressionStatus, progression01, "", "", customFields, mergeFields);
		}

		public static void AddProgressionEvent(EGAProgressionStatus progressionStatus, string progression01, double score, IDictionary<string, object> customFields = null, bool mergeFields = false)
		{
			AddProgressionEvent(progressionStatus, progression01, "", "", score, customFields, mergeFields);
		}

		public static void AddProgressionEvent(EGAProgressionStatus progressionStatus, string progression01, string progression02, IDictionary<string, object> customFields = null, bool mergeFields = false)
		{
			AddProgressionEvent(progressionStatus, progression01, progression02, "", customFields, mergeFields);
		}

		public static void AddProgressionEvent(EGAProgressionStatus progressionStatus, string progression01, string progression02, double score, IDictionary<string, object> customFields = null, bool mergeFields = false)
		{
			AddProgressionEvent(progressionStatus, progression01, progression02, "", score, customFields, mergeFields);
		}

		public static void AddProgressionEvent(EGAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, IDictionary<string, object> customFields = null, bool mergeFields = false)
		{
			if (_endThread)
			{
				return;
			}
			GADevice.UpdateConnectionType();
			GAThreading.PerformTaskOnGAThread("addProgressionEvent", delegate
			{
				if (IsSdkReady(needsInitialized: true, warn: true, "Could not add progression event"))
				{
					GAEvents.AddProgressionEvent(progressionStatus, progression01, progression02, progression03, 0.0, sendScore: false, customFields, mergeFields);
				}
			});
		}

		public static void AddProgressionEvent(EGAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, double score, IDictionary<string, object> customFields = null, bool mergeFields = false)
		{
			if (_endThread)
			{
				return;
			}
			GADevice.UpdateConnectionType();
			GAThreading.PerformTaskOnGAThread("addProgressionEvent", delegate
			{
				if (IsSdkReady(needsInitialized: true, warn: true, "Could not add progression event"))
				{
					GAEvents.AddProgressionEvent(progressionStatus, progression01, progression02, progression03, score, sendScore: true, customFields, mergeFields);
				}
			});
		}

		public static void AddDesignEvent(string eventId, IDictionary<string, object> customFields = null, bool mergeFields = false)
		{
			if (_endThread)
			{
				return;
			}
			GADevice.UpdateConnectionType();
			GAThreading.PerformTaskOnGAThread("addDesignEvent", delegate
			{
				if (IsSdkReady(needsInitialized: true, warn: true, "Could not add design event"))
				{
					GAEvents.AddDesignEvent(eventId, 0.0, sendValue: false, customFields, mergeFields);
				}
			});
		}

		public static void AddDesignEvent(string eventId, double value, IDictionary<string, object> customFields = null, bool mergeFields = false)
		{
			if (_endThread)
			{
				return;
			}
			GADevice.UpdateConnectionType();
			GAThreading.PerformTaskOnGAThread("addDesignEvent", delegate
			{
				if (IsSdkReady(needsInitialized: true, warn: true, "Could not add design event"))
				{
					GAEvents.AddDesignEvent(eventId, value, sendValue: true, customFields, mergeFields);
				}
			});
		}

		public static void AddErrorEvent(EGAErrorSeverity severity, string message, IDictionary<string, object> customFields = null, bool mergeFields = false)
		{
			if (_endThread)
			{
				return;
			}
			GADevice.UpdateConnectionType();
			GAThreading.PerformTaskOnGAThread("addErrorEvent", delegate
			{
				if (IsSdkReady(needsInitialized: true, warn: true, "Could not add error event"))
				{
					GAEvents.AddErrorEvent(severity, message, customFields, mergeFields);
				}
			});
		}

		public static void SetEnabledInfoLog(bool flag)
		{
			if (_endThread)
			{
				return;
			}
			GAThreading.PerformTaskOnGAThread("setEnabledInfoLog", delegate
			{
				if (flag)
				{
					GALogger.InfoLog = flag;
					GALogger.I("Info logging enabled");
				}
				else
				{
					GALogger.I("Info logging disabled");
					GALogger.InfoLog = flag;
				}
			});
		}

		public static void SetEnabledVerboseLog(bool flag)
		{
			if (_endThread)
			{
				return;
			}
			GAThreading.PerformTaskOnGAThread("setEnabledVerboseLog", delegate
			{
				if (flag)
				{
					GALogger.VerboseLog = flag;
					GALogger.I("Verbose logging enabled");
				}
				else
				{
					GALogger.I("Verbose logging disabled");
					GALogger.VerboseLog = flag;
				}
			});
		}

		public static void SetEnabledManualSessionHandling(bool flag)
		{
			if (!_endThread)
			{
				GAThreading.PerformTaskOnGAThread("setEnabledManualSessionHandling", delegate
				{
					GAState.SetManualSessionHandling(flag);
				});
			}
		}

		public static void SetEnabledEventSubmission(bool flag)
		{
			if (_endThread)
			{
				return;
			}
			GAThreading.PerformTaskOnGAThread("setEnabledEventSubmission", delegate
			{
				if (flag)
				{
					GAState.SetEnabledEventSubmission(flag);
					GALogger.I("Event submission enabled");
				}
				else
				{
					GALogger.I("Event submission disabled");
					GAState.SetEnabledEventSubmission(flag);
				}
			});
		}

		public static void SetCustomDimension01(string dimension)
		{
			if (_endThread)
			{
				return;
			}
			GAThreading.PerformTaskOnGAThread("setCustomDimension01", delegate
			{
				if (!GAValidator.ValidateDimension01(dimension))
				{
					GALogger.W("Could not set custom01 dimension value to '" + dimension + "'. Value not found in available custom01 dimension values");
				}
				else
				{
					GAState.SetCustomDimension01(dimension);
				}
			});
		}

		public static void SetCustomDimension02(string dimension)
		{
			if (_endThread)
			{
				return;
			}
			GAThreading.PerformTaskOnGAThread("setCustomDimension02", delegate
			{
				if (!GAValidator.ValidateDimension02(dimension))
				{
					GALogger.W("Could not set custom02 dimension value to '" + dimension + "'. Value not found in available custom02 dimension values");
				}
				else
				{
					GAState.SetCustomDimension02(dimension);
				}
			});
		}

		public static void SetCustomDimension03(string dimension)
		{
			if (_endThread)
			{
				return;
			}
			GAThreading.PerformTaskOnGAThread("setCustomDimension03", delegate
			{
				if (!GAValidator.ValidateDimension03(dimension))
				{
					GALogger.W("Could not set custom03 dimension value to '" + dimension + "'. Value not found in available custom03 dimension values");
				}
				else
				{
					GAState.SetCustomDimension03(dimension);
				}
			});
		}

		public static void SetGlobalCustomEventFields(IDictionary<string, object> customFields)
		{
			if (!_endThread)
			{
				GAThreading.PerformTaskOnGAThread("setGlobalCustomEventFields", delegate
				{
					GAState.SetGlobalCustomEventFields(customFields);
				});
			}
		}

		public static void StartSession()
		{
			if (_endThread)
			{
				return;
			}
			GAThreading.PerformTaskOnGAThread("startSession", delegate
			{
				if (GAState.Initialized)
				{
					if (GAState.IsEnabled() && GAState.SessionIsStarted())
					{
						GAState.EndSessionAndStopQueue(endThread: false);
					}
					GAState.ResumeSessionAndStartQueue();
				}
			});
		}

		public static void EndSession()
		{
			OnSuspend();
		}

		public static void OnResume()
		{
			if (!_endThread)
			{
				GALogger.D("OnResume() called");
				GAThreading.PerformTaskOnGAThread("onResume", delegate
				{
					GAState.ResumeSessionAndStartQueue();
				});
			}
		}

		public static void OnSuspend()
		{
			if (_endThread)
			{
				return;
			}
			GALogger.D("OnSuspend() called");
			GAThreading.PerformTaskOnGAThread("onSuspend", delegate
			{
				try
				{
					GAState.EndSessionAndStopQueue(endThread: false);
				}
				catch (Exception)
				{
				}
			});
		}

		public static void OnQuit()
		{
			if (_endThread)
			{
				return;
			}
			GALogger.D("OnQuit() called");
			GAThreading.PerformTaskOnGAThread("onQuit", delegate
			{
				try
				{
					_endThread = true;
					GAState.EndSessionAndStopQueue(endThread: true);
				}
				catch (Exception)
				{
				}
			});
		}

		public static string GetRemoteConfigsValueAsString(string key, string defaultValue = null)
		{
			return GAState.GetRemoteConfigsStringValue(key, defaultValue);
		}

		public static bool IsRemoteConfigsReady()
		{
			return GAState.IsRemoteConfigsReady();
		}

		public static void AddRemoteConfigsListener(IRemoteConfigsListener listener)
		{
			GAState.AddRemoteConfigsListener(listener);
		}

		public static void RemoveRemoteConfigsListener(IRemoteConfigsListener listener)
		{
			GAState.RemoveRemoteConfigsListener(listener);
		}

		public static string GetRemoteConfigsAsString()
		{
			return GAState.GetRemoteConfigsAsString();
		}

		public static string GetABTestingId()
		{
			return GAState.GetABTestingId();
		}

		public static string GetABTestingVariantId()
		{
			return GAState.GetABTestingVariantId();
		}

		public static string GetUserId()
		{
			if (_endThread)
			{
				return null;
			}
			if (!string.IsNullOrEmpty(GAState.Identifier))
			{
				return GAState.Identifier;
			}
			return string.Empty;
		}

		public static bool IsInitialized()
		{
			if (_endThread)
			{
				return false;
			}
			return IsSdkReady(needsInitialized: true, warn: true);
		}

		private static bool IsSdkReady(bool needsInitialized)
		{
			return IsSdkReady(needsInitialized, warn: true);
		}

		private static bool IsSdkReady(bool needsInitialized, bool warn)
		{
			return IsSdkReady(needsInitialized, warn, "");
		}

		private static bool IsSdkReady(bool needsInitialized, bool warn, string message)
		{
			if (!string.IsNullOrEmpty(message))
			{
				message += ": ";
			}
			if (!GAStore.IsTableReady)
			{
				if (warn)
				{
					GALogger.W(message + "Datastore not initialized");
				}
				return false;
			}
			if (needsInitialized && !GAState.Initialized)
			{
				if (warn)
				{
					GALogger.W(message + "SDK is not initialized");
				}
				return false;
			}
			if (needsInitialized && !GAState.IsEnabled())
			{
				if (warn)
				{
					GALogger.W(message + "SDK is disabled");
				}
				return false;
			}
			if (needsInitialized && !GAState.SessionIsStarted())
			{
				if (warn)
				{
					GALogger.W(message + "Session has not started yet");
				}
				return false;
			}
			return true;
		}
	}
}
