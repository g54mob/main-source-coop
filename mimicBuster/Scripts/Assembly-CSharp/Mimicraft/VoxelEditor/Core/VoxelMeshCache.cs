using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Mimicraft.VoxelEditor.Core
{
	public sealed class VoxelMeshCache
	{
		private sealed class ChunkGeometry
		{
			public readonly List<Vector3> Vertices = new List<Vector3>();

			public readonly List<Vector3> Normals = new List<Vector3>();

			public readonly List<Color32> Colors = new List<Color32>();

			public void Clear()
			{
				Vertices.Clear();
				Normals.Clear();
				Colors.Clear();
			}
		}

		private readonly Dictionary<Vector3Int, ChunkGeometry> geometry = new Dictionary<Vector3Int, ChunkGeometry>();

		private readonly Stack<ChunkGeometry> pool = new Stack<ChunkGeometry>();

		private readonly List<Vector3> vertices = new List<Vector3>();

		private readonly List<Vector3> normals = new List<Vector3>();

		private readonly List<Color32> colors = new List<Color32>();

		private readonly List<int> triangles = new List<int>();

		private Mesh mesh;

		public Mesh Build(VoxelGrid grid)
		{
			if (grid.EverythingDirty)
			{
				RebuildAll(grid);
			}
			else
			{
				RebuildDirty(grid);
			}
			grid.ClearDirty();
			return Upload();
		}

		private void RebuildAll(VoxelGrid grid)
		{
			foreach (ChunkGeometry value in geometry.Values)
			{
				Recycle(value);
			}
			geometry.Clear();
			foreach (VoxelGrid.ChunkView chunk in grid.Chunks)
			{
				EmitInto(grid, chunk);
			}
		}

		private void RebuildDirty(VoxelGrid grid)
		{
			foreach (Vector3Int item in new List<Vector3Int>(grid.DirtyChunks))
			{
				if (geometry.TryGetValue(item, out var value))
				{
					geometry.Remove(item);
					Recycle(value);
				}
				if (grid.TryGetChunk(item, out var view))
				{
					EmitInto(grid, view);
				}
			}
		}

		private void EmitInto(VoxelGrid grid, VoxelGrid.ChunkView chunk)
		{
			ChunkGeometry chunkGeometry = ((pool.Count > 0) ? pool.Pop() : new ChunkGeometry());
			VoxelMeshBuilder.EmitChunk(grid, chunk, chunkGeometry.Vertices, chunkGeometry.Normals, chunkGeometry.Colors);
			if (chunkGeometry.Vertices.Count == 0)
			{
				pool.Push(chunkGeometry);
			}
			else
			{
				geometry[ChunkKey(chunk.Origin)] = chunkGeometry;
			}
		}

		private void Recycle(ChunkGeometry cached)
		{
			cached.Clear();
			pool.Push(cached);
		}

		private static Vector3Int ChunkKey(Vector3Int origin)
		{
			return new Vector3Int(origin.x / VoxelGrid.Size, origin.y / VoxelGrid.Size, origin.z / VoxelGrid.Size);
		}

		private Mesh Upload()
		{
			vertices.Clear();
			normals.Clear();
			colors.Clear();
			foreach (ChunkGeometry value in geometry.Values)
			{
				vertices.AddRange(value.Vertices);
				normals.AddRange(value.Normals);
				colors.AddRange(value.Colors);
			}
			VoxelMeshBuilder.BuildQuadTriangles(triangles, vertices.Count / 4);
			if (mesh == null)
			{
				mesh = new Mesh
				{
					name = "Voxel Mesh"
				};
				mesh.MarkDynamic();
			}
			mesh.Clear();
			mesh.indexFormat = ((vertices.Count > 65000) ? IndexFormat.UInt32 : IndexFormat.UInt16);
			mesh.SetVertices(vertices);
			mesh.SetNormals(normals);
			mesh.SetColors(colors);
			mesh.SetTriangles(triangles, 0);
			mesh.RecalculateBounds();
			return mesh;
		}
	}
}
