using UnityEngine;

namespace Ami.Extension
{
	public static class EventExtension
	{
		public static bool IsDoubleClicking(Rect rect, Event current = null)
		{
			if (current == null)
			{
				current = Event.current;
			}
			if (current.type == EventType.MouseDown && current.button == 0 && current.clickCount == 2)
			{
				return rect.Contains(current.mousePosition);
			}
			return false;
		}

		public static bool IsRightClick(Rect rect, Event current = null)
		{
			if (current == null)
			{
				current = Event.current;
			}
			if (current.type == EventType.MouseDown && current.button == 1)
			{
				return rect.Contains(current.mousePosition);
			}
			return false;
		}
	}
}
