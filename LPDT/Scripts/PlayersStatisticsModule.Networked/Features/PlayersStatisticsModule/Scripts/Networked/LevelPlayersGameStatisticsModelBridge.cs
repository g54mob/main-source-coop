using System;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;

namespace Features.PlayersStatisticsModule.Scripts.Networked
{
	public class LevelPlayersGameStatisticsModelBridge : IDisposable, INetworkedModelShadowBridge
	{
		private readonly LevelPlayersGameStatisticsModel _levelPlayersGameStatisticsModel;

		private LevelPlayersGameStatisticsNetworkObject _levelPlayersGameStatisticsNetworkObject;

		private bool _hasPendingSlot0PlayerIdPlusOne;

		private int _pendingSlot0PlayerIdPlusOne;

		private bool _hasPendingSlot0Deaths;

		private int _pendingSlot0Deaths;

		private bool _hasPendingSlot0Kills;

		private int _pendingSlot0Kills;

		private bool _hasPendingSlot0Revives;

		private int _pendingSlot0Revives;

		private bool _hasPendingSlot0Cents;

		private int _pendingSlot0Cents;

		private bool _hasPendingSlot1PlayerIdPlusOne;

		private int _pendingSlot1PlayerIdPlusOne;

		private bool _hasPendingSlot1Deaths;

		private int _pendingSlot1Deaths;

		private bool _hasPendingSlot1Kills;

		private int _pendingSlot1Kills;

		private bool _hasPendingSlot1Revives;

		private int _pendingSlot1Revives;

		private bool _hasPendingSlot1Cents;

		private int _pendingSlot1Cents;

		private bool _hasPendingSlot2PlayerIdPlusOne;

		private int _pendingSlot2PlayerIdPlusOne;

		private bool _hasPendingSlot2Deaths;

		private int _pendingSlot2Deaths;

		private bool _hasPendingSlot2Kills;

		private int _pendingSlot2Kills;

		private bool _hasPendingSlot2Revives;

		private int _pendingSlot2Revives;

		private bool _hasPendingSlot2Cents;

		private int _pendingSlot2Cents;

		private bool _hasPendingSlot3PlayerIdPlusOne;

		private int _pendingSlot3PlayerIdPlusOne;

		private bool _hasPendingSlot3Deaths;

		private int _pendingSlot3Deaths;

		private bool _hasPendingSlot3Kills;

		private int _pendingSlot3Kills;

		private bool _hasPendingSlot3Revives;

		private int _pendingSlot3Revives;

		private bool _hasPendingSlot3Cents;

		private int _pendingSlot3Cents;

		private bool _hasPendingCouldroneCount;

		private int _pendingCouldroneCount;

		private bool _hasPendingCartCount;

		private int _pendingCartCount;

		private bool _hasPendingDeadPartCount;

		private int _pendingDeadPartCount;

		private bool _hasPendingSpawnedNewItemId;

		private int _pendingSpawnedNewItemId;

		public LevelPlayersGameStatisticsModelBridge(LevelPlayersGameStatisticsModel levelPlayersGameStatisticsModel)
		{
			_levelPlayersGameStatisticsModel = levelPlayersGameStatisticsModel;
		}

		public void Bind(LevelPlayersGameStatisticsNetworkObject levelPlayersGameStatisticsNetworkObject)
		{
			Unbind();
			_levelPlayersGameStatisticsNetworkObject = levelPlayersGameStatisticsNetworkObject;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot0PlayerIdPlusOneChanged += HandleNetworkedSlot0PlayerIdPlusOneChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot0DeathsChanged += HandleNetworkedSlot0DeathsChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot0KillsChanged += HandleNetworkedSlot0KillsChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot0RevivesChanged += HandleNetworkedSlot0RevivesChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot0CentsChanged += HandleNetworkedSlot0CentsChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot1PlayerIdPlusOneChanged += HandleNetworkedSlot1PlayerIdPlusOneChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot1DeathsChanged += HandleNetworkedSlot1DeathsChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot1KillsChanged += HandleNetworkedSlot1KillsChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot1RevivesChanged += HandleNetworkedSlot1RevivesChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot1CentsChanged += HandleNetworkedSlot1CentsChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot2PlayerIdPlusOneChanged += HandleNetworkedSlot2PlayerIdPlusOneChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot2DeathsChanged += HandleNetworkedSlot2DeathsChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot2KillsChanged += HandleNetworkedSlot2KillsChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot2RevivesChanged += HandleNetworkedSlot2RevivesChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot2CentsChanged += HandleNetworkedSlot2CentsChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot3PlayerIdPlusOneChanged += HandleNetworkedSlot3PlayerIdPlusOneChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot3DeathsChanged += HandleNetworkedSlot3DeathsChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot3KillsChanged += HandleNetworkedSlot3KillsChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot3RevivesChanged += HandleNetworkedSlot3RevivesChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot3CentsChanged += HandleNetworkedSlot3CentsChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedCouldroneCountChanged += HandleNetworkedCouldroneCountChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedCartCountChanged += HandleNetworkedCartCountChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedDeadPartCountChanged += HandleNetworkedDeadPartCountChanged;
			_levelPlayersGameStatisticsNetworkObject.OnNetworkedSpawnedNewItemIdChanged += HandleNetworkedSpawnedNewItemIdChanged;
			_levelPlayersGameStatisticsNetworkObject.OnAuthoritativeTick += HandleAuthoritativeTick;
			_levelPlayersGameStatisticsNetworkObject.OnDespawned += HandleDespawned;
			_levelPlayersGameStatisticsNetworkObject.OnReviveSignalReceived += HandleReviveSignalReceived;
			_levelPlayersGameStatisticsNetworkObject.OnResetSignalReceived += HandleResetSignalReceived;
			_levelPlayersGameStatisticsModel.Slot0PlayerIdPlusOne.BindWriter(WriteSlot0PlayerIdPlusOne);
			_levelPlayersGameStatisticsModel.Slot0Deaths.BindWriter(WriteSlot0Deaths);
			_levelPlayersGameStatisticsModel.Slot0Kills.BindWriter(WriteSlot0Kills);
			_levelPlayersGameStatisticsModel.Slot0Revives.BindWriter(WriteSlot0Revives);
			_levelPlayersGameStatisticsModel.Slot0Cents.BindWriter(WriteSlot0Cents);
			_levelPlayersGameStatisticsModel.Slot1PlayerIdPlusOne.BindWriter(WriteSlot1PlayerIdPlusOne);
			_levelPlayersGameStatisticsModel.Slot1Deaths.BindWriter(WriteSlot1Deaths);
			_levelPlayersGameStatisticsModel.Slot1Kills.BindWriter(WriteSlot1Kills);
			_levelPlayersGameStatisticsModel.Slot1Revives.BindWriter(WriteSlot1Revives);
			_levelPlayersGameStatisticsModel.Slot1Cents.BindWriter(WriteSlot1Cents);
			_levelPlayersGameStatisticsModel.Slot2PlayerIdPlusOne.BindWriter(WriteSlot2PlayerIdPlusOne);
			_levelPlayersGameStatisticsModel.Slot2Deaths.BindWriter(WriteSlot2Deaths);
			_levelPlayersGameStatisticsModel.Slot2Kills.BindWriter(WriteSlot2Kills);
			_levelPlayersGameStatisticsModel.Slot2Revives.BindWriter(WriteSlot2Revives);
			_levelPlayersGameStatisticsModel.Slot2Cents.BindWriter(WriteSlot2Cents);
			_levelPlayersGameStatisticsModel.Slot3PlayerIdPlusOne.BindWriter(WriteSlot3PlayerIdPlusOne);
			_levelPlayersGameStatisticsModel.Slot3Deaths.BindWriter(WriteSlot3Deaths);
			_levelPlayersGameStatisticsModel.Slot3Kills.BindWriter(WriteSlot3Kills);
			_levelPlayersGameStatisticsModel.Slot3Revives.BindWriter(WriteSlot3Revives);
			_levelPlayersGameStatisticsModel.Slot3Cents.BindWriter(WriteSlot3Cents);
			_levelPlayersGameStatisticsModel.CouldroneCount.BindWriter(WriteCouldroneCount);
			_levelPlayersGameStatisticsModel.CartCount.BindWriter(WriteCartCount);
			_levelPlayersGameStatisticsModel.DeadPartCount.BindWriter(WriteDeadPartCount);
			_levelPlayersGameStatisticsModel.SpawnedNewItemId.BindWriter(WriteSpawnedNewItemId);
			_levelPlayersGameStatisticsModel.ReviveSignal.BindSender(_levelPlayersGameStatisticsNetworkObject.RpcRaiseReviveSignal, "LevelPlayersGameStatisticsModel.ReviveSignal");
			_levelPlayersGameStatisticsModel.ResetSignal.BindSender(_levelPlayersGameStatisticsNetworkObject.RpcRaiseResetSignal, "LevelPlayersGameStatisticsModel.ResetSignal");
			ApplySlot0PlayerIdPlusOneFromNetwork(_levelPlayersGameStatisticsNetworkObject.Slot0PlayerIdPlusOne);
			ApplySlot0DeathsFromNetwork(_levelPlayersGameStatisticsNetworkObject.Slot0Deaths);
			ApplySlot0KillsFromNetwork(_levelPlayersGameStatisticsNetworkObject.Slot0Kills);
			ApplySlot0RevivesFromNetwork(_levelPlayersGameStatisticsNetworkObject.Slot0Revives);
			ApplySlot0CentsFromNetwork(_levelPlayersGameStatisticsNetworkObject.Slot0Cents);
			ApplySlot1PlayerIdPlusOneFromNetwork(_levelPlayersGameStatisticsNetworkObject.Slot1PlayerIdPlusOne);
			ApplySlot1DeathsFromNetwork(_levelPlayersGameStatisticsNetworkObject.Slot1Deaths);
			ApplySlot1KillsFromNetwork(_levelPlayersGameStatisticsNetworkObject.Slot1Kills);
			ApplySlot1RevivesFromNetwork(_levelPlayersGameStatisticsNetworkObject.Slot1Revives);
			ApplySlot1CentsFromNetwork(_levelPlayersGameStatisticsNetworkObject.Slot1Cents);
			ApplySlot2PlayerIdPlusOneFromNetwork(_levelPlayersGameStatisticsNetworkObject.Slot2PlayerIdPlusOne);
			ApplySlot2DeathsFromNetwork(_levelPlayersGameStatisticsNetworkObject.Slot2Deaths);
			ApplySlot2KillsFromNetwork(_levelPlayersGameStatisticsNetworkObject.Slot2Kills);
			ApplySlot2RevivesFromNetwork(_levelPlayersGameStatisticsNetworkObject.Slot2Revives);
			ApplySlot2CentsFromNetwork(_levelPlayersGameStatisticsNetworkObject.Slot2Cents);
			ApplySlot3PlayerIdPlusOneFromNetwork(_levelPlayersGameStatisticsNetworkObject.Slot3PlayerIdPlusOne);
			ApplySlot3DeathsFromNetwork(_levelPlayersGameStatisticsNetworkObject.Slot3Deaths);
			ApplySlot3KillsFromNetwork(_levelPlayersGameStatisticsNetworkObject.Slot3Kills);
			ApplySlot3RevivesFromNetwork(_levelPlayersGameStatisticsNetworkObject.Slot3Revives);
			ApplySlot3CentsFromNetwork(_levelPlayersGameStatisticsNetworkObject.Slot3Cents);
			ApplyCouldroneCountFromNetwork(_levelPlayersGameStatisticsNetworkObject.CouldroneCount);
			ApplyCartCountFromNetwork(_levelPlayersGameStatisticsNetworkObject.CartCount);
			ApplyDeadPartCountFromNetwork(_levelPlayersGameStatisticsNetworkObject.DeadPartCount);
			ApplySpawnedNewItemIdFromNetwork(_levelPlayersGameStatisticsNetworkObject.SpawnedNewItemId);
			_levelPlayersGameStatisticsModel.SetAuthorityProvider(() => _levelPlayersGameStatisticsNetworkObject.HasStateAuthority);
			_levelPlayersGameStatisticsModel.SetAttached(isAttached: true);
		}

		public void Unbind()
		{
			if (!(_levelPlayersGameStatisticsNetworkObject == null))
			{
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot0PlayerIdPlusOneChanged -= HandleNetworkedSlot0PlayerIdPlusOneChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot0DeathsChanged -= HandleNetworkedSlot0DeathsChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot0KillsChanged -= HandleNetworkedSlot0KillsChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot0RevivesChanged -= HandleNetworkedSlot0RevivesChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot0CentsChanged -= HandleNetworkedSlot0CentsChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot1PlayerIdPlusOneChanged -= HandleNetworkedSlot1PlayerIdPlusOneChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot1DeathsChanged -= HandleNetworkedSlot1DeathsChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot1KillsChanged -= HandleNetworkedSlot1KillsChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot1RevivesChanged -= HandleNetworkedSlot1RevivesChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot1CentsChanged -= HandleNetworkedSlot1CentsChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot2PlayerIdPlusOneChanged -= HandleNetworkedSlot2PlayerIdPlusOneChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot2DeathsChanged -= HandleNetworkedSlot2DeathsChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot2KillsChanged -= HandleNetworkedSlot2KillsChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot2RevivesChanged -= HandleNetworkedSlot2RevivesChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot2CentsChanged -= HandleNetworkedSlot2CentsChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot3PlayerIdPlusOneChanged -= HandleNetworkedSlot3PlayerIdPlusOneChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot3DeathsChanged -= HandleNetworkedSlot3DeathsChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot3KillsChanged -= HandleNetworkedSlot3KillsChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot3RevivesChanged -= HandleNetworkedSlot3RevivesChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedSlot3CentsChanged -= HandleNetworkedSlot3CentsChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedCouldroneCountChanged -= HandleNetworkedCouldroneCountChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedCartCountChanged -= HandleNetworkedCartCountChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedDeadPartCountChanged -= HandleNetworkedDeadPartCountChanged;
				_levelPlayersGameStatisticsNetworkObject.OnNetworkedSpawnedNewItemIdChanged -= HandleNetworkedSpawnedNewItemIdChanged;
				_levelPlayersGameStatisticsNetworkObject.OnAuthoritativeTick -= HandleAuthoritativeTick;
				_levelPlayersGameStatisticsNetworkObject.OnDespawned -= HandleDespawned;
				_levelPlayersGameStatisticsNetworkObject.OnReviveSignalReceived -= HandleReviveSignalReceived;
				_levelPlayersGameStatisticsNetworkObject.OnResetSignalReceived -= HandleResetSignalReceived;
				_levelPlayersGameStatisticsModel.Slot0PlayerIdPlusOne.BindWriter(null);
				_levelPlayersGameStatisticsModel.Slot0Deaths.BindWriter(null);
				_levelPlayersGameStatisticsModel.Slot0Kills.BindWriter(null);
				_levelPlayersGameStatisticsModel.Slot0Revives.BindWriter(null);
				_levelPlayersGameStatisticsModel.Slot0Cents.BindWriter(null);
				_levelPlayersGameStatisticsModel.Slot1PlayerIdPlusOne.BindWriter(null);
				_levelPlayersGameStatisticsModel.Slot1Deaths.BindWriter(null);
				_levelPlayersGameStatisticsModel.Slot1Kills.BindWriter(null);
				_levelPlayersGameStatisticsModel.Slot1Revives.BindWriter(null);
				_levelPlayersGameStatisticsModel.Slot1Cents.BindWriter(null);
				_levelPlayersGameStatisticsModel.Slot2PlayerIdPlusOne.BindWriter(null);
				_levelPlayersGameStatisticsModel.Slot2Deaths.BindWriter(null);
				_levelPlayersGameStatisticsModel.Slot2Kills.BindWriter(null);
				_levelPlayersGameStatisticsModel.Slot2Revives.BindWriter(null);
				_levelPlayersGameStatisticsModel.Slot2Cents.BindWriter(null);
				_levelPlayersGameStatisticsModel.Slot3PlayerIdPlusOne.BindWriter(null);
				_levelPlayersGameStatisticsModel.Slot3Deaths.BindWriter(null);
				_levelPlayersGameStatisticsModel.Slot3Kills.BindWriter(null);
				_levelPlayersGameStatisticsModel.Slot3Revives.BindWriter(null);
				_levelPlayersGameStatisticsModel.Slot3Cents.BindWriter(null);
				_levelPlayersGameStatisticsModel.CouldroneCount.BindWriter(null);
				_levelPlayersGameStatisticsModel.CartCount.BindWriter(null);
				_levelPlayersGameStatisticsModel.DeadPartCount.BindWriter(null);
				_levelPlayersGameStatisticsModel.SpawnedNewItemId.BindWriter(null);
				_levelPlayersGameStatisticsModel.ReviveSignal.BindSender(null, null);
				_levelPlayersGameStatisticsModel.ResetSignal.BindSender(null, null);
				_levelPlayersGameStatisticsNetworkObject = null;
				_hasPendingSlot0PlayerIdPlusOne = false;
				_hasPendingSlot0Deaths = false;
				_hasPendingSlot0Kills = false;
				_hasPendingSlot0Revives = false;
				_hasPendingSlot0Cents = false;
				_hasPendingSlot1PlayerIdPlusOne = false;
				_hasPendingSlot1Deaths = false;
				_hasPendingSlot1Kills = false;
				_hasPendingSlot1Revives = false;
				_hasPendingSlot1Cents = false;
				_hasPendingSlot2PlayerIdPlusOne = false;
				_hasPendingSlot2Deaths = false;
				_hasPendingSlot2Kills = false;
				_hasPendingSlot2Revives = false;
				_hasPendingSlot2Cents = false;
				_hasPendingSlot3PlayerIdPlusOne = false;
				_hasPendingSlot3Deaths = false;
				_hasPendingSlot3Kills = false;
				_hasPendingSlot3Revives = false;
				_hasPendingSlot3Cents = false;
				_hasPendingCouldroneCount = false;
				_hasPendingCartCount = false;
				_hasPendingDeadPartCount = false;
				_hasPendingSpawnedNewItemId = false;
				_levelPlayersGameStatisticsModel.SetAuthorityProvider(null);
				_levelPlayersGameStatisticsModel.SetAttached(isAttached: false);
			}
		}

		public void Dispose()
		{
			Unbind();
		}

		public void BindObject(NetworkObject networkObject)
		{
			Bind(networkObject.GetComponent<LevelPlayersGameStatisticsNetworkObject>());
		}

		private void HandleDespawned()
		{
			Unbind();
		}

		private void HandleReviveSignalReceived(int value)
		{
			_levelPlayersGameStatisticsModel.ReviveSignal.RaiseReceived(value);
		}

		private void HandleResetSignalReceived()
		{
			_levelPlayersGameStatisticsModel.ResetSignal.RaiseReceived();
		}

		private void HandleNetworkedSlot0PlayerIdPlusOneChanged(int slot0PlayerIdPlusOne)
		{
			ApplySlot0PlayerIdPlusOneFromNetwork(slot0PlayerIdPlusOne);
		}

		private void HandleNetworkedSlot0DeathsChanged(int slot0Deaths)
		{
			ApplySlot0DeathsFromNetwork(slot0Deaths);
		}

		private void HandleNetworkedSlot0KillsChanged(int slot0Kills)
		{
			ApplySlot0KillsFromNetwork(slot0Kills);
		}

		private void HandleNetworkedSlot0RevivesChanged(int slot0Revives)
		{
			ApplySlot0RevivesFromNetwork(slot0Revives);
		}

		private void HandleNetworkedSlot0CentsChanged(int slot0Cents)
		{
			ApplySlot0CentsFromNetwork(slot0Cents);
		}

		private void HandleNetworkedSlot1PlayerIdPlusOneChanged(int slot1PlayerIdPlusOne)
		{
			ApplySlot1PlayerIdPlusOneFromNetwork(slot1PlayerIdPlusOne);
		}

		private void HandleNetworkedSlot1DeathsChanged(int slot1Deaths)
		{
			ApplySlot1DeathsFromNetwork(slot1Deaths);
		}

		private void HandleNetworkedSlot1KillsChanged(int slot1Kills)
		{
			ApplySlot1KillsFromNetwork(slot1Kills);
		}

		private void HandleNetworkedSlot1RevivesChanged(int slot1Revives)
		{
			ApplySlot1RevivesFromNetwork(slot1Revives);
		}

		private void HandleNetworkedSlot1CentsChanged(int slot1Cents)
		{
			ApplySlot1CentsFromNetwork(slot1Cents);
		}

		private void HandleNetworkedSlot2PlayerIdPlusOneChanged(int slot2PlayerIdPlusOne)
		{
			ApplySlot2PlayerIdPlusOneFromNetwork(slot2PlayerIdPlusOne);
		}

		private void HandleNetworkedSlot2DeathsChanged(int slot2Deaths)
		{
			ApplySlot2DeathsFromNetwork(slot2Deaths);
		}

		private void HandleNetworkedSlot2KillsChanged(int slot2Kills)
		{
			ApplySlot2KillsFromNetwork(slot2Kills);
		}

		private void HandleNetworkedSlot2RevivesChanged(int slot2Revives)
		{
			ApplySlot2RevivesFromNetwork(slot2Revives);
		}

		private void HandleNetworkedSlot2CentsChanged(int slot2Cents)
		{
			ApplySlot2CentsFromNetwork(slot2Cents);
		}

		private void HandleNetworkedSlot3PlayerIdPlusOneChanged(int slot3PlayerIdPlusOne)
		{
			ApplySlot3PlayerIdPlusOneFromNetwork(slot3PlayerIdPlusOne);
		}

		private void HandleNetworkedSlot3DeathsChanged(int slot3Deaths)
		{
			ApplySlot3DeathsFromNetwork(slot3Deaths);
		}

		private void HandleNetworkedSlot3KillsChanged(int slot3Kills)
		{
			ApplySlot3KillsFromNetwork(slot3Kills);
		}

		private void HandleNetworkedSlot3RevivesChanged(int slot3Revives)
		{
			ApplySlot3RevivesFromNetwork(slot3Revives);
		}

		private void HandleNetworkedSlot3CentsChanged(int slot3Cents)
		{
			ApplySlot3CentsFromNetwork(slot3Cents);
		}

		private void HandleNetworkedCouldroneCountChanged(int couldroneCount)
		{
			ApplyCouldroneCountFromNetwork(couldroneCount);
		}

		private void HandleNetworkedCartCountChanged(int cartCount)
		{
			ApplyCartCountFromNetwork(cartCount);
		}

		private void HandleNetworkedDeadPartCountChanged(int deadPartCount)
		{
			ApplyDeadPartCountFromNetwork(deadPartCount);
		}

		private void HandleNetworkedSpawnedNewItemIdChanged(int spawnedNewItemId)
		{
			ApplySpawnedNewItemIdFromNetwork(spawnedNewItemId);
		}

		private void ApplySlot0PlayerIdPlusOneFromNetwork(int slot0PlayerIdPlusOne)
		{
			_levelPlayersGameStatisticsModel.Slot0PlayerIdPlusOne.ApplyFromNetwork(slot0PlayerIdPlusOne);
		}

		private void ApplySlot0DeathsFromNetwork(int slot0Deaths)
		{
			_levelPlayersGameStatisticsModel.Slot0Deaths.ApplyFromNetwork(slot0Deaths);
		}

		private void ApplySlot0KillsFromNetwork(int slot0Kills)
		{
			_levelPlayersGameStatisticsModel.Slot0Kills.ApplyFromNetwork(slot0Kills);
		}

		private void ApplySlot0RevivesFromNetwork(int slot0Revives)
		{
			_levelPlayersGameStatisticsModel.Slot0Revives.ApplyFromNetwork(slot0Revives);
		}

		private void ApplySlot0CentsFromNetwork(int slot0Cents)
		{
			_levelPlayersGameStatisticsModel.Slot0Cents.ApplyFromNetwork(slot0Cents);
		}

		private void ApplySlot1PlayerIdPlusOneFromNetwork(int slot1PlayerIdPlusOne)
		{
			_levelPlayersGameStatisticsModel.Slot1PlayerIdPlusOne.ApplyFromNetwork(slot1PlayerIdPlusOne);
		}

		private void ApplySlot1DeathsFromNetwork(int slot1Deaths)
		{
			_levelPlayersGameStatisticsModel.Slot1Deaths.ApplyFromNetwork(slot1Deaths);
		}

		private void ApplySlot1KillsFromNetwork(int slot1Kills)
		{
			_levelPlayersGameStatisticsModel.Slot1Kills.ApplyFromNetwork(slot1Kills);
		}

		private void ApplySlot1RevivesFromNetwork(int slot1Revives)
		{
			_levelPlayersGameStatisticsModel.Slot1Revives.ApplyFromNetwork(slot1Revives);
		}

		private void ApplySlot1CentsFromNetwork(int slot1Cents)
		{
			_levelPlayersGameStatisticsModel.Slot1Cents.ApplyFromNetwork(slot1Cents);
		}

		private void ApplySlot2PlayerIdPlusOneFromNetwork(int slot2PlayerIdPlusOne)
		{
			_levelPlayersGameStatisticsModel.Slot2PlayerIdPlusOne.ApplyFromNetwork(slot2PlayerIdPlusOne);
		}

		private void ApplySlot2DeathsFromNetwork(int slot2Deaths)
		{
			_levelPlayersGameStatisticsModel.Slot2Deaths.ApplyFromNetwork(slot2Deaths);
		}

		private void ApplySlot2KillsFromNetwork(int slot2Kills)
		{
			_levelPlayersGameStatisticsModel.Slot2Kills.ApplyFromNetwork(slot2Kills);
		}

		private void ApplySlot2RevivesFromNetwork(int slot2Revives)
		{
			_levelPlayersGameStatisticsModel.Slot2Revives.ApplyFromNetwork(slot2Revives);
		}

		private void ApplySlot2CentsFromNetwork(int slot2Cents)
		{
			_levelPlayersGameStatisticsModel.Slot2Cents.ApplyFromNetwork(slot2Cents);
		}

		private void ApplySlot3PlayerIdPlusOneFromNetwork(int slot3PlayerIdPlusOne)
		{
			_levelPlayersGameStatisticsModel.Slot3PlayerIdPlusOne.ApplyFromNetwork(slot3PlayerIdPlusOne);
		}

		private void ApplySlot3DeathsFromNetwork(int slot3Deaths)
		{
			_levelPlayersGameStatisticsModel.Slot3Deaths.ApplyFromNetwork(slot3Deaths);
		}

		private void ApplySlot3KillsFromNetwork(int slot3Kills)
		{
			_levelPlayersGameStatisticsModel.Slot3Kills.ApplyFromNetwork(slot3Kills);
		}

		private void ApplySlot3RevivesFromNetwork(int slot3Revives)
		{
			_levelPlayersGameStatisticsModel.Slot3Revives.ApplyFromNetwork(slot3Revives);
		}

		private void ApplySlot3CentsFromNetwork(int slot3Cents)
		{
			_levelPlayersGameStatisticsModel.Slot3Cents.ApplyFromNetwork(slot3Cents);
		}

		private void ApplyCouldroneCountFromNetwork(int couldroneCount)
		{
			_levelPlayersGameStatisticsModel.CouldroneCount.ApplyFromNetwork(couldroneCount);
		}

		private void ApplyCartCountFromNetwork(int cartCount)
		{
			_levelPlayersGameStatisticsModel.CartCount.ApplyFromNetwork(cartCount);
		}

		private void ApplyDeadPartCountFromNetwork(int deadPartCount)
		{
			_levelPlayersGameStatisticsModel.DeadPartCount.ApplyFromNetwork(deadPartCount);
		}

		private void ApplySpawnedNewItemIdFromNetwork(int spawnedNewItemId)
		{
			_levelPlayersGameStatisticsModel.SpawnedNewItemId.ApplyFromNetwork(spawnedNewItemId);
		}

		private bool WriteSlot0PlayerIdPlusOne(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.Slot0PlayerIdPlusOne was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot0PlayerIdPlusOne = value;
			_hasPendingSlot0PlayerIdPlusOne = true;
			return true;
		}

		private bool WriteSlot0Deaths(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.Slot0Deaths was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot0Deaths = value;
			_hasPendingSlot0Deaths = true;
			return true;
		}

		private bool WriteSlot0Kills(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.Slot0Kills was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot0Kills = value;
			_hasPendingSlot0Kills = true;
			return true;
		}

		private bool WriteSlot0Revives(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.Slot0Revives was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot0Revives = value;
			_hasPendingSlot0Revives = true;
			return true;
		}

		private bool WriteSlot0Cents(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.Slot0Cents was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot0Cents = value;
			_hasPendingSlot0Cents = true;
			return true;
		}

		private bool WriteSlot1PlayerIdPlusOne(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.Slot1PlayerIdPlusOne was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot1PlayerIdPlusOne = value;
			_hasPendingSlot1PlayerIdPlusOne = true;
			return true;
		}

		private bool WriteSlot1Deaths(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.Slot1Deaths was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot1Deaths = value;
			_hasPendingSlot1Deaths = true;
			return true;
		}

		private bool WriteSlot1Kills(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.Slot1Kills was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot1Kills = value;
			_hasPendingSlot1Kills = true;
			return true;
		}

		private bool WriteSlot1Revives(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.Slot1Revives was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot1Revives = value;
			_hasPendingSlot1Revives = true;
			return true;
		}

		private bool WriteSlot1Cents(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.Slot1Cents was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot1Cents = value;
			_hasPendingSlot1Cents = true;
			return true;
		}

		private bool WriteSlot2PlayerIdPlusOne(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.Slot2PlayerIdPlusOne was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot2PlayerIdPlusOne = value;
			_hasPendingSlot2PlayerIdPlusOne = true;
			return true;
		}

		private bool WriteSlot2Deaths(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.Slot2Deaths was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot2Deaths = value;
			_hasPendingSlot2Deaths = true;
			return true;
		}

		private bool WriteSlot2Kills(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.Slot2Kills was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot2Kills = value;
			_hasPendingSlot2Kills = true;
			return true;
		}

		private bool WriteSlot2Revives(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.Slot2Revives was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot2Revives = value;
			_hasPendingSlot2Revives = true;
			return true;
		}

		private bool WriteSlot2Cents(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.Slot2Cents was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot2Cents = value;
			_hasPendingSlot2Cents = true;
			return true;
		}

		private bool WriteSlot3PlayerIdPlusOne(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.Slot3PlayerIdPlusOne was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot3PlayerIdPlusOne = value;
			_hasPendingSlot3PlayerIdPlusOne = true;
			return true;
		}

		private bool WriteSlot3Deaths(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.Slot3Deaths was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot3Deaths = value;
			_hasPendingSlot3Deaths = true;
			return true;
		}

		private bool WriteSlot3Kills(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.Slot3Kills was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot3Kills = value;
			_hasPendingSlot3Kills = true;
			return true;
		}

		private bool WriteSlot3Revives(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.Slot3Revives was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot3Revives = value;
			_hasPendingSlot3Revives = true;
			return true;
		}

		private bool WriteSlot3Cents(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.Slot3Cents was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSlot3Cents = value;
			_hasPendingSlot3Cents = true;
			return true;
		}

		private bool WriteCouldroneCount(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.CouldroneCount was written without state authority; the write was ignored.");
				return false;
			}
			_pendingCouldroneCount = value;
			_hasPendingCouldroneCount = true;
			return true;
		}

		private bool WriteCartCount(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.CartCount was written without state authority; the write was ignored.");
				return false;
			}
			_pendingCartCount = value;
			_hasPendingCartCount = true;
			return true;
		}

		private bool WriteDeadPartCount(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.DeadPartCount was written without state authority; the write was ignored.");
				return false;
			}
			_pendingDeadPartCount = value;
			_hasPendingDeadPartCount = true;
			return true;
		}

		private bool WriteSpawnedNewItemId(int value)
		{
			if (!_levelPlayersGameStatisticsModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayersGameStatisticsModel.SpawnedNewItemId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSpawnedNewItemId = value;
			_hasPendingSpawnedNewItemId = true;
			return true;
		}

		private bool IsAuthorityMigratingToMaster()
		{
			if (_levelPlayersGameStatisticsNetworkObject == null || _levelPlayersGameStatisticsNetworkObject.Object == null || _levelPlayersGameStatisticsNetworkObject.Runner == null)
			{
				return false;
			}
			if (_levelPlayersGameStatisticsNetworkObject.Runner.IsSharedModeMasterClient)
			{
				return (_levelPlayersGameStatisticsNetworkObject.Object.Flags & NetworkObjectFlags.MasterClientObject) != 0;
			}
			return false;
		}

		private void HandleAuthoritativeTick()
		{
			if (_hasPendingSlot0PlayerIdPlusOne)
			{
				_hasPendingSlot0PlayerIdPlusOne = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteSlot0PlayerIdPlusOne(_pendingSlot0PlayerIdPlusOne);
			}
			if (_hasPendingSlot0Deaths)
			{
				_hasPendingSlot0Deaths = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteSlot0Deaths(_pendingSlot0Deaths);
			}
			if (_hasPendingSlot0Kills)
			{
				_hasPendingSlot0Kills = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteSlot0Kills(_pendingSlot0Kills);
			}
			if (_hasPendingSlot0Revives)
			{
				_hasPendingSlot0Revives = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteSlot0Revives(_pendingSlot0Revives);
			}
			if (_hasPendingSlot0Cents)
			{
				_hasPendingSlot0Cents = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteSlot0Cents(_pendingSlot0Cents);
			}
			if (_hasPendingSlot1PlayerIdPlusOne)
			{
				_hasPendingSlot1PlayerIdPlusOne = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteSlot1PlayerIdPlusOne(_pendingSlot1PlayerIdPlusOne);
			}
			if (_hasPendingSlot1Deaths)
			{
				_hasPendingSlot1Deaths = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteSlot1Deaths(_pendingSlot1Deaths);
			}
			if (_hasPendingSlot1Kills)
			{
				_hasPendingSlot1Kills = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteSlot1Kills(_pendingSlot1Kills);
			}
			if (_hasPendingSlot1Revives)
			{
				_hasPendingSlot1Revives = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteSlot1Revives(_pendingSlot1Revives);
			}
			if (_hasPendingSlot1Cents)
			{
				_hasPendingSlot1Cents = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteSlot1Cents(_pendingSlot1Cents);
			}
			if (_hasPendingSlot2PlayerIdPlusOne)
			{
				_hasPendingSlot2PlayerIdPlusOne = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteSlot2PlayerIdPlusOne(_pendingSlot2PlayerIdPlusOne);
			}
			if (_hasPendingSlot2Deaths)
			{
				_hasPendingSlot2Deaths = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteSlot2Deaths(_pendingSlot2Deaths);
			}
			if (_hasPendingSlot2Kills)
			{
				_hasPendingSlot2Kills = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteSlot2Kills(_pendingSlot2Kills);
			}
			if (_hasPendingSlot2Revives)
			{
				_hasPendingSlot2Revives = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteSlot2Revives(_pendingSlot2Revives);
			}
			if (_hasPendingSlot2Cents)
			{
				_hasPendingSlot2Cents = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteSlot2Cents(_pendingSlot2Cents);
			}
			if (_hasPendingSlot3PlayerIdPlusOne)
			{
				_hasPendingSlot3PlayerIdPlusOne = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteSlot3PlayerIdPlusOne(_pendingSlot3PlayerIdPlusOne);
			}
			if (_hasPendingSlot3Deaths)
			{
				_hasPendingSlot3Deaths = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteSlot3Deaths(_pendingSlot3Deaths);
			}
			if (_hasPendingSlot3Kills)
			{
				_hasPendingSlot3Kills = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteSlot3Kills(_pendingSlot3Kills);
			}
			if (_hasPendingSlot3Revives)
			{
				_hasPendingSlot3Revives = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteSlot3Revives(_pendingSlot3Revives);
			}
			if (_hasPendingSlot3Cents)
			{
				_hasPendingSlot3Cents = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteSlot3Cents(_pendingSlot3Cents);
			}
			if (_hasPendingCouldroneCount)
			{
				_hasPendingCouldroneCount = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteCouldroneCount(_pendingCouldroneCount);
			}
			if (_hasPendingCartCount)
			{
				_hasPendingCartCount = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteCartCount(_pendingCartCount);
			}
			if (_hasPendingDeadPartCount)
			{
				_hasPendingDeadPartCount = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteDeadPartCount(_pendingDeadPartCount);
			}
			if (_hasPendingSpawnedNewItemId)
			{
				_hasPendingSpawnedNewItemId = false;
				_levelPlayersGameStatisticsNetworkObject.TryWriteSpawnedNewItemId(_pendingSpawnedNewItemId);
			}
		}
	}
}
