using Fusion;
using Zenject;

namespace Features.MultiplayerSessionServices.Scripts.NetworkMasterClientTracking
{
	public class NetworkMasterClientTrackerFactory : INetworkMasterClientTrackerFactory
	{
		private readonly MultiplayerModel _multiplayerModel;

		private readonly NetworkMasterClientTrackingConfiguration _networkMasterClientTrackingConfiguration;

		private readonly DiContainer _diContainer;

		public NetworkMasterClientTrackerFactory(MultiplayerModel multiplayerModel, NetworkMasterClientTrackingConfiguration networkMasterClientTrackingConfiguration, DiContainer diContainer)
		{
			_multiplayerModel = multiplayerModel;
			_networkMasterClientTrackingConfiguration = networkMasterClientTrackingConfiguration;
			_diContainer = diContainer;
		}

		public NetworkMasterClientTracker CreateNetworkMasterClientTracker()
		{
			NetworkObject networkObject = _multiplayerModel.NetworkRunner.Spawn(_networkMasterClientTrackingConfiguration.NetworkMasterClientTrackerPrefab);
			_diContainer.InjectGameObject(networkObject.gameObject);
			return networkObject.GetComponent<NetworkMasterClientTracker>();
		}
	}
}
