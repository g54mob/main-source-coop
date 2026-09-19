using System;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.StoreModule.Scripts.Networked
{
	[NetworkBehaviourWeaved(1)]
	public class StoreReadyNetworkObject : NetworkBehaviour
	{
		private INetworkedModelInstanceProvider _networkedModelInstanceProvider;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsReady", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsReady;

		[Networked]
		[OnChangedRender("OnIsReadyChangedRender")]
		[NetworkedWeaved(0, 1)]
		public unsafe bool IsReady
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing StoreReadyNetworkObject.IsReady. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing StoreReadyNetworkObject.IsReady. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		public event Action<bool> OnNetworkedIsReadyChanged;

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
			this.OnNetworkedIsReadyChanged?.Invoke(IsReady);
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

		public bool TryWriteIsReady(bool isReady)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			IsReady = isReady;
			return true;
		}

		private void OnIsReadyChangedRender()
		{
			this.OnNetworkedIsReadyChanged?.Invoke(IsReady);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			IsReady = _IsReady;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_IsReady = IsReady;
		}
	}
}
