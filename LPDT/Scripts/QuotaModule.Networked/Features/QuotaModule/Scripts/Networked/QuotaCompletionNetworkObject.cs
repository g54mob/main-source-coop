using System;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.QuotaModule.Scripts.Networked
{
	[NetworkBehaviourWeaved(2)]
	public class QuotaCompletionNetworkObject : NetworkBehaviour
	{
		private INetworkedModelInstanceProvider _networkedModelInstanceProvider;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsQuotaCompleted", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsQuotaCompleted;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsBellActivated", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsBellActivated;

		[Networked]
		[OnChangedRender("OnIsQuotaCompletedChangedRender")]
		[NetworkedWeaved(0, 1)]
		public unsafe bool IsQuotaCompleted
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing QuotaCompletionNetworkObject.IsQuotaCompleted. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing QuotaCompletionNetworkObject.IsQuotaCompleted. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		[Networked]
		[OnChangedRender("OnIsBellActivatedChangedRender")]
		[NetworkedWeaved(1, 1)]
		public unsafe bool IsBellActivated
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing QuotaCompletionNetworkObject.IsBellActivated. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 1);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing QuotaCompletionNetworkObject.IsBellActivated. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 1) = new NetworkBool(value);
			}
		}

		public event Action<bool> OnNetworkedIsQuotaCompletedChanged;

		public event Action<bool> OnNetworkedIsBellActivatedChanged;

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
			this.OnNetworkedIsQuotaCompletedChanged?.Invoke(IsQuotaCompleted);
			this.OnNetworkedIsBellActivatedChanged?.Invoke(IsBellActivated);
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

		public bool TryWriteIsQuotaCompleted(bool isQuotaCompleted)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			IsQuotaCompleted = isQuotaCompleted;
			return true;
		}

		public bool TryWriteIsBellActivated(bool isBellActivated)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			IsBellActivated = isBellActivated;
			return true;
		}

		private void OnIsQuotaCompletedChangedRender()
		{
			this.OnNetworkedIsQuotaCompletedChanged?.Invoke(IsQuotaCompleted);
		}

		private void OnIsBellActivatedChangedRender()
		{
			this.OnNetworkedIsBellActivatedChanged?.Invoke(IsBellActivated);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			IsQuotaCompleted = _IsQuotaCompleted;
			IsBellActivated = _IsBellActivated;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_IsQuotaCompleted = IsQuotaCompleted;
			_IsBellActivated = IsBellActivated;
		}
	}
}
