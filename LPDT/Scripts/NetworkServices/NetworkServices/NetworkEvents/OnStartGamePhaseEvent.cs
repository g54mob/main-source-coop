namespace NetworkServices.NetworkEvents
{
	public class OnStartGamePhaseEvent : NetworkRunnerEvent
	{
		public string LevelName { get; }

		public int PlayerCount { get; }

		public OnStartGamePhaseEvent(string levelName = null, int playerCount = 0)
		{
			LevelName = levelName;
			PlayerCount = playerCount;
		}
	}
}
