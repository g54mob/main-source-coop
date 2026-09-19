using Fusion;

namespace NetworkServices.NetworkEvents
{
	public class OnConnectRequestEvent : NetworkRunnerEvent
	{
		public readonly NetworkRunner Runner;

		public readonly NetworkRunnerCallbackArgs.ConnectRequest Request;

		public readonly byte[] Token;

		public OnConnectRequestEvent(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
		{
			Runner = runner;
			Request = request;
			Token = token;
		}
	}
}
