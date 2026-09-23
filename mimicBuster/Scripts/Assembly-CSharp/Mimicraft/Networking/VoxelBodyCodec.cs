using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.Networking
{
	public static class VoxelBodyCodec
	{
		private struct Run
		{
			public uint Start;

			public ushort Length;

			public Color32 Color;
		}

		private struct FaceEntry
		{
			public uint Index;

			public byte Face;

			public Color32 Color;
		}

		private struct PieceRuns
		{
			public VoxelPieceData Piece;

			public Vector3Int Min;

			public Vector3Int Size;

			public List<Run> Runs;

			public List<FaceEntry> Faces;
		}

		private struct PieceResult
		{
			public Vector3 Position;

			public Quaternion Rotation;

			public bool HasVoxels;

			public Vector3Int Min;

			public Vector3Int Max;

			public int Voxels;

			public VoxelGrid Grid;

			public List<VoxelIsland> Islands;

			public PieceShapeData Shape;
		}

		private const byte FormatRaw = 1;

		private const byte FormatDeflated = 2;

		private const int PieceFixedHeaderBytes = 40;

		public const int DefaultMaxVoxels = 200000;

		public const int MaxPieceBoxExtent = 64;

		public const int MaxAllowedBoxExtent = 512;

		private const int MaxInflatedBytes = 2097152;

		private static bool[] islandCells = new bool[4096];

		public static byte[] Encode(VoxelBodyData body)
		{
			List<PieceRuns> list = new List<PieceRuns>(body.pieces.Count);
			foreach (VoxelPieceData piece in body.pieces)
			{
				list.Add(BuildPieceRuns(piece));
			}
			Dictionary<int, int> lookup;
			List<Color32> list2 = BuildPalette(list, out lookup);
			List<byte> list3 = new List<byte>(1024);
			list3.AddRange(BitConverter.GetBytes(body.voxelSize));
			WriteVarUInt(list3, (uint)body.pieces.Count);
			int num = list2?.Count ?? 0;
			WriteVarUInt(list3, (uint)num);
			if (list2 != null)
			{
				foreach (Color32 item in list2)
				{
					list3.Add(item.r);
					list3.Add(item.g);
					list3.Add(item.b);
				}
			}
			foreach (PieceRuns item2 in list)
			{
				WritePiece(list3, item2, lookup, num);
			}
			return Frame(list3.ToArray());
		}

		private static byte[] Frame(byte[] raw)
		{
			byte[] array = Compress(raw);
			bool flag = array != null && array.Length < raw.Length;
			byte[] array2 = (flag ? array : raw);
			byte[] array3 = new byte[array2.Length + 1];
			array3[0] = (byte)((!flag) ? 1 : 2);
			Buffer.BlockCopy(array2, 0, array3, 1, array2.Length);
			return array3;
		}

		private static byte[] Compress(byte[] raw)
		{
			using MemoryStream memoryStream = new MemoryStream();
			using (DeflateStream deflateStream = new DeflateStream(memoryStream, System.IO.Compression.CompressionLevel.Optimal, leaveOpen: true))
			{
				deflateStream.Write(raw, 0, raw.Length);
			}
			return memoryStream.ToArray();
		}

		private static PieceRuns BuildPieceRuns(VoxelPieceData piece)
		{
			VoxelGrid voxelGrid = piece?.grid;
			if (voxelGrid == null || !voxelGrid.TryGetBounds(out var min, out var max))
			{
				return new PieceRuns
				{
					Piece = piece
				};
			}
			Vector3Int size = max - min + Vector3Int.one;
			return new PieceRuns
			{
				Piece = piece,
				Min = min,
				Size = size,
				Runs = BuildRuns(voxelGrid, min, size),
				Faces = BuildFaceEntries(voxelGrid, min, size)
			};
		}

		private static List<FaceEntry> BuildFaceEntries(VoxelGrid grid, Vector3Int min, Vector3Int size)
		{
			if (grid.FaceColorCount == 0)
			{
				return null;
			}
			List<FaceEntry> list = new List<FaceEntry>(grid.FaceColorCount);
			foreach (KeyValuePair<(Vector3Int, int), Color32> faceColor in grid.FaceColors)
			{
				Vector3Int vector3Int = faceColor.Key.Item1 - min;
				if (vector3Int.x >= 0 && vector3Int.y >= 0 && vector3Int.z >= 0 && vector3Int.x < size.x && vector3Int.y < size.y && vector3Int.z < size.z && faceColor.Key.Item2 >= 0 && faceColor.Key.Item2 < 6 && grid.IsFaceVisible(faceColor.Key.Item1, faceColor.Key.Item2))
				{
					list.Add(new FaceEntry
					{
						Index = (uint)(((long)vector3Int.y * (long)size.z + vector3Int.z) * size.x + vector3Int.x),
						Face = (byte)faceColor.Key.Item2,
						Color = faceColor.Value
					});
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			list.Sort((FaceEntry a, FaceEntry b) => (a.Index == b.Index) ? a.Face.CompareTo(b.Face) : a.Index.CompareTo(b.Index));
			return list;
		}

		private static List<Run> BuildRuns(VoxelGrid grid, Vector3Int min, Vector3Int size)
		{
			List<Run> list = new List<Run>();
			bool flag = false;
			Run item = default(Run);
			for (int i = 0; i < size.y; i++)
			{
				for (int j = 0; j < size.z; j++)
				{
					for (int k = 0; k < size.x; k++)
					{
						Vector3Int position = new Vector3Int(min.x + k, min.y + i, min.z + j);
						VoxelData data;
						bool flag2 = grid.TryGet(position, out data);
						if (flag2 && flag && item.Color.r == data.Color.r && item.Color.g == data.Color.g && item.Color.b == data.Color.b && item.Length < ushort.MaxValue && k != 0)
						{
							item.Length++;
							continue;
						}
						if (flag)
						{
							list.Add(item);
							flag = false;
						}
						if (flag2)
						{
							item = new Run
							{
								Start = (uint)(((long)i * (long)size.z + j) * size.x + k),
								Length = 1,
								Color = data.Color
							};
							flag = true;
						}
					}
				}
			}
			if (flag)
			{
				list.Add(item);
			}
			return list;
		}

		private static List<Color32> BuildPalette(List<PieceRuns> pieces, out Dictionary<int, int> lookup)
		{
			List<Color32> list = new List<Color32>();
			lookup = new Dictionary<int, int>();
			foreach (PieceRuns piece in pieces)
			{
				if (piece.Runs != null)
				{
					foreach (Run run in piece.Runs)
					{
						if (!TryAddPaletteColor(list, lookup, run.Color))
						{
							lookup = null;
							return null;
						}
					}
				}
				if (piece.Faces == null)
				{
					continue;
				}
				foreach (FaceEntry face in piece.Faces)
				{
					if (!TryAddPaletteColor(list, lookup, face.Color))
					{
						lookup = null;
						return null;
					}
				}
			}
			return list;
		}

		private static bool TryAddPaletteColor(List<Color32> palette, Dictionary<int, int> lookup, Color32 color)
		{
			int key = Pack(color);
			if (lookup.ContainsKey(key))
			{
				return true;
			}
			if (palette.Count >= 65535)
			{
				return false;
			}
			lookup[key] = palette.Count;
			palette.Add(color);
			return true;
		}

		private static int Pack(Color32 c)
		{
			return (c.r << 16) | (c.g << 8) | c.b;
		}

		private static void WritePiece(List<byte> bytes, PieceRuns piece, Dictionary<int, int> palette, int paletteCount)
		{
			Vector3 vector = piece.Piece?.localPosition ?? Vector3.zero;
			Quaternion quaternion = piece.Piece?.localRotation ?? Quaternion.identity;
			bytes.AddRange(BitConverter.GetBytes(vector.x));
			bytes.AddRange(BitConverter.GetBytes(vector.y));
			bytes.AddRange(BitConverter.GetBytes(vector.z));
			bytes.AddRange(BitConverter.GetBytes(quaternion.x));
			bytes.AddRange(BitConverter.GetBytes(quaternion.y));
			bytes.AddRange(BitConverter.GetBytes(quaternion.z));
			bytes.AddRange(BitConverter.GetBytes(quaternion.w));
			Vector3Int vector3Int = ((piece.Runs == null) ? Vector3Int.zero : piece.Min);
			Vector3Int vector3Int2 = ((piece.Runs == null) ? Vector3Int.zero : piece.Size);
			bytes.AddRange(BitConverter.GetBytes((short)vector3Int.x));
			bytes.AddRange(BitConverter.GetBytes((short)vector3Int.y));
			bytes.AddRange(BitConverter.GetBytes((short)vector3Int.z));
			bytes.AddRange(BitConverter.GetBytes((ushort)vector3Int2.x));
			bytes.AddRange(BitConverter.GetBytes((ushort)vector3Int2.y));
			bytes.AddRange(BitConverter.GetBytes((ushort)vector3Int2.z));
			if (piece.Runs == null)
			{
				WriteVarUInt(bytes, 0u);
				WriteVarUInt(bytes, 0u);
				return;
			}
			WriteVarUInt(bytes, (uint)piece.Runs.Count);
			long num = 0L;
			foreach (Run run in piece.Runs)
			{
				WriteVarUInt(bytes, (uint)(run.Start - num));
				WriteVarUInt(bytes, run.Length);
				WriteColor(bytes, run.Color, palette, paletteCount);
				num = run.Start + run.Length;
			}
			int num2 = piece.Faces?.Count ?? 0;
			WriteVarUInt(bytes, (uint)num2);
			long num3 = 0L;
			for (int i = 0; i < num2; i++)
			{
				FaceEntry faceEntry = piece.Faces[i];
				WriteVarUInt(bytes, (uint)(faceEntry.Index - num3));
				bytes.Add(faceEntry.Face);
				WriteColor(bytes, faceEntry.Color, palette, paletteCount);
				num3 = faceEntry.Index;
			}
		}

		private static void WriteColor(List<byte> bytes, Color32 color, Dictionary<int, int> palette, int paletteCount)
		{
			if (paletteCount > 0)
			{
				int num = palette[Pack(color)];
				bytes.Add((byte)num);
				if (paletteCount > 256)
				{
					bytes.Add((byte)(num >> 8));
				}
			}
			else
			{
				bytes.Add(color.r);
				bytes.Add(color.g);
				bytes.Add(color.b);
			}
		}

		public static bool TryDecode(byte[] data, out VoxelBodyData body, int maxVoxels = 200000, int maxBoxExtent = 64)
		{
			body = null;
			if (!TryReadPieces(data, maxVoxels, maxBoxExtent, materialize: true, out var voxelSize, out var pieces))
			{
				return false;
			}
			VoxelBodyData voxelBodyData = new VoxelBodyData
			{
				voxelSize = voxelSize
			};
			foreach (PieceResult item in pieces)
			{
				item.Grid?.PruneHiddenFaceColors();
				voxelBodyData.pieces.Add(new VoxelPieceData
				{
					grid = item.Grid,
					localPosition = item.Position,
					localRotation = item.Rotation
				});
			}
			body = voxelBodyData;
			return true;
		}

		public static bool TryDecodeSummary(byte[] data, out VoxelBodySummary summary, int maxVoxels = 200000, int maxBoxExtent = 64, bool findIslands = false, bool findShapes = false)
		{
			summary = default(VoxelBodySummary);
			if (!TryReadPieces(data, maxVoxels, maxBoxExtent, materialize: false, out var voxelSize, out var pieces, findIslands, findShapes))
			{
				return false;
			}
			List<VoxelPieceSummary> list = new List<VoxelPieceSummary>(pieces.Count);
			int num = 0;
			foreach (PieceResult item in pieces)
			{
				list.Add(new VoxelPieceSummary(item.Position, item.Rotation, item.HasVoxels, item.Min, item.Max, item.Voxels, item.Islands, item.Shape));
				num += item.Voxels;
			}
			summary = new VoxelBodySummary(voxelSize, num, list);
			return true;
		}

		private static bool TryReadPieces(byte[] data, int maxVoxels, int maxBoxExtent, bool materialize, out float voxelSize, out List<PieceResult> pieces, bool findIslands = false, bool findShapes = false)
		{
			voxelSize = 0f;
			pieces = null;
			if (data == null || data.Length < 2)
			{
				return false;
			}
			byte[] raw;
			switch (data[0])
			{
			case 2:
				if (!TryDecompress(data, out raw))
				{
					return false;
				}
				break;
			case 1:
				raw = new byte[data.Length - 1];
				Buffer.BlockCopy(data, 1, raw, 0, raw.Length);
				break;
			default:
				return false;
			}
			if (raw.Length < 4)
			{
				return false;
			}
			int o = 0;
			voxelSize = ReadFloat(raw, ref o);
			if (!(voxelSize > 0f) || float.IsNaN(voxelSize) || float.IsInfinity(voxelSize))
			{
				return false;
			}
			if (!TryReadVarUInt(raw, ref o, out var value))
			{
				return false;
			}
			if (!TryReadPalette(raw, ref o, out var palette))
			{
				return false;
			}
			int remainingVoxels = maxVoxels;
			List<PieceResult> list = new List<PieceResult>((int)Math.Min(value, 64u));
			for (uint num = 0u; num < value; num++)
			{
				if (!TryReadPiece(raw, ref o, palette, ref remainingVoxels, maxBoxExtent, materialize, findIslands, findShapes, out var piece))
				{
					return false;
				}
				list.Add(piece);
			}
			pieces = list;
			return true;
		}

		private static bool TryDecompress(byte[] data, out byte[] raw)
		{
			raw = null;
			try
			{
				using MemoryStream stream = new MemoryStream(data, 1, data.Length - 1);
				using DeflateStream deflateStream = new DeflateStream(stream, CompressionMode.Decompress);
				using MemoryStream memoryStream = new MemoryStream();
				byte[] array = new byte[8192];
				int num;
				while ((num = deflateStream.Read(array, 0, array.Length)) > 0)
				{
					if (memoryStream.Length + num > 2097152)
					{
						return false;
					}
					memoryStream.Write(array, 0, num);
				}
				raw = memoryStream.ToArray();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		private static bool TryReadPalette(byte[] data, ref int offset, out Color32[] palette)
		{
			palette = null;
			if (!TryReadVarUInt(data, ref offset, out var value))
			{
				return false;
			}
			if (value == 0)
			{
				return true;
			}
			if (value > 65535 || offset + (long)value * 3L > data.Length)
			{
				return false;
			}
			palette = new Color32[value];
			for (int i = 0; i < value; i++)
			{
				palette[i] = new Color32(data[offset], data[offset + 1], data[offset + 2], byte.MaxValue);
				offset += 3;
			}
			return true;
		}

		private static bool TryReadPiece(byte[] data, ref int offset, Color32[] palette, ref int remainingVoxels, int maxBoxExtent, bool materialize, bool findIslands, bool findShapes, out PieceResult piece)
		{
			piece = default(PieceResult);
			if (offset + 40 > data.Length)
			{
				return false;
			}
			piece.Position = new Vector3(ReadFloat(data, ref offset), ReadFloat(data, ref offset), ReadFloat(data, ref offset));
			piece.Rotation = new Quaternion(ReadFloat(data, ref offset), ReadFloat(data, ref offset), ReadFloat(data, ref offset), ReadFloat(data, ref offset));
			Vector3Int vector3Int = new Vector3Int(ReadShort(data, ref offset), ReadShort(data, ref offset), ReadShort(data, ref offset));
			int num = ReadUShort(data, ref offset);
			int num2 = ReadUShort(data, ref offset);
			int num3 = ReadUShort(data, ref offset);
			if (num > maxBoxExtent || num2 > maxBoxExtent || num3 > maxBoxExtent)
			{
				return false;
			}
			if (!TryReadVarUInt(data, ref offset, out var value))
			{
				return false;
			}
			if (materialize)
			{
				piece.Grid = new VoxelGrid();
			}
			long num4 = (long)num * (long)num2 * num3;
			long num5 = 0L;
			bool[] array = null;
			if ((findIslands || findShapes) && num4 > 0)
			{
				if (islandCells.Length < num4)
				{
					islandCells = new bool[num4];
				}
				else
				{
					Array.Clear(islandCells, 0, (int)num4);
				}
				array = islandCells;
			}
			for (uint num6 = 0u; num6 < value; num6++)
			{
				if (!TryReadVarUInt(data, ref offset, out var value2) || !TryReadVarUInt(data, ref offset, out var value3))
				{
					return false;
				}
				long num7 = num5 + value2;
				if (value3 == 0 || num7 + value3 > num4)
				{
					return false;
				}
				int num8 = (int)(num7 / num);
				int num9 = (int)(num7 % num);
				if (num9 + value3 > num)
				{
					return false;
				}
				if (value3 > (uint)remainingVoxels)
				{
					return false;
				}
				remainingVoxels -= (int)value3;
				if (!TryReadColor(data, ref offset, palette, out var color))
				{
					return false;
				}
				int num10 = num8 % num3;
				int num11 = num8 / num3;
				int num12 = num9 + (int)value3 - 1;
				Vector3Int vector3Int2 = new Vector3Int(vector3Int.x + num9, vector3Int.y + num11, vector3Int.z + num10);
				Vector3Int vector3Int3 = new Vector3Int(vector3Int.x + num12, vector3Int.y + num11, vector3Int.z + num10);
				if (!piece.HasVoxels)
				{
					piece.HasVoxels = true;
					piece.Min = vector3Int2;
					piece.Max = vector3Int3;
				}
				else
				{
					piece.Min = Vector3Int.Min(piece.Min, vector3Int2);
					piece.Max = Vector3Int.Max(piece.Max, vector3Int3);
				}
				piece.Voxels += (int)value3;
				if (array != null)
				{
					int num13 = (int)num7;
					for (int i = 0; i < (int)value3; i++)
					{
						array[num13 + i] = true;
					}
				}
				if (materialize)
				{
					for (int j = num9; j <= num12; j++)
					{
						piece.Grid.Set(new Vector3Int(vector3Int.x + j, vector3Int.y + num11, vector3Int.z + num10), new VoxelData(color));
					}
				}
				num5 = num7 + value3;
			}
			if (findIslands && array != null && piece.HasVoxels)
			{
				piece.Islands = new List<VoxelIsland>(1);
				VoxelIslandFinder.Find(array, num, num2, num3, piece.Islands);
			}
			if (findShapes && array != null && piece.HasVoxels)
			{
				piece.Shape = new PieceShapeData();
				PieceShape.FromOccupancy(array, vector3Int, num, num2, num3, piece.Shape);
			}
			return TryReadFaces(data, ref offset, palette, piece.Voxels, vector3Int, num, num2, num3, materialize, piece.Grid);
		}

		private static bool TryReadFaces(byte[] data, ref int offset, Color32[] palette, int voxels, Vector3Int boxMin, int sizeX, int sizeY, int sizeZ, bool materialize, VoxelGrid grid)
		{
			if (!TryReadVarUInt(data, ref offset, out var value))
			{
				return false;
			}
			if (value > (long)voxels * 6L)
			{
				return false;
			}
			long num = (long)sizeX * (long)sizeY * sizeZ;
			long num2 = 0L;
			int num3 = -1;
			bool flag = true;
			for (uint num4 = 0u; num4 < value; num4++)
			{
				if (!TryReadVarUInt(data, ref offset, out var value2))
				{
					return false;
				}
				if (offset >= data.Length)
				{
					return false;
				}
				byte b = data[offset++];
				if (b >= 6)
				{
					return false;
				}
				long num5 = num2 + value2;
				if (num5 >= num)
				{
					return false;
				}
				if (!flag && num5 == num2 && b <= num3)
				{
					return false;
				}
				if (!TryReadColor(data, ref offset, palette, out var color))
				{
					return false;
				}
				if (materialize)
				{
					int num6 = (int)(num5 % sizeX);
					long num7 = num5 / sizeX;
					int num8 = (int)(num7 % sizeZ);
					int num9 = (int)(num7 / sizeZ);
					grid.SetFaceColor(new Vector3Int(boxMin.x + num6, boxMin.y + num9, boxMin.z + num8), b, color);
				}
				num2 = num5;
				num3 = b;
				flag = false;
			}
			return true;
		}

		private static bool TryReadColor(byte[] data, ref int offset, Color32[] palette, out Color32 color)
		{
			color = default(Color32);
			if (palette == null)
			{
				if (offset + 3 > data.Length)
				{
					return false;
				}
				color = new Color32(data[offset], data[offset + 1], data[offset + 2], byte.MaxValue);
				offset += 3;
				return true;
			}
			int num = ((palette.Length <= 256) ? 1 : 2);
			if (offset + num > data.Length)
			{
				return false;
			}
			int num2 = data[offset];
			if (num == 2)
			{
				num2 |= data[offset + 1] << 8;
			}
			offset += num;
			if (num2 >= palette.Length)
			{
				return false;
			}
			color = palette[num2];
			return true;
		}

		private static void WriteVarUInt(List<byte> bytes, uint value)
		{
			while (value >= 128)
			{
				bytes.Add((byte)(value | 0x80));
				value >>= 7;
			}
			bytes.Add((byte)value);
		}

		private static bool TryReadVarUInt(byte[] data, ref int offset, out uint value)
		{
			value = 0u;
			int num = 0;
			for (int i = 0; i < 5; i++)
			{
				if (offset >= data.Length)
				{
					return false;
				}
				byte b = data[offset++];
				value |= (uint)((b & 0x7F) << num);
				if ((b & 0x80) == 0)
				{
					return true;
				}
				num += 7;
			}
			return false;
		}

		private static float ReadFloat(byte[] d, ref int o)
		{
			float result = BitConverter.ToSingle(d, o);
			o += 4;
			return result;
		}

		private static int ReadUShort(byte[] d, ref int o)
		{
			ushort result = BitConverter.ToUInt16(d, o);
			o += 2;
			return result;
		}

		private static int ReadShort(byte[] d, ref int o)
		{
			short result = BitConverter.ToInt16(d, o);
			o += 2;
			return result;
		}
	}
}
