using System;
using Den.Tools;
using Den.Tools.Tasks;
using MapMagic.Locks;
using MapMagic.Nodes;
using MapMagic.Products;
using MapMagic.Terrains;
using UnityEngine;
using UnityEngine.Rendering;

namespace MapMagic.Core
{
	[SelectionBase]
	[ExecuteInEditMode]
	[HelpURL("https://gitlab.com/denispahunov/mapmagic/wikis/home")]
	[DisallowMultipleComponent]
	public class MapMagicObject : MonoBehaviour, IMapMagic, ISerializationCallbackReceiver
	{
		public enum Resolution
		{
			_33 = 33,
			_65 = 65,
			_129 = 129,
			_257 = 257,
			_513 = 513,
			_1025 = 1025,
			_2049 = 2049
		}

		public static readonly SemVer version = new SemVer(2, 1, 16);

		public Graph graph;

		public bool guiOpenObjectSelector;

		public bool guiTiles = true;

		public TerrainTileManager tiles = new TerrainTileManager
		{
			allowMove = true
		};

		public Vector2D tileSize = new Vector2D(1000f, 1000f);

		[SerializeField]
		private Coord previewCoord;

		[SerializeField]
		private bool previewAssigned;

		public Resolution tileResolution = Resolution._513;

		public int tileMargins = 16;

		public bool draftsInEditor = true;

		public bool draftsInPlaymode = true;

		public Resolution draftResolution = Resolution._65;

		public int draftMargins = 2;

		public bool guiLocks;

		public Lock[] locks = new Lock[0];

		public bool guiInfiniteTerrains;

		public bool hideFarTerrains = true;

		public int mainRange = 1;

		public bool clearOnNodeRemove = true;

		public TerrainSettings terrainSettings = new TerrainSettings();

		public Globals globals = new Globals();

		public bool guiSettings;

		public bool guiTileSettings;

		public bool guiOutputsSettings;

		public bool guiExposedVariables;

		public bool guiTerrainSettings;

		public bool guiDraftSettings;

		public bool guiTreesGrassSettings;

		public bool instantGenerate = true;

		public bool saveIntermediate = true;

		public int heightWeldMargins = 5;

		public int splatsWeldMargins = 2;

		public bool guiHideWireframe;

		public bool guiThreads;

		[NonSerialized]
		public bool guiDraggingField;

		public bool applyColliders = true;

		public bool setDirty;

		public bool shift;

		public int shiftThreshold = 4000;

		public int shiftExcludeLayers;

		public static bool isPlaying = true;

		public bool serializedMultithreading = true;

		public int serializedMaxThreads = 3;

		public bool serializedAutoMaxThreads = true;

		public float serializedMaxApplyTime = 10f;

		public Graph Graph => graph;

		public int DraftRange => tiles.generateRange;

		public Globals Globals => globals;

		public TerrainTile PreviewTile
		{
			get
			{
				TerrainTile terrainTile = null;
				if (previewAssigned)
				{
					terrainTile = tiles[previewCoord];
				}
				if (terrainTile == null)
				{
					terrainTile = tiles.ClosestMain();
				}
				return terrainTile;
			}
		}

		public TileData PreviewData => PreviewTile?.main.data;

		public Terrain PreviewTerrain => PreviewTile?.main?.terrain;

		public TerrainTile AssignedPreviewTile => tiles[previewCoord];

		public TileData AssignedPreviewData => AssignedPreviewTile?.main.data;

		public Terrain AssignedPreviewTerrain => AssignedPreviewTile?.main?.terrain;

		public void OnEnable()
		{
			if (terrainSettings.material == null)
			{
				terrainSettings.material = DefaultTerrainMaterial();
			}
			StopGenerate();
			StartGenerateNonReady();
		}

		public void OnDisable()
		{
		}

		public void EditorUpdate()
		{
		}

		public void Update()
		{
			Debug.developerConsoleVisible = true;
			tiles.Update((Vector3)tileSize, tiles.pinned, this, !isPlaying);
			CoroutineManager.Update();
		}

		public void ApplyTileSettings()
		{
			StopGenerate();
			foreach (TerrainTile item in tiles.Tiles())
			{
				item.Resize();
			}
			Refresh(clearAll: true);
		}

		public void ApplyTerrainSettings()
		{
			foreach (TerrainTile item in tiles.All())
			{
				if (item.main != null)
				{
					terrainSettings.ApplyAll(item.main.terrain);
				}
				if (item.draft != null)
				{
					terrainSettings.ApplyAll(item.draft.terrain);
					item.draft.terrain.groupingID = -1;
				}
			}
		}

		public Material DefaultTerrainMaterial()
		{
			Shader shader = GraphicsSettings.defaultRenderPipeline?.defaultTerrainMaterial?.shader;
			if (shader == null)
			{
				shader = Shader.Find("HDRP/TerrainLit");
			}
			if (shader == null)
			{
				shader = Shader.Find("Nature/Terrain/Standard");
			}
			if (shader == null)
			{
				shader = Shader.Find("Lightweight Render Pipeline/Terrain/Lit");
			}
			return new Material(shader);
		}

		public void AssignPreviewTile(TerrainTile tile)
		{
			previewCoord = tile.coord;
			previewAssigned = true;
		}

		public void ClearPreviewTile()
		{
			previewAssigned = false;
		}

		public void Refresh(bool clearAll = false)
		{
			if (graph == null)
			{
				return;
			}
			if (instantGenerate)
			{
				foreach (TerrainTile item in tiles.All())
				{
					item.Refresh(graph, clearAll);
				}
				return;
			}
			StopGenerate();
			ClearChanged(clearAll);
		}

		private void ClearChanged(bool clearAll = false)
		{
			foreach (TerrainTile item in tiles.All())
			{
				item.ClearChanged(graph, clearAll);
			}
		}

		public bool ContainsGraph(Graph graph)
		{
			if (this.graph == null)
			{
				return false;
			}
			if (this.graph.generators == null)
			{
				return false;
			}
			if (this.graph == graph || this.graph.ContainsSubGraph(graph, recursively: true))
			{
				return true;
			}
			return false;
		}

		[Obsolete]
		public void Purge(OutputGenerator outGen, bool main = true, bool draft = true)
		{
			foreach (TerrainTile item in tiles.Tiles())
			{
				if (item.main?.data != null)
				{
					outGen.ClearApplied(item.main.data, item.main.terrain);
				}
				if (item.draft?.data != null)
				{
					outGen.ClearApplied(item.draft.data, item.draft.terrain);
				}
			}
		}

		public void ResetTerrains()
		{
			if (!clearOnNodeRemove)
			{
				return;
			}
			foreach (TerrainTile item in tiles.Tiles())
			{
				item.ResetTerrain();
			}
		}

		public void SwitchLods()
		{
			foreach (TerrainTile item in tiles.All())
			{
				item.SwitchLod();
			}
		}

		public void EnableEditorDrafts(bool enabled)
		{
			foreach (TerrainTile item in tiles.All())
			{
				if (enabled && item.draft == null)
				{
					item.draft = new TerrainTile.DetailLevel(item, isDraft: true);
				}
				if (!enabled && item.draft != null)
				{
					item.draft.Remove();
					item.draft = null;
				}
			}
		}

		public bool IsGenerating()
		{
			foreach (TerrainTile item in tiles.All())
			{
				if (item.IsGenerating)
				{
					return true;
				}
			}
			return false;
		}

		public float GetProgress()
		{
			float generateComplexity = graph.GetGenerateComplexity();
			float applyComplexity = graph.GetApplyComplexity();
			float num = 0f;
			float num2 = 0f;
			foreach (TerrainTile item in tiles.All())
			{
				(float, float) progress = item.GetProgress(graph, generateComplexity, applyComplexity);
				num2 += progress.Item1;
				num += progress.Item2;
			}
			return num2 / num;
		}

		public void StartGenerate(bool main = true, bool draft = true)
		{
			if (graph == null)
			{
				throw new Exception("MapMagic: Graph data is not assigned");
			}
			if (!(draft || main))
			{
				return;
			}
			foreach (TerrainTile item in tiles.All())
			{
				item.StartGenerate(graph, main, draft);
			}
		}

		public void StartGenerate(TerrainTile tile, bool generateMain = true, bool generateLod = true)
		{
			if (instantGenerate)
			{
				tile.StartGenerate(graph, generateMain, generateLod);
			}
		}

		private void StartGenerateNonReady()
		{
			if (graph == null)
			{
				Debug.LogWarning("MapMagic: Graph data is not assigned");
			}
			foreach (TerrainTile item in tiles.All())
			{
				if (!item.Ready)
				{
					item.StartGenerate(graph);
				}
			}
		}

		private void StopGenerate()
		{
			if (!(graph != null))
			{
				return;
			}
			foreach (TerrainTile item in tiles.All())
			{
				item.Stop();
			}
		}

		public virtual void OnBeforeSerialize()
		{
			serializedMultithreading = ThreadManager.useMultithreading;
			serializedMaxThreads = ThreadManager.maxThreads;
			serializedAutoMaxThreads = ThreadManager.autoMaxThreads;
			serializedMaxApplyTime = CoroutineManager.timePerFrame;
		}

		public virtual void OnAfterDeserialize()
		{
			ThreadManager.useMultithreading = serializedMultithreading;
			ThreadManager.maxThreads = serializedMaxThreads;
			ThreadManager.autoMaxThreads = serializedAutoMaxThreads;
			CoroutineManager.timePerFrame = serializedMaxApplyTime;
		}
	}
}
