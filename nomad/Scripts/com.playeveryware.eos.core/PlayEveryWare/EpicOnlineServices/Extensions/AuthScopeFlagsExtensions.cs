using System;
using System.Collections.Generic;
using Epic.OnlineServices.Auth;
using PlayEveryWare.EpicOnlineServices.Utility;

namespace PlayEveryWare.EpicOnlineServices.Extensions
{
	public static class AuthScopeFlagsExtensions
	{
		public static Dictionary<string, AuthScopeFlags> CustomMappings { get; } = new Dictionary<string, AuthScopeFlags>
		{
			{
				"EOS_AS_NoFlags",
				AuthScopeFlags.NoFlags
			},
			{
				"EOS_AS_BasicProfile",
				AuthScopeFlags.BasicProfile
			},
			{
				"EOS_AS_FriendsList",
				AuthScopeFlags.FriendsList
			},
			{
				"EOS_AS_Presence",
				AuthScopeFlags.Presence
			},
			{
				"EOS_AS_FriendsManagement",
				AuthScopeFlags.FriendsManagement
			},
			{
				"EOS_AS_Email",
				AuthScopeFlags.Email
			},
			{
				"EOS_AS_Country",
				AuthScopeFlags.Country
			}
		};

		public static string GetDescription(this AuthScopeFlags flags)
		{
			return flags switch
			{
				AuthScopeFlags.NoFlags => "No flags.", 
				AuthScopeFlags.BasicProfile => "Permissions to see your account ID, display name, and language.", 
				AuthScopeFlags.FriendsList => "Permissions to see a list of your friends who use this application.", 
				AuthScopeFlags.Presence => "Permissions to set your online presence and see presence of your friends.", 
				AuthScopeFlags.FriendsManagement => "Permissions to manage the Epic friends list. This cope is restricted to Epic first party products, and attempting to use it will result in authentication failures.", 
				AuthScopeFlags.Email => "Permissions to see email in the response when fetching information for a user. This scope is restricted to Epic first party products, and attempting to use it will result in authentication failures.", 
				AuthScopeFlags.Country => "Permissions to see your country.", 
				_ => throw new ArgumentOutOfRangeException("flags", flags, null), 
			};
		}

		public static bool TryParse(IList<string> stringFlags, out AuthScopeFlags result)
		{
			return EnumUtility<AuthScopeFlags>.TryParse(stringFlags, CustomMappings, out result, AuthScopeFlags.NoFlags);
		}
	}
}
