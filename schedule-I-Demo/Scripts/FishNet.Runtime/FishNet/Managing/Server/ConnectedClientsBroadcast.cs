using System.Collections.Generic;
using FishNet.Broadcast;

namespace FishNet.Managing.Server
{
	public struct ConnectedClientsBroadcast : IBroadcast
	{
		public List<int> Values;
	}
}
