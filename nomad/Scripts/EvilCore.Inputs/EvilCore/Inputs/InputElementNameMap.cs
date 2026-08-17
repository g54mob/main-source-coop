using System;
using System.Collections.Generic;

namespace EvilCore.Inputs
{
	public static class InputElementNameMap
	{
		private static readonly Dictionary<string, string> Friendly = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			{ "Left Button", "Left Click" },
			{ "Right Button", "Right Click" },
			{ "Middle Button", "Middle Click" },
			{ "Mouse Wheel", "Scroll" },
			{ "Mouse Wheel Up", "Scroll Up" },
			{ "Mouse Wheel Down", "Scroll Down" },
			{ "Return", "Enter" },
			{ "Left Control", "Left Ctrl" },
			{ "Right Control", "Right Ctrl" }
		};

		public static string GetFriendly(string elementIdentifierName)
		{
			if (string.IsNullOrEmpty(elementIdentifierName))
			{
				return elementIdentifierName;
			}
			if (!Friendly.TryGetValue(elementIdentifierName, out var value))
			{
				return elementIdentifierName;
			}
			return value;
		}
	}
}
