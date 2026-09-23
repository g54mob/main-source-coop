using System;
using System.Collections.Generic;
using System.Text;

namespace Mimicraft.Gameplay
{
	public static class GuessMatcher
	{
		public static bool Matches(string guess, string word)
		{
			if (string.IsNullOrWhiteSpace(guess) || string.IsNullOrWhiteSpace(word))
			{
				return false;
			}
			string text = Normalize(guess);
			if (text.Length > 0)
			{
				return text == Normalize(word);
			}
			return false;
		}

		public static string Normalize(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder(text.Length);
			for (int i = 0; i < text.Length; i++)
			{
				char c = Fold(text[i]);
				if (c != 0)
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		private static char Fold(char c)
		{
			switch (c)
			{
			case 'I':
			case 'i':
			case 'İ':
			case 'ı':
				return 'i';
			case 'Ğ':
			case 'ğ':
				return 'g';
			case 'Ü':
			case 'ü':
				return 'u';
			case 'Ş':
			case 'ş':
				return 's';
			case 'Ö':
			case 'ö':
				return 'o';
			case 'Ç':
			case 'ç':
				return 'c';
			default:
				switch (c)
				{
				case 'Ά':
				case 'ά':
					return 'α';
				case 'Έ':
				case 'έ':
					return 'ε';
				case 'Ή':
				case 'ή':
					return 'η';
				case 'Ί':
				case 'ΐ':
				case 'Ϊ':
				case 'ί':
				case 'ϊ':
					return 'ι';
				case 'Ό':
				case 'ό':
					return 'ο';
				case 'Ύ':
				case 'Ϋ':
				case 'ΰ':
				case 'ϋ':
				case 'ύ':
					return 'υ';
				case 'Ώ':
				case 'ώ':
					return 'ω';
				case 'ς':
					return 'σ';
				case 'Ё':
				case 'ё':
					return 'е';
				default:
					switch (c)
					{
					case 'آ':
					case 'أ':
					case 'إ':
					case 'ٱ':
						return 'ا';
					case 'ة':
						return 'ه';
					case 'ى':
						return 'ي';
					default:
						if (c < '\u064b' || c > '\u065f')
						{
							switch (c)
							{
							case 'ـ':
							case '\u0670':
								break;
							case 'a':
							case 'b':
							case 'c':
							case 'd':
							case 'e':
							case 'f':
							case 'g':
							case 'h':
							case 'i':
							case 'j':
							case 'k':
							case 'l':
							case 'm':
							case 'n':
							case 'o':
							case 'p':
							case 'q':
							case 'r':
							case 's':
							case 't':
							case 'u':
							case 'v':
							case 'w':
							case 'x':
							case 'y':
							case 'z':
								return c;
							default:
								if (c >= 'A' && c <= 'Z')
								{
									return (char)(c + 32);
								}
								if (c >= '0' && c <= '9')
								{
									return c;
								}
								if (c > '\u007f' && c < 'ɐ')
								{
									string text = c.ToString().Normalize(NormalizationForm.FormD);
									if (text.Length > 0 && text[0] < '\u0080' && char.IsLetter(text[0]))
									{
										return char.ToLowerInvariant(text[0]);
									}
								}
								if (char.IsLetterOrDigit(c))
								{
									return char.ToLowerInvariant(c);
								}
								return '\0';
							}
						}
						return '\0';
					}
				}
			}
		}

		public static bool IsClose(string guess, string word)
		{
			string text = Normalize(guess);
			string text2 = Normalize(word);
			if (text.Length == 0 || text2.Length == 0 || text == text2)
			{
				return false;
			}
			int num = ((text2.Length < 7) ? 1 : 2);
			if (Math.Abs(text.Length - text2.Length) > num)
			{
				return false;
			}
			return Distance(text, text2, num) <= num;
		}

		private static int Distance(string a, string b, int limit)
		{
			int[] array = new int[b.Length + 1];
			int[] array2 = new int[b.Length + 1];
			for (int i = 0; i <= b.Length; i++)
			{
				array[i] = i;
			}
			for (int j = 1; j <= a.Length; j++)
			{
				array2[0] = j;
				int num = array2[0];
				for (int k = 1; k <= b.Length; k++)
				{
					int num2 = ((a[j - 1] != b[k - 1]) ? 1 : 0);
					array2[k] = Math.Min(Math.Min(array2[k - 1] + 1, array[k] + 1), array[k - 1] + num2);
					if (array2[k] < num)
					{
						num = array2[k];
					}
				}
				if (num > limit)
				{
					return limit + 1;
				}
				int[] array3 = array2;
				int[] array4 = array;
				array = array3;
				array2 = array4;
			}
			return array[b.Length];
		}

		public static string Mask(string word)
		{
			return Mask(word, null);
		}

		public static string Mask(string word, ICollection<int> revealed)
		{
			if (string.IsNullOrEmpty(word))
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder(word.Length * 2);
			for (int i = 0; i < word.Length; i++)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(' ');
				}
				char c = word[i];
				bool flag = c == ' ' || (revealed?.Contains(i) ?? false);
				stringBuilder.Append(flag ? c : '_');
			}
			return stringBuilder.ToString();
		}

		public static List<int> LetterIndices(string word)
		{
			List<int> list = new List<int>();
			if (string.IsNullOrEmpty(word))
			{
				return list;
			}
			for (int i = 0; i < word.Length; i++)
			{
				if (word[i] != ' ')
				{
					list.Add(i);
				}
			}
			return list;
		}
	}
}
