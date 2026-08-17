using System.Collections.Generic;

namespace NomadDrive.Features.Plates
{
	public static class PlatePatternDetector
	{
		public const int CodeLength = 7;

		public static void Detect(string code, List<PlatePatternType> results)
		{
			results.Clear();
			if (!string.IsNullOrEmpty(code) && code.Length == 7)
			{
				char c = code[0];
				char num = code[1];
				char c2 = code[2];
				char c3 = code[3];
				char c4 = code[4];
				char c5 = code[5];
				char c6 = code[6];
				bool num2 = num == c2 && c2 == c3;
				bool flag = c4 == c5 && c5 == c6;
				bool flag2 = flag && c == c4;
				if (num2 && flag2)
				{
					results.Add(PlatePatternType.Solid);
				}
				if (num2)
				{
					results.Add(PlatePatternType.AllLettersSame);
				}
				if (flag)
				{
					results.Add(PlatePatternType.AllDigitsSame);
				}
				if (IsSequential(c4, c5, c6))
				{
					results.Add(PlatePatternType.SequentialDigits);
				}
				if (IsLowNumber(c4, c5))
				{
					results.Add(PlatePatternType.LowNumber);
				}
				if (HasExactPair(num, c2, c3) || HasExactPair(c4, c5, c6))
				{
					results.Add(PlatePatternType.RepeatingPair);
				}
			}
		}

		private static bool IsSequential(char a, char b, char c)
		{
			int num = a - 48;
			int num2 = b - 48;
			int num3 = c - 48;
			if (num2 - num != 1 || num3 - num2 != 1)
			{
				if (num - num2 == 1)
				{
					return num2 - num3 == 1;
				}
				return false;
			}
			return true;
		}

		private static bool IsLowNumber(char d0, char d1)
		{
			if (d0 == '0')
			{
				return d1 == '0';
			}
			return false;
		}

		private static bool HasExactPair(char a, char b, char c)
		{
			bool flag = a == b;
			bool flag2 = b == c;
			bool flag3 = a == c;
			if (flag && flag2)
			{
				return false;
			}
			return flag || flag2 || flag3;
		}
	}
}
