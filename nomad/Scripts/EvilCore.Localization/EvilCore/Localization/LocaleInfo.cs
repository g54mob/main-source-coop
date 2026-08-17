namespace EvilCore.Localization
{
	public struct LocaleInfo
	{
		public string Code;

		public string DisplayName;

		public string NativeName;

		public bool IsRTL;

		public LocaleInfo(string code, string displayName, string nativeName, bool isRTL = false)
		{
			Code = code;
			DisplayName = displayName;
			NativeName = nativeName;
			IsRTL = isRTL;
		}
	}
}
