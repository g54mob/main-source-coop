using System;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Networked
{
	[NetworkBehaviourWeaved(1)]
	public class MonkeyPorterRunNetworkObject : NetworkBehaviour
	{
		private INetworkedModelInstanceProvider _networkedModelInstanceProvider;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsOwned", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsOwned;

		[Networked]
		[OnChangedRender("OnIsOwnedChangedRender")]
		[NetworkedWeaved(0, 1)]
		public unsafe bool IsOwned
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterRunNetworkObject.IsOwned. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonkeyPorterRunNetworkObject.IsOwned. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		public event Action<bool> OnNetworkedIsOwnedChanged;

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
			this.OnNetworkedIsOwnedChanged?.Invoke(IsOwned);
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

		public bool TryWriteIsOwned(bool isOwned)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			IsOwned = isOwned;
			return true;
		}

		private void OnIsOwnedChangedRender()
		{
			this.OnNetworkedIsOwnedChanged?.Invoke(IsOwned);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			IsOwned = _IsOwned;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_IsOwned = IsOwned;
		}
	}
}
