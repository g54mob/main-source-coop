using Mirror;

namespace EvilCore.Networking
{
	public interface IConnectionOwnedCleanup
	{
		void OnOwnerDisconnecting(NetworkConnectionToClient conn);
	}
}
