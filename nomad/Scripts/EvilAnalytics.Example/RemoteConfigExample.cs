using System.Collections;
using System.Collections.Generic;
using EvilAnalytics.SDK.Core;
using UnityEngine;

public class RemoteConfigExample : MonoBehaviour
{
	private bool _isReady;

	private bool _configFetched;

	private int _enemyHealth = 100;

	private float _playerSpeed = 5f;

	private bool _doubleXpEnabled;

	private string _welcomeMessage = "Welcome!";

	private string _statusText = "Press 'Fetch Config' to load remote values";

	private IEnumerator Start()
	{
		while (EvilAnalyticsManager.Instance == null || !EvilAnalyticsManager.Instance.IsReady)
		{
			yield return null;
		}
		_isReady = true;
		SetDefaults();
		Debug.Log("[RemoteConfigExample] Ready! Press 'Fetch Config' to load remote values.");
	}

	private void SetDefaults()
	{
		Analytics.RemoteConfig.SetDefaults(new Dictionary<string, object>
		{
			{ "enemy_health", 100 },
			{ "player_speed", 5f },
			{ "double_xp_enabled", false },
			{ "welcome_message", "Welcome to the game!" },
			{ "daily_reward_coins", 50 },
			{ "max_lives", 3 }
		});
	}

	public async void FetchConfig()
	{
		if (_isReady)
		{
			_statusText = "Fetching...";
			Debug.Log("[RemoteConfig] Fetching remote config...");
			if (await Analytics.RemoteConfig.FetchAsync())
			{
				_configFetched = true;
				_statusText = "Config fetched successfully!";
				Debug.Log("[RemoteConfig] Config fetched! Applying values...");
				ApplyConfig();
			}
			else
			{
				_statusText = "Fetch failed - using default values";
				Debug.LogWarning("[RemoteConfig] Fetch failed, using defaults");
			}
		}
	}

	private void ApplyConfig()
	{
		_enemyHealth = Analytics.RemoteConfig.GetInt("enemy_health", 100);
		_playerSpeed = Analytics.RemoteConfig.GetFloat("player_speed", 5f);
		_doubleXpEnabled = Analytics.RemoteConfig.GetBool("double_xp_enabled");
		_welcomeMessage = Analytics.RemoteConfig.GetString("welcome_message", "Welcome!");
		Debug.Log($"[RemoteConfig] Applied: health={_enemyHealth}, speed={_playerSpeed}, " + $"doubleXP={_doubleXpEnabled}, message='{_welcomeMessage}'");
		if (Analytics.RemoteConfig.HasKey("seasonal_event"))
		{
			string text = Analytics.RemoteConfig.GetString("seasonal_event");
			Debug.Log("[RemoteConfig] Seasonal event active: " + text);
		}
		HashSet<string> allKeys = Analytics.RemoteConfig.GetAllKeys();
		Debug.Log("[RemoteConfig] Available keys: " + string.Join(", ", allKeys));
	}

	public void SpawnEnemy()
	{
		int num = Analytics.RemoteConfig.GetInt("enemy_health", 100);
		Debug.Log($"[RemoteConfig] Spawning enemy with {num} HP (from remote config)");
	}

	private void OnGUI()
	{
		if (_isReady)
		{
			GUILayout.BeginArea(new Rect(Screen.width - 310, 10f, 300f, 400f));
			GUILayout.BeginVertical("box");
			GUILayout.Label("Remote Config Example");
			GUILayout.Label(_statusText);
			GUILayout.Space(10f);
			if (GUILayout.Button("Fetch Config"))
			{
				FetchConfig();
			}
			GUILayout.Space(10f);
			GUILayout.Label("Current Values:");
			GUILayout.Label($"  Enemy Health: {_enemyHealth}");
			GUILayout.Label($"  Player Speed: {_playerSpeed:F1}");
			GUILayout.Label("  Double XP: " + (_doubleXpEnabled ? "ON" : "OFF"));
			GUILayout.Label("  Welcome: " + _welcomeMessage);
			if (_configFetched)
			{
				GUILayout.Label($"  Config Version: {Analytics.RemoteConfig.ConfigVersion}");
			}
			GUILayout.Space(10f);
			if (GUILayout.Button("Spawn Enemy (uses config)"))
			{
				SpawnEnemy();
			}
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}
}
