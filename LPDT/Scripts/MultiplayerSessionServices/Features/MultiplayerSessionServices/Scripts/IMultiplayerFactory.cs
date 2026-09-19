using Fusion;

namespace Features.MultiplayerSessionServices.Scripts
{
	public interface IMultiplayerFactory
	{
		NetworkRunner CreateNetworkRunner();
	}
}
