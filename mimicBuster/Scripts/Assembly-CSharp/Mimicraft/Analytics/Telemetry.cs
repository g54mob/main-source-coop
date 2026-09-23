using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mimicraft.Localization;
using Mimicraft.Networking;
using Mimicraft.Settings;
using Mimicraft.Tutorial;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

namespace Mimicraft.Analytics
{
	public static class Telemetry
	{
		public static bool Debug;

		private static bool started;

		private static bool debugForced;

		private static bool initialised;

		private static bool sessionStartSent;

		private static float sessionStartedAt;

		private static readonly Dictionary<string, int> counters = new Dictionary<string, int>();

		private const int Remembered = 40;

		private static readonly List<string> recent = new List<string>();

		private const int MaxParameters = 10;

		private const int MaxStringLength = 100;

		private const string VersionParameter = "clientVersion";

		private const int InitialiseTimeoutMs = 15000;

		private static readonly HashSet<string> failuresSaid = new HashSet<string>();

		public static string EnvironmentName => Build.Kind switch
		{
			BuildKind.Demo => "demo", 
			BuildKind.Playtest => "playtest", 
			_ => "production", 
		};

		public static bool Enabled
		{
			get
			{
				if (GameSettings.AnalyticsConsent)
				{
					if (!started)
					{
						return debugForced;
					}
					return true;
				}
				return false;
			}
		}

		public static int SessionSeconds => Mathf.RoundToInt(Time.realtimeSinceStartup - sessionStartedAt);

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			started = false;
			debugForced = false;
			initialised = false;
			sessionStartSent = false;
			Debug = false;
			counters.Clear();
			recent.Clear();
			failuresSaid.Clear();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Bootstrap()
		{
			if (!initialised)
			{
				initialised = true;
				sessionStartedAt = Time.realtimeSinceStartup;
				try
				{
					GameSettings.Load();
					GameSettings.Changed += OnSettingsChanged;
					Application.quitting += OnQuitting;
				}
				catch (Exception e)
				{
					Fail("baslatilamadi", e);
				}
				Start();
			}
		}

		private static async void Start()
		{
			_ = 1;
			try
			{
				if (UnityServices.State == ServicesInitializationState.Uninitialized)
				{
					InitializationOptions options = new InitializationOptions().SetEnvironmentName(EnvironmentName);
					Task initialise = UnityServices.InitializeAsync(options);
					if (await Task.WhenAny(initialise, Task.Delay(15000)) == initialise)
					{
						await initialise;
					}
					else
					{
						UnityEngine.Debug.LogError($"[Telemetry] Servisler {15} saniyede " + "baslamadi - analitik bu oturumda kapali kalabilir, oyun etkilenmez.");
						initialise.ContinueWith(delegate(Task t)
						{
							_ = t.Exception;
						}, TaskContinuationOptions.OnlyOnFaulted);
					}
				}
				EnsureStarted();
			}
			catch (Exception e)
			{
				Fail("baslatilamadi", e);
			}
			SessionStart();
		}

		private static void EnsureStarted()
		{
			if (!started && GameSettings.AnalyticsConsent && (!Application.isEditor || debugForced) && UnityServices.State == ServicesInitializationState.Initialized)
			{
				AnalyticsService.Instance.StartDataCollection();
				started = true;
			}
		}

		private static void OnSettingsChanged()
		{
			try
			{
				if (!initialised || UnityServices.State != ServicesInitializationState.Initialized)
				{
					return;
				}
				if (GameSettings.AnalyticsConsent && !started)
				{
					EnsureStarted();
					if (started)
					{
						SessionStart();
					}
				}
				else if (!GameSettings.AnalyticsConsent && started)
				{
					started = false;
					AnalyticsService.Instance.StopDataCollection();
					AnalyticsService.Instance.RequestDataDeletion();
				}
			}
			catch (Exception e)
			{
				Fail("onay degisikligi uygulanamadi", e);
			}
		}

		private static void SessionStart()
		{
			if (!sessionStartSent)
			{
				string item = "";
				try
				{
					item = Loc.Language ?? "";
				}
				catch (Exception)
				{
				}
				Send("session_start", ("languageGame", item), ("steam_running", SteamManager.IsInitialized), ("first_run", !TutorialState.Completed), ("tutorial_done", TutorialState.Completed));
				sessionStartSent = Enabled;
			}
		}

		private static void OnQuitting()
		{
			Send("session_end", ("elapsed_seconds", SessionSeconds), ("rounds_played", Count("rounds")), ("lobbies_joined", Count("lobbies")), ("duels", Count("duels")), ("mimics_saved", Count("mimics")));
			try
			{
				if (started)
				{
					AnalyticsService.Instance.Flush();
				}
			}
			catch (Exception e)
			{
				Fail("cikista gonderilemedi", e);
			}
		}

		public static int Bump(string counter)
		{
			counters.TryGetValue(counter, out var value);
			return counters[counter] = value + 1;
		}

		public static int Count(string counter)
		{
			if (!counters.TryGetValue(counter, out var value))
			{
				return 0;
			}
			return value;
		}

		public static void Send(string eventName, params (string key, object value)[] parameters)
		{
			if (string.IsNullOrEmpty(eventName) || parameters == null)
			{
				return;
			}
			try
			{
				Remember(eventName, parameters);
			}
			catch (Exception e)
			{
				Fail("'" + eventName + "' kaydedilemedi", e);
				return;
			}
			try
			{
				if (!started)
				{
					EnsureStarted();
				}
				if (!Enabled)
				{
					return;
				}
				CustomEvent customEvent = new CustomEvent(eventName);
				customEvent["clientVersion"] = Application.version;
				int num = 1;
				for (int i = 0; i < parameters.Length; i++)
				{
					var (text, obj) = parameters[i];
					if (string.IsNullOrEmpty(text) || obj == null || text == "clientVersion")
					{
						continue;
					}
					if (num++ >= 10)
					{
						break;
					}
					if (!(obj is bool flag))
					{
						if (!(obj is int num2))
						{
							if (!(obj is long num3))
							{
								if (!(obj is float num4))
								{
									if (!(obj is double num5))
									{
										if (obj is string s)
										{
											customEvent[text] = Clip(s);
										}
										else
										{
											customEvent[text] = Clip(obj.ToString());
										}
									}
									else
									{
										customEvent[text] = num5;
									}
								}
								else
								{
									customEvent[text] = num4;
								}
							}
							else
							{
								customEvent[text] = num3;
							}
						}
						else
						{
							customEvent[text] = num2;
						}
					}
					else
					{
						customEvent[text] = flag;
					}
				}
				AnalyticsService.Instance.RecordEvent(customEvent);
			}
			catch (Exception e2)
			{
				Fail("'" + eventName + "' gonderilemedi", e2);
			}
		}

		private static void Fail(string what, Exception e)
		{
			string text = "[Telemetry] " + what + ": " + e.GetType().Name + ": " + e.Message;
			if (failuresSaid.Add(text))
			{
				UnityEngine.Debug.LogError(text + " (analitik bu olayi atladi, oyun devam ediyor)");
			}
		}

		private static string Clip(string s)
		{
			if (s.Length > 100)
			{
				return s.Substring(0, 100);
			}
			return s;
		}

		private static void Remember(string eventName, (string key, object value)[] parameters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(SessionSeconds.ToString("0000")).Append("s ").Append(eventName);
			for (int i = 0; i < parameters.Length; i++)
			{
				var (value, obj) = parameters[i];
				if (!string.IsNullOrEmpty(value) && obj != null)
				{
					stringBuilder.Append(' ').Append(value).Append('=')
						.Append(obj);
				}
			}
			if (recent.Count >= 40)
			{
				recent.RemoveAt(0);
			}
			recent.Add(stringBuilder.ToString());
			if (Debug)
			{
				UnityEngine.Debug.Log("[Telemetry] " + stringBuilder);
			}
		}

		public static string Report()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("environment=").Append(EnvironmentName).Append(" asked=")
				.Append(AnalyticsConsentPrompt.Answered)
				.Append(" consent=")
				.Append(GameSettings.AnalyticsConsent)
				.Append(" started=")
				.Append(started)
				.Append(" enabled=")
				.Append(Enabled)
				.Append(" debug=")
				.Append(Debug)
				.Append(" sdk=on");
			stringBuilder.Append('\n').Append("counters:");
			foreach (KeyValuePair<string, int> counter in counters)
			{
				stringBuilder.Append(' ').Append(counter.Key).Append('=')
					.Append(counter.Value);
			}
			stringBuilder.Append('\n').Append("last ").Append(recent.Count)
				.Append(" events:");
			foreach (string item in recent)
			{
				stringBuilder.Append('\n').Append("  ").Append(item);
			}
			return stringBuilder.ToString();
		}

		public static void ForceInEditor(bool force)
		{
			debugForced = force && Application.isEditor;
			if (debugForced)
			{
				Start();
			}
		}
	}
}
