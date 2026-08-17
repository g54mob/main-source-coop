using System;
using System.Collections.Generic;
using CW.Common;
using PaintCore;
using UnityEngine;

namespace PaintIn3D
{
	[RequireComponent(typeof(Renderer))]
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwMeshModel")]
	public abstract class CwMeshModel : CwModel
	{
		public enum UseMeshType
		{
			AsIs = 0,
			AutoSeamFix = 1
		}

		[SerializeField]
		protected bool includeScale = true;

		[SerializeField]
		protected UseMeshType useMesh;

		[NonSerialized]
		private SkinnedMeshRenderer cachedSkinned;

		[NonSerialized]
		private bool cachedFilterSet;

		[NonSerialized]
		private MeshFilter cachedFilter;

		[NonSerialized]
		private bool cachedSkinnedSet;

		[NonSerialized]
		private Material[] materials;

		[NonSerialized]
		private bool materialsSet;

		[NonSerialized]
		protected Mesh bakedMesh;

		[NonSerialized]
		protected bool bakedMeshSet;

		[NonSerialized]
		protected Mesh preparedMesh;

		[NonSerialized]
		protected Matrix4x4 preparedMatrix;

		[NonSerialized]
		private int[] preparedTriangles;

		[NonSerialized]
		private Vector3[] preparedPositions;

		[NonSerialized]
		private Vector2[] preparedCoord0;

		[NonSerialized]
		private Vector2[] preparedCoord1;

		[NonSerialized]
		protected static List<Vector3> tempVertices = new List<Vector3>();

		public virtual bool IncludeScale
		{
			get
			{
				return includeScale;
			}
			set
			{
				includeScale = value;
			}
		}

		public UseMeshType UseMesh
		{
			get
			{
				return useMesh;
			}
			set
			{
				useMesh = value;
			}
		}

		public Mesh PreparedMesh => preparedMesh;

		public Material[] Materials
		{
			get
			{
				if (!materialsSet)
				{
					materials = base.CachedRenderer.sharedMaterials;
					materialsSet = true;
				}
				return materials;
			}
		}

		public int GetMaterialIndex(Material material)
		{
			if (material != null)
			{
				Material[] array = Materials;
				for (int num = array.Length - 1; num >= 0; num--)
				{
					if (array[num] == material)
					{
						if (base.CachedRenderer.isPartOfStaticBatch)
						{
							MeshRenderer meshRenderer = base.CachedRenderer as MeshRenderer;
							if (meshRenderer != null)
							{
								return meshRenderer.subMeshStartIndex + num;
							}
						}
						return num;
					}
				}
			}
			return -1;
		}

		public override void RemoveComponents()
		{
		}

		protected override void CacheRenderer()
		{
			base.CacheRenderer();
			if (!TryCacheRenderer())
			{
				Debug.LogError("This CwModel/CwPaintable (" + base.name + ") doesn't have a suitable Renderer, so it cannot be painted.", this);
			}
		}

		private bool TryCacheRenderer()
		{
			if (cachedRenderer is SkinnedMeshRenderer)
			{
				cachedSkinned = (SkinnedMeshRenderer)cachedRenderer;
				cachedSkinnedSet = true;
				return true;
			}
			if (cachedRenderer is MeshRenderer)
			{
				cachedFilter = GetComponent<MeshFilter>();
				cachedFilterSet = true;
				return true;
			}
			return false;
		}

		[ContextMenu("Dirty Materials")]
		public void DirtyMaterials()
		{
			materialsSet = false;
		}

		public void GetPreparedPoints(int triangleIndex, ref Vector3 pointA, ref Vector3 pointB, ref Vector3 pointC)
		{
			if (prepared && preparedMesh != null)
			{
				if (preparedPositions == null)
				{
					preparedPositions = preparedMesh.vertices;
				}
				if (preparedTriangles == null)
				{
					preparedTriangles = preparedMesh.triangles;
				}
				pointA = preparedPositions[preparedTriangles[triangleIndex * 3]];
				pointB = preparedPositions[preparedTriangles[triangleIndex * 3 + 1]];
				pointC = preparedPositions[preparedTriangles[triangleIndex * 3 + 2]];
			}
		}

		public void GetPreparedCoords0(int triangleIndex, ref Vector2 coordA, ref Vector2 coordB, ref Vector2 coordC)
		{
			if (prepared && preparedMesh != null)
			{
				if (preparedTriangles == null)
				{
					preparedTriangles = preparedMesh.triangles;
				}
				if (preparedCoord0 == null)
				{
					preparedCoord0 = preparedMesh.uv;
				}
				coordA = preparedCoord0[preparedTriangles[triangleIndex * 3]];
				coordB = preparedCoord0[preparedTriangles[triangleIndex * 3 + 1]];
				coordC = preparedCoord0[preparedTriangles[triangleIndex * 3 + 2]];
			}
		}

		public void GetPreparedCoords1(int triangleIndex, ref Vector2 coordA, ref Vector2 coordB, ref Vector2 coordC)
		{
			if (prepared && preparedMesh != null)
			{
				if (preparedTriangles == null)
				{
					preparedTriangles = preparedMesh.triangles;
				}
				if (preparedCoord1 == null)
				{
					preparedCoord1 = preparedMesh.uv;
				}
				coordA = preparedCoord1[preparedTriangles[triangleIndex * 3]];
				coordB = preparedCoord1[preparedTriangles[triangleIndex * 3 + 1]];
				coordC = preparedCoord1[preparedTriangles[triangleIndex * 3 + 2]];
			}
		}

		public override void GetPrepared(ref Mesh mesh, ref Matrix4x4 matrix, CwCoord coord)
		{
			if (!prepared)
			{
				prepared = true;
				if (!cachedRendererSet)
				{
					CacheRenderer();
				}
				TryGetPrepared(coord);
			}
			mesh = preparedMesh;
			matrix = preparedMatrix;
		}

		private void TryGetPrepared(CwCoord coord)
		{
			if (cachedSkinnedSet)
			{
				if (!bakedMeshSet)
				{
					bakedMesh = new Mesh();
					bakedMeshSet = true;
				}
				if (useMesh == UseMeshType.AutoSeamFix)
				{
					Mesh sharedMesh = cachedSkinned.sharedMesh;
					if (sharedMesh != null && !sharedMesh.name.EndsWith("Fixed Seams)") && !sharedMesh.name.EndsWith("(Fixed)"))
					{
						cachedSkinned.sharedMesh = CwMeshFixer.GetCachedMesh(sharedMesh, coord);
					}
				}
				Vector3 lossyScale = cachedTransform.lossyScale;
				Vector3 vector = new Vector3(CwHelper.Reciprocal(lossyScale.x), CwHelper.Reciprocal(lossyScale.y), CwHelper.Reciprocal(lossyScale.z));
				Vector3 localScale = cachedTransform.localScale;
				cachedTransform.localScale = Vector3.one;
				cachedSkinned.BakeMesh(bakedMesh);
				cachedTransform.localScale = localScale;
				preparedMesh = bakedMesh;
				preparedMatrix = cachedRenderer.localToWorldMatrix;
				if (includeScale)
				{
					preparedMatrix *= Matrix4x4.Scale(vector);
				}
			}
			else if (cachedFilterSet)
			{
				preparedMesh = cachedFilter.sharedMesh;
				preparedMatrix = cachedRenderer.localToWorldMatrix;
				if (useMesh == UseMeshType.AutoSeamFix)
				{
					preparedMesh = CwMeshFixer.GetCachedMesh(preparedMesh, coord);
				}
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			CwHelper.Destroy(bakedMesh);
		}
	}
}
