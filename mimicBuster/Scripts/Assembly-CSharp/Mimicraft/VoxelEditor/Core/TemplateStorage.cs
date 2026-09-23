using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public static class TemplateStorage
	{
		private const string TemplatesFolderName = "Templates";

		private static string TemplatesDirectory => Path.Combine(Application.persistentDataPath, "Templates");

		public static List<string> TagsOf(IEnumerable<TemplateListEntry> entries)
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			List<string> list = new List<string>();
			foreach (TemplateListEntry entry in entries)
			{
				if (!string.IsNullOrWhiteSpace(entry.Tag) && hashSet.Add(entry.Tag))
				{
					list.Add(entry.Tag);
				}
			}
			list.Sort(StringComparer.OrdinalIgnoreCase);
			return list;
		}

		public static List<TemplateListEntry> ListTemplates()
		{
			List<TemplateListEntry> list = new List<TemplateListEntry>();
			if (!Directory.Exists(TemplatesDirectory))
			{
				return list;
			}
			string[] files = Directory.GetFiles(TemplatesDirectory, "*.template");
			foreach (string filePath in files)
			{
				byte[] array = TryReadBytes(filePath);
				if (array != null && TemplateFile.TryReadHeader(array, out var name, out var category, out var tag, out var pieceCount))
				{
					list.Add(new TemplateListEntry(filePath, name, category, tag, pieceCount));
				}
			}
			files = Directory.GetFiles(TemplatesDirectory, "*.json");
			foreach (string filePath2 in files)
			{
				VoxelTemplateData voxelTemplateData = TryReadJson(filePath2);
				if (voxelTemplateData != null)
				{
					list.Add(new TemplateListEntry(filePath2, voxelTemplateData.modelName, voxelTemplateData.category, voxelTemplateData.tag));
				}
			}
			list.Sort((TemplateListEntry a, TemplateListEntry b) => string.Compare(a.ModelName, b.ModelName, StringComparison.OrdinalIgnoreCase));
			return list;
		}

		public static TemplateModel LoadTemplate(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				return null;
			}
			if (filePath.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
			{
				return FromJson(TryReadJson(filePath));
			}
			byte[] array = TryReadBytes(filePath);
			if (array == null)
			{
				return null;
			}
			if (TemplateFile.TryDecode(array, out var model))
			{
				return model;
			}
			Debug.LogWarning("[TemplateStorage] '" + filePath + "' okunamadi - bozuk ya da baska bir format.");
			return null;
		}

		private static TemplateModel FromJson(VoxelTemplateData data)
		{
			if (data == null)
			{
				return null;
			}
			TemplateModel templateModel = new TemplateModel
			{
				Name = (data.modelName ?? ""),
				Category = (data.category ?? ""),
				Tag = (data.tag ?? "")
			};
			foreach (TemplatePieceData item in PiecesOf(data))
			{
				VoxelGrid grid = new VoxelGrid();
				Fill(grid, item.voxels, item.faces);
				templateModel.Pieces.Add(new TemplatePiece(item.name, grid, PositionOf(item), RotationOf(item)));
			}
			return templateModel;
		}

		private static byte[] TryReadBytes(string filePath)
		{
			try
			{
				return File.ReadAllBytes(filePath);
			}
			catch (IOException ex)
			{
				Debug.LogWarning("[TemplateStorage] '" + filePath + "' okunamadi: " + ex.Message);
				return null;
			}
		}

		private static VoxelTemplateData TryReadJson(string filePath)
		{
			try
			{
				return JsonUtility.FromJson<VoxelTemplateData>(File.ReadAllText(filePath));
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[TemplateStorage] '" + filePath + "' okunamadi: " + ex.Message);
				return null;
			}
		}

		public static string SaveTemplate(string filePath, string modelName, VoxelGrid grid)
		{
			return SaveTemplate(filePath, modelName, new TemplatePiece[1]
			{
				new TemplatePiece(null, grid)
			});
		}

		public static string SaveTemplate(string filePath, string modelName, IReadOnlyList<TemplatePiece> pieces, string category = null, string tag = null, float voxelSize = 1f)
		{
			if (!Directory.Exists(TemplatesDirectory))
			{
				Directory.CreateDirectory(TemplatesDirectory);
			}
			string text = null;
			if (!string.IsNullOrEmpty(filePath) && filePath.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
			{
				text = filePath;
				filePath = Path.ChangeExtension(filePath, ".template");
			}
			if (string.IsNullOrEmpty(filePath))
			{
				filePath = Path.Combine(TemplatesDirectory, string.Format("{0:N}{1}", Guid.NewGuid(), ".template"));
			}
			TemplateModel templateModel = new TemplateModel
			{
				Name = (modelName ?? ""),
				Category = (category ?? ""),
				Tag = (tag ?? ""),
				VoxelSize = voxelSize
			};
			templateModel.Pieces.AddRange(pieces);
			try
			{
				File.WriteAllBytes(filePath, TemplateFile.Encode(templateModel));
			}
			catch (IOException ex)
			{
				Debug.LogWarning("[TemplateStorage] '" + filePath + "' yazilamadi: " + ex.Message);
				return text ?? filePath;
			}
			if (text != null)
			{
				try
				{
					File.Delete(text);
				}
				catch (IOException ex2)
				{
					Debug.LogWarning("[TemplateStorage] Eski '" + text + "' silinemedi: " + ex2.Message);
				}
			}
			return filePath;
		}

		public static string DuplicateTemplate(string filePath, string newName)
		{
			TemplateModel templateModel = LoadTemplate(filePath);
			if (templateModel == null)
			{
				return null;
			}
			return SaveTemplate(null, newName, templateModel.Pieces, templateModel.Category, templateModel.Tag, templateModel.VoxelSize);
		}

		public static string UnusedName(string baseName)
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (TemplateListEntry item in ListTemplates())
			{
				hashSet.Add(item.ModelName ?? "");
			}
			if (string.IsNullOrWhiteSpace(baseName))
			{
				baseName = "Model";
			}
			if (!hashSet.Contains(baseName))
			{
				return baseName;
			}
			for (int i = 2; i < 1000; i++)
			{
				string text = $"{baseName} ({i})";
				if (!hashSet.Contains(text))
				{
					return text;
				}
			}
			return $"{baseName} ({Guid.NewGuid():N})".Substring(0, 40);
		}

		public static bool Exists(string filePath)
		{
			if (!string.IsNullOrEmpty(filePath))
			{
				return File.Exists(filePath);
			}
			return false;
		}

		public static void DeleteTemplate(string filePath)
		{
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
		}

		public static void ApplyPiece(TemplatePiece piece, VoxelGrid grid)
		{
			grid.Clear();
			if (piece.Grid == null)
			{
				return;
			}
			foreach (KeyValuePair<Vector3Int, VoxelData> voxel in piece.Grid.Voxels)
			{
				grid.Set(voxel.Key, voxel.Value);
			}
			foreach (KeyValuePair<(Vector3Int, int), Color32> faceColor in piece.Grid.FaceColors)
			{
				grid.SetFaceColor(faceColor.Key.Item1, faceColor.Key.Item2, faceColor.Value);
			}
		}

		private static List<TemplatePieceData> PiecesOf(VoxelTemplateData data)
		{
			if (data.pieces != null && data.pieces.Count > 0)
			{
				return data.pieces;
			}
			return new List<TemplatePieceData>
			{
				new TemplatePieceData
				{
					name = data.modelName,
					rw = 1f,
					voxels = data.voxels,
					faces = data.faces
				}
			};
		}

		private static void ApplyPiece(TemplatePieceData piece, VoxelGrid grid)
		{
			Fill(grid, piece.voxels, piece.faces);
		}

		private static Vector3 PositionOf(TemplatePieceData piece)
		{
			return new Vector3(piece.px, piece.py, piece.pz);
		}

		private static Quaternion RotationOf(TemplatePieceData piece)
		{
			Quaternion q = new Quaternion(piece.rx, piece.ry, piece.rz, piece.rw);
			if (!(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w < 0.0001f))
			{
				return Quaternion.Normalize(q);
			}
			return Quaternion.identity;
		}

		private static void Fill(VoxelGrid grid, List<VoxelEntryData> voxels, List<FaceEntryData> faces)
		{
			grid.Clear();
			if (voxels != null)
			{
				foreach (VoxelEntryData voxel in voxels)
				{
					grid.Set(new Vector3Int(voxel.x, voxel.y, voxel.z), new VoxelData(HexToColor(voxel.color)));
				}
			}
			if (faces == null)
			{
				return;
			}
			foreach (FaceEntryData face in faces)
			{
				if (face.face >= 0 && face.face < 6)
				{
					grid.SetFaceColor(new Vector3Int(face.x, face.y, face.z), face.face, HexToColor(face.color));
				}
			}
		}

		private static string ColorToHex(Color32 c)
		{
			return $"#{c.r:X2}{c.g:X2}{c.b:X2}";
		}

		private static Color32 HexToColor(string hex)
		{
			if (string.IsNullOrEmpty(hex))
			{
				return new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			}
			hex = hex.TrimStart('#');
			byte r = Convert.ToByte(hex.Substring(0, 2), 16);
			byte g = Convert.ToByte(hex.Substring(2, 2), 16);
			byte b = Convert.ToByte(hex.Substring(4, 2), 16);
			return new Color32(r, g, b, byte.MaxValue);
		}
	}
}
