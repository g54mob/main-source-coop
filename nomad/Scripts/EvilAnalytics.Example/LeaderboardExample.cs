using System;
using System.Collections;
using EvilAnalytics.SDK.Core;
using EvilAnalytics.Shared.Leaderboards;
using UnityEngine;

public class LeaderboardExample : MonoBehaviour
{
	[Header("Leaderboard Settings")]
	[Tooltip("Paste the Leaderboard ID from the dashboard here")]
	[SerializeField]
	private string leaderboardId = "YOUR_LEADERBOARD_ID_HERE";

	private bool _isReady;

	private string _statusText = "Waiting for SDK...";

	private string _rankingsText = "";

	private string _myRankText = "";

	private string _scoreInput = "1000";

	private IEnumerator Start()
	{
		while (EvilAnalyticsManager.Instance == null || !EvilAnalyticsManager.Instance.IsReady)
		{
			yield return null;
		}
		_isReady = true;
		_statusText = "Ready! Enter a Leaderboard ID and try the buttons.";
		Debug.Log("[LeaderboardExample] Ready!");
	}

	public async void SubmitScore(decimal score)
	{
		if (!_isReady)
		{
			return;
		}
		if (!Guid.TryParse(leaderboardId, out var result))
		{
			_statusText = "ERROR: Invalid Leaderboard ID!";
			return;
		}
		_statusText = "Submitting score...";
		Debug.Log($"[Leaderboard] Submitting score: {score}");
		try
		{
			SubmitScoreResponse submitScoreResponse = await Analytics.Leaderboards.SubmitScoreAsync(result, score);
			if (submitScoreResponse.Accepted)
			{
				_statusText = $"Score accepted! Current: {submitScoreResponse.NewScore}" + (submitScoreResponse.NewRank.HasValue ? $" | Rank: #{submitScoreResponse.NewRank}" : "") + (submitScoreResponse.IsNewBest ? " | NEW BEST!" : "");
				Debug.Log($"[Leaderboard] Score accepted. New score: {submitScoreResponse.NewScore}, " + $"Rank: {submitScoreResponse.NewRank}, New best: {submitScoreResponse.IsNewBest}");
			}
			else
			{
				_statusText = "Score was not accepted by server";
			}
		}
		catch (Exception ex)
		{
			_statusText = "Error: " + ex.Message;
			Debug.LogError("[Leaderboard] Submit failed: " + ex.Message);
		}
	}

	public async void GetTopRankings(string timeWindow = "AllTime", int limit = 10)
	{
		if (!_isReady)
		{
			return;
		}
		if (!Guid.TryParse(leaderboardId, out var result))
		{
			_statusText = "ERROR: Invalid Leaderboard ID!";
			return;
		}
		_statusText = "Loading rankings...";
		_rankingsText = "";
		try
		{
			RankingsResponse rankingsResponse = await Analytics.Leaderboards.GetRankingsAsync(result, timeWindow, limit);
			_statusText = $"Top {rankingsResponse.Rankings.Count} of {rankingsResponse.TotalPlayers} players ({rankingsResponse.TimeWindow})";
			_rankingsText = "--- " + rankingsResponse.LeaderboardName + " (" + rankingsResponse.TimeWindow + ") ---\n";
			foreach (RankingEntry ranking in rankingsResponse.Rankings)
			{
				_rankingsText = _rankingsText + $"#{ranking.Rank} | {ranking.DeviceId.Substring(0, 8)}... | " + $"Score: {ranking.Score} | Plays: {ranking.SubmissionCount}\n";
			}
			Debug.Log($"[Leaderboard] Loaded {rankingsResponse.Rankings.Count} rankings");
		}
		catch (Exception ex)
		{
			_statusText = "Error: " + ex.Message;
			Debug.LogError("[Leaderboard] Get rankings failed: " + ex.Message);
		}
	}

	public async void GetMyRank(string timeWindow = "AllTime")
	{
		if (!_isReady)
		{
			return;
		}
		if (!Guid.TryParse(leaderboardId, out var result))
		{
			_statusText = "ERROR: Invalid Leaderboard ID!";
			return;
		}
		_statusText = "Loading your rank...";
		_myRankText = "";
		try
		{
			PlayerRankResponse playerRankResponse = await Analytics.Leaderboards.GetMyRankAsync(result, timeWindow);
			if (playerRankResponse == null || playerRankResponse.Player == null)
			{
				_myRankText = "You haven't submitted a score yet!";
				_statusText = "No rank found - submit a score first";
				return;
			}
			_statusText = $"Your rank: #{playerRankResponse.Player.Rank} of {playerRankResponse.TotalPlayers}";
			_myRankText = "--- Your Position ---\n";
			foreach (RankingEntry nearbyPlayer in playerRankResponse.NearbyPlayers)
			{
				if (nearbyPlayer.Rank < playerRankResponse.Player.Rank)
				{
					_myRankText += $"  #{nearbyPlayer.Rank} | {nearbyPlayer.DeviceId.Substring(0, 8)}... | {nearbyPlayer.Score}\n";
				}
			}
			_myRankText += $"> #{playerRankResponse.Player.Rank} | YOU | Score: {playerRankResponse.Player.Score} <\n";
			foreach (RankingEntry nearbyPlayer2 in playerRankResponse.NearbyPlayers)
			{
				if (nearbyPlayer2.Rank > playerRankResponse.Player.Rank)
				{
					_myRankText += $"  #{nearbyPlayer2.Rank} | {nearbyPlayer2.DeviceId.Substring(0, 8)}... | {nearbyPlayer2.Score}\n";
				}
			}
			Debug.Log($"[Leaderboard] Your rank: #{playerRankResponse.Player.Rank} / {playerRankResponse.TotalPlayers}");
		}
		catch (Exception ex)
		{
			_statusText = "Error: " + ex.Message;
			Debug.LogError("[Leaderboard] Get my rank failed: " + ex.Message);
		}
	}

	private void OnGUI()
	{
		if (_isReady)
		{
			GUILayout.BeginArea(new Rect(Screen.width - 310, 10f, 300f, 600f));
			GUILayout.BeginVertical("box");
			GUILayout.Label("Leaderboard Example");
			GUILayout.Label(_statusText);
			GUILayout.Space(5f);
			GUILayout.Label("Leaderboard ID:");
			leaderboardId = GUILayout.TextField(leaderboardId);
			GUILayout.Space(10f);
			GUILayout.Label("Submit Score:");
			GUILayout.BeginHorizontal();
			_scoreInput = GUILayout.TextField(_scoreInput, GUILayout.Width(100f));
			if (GUILayout.Button("Submit") && decimal.TryParse(_scoreInput, out var result))
			{
				SubmitScore(result);
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.Label("Get Rankings:");
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("All Time"))
			{
				GetTopRankings();
			}
			if (GUILayout.Button("Daily"))
			{
				GetTopRankings("Daily");
			}
			if (GUILayout.Button("Weekly"))
			{
				GetTopRankings("Weekly");
			}
			GUILayout.EndHorizontal();
			if (!string.IsNullOrEmpty(_rankingsText))
			{
				GUILayout.Label(_rankingsText);
			}
			GUILayout.Space(10f);
			if (GUILayout.Button("Get My Rank"))
			{
				GetMyRank();
			}
			if (!string.IsNullOrEmpty(_myRankText))
			{
				GUILayout.Label(_myRankText);
			}
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}
}
