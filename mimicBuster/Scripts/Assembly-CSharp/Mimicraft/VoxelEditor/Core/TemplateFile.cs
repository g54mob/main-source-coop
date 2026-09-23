using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mimicraft.Networking;

namespace Mimicraft.VoxelEditor.Core
{
	public static class TemplateFile
	{
		public const string Extension = ".template";

		public const string LegacyExtension = ".json";

		private const string Magic = "MTF";

		private const byte Version = 1;

		private const int MaxPieces = 4096;

		public static VoxelBodyData ToBodyData(TemplateModel model)
		{
			VoxelBodyData voxelBodyData = new VoxelBodyData
			{
				voxelSize = (model?.VoxelSize ?? 1f)
			};
			if (model == null)
			{
				return voxelBodyData;
			}
			foreach (TemplatePiece piece in model.Pieces)
			{
				voxelBodyData.pieces.Add(new VoxelPieceData
				{
					grid = piece.Grid,
					localPosition = piece.LocalPosition,
					localRotation = piece.LocalRotation
				});
			}
			return voxelBodyData;
		}

		public static byte[] Encode(TemplateModel model)
		{
			VoxelBodyData body = ToBodyData(model);
			List<string> list = new List<string>(model.Pieces.Count);
			foreach (TemplatePiece piece in model.Pieces)
			{
				list.Add(piece.Name ?? "");
			}
			byte[] array = VoxelBodyCodec.Encode(body);
			using MemoryStream memoryStream = new MemoryStream();
			using BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8);
			binaryWriter.Write(Encoding.ASCII.GetBytes("MTF"));
			binaryWriter.Write((byte)1);
			binaryWriter.Write(model.Name ?? "");
			binaryWriter.Write(model.Category ?? "");
			binaryWriter.Write(model.Tag ?? "");
			binaryWriter.Write(list.Count);
			foreach (string item in list)
			{
				binaryWriter.Write(item);
			}
			binaryWriter.Write(array.Length);
			binaryWriter.Write(array);
			binaryWriter.Flush();
			return memoryStream.ToArray();
		}

		public static bool TryReadHeader(byte[] bytes, out string name, out string category, out string tag)
		{
			int pieceCount;
			return TryReadHeader(bytes, out name, out category, out tag, out pieceCount);
		}

		public static bool TryReadHeader(byte[] bytes, out string name, out string category, out string tag, out int pieceCount)
		{
			name = "";
			category = "";
			tag = "";
			pieceCount = 0;
			if (bytes == null || bytes.Length < 5)
			{
				return false;
			}
			try
			{
				using MemoryStream input = new MemoryStream(bytes, writable: false);
				using BinaryReader binaryReader = new BinaryReader(input, Encoding.UTF8);
				if (Encoding.ASCII.GetString(binaryReader.ReadBytes(3)) != "MTF")
				{
					return false;
				}
				if (binaryReader.ReadByte() != 1)
				{
					return false;
				}
				name = binaryReader.ReadString();
				category = binaryReader.ReadString();
				tag = binaryReader.ReadString();
				pieceCount = binaryReader.ReadInt32();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static bool TryDecode(byte[] bytes, out TemplateModel model)
		{
			model = null;
			if (bytes == null || bytes.Length < 5)
			{
				return false;
			}
			try
			{
				using MemoryStream memoryStream = new MemoryStream(bytes, writable: false);
				using BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8);
				if (Encoding.ASCII.GetString(binaryReader.ReadBytes(3)) != "MTF")
				{
					return false;
				}
				if (binaryReader.ReadByte() != 1)
				{
					return false;
				}
				TemplateModel templateModel = new TemplateModel
				{
					Name = binaryReader.ReadString(),
					Category = binaryReader.ReadString(),
					Tag = binaryReader.ReadString()
				};
				int num = binaryReader.ReadInt32();
				if (num < 0 || num > 4096)
				{
					return false;
				}
				string[] array = new string[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = binaryReader.ReadString();
				}
				int num2 = binaryReader.ReadInt32();
				if (num2 < 0 || num2 > memoryStream.Length - memoryStream.Position)
				{
					return false;
				}
				if (!VoxelBodyCodec.TryDecode(binaryReader.ReadBytes(num2), out var body, 200000, 512))
				{
					return false;
				}
				templateModel.VoxelSize = body.voxelSize;
				for (int j = 0; j < body.pieces.Count; j++)
				{
					VoxelPieceData voxelPieceData = body.pieces[j];
					templateModel.Pieces.Add(new TemplatePiece((j < array.Length) ? array[j] : "", voxelPieceData.grid, voxelPieceData.localPosition, voxelPieceData.localRotation));
				}
				model = templateModel;
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
