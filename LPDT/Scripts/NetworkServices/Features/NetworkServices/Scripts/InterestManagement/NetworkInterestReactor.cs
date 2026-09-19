using Fusion;

namespace Features.NetworkServices.Scripts.InterestManagement
{
	[NetworkBehaviourWeaved(0)]
	public class NetworkInterestReactor : NetworkBehaviour, IInterestEnter, IPublicFacingInterface, IInterestExit
	{
		public void InterestEnter(PlayerRef player)
		{
		}

		public void InterestExit(PlayerRef player)
		{
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
