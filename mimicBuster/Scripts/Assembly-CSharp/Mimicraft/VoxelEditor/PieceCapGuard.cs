using System.Collections.Generic;
using Mimicraft.Localization;
using Mimicraft.UI;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public static class PieceCapGuard
	{
		private const float WarnCooldownSeconds = 1f;

		private static float lastWarned = float.NegativeInfinity;

		public static void Refuse()
		{
			if (!(Time.unscaledTime - lastWarned < 1f))
			{
				lastWarned = Time.unscaledTime;
				if (ToastView.Instance != null)
				{
					ToastView.Instance.Show(string.Format(Loc.Get("Tool.TooManyPieces"), VoxelEditorSettings.MaxPieces));
				}
				AudioLibrary.PlayOneShotClip((AudioLibrary.Instance != null) ? AudioLibrary.Instance.editDeniedClip : null);
			}
		}

		public static void RefuseFragment()
		{
			if (!(Time.unscaledTime - lastWarned < 1f))
			{
				lastWarned = Time.unscaledTime;
				if (ToastView.Instance != null)
				{
					ToastView.Instance.Show(Loc.Get("Reject.ModelFragments"));
				}
				AudioLibrary.PlayOneShotClip((AudioLibrary.Instance != null) ? AudioLibrary.Instance.editDeniedClip : null);
			}
		}

		public static void RefuseTooSmall()
		{
			if (!(Time.unscaledTime - lastWarned < 1f))
			{
				lastWarned = Time.unscaledTime;
				if (ToastView.Instance != null)
				{
					ToastView.Instance.Show(string.Format(Loc.Get("Tool.PieceTooSmall"), VoxelEditorSettings.MinBoundExtent));
				}
				AudioLibrary.PlayOneShotClip((AudioLibrary.Instance != null) ? AudioLibrary.Instance.editDeniedClip : null);
			}
		}

		public static bool SplitLeavesLargeEnoughPieces(VoxelGrid grid, IEnumerable<KeyValuePair<Vector3Int, VoxelData>> moving)
		{
			if (grid == null)
			{
				return true;
			}
			HashSet<Vector3Int> hashSet = new HashSet<Vector3Int>();
			foreach (KeyValuePair<Vector3Int, VoxelData> item in moving)
			{
				hashSet.Add(item.Key);
			}
			if (hashSet.Count == 0)
			{
				return true;
			}
			if (!Extent(hashSet, null, out var min, out var max) || !VoxelBodyRules.IsPieceLargeEnough(min, max))
			{
				return false;
			}
			if (!Extent(grid.Positions, hashSet, out var min2, out var max2))
			{
				return true;
			}
			return VoxelBodyRules.IsPieceLargeEnough(min2, max2);
		}

		private static bool Extent(IEnumerable<Vector3Int> cells, HashSet<Vector3Int> except, out Vector3Int min, out Vector3Int max)
		{
			min = default(Vector3Int);
			max = default(Vector3Int);
			bool flag = false;
			foreach (Vector3Int cell in cells)
			{
				if (except == null || !except.Contains(cell))
				{
					if (!flag)
					{
						min = cell;
						max = cell;
						flag = true;
					}
					else
					{
						min = Vector3Int.Min(min, cell);
						max = Vector3Int.Max(max, cell);
					}
				}
			}
			return flag;
		}
	}
}
