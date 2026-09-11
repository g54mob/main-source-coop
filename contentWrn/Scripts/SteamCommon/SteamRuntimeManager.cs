using System;
using System.Collections;
using Portningsbolaget;
using Portningsbolaget.Platforms;
using Steamworks;
using UnityEngine;

public class SteamRuntimeManager : IPlatform
{
	private enum Achievement
	{
		ACH_WEEKLY_VIEWS_01 = 0,
		ACH_WEEKLY_VIEWS_02 = 1,
		ACH_WEEKLY_VIEWS_03 = 2,
		ACH_WEEKLY_VIEWS_04 = 3,
		ACH_MONITOR_ROOM = 4,
		ACH_FACE = 5,
		ACH_SAVE_VIDEO = 6,
		ACH_ALL_HATS = 7,
		ACH_FIRST_VIDEO = 8,
		ACH_HUNDRED_VIDEO = 9,
		ACH_TRAMPOLINE = 10,
		ACH_PODCAST = 11,
		ACH_GREENSCREEN = 12,
		ACH_CINEMA = 13,
		ACH_POOL = 14,
		ACH_FIRST_MONEY = 15,
		ACH_1000000_MONEY = 16,
		ACH_REVIVE = 17,
		ACH_LEFT_BEHIND = 18,
		ACH_SUMMON_BIGSLAP = 19,
		ACH_CAPTCHA = 20,
		ACH_CAPTCHA_MASTER = 21,
		ACH_DEAL_NORF = 22,
		ACH_DEAL_MONEY = 23,
		ACH_DEAL_REPORTER = 24,
		ACH_DEAL_DANCE = 25,
		ACH_DEAL_DONKEY = 26,
		ACH_DEAL_MULTIMONSTER = 27,
		ACH_DEAL_BOMB = 28,
		ACH_PHONK = 29,
		ACH_VIEWS_01 = 30,
		ACH_VIEWS_02 = 31,
		ACH_VIEWS_03 = 32,
		ACH_VIEWS_04 = 33,
		ACH_CEILING_SLEEP = 34,
		ACH_SHROOM = 35,
		ACH_FILM_BIGSLAP = 36,
		ACH_FILM_FLICKER = 37,
		ACH_FILM_STREAMER = 38,
		ACH_EMOTE_ANCIENT_GESTURE = 39,
		ACH_EMOTE_PEACE = 40,
		ACH_ZAP_BIGSLAP = 41,
		ACH_PWNED = 42,
		ACH_JELLO = 43,
		ACH_RECOVER_CAMERA = 44,
		ACH_PARTY_POPPER = 45,
		ACH_TRAMPOLINE_SLIP = 46,
		ACH_HURT_EAR = 47
	}

	private class Achievement_t
	{
		public Achievement m_eAchievementID;

		public string m_strCodeName;

		public string m_strName;

		public string m_strDescription;

		public bool m_bAchieved;

		public Achievement_t(Achievement achievementID, string codeName)
		{
			m_eAchievementID = achievementID;
			m_strCodeName = codeName;
			m_bAchieved = false;
		}
	}

	private const string STAT_NAME_WEEKS_CLEARED = "WeeksCleared";

	private const string STAT_NAME_DEATHS = "Deaths";

	private const string STAT_NAME_MONEY_EARNED = "Money";

	private const string STAT_NAME_MEDICAL_BILL = "MedicalBill";

	private const string STAT_NAME_VIDEOS_UPLOADED = "Videos";

	private const string STAT_NAME_WEEKLY_VIEWS = "WeeklyViews";

	private const string STAT_NAME_SINGLE_VIDEO_VIEWS = "Views";

	private const string STAT_NAME_CAPTCHAS = "Captchas";

	private Achievement_t[] m_Achievements;

	private CGameID m_GameID;

	private CSteamID m_SteamID;

	private bool m_bRequestedStats;

	private bool m_bStatsValid;

	private bool m_bStoreStats;

	private int m_nWeeksCleared;

	private int m_nDeaths;

	private int m_nCaptchasCompleted;

	private int m_nMoneyEarned;

	private int m_nMedicalBill;

	private int m_nVideosUploaded;

	private int m_nViewsForCurrentVideo;

	private int m_nViewsForCurrentWeek;

	protected Callback<UserStatsReceived_t> m_UserStatsReceived;

	protected Callback<UserStatsStored_t> m_UserStatsStored;

	protected Callback<UserAchievementStored_t> m_UserAchievementStored;

	private static bool m_PopulatedAchievements;

	private bool m_requestingLobbyData;

	protected Callback<GameLobbyJoinRequested_t> m_GameLobbyJoinRequested;

	protected Callback<LobbyDataUpdate_t> m_LobbyDataUpdate;

	private Coroutine m_joinLobbyCoroutine;

	private MonoBehaviour m_coroutineRunner;

	private Callback<GamepadTextInputDismissed_t> m_dialogCallback;

	private Action<string, DialogueResult> m_onDialogDone;

	public bool Initialized => SteamManager.Initialized;

	public string NickName => SteamFriends.GetPersonaName();

	public ulong UserID => m_SteamID.m_SteamID;

	public PlatformUtility.PlatformFamily PlatformFamily => PlatformUtility.PlatformFamily.Windows;

	public bool UsingBigPictureMode => SteamUtils.IsSteamInBigPictureMode();

	public bool OnSteamDeck => SteamUtils.IsSteamRunningOnSteamDeck();

	public Action<string, string> OnJoinedSession { get; set; }

	public void InitializeAfterAssembliesLoaded()
	{
	}

	public void InitializeAfterSceneLoad()
	{
		Debug.LogError($"Steam runtime manager initialized: {SteamManager.Initialized}");
		if (SteamManager.Initialized)
		{
			m_GameID = new CGameID(SteamUtils.GetAppID());
			m_SteamID = SteamUser.GetSteamID();
			GameObject gameObject = new GameObject("[SteamCoroutineRunner]");
			m_coroutineRunner = gameObject.AddComponent<EmptyBehaviour>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
			m_UserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
			m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);
			PopulateAchievements();
			m_LobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(OnLobbyDataUpdated);
			m_GameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnLobbyJoinRequest);
			CheckLaunchCommands();
		}
	}

	public void Update()
	{
		if (SteamManager.Initialized)
		{
			SteamAPI.RunCallbacks();
			CheckStats();
		}
	}

	private void CheckStats()
	{
		if (!m_bRequestedStats)
		{
			if (!SteamManager.Initialized)
			{
				m_bRequestedStats = true;
				return;
			}
			bool bRequestedStats = SteamUserStats.RequestCurrentStats();
			m_bRequestedStats = bRequestedStats;
		}
		if (m_bStatsValid)
		{
			Achievement_t[] achievements = m_Achievements;
			for (int i = 0; i < achievements.Length; i++)
			{
				_ = achievements[i].m_bAchieved;
			}
			if (m_bStoreStats)
			{
				SteamUserStats.SetStat("WeeksCleared", m_nWeeksCleared);
				SteamUserStats.SetStat("Deaths", m_nDeaths);
				SteamUserStats.SetStat("Money", m_nMoneyEarned);
				SteamUserStats.SetStat("MedicalBill", m_nMedicalBill);
				SteamUserStats.SetStat("Videos", m_nVideosUploaded);
				SteamUserStats.SetStat("WeeklyViews", m_nViewsForCurrentWeek);
				SteamUserStats.SetStat("Views", m_nViewsForCurrentVideo);
				SteamUserStats.SetStat("Captchas", m_nCaptchasCompleted);
				bool flag = SteamUserStats.StoreStats();
				m_bStoreStats = !flag;
			}
		}
	}

	private void OnLobbyJoinRequest(GameLobbyJoinRequested_t request)
	{
		JoinLobby(request.m_steamIDLobby);
	}

	private void CheckLaunchCommands()
	{
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		if (commandLineArgs.Length < 2)
		{
			return;
		}
		for (int i = 0; i < commandLineArgs.Length - 1; i++)
		{
			if (commandLineArgs[i].Equals("+connect_lobby", StringComparison.OrdinalIgnoreCase))
			{
				if (ulong.TryParse(commandLineArgs[i + 1], out var result) && result != 0)
				{
					JoinLobby(new CSteamID(result));
				}
				break;
			}
		}
	}

	private void JoinLobby(CSteamID lobbySteamId)
	{
		if (m_joinLobbyCoroutine != null)
		{
			m_coroutineRunner.StopCoroutine(m_joinLobbyCoroutine);
		}
		m_joinLobbyCoroutine = m_coroutineRunner.StartCoroutine(JoinLobbyRoutine(lobbySteamId));
	}

	private IEnumerator JoinLobbyRoutine(CSteamID lobbySteamId)
	{
		Debug.Log($"Joining Steam Lobby: ID {lobbySteamId.m_SteamID}");
		SteamMatchmaking.RequestLobbyData(lobbySteamId);
		m_requestingLobbyData = true;
		while (m_requestingLobbyData)
		{
			yield return null;
		}
		string room = SteamMatchmaking.GetLobbyData(lobbySteamId, "PhotonRoom");
		if (string.IsNullOrEmpty(room))
		{
			Debug.LogError("Invalid Steam Lobby: Missing Room Data");
			yield break;
		}
		string region = SteamMatchmaking.GetLobbyData(lobbySteamId, "PhotonRegion");
		if (string.IsNullOrEmpty(region))
		{
			Debug.LogError("Invalid Steam Lobby: Missing Region Data");
			yield break;
		}
		while (OnJoinedSession == null)
		{
			yield return null;
		}
		OnJoinedSession?.Invoke(region, room);
	}

	private void OnLobbyDataUpdated(LobbyDataUpdate_t data)
	{
		Debug.Log($"Updated Steam Lobby Data: ID {data.m_ulSteamIDLobby}");
		m_requestingLobbyData = false;
	}

	public void OpenDialog(string title, DialogType dialogType, Action<string, DialogueResult> onDone)
	{
		if (!UsingBigPictureMode && !OnSteamDeck)
		{
			Debug.Log("Open Dialog: Not Supported");
			return;
		}
		EGamepadTextInputMode eInputMode = EGamepadTextInputMode.k_EGamepadTextInputModeNormal;
		EGamepadTextInputLineMode eLineInputMode = EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine;
		uint unCharMax = 20u;
		switch (dialogType)
		{
		case DialogType.Chat:
			eLineInputMode = EGamepadTextInputLineMode.k_EGamepadTextInputLineModeMultipleLines;
			unCharMax = 200u;
			break;
		case DialogType.JoinRoom:
			unCharMax = 6u;
			break;
		}
		if (m_dialogCallback == null)
		{
			m_dialogCallback = Callback<GamepadTextInputDismissed_t>.Create(OnDialogDismissed);
		}
		if (!SteamUtils.ShowGamepadTextInput(eInputMode, eLineInputMode, title, unCharMax, string.Empty))
		{
			Debug.LogError("Open Dialog: Failed");
			onDone?.Invoke(string.Empty, DialogueResult.Failed);
		}
		Debug.Log("Opening Dialog...");
		m_onDialogDone = onDone;
	}

	private void OnDialogDismissed(GamepadTextInputDismissed_t callback)
	{
		if (callback.m_bSubmitted)
		{
			if (callback.m_unSubmittedText != 0 && SteamUtils.GetEnteredGamepadTextInput(out var pchText, callback.m_unSubmittedText))
			{
				Debug.Log("Get Dialog Result: Successful");
				m_onDialogDone?.Invoke(pchText, DialogueResult.Succeeded);
			}
			else
			{
				Debug.LogError("Get Dialog Result: Failed");
				m_onDialogDone?.Invoke(string.Empty, DialogueResult.Failed);
			}
		}
		else
		{
			Debug.Log("Get Dialog Result: Aborted");
			m_onDialogDone?.Invoke(string.Empty, DialogueResult.Aborted);
		}
		SteamUtils.DismissGamepadTextInput();
	}

	public void CloseDialog()
	{
		Debug.Log("Closing Dialog...");
		SteamUtils.DismissGamepadTextInput();
	}

	public void Teardown()
	{
	}

	public void VerifyString(string stringToVerify, Action<string> callback)
	{
		callback?.Invoke(stringToVerify);
	}

	public void RequestSave(string fileName, byte[] dataToSave, Action<bool> onSaveOperationFinished)
	{
	}

	public void RequestLoadAsync(string fileName, Action<bool, byte[]> onLoadOperationFinished)
	{
	}

	public IEnumerator RequestLoad(string fileName, Action<bool, byte[]> onLoadOperationFinished)
	{
		yield break;
	}

	public void UnlockAchievement(Achievements achievement)
	{
		UnlockAchievement(m_Achievements[(int)achievement]);
	}

	public void ProgressAchievement(Achievements achievement)
	{
	}

	public void ClearAchievementsProgress()
	{
	}

	private void PopulateAchievements()
	{
		Achievement[] array = (Achievement[])Enum.GetValues(typeof(Achievement));
		m_Achievements = new Achievement_t[array.Length];
		string text = "ACH_";
		string empty = string.Empty;
		for (int i = 0; i < m_Achievements.Length; i++)
		{
			empty = ((i >= 10) ? (text + i) : (text + "0" + i));
			m_Achievements[i] = new Achievement_t(array[i], empty);
			Debug.Log("Ach: " + m_Achievements[i].m_eAchievementID.ToString() + " CodeName: " + m_Achievements[i].m_strCodeName);
		}
		m_PopulatedAchievements = true;
	}

	private void UnlockAchievement(Achievement_t achievement)
	{
		if (achievement.m_bAchieved)
		{
			Debug.Log("Returning Early since Achievement: " + achievement.m_eAchievementID.ToString() + " Already Achieved!");
			return;
		}
		achievement.m_bAchieved = true;
		Debug.Log("Unlocking Achievement: " + achievement.m_eAchievementID);
		SteamUserStats.SetAchievement(achievement.m_strCodeName.ToString());
		m_bStoreStats = true;
	}

	private void OnUserStatsReceived(UserStatsReceived_t pCallback)
	{
		if (!SteamManager.Initialized || (ulong)m_GameID != pCallback.m_nGameID)
		{
			return;
		}
		if (EResult.k_EResultOK == pCallback.m_eResult)
		{
			Debug.Log("Received stats and achievements from Steam\n");
			m_bStatsValid = true;
			Achievement_t[] achievements = m_Achievements;
			foreach (Achievement_t achievement_t in achievements)
			{
				if (SteamUserStats.GetAchievement(achievement_t.m_strCodeName, out achievement_t.m_bAchieved))
				{
					achievement_t.m_strName = SteamUserStats.GetAchievementDisplayAttribute(achievement_t.m_eAchievementID.ToString(), "name");
					achievement_t.m_strDescription = SteamUserStats.GetAchievementDisplayAttribute(achievement_t.m_eAchievementID.ToString(), "desc");
					if (achievement_t.m_bAchieved)
					{
						Debug.Log("SteamUserStats GOT Achievement UNLOCKED " + achievement_t.m_eAchievementID.ToString() + "Name: " + achievement_t.m_strName);
						continue;
					}
					Debug.Log("SteamUserStats GOT Achievement " + achievement_t.m_eAchievementID.ToString() + "Name: " + achievement_t.m_strName + "\n Desc: " + achievement_t.m_strDescription);
				}
				else
				{
					Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + achievement_t.m_eAchievementID.ToString() + "\nIs it registered in the Steam Partner site?");
				}
			}
			SteamUserStats.GetStat("WeeksCleared", out m_nWeeksCleared);
			SteamUserStats.GetStat("Deaths", out m_nDeaths);
			SteamUserStats.GetStat("Money", out m_nMoneyEarned);
			SteamUserStats.GetStat("MedicalBill", out m_nMedicalBill);
			SteamUserStats.GetStat("Videos", out m_nVideosUploaded);
			SteamUserStats.GetStat("WeeklyViews", out m_nViewsForCurrentWeek);
			SteamUserStats.GetStat("Views", out m_nViewsForCurrentVideo);
			SteamUserStats.GetStat("Captchas", out m_nCaptchasCompleted);
			Debug.Log("Got Steam Stats: WeeksCleared : " + m_nWeeksCleared);
			Debug.Log("Got Steam Stats: Deaths : " + m_nDeaths);
			Debug.Log("Got Steam Stats: Money : " + m_nMoneyEarned);
			Debug.Log("Got Steam Stats: MedicalBill : " + m_nMedicalBill);
			Debug.Log("Got Steam Stats: Videos : " + m_nVideosUploaded);
			Debug.Log("Got Steam Stats: WeeklyViews : " + m_nViewsForCurrentWeek);
			Debug.Log("Got Steam Stats: Views : " + m_nViewsForCurrentVideo);
			Debug.Log("Got Steam Stats: Captchas : " + m_nCaptchasCompleted);
		}
		else
		{
			Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
		}
	}

	private void OnUserStatsStored(UserStatsStored_t pCallback)
	{
		if ((ulong)m_GameID == pCallback.m_nGameID)
		{
			if (EResult.k_EResultOK == pCallback.m_eResult)
			{
				Debug.Log("StoreStats - success");
			}
			else if (EResult.k_EResultInvalidParam == pCallback.m_eResult)
			{
				Debug.Log("StoreStats - some failed to validate");
				OnUserStatsReceived(new UserStatsReceived_t
				{
					m_eResult = EResult.k_EResultOK,
					m_nGameID = (ulong)m_GameID
				});
			}
			else
			{
				Debug.Log("StoreStats - failed, " + pCallback.m_eResult);
			}
		}
	}

	private void OnAchievementStored(UserAchievementStored_t pCallback)
	{
		if ((ulong)m_GameID == pCallback.m_nGameID)
		{
			if (pCallback.m_nMaxProgress == 0)
			{
				Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
				return;
			}
			Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' progress callback, (" + pCallback.m_nCurProgress + "," + pCallback.m_nMaxProgress + ")");
		}
	}

	public void AddWeeksCleared()
	{
		m_nWeeksCleared++;
	}

	public void AddDeath(int medicalBill)
	{
		m_nDeaths++;
		m_nMedicalBill += medicalBill;
		m_bStoreStats = true;
	}

	public void OnCaptchaCompleted()
	{
		UnlockAchievement(Achievements.ACH_CAPTCHA);
		m_nCaptchasCompleted++;
		m_bStoreStats = true;
	}

	public void OnVideoUploaded(int views, int money, int weeklyViews)
	{
		m_nViewsForCurrentVideo = views;
		m_nViewsForCurrentWeek = weeklyViews;
		m_nMoneyEarned += money;
		m_nVideosUploaded++;
		m_bStoreStats = true;
	}
}
