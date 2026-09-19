using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerIdentityModule;
using Features.PlayerPresenceModule;
using Features.PlayerPresenceModule.Networked;
using Features.SessionManagementModule.Models;
using Fusion;
using UnityEngine;

namespace Features.SessionManagementModule.Presence
{
	public sealed class SessionPlayerPresenceService : ISessionPlayerPresence, IDisposable
	{
		private const string SESSION_PLAYER_PREFAB = "SessionPlayerObject";

		private const float OPERATION_TIMEOUT_SECONDS = 4f;

		private const float AUTHORITY_RETRY_SECONDS = 0.5f;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly IPersistentPlayerIdProvider _persistentPlayerIdProvider;

		private readonly SessionPlayerBridge _sessionPlayerBridge;

		private readonly IMultiplayerService _multiplayerService;

		private readonly SessionStateSnapshot _sessionStateSnapshot;

		private readonly NetworkObject _sessionPlayerPrefab;

		private SessionPlayerNetworkObject _bound;

		public SessionPlayerNetworkObject BoundObject => FindOwned(_multiplayerModel.NetworkRunner, _persistentPlayerIdProvider.LocalId, requireAuthority: false);

		public bool IsAttached => IsBoundToOwn(_persistentPlayerIdProvider.LocalId);

		public int PersistedLifeState
		{
			get
			{
				SessionPlayerNetworkObject boundObject = BoundObject;
				if (!(boundObject != null))
				{
					return 0;
				}
				return boundObject.LifeState;
			}
			set
			{
				SessionPlayerNetworkObject boundObject = BoundObject;
				if (!(boundObject == null) && boundObject.HasStateAuthority && boundObject.LifeState != value)
				{
					SessionPlayerProfileData currentProfile = boundObject.CurrentProfile;
					boundObject.TryWriteProfile(new SessionPlayerProfileData(currentProfile.Nickname, currentProfile.ColorId, currentProfile.CosmeticsMask, currentProfile.StatusMask, value));
				}
			}
		}

		public SessionReconnectState ReconnectState
		{
			get
			{
				SessionPlayerNetworkObject boundObject = BoundObject;
				if (boundObject == null || !boundObject.HasSavedReconnectState)
				{
					return default(SessionReconnectState);
				}
				return new SessionReconnectState(boundObject.SavedLevelId, new Vector3(boundObject.SavedPosX, boundObject.SavedPosY, boundObject.SavedPosZ), boundObject.SavedYaw, boundObject.SavedIsCrouching, boundObject.SavedHealth);
			}
		}

		public async UniTask WaitForReplicatedStateAsync()
		{
			if (!(await WaitForConditionAsync(() => PersistedLifeState != 0)))
			{
				Debug.LogWarning($"SessionPlayer: reclaimed object state for identity {_persistentPlayerIdProvider.LocalId} " + "did not replicate within the timeout — persisted life/placement may read stale.");
			}
		}

		public void SaveReconnectState(int levelId, Vector3 position, float yaw, bool isCrouching, float health)
		{
			SessionPlayerNetworkObject boundObject = BoundObject;
			if (!(boundObject == null) && boundObject.HasStateAuthority)
			{
				boundObject.TryWriteReconnectState(levelId, position.x, position.y, position.z, yaw, isCrouching, health);
			}
		}

		public void SaveShopVote(int voteEpoch, bool hasVoted)
		{
			SessionPlayerNetworkObject boundObject = BoundObject;
			if (!(boundObject == null) && boundObject.HasStateAuthority)
			{
				boundObject.TryWriteShopVote(voteEpoch, hasVoted);
			}
		}

		public bool HasShopVoteForEpoch(int currentEpoch)
		{
			SessionPlayerNetworkObject boundObject = BoundObject;
			if (boundObject != null && (bool)boundObject.HasVotedToLeaveShop)
			{
				return boundObject.ShopVoteEpoch == currentEpoch;
			}
			return false;
		}

		public SessionPlayerPresenceService(MultiplayerModel multiplayerModel, IPersistentPlayerIdProvider persistentPlayerIdProvider, SessionPlayerBridge sessionPlayerBridge, IMultiplayerService multiplayerService, SessionStateSnapshot sessionStateSnapshot)
		{
			_multiplayerModel = multiplayerModel;
			_persistentPlayerIdProvider = persistentPlayerIdProvider;
			_sessionPlayerBridge = sessionPlayerBridge;
			_multiplayerService = multiplayerService;
			_sessionStateSnapshot = sessionStateSnapshot;
			_sessionPlayerPrefab = Resources.Load<NetworkObject>("SessionPlayerObject");
		}

		public async UniTask PinAsync()
		{
			NetworkRunner runner = _multiplayerModel.NetworkRunner;
			PersistentPlayerId localId = _persistentPlayerIdProvider.LocalId;
			if (FindOwned(runner, localId, requireAuthority: false) != null)
			{
				Debug.LogError($"SessionPlayer: Pin Async - pin already exists for identity {localId}. Requesting to create Pin is not expected, and indication that previous object was not disposed.");
				return;
			}
			NetworkObject spawned = null;
			try
			{
				spawned = await runner.SpawnAsync(_sessionPlayerPrefab);
				if (spawned == null || !spawned.TryGetComponent<SessionPlayerNetworkObject>(out var component) || !component.TryClaimOwnership(localId))
				{
					throw new InvalidOperationException($"SessionPlayer pin could not spawn or claim an object for identity {localId}.");
				}
				component.TryWriteCosmeticSeed(GenerateCosmeticSeed());
				await WaitUntilAsync(() => FindOwned(runner, localId, requireAuthority: true) != null, $"SessionPlayer pin never confirmed a state-authoritative object for identity {localId}.");
			}
			catch
			{
				if (spawned != null && spawned.IsValid && spawned.HasStateAuthority)
				{
					runner.Despawn(spawned);
				}
				throw;
			}
		}

		public async UniTask AttachAsync()
		{
			NetworkRunner runner = _multiplayerModel.NetworkRunner;
			PersistentPlayerId localId = _persistentPlayerIdProvider.LocalId;
			if (!IsBoundToOwn(localId))
			{
				if (!(await WaitForConditionAsync(() => FindOwned(runner, localId, requireAuthority: false) != null)))
				{
					Debug.LogWarning($"SessionPlayer: no persistent object for identity {localId} in a locked session — self-disconnecting (§K7 second layer / §M5).");
					_sessionStateSnapshot.MarkRosterRejected();
					_multiplayerService.Shutdown(runner, ShutdownReason.Ok).Forget();
					throw new SessionRosterRejectedException($"No persistent player object to attach for identity {localId} — not part of the locked roster (§K7/§M5).");
				}
				await WaitUntilAsync(() => TryEnsureAuthority(runner, localId), $"SessionPlayer attach never secured a state-authoritative object for identity {localId}.");
				_bound = FindOwned(runner, localId, requireAuthority: true);
				_sessionPlayerBridge.Bind(_bound);
			}
		}

		public async UniTask ReleaseAllAsync()
		{
			NetworkRunner runner = _multiplayerModel.NetworkRunner;
			if (runner == null || !runner.IsRunning || !runner.IsSharedModeMasterClient)
			{
				return;
			}
			HashSet<NetworkObject> despawnRequested = new HashSet<NetworkObject>();
			Dictionary<NetworkObject, float> nextAuthorityRequest = new Dictionary<NetworkObject, float>();
			float deadline = Time.realtimeSinceStartup + 4f;
			while (true)
			{
				List<SessionPlayerNetworkObject> list = CollectAll(runner);
				if (list.Count == 0)
				{
					_bound = null;
					return;
				}
				foreach (SessionPlayerNetworkObject item in list)
				{
					NetworkObject networkObject = item.Object;
					if (!despawnRequested.Contains(networkObject))
					{
						float value;
						if (networkObject.HasStateAuthority)
						{
							runner.Despawn(networkObject);
							despawnRequested.Add(networkObject);
						}
						else if (!nextAuthorityRequest.TryGetValue(networkObject, out value) || Time.realtimeSinceStartup >= value)
						{
							networkObject.RequestStateAuthority();
							nextAuthorityRequest[networkObject] = Time.realtimeSinceStartup + 0.5f;
						}
					}
				}
				if (Time.realtimeSinceStartup > deadline)
				{
					break;
				}
				await UniTask.Yield();
			}
			_bound = null;
			throw new Exception("SessionPlayer.ReleaseAllAsync: failed to release the persistent player roster.");
		}

		public void Dispose()
		{
			_sessionPlayerBridge.Unbind();
		}

		private static int GenerateCosmeticSeed()
		{
			int hashCode = Guid.NewGuid().GetHashCode();
			if (hashCode != 0)
			{
				return hashCode;
			}
			return 1;
		}

		private bool TryEnsureAuthority(NetworkRunner runner, PersistentPlayerId localId)
		{
			SessionPlayerNetworkObject sessionPlayerNetworkObject = FindOwned(runner, localId, requireAuthority: false);
			if (sessionPlayerNetworkObject == null)
			{
				return false;
			}
			if (sessionPlayerNetworkObject.HasStateAuthority)
			{
				return true;
			}
			sessionPlayerNetworkObject.TryReclaim(localId);
			return false;
		}

		private bool IsBoundToOwn(PersistentPlayerId localId)
		{
			if (_bound != null && _bound.Object != null && _bound.Object.IsValid && _bound.HasStateAuthority)
			{
				return _bound.OwnerIdValue.Equals(localId);
			}
			return false;
		}

		private SessionPlayerNetworkObject FindOwned(NetworkRunner runner, PersistentPlayerId ownerId, bool requireAuthority)
		{
			foreach (NetworkObject allNetworkObject in runner.GetAllNetworkObjects())
			{
				if (!(allNetworkObject == null) && allNetworkObject.IsValid && allNetworkObject.TryGetComponent<SessionPlayerNetworkObject>(out var component) && component.OwnerIdValue.Equals(ownerId) && (!requireAuthority || component.HasStateAuthority))
				{
					return component;
				}
			}
			return null;
		}

		private List<SessionPlayerNetworkObject> CollectAll(NetworkRunner runner)
		{
			List<SessionPlayerNetworkObject> list = new List<SessionPlayerNetworkObject>();
			foreach (NetworkObject allNetworkObject in runner.GetAllNetworkObjects())
			{
				if (!(allNetworkObject == null) && allNetworkObject.IsValid && allNetworkObject.TryGetComponent<SessionPlayerNetworkObject>(out var component))
				{
					list.Add(component);
				}
			}
			return list;
		}

		private async UniTask WaitUntilAsync(Func<bool> condition, string timeoutMessage)
		{
			float deadline = Time.realtimeSinceStartup + 4f;
			while (!condition())
			{
				if (Time.realtimeSinceStartup > deadline)
				{
					throw new TimeoutException(timeoutMessage);
				}
				await UniTask.Yield();
			}
		}

		private async UniTask<bool> WaitForConditionAsync(Func<bool> condition)
		{
			float deadline = Time.realtimeSinceStartup + 4f;
			while (!condition())
			{
				if (Time.realtimeSinceStartup > deadline)
				{
					return false;
				}
				await UniTask.Yield();
			}
			return true;
		}
	}
}
