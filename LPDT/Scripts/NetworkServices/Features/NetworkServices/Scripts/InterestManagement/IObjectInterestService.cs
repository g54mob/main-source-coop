using Fusion;

namespace Features.NetworkServices.Scripts.InterestManagement
{
	public interface IObjectInterestService
	{
		void OverrideAOI(NetworkTRSP networkTRSP, NetworkObject proxyObject);

		void MarkPlayerInterestedInObject(NetworkObject networkObject, PlayerRef player, bool interested);

		void OverrideActiveAOILayer(AOILayer activeAOILayer);
	}
}
