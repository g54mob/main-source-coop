using System.Collections.Generic;
using System.Text;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public static class StrandedPieceVisibility
	{
		private static readonly List<VoxelModel> valid = new List<VoxelModel>();

		private static readonly List<VoxelBodyPiece> pieces = new List<VoxelBodyPiece>();

		private static readonly List<Bounds> pieceBoundsForReport = new List<Bounds>();

		public static void Apply(IEnumerable<VoxelModel> models, bool hideStranded)
		{
			valid.Clear();
			foreach (VoxelModel model in models)
			{
				if (!(model == null))
				{
					MeshRenderer component = model.GetComponent<MeshRenderer>();
					if (!(component == null))
					{
						component.enabled = true;
						valid.Add(model);
					}
				}
			}
			if (!hideStranded || valid.Count < 2)
			{
				if (hideStranded)
				{
					Report($"{valid.Count} parca - gizlenecek bir sey yok.");
				}
				return;
			}
			pieces.Clear();
			pieceBoundsForReport.Clear();
			Transform transform = valid[0].transform;
			foreach (VoxelModel item in valid)
			{
				VoxelBodyPiece voxelBodyPiece = VoxelBodyPiece.FromGrid(item.Grid, transform.worldToLocalMatrix * item.transform.localToWorldMatrix);
				pieces.Add(voxelBodyPiece);
				pieceBoundsForReport.Add(VoxelBodyRules.TryGetPieceBounds(voxelBodyPiece, out var bounds) ? bounds : default(Bounds));
			}
			VoxelBodyReport voxelBodyReport = VoxelBodyRules.Evaluate(pieces);
			StringBuilder stringBuilder = new StringBuilder($"{valid.Count} parca olculdu (bosluk siniri {VoxelEditorSettings.MaxPieceGapVoxels} voxel), " + $"{((voxelBodyReport.StrandedPieces != null) ? voxelBodyReport.StrandedPieces.Count : 0)} tanesi kopuk:");
			for (int i = 0; i < valid.Count && i < pieceBoundsForReport.Count; i++)
			{
				bool flag = voxelBodyReport.StrandedPieces != null && voxelBodyReport.StrandedPieces.Contains(i);
				Bounds bounds2 = pieceBoundsForReport[i];
				stringBuilder.Append($"\n  {i} '{valid[i].name}' merkez={bounds2.center} boyut={bounds2.size}" + (flag ? "  <- GIZLENDI" : ""));
			}
			Report(stringBuilder.ToString());
			if (voxelBodyReport.StrandedPieces == null)
			{
				return;
			}
			foreach (int strandedPiece in voxelBodyReport.StrandedPieces)
			{
				if (strandedPiece >= 0 && strandedPiece < valid.Count)
				{
					MeshRenderer component2 = valid[strandedPiece].GetComponent<MeshRenderer>();
					if (component2 != null)
					{
						component2.enabled = false;
					}
				}
			}
		}

		private static void Report(string message)
		{
			Debug.Log("[StrandedPieceVisibility] " + message);
		}
	}
}
