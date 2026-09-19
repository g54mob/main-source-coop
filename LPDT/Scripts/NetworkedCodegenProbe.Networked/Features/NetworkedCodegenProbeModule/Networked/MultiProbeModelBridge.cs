using System;
using Features.NetworkedCodegenProbeModule.Data;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;

namespace Features.NetworkedCodegenProbeModule.Networked
{
	public class MultiProbeModelBridge : IDisposable, INetworkedModelShadowBridge
	{
		private readonly MultiProbeModel _multiProbeModel;

		private MultiProbeNetworkObject _multiProbeNetworkObject;

		private bool _hasPendingCount;

		private int _pendingCount;

		private bool _hasPendingBigScore;

		private long _pendingBigScore;

		private bool _hasPendingSmallCount;

		private short _pendingSmallCount;

		private bool _hasPendingTier;

		private byte _pendingTier;

		private bool _hasPendingCharge;

		private float _pendingCharge;

		private bool _hasPendingPrecision;

		private double _pendingPrecision;

		private bool _hasPendingIsReady;

		private bool _pendingIsReady;

		private bool _hasPendingPhase;

		private ProbePhase _pendingPhase;

		public MultiProbeModelBridge(MultiProbeModel multiProbeModel)
		{
			_multiProbeModel = multiProbeModel;
		}

		public void Bind(MultiProbeNetworkObject multiProbeNetworkObject)
		{
			Unbind();
			_multiProbeNetworkObject = multiProbeNetworkObject;
			_multiProbeNetworkObject.OnNetworkedCountChanged += HandleNetworkedCountChanged;
			_multiProbeNetworkObject.OnNetworkedBigScoreChanged += HandleNetworkedBigScoreChanged;
			_multiProbeNetworkObject.OnNetworkedSmallCountChanged += HandleNetworkedSmallCountChanged;
			_multiProbeNetworkObject.OnNetworkedTierChanged += HandleNetworkedTierChanged;
			_multiProbeNetworkObject.OnNetworkedChargeChanged += HandleNetworkedChargeChanged;
			_multiProbeNetworkObject.OnNetworkedPrecisionChanged += HandleNetworkedPrecisionChanged;
			_multiProbeNetworkObject.OnNetworkedIsReadyChanged += HandleNetworkedIsReadyChanged;
			_multiProbeNetworkObject.OnNetworkedPhaseChanged += HandleNetworkedPhaseChanged;
			_multiProbeNetworkObject.OnAuthoritativeTick += HandleAuthoritativeTick;
			_multiProbeNetworkObject.OnDespawned += HandleDespawned;
			_multiProbeNetworkObject.OnRender += HandleRender;
			_multiProbeModel.Count.BindWriter(WriteCount);
			_multiProbeModel.BigScore.BindWriter(WriteBigScore);
			_multiProbeModel.SmallCount.BindWriter(WriteSmallCount);
			_multiProbeModel.Tier.BindWriter(WriteTier);
			_multiProbeModel.Charge.BindWriter(WriteCharge);
			_multiProbeModel.Precision.BindWriter(WritePrecision);
			_multiProbeModel.IsReady.BindWriter(WriteIsReady);
			_multiProbeModel.Phase.BindWriter(WritePhase);
			ApplyCountFromNetwork(_multiProbeNetworkObject.Count);
			ApplyBigScoreFromNetwork(_multiProbeNetworkObject.BigScore);
			ApplySmallCountFromNetwork(_multiProbeNetworkObject.SmallCount);
			ApplyTierFromNetwork(_multiProbeNetworkObject.Tier);
			ApplyChargeFromNetwork(_multiProbeNetworkObject.Charge);
			ApplyPrecisionFromNetwork(_multiProbeNetworkObject.Precision);
			ApplyIsReadyFromNetwork(_multiProbeNetworkObject.IsReady);
			ApplyPhaseFromNetwork(_multiProbeNetworkObject.Phase);
			_multiProbeModel.SetAuthorityProvider(() => _multiProbeNetworkObject.HasStateAuthority);
			_multiProbeModel.SetAttached(isAttached: true);
		}

		public void Unbind()
		{
			if (!(_multiProbeNetworkObject == null))
			{
				_multiProbeNetworkObject.OnNetworkedCountChanged -= HandleNetworkedCountChanged;
				_multiProbeNetworkObject.OnNetworkedBigScoreChanged -= HandleNetworkedBigScoreChanged;
				_multiProbeNetworkObject.OnNetworkedSmallCountChanged -= HandleNetworkedSmallCountChanged;
				_multiProbeNetworkObject.OnNetworkedTierChanged -= HandleNetworkedTierChanged;
				_multiProbeNetworkObject.OnNetworkedChargeChanged -= HandleNetworkedChargeChanged;
				_multiProbeNetworkObject.OnNetworkedPrecisionChanged -= HandleNetworkedPrecisionChanged;
				_multiProbeNetworkObject.OnNetworkedIsReadyChanged -= HandleNetworkedIsReadyChanged;
				_multiProbeNetworkObject.OnNetworkedPhaseChanged -= HandleNetworkedPhaseChanged;
				_multiProbeNetworkObject.OnAuthoritativeTick -= HandleAuthoritativeTick;
				_multiProbeNetworkObject.OnDespawned -= HandleDespawned;
				_multiProbeNetworkObject.OnRender -= HandleRender;
				_multiProbeModel.Count.BindWriter(null);
				_multiProbeModel.BigScore.BindWriter(null);
				_multiProbeModel.SmallCount.BindWriter(null);
				_multiProbeModel.Tier.BindWriter(null);
				_multiProbeModel.Charge.BindWriter(null);
				_multiProbeModel.Precision.BindWriter(null);
				_multiProbeModel.IsReady.BindWriter(null);
				_multiProbeModel.Phase.BindWriter(null);
				_multiProbeNetworkObject = null;
				_hasPendingCount = false;
				_hasPendingBigScore = false;
				_hasPendingSmallCount = false;
				_hasPendingTier = false;
				_hasPendingCharge = false;
				_hasPendingPrecision = false;
				_hasPendingIsReady = false;
				_hasPendingPhase = false;
				_multiProbeModel.SetAuthorityProvider(null);
				_multiProbeModel.SetAttached(isAttached: false);
			}
		}

		public void Dispose()
		{
			Unbind();
		}

		public void BindObject(NetworkObject networkObject)
		{
			Bind(networkObject.GetComponent<MultiProbeNetworkObject>());
		}

		private void HandleDespawned()
		{
			Unbind();
		}

		private void HandleRender()
		{
			_multiProbeModel.RaiseFusionUpdate();
		}

		private void HandleNetworkedCountChanged(int count)
		{
			ApplyCountFromNetwork(count);
		}

		private void HandleNetworkedBigScoreChanged(long bigScore)
		{
			ApplyBigScoreFromNetwork(bigScore);
		}

		private void HandleNetworkedSmallCountChanged(short smallCount)
		{
			ApplySmallCountFromNetwork(smallCount);
		}

		private void HandleNetworkedTierChanged(byte tier)
		{
			ApplyTierFromNetwork(tier);
		}

		private void HandleNetworkedChargeChanged(float charge)
		{
			ApplyChargeFromNetwork(charge);
		}

		private void HandleNetworkedPrecisionChanged(double precision)
		{
			ApplyPrecisionFromNetwork(precision);
		}

		private void HandleNetworkedIsReadyChanged(bool isReady)
		{
			ApplyIsReadyFromNetwork(isReady);
		}

		private void HandleNetworkedPhaseChanged(ProbePhase phase)
		{
			ApplyPhaseFromNetwork(phase);
		}

		private void ApplyCountFromNetwork(int count)
		{
			_multiProbeModel.Count.ApplyFromNetwork(count);
		}

		private void ApplyBigScoreFromNetwork(long bigScore)
		{
			_multiProbeModel.BigScore.ApplyFromNetwork(bigScore);
		}

		private void ApplySmallCountFromNetwork(short smallCount)
		{
			_multiProbeModel.SmallCount.ApplyFromNetwork(smallCount);
		}

		private void ApplyTierFromNetwork(byte tier)
		{
			_multiProbeModel.Tier.ApplyFromNetwork(tier);
		}

		private void ApplyChargeFromNetwork(float charge)
		{
			_multiProbeModel.Charge.ApplyFromNetwork(charge);
		}

		private void ApplyPrecisionFromNetwork(double precision)
		{
			_multiProbeModel.Precision.ApplyFromNetwork(precision);
		}

		private void ApplyIsReadyFromNetwork(bool isReady)
		{
			_multiProbeModel.IsReady.ApplyFromNetwork(isReady);
		}

		private void ApplyPhaseFromNetwork(ProbePhase phase)
		{
			_multiProbeModel.Phase.ApplyFromNetwork(phase);
		}

		private bool WriteCount(int value)
		{
			if (!_multiProbeModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] MultiProbeModel.Count was written without state authority; the write was ignored.");
				return false;
			}
			_pendingCount = value;
			_hasPendingCount = true;
			return true;
		}

		private bool WriteBigScore(long value)
		{
			if (!_multiProbeModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] MultiProbeModel.BigScore was written without state authority; the write was ignored.");
				return false;
			}
			_pendingBigScore = value;
			_hasPendingBigScore = true;
			return true;
		}

		private bool WriteSmallCount(short value)
		{
			if (!_multiProbeModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] MultiProbeModel.SmallCount was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSmallCount = value;
			_hasPendingSmallCount = true;
			return true;
		}

		private bool WriteTier(byte value)
		{
			if (!_multiProbeModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] MultiProbeModel.Tier was written without state authority; the write was ignored.");
				return false;
			}
			_pendingTier = value;
			_hasPendingTier = true;
			return true;
		}

		private bool WriteCharge(float value)
		{
			if (!_multiProbeModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] MultiProbeModel.Charge was written without state authority; the write was ignored.");
				return false;
			}
			_pendingCharge = value;
			_hasPendingCharge = true;
			return true;
		}

		private bool WritePrecision(double value)
		{
			if (!_multiProbeModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] MultiProbeModel.Precision was written without state authority; the write was ignored.");
				return false;
			}
			_pendingPrecision = value;
			_hasPendingPrecision = true;
			return true;
		}

		private bool WriteIsReady(bool value)
		{
			if (!_multiProbeModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] MultiProbeModel.IsReady was written without state authority; the write was ignored.");
				return false;
			}
			_pendingIsReady = value;
			_hasPendingIsReady = true;
			return true;
		}

		private bool WritePhase(ProbePhase value)
		{
			if (!_multiProbeModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] MultiProbeModel.Phase was written without state authority; the write was ignored.");
				return false;
			}
			_pendingPhase = value;
			_hasPendingPhase = true;
			return true;
		}

		private bool IsAuthorityMigratingToMaster()
		{
			if (_multiProbeNetworkObject == null || _multiProbeNetworkObject.Object == null || _multiProbeNetworkObject.Runner == null)
			{
				return false;
			}
			if (_multiProbeNetworkObject.Runner.IsSharedModeMasterClient)
			{
				return (_multiProbeNetworkObject.Object.Flags & NetworkObjectFlags.MasterClientObject) != 0;
			}
			return false;
		}

		private void HandleAuthoritativeTick()
		{
			if (_hasPendingCount)
			{
				_hasPendingCount = false;
				_multiProbeNetworkObject.TryWriteCount(_pendingCount);
			}
			if (_hasPendingBigScore)
			{
				_hasPendingBigScore = false;
				_multiProbeNetworkObject.TryWriteBigScore(_pendingBigScore);
			}
			if (_hasPendingSmallCount)
			{
				_hasPendingSmallCount = false;
				_multiProbeNetworkObject.TryWriteSmallCount(_pendingSmallCount);
			}
			if (_hasPendingTier)
			{
				_hasPendingTier = false;
				_multiProbeNetworkObject.TryWriteTier(_pendingTier);
			}
			if (_hasPendingCharge)
			{
				_hasPendingCharge = false;
				_multiProbeNetworkObject.TryWriteCharge(_pendingCharge);
			}
			if (_hasPendingPrecision)
			{
				_hasPendingPrecision = false;
				_multiProbeNetworkObject.TryWritePrecision(_pendingPrecision);
			}
			if (_hasPendingIsReady)
			{
				_hasPendingIsReady = false;
				_multiProbeNetworkObject.TryWriteIsReady(_pendingIsReady);
			}
			if (_hasPendingPhase)
			{
				_hasPendingPhase = false;
				_multiProbeNetworkObject.TryWritePhase(_pendingPhase);
			}
		}
	}
}
