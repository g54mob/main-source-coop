using System;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.SessionManagementModule.Networked
{
	[NetworkBehaviourWeaved(1)]
	public class LevelPlayerNetworkObject : NetworkBehaviour
	{
		private INetworkedModelInstanceProvider _networkedModelInstanceProvider;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("OwnerSlot", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _OwnerSlot;

		[Networked]
		[OnChangedRender("OnOwnerSlotChangedRender")]
		[NetworkedWeaved(0, 1)]
		public unsafe int OwnerSlot
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayerNetworkObject.OwnerSlot. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayerNetworkObject.OwnerSlot. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		public event Action<int> OnNetworkedOwnerSlotChanged;

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
			this.OnNetworkedOwnerSlotChanged?.Invoke(OwnerSlot);
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

		public bool TryWriteOwnerSlot(int ownerSlot)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			OwnerSlot = ownerSlot;
			return true;
		}

		private void OnOwnerSlotChangedRender()
		{
			this.OnNetworkedOwnerSlotChanged?.Invoke(OwnerSlot);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			OwnerSlot = _OwnerSlot;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_OwnerSlot = OwnerSlot;
		}
	}
}
