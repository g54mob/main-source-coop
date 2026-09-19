using System.Collections.Generic;
using GameAnalyticsSDK.Wrapper;

namespace GameAnalyticsSDK.Events
{
	public static class GA_Design
	{
		public static void NewEvent(string eventName, float eventValue, IDictionary<string, object> fields, bool mergeFields)
		{
			CreateNewEvent(eventName, eventValue, fields, mergeFields);
		}

		public static void NewEvent(string eventName, IDictionary<string, object> fields, bool mergeFields)
		{
			CreateNewEvent(eventName, null, fields, mergeFields);
		}

		private static void CreateNewEvent(string eventName, float? eventValue, IDictionary<string, object> fields, bool mergeFields)
		{
			if (eventValue.HasValue)
			{
				GA_Wrapper.AddDesignEvent(eventName, eventValue.Value, fields, mergeFields);
			}
			else
			{
				GA_Wrapper.AddDesignEvent(eventName, fields, mergeFields);
			}
		}
	}
}
