using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using UnityEngine;

namespace FishNet.Example.CustomSyncObject
{
	public class StructSyncBehaviour : NetworkBehaviour
	{
		[SyncObject]
		private readonly StructySync _structy = new StructySync();

		private bool NetworkInitialize___EarlyFishNet_002EExample_002ECustomSyncObject_002EStructSyncBehaviourFishNet_002EDemos_002Edll_Excuted;

		private bool NetworkInitialize__LateFishNet_002EExample_002ECustomSyncObject_002EStructSyncBehaviourFishNet_002EDemos_002Edll_Excuted;

		public virtual void Awake()
		{
			NetworkInitialize___Early();
			Awake_UserLogic_FishNet_002EExample_002ECustomSyncObject_002EStructSyncBehaviour_FishNet_002EDemos_002Edll();
			NetworkInitialize__Late();
		}

		private void _structy_OnChange(StructySync.CustomOperation op, Structy oldItem, Structy newItem, bool asServer)
		{
			Debug.Log("Changed " + op.ToString() + ", " + newItem.Age + ", " + asServer);
		}

		private void Update()
		{
			if (base.IsServer && Time.frameCount % 200 == 0)
			{
				_structy.Value.Age++;
				_structy.ValuesChanged();
			}
		}

		public virtual void NetworkInitialize___Early()
		{
			if (!NetworkInitialize___EarlyFishNet_002EExample_002ECustomSyncObject_002EStructSyncBehaviourFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize___EarlyFishNet_002EExample_002ECustomSyncObject_002EStructSyncBehaviourFishNet_002EDemos_002Edll_Excuted = true;
				_structy.InitializeInstance(this, 0u, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, isSyncObject: true);
			}
		}

		public virtual void NetworkInitialize__Late()
		{
			if (!NetworkInitialize__LateFishNet_002EExample_002ECustomSyncObject_002EStructSyncBehaviourFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize__LateFishNet_002EExample_002ECustomSyncObject_002EStructSyncBehaviourFishNet_002EDemos_002Edll_Excuted = true;
				_structy.SetRegistered();
			}
		}

		public override void NetworkInitializeIfDisabled()
		{
			NetworkInitialize___Early();
			NetworkInitialize__Late();
		}

		private void Awake_UserLogic_FishNet_002EExample_002ECustomSyncObject_002EStructSyncBehaviour_FishNet_002EDemos_002Edll()
		{
			_structy.OnChange += _structy_OnChange;
		}
	}
}
