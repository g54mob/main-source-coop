using System.Collections.Generic;
using Fusion;

namespace NetworkServices.NetworkEvents
{
	public class OnSessionListUpdatedEvent : NetworkRunnerEvent
	{
		public readonly NetworkRunner Runner;

		public readonly List<SessionInfo> SessionList;

		public OnSessionListUpdatedEvent(NetworkRunner runner, List<SessionInfo> sessionList)
		{
			Runner = runner;
			SessionList = sessionList;
		}
	}
}
