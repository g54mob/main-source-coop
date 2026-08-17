using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Utils
{
	public static class GUITools
	{
		public static GUIContent[] ToGUIContentArray(string[] items)
		{
			if (items == null)
			{
				return null;
			}
			GUIContent[] array = new GUIContent[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				array[i] = new GUIContent(items[i]);
			}
			return array;
		}

		public static GUIContent[] ToGUIContentArray(IList<string> items)
		{
			if (items == null)
			{
				return null;
			}
			GUIContent[] array = new GUIContent[items.Count];
			for (int i = 0; i < items.Count; i++)
			{
				array[i] = new GUIContent(items[i]);
			}
			return array;
		}
	}
}
