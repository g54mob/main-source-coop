using Fusion;

namespace Features.NetworkServices.Scripts.InterestManagement
{
	public class ObjectInterestService : IObjectInterestService
	{
		private readonly ActiveAOILayerModel _activeAOILayerModel;

		public ObjectInterestService(ActiveAOILayerModel activeAOILayerModel)
		{
			_activeAOILayerModel = activeAOILayerModel;
		}

		public void OverrideAOI(NetworkTRSP networkTRSP, NetworkObject proxyObject)
		{
			networkTRSP.SetAreaOfInterestOverride(proxyObject);
		}

		public void MarkPlayerInterestedInObject(NetworkObject networkObject, PlayerRef player, bool interested)
		{
			networkObject.SetPlayerAlwaysInterested(player, interested);
		}

		public void OverrideActiveAOILayer(AOILayer activeAOILayer)
		{
			_activeAOILayerModel.ActiveAOILayer = activeAOILayer;
		}
	}
}
