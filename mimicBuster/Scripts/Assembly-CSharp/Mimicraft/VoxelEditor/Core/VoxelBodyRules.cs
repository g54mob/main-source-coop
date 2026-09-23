using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public static class VoxelBodyRules
	{
		private static readonly List<Bounds> scratchBounds = new List<Bounds>();

		private static readonly List<int> scratchIndices = new List<int>();

		private static readonly List<VoxelBodyPiece> scratchSubset = new List<VoxelBodyPiece>();

		private static readonly List<Bounds> scratchPadded = new List<Bounds>();

		private static int[] scratchGroup = Array.Empty<int>();

		public static bool WouldRemainValidWithout(IReadOnlyList<VoxelBodyPiece> pieces, int excludedIndex)
		{
			if (pieces == null)
			{
				return false;
			}
			scratchSubset.Clear();
			for (int i = 0; i < pieces.Count; i++)
			{
				if (i != excludedIndex)
				{
					scratchSubset.Add(pieces[i]);
				}
			}
			VoxelBodyReport voxelBodyReport = Evaluate(scratchSubset);
			if (voxelBodyReport.HasVoxels && !voxelBodyReport.BelowMin)
			{
				return !voxelBodyReport.AboveMax;
			}
			return false;
		}

		public static VoxelBodyReport Evaluate(IReadOnlyList<VoxelBodyPiece> pieces)
		{
			VoxelBodyReport result = default(VoxelBodyReport);
			if (pieces == null || pieces.Count == 0)
			{
				return result;
			}
			List<Bounds> list = scratchBounds;
			List<int> list2 = scratchIndices;
			list.Clear();
			list2.Clear();
			for (int i = 0; i < pieces.Count; i++)
			{
				if (TryGetPieceBounds(pieces[i], out var bounds))
				{
					if (!result.HasVoxels)
					{
						result.Bounds = bounds;
						result.HasVoxels = true;
					}
					else
					{
						result.Bounds.Encapsulate(bounds);
					}
					list.Add(bounds);
					list2.Add(i);
				}
			}
			if (!result.HasVoxels)
			{
				return result;
			}
			result.Extent = result.Bounds.size;
			int maxBoundExtent = VoxelEditorSettings.MaxBoundExtent;
			result.BelowMin = !FitsMinimum(result.Extent);
			result.AboveMax = result.Extent.x > (float)maxBoundExtent || result.Extent.y > (float)maxBoundExtent || result.Extent.z > (float)maxBoundExtent;
			result.StrandedPieces = FindStrandedPieces(list, list2);
			result.OverlappingPieces = FindOverlappingPieces(list, list2);
			result.SmallPieces = FindSmallPieces(pieces);
			result.FragmentedPieces = FindFragmentedPieces(pieces);
			return result;
		}

		public static bool TryGetPieceBounds(VoxelBodyPiece piece, out Bounds bounds)
		{
			bounds = default(Bounds);
			if (!piece.HasVoxels)
			{
				return false;
			}
			Vector3 vector = piece.Min;
			Vector3 vector2 = piece.Max + Vector3.one;
			bool flag = true;
			for (int i = 0; i < 8; i++)
			{
				Vector3 point = new Vector3(((i & 1) == 0) ? vector.x : vector2.x, ((i & 2) == 0) ? vector.y : vector2.y, ((i & 4) == 0) ? vector.z : vector2.z);
				Vector3 vector3 = piece.BodyFromPiece.MultiplyPoint3x4(point);
				if (flag)
				{
					bounds = new Bounds(vector3, Vector3.zero);
					flag = false;
				}
				else
				{
					bounds.Encapsulate(vector3);
				}
			}
			return true;
		}

		private static List<int> FindStrandedPieces(List<Bounds> bounds, List<int> indices)
		{
			if (bounds.Count <= 1)
			{
				return null;
			}
			float num = Mathf.Max(0f, VoxelEditorSettings.MaxPieceGapVoxels) * 0.5f;
			List<Bounds> list = scratchPadded;
			list.Clear();
			foreach (Bounds bound in bounds)
			{
				bound.Expand(num * 2f);
				list.Add(bound);
			}
			int count = bounds.Count;
			if (scratchGroup.Length < count)
			{
				scratchGroup = new int[count];
			}
			int[] array = scratchGroup;
			for (int i = 0; i < count; i++)
			{
				array[i] = i;
			}
			for (int j = 0; j < list.Count; j++)
			{
				for (int k = j + 1; k < list.Count; k++)
				{
					if (list[j].Intersects(list[k]))
					{
						Union(array, j, k);
					}
				}
			}
			List<int> list2 = null;
			int num2 = Find(array, 0);
			for (int l = 1; l < count; l++)
			{
				if (Find(array, l) != num2)
				{
					(list2 ?? (list2 = new List<int>())).Add(indices[l]);
				}
			}
			return list2;
		}

		private static List<int> FindOverlappingPieces(List<Bounds> bounds, List<int> indices)
		{
			if (bounds.Count <= 1)
			{
				return null;
			}
			float num = Mathf.Clamp01(VoxelEditorSettings.MaxPieceOverlapFraction);
			List<int> list = null;
			for (int i = 0; i < bounds.Count; i++)
			{
				for (int j = i + 1; j < bounds.Count; j++)
				{
					Vector3 vector = Vector3.Max(bounds[i].min, bounds[j].min);
					Vector3 vector2 = Vector3.Min(bounds[i].max, bounds[j].max) - vector;
					if (vector2.x <= 0f || vector2.y <= 0f || vector2.z <= 0f)
					{
						continue;
					}
					float num2 = vector2.x * vector2.y * vector2.z;
					float num3 = Volume(bounds[i]);
					float num4 = Volume(bounds[j]);
					float num5 = Mathf.Min(num3, num4);
					if (!(num5 <= 0f) && !(num2 <= num * num5))
					{
						int item = ((num3 <= num4) ? indices[i] : indices[j]);
						if (list == null)
						{
							list = new List<int>();
						}
						if (!list.Contains(item))
						{
							list.Add(item);
						}
					}
				}
			}
			return list;
		}

		public static bool FitsMinimum(Vector3 extent)
		{
			return FitsMinimum(extent, VoxelEditorSettings.MinBoundExtent, VoxelEditorSettings.SlimBoundExtent);
		}

		public static bool FitsMinimum(Vector3Int extent)
		{
			return FitsMinimum((Vector3)extent);
		}

		public static bool FitsMinimum(Vector3 extent, int min, int slim)
		{
			if (slim > min)
			{
				slim = min;
			}
			int thin = 0;
			if (!Fits(extent.x, min, slim, ref thin))
			{
				return false;
			}
			if (!Fits(extent.y, min, slim, ref thin))
			{
				return false;
			}
			return Fits(extent.z, min, slim, ref thin);
		}

		public static bool FitsMinimum(Vector3Int extent, int min, int slim)
		{
			return FitsMinimum((Vector3)extent, min, slim);
		}

		private static bool Fits(float extent, int min, int slim, ref int thin)
		{
			if (extent >= (float)min)
			{
				return true;
			}
			if (extent < (float)slim)
			{
				return false;
			}
			thin++;
			return thin <= 1;
		}

		public static bool IsPieceLargeEnough(Vector3Int min, Vector3Int max)
		{
			return FitsMinimum(max - min + Vector3Int.one);
		}

		private static List<int> FindSmallPieces(IReadOnlyList<VoxelBodyPiece> pieces)
		{
			List<int> list = null;
			for (int i = 0; i < pieces.Count; i++)
			{
				if (pieces[i].HasVoxels && !IsPieceLargeEnough(pieces[i].Min, pieces[i].Max))
				{
					(list ?? (list = new List<int>())).Add(i);
				}
			}
			return list;
		}

		private static List<int> FindFragmentedPieces(IReadOnlyList<VoxelBodyPiece> pieces)
		{
			List<int> list = null;
			for (int i = 0; i < pieces.Count; i++)
			{
				if (pieces[i].HasVoxels && pieces[i].WeakIslands > 0)
				{
					(list ?? (list = new List<int>())).Add(i);
				}
			}
			return list;
		}

		private static float Volume(Bounds b)
		{
			return Mathf.Max(0f, b.size.x) * Mathf.Max(0f, b.size.y) * Mathf.Max(0f, b.size.z);
		}

		private static int Find(int[] group, int i)
		{
			while (group[i] != i)
			{
				group[i] = group[group[i]];
				i = group[i];
			}
			return i;
		}

		private static void Union(int[] group, int a, int b)
		{
			int num = Find(group, a);
			int num2 = Find(group, b);
			if (num != num2)
			{
				group[num2] = num;
			}
		}
	}
}
