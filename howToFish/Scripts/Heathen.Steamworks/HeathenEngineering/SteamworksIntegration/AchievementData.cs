using System;
using System.Linq;
using HeathenEngineering.SteamworksIntegration.API;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public struct AchievementData : IEquatable<AchievementData>, IEquatable<string>, IComparable<AchievementData>, IComparable<string>
	{
		[NonSerialized]
		private AchievementObject _so;

		[SerializeField]
		private string id;

		public readonly string Name => StatsAndAchievements.Client.GetAchievementDisplayAttribute(id, AchievementAttributes.name);

		public readonly string Description => StatsAndAchievements.Client.GetAchievementDisplayAttribute(id, AchievementAttributes.desc);

		public readonly bool Hidden => StatsAndAchievements.Client.GetAchievementDisplayAttribute(id, AchievementAttributes.hidden) == "1";

		public AchievementObject ScriptableObject
		{
			get
			{
				if (SteamSettings.current == null)
				{
					return null;
				}
				if (_so == null)
				{
					AchievementData nId = this;
					_so = SteamSettings.Achievements.FirstOrDefault((AchievementObject p) => p.Id == nId);
				}
				return _so;
			}
			set
			{
				_so = value;
				id = value.Id;
			}
		}

		public readonly string ApiName => id;

		public readonly bool IsAchieved
		{
			get
			{
				if (StatsAndAchievements.Client.GetAchievement(id, out var achieved))
				{
					return achieved;
				}
				return false;
			}
			set
			{
				if (value)
				{
					StatsAndAchievements.Client.SetAchievement(id);
				}
				else
				{
					StatsAndAchievements.Client.ClearAchievement(id);
				}
			}
		}

		public readonly DateTime? UnlockTime
		{
			get
			{
				if (StatsAndAchievements.Client.GetAchievement(id, out var _, out var unlockTime))
				{
					return unlockTime;
				}
				return null;
			}
		}

		public readonly float GlobalPercent
		{
			get
			{
				StatsAndAchievements.Client.GetAchievementAchievedPercent(id, out var percent);
				return percent;
			}
		}

		public readonly void Unlock()
		{
			IsAchieved = true;
		}

		public readonly void ClearAchievement()
		{
			IsAchieved = false;
		}

		public readonly void Unlock(UserData user)
		{
			StatsAndAchievements.Server.SetUserAchievement(user, id);
		}

		public readonly void ClearAchievement(UserData user)
		{
			StatsAndAchievements.Server.ClearUserAchievement(user, id);
		}

		public readonly bool GetAchievementStatus(UserData user)
		{
			StatsAndAchievements.Client.GetAchievement(user, id, out var achieved);
			return achieved;
		}

		public readonly (bool unlocked, DateTime unlockTime) GetAchievementAndUnlockTime(UserData user)
		{
			StatsAndAchievements.Client.GetAchievement(user, id, out var achieved, out var unlockTime);
			return (unlocked: achieved, unlockTime: unlockTime);
		}

		public readonly void GetIcon(Action<Texture2D> callback)
		{
			StatsAndAchievements.Client.GetAchievementIcon(id, callback);
		}

		public readonly void Store()
		{
			StatsAndAchievements.Client.StoreStats();
		}

		public static AchievementData Get(string apiName)
		{
			return apiName;
		}

		public readonly AchievementObject CreateScriptableObject()
		{
			AchievementObject achievementObject = UnityEngine.ScriptableObject.CreateInstance<AchievementObject>();
			achievementObject.Id = this;
			return achievementObject;
		}

		public static AchievementObject CreateScriptableObject(string apiName)
		{
			AchievementObject achievementObject = UnityEngine.ScriptableObject.CreateInstance<AchievementObject>();
			achievementObject.Id = apiName;
			return achievementObject;
		}

		public override readonly string ToString()
		{
			return id;
		}

		public readonly bool Equals(string other)
		{
			return id.Equals(other);
		}

		public readonly bool Equals(AchievementData other)
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

		public readonly int CompareTo(AchievementData other)
		{
			return id.CompareTo(other.id);
		}

		public readonly int CompareTo(string other)
		{
			return id.CompareTo(other);
		}

		public static bool operator ==(AchievementData l, AchievementData r)
		{
			return l.id == r.id;
		}

		public static bool operator ==(string l, AchievementData r)
		{
			return l == r.id;
		}

		public static bool operator ==(AchievementData l, string r)
		{
			return l.id == r;
		}

		public static bool operator !=(AchievementData l, AchievementData r)
		{
			return l.id != r.id;
		}

		public static bool operator !=(string l, AchievementData r)
		{
			return l != r.id;
		}

		public static bool operator !=(AchievementData l, string r)
		{
			return l.id != r;
		}

		public static implicit operator string(AchievementData c)
		{
			return c.id;
		}

		public static implicit operator AchievementData(string id)
		{
			return new AchievementData
			{
				id = id
			};
		}
	}
}
