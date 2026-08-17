using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Den.Tools;
using EvilCore;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Extensions;
using EvilCore.Networking;
using MapMagic.Core;
using MapMagic.Products;
using MapMagic.Terrains;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.EvilRoads;
using NomadDrive.Features.LiquidTransferSystem;
using NomadDrive.Features.Plates;
using NomadDrive.Features.Player;
using NomadDrive.Features.WorldGeneration.ObjectSpawning;
using NomadDrive.Features.WorldGeneration.POISpawning;
using NomadDrive.Features.WorldGeneration.RoadGeneration;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Serialization;
using VContainer;

namespace NomadDrive.Features.WorldGeneration
{
	public class WorldGenerator : NetworkSingleton<WorldGenerator>, IWorldGenerator
	{
		[Header("Seed Settings")]
		[SyncVar]
		[SerializeField]
		private int seed;

		[SyncVar]
		private Vector3 _networkOriginShift;

		[Header("MapMagic Settings")]
		[SerializeField]
		private MapMagicObjectConfig mapMagicObjectConfig;

		[FormerlySerializedAs("divisionRoadsManager")]
		[Header("Road Generation Settings")]
		[SerializeField]
		private EvilRoadsManager evilRoadsManager;

		[SerializeField]
		private RoadGenerationConfig roadGenerationConfig;

		[Header("Debug")]
		[SerializeField]
		private List<TileInfo> debugTileInfoList = new List<TileInfo>();

		private readonly Dictionary<Coord, TileInfo> _activeTileInfosDict = new Dictionary<Coord, TileInfo>();

		private readonly HashSet<Coord> _loadedChunks = new HashSet<Coord>();

		private readonly HashSet<Coord> _poiProcessedChunks = new HashSet<Coord>();

		private ChunkPoiManager _chunkPoiManager;

		private readonly HashSet<int> _spawnedLootSeeds = new HashSet<int>();

		private readonly LootSpawningService _lootSpawningService = new LootSpawningService();

		private RoadGenerationCoordinator _roadCoordinator;

		private bool _isProcessingChunks;

		private const float ReconcilePollIntervalSeconds = 1f;

		private float _reconcilePollTimer;

		private const float WorldReadyTimeoutSeconds = 30f;

		private const float WorldReadyHardSafetySeconds = 45f;

		private readonly HashSet<Coord> _initialChunkSet = new HashSet<Coord>();

		private readonly HashSet<Coord> _pendingInitialTerrain = new HashSet<Coord>();

		private readonly HashSet<UnityEngine.Object> _pendingPoiLoot = new HashSet<UnityEngine.Object>();

		private bool _initialChunksLocked;

		private bool _worldReadyFired;

		private bool _lootStateAnnounced;

		private Coroutine _worldReadyTimeoutCoroutine;

		private Coroutine _worldReadyHardSafetyCoroutine;

		[Inject]
		private IGameLoadingManager _loadingManager;

		[Inject]
		private IPlayerService _playerService;

		[Inject]
		private IEOSLobbyManager _lobbyManager;

		[Inject]
		private IWorldSeedProvider _worldSeedProvider;

		[Inject]
		private IGameSaveService _gameSaveService;

		private MapMagicObject _mapMagic;

		private bool _playerBound;

		private Transform _terrainObserverOverride;

		private bool _hostInitialSpawnDone;

		public int Seed => seed;

		public SeedManager SeedManager { get; private set; }

		public bool IsReady { get; private set; }

		[Header("Spawn Point")]
		[field: SerializeField]
		public Vector3 PlayerSpawnPoint { get; private set; }

		public UnityEvent<Vector3> OnPlayerSpawnPointRegistered { get; set; } = new UnityEvent<Vector3>();

		public float TileSize
		{
			get
			{
				if (!(mapMagicObjectConfig != null))
				{
					return 512f;
				}
				return mapMagicObjectConfig.tileSize.x;
			}
		}

		public Transform MapMagicRoot
		{
			get
			{
				if (!(_mapMagic != null))
				{
					return null;
				}
				return _mapMagic.transform;
			}
		}

		public Vector3 NetworkOriginShift => _networkOriginShift;

		public bool IsWorldFullyReady { get; private set; }

		public int Networkseed
		{
			get
			{
				return seed;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref seed, 1uL, null);
			}
		}

		public Vector3 Network_networkOriginShift
		{
			get
			{
				return _networkOriginShift;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _networkOriginShift, 2uL, null);
			}
		}

		public event Action<Vector2Int, Terrain> OnTileFullyLoaded;

		public event Action<Vector2Int> OnTileUnloaded;

		public event Action OnWorldFullyReady;

		public void ApplyOriginShift(Vector3 delta)
		{
			PlayerSpawnPoint += delta;
			foreach (TileInfo value in _activeTileInfosDict.Values)
			{
				value.roadStartPointX += delta.x;
				value.roadEndPointX += delta.x;
			}
			_chunkPoiManager?.ApplyOriginShift(delta);
		}

		public void ServerBroadcastOriginShift(Vector3 totalShift)
		{
			if (base.isServer)
			{
				Network_networkOriginShift = totalShift;
			}
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			if (!TryGetComponent<ChunkPoiManager>(out _chunkPoiManager))
			{
				EvilLogger.LogError("<color=red>[WorldGenerator]</color> ChunkPOISpawner component not found on WorldGenerator!", "OnStartServer", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\WorldGenerator.cs", 176);
			}
			InitializeSeedManager();
			if (_chunkPoiManager != null && SeedManager != null)
			{
				float roadWidth = ((evilRoadsManager != null) ? evilRoadsManager.GetRoadWidth() : 10f);
				_chunkPoiManager.Initialize(SeedManager, (roadGenerationConfig != null) ? roadGenerationConfig.roadCurveAmplitude : 30f, (roadGenerationConfig != null) ? roadGenerationConfig.roadCurveFrequency : 0.005f, roadGenerationConfig == null || roadGenerationConfig.usePerlinNoise, roadWidth, roadGenerationConfig == null || roadGenerationConfig.useMeander, (roadGenerationConfig != null) ? roadGenerationConfig.meanderOctaves : 3, (roadGenerationConfig != null) ? roadGenerationConfig.meanderBaseAmplitude : 120f, (roadGenerationConfig != null) ? roadGenerationConfig.meanderBaseFrequency : 0.0015f, (roadGenerationConfig != null) ? roadGenerationConfig.meanderPersistence : 0.45f, (roadGenerationConfig != null) ? roadGenerationConfig.meanderLacunarity : 2.3f, (roadGenerationConfig != null) ? roadGenerationConfig.maxMeanderAmplitude : 200f, SeedManager.GetSubSeed("RoadMeander"));
			}
			IsReady = true;
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			if (_loadingManager != null)
			{
				NomadDriveLoadingStates.RegisterAll(_loadingManager);
			}
			_loadingManager?.SetState(10);
			_loadingManager?.SetState(30);
			if (!base.isServer)
			{
				InitializeSeedManager();
			}
			if (!base.isServer)
			{
				if (!TryGetComponent<ChunkPoiManager>(out _chunkPoiManager))
				{
					EvilLogger.LogError("<color=red>[WorldGenerator.Client]</color> ChunkPOISpawner component not found on WorldGenerator!", "OnStartClient", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\WorldGenerator.cs", 222);
				}
				if (_chunkPoiManager != null && SeedManager != null)
				{
					float roadWidth = ((evilRoadsManager != null) ? evilRoadsManager.GetRoadWidth() : 10f);
					_chunkPoiManager.Initialize(SeedManager, (roadGenerationConfig != null) ? roadGenerationConfig.roadCurveAmplitude : 30f, (roadGenerationConfig != null) ? roadGenerationConfig.roadCurveFrequency : 0.005f, roadGenerationConfig == null || roadGenerationConfig.usePerlinNoise, roadWidth, roadGenerationConfig == null || roadGenerationConfig.useMeander, (roadGenerationConfig != null) ? roadGenerationConfig.meanderOctaves : 3, (roadGenerationConfig != null) ? roadGenerationConfig.meanderBaseAmplitude : 120f, (roadGenerationConfig != null) ? roadGenerationConfig.meanderBaseFrequency : 0.0015f, (roadGenerationConfig != null) ? roadGenerationConfig.meanderPersistence : 0.45f, (roadGenerationConfig != null) ? roadGenerationConfig.meanderLacunarity : 2.3f, (roadGenerationConfig != null) ? roadGenerationConfig.maxMeanderAmplitude : 200f, SeedManager.GetSubSeed("RoadMeander"));
				}
			}
			SetupMapmagic(Seed);
			IsReady = true;
			if (_worldReadyHardSafetyCoroutine != null)
			{
				StopCoroutine(_worldReadyHardSafetyCoroutine);
			}
			_worldReadyHardSafetyCoroutine = StartCoroutine(WorldReadyHardSafetyCoroutine());
			if (_chunkPoiManager != null)
			{
				WorldPrefabCache.Preload(_chunkPoiManager.GetAllPoiGuids());
			}
			if (base.isServer)
			{
				WorldPrefabCache.PreloadByLabel("Loot");
			}
			if (_playerService != null)
			{
				if (_playerService.IsPlayerSpawned)
				{
					BindPlayerToMapMagic();
				}
				else
				{
					_playerService.OnPlayerRegistered += BindPlayerToMapMagic;
				}
				_playerService.OnPlayerCleared += TeardownMapMagicBinding;
			}
			_roadCoordinator = new RoadGenerationCoordinator(evilRoadsManager, roadGenerationConfig, SeedManager, _activeTileInfosDict, _chunkPoiManager);
			if (evilRoadsManager != null && evilRoadsManager.IsDynamicUpgradeEnabled() && evilRoadsManager.IsLazyLoadingEnabled())
			{
				StartCoroutine(_roadCoordinator.DynamicRoadUpgradeCoroutine());
			}
		}

		public override void OnStopClient()
		{
			base.OnStopClient();
			TeardownMapMagicBinding();
			_worldSeedProvider?.Clear();
			TerrainColliderRefreshScheduler.Clear();
			MainThreadWorkBudget.Clear();
			WorldPrefabCache.ReleaseAll();
		}

		private void InitializeSeedManager()
		{
			SeedManager = new SeedManager();
			if (base.isServer && seed == 0)
			{
				if (_gameSaveService != null && _gameSaveService.IsLoadedWorld && _gameSaveService.LoadedSeed > 0)
				{
					Networkseed = _gameSaveService.LoadedSeed;
					if (_gameSaveService.LoadedLootSeeds != null)
					{
						foreach (int loadedLootSeed in _gameSaveService.LoadedLootSeeds)
						{
							_spawnedLootSeeds.Add(loadedLootSeed);
						}
					}
				}
				else
				{
					int num = _lobbyManager?.PendingWorldSeed ?? 0;
					Networkseed = ((num > 0) ? num : UnityEngine.Random.Range(1, 10000000));
				}
			}
			SeedManager.SetSeed(seed);
			_worldSeedProvider?.SetSeed(seed);
		}

		private void Update()
		{
			ReconcileUnprocessedChunks();
		}

		private void ReconcileUnprocessedChunks()
		{
			if (_isProcessingChunks)
			{
				return;
			}
			_reconcilePollTimer += Time.unscaledDeltaTime;
			if (_reconcilePollTimer < 1f)
			{
				return;
			}
			_reconcilePollTimer = 0f;
			foreach (KeyValuePair<Coord, TileInfo> item in _activeTileInfosDict)
			{
				if (item.Key.x == 0 && !_poiProcessedChunks.Contains(item.Key))
				{
					OnAllCompleteAsync().Forget();
					break;
				}
			}
		}

		private void SetupMapmagic(int seed = 0)
		{
			GameObject gameObject = new GameObject("GameWorld", typeof(MapMagicObject));
			_mapMagic = gameObject.GetComponent<MapMagicObject>();
			_mapMagic.graph = mapMagicObjectConfig.graph;
			_mapMagic.graph.random = new Noise(seed, 32768);
			_mapMagic.mainRange = mapMagicObjectConfig.mainRange;
			_mapMagic.tiles.generateRange = mapMagicObjectConfig.generateRange;
			_mapMagic.tiles.generateInfinite = mapMagicObjectConfig.generateInfinite;
			_mapMagic.tiles.retainMargin = mapMagicObjectConfig.retainMargin;
			_mapMagic.tiles.genAroundMainCam = mapMagicObjectConfig.genAroundMainCam;
			_mapMagic.tileResolution = mapMagicObjectConfig.tileResolution;
			_mapMagic.tileSize = mapMagicObjectConfig.tileSize;
			if (mapMagicObjectConfig.terrainMaterial != null)
			{
				_mapMagic.terrainSettings.material = mapMagicObjectConfig.terrainMaterial;
			}
			_mapMagic.tiles.genAroundTfms = true;
			_mapMagic.tiles.genAroundTfmsList = Array.Empty<Transform>();
			_mapMagic.Refresh();
		}

		private void BindPlayerToMapMagic()
		{
			if (!(_mapMagic == null) && !_playerBound && _playerService != null && _playerService.IsPlayerSpawned && !(_playerService.LocalPlayer == null))
			{
				ApplyMapMagicFollow(refresh: true);
				_playerBound = true;
			}
		}

		private void ApplyMapMagicFollow(bool refresh)
		{
			if (_mapMagic == null)
			{
				return;
			}
			Transform transform = ((_terrainObserverOverride != null) ? _terrainObserverOverride : ((_playerService?.LocalPlayer != null) ? _playerService.LocalPlayer.transform : null));
			if (transform == null)
			{
				_mapMagic.tiles.genAroundTfmsList = Array.Empty<Transform>();
				return;
			}
			_mapMagic.tiles.genAroundTfms = true;
			_mapMagic.tiles.genAroundTfmsList = new Transform[1] { transform };
			_mapMagic.tiles.genAroundMainCam = false;
			if (refresh)
			{
				_mapMagic.Refresh();
			}
		}

		public void SetTerrainObserver(Transform observer)
		{
			_terrainObserverOverride = observer;
			ApplyMapMagicFollow(refresh: false);
		}

		private void TeardownMapMagicBinding()
		{
			if (_playerService != null)
			{
				_playerService.OnPlayerRegistered -= BindPlayerToMapMagic;
				_playerService.OnPlayerCleared -= TeardownMapMagicBinding;
			}
			_terrainObserverOverride = null;
			if (_mapMagic != null)
			{
				_mapMagic.tiles.genAroundTfms = false;
				_mapMagic.tiles.genAroundTfmsList = Array.Empty<Transform>();
				if (_mapMagic.gameObject != null)
				{
					_mapMagic.gameObject.SetActive(value: false);
				}
			}
			_playerBound = false;
		}

		private void Start()
		{
			TerrainTile.OnTileMoved = (Action<TerrainTile>)Delegate.Combine(TerrainTile.OnTileMoved, new Action<TerrainTile>(OnTileMoved));
			TerrainTile.OnTileApplied = (Action<TerrainTile, TileData, StopToken>)Delegate.Combine(TerrainTile.OnTileApplied, new Action<TerrainTile, TileData, StopToken>(OnTileApplied));
			TerrainTile.OnAllComplete = (Action<MapMagicObject>)Delegate.Combine(TerrainTile.OnAllComplete, new Action<MapMagicObject>(OnAllComplete));
		}

		private new void OnDestroy()
		{
			TerrainTile.OnTileMoved = (Action<TerrainTile>)Delegate.Remove(TerrainTile.OnTileMoved, new Action<TerrainTile>(OnTileMoved));
			TerrainTile.OnTileApplied = (Action<TerrainTile, TileData, StopToken>)Delegate.Remove(TerrainTile.OnTileApplied, new Action<TerrainTile, TileData, StopToken>(OnTileApplied));
			TerrainTile.OnAllComplete = (Action<MapMagicObject>)Delegate.Remove(TerrainTile.OnAllComplete, new Action<MapMagicObject>(OnAllComplete));
			if (_playerService != null)
			{
				_playerService.OnPlayerRegistered -= BindPlayerToMapMagic;
				_playerService.OnPlayerCleared -= TeardownMapMagicBinding;
			}
			if (_worldReadyTimeoutCoroutine != null)
			{
				StopCoroutine(_worldReadyTimeoutCoroutine);
				_worldReadyTimeoutCoroutine = null;
			}
			if (_worldReadyHardSafetyCoroutine != null)
			{
				StopCoroutine(_worldReadyHardSafetyCoroutine);
				_worldReadyHardSafetyCoroutine = null;
			}
		}

		private void OnTileMoved(TerrainTile tile)
		{
			Coord coord = tile.coord;
			Coord? coord2 = null;
			foreach (KeyValuePair<Coord, TileInfo> item in _activeTileInfosDict)
			{
				if (item.Value.tile == tile && item.Key != coord)
				{
					coord2 = item.Key;
					break;
				}
			}
			if (coord2.HasValue)
			{
				TileInfo tileInfo = _activeTileInfosDict[coord2.Value];
				TerrainColliderRefreshScheduler.DropPendingTrees(tile.GetTerrain(isDraft: false));
				if (tileInfo.road != null && _roadCoordinator != null)
				{
					_roadCoordinator.DestroyRoad(tileInfo);
				}
				else
				{
					_ = tileInfo.road != null;
				}
				ChunkPoiManager.ProcessChunkForUnloading(_activeTileInfosDict[coord2.Value]);
				_activeTileInfosDict.Remove(coord2.Value);
				_poiProcessedChunks.Remove(coord2.Value);
				_loadedChunks.Remove(coord2.Value);
				try
				{
					this.OnTileUnloaded?.Invoke(new Vector2Int(coord2.Value.x, coord2.Value.z));
				}
				catch (Exception ex)
				{
					EvilLogger.LogError($"<color=red>[WorldGenerator]</color> OnTileUnloaded handler failed for chunk {coord2.Value}: {ex.Message}", "OnTileMoved", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\WorldGenerator.cs", 572);
				}
			}
		}

		private void OnTileApplied(TerrainTile tile, TileData data, StopToken token)
		{
			if (!data.isDraft)
			{
				Coord coord = tile.coord;
				if (_activeTileInfosDict.ContainsKey(coord))
				{
					_activeTileInfosDict[coord].Data = data;
					_activeTileInfosDict[coord].tile = tile;
				}
				else
				{
					_activeTileInfosDict[coord] = new TileInfo
					{
						tile = tile,
						Data = data
					};
					_loadedChunks.Add(coord);
				}
				Terrain terrain = tile.GetTerrain(isDraft: false);
				if (terrain != null)
				{
					WorldGenerationLayerUtility.ApplyLayerSafe(terrain.gameObject, (mapMagicObjectConfig != null) ? mapMagicObjectConfig.terrainLayer : null);
				}
			}
		}

		private void OnAllComplete(MapMagicObject mapMagicObject)
		{
			OnAllCompleteAsync().Forget();
		}

		private async UniTask OnAllCompleteAsync()
		{
			try
			{
				await ProcessChunksForPoiSpawning();
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("<color=red>[WorldGenerator]</color> OnAllCompleteAsync failed: " + ex.Message + "\n" + ex.StackTrace, "OnAllCompleteAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\WorldGenerator.cs", 634);
				_isProcessingChunks = false;
				CheckWorldReadiness();
			}
		}

		private async UniTaskVoid RetryProcessingAfterDelayAsync()
		{
			try
			{
				await UniTask.Delay(TimeSpan.FromMilliseconds(250.0), ignoreTimeScale: false, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
			}
			catch (OperationCanceledException)
			{
				return;
			}
			await OnAllCompleteAsync();
		}

		private async UniTask ProcessChunksForPoiSpawning()
		{
			if (_chunkPoiManager == null || _isProcessingChunks)
			{
				return;
			}
			HashSet<Coord> hashSet = new HashSet<Coord>(_loadedChunks);
			foreach (KeyValuePair<Coord, TileInfo> item in _activeTileInfosDict)
			{
				if (item.Key.x == 0 && !_poiProcessedChunks.Contains(item.Key))
				{
					hashSet.Add(item.Key);
				}
			}
			if (hashSet.Count == 0)
			{
				if (_loadingManager != null && !_loadingManager.IsLoadingComplete())
				{
					CheckWorldReadiness();
				}
				return;
			}
			_isProcessingChunks = true;
			List<Coord> chunksToProcess = hashSet.ToList();
			LockInitialChunksIfNeeded(chunksToProcess);
			SetLoadingState(40);
			int poiCount = 0;
			TileInfo tileInfo;
			foreach (Coord coord in chunksToProcess)
			{
				if (coord.x != 0 || !_activeTileInfosDict.TryGetValue(coord, out tileInfo))
				{
					continue;
				}
				Terrain terrain = tileInfo.tile.GetTerrain(isDraft: false);
				if (!(terrain == null) && !(terrain.terrainData == null))
				{
					if (!terrain.TryGetComponent<SurfaceTypeComponent>(out var _))
					{
						terrain.gameObject.AddComponent<SurfaceTypeComponent>().surfaceType = SurfaceType.Sand;
					}
					WorldGenerationTagUtility.ApplyTagSafe(terrain.gameObject, (mapMagicObjectConfig != null) ? mapMagicObjectConfig.terrainTag : null);
					if (poiCount > 0)
					{
						await UniTask.DelayFrame(2);
					}
					await UniTask.Yield();
					await _chunkPoiManager.ProcessChunkForLoading(tileInfo);
					_poiProcessedChunks.Add(coord);
					poiCount++;
					tileInfo = null;
				}
			}
			SetLoadingState(50);
			int roadCount = 0;
			foreach (Coord item2 in chunksToProcess)
			{
				if (item2.x != 0 || !_activeTileInfosDict.TryGetValue(item2, out tileInfo) || tileInfo.isRoadGenerated)
				{
					continue;
				}
				Terrain terrain2 = tileInfo.tile?.GetTerrain(isDraft: false);
				if (!(terrain2 == null) && !(terrain2.terrainData == null))
				{
					if (roadCount > 0)
					{
						await UniTask.DelayFrame(UnityEngine.Random.Range(1, 4));
					}
					if (_roadCoordinator != null)
					{
						await _roadCoordinator.CreateRoadAsync(tileInfo);
					}
					roadCount++;
					tileInfo = null;
				}
			}
			_ = roadCount;
			_ = 1;
			await UniTask.Yield();
			await TerrainColliderRefreshScheduler.FlushAllAsync();
			foreach (Coord item3 in chunksToProcess)
			{
				if (item3.x != 0 || !_activeTileInfosDict.TryGetValue(item3, out var value))
				{
					continue;
				}
				Terrain terrain3 = value.tile?.GetTerrain(isDraft: false);
				if (!(terrain3 == null) && !(terrain3.terrainData == null))
				{
					try
					{
						ReGroundRoadsidePois(value, terrain3);
					}
					catch (Exception ex)
					{
						EvilLogger.LogError($"<color=red>[WorldGenerator]</color> ReGroundRoadsidePois failed for chunk ({item3.x}, {item3.z}): {ex.Message}", "ProcessChunksForPoiSpawning", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\WorldGenerator.cs", 838);
					}
				}
			}
			foreach (Coord item4 in chunksToProcess)
			{
				if (!_activeTileInfosDict.TryGetValue(item4, out var value2))
				{
					continue;
				}
				Terrain terrain4 = value2.tile?.GetTerrain(isDraft: false);
				if (!(terrain4 == null) && !(terrain4.terrainData == null))
				{
					Vector2Int vector2Int = new Vector2Int(item4.x, item4.z);
					try
					{
						this.OnTileFullyLoaded?.Invoke(vector2Int, terrain4);
					}
					catch (Exception ex2)
					{
						EvilLogger.LogError($"<color=red>[WorldGenerator]</color> OnTileFullyLoaded handler failed for chunk {vector2Int}: {ex2.Message}", "ProcessChunksForPoiSpawning", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\WorldGenerator.cs", 860);
					}
					if (_pendingInitialTerrain.Remove(item4))
					{
						CheckWorldReadiness();
					}
				}
			}
			foreach (Coord item5 in chunksToProcess)
			{
				_loadedChunks.Remove(item5);
			}
			_isProcessingChunks = false;
			CheckWorldReadiness();
			bool flag = false;
			foreach (KeyValuePair<Coord, TileInfo> item6 in _activeTileInfosDict)
			{
				if (item6.Key.x == 0 && !_poiProcessedChunks.Contains(item6.Key))
				{
					flag = true;
					break;
				}
			}
			if (_loadedChunks.Count > 0)
			{
				OnAllCompleteAsync().Forget();
			}
			else if (flag)
			{
				RetryProcessingAfterDelayAsync().Forget();
			}
		}

		private static void ReGroundRoadsidePois(TileInfo tileInfo, Terrain terrain)
		{
			if (tileInfo.spawnedPois == null)
			{
				return;
			}
			foreach (Poi spawnedPoi in tileInfo.spawnedPois)
			{
				if (!(spawnedPoi == null) && spawnedPoi.PlaceNearRoad)
				{
					Vector3 position = spawnedPoi.transform.position;
					float num = RoadHeightCalculator.GetTerrainHeight(position, terrain) - spawnedPoi.TerrainHeightOffset;
					if (!Mathf.Approximately(position.y, num))
					{
						spawnedPoi.transform.position = new Vector3(position.x, num, position.z);
					}
					spawnedPoi.MarkRoadsideRegrounded();
				}
			}
		}

		public bool TryReserveLootSpawn(int seed)
		{
			if (_gameSaveService != null && _gameSaveService.IsLoadedWorld && !_gameSaveService.HasLoadedLootLedger)
			{
				return false;
			}
			return _spawnedLootSeeds.Add(seed);
		}

		public void UnregisterLootSpawn(int seed)
		{
			if (_gameSaveService == null || !_gameSaveService.IsLoadedWorld || _gameSaveService.HasLoadedLootLedger)
			{
				_spawnedLootSeeds.Remove(seed);
			}
		}

		public IReadOnlyCollection<int> GetSpawnedLootSeeds()
		{
			return _spawnedLootSeeds;
		}

		public void RegisterPlayerSpawnPosition(Vector3 position)
		{
			PlayerSpawnPoint = position;
			OnPlayerSpawnPointRegistered.Invoke(position);
			bool flag = _gameSaveService == null || !_gameSaveService.IsLoadedWorld;
			if (NetworkServer.active && NetworkClient.localPlayer != null && !_hostInitialSpawnDone && flag)
			{
				_hostInitialSpawnDone = true;
				NetworkClient.localPlayer.GetComponent<NomadDrive.Features.Player.Player>().SetPositionAndRotation(PlayerSpawnPoint);
			}
		}

		[Command(requiresAuthority = false)]
		public void CmdRequestLootSpawn(Vector3 lootSpawnPosition, int seed, LootSpawnDataNetwork[] lootSpawnDatas, NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(lootSpawnPosition);
			writer.WriteVarInt(seed);
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EWorldGeneration_002EObjectSpawning_002ELootSpawnDataNetwork_005B_005D(writer, lootSpawnDatas);
			SendCommandInternal("System.Void NomadDrive.Features.WorldGeneration.WorldGenerator::CmdRequestLootSpawn(UnityEngine.Vector3,System.Int32,NomadDrive.Features.WorldGeneration.ObjectSpawning.LootSpawnDataNetwork[],Mirror.NetworkConnectionToClient)", -1257937474, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private async UniTask SpawnLootsOnServer(Vector3 position, int seed, LootSpawnDataNetwork[] lootSpawnDatas)
		{
			LootPlacementContext context = new LootPlacementContext();
			GameObject gameObject = await _lootSpawningService.SpawnLootDeterministicallyFromNetwork(position, seed, lootSpawnDatas, Quaternion.identity, context);
			if (gameObject != null)
			{
				LootLifecycleManager.Instance?.ServerTrackWildLoot(gameObject, seed);
				if (gameObject.TryGetComponent<ConditionComponent>(out var component))
				{
					component.ServerInitializeCondition(seed);
				}
				if (gameObject.TryGetComponent<Plate>(out var component2))
				{
					component2.ServerInitializeFromSeed(seed);
				}
			}
			else
			{
				_spawnedLootSeeds.Remove(seed);
			}
		}

		[Command(requiresAuthority = false)]
		public void CmdDebugSpawnLootByGuid(string guid, Vector3 position, NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(guid);
			writer.WriteVector3(position);
			SendCommandInternal("System.Void NomadDrive.Features.WorldGeneration.WorldGenerator::CmdDebugSpawnLootByGuid(System.String,UnityEngine.Vector3,Mirror.NetworkConnectionToClient)", 383269458, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private async UniTask DebugSpawnLootByGuidAsync(string guid, Vector3 position)
		{
			if (string.IsNullOrEmpty(guid))
			{
				return;
			}
			GameObject gameObject;
			try
			{
				gameObject = await Addressables.LoadAssetAsync<GameObject>(guid).Task;
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("[WorldGenerator] Failed to load loot prefab for guid " + guid + ": " + ex.Message, "DebugSpawnLootByGuidAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\WorldGenerator.cs", 1084);
				return;
			}
			if (gameObject == null)
			{
				EvilLogger.LogError("[WorldGenerator] Loot prefab not found for guid " + guid + ".", "DebugSpawnLootByGuidAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\WorldGenerator.cs", 1090);
				return;
			}
			GameObject instance = UnityEngine.Object.Instantiate(gameObject, position, Quaternion.identity);
			NetworkServer.Spawn(instance);
			await UniTask.Yield();
			if (!(instance == null))
			{
				if (instance.TryGetComponent<ConditionComponent>(out var component))
				{
					component.ServerSetCondition(100f);
				}
				if (instance.TryGetComponent<LiquidContainerComponent>(out var component2))
				{
					component2.SetAmount(component2.Capacity);
				}
			}
		}

		public bool TryGetTerrainByChunkCoord(Vector2Int chunkCoord, out Terrain terrain)
		{
			terrain = null;
			Coord key = new Coord(chunkCoord.x, chunkCoord.y);
			if (!_activeTileInfosDict.TryGetValue(key, out var value))
			{
				return false;
			}
			terrain = value.tile?.GetTerrain(isDraft: false);
			return terrain != null;
		}

		public Vector2Int GetChunkCoordFromWorldPosition(Vector3 worldPosition)
		{
			float tileSize = TileSize;
			int x = Mathf.FloorToInt(worldPosition.x / tileSize);
			int y = Mathf.FloorToInt(worldPosition.z / tileSize);
			return new Vector2Int(x, y);
		}

		public bool IsInitialChunk(Vector3 worldPosition)
		{
			Vector2Int chunkCoordFromWorldPosition = GetChunkCoordFromWorldPosition(worldPosition);
			Coord item = new Coord(chunkCoordFromWorldPosition.x, chunkCoordFromWorldPosition.y);
			return _initialChunkSet.Contains(item);
		}

		public void RegisterPendingPoi(UnityEngine.Object poi)
		{
			if (!(poi == null) && !_worldReadyFired && _pendingPoiLoot.Add(poi) && !_lootStateAnnounced)
			{
				_lootStateAnnounced = true;
				_loadingManager?.SetState(70);
			}
		}

		public void NotifyPoiLootComplete(UnityEngine.Object poi)
		{
			if (!(poi == null) && _pendingPoiLoot.Remove(poi))
			{
				CheckWorldReadiness();
			}
		}

		private void LockInitialChunksIfNeeded(IEnumerable<Coord> chunks)
		{
			if (_initialChunksLocked)
			{
				return;
			}
			_initialChunksLocked = true;
			foreach (Coord chunk in chunks)
			{
				_initialChunkSet.Add(chunk);
				_pendingInitialTerrain.Add(chunk);
			}
			if (_worldReadyTimeoutCoroutine != null)
			{
				StopCoroutine(_worldReadyTimeoutCoroutine);
			}
			_worldReadyTimeoutCoroutine = StartCoroutine(WorldReadyTimeoutCoroutine());
		}

		private IEnumerator WorldReadyTimeoutCoroutine()
		{
			yield return new WaitForSeconds(30f);
			if (!_worldReadyFired)
			{
				_pendingInitialTerrain.Clear();
				_pendingPoiLoot.Clear();
				CheckWorldReadiness();
			}
		}

		private IEnumerator WorldReadyHardSafetyCoroutine()
		{
			yield return new WaitForSeconds(45f);
			if (!_worldReadyFired)
			{
				_initialChunksLocked = true;
				_pendingInitialTerrain.Clear();
				_pendingPoiLoot.Clear();
				CheckWorldReadiness();
			}
		}

		private void SetLoadingState(int state)
		{
			if (!_worldReadyFired)
			{
				_loadingManager?.SetState(state);
			}
		}

		private void CheckWorldReadiness()
		{
			if (!_worldReadyFired && _initialChunksLocked && _pendingInitialTerrain.Count <= 0 && _pendingPoiLoot.Count <= 0)
			{
				_worldReadyFired = true;
				IsWorldFullyReady = true;
				if (_worldReadyTimeoutCoroutine != null)
				{
					StopCoroutine(_worldReadyTimeoutCoroutine);
					_worldReadyTimeoutCoroutine = null;
				}
				if (_worldReadyHardSafetyCoroutine != null)
				{
					StopCoroutine(_worldReadyHardSafetyCoroutine);
					_worldReadyHardSafetyCoroutine = null;
				}
				_loadingManager?.SetState(90);
				bool flag = base.isServer && _gameSaveService != null && _gameSaveService.IsLoadedWorld;
				if (flag)
				{
					_gameSaveService.OnRestoreComplete += HandleRestoreCompleteForReady;
					_loadingManager?.SuspendTimeout();
				}
				try
				{
					this.OnWorldFullyReady?.Invoke();
				}
				catch (Exception ex)
				{
					EvilLogger.LogError("<color=red>[WorldGenerator]</color> OnWorldFullyReady handler failed: " + ex.Message + "\n" + ex.StackTrace, "CheckWorldReadiness", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\WorldGenerator.cs", 1273);
				}
				if (!flag)
				{
					_loadingManager?.SetState(100);
				}
			}
		}

		private void HandleRestoreCompleteForReady()
		{
			if (_gameSaveService != null)
			{
				_gameSaveService.OnRestoreComplete -= HandleRestoreCompleteForReady;
			}
			_loadingManager?.SetState(100);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdRequestLootSpawn__Vector3__Int32__LootSpawnDataNetwork_005B_005D__NetworkConnectionToClient(Vector3 lootSpawnPosition, int seed, LootSpawnDataNetwork[] lootSpawnDatas, NetworkConnectionToClient sender)
		{
			if (TryReserveLootSpawn(seed))
			{
				SpawnLootsOnServer(lootSpawnPosition, seed, lootSpawnDatas).Forget();
			}
		}

		protected static void InvokeUserCode_CmdRequestLootSpawn__Vector3__Int32__LootSpawnDataNetwork_005B_005D__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRequestLootSpawn called on client.");
			}
			else
			{
				((WorldGenerator)obj).UserCode_CmdRequestLootSpawn__Vector3__Int32__LootSpawnDataNetwork_005B_005D__NetworkConnectionToClient(reader.ReadVector3(), reader.ReadVarInt(), GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EWorldGeneration_002EObjectSpawning_002ELootSpawnDataNetwork_005B_005D(reader), senderConnection);
			}
		}

		protected void UserCode_CmdDebugSpawnLootByGuid__String__Vector3__NetworkConnectionToClient(string guid, Vector3 position, NetworkConnectionToClient sender)
		{
			DebugSpawnLootByGuidAsync(guid, position).Forget();
		}

		protected static void InvokeUserCode_CmdDebugSpawnLootByGuid__String__Vector3__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdDebugSpawnLootByGuid called on client.");
			}
			else
			{
				((WorldGenerator)obj).UserCode_CmdDebugSpawnLootByGuid__String__Vector3__NetworkConnectionToClient(reader.ReadString(), reader.ReadVector3(), senderConnection);
			}
		}

		static WorldGenerator()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(WorldGenerator), "System.Void NomadDrive.Features.WorldGeneration.WorldGenerator::CmdRequestLootSpawn(UnityEngine.Vector3,System.Int32,NomadDrive.Features.WorldGeneration.ObjectSpawning.LootSpawnDataNetwork[],Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdRequestLootSpawn__Vector3__Int32__LootSpawnDataNetwork_005B_005D__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(WorldGenerator), "System.Void NomadDrive.Features.WorldGeneration.WorldGenerator::CmdDebugSpawnLootByGuid(System.String,UnityEngine.Vector3,Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdDebugSpawnLootByGuid__String__Vector3__NetworkConnectionToClient, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteVarInt(seed);
				writer.WriteVector3(_networkOriginShift);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteVarInt(seed);
			}
			if ((syncVarDirtyBits & 2L) != 0L)
			{
				writer.WriteVector3(_networkOriginShift);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref seed, null, reader.ReadVarInt());
				GeneratedSyncVarDeserialize(ref _networkOriginShift, null, reader.ReadVector3());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref seed, null, reader.ReadVarInt());
			}
			if ((num & 2L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _networkOriginShift, null, reader.ReadVector3());
			}
		}
	}
}
