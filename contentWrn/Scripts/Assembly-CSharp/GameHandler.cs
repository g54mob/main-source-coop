using System.Collections;
using DefaultNamespace;
using Photon.Pun;
using Portningsbolaget.Photon;
using Portningsbolaget.Platforms;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zorro.ControllerSupport;
using Zorro.Core;
using Zorro.Core.CLI;
using Zorro.PhotonUtility;

public class GameHandler : DisableIntercept
{
	private static GameHandler _instance;

	private static Hash128? m_pluginHash;

	public static GameHandler Instance => _instance;

	public SettingsHandler SettingsHandler { get; private set; }

	public void Initialize()
	{
		_instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
		DoPluginThings();
		SettingsHandler = new SettingsHandler();
		SingletonAsset<LostFootageDatabase>.Instance.Init();
		RichPresenceHandler.Initialize();
		ContentCombiningSystem.Init();
		RetrievableResourceSingleton<InputHandler>.Instance.Initialize(SteamUtils.IsSteamRunningOnSteamDeck);
		MetaProgressionHandler.Init();
		RetrievableResourceSingleton<DebugToolsManager>.Instance.Initialize(() => false);
		CustomCommandListener<CustomCommandType>.RegisterListener<KickPlayerNotificationPackage>(OnKickNotifactionReceived);
	}

	private static async void DoPluginThings()
	{
		await PluginHandler.DoPluginThings();
		m_pluginHash = PluginHandler.CheckPlugins();
	}

	private void OnKickNotifactionReceived(KickPlayerNotificationPackage obj)
	{
		Modal.ShowError(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Modal_Kicked_Title), LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Modal_Kicked_Body));
		RetrievableSingleton<ConnectionStateHandler>.Instance.Disconnect();
		SceneManager.LoadScene("NewMainMenuOptimized");
	}

	private void Start()
	{
		SettingsHandler.RegisterPage();
		SingletonAsset<RecordingPageInitter>.Instance.Init();
	}

	private void OnDestroy()
	{
		SettingsHandler.Dispose();
		RichPresenceHandler.Deinitialize();
	}

	private void OnApplicationQuit()
	{
	}

	private void Update()
	{
		PhotonAuthManager.Update();
		PlatformManager.Update();
	}

	private void LateUpdate()
	{
		if (PluginHandler.needsHashRefresh)
		{
			m_pluginHash = PluginHandler.CheckPlugins();
		}
		SettingsHandler.Update();
		RichPresenceHandler.Update();
	}

	[ConsoleCommand]
	public static void JoinRandom()
	{
		MainMenuHandler.Instance.JoinRandom();
	}

	public static string GetPluginHash()
	{
		if (PluginHandler.needsHashRefresh)
		{
			m_pluginHash = PluginHandler.CheckPlugins();
		}
		if (!m_pluginHash.HasValue)
		{
			return "none";
		}
		return m_pluginHash.ToString();
	}

	public override bool CanIntercept()
	{
		return !m_pluginHash.HasValue;
	}

	public static Coroutine StartGlobalCoroutine(IEnumerator routine)
	{
		return Instance.StartCoroutine(routine);
	}
}
