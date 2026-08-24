using System;
using HeathenEngineering.SteamworksIntegration.API;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public struct StatData : IEquatable<StatData>, IEquatable<string>, IComparable<StatData>, IComparable<string>
	{
		[SerializeField]
		private string id;

		public readonly float FloatValue()
		{
			StatsAndAchievements.Client.GetStat(id, out float data);
			return data;
		}

		public readonly int IntValue()
		{
			StatsAndAchievements.Client.GetStat(id, out int data);
			return data;
		}

		public readonly void RequestUserStats(UserData user, Action<UserStatsReceived, bool> callback)
		{
			StatsAndAchievements.Client.RequestUserStats(user, callback);
		}

		public readonly bool GetValue(UserData user, out int value)
		{
			return StatsAndAchievements.Client.GetStat(user, (string)this, out value);
		}

		public readonly bool GetValue(UserData user, out float value)
		{
			return StatsAndAchievements.Client.GetStat(user, (string)this, out value);
		}

		public readonly void Set(float value)
		{
			StatsAndAchievements.Client.SetStat(id, value);
		}

		public readonly void Set(int value)
		{
			StatsAndAchievements.Client.SetStat(id, value);
		}

		public readonly void Set(float value, double length)
		{
			StatsAndAchievements.Client.UpdateAvgRateStat(id, value, length);
		}

		public readonly void Store()
		{
			StatsAndAchievements.Client.StoreStats();
		}

		public readonly void ServerSetValue(UserData user, int value)
		{
			StatsAndAchievements.Server.SetUserStat(user, this, value);
		}

		public readonly void ServerSetValue(UserData user, float value)
		{
			StatsAndAchievements.Server.SetUserStat(user, this, value);
		}

		public readonly bool ServerGetValue(UserData user, out int value)
		{
			return StatsAndAchievements.Server.GetUserStat((CSteamID)user, (string)this, out value);
		}

		public readonly bool ServerGetValue(UserData user, out float value)
		{
			return StatsAndAchievements.Server.GetUserStat((CSteamID)user, (string)this, out value);
		}

		public readonly bool Equals(string other)
		{
			return id.Equals(other);
		}

		public readonly bool Equals(StatData other)
		{
			return id.Equals(other.id);
		}

		public override readonly bool Equals(object obj)
		{
			return id.Equals(obj);
		}

		public override readonly int GetHashCode()
		{
			return id.GetHashCode();
		}

		public readonly int CompareTo(StatData other)
		{
			return id.CompareTo(other.id);
		}

		public readonly int CompareTo(string other)
		{
			return id.CompareTo(other);
		}

		public static bool operator ==(StatData l, StatData r)
		{
			return l.id == r.id;
		}

		public static bool operator ==(string l, StatData r)
		{
			return l == r.id;
		}

		public static bool operator ==(StatData l, string r)
		{
			return l.id == r;
		}

		public static bool operator !=(StatData l, StatData r)
		{
			return l.id != r.id;
		}

		public static bool operator !=(string l, StatData r)
		{
			return l != r.id;
		}

		public static bool operator !=(StatData l, string r)
		{
			return l.id != r;
		}

		public static implicit operator string(StatData c)
		{
			return c.id;
		}

		public static implicit operator StatData(string id)
		{
			return new StatData
			{
				id = id
			};
		}
	}
}
