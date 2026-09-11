using System;
using Steamworks;
using UnityEngine.UIElements;
using Zorro.Core.CLI;

public class GameStatePage : DebugPage
{
	private VisualTreeAsset m_roomStatsCell;

	public GameStatePage(VisualTreeAsset roomStatsPage, VisualTreeAsset roomStatsCell)
	{
		m_roomStatsCell = roomStatsCell;
		roomStatsPage.CloneTree(this);
		RoomStatsHolder roomStats = SurfaceNetworkHandler.RoomStats;
		if (roomStats == null)
		{
			WriteoutRoomStat("No game state available");
			return;
		}
		WriteoutRoomStat("Time of day: " + TimeOfDayHandler.TimeOfDay);
		WriteoutRoomStat("Current Day: " + roomStats.CurrentDay);
		WriteoutRoomStat("Current Quota Day: " + roomStats.CurrentQuotaDay);
		WriteoutRoomStat("Current Quota: " + roomStats.QuotaToReach);
		WriteoutRoomStat("Current Views: " + roomStats.CurrentQuota);
		WriteoutRoomStat("Match Seed: " + roomStats.MatchSeed);
		SteamLobbyHandler steamLobbyHandler = MainMenuHandler.SteamLobbyHandler;
		if (steamLobbyHandler != null)
		{
			WriteoutRoomStat("Use Steam Network: " + steamLobbyHandler.UseSteamNetwork);
			foreach (SteamNetworkingIdentity member in steamLobbyHandler.Members)
			{
				WriteoutRoomStat($"Steam Lobby Member: {member.GetSteamID()}, type: {member.m_eType}, is invalid: {member.IsInvalid()}");
			}
		}
		WriteoutRoomStat("Has Deals To Select: " + (roomStats.NetworkDealsToSelect.Length != 0));
		NetworkDealBase[] networkDealsToSelect = roomStats.NetworkDealsToSelect;
		foreach (NetworkDealBase networkDealBase in networkDealsToSelect)
		{
			WriteoutRoomStat("Selectable Deal: " + networkDealBase.DealName + " - " + networkDealBase.difficulty);
		}
		AddButton("Set Time of day to Morning", delegate
		{
			TimeOfDayHandler.SetTimeOfDay(TimeOfDay.Morning);
		});
		AddButton("Set Time of day to Evening", delegate
		{
			TimeOfDayHandler.SetTimeOfDay(TimeOfDay.Evening);
		});
	}

	private RoomStatsCell WriteoutRoomStat(string defaultText)
	{
		RoomStatsCell roomStatsCell = new RoomStatsCell(m_roomStatsCell, defaultText);
		roomStatsCell.Q<Label>().text = defaultText;
		Add(roomStatsCell);
		return roomStatsCell;
	}

	public void AddButton(string text, Action onClick)
	{
		Button button = new Button(onClick);
		button.text = text;
		Add(button);
	}
}
