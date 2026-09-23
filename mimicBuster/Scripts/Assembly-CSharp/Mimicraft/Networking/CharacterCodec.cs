using System;
using System.Collections.Generic;
using System.Text;
using Mimicraft.Customization;
using UnityEngine;

namespace Mimicraft.Networking
{
	public static class CharacterCodec
	{
		private const byte Magic0 = 77;

		private const byte Magic1 = 67;

		private const byte Magic2 = 67;

		private const byte Version = 1;

		public const int MaxParts = 64;

		public const int MaxPartIdBytes = 64;

		public static byte[] Encode(CharacterData character)
		{
			if (character == null || character.Parts.Count == 0)
			{
				return Array.Empty<byte>();
			}
			List<string> list = new List<string>();
			VoxelBodyData voxelBodyData = new VoxelBodyData
			{
				voxelSize = 1f
			};
			foreach (CharacterPartData part in character.Parts)
			{
				if (part != null && !string.IsNullOrEmpty(part.PartId) && part.Grid != null)
				{
					list.Add(part.PartId);
					voxelBodyData.pieces.Add(new VoxelPieceData
					{
						grid = part.Grid,
						localPosition = Vector3.zero,
						localRotation = Quaternion.identity
					});
				}
			}
			if (list.Count == 0)
			{
				return Array.Empty<byte>();
			}
			byte[] array = VoxelBodyCodec.Encode(voxelBodyData);
			List<byte> list2 = new List<byte> { 77, 67, 67, 1 };
			WriteVarint(list2, (uint)list.Count);
			foreach (string item in list)
			{
				byte[] bytes = Encoding.UTF8.GetBytes(item);
				WriteVarint(list2, (uint)bytes.Length);
				list2.AddRange(bytes);
			}
			WriteVarint(list2, (uint)array.Length);
			list2.AddRange(array);
			return list2.ToArray();
		}

		public static bool TryDecode(byte[] data, string rigId, out CharacterData character, int maxBoxExtent = 64)
		{
			character = null;
			if (data == null || data.Length < 5)
			{
				return false;
			}
			if (data[0] != 77 || data[1] != 67 || data[2] != 67 || data[3] != 1)
			{
				return false;
			}
			int offset = 4;
			if (!TryReadVarint(data, ref offset, out var value) || value > 64)
			{
				return false;
			}
			List<string> list = new List<string>((int)value);
			for (int i = 0; i < value; i++)
			{
				if (!TryReadVarint(data, ref offset, out var value2) || value2 > 64)
				{
					return false;
				}
				if (value2 > data.Length - offset)
				{
					return false;
				}
				list.Add(Encoding.UTF8.GetString(data, offset, (int)value2));
				offset += (int)value2;
			}
			if (!TryReadVarint(data, ref offset, out var value3) || value3 > data.Length - offset)
			{
				return false;
			}
			byte[] array = new byte[value3];
			Array.Copy(data, offset, array, 0, (int)value3);
			if (!VoxelBodyCodec.TryDecode(array, out var body, 200000, Mathf.Clamp(maxBoxExtent, 1, 512)))
			{
				return false;
			}
			if (body.pieces.Count != list.Count)
			{
				return false;
			}
			character = new CharacterData(rigId ?? "");
			for (int j = 0; j < list.Count; j++)
			{
				VoxelPieceData voxelPieceData = body.pieces[j];
				if (voxelPieceData?.grid != null && !string.IsNullOrEmpty(list[j]))
				{
					character.Set(list[j], voxelPieceData.grid);
				}
			}
			return true;
		}

		private static void WriteVarint(List<byte> output, uint value)
		{
			while (value >= 128)
			{
				output.Add((byte)(value | 0x80));
				value >>= 7;
			}
			output.Add((byte)value);
		}

		private static bool TryReadVarint(byte[] data, ref int offset, out uint value)
		{
			value = 0u;
			int num = 0;
			while (true)
			{
				if (offset >= data.Length || num > 28)
				{
					return false;
				}
				byte b = data[offset++];
				value |= (uint)((b & 0x7F) << num);
				if ((b & 0x80) == 0)
				{
					break;
				}
				num += 7;
			}
			return true;
		}
	}
}
