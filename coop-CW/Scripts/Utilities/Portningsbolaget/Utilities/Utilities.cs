using System;
using System.Globalization;
using UnityEngine;

namespace Portningsbolaget.Utilities
{
	public static class Utilities
	{
		private static readonly string[] REGIONS = new string[15]
		{
			"us", "usw", "ussc", "eu", "au", "za", "asia", "cae", "in", "jp",
			"sa", "kr", "tr", "ru", "rue"
		};

		private static readonly int NUMBERS_MIN = 48;

		private static readonly int NUMBERS_MAX = 58;

		private static readonly int NUMBER_THRESHOLD = 75;

		private static readonly int UPPERCASE_MIN = 65;

		private static readonly int UPPERCASE_MAX = 91;

		private static readonly int LOWERCASE_MIN = 97;

		private static readonly int LOWERCASE_MAX = 123;

		private static readonly CultureInfo CULTURE = new CultureInfo("en-US");

		public static char RegionToCode(string region)
		{
			for (int i = 0; i < REGIONS.Length; i++)
			{
				if (REGIONS[i] == region)
				{
					return (char)(65 + i);
				}
			}
			return '-';
		}

		public static string CodeToRegion(char code)
		{
			int value = code - 65;
			value = Mathf.Clamp(value, 0, REGIONS.Length - 1);
			return REGIONS[value];
		}

		public static char ModStateToCode(bool usingMods)
		{
			int uPPERCASE_MIN = UPPERCASE_MIN;
			int uPPERCASE_MAX = UPPERCASE_MAX;
			int num = Mathf.RoundToInt((float)uPPERCASE_MIN + (float)(uPPERCASE_MAX - uPPERCASE_MIN) / 2f);
			if (!usingMods)
			{
				return Convert.ToChar(UnityEngine.Random.Range(num, uPPERCASE_MAX), CULTURE);
			}
			return Convert.ToChar(UnityEngine.Random.Range(uPPERCASE_MIN, num), CULTURE);
		}

		public static bool CodeToModState(char code)
		{
			int uPPERCASE_MIN = UPPERCASE_MIN;
			int uPPERCASE_MAX = UPPERCASE_MAX;
			int num = Mathf.RoundToInt((float)uPPERCASE_MIN + (float)(uPPERCASE_MAX - uPPERCASE_MIN) / 2f);
			return Convert.ToInt32(code, CULTURE) < num;
		}

		public static string CreateRandomName(string region, int length = 5, bool upperCase = true)
		{
			string text = "";
			int num = 0;
			for (int i = 0; i < length; i++)
			{
				num = ((UnityEngine.Random.Range(0, 100) <= NUMBER_THRESHOLD) ? (upperCase ? UnityEngine.Random.Range(UPPERCASE_MIN, UPPERCASE_MAX) : UnityEngine.Random.Range(LOWERCASE_MIN, LOWERCASE_MAX)) : UnityEngine.Random.Range(NUMBERS_MIN, NUMBERS_MAX));
				text += (char)num;
			}
			char c = RegionToCode(region);
			return $"{c}{text}";
		}

		public static string CreateRandomName(string region, bool usingMods, int length = 4, bool upperCase = true)
		{
			string text = "";
			int num = 0;
			for (int i = 0; i < length; i++)
			{
				num = ((UnityEngine.Random.Range(0, 100) <= NUMBER_THRESHOLD) ? (upperCase ? UnityEngine.Random.Range(UPPERCASE_MIN, UPPERCASE_MAX) : UnityEngine.Random.Range(LOWERCASE_MIN, LOWERCASE_MAX)) : UnityEngine.Random.Range(NUMBERS_MIN, NUMBERS_MAX));
				text += (char)num;
			}
			char c = RegionToCode(region);
			char c2 = ModStateToCode(usingMods);
			return $"{c}{c2}{text}";
		}
	}
}
