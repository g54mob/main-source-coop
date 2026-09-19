namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts
{
	public static class GameAnalyticsCustomDimensions
	{
		public static class Dimension01
		{
			public const string DEBUG_BUILD = "debug_build";

			public const string RELEASE_BUILD = "release_build";

			public const string DEFAULT_GROUP = "default_group";

			public const string B_GROUP = "b_group";
		}

		public static class Dimension02
		{
			public const string DEVELOPER = "developer";

			public const string CHEATER = "cheater";
		}

		public static class Dimension03
		{
			public const string NEVER_VISITED_INTERACTIVE_MINE = "never_visited_interactive_mine";

			public const string VISITED_INTERACTIVE_MINE = "visited_interactive_mine";

			public const string FINISHED_INTERACTIVE_MINE = "finished_interactive_mine";
		}
	}
}
