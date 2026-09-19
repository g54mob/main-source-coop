using System;
using GameAnalyticsSDK.Net.Http;
using GameAnalyticsSDK.Net.Logging;
using GameAnalyticsSDK.Net.State;
using GameAnalyticsSDK.Net.Utilities;

namespace GameAnalyticsSDK.Net.Validators
{
	internal static class GAValidator
	{
		public static bool ValidateBusinessEvent(string currency, long amount, string cartType, string itemType, string itemId)
		{
			if (!ValidateCurrency(currency))
			{
				GALogger.W("Validation fail - business event - currency: Cannot be (null) and need to be A-Z, 3 characters and in the standard at openexchangerates.org. Failed currency: " + currency);
				return false;
			}
			if (amount < 0)
			{
				GALogger.W("Validation fail - business event - amount. Cannot be less than 0. Failed amount: " + amount);
				return false;
			}
			if (!ValidateShortString(cartType, canBeEmpty: true))
			{
				GALogger.W("Validation fail - business event - cartType. Cannot be above 32 length. String: " + cartType);
				return false;
			}
			if (!ValidateEventPartLength(itemType, allowNull: false))
			{
				GALogger.W("Validation fail - business event - itemType: Cannot be (null), empty or above 64 characters. String: " + itemType);
				return false;
			}
			if (!ValidateEventPartCharacters(itemType))
			{
				GALogger.W("Validation fail - business event - itemType: Cannot contain other characters than A-z, 0-9, -_., ()!?. String: " + itemType);
				return false;
			}
			if (!ValidateEventPartLength(itemId, allowNull: false))
			{
				GALogger.W("Validation fail - business event - itemId. Cannot be (null), empty or above 64 characters. String: " + itemId);
				return false;
			}
			if (!ValidateEventPartCharacters(itemId))
			{
				GALogger.W("Validation fail - business event - itemId: Cannot contain other characters than A-z, 0-9, -_., ()!?. String: " + itemId);
				return false;
			}
			return true;
		}

		public static bool ValidateResourceEvent(EGAResourceFlowType flowType, string currency, long amount, string itemType, string itemId)
		{
			if (flowType == EGAResourceFlowType.Undefined)
			{
				GALogger.W("Validation fail - resource event - flowType: Invalid flow type.");
				return false;
			}
			if (string.IsNullOrEmpty(currency))
			{
				GALogger.W("Validation fail - resource event - currency: Cannot be (null)");
				return false;
			}
			if (!GAState.HasAvailableResourceCurrency(currency))
			{
				GALogger.W("Validation fail - resource event - currency: Not found in list of pre-defined available resource currencies. String: " + currency);
				return false;
			}
			if (amount <= 0)
			{
				GALogger.W("Validation fail - resource event - amount: Float amount cannot be 0 or negative. Value: " + amount);
				return false;
			}
			if (string.IsNullOrEmpty(itemType))
			{
				GALogger.W("Validation fail - resource event - itemType: Cannot be (null)");
				return false;
			}
			if (!ValidateEventPartLength(itemType, allowNull: false))
			{
				GALogger.W("Validation fail - resource event - itemType: Cannot be (null), empty or above 64 characters. String: " + itemType);
				return false;
			}
			if (!ValidateEventPartCharacters(itemType))
			{
				GALogger.W("Validation fail - resource event - itemType: Cannot contain other characters than A-z, 0-9, -_., ()!?. String: " + itemType);
				return false;
			}
			if (!GAState.HasAvailableResourceItemType(itemType))
			{
				GALogger.W("Validation fail - resource event - itemType: Not found in list of pre-defined available resource itemTypes. String: " + itemType);
				return false;
			}
			if (!ValidateEventPartLength(itemId, allowNull: false))
			{
				GALogger.W("Validation fail - resource event - itemId: Cannot be (null), empty or above 64 characters. String: " + itemId);
				return false;
			}
			if (!ValidateEventPartCharacters(itemId))
			{
				GALogger.W("Validation fail - resource event - itemId: Cannot contain other characters than A-z, 0-9, -_., ()!?. String: " + itemId);
				return false;
			}
			return true;
		}

		public static bool ValidateProgressionEvent(EGAProgressionStatus progressionStatus, string progression01, string progression02, string progression03)
		{
			if (progressionStatus == EGAProgressionStatus.Undefined)
			{
				GALogger.W("Validation fail - progression event: Invalid progression status.");
				return false;
			}
			if (!string.IsNullOrEmpty(progression03) && string.IsNullOrEmpty(progression02) && !string.IsNullOrEmpty(progression01))
			{
				GALogger.W("Validation fail - progression event: 03 found but 01+02 are invalid. Progression must be set as either 01, 01+02 or 01+02+03.");
				return false;
			}
			if (!string.IsNullOrEmpty(progression02) && string.IsNullOrEmpty(progression01))
			{
				GALogger.W("Validation fail - progression event: 02 found but not 01. Progression must be set as either 01, 01+02 or 01+02+03");
				return false;
			}
			if (string.IsNullOrEmpty(progression01))
			{
				GALogger.W("Validation fail - progression event: progression01 not valid. Progressions must be set as either 01, 01+02 or 01+02+03");
				return false;
			}
			if (!ValidateEventPartLength(progression01, allowNull: false))
			{
				GALogger.W("Validation fail - progression event - progression01: Cannot be (null), empty or above 64 characters. String: " + progression01);
				return false;
			}
			if (!ValidateEventPartCharacters(progression01))
			{
				GALogger.W("Validation fail - progression event - progression01: Cannot contain other characters than A-z, 0-9, -_., ()!?. String: " + progression01);
				return false;
			}
			if (!string.IsNullOrEmpty(progression02))
			{
				if (!ValidateEventPartLength(progression02, allowNull: true))
				{
					GALogger.W("Validation fail - progression event - progression02: Cannot be empty or above 64 characters. String: " + progression02);
					return false;
				}
				if (!ValidateEventPartCharacters(progression02))
				{
					GALogger.W("Validation fail - progression event - progression02: Cannot contain other characters than A-z, 0-9, -_., ()!?. String: " + progression02);
					return false;
				}
			}
			if (!string.IsNullOrEmpty(progression03))
			{
				if (!ValidateEventPartLength(progression03, allowNull: true))
				{
					GALogger.W("Validation fail - progression event - progression03: Cannot be empty or above 64 characters. String: " + progression03);
					return false;
				}
				if (!ValidateEventPartCharacters(progression03))
				{
					GALogger.W("Validation fail - progression event - progression03: Cannot contain other characters than A-z, 0-9, -_., ()!?. String: " + progression03);
					return false;
				}
			}
			return true;
		}

		public static bool ValidateDesignEvent(string eventId, double value)
		{
			if (!ValidateEventIdLength(eventId))
			{
				GALogger.W("Validation fail - design event - eventId: Cannot be (null) or empty. Only 5 event parts allowed seperated by :. Each part need to be 64 characters or less. String: " + eventId);
				return false;
			}
			if (!ValidateEventIdCharacters(eventId))
			{
				GALogger.W("Validation fail - design event - eventId: Non valid characters. Only allowed A-z, 0-9, -_., ()!?. String: " + eventId);
				return false;
			}
			return true;
		}

		public static bool ValidateErrorEvent(EGAErrorSeverity severity, string message)
		{
			if (severity == EGAErrorSeverity.Undefined)
			{
				GALogger.W("Validation fail - error event - severity: Severity was unsupported value.");
				return false;
			}
			if (!ValidateLongString(message, canBeEmpty: true))
			{
				GALogger.W("Validation fail - error event - message: Message cannot be above 8192 characters.");
				return false;
			}
			return true;
		}

		public static bool ValidateSdkErrorEvent(string gameKey, string gameSecret, EGASdkErrorType type)
		{
			if (!ValidateKeys(gameKey, gameSecret))
			{
				return false;
			}
			if (type == EGASdkErrorType.Undefined)
			{
				GALogger.W("Validation fail - sdk error event - type: Type was unsupported value.");
				return false;
			}
			return true;
		}

		public static bool ValidateKeys(string gameKey, string gameSecret)
		{
			if (GAUtilities.StringMatch(gameKey, "^[A-z0-9]{32}$") && GAUtilities.StringMatch(gameSecret, "^[A-z0-9]{40}$"))
			{
				return true;
			}
			return false;
		}

		public static bool ValidateCurrency(string currency)
		{
			if (string.IsNullOrEmpty(currency))
			{
				return false;
			}
			if (!GAUtilities.StringMatch(currency, "^[A-Z]{3}$"))
			{
				return false;
			}
			return true;
		}

		public static bool ValidateEventPartLength(string eventPart, bool allowNull)
		{
			if (allowNull && string.IsNullOrEmpty(eventPart))
			{
				return true;
			}
			if (string.IsNullOrEmpty(eventPart))
			{
				return false;
			}
			if (eventPart.Length > 64)
			{
				return false;
			}
			return true;
		}

		public static bool ValidateEventPartCharacters(string eventPart)
		{
			if (!GAUtilities.StringMatch(eventPart, "^[A-Za-z0-9\\s\\-_\\.\\(\\)\\!\\?]{1,64}$"))
			{
				return false;
			}
			return true;
		}

		public static bool ValidateEventIdLength(string eventId)
		{
			if (string.IsNullOrEmpty(eventId))
			{
				return false;
			}
			if (!GAUtilities.StringMatch(eventId, "^[^:]{1,64}(?::[^:]{1,64}){0,4}$"))
			{
				return false;
			}
			return true;
		}

		public static bool ValidateEventIdCharacters(string eventId)
		{
			if (string.IsNullOrEmpty(eventId))
			{
				return false;
			}
			if (!GAUtilities.StringMatch(eventId, "^[A-Za-z0-9\\s\\-_\\.\\(\\)\\!\\?]{1,64}(:[A-Za-z0-9\\s\\-_\\.\\(\\)\\!\\?]{1,64}){0,4}$"))
			{
				return false;
			}
			return true;
		}

		public static JSONObject ValidateAndCleanInitRequestResponse(JSONNode initResponse, bool configsCreated)
		{
			if (initResponse == null)
			{
				GALogger.W("validateInitRequestResponse failed - no response dictionary.");
				return null;
			}
			JSONObject jSONObject = new JSONObject();
			try
			{
				long num = (initResponse["server_ts"].IsNumber ? initResponse["server_ts"].AsLong : (-1));
				if (num > 0)
				{
					jSONObject.Add("server_ts", new JSONNumber(num));
				}
			}
			catch (Exception ex)
			{
				GALogger.W(string.Concat("validateInitRequestResponse failed - invalid type in 'server_ts' field. type=", initResponse["server_ts"].GetType()?.ToString(), ", value=", initResponse["server_ts"], ", ", ex?.ToString()));
				return null;
			}
			if (configsCreated)
			{
				try
				{
					jSONObject.Add("configs", initResponse["configs"].IsArray ? initResponse["configs"].AsArray : new JSONArray());
				}
				catch (Exception ex2)
				{
					GALogger.W(string.Concat("validateInitRequestResponse failed - invalid type in 'configs' field. type=", initResponse["configs"].GetType()?.ToString(), ", value=", initResponse["configs"], ", ", ex2?.ToString()));
					return null;
				}
				try
				{
					jSONObject.Add("configs_hash", initResponse["configs_hash"].IsString ? initResponse["configs_hash"].Value : "");
				}
				catch (Exception ex3)
				{
					GALogger.W(string.Concat("validateInitRequestResponse failed - invalid type in 'configs_hash' field. type=", initResponse["configs_hash"].GetType()?.ToString(), ", value=", initResponse["configs_hash"], ", ", ex3?.ToString()));
					return null;
				}
				try
				{
					jSONObject.Add("ab_id", initResponse["ab_id"].IsString ? initResponse["ab_id"].Value : "");
				}
				catch (Exception ex4)
				{
					GALogger.W(string.Concat("validateInitRequestResponse failed - invalid type in 'ab_id' field. type=", initResponse["ab_id"].GetType()?.ToString(), ", value=", initResponse["ab_id"], ", ", ex4?.ToString()));
					return null;
				}
				try
				{
					jSONObject.Add("ab_variant_id", initResponse["ab_variant_id"].IsString ? initResponse["ab_variant_id"].Value : "");
				}
				catch (Exception ex5)
				{
					GALogger.W(string.Concat("validateInitRequestResponse failed - invalid type in 'ab_variant_id' field. type=", initResponse["ab_variant_id"].GetType()?.ToString(), ", value=", initResponse["ab_variant_id"], ", ", ex5?.ToString()));
					return null;
				}
			}
			return jSONObject;
		}

		public static bool ValidateBuild(string build)
		{
			if (!ValidateShortString(build, canBeEmpty: false))
			{
				return false;
			}
			return true;
		}

		public static bool ValidateSdkWrapperVersion(string wrapperVersion)
		{
			if (!GAUtilities.StringMatch(wrapperVersion, "^(unity) [0-9]{0,5}(\\.[0-9]{0,5}){0,2}$"))
			{
				return false;
			}
			return true;
		}

		public static bool ValidateEngineVersion(string engineVersion)
		{
			if (engineVersion == null || !GAUtilities.StringMatch(engineVersion, "^(unity) [0-9]{0,5}(\\.[0-9]{0,5}){0,2}$"))
			{
				return false;
			}
			return true;
		}

		public static bool ValidateUserId(string uId)
		{
			if (!ValidateString(uId, canBeEmpty: false))
			{
				GALogger.W("Validation fail - user id: id cannot be (null), empty or above 64 characters.");
				return false;
			}
			return true;
		}

		public static bool ValidateShortString(string shortString, bool canBeEmpty)
		{
			if (canBeEmpty && string.IsNullOrEmpty(shortString))
			{
				return true;
			}
			if (string.IsNullOrEmpty(shortString) || shortString.Length > 32)
			{
				return false;
			}
			return true;
		}

		public static bool ValidateString(string s, bool canBeEmpty)
		{
			if (canBeEmpty && string.IsNullOrEmpty(s))
			{
				return true;
			}
			if (string.IsNullOrEmpty(s) || s.Length > 64)
			{
				return false;
			}
			return true;
		}

		public static bool ValidateLongString(string longString, bool canBeEmpty)
		{
			if (canBeEmpty && string.IsNullOrEmpty(longString))
			{
				return true;
			}
			if (string.IsNullOrEmpty(longString) || longString.Length > 8192)
			{
				return false;
			}
			return true;
		}

		public static bool ValidateConnectionType(string connectionType)
		{
			return GAUtilities.StringMatch(connectionType, "^(wwan|wifi|lan|offline)$");
		}

		public static bool ValidateCustomDimensions(params string[] customDimensions)
		{
			return ValidateArrayOfStrings(20L, 32L, allowNoValues: false, "custom dimensions", customDimensions);
		}

		public static bool ValidateResourceCurrencies(params string[] resourceCurrencies)
		{
			if (!ValidateArrayOfStrings(20L, 64L, allowNoValues: false, "resource currencies", resourceCurrencies))
			{
				return false;
			}
			foreach (string text in resourceCurrencies)
			{
				if (!GAUtilities.StringMatch(text, "^[A-Za-z]+$"))
				{
					GALogger.W("resource currencies validation failed: a resource currency can only be A-Z, a-z. String was: " + text);
					return false;
				}
			}
			return true;
		}

		public static bool ValidateResourceItemTypes(params string[] resourceItemTypes)
		{
			if (!ValidateArrayOfStrings(20L, 32L, allowNoValues: false, "resource item types", resourceItemTypes))
			{
				return false;
			}
			foreach (string text in resourceItemTypes)
			{
				if (!ValidateEventPartCharacters(text))
				{
					GALogger.W("resource item types validation failed: a resource item type cannot contain other characters than A-z, 0-9, -_., ()!?. String was: " + text);
					return false;
				}
			}
			return true;
		}

		public static bool ValidateDimension01(string dimension01)
		{
			if (string.IsNullOrEmpty(dimension01))
			{
				return true;
			}
			if (!GAState.HasAvailableCustomDimensions01(dimension01))
			{
				return false;
			}
			return true;
		}

		public static bool ValidateDimension02(string dimension02)
		{
			if (string.IsNullOrEmpty(dimension02))
			{
				return true;
			}
			if (!GAState.HasAvailableCustomDimensions02(dimension02))
			{
				return false;
			}
			return true;
		}

		public static bool ValidateDimension03(string dimension03)
		{
			if (string.IsNullOrEmpty(dimension03))
			{
				return true;
			}
			if (!GAState.HasAvailableCustomDimensions03(dimension03))
			{
				return false;
			}
			return true;
		}

		public static bool ValidateArrayOfStrings(long maxCount, long maxStringLength, bool allowNoValues, string logTag, params string[] arrayOfStrings)
		{
			string text = logTag;
			if (string.IsNullOrEmpty(text))
			{
				text = "Array";
			}
			if (arrayOfStrings == null)
			{
				GALogger.W(text + " validation failed: array cannot be null. ");
				return false;
			}
			if (!allowNoValues && arrayOfStrings.Length == 0)
			{
				GALogger.W(text + " validation failed: array cannot be empty. ");
				return false;
			}
			if (maxCount > 0 && arrayOfStrings.Length > maxCount)
			{
				GALogger.W(text + " validation failed: array cannot exceed " + maxCount + " values. It has " + arrayOfStrings.Length + " values.");
				return false;
			}
			foreach (string text2 in arrayOfStrings)
			{
				int num = text2?.Length ?? 0;
				if (num == 0)
				{
					GALogger.W(text + " validation failed: contained an empty string.");
					return false;
				}
				if (maxStringLength > 0 && num > maxStringLength)
				{
					GALogger.W(text + " validation failed: a string exceeded max allowed length (which is: " + maxStringLength + "). String was: " + text2);
					return false;
				}
			}
			return true;
		}

		public static bool ValidateClientTs(long clientTs)
		{
			if (clientTs < 0 || clientTs > 99999999999L)
			{
				return false;
			}
			return true;
		}
	}
}
