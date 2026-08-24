using System;
using HeathenEngineering.SteamworksIntegration.API;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public struct UserData : IEquatable<CSteamID>, IEquatable<ulong>, IEquatable<UserData>
	{
		public CSteamID id;

		public static UserData Me => User.Client.Id;

		public readonly bool IsMe => id == User.Client.Id.id;

		public readonly ulong SteamId => id.m_SteamID;

		public readonly bool IsValid
		{
			get
			{
				if (id == CSteamID.Nil || id.GetEAccountType() != EAccountType.k_EAccountTypeIndividual || id.GetEUniverse() != EUniverse.k_EUniversePublic)
				{
					return false;
				}
				return true;
			}
		}

		public readonly Texture2D Avatar => Friends.Client.GetLoadedAvatar(id);

		public readonly string Name => Friends.Client.GetFriendPersonaName(id);

		public readonly string Nickname
		{
			get
			{
				string playerNickname = Friends.Client.GetPlayerNickname(id);
				if (!string.IsNullOrEmpty(playerNickname))
				{
					return playerNickname;
				}
				return Friends.Client.GetFriendPersonaName(id);
			}
		}

		public readonly EPersonaState State => SteamFriends.GetFriendPersonaState(id);

		public readonly bool InGame
		{
			get
			{
				FriendGameInfo_t pFriendGameInfo;
				return SteamFriends.GetFriendGamePlayed(id, out pFriendGameInfo);
			}
		}

		public readonly FriendGameInfo_t GameInfo
		{
			get
			{
				SteamFriends.GetFriendGamePlayed(id, out var pFriendGameInfo);
				return pFriendGameInfo;
			}
		}

		public readonly int Level => SteamFriends.GetFriendSteamLevel(id);

		public readonly AccountID_t AccountId => id.GetAccountID();

		public readonly uint FriendId => AccountId.m_AccountID;

		public readonly string HexId => FriendId.ToString("X");

		public readonly string[] NameHistory => Friends.Client.GetFriendPersonaNameHistory(this);

		public readonly void LoadAvatar(Action<Texture2D> callback)
		{
			Friends.Client.GetFriendAvatar(id, callback);
		}

		public readonly bool GetGamePlayed(out FriendGameInfo gameInfo)
		{
			return Friends.Client.GetFriendGamePlayed(id, out gameInfo);
		}

		public readonly void InviteToGame(string connectString)
		{
			Friends.Client.InviteUserToGame(this, connectString);
		}

		public readonly bool SendMessage(string message)
		{
			return Friends.Client.ReplyToFriendMessage(this, message);
		}

		public readonly bool RequestInformation()
		{
			return Friends.Client.RequestUserInformation(this, nameOnly: false);
		}

		public readonly string GetRichPresenceValue(string key)
		{
			return Friends.Client.GetFriendRichPresence(this, key);
		}

		public readonly void AddFriend()
		{
			AddFriend(this);
		}

		public readonly void RemoveFriend()
		{
			RemoveFriend(this);
		}

		public readonly void SetPlayedWith()
		{
			Friends.Client.SetPlayedWith(this);
		}

		public static void ClearRichPresence()
		{
			SteamFriends.ClearRichPresence();
		}

		public static UserData Get(string accountId)
		{
			uint num = Convert.ToUInt32(accountId, 16);
			if (num != 0)
			{
				return Get(num);
			}
			return CSteamID.Nil;
		}

		public static UserData Get(ulong id)
		{
			return new UserData
			{
				id = new CSteamID(id)
			};
		}

		public static UserData Get(CSteamID id)
		{
			return new UserData
			{
				id = id
			};
		}

		public static UserData Get()
		{
			return User.Client.Id;
		}

		public static UserData Get(uint accountId)
		{
			return Get(new AccountID_t(accountId));
		}

		public static UserData Get(AccountID_t accountId)
		{
			return new CSteamID(accountId, EUniverse.k_EUniversePublic, EAccountType.k_EAccountTypeIndividual);
		}

		public static bool SetRichPresence(string key, string value)
		{
			return SteamFriends.SetRichPresence(key, value);
		}

		public static bool AddFriend(string friendId)
		{
			if (uint.TryParse(friendId, out var result))
			{
				AddFriend(result);
				return true;
			}
			return false;
		}

		public static void AddFriend(uint friendId)
		{
			AddFriend(Get(friendId));
		}

		public static void AddFriend(UserData user)
		{
			Overlay.Client.Activate(FriendDialog.friendadd, user);
		}

		public static void AddFriend(AccountID_t user)
		{
			Overlay.Client.Activate(FriendDialog.friendadd, Get(user));
		}

		public static void RemoveFriend(UserData user)
		{
			Overlay.Client.Activate(FriendDialog.friendremove, user);
		}

		public static void RemoveFriend(AccountID_t user)
		{
			Overlay.Client.Activate(FriendDialog.friendremove, Get(user));
		}

		public readonly int CompareTo(UserData other)
		{
			return id.CompareTo(other.id);
		}

		public readonly int CompareTo(CSteamID other)
		{
			return id.CompareTo(other);
		}

		public readonly int CompareTo(ulong other)
		{
			return id.m_SteamID.CompareTo(other);
		}

		public override readonly string ToString()
		{
			return HexId;
		}

		public readonly bool Equals(UserData other)
		{
			return id.Equals(other.id);
		}

		public readonly bool Equals(CSteamID other)
		{
			return id.Equals(other);
		}

		public readonly bool Equals(ulong other)
		{
			return id.m_SteamID.Equals(other);
		}

		public override readonly bool Equals(object obj)
		{
			return id.m_SteamID.Equals(obj);
		}

		public override readonly int GetHashCode()
		{
			return id.GetHashCode();
		}

		public static bool operator ==(UserData l, UserData r)
		{
			return l.id == r.id;
		}

		public static bool operator ==(CSteamID l, UserData r)
		{
			return l == r.id;
		}

		public static bool operator ==(UserData l, CSteamID r)
		{
			return l.id == r;
		}

		public static bool operator !=(UserData l, UserData r)
		{
			return l.id != r.id;
		}

		public static bool operator !=(CSteamID l, UserData r)
		{
			return l != r.id;
		}

		public static bool operator !=(UserData l, CSteamID r)
		{
			return l.id != r;
		}

		public static implicit operator ulong(UserData c)
		{
			return c.id.m_SteamID;
		}

		public static implicit operator UserData(ulong id)
		{
			return new UserData
			{
				id = new CSteamID(id)
			};
		}

		public static implicit operator CSteamID(UserData c)
		{
			return c.id;
		}

		public static implicit operator UserData(CSteamID id)
		{
			return new UserData
			{
				id = id
			};
		}
	}
}
