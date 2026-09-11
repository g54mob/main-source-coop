using System;
using JetBrains.Annotations;

namespace Zorro.Core
{
	public static class StringUtility
	{
		[CanBeNull]
		public static string[] SplitOnFirstOfChar(string str, char c)
		{
			if (!str.Contains("."))
			{
				return null;
			}
			Optionable<int> indexFirstOfChar = GetIndexFirstOfChar(str, c);
			if (indexFirstOfChar.IsNone)
			{
				return null;
			}
			string text = str.Substring(0, indexFirstOfChar.Value);
			string text2 = str.Substring(indexFirstOfChar.Value + 1, str.Length - (indexFirstOfChar.Value + 1));
			return new string[2] { text, text2 };
		}

		public static Optionable<int> GetIndexFirstOfChar(string str, char c)
		{
			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] == c)
				{
					return Optionable<int>.Some(i);
				}
			}
			return Optionable<int>.None;
		}

		public static bool EndsWithOneSpace(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return false;
			}
			if (str.Length == 1)
			{
				return str.EndsWith(' ');
			}
			if (str[str.Length - 1] == ' ')
			{
				return str[str.Length - 2] != ' ';
			}
			return false;
		}

		public static bool MakeSureNoDoublleChar(string input, char c)
		{
			bool flag = false;
			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] == c)
				{
					if (flag)
					{
						return false;
					}
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			return true;
		}

		public static string EnsureSpaceAfterPhrase(string input, string phrase)
		{
			if (!input.Contains(phrase))
			{
				return input;
			}
			int num = input.IndexOf(phrase, StringComparison.Ordinal);
			if (num != -1 && num + phrase.Length < input.Length && input[num + phrase.Length] != ' ')
			{
				input = input.Insert(num + phrase.Length, " ");
			}
			return input;
		}
	}
}
