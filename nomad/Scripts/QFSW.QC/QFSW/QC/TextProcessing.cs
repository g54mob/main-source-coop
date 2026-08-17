using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QFSW.QC.Containers;
using QFSW.QC.Pooling;

namespace QFSW.QC
{
	public static class TextProcessing
	{
		public struct ReduceScopeOptions
		{
			public int MaxReductions;

			public bool ReduceIncompleteScope;

			public static readonly ReduceScopeOptions Default = new ReduceScopeOptions
			{
				MaxReductions = -1,
				ReduceIncompleteScope = false
			};
		}

		public struct ScopedSplitOptions
		{
			public int MaxCount;

			public bool AutoReduceScope;

			public static readonly ScopedSplitOptions Default = new ScopedSplitOptions
			{
				MaxCount = -1,
				AutoReduceScope = false
			};
		}

		private enum CharType
		{
			Whitespace = 0,
			Identifier = 1,
			Symbol = 2
		}

		public static readonly char[] DefaultLeftScopers = new char[5] { '<', '[', '(', '{', '"' };

		public static readonly char[] DefaultRightScopers = new char[5] { '>', ']', ')', '}', '"' };

		private static readonly ConcurrentStringBuilderPool _stringBuilderPool = new ConcurrentStringBuilderPool();

		public static int GetMaxScopeDepthAtEnd(this string input)
		{
			return input.GetMaxScopeDepthAtEnd(DefaultLeftScopers, DefaultRightScopers);
		}

		public static int GetMaxScopeDepthAtEnd(this string input, char leftScoper, char rightScoper)
		{
			return input.GetMaxScopeDepthAtEnd(leftScoper.AsArraySingle(), rightScoper.AsArraySingle());
		}

		public static int GetMaxScopeDepthAtEnd<T>(this string input, T leftScopers, T rightScopers) where T : IReadOnlyList<char>
		{
			return input.GetMaxScopeDepthAt(input.Length - 1, leftScopers, rightScopers);
		}

		public static int GetMaxScopeDepthAt(this string input, int cursor)
		{
			return input.GetMaxScopeDepthAt(cursor, DefaultLeftScopers, DefaultRightScopers);
		}

		public static int GetMaxScopeDepthAt(this string input, int cursor, char leftScoper, char rightScoper)
		{
			return input.GetMaxScopeDepthAt(cursor, leftScoper.AsArraySingle(), rightScoper.AsArraySingle());
		}

		public static int GetMaxScopeDepthAt<T>(this string input, int cursor, T leftScopers, T rightScopers) where T : IReadOnlyList<char>
		{
			int[] array = new int[leftScopers.Count];
			for (int i = 0; i <= cursor; i++)
			{
				if (i != 0 && input[i - 1] == '\\')
				{
					continue;
				}
				for (int j = 0; j < leftScopers.Count; j++)
				{
					char c = leftScopers[j];
					char c2 = rightScopers[j];
					if (input[i] == c && c == c2)
					{
						array[j] = 1 - array[j];
					}
					else if (input[i] == c)
					{
						array[j]++;
					}
					else if (input[i] == c2)
					{
						array[j]--;
					}
				}
			}
			return array.Max();
		}

		public static string ReduceScope(this string input)
		{
			return input.ReduceScope(DefaultLeftScopers, DefaultRightScopers, ReduceScopeOptions.Default);
		}

		public static string ReduceScope(this string input, ReduceScopeOptions options)
		{
			return input.ReduceScope(DefaultLeftScopers, DefaultRightScopers, options);
		}

		public static string ReduceScope(this string input, char leftScoper, char rightScoper)
		{
			return input.ReduceScope(leftScoper.AsArraySingle(), rightScoper.AsArraySingle(), ReduceScopeOptions.Default);
		}

		public static string ReduceScope(this string input, char leftScoper, char rightScoper, ReduceScopeOptions options)
		{
			return input.ReduceScope(leftScoper.AsArraySingle(), rightScoper.AsArraySingle(), options);
		}

		public static string ReduceScope<T>(this string input, T leftScopers, T rightScopers) where T : IReadOnlyList<char>
		{
			return input.ReduceScope(leftScopers, rightScopers, ReduceScopeOptions.Default);
		}

		public static string ReduceScope<T>(this string input, T leftScopers, T rightScopers, ReduceScopeOptions options) where T : IReadOnlyList<char>
		{
			if (leftScopers.Count != rightScopers.Count)
			{
				throw new ArgumentException("There must be an equal number of corresponding left and right scopers");
			}
			if (string.IsNullOrWhiteSpace(input))
			{
				return string.Empty;
			}
			if (options.MaxReductions == 0)
			{
				return input;
			}
			int i = 0;
			int num = input.Length - 1;
			int num2 = 0;
			bool flag = true;
			while (flag && (num2 < options.MaxReductions || options.MaxReductions < 0))
			{
				if (i > num)
				{
					return string.Empty;
				}
				flag = false;
				for (; char.IsWhiteSpace(input[i]); i++)
				{
				}
				while (char.IsWhiteSpace(input[num]))
				{
					num--;
				}
				if (IsEscaped(num))
				{
					break;
				}
				for (int j = 0; j < leftScopers.Count; j++)
				{
					char leftScoper = leftScopers[j];
					char c = rightScopers[j];
					bool flag2 = leftScoper == c;
					bool flag3 = input[i] == leftScoper && input[num] == c;
					bool flag4 = false;
					if (!flag3 && options.ReduceIncompleteScope)
					{
						flag3 = input[i] == leftScoper;
						flag4 = flag3;
					}
					if (!flag3)
					{
						continue;
					}
					bool flag5 = false;
					int num3 = 1;
					int k = i + 1;
					int num4 = num - 1;
					if (k <= num4)
					{
						if (flag2)
						{
							for (; SkipSearch(k); k++)
							{
							}
							while (SkipSearch(num4))
							{
								num4--;
							}
						}
						for (int l = k; l <= num4; l++)
						{
							if (IsEscaped(l))
							{
								continue;
							}
							if (flag2)
							{
								if (input[l] == leftScoper)
								{
									flag5 = true;
									break;
								}
								continue;
							}
							if (input[l] == leftScoper)
							{
								num3++;
							}
							else if (input[l] == c)
							{
								num3--;
							}
							if (num3 == 0)
							{
								flag5 = true;
								break;
							}
						}
					}
					if (!flag5)
					{
						if (!flag4)
						{
							num--;
						}
						i++;
						num2++;
						flag = true;
						break;
					}
					bool SkipSearch(int cursor)
					{
						if (IsEscaped(cursor))
						{
							return false;
						}
						if (input[cursor] != leftScoper)
						{
							return char.IsWhiteSpace(input[cursor]);
						}
						return true;
					}
				}
			}
			if (num2 <= 0)
			{
				return input;
			}
			return input.Substring(i, num - i + 1);
			bool IsEscaped(int cursor)
			{
				if (cursor > 0)
				{
					return input[cursor - 1] == '\\';
				}
				return false;
			}
		}

		public static string[] SplitScoped(this string input, char splitChar)
		{
			return input.SplitScoped(splitChar, ScopedSplitOptions.Default);
		}

		public static string[] SplitScoped(this string input, char splitChar, ScopedSplitOptions options)
		{
			return input.SplitScoped(splitChar, DefaultLeftScopers, DefaultRightScopers, options);
		}

		public static string[] SplitScoped(this string input, char splitChar, char leftScoper, char rightScoper)
		{
			return input.SplitScoped(splitChar, leftScoper.AsArraySingle(), rightScoper.AsArraySingle(), ScopedSplitOptions.Default);
		}

		public static string[] SplitScoped(this string input, char splitChar, char leftScoper, char rightScoper, ScopedSplitOptions options)
		{
			return input.SplitScoped(splitChar, leftScoper.AsArraySingle(), rightScoper.AsArraySingle(), options);
		}

		public static string[] SplitScoped<T>(this string input, char splitChar, T leftScopers, T rightScopers) where T : IReadOnlyList<char>
		{
			return input.SplitScoped(splitChar, leftScopers, rightScopers, ScopedSplitOptions.Default);
		}

		public static string[] SplitScoped<T>(this string input, char splitChar, T leftScopers, T rightScopers, ScopedSplitOptions options) where T : IReadOnlyList<char>
		{
			if (options.AutoReduceScope)
			{
				input = input.ReduceScope(leftScopers, rightScopers);
			}
			if (string.IsNullOrWhiteSpace(input))
			{
				return Array.Empty<string>();
			}
			IEnumerable<int> scopedSplitPoints = GetScopedSplitPoints(input, splitChar, leftScopers, rightScopers);
			int[] array = ((options.MaxCount > 0) ? scopedSplitPoints.Take(options.MaxCount - 1).ToArray() : scopedSplitPoints.ToArray());
			if (array.Length == 0)
			{
				return new string[1] { input };
			}
			string[] array2 = new string[array.Length + 1];
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = input.Substring(num, array[i] - num).Trim();
				num = array[i] + 1;
			}
			array2[array.Length] = input.Substring(num).Trim();
			return array2;
		}

		public static IEnumerable<int> GetScopedSplitPoints<T>(string input, char splitChar, T leftScopers, T rightScopers) where T : IReadOnlyList<char>
		{
			return GetScopedSplitPoints(input, splitChar, leftScopers, rightScopers, ScopedSplitOptions.Default);
		}

		public static IEnumerable<int> GetScopedSplitPoints<T>(string input, char splitChar, T leftScopers, T rightScopers, ScopedSplitOptions options) where T : IReadOnlyList<char>
		{
			if (leftScopers.Count != rightScopers.Count)
			{
				throw new ArgumentException("There must be an equal number of corresponding left and right scopers");
			}
			int[] scopes = new int[leftScopers.Count];
			for (int i = 0; i < input.Length; i++)
			{
				if (i == 0 || input[i - 1] != '\\')
				{
					for (int j = 0; j < leftScopers.Count; j++)
					{
						char c = leftScopers[j];
						char c2 = rightScopers[j];
						if (input[i] == c && c == c2)
						{
							scopes[j] = 1 - scopes[j];
						}
						else if (input[i] == c)
						{
							scopes[j]++;
						}
						else if (input[i] == c2)
						{
							scopes[j]--;
						}
					}
				}
				if (input[i] == splitChar && scopes.All((int x) => x == 0))
				{
					yield return i;
				}
			}
		}

		public static bool CanSplitScoped(this string input, char splitChar)
		{
			return input.CanSplitScoped(splitChar, DefaultLeftScopers, DefaultRightScopers);
		}

		public static bool CanSplitScoped(this string input, char splitChar, char leftScoper, char rightScoper)
		{
			return input.CanSplitScoped(splitChar, leftScoper.AsArraySingle(), rightScoper.AsArraySingle());
		}

		public static bool CanSplitScoped<T>(this string input, char splitChar, T leftScopers, T rightScopers) where T : IReadOnlyList<char>
		{
			return GetScopedSplitPoints(input, splitChar, leftScopers, rightScopers).Any();
		}

		public static string SplitFirst(this string input, char splitChar)
		{
			return input.SplitScopedFirst(splitChar, Array.Empty<char>(), Array.Empty<char>());
		}

		public static string SplitScopedFirst(this string input, char splitChar)
		{
			return input.SplitScopedFirst(splitChar, DefaultLeftScopers, DefaultRightScopers);
		}

		public static string SplitScopedFirst(this string input, char splitChar, char leftScoper, char rightScoper)
		{
			return input.SplitScopedFirst(splitChar, leftScoper.AsArraySingle(), rightScoper.AsArraySingle());
		}

		public static string SplitScopedFirst<T>(this string input, char splitChar, T leftScopers, T rightScopers) where T : IReadOnlyList<char>
		{
			using IEnumerator<int> enumerator = GetScopedSplitPoints(input, splitChar, leftScopers, rightScopers).GetEnumerator();
			if (enumerator.MoveNext())
			{
				int current = enumerator.Current;
				return input.Substring(0, current).Trim();
			}
			return input;
		}

		public static string UnescapeText(this string input, char escapeChar)
		{
			return input.UnescapeText(escapeChar.AsArraySingle());
		}

		public static string UnescapeText<T>(this string input, T escapeChars) where T : IReadOnlyCollection<char>
		{
			foreach (char item in escapeChars)
			{
				input = input.Replace($"\\{item}", item.ToString());
			}
			return input;
		}

		public static string ReverseItems(this string input, char splitChar)
		{
			int num = input.Length;
			StringBuilder stringBuilder = _stringBuilderPool.GetStringBuilder(input.Length);
			for (int num2 = input.Length - 1; num2 >= 0; num2--)
			{
				if (input[num2] == splitChar)
				{
					int num3 = num2 + 1;
					if (num3 < input.Length)
					{
						stringBuilder.Append(input, num3, num - num3);
					}
					stringBuilder.Append(splitChar);
					num = num2;
				}
				else if (num2 == 0)
				{
					stringBuilder.Append(input, 0, num);
				}
			}
			return _stringBuilderPool.ReleaseAndToString(stringBuilder);
		}

		private static CharType GetCharType(this char character)
		{
			if (char.IsWhiteSpace(character))
			{
				return CharType.Whitespace;
			}
			if (char.IsLetterOrDigit(character) || character == '-')
			{
				return CharType.Identifier;
			}
			return CharType.Symbol;
		}

		public static string WithoutWord(this string text, int wordEnd)
		{
			if (wordEnd <= 0 || wordEnd > text.Length)
			{
				return text;
			}
			int length = 0;
			if (text[wordEnd - 1].GetCharType() == CharType.Symbol)
			{
				length = wordEnd - 1;
			}
			else
			{
				for (int num = wordEnd - 1; num > 0; num--)
				{
					if (text[num].GetCharType() != text[num - 1].GetCharType())
					{
						length = num;
						break;
					}
				}
			}
			return text.Substring(0, length) + text.Substring(Math.Min(wordEnd, text.Length));
		}
	}
}
