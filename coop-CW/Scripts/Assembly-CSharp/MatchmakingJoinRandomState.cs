using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Portningsbolaget;
using Steamworks;
using UnityEngine;
using Zorro.Core;
using Zorro.UI;

public class MatchmakingJoinRandomState : MatchmakingState
{
	public class Task
	{
		public LanguageMatchmakingSetting.MatchmakingLanguage language;

		public ELobbyDistanceFilter distanceFilter;

		public Task()
		{
			distanceFilter = ELobbyDistanceFilter.k_ELobbyDistanceFilterDefault;
		}
	}

	private const string PHOTON_REGION_KEY = "PhotonRegion";

	private const string GAME_VERSION_KEY = "ContentWarningVersion";

	private const string MAIN_LANGUAGE_KEY = "MainLanguage";

	private const string PLUGINS_KEY = "Plugins";

	private CallResult<LobbyMatchList_t> m_OnMatchListCallResult;

	private UIPageHandler m_uiPageHandler;

	private List<Task> m_tasks = new List<Task>();

	public override void Enter()
	{
		base.Enter();
		m_tasks.Clear();
	}

	public override void Exit()
	{
		base.Exit();
		m_tasks.Clear();
	}

	public MatchmakingJoinRandomState(UIPageHandler pageHandler)
	{
		m_uiPageHandler = pageHandler;
		m_OnMatchListCallResult = CallResult<LobbyMatchList_t>.Create(OnMatchListReceived);
	}

	public void StartMatchmaking()
	{
		LanguageMatchmakingSetting.MatchmakingLanguage value = (LanguageMatchmakingSetting.MatchmakingLanguage)GameHandler.Instance.SettingsHandler.GetSetting<PreferredLanguageMatchmakingSetting>().Value;
		ELobbyDistanceFilter eLobbyDistanceFilter = ELobbyDistanceFilter.k_ELobbyDistanceFilterDefault;
		for (int i = 0; i < 4; i++)
		{
			eLobbyDistanceFilter = (ELobbyDistanceFilter)i;
			if (eLobbyDistanceFilter > ELobbyDistanceFilter.k_ELobbyDistanceFilterDefault)
			{
				eLobbyDistanceFilter = ELobbyDistanceFilter.k_ELobbyDistanceFilterDefault;
			}
			m_tasks.Add(new Task
			{
				language = value,
				distanceFilter = eLobbyDistanceFilter
			});
		}
		if (GameHandler.Instance.SettingsHandler.GetSetting<SecondLanguageMatchmakingSetting>().Value != 0)
		{
			LanguageMatchmakingSetting.MatchmakingLanguage value2 = (LanguageMatchmakingSetting.MatchmakingLanguage)GameHandler.Instance.SettingsHandler.GetSetting<SecondLanguageMatchmakingSetting>().Value;
			for (int j = 0; j < 2; j++)
			{
				eLobbyDistanceFilter = (ELobbyDistanceFilter)j;
				m_tasks.Add(new Task
				{
					language = value2,
					distanceFilter = eLobbyDistanceFilter
				});
			}
		}
		if (GameHandler.Instance.SettingsHandler.GetSetting<ThirdLanguageMatchmakingSetting>().Value != 0)
		{
			LanguageMatchmakingSetting.MatchmakingLanguage value3 = (LanguageMatchmakingSetting.MatchmakingLanguage)GameHandler.Instance.SettingsHandler.GetSetting<ThirdLanguageMatchmakingSetting>().Value;
			for (int k = 0; k < 2; k++)
			{
				eLobbyDistanceFilter = (ELobbyDistanceFilter)k;
				m_tasks.Add(new Task
				{
					language = value3,
					distanceFilter = eLobbyDistanceFilter
				});
			}
		}
		Debug.Log($"Started matchmaking with {m_tasks.Count} tasks!");
		NextTask();
	}

	private void NextTask()
	{
		base.MatchmakingHandler.StartCoroutine(NextTaskCoroutine());
		IEnumerator NextTaskCoroutine()
		{
			yield return new WaitForSecondsRealtime(0.35f);
			Debug.Log("Going to next task!");
			if (m_tasks.Count == 0)
			{
				Debug.Log("Found No Matches hosting");
				base.MatchmakingHandler.StopMatchmaking();
				LanguageMatchmakingSetting.MatchmakingLanguage value = (LanguageMatchmakingSetting.MatchmakingLanguage)GameHandler.Instance.SettingsHandler.GetSetting<PreferredLanguageMatchmakingSetting>().Value;
				MainMenuLoadingPage mainMenuLoadingPage = m_uiPageHandler.TransistionToPage<MainMenuLoadingPage>();
				string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.HostingGame);
				mainMenuLoadingPage.SetText(localizedString + Environment.NewLine + value);
				UnityEngine.Object.FindObjectOfType<MainMenuHandler>().SilentHost();
			}
			else
			{
				Task task = m_tasks[0];
				m_tasks.RemoveAt(0);
				MatchmakeForLanguage(task);
			}
		}
	}

	private void MatchmakeForLanguage(Task task)
	{
		LanguageMatchmakingSetting.MatchmakingLanguage language = task.language;
		ELobbyDistanceFilter distanceFilter = task.distanceFilter;
		Debug.Log("Matchmaking for language: " + language.ToString() + " Distance: " + distanceFilter);
		MainMenuLoadingPage mainMenuLoadingPage = m_uiPageHandler.TransistionToPage<MainMenuLoadingPage>();
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.JoinRandom);
		mainMenuLoadingPage.SetText(localizedString + Environment.NewLine + language);
		string matchmakingVersion = PlatformsConfig.GetMatchmakingVersion();
		SteamMatchmaking.AddRequestLobbyListStringFilter("ContentWarningVersion", matchmakingVersion, ELobbyComparison.k_ELobbyComparisonEqual);
		SteamMatchmaking.AddRequestLobbyListStringFilter("MainLanguage", language.ToString(), ELobbyComparison.k_ELobbyComparisonEqual);
		SteamMatchmaking.AddRequestLobbyListStringFilter("Plugins", GameHandler.GetPluginHash(), ELobbyComparison.k_ELobbyComparisonEqual);
		SteamMatchmaking.AddRequestLobbyListResultCountFilter(10);
		SteamMatchmaking.AddRequestLobbyListDistanceFilter(distanceFilter);
		SteamAPICall_t hAPICall = SteamMatchmaking.RequestLobbyList();
		m_OnMatchListCallResult.Set(hAPICall);
	}

	private void OnMatchListReceived(LobbyMatchList_t param, bool biofailure)
	{
		if (!activeState)
		{
			Debug.LogError("MatchmakingJoinRoomState is not active, ignoring match list callback");
			NextTask();
			return;
		}
		if (biofailure)
		{
			Debug.LogError("Matchlist Biofail");
			NextTask();
			return;
		}
		if (param.m_nLobbiesMatching == 0)
		{
			Debug.Log("Found No Matches hosting");
			NextTask();
			return;
		}
		Debug.Log("Filtering SteamLobby Matchlist, Match count: " + param.m_nLobbiesMatching);
		List<(CSteamID, int)> list = new List<(CSteamID, int)>();
		for (int i = 0; i < param.m_nLobbiesMatching; i++)
		{
			CSteamID lobbyByIndex = SteamMatchmaking.GetLobbyByIndex(i);
			string lobbyData = SteamMatchmaking.GetLobbyData(lobbyByIndex, "ContentWarningVersion");
			string lobbyData2 = SteamMatchmaking.GetLobbyData(lobbyByIndex, "PhotonRegion");
			int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(lobbyByIndex);
			int lobbyMemberLimit = SteamMatchmaking.GetLobbyMemberLimit(lobbyByIndex);
			string[] obj = new string[10] { "Checking Lobby: ", null, null, null, null, null, null, null, null, null };
			CSteamID cSteamID = lobbyByIndex;
			obj[1] = cSteamID.ToString();
			obj[2] = " GameVersion: ";
			obj[3] = lobbyData;
			obj[4] = "Region: ";
			obj[5] = lobbyData2;
			obj[6] = "Number of players: ";
			obj[7] = numLobbyMembers.ToString();
			obj[8] = " / ";
			obj[9] = lobbyMemberLimit.ToString();
			Debug.Log(string.Concat(obj));
			string cloudRegion = PhotonNetwork.CloudRegion;
			bool flag = !string.IsNullOrEmpty(lobbyData2) && lobbyData2 == cloudRegion;
			if (lobbyData == PlatformsConfig.GetMatchmakingVersion() && flag && numLobbyMembers < 4)
			{
				list.Add((lobbyByIndex, i));
			}
			else
			{
				Debug.Log("GameVersion Mismatch: " + lobbyData + " Current: " + PlatformsConfig.GetMatchmakingVersion());
			}
		}
		Debug.Log("Received SteamLobby Matchlist: " + param.m_nLobbiesMatching + " Matching: " + list.Count);
		if (list.Count > 0)
		{
			base.MatchmakingHandler.StopMatchmaking();
			CSteamID lobbyByIndex2 = SteamMatchmaking.GetLobbyByIndex(list.GetRandom().Item2);
			MainMenuHandler.SteamLobbyHandler.JoinLobby(lobbyByIndex2);
		}
		else
		{
			NextTask();
		}
	}
}
