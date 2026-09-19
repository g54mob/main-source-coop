using System;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.SessionManagementModule.Networked
{
	[NetworkBehaviourWeaved(1)]
	public class ShopVoteNetworkObject : NetworkBehaviour
	{
		private INetworkedModelInstanceProvider _networkedModelInstanceProvider;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("HasVotedToLeave", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _HasVotedToLeave;

		[Networked]
		[OnChangedRender("OnHasVotedToLeaveChangedRender")]
		[NetworkedWeaved(0, 1)]
		public unsafe bool HasVotedToLeave
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ShopVoteNetworkObject.HasVotedToLeave. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ShopVoteNetworkObject.HasVotedToLeave. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		public event Action<bool> OnNetworkedHasVotedToLeaveChanged;

		public event Action OnAuthoritativeTick;

		public event Action OnDespawned;

		[Inject]
		public void InjectDependencies(INetworkedModelInstanceProvider networkedModelInstanceProvider)
		{
			_networkedModelInstanceProvider = networkedModelInstanceProvider;
		}

		public override void Spawned()
		{
			_networkedModelInstanceProvider?.Register(this);
			this.OnNetworkedHasVotedToLeaveChanged?.Invoke(HasVotedToLeave);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_networkedModelInstanceProvider?.Unregister(this);
			this.OnDespawned?.Invoke();
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority)
			{
				this.OnAuthoritativeTick?.Invoke();
			}
		}

		public bool TryWriteHasVotedToLeave(bool hasVotedToLeave)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			HasVotedToLeave = hasVotedToLeave;
			return true;
		}

		private void OnHasVotedToLeaveChangedRender()
		{
			this.OnNetworkedHasVotedToLeaveChanged?.Invoke(HasVotedToLeave);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			HasVotedToLeave = _HasVotedToLeave;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_HasVotedToLeave = HasVotedToLeave;
		}
	}
}
