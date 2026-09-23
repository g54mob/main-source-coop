using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public class VoxelGrid
	{
		private sealed class Chunk
		{
			public readonly Color32[] Colors = new Color32[4096];

			public readonly ulong[] Occupancy = new ulong[64];

			public int Count;

			public bool Has(int index)
			{
				return (Occupancy[index >> 6] & (ulong)(1L << index)) != 0;
			}

			public bool Fill(int index, Color32 color)
			{
				Colors[index] = color;
				ulong num = (ulong)(1L << (index & 0x3F));
				int num2 = index >> 6;
				if ((Occupancy[num2] & num) != 0L)
				{
					return false;
				}
				Occupancy[num2] |= num;
				Count++;
				return true;
			}

			public bool Erase(int index)
			{
				ulong num = (ulong)(1L << (index & 0x3F));
				int num2 = index >> 6;
				if ((Occupancy[num2] & num) == 0L)
				{
					return false;
				}
				Occupancy[num2] &= ~num;
				Count--;
				return true;
			}
		}

		internal readonly struct ChunkView
		{
			public readonly Vector3Int Origin;

			public readonly Color32[] Colors;

			public readonly ulong[] Occupancy;

			public ChunkView(Vector3Int origin, Color32[] colors, ulong[] occupancy)
			{
				Origin = origin;
				Colors = colors;
				Occupancy = occupancy;
			}
		}

		private const int ChunkBits = 4;

		private const int ChunkSize = 16;

		private const int ChunkMask = 15;

		private const int ChunkVolume = 4096;

		private readonly Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();

		private int count;

		private readonly HashSet<Vector3Int> dirtyChunks = new HashSet<Vector3Int>();

		private bool allDirty = true;

		private Vector3Int cachedMin;

		private Vector3Int cachedMax;

		private bool boundsValid;

		private readonly Dictionary<(Vector3Int Position, int Face), Color32> faceColors = new Dictionary<(Vector3Int, int), Color32>();

		public int Count => count;

		public int Version { get; private set; }

		public IEnumerable<KeyValuePair<Vector3Int, VoxelData>> Voxels
		{
			get
			{
				foreach (KeyValuePair<Vector3Int, Chunk> chunk2 in chunks)
				{
					Chunk chunk = chunk2.Value;
					if (chunk.Count == 0)
					{
						continue;
					}
					Vector3Int origin = chunk2.Key * 16;
					for (int index = 0; index < 4096; index++)
					{
						if (chunk.Has(index))
						{
							yield return new KeyValuePair<Vector3Int, VoxelData>(origin + LocalOf(index), new VoxelData(chunk.Colors[index]));
						}
					}
				}
			}
		}

		public IEnumerable<Vector3Int> Positions
		{
			get
			{
				foreach (KeyValuePair<Vector3Int, Chunk> chunk2 in chunks)
				{
					Chunk chunk = chunk2.Value;
					if (chunk.Count == 0)
					{
						continue;
					}
					Vector3Int origin = chunk2.Key * 16;
					for (int index = 0; index < 4096; index++)
					{
						if (chunk.Has(index))
						{
							yield return origin + LocalOf(index);
						}
					}
				}
			}
		}

		public int FaceColorCount => faceColors.Count;

		public IEnumerable<KeyValuePair<(Vector3Int Position, int Face), Color32>> FaceColors => faceColors;

		internal IReadOnlyCollection<Vector3Int> DirtyChunks => dirtyChunks;

		internal bool EverythingDirty => allDirty;

		internal IEnumerable<ChunkView> Chunks
		{
			get
			{
				foreach (KeyValuePair<Vector3Int, Chunk> chunk in chunks)
				{
					if (chunk.Value.Count > 0)
					{
						yield return new ChunkView(chunk.Key * 16, chunk.Value.Colors, chunk.Value.Occupancy);
					}
				}
			}
		}

		internal static int Size => 16;

		public bool Contains(Vector3Int position)
		{
			if (chunks.TryGetValue(ChunkOf(position), out var value))
			{
				return value.Has(IndexOf(position));
			}
			return false;
		}

		public bool TryGet(Vector3Int position, out VoxelData data)
		{
			if (chunks.TryGetValue(ChunkOf(position), out var value))
			{
				int num = IndexOf(position);
				if (value.Has(num))
				{
					data = new VoxelData(value.Colors[num]);
					return true;
				}
			}
			data = default(VoxelData);
			return false;
		}

		public void Set(Vector3Int position, VoxelData data)
		{
			Vector3Int key = ChunkOf(position);
			if (!chunks.TryGetValue(key, out var value))
			{
				value = new Chunk();
				chunks[key] = value;
			}
			if (!value.Fill(IndexOf(position), data.Color))
			{
				Version++;
				MarkDirty(position);
				return;
			}
			Version++;
			MarkDirty(position);
			count++;
			if (boundsValid)
			{
				cachedMin = Vector3Int.Min(cachedMin, position);
				cachedMax = Vector3Int.Max(cachedMax, position);
			}
		}

		public bool Remove(Vector3Int position)
		{
			if (!chunks.TryGetValue(ChunkOf(position), out var value) || !value.Erase(IndexOf(position)))
			{
				return false;
			}
			ClearFaceColors(position);
			Version++;
			MarkDirty(position);
			count--;
			if (value.Count == 0)
			{
				chunks.Remove(ChunkOf(position));
			}
			if (boundsValid && IsOnBoundary(position))
			{
				boundsValid = false;
			}
			return true;
		}

		public void Clear()
		{
			Version++;
			chunks.Clear();
			faceColors.Clear();
			count = 0;
			boundsValid = false;
			dirtyChunks.Clear();
			allDirty = true;
		}

		public Color32 GetFaceColor(Vector3Int position, int face)
		{
			if (faceColors.TryGetValue((position, face), out var value))
			{
				return value;
			}
			if (!TryGet(position, out var data))
			{
				return default(Color32);
			}
			return data.Color;
		}

		public bool TryGetFaceColor(Vector3Int position, int face, out Color32 color)
		{
			return faceColors.TryGetValue((position, face), out color);
		}

		public bool IsFaceVisible(Vector3Int position, int face)
		{
			if (face < 0 || face >= 6 || !Contains(position))
			{
				return false;
			}
			return !Contains(position + FaceAxes.Normals[face]);
		}

		public int PruneHiddenFaceColors()
		{
			if (faceColors.Count == 0)
			{
				return 0;
			}
			List<(Vector3Int, int)> list = new List<(Vector3Int, int)>();
			foreach (KeyValuePair<(Vector3Int, int), Color32> faceColor in faceColors)
			{
				if (!IsFaceVisible(faceColor.Key.Item1, faceColor.Key.Item2))
				{
					list.Add(faceColor.Key);
				}
			}
			foreach (var (vector3Int, item) in list)
			{
				faceColors.Remove((vector3Int, item));
				MarkDirty(vector3Int);
			}
			if (list.Count > 0)
			{
				Version++;
			}
			return list.Count;
		}

		public void SetFaceColor(Vector3Int position, int face, Color32 color)
		{
			if (TryGet(position, out var data))
			{
				(Vector3Int, int) key = (position, face);
				bool flag;
				if (Same(data.Color, color))
				{
					flag = faceColors.Remove(key);
				}
				else
				{
					flag = !faceColors.TryGetValue(key, out var value) || !Same(value, color);
					faceColors[key] = color;
				}
				if (flag)
				{
					Version++;
					MarkDirty(position);
				}
			}
		}

		public bool ClearFaceColor(Vector3Int position, int face)
		{
			if (!faceColors.Remove((position, face)))
			{
				return false;
			}
			Version++;
			MarkDirty(position);
			return true;
		}

		public bool ClearFaceColors(Vector3Int position)
		{
			bool flag = false;
			for (int i = 0; i < 6; i++)
			{
				flag |= faceColors.Remove((position, i));
			}
			if (flag)
			{
				Version++;
				MarkDirty(position);
			}
			return flag;
		}

		private static bool Same(Color32 a, Color32 b)
		{
			if (a.r == b.r && a.g == b.g && a.b == b.b)
			{
				return a.a == b.a;
			}
			return false;
		}

		internal void ClearDirty()
		{
			dirtyChunks.Clear();
			allDirty = false;
		}

		private void MarkDirty(Vector3Int position)
		{
			if (!allDirty)
			{
				Vector3Int vector3Int = ChunkOf(position);
				dirtyChunks.Add(vector3Int);
				int num = position.x & 0xF;
				int num2 = position.y & 0xF;
				int num3 = position.z & 0xF;
				switch (num)
				{
				case 0:
					dirtyChunks.Add(vector3Int + new Vector3Int(-1, 0, 0));
					break;
				case 15:
					dirtyChunks.Add(vector3Int + new Vector3Int(1, 0, 0));
					break;
				}
				switch (num2)
				{
				case 0:
					dirtyChunks.Add(vector3Int + new Vector3Int(0, -1, 0));
					break;
				case 15:
					dirtyChunks.Add(vector3Int + new Vector3Int(0, 1, 0));
					break;
				}
				switch (num3)
				{
				case 0:
					dirtyChunks.Add(vector3Int + new Vector3Int(0, 0, -1));
					break;
				case 15:
					dirtyChunks.Add(vector3Int + new Vector3Int(0, 0, 1));
					break;
				}
			}
		}

		public bool TryGetBounds(out Vector3Int min, out Vector3Int max)
		{
			if (count == 0)
			{
				min = default(Vector3Int);
				max = default(Vector3Int);
				return false;
			}
			if (!boundsValid)
			{
				bool flag = true;
				foreach (Vector3Int position in Positions)
				{
					if (flag)
					{
						cachedMin = position;
						cachedMax = position;
						flag = false;
					}
					else
					{
						cachedMin = Vector3Int.Min(cachedMin, position);
						cachedMax = Vector3Int.Max(cachedMax, position);
					}
				}
				boundsValid = true;
			}
			min = cachedMin;
			max = cachedMax;
			return true;
		}

		private bool IsOnBoundary(Vector3Int p)
		{
			if (p.x != cachedMin.x && p.x != cachedMax.x && p.y != cachedMin.y && p.y != cachedMax.y && p.z != cachedMin.z)
			{
				return p.z == cachedMax.z;
			}
			return true;
		}

		public static VoxelGrid CreateDefaultCube(Color32 color, int size = 3)
		{
			VoxelGrid voxelGrid = new VoxelGrid();
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					for (int k = 0; k < size; k++)
					{
						voxelGrid.Set(new Vector3Int(i, j, k), new VoxelData(color));
					}
				}
			}
			return voxelGrid;
		}

		private static Vector3Int ChunkOf(Vector3Int p)
		{
			return new Vector3Int(p.x >> 4, p.y >> 4, p.z >> 4);
		}

		private static int IndexOf(Vector3Int p)
		{
			return LocalIndex(p.x & 0xF, p.y & 0xF, p.z & 0xF);
		}

		private static Vector3Int LocalOf(int index)
		{
			return new Vector3Int(index & 0xF, index >> 8, (index >> 4) & 0xF);
		}

		internal bool TryGetChunk(Vector3Int chunkCoord, out ChunkView view)
		{
			if (chunks.TryGetValue(chunkCoord, out var value) && value.Count > 0)
			{
				view = new ChunkView(chunkCoord * 16, value.Colors, value.Occupancy);
				return true;
			}
			view = default(ChunkView);
			return false;
		}

		internal static int LocalIndex(int x, int y, int z)
		{
			return (y * 16 + z) * 16 + x;
		}

		internal static bool Occupied(ulong[] occupancy, int index)
		{
			return (occupancy[index >> 6] & (ulong)(1L << index)) != 0;
		}
	}
}
