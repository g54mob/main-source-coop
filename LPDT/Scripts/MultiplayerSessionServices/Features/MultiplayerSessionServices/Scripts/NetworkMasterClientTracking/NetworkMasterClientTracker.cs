using Fusion;
using UnityEngine;
using Zenject;

namespace Features.MultiplayerSessionServices.Scripts.NetworkMasterClientTracking
{
	[NetworkBehaviourWeaved(0)]
	public class NetworkMasterClientTracker : NetworkBehaviour
	{
		private MultiplayerModel _multiplayerModel;

		[Inject]
		public void InjectDependencies(MultiplayerModel multiplayerModel)
		{
			_multiplayerModel = multiplayerModel;
		}

		public override void Spawned()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			RegisterTracker();
		}

		private void Start()
		{
			RegisterTracker();
		}

		private void RegisterTracker()
		{
			if (_multiplayerModel != null)
			{
				_multiplayerModel.NetworkMasterClientTracker = this;
			}
		}

		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			if (_multiplayerModel != null)
			{
				_multiplayerModel.NetworkMasterClientTracker = null;
			}
		}

		public bool CheckMasterClient(PlayerRef player)
		{
			return base.Object.StateAuthority == player;
		}

		public PlayerRef GetMasterClient()
		{
			return base.Object.StateAuthority;
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
