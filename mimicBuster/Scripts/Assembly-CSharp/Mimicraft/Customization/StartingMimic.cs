using System.Collections.Generic;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.Customization
{
	public static class StartingMimic
	{
		private const string SelectedKey = "Mimicraft.StartingMimic";

		public static string SelectedPath
		{
			get
			{
				return PlayerPrefs.GetString("Mimicraft.StartingMimic", "");
			}
			set
			{
				PlayerPrefs.SetString("Mimicraft.StartingMimic", value ?? "");
				PlayerPrefs.Save();
			}
		}

		public static string Resolve()
		{
			string selectedPath = SelectedPath;
			if (string.IsNullOrEmpty(selectedPath) || !TemplateStorage.Exists(selectedPath))
			{
				return "";
			}
			foreach (TemplateListEntry item in List())
			{
				if (item.FilePath == selectedPath)
				{
					return selectedPath;
				}
			}
			return "";
		}

		public static bool IsSelected(string path)
		{
			if (!string.IsNullOrEmpty(path))
			{
				return path == SelectedPath;
			}
			return false;
		}

		public static void Clear()
		{
			SelectedPath = "";
		}

		public static List<TemplateListEntry> List()
		{
			List<TemplateListEntry> list = new List<TemplateListEntry>();
			foreach (TemplateListEntry item in TemplateStorage.ListTemplates())
			{
				if (string.IsNullOrEmpty(item.Category))
				{
					list.Add(item);
				}
			}
			return list;
		}

		public static TemplateModel LoadPlayable(out string rejection)
		{
			rejection = null;
			string text = Resolve();
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			TemplateModel templateModel = TemplateStorage.LoadTemplate(text);
			if (templateModel == null || templateModel.Pieces.Count == 0)
			{
				rejection = "Reject.StartingMimicUnreadable";
				return null;
			}
			if (templateModel.Pieces.Count > 1)
			{
				templateModel.Pieces.RemoveRange(1, templateModel.Pieces.Count - 1);
			}
			if (templateModel.Pieces.Count > VoxelEditorSettings.MaxPieces)
			{
				rejection = "Reject.StartingMimicTooManyPieces";
				return null;
			}
			if (!FitsBodyRules(templateModel))
			{
				rejection = "Reject.StartingMimicTooSmall";
				return null;
			}
			return templateModel;
		}

		private static bool FitsBodyRules(TemplateModel model)
		{
			List<VoxelBodyPiece> list = new List<VoxelBodyPiece>(model.Pieces.Count);
			float num = Mathf.Max(model.VoxelSize, 0.0001f);
			foreach (TemplatePiece piece in model.Pieces)
			{
				if (piece.Grid != null && piece.Grid.TryGetBounds(out var min, out var max))
				{
					list.Add(new VoxelBodyPiece(min, max, Matrix4x4.TRS(piece.LocalPosition / num, piece.LocalRotation, Vector3.one)));
				}
			}
			if (list.Count == 0)
			{
				return false;
			}
			VoxelBodyReport voxelBodyReport = VoxelBodyRules.Evaluate(list);
			if (voxelBodyReport.HasVoxels)
			{
				return voxelBodyReport.IsValid;
			}
			return false;
		}
	}
}
