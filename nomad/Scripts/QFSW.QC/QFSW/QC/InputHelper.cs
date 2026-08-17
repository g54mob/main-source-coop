using UnityEngine;

namespace QFSW.QC
{
	public static class InputHelper
	{
		private static bool IsKeySupported(KeyCode key)
		{
			return true;
		}

		public static bool GetKey(KeyCode key)
		{
			return Input.GetKey(key);
		}

		public static bool GetKeyDown(KeyCode key)
		{
			return Input.GetKeyDown(key);
		}

		public static bool GetKeyUp(KeyCode key)
		{
			return Input.GetKeyDown(key);
		}

		public static Vector2 GetMousePosition()
		{
			return Input.mousePosition;
		}
	}
}
