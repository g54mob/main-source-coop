namespace Mimicraft.Networking
{
	public static class LobbyPhase
	{
		public const string Waiting = "waiting";

		public const string Preparing = "prep";

		public const string Playing = "playing";

		public const string Ending = "end";

		public static string Of(RoundPhase phase)
		{
			return phase switch
			{
				RoundPhase.WaitingForPlayers => "waiting", 
				RoundPhase.Prep => "prep", 
				RoundPhase.Hunt => "playing", 
				RoundPhase.RoundEnd => "end", 
				_ => "waiting", 
			};
		}

		public static bool IsOpen(string id)
		{
			if (!(id == "waiting"))
			{
				return id == "end";
			}
			return true;
		}

		public static bool IsKnown(string id)
		{
			switch (id)
			{
			default:
				return id == "end";
			case "waiting":
			case "prep":
			case "playing":
				return true;
			}
		}

		public static string LabelKey(string id)
		{
			return id switch
			{
				"waiting" => "Lobby.Phase.Waiting", 
				"prep" => "Lobby.Phase.Preparing", 
				"playing" => "Lobby.Phase.Playing", 
				"end" => "Lobby.Phase.Ending", 
				_ => "", 
			};
		}
	}
}
