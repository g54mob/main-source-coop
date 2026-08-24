using System;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	[HelpURL("https://kb.heathenengineering.com/assets/steamworks/stats-object")]
	public abstract class StatObject : ScriptableObject
	{
		public enum DataType
		{
			Int = 0,
			Float = 1,
			AvgRate = 2
		}

		[HideInInspector]
		public StatData data;

		public abstract DataType Type { get; }

		public int GetIntValue()
		{
			return data.IntValue();
		}

		public float GetFloatValue()
		{
			return data.FloatValue();
		}

		public void RequestUserStats(UserData user, Action<UserStatsReceived, bool> callback)
		{
			data.RequestUserStats(user, callback);
		}

		public bool GetValue(UserData user, out int value)
		{
			return data.GetValue(user, out value);
		}

		public bool GetValue(UserData user, out float value)
		{
			return data.GetValue(user, out value);
		}

		public void SetIntStat(int value)
		{
			data.Set(value);
		}

		public void SetFloatStat(float value)
		{
			data.Set(value);
		}

		public void AddFloatStat(float value)
		{
			SetFloatStat(GetFloatValue() + value);
		}

		public void AddIntStat(int value)
		{
			SetIntStat(GetIntValue() + value);
		}

		public void StoreStats()
		{
			data.Store();
		}
	}
}
