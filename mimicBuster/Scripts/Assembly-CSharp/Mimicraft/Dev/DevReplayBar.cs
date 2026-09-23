using UnityEngine;
using UnityEngine.InputSystem;

namespace Mimicraft.Dev
{
	public class DevReplayBar : MonoBehaviour
	{
		private static DevReplayBar instance;

		private const float Height = 88f;

		private const float Margin = 12f;

		private const float Pad = 14f;

		private const float RowHeight = 26f;

		private const float SliderHeight = 22f;

		private const float WidthFraction = 0.72f;

		private const float MinWidth = 640f;

		private const float SkipSeconds = 5f;

		private bool visible = true;

		private bool speedOpen;

		private GUIStyle labelStyle;

		private GUIStyle timeStyle;

		private GUIStyle buttonStyle;

		private GUIStyle activeButtonStyle;

		private GUIStyle popupItemStyle;

		private Texture2D background;

		private Texture2D popupBackground;

		private bool claimedPointer;

		private Rect lastArea;

		private static readonly float[] Speeds = new float[6] { 0.1f, 0.25f, 0.5f, 1f, 2f, 4f };

		public static bool Visible
		{
			get
			{
				if (instance != null)
				{
					return instance.visible;
				}
				return false;
			}
			set
			{
				if (instance != null)
				{
					instance.visible = value;
				}
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Bootstrap()
		{
			if (!(instance != null))
			{
				GameObject obj = new GameObject("DevReplayBar");
				Object.DontDestroyOnLoad(obj);
				instance = obj.AddComponent<DevReplayBar>();
			}
		}

		private void OnDestroy()
		{
			if (claimedPointer)
			{
				GameMenuState.SetDevPointerOpen(open: false);
			}
			if (instance == this)
			{
				instance = null;
			}
		}

		private void Update()
		{
			if (DevReplay.Active && Keyboard.current != null && Keyboard.current.f2Key.wasPressedThisFrame)
			{
				visible = !visible;
			}
			if (DevReplay.Active && !GameMenuState.IsDevConsoleOpen && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
			{
				DevReplay.Instance.Paused = !DevReplay.Instance.Paused;
			}
			bool flag = visible && DevReplay.Active;
			if (flag != claimedPointer)
			{
				claimedPointer = flag;
				GameMenuState.SetDevPointerOpen(flag);
			}
			if (!flag)
			{
				speedOpen = false;
				return;
			}
			if (Cursor.lockState != CursorLockMode.None)
			{
				Cursor.lockState = CursorLockMode.None;
			}
			if (!Cursor.visible)
			{
				Cursor.visible = true;
			}
		}

		public static bool ContainsPointer(Vector2 pointer)
		{
			if (instance == null || !instance.visible || instance.lastArea.width <= 0f)
			{
				return false;
			}
			Vector2 point = new Vector2(pointer.x, (float)Screen.height - pointer.y);
			if (instance.lastArea.Contains(point))
			{
				return true;
			}
			if (instance.speedOpen)
			{
				return new Rect(instance.lastArea.x, instance.lastArea.y - 180f, instance.lastArea.width, 180f).Contains(point);
			}
			return false;
		}

		private void OnGUI()
		{
			if (visible && DevReplay.Active)
			{
				EnsureStyles();
				DevReplay replay = DevReplay.Instance;
				float num = Mathf.Min((float)Screen.width - 24f, Mathf.Max(640f, (float)Screen.width * 0.72f));
				Rect rect = (lastArea = new Rect(((float)Screen.width - num) * 0.5f, (float)Screen.height - 88f - 12f, num, 88f));
				GUI.depth = -900;
				GUI.DrawTexture(rect, background);
				Rect button = DrawTransport(replay, rect);
				DrawScrubber(replay, rect);
				DrawSpeedPopup(replay, button);
			}
		}

		private Rect DrawTransport(DevReplay replay, Rect area)
		{
			float[] array = new float[7] { 40f, 44f, 52f, 44f, 40f, 12f, 66f };
			float num = 0f;
			float[] array2 = array;
			foreach (float num2 in array2)
			{
				num += num2 + 4f;
			}
			float x = area.x + (area.width - num) * 0.5f;
			float y = area.y + 8.400001f;
			if (Button(ref x, y, array[0], "|◀", "Go to start"))
			{
				SeekAndPause(replay, 0f);
			}
			if (Button(ref x, y, array[1], "◀◀", $"Back {5f:0}s"))
			{
				SeekAndPause(replay, replay.Time - 5f);
			}
			if (Button(ref x, y, array[2], replay.Paused ? "▶" : "❚❚", replay.Paused ? "Play" : "Pause"))
			{
				replay.Paused = !replay.Paused;
			}
			if (Button(ref x, y, array[3], "▶▶", $"Forward {5f:0}s"))
			{
				SeekAndPause(replay, replay.Time + 5f);
			}
			if (Button(ref x, y, array[4], "▶|", "Go to end"))
			{
				SeekAndPause(replay, replay.Duration - 0.01f);
			}
			x += array[5];
			Rect rect = new Rect(x, y, array[6], 26f);
			if (GUI.Button(rect, new GUIContent($"{replay.Speed:0.##}×  ▴", "Playback speed"), speedOpen ? activeButtonStyle : buttonStyle))
			{
				speedOpen = !speedOpen;
			}
			GUI.Label(new Rect(area.x + 14f, y, area.width - 28f, 26f), string.IsNullOrEmpty(GUI.tooltip) ? $"{replay.Name}     {replay.GhostCount} ghosts     F2 hides" : GUI.tooltip, labelStyle);
			return rect;
		}

		private void DrawScrubber(DevReplay replay, Rect area)
		{
			float num = area.y + 8.400001f + 26f + 10f;
			Rect position = new Rect(area.x + 14f, num, 58f, 22f);
			Rect position2 = new Rect(area.xMax - 14f - 58f, num, 58f, 22f);
			Rect position3 = new Rect(position.xMax + 8f, num + 2f, position2.x - position.xMax - 16f, 18f);
			GUI.Label(position, Clock(replay.Time), timeStyle);
			GUI.Label(position2, Clock(replay.Duration), timeStyle);
			float num2 = GUI.HorizontalSlider(position3, replay.Time, 0f, Mathf.Max(0.01f, replay.Duration));
			if (!Mathf.Approximately(num2, replay.Time))
			{
				SeekAndPause(replay, num2);
			}
		}

		private void DrawSpeedPopup(DevReplay replay, Rect button)
		{
			if (!speedOpen)
			{
				return;
			}
			float num = 24f;
			float num2 = (float)Speeds.Length * num + 8f;
			Rect position = new Rect(button.x, button.y - num2 - 4f, button.width, num2);
			GUI.DrawTexture(position, popupBackground);
			for (int i = 0; i < Speeds.Length; i++)
			{
				float num3 = Speeds[Speeds.Length - 1 - i];
				if (GUI.Button(new Rect(position.x + 4f, position.y + 4f + (float)i * num, position.width - 8f, num), style: Mathf.Approximately(replay.Speed, num3) ? activeButtonStyle : popupItemStyle, text: $"{num3:0.##}×"))
				{
					replay.Speed = num3;
					speedOpen = false;
				}
			}
			if (Event.current.type == EventType.MouseDown && !position.Contains(Event.current.mousePosition) && !button.Contains(Event.current.mousePosition))
			{
				speedOpen = false;
			}
		}

		private bool Button(ref float x, float y, float width, string caption, string tooltip)
		{
			Rect position = new Rect(x, y, width, 26f);
			x += width + 4f;
			return GUI.Button(position, new GUIContent(caption, tooltip), buttonStyle);
		}

		private static void SeekAndPause(DevReplay replay, float seconds)
		{
			replay.Seek(Mathf.Clamp(seconds, 0f, Mathf.Max(0f, replay.Duration - 0.01f)));
			replay.Paused = true;
		}

		private static string Clock(float seconds)
		{
			int num = Mathf.Max(0, Mathf.FloorToInt(seconds));
			int num2 = Mathf.Abs(Mathf.FloorToInt(seconds * 10f)) % 10;
			return $"{num / 60:0}:{num % 60:00}.{num2}";
		}

		private void EnsureStyles()
		{
			if (labelStyle == null)
			{
				background = Fill(new Color(0.05f, 0.06f, 0.08f, 0.9f));
				popupBackground = Fill(new Color(0.09f, 0.11f, 0.14f, 0.98f));
				labelStyle = new GUIStyle(GUI.skin.label)
				{
					fontSize = 12,
					alignment = TextAnchor.MiddleRight
				};
				labelStyle.normal.textColor = new Color(0.55f, 0.6f, 0.69f);
				timeStyle = new GUIStyle(GUI.skin.label)
				{
					fontSize = 13,
					alignment = TextAnchor.MiddleCenter,
					fontStyle = FontStyle.Bold
				};
				timeStyle.normal.textColor = new Color(0.88f, 0.91f, 0.96f);
				buttonStyle = new GUIStyle(GUI.skin.button)
				{
					fontSize = 12
				};
				activeButtonStyle = new GUIStyle(buttonStyle)
				{
					fontStyle = FontStyle.Bold
				};
				activeButtonStyle.normal.textColor = new Color(1f, 0.8f, 0.3f);
				activeButtonStyle.hover.textColor = new Color(1f, 0.86f, 0.45f);
				popupItemStyle = new GUIStyle(buttonStyle);
			}
		}

		private static Texture2D Fill(Color colour)
		{
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.SetPixel(0, 0, colour);
			texture2D.Apply();
			texture2D.hideFlags = HideFlags.HideAndDontSave;
			return texture2D;
		}
	}
}
