using System.Collections.Generic;
using Fusion;

namespace NetworkServices.NetworkEvents
{
	public class OnCustomAuthenticationResponseEvent : NetworkRunnerEvent
	{
		public readonly NetworkRunner Runner;

		public readonly Dictionary<string, object> Data;

		public OnCustomAuthenticationResponseEvent(NetworkRunner runner, Dictionary<string, object> data)
		{
			Runner = runner;
			Data = data;
		}
	}
}
