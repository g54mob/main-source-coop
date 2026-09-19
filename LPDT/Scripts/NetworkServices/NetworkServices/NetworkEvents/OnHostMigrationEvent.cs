using Fusion;

namespace NetworkServices.NetworkEvents
{
	public class OnHostMigrationEvent : NetworkRunnerEvent
	{
		public readonly NetworkRunner Runner;

		public readonly HostMigrationToken Token;

		public OnHostMigrationEvent(NetworkRunner runner, HostMigrationToken token)
		{
			Runner = runner;
			Token = token;
		}
	}
}
