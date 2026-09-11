using System;
using Discord;
using Steamworks;
using UnityEngine;

public static class RichPresenceHandler
{
	private static global::Discord.Discord _discord;

	private static RichPresenceState _currentState;

	private static int _currentDay;

	private static string _currentViews;

	private static string _currentQuota;

	private static string _currentGroup;

	private static int _currentPlayersInGroup;

	private static int _currentMaxPlayersInGroup;

	private static bool m_isDiscordDirty;

	public static void SetPresenceState(RichPresenceState state)
	{
		SteamFriends.SetRichPresence("steam_display", "#" + state);
		_currentState = state;
		DirtyDiscord();
	}

	public static void SetGroup(string group)
	{
		_currentGroup = group;
		SteamFriends.SetRichPresence("steam_player_group", group);
		DirtyDiscord();
	}

	public static void SetGroupSize(int playersInServer, int maxSize)
	{
		_currentPlayersInGroup = playersInServer;
		_currentMaxPlayersInGroup = maxSize;
		SteamFriends.SetRichPresence("steam_player_group_size", playersInServer.ToString());
		SteamFriends.SetRichPresence("players", playersInServer.ToString());
		SteamFriends.SetRichPresence("max_players", maxSize.ToString());
		DirtyDiscord();
	}

	public static void Clear()
	{
		_currentDay = 0;
		_currentViews = "";
		_currentGroup = "";
		_currentPlayersInGroup = 0;
		_currentMaxPlayersInGroup = 0;
		_currentState = RichPresenceState.Status_MainMenu;
		SteamFriends.ClearRichPresence();
		DirtyDiscord();
	}

	public static void SetRoomsStats(string views, string quota, int currentDay)
	{
		_currentQuota = quota;
		_currentViews = views;
		_currentDay = currentDay;
		SteamFriends.SetRichPresence("views", views);
		SteamFriends.SetRichPresence("quota", quota);
		SteamFriends.SetRichPresence("day", currentDay.ToString());
		DirtyDiscord();
	}

	private static void DirtyDiscord()
	{
		m_isDiscordDirty = true;
	}

	public static void Update()
	{
		if (m_isDiscordDirty)
		{
			m_isDiscordDirty = false;
			UpdateDiscord();
		}
		_discord?.RunCallbacks();
	}

	private static void UpdateDiscord()
	{
		if (_discord != null)
		{
			Debug.Log("Updating Discord Rich Presence");
			ActivityManager activityManager = _discord.GetActivityManager();
			Activity activity = new Activity
			{
				State = GetStateString(),
				Details = GetDetailsString(),
				Assets = new ActivityAssets
				{
					LargeImage = "icon",
					LargeText = "Content Warning. Out now on steam"
				}
			};
			if (_currentPlayersInGroup > 0)
			{
				activity.Party = new ActivityParty
				{
					Id = _currentGroup,
					Size = new PartySize
					{
						CurrentSize = _currentPlayersInGroup,
						MaxSize = _currentMaxPlayersInGroup
					},
					Privacy = ActivityPartyPrivacy.Public
				};
			}
			activityManager.UpdateActivity(activity, delegate(Result result)
			{
				Debug.Log("Discord Rich Presence Updated, result: " + result);
			});
		}
	}

	private static string GetDetailsString()
	{
		switch (_currentState)
		{
		case RichPresenceState.Status_MainMenu:
			return "In Main Menu";
		case RichPresenceState.Status_AtHouse:
		case RichPresenceState.Status_InFactory:
		case RichPresenceState.Status_InShip:
			return "Day " + _currentDay + " - " + _currentViews + "/" + _currentQuota + " views";
		default:
			return "";
		}
	}

	private static string GetStateString()
	{
		switch (_currentState)
		{
		case RichPresenceState.Status_PlayingWithFriends:
			return "In friends lobby";
		case RichPresenceState.Status_PlayingWithRandoms:
			return "In public lobby";
		case RichPresenceState.Status_AtHouse:
			return "At the house";
		case RichPresenceState.Status_InFactory:
		case RichPresenceState.Status_InShip:
			return "In the old world";
		default:
			return "";
		}
	}

	public static void Initialize()
	{
		try
		{
			_discord = new global::Discord.Discord(1225839934888218725L, 1uL);
		}
		catch (Exception arg)
		{
			Debug.Log($"Failed to initialize Discord Rich Presence: {arg}");
		}
	}

	public static void Deinitialize()
	{
		_discord?.Dispose();
	}
}
