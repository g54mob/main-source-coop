using System.Collections.Generic;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.Networking
{
	public static class VoxelBodyValidator
	{
		public const int MaxPayloadBytes = 61440;

		public const int MaxTotalVoxels = 150000;

		public static int MaxPieces => VoxelEditorSettings.MaxPieces;

		public static bool Accepts(byte[] data, out string rejection, float expectedVoxelSize = 0f)
		{
			rejection = null;
			if (data == null || data.Length == 0)
			{
				return true;
			}
			if (data.Length > 61440)
			{
				rejection = "Reject.ModelTooLarge";
				return false;
			}
			if (!VoxelBodyCodec.TryDecodeSummary(data, out var summary))
			{
				rejection = "Reject.ModelUnreadable";
				return false;
			}
			if (expectedVoxelSize > 0f && Mathf.Abs(summary.VoxelSize - expectedVoxelSize) > expectedVoxelSize * 0.001f)
			{
				rejection = "Reject.ModelUnreadable";
				return false;
			}
			if (summary.Pieces.Count == 0)
			{
				rejection = "Reject.ModelEmpty";
				return false;
			}
			if (summary.Pieces.Count > MaxPieces)
			{
				rejection = $"Model en fazla {MaxPieces} parçadan oluşabilir.";
				return false;
			}
			if (summary.TotalVoxels == 0)
			{
				rejection = "Reject.InvisibleModel";
				return false;
			}
			if (summary.TotalVoxels > 150000)
			{
				rejection = $"Model en fazla {150000} voxel içerebilir.";
				return false;
			}
			if (AcceptsShape(summary, out rejection))
			{
				return AcceptsSubstance(data, out rejection);
			}
			return false;
		}

		private static bool AcceptsSubstance(byte[] data, out string rejection)
		{
			rejection = null;
			int minBoundExtent = VoxelEditorSettings.MinBoundExtent;
			int slimBoundExtent = VoxelEditorSettings.SlimBoundExtent;
			int minIslandVoxels = VoxelEditorSettings.MinIslandVoxels;
			if (minBoundExtent <= 1 && minIslandVoxels <= 1)
			{
				return true;
			}
			if (!VoxelBodyCodec.TryDecodeSummary(data, out var summary, 200000, 64, findIslands: true))
			{
				rejection = "Reject.ModelUnreadable";
				return false;
			}
			foreach (VoxelPieceSummary piece in summary.Pieces)
			{
				if (piece.Islands == null)
				{
					continue;
				}
				foreach (VoxelIsland island in piece.Islands)
				{
					if (!island.IsSubstantial(minBoundExtent, slimBoundExtent, minIslandVoxels))
					{
						rejection = "Reject.ModelFragments";
						return false;
					}
				}
			}
			return true;
		}

		private static bool AcceptsShape(VoxelBodySummary body, out string rejection)
		{
			rejection = null;
			float num = Mathf.Max(body.VoxelSize, 0.0001f);
			List<VoxelBodyPiece> list = new List<VoxelBodyPiece>(body.Pieces.Count);
			foreach (VoxelPieceSummary piece in body.Pieces)
			{
				if (piece.HasVoxels)
				{
					list.Add(new VoxelBodyPiece(piece.Min, piece.Max, Matrix4x4.TRS(piece.LocalPosition / num, piece.LocalRotation, Vector3.one)));
				}
			}
			VoxelBodyReport voxelBodyReport = VoxelBodyRules.Evaluate(list);
			if (voxelBodyReport.BelowMin)
			{
				rejection = $"Model çok küçük. Her eksende en az {VoxelEditorSettings.MinBoundExtent} birim " + $"olmalı; yalnızca bir eksen {VoxelEditorSettings.SlimBoundExtent} birime kadar inebilir.";
				return false;
			}
			if (voxelBodyReport.AboveMax)
			{
				rejection = $"Model çok büyük - her eksende en fazla {VoxelEditorSettings.MaxBoundExtent} birim olabilir.";
				return false;
			}
			if (voxelBodyReport.StrandedPieces != null && voxelBodyReport.StrandedPieces.Count > 0)
			{
				rejection = "Reject.InvalidPlacement";
				return false;
			}
			if (voxelBodyReport.OverlappingPieces != null && voxelBodyReport.OverlappingPieces.Count > 0)
			{
				rejection = "Reject.PiecesOverlap";
				return false;
			}
			if (voxelBodyReport.SmallPieces != null && voxelBodyReport.SmallPieces.Count > 0)
			{
				rejection = "Reject.PieceTooSmall";
				return false;
			}
			return true;
		}
	}
}
