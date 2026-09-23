using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mimicraft.Analytics;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.UI;
using Steamworks;
using Steamworks.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mimicraft.Networking
{
	public static class QuickPlay
	{
		private const int MaxResults = 50;

		private const float PingWaitSeconds = 3f;

		private const float SearchSeconds = 20f;

		private const float RetryIntervalSeconds = 3f;

		private const float FriendWeight = 120f;

		private const float OpenLobbyWeight = 60f;

		private const float PingWeight = 45f;

		private const float PopulationWeight = 30f;

		private const float LanguageWeight = 25f;

		private const float ModeWeight = 20f;

		private const int WorstUsefulPingMs = 250;

		private const int UnknownPingMs = 120;

		private const float IdealFullness = 0.66f;

		public static bool IsAvailable => SteamManager.IsInitialized;

		public static bool IsSearching { get; private set; }

		public static bool RequestedOnMenu { get; private set; }

		public static void RequestOnMenu()
		{
			RequestedOnMenu = true;
		}

		public static bool ConsumeMenuRequest()
		{
			bool requestedOnMenu = RequestedOnMenu;
			RequestedOnMenu = false;
			return requestedOnMenu;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			RequestedOnMenu = false;
		}

		public static async void Start(Scene keepScene)
		{
			if (IsSearching)
			{
				return;
			}
			if (!SteamManager.IsInitialized)
			{
				Report("Status.SteamNotRunning");
				Telemetry.Send("quickplay_result", ("outcome", "steam_off"), ("elapsed_seconds", 0));
				return;
			}
			Telemetry.Send("menu_action", ("menu_item", "quick_play"), ("session_seconds", Telemetry.SessionSeconds));
			float searchStartedAt = Time.realtimeSinceStartup;
			IsSearching = true;
			try
			{
				LoadingScreen.ShowWaiting("QuickPlay.Searching");
				await SteamPing.WaitAsync(3f);
				Lobby? best = await SearchAsync();
				if (!best.HasValue)
				{
					Telemetry.Send("quickplay_result", ("outcome", "none"), ("elapsed_seconds", Mathf.RoundToInt(Time.realtimeSinceStartup - searchStartedAt)));
					LoadingScreen.Hide();
					DialogView.Show(new DialogRequest(Loc.Get("QuickPlay.NoLobby"), Loc.Get("Common.Ok")), delegate
					{
					});
					return;
				}
				string data = best.Value.GetData("name");
				LoadingScreen.ShowWaiting("QuickPlay.JoiningStep");
				Toast(Loc.Format("QuickPlay.Joining", string.IsNullOrEmpty(data) ? Loc.Get("LobbyBrowser.Unnamed") : data));
				bool flag = await SteamLobbyJoin.JoinAsync(best.Value, keepScene, Toast);
				if (!flag)
				{
					LoadingScreen.Hide();
				}
				Telemetry.Send("quickplay_result", ("outcome", flag ? "joined" : "join_failed"), ("elapsed_seconds", Mathf.RoundToInt(Time.realtimeSinceStartup - searchStartedAt)), ("lobby_phase", best.Value.GetData("phase") ?? ""));
			}
			catch (Exception exception)
			{
				Telemetry.Send("quickplay_result", ("outcome", "error"), ("elapsed_seconds", Mathf.RoundToInt(Time.realtimeSinceStartup - searchStartedAt)));
				Debug.LogException(exception);
				LoadingScreen.Hide();
				Report("QuickPlay.NoLobby");
			}
			finally
			{
				IsSearching = false;
			}
		}

		private static async Task<Lobby?> SearchAsync()
		{
			float deadline = Time.realtimeSinceStartup + 20f;
			while (true)
			{
				float num = deadline - Time.realtimeSinceStartup;
				if (num <= 0f || !SteamManager.IsInitialized)
				{
					return null;
				}
				Task<Lobby?> look = FindBestAsync();
				if (await Task.WhenAny(look, Task.Delay(TimeSpan.FromSeconds(num))) != look)
				{
					return null;
				}
				if (look.Result.HasValue)
				{
					return look.Result;
				}
				float num2 = Mathf.Min(3f, deadline - Time.realtimeSinceStartup);
				if (num2 <= 0f)
				{
					break;
				}
				await Task.Delay(TimeSpan.FromSeconds(num2));
			}
			return null;
		}

		private static async Task<Lobby?> FindBestAsync()
		{
			HashSet<ulong> seen = new HashSet<ulong>();
			HashSet<ulong> friendLobbies = new HashSet<ulong>();
			List<Lobby> candidates = new List<Lobby>();
			Lobby? best = null;
			float bestScore = float.MinValue;
			foreach (Friend friend in SteamFriends.GetFriends())
			{
				if (friend.IsPlayingThisGame)
				{
					Lobby? lobby = friend.GameInfo?.Lobby;
					if (lobby.HasValue)
					{
						friendLobbies.Add(lobby.Value.Id.Value);
						candidates.Add(lobby.Value);
					}
				}
			}
			Lobby[] array = await SteamMatchmaking.LobbyList.WithMaxResults(50).RequestAsync();
			if (array != null)
			{
				candidates.AddRange(array);
			}
			string preferredMode = LobbyPrefs.GetString("Browser.ModeFilter");
			string language = Loc.Language;
			int num = 0;
			foreach (Lobby item in candidates)
			{
				if (seen.Add(item.Id.Value) && !Rejects(item))
				{
					num++;
					float num2 = Score(item, preferredMode, language, friendLobbies.Contains(item.Id.Value));
					if (!(num2 <= bestScore))
					{
						bestScore = num2;
						best = item;
					}
				}
			}
			string arg = (best.HasValue ? $"Seçilen: {best.Value.Id}, puan {bestScore:0.#}, ping {PingOf(best.Value)} ms" : "Seçim yok.");
			Debug.Log($"[QuickPlay] {seen.Count} lobi bulundu, {num} tanesi uygun. {arg} " + $"(ping verisi hazır mı: {SteamPing.IsReady})");
			return best;
		}

		private static bool Rejects(Lobby lobby)
		{
			if (lobby.MemberCount <= 0)
			{
				return true;
			}
			if (lobby.MaxMembers > 0 && lobby.MemberCount >= lobby.MaxMembers)
			{
				return true;
			}
			if (lobby.GetData("has_password") == "1")
			{
				return true;
			}
			if (!GameVersion.Matches(lobby.GetData("version")))
			{
				return true;
			}
			string data = lobby.GetData("mode");
			if (!string.IsNullOrEmpty(data))
			{
				return GameModeCatalog.Find(data) == null;
			}
			return true;
		}

		private static float Score(Lobby lobby, string preferredMode, string language, bool friendIsIn)
		{
			float num = 0f;
			if (friendIsIn)
			{
				num += 120f;
			}
			if (LobbyPhase.IsOpen(lobby.GetData("phase")))
			{
				num += 60f;
			}
			num += PingScore(PingOf(lobby));
			num += PopulationScore(lobby);
			if (!string.IsNullOrEmpty(language) && lobby.GetData("lang") == language)
			{
				num += 25f;
			}
			if (!string.IsNullOrEmpty(preferredMode) && lobby.GetData("mode") == preferredMode)
			{
				num += 20f;
			}
			return num;
		}

		private static int PingOf(Lobby lobby)
		{
			return SteamPing.EstimateMs(lobby.GetData("ping_location"));
		}

		private static float PingScore(int ms)
		{
			if (ms == -1)
			{
				ms = 120;
			}
			return 45f * Mathf.Clamp01(1f - (float)ms / 250f);
		}

		private static float PopulationScore(Lobby lobby)
		{
			if (lobby.MaxMembers <= 0)
			{
				return 15f;
			}
			float num = Mathf.Abs((float)lobby.MemberCount / (float)lobby.MaxMembers - 0.66f);
			float num2 = Mathf.Max(0.66f, 0.33999997f);
			return 30f * Mathf.Clamp01(1f - num / num2);
		}

		private static void Report(string key)
		{
			Toast(Loc.Get(key));
		}

		private static void Toast(string message)
		{
			if (ToastView.Instance != null)
			{
				ToastView.Instance.Show(message);
			}
			else
			{
				Debug.Log("[QuickPlay] " + message);
			}
		}
	}
}
