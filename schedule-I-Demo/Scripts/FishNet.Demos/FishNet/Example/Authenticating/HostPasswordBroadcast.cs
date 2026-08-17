using FishNet.Broadcast;

namespace FishNet.Example.Authenticating
{
	public struct HostPasswordBroadcast : IBroadcast
	{
		public string Password;
	}
}
