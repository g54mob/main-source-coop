using Fusion;
using UnityEngine;

namespace Features.GrabModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class NetworkObjectComposite : NetworkBehaviour, IAfterSpawned, IPublicFacingInterface
	{
		[SerializeField]
		private NetworkObject[] _explicitNetworkObjects;

		[SerializeField]
		private bool _useExplicitNetworkObjects;

		[SerializeField]
		private bool _reclaimOwnerlessOnMaster;

		private NetworkObject[] _networkObjects;

		public void AfterSpawned()
		{
			if (_networkObjects == null)
			{
				if (_useExplicitNetworkObjects)
				{
					_networkObjects = _explicitNetworkObjects;
				}
				else
				{
					_networkObjects = base.transform.GetComponentsInChildren<NetworkObject>();
				}
			}
		}

		public override void Render()
		{
			if (!_reclaimOwnerlessOnMaster || _networkObjects == null || base.Runner == null || !base.Runner.IsRunning || !base.Runner.IsSharedModeMasterClient)
			{
				return;
			}
			NetworkObject[] networkObjects = _networkObjects;
			foreach (NetworkObject networkObject in networkObjects)
			{
				if (networkObject != null && networkObject.IsValid && networkObject.StateAuthority == PlayerRef.None)
				{
					networkObject.RequestStateAuthority();
				}
			}
		}

		public void SetNetworkObjects(NetworkObject[] networkObjects)
		{
			_networkObjects = networkObjects;
		}

		public void RequestStateAuthority()
		{
			if (!(base.Runner == null) && base.Runner.IsRunning)
			{
				NetworkObject[] networkObjects = _networkObjects;
				for (int i = 0; i < networkObjects.Length; i++)
				{
					networkObjects[i].RequestStateAuthority();
				}
			}
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
