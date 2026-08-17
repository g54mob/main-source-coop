using System.Collections;
using System.Collections.Generic;
using EvilAnalytics.SDK.Core;
using EvilAnalytics.Shared.Common;
using UnityEngine;

public class EventTrackingExample : MonoBehaviour
{
	private bool _isReady;

	private IEnumerator Start()
	{
		while (EvilAnalyticsManager.Instance == null || !EvilAnalyticsManager.Instance.IsReady)
		{
			yield return null;
		}
		_isReady = true;
		Debug.Log("[EventTrackingExample] Ready!");
	}

	public async void TrackSimpleEvent()
	{
		if (_isReady)
		{
			await Analytics.TrackEventAsync("main_menu_opened");
			Debug.Log("Tracked: main_menu_opened");
		}
	}

	public async void TrackEventWithProperties()
	{
		if (_isReady)
		{
			await Analytics.TrackEventAsync("item_equipped", new Dictionary<string, object>
			{
				{ "item_name", "Fire Sword" },
				{ "item_rarity", "Legendary" },
				{ "player_level", 25 },
				{ "slot", "weapon" }
			});
			Debug.Log("Tracked: item_equipped with properties");
		}
	}

	public async void TrackCategorizedEvent()
	{
		if (_isReady)
		{
			await Analytics.TrackEventAsync("friend_invited", EventCategory.Social, new Dictionary<string, object>
			{
				{ "invite_method", "share_link" },
				{ "platform", "discord" }
			});
			Debug.Log("Tracked: friend_invited (Social category)");
		}
	}

	public void TrackLevelStart()
	{
		if (_isReady)
		{
			EvilAnalyticsManager.Instance.TrackLevelStart(1, "Forest of Beginnings");
			Debug.Log("Tracked: level_start (Level 1)");
		}
	}

	public void TrackLevelComplete()
	{
		if (_isReady)
		{
			EvilAnalyticsManager.Instance.TrackLevelComplete(1, 45.5f, 12500, 3);
			Debug.Log("Tracked: level_complete (Level 1, 45.5s, 12500 points, 3 stars)");
		}
	}

	public void TrackLevelFailed()
	{
		if (_isReady)
		{
			EvilAnalyticsManager.Instance.TrackLevelFailed(1, 30.2f, "fell_off_platform");
			Debug.Log("Tracked: level_failed (Level 1, 30.2s, fell_off_platform)");
		}
	}

	public void TrackPurchase()
	{
		if (_isReady)
		{
			EvilAnalyticsManager.Instance.TrackPurchase("gem_pack_500", "500 Gems", 4.99m);
			Debug.Log("Tracked: purchase ($4.99 - 500 Gems)");
		}
	}

	public void TrackAdShown()
	{
		if (_isReady)
		{
			EvilAnalyticsManager.Instance.TrackAdShown("rewarded_video", "AdMob", "level_complete_bonus");
			Debug.Log("Tracked: ad_shown (rewarded_video on AdMob)");
		}
	}

	public void TrackTutorialStep()
	{
		if (_isReady)
		{
			EvilAnalyticsManager.Instance.TrackTutorialStep(3, "learn_combat");
			Debug.Log("Tracked: tutorial_step (Step 3: learn_combat)");
		}
	}

	public void TrackTutorialComplete()
	{
		if (_isReady)
		{
			EvilAnalyticsManager.Instance.TrackTutorialComplete();
			Debug.Log("Tracked: tutorial_complete");
		}
	}

	public async void TrackEventWithValue()
	{
		if (_isReady)
		{
			await Analytics.TrackEventWithValueAsync("coins_earned", 150m, EventCategory.Progression, new Dictionary<string, object>
			{
				{ "source", "chest_reward" },
				{ "chest_type", "golden" }
			});
			Debug.Log("Tracked: coins_earned (150 coins from golden chest)");
		}
	}

	private void OnGUI()
	{
		if (_isReady)
		{
			GUILayout.BeginArea(new Rect(Screen.width - 310, 10f, 300f, 500f));
			GUILayout.BeginVertical("box");
			GUILayout.Label("Event Tracking Examples");
			if (GUILayout.Button("Simple Event"))
			{
				TrackSimpleEvent();
			}
			if (GUILayout.Button("Event with Properties"))
			{
				TrackEventWithProperties();
			}
			if (GUILayout.Button("Categorized Event"))
			{
				TrackCategorizedEvent();
			}
			GUILayout.Space(10f);
			GUILayout.Label("Level Events:");
			if (GUILayout.Button("Level Start"))
			{
				TrackLevelStart();
			}
			if (GUILayout.Button("Level Complete"))
			{
				TrackLevelComplete();
			}
			if (GUILayout.Button("Level Failed"))
			{
				TrackLevelFailed();
			}
			GUILayout.Space(10f);
			GUILayout.Label("Monetization:");
			if (GUILayout.Button("Track Purchase"))
			{
				TrackPurchase();
			}
			if (GUILayout.Button("Track Ad Shown"))
			{
				TrackAdShown();
			}
			GUILayout.Space(10f);
			GUILayout.Label("Tutorial:");
			if (GUILayout.Button("Tutorial Step 3"))
			{
				TrackTutorialStep();
			}
			if (GUILayout.Button("Tutorial Complete"))
			{
				TrackTutorialComplete();
			}
			GUILayout.Space(10f);
			if (GUILayout.Button("Event with Value"))
			{
				TrackEventWithValue();
			}
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}
}
