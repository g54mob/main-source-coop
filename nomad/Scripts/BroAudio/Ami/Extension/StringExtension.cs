using UnityEngine;

namespace Ami.Extension
{
	public static class StringExtension
	{
		public static string SetColor(this string text, Color color)
		{
			string text2 = ColorUtility.ToHtmlStringRGB(color);
			return "<color=#" + text2 + ">" + text + "</color>";
		}

		public static string SetColor(this string text, string colorCode)
		{
			return "<color=#" + colorCode + ">" + text + "</color>";
		}

		public static string ToWhiteBold(this string text)
		{
			return text.ToBold().SetColor(Color.white);
		}

		public static string ToBold(this string text)
		{
			return "<b>" + text + "</b>";
		}

		public static string ToItalics(this string text)
		{
			return "<i>" + text + "</i>";
		}

		public static string SetSize(this string text, int size)
		{
			return $"<size={size}>{text}</size>";
		}

		public static bool IsEnglishLetter(char word)
		{
			if (word < 'A' || word > 'Z')
			{
				if (word >= 'a')
				{
					return word <= 'z';
				}
				return false;
			}
			return true;
		}

		public static char ToLower(this char word)
		{
			if (word >= 'A' && word <= 'Z')
			{
				return (char)(word + 32);
			}
			if (word >= 'a' && word <= 'z')
			{
				return word;
			}
			return char.ToLower(word);
		}

		public static char ToUpper(this char word)
		{
			if (word >= 'A' && word <= 'Z')
			{
				return word;
			}
			if (word >= 'a' && word <= 'z')
			{
				return (char)(word - 32);
			}
			return char.ToUpper(word);
		}

		public static string TrimStartAndEnd(this string text)
		{
			if (char.IsWhiteSpace(text[0]))
			{
				text = text.TrimStart();
			}
			if (char.IsWhiteSpace(text[text.Length - 1]))
			{
				text = text.TrimEnd();
			}
			return text;
		}
	}
}
