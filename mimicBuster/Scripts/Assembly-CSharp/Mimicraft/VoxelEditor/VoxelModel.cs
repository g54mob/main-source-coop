using System.Diagnostics;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
	public class VoxelModel : MonoBehaviour
	{
		[SerializeField]
		private Color32 initialColor = new Color32(139, 90, 43, byte.MaxValue);

		[SerializeField]
		private int initialSize = 3;

		[SerializeField]
		private string displayName;

		[Tooltip("World-space size of one voxel unit. Drives the model's transform scale directly, so every grid coordinate still maps 1:1 to local space.")]
		[SerializeField]
		[Min(0.001f)]
		private float voxelSize = 0.3f;

		[Tooltip("Assigned once by VoxelTestSceneSetup. SetUnlit() swaps the renderer between these two - see VoxelEditorSettings.UnlitMode.")]
		[SerializeField]
		private Material unlitMaterial;

		[SerializeField]
		private Material litMaterial;

		private readonly VoxelMeshCache meshCache = new VoxelMeshCache();

		private MeshFilter meshFilter;

		private MeshRenderer meshRenderer;

		private MeshCollider meshCollider;

		private VoxelGrid grid;

		private bool initialised;

		public string DisplayName
		{
			get
			{
				if (!string.IsNullOrEmpty(displayName))
				{
					return displayName;
				}
				return base.gameObject.name;
			}
			set
			{
				displayName = value;
			}
		}

		public VoxelGrid Grid
		{
			get
			{
				EnsureInitialised();
				return grid;
			}
			private set
			{
				grid = value;
			}
		}

		public IVoxelEditBounds EditBounds { get; set; }

		public float VoxelSize => voxelSize;

		public Material UnlitMaterial => unlitMaterial;

		public Material LitMaterial => litMaterial;

		public int InitialSize => initialSize;

		public Color32 InitialColor => initialColor;

		public float InitialWorldSize => (float)initialSize * voxelSize;

		public static int MeshBuildCount { get; private set; }

		public static double MeshBuildMsTotal { get; private set; }

		public static int ColliderBakeCount { get; private set; }

		public static double ColliderBakeMsTotal { get; private set; }

		public static double LastColliderBakeMs { get; private set; }

		private void EnsureInitialised()
		{
			if (!initialised)
			{
				initialised = true;
				ApplyVoxelSize();
				meshFilter = GetComponent<MeshFilter>();
				meshRenderer = GetComponent<MeshRenderer>();
				meshCollider = GetComponent<MeshCollider>();
				if (meshCollider != null)
				{
					meshCollider.convex = false;
				}
				ResetToDefaultCube();
			}
		}

		public bool CanAdd(Vector3Int cell)
		{
			if (EditBounds != null)
			{
				return EditBounds.CanAdd(cell);
			}
			return true;
		}

		public bool CanRemove(Vector3Int cell)
		{
			if (EditBounds != null)
			{
				return EditBounds.CanRemove(cell);
			}
			return true;
		}

		public Vector3 GetInitialBoundsCenterLocal()
		{
			return new Vector3(initialSize, initialSize, initialSize) * 0.5f;
		}

		public Vector3 GetCurrentBoundsCenterLocal()
		{
			if (!GridBounds.TryCompute(Grid, out var min, out var max))
			{
				return GetInitialBoundsCenterLocal();
			}
			return ((Vector3)min + (Vector3)max + Vector3.one) * 0.5f;
		}

		public void ConfigureVoxelSize(float size)
		{
			voxelSize = size;
			ApplyVoxelSize();
		}

		public void ConfigureInitialSize(int size)
		{
			initialSize = Mathf.Max(1, size);
		}

		public void ConfigureMaterials(Material unlit, Material lit)
		{
			unlitMaterial = unlit;
			litMaterial = lit;
		}

		public void SetUnlit(bool unlit)
		{
			EnsureInitialised();
			Material material = (unlit ? unlitMaterial : litMaterial);
			if (material != null && meshRenderer != null)
			{
				meshRenderer.sharedMaterial = material;
			}
		}

		private void Awake()
		{
			EnsureInitialised();
		}

		public void ResetToDefaultCube()
		{
			EnsureInitialised();
			grid = VoxelGrid.CreateDefaultCube(initialColor, initialSize);
			RebuildMesh();
		}

		private void OnValidate()
		{
			ApplyVoxelSize();
		}

		private void ApplyVoxelSize()
		{
			base.transform.localScale = Vector3.one * voxelSize;
		}

		public void RebuildMesh(bool updateCollider = true)
		{
			EnsureInitialised();
			Stopwatch stopwatch = Stopwatch.StartNew();
			meshFilter.sharedMesh = meshCache.Build(grid);
			stopwatch.Stop();
			MeshBuildCount++;
			MeshBuildMsTotal += stopwatch.Elapsed.TotalMilliseconds;
			if (updateCollider)
			{
				RefreshCollider();
			}
		}

		public static void ResetBuildStats()
		{
			MeshBuildCount = 0;
			MeshBuildMsTotal = 0.0;
			ColliderBakeCount = 0;
			ColliderBakeMsTotal = 0.0;
			LastColliderBakeMs = 0.0;
		}

		public void RefreshCollider()
		{
			EnsureInitialised();
			Stopwatch stopwatch = Stopwatch.StartNew();
			meshCollider.sharedMesh = null;
			meshCollider.sharedMesh = meshFilter.sharedMesh;
			stopwatch.Stop();
			ColliderBakeCount++;
			LastColliderBakeMs = stopwatch.Elapsed.TotalMilliseconds;
			ColliderBakeMsTotal += LastColliderBakeMs;
		}
	}
}
