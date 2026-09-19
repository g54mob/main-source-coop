using System;
using Features.NetworkedCodegenProbeModule.Data;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.NetworkedCodegenProbeModule.Networked
{
	[NetworkBehaviourWeaved(10)]
	public class MultiProbeNetworkObject : NetworkBehaviour
	{
		private INetworkedModelInstanceProvider _networkedModelInstanceProvider;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Count", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Count;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("BigScore", 1, 2)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private long _BigScore;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("SmallCount", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private short _SmallCount;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Tier", 4, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private byte _Tier;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Charge", 5, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _Charge;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Precision", 6, 2)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private double _Precision;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsReady", 8, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsReady;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Phase", 9, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private ProbePhase _Phase;

		[Networked]
		[OnChangedRender("OnCountChangedRender")]
		[NetworkedWeaved(0, 1)]
		public unsafe int Count
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MultiProbeNetworkObject.Count. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MultiProbeNetworkObject.Count. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[OnChangedRender("OnBigScoreChangedRender")]
		[NetworkedWeaved(1, 2)]
		public unsafe long BigScore
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MultiProbeNetworkObject.BigScore. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(long*)(Ptr + 1);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MultiProbeNetworkObject.BigScore. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(long*)(Ptr + 1) = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSmallCountChangedRender")]
		[NetworkedWeaved(3, 1)]
		public unsafe short SmallCount
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MultiProbeNetworkObject.SmallCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((short*)Ptr)[6];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MultiProbeNetworkObject.SmallCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				((short*)Ptr)[6] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnTierChangedRender")]
		[NetworkedWeaved(4, 1)]
		public unsafe byte Tier
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MultiProbeNetworkObject.Tier. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((byte*)Ptr)[16];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MultiProbeNetworkObject.Tier. Networked properties can only be accessed when Spawned() has been called.");
				}
				((sbyte*)Ptr)[16] = (sbyte)value;
			}
		}

		[Networked]
		[OnChangedRender("OnChargeChangedRender")]
		[NetworkedWeaved(5, 1)]
		public unsafe float Charge
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MultiProbeNetworkObject.Charge. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 5);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MultiProbeNetworkObject.Charge. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)(Ptr + 5) = value;
			}
		}

		[Networked]
		[OnChangedRender("OnPrecisionChangedRender")]
		[NetworkedWeaved(6, 2)]
		public unsafe double Precision
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MultiProbeNetworkObject.Precision. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((double*)Ptr)[3];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MultiProbeNetworkObject.Precision. Networked properties can only be accessed when Spawned() has been called.");
				}
				((double*)Ptr)[3] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnIsReadyChangedRender")]
		[NetworkedWeaved(8, 1)]
		public unsafe bool IsReady
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MultiProbeNetworkObject.IsReady. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 8);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MultiProbeNetworkObject.IsReady. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 8) = new NetworkBool(value);
			}
		}

		[Networked]
		[OnChangedRender("OnPhaseChangedRender")]
		[NetworkedWeaved(9, 1)]
		public unsafe ProbePhase Phase
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MultiProbeNetworkObject.Phase. Networked properties can only be accessed when Spawned() has been called.");
				}
				return (ProbePhase)Ptr[9];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MultiProbeNetworkObject.Phase. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[9] = (int)value;
			}
		}

		public event Action<int> OnNetworkedCountChanged;

		public event Action<long> OnNetworkedBigScoreChanged;

		public event Action<short> OnNetworkedSmallCountChanged;

		public event Action<byte> OnNetworkedTierChanged;

		public event Action<float> OnNetworkedChargeChanged;

		public event Action<double> OnNetworkedPrecisionChanged;

		public event Action<bool> OnNetworkedIsReadyChanged;

		public event Action<ProbePhase> OnNetworkedPhaseChanged;

		public event Action OnAuthoritativeTick;

		public event Action OnDespawned;

		public event Action OnRender;

		[Inject]
		public void InjectDependencies(INetworkedModelInstanceProvider networkedModelInstanceProvider)
		{
			_networkedModelInstanceProvider = networkedModelInstanceProvider;
		}

		public override void Spawned()
		{
			_networkedModelInstanceProvider?.Register(this);
			this.OnNetworkedCountChanged?.Invoke(Count);
			this.OnNetworkedBigScoreChanged?.Invoke(BigScore);
			this.OnNetworkedSmallCountChanged?.Invoke(SmallCount);
			this.OnNetworkedTierChanged?.Invoke(Tier);
			this.OnNetworkedChargeChanged?.Invoke(Charge);
			this.OnNetworkedPrecisionChanged?.Invoke(Precision);
			this.OnNetworkedIsReadyChanged?.Invoke(IsReady);
			this.OnNetworkedPhaseChanged?.Invoke(Phase);
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

		public override void Render()
		{
			this.OnRender?.Invoke();
		}

		public bool TryWriteCount(int count)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Count = count;
			return true;
		}

		public bool TryWriteBigScore(long bigScore)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			BigScore = bigScore;
			return true;
		}

		public bool TryWriteSmallCount(short smallCount)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			SmallCount = smallCount;
			return true;
		}

		public bool TryWriteTier(byte tier)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Tier = tier;
			return true;
		}

		public bool TryWriteCharge(float charge)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Charge = charge;
			return true;
		}

		public bool TryWritePrecision(double precision)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Precision = precision;
			return true;
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

		public bool TryWritePhase(ProbePhase phase)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Phase = phase;
			return true;
		}

		private void OnCountChangedRender()
		{
			this.OnNetworkedCountChanged?.Invoke(Count);
		}

		private void OnBigScoreChangedRender()
		{
			this.OnNetworkedBigScoreChanged?.Invoke(BigScore);
		}

		private void OnSmallCountChangedRender()
		{
			this.OnNetworkedSmallCountChanged?.Invoke(SmallCount);
		}

		private void OnTierChangedRender()
		{
			this.OnNetworkedTierChanged?.Invoke(Tier);
		}

		private void OnChargeChangedRender()
		{
			this.OnNetworkedChargeChanged?.Invoke(Charge);
		}

		private void OnPrecisionChangedRender()
		{
			this.OnNetworkedPrecisionChanged?.Invoke(Precision);
		}

		private void OnIsReadyChangedRender()
		{
			this.OnNetworkedIsReadyChanged?.Invoke(IsReady);
		}

		private void OnPhaseChangedRender()
		{
			this.OnNetworkedPhaseChanged?.Invoke(Phase);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			Count = _Count;
			BigScore = _BigScore;
			SmallCount = _SmallCount;
			Tier = _Tier;
			Charge = _Charge;
			Precision = _Precision;
			IsReady = _IsReady;
			Phase = _Phase;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_Count = Count;
			_BigScore = BigScore;
			_SmallCount = SmallCount;
			_Tier = Tier;
			_Charge = Charge;
			_Precision = Precision;
			_IsReady = IsReady;
			_Phase = Phase;
		}
	}
}
