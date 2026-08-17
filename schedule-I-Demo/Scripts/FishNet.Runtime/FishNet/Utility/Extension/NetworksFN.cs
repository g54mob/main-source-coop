using FishNet.Documenting;
using FishNet.Managing;
using FishNet.Object;

namespace FishNet.Utility.Extension
{
	[APIExclude]
	public static class NetworksFN
	{
		public static bool DoubleLogic(this NetworkObject nob, bool asServer)
		{
			if (!asServer)
			{
				return nob.NetworkManager.IsServer;
			}
			return false;
		}

		public static bool DoubleLogic(this NetworkManager manager, bool asServer)
		{
			if (!asServer)
			{
				return manager.IsServer;
			}
			return false;
		}

		public static bool DoubleLogic(this NetworkBehaviour nb, bool asServer)
		{
			if (!asServer)
			{
				return nb.NetworkManager.IsServer;
			}
			return false;
		}
	}
}
