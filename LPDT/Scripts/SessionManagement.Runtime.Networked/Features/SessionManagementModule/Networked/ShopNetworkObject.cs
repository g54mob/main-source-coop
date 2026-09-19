using System;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.SessionManagementModule.Networked
{
	[NetworkBehaviourWeaved(1)]
	public class ShopNetworkObject : NetworkBehaviour
	{
		private INetworkedModelInstanceProvider _networkedModelInstanceProvider;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Revision", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Revision;

		[Networked]
		[OnChangedRender("OnRevisionChangedRender")]
		[NetworkedWeaved(0, 1)]
		public unsafe int Revision
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ShopNetworkObject.Revision. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ShopNetworkObject.Revision. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		public event Action<int> OnNetworkedRevisionChanged;

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
			this.OnNetworkedRevisionChanged?.Invoke(Revision);
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

		public bool TryWriteRevision(int revision)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Revision = revision;
			return true;
		}

		private void OnRevisionChangedRender()
		{
			this.OnNetworkedRevisionChanged?.Invoke(Revision);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			Revision = _Revision;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_Revision = Revision;
		}
	}
}
