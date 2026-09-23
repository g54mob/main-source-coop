using System;
using System.Collections.Generic;
using System.IO;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.Export
{
	public static class ModelFbxExporter
	{
		private sealed class GridSource : IVoxelSource
		{
			private readonly VoxelGrid grid;

			public GridSource(VoxelGrid grid)
			{
				this.grid = grid;
			}

			public bool TryGetBounds(out int minX, out int minY, out int minZ, out int maxX, out int maxY, out int maxZ)
			{
				Vector3Int min;
				Vector3Int max;
				bool result = grid.TryGetBounds(out min, out max);
				minX = min.x;
				minY = min.y;
				minZ = min.z;
				maxX = max.x;
				maxY = max.y;
				maxZ = max.z;
				return result;
			}

			public bool IsSolid(int x, int y, int z)
			{
				return grid.Contains(new Vector3Int(x, y, z));
			}

			public uint FaceColor(int x, int y, int z, int face)
			{
				Color32 faceColor = grid.GetFaceColor(new Vector3Int(x, y, z), face);
				return (uint)(-16777216 | (faceColor.b << 16) | (faceColor.g << 8) | faceColor.r);
			}
		}

		public static string ExportFolder => Path.Combine(Application.persistentDataPath, "Exports");

		public static ModelExportFailure TryExport(TemplateModel model, string fileName, ModelExportSettings settings, out ModelExportReport report, out string detail)
		{
			report = default(ModelExportReport);
			detail = "";
			if (settings == null)
			{
				settings = new ModelExportSettings();
			}
			List<IVoxelSource> list = new List<IVoxelSource>();
			List<string> list2 = new List<string>();
			List<TemplatePiece> list3 = new List<TemplatePiece>();
			if (model != null)
			{
				foreach (TemplatePiece piece in model.Pieces)
				{
					if (piece.Grid != null && piece.Grid.Count != 0)
					{
						list3.Add(piece);
						list.Add(new GridSource(piece.Grid));
						list2.Add(string.IsNullOrWhiteSpace(piece.Name) ? $"Piece{list3.Count}" : piece.Name);
					}
				}
			}
			if (list.Count == 0)
			{
				return ModelExportFailure.Empty;
			}
			VoxelExportMesher.Result result;
			try
			{
				result = VoxelExportMesher.Build(list, list2, settings.pixelsPerVoxel, settings.padding, settings.powerOfTwo);
			}
			catch (InvalidOperationException ex)
			{
				detail = ex.Message;
				return ModelExportFailure.TextureTooLarge;
			}
			if (result.QuadCount == 0)
			{
				return ModelExportFailure.Empty;
			}
			float num = Mathf.Max(0.0001f, settings.scale);
			float scale = Mathf.Max(0.0001f, model.VoxelSize) * num;
			for (int i = 0; i < result.Meshes.Count; i++)
			{
				Quaternion quaternion = ((i == 0) ? Quaternion.identity : list3[i].LocalRotation);
				Vector3 vector = ((i == 0) ? Vector3.zero : (list3[i].LocalPosition * num));
				ExportMeshOps.Transform(result.Meshes[i], scale, quaternion.x, quaternion.y, quaternion.z, quaternion.w, vector.x, vector.y, vector.z);
			}
			Recenter(result.Meshes, settings.pivot);
			foreach (ExportMesh mesh in result.Meshes)
			{
				ExportMeshOps.MirrorX(mesh);
				ExportMeshOps.FixWinding(mesh);
			}
			string text = SafeFileName(string.IsNullOrWhiteSpace(fileName) ? model.Name : fileName);
			string exportFolder = ExportFolder;
			string text2 = Path.Combine(exportFolder, text + ".fbx");
			string text3 = text + ".png";
			IReadOnlyList<ExportMesh> readOnlyList;
			if (!settings.mergePieces || result.Meshes.Count <= 1)
			{
				IReadOnlyList<ExportMesh> meshes = result.Meshes;
				readOnlyList = meshes;
			}
			else
			{
				IReadOnlyList<ExportMesh> meshes = new ExportMesh[1] { ExportMeshOps.Merge(result.Meshes, text) };
				readOnlyList = meshes;
			}
			IReadOnlyList<ExportMesh> readOnlyList2 = readOnlyList;
			try
			{
				Directory.CreateDirectory(exportFolder);
				byte[] array = PngEncoder.Encode(result.AtlasRgba, result.AtlasWidth, result.AtlasHeight);
				File.WriteAllBytes(Path.Combine(exportFolder, text3), array);
				string text4 = text2 + ".tmp";
				using (FileStream output = File.Create(text4))
				{
					FbxBinaryWriter.Write(output, readOnlyList2, text, new FbxBinaryWriter.Texture
					{
						FileName = text3,
						EmbeddedPng = (settings.embedTexture ? array : null)
					}, "Mimic Busters " + Application.version);
				}
				if (File.Exists(text2))
				{
					File.Delete(text2);
				}
				File.Move(text4, text2);
			}
			catch (Exception ex2) when (ex2 is IOException || ex2 is UnauthorizedAccessException)
			{
				detail = ex2.Message;
				return ModelExportFailure.WriteFailed;
			}
			report = new ModelExportReport(text2, result.QuadCount, readOnlyList2.Count, result.AtlasWidth, result.AtlasHeight);
			return ModelExportFailure.None;
		}

		private static void Recenter(List<ExportMesh> meshes, ExportPivot pivot)
		{
			if (pivot == ExportPivot.Origin)
			{
				return;
			}
			float[] array = new float[3];
			float[] array2 = new float[3];
			if (!ExportMeshOps.Bounds(meshes, array, array2))
			{
				return;
			}
			float dx = (0f - (array[0] + array2[0])) * 0.5f;
			float dz = (0f - (array[2] + array2[2])) * 0.5f;
			float dy = ((pivot == ExportPivot.BottomCenter) ? (0f - array[1]) : ((0f - (array[1] + array2[1])) * 0.5f));
			foreach (ExportMesh mesh in meshes)
			{
				ExportMeshOps.Translate(mesh, dx, dy, dz);
			}
		}

		private static string SafeFileName(string name)
		{
			name = (string.IsNullOrWhiteSpace(name) ? "model" : name.Trim());
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			foreach (char oldChar in invalidFileNameChars)
			{
				name = name.Replace(oldChar, '_');
			}
			if (name.Length != 0)
			{
				return name;
			}
			return "model";
		}

		public static void RevealFolder()
		{
			string exportFolder = ExportFolder;
			Directory.CreateDirectory(exportFolder);
			Application.OpenURL("file:///" + exportFolder.Replace('\\', '/'));
		}
	}
}
