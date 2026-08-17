using UnityEngine;

namespace NWH.Common.Input
{
	public class InputUtils
	{
		private static int _warningCount;

		public static bool TryGetButton(string buttonName, KeyCode altKey, bool showWarning = true)
		{
			try
			{
				return UnityEngine.Input.GetButton(buttonName);
			}
			catch
			{
				if (_warningCount < 100 && showWarning)
				{
					Debug.LogWarning(buttonName + " input binding missing, falling back to default. Check Input section in manual for more info.");
					_warningCount++;
				}
				return UnityEngine.Input.GetKey(altKey);
			}
		}

		public static bool TryGetButtonDown(string buttonName, KeyCode altKey, bool showWarning = true)
		{
			try
			{
				return UnityEngine.Input.GetButtonDown(buttonName);
			}
			catch
			{
				if (_warningCount < 100 && showWarning)
				{
					Debug.LogWarning(buttonName + " input binding missing, falling back to default. Check Input section in manual for more info.");
					_warningCount++;
				}
				return UnityEngine.Input.GetKeyDown(altKey);
			}
		}

		public static float TryGetAxis(string axisName, bool showWarning = true)
		{
			try
			{
				return UnityEngine.Input.GetAxis(axisName);
			}
			catch
			{
				if (_warningCount < 100 && showWarning)
				{
					Debug.LogWarning(axisName + " input binding missing. Check Input section in manual for more info.");
					_warningCount++;
				}
			}
			return 0f;
		}

		public static float TryGetAxisRaw(string axisName, bool showWarning = true)
		{
			try
			{
				return UnityEngine.Input.GetAxisRaw(axisName);
			}
			catch
			{
				if (_warningCount < 100 && showWarning)
				{
					Debug.LogWarning(axisName + " input binding missing. Check Input section in manual for more info.");
					_warningCount++;
				}
			}
			return 0f;
		}
	}
}
