namespace NetworkServices.NetworkEvents
{
	public class OnSuccessfullyStartGameEvent : NetworkRunnerEvent
	{
		public bool StartedAsHost { get; }

		public string SessionName { get; }

		public string SessionRegion { get; }

		public OnSuccessfullyStartGameEvent(bool startedAsHost, string sessionName, string sessionRegion)
		{
			StartedAsHost = startedAsHost;
			SessionName = sessionName;
			SessionRegion = sessionRegion;
		}
	}
}
