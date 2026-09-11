using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace EasyTextEffects.Editor.MyBoxCopy.Extensions
{
	public static class MyString
	{
		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		public static bool NotNullOrEmpty(this string str)
		{
			return !string.IsNullOrEmpty(str);
		}

		public static string RemoveStart(this string str, string remove)
		{
			int num = str.IndexOf(remove, StringComparison.Ordinal);
			if (num >= 0)
			{
				return str.Remove(num, remove.Length);
			}
			return str;
		}

		public static string RemoveEnd(this string str, string remove)
		{
			if (!str.EndsWith(remove))
			{
				return str;
			}
			return str.Remove(str.LastIndexOf(remove, StringComparison.Ordinal));
		}

		public static string ToCamelCase(this string message)
		{
			message = message.Replace("-", " ").Replace("_", " ");
			message = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(message);
			message = message.Replace(" ", "");
			return message;
		}

		public static string SplitCamelCase(this string camelCaseString)
		{
			if (string.IsNullOrEmpty(camelCaseString))
			{
				return camelCaseString;
			}
			string text = Regex.Replace(Regex.Replace(camelCaseString, "(\\P{Ll})(\\P{Ll}\\p{Ll})", "$1 $2"), "(\\p{Ll})(\\P{Ll})", "$1 $2");
			string text2 = text.Substring(0, 1).ToUpper();
			if (camelCaseString.Length > 1)
			{
				string text3 = text.Substring(1);
				return text2 + text3;
			}
			return text2;
		}

		public static T AsEnum<T>(this string source, bool ignoreCase = true) where T : Enum
		{
			return (T)Enum.Parse(typeof(T), source, ignoreCase);
		}

		public static string ToRoman(this int i)
		{
			if (i > 999)
			{
				return "M" + (i - 1000).ToRoman();
			}
			if (i > 899)
			{
				return "CM" + (i - 900).ToRoman();
			}
			if (i > 499)
			{
				return "D" + (i - 500).ToRoman();
			}
			if (i > 399)
			{
				return "CD" + (i - 400).ToRoman();
			}
			if (i > 99)
			{
				return "C" + (i - 100).ToRoman();
			}
			if (i > 89)
			{
				return "XC" + (i - 90).ToRoman();
			}
			if (i > 49)
			{
				return "L" + (i - 50).ToRoman();
			}
			if (i > 39)
			{
				return "XL" + (i - 40).ToRoman();
			}
			if (i > 9)
			{
				return "X" + (i - 10).ToRoman();
			}
			if (i > 8)
			{
				return "IX" + (i - 9).ToRoman();
			}
			if (i > 4)
			{
				return "V" + (i - 5).ToRoman();
			}
			if (i > 3)
			{
				return "IV" + (i - 4).ToRoman();
			}
			if (i > 0)
			{
				return "I" + (i - 1).ToRoman();
			}
			return "";
		}

		public static string SurroundedWith(this string message, string surround)
		{
			return surround + message + surround;
		}

		public static string SurroundedWith(this string message, string start, string end)
		{
			return start + message + end;
		}

		public static string Colored(this string message, Colors color)
		{
			return $"<color={color}>{message}</color>";
		}

		public static string Colored(this string message, Color color)
		{
			return "<color=" + color.ToHex() + ">" + message + "</color>";
		}

		public static string Colored(this string message, string colorCode)
		{
			return "<color=" + colorCode + ">" + message + "</color>";
		}

		public static string Sized(this string message, int size)
		{
			return $"<size={size}>{message}</size>";
		}

		public static string Underlined(this string message)
		{
			return "<u>" + message + "</u>";
		}

		public static string Bold(this string message)
		{
			return "<b>" + message + "</b>";
		}

		public static string Italics(this string message)
		{
			return "<i>" + message + "</i>";
		}
	}
}
