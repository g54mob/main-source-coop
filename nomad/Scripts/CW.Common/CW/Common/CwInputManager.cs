using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CW.Common
{
	[HelpURL("https://carloswilkes.com/Documentation/Common#CwInputManager")]
	[AddComponentMenu("Common/CW Input Manager")]
	public class CwInputManager : MonoBehaviour
	{
		public enum AxisGesture
		{
			HorizontalDrag = 0,
			VerticalDrag = 1,
			Twist = 2,
			HorizontalPull = 3,
			VerticalPull = 4
		}

		[Serializable]
		public struct Axis
		{
			public int FingerCount;

			public bool FingerInvert;

			public AxisGesture FingerGesture;

			public float FingerSensitivity;

			public KeyCode KeyNegative;

			public KeyCode KeyPositive;

			public KeyCode KeyNegativeAlt;

			public KeyCode KeyPositiveAlt;

			public float KeySensitivity;

			public Axis(int fCount, bool fInvert, AxisGesture fGesture, float fSensitivty, KeyCode kNegative, KeyCode kPositive, KeyCode kNegativeAlt, KeyCode kPositiveAlt, float kSensitivity)
			{
				FingerCount = fCount;
				FingerInvert = fInvert;
				FingerGesture = fGesture;
				FingerSensitivity = fSensitivty;
				KeyNegative = kNegative;
				KeyPositive = kPositive;
				KeyNegativeAlt = kNegativeAlt;
				KeyPositiveAlt = kPositiveAlt;
				KeySensitivity = kSensitivity;
			}

			public float GetValue(float delta)
			{
				float num = 0f;
				List<Finger> fingers = GetFingers(ignoreStartedOverGui: true, ignoreHover: true);
				float num2 = 1f;
				num -= (CwInput.GetKeyIsHeld(KeyNegative) ? (KeySensitivity * delta) : 0f);
				num += (CwInput.GetKeyIsHeld(KeyPositive) ? (KeySensitivity * delta) : 0f);
				num -= (CwInput.GetKeyIsHeld(KeyNegativeAlt) ? (KeySensitivity * delta) : 0f);
				num += (CwInput.GetKeyIsHeld(KeyPositiveAlt) ? (KeySensitivity * delta) : 0f);
				if (FingerCount > 0 && fingers.Count == FingerCount)
				{
					if (FingerInvert && fingers[0].Index >= 0)
					{
						num2 = -1f;
					}
					switch (FingerGesture)
					{
					case AxisGesture.HorizontalDrag:
						num += GetAverageDeltaScaled(fingers).x * FingerSensitivity * num2;
						break;
					case AxisGesture.VerticalDrag:
						num += GetAverageDeltaScaled(fingers).y * FingerSensitivity * num2;
						break;
					case AxisGesture.Twist:
						num += GetAverageTwistRadians(fingers) * FingerSensitivity;
						break;
					case AxisGesture.HorizontalPull:
						num += GetAveragePullScaled(fingers).x * FingerSensitivity * delta * num2;
						break;
					case AxisGesture.VerticalPull:
						num += GetAveragePullScaled(fingers).y * FingerSensitivity * delta * num2;
						break;
					}
				}
				return num;
			}
		}

		[Serializable]
		public struct Trigger
		{
			public bool UseFinger;

			public bool UseMouse;

			public KeyCode UseKey;

			public Trigger(bool uFinger, bool uMouse, KeyCode uKey)
			{
				UseFinger = uFinger;
				UseMouse = uMouse;
				UseKey = uKey;
			}

			public bool WentDown(Finger finger)
			{
				if (UseFinger && finger.Index >= 0 && finger.Down)
				{
					return true;
				}
				if (UseMouse && finger.Index == -1 && finger.Down)
				{
					return true;
				}
				if (UseKey != KeyCode.None && finger.Index == -1337 && CwInput.GetKeyWentDown(UseKey))
				{
					return true;
				}
				return false;
			}

			public bool IsDown(Finger finger)
			{
				if (UseFinger && finger.Index >= 0 && !finger.Up)
				{
					return true;
				}
				if (UseMouse && finger.Index == -1 && !finger.Up)
				{
					return true;
				}
				if (UseKey != KeyCode.None && finger.Index == -1337 && CwInput.GetKeyIsHeld(UseKey))
				{
					return true;
				}
				return false;
			}

			public bool WentUp(Finger finger, bool useAnyFinger = false)
			{
				if (useAnyFinger && finger.Up)
				{
					return true;
				}
				if (UseFinger && finger.Index >= 0 && finger.Up)
				{
					return true;
				}
				if (UseMouse && finger.Index == -1 && finger.Up)
				{
					return true;
				}
				if (UseKey != KeyCode.None && finger.Index == -1337 && CwInput.GetKeyWentUp(UseKey))
				{
					return true;
				}
				return false;
			}
		}

		public abstract class Link
		{
			public Finger Finger;

			public static T Find<T>(List<T> links, Finger finger) where T : Link, new()
			{
				if (links != null)
				{
					foreach (T link in links)
					{
						if (link.Finger == finger)
						{
							return link;
						}
					}
				}
				return null;
			}

			public static T Create<T>(ref List<T> links, Finger finger) where T : Link, new()
			{
				T val = Find(links, finger);
				if (val == null)
				{
					if (links == null)
					{
						links = new List<T>();
					}
					val = new T
					{
						Finger = finger
					};
					links.Add(val);
				}
				else
				{
					Debug.LogError("Link already exists!");
				}
				return val;
			}

			public static void ClearAll<T>(List<T> links) where T : Link
			{
				if (links == null)
				{
					return;
				}
				foreach (T link in links)
				{
					link.Clear();
				}
				links.Clear();
			}

			public static void ClearAndRemove<T>(List<T> links, T link) where T : Link
			{
				if (link != null)
				{
					link.Clear();
					links?.Remove(link);
				}
			}

			public virtual void Clear()
			{
			}
		}

		public class Finger
		{
			public int Index;

			public float Pressure;

			public bool Down;

			public bool Up;

			public float Age;

			public bool StartedOverGui;

			public Vector2 StartScreenPosition;

			public Vector2 ScreenPosition;

			public Vector2 ScreenPositionOld;

			public Vector2 ScreenPositionOldOld;

			public Vector2 ScreenPositionOldOldOld;

			public float SmoothScreenPositionDelta
			{
				get
				{
					if (!Up)
					{
						return Vector2.Distance(ScreenPositionOldOld, ScreenPositionOld);
					}
					return Vector2.Distance(ScreenPositionOldOld, ScreenPosition);
				}
			}

			public Vector2 GetSmoothScreenPosition(float t)
			{
				if (!Up)
				{
					return Hermite(ScreenPositionOldOldOld, ScreenPositionOldOld, ScreenPositionOld, ScreenPosition, t);
				}
				return Vector2.LerpUnclamped(ScreenPositionOldOld, ScreenPosition, t);
			}
		}

		[SerializeField]
		private LayerMask guiLayers = 32;

		public const int MOUSE_FINGER_INDEX = -1;

		public const int HOVER_FINGER_INDEX = -1337;

		private static List<RaycastResult> tempRaycastResults = new List<RaycastResult>(10);

		private static PointerEventData tempPointerEventData;

		private static EventSystem tempEventSystem;

		private static List<Finger> fingers = new List<Finger>();

		private static List<Finger> filteredFingers = new List<Finger>();

		private static Stack<Finger> pool = new Stack<Finger>();

		public LayerMask GuiLayers
		{
			get
			{
				return guiLayers;
			}
			set
			{
				guiLayers = value;
			}
		}

		public static List<Finger> Fingers => fingers;

		public static float ScaleFactor
		{
			get
			{
				float num = Screen.dpi;
				if (num <= 0f)
				{
					num = 200f;
				}
				return 200f / num;
			}
		}

		public static event Action<Finger> OnFingerDown;

		public static event Action<Finger> OnFingerUpdate;

		public static event Action<Finger> OnFingerUp;

		public static List<Finger> GetFingers(bool ignoreStartedOverGui = false, bool ignoreHover = false)
		{
			filteredFingers.Clear();
			foreach (Finger finger in fingers)
			{
				if ((!ignoreStartedOverGui || !finger.StartedOverGui) && (!ignoreHover || finger.Index != -1337))
				{
					filteredFingers.Add(finger);
				}
			}
			return filteredFingers;
		}

		public static bool PointOverGui(Vector2 screenPosition, int guiLayers = 32)
		{
			return RaycastGui(screenPosition, guiLayers).Count > 0;
		}

		public static List<RaycastResult> RaycastGui(Vector2 screenPosition, int guiLayers = 32)
		{
			tempRaycastResults.Clear();
			EventSystem current = EventSystem.current;
			if (current != null)
			{
				if (current != tempEventSystem)
				{
					tempEventSystem = current;
					if (tempPointerEventData == null)
					{
						tempPointerEventData = new PointerEventData(tempEventSystem);
					}
					else
					{
						tempPointerEventData.Reset();
					}
				}
				tempPointerEventData.position = screenPosition;
				current.RaycastAll(tempPointerEventData, tempRaycastResults);
				if (tempRaycastResults.Count > 0)
				{
					for (int num = tempRaycastResults.Count - 1; num >= 0; num--)
					{
						if (((1 << tempRaycastResults[num].gameObject.layer) & guiLayers) == 0)
						{
							tempRaycastResults.RemoveAt(num);
						}
					}
				}
			}
			return tempRaycastResults;
		}

		public static Vector2 GetAveragePosition(List<Finger> fingers)
		{
			Vector2 zero = Vector2.zero;
			foreach (Finger finger in fingers)
			{
				zero += finger.ScreenPosition;
			}
			if (fingers.Count != 0)
			{
				return zero / fingers.Count;
			}
			return zero;
		}

		public static Vector2 GetAverageOldPosition(List<Finger> fingers)
		{
			Vector2 zero = Vector2.zero;
			foreach (Finger finger in fingers)
			{
				zero += finger.ScreenPositionOld;
			}
			if (fingers.Count != 0)
			{
				return zero / fingers.Count;
			}
			return zero;
		}

		public static Vector2 GetAveragePullScaled(List<Finger> fingers)
		{
			Vector2 zero = Vector2.zero;
			foreach (Finger finger in fingers)
			{
				zero += finger.ScreenPosition - finger.StartScreenPosition;
			}
			if (fingers.Count != 0)
			{
				return zero * ScaleFactor / fingers.Count;
			}
			return zero;
		}

		public static Vector2 GetAverageDeltaScaled(List<Finger> fingers)
		{
			Vector2 zero = Vector2.zero;
			foreach (Finger finger in fingers)
			{
				zero += finger.ScreenPosition - finger.ScreenPositionOld;
			}
			if (fingers.Count != 0)
			{
				return zero * ScaleFactor / fingers.Count;
			}
			return zero;
		}

		public static float GetAverageTwistRadians(List<Finger> fingers)
		{
			float num = 0f;
			Vector2 averagePosition = GetAveragePosition(fingers);
			Vector2 averageOldPosition = GetAverageOldPosition(fingers);
			foreach (Finger finger in fingers)
			{
				num += GetDeltaRadians(finger, averagePosition, averageOldPosition);
			}
			if (fingers.Count != 0)
			{
				return num / (float)fingers.Count;
			}
			return num;
		}

		public static void EnsureThisComponentExists()
		{
			if (Application.isPlaying && CwHelper.FindAnyObjectByType<CwInputManager>() == null)
			{
				new GameObject(typeof(CwInputManager).Name).AddComponent<CwInputManager>();
			}
		}

		protected virtual void Update()
		{
			for (int num = fingers.Count - 1; num >= 0; num--)
			{
				Finger finger = fingers[num];
				if (finger.Up)
				{
					fingers.RemoveAt(num);
					pool.Push(finger);
				}
				else
				{
					finger.Up = true;
				}
			}
			if (CwInput.GetTouchCount() > 0)
			{
				for (int i = 0; i < CwInput.GetTouchCount(); i++)
				{
					CwInput.GetTouch(i, out var id, out var position, out var pressure, out var set);
					AddFinger(id, position, pressure, set);
				}
			}
			else if (CwInput.GetMouseExists())
			{
				bool flag = false;
				bool flag2 = false;
				for (int j = 0; j < 5; j++)
				{
					flag |= CwInput.GetMouseIsHeld(j);
					flag2 |= CwInput.GetMouseWentUp(j);
				}
				AddFinger(-1337, CwInput.GetMousePosition(), 0f, set: true);
				if (flag || flag2)
				{
					AddFinger(-1, CwInput.GetMousePosition(), 1f, flag);
				}
			}
			foreach (Finger finger2 in fingers)
			{
				if (finger2.Down && CwInputManager.OnFingerDown != null)
				{
					CwInputManager.OnFingerDown(finger2);
				}
				if (CwInputManager.OnFingerUpdate != null)
				{
					CwInputManager.OnFingerUpdate(finger2);
				}
				if (finger2.Up && CwInputManager.OnFingerUp != null)
				{
					CwInputManager.OnFingerUp(finger2);
				}
			}
		}

		private Finger FindFinger(int index)
		{
			foreach (Finger finger in fingers)
			{
				if (finger.Index == index)
				{
					return finger;
				}
			}
			return null;
		}

		private void AddFinger(int index, Vector2 screenPosition, float pressure, bool set)
		{
			Finger finger = FindFinger(index);
			if (finger == null)
			{
				finger = ((pool.Count > 0) ? pool.Pop() : new Finger());
				finger.Index = index;
				finger.Down = true;
				finger.Age = 0f;
				finger.StartedOverGui = PointOverGui(screenPosition, guiLayers);
				finger.StartScreenPosition = screenPosition;
				finger.ScreenPositionOld = screenPosition;
				finger.ScreenPositionOldOld = screenPosition;
				finger.ScreenPositionOldOldOld = screenPosition;
				fingers.Add(finger);
			}
			else
			{
				finger.Down = false;
				finger.Age += Time.deltaTime;
				finger.ScreenPositionOldOldOld = finger.ScreenPositionOldOld;
				finger.ScreenPositionOldOld = finger.ScreenPositionOld;
				finger.ScreenPositionOld = finger.ScreenPosition;
			}
			finger.Pressure = pressure;
			finger.ScreenPosition = screenPosition;
			finger.Up = !set;
		}

		private static Vector2 Hermite(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
		{
			float num = t * t;
			float mu = num * t;
			float x = HermiteInterpolate(a.x, b.x, c.x, d.x, t, num, mu);
			float y = HermiteInterpolate(a.y, b.y, c.y, d.y, t, num, mu);
			return new Vector2(x, y);
		}

		private static float HermiteInterpolate(float y0, float y1, float y2, float y3, float mu, float mu2, float mu3)
		{
			float num = (y1 - y0) * 0.5f + (y2 - y1) * 0.5f;
			float num2 = (y2 - y1) * 0.5f + (y3 - y2) * 0.5f;
			float num3 = 2f * mu3 - 3f * mu2 + 1f;
			float num4 = mu3 - 2f * mu2 + mu;
			float num5 = mu3 - mu2;
			float num6 = -2f * mu3 + 3f * mu2;
			return num3 * y1 + num4 * num + num5 * num2 + num6 * y2;
		}

		private static float GetRadians(Vector2 screenPosition, Vector2 referencePoint)
		{
			return Mathf.Atan2(screenPosition.x - referencePoint.x, screenPosition.y - referencePoint.y);
		}

		private static float GetDeltaRadians(Finger finger, Vector2 referencePoint, Vector2 lastReferencePoint)
		{
			float radians = GetRadians(finger.ScreenPositionOld, lastReferencePoint);
			float radians2 = GetRadians(finger.ScreenPosition, referencePoint);
			float num = Mathf.Repeat(radians - radians2, (float)Math.PI * 2f);
			if (num > (float)Math.PI)
			{
				num -= (float)Math.PI * 2f;
			}
			return num;
		}
	}
}
