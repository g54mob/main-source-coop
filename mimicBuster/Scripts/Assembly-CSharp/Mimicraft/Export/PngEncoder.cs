using System;
using System.IO;
using System.IO.Compression;

namespace Mimicraft.Export
{
	public static class PngEncoder
	{
		private static readonly byte[] Signature = new byte[8] { 137, 80, 78, 71, 13, 10, 26, 10 };

		private static uint[] crcTable;

		public static byte[] Encode(byte[] rgba, int width, int height)
		{
			if (rgba == null || width <= 0 || height <= 0 || rgba.Length < width * height * 4)
			{
				throw new ArgumentException("The pixel buffer does not match the image size.");
			}
			int num = width * 4;
			byte[] array = new byte[(num + 1) * height];
			for (int i = 0; i < height; i++)
			{
				int num2 = i * (num + 1);
				array[num2] = 0;
				Buffer.BlockCopy(rgba, (height - 1 - i) * num, array, num2 + 1, num);
			}
			using MemoryStream memoryStream = new MemoryStream();
			memoryStream.Write(Signature, 0, Signature.Length);
			byte[] array2 = new byte[13];
			WriteBigEndian(array2, 0, (uint)width);
			WriteBigEndian(array2, 4, (uint)height);
			array2[8] = 8;
			array2[9] = 6;
			array2[10] = 0;
			array2[11] = 0;
			array2[12] = 0;
			WriteChunk(memoryStream, "IHDR", array2);
			WriteChunk(memoryStream, "IDAT", ZlibCompress(array));
			WriteChunk(memoryStream, "IEND", Array.Empty<byte>());
			return memoryStream.ToArray();
		}

		private static byte[] ZlibCompress(byte[] data)
		{
			using MemoryStream memoryStream = new MemoryStream();
			memoryStream.WriteByte(120);
			memoryStream.WriteByte(156);
			using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionLevel.Optimal, leaveOpen: true))
			{
				deflateStream.Write(data, 0, data.Length);
			}
			byte[] array = new byte[4];
			WriteBigEndian(array, 0, Adler32(data));
			memoryStream.Write(array, 0, 4);
			return memoryStream.ToArray();
		}

		private static uint Adler32(byte[] data)
		{
			uint num = 1u;
			uint num2 = 0u;
			foreach (byte b in data)
			{
				num = (num + b) % 65521;
				num2 = (num2 + num) % 65521;
			}
			return (num2 << 16) | num;
		}

		private static void WriteChunk(Stream output, string type, byte[] data)
		{
			byte[] array = new byte[4];
			WriteBigEndian(array, 0, (uint)data.Length);
			output.Write(array, 0, 4);
			byte[] array2 = new byte[4];
			for (int i = 0; i < 4; i++)
			{
				array2[i] = (byte)type[i];
			}
			output.Write(array2, 0, 4);
			output.Write(data, 0, data.Length);
			uint crc = Crc32(array2, uint.MaxValue);
			crc = Crc32(data, crc) ^ 0xFFFFFFFFu;
			byte[] array3 = new byte[4];
			WriteBigEndian(array3, 0, crc);
			output.Write(array3, 0, 4);
		}

		private static uint Crc32(byte[] data, uint crc)
		{
			if (crcTable == null)
			{
				uint[] array = new uint[256];
				for (uint num = 0u; num < 256; num++)
				{
					uint num2 = num;
					for (int i = 0; i < 8; i++)
					{
						num2 = (((num2 & 1) != 0) ? (0xEDB88320u ^ (num2 >> 1)) : (num2 >> 1));
					}
					array[num] = num2;
				}
				crcTable = array;
			}
			foreach (byte b in data)
			{
				crc = crcTable[(crc ^ b) & 0xFF] ^ (crc >> 8);
			}
			return crc;
		}

		private static void WriteBigEndian(byte[] target, int offset, uint value)
		{
			target[offset] = (byte)(value >> 24);
			target[offset + 1] = (byte)(value >> 16);
			target[offset + 2] = (byte)(value >> 8);
			target[offset + 3] = (byte)value;
		}
	}
}
