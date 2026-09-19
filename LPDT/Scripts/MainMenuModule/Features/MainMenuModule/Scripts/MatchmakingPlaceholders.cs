namespace Features.MainMenuModule.Scripts
{
	public static class MatchmakingPlaceholders
	{
		public const string Searching = "Searching for teammates…";

		public const string NoGamesFound = "No open games found";

		public static string OrFallback(string localized, string fallback)
		{
			if (!string.IsNullOrEmpty(localized))
			{
				return localized;
			}
			return fallback;
		}
	}
}
