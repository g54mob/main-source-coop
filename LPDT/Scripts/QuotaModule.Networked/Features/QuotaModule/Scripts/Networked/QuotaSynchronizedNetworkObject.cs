using System;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.QuotaModule.Scripts.Networked
{
	[NetworkBehaviourWeaved(2)]
	public class QuotaSynchronizedNetworkObject : NetworkBehaviour
	{
		private INetworkedModelInstanceProvider _networkedModelInstanceProvider;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("CurrentQuota", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _CurrentQuota;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("MaxQuota", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _MaxQuota;

		[Networked]
		[OnChangedRender("OnCurrentQuotaChangedRender")]
		[NetworkedWeaved(0, 1)]
		public unsafe float CurrentQuota
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing QuotaSynchronizedNetworkObject.CurrentQuota. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing QuotaSynchronizedNetworkObject.CurrentQuota. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[OnChangedRender("OnMaxQuotaChangedRender")]
		[NetworkedWeaved(1, 1)]
		public unsafe float MaxQuota
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing QuotaSynchronizedNetworkObject.MaxQuota. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 1);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing QuotaSynchronizedNetworkObject.MaxQuota. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)(Ptr + 1) = value;
			}
		}

		public event Action<float> OnNetworkedCurrentQuotaChanged;

		public event Action<float> OnNetworkedMaxQuotaChanged;

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
			this.OnNetworkedCurrentQuotaChanged?.Invoke(CurrentQuota);
			this.OnNetworkedMaxQuotaChanged?.Invoke(MaxQuota);
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

		public bool TryWriteCurrentQuota(float currentQuota)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			CurrentQuota = currentQuota;
			return true;
		}

		public bool TryWriteMaxQuota(float maxQuota)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			MaxQuota = maxQuota;
			return true;
		}

		private void OnCurrentQuotaChangedRender()
		{
			this.OnNetworkedCurrentQuotaChanged?.Invoke(CurrentQuota);
		}

		private void OnMaxQuotaChangedRender()
		{
			this.OnNetworkedMaxQuotaChanged?.Invoke(MaxQuota);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			CurrentQuota = _CurrentQuota;
			MaxQuota = _MaxQuota;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_CurrentQuota = CurrentQuota;
			_MaxQuota = MaxQuota;
		}
	}
}
