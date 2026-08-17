using System.Collections.Generic;

namespace EvilCore.Localization
{
	public struct TranslationCoverage
	{
		public string LocaleCode;

		public int TotalKeys;

		public int TranslatedKeys;

		public int MissingKeys;

		public float Percentage;

		public List<string> MissingKeyList;
	}
}
