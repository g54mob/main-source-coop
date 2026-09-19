using System;
using Features.NetworkedModelRuntime;
using Features.SessionManagementModule.Models;
using Fusion;
using UnityEngine;

namespace Features.SessionManagementModule.Networked
{
	public class LevelModelBridge : IDisposable, INetworkedModelShadowBridge
	{
		private readonly LevelModel _levelModel;

		private LevelNetworkObject _levelNetworkObject;

		private bool _hasPendingLevelId;

		private int _pendingLevelId;

		private bool _hasPendingBellStrikeCount;

		private int _pendingBellStrikeCount;

		private bool _hasPendingCountdownStartTick;

		private int _pendingCountdownStartTick;

		public LevelModelBridge(LevelModel levelModel)
		{
			_levelModel = levelModel;
		}

		public void Bind(LevelNetworkObject levelNetworkObject)
		{
			Unbind();
			_levelNetworkObject = levelNetworkObject;
			_levelNetworkObject.OnNetworkedLevelIdChanged += HandleNetworkedLevelIdChanged;
			_levelNetworkObject.OnNetworkedBellStrikeCountChanged += HandleNetworkedBellStrikeCountChanged;
			_levelNetworkObject.OnNetworkedCountdownStartTickChanged += HandleNetworkedCountdownStartTickChanged;
			_levelNetworkObject.OnAuthoritativeTick += HandleAuthoritativeTick;
			_levelNetworkObject.OnDespawned += HandleDespawned;
			_levelModel.LevelId.BindWriter(WriteLevelId);
			_levelModel.BellStrikeCount.BindWriter(WriteBellStrikeCount);
			_levelModel.CountdownStartTick.BindWriter(WriteCountdownStartTick);
			ApplyLevelIdFromNetwork(_levelNetworkObject.LevelId);
			ApplyBellStrikeCountFromNetwork(_levelNetworkObject.BellStrikeCount);
			ApplyCountdownStartTickFromNetwork(_levelNetworkObject.CountdownStartTick);
			_levelModel.SetAuthorityProvider(() => _levelNetworkObject.HasStateAuthority);
			_levelModel.SetAttached(isAttached: true);
		}

		public void Unbind()
		{
			if (!(_levelNetworkObject == null))
			{
				_levelNetworkObject.OnNetworkedLevelIdChanged -= HandleNetworkedLevelIdChanged;
				_levelNetworkObject.OnNetworkedBellStrikeCountChanged -= HandleNetworkedBellStrikeCountChanged;
				_levelNetworkObject.OnNetworkedCountdownStartTickChanged -= HandleNetworkedCountdownStartTickChanged;
				_levelNetworkObject.OnAuthoritativeTick -= HandleAuthoritativeTick;
				_levelNetworkObject.OnDespawned -= HandleDespawned;
				_levelModel.LevelId.BindWriter(null);
				_levelModel.BellStrikeCount.BindWriter(null);
				_levelModel.CountdownStartTick.BindWriter(null);
				_levelNetworkObject = null;
				_hasPendingLevelId = false;
				_hasPendingBellStrikeCount = false;
				_hasPendingCountdownStartTick = false;
				_levelModel.SetAuthorityProvider(null);
				_levelModel.SetAttached(isAttached: false);
			}
		}

		public void Dispose()
		{
			Unbind();
		}

		public void BindObject(NetworkObject networkObject)
		{
			Bind(networkObject.GetComponent<LevelNetworkObject>());
		}

		private void HandleDespawned()
		{
			Unbind();
		}

		private void HandleNetworkedLevelIdChanged(int levelId)
		{
			ApplyLevelIdFromNetwork(levelId);
		}

		private void HandleNetworkedBellStrikeCountChanged(int bellStrikeCount)
		{
			ApplyBellStrikeCountFromNetwork(bellStrikeCount);
		}

		private void HandleNetworkedCountdownStartTickChanged(int countdownStartTick)
		{
			ApplyCountdownStartTickFromNetwork(countdownStartTick);
		}

		private void ApplyLevelIdFromNetwork(int levelId)
		{
			_levelModel.LevelId.ApplyFromNetwork(levelId);
		}

		private void ApplyBellStrikeCountFromNetwork(int bellStrikeCount)
		{
			_levelModel.BellStrikeCount.ApplyFromNetwork(bellStrikeCount);
		}

		private void ApplyCountdownStartTickFromNetwork(int countdownStartTick)
		{
			_levelModel.CountdownStartTick.ApplyFromNetwork(countdownStartTick);
		}

		private bool WriteLevelId(int value)
		{
			if (!_levelModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelModel.LevelId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingLevelId = value;
			_hasPendingLevelId = true;
			return true;
		}

		private bool WriteBellStrikeCount(int value)
		{
			if (!_levelModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelModel.BellStrikeCount was written without state authority; the write was ignored.");
				return false;
			}
			_pendingBellStrikeCount = value;
			_hasPendingBellStrikeCount = true;
			return true;
		}

		private bool WriteCountdownStartTick(int value)
		{
			if (!_levelModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelModel.CountdownStartTick was written without state authority; the write was ignored.");
				return false;
			}
			_pendingCountdownStartTick = value;
			_hasPendingCountdownStartTick = true;
			return true;
		}

		private bool IsAuthorityMigratingToMaster()
		{
			if (_levelNetworkObject == null || _levelNetworkObject.Object == null || _levelNetworkObject.Runner == null)
			{
				return false;
			}
			if (_levelNetworkObject.Runner.IsSharedModeMasterClient)
			{
				return (_levelNetworkObject.Object.Flags & NetworkObjectFlags.MasterClientObject) != 0;
			}
			return false;
		}

		private void HandleAuthoritativeTick()
		{
			if (_hasPendingLevelId)
			{
				_hasPendingLevelId = false;
				_levelNetworkObject.TryWriteLevelId(_pendingLevelId);
			}
			if (_hasPendingBellStrikeCount)
			{
				_hasPendingBellStrikeCount = false;
				_levelNetworkObject.TryWriteBellStrikeCount(_pendingBellStrikeCount);
			}
			if (_hasPendingCountdownStartTick)
			{
				_hasPendingCountdownStartTick = false;
				_levelNetworkObject.TryWriteCountdownStartTick(_pendingCountdownStartTick);
			}
		}
	}
}
