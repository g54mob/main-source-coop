using UnityEngine;
using UnityEngine.InputSystem;

namespace Mimicraft.Dev
{
	public class DevCameraGrid : MonoBehaviour
	{
		private enum Mode
		{
			Off = 0,
			Thirds = 1,
			Safe = 2,
			Both = 3
		}

		private static DevCameraGrid instance;

		private Mode mode;

		private Texture2D line;

		private static readonly Color ThirdsColour = new Color(1f, 1f, 1f, 0.28f);

		private static readonly Color CentreColour = new Color(1f, 0.82f, 0.3f, 0.55f);

		private static readonly Color SafeColour = new Color(0.45f, 0.8f, 1f, 0.42f);

		private const float ActionSafe = 0.93f;

		private const float TitleSafe = 0.9f;

		private static bool Filming
		{
			get
			{
				if (!DevReplay.Active)
				{
					return DevFreeCamera.Active;
				}
				return true;
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Bootstrap()
		{
			if (!(instance != null))
			{
				GameObject obj = new GameObject("DevCameraGrid");
				Object.DontDestroyOnLoad(obj);
				instance = obj.AddComponent<DevCameraGrid>();
			}
		}

		private void OnDestroy()
		{
			if (instance == this)
			{
				instance = null;
			}
		}

		public static void Hide()
		{
			if (instance != null)
			{
				instance.mode = Mode.Off;
			}
		}

		private void Update()
		{
			if (!Filming)
			{
				mode = Mode.Off;
			}
			else if (Keyboard.current != null && Keyboard.current.f3Key.wasPressedThisFrame && !GameMenuState.IsDevConsoleOpen)
			{
				mode = ((mode != Mode.Both) ? (mode + 1) : Mode.Off);
			}
		}

		private void OnGUI()
		{
			if (mode != Mode.Off)
			{
				EnsureTexture();
				GUI.depth = -700;
				Rect frame = new Rect(0f, 0f, Screen.width, Screen.height);
				if (mode == Mode.Thirds || mode == Mode.Both)
				{
					DrawThirds(frame);
				}
				if (mode == Mode.Safe || mode == Mode.Both)
				{
					DrawSafeAreas(frame);
				}
			}
		}

		private void DrawThirds(Rect frame)
		{
			for (int i = 1; i <= 2; i++)
			{
				Vertical(frame.width * (float)i / 3f, frame, ThirdsColour);
				Horizontal(frame.height * (float)i / 3f, frame, ThirdsColour);
			}
			float num = frame.width * 0.5f;
			float num2 = frame.height * 0.5f;
			GUI.color = CentreColour;
			GUI.DrawTexture(new Rect(num - 14f, num2, 28f, 1f), line);
			GUI.DrawTexture(new Rect(num, num2 - 14f, 1f, 28f), line);
			GUI.color = Color.white;
		}

		private void DrawSafeAreas(Rect frame)
		{
			Rect rect = Delivery(frame);
			if (rect != frame)
			{
				GUI.color = new Color(0f, 0f, 0f, 0.55f);
				GUI.DrawTexture(new Rect(0f, 0f, frame.width, rect.y), line);
				GUI.DrawTexture(new Rect(0f, rect.yMax, frame.width, frame.height - rect.yMax), line);
				GUI.DrawTexture(new Rect(0f, rect.y, rect.x, rect.height), line);
				GUI.DrawTexture(new Rect(rect.xMax, rect.y, frame.width - rect.xMax, rect.height), line);
				GUI.color = Color.white;
			}
			Box(Inset(rect, 0.93f), SafeColour);
			Box(Inset(rect, 0.9f), SafeColour);
		}

		private static Rect Delivery(Rect frame)
		{
			float num = frame.width / Mathf.Max(1f, frame.height);
			if (Mathf.Abs(num - 1.7777778f) < 0.01f)
			{
				return frame;
			}
			if (num > 1.7777778f)
			{
				float num2 = frame.height * 1.7777778f;
				return new Rect((frame.width - num2) * 0.5f, 0f, num2, frame.height);
			}
			float num3 = frame.width / 1.7777778f;
			return new Rect(0f, (frame.height - num3) * 0.5f, frame.width, num3);
		}

		private static Rect Inset(Rect rect, float fraction)
		{
			float num = rect.width * fraction;
			float num2 = rect.height * fraction;
			return new Rect(rect.x + (rect.width - num) * 0.5f, rect.y + (rect.height - num2) * 0.5f, num, num2);
		}

		private void Box(Rect rect, Color colour)
		{
			GUI.color = colour;
			GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, 1f), line);
			GUI.DrawTexture(new Rect(rect.x, rect.yMax - 1f, rect.width, 1f), line);
			GUI.DrawTexture(new Rect(rect.x, rect.y, 1f, rect.height), line);
			GUI.DrawTexture(new Rect(rect.xMax - 1f, rect.y, 1f, rect.height), line);
			GUI.color = Color.white;
		}

		private void Vertical(float x, Rect frame, Color colour)
		{
			GUI.color = colour;
			GUI.DrawTexture(new Rect(x, frame.y, 1f, frame.height), line);
			GUI.color = Color.white;
		}

		private void Horizontal(float y, Rect frame, Color colour)
		{
			GUI.color = colour;
			GUI.DrawTexture(new Rect(frame.x, y, frame.width, 1f), line);
			GUI.color = Color.white;
		}

		private void EnsureTexture()
		{
			if (!(line != null))
			{
				line = new Texture2D(1, 1);
				line.SetPixel(0, 0, Color.white);
				line.Apply();
				line.hideFlags = HideFlags.HideAndDontSave;
			}
		}
	}
}
