using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using UnityEngine;

namespace FishNet.Example.ComponentStateSync
{
	public class ComponentSyncStateBehaviour : NetworkBehaviour
	{
		[SyncObject]
		private readonly ComponentStateSync<AMonoScript> _syncScript = new ComponentStateSync<AMonoScript>();

		private bool NetworkInitialize___EarlyFishNet_002EExample_002EComponentStateSync_002EComponentSyncStateBehaviourFishNet_002EDemos_002Edll_Excuted;

		private bool NetworkInitialize__LateFishNet_002EExample_002EComponentStateSync_002EComponentSyncStateBehaviourFishNet_002EDemos_002Edll_Excuted;

		public virtual void Awake()
		{
			NetworkInitialize___Early();
			Awake_UserLogic_FishNet_002EExample_002EComponentStateSync_002EComponentSyncStateBehaviour_FishNet_002EDemos_002Edll();
			NetworkInitialize__Late();
		}

		private void _syncScript_OnChange(AMonoScript component, bool prevState, bool nextState, bool asServer)
		{
			Debug.Log($"Change received on {component.GetType().Name}. New value is {nextState}. Received asServer {asServer}.");
		}

		private void Update()
		{
			if (base.IsServer && Time.frameCount % 200 == 0)
			{
				_syncScript.Enabled = !_syncScript.Enabled;
			}
		}

		public virtual void NetworkInitialize___Early()
		{
			if (!NetworkInitialize___EarlyFishNet_002EExample_002EComponentStateSync_002EComponentSyncStateBehaviourFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize___EarlyFishNet_002EExample_002EComponentStateSync_002EComponentSyncStateBehaviourFishNet_002EDemos_002Edll_Excuted = true;
				_syncScript.InitializeInstance(this, 0u, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, isSyncObject: true);
			}
		}

		public virtual void NetworkInitialize__Late()
		{
			if (!NetworkInitialize__LateFishNet_002EExample_002EComponentStateSync_002EComponentSyncStateBehaviourFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize__LateFishNet_002EExample_002EComponentStateSync_002EComponentSyncStateBehaviourFishNet_002EDemos_002Edll_Excuted = true;
				_syncScript.SetRegistered();
			}
		}

		public override void NetworkInitializeIfDisabled()
		{
			NetworkInitialize___Early();
			NetworkInitialize__Late();
		}

		private void Awake_UserLogic_FishNet_002EExample_002EComponentStateSync_002EComponentSyncStateBehaviour_FishNet_002EDemos_002Edll()
		{
			AMonoScript component = GetComponent<AMonoScript>();
			_syncScript.Initialize(component);
			_syncScript.OnChange += _syncScript_OnChange;
		}
	}
}
