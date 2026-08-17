using System;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.WorldGeneration
{
	public interface IWorldGenerator
	{
		int Seed { get; }

		SeedManager SeedManager { get; }

		bool IsReady { get; }

		bool IsWorldFullyReady { get; }

		Vector3 PlayerSpawnPoint { get; }

		UnityEvent<Vector3> OnPlayerSpawnPointRegistered { get; }

		float TileSize { get; }

		Transform MapMagicRoot { get; }

		Vector3 NetworkOriginShift { get; }

		event Action OnWorldFullyReady;

		event Action<Vector2Int, Terrain> OnTileFullyLoaded;

		event Action<Vector2Int> OnTileUnloaded;

		void SetTerrainObserver(Transform observer);

		void ApplyOriginShift(Vector3 delta);

		void ServerBroadcastOriginShift(Vector3 totalShift);

		bool TryGetTerrainByChunkCoord(Vector2Int chunkCoord, out Terrain terrain);

		Vector2Int GetChunkCoordFromWorldPosition(Vector3 worldPosition);

		bool IsInitialChunk(Vector3 worldPosition);

		void RegisterPendingPoi(UnityEngine.Object poi);

		void NotifyPoiLootComplete(UnityEngine.Object poi);
	}
}
