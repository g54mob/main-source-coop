using System;
using System.Collections.Generic;
using System.Text;

namespace Mimicraft.Localization
{
	public static class ArabicText
	{
		private const char Lam = 'ل';

		private const char Tatweel = 'ـ';

		private static readonly Dictionary<char, (char Iso, char Fin, char Ini, char Med)> Forms = Build();

		private static Dictionary<char, (char, char, char, char)> Build()
		{
			Dictionary<char, (char, char, char, char)> map = new Dictionary<char, (char, char, char, char)>();
			map['ء'] = ('ﺀ', '\0', '\0', '\0');
			Right(1570, 65153);
			Right(1571, 65155);
			Right(1572, 65157);
			Right(1573, 65159);
			Dual(1574, 65161);
			Right(1575, 65165);
			Dual(1576, 65167);
			Right(1577, 65171);
			Dual(1578, 65173);
			Dual(1579, 65177);
			Dual(1580, 65181);
			Dual(1581, 65185);
			Dual(1582, 65189);
			Right(1583, 65193);
			Right(1584, 65195);
			Right(1585, 65197);
			Right(1586, 65199);
			Dual(1587, 65201);
			Dual(1588, 65205);
			Dual(1589, 65209);
			Dual(1590, 65213);
			Dual(1591, 65217);
			Dual(1592, 65221);
			Dual(1593, 65225);
			Dual(1594, 65229);
			Dual(1601, 65233);
			Dual(1602, 65237);
			Dual(1603, 65241);
			Dual(1604, 65245);
			Dual(1605, 65249);
			Dual(1606, 65253);
			Dual(1607, 65257);
			Right(1608, 65261);
			Right(1609, 65263);
			Dual(1610, 65265);
			Dual(1662, 64342);
			Dual(1670, 64378);
			Right(1688, 64394);
			Dual(1705, 64398);
			Dual(1711, 64402);
			Dual(1740, 64508);
			return map;
			void Dual(int c, int iso)
			{
				map[(char)c] = ((char)iso, (char)(iso + 1), (char)(iso + 2), (char)(iso + 3));
			}
			void Right(int c, int iso)
			{
				map[(char)c] = ((char)iso, (char)(iso + 1), '\0', '\0');
			}
		}

		public static bool IsRtl(char c)
		{
			if ((c < '\u0590' || c > '\u08ff') && (c < 'יִ' || c > '﷿'))
			{
				if (c >= 'ﹰ')
				{
					return c <= '\ufeff';
				}
				return false;
			}
			return true;
		}

		private static bool IsLtr(char c)
		{
			if (!IsRtl(c))
			{
				return char.IsLetterOrDigit(c);
			}
			return false;
		}

		private static bool IsTransparent(char c)
		{
			if (c < '\u064b' || c > '\u065f')
			{
				switch (c)
				{
				default:
					if (c >= '\u06d6')
					{
						return c <= '\u06ed';
					}
					return false;
				case '\u0610':
				case '\u0611':
				case '\u0612':
				case '\u0613':
				case '\u0614':
				case '\u0615':
				case '\u0616':
				case '\u0617':
				case '\u0618':
				case '\u0619':
				case '\u061a':
				case '\u0670':
					break;
				}
			}
			return true;
		}

		public static bool ContainsRtl(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			for (int i = 0; i < text.Length; i++)
			{
				if (IsRtl(text[i]))
				{
					return true;
				}
			}
			return false;
		}

		private static bool JoinsToNext(char c)
		{
			if (c != 'ـ')
			{
				if (Forms.TryGetValue(c, out (char, char, char, char) value))
				{
					return value.Item3 != '\0';
				}
				return false;
			}
			return true;
		}

		private static bool JoinsToPrevious(char c)
		{
			if (c != 'ـ')
			{
				if (Forms.TryGetValue(c, out (char, char, char, char) value))
				{
					return value.Item2 != '\0';
				}
				return false;
			}
			return true;
		}

		private static int Previous(string s, int i)
		{
			for (int num = i - 1; num >= 0; num--)
			{
				if (!IsTransparent(s[num]))
				{
					return num;
				}
			}
			return -1;
		}

		private static int Next(string s, int i)
		{
			for (int j = i + 1; j < s.Length; j++)
			{
				if (!IsTransparent(s[j]))
				{
					return j;
				}
			}
			return -1;
		}

		private static bool TryLamAlef(char alef, out char isolated, out char final)
		{
			switch (alef)
			{
			case 'آ':
				isolated = 'ﻵ';
				final = 'ﻶ';
				return true;
			case 'أ':
				isolated = 'ﻷ';
				final = 'ﻸ';
				return true;
			case 'إ':
				isolated = 'ﻹ';
				final = 'ﻺ';
				return true;
			case 'ا':
				isolated = 'ﻻ';
				final = 'ﻼ';
				return true;
			default:
				isolated = (final = '\0');
				return false;
			}
		}

		public static string Shape(string s)
		{
			StringBuilder stringBuilder = new StringBuilder(s.Length);
			for (int i = 0; i < s.Length; i++)
			{
				char c = s[i];
				if (!Forms.TryGetValue(c, out (char, char, char, char) value))
				{
					stringBuilder.Append(c);
					continue;
				}
				int num = Previous(s, i);
				int num2 = Next(s, i);
				bool flag = num >= 0 && JoinsToNext(s[num]) && value.Item2 != '\0';
				if (c == 'ل' && num2 >= 0 && TryLamAlef(s[num2], out var isolated, out var final))
				{
					stringBuilder.Append(flag ? final : isolated);
					for (int j = i + 1; j < num2; j++)
					{
						stringBuilder.Append(s[j]);
					}
					i = num2;
				}
				else
				{
					bool flag2 = value.Item3 != 0 && num2 >= 0 && JoinsToPrevious(s[num2]);
					char c2 = ((!flag) ? (flag2 ? value.Item3 : value.Item1) : (flag2 ? value.Item4 : value.Item2));
					stringBuilder.Append((c2 != 0) ? c2 : c);
				}
			}
			return stringBuilder.ToString();
		}

		public static string ForRightToLeftLayout(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			char[] chars = Shape(text).ToCharArray();
			ForEachPlainSegment(chars, delegate(int start, int end)
			{
				bool[] array = new bool[end - start];
				int num = start;
				while (num < end)
				{
					if (!IsLtr(chars[num]))
					{
						num++;
					}
					else
					{
						int num2 = num;
						for (int i = num; i < end && !IsRtl(chars[i]); i++)
						{
							if (IsLtr(chars[i]))
							{
								num2 = i;
							}
						}
						Array.Reverse(chars, num, num2 - num + 1);
						for (int j = num; j <= num2; j++)
						{
							array[j - start] = true;
						}
						num = num2 + 1;
					}
				}
				for (int k = start; k < end; k++)
				{
					if (!array[k - start])
					{
						chars[k] = Mirror(chars[k]);
					}
				}
			});
			return new string(chars);
		}

		public static string ForLeftToRightLayout(string text)
		{
			if (!ContainsRtl(text))
			{
				return text;
			}
			char[] chars = Shape(text).ToCharArray();
			ForEachPlainSegment(chars, delegate(int start, int end)
			{
				int num = start;
				while (num < end)
				{
					if (!IsRtl(chars[num]))
					{
						num++;
					}
					else
					{
						int num2 = num;
						for (int i = num; i < end && (!IsLtr(chars[i]) || char.IsDigit(chars[i])); i++)
						{
							if (IsRtl(chars[i]))
							{
								num2 = i;
							}
						}
						Array.Reverse(chars, num, num2 - num + 1);
						for (int j = num; j <= num2; j++)
						{
							chars[j] = Mirror(chars[j]);
						}
						int num3 = num;
						while (num3 <= num2)
						{
							if (!char.IsDigit(chars[num3]))
							{
								num3++;
							}
							else
							{
								int k;
								for (k = num3; k + 1 <= num2 && (char.IsDigit(chars[k + 1]) || chars[k + 1] == '.' || chars[k + 1] == ',' || chars[k + 1] == ':'); k++)
								{
								}
								while (k > num3 && !char.IsDigit(chars[k]))
								{
									k--;
								}
								Array.Reverse(chars, num3, k - num3 + 1);
								num3 = k + 1;
							}
						}
						num = num2 + 1;
					}
				}
			});
			return new string(chars);
		}

		private static char Mirror(char c)
		{
			return c switch
			{
				'(' => ')', 
				')' => '(', 
				'[' => ']', 
				']' => '[', 
				'{' => '}', 
				'}' => '{', 
				'«' => '»', 
				'»' => '«', 
				_ => c, 
			};
		}

		private static void ForEachPlainSegment(char[] chars, Action<int, int> segment)
		{
			int num = 0;
			int num2 = 0;
			while (num2 < chars.Length)
			{
				int close;
				if (chars[num2] == '\n')
				{
					if (num2 > num)
					{
						segment(num, num2);
					}
					num2++;
					num = num2;
				}
				else if (chars[num2] == '<' && TryTagEnd(chars, num2, out close))
				{
					if (num2 > num)
					{
						segment(num, num2);
					}
					num2 = close + 1;
					num = num2;
				}
				else
				{
					num2++;
				}
			}
			if (chars.Length > num)
			{
				segment(num, chars.Length);
			}
		}

		private static bool TryTagEnd(char[] chars, int open, out int close)
		{
			close = -1;
			int num = Math.Min(chars.Length, open + 128);
			for (int i = open + 1; i < num; i++)
			{
				if (chars[i] == '<' || chars[i] == '\n')
				{
					return false;
				}
				if (chars[i] == '>')
				{
					close = i;
					return i > open + 1;
				}
			}
			return false;
		}
	}
}
