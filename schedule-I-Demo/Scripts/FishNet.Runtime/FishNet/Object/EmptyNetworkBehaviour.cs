namespace FishNet.Object
{
	public class EmptyNetworkBehaviour : NetworkBehaviour
	{
		private bool NetworkInitialize___EarlyFishNet_002EObject_002EEmptyNetworkBehaviourFishNet_002ERuntime_002Edll_Excuted;

		private bool NetworkInitialize__LateFishNet_002EObject_002EEmptyNetworkBehaviourFishNet_002ERuntime_002Edll_Excuted;

		public virtual void NetworkInitialize___Early()
		{
			if (!NetworkInitialize___EarlyFishNet_002EObject_002EEmptyNetworkBehaviourFishNet_002ERuntime_002Edll_Excuted)
			{
				NetworkInitialize___EarlyFishNet_002EObject_002EEmptyNetworkBehaviourFishNet_002ERuntime_002Edll_Excuted = true;
			}
		}

		public virtual void NetworkInitialize__Late()
		{
			if (!NetworkInitialize__LateFishNet_002EObject_002EEmptyNetworkBehaviourFishNet_002ERuntime_002Edll_Excuted)
			{
				NetworkInitialize__LateFishNet_002EObject_002EEmptyNetworkBehaviourFishNet_002ERuntime_002Edll_Excuted = true;
			}
		}

		public override void NetworkInitializeIfDisabled()
		{
			NetworkInitialize___Early();
			NetworkInitialize__Late();
		}

		public virtual void Awake()
		{
			NetworkInitialize___Early();
			NetworkInitialize__Late();
		}
	}
}
