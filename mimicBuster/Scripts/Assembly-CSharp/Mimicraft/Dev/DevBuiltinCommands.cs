using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Mimicraft.Analytics;
using Mimicraft.Cameras;
using Mimicraft.Customization;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.Networking;
using Mimicraft.Settings;
using Mimicraft.Tutorial;
using Mimicraft.UI;
using Mimicraft.Voice;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using Steamworks;
using Steamworks.Data;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;

namespace Mimicraft.Dev
{
	public static class DevBuiltinCommands
	{
		public static void RegisterAll(DevConsole console)
		{
			DevCommandRegistry.Register("help", "[command]", "Lists the commands, or explains what one of them does. Cheat commands are marked.", delegate(string[] args)
			{
				Help(console, args);
			});
			DevCommandRegistry.Register("sv_cheats", "[0|1]", "Whether cheat commands (marked [cheat] in help) may run in this session. Host only. Always 0 in a public Steam lobby and cannot be changed there; a Friends Only or Invites Only lobby may set it to 1. Everyone in the lobby is told when it changes.", delegate(string[] args)
			{
				Cheats(console, args);
			});
			DevCommandRegistry.Register("echo", "<text>", "Prints back what you typed.", delegate(string[] args)
			{
				console.Print(string.Join(" ", args));
			});
			DevCommandRegistry.Register("clear", "", "Clears the console output.", delegate
			{
				console.Clear();
			});
			DevCommandRegistry.Register("log", "<text>", "Writes a line to the Unity log - to mark a moment for when the log is read later.", delegate(string[] args)
			{
				Debug.Log("[Console] " + string.Join(" ", args));
			});
			DevCommandRegistry.Register("unitylog", "[on|off]", "Mirrors console output into Unity's own console, where it can be copied from. On by default in the Editor, off in a build.", delegate(string[] args)
			{
				UnityLog(console, args);
			});
			DevCommandRegistry.Register("quit", "", "Quits the game. In the Editor, stops Play mode.", delegate
			{
				Quit(console);
			});
			DevCommandRegistry.Register("binds", "[reset]", "Lists the key bindings, with the command ids a hand-placed row would ask for. 'binds reset' returns them all to default.", delegate(string[] args)
			{
				Binds(console, args);
			});
			DevCommandRegistry.Register("fpsarms", "[punch]", "Prints the first-person view model's animation state: which rig is loaded, whether it has an Animator, which parameters it carries, whether it updates. 'fpsarms punch' sends the trigger DIRECTLY to that Animator - if the state changes the fault is in how the game triggers it, if not it is in the controller's transitions.", delegate(string[] args)
			{
				FpsArms(console, args);
			});
			DevCommandRegistry.Register("ads", "", "Prints the aim-down-sights state: which ADS point is in use (placed or computed from the model), how far the pivot is offset, and where that point ACTUALLY lands relative to the camera. Run it while aiming - x/y of 0 means dead centre.", delegate
			{
				Ads(console);
			});
			DevCommandRegistry.Register("tpsweapon", "[player name]", "Prints the chain the third-person weapon hangs from - every transform from the weapon root to the model root with its local and world rotation, the socket sway's recoil state, the rig weights. For a tilt/drift complaint: run once at match start and once at the end; the line that changed is the culprit. Without a name, your own player.", delegate(string[] args)
			{
				TpsWeapon(console, args);
			});
			DevCommandRegistry.Register("lobbies", "", "Scans Steam lobbies and prints the raw data each one PUBLISHES - mode, map, country, ping location, member count. If a browser card looks empty, this shows which key came back blank.", delegate
			{
				Lobbies(console);
			});
			DevCommandRegistry.Register("body", "[reset]", "What the voxel body costs this machine: how many sends, encode ms, server validation ms (on the host), how many mesh builds and collider bakes and their ms, whether a send is pending. For an 'undo stutters' complaint this says which half stutters. 'body reset' zeroes the counters.", delegate(string[] args)
			{
				Body(console, args);
			});
			DevCommandRegistry.Register("tickrate", "[default|high|ultra|<10-256>]", "The server tick rate for the sessions you host: default (30), high (64), ultra (128) or any number from 10 to 256. Menu only; it applies from the next session you host and is remembered. Players joining through a Steam lobby or the LAN list take it over automatically; for Direct Connect both sides must set the same value.", delegate(string[] args)
			{
				TickRate(console, args);
			});
			DevCommandRegistry.Register("clearance", "", "How your own body sits in the level: per piece, how much of it is inside objects, how many box corners are outside the map, what it is inside, and whether it is being left out of what the other players see. With timings. Only ever about your own body.", delegate
			{
				Clearance(console);
			});
			DevCommandRegistry.Register("library", "", "Prints the template library's state: is the window open, which category it shows, is the card pool intact, is anything listening for saves, how many templates are on disk. For when a saved model does not appear in the list.", delegate
			{
				Library(console);
			});
			DevCommandRegistry.Register("voice", "", "Prints the voice chat state: is the microphone on, is the noise gate passing, which key is held, how many packets came from whom and how many were lost. To tell apart the six separate reasons for 'I cannot hear X'.", delegate
			{
				PrintLines(console, PlayerVoice.Report());
			});
			DevCommandRegistry.Register("weaponpreview", "", "Walks the weapon customizer's Preview button from the click to the speakers and prints every step: is the button wired, which weapon is on the bench, which clip its sound type picks, where the muzzle flash is parented, and whether the library, the mixer group and the AudioListener are awake to carry the sound. Then presses it. For 'the preview button does nothing'.", delegate
			{
				WeaponPreview(console);
			});
			DevCommandRegistry.Register("timescale", "[value]", "Speed of time. 0.25 is slow motion, 0 pauses, 1 is normal. Without a value, prints the current one.", delegate(string[] args)
			{
				TimeScale(console, args);
			}, cheat: true);
			DevCommandRegistry.Register("fps", "[on|off]", "Shows/hides the frame rate counter.", delegate(string[] args)
			{
				Toggle(console, args, console.ShowFps, delegate(bool v)
				{
					console.ShowFps = v;
				}, "FPS counter");
			});
			DevCommandRegistry.Register("vsync", "[0|1|2]", "Vertical sync interval. 0 = off, and then maxfps takes over.", delegate(string[] args)
			{
				VSync(console, args);
			});
			DevCommandRegistry.Register("maxfps", "[value]", "Target frame rate. -1 = unlimited. Only effective while vsync is 0.", delegate(string[] args)
			{
				MaxFps(console, args);
			});
			DevCommandRegistry.Register("screenshot", "[scale]", "Takes a screenshot. A scale of 2 saves it at twice the resolution.", delegate(string[] args)
			{
				Screenshot(console, args);
			});
			DevCommandRegistry.Register("res", "<width> <height> [full|window]", "Changes the resolution. Without arguments, prints the current one.", delegate(string[] args)
			{
				Resolution(console, args);
			});
			DevCommandRegistry.Register("scene", "", "Lists the loaded scenes and marks the active one.", delegate
			{
				Scenes(console);
			});
			DevCommandRegistry.Register("net", "", "Network state: server or client, how many connections, which transport.", delegate
			{
				Net(console);
			});
			DevCommandRegistry.Register("votekick", "[player]", "Starts a kick vote against a player, by name or by the id `players` prints. Without a name, opens the player list on screen and puts the console away. For testing the vote off Steam: the button is Steam only because the ban needs an account, so on LAN turn on sv_cheats and the whole feature - button included - comes with it. The kick still works there; the ban does not, so the player can rejoin.", delegate(string[] args)
			{
				VoteKick(console, args);
			}, cheat: true);
			DevCommandRegistry.Register("players", "", "Connected players, their roles and health. A cheat: it tells a hunter who is hiding.", delegate
			{
				Players(console);
			}, cheat: true);
			DevCommandRegistry.Register("bodies", "", "Every player's voxel body as this machine has it: pieces, each piece's size in voxels, how many voxels it holds, and its connected clumps - with the ones the size rules would refuse marked. For 'that player is tiny': shows whether the body on screen is one the server accepted, and if so what about it is small. A cheat: it says who is a model.", delegate
			{
				Bodies(console);
			}, cheat: true);
			DevCommandRegistry.Register("round", "", "Round state: phase, time remaining, and how the Hunter/Modelci split was worked out - the ratio, what it comes to at this player count, and whether Min. Avci or Min. Modelci overrode it. For 'I set two Hunters and got one'.", delegate
			{
				Round(console);
			});
			DevCommandRegistry.Register("chatui", "[on|off]", "Hides/shows the chat UI. Without an argument, toggles. Messages keep arriving while it is hidden, they are just not shown - when you bring it back the chat continues where it was. Hiding it while typing also closes typing, or you would be locked behind an invisible box.", delegate(string[] args)
			{
				Toggle(console, args, ChatView.Shown, delegate(bool v)
				{
					ChatView.Shown = v;
				}, "Chat UI");
			});
			DevCommandRegistry.Register("scoreboardui", "[on|off]", "Hides/shows the score card that sits in the corner. Does not touch the full table on F1 - that already goes away when the key is released, so there is nothing to hide.", delegate(string[] args)
			{
				Toggle(console, args, RoundHudView.Shown, delegate(bool v)
				{
					RoundHudView.Shown = v;
				}, "Score card");
			});
			DevCommandRegistry.Register("version", "", "Version, platform and build information.", delegate
			{
				Version(console);
			});
			DevCommandRegistry.Register("audio", "", "The audio chain's real state right now: the settings, the dB values the mixer holds, and which mixer group the listener and every playing source go through. The only certain way to tell whether a saved audio setting is being applied.", delegate
			{
				Audio(console);
			});
			DevCommandRegistry.Register("about", "[reset]", "Opens the build note. 'about reset' clears the 'this build's note was read' marker, so the note opens by itself next time the menu loads - the only way to test the first-run flow, since that marker is set once per build.", delegate(string[] args)
			{
				About(console, args);
			});
			DevCommandRegistry.Register("analytics", "[debug on|off | editor on|off | forget]", "What the telemetry is doing: environment, whether the consent question was put and how it was answered, whether the SDK is wired, the session's tallies and the last forty events with their parameters. 'analytics debug on' echoes every event to the log as it is recorded; 'analytics editor on' lets an Editor session actually send, for checking the pipeline end to end; 'analytics forget' clears the answer so the first-run consent question comes back on the next launch. See Docs/Analytics-Plani.md.", delegate(string[] args)
			{
				AnalyticsCommand(console, args);
			});
			DevCommandRegistry.Register("language", "[<kod>|forget]", "Why the game is in the language it is in. Prints what Steam says this game's language is, what the operating system reports, what startup detection made of the two, whether a language has been SAVED (a saved one is a choice and stops detection running), and what is in force. 'language de' switches now and saves it; 'language forget' throws the saved one away so the next launch detects again - which is how to reproduce a foreign player's first run on your own machine.", delegate(string[] args)
			{
				Language(console, args);
			});
			DevCommandRegistry.Register("safechat", "[on|off|reload|test <metin>]", "The chat profanity filter. Shows whether it is on and how many words are loaded; 'safechat on'/'off' is the same switch as the Settings toggle; 'safechat reload' re-reads Assets/Resources/ChatFilterWords.txt without restarting, for editing the list; 'safechat test merhaba' shows what a line would look like after filtering, which is the only way to check a new entry without asking somebody to swear in chat.", delegate(string[] args)
			{
				SafeChat(console, args);
			});
			DevCommandRegistry.Register("cinematic", "[probe]", "Flies the round-end fly-past around YOUR OWN body, here and now, with no round and no server - so the shot can be watched in Practice from wherever you are standing. Prints where the camera goes and what is in the way first: every sample of the arc it will fly, then the whole compass around you at the same distance, saying for each whether the camera would be inside geometry and whether the line back to the body is blocked and by what. 'cinematic probe' prints that and does not fly. Stand where the shot looks wrong and run it there.", delegate(string[] args)
			{
				Cinematic(console, args);
			});
			DevCommandRegistry.Register("shotdebug", "[on|off]", "Server only. Prints what every shot resolved against: the server's origin and aim, the shooter's crosshair point and whether the correction was applied, then every other player's capsule span against their head bone, then the first three colliders each pellet met. For 'I aimed at them and hit the wall' - the answer is in the first line that says 'no player'. Toggles when called with no argument.", delegate(string[] args)
			{
				ShotDebug(console, args);
			});
			DevCommandRegistry.Register("tutorial", "[clear|offer|tip]", "Tutorial telemetry: the last session card by card, then every session per card - times seen, median duration, hint / do-it-for-me / skip rates. Read this instead of guessing which card loses the player. 'tutorial clear' clears the TELEMETRY. 'tutorial offer' forgets that this player has had their first run, so the next launch opens the tutorial straight from the splash screen again - two different things, and one does not do the other. 'tutorial tip' forgets the customization notice the menu shows a graduate once, so the next trip to the menu shows it again.", delegate(string[] args)
			{
				Tutorial(console, args);
			});
		}

		private static void Cheats(DevConsole console, string[] args)
		{
			if (args.Length == 0)
			{
				console.Print($"sv_cheats = {(DevCheats.Enabled ? 1 : 0)}  {DevCheats.Describe()}");
				return;
			}
			bool? flag;
			switch (args[0].ToLowerInvariant())
			{
			case "1":
			case "on":
			case "true":
				flag = true;
				break;
			case "0":
			case "off":
			case "false":
				flag = false;
				break;
			default:
				flag = null;
				break;
			}
			bool? flag2 = flag;
			if (!flag2.HasValue)
			{
				console.Print("'" + args[0] + "' is not 0 or 1.");
			}
			else
			{
				console.Print(DevCheats.TrySet(flag2.Value));
			}
		}

		private static void AnalyticsCommand(DevConsole console, string[] args)
		{
			if (args.Length == 1 && args[0].ToLowerInvariant() == "forget")
			{
				AnalyticsConsentPrompt.Forget();
				console.Print("analytics: consent forgotten, the question returns on the next launch.");
				return;
			}
			if (args.Length >= 2)
			{
				string text = args[1].ToLowerInvariant();
				bool flag = text == "on" || text == "1" || text == "true";
				text = args[0].ToLowerInvariant();
				if (text == "debug")
				{
					Telemetry.Debug = flag;
					console.Print("analytics debug: " + (flag ? "on" : "off"));
					return;
				}
				if (text == "editor")
				{
					Telemetry.ForceInEditor(flag);
					console.Print("analytics editor sending: " + (flag ? "on" : "off"));
					return;
				}
			}
			console.Print(Telemetry.Report());
		}

		private static void Language(DevConsole console, string[] args)
		{
			if (args.Length >= 1)
			{
				if (args[0].ToLowerInvariant() == "forget")
				{
					GameSettings.ForgetLanguage();
					console.Print("language: kayitli dil silindi, sonraki acilista yeniden tespit edilir.");
					return;
				}
				Loc.SetLanguage(args[0]);
			}
			string arg = "-";
			try
			{
				if (SteamClient.IsValid)
				{
					arg = SteamApps.GameLanguage ?? "-";
				}
			}
			catch (Exception)
			{
				arg = "(Steam yok)";
			}
			console.Print($"steam='{arg}'  sistem={Application.systemLanguage}");
			console.Print("tespit=" + (StartupLanguage.Detect(Loc.Languages) ?? "-") + "  (tam eslesme=" + (StartupLanguage.DetectExact(Loc.Languages) ?? "-") + ")");
			console.Print("kayitli=" + (GameSettings.HasSavedLanguage ? GameSettings.LanguageId : "yok") + "  bellekte=" + GameSettings.LanguageId + "  yururlukte=" + Loc.Language);
		}

		private static void SafeChat(DevConsole console, string[] args)
		{
			if (args.Length >= 1)
			{
				switch (args[0].ToLowerInvariant())
				{
				case "on":
				case "off":
					GameSettings.SetSafeChat(args[0].ToLowerInvariant() == "on");
					break;
				case "reload":
					ChatFilter.Forget();
					console.Print("safechat: kelime listesi yeniden okunacak.");
					return;
				case "test":
				{
					string text = ((args.Length > 1) ? string.Join(" ", args, 1, args.Length - 1) : "");
					console.Print("'" + text + "' -> '" + ChatFilter.Apply(text) + "'");
					return;
				}
				}
			}
			console.Print("safechat: " + (GameSettings.SafeChat ? "on" : "off"));
		}

		private static void Cinematic(DevConsole console, string[] args)
		{
			NetworkManager singleton = NetworkManager.Singleton;
			GameObject gameObject = ((singleton != null && singleton.LocalClient != null && singleton.LocalClient.PlayerObject != null) ? singleton.LocalClient.PlayerObject.gameObject : null);
			if (gameObject == null)
			{
				console.Print("No local player - try inside a session (Practice is enough).");
				return;
			}
			bool probeOnly = args.Length != 0 && args[0].ToLowerInvariant() == "probe";
			console.Print(RoundCinematicDirector.DebugPlay(gameObject.transform, probeOnly));
		}

		private static void ShotDebug(DevConsole console, string[] args)
		{
			if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsServer)
			{
				console.Print("Server only - shots are resolved by the server, so only it can say what they met.");
				return;
			}
			int debugShots;
			if (args.Length != 0)
			{
				string text = args[0].ToLowerInvariant();
				debugShots = ((text == "on" || text == "1" || text == "true") ? 1 : 0);
			}
			else
			{
				debugShots = ((!PlayerCombat.DebugShots) ? 1 : 0);
			}
			PlayerCombat.DebugShots = (byte)debugShots != 0;
			console.Print("shotdebug: " + (PlayerCombat.DebugShots ? "on" : "off"));
		}

		private static void Tutorial(DevConsole console, string[] args)
		{
			if (args.Length != 0 && args[0] == "clear")
			{
				TutorialTelemetry.Clear();
				console.Print("Tutorial telemetry cleared.");
			}
			else if (args.Length != 0 && args[0] == "offer")
			{
				TutorialState.ForgetOffered();
				console.Print("First-run tutorial reset - the next launch opens it straight from the splash screen.");
			}
			else if (args.Length != 0 && args[0] == "tip")
			{
				TutorialState.ForgetGraduateTip();
				console.Print("Customization notice reset - it shows again on the next trip to the main menu (the tutorial has to have been finished at least once).");
			}
			else
			{
				console.Print(TutorialTelemetry.Report());
			}
		}

		private static void Audio(DevConsole console)
		{
			if (SettingsApplier.Instance == null)
			{
				console.Print("No SettingsApplier - no audio setting is being applied.");
			}
			else
			{
				console.Print(SettingsApplier.Instance.AudioReport());
			}
		}

		private static void About(DevConsole console, string[] args)
		{
			if (args.Length != 0 && args[0].Equals("reset", StringComparison.OrdinalIgnoreCase))
			{
				AboutView.ForgetSeenBuild();
				console.Print("Build note marker cleared - it will open by itself when you return to the menu.");
			}
			else if (AboutView.Instance == null)
			{
				console.Print("No AboutView in this scene.");
			}
			else
			{
				AboutView.Instance.Show();
			}
		}

		private static void Help(DevConsole console, string[] args)
		{
			if (args.Length != 0 && DevCommandRegistry.TryGet(args[0], out var command))
			{
				console.Print(command.Name + " " + command.Usage + (command.Cheat ? "  [cheat]" : ""));
				console.Print("  " + command.Help);
				return;
			}
			StringBuilder stringBuilder = new StringBuilder($"{DevCommandRegistry.Count} commands ([cheat] = needs sv_cheats 1):");
			foreach (DevCommand item in DevCommandRegistry.All)
			{
				stringBuilder.Append(string.Format("\n  {0,-12} {1}{2}", item.Name, item.Cheat ? "[cheat] " : "", item.Help));
			}
			console.Print(stringBuilder.ToString());
		}

		private static void UnityLog(DevConsole console, string[] args)
		{
			int mirrorToUnityLog;
			if (args.Length != 0)
			{
				string text = args[0].ToLowerInvariant();
				mirrorToUnityLog = ((text == "on" || text == "1" || text == "true") ? 1 : 0);
			}
			else
			{
				mirrorToUnityLog = ((!console.MirrorToUnityLog) ? 1 : 0);
			}
			console.MirrorToUnityLog = (byte)mirrorToUnityLog != 0;
			console.Print("Mirror to Unity console: " + (console.MirrorToUnityLog ? "on" : "off"));
		}

		private static void Binds(DevConsole console, string[] args)
		{
			if (args.Length != 0 && args[0].Equals("reset", StringComparison.OrdinalIgnoreCase))
			{
				GameInput.ResetAll();
				console.Print("All key bindings reset to default.");
				return;
			}
			string text = null;
			foreach (GameInput.Command command in GameInput.Commands)
			{
				if (command.Group != text)
				{
					text = command.Group;
					console.Print("-- " + text);
				}
				string text2 = (GameInput.IsConflicting(command) ? "  (CONFLICT)" : "");
				console.Print($"  {command.Label,-18} {command.DisplayString,-14} {command.Id}{text2}");
			}
		}

		private static void Quit(DevConsole console)
		{
			console.Print("Quitting.");
			Application.Quit();
		}

		private static void TimeScale(DevConsole console, string[] args)
		{
			float value;
			if (args.Length == 0)
			{
				console.Print("timescale = " + Time.timeScale.ToString("0.###", CultureInfo.InvariantCulture));
			}
			else if (TryParse(console, args[0], out value))
			{
				Time.timeScale = Mathf.Clamp(value, 0f, 10f);
				Time.fixedDeltaTime = 0.02f * Mathf.Max(Time.timeScale, 0.0001f);
				console.Print("timescale = " + Time.timeScale.ToString("0.###", CultureInfo.InvariantCulture));
			}
		}

		private static void VSync(DevConsole console, string[] args)
		{
			int value;
			if (args.Length == 0)
			{
				console.Print($"vsync = {GameSettings.VSyncCount}");
			}
			else if (TryParse(console, args[0], out value))
			{
				GameSettings.SetVSyncCount(value);
				console.Print($"vsync = {GameSettings.VSyncCount}");
			}
		}

		private static void MaxFps(DevConsole console, string[] args)
		{
			int value;
			if (args.Length == 0)
			{
				console.Print("maxfps = " + Limit(GameSettings.FrameRateLimit));
			}
			else if (TryParse(console, args[0], out value))
			{
				GameSettings.SetFrameRateLimit((value >= 0) ? Mathf.Max(10, value) : 0);
				console.Print("maxfps = " + Limit(GameSettings.FrameRateLimit) + ((GameSettings.VSyncCount > 0) ? "  (no effect while vsync is on)" : ""));
			}
		}

		private static string Limit(int stored)
		{
			if (stored > 0)
			{
				return stored.ToString();
			}
			return "-1";
		}

		private static void Screenshot(DevConsole console, string[] args)
		{
			int num = 1;
			if (args.Length != 0 && TryParse(console, args[0], out int value))
			{
				num = Mathf.Clamp(value, 1, 8);
			}
			string path = $"Mimicraft_{DateTime.Now:yyyyMMdd_HHmmss}.png";
			string text = Path.Combine(Application.persistentDataPath, path);
			ScreenCapture.CaptureScreenshot(text, num);
			console.Print($"Screenshot requested (x{num}): {text}");
		}

		private static void Resolution(DevConsole console, string[] args)
		{
			int value;
			int value2;
			if (args.Length < 2)
			{
				console.Print($"{Screen.width}x{Screen.height} @ {Screen.currentResolution.refreshRateRatio.value:0.#}Hz" + $"  {Screen.fullScreenMode}");
			}
			else if (TryParse(console, args[0], out value) && TryParse(console, args[1], out value2))
			{
				FullScreenMode fullScreenMode = Screen.fullScreenMode;
				if (args.Length > 2)
				{
					string text = args[2].ToLowerInvariant();
					FullScreenMode fullScreenMode2 = ((!(text == "full")) ? ((!(text == "window")) ? fullScreenMode : FullScreenMode.Windowed) : FullScreenMode.ExclusiveFullScreen);
					fullScreenMode = fullScreenMode2;
				}
				GameSettings.SetResolution(Mathf.Max(320, value), Mathf.Max(240, value2));
				GameSettings.SetWindowMode(fullScreenMode);
				console.Print($"{GameSettings.ResolutionWidth}x{GameSettings.ResolutionHeight} {fullScreenMode}");
			}
		}

		private static void Scenes(DevConsole console)
		{
			Scene activeScene = SceneManager.GetActiveScene();
			StringBuilder stringBuilder = new StringBuilder($"{SceneManager.sceneCount} scenes loaded:");
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene sceneAt = SceneManager.GetSceneAt(i);
				stringBuilder.Append("\n  " + ((sceneAt == activeScene) ? "*" : " ") + " " + sceneAt.name + string.Format("  ({0} root objects{1})", sceneAt.rootCount, sceneAt.isLoaded ? "" : ", loading"));
			}
			stringBuilder.Append("\n  * = active scene (RenderSettings are read from it)");
			console.Print(stringBuilder.ToString());
		}

		private static void Net(DevConsole console)
		{
			NetworkManager singleton = NetworkManager.Singleton;
			if (singleton == null)
			{
				console.Print("No NetworkManager - networking has not started.");
				return;
			}
			if (!singleton.IsListening)
			{
				console.Print("Networking is not running (no host/client started).");
				return;
			}
			string arg = (singleton.IsHost ? "Host" : (singleton.IsServer ? "Server" : "Client"));
			console.Print($"{arg}  clientId={singleton.LocalClientId}  " + $"connections={singleton.ConnectedClientsIds.Count}  " + "transport=" + (singleton.NetworkConfig.NetworkTransport?.GetType().Name ?? "none"));
		}

		private static void WeaponPreview(DevConsole console)
		{
			WeaponCustomizationView weaponCustomizationView = UnityEngine.Object.FindFirstObjectByType<WeaponCustomizationView>(FindObjectsInactive.Include);
			if (weaponCustomizationView == null)
			{
				console.Print("No WeaponCustomizationView in the loaded scenes.");
			}
			else
			{
				PrintLines(console, weaponCustomizationView.PreviewReport());
			}
		}

		private static void Library(DevConsole console)
		{
			TemplateBrowserView templateBrowserView = UnityEngine.Object.FindFirstObjectByType<TemplateBrowserView>(FindObjectsInactive.Include);
			if (templateBrowserView == null)
			{
				console.Print("No TemplateBrowserView in the scene.");
			}
			else
			{
				PrintLines(console, templateBrowserView.Report());
			}
		}

		private static void PrintLines(DevConsole console, string report)
		{
			string[] array = (report ?? "").Split('\n');
			foreach (string text in array)
			{
				console.Print(text.TrimEnd());
			}
		}

		private static void TickRate(DevConsole console, string[] args)
		{
			NetworkManager singleton = NetworkManager.Singleton;
			bool flag = singleton != null && singleton.IsListening;
			uint rate;
			if (args.Length == 0)
			{
				console.Print("Hosting tick rate: " + ServerTickRate.Describe(ServerTickRate.Preferred));
				if (flag)
				{
					console.Print("This session runs at: " + ServerTickRate.Describe(ServerTickRate.Active));
				}
				console.Print($"Presets: default {30u}, high {64u}, " + $"ultra {128u}. Custom: {10u}-{256u}.");
			}
			else if (flag)
			{
				console.Print("The tick rate can only be changed from the menu, outside a session.");
			}
			else if (!ServerTickRate.TryParse(args[0], out rate))
			{
				console.Print("Unknown value '" + args[0] + "'. Use default, high, ultra or a number from " + $"{10u} to {256u}.");
			}
			else
			{
				ServerTickRate.Preferred = rate;
				console.Print("Hosting tick rate set to " + ServerTickRate.Describe(rate) + ". It applies from the next session you host.");
			}
		}

		private static void Clearance(DevConsole console)
		{
			PlayerVoxelBody local = PlayerVoxelBody.Local;
			if (local == null || !local.HasVoxelBody)
			{
				console.Print("You have no voxel body.");
			}
			else
			{
				PrintLines(console, local.ClearanceReport());
			}
		}

		private static void VoteKick(DevConsole console, string[] args)
		{
			NetworkManager singleton = NetworkManager.Singleton;
			if (singleton == null || !singleton.IsListening)
			{
				console.Print("Networking is not running.");
				return;
			}
			GameModeController current = GameModeController.Current;
			if (current == null || !current.IsSpawned)
			{
				console.Print("No game mode is running.");
				return;
			}
			if (args.Length == 0)
			{
				OpenVoteKickList(console, singleton, current);
				return;
			}
			if (!TryFindClient(singleton, current, string.Join(" ", args), out var found, out var failure))
			{
				console.Print(failure);
				return;
			}
			if (found == singleton.LocalClientId)
			{
				console.Print("You cannot vote yourself out. Ask somebody else.");
				return;
			}
			if (found == 0L)
			{
				console.Print("The host cannot be voted out - they are the lobby, so it would end the game for everyone including whoever voted. Leaving is the answer to a bad host.");
				return;
			}
			if (singleton.ConnectedClientsIds.Count <= 2)
			{
				console.Print("A vote needs three players and this lobby has " + $"{singleton.ConnectedClientsIds.Count}: the target does not vote, so two people " + "cannot reach a majority without them.");
				return;
			}
			if (current.VoteKick.Value.Active)
			{
				console.Print("A vote is already running. One at a time.");
				return;
			}
			current.StartVoteKickServerRpc(found);
			console.Print($"Vote started against {current.GetPlayerName(found)} ({found}). " + "Everybody answers with the Yes/No keys, and it decides as soon as the answer cannot change. If nothing happened, the last vote's cooldown is probably still running.");
		}

		private static void OpenVoteKickList(DevConsole console, NetworkManager manager, GameModeController mode)
		{
			VoteKickView voteKickView = UnityEngine.Object.FindFirstObjectByType<VoteKickView>(FindObjectsInactive.Include);
			if (voteKickView == null)
			{
				console.Print("No VoteKickView in the loaded scenes, so there is no list to open. Here is who could be voted on:");
				PrintVoteKickTargets(console, manager, mode);
			}
			else if (mode.VoteKick.Value.Active)
			{
				console.Print("A vote is already running, so the list would refuse every row. One at a time.");
			}
			else if (manager.ConnectedClientsIds.Count <= 2)
			{
				console.Print("A vote needs three players and this lobby has " + $"{manager.ConnectedClientsIds.Count}: the target does not vote, so two people " + "cannot reach a majority without them.");
			}
			else if (!mode.VoteKickAvailable)
			{
				console.Print("Vote kick is not available in this session. Off Steam it needs `sv_cheats 1`, set by the host.");
			}
			else
			{
				voteKickView.OpenList();
				console.Close();
			}
		}

		private static void PrintVoteKickTargets(DevConsole console, NetworkManager manager, GameModeController mode)
		{
			StringBuilder stringBuilder = new StringBuilder("Who can be voted on:");
			foreach (ulong connectedClientsId in manager.ConnectedClientsIds)
			{
				string arg = ((connectedClientsId == 0L) ? "  (host - cannot be voted out)" : ((connectedClientsId == manager.LocalClientId) ? "  (you)" : ""));
				stringBuilder.Append($"\n  {connectedClientsId,3}  {mode.GetPlayerName(connectedClientsId)}{arg}");
			}
			stringBuilder.Append("\n\nVote kick is " + (mode.VoteKickAvailable ? "available" : "NOT available") + " here.");
			if (!mode.VoteKickAvailable && manager.ConnectedClientsIds.Count > 2)
			{
				stringBuilder.Append(" Off Steam it needs sv_cheats 1, set by the host.");
			}
			PrintLines(console, stringBuilder.ToString());
		}

		private static bool TryFindClient(NetworkManager manager, GameModeController mode, string query, out ulong found, out string failure)
		{
			found = 0uL;
			failure = "";
			if (ulong.TryParse(query, out var result))
			{
				if (manager.ConnectedClients.ContainsKey(result))
				{
					found = result;
					return true;
				}
				failure = $"No player with id {result}. Run `votekick` with no name to see the list.";
				return false;
			}
			List<ulong> list = new List<ulong>();
			foreach (ulong connectedClientsId in manager.ConnectedClientsIds)
			{
				string playerName = mode.GetPlayerName(connectedClientsId);
				if (!string.IsNullOrEmpty(playerName) && playerName.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					list.Add(connectedClientsId);
				}
			}
			if (list.Count == 1)
			{
				found = list[0];
				return true;
			}
			if (list.Count == 0)
			{
				failure = "Nobody here matches '" + query + "'. Run `votekick` with no name to see the list.";
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder($"'{query}' matches {list.Count} players - use the id:");
			foreach (ulong item in list)
			{
				stringBuilder.Append($"  {item} ({mode.GetPlayerName(item)})");
			}
			failure = stringBuilder.ToString();
			return false;
		}

		private static void Players(DevConsole console)
		{
			NetworkManager singleton = NetworkManager.Singleton;
			if (singleton == null || !singleton.IsListening)
			{
				console.Print("Networking is not running.");
				return;
			}
			RoundManager roundManager = UnityEngine.Object.FindFirstObjectByType<RoundManager>();
			StringBuilder stringBuilder = new StringBuilder($"{singleton.ConnectedClientsIds.Count} players:");
			foreach (ulong connectedClientsId in singleton.ConnectedClientsIds)
			{
				string arg = ((singleton.IsServer && roundManager != null) ? roundManager.GetServerRole(connectedClientsId).ToString() : ((connectedClientsId == singleton.LocalClientId) ? "(you)" : "?"));
				string arg2 = "?";
				if (singleton.ConnectedClients.TryGetValue(connectedClientsId, out var value) && value.PlayerObject != null)
				{
					PlayerHealth component = value.PlayerObject.GetComponent<PlayerHealth>();
					if (component != null)
					{
						arg2 = component.Health.Value.ToString();
					}
				}
				stringBuilder.Append($"\n  {connectedClientsId,3}  {arg,-8} hp={arg2}");
			}
			console.Print(stringBuilder.ToString());
		}

		private static void Bodies(DevConsole console)
		{
			PlayerVoxelBody[] array = UnityEngine.Object.FindObjectsByType<PlayerVoxelBody>(FindObjectsSortMode.None);
			if (array.Length == 0)
			{
				console.Print("No players in this scene.");
				return;
			}
			int minBoundExtent = VoxelEditorSettings.MinBoundExtent;
			int minIslandVoxels = VoxelEditorSettings.MinIslandVoxels;
			console.Print($"rules in force here: min extent {minBoundExtent}, min voxels per clump {minIslandVoxels}");
			PlayerVoxelBody[] array2 = array;
			foreach (PlayerVoxelBody playerVoxelBody in array2)
			{
				string arg = ((GameModeController.Current != null) ? GameModeController.Current.GetPlayerName(playerVoxelBody.OwnerClientId) : "");
				byte[] data = playerVoxelBody.ModelData.Value.Data;
				if (data == null || data.Length == 0)
				{
					console.Print($"{playerVoxelBody.OwnerClientId,3} {arg}: no voxel body");
					continue;
				}
				if (!VoxelBodyCodec.TryDecodeSummary(data, out var summary, 200000, 64, findIslands: true))
				{
					console.Print($"{playerVoxelBody.OwnerClientId,3} {arg}: payload of {data.Length} bytes does not decode");
					continue;
				}
				console.Print($"{playerVoxelBody.OwnerClientId,3} {arg}: {summary.Pieces.Count} piece(s), " + $"{summary.TotalVoxels} voxels, voxel size {summary.VoxelSize:0.####}, {data.Length} bytes");
				for (int j = 0; j < summary.Pieces.Count; j++)
				{
					VoxelPieceSummary voxelPieceSummary = summary.Pieces[j];
					if (!voxelPieceSummary.HasVoxels)
					{
						console.Print($"      piece {j}: empty");
						continue;
					}
					Vector3Int vector3Int = voxelPieceSummary.Max - voxelPieceSummary.Min + Vector3Int.one;
					int num = ((voxelPieceSummary.Islands != null) ? voxelPieceSummary.Islands.Count : 0);
					console.Print($"      piece {j}: box {vector3Int.x}x{vector3Int.y}x{vector3Int.z}, {voxelPieceSummary.Voxels} voxels, " + $"{num} clump(s)");
					if (voxelPieceSummary.Islands == null)
					{
						continue;
					}
					foreach (VoxelIsland island in voxelPieceSummary.Islands)
					{
						if (!island.IsSubstantial(minBoundExtent, VoxelEditorSettings.SlimBoundExtent, minIslandVoxels))
						{
							console.Print($"         TOO SMALL: clump {island.ExtentX}x{island.ExtentY}x{island.ExtentZ}, " + $"{island.Voxels} voxels");
						}
					}
				}
			}
		}

		private static void Round(DevConsole console)
		{
			RoundManager roundManager = UnityEngine.Object.FindFirstObjectByType<RoundManager>();
			if (roundManager == null)
			{
				console.Print("No RoundManager in the scene - this mode has no rounds.");
				return;
			}
			double num = ((roundManager.PhaseEndServerTime.Value <= 0.0 || NetworkManager.Singleton == null) ? 0.0 : (roundManager.PhaseEndServerTime.Value - NetworkManager.Singleton.ServerTime.Time));
			console.Print($"phase={roundManager.CurrentPhase.Value}  remaining={Mathf.Max(0f, (float)num):0.0}s");
			PrintLines(console, roundManager.SplitReport());
		}

		private static void Version(DevConsole console)
		{
			console.Print(Application.productName + " " + Application.version + "\n" + $"  Unity {Application.unityVersion}  {Application.platform}\n" + "  " + (Debug.isDebugBuild ? "development build" : "release build") + "    data: " + Application.persistentDataPath);
		}

		private static void Toggle(DevConsole console, string[] args, bool current, Action<bool> set, string label)
		{
			int num;
			if (args.Length != 0)
			{
				string text = args[0].ToLowerInvariant();
				num = ((text == "on" || text == "1" || text == "true") ? 1 : 0);
			}
			else
			{
				num = ((!current) ? 1 : 0);
			}
			bool flag = (byte)num != 0;
			set(flag);
			console.Print(label + ": " + (flag ? "on" : "off"));
		}

		private static bool TryParse(DevConsole console, string text, out float value)
		{
			if (float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
			{
				return true;
			}
			console.Print("'" + text + "' is not a number.");
			return false;
		}

		private static bool TryParse(DevConsole console, string text, out int value)
		{
			if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
			{
				return true;
			}
			console.Print("'" + text + "' is not an integer.");
			return false;
		}

		private static void TpsWeapon(DevConsole console, string[] args)
		{
			NetworkManager singleton = NetworkManager.Singleton;
			if (singleton == null)
			{
				console.Print("No NetworkManager.");
				return;
			}
			string text = ((args.Length != 0) ? string.Join(" ", args) : null);
			GameObject gameObject = null;
			foreach (NetworkClient connectedClients in singleton.ConnectedClientsList)
			{
				if (connectedClients.PlayerObject == null)
				{
					continue;
				}
				bool num;
				if (text != null)
				{
					if (!(GameModeController.Current != null))
					{
						continue;
					}
					num = string.Equals(GameModeController.Current.GetPlayerName(connectedClients.ClientId), text, StringComparison.OrdinalIgnoreCase);
				}
				else
				{
					num = connectedClients.ClientId == singleton.LocalClientId;
				}
				if (num)
				{
					gameObject = connectedClients.PlayerObject.gameObject;
					break;
				}
			}
			if (gameObject == null)
			{
				console.Print((text == null) ? "No local player." : ("No player named '" + text + "'."));
				return;
			}
			PlayerWeapons component = gameObject.GetComponent<PlayerWeapons>();
			GameObject gameObject2 = ((component != null) ? component.ThirdPersonInstance : null);
			if (gameObject2 == null)
			{
				console.Print(gameObject.name + ": no third-person weapon (unarmed, or hidden).");
				return;
			}
			console.Print("== " + gameObject.name + " - " + gameObject2.name + " (root -> model root)");
			Transform transform = gameObject2.transform;
			int num2 = 0;
			while (transform != null && num2 < 24)
			{
				Vector3 eulerAngles = transform.localRotation.eulerAngles;
				Vector3 eulerAngles2 = transform.rotation.eulerAngles;
				Vector3 localPosition = transform.localPosition;
				string text2 = "";
				WeaponSway component2 = transform.GetComponent<WeaponSway>();
				if (component2 != null)
				{
					text2 = text2 + "  [sway: " + component2.DebugState + "]";
				}
				Rig component3 = transform.GetComponent<Rig>();
				if (component3 != null)
				{
					text2 += $"  [rig weight {component3.weight:0.00}]";
				}
				if (transform.GetComponent<Animator>() != null)
				{
					text2 += "  [Animator]";
				}
				console.Print($"{new string(' ', num2)}{transform.name}: pos {localPosition:F3} local {Wrap(eulerAngles)} world {Wrap(eulerAngles2)}{text2}");
				if (transform == gameObject.transform)
				{
					break;
				}
				transform = transform.parent;
				num2++;
			}
			PlayerAnimator componentInChildren = gameObject.GetComponentInChildren<PlayerAnimator>(includeInactive: true);
			if (componentInChildren != null)
			{
				console.Print(componentInChildren.RigReport());
			}
		}

		private static string Wrap(Vector3 euler)
		{
			return $"({W(euler.x):0.0}, {W(euler.y):0.0}, {W(euler.z):0.0})";
			static float W(float a)
			{
				if (!(a > 180f))
				{
					return a;
				}
				return a - 360f;
			}
		}

		private static async void Lobbies(DevConsole console)
		{
			if (!SteamManager.IsInitialized)
			{
				console.Print("Steam is not running - cannot scan lobbies.");
				return;
			}
			console.Print("Scanning Steam lobbies...");
			Lobby[] array;
			try
			{
				array = await SteamMatchmaking.LobbyList.WithMaxResults(20).RequestAsync();
			}
			catch (Exception ex)
			{
				console.Print("Scan failed: " + ex.Message);
				return;
			}
			if (array == null || array.Length == 0)
			{
				console.Print("No lobbies found.");
				return;
			}
			Lobby[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				Lobby lobby = array2[i];
				console.Print($"== {lobby.Id} members {lobby.MemberCount}/{lobby.MaxMembers}");
				string[] array3 = new string[18]
				{
					"name", "mode", "map", "prep_seconds", "hunt_seconds", "round_end_seconds", "min_hiders", "min_hunters", "has_password", "lang",
					"country", "ping_location", "tickrate", "min_players", "score_limit", "time_limit_seconds", "extra_warmup_seconds", "respawn_seconds"
				};
				foreach (string text in array3)
				{
					string data = lobby.GetData(text);
					console.Print("   " + text + " = " + (string.IsNullOrEmpty(data) ? "(empty)" : data));
				}
			}
		}

		private static void Body(DevConsole console, string[] args)
		{
			if (args.Length != 0 && args[0] == "reset")
			{
				PlayerVoxelBody.ResetSendStats();
				VoxelModel.ResetBuildStats();
				console.Print("Body counters reset.");
				return;
			}
			NetworkManager singleton = NetworkManager.Singleton;
			PlayerVoxelBody playerVoxelBody = ((singleton != null && singleton.LocalClient != null && singleton.LocalClient.PlayerObject != null) ? singleton.LocalClient.PlayerObject.GetComponent<PlayerVoxelBody>() : null);
			console.Print($"sends: {PlayerVoxelBody.SendCount} times, last encode {PlayerVoxelBody.LastEncodeMs:0.00} ms " + $"({PlayerVoxelBody.LastPayloadBytes} bytes), total encode {PlayerVoxelBody.EncodeMsTotal:0.0} ms");
			console.Print($"server validation: {PlayerVoxelBody.ValidateCount} times, last {PlayerVoxelBody.LastValidateMs:0.00} ms, " + $"total {PlayerVoxelBody.ValidateMsTotal:0.0} ms" + ((singleton != null && singleton.IsServer) ? "" : " (this machine is not the server)"));
			console.Print($"mesh builds: {VoxelModel.MeshBuildCount} times, total {VoxelModel.MeshBuildMsTotal:0.0} ms");
			console.Print($"collider bakes: {VoxelModel.ColliderBakeCount} times, last {VoxelModel.LastColliderBakeMs:0.00} ms, " + $"total {VoxelModel.ColliderBakeMsTotal:0.0} ms");
			if (playerVoxelBody != null)
			{
				console.Print(playerVoxelBody.HasPendingBody ? $"pending send: yes, for {playerVoxelBody.PendingBodyAgeSeconds:0.00} s" : "pending send: none");
			}
		}

		private static void Ads(DevConsole console)
		{
			NetworkManager singleton = NetworkManager.Singleton;
			GameObject gameObject = ((singleton != null && singleton.LocalClient != null && singleton.LocalClient.PlayerObject != null) ? singleton.LocalClient.PlayerObject.gameObject : null);
			if (gameObject == null)
			{
				console.Print("No local player - try after joining a match.");
				return;
			}
			PlayerWeapons componentInChildren = gameObject.GetComponentInChildren<PlayerWeapons>(includeInactive: true);
			if (componentInChildren == null || componentInChildren.FpsVisual == null)
			{
				console.Print("No first-person weapon model.");
				return;
			}
			PlayerScope componentInChildren2 = gameObject.GetComponentInChildren<PlayerScope>(includeInactive: true);
			WeaponDefinition shown = componentInChildren.Shown;
			console.Print("weapon=" + ((shown != null) ? shown.WeaponId : "-") + "  aiming=" + ((componentInChildren2 != null && componentInChildren2.IsAiming) ? "YES" : "no") + $"  feature={Features.AimDownSights}");
			GameObject firstPersonInstance = componentInChildren.FirstPersonInstance;
			WeaponSkinAssembler weaponSkinAssembler = ((firstPersonInstance != null) ? firstPersonInstance.GetComponentInChildren<WeaponSkinAssembler>(includeInactive: true) : null);
			console.Print((weaponSkinAssembler == null) ? "assembler: NONE" : ("assembler: " + weaponSkinAssembler.AdsReport));
			WeaponVisual fpsVisual = componentInChildren.FpsVisual;
			console.Print("visual: " + fpsVisual.AdsReport);
			Transform aimPivot = fpsVisual.AimPivot;
			console.Print((aimPivot == null) ? "pivot: NONE" : ("pivot now=" + aimPivot.localPosition.ToString("F3") + "  socket=" + ((aimPivot.parent != null && aimPivot.parent.parent != null) ? aimPivot.parent.parent.localPosition.ToString("F3") : "?")));
			PlayerCameraRig component = gameObject.GetComponent<PlayerCameraRig>();
			Camera camera = ((component != null) ? component.FpsCamera : null);
			Vector3 world;
			if (camera == null)
			{
				console.Print("camera: NONE");
			}
			else if (!fpsVisual.TryGetAdsAimPointWorld(out world))
			{
				console.Print("ADS point: none (this weapon uses Ads Pivot Position)");
			}
			else
			{
				console.Print("ADS point relative to camera=" + camera.transform.InverseTransformPoint(world).ToString("F3") + "  (x/y = offset from centre, metres)");
			}
		}

		private static void FpsArms(DevConsole console, string[] args)
		{
			NetworkManager singleton = NetworkManager.Singleton;
			GameObject gameObject = ((singleton != null && singleton.LocalClient != null && singleton.LocalClient.PlayerObject != null) ? singleton.LocalClient.PlayerObject.gameObject : null);
			if (gameObject == null)
			{
				console.Print("No local player - try after joining a match.");
				return;
			}
			PlayerWeapons componentInChildren = gameObject.GetComponentInChildren<PlayerWeapons>(includeInactive: true);
			if (componentInChildren == null)
			{
				console.Print("PlayerWeapons not found.");
				return;
			}
			GameObject firstPersonInstance = componentInChildren.FirstPersonInstance;
			console.Print((firstPersonInstance == null) ? "view model: NONE (if unarmed, Unarmed Fps Prefab may be empty)" : $"view model: {firstPersonInstance.name}  active={firstPersonInstance.activeInHierarchy}");
			if (firstPersonInstance == null)
			{
				return;
			}
			PlayerFight componentInChildren2 = gameObject.GetComponentInChildren<PlayerFight>(includeInactive: true);
			console.Print((componentInChildren2 == null) ? "PlayerFight: NONE" : ("PlayerFight: punch " + ((componentInChildren2.FightRefusalReason == null) ? "ALLOWED" : ("REFUSED - " + componentInChildren2.FightRefusalReason)) + $"  fighting={componentInChildren2.IsFighting} blocking={componentInChildren2.IsBlocking}"));
			PlayerAnimator componentInChildren3 = gameObject.GetComponentInChildren<PlayerAnimator>(includeInactive: true);
			console.Print((componentInChildren3 == null) ? "PlayerAnimator: NONE" : $"PlayerAnimator: active={componentInChildren3.isActiveAndEnabled} object={componentInChildren3.gameObject.name}");
			if (componentInChildren3 != null)
			{
				console.Print("  last mirror: " + componentInChildren3.LastMirrorReport);
			}
			Animator componentInChildren4 = firstPersonInstance.GetComponentInChildren<Animator>(includeInactive: true);
			if (componentInChildren4 == null)
			{
				console.Print("Animator: NONE - no Animator on or under the rig.");
				return;
			}
			console.Print($"Animator: {componentInChildren4.name}  enabled={componentInChildren4.enabled}  " + $"active={componentInChildren4.gameObject.activeInHierarchy}");
			console.Print("  controller=" + ((componentInChildren4.runtimeAnimatorController != null) ? componentInChildren4.runtimeAnimatorController.name : "NONE") + $"  culling={componentInChildren4.cullingMode}  update={componentInChildren4.updateMode}");
			if (!(componentInChildren4.runtimeAnimatorController == null))
			{
				StringBuilder stringBuilder = new StringBuilder("  parameters:");
				AnimatorControllerParameter[] parameters = componentInChildren4.parameters;
				foreach (AnimatorControllerParameter animatorControllerParameter in parameters)
				{
					stringBuilder.Append(' ').Append(animatorControllerParameter.name).Append('(')
						.Append(animatorControllerParameter.type)
						.Append(')');
				}
				console.Print((componentInChildren4.parameters.Length == 0) ? "  parameters: NONE" : stringBuilder.ToString());
				for (int j = 0; j < componentInChildren4.layerCount; j++)
				{
					AnimatorStateInfo currentAnimatorStateInfo = componentInChildren4.GetCurrentAnimatorStateInfo(j);
					console.Print($"  layer {j} '{componentInChildren4.GetLayerName(j)}' weight={componentInChildren4.GetLayerWeight(j):0.00}" + $" state={currentAnimatorStateInfo.shortNameHash} t={currentAnimatorStateInfo.normalizedTime:0.00}");
				}
				if (args == null || args.Length == 0 || args[0] != "punch")
				{
					console.Print("  ('fpsarms punch' sends the trigger directly)");
					return;
				}
				int shortNameHash = componentInChildren4.GetCurrentAnimatorStateInfo(0).shortNameHash;
				componentInChildren4.SetInteger("PunchIndex", 0);
				componentInChildren4.SetTrigger("Punch");
				console.Print($"  Punch sent DIRECTLY. previous state={shortNameHash}" + " - check again with 'fpsarms' in a second.");
			}
		}
	}
}
