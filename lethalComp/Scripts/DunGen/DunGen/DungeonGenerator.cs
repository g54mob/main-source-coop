using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DunGen.Collision;
using DunGen.Generation;
using DunGen.Graph;
using DunGen.LockAndKey;
using DunGen.Pooling;
using UnityEngine;
using UnityEngine.Serialization;

namespace DunGen
{
	[Serializable]
	public class DungeonGenerator : ISerializationCallbackReceiver
	{
		private struct PropProcessingData
		{
			public RandomProp PropComponent;

			public int HierarchyDepth;

			public Tile OwningTile;
		}

		public const int CurrentFileVersion = 3;

		[SerializeField]
		[FormerlySerializedAs("AllowImmediateRepeats")]
		private bool allowImmediateRepeats;

		[Obsolete("Use the 'CollisionSettings' property instead")]
		public float OverlapThreshold = 0.01f;

		[Obsolete("Use the 'CollisionSettings' property instead")]
		public float Padding;

		[Obsolete("Use the 'CollisionSettings' property instead")]
		public bool DisallowOverhangs;

		[Obsolete("Use the 'CollisionSettings' property instead")]
		public bool AvoidCollisionsWithOtherDungeons = true;

		[Obsolete("Use the 'CollisionSettings' property instead")]
		public readonly List<Bounds> AdditionalCollisionBounds = new List<Bounds>();

		[Obsolete("Use the 'TriggerPlacement' enum property instead")]
		public bool PlaceTileTriggers = true;

		public int Seed;

		public bool ShouldRandomizeSeed = true;

		public int MaxAttemptCount = 20;

		public bool UseMaximumPairingAttempts;

		public int MaxPairingAttempts = 5;

		public AxisDirection UpDirection = AxisDirection.PosY;

		[FormerlySerializedAs("OverrideAllowImmediateRepeats")]
		public bool OverrideRepeatMode;

		public TileRepeatMode RepeatMode;

		public bool OverrideAllowTileRotation;

		public bool AllowTileRotation;

		public bool DebugRender;

		public float LengthMultiplier = 1f;

		public TriggerPlacementMode TriggerPlacement = TriggerPlacementMode.ThreeDimensional;

		public int TileTriggerLayer = 2;

		public bool GenerateAsynchronously;

		public float MaxAsyncFrameMilliseconds = 10f;

		public float PauseBetweenRooms;

		public bool RestrictDungeonToBounds;

		public Bounds TilePlacementBounds = new Bounds(Vector3.zero, Vector3.one * 10f);

		public DungeonCollisionSettings CollisionSettings = new DungeonCollisionSettings();

		public GameObject Root;

		public DungeonFlow DungeonFlow;

		protected int retryCount;

		protected DungeonProxy proxyDungeon;

		protected readonly List<TilePlacementResult> tilePlacementResults = new List<TilePlacementResult>();

		protected readonly List<GameObject> useableTiles = new List<GameObject>();

		protected int targetLength;

		protected List<InjectedTile> tilesPendingInjection;

		protected List<DungeonGeneratorPostProcessStep> postProcessSteps = new List<DungeonGeneratorPostProcessStep>();

		[SerializeField]
		private int fileVersion;

		private int nextNodeIndex;

		private DungeonArchetype currentArchetype;

		private GraphLine previousLineSegment;

		private readonly Dictionary<GameObject, TileProxy> preProcessData = new Dictionary<GameObject, TileProxy>();

		private readonly Dictionary<TileProxy, InjectedTile> injectedTiles = new Dictionary<TileProxy, InjectedTile>();

		private readonly Stopwatch yieldTimer = new Stopwatch();

		private readonly BucketedObjectPool<TileProxy, TileProxy> tileProxyPool;

		private readonly TileInstanceSource tileInstanceSource;

		[Obsolete("Use the 'CollisionSettings' property instead")]
		public AdditionalCollisionsPredicate AdditionalCollisionsPredicate { get; set; }

		public RandomStream RandomStream { get; protected set; }

		public Vector3 UpVector => UpDirection switch
		{
			AxisDirection.PosX => new Vector3(1f, 0f, 0f), 
			AxisDirection.NegX => new Vector3(-1f, 0f, 0f), 
			AxisDirection.PosY => new Vector3(0f, 1f, 0f), 
			AxisDirection.NegY => new Vector3(0f, -1f, 0f), 
			AxisDirection.PosZ => new Vector3(0f, 0f, 1f), 
			AxisDirection.NegZ => new Vector3(0f, 0f, -1f), 
			_ => throw new NotImplementedException("AxisDirection '" + UpDirection.ToString() + "' not implemented"), 
		};

		public GenerationStatus Status { get; private set; }

		public GenerationStats GenerationStats { get; private set; }

		public int ChosenSeed { get; protected set; }

		public Dungeon CurrentDungeon { get; private set; }

		public bool IsGenerating { get; private set; }

		public bool IsAnalysis { get; set; }

		public bool AllowTilePooling { get; set; }

		public DungeonAttachmentSettings AttachmentSettings { get; set; }

		public DungeonCollisionManager CollisionManager { get; private set; }

		public event GenerationStatusDelegate OnGenerationStatusChanged;

		public event DungeonGenerationDelegate OnGenerationStarted;

		public event DungeonGenerationDelegate OnGenerationComplete;

		public static event GenerationStatusDelegate OnAnyDungeonGenerationStatusChanged;

		public static event DungeonGenerationDelegate OnAnyDungeonGenerationStarted;

		public static event DungeonGenerationDelegate OnAnyDungeonGenerationComplete;

		public event TileInjectionDelegate TileInjectionMethods;

		public event Action Cleared;

		public event Action Retrying;

		public static event GenerationFailureReportProduced OnGenerationFailureReportProduced;

		public DungeonGenerator()
		{
			AllowTilePooling = true;
			GenerationStats = new GenerationStats();
			CollisionManager = new DungeonCollisionManager();
			tileProxyPool = new BucketedObjectPool<TileProxy, TileProxy>((TileProxy template) => new TileProxy(template), delegate(TileProxy x)
			{
				x.Placement.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
			});
			tileInstanceSource = new TileInstanceSource();
			tileInstanceSource.TileInstanceSpawned += delegate(Tile tilePrefab, Tile tileInstance, bool fromPool)
			{
				GenerationStats.TileAdded(tilePrefab, fromPool);
			};
		}

		public DungeonGenerator(GameObject root)
			: this()
		{
			Root = root;
		}

		public void Generate()
		{
			if (!IsGenerating)
			{
				this.OnGenerationStarted?.Invoke(this);
				DungeonGenerator.OnAnyDungeonGenerationStarted?.Invoke(this);
				if (AttachmentSettings != null && CurrentDungeon != null)
				{
					DetachDungeon();
				}
				if (CollisionSettings == null)
				{
					CollisionSettings = new DungeonCollisionSettings();
				}
				if (CollisionManager == null)
				{
					CollisionManager = new DungeonCollisionManager();
				}
				CollisionManager.Settings = CollisionSettings;
				DoorwayPairFinder.SortCustomConnectionRules();
				IsGenerating = true;
				Wait(OuterGenerate());
			}
		}

		public void Cancel()
		{
			if (IsGenerating)
			{
				Clear(stopCoroutines: true);
				IsGenerating = false;
			}
		}

		public Dungeon DetachDungeon()
		{
			if (CurrentDungeon == null)
			{
				return null;
			}
			Dungeon currentDungeon = CurrentDungeon;
			CurrentDungeon = null;
			Root = null;
			Clear(stopCoroutines: true);
			if (currentDungeon.transform.childCount == 0)
			{
				UnityEngine.Object.DestroyImmediate(currentDungeon.gameObject);
			}
			return currentDungeon;
		}

		protected virtual IEnumerator OuterGenerate()
		{
			Clear(stopCoroutines: false);
			yieldTimer.Restart();
			Status = GenerationStatus.NotStarted;
			ChosenSeed = (ShouldRandomizeSeed ? new RandomStream().Next() : Seed);
			RandomStream = new RandomStream(ChosenSeed);
			if (Root == null)
			{
				Root = new GameObject("Dungeon");
			}
			bool enableTilePooling = AllowTilePooling && DunGenSettings.Instance.EnableTilePooling;
			tileInstanceSource.Initialise(enableTilePooling, Root);
			yield return Wait(InnerGenerate(isRetry: false));
			IsGenerating = false;
		}

		private Coroutine Wait(IEnumerator routine)
		{
			if (GenerateAsynchronously)
			{
				return CoroutineHelper.Start(routine);
			}
			while (routine.MoveNext())
			{
			}
			return null;
		}

		public void RandomizeSeed()
		{
			Seed = new RandomStream().Next();
		}

		protected virtual IEnumerator InnerGenerate(bool isRetry)
		{
			if (isRetry)
			{
				ChosenSeed = RandomStream.Next();
				RandomStream = new RandomStream(ChosenSeed);
				if (retryCount >= MaxAttemptCount && Application.isEditor)
				{
					if (!IsAnalysis)
					{
						UnityEngine.Debug.LogError(TilePlacementResult.ProduceReport(tilePlacementResults, MaxAttemptCount));
						DungeonGenerator.OnGenerationFailureReportProduced?.Invoke(this, new GenerationFailureReport(MaxAttemptCount, tilePlacementResults));
					}
					ChangeStatus(GenerationStatus.Failed);
					yield break;
				}
				retryCount++;
				GenerationStats.IncrementRetryCount();
				this.Retrying?.Invoke();
			}
			else
			{
				retryCount = 0;
				GenerationStats.Clear();
			}
			CurrentDungeon = Root.GetComponent<Dungeon>();
			if (CurrentDungeon == null)
			{
				CurrentDungeon = Root.AddComponent<Dungeon>();
			}
			CurrentDungeon.TileInstanceSource = tileInstanceSource;
			CollisionManager.Initialize(this);
			CurrentDungeon.DebugRender = DebugRender;
			CurrentDungeon.PreGenerateDungeon(this);
			Clear(stopCoroutines: false);
			targetLength = Mathf.RoundToInt((float)DungeonFlow.Length.GetRandom(RandomStream) * LengthMultiplier);
			targetLength = Mathf.Max(targetLength, 2);
			PauseBetweenRooms = 0f;
			Transform debugVisualsRoot = ((PauseBetweenRooms > 0f) ? Root.transform : null);
			proxyDungeon = new DungeonProxy(debugVisualsRoot);
			GenerationStats.BeginTime(GenerationStatus.TileInjection);
			ChangeStatus(GenerationStatus.TileInjection);
			if (tilesPendingInjection == null)
			{
				tilesPendingInjection = new List<InjectedTile>();
			}
			else
			{
				tilesPendingInjection.Clear();
			}
			injectedTiles.Clear();
			GatherTilesToInject();
			GenerationStats.BeginTime(GenerationStatus.PreProcessing);
			PreProcess();
			GenerationStats.BeginTime(GenerationStatus.MainPath);
			yield return Wait(GenerateMainPath());
			if (Status == GenerationStatus.Complete || Status == GenerationStatus.Failed)
			{
				yield break;
			}
			GenerationStats.BeginTime(GenerationStatus.Branching);
			yield return Wait(GenerateBranchPaths());
			foreach (InjectedTile item in tilesPendingInjection)
			{
				if (item.IsRequired)
				{
					tilePlacementResults.Add(new RequiredTileInjectionFailedResult(item.TileSet));
					yield return Wait(InnerGenerate(isRetry: true));
					yield break;
				}
			}
			if (Status == GenerationStatus.Complete || Status == GenerationStatus.Failed)
			{
				yield break;
			}
			GenerationStats.BeginTime(GenerationStatus.BranchPruning);
			ChangeStatus(GenerationStatus.BranchPruning);
			if (DungeonFlow.BranchPruneTags.Count > 0)
			{
				PruneBranches();
			}
			GenerationStats.BeginTime(GenerationStatus.InstantiatingTiles);
			ChangeStatus(GenerationStatus.InstantiatingTiles);
			proxyDungeon.ConnectOverlappingDoorways(DungeonFlow.DoorwayConnectionChance, DungeonFlow, RandomStream);
			yield return Wait(CurrentDungeon.FromProxy(proxyDungeon, this, () => ShouldSkipFrame(isRoomPlacement: false)));
			yield return Wait(PostProcess());
			proxyDungeon.ClearDebugVisuals();
			yield return null;
			IDungeonCompleteReceiver[] componentsInChildren = CurrentDungeon.gameObject.GetComponentsInChildren<IDungeonCompleteReceiver>(includeInactive: false);
			for (int num = 0; num < componentsInChildren.Length; num++)
			{
				componentsInChildren[num].OnDungeonComplete(CurrentDungeon);
			}
			ChangeStatus(GenerationStatus.Complete);
			if (true)
			{
				DungenCharacter[] array = UnityUtil.FindObjectsByType<DungenCharacter>();
				for (int num = 0; num < array.Length; num++)
				{
					array[num].ForceRecheckTile();
				}
			}
		}

		private void PruneBranches()
		{
			Stack<TileProxy> stack = new Stack<TileProxy>();
			foreach (TileProxy tile in proxyDungeon.BranchPathTiles)
			{
				if (!tile.UsedDoorways.Select((DoorwayProxy d) => d.ConnectedDoorway.TileProxy).Any((TileProxy t) => t.Placement.BranchDepth > tile.Placement.BranchDepth))
				{
					stack.Push(tile);
				}
			}
			while (stack.Count > 0)
			{
				TileProxy tile2 = stack.Pop();
				if ((tile2.Placement.InjectionData == null || !tile2.Placement.InjectionData.IsRequired) && DungeonFlow.ShouldPruneTileWithTags(tile2.PrefabTile.Tags))
				{
					ProxyDoorwayConnection connection = (from d in tile2.UsedDoorways
						select d.ConnectedDoorway into d
						where d.TileProxy.Placement.IsOnMainPath || d.TileProxy.Placement.BranchDepth < tile2.Placement.BranchDepth
						select new ProxyDoorwayConnection(d, d.ConnectedDoorway)).First();
					proxyDungeon.RemoveTile(tile2);
					CollisionManager.RemoveTile(tile2);
					proxyDungeon.RemoveConnection(connection);
					GenerationStats.PrunedBranchTileCount++;
					TileProxy tileProxy = connection.A.TileProxy;
					if (!tileProxy.Placement.IsOnMainPath)
					{
						stack.Push(tileProxy);
					}
				}
			}
		}

		public virtual void Clear(bool stopCoroutines)
		{
			if (stopCoroutines)
			{
				CoroutineHelper.StopAll();
			}
			if (proxyDungeon != null)
			{
				proxyDungeon.ClearDebugVisuals();
			}
			proxyDungeon = null;
			if (CurrentDungeon != null)
			{
				CurrentDungeon.Clear();
			}
			useableTiles.Clear();
			preProcessData.Clear();
			previousLineSegment = null;
			tilePlacementResults.Clear();
			this.Cleared?.Invoke();
		}

		private void ChangeStatus(GenerationStatus status)
		{
			GenerationStatus status2 = Status;
			Status = status;
			if (status == GenerationStatus.Complete || status == GenerationStatus.Failed)
			{
				IsGenerating = false;
			}
			if (status == GenerationStatus.Failed)
			{
				Clear(stopCoroutines: true);
			}
			if (status2 != status)
			{
				this.OnGenerationStatusChanged?.Invoke(this, status);
				DungeonGenerator.OnAnyDungeonGenerationStatusChanged?.Invoke(this, status);
				if (status == GenerationStatus.Complete)
				{
					this.OnGenerationComplete?.Invoke(this);
					DungeonGenerator.OnAnyDungeonGenerationComplete?.Invoke(this);
				}
			}
		}

		protected virtual void PreProcess()
		{
			if (preProcessData.Count > 0)
			{
				return;
			}
			ChangeStatus(GenerationStatus.PreProcessing);
			foreach (TileSet item in DungeonFlow.GetUsedTileSets().Concat(tilesPendingInjection.Select((InjectedTile x) => x.TileSet)).Distinct())
			{
				foreach (GameObjectChance weight in item.TileWeights.Weights)
				{
					if (weight.Value != null)
					{
						useableTiles.Add(weight.Value);
						weight.TileSet = item;
					}
				}
			}
		}

		protected virtual void GatherTilesToInject()
		{
			RandomStream randomStream = new RandomStream(ChosenSeed);
			foreach (TileInjectionRule tileInjectionRule in DungeonFlow.TileInjectionRules)
			{
				if (!(tileInjectionRule.TileSet == null) && (tileInjectionRule.CanAppearOnMainPath || tileInjectionRule.CanAppearOnBranchPath))
				{
					bool isOnMainPath = !tileInjectionRule.CanAppearOnBranchPath || (tileInjectionRule.CanAppearOnMainPath && randomStream.NextDouble() > 0.5);
					InjectedTile item = new InjectedTile(tileInjectionRule, isOnMainPath, randomStream);
					tilesPendingInjection.Add(item);
				}
			}
			this.TileInjectionMethods?.Invoke(randomStream, ref tilesPendingInjection);
		}

		protected virtual IEnumerator GenerateMainPath()
		{
			ChangeStatus(GenerationStatus.MainPath);
			nextNodeIndex = 0;
			List<GraphNode> list = new List<GraphNode>(DungeonFlow.Nodes.Count);
			bool flag = false;
			int num = 0;
			List<TilePlacementParameters> placementSlots = new List<TilePlacementParameters>(targetLength);
			List<List<TileSet>> slotTileSets = new List<List<TileSet>>();
			while (!flag)
			{
				float num2 = Mathf.Clamp((float)num / (float)(targetLength - 1), 0f, 1f);
				GraphLine lineAtDepth = DungeonFlow.GetLineAtDepth(num2);
				if (lineAtDepth == null)
				{
					yield return Wait(InnerGenerate(isRetry: true));
					yield break;
				}
				if (lineAtDepth != previousLineSegment)
				{
					currentArchetype = lineAtDepth.GetRandomArchetype(RandomStream, placementSlots.Select((TilePlacementParameters x) => x.Archetype));
					previousLineSegment = lineAtDepth;
				}
				GraphNode graphNode = null;
				GraphNode[] array = DungeonFlow.Nodes.OrderBy((GraphNode x) => x.Position).ToArray();
				GraphNode[] array2 = array;
				foreach (GraphNode graphNode2 in array2)
				{
					if (num2 >= graphNode2.Position && !list.Contains(graphNode2))
					{
						graphNode = graphNode2;
						list.Add(graphNode2);
						break;
					}
				}
				TilePlacementParameters tilePlacementParameters = new TilePlacementParameters();
				placementSlots.Add(tilePlacementParameters);
				List<TileSet> tileSets;
				if (graphNode != null)
				{
					tileSets = graphNode.TileSets;
					nextNodeIndex = ((nextNodeIndex >= array.Length - 1) ? (-1) : (nextNodeIndex + 1));
					tilePlacementParameters.Node = graphNode;
					if (graphNode == array[^1])
					{
						flag = true;
					}
				}
				else
				{
					tileSets = currentArchetype.TileSets;
					tilePlacementParameters.Archetype = currentArchetype;
					tilePlacementParameters.Line = lineAtDepth;
				}
				slotTileSets.Add(tileSets);
				num++;
			}
			int tileRetryCount = 0;
			int totalForLoopRetryCount = 0;
			for (int j = 0; j < placementSlots.Count; j++)
			{
				TileProxy attachTo = ((j == 0) ? null : proxyDungeon.MainPathTiles[proxyDungeon.MainPathTiles.Count - 1]);
				TileProxy tileProxy = AddTile(attachTo, slotTileSets[j], (float)j / (float)(placementSlots.Count - 1), placementSlots[j]);
				if (j > 5 && tileProxy == null && tileRetryCount < 5 && totalForLoopRetryCount < 20)
				{
					TileProxy tileProxy2 = proxyDungeon.MainPathTiles[j - 1];
					if (injectedTiles.TryGetValue(tileProxy2, out var value))
					{
						tilesPendingInjection.Add(value);
						injectedTiles.Remove(tileProxy2);
					}
					proxyDungeon.RemoveLastConnection();
					proxyDungeon.RemoveTile(tileProxy2);
					CollisionManager.RemoveTile(tileProxy2);
					j -= 2;
					tileRetryCount++;
					totalForLoopRetryCount++;
				}
				else
				{
					if (tileProxy == null)
					{
						yield return Wait(InnerGenerate(isRetry: true));
						break;
					}
					tileProxy.Placement.PlacementParameters = placementSlots[j];
					tileRetryCount = 0;
					if (ShouldSkipFrame(isRoomPlacement: true))
					{
						yield return GetRoomPause();
					}
				}
			}
		}

		private bool ShouldSkipFrame(bool isRoomPlacement)
		{
			if (!GenerateAsynchronously)
			{
				return false;
			}
			if (isRoomPlacement && PauseBetweenRooms > 0f)
			{
				return true;
			}
			if (MaxAsyncFrameMilliseconds <= 0f || yieldTimer.Elapsed.TotalMilliseconds >= (double)MaxAsyncFrameMilliseconds)
			{
				yieldTimer.Restart();
				return true;
			}
			return false;
		}

		private YieldInstruction GetRoomPause()
		{
			if (PauseBetweenRooms > 0f)
			{
				return new WaitForSeconds(PauseBetweenRooms);
			}
			return null;
		}

		protected virtual IEnumerator GenerateBranchPaths()
		{
			ChangeStatus(GenerationStatus.Branching);
			int[] mainPathBranches = new int[proxyDungeon.MainPathTiles.Count];
			BranchCountHelper.ComputeBranchCounts(DungeonFlow, RandomStream, proxyDungeon, ref mainPathBranches);
			int branchId = 0;
			for (int b = 0; b < mainPathBranches.Length; b++)
			{
				TileProxy tile = proxyDungeon.MainPathTiles[b];
				int branchCount = mainPathBranches[b];
				if (tile.Placement.Archetype == null || branchCount == 0)
				{
					continue;
				}
				for (int i = 0; i < branchCount; i++)
				{
					TileProxy previousTile = tile;
					int branchDepth = tile.Placement.Archetype.BranchingDepth.GetRandom(RandomStream);
					for (int j = 0; j < branchDepth; j++)
					{
						List<TileSet> useableTileSets = ((j == 0 && tile.Placement.Archetype.GetHasValidBranchStartTiles()) ? ((tile.Placement.Archetype.BranchStartType != BranchCapType.InsteadOf) ? tile.Placement.Archetype.TileSets.Concat(tile.Placement.Archetype.BranchStartTileSets).ToList() : tile.Placement.Archetype.BranchStartTileSets) : ((j != branchDepth - 1 || !tile.Placement.Archetype.GetHasValidBranchCapTiles()) ? tile.Placement.Archetype.TileSets : ((tile.Placement.Archetype.BranchCapType != BranchCapType.InsteadOf) ? tile.Placement.Archetype.TileSets.Concat(tile.Placement.Archetype.BranchCapTileSets).ToList() : tile.Placement.Archetype.BranchCapTileSets)));
						float num = ((branchDepth <= 1) ? 1f : ((float)j / (float)(branchDepth - 1)));
						TileProxy tileProxy = AddTile(previousTile, useableTileSets, num, tile.Placement.PlacementParameters);
						if (tileProxy == null)
						{
							break;
						}
						tileProxy.Placement.BranchDepth = j;
						tileProxy.Placement.NormalizedBranchDepth = num;
						tileProxy.Placement.BranchId = branchId;
						tileProxy.Placement.PlacementParameters = previousTile.Placement.PlacementParameters;
						previousTile = tileProxy;
						if (ShouldSkipFrame(isRoomPlacement: true))
						{
							yield return GetRoomPause();
						}
					}
					branchId++;
				}
			}
		}

		protected virtual TileProxy AddTile(TileProxy attachTo, IEnumerable<TileSet> useableTileSets, float normalizedDepth, TilePlacementParameters placementParams)
		{
			bool flag = Status == GenerationStatus.MainPath;
			if (attachTo == null && AttachmentSettings != null)
			{
				attachTo = AttachmentSettings.GenerateAttachmentProxy(UpVector, RandomStream);
			}
			InjectedTile injectedTile = null;
			int index = -1;
			bool flag2 = flag && placementParams.Archetype == null;
			if (tilesPendingInjection != null && !flag2)
			{
				float pathDepth = (flag ? normalizedDepth : ((float)attachTo.Placement.PathDepth / ((float)targetLength - 1f)));
				float branchDepth = (flag ? 0f : normalizedDepth);
				for (int i = 0; i < tilesPendingInjection.Count; i++)
				{
					InjectedTile injectedTile2 = tilesPendingInjection[i];
					if (injectedTile2.ShouldInjectTileAtPoint(flag, pathDepth, branchDepth))
					{
						injectedTile = injectedTile2;
						index = i;
						break;
					}
				}
			}
			IEnumerable<GameObjectChance> collection = ((injectedTile == null) ? useableTileSets.SelectMany((TileSet x) => x.TileWeights.Weights) : new List<GameObjectChance>(injectedTile.TileSet.TileWeights.Weights));
			bool? allowRotation = null;
			if (OverrideAllowTileRotation)
			{
				allowRotation = AllowTileRotation;
			}
			DoorwayPairFinder obj = new DoorwayPairFinder
			{
				DungeonFlow = DungeonFlow,
				RandomStream = RandomStream,
				PlacementParameters = placementParams,
				GetTileTemplateDelegate = GetTileTemplate,
				IsOnMainPath = flag,
				NormalizedDepth = normalizedDepth,
				PreviousTile = attachTo,
				UpVector = UpVector,
				AllowRotation = allowRotation,
				TileWeights = new List<GameObjectChance>(collection),
				DungeonProxy = proxyDungeon,
				IsTileAllowedPredicate = delegate(TileProxy previousTile, TileProxy potentialNextTile, ref float weight)
				{
					bool flag3 = previousTile != null && potentialNextTile.Prefab == previousTile.Prefab;
					TileRepeatMode tileRepeatMode = TileRepeatMode.Allow;
					if (OverrideRepeatMode)
					{
						tileRepeatMode = RepeatMode;
					}
					else if (potentialNextTile != null)
					{
						tileRepeatMode = potentialNextTile.PrefabTile.RepeatMode;
					}
					bool flag4 = true;
					return tileRepeatMode switch
					{
						TileRepeatMode.Allow => true, 
						TileRepeatMode.DisallowImmediate => !flag3, 
						TileRepeatMode.Disallow => !proxyDungeon.AllTiles.Where((TileProxy t) => t.Prefab == potentialNextTile.Prefab).Any(), 
						_ => throw new NotImplementedException("TileRepeatMode " + tileRepeatMode.ToString() + " is not implemented"), 
					};
				}
			};
			int? maxCount = (UseMaximumPairingAttempts ? new int?(MaxPairingAttempts) : ((int?)null));
			Queue<DoorwayPair> doorwayPairs = obj.GetDoorwayPairs(maxCount);
			TilePlacementResult tilePlacementResult = null;
			TileProxy tile = null;
			if (doorwayPairs.Count == 0)
			{
				tilePlacementResults.Add(new NoMatchingDoorwayPlacementResult(attachTo));
			}
			while (doorwayPairs.Count > 0)
			{
				DoorwayPair pair = doorwayPairs.Dequeue();
				tilePlacementResult = TryPlaceTile(pair, placementParams, out tile);
				if (tilePlacementResult is SuccessPlacementResult)
				{
					break;
				}
				tilePlacementResults.Add(tilePlacementResult);
			}
			if (tilePlacementResult is SuccessPlacementResult)
			{
				if (injectedTile != null)
				{
					tile.Placement.InjectionData = injectedTile;
					injectedTiles[tile] = injectedTile;
					tilesPendingInjection.RemoveAt(index);
					if (flag)
					{
						targetLength++;
					}
				}
				return tile;
			}
			return null;
		}

		protected TilePlacementResult TryPlaceTile(DoorwayPair pair, TilePlacementParameters placementParameters, out TileProxy tile)
		{
			tile = null;
			TileProxy nextTemplate = pair.NextTemplate;
			DoorwayProxy previousDoorway = pair.PreviousDoorway;
			if (nextTemplate == null)
			{
				return new NullTemplatePlacementResult();
			}
			int index = pair.NextTemplate.Doorways.IndexOf(pair.NextDoorway);
			tile = tileProxyPool.TakeObject(nextTemplate);
			tile.Placement.IsOnMainPath = Status == GenerationStatus.MainPath;
			tile.Placement.PlacementParameters = placementParameters;
			tile.Placement.TileSet = pair.NextTileSet;
			if (previousDoorway != null)
			{
				DoorwayProxy myDoorway = tile.Doorways[index];
				tile.PositionBySocket(myDoorway, previousDoorway);
				Bounds bounds = tile.Placement.Bounds;
				if (RestrictDungeonToBounds && !TilePlacementBounds.Contains(bounds))
				{
					tileProxyPool.ReturnObject(tile);
					return new OutOfBoundsPlacementResult(nextTemplate);
				}
				if (CollisionManager.IsCollidingWithAnyTile(UpDirection, tile, previousDoorway.TileProxy))
				{
					tileProxyPool.ReturnObject(tile);
					return new TileIsCollidingPlacementResult(nextTemplate);
				}
			}
			if (tile.Placement.IsOnMainPath)
			{
				if (pair.PreviousTile != null)
				{
					tile.Placement.PathDepth = pair.PreviousTile.Placement.PathDepth + 1;
				}
			}
			else
			{
				tile.Placement.PathDepth = pair.PreviousTile.Placement.PathDepth;
				tile.Placement.BranchDepth = ((!pair.PreviousTile.Placement.IsOnMainPath) ? (pair.PreviousTile.Placement.BranchDepth + 1) : 0);
			}
			DoorwayProxy b = tile.Doorways[index];
			if (previousDoorway != null)
			{
				proxyDungeon.MakeConnection(previousDoorway, b);
			}
			proxyDungeon.AddTile(tile);
			CollisionManager.AddTile(tile);
			return new SuccessPlacementResult();
		}

		protected TileProxy GetTileTemplate(GameObject prefab)
		{
			if (!preProcessData.TryGetValue(prefab, out var value))
			{
				value = new TileProxy(prefab);
				preProcessData.Add(prefab, value);
			}
			return value;
		}

		protected TileProxy PickRandomTemplate(DoorwaySocket socketGroupFilter)
		{
			GameObject prefab = useableTiles[RandomStream.Next(0, useableTiles.Count)];
			TileProxy tileTemplate = GetTileTemplate(prefab);
			if (socketGroupFilter != null && !tileTemplate.UnusedDoorways.Where((DoorwayProxy d) => d.Socket == socketGroupFilter).Any())
			{
				return PickRandomTemplate(socketGroupFilter);
			}
			return tileTemplate;
		}

		protected int NormalizedDepthToIndex(float normalizedDepth)
		{
			return Mathf.RoundToInt(normalizedDepth * (float)(targetLength - 1));
		}

		protected float IndexToNormalizedDepth(int index)
		{
			return (float)index / (float)targetLength;
		}

		public void RegisterPostProcessStep(Action<DungeonGenerator> postProcessCallback, int priority = 0, PostProcessPhase phase = PostProcessPhase.AfterBuiltIn)
		{
			postProcessSteps.Add(new DungeonGeneratorPostProcessStep(postProcessCallback, priority, phase));
		}

		public void UnregisterPostProcessStep(Action<DungeonGenerator> postProcessCallback)
		{
			for (int i = 0; i < postProcessSteps.Count; i++)
			{
				if (postProcessSteps[i].PostProcessCallback == postProcessCallback)
				{
					postProcessSteps.RemoveAt(i);
				}
			}
		}

		protected virtual IEnumerator PostProcess()
		{
			GenerationStats.BeginTime(GenerationStatus.PostProcessing);
			ChangeStatus(GenerationStatus.PostProcessing);
			int length = proxyDungeon.MainPathTiles.Count;
			int maxBranchDepth = 0;
			if (proxyDungeon.BranchPathTiles.Count > 0)
			{
				foreach (TileProxy branchPathTile in proxyDungeon.BranchPathTiles)
				{
					int branchDepth = branchPathTile.Placement.BranchDepth;
					if (branchDepth > maxBranchDepth)
					{
						maxBranchDepth = branchDepth;
					}
				}
			}
			yield return null;
			postProcessSteps.Sort((DungeonGeneratorPostProcessStep a, DungeonGeneratorPostProcessStep b) => b.Priority.CompareTo(a.Priority));
			foreach (DungeonGeneratorPostProcessStep step in postProcessSteps)
			{
				if (ShouldSkipFrame(isRoomPlacement: false))
				{
					yield return null;
				}
				if (step.Phase == PostProcessPhase.BeforeBuiltIn)
				{
					step.PostProcessCallback(this);
				}
			}
			yield return null;
			foreach (Tile tile in CurrentDungeon.AllTiles)
			{
				if (ShouldSkipFrame(isRoomPlacement: false))
				{
					yield return null;
				}
				tile.Placement.NormalizedPathDepth = (float)tile.Placement.PathDepth / (float)(length - 1);
			}
			CurrentDungeon.PostGenerateDungeon(this);
			ProcessLocalProps();
			ProcessGlobalProps();
			if (DungeonFlow.KeyManager != null)
			{
				PlaceLocksAndKeys();
			}
			GenerationStats.SetRoomStatistics(CurrentDungeon.MainPathTiles.Count, CurrentDungeon.BranchPathTiles.Count, maxBranchDepth);
			preProcessData.Clear();
			yield return null;
			foreach (DungeonGeneratorPostProcessStep step in postProcessSteps)
			{
				if (ShouldSkipFrame(isRoomPlacement: false))
				{
					yield return null;
				}
				if (step.Phase == PostProcessPhase.AfterBuiltIn)
				{
					step.PostProcessCallback(this);
				}
			}
			GenerationStats.EndTime();
			foreach (GameObject door in CurrentDungeon.Doors)
			{
				if (door != null)
				{
					door.SetActive(value: true);
				}
			}
		}

		protected virtual void ProcessLocalProps()
		{
			RandomProp[] componentsInChildren = Root.GetComponentsInChildren<RandomProp>();
			List<PropProcessingData> list = new List<PropProcessingData>();
			RandomProp[] array = componentsInChildren;
			foreach (RandomProp randomProp in array)
			{
				int depth = 0;
				GetHierarchyDepth(randomProp.transform, ref depth);
				list.Add(new PropProcessingData
				{
					PropComponent = randomProp,
					HierarchyDepth = depth,
					OwningTile = randomProp.GetComponentInParent<Tile>()
				});
			}
			list = list.OrderBy((PropProcessingData x) => x.HierarchyDepth).ToList();
			List<GameObject> spawnedObjects = new List<GameObject>();
			for (int num = 0; num < list.Count; num++)
			{
				PropProcessingData propProcessingData = list[num];
				if (propProcessingData.PropComponent == null)
				{
					continue;
				}
				spawnedObjects.Clear();
				propProcessingData.PropComponent.Process(RandomStream, propProcessingData.OwningTile, ref spawnedObjects);
				foreach (RandomProp item in spawnedObjects.SelectMany((GameObject x) => x.GetComponentsInChildren<RandomProp>()).Distinct())
				{
					list.Insert(num + 1, new PropProcessingData
					{
						PropComponent = item,
						HierarchyDepth = propProcessingData.HierarchyDepth + 1,
						OwningTile = propProcessingData.OwningTile
					});
				}
			}
			static void GetHierarchyDepth(Transform transform, ref int reference)
			{
				if (transform.parent != null)
				{
					reference++;
					GetHierarchyDepth(transform.parent, ref reference);
				}
			}
		}

		protected virtual void ProcessGlobalProps()
		{
			Dictionary<int, GameObjectChanceTable> dictionary = new Dictionary<int, GameObjectChanceTable>();
			foreach (Tile allTile in CurrentDungeon.AllTiles)
			{
				GlobalProp[] componentsInChildren = allTile.GetComponentsInChildren<GlobalProp>(includeInactive: true);
				foreach (GlobalProp globalProp in componentsInChildren)
				{
					GameObjectChanceTable value = null;
					if (!dictionary.TryGetValue(globalProp.PropGroupID, out value))
					{
						value = new GameObjectChanceTable();
						dictionary[globalProp.PropGroupID] = value;
					}
					float num = (allTile.Placement.IsOnMainPath ? globalProp.MainPathWeight : globalProp.BranchPathWeight);
					num *= globalProp.DepthWeightScale.Evaluate(allTile.Placement.NormalizedDepth);
					value.Weights.Add(new GameObjectChance(globalProp.gameObject, num, 0f, null));
				}
			}
			foreach (GameObjectChanceTable value2 in dictionary.Values)
			{
				foreach (GameObjectChance weight in value2.Weights)
				{
					weight.Value.SetActive(value: false);
				}
			}
			List<int> list = new List<int>(dictionary.Count);
			foreach (KeyValuePair<int, GameObjectChanceTable> pair in dictionary)
			{
				if (list.Contains(pair.Key))
				{
					UnityEngine.Debug.LogWarning("Dungeon Flow contains multiple entries for the global prop group ID: " + pair.Key + ". Only the first entry will be used.");
					continue;
				}
				DungeonFlow.GlobalPropSettings globalPropSettings = DungeonFlow.GlobalProps.Where((DungeonFlow.GlobalPropSettings x) => x.ID == pair.Key).FirstOrDefault();
				if (globalPropSettings == null)
				{
					continue;
				}
				GameObjectChanceTable gameObjectChanceTable = pair.Value.Clone();
				int random = globalPropSettings.Count.GetRandom(RandomStream);
				random = Mathf.Clamp(random, 0, gameObjectChanceTable.Weights.Count);
				for (int num2 = 0; num2 < random; num2++)
				{
					GameObjectChance random2 = gameObjectChanceTable.GetRandom(RandomStream, isOnMainPath: true, 0f, null, allowImmediateRepeats: true, removeFromTable: true);
					if (random2 != null && random2.Value != null)
					{
						random2.Value.SetActive(value: true);
					}
				}
				list.Add(pair.Key);
			}
		}

		protected virtual void PlaceLocksAndKeys()
		{
			GraphNode[] array = (from x in CurrentDungeon.ConnectionGraph.Nodes
				select x.Tile.Placement.GraphNode into x
				where x != null
				select x).Distinct().ToArray();
			GraphLine[] array2 = (from x in CurrentDungeon.ConnectionGraph.Nodes
				select x.Tile.Placement.GraphLine into x
				where x != null
				select x).Distinct().ToArray();
			Dictionary<Doorway, Key> lockedDoorways = new Dictionary<Doorway, Key>();
			GraphNode[] array3 = array;
			foreach (GraphNode node in array3)
			{
				foreach (KeyLockPlacement @lock in node.Locks)
				{
					Tile tile = CurrentDungeon.AllTiles.Where((Tile x) => x.Placement.GraphNode == node).FirstOrDefault();
					List<DungeonGraphConnection> connections = CurrentDungeon.ConnectionGraph.Nodes.Where((DungeonGraphNode x) => x.Tile == tile).FirstOrDefault().Connections;
					Doorway doorway = null;
					Doorway doorway2 = null;
					foreach (DungeonGraphConnection item in connections)
					{
						if (item.DoorwayA.Tile == tile)
						{
							doorway2 = item.DoorwayA;
						}
						else if (item.DoorwayB.Tile == tile)
						{
							doorway = item.DoorwayB;
						}
					}
					Key keyByID = node.Graph.KeyManager.GetKeyByID(@lock.ID);
					if (doorway != null && (node.LockPlacement & NodeLockPlacement.Entrance) == NodeLockPlacement.Entrance)
					{
						lockedDoorways.Add(doorway, keyByID);
					}
					if (doorway2 != null && (node.LockPlacement & NodeLockPlacement.Exit) == NodeLockPlacement.Exit)
					{
						lockedDoorways.Add(doorway2, keyByID);
					}
				}
			}
			GraphLine[] array4 = array2;
			foreach (GraphLine line in array4)
			{
				List<Doorway> list = (from x in CurrentDungeon.ConnectionGraph.Connections.Where(delegate(DungeonGraphConnection x)
					{
						TileSet tileSet = x.DoorwayA.Tile.Placement.TileSet;
						if (tileSet == null)
						{
							return false;
						}
						bool flag4 = lockedDoorways.ContainsKey(x.DoorwayA) || lockedDoorways.ContainsKey(x.DoorwayB);
						bool flag5 = tileSet.LockPrefabs.Count > 0;
						return x.DoorwayA.Tile.Placement.GraphLine == line && x.DoorwayB.Tile.Placement.GraphLine == line && !flag4 && flag5;
					})
					select x.DoorwayA).ToList();
				if (list.Count == 0)
				{
					continue;
				}
				foreach (KeyLockPlacement lock2 in line.Locks)
				{
					int random = lock2.Range.GetRandom(RandomStream);
					random = Mathf.Clamp(random, 0, list.Count);
					for (int num2 = 0; num2 < random; num2++)
					{
						if (list.Count == 0)
						{
							break;
						}
						Doorway doorway3 = list[RandomStream.Next(0, list.Count)];
						list.Remove(doorway3);
						if (!lockedDoorways.ContainsKey(doorway3))
						{
							Key keyByID2 = line.Graph.KeyManager.GetKeyByID(lock2.ID);
							lockedDoorways.Add(doorway3, keyByID2);
						}
					}
				}
			}
			foreach (Tile allTile in CurrentDungeon.AllTiles)
			{
				if (allTile.Placement.InjectionData == null || !allTile.Placement.InjectionData.IsLocked)
				{
					continue;
				}
				List<Doorway> list2 = new List<Doorway>();
				foreach (Doorway usedDoorway in allTile.UsedDoorways)
				{
					bool num3 = lockedDoorways.ContainsKey(usedDoorway) || lockedDoorways.ContainsKey(usedDoorway.ConnectedDoorway);
					bool flag = allTile.Placement.TileSet.LockPrefabs.Count > 0;
					bool flag2 = allTile.GetEntranceDoorway() == usedDoorway;
					if (!num3 && flag && flag2)
					{
						list2.Add(usedDoorway);
					}
				}
				if (list2.Any())
				{
					Doorway key = list2.First();
					Key keyByID3 = DungeonFlow.KeyManager.GetKeyByID(allTile.Placement.InjectionData.LockID);
					lockedDoorways.Add(key, keyByID3);
				}
			}
			List<Doorway> list3 = new List<Doorway>();
			List<IKeySpawner> list4 = new List<IKeySpawner>();
			foreach (KeyValuePair<Doorway, Key> item2 in lockedDoorways)
			{
				Doorway key2 = item2.Key;
				Key key3 = item2.Value;
				List<Tile> list5 = new List<Tile>();
				foreach (Tile allTile2 in CurrentDungeon.AllTiles)
				{
					if (!(allTile2.Placement.NormalizedPathDepth >= key2.Tile.Placement.NormalizedPathDepth))
					{
						bool flag3 = false;
						if (allTile2.Placement.GraphNode != null && allTile2.Placement.GraphNode.Keys.Where((KeyLockPlacement x) => x.ID == key3.ID).Count() > 0)
						{
							flag3 = true;
						}
						else if (allTile2.Placement.GraphLine != null && allTile2.Placement.GraphLine.Keys.Where((KeyLockPlacement x) => x.ID == key3.ID).Count() > 0)
						{
							flag3 = true;
						}
						if (flag3)
						{
							list5.Add(allTile2);
						}
					}
				}
				IKeySpawner[] array5 = (from x in list5.SelectMany((Tile x) => x.GetComponentsInChildren<Component>().OfType<IKeySpawner>()).Except(list4)
					where x.CanSpawnKey(DungeonFlow.KeyManager, key3)
					select x).ToArray();
				GameObject gameObject = null;
				if (array5.Any())
				{
					gameObject = TryGetRandomLockedDoorPrefab(key2, key3, DungeonFlow.KeyManager);
				}
				if (!array5.Any() || gameObject == null)
				{
					list3.Add(key2);
					continue;
				}
				key2.LockID = key3.ID;
				KeySpawnParameters keySpawnParameters = new KeySpawnParameters(key3, DungeonFlow.KeyManager, this);
				int random2 = key3.KeysPerLock.GetRandom(RandomStream);
				random2 = Math.Min(random2, array5.Length);
				for (int num4 = 0; num4 < random2; num4++)
				{
					int num5 = RandomStream.Next(0, array5.Length);
					IKeySpawner keySpawner = array5[num5];
					keySpawnParameters.OutputSpawnedKeys.Clear();
					keySpawner.SpawnKey(keySpawnParameters);
					foreach (IKeyLock outputSpawnedKey in keySpawnParameters.OutputSpawnedKeys)
					{
						outputSpawnedKey.OnKeyAssigned(key3, DungeonFlow.KeyManager);
					}
					list4.Add(keySpawner);
				}
				LockDoorway(key2, gameObject, key3, DungeonFlow.KeyManager);
			}
			foreach (Doorway item3 in list3)
			{
				item3.LockID = null;
				lockedDoorways.Remove(item3);
			}
		}

		protected virtual GameObject TryGetRandomLockedDoorPrefab(Doorway doorway, Key key, KeyManager keyManager)
		{
			TilePlacementData placement = doorway.Tile.Placement;
			GameObjectChanceTable[] array = (from x in doorway.Tile.Placement.TileSet.LockPrefabs.Where(delegate(LockedDoorwayAssociation x)
				{
					if (x == null || x.LockPrefabs == null)
					{
						return false;
					}
					if (!x.LockPrefabs.HasAnyValidEntries(placement.IsOnMainPath, placement.NormalizedDepth, null, allowImmediateRepeats: true))
					{
						return false;
					}
					DoorwaySocket socket = x.Socket;
					return socket == null || DoorwaySocket.CanSocketsConnect(socket, doorway.Socket);
				})
				select x.LockPrefabs).ToArray();
			if (array.Length == 0)
			{
				return null;
			}
			return array[RandomStream.Next(0, array.Length)].GetRandom(RandomStream, placement.IsOnMainPath, placement.NormalizedDepth, null, allowImmediateRepeats: true).Value;
		}

		protected virtual void LockDoorway(Doorway doorway, GameObject doorPrefab, Key key, KeyManager keyManager)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(doorPrefab, doorway.transform);
			DungeonUtil.AddAndSetupDoorComponent(CurrentDungeon, gameObject, doorway);
			doorway.RemoveUsedPrefab();
			doorway.SetUsedPrefab(gameObject);
			doorway.ConnectedDoorway.SetUsedPrefab(gameObject);
			foreach (IKeyLock item in gameObject.GetComponentsInChildren<Component>().OfType<IKeyLock>())
			{
				item.OnKeyAssigned(key, keyManager);
			}
		}

		public void OnBeforeSerialize()
		{
			fileVersion = 3;
		}

		public void OnAfterDeserialize()
		{
			if (fileVersion < 1)
			{
				RepeatMode = ((!allowImmediateRepeats) ? TileRepeatMode.DisallowImmediate : TileRepeatMode.Allow);
			}
			if (fileVersion < 2)
			{
				if (CollisionSettings == null)
				{
					CollisionSettings = new DungeonCollisionSettings();
				}
				CollisionSettings.DisallowOverhangs = DisallowOverhangs;
				CollisionSettings.OverlapThreshold = OverlapThreshold;
				CollisionSettings.Padding = Padding;
				CollisionSettings.AvoidCollisionsWithOtherDungeons = AvoidCollisionsWithOtherDungeons;
			}
			if (fileVersion < 3)
			{
				TriggerPlacement = (PlaceTileTriggers ? TriggerPlacementMode.ThreeDimensional : TriggerPlacementMode.None);
			}
		}
	}
}
