using FishNet.Broadcast;

namespace FishNet.Example.Authenticating
{
	public struct ResponseBroadcast : IBroadcast
	{
		public bool Passed;
	}
}
