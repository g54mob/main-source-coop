using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

namespace Mimicraft.Dev
{
	public static class DevRecordCommands
	{
		private const float DefaultTickRate = 30f;

		public static void RegisterAll(DevConsole console)
		{
			DevCommandRegistry.Register("record", "[name] [tickrate]", "Starts recording the game. Stops by itself when the round ends and writes the file.", delegate(string[] args)
			{
				Record(console, args);
			});
			DevCommandRegistry.Register("stop", "", "Stops the recording and writes it to a file. Closes the replay if one is open.", delegate
			{
				Stop(console);
			});
			DevCommandRegistry.Register("play", "<name>", "Plays a recording back as ghosts. Fly around it with `freecam`.", delegate(string[] args)
			{
				Play(console, args);
			});
			DevCommandRegistry.Register("records", "", "Lists the saved recordings.", delegate
			{
				Records(console);
			});
			DevCommandRegistry.Register("pause", "[on|off]", "Pauses the replay - to film a frozen moment from any angle.", delegate(string[] args)
			{
				Pause(console, args);
			});
			DevCommandRegistry.Register("seek", "<seconds>", "Scrubs the replay to a moment.", delegate(string[] args)
			{
				Seek(console, args);
			});
			DevCommandRegistry.Register("replayui", "[on|off]", "Hides/shows the replay bar. F2 does the same - to get it out of the frame while filming.", delegate(string[] args)
			{
				ReplayUi(console, args);
			});
			DevCommandRegistry.Register("ghostui", "[on|off]", "Opens/closes the ghost editor. It only shows while the replay bar is up anyway - hiding the bar (F2) removes both the labels and the panel.", delegate(string[] args)
			{
				GhostUi(console, args);
			});
			DevCommandRegistry.Register("ghosts", "", "Prints the ghosts' animator state - for when characters do not move in a replay.", delegate
			{
				Ghosts(console);
			});
			DevCommandRegistry.Register("campath", "rec <name> | stop | play <name> | list | smooth [passes] | loop | speed [multiplier]", "Records a camera move and flies it again exactly - for a repeatable dolly shot.", delegate(string[] args)
			{
				CamPath(console, args);
			});
			DevCommandRegistry.Register("replayspeed", "[multiplier]", "Replay speed. 0.25 is slow motion, 2 is fast.", delegate(string[] args)
			{
				Speed(console, args);
			});
		}

		private static void Record(DevConsole console, string[] args)
		{
			if (DevRecorder.Active)
			{
				console.Print("Already recording: '" + DevRecorder.Instance.Name + "' " + $"({DevRecorder.Instance.Seconds:0.0}s). Run `stop` first.");
				return;
			}
			string name = ((args.Length != 0) ? args[0] : $"rec_{DateTime.Now:yyyyMMdd_HHmmss}");
			float num = 30f;
			if (args.Length > 1 && float.TryParse(args[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
			{
				num = result;
			}
			DevRecorder.Begin(name, num);
			console.Print($"Recording started: '{DevRecorder.Instance.Name}' @ {num:0.#} Hz. " + "It stops by itself when the round ends, or on `stop`.");
		}

		private static void Stop(DevConsole console)
		{
			bool flag = false;
			if (DevReplay.Active)
			{
				DevReplay.Stop();
				console.Print("Replay stopped.");
				flag = true;
			}
			if (DevRecorder.Active)
			{
				int frames = DevRecorder.Instance.Frames;
				int actorCount = DevRecorder.Instance.ActorCount;
				string text = DevRecorder.Stop(save: true);
				console.Print((text.Length > 0) ? $"Recording written: {text}\n  {frames} frames, {actorCount} players." : "Recording was empty - no file written.");
				flag = true;
			}
			if (!flag)
			{
				console.Print("Neither recording nor replay is active.");
			}
		}

		private static void Play(DevConsole console, string[] args)
		{
			if (args.Length == 0)
			{
				Records(console);
				return;
			}
			string path = DevRecording.PathFor(args[0]);
			DevRecording recording;
			string problem;
			if (!File.Exists(path))
			{
				console.Print("No recording called '" + args[0] + "'. `records` lists them.");
			}
			else if (!DevRecording.TryLoad(path, out recording))
			{
				console.Print("Could not read '" + args[0] + "' - the file may be corrupt.");
			}
			else if (!DevReplay.Begin(recording, out problem))
			{
				console.Print(problem);
			}
			else
			{
				console.Print($"Playing: '{recording.Name}'  {recording.Duration:0.0}s, " + $"{DevReplay.Instance.GhostCount} ghosts.\n" + "  `freecam` to fly around, `pause` to freeze, `seek <seconds>` to scrub.");
			}
		}

		private static void Records(DevConsole console)
		{
			List<string> list = DevRecording.ListNames();
			if (list.Count == 0)
			{
				console.Print("No recordings. Folder: " + DevRecording.Directory);
				return;
			}
			StringBuilder stringBuilder = new StringBuilder($"{list.Count} recordings ({DevRecording.Directory}):");
			foreach (string item in list)
			{
				FileInfo fileInfo = new FileInfo(DevRecording.PathFor(item));
				stringBuilder.Append($"\n  {item,-28} {(float)fileInfo.Length / 1024f:0.#} KB   {fileInfo.LastWriteTime:dd.MM HH:mm}");
			}
			console.Print(stringBuilder.ToString());
		}

		private static void Pause(DevConsole console, string[] args)
		{
			if (!DevReplay.Active)
			{
				console.Print("Replay is not active.");
				return;
			}
			DevReplay instance = DevReplay.Instance;
			int paused;
			if (args.Length != 0)
			{
				string text = args[0].ToLowerInvariant();
				paused = ((text == "on" || text == "1" || text == "true") ? 1 : 0);
			}
			else
			{
				paused = ((!DevReplay.Instance.Paused) ? 1 : 0);
			}
			instance.Paused = (byte)paused != 0;
			console.Print("Replay: " + (DevReplay.Instance.Paused ? "paused" : "playing") + "  " + $"({DevReplay.Instance.Time:0.0} / {DevReplay.Instance.Duration:0.0}s)");
		}

		private static void Seek(DevConsole console, string[] args)
		{
			if (!DevReplay.Active)
			{
				console.Print("Replay is not active.");
				return;
			}
			if (args.Length == 0 || !float.TryParse(args[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
			{
				console.Print($"Usage: seek <seconds>   (0 - {DevReplay.Instance.Duration:0.0})");
				return;
			}
			DevReplay.Instance.Seek(result);
			console.Print($"{DevReplay.Instance.Time:0.0} / {DevReplay.Instance.Duration:0.0}s");
		}

		private static void ReplayUi(DevConsole console, string[] args)
		{
			int visible;
			if (args.Length != 0)
			{
				string text = args[0].ToLowerInvariant();
				visible = ((text == "on" || text == "1" || text == "true") ? 1 : 0);
			}
			else
			{
				visible = ((!DevReplayBar.Visible) ? 1 : 0);
			}
			DevReplayBar.Visible = (byte)visible != 0;
			console.Print("Replay bar: " + (DevReplayBar.Visible ? "shown" : "hidden") + "  (F2)");
		}

		private static void GhostUi(DevConsole console, string[] args)
		{
			int visible;
			if (args.Length != 0)
			{
				string text = args[0].ToLowerInvariant();
				visible = ((text == "on" || text == "1" || text == "true") ? 1 : 0);
			}
			else
			{
				visible = ((!DevGhostInspector.Visible) ? 1 : 0);
			}
			DevGhostInspector.Visible = (byte)visible != 0;
			console.Print("Ghost editor: " + (DevGhostInspector.Visible ? "on" : "off"));
		}

		private static void Ghosts(DevConsole console)
		{
			if (!DevReplay.Active)
			{
				console.Print("Replay is not active.");
			}
			else
			{
				console.Print(DevReplay.Instance.DescribeGhosts());
			}
		}

		private static void CamPath(DevConsole console, string[] args)
		{
			string text = ((args.Length != 0) ? args[0].ToLowerInvariant() : "list");
			string text2 = ((args.Length > 1) ? args[1] : "");
			switch (text)
			{
			case "rec":
			case "record":
				if (!DevFreeCamera.Active)
				{
					console.Print("Free camera is off - run `freecam` first.");
					return;
				}
				DevCameraPathPlayer.BeginRecording((text2.Length > 0) ? text2 : $"path_{DateTime.Now:HHmmss}", 60f);
				console.Print("Recording camera path: '" + DevCameraPathPlayer.Current.Name + "'. Keep flying; `campath stop` when done.");
				return;
			case "stop":
				if (DevCameraPathPlayer.Recording)
				{
					int count = DevCameraPathPlayer.Current.Keys.Count;
					string text3 = DevCameraPathPlayer.EndRecording();
					console.Print((text3.Length > 0) ? $"Path written ({count} keys): {text3}" : "The camera never moved - no file written.");
				}
				else
				{
					DevCameraPathPlayer.Stop();
					console.Print("Camera path stopped.");
				}
				return;
			case "play":
			{
				if (!DevFreeCamera.Active)
				{
					console.Print("Free camera is off - run `freecam` first.");
					return;
				}
				string path = DevCameraPath.PathFor(text2);
				DevCameraPath loaded;
				if (text2.Length == 0 || !File.Exists(path))
				{
					console.Print("No path called '" + text2 + "'. `campath list` lists them.");
				}
				else if (!DevCameraPath.TryLoad(path, out loaded) || !DevCameraPathPlayer.BeginPlayback(loaded))
				{
					console.Print("Could not play '" + text2 + "'.");
				}
				else
				{
					console.Print($"Playing path: '{loaded.Name}'  {loaded.Duration:0.0}s, " + string.Format("{0} keys.  loop={1}", loaded.Keys.Count, DevCameraPathPlayer.Loop ? "on" : "off"));
				}
				return;
			}
			case "smooth":
			{
				if (DevCameraPathPlayer.Current == null)
				{
					console.Print("Record or play a path first.");
					return;
				}
				int num = 1;
				if (args.Length > 1 && int.TryParse(args[1], out var result))
				{
					num = Mathf.Clamp(result, 1, 10);
				}
				for (int i = 0; i < num; i++)
				{
					DevCameraPathPlayer.Current.Smooth(2);
				}
				DevCameraPathPlayer.Current.Save(DevCameraPath.PathFor(DevCameraPathPlayer.Current.Name));
				console.Print($"'{DevCameraPathPlayer.Current.Name}' smoothed ({num} pass(es)) and saved.");
				return;
			}
			case "speed":
			{
				if (text2.Length > 0 && float.TryParse(text2, NumberStyles.Float, CultureInfo.InvariantCulture, out var result2))
				{
					DevCameraPathPlayer.Speed = Mathf.Clamp(result2, 0.05f, 8f);
				}
				console.Print($"Path speed: {DevCameraPathPlayer.Speed:0.##}x" + ((text2.Length > 0) ? "" : " (pass a value to change it)"));
				return;
			}
			case "loop":
				DevCameraPathPlayer.Loop = !DevCameraPathPlayer.Loop;
				console.Print("Path loop: " + (DevCameraPathPlayer.Loop ? "on" : "off"));
				return;
			}
			List<string> list = DevCameraPath.ListNames();
			if (list.Count == 0)
			{
				console.Print("No saved paths. Folder: " + DevCameraPath.Directory);
				return;
			}
			StringBuilder stringBuilder = new StringBuilder($"{list.Count} camera paths:");
			foreach (string item in list)
			{
				stringBuilder.Append("\n  " + item);
			}
			console.Print(stringBuilder.ToString());
		}

		private static void Speed(DevConsole console, string[] args)
		{
			if (!DevReplay.Active)
			{
				console.Print("Replay is not active.");
				return;
			}
			if (args.Length != 0 && float.TryParse(args[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
			{
				DevReplay.Instance.Speed = result;
			}
			console.Print($"replayspeed = {DevReplay.Instance.Speed:0.##}");
		}
	}
}
