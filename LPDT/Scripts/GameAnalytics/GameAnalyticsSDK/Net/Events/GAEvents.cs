using System;
using System.Collections.Generic;
using System.Globalization;
using GameAnalyticsSDK.Net.Http;
using GameAnalyticsSDK.Net.Logging;
using GameAnalyticsSDK.Net.State;
using GameAnalyticsSDK.Net.Store;
using GameAnalyticsSDK.Net.Threading;
using GameAnalyticsSDK.Net.Utilities;
using GameAnalyticsSDK.Net.Validators;

namespace GameAnalyticsSDK.Net.Events
{
	internal class GAEvents
	{
		private static readonly GAEvents _instance = new GAEvents();

		private const string CategorySessionStart = "user";

		private const string CategorySessionEnd = "session_end";

		private const string CategoryDesign = "design";

		private const string CategoryBusiness = "business";

		private const string CategoryProgression = "progression";

		private const string CategoryResource = "resource";

		private const string CategoryError = "error";

		private bool isRunning;

		private bool keepRunning;

		private const double ProcessEventsIntervalInSeconds = 8.0;

		private const int MaxEventCount = 500;

		private static GAEvents Instance => _instance;

		private GAEvents()
		{
		}

		public static void StopEventQueue()
		{
			Instance.keepRunning = false;
		}

		public static void EnsureEventQueueIsRunning()
		{
			Instance.keepRunning = true;
			if (!Instance.isRunning)
			{
				Instance.isRunning = true;
				GAThreading.ScheduleTimer(8.0, "processEventQueue", ProcessEventQueue);
			}
		}

		public static void AddSessionStartEvent()
		{
			if (GAState.IsEventSubmissionEnabled)
			{
				string text = "user";
				JSONObject eventData = new JSONObject { ["category"] = text };
				GAState.IncrementSessionNum();
				GAStore.SetState("session_num", GAState.SessionNum.ToString(CultureInfo.InvariantCulture));
				AddDimensionsToEvent(eventData);
				IDictionary<string, object> currentGlobalCustomEventFields = GAState.CurrentGlobalCustomEventFields;
				AddFieldsToEvent(eventData, GAState.ValidateAndCleanCustomFields(currentGlobalCustomEventFields));
				AddEventToStore(eventData);
				GALogger.I("Add SESSION START event");
				ProcessEvents(text, performCleanUp: false);
			}
		}

		public static void AddSessionEndEvent()
		{
			if (GAState.IsEventSubmissionEnabled)
			{
				long sessionStart = GAState.SessionStart;
				long num = GAState.GetClientTsAdjusted() - sessionStart;
				if (num < 0)
				{
					GALogger.W("Session length was calculated to be less then 0. Should not be possible. Resetting to 0.");
					num = 0L;
				}
				JSONObject jSONObject = new JSONObject();
				jSONObject["category"] = "session_end";
				jSONObject.Add("length", new JSONNumber(num));
				AddDimensionsToEvent(jSONObject);
				IDictionary<string, object> currentGlobalCustomEventFields = GAState.CurrentGlobalCustomEventFields;
				AddFieldsToEvent(jSONObject, GAState.ValidateAndCleanCustomFields(currentGlobalCustomEventFields));
				AddEventToStore(jSONObject);
				GALogger.I("Add SESSION END event.");
				ProcessEvents("", performCleanUp: false);
			}
		}

		public static void AddBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType, IDictionary<string, object> fields, bool mergeFields)
		{
			if (!GAState.IsEventSubmissionEnabled || !GAValidator.ValidateBusinessEvent(currency, amount, cartType, itemType, itemId))
			{
				return;
			}
			JSONObject jSONObject = new JSONObject();
			GAState.IncrementTransactionNum();
			GAStore.SetState("transaction_num", GAState.TransactionNum.ToString(CultureInfo.InvariantCulture));
			jSONObject["event_id"] = itemType + ":" + itemId;
			jSONObject["category"] = "business";
			jSONObject["currency"] = currency;
			jSONObject.Add("amount", new JSONNumber(amount));
			jSONObject.Add("transaction_num", new JSONNumber(GAState.TransactionNum));
			if (!string.IsNullOrEmpty(cartType))
			{
				jSONObject.Add("cart_type", cartType);
			}
			AddDimensionsToEvent(jSONObject);
			IDictionary<string, object> dictionary = new Dictionary<string, object>((fields != null && fields.Count > 0) ? fields : GAState.CurrentGlobalCustomEventFields);
			if (mergeFields && fields != null && fields.Count > 0)
			{
				foreach (KeyValuePair<string, object> currentGlobalCustomEventField in GAState.CurrentGlobalCustomEventFields)
				{
					if (!dictionary.ContainsKey(currentGlobalCustomEventField.Key))
					{
						dictionary.Add(currentGlobalCustomEventField.Key, currentGlobalCustomEventField.Value);
					}
				}
			}
			AddFieldsToEvent(jSONObject, GAState.ValidateAndCleanCustomFields(dictionary));
			GALogger.I("Add BUSINESS event: {currency:" + currency + ", amount:" + amount + ", itemType:" + itemType + ", itemId:" + itemId + ", cartType:" + cartType + "}");
			AddEventToStore(jSONObject);
		}

		public static void AddResourceEvent(EGAResourceFlowType flowType, string currency, double amount, string itemType, string itemId, IDictionary<string, object> fields, bool mergeFields)
		{
			if (!GAState.IsEventSubmissionEnabled || !GAValidator.ValidateResourceEvent(flowType, currency, (long)amount, itemType, itemId))
			{
				return;
			}
			if (flowType == EGAResourceFlowType.Sink)
			{
				amount *= -1.0;
			}
			JSONObject jSONObject = new JSONObject();
			string text = ResourceFlowTypeToString(flowType);
			jSONObject["event_id"] = text + ":" + currency + ":" + itemType + ":" + itemId;
			jSONObject["category"] = "resource";
			jSONObject.Add("amount", new JSONNumber(amount));
			AddDimensionsToEvent(jSONObject);
			IDictionary<string, object> dictionary = new Dictionary<string, object>((fields != null && fields.Count > 0) ? fields : GAState.CurrentGlobalCustomEventFields);
			if (mergeFields && fields != null && fields.Count > 0)
			{
				foreach (KeyValuePair<string, object> currentGlobalCustomEventField in GAState.CurrentGlobalCustomEventFields)
				{
					if (!dictionary.ContainsKey(currentGlobalCustomEventField.Key))
					{
						dictionary.Add(currentGlobalCustomEventField.Key, currentGlobalCustomEventField.Value);
					}
				}
			}
			AddFieldsToEvent(jSONObject, GAState.ValidateAndCleanCustomFields(dictionary));
			GALogger.I("Add RESOURCE event: {currency:" + currency + ", amount:" + amount + ", itemType:" + itemType + ", itemId:" + itemId + "}");
			AddEventToStore(jSONObject);
		}

		public static void AddProgressionEvent(EGAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, double score, bool sendScore, IDictionary<string, object> fields, bool mergeFields)
		{
			if (!GAState.IsEventSubmissionEnabled)
			{
				return;
			}
			string text = ProgressionStatusToString(progressionStatus);
			if (!GAValidator.ValidateProgressionEvent(progressionStatus, progression01, progression02, progression03))
			{
				return;
			}
			JSONObject jSONObject = new JSONObject();
			string text2 = (string.IsNullOrEmpty(progression02) ? progression01 : ((!string.IsNullOrEmpty(progression03)) ? (progression01 + ":" + progression02 + ":" + progression03) : (progression01 + ":" + progression02)));
			jSONObject["category"] = "progression";
			jSONObject["event_id"] = text + ":" + text2;
			double aData = 0.0;
			if (sendScore && progressionStatus != EGAProgressionStatus.Start)
			{
				jSONObject.Add("score", new JSONNumber(score));
			}
			if (progressionStatus == EGAProgressionStatus.Fail)
			{
				GAState.IncrementProgressionTries(text2);
			}
			if (progressionStatus == EGAProgressionStatus.Complete)
			{
				GAState.IncrementProgressionTries(text2);
				aData = GAState.GetProgressionTries(text2);
				jSONObject.Add("attempt_num", new JSONNumber(aData));
				GAState.ClearProgressionTries(text2);
			}
			AddDimensionsToEvent(jSONObject);
			IDictionary<string, object> dictionary = new Dictionary<string, object>((fields != null && fields.Count > 0) ? fields : GAState.CurrentGlobalCustomEventFields);
			if (mergeFields && fields != null && fields.Count > 0)
			{
				foreach (KeyValuePair<string, object> currentGlobalCustomEventField in GAState.CurrentGlobalCustomEventFields)
				{
					if (!dictionary.ContainsKey(currentGlobalCustomEventField.Key))
					{
						dictionary.Add(currentGlobalCustomEventField.Key, currentGlobalCustomEventField.Value);
					}
				}
			}
			AddFieldsToEvent(jSONObject, GAState.ValidateAndCleanCustomFields(dictionary));
			GALogger.I("Add PROGRESSION event: {status:" + text + ", progression01:" + progression01 + ", progression02:" + progression02 + ", progression03:" + progression03 + ", score:" + score + ", attempt:" + aData + "}");
			AddEventToStore(jSONObject);
		}

		public static void AddDesignEvent(string eventId, double value, bool sendValue, IDictionary<string, object> fields, bool mergeFields)
		{
			if (!GAState.IsEventSubmissionEnabled || !GAValidator.ValidateDesignEvent(eventId, value))
			{
				return;
			}
			JSONObject jSONObject = new JSONObject();
			jSONObject["category"] = "design";
			jSONObject["event_id"] = eventId;
			if (sendValue)
			{
				jSONObject.Add("value", new JSONNumber(value));
			}
			AddDimensionsToEvent(jSONObject);
			IDictionary<string, object> dictionary = new Dictionary<string, object>((fields != null && fields.Count > 0) ? fields : GAState.CurrentGlobalCustomEventFields);
			if (mergeFields && fields != null && fields.Count > 0)
			{
				foreach (KeyValuePair<string, object> currentGlobalCustomEventField in GAState.CurrentGlobalCustomEventFields)
				{
					if (!dictionary.ContainsKey(currentGlobalCustomEventField.Key))
					{
						dictionary.Add(currentGlobalCustomEventField.Key, currentGlobalCustomEventField.Value);
					}
				}
			}
			AddFieldsToEvent(jSONObject, GAState.ValidateAndCleanCustomFields(dictionary));
			GALogger.I("Add DESIGN event: {eventId:" + eventId + ", value:" + value + "}");
			AddEventToStore(jSONObject);
		}

		public static void AddErrorEvent(EGAErrorSeverity severity, string message, IDictionary<string, object> fields, bool mergeFields)
		{
			AddErrorEvent(severity, message, fields, mergeFields, skipAddingFields: false);
		}

		public static void AddErrorEvent(EGAErrorSeverity severity, string message, IDictionary<string, object> fields, bool mergeFields, bool skipAddingFields)
		{
			if (!GAState.IsEventSubmissionEnabled)
			{
				return;
			}
			string text = ErrorSeverityToString(severity);
			if (!GAValidator.ValidateErrorEvent(severity, message))
			{
				return;
			}
			JSONObject jSONObject = new JSONObject();
			jSONObject["category"] = "error";
			jSONObject["severity"] = text;
			jSONObject["message"] = message;
			AddDimensionsToEvent(jSONObject);
			if (!skipAddingFields)
			{
				IDictionary<string, object> dictionary = new Dictionary<string, object>((fields != null && fields.Count > 0) ? fields : GAState.CurrentGlobalCustomEventFields);
				if (mergeFields && fields != null && fields.Count > 0)
				{
					foreach (KeyValuePair<string, object> currentGlobalCustomEventField in GAState.CurrentGlobalCustomEventFields)
					{
						if (!dictionary.ContainsKey(currentGlobalCustomEventField.Key))
						{
							dictionary.Add(currentGlobalCustomEventField.Key, currentGlobalCustomEventField.Value);
						}
					}
				}
				AddFieldsToEvent(jSONObject, GAState.ValidateAndCleanCustomFields(dictionary));
			}
			GALogger.I("Add ERROR event: {severity:" + text + ", message:" + message + "}");
			AddEventToStore(jSONObject);
		}

		private static void ProcessEventQueue()
		{
			ProcessEvents("", performCleanUp: true);
			if (Instance.keepRunning)
			{
				GAThreading.ScheduleTimer(8.0, "processEventQueue", ProcessEventQueue);
			}
			else
			{
				Instance.isRunning = false;
			}
		}

		private static void ProcessEvents(string category, bool performCleanUp)
		{
			if (!GAState.IsEventSubmissionEnabled)
			{
				return;
			}
			try
			{
				string text = Guid.NewGuid().ToString();
				string deleteSql = "DELETE FROM ga_events WHERE status = '" + text + "'";
				string putbackSql = "UPDATE ga_events SET status = 'new' WHERE status = '" + text + "';";
				if (performCleanUp)
				{
					CleanupEvents();
					FixMissingSessionEndEvents();
				}
				string text2 = "";
				if (!string.IsNullOrEmpty(category))
				{
					text2 = " AND category='" + category + "' ";
				}
				string sql = "SELECT event FROM ga_events WHERE status = 'new' " + text2 + ";";
				string sql2 = "UPDATE ga_events SET status = '" + text + "' WHERE status = 'new' " + text2 + ";";
				JSONArray jSONArray = GAStore.ExecuteQuerySync(sql);
				if (jSONArray == null || jSONArray.Count == 0)
				{
					GALogger.I("Event queue: No events to send");
					UpdateSessionTime();
					return;
				}
				if (jSONArray.Count > 500)
				{
					sql = "SELECT client_ts FROM ga_events WHERE status = 'new' " + text2 + " ORDER BY client_ts ASC LIMIT 0," + 500 + ";";
					jSONArray = GAStore.ExecuteQuerySync(sql);
					if (jSONArray == null)
					{
						return;
					}
					string value = jSONArray[jSONArray.Count - 1]["client_ts"].Value;
					sql = "SELECT event FROM ga_events WHERE status = 'new' " + text2 + " AND client_ts<='" + value + "';";
					jSONArray = GAStore.ExecuteQuerySync(sql);
					if (jSONArray == null)
					{
						return;
					}
					sql2 = "UPDATE ga_events SET status='" + text + "' WHERE status='new' " + text2 + " AND client_ts<='" + value + "';";
				}
				GALogger.I("Event queue: Sending " + jSONArray.Count + " events.");
				if (GAStore.ExecuteQuerySync(sql2) == null)
				{
					return;
				}
				List<JSONNode> list = new List<JSONNode>();
				for (int i = 0; i < jSONArray.Count; i++)
				{
					JSONNode jSONNode = jSONArray[i];
					JSONNode jSONNode2 = null;
					try
					{
						jSONNode2 = JSONNode.LoadFromBinaryBase64(jSONNode["event"].Value);
					}
					catch (Exception)
					{
					}
					if (jSONNode2 != null && jSONNode2.Count != 0)
					{
						if (!jSONNode2["client_ts"].IsNull && !GAValidator.ValidateClientTs(jSONNode2["client_ts"].AsLong))
						{
							jSONNode2.Remove("client_ts");
						}
						list.Add(jSONNode2);
					}
				}
				KeyValuePair<EGAHTTPApiResponse, JSONNode> keyValuePair = GAHTTPApi.Instance.SendEventsInArray(list);
				ProcessEvents(keyValuePair.Key, keyValuePair.Value, putbackSql, deleteSql, list.Count);
			}
			catch (Exception ex2)
			{
				GALogger.E("Error during ProcessEvents(): " + ex2);
			}
		}

		public static void ProcessEvents(EGAHTTPApiResponse responseEnum, JSONNode dataDict, string putbackSql, string deleteSql, int eventCount)
		{
			switch (responseEnum)
			{
			case EGAHTTPApiResponse.Ok:
				GAStore.ExecuteQuerySync(deleteSql);
				GALogger.I("Event queue: " + eventCount + " events sent.");
				return;
			case EGAHTTPApiResponse.NoResponse:
				GALogger.W("Event queue: Failed to send events to collector - Retrying next time");
				GAStore.ExecuteQuerySync(putbackSql);
				return;
			}
			if (dataDict != null)
			{
				JSONNode jSONNode = null;
				IEnumerator<JSONNode> enumerator = dataDict.Children.GetEnumerator();
				if (enumerator.MoveNext())
				{
					jSONNode = enumerator.Current;
				}
				if (responseEnum == EGAHTTPApiResponse.BadRequest && jSONNode is JSONArray)
				{
					GALogger.W("Event queue: " + eventCount + " events sent. " + dataDict.Count + " events failed GA server validation.");
				}
				else
				{
					GALogger.W("Event queue: Failed to send events.");
				}
			}
			else
			{
				GALogger.W("Event queue: Failed to send events.");
			}
			GAStore.ExecuteQuerySync(deleteSql);
		}

		private static void CleanupEvents()
		{
			GAStore.ExecuteQuerySync("UPDATE ga_events SET status = 'new';");
		}

		private static void FixMissingSessionEndEvents()
		{
			if (!GAState.IsEventSubmissionEnabled)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("$session_id", GAState.SessionId);
			JSONArray jSONArray = GAStore.ExecuteQuerySync("SELECT timestamp, event FROM ga_session WHERE session_id != $session_id;", dictionary);
			if (jSONArray == null || jSONArray.Count == 0)
			{
				return;
			}
			GALogger.I(jSONArray.Count + " session(s) located with missing session_end event.");
			for (int i = 0; i < jSONArray.Count; i++)
			{
				JSONNode jSONNode = jSONArray[i];
				JSONNode jSONNode2 = null;
				try
				{
					jSONNode2 = JSONNode.LoadFromBinaryBase64(jSONNode["event"].Value);
				}
				catch (Exception)
				{
				}
				if (jSONNode2 != null)
				{
					long asLong = jSONNode2["client_ts"].AsLong;
					long asLong2 = jSONNode["timestamp"].AsLong;
					long val = asLong - asLong2;
					val = Math.Max(0L, val);
					GALogger.D("fixMissingSessionEndEvents length calculated: " + val);
					jSONNode2["category"] = "session_end";
					jSONNode2.Add("length", new JSONNumber(val));
					AddEventToStore(jSONNode2.AsObject);
				}
				else
				{
					GALogger.I("Problem decoding session_end event. Skipping  this session_end event.");
				}
			}
		}

		private static void AddEventToStore(JSONObject eventData)
		{
			if (!GAStore.IsTableReady)
			{
				GALogger.W("Could not add event: SDK datastore error");
				return;
			}
			if (!GAState.Initialized)
			{
				GALogger.W("Could not add event: SDK is not initialized");
				return;
			}
			try
			{
				if (GAStore.IsDbTooLargeForEvents && !GAUtilities.StringMatch(eventData["category"].Value, "^(user|session_end|business)$"))
				{
					GALogger.W("Database too large. Event has been blocked.");
					return;
				}
				JSONObject eventAnnotations = GAState.GetEventAnnotations();
				JSONNode.Enumerator enumerator = eventData.GetEnumerator();
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, JSONNode> current = enumerator.Current;
					eventAnnotations.Add(current.Key, current.Value);
				}
				string text = eventAnnotations.ToString();
				GALogger.II("Event added to queue: " + text);
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Add("$status", "new");
				dictionary.Add("$category", eventAnnotations["category"].Value);
				dictionary.Add("$session_id", eventAnnotations["session_id"].Value);
				dictionary.Add("$client_ts", eventAnnotations["client_ts"].Value);
				dictionary.Add("$event", eventAnnotations.SaveToBinaryBase64());
				GAStore.ExecuteQuerySync("INSERT INTO ga_events (status, category, session_id, client_ts, event) VALUES($status, $category, $session_id, $client_ts, $event);", dictionary);
				if (eventData["category"].Value.Equals("session_end"))
				{
					dictionary.Clear();
					dictionary.Add("$session_id", eventAnnotations["session_id"].Value);
					GAStore.ExecuteQuerySync("DELETE FROM ga_session WHERE session_id = $session_id;", dictionary);
				}
				else
				{
					UpdateSessionTime();
				}
			}
			catch (Exception ex)
			{
				GALogger.E("addEventToStoreWithEventData: error using json");
				GALogger.E(ex.ToString());
			}
		}

		private static void AddDimensionsToEvent(JSONObject eventData)
		{
			if (!(eventData == null))
			{
				if (!string.IsNullOrEmpty(GAState.CurrentCustomDimension01))
				{
					eventData["custom_01"] = GAState.CurrentCustomDimension01;
				}
				if (!string.IsNullOrEmpty(GAState.CurrentCustomDimension02))
				{
					eventData["custom_02"] = GAState.CurrentCustomDimension02;
				}
				if (!string.IsNullOrEmpty(GAState.CurrentCustomDimension03))
				{
					eventData["custom_03"] = GAState.CurrentCustomDimension03;
				}
			}
		}

		private static void AddFieldsToEvent(JSONObject eventData, JSONObject fields)
		{
			if (!(eventData == null) && fields != null && fields.Count > 0)
			{
				eventData["custom_fields"] = fields;
			}
		}

		private static string ResourceFlowTypeToString(EGAResourceFlowType value)
		{
			return value switch
			{
				EGAResourceFlowType.Source => "Source", 
				EGAResourceFlowType.Sink => "Sink", 
				_ => "", 
			};
		}

		private static string ProgressionStatusToString(EGAProgressionStatus value)
		{
			return value switch
			{
				EGAProgressionStatus.Start => "Start", 
				EGAProgressionStatus.Complete => "Complete", 
				EGAProgressionStatus.Fail => "Fail", 
				_ => "", 
			};
		}

		private static void UpdateSessionTime()
		{
			if (GAState.SessionIsStarted())
			{
				JSONObject eventAnnotations = GAState.GetEventAnnotations();
				AddDimensionsToEvent(eventAnnotations);
				IDictionary<string, object> currentGlobalCustomEventFields = GAState.CurrentGlobalCustomEventFields;
				AddFieldsToEvent(eventAnnotations, GAState.ValidateAndCleanCustomFields(currentGlobalCustomEventFields));
				string value = eventAnnotations.SaveToBinaryBase64();
				GAStore.ExecuteQuerySync("INSERT OR REPLACE INTO ga_session(session_id, timestamp, event) VALUES($session_id, $timestamp, $event);", new Dictionary<string, object>
				{
					{
						"$session_id",
						eventAnnotations["session_id"].Value
					},
					{
						"$timestamp",
						GAState.SessionStart
					},
					{ "$event", value }
				});
			}
		}

		private static string ErrorSeverityToString(EGAErrorSeverity value)
		{
			return value switch
			{
				EGAErrorSeverity.Debug => "debug", 
				EGAErrorSeverity.Info => "info", 
				EGAErrorSeverity.Warning => "warning", 
				EGAErrorSeverity.Error => "error", 
				EGAErrorSeverity.Critical => "critical", 
				_ => "", 
			};
		}
	}
}
