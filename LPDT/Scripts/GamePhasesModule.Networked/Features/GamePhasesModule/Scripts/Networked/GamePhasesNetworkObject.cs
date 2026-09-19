using System;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.GamePhasesModule.Scripts.Networked
{
	[NetworkBehaviourWeaved(2)]
	public class GamePhasesNetworkObject : NetworkBehaviour
	{
		private INetworkedModelInstanceProvider _networkedModelInstanceProvider;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("NetworkedCurrentPhaseCount", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _NetworkedCurrentPhaseCount;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("NetworkedIsActive", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _NetworkedIsActive;

		[Networked]
		[OnChangedRender("OnNetworkedCurrentPhaseCountChangedRender")]
		[NetworkedWeaved(0, 1)]
		public unsafe int NetworkedCurrentPhaseCount
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing GamePhasesNetworkObject.NetworkedCurrentPhaseCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing GamePhasesNetworkObject.NetworkedCurrentPhaseCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[OnChangedRender("OnNetworkedIsActiveChangedRender")]
		[NetworkedWeaved(1, 1)]
		public unsafe bool NetworkedIsActive
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing GamePhasesNetworkObject.NetworkedIsActive. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 1);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing GamePhasesNetworkObject.NetworkedIsActive. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 1) = new NetworkBool(value);
			}
		}

		public event Action<int> OnNetworkedNetworkedCurrentPhaseCountChanged;

		public event Action<bool> OnNetworkedNetworkedIsActiveChanged;

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
			this.OnNetworkedNetworkedCurrentPhaseCountChanged?.Invoke(NetworkedCurrentPhaseCount);
			this.OnNetworkedNetworkedIsActiveChanged?.Invoke(NetworkedIsActive);
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

		public bool TryWriteNetworkedCurrentPhaseCount(int networkedCurrentPhaseCount)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			NetworkedCurrentPhaseCount = networkedCurrentPhaseCount;
			return true;
		}

		public bool TryWriteNetworkedIsActive(bool networkedIsActive)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			NetworkedIsActive = networkedIsActive;
			return true;
		}

		private void OnNetworkedCurrentPhaseCountChangedRender()
		{
			this.OnNetworkedNetworkedCurrentPhaseCountChanged?.Invoke(NetworkedCurrentPhaseCount);
		}

		private void OnNetworkedIsActiveChangedRender()
		{
			this.OnNetworkedNetworkedIsActiveChanged?.Invoke(NetworkedIsActive);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			NetworkedCurrentPhaseCount = _NetworkedCurrentPhaseCount;
			NetworkedIsActive = _NetworkedIsActive;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_NetworkedCurrentPhaseCount = NetworkedCurrentPhaseCount;
			_NetworkedIsActive = NetworkedIsActive;
		}
	}
}
