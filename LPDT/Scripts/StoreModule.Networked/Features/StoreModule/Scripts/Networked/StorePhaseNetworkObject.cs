using System;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.StoreModule.Scripts.Networked
{
	[NetworkBehaviourWeaved(1)]
	public class StorePhaseNetworkObject : NetworkBehaviour
	{
		private INetworkedModelInstanceProvider _networkedModelInstanceProvider;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsStoreActive", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsStoreActive;

		[Networked]
		[OnChangedRender("OnIsStoreActiveChangedRender")]
		[NetworkedWeaved(0, 1)]
		public unsafe bool IsStoreActive
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing StorePhaseNetworkObject.IsStoreActive. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing StorePhaseNetworkObject.IsStoreActive. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		public event Action<bool> OnNetworkedIsStoreActiveChanged;

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
			this.OnNetworkedIsStoreActiveChanged?.Invoke(IsStoreActive);
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

		public bool TryWriteIsStoreActive(bool isStoreActive)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			IsStoreActive = isStoreActive;
			return true;
		}

		private void OnIsStoreActiveChangedRender()
		{
			this.OnNetworkedIsStoreActiveChanged?.Invoke(IsStoreActive);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			IsStoreActive = _IsStoreActive;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_IsStoreActive = IsStoreActive;
		}
	}
}
