using System;
using System.Collections.Generic;
using Mimicraft.Settings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mimicraft.Dev
{
	public class DevConsole : MonoBehaviour
	{
		private const int MaxSuggestions = 10;

		private const int MaxLines = 256;

		private const int MaxHistory = 64;

		private const float ConsoleHeightFraction = 0.45f;

		private const float LineHeight = 18f;

		private const float Padding = 8f;

		private static DevConsole instance;

		private readonly List<string> lines = new List<string>();

		private readonly List<string> history = new List<string>();

		private readonly List<DevCommand> suggestions = new List<DevCommand>();

		private string input = "";

		private int suggestionIndex = -1;

		private int historyIndex = -1;

		private Vector2 scroll;

		private bool focusRequested;

		private bool open;

		private string suggestionsFor;

		private GUIStyle lineStyle;

		private GUIStyle inputStyle;

		private GUIStyle suggestionStyle;

		private GUIStyle selectedSuggestionStyle;

		private Texture2D background;

		private Texture2D selectionBackground;

		private bool showFps;

		private float smoothedDeltaTime;

		public bool MirrorToUnityLog { get; set; } = Application.isEditor;

		public bool ShowFps
		{
			get
			{
				return showFps;
			}
			set
			{
				showFps = value;
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Bootstrap()
		{
			if (!(instance != null))
			{
				GameObject obj = new GameObject("DevConsole");
				UnityEngine.Object.DontDestroyOnLoad(obj);
				instance = obj.AddComponent<DevConsole>();
				DevBuiltinCommands.RegisterAll(instance);
				DevTrailerCommands.RegisterAll(instance);
				DevRecordCommands.RegisterAll(instance);
				instance.Print($"Developer console ready - {DevCommandRegistry.Count} commands. " + "Type `help`, or leave the field empty and browse with the arrow keys.");
			}
		}

		public static void Log(string text)
		{
			instance?.Print(text);
		}

		public void Print(string text)
		{
			if (MirrorToUnityLog)
			{
				Debug.Log("[Console] " + text);
			}
			string[] array = (text ?? "").Split('\n');
			foreach (string item in array)
			{
				lines.Add(item);
			}
			if (lines.Count > 256)
			{
				lines.RemoveRange(0, lines.Count - 256);
			}
			scroll.y = float.MaxValue;
		}

		public void Clear()
		{
			lines.Clear();
			scroll = Vector2.zero;
		}

		private void OnDestroy()
		{
			if (instance == this)
			{
				instance = null;
			}
			if (open)
			{
				GameMenuState.SetDevConsoleOpen(open: false);
			}
		}

		private void Update()
		{
			smoothedDeltaTime += (Time.unscaledDeltaTime - smoothedDeltaTime) * 0.1f;
			if (open && !GameSettings.DeveloperConsole)
			{
				Toggle();
			}
			if (GameSettings.DeveloperConsole && Keyboard.current != null && Keyboard.current.backquoteKey.wasPressedThisFrame)
			{
				Toggle();
			}
			if (open)
			{
				if (Cursor.lockState != CursorLockMode.None)
				{
					Cursor.lockState = CursorLockMode.None;
				}
				if (!Cursor.visible)
				{
					Cursor.visible = true;
				}
			}
		}

		public void Close()
		{
			if (open)
			{
				open = false;
				GameMenuState.SetDevConsoleOpen(open: false);
			}
		}

		private void Toggle()
		{
			open = !open;
			GameMenuState.SetDevConsoleOpen(open);
			if (open)
			{
				input = "";
				suggestionIndex = -1;
				historyIndex = -1;
				focusRequested = true;
			}
		}

		private void OnGUI()
		{
			EnsureStyles();
			if (showFps)
			{
				DrawFps();
			}
			if (open)
			{
				HandleKeys();
				float num = (float)Screen.height * 0.45f;
				Rect position = new Rect(0f, 0f, Screen.width, num);
				GUI.depth = -1000;
				GUI.DrawTexture(position, background);
				GUILayout.BeginArea(new Rect(8f, 8f, (float)Screen.width - 16f, num - 16f));
				DrawOutput(num);
				DrawInput();
				DrawSuggestions();
				GUILayout.EndArea();
			}
		}

		private void DrawOutput(float height)
		{
			float num = Mathf.Min(suggestions.Count, 10);
			float num2 = 18f * (num + 2f) + 16f;
			scroll = GUILayout.BeginScrollView(scroll, GUILayout.Height(Mathf.Max(18f, height - num2)));
			foreach (string line in lines)
			{
				GUILayout.Label(line, lineStyle);
			}
			GUILayout.EndScrollView();
		}

		private void DrawInput()
		{
			GUI.SetNextControlName("DevConsoleInput");
			string text = GUILayout.TextField(input, inputStyle, GUILayout.Height(24f));
			if (text != input)
			{
				input = text;
				suggestionIndex = -1;
				historyIndex = -1;
			}
			RebuildSuggestions();
			if (focusRequested)
			{
				focusRequested = false;
				GUI.FocusControl("DevConsoleInput");
			}
		}

		private void DrawSuggestions()
		{
			for (int i = 0; i < suggestions.Count; i++)
			{
				DevCommand devCommand = suggestions[i];
				bool flag = i == suggestionIndex;
				string text = (string.IsNullOrEmpty(devCommand.Usage) ? devCommand.Name : (devCommand.Name + "  " + devCommand.Usage));
				if (devCommand.Cheat)
				{
					text += "  [cheat]";
				}
				Rect rect = GUILayoutUtility.GetRect(new GUIContent(text), suggestionStyle, GUILayout.Height(18f));
				if (flag)
				{
					GUI.DrawTexture(rect, selectionBackground);
				}
				GUI.Label(rect, text, flag ? selectedSuggestionStyle : suggestionStyle);
			}
		}

		private void DrawFps()
		{
			float num = ((smoothedDeltaTime > 0f) ? (1f / smoothedDeltaTime) : 0f);
			Rect position = new Rect((float)Screen.width - 130f, 4f, 126f, 20f);
			GUI.DrawTexture(position, background);
			GUI.Label(position, $" {num:0.} fps   {smoothedDeltaTime * 1000f:0.0} ms", lineStyle);
		}

		private void RebuildSuggestions()
		{
			string text = (input.Contains(" ") ? null : input);
			if (text == null)
			{
				suggestions.Clear();
				suggestionsFor = null;
			}
			else if (!(suggestionsFor == text))
			{
				suggestionsFor = text;
				suggestions.Clear();
				suggestions.AddRange(DevCommandRegistry.Match(text, 10));
				suggestionIndex = Mathf.Min(suggestionIndex, suggestions.Count - 1);
			}
		}

		private void HandleKeys()
		{
			Event current = Event.current;
			if (current.type != EventType.KeyDown)
			{
				return;
			}
			switch (current.keyCode)
			{
			case KeyCode.BackQuote:
				current.Use();
				break;
			case KeyCode.Escape:
				GameMenuState.ConsumeEscape();
				Toggle();
				current.Use();
				break;
			case KeyCode.UpArrow:
				Step(-1);
				current.Use();
				break;
			case KeyCode.DownArrow:
				Step(1);
				current.Use();
				break;
			case KeyCode.Tab:
				Complete();
				current.Use();
				break;
			case KeyCode.Return:
			case KeyCode.KeypadEnter:
				if (!Complete())
				{
					Submit();
				}
				current.Use();
				break;
			}
		}

		private void Step(int direction)
		{
			if (suggestions.Count > 0)
			{
				suggestionIndex = Mathf.Clamp(suggestionIndex + direction, -1, suggestions.Count - 1);
			}
			else if (history.Count != 0)
			{
				historyIndex = Mathf.Clamp(historyIndex - direction, -1, history.Count - 1);
				input = ((historyIndex < 0) ? "" : history[history.Count - 1 - historyIndex]);
			}
		}

		private bool Complete()
		{
			if (suggestionIndex < 0 || suggestionIndex >= suggestions.Count)
			{
				return false;
			}
			string b = suggestions[suggestionIndex].Name;
			if (string.Equals(input, b, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			input = b;
			suggestionIndex = -1;
			focusRequested = true;
			return true;
		}

		private void Submit()
		{
			string text = input.Trim();
			input = "";
			suggestionIndex = -1;
			historyIndex = -1;
			focusRequested = true;
			if (text.Length != 0)
			{
				if (history.Count == 0 || history[history.Count - 1] != text)
				{
					history.Add(text);
				}
				if (history.Count > 64)
				{
					history.RemoveAt(0);
				}
				Print("> " + text);
				Execute(text);
			}
		}

		private void Execute(string line)
		{
			string[] array = line.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length == 0)
			{
				return;
			}
			if (!DevCommandRegistry.TryGet(array[0], out var command))
			{
				Print("Unknown command: '" + array[0] + "'. `help` lists them.");
				return;
			}
			string[] array2 = new string[array.Length - 1];
			Array.Copy(array, 1, array2, 0, array2.Length);
			if (command.Cheat && !DevCheats.Enabled)
			{
				Print("'" + command.Name + "' is a cheat command and sv_cheats is 0. " + DevCheats.Describe());
				return;
			}
			try
			{
				command.Run(array2);
			}
			catch (Exception ex)
			{
				Print("'" + command.Name + "' failed: " + ex.Message);
				Debug.LogException(ex);
			}
		}

		private void EnsureStyles()
		{
			if (lineStyle == null)
			{
				background = SolidTexture(new Color(0.05f, 0.06f, 0.08f, 0.92f));
				selectionBackground = SolidTexture(new Color(0.2f, 0.42f, 0.7f, 0.85f));
				lineStyle = new GUIStyle(GUI.skin.label)
				{
					font = (Font.CreateDynamicFontFromOSFont("Consolas", 13) ?? GUI.skin.label.font),
					fontSize = 13,
					wordWrap = true,
					richText = false
				};
				lineStyle.normal.textColor = new Color(0.85f, 0.88f, 0.92f);
				inputStyle = new GUIStyle(GUI.skin.textField)
				{
					fontSize = 14
				};
				suggestionStyle = new GUIStyle(lineStyle)
				{
					wordWrap = false
				};
				suggestionStyle.normal.textColor = new Color(0.62f, 0.66f, 0.72f);
				selectedSuggestionStyle = new GUIStyle(suggestionStyle);
				selectedSuggestionStyle.normal.textColor = Color.white;
			}
		}

		private static Texture2D SolidTexture(Color color)
		{
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.SetPixel(0, 0, color);
			texture2D.Apply();
			texture2D.hideFlags = HideFlags.HideAndDontSave;
			return texture2D;
		}
	}
}
