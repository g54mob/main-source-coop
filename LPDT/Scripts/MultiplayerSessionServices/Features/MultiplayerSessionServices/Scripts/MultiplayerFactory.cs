using Fusion;
using Zenject;

namespace Features.MultiplayerSessionServices.Scripts
{
	public class MultiplayerFactory : IMultiplayerFactory
	{
		private readonly DiContainer _container;

		private readonly MultiplayerSessionConfig _multiplayerSessionConfig;

		public MultiplayerFactory(DiContainer container, MultiplayerSessionConfig multiplayerSessionConfig)
		{
			_container = container;
			_multiplayerSessionConfig = multiplayerSessionConfig;
		}

		public NetworkRunner CreateNetworkRunner()
		{
			return _container.InstantiatePrefab(_multiplayerSessionConfig.NetworkRunnerPrefab).GetComponent<NetworkRunner>();
		}
	}
}
