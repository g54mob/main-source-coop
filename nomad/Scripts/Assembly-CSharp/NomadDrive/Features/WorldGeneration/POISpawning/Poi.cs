using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using EvilCore.Networking;
using NomadDrive.Features.WorldGeneration.ObjectSpawning;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.POISpawning
{
	public class Poi : MonoBehaviour
	{
		[Header("Player Spawn")]
		[SerializeField]
		private Transform playerSpawnPoint;

		private LootSpawnManager[] _lootSpawnManagers;

		public Transform PlayerSpawnPoint => playerSpawnPoint;

		public bool HasPlayerSpawnPoint => playerSpawnPoint != null;

		public bool PlaceNearRoad { get; private set; }

		public float TerrainHeightOffset { get; private set; }

		public bool RoadsideRegrounded { get; private set; }

		public int StableSeedKey { get; private set; }

		public void SetRoadsidePlacement(bool placeNearRoad, float terrainHeightOffset)
		{
			PlaceNearRoad = placeNearRoad;
			TerrainHeightOffset = terrainHeightOffset;
		}

		public void MarkRoadsideRegrounded()
		{
			RoadsideRegrounded = true;
		}

		public void SetStableKey(int stableSeedKey)
		{
			StableSeedKey = stableSeedKey;
		}

		private void Awake()
		{
			_lootSpawnManagers = GetComponentsInChildren<LootSpawnManager>();
			_ = _lootSpawnManagers;
		}

		protected virtual void Start()
		{
			if (_lootSpawnManagers != null)
			{
				SpawnLootsAsync().Forget();
			}
		}

		protected virtual async UniTaskVoid SpawnLootsAsync()
		{
			await UniTask.WaitUntil(() => NetworkSingleton<WorldGenerator>.Instance != null && NetworkSingleton<WorldGenerator>.Instance.IsReady, PlayerLoopTiming.Update, base.destroyCancellationToken);
			IWorldGenerator worldGen = NetworkSingleton<WorldGenerator>.Instance;
			bool gatesLoading = worldGen?.IsInitialChunk(base.transform.position) ?? false;
			if (gatesLoading)
			{
				worldGen.RegisterPendingPoi(this);
			}
			try
			{
				if (PlaceNearRoad && !RoadsideRegrounded)
				{
					using CancellationTokenSource regroundCts = CancellationTokenSource.CreateLinkedTokenSource(base.destroyCancellationToken);
					using (regroundCts.CancelAfterSlim(TimeSpan.FromSeconds(5.0)))
					{
						_ = 1;
						try
						{
							await UniTask.WaitUntil(() => RoadsideRegrounded, PlayerLoopTiming.Update, regroundCts.Token);
						}
						catch (OperationCanceledException)
						{
							if (base.destroyCancellationToken.IsCancellationRequested)
							{
								throw;
							}
						}
					}
				}
				LootPlacementContext placementContext = new LootPlacementContext();
				for (int i = 0; i < _lootSpawnManagers.Length; i++)
				{
					int baseSeed = SeedManager.CombineSeed(StableSeedKey, i);
					await _lootSpawnManagers[i].CreateLootsAsync(baseSeed, placementContext, base.destroyCancellationToken);
					await UniTask.Yield(base.destroyCancellationToken);
				}
			}
			finally
			{
				if (gatesLoading)
				{
					worldGen.NotifyPoiLootComplete(this);
				}
			}
		}
	}
}
