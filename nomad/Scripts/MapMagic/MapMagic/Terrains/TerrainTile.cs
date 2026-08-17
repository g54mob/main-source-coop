using System;
using System.Collections;
using System.Collections.Generic;
using Den.Tools;
using Den.Tools.Tasks;
using MapMagic.Core;
using MapMagic.Nodes;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Terrains
{
	public class TerrainTile : MonoBehaviour, ITile, ISerializationCallbackReceiver
	{
		[Serializable]
		public class DetailLevel
		{
			[NonSerialized]
			public TileData data;

			public Terrain terrain;

			public EdgesSet edges = new EdgesSet();

			public bool generateStarted = true;

			public bool generateReady;

			public bool applyReady;

			[NonSerialized]
			public StopToken stop;

			[NonSerialized]
			public ThreadManager.Task task;

			[NonSerialized]
			public CoroutineManager.Task coroutine;

			[NonSerialized]
			public Stack<CoroutineManager.Task> applyMainCoroutines;

			[NonSerialized]
			public CoroutineManager.Task applyDraftCoroutine;

			[NonSerialized]
			public CoroutineManager.Task switchLodCoroutine;

			public DetailLevel(TerrainTile tile, bool isDraft)
			{
				data = new TileData();
				terrain = tile.CreateTerrain(isDraft);
			}

			public void Remove()
			{
				data?.Clear(clearApply: true, inSubs: true);
				if (terrain != null)
				{
					UnityEngine.Object.DestroyImmediate(terrain.gameObject);
				}
			}
		}

		public MapMagicObject mapMagic;

		public Coord coord = new Coord(2147483647, 2147483647);

		public float distance = -1f;

		public bool preview = true;

		public TerrainData defaultTerrainData;

		public static Action<TerrainTile, TileData> OnBeforeTileStart;

		public static Action<TerrainTile, TileData> OnBeforeTilePrepare;

		public static Action<TerrainTile, TileData, StopToken> OnBeforeTileGenerate;

		public static Action<TerrainTile, TileData, StopToken> OnTileFinalized;

		public static Action<TerrainTile, TileData, StopToken> OnTileApplied;

		public static Action<MapMagicObject> OnAllComplete;

		public static Action<TerrainTile, bool, bool> OnLodSwitched;

		public static Action<TileData> OnPreviewAssigned;

		public static Action<TerrainTile> OnTileMoved;

		public static Action<TerrainTile> OnBeforeResetTerrain;

		public static Action<TerrainTile> OnAfterResetTerrain;

		[NonSerialized]
		public DetailLevel main;

		[NonSerialized]
		public DetailLevel draft;

		public ObjectsPool objectsPool;

		public bool guiMain;

		public bool guiDraft;

		[SerializeField]
		private DetailLevel serialized_main;

		[SerializeField]
		private bool serialized_mainNull;

		[SerializeField]
		private DetailLevel serialized_draft;

		[SerializeField]
		private bool serialized_draftNull;

		public int Priority => (int)((0f - distance) * 100f);

		public Rect WorldRect => new Rect((float)coord.x * mapMagic.tileSize.x, (float)coord.z * mapMagic.tileSize.z, mapMagic.tileSize.x, mapMagic.tileSize.z);

		public Vector2D Min => new Vector2D((float)coord.x * mapMagic.tileSize.x, (float)coord.z * mapMagic.tileSize.z);

		public Vector2D Max => new Vector2D((float)(coord.x + 1) * mapMagic.tileSize.x, (float)(coord.z + 1) * mapMagic.tileSize.z);

		public Terrain ActiveTerrain
		{
			get
			{
				if (main != null && main.terrain != null && main.terrain.isActiveAndEnabled)
				{
					return main.terrain;
				}
				if (draft != null && draft.terrain != null && draft.terrain.isActiveAndEnabled)
				{
					return draft.terrain;
				}
				return null;
			}
			set
			{
				if (main != null && value == main.terrain)
				{
					if (main.terrain != null && !main.terrain.isActiveAndEnabled)
					{
						main.terrain.gameObject.SetActive(value: true);
					}
					if (draft != null && draft.terrain != null && draft.terrain.isActiveAndEnabled)
					{
						draft.terrain.gameObject.SetActive(value: false);
					}
				}
				else if (draft != null && value == draft.terrain)
				{
					if (main != null && main.terrain != null && main.terrain.isActiveAndEnabled)
					{
						main.terrain.gameObject.SetActive(value: false);
					}
					if (draft.terrain != null && !draft.terrain.isActiveAndEnabled)
					{
						draft.terrain.gameObject.SetActive(value: true);
					}
				}
				else
				{
					if (main?.terrain != null && main.terrain.isActiveAndEnabled)
					{
						main.terrain.gameObject.SetActive(value: false);
					}
					if (draft?.terrain != null && draft.terrain.isActiveAndEnabled)
					{
						draft.terrain.gameObject.SetActive(value: false);
					}
				}
			}
		}

		public bool IsNull
		{
			get
			{
				if (!(this == null) && !Equals(null) && !(base.gameObject == null))
				{
					return base.gameObject.Equals(null);
				}
				return true;
			}
		}

		public bool IsGenerating
		{
			get
			{
				if (main != null && main.generateStarted && !main.applyReady)
				{
					return true;
				}
				if (draft != null && draft.generateStarted && !draft.applyReady)
				{
					return true;
				}
				return false;
			}
		}

		public bool Ready
		{
			get
			{
				if (main != null && (!main.applyReady || !main.generateReady))
				{
					return false;
				}
				if (draft != null && (!draft.applyReady || !draft.generateReady))
				{
					return false;
				}
				return true;
			}
		}

		public bool ContainsWorldPosition(float x, float z)
		{
			Vector2D vector2D = new Vector2D((float)coord.x * mapMagic.tileSize.x, (float)coord.z * mapMagic.tileSize.z);
			if (x > vector2D.x && x < vector2D.x + mapMagic.tileSize.x && z > vector2D.z)
			{
				return z < vector2D.z + mapMagic.tileSize.z;
			}
			return false;
		}

		public Terrain GetTerrain(bool isDraft)
		{
			if (!isDraft)
			{
				return main?.terrain;
			}
			return draft?.terrain;
		}

		public bool ContainsTerrain(Terrain terrain)
		{
			if (!(terrain == draft?.terrain))
			{
				return terrain == main?.terrain;
			}
			return true;
		}

		public void SwitchLod()
		{
			if (this == null)
			{
				return;
			}
			bool flag = main != null;
			bool flag2 = draft != null;
			if (mapMagic.draftsInPlaymode)
			{
				if ((int)distance > mapMagic.mainRange)
				{
					flag = false;
				}
				if ((int)distance > mapMagic.tiles.generateRange && mapMagic.hideFarTerrains)
				{
					flag2 = false;
				}
			}
			else
			{
				if ((int)distance > mapMagic.tiles.generateRange && mapMagic.hideFarTerrains)
				{
					flag = false;
				}
				flag2 = false;
			}
			if (main != null && !main.applyReady)
			{
				flag = false;
			}
			if (draft != null && !draft.applyReady)
			{
				flag2 = false;
			}
			if (flag && flag2 && !main.applyReady)
			{
				flag = false;
			}
			Terrain terrain = (flag ? main.terrain : ((!flag2) ? null : draft.terrain));
			bool flag3 = false;
			if (ActiveTerrain != terrain)
			{
				flag3 = true;
				ActiveTerrain = terrain;
			}
			bool num = flag;
			bool flag4 = objectsPool.isActiveAndEnabled;
			if (!num && flag4)
			{
				objectsPool.gameObject.SetActive(value: false);
			}
			if (num && !flag4)
			{
				objectsPool.gameObject.SetActive(value: true);
			}
			if (flag3 && mapMagic.tiles.Contains(coord))
			{
				if (flag)
				{
					Weld.WeldSurroundingDraftsToThisMain(mapMagic.tiles, coord);
					Weld.WeldCorners(mapMagic.tiles, coord);
				}
				else if (flag2 && draft.applyReady)
				{
					Weld.WeldThisDraftWithSurroundings(mapMagic.tiles, coord);
				}
			}
			if (flag3)
			{
				OnLodSwitched?.Invoke(this, flag, flag2);
			}
		}

		public void ResetTerrain()
		{
			OnBeforeResetTerrain?.Invoke(this);
			bool flag = main != null;
			bool flag2 = draft != null;
			for (int num = base.transform.childCount - 1; num > 0; num--)
			{
				UnityEngine.Object.DestroyImmediate(base.transform.GetChild(num).gameObject);
			}
			if (flag)
			{
				main = new DetailLevel(this, isDraft: false);
			}
			if (flag2)
			{
				draft = new DetailLevel(this, isDraft: true);
			}
			CreateObjectsPool();
			OnAfterResetTerrain?.Invoke(this);
		}

		public static TerrainTile Construct(MapMagicObject mapMagic)
		{
			GameObject obj = new GameObject();
			obj.transform.parent = mapMagic.transform;
			TerrainTile terrainTile = obj.AddComponent<TerrainTile>();
			terrainTile.mapMagic = mapMagic;
			if (MapMagicObject.isPlaying)
			{
				terrainTile.main = new DetailLevel(terrainTile, isDraft: false);
				if (mapMagic.draftsInPlaymode)
				{
					terrainTile.draft = new DetailLevel(terrainTile, isDraft: true);
				}
			}
			terrainTile.CreateObjectsPool();
			return terrainTile;
		}

		public void Pin(bool asDraftOnly)
		{
			if (mapMagic.draftsInEditor && draft == null)
			{
				draft = new DetailLevel(this, isDraft: true);
			}
			if (!asDraftOnly && main == null)
			{
				main = new DetailLevel(this, isDraft: false);
			}
			if (asDraftOnly && main != null)
			{
				main.Remove();
				main = null;
			}
		}

		public void Move(Coord newCoord, float newRemoteness)
		{
			coord = newCoord;
			Stop();
			main?.data?.Clear(clearApply: true, inSubs: true);
			draft?.data?.Clear(clearApply: true, inSubs: true);
			if (main != null)
			{
				main.applyReady = false;
				main.generateReady = false;
				main.generateStarted = false;
			}
			if (draft != null)
			{
				draft.applyReady = false;
				draft.generateReady = false;
				draft.generateStarted = false;
			}
			ActiveTerrain = null;
			Vector3 vector = (Vector3)mapMagic.tileSize;
			Vector3 localPosition = new Vector3((float)coord.x * vector.x, 0f, (float)coord.z * vector.z);
			if (main != null && main.terrain != null && main.terrain.terrainData.size != new Vector3(vector.x, main.terrain.terrainData.size.y, vector.z))
			{
				main.terrain.terrainData.size = new Vector3(vector.x, main.terrain.terrainData.size.y, vector.z);
			}
			if (draft != null && draft.terrain != null && draft.terrain.terrainData.size != new Vector3(vector.x, draft.terrain.terrainData.size.y, vector.z))
			{
				draft.terrain.terrainData.size = new Vector3(vector.x, draft.terrain.terrainData.size.y, vector.z);
			}
			base.transform.localPosition = localPosition;
			base.gameObject.name = "Tile " + coord.x + "," + coord.z;
			Dist(newRemoteness);
			OnTileMoved?.Invoke(this);
		}

		public void Dist(float newRemoteness)
		{
			distance = newRemoteness;
			if (MapMagicObject.isPlaying)
			{
				if (main != null && !main.generateStarted && (int)distance <= mapMagic.mainRange)
				{
					StartGenerate(mapMagic.graph, generateMain: true, generateLod: false);
				}
				if (draft != null && !draft.generateStarted && (int)distance <= mapMagic.tiles.generateRange)
				{
					StartGenerate(mapMagic.graph, generateMain: false);
				}
				if (coord != new Coord(2147483647, 2147483647))
				{
					SwitchLod();
				}
			}
			else
			{
				if (draft != null && !draft.generateStarted)
				{
					StartGenerate(mapMagic.graph, generateMain: false);
				}
				if (main != null && !main.generateStarted)
				{
					StartGenerate(mapMagic.graph, generateMain: true, generateLod: false);
				}
			}
		}

		public void Remove()
		{
			Stop();
			UnityEngine.Object.Destroy(base.gameObject);
		}

		public void Resize()
		{
			Move(coord, distance);
		}

		public Terrain CreateTerrain(bool isDraft)
		{
			GameObject obj = new GameObject();
			obj.transform.parent = base.transform;
			obj.transform.localPosition = new Vector3(0f, 0f, 0f);
			obj.name = (isDraft ? "Draft Terrain" : "Main Terrain");
			Terrain terrain = obj.AddComponent<Terrain>();
			TerrainCollider terrainCollider = obj.AddComponent<TerrainCollider>();
			TerrainData terrainData = Resources.Load<TerrainData>("MapMagicDefaultTerrainData");
			TerrainData terrainData2 = (terrainCollider.terrainData = (terrain.terrainData = ((!(terrainData != null)) ? new TerrainData() : UnityEngine.Object.Instantiate(terrainData))));
			terrainData2.size = (Vector3)mapMagic.tileSize;
			mapMagic.terrainSettings.ApplyAll(terrain);
			terrain.groupingID = (isDraft ? (-2) : (-1));
			return terrain;
		}

		public void CreateObjectsPool()
		{
			GameObject gameObject = new GameObject();
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = default(Vector3);
			gameObject.name = "Objects";
			objectsPool = gameObject.AddComponent<ObjectsPool>();
		}

		public void Refresh(Graph graph, bool clearAll = false)
		{
			if (main != null)
			{
				StopTask(main);
			}
			ClearChanged(graph, clearAll);
			StartGenerate(graph);
		}

		public void ClearChanged(Graph graph, bool clearAll = false)
		{
			if (clearAll)
			{
				Stop();
				main?.data?.Clear(clearApply: true, inSubs: true);
				draft?.data?.Clear(clearApply: true, inSubs: true);
			}
			if (main?.data != null)
			{
				graph.ClearChanged(main.data, clearAll);
			}
			if (draft?.data != null)
			{
				graph.ClearChanged(draft.data, clearAll);
			}
		}

		public void StartGenerate(Graph graph, bool generateMain = true, bool generateLod = true)
		{
			if (graph == null)
			{
				return;
			}
			if (generateLod && draft != null)
			{
				if (draft.data == null)
				{
					draft.data = new TileData();
				}
				draft.data.area = new Area(coord, (int)mapMagic.draftResolution, mapMagic.draftMargins, mapMagic.tileSize);
				draft.data.globals = mapMagic.globals;
				draft.data.random = graph.random;
				draft.data.isPreview = false;
				draft.data.isDraft = true;
				draft.generateStarted = true;
				draft.applyReady = false;
				draft.generateReady = false;
				EnqueueDraftTask(draft, graph, Priority + 1000, "Draft");
			}
			if (generateMain && main != null)
			{
				if (main.data == null)
				{
					main.data = new TileData();
				}
				main.data.area = new Area(coord, (int)mapMagic.tileResolution, mapMagic.tileMargins, mapMagic.tileSize);
				main.data.globals = mapMagic.globals;
				main.data.random = graph.random;
				main.data.isPreview = mapMagic.PreviewData == main.data;
				main.data.isDraft = false;
				main.generateStarted = true;
				main.applyReady = false;
				main.generateReady = false;
				EnqueueMainTask(main, graph, Priority, "Main");
			}
			SwitchLod();
		}

		private void EnqueueMainTask(DetailLevel det, Graph graph, int priority = 0, string name = "Task")
		{
			if (det.task == null || !det.task.Enqueued)
			{
				Prepare(graph, this, main);
				det.stop = new StopToken();
				StopToken stop = det.stop;
				DetailLevel detailLevel = det;
				ThreadManager.Task obj = new ThreadManager.Task
				{
					action = delegate
					{
						Generate(graph, this, det, stop);
					},
					priority = priority
				};
				Coord coord = this.coord;
				obj.name = name + " " + coord.ToString();
				detailLevel.task = obj;
				ThreadManager.Enqueue(det.task);
			}
			det.task.priority = priority;
		}

		private void EnqueueDraftTask(DetailLevel det, Graph graph, int priority = 0, string name = "Task")
		{
			if (det.task == null)
			{
				det.stop = new StopToken();
				DetailLevel detailLevel = det;
				ThreadManager.Task obj = new ThreadManager.Task
				{
					action = delegate
					{
						Generate(graph, this, det, det.stop);
					},
					priority = priority
				};
				Coord coord = this.coord;
				obj.name = name + " " + coord.ToString();
				detailLevel.task = obj;
			}
			det.task.priority = priority;
			if (det.task.Active)
			{
				det.stop.restart = true;
			}
			else if (!det.task.Enqueued)
			{
				Prepare(graph, this, det);
				ThreadManager.Enqueue(det.task);
			}
		}

		private void StopTask(DetailLevel det, bool dequeue = true)
		{
			if (det.applyMainCoroutines == null)
			{
				det.applyMainCoroutines = new Stack<CoroutineManager.Task>();
			}
			while (det.applyMainCoroutines.Count != 0)
			{
				CoroutineManager.Stop(det.applyMainCoroutines.Pop());
			}
			if (det.switchLodCoroutine != null)
			{
				CoroutineManager.Stop(det.switchLodCoroutine);
			}
			if (det.coroutine != null)
			{
				CoroutineManager.Stop(det.coroutine);
			}
			if (dequeue && det.task != null)
			{
				ThreadManager.Dequeue(det.task);
			}
			if (det.task != null && det.task.Active && det.stop != null)
			{
				det.stop.stop = true;
				det.stop.restart = false;
			}
			if (dequeue)
			{
				det.task = null;
			}
		}

		public void Stop()
		{
			if (main != null)
			{
				StopTask(main);
			}
			if (draft != null)
			{
				StopTask(draft);
			}
		}

		[Obsolete]
		private void StopEnqueueTask(DetailLevel det, Graph graph, int priority = 0, string name = "Task")
		{
			if (det.applyMainCoroutines == null)
			{
				det.applyMainCoroutines = new Stack<CoroutineManager.Task>();
			}
			while (det.applyMainCoroutines.Count != 0)
			{
				CoroutineManager.Stop(det.applyMainCoroutines.Pop());
			}
			if (det.switchLodCoroutine != null)
			{
				CoroutineManager.Stop(det.switchLodCoroutine);
			}
			if (det.coroutine != null)
			{
				CoroutineManager.Stop(det.coroutine);
			}
			_ = det.task;
			if (det.task != null && det.task.Active)
			{
				det.stop.stop = true;
			}
			if (det.task == null || !det.task.Enqueued)
			{
				Prepare(graph, this, main);
				det.stop = new StopToken();
				StopToken stop = det.stop;
				DetailLevel detailLevel = det;
				ThreadManager.Task obj = new ThreadManager.Task
				{
					action = delegate
					{
						Generate(graph, this, det, stop);
					},
					priority = priority
				};
				Coord coord = this.coord;
				obj.name = name + " " + coord.ToString();
				detailLevel.task = obj;
				ThreadManager.Enqueue(det.task);
			}
			det.task.priority = priority;
		}

		private void Prepare(Graph graph, TerrainTile tile, DetailLevel det)
		{
			det.edges.ready = false;
			OnBeforeTilePrepare?.Invoke(tile, det.data);
			graph.Prepare(det.data, det.terrain);
		}

		private void Generate(Graph graph, TerrainTile tile, DetailLevel det, StopToken stop)
		{
			OnBeforeTileGenerate?.Invoke(tile, det.data, stop);
			if (!stop.stop)
			{
				graph.Generate(det.data, stop);
			}
			if (!stop.stop)
			{
				graph.Finalize(det.data, stop);
			}
			OnTileFinalized?.Invoke(tile, det.data, stop);
			if (MapMagicObject.isPlaying)
			{
				det.data.Clear(clearApply: false, inSubs: true);
			}
			if (!stop.stop)
			{
				Weld.ReadEdges(det.data, det.edges);
			}
			if (!stop.stop)
			{
				Weld.WeldEdgesInThread(det.edges, tile.mapMagic.tiles, tile.coord, det.data.isDraft);
			}
			if (!stop.stop)
			{
				Weld.WriteEdges(det.data, det.edges);
			}
			if (det.data.isDraft)
			{
				DetailLevel detailLevel = det;
				Action action = delegate
				{
					ApplyNow(det, stop);
				};
				int priority = Priority + 1000;
				Coord coord = this.coord;
				detailLevel.coroutine = CoroutineManager.Enqueue(action, priority, "ApplyNow " + coord.ToString());
			}
			else
			{
				IEnumerator routine = ApplyRoutine(det, stop);
				DetailLevel detailLevel2 = det;
				int priority2 = Priority;
				Coord coord = this.coord;
				detailLevel2.coroutine = CoroutineManager.Enqueue(routine, priority2, "ApplyRoutine " + coord.ToString());
			}
			det.generateReady = true;
		}

		private void ApplyNow(DetailLevel det, StopToken stop)
		{
			if (this == null)
			{
				return;
			}
			if (stop == null || !stop.stop)
			{
				while (det.data.ApplyMarksCount != 0)
				{
					det.data.DequeueApply().Apply(det.terrain);
				}
				det.applyReady = true;
				SwitchLod();
				OnTileApplied?.Invoke(this, det.data, stop);
				if (!ThreadManager.IsWorking && CoroutineManager.IsQueueEmpty)
				{
					OnAllComplete?.Invoke(mapMagic);
				}
			}
			if (stop.restart)
			{
				stop.restart = false;
				if (!det.task.Enqueued)
				{
					ThreadManager.Enqueue(det.task);
				}
			}
		}

		private IEnumerator ApplyRoutine(DetailLevel det, StopToken stop)
		{
			if (this == null)
			{
				yield break;
			}
			if (stop == null || !stop.stop)
			{
				while (det.data.ApplyMarksCount != 0)
				{
					if (stop != null && stop.stop)
					{
						yield break;
					}
					IApplyData applyData = det.data.DequeueApply();
					if (applyData is IApplyDataRoutine)
					{
						IEnumerator routine = (applyData as IApplyDataRoutine).ApplyRoutine(det.terrain);
						bool move;
						do
						{
							if (stop != null && stop.stop)
							{
								yield break;
							}
							move = routine.MoveNext();
							yield return null;
						}
						while (move);
					}
					else
					{
						applyData.Apply(det.terrain);
						yield return null;
					}
				}
			}
			if (stop == null || (!stop.stop && !stop.restart))
			{
				det.applyReady = true;
				SwitchLod();
				OnTileApplied?.Invoke(this, det.data, stop);
				if (!ThreadManager.IsWorking && CoroutineManager.IsQueueEmpty)
				{
					OnAllComplete?.Invoke(mapMagic);
				}
			}
			if (stop != null && stop.restart)
			{
				stop.restart = false;
				if (!det.task.Enqueued)
				{
					ThreadManager.Enqueue(det.task);
				}
			}
		}

		public (float progress, float max) GetProgress(Graph graph, float generateComplexity, float applyComplexity)
		{
			float num = 0f;
			float num2 = 0f;
			if (main != null && main.generateStarted)
			{
				num2 += generateComplexity + applyComplexity;
				if (main.generateReady)
				{
					num += generateComplexity;
				}
				else if (main.data != null)
				{
					num += graph.GetGenerateProgress(main.data);
				}
				if (main.applyReady)
				{
					num += applyComplexity;
				}
				else if (main.data != null)
				{
					num += graph.GetApplyProgress(main.data);
				}
			}
			if (draft != null && draft.generateStarted)
			{
				num2 += 2f;
				if (draft.generateReady)
				{
					num += 1f;
				}
				if (draft.applyReady)
				{
					num += 1f;
				}
			}
			return (progress: num, max: num2);
		}

		public void OnBeforeSerialize()
		{
			serialized_main = main;
			serialized_mainNull = main == null;
			serialized_draft = draft;
			serialized_draftNull = draft == null;
		}

		public void OnAfterDeserialize()
		{
			if (!serialized_mainNull)
			{
				main = serialized_main;
				if (!main.applyReady || !main.generateReady)
				{
					main.applyReady = false;
					main.generateReady = false;
				}
			}
			if (!serialized_draftNull)
			{
				draft = serialized_draft;
				if (!draft.applyReady || !draft.generateReady)
				{
					draft.applyReady = false;
					draft.generateReady = false;
				}
			}
		}

		public void OnDrawGizmos_Tmp()
		{
			Gizmos.color = Color.blue;
			Vector3 vector = (Vector3)(this.coord.vector2d * mapMagic.tileSize.x + mapMagic.tileSize / 2f);
			Gizmos.DrawWireCube(vector, (Vector3)mapMagic.tileSize);
			vector.y += 150f;
			Gizmos.color = Color.red;
			if (draft != null && ActiveTerrain == draft.terrain)
			{
				Gizmos.color = Color.yellow;
			}
			if (main != null && ActiveTerrain == main.terrain)
			{
				Gizmos.color = Color.green;
			}
			Gizmos.DrawCube(vector + new Vector3(-150f, 0f, 0f), new Vector3(60f, 60f, 60f));
			Gizmos.color = Color.black;
			if (main != null)
			{
				Gizmos.color = Color.green;
				if (!main.applyReady)
				{
					if (main.task.Enqueued)
					{
						Gizmos.color = Color.red;
					}
					if (main.task.Active)
					{
						Gizmos.color = new Color(0.8f, 0.3f, 0f, 1f);
					}
					if (main.applyMainCoroutines != null)
					{
						foreach (CoroutineManager.Task applyMainCoroutine in main.applyMainCoroutines)
						{
							if (applyMainCoroutine.Active || applyMainCoroutine.Enqueued)
							{
								Gizmos.color = Color.yellow;
							}
						}
					}
				}
			}
			Gizmos.DrawSphere(vector + new Vector3(-30f, 0f, 0f), 60f);
			Gizmos.color = Color.black;
			if (draft != null)
			{
				Gizmos.color = Color.green;
				if (!draft.applyReady)
				{
					if (draft.task.Enqueued)
					{
						Gizmos.color = Color.red;
					}
					if (draft.task.Active)
					{
						Gizmos.color = new Color(0.8f, 0.3f, 0f, 1f);
					}
					if (draft.applyMainCoroutines != null)
					{
						foreach (CoroutineManager.Task applyMainCoroutine2 in draft.applyMainCoroutines)
						{
							if (applyMainCoroutine2.Active || applyMainCoroutine2.Enqueued)
							{
								Gizmos.color = Color.yellow;
							}
						}
					}
				}
			}
			Gizmos.DrawSphere(vector + new Vector3(90f, 0f, 0f), 40f);
			Coord coord = this.coord;
			if (CoroutineManager.IsNameEnqueued("LodSwitch " + coord.ToString()))
			{
				Gizmos.color = Color.red;
			}
			else
			{
				coord = this.coord;
				if (CoroutineManager.IsNameActive("LodSwitch " + coord.ToString()))
				{
					Gizmos.color = Color.yellow;
				}
				else
				{
					Gizmos.color = Color.green;
				}
			}
			Gizmos.DrawSphere(vector + new Vector3(180f, 0f, 0f), 30f);
		}
	}
}
