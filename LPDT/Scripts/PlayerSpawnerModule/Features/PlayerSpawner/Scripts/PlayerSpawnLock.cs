using Fusion.Addons.Physics;
using UnityEngine;

namespace Features.PlayerSpawner.Scripts
{
	public static class PlayerSpawnLock
	{
		private static readonly object Gate = new object();

		private static Vector3? _pendingSpawnPosition;

		private static Quaternion? _pendingSpawnRotation;

		private static bool _isFirstSpawnInProgress;

		private static bool _isReconnectSpawn;

		private static bool _atomicTeleportConsumed;

		private static bool _levelSpawnTeleportSuppressed;

		public static bool IsLevelSpawnTeleportSuppressed
		{
			get
			{
				lock (Gate)
				{
					return _levelSpawnTeleportSuppressed;
				}
			}
		}

		public static bool IsFirstSpawnInProgress
		{
			get
			{
				lock (Gate)
				{
					return _isFirstSpawnInProgress;
				}
			}
		}

		public static bool IsReconnectSpawn
		{
			get
			{
				lock (Gate)
				{
					return _isReconnectSpawn;
				}
			}
		}

		public static void SetLevelSpawnTeleportSuppressed(bool value)
		{
			lock (Gate)
			{
				_levelSpawnTeleportSuppressed = value;
			}
		}

		public static void BeginSpawn(Vector3 position, Quaternion rotation, bool isReconnect)
		{
			lock (Gate)
			{
				_pendingSpawnPosition = position;
				_pendingSpawnRotation = rotation;
				_isFirstSpawnInProgress = true;
				_isReconnectSpawn = isReconnect;
				_atomicTeleportConsumed = false;
			}
		}

		public static void CancelSpawn()
		{
			lock (Gate)
			{
				ResetLockedState();
			}
		}

		public static bool ShouldBlockPositionOverride(bool isForced = false)
		{
			lock (Gate)
			{
				if (!_isFirstSpawnInProgress)
				{
					return false;
				}
				return !isForced;
			}
		}

		public static bool TryConsumePendingSpawn(NetworkRigidbody networkRigidbody, out Vector3 position, out Quaternion rotation)
		{
			lock (Gate)
			{
				if (_atomicTeleportConsumed || !_isFirstSpawnInProgress || !_pendingSpawnPosition.HasValue)
				{
					position = default(Vector3);
					rotation = Quaternion.identity;
					return false;
				}
				position = _pendingSpawnPosition.Value;
				rotation = _pendingSpawnRotation ?? ((networkRigidbody != null) ? networkRigidbody.transform.rotation : Quaternion.identity);
				networkRigidbody?.Teleport(position, rotation);
				_atomicTeleportConsumed = true;
				_isFirstSpawnInProgress = false;
				_isReconnectSpawn = false;
				_pendingSpawnPosition = null;
				_pendingSpawnRotation = null;
				return true;
			}
		}

		public static void CompleteSpawn()
		{
			lock (Gate)
			{
				ResetLockedState();
			}
		}

		private static void ResetLockedState()
		{
			_pendingSpawnPosition = null;
			_pendingSpawnRotation = null;
			_isFirstSpawnInProgress = false;
			_isReconnectSpawn = false;
			_atomicTeleportConsumed = false;
		}
	}
}
