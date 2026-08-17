using System;

namespace EvilCore.Localization
{
	public struct MissingTranslation
	{
		public string Key;

		public string LocaleCode;

		public string CallerInfo;

		public DateTime Timestamp;
	}
}
