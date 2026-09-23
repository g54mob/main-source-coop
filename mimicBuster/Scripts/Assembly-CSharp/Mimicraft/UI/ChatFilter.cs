using System;
using System.Collections.Generic;
using System.Text;
using Mimicraft.Settings;
using UnityEngine;

namespace Mimicraft.UI
{
	public static class ChatFilter
	{
		private const string ResourcePath = "ChatFilterWords";

		private const char MaskChar = '*';

		private static HashSet<string> exact;

		private static List<string> prefixes;

		private static readonly string[] BuiltIn = new string[21]
		{
			"amk*", "orospu*", "pic*", "siktir*", "sikik*", "sikey*", "yarrak*", "amcik*", "pezevenk*", "gavat*",
			"fuck*", "shit*", "bitch*", "asshole*", "cunt*", "bastard*", "whore*", "slut*", "nigg*", "faggot*",
			"motherfuck*"
		};

		public static string Apply(string line)
		{
			if (string.IsNullOrEmpty(line) || !GameSettings.SafeChat)
			{
				return line;
			}
			try
			{
				return Mask(line);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[ChatFilter] Satir suzulemedi: " + ex.Message);
				return line;
			}
		}

		private static string Mask(string line)
		{
			EnsureWords();
			if (exact.Count == 0 && prefixes.Count == 0)
			{
				return line;
			}
			StringBuilder stringBuilder = new StringBuilder(line.Length);
			int i = 0;
			while (i < line.Length)
			{
				if (line[i] == '<')
				{
					int num = line.IndexOf('>', i);
					if (num >= 0)
					{
						stringBuilder.Append(line, i, num - i + 1);
						i = num + 1;
						continue;
					}
				}
				if (!IsWordChar(line[i]))
				{
					stringBuilder.Append(line[i]);
					i++;
					continue;
				}
				int num2 = i;
				for (; i < line.Length && IsWordChar(line[i]); i++)
				{
				}
				string text = line.Substring(num2, i - num2);
				if (IsBlocked(Normalize(text)))
				{
					stringBuilder.Append('*', text.Length);
				}
				else
				{
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}

		private static bool IsWordChar(char c)
		{
			if (!char.IsLetterOrDigit(c) && c != '\'')
			{
				return c == '’';
			}
			return true;
		}

		private static bool IsBlocked(string normalized)
		{
			if (normalized.Length == 0)
			{
				return false;
			}
			if (exact.Contains(normalized))
			{
				return true;
			}
			foreach (string prefix in prefixes)
			{
				if (normalized.StartsWith(prefix, StringComparison.Ordinal))
				{
					return true;
				}
			}
			return false;
		}

		private static string Normalize(string token)
		{
			StringBuilder stringBuilder = new StringBuilder(token.Length);
			char c = '\0';
			foreach (char c2 in token)
			{
				char c3;
				switch (c2)
				{
				case '!':
				case '1':
				case 'I':
				case 'i':
				case '|':
				case 'İ':
				case 'ı':
					c3 = 'i';
					break;
				case 'Ş':
				case 'ş':
					c3 = 's';
					break;
				case 'Ğ':
				case 'ğ':
					c3 = 'g';
					break;
				case 'Ü':
				case 'ü':
					c3 = 'u';
					break;
				case 'Ö':
				case 'ö':
					c3 = 'o';
					break;
				case 'Ç':
				case 'ç':
					c3 = 'c';
					break;
				case '0':
					c3 = 'o';
					break;
				case '3':
					c3 = 'e';
					break;
				case '4':
				case '@':
					c3 = 'a';
					break;
				case '$':
				case '5':
					c3 = 's';
					break;
				case '7':
					c3 = 't';
					break;
				default:
					c3 = char.ToLowerInvariant(c2);
					break;
				}
				char c4 = c3;
				if (char.IsLetter(c4) && c4 != c)
				{
					stringBuilder.Append(c4);
					c = c4;
				}
			}
			return stringBuilder.ToString();
		}

		private static void EnsureWords()
		{
			if (exact != null)
			{
				return;
			}
			exact = new HashSet<string>(StringComparer.Ordinal);
			prefixes = new List<string>();
			TextAsset textAsset = Resources.Load<TextAsset>("ChatFilterWords");
			string[] array = ((textAsset != null) ? textAsset.text.Split('\n') : BuiltIn);
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				if (text.Length == 0 || text[0] == '#')
				{
					continue;
				}
				bool flag = text.EndsWith("*", StringComparison.Ordinal);
				if (flag)
				{
					text = text.Substring(0, text.Length - 1);
				}
				string text2 = Normalize(text);
				if (text2.Length >= 3)
				{
					if (flag)
					{
						prefixes.Add(text2);
					}
					else
					{
						exact.Add(text2);
					}
				}
			}
		}

		public static void Forget()
		{
			exact = null;
		}
	}
}
